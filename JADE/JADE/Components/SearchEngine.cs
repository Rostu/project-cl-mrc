using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml.Linq;
using System.Windows.Forms;
using System.IO;
using System.Collections;

namespace WindowsFormsApplication1
{
    public class SearchEngine
    {
        private static SearchEngine engine;
        private static System.Data.DataSet dataSet;

        private static XNamespace xns = "http://www.wadoku.de/xml/entry";
        private static XDocument wadoku;

        private static Hashtable posDeciphering;


        private SearchEngine()
        {
            if (engine != null)
            {
                throw new InvalidOperationException("Es ist unmöglich eine weitere Instanz der Singleton-Klasse SearchEngine zu erzeugen.");
            }
            if (!File.Exists("wadoku.xml"))
            {
                throw new ArgumentException("Die Wörterbuchdatei konnte nicht gefunden werden.");
            }

            dataSet = new DataSet();
            xns = "http://www.wadoku.de/xml/entry";
            wadoku = XDocument.Load("wadoku.xml");

            posDeciphering = new Hashtable()
            {
	            {"N", "Nomen"},
	            {"V", "Verb"},
	            {"Adj", "Adjektiv"},
	            {"Adn", "Adnomen"},
	            {"Adv", "Adverb"},
	            {"BspSatz", "Beispielsatz"},
	            {"Hilfsv", "Hilfsverb"},
	            {"Interj", "Interjektion"},
	            {"Kanji", "Einzel-Kanji"},
	            {"Konj", "Konjunktion"},
	            {"Part", "Partikel"},
	            {"Praef", "Präfix"},
	            {"Pron", "Pronomen"},
	            {"Suff", "Suffix"},
	            {"Zus", "Zusammensetzung"},
	            {"Redensart", "Redensart"},
	            {"Wortkomp", "Wortkomposition"},
	            {"Sonderzeichen", "Sonderzeichen"},
	            {"Themenpart", "Themenpartikel"},
	            {"Sonderform", "Sonderform"},
	            {"Undef", "Undefiniert"}
            };
        }

        public static SearchEngine Engine
        {
            get
            {
                if (engine == null)
                {
                    engine = new SearchEngine();
                }
                return engine;
            }
        }



        public void clearDataSet()
        {
            while (dataSet.Tables.Count > 0)
            {
                DataTable table = dataSet.Tables[0];
                if (dataSet.Tables.CanRemove(table)) dataSet.Tables.Remove(table);
            }
        }

        public void DisposeTable(int satzNr, int tokenNr)
        {
            DataTable tableToDispose = dataSet.Tables["results_" + satzNr + tokenNr];
            DataTable tableToDispose_absolute = dataSet.Tables["results_" + satzNr + tokenNr + "_absolute"];

            if (dataSet.Tables.CanRemove(tableToDispose)) dataSet.Tables.Remove(tableToDispose);
            if (dataSet.Tables.CanRemove(tableToDispose_absolute)) dataSet.Tables.Remove(tableToDispose_absolute);
        }

        public DataTable search(string token, int satzNr, int tokenNr, bool absolute)
        {
            if (dataSet.Tables.Contains("results_" + satzNr + tokenNr + ((absolute) ? "_absolute" : "")))
            {
                return dataSet.Tables["results_" + satzNr + tokenNr + ((absolute) ? "_absolute" : "")];
            }
            else
            {
                XElement results = new XElement("results",
                                (from entry in wadoku.Root.Elements().OfType<XElement>()

                                 where validateEntry(entry.Element(xns + "form"), token, absolute)

                                 select new XElement("result", new XElement("orths", entry.Element(xns + "form").Elements(xns + "orth")),
                                                               entry.Element(xns + "gramGrp"),
                                                               new XElement("prons", entry.Element(xns + "form").Elements(xns + "pron")),
                                                               new XElement("senses", entry.Elements(xns + "sense")))));
                
                DataTable outputTable = (results.HasElements) 
                                            ? (createResultTable(results, satzNr, tokenNr, absolute)) : (new DataTable()); 
                return outputTable;
            }

        }

        private bool validateEntry(XElement form, string token, bool absolute)
        {
            bool entryIsValid = false;
            bool currentFormDescendantIsValid = false;

            foreach (XElement formEntry in form.Elements())
            {
                if ((absolute && (currentFormDescendantIsValid = formEntry.Element(xns + "text").Value == token)) ||
                    (!absolute && (currentFormDescendantIsValid = formEntry.Element(xns + "text").Value.StartsWith(token))))
                {
                    entryIsValid = currentFormDescendantIsValid;
                }
            }
            return entryIsValid;
        }

        private DataTable createResultTable(XElement resEntries, int satzNr, int tokenNr, bool absolute)
        {
            DataTable searchResults = defineResultTable("results_" + satzNr + tokenNr + ((absolute) ? "_absolute" : ""));
            dataSet.Tables.Add(searchResults); // Add the new DataTable to the DataSet.

            int resultCount = 0;

            foreach (XElement resultEntry in resEntries.Nodes())
            {
                List<String> senses = new List<String>();

                int senseCount = 0;

                foreach (XElement sense in resultEntry.Element("senses").Elements(xns + "sense"))
                {
                    //Console.WriteLine(sense.ToString());
                    List<String> transPerSense = new List<String>();
                    string senseData = "";

                    #region Einzelne_<trans>'s_auflisten
                    foreach (XElement trans in sense.Elements(xns + "trans"))
                    {
                        //Console.WriteLine(trans.ToString());
                        List<String> trPerTrans = new List<String>();
                        string transData = "";

                        #region Einzelne_<tr>'s_auflisten
                        foreach (XElement tr in trans.Elements(xns + "tr"))
                        {
                            string trData = manage_trContent(tr);
                            if ((trData = trData.TrimEnd()) != "") trPerTrans.Add(trData);
                        }
                        #endregion

                        transData = String.Join(", ", trPerTrans.ToArray());
                        if (transData != "") transPerSense.Add(transData);
                    }
                    #endregion

                    senseData = String.Join("; ", transPerSense.ToArray());
                    if (senseData != "") senses.Add("(" + ++senseCount + ") " + senseData);
                }

                searchResults.Rows.Add(++resultCount, manage_orths(resultEntry.Element("orths")), manage_gramGrpData(resultEntry.Element(xns + "gramGrp")), manage_prons(resultEntry.Element("prons")), (String.Join(". ", senses) + "."));
            }
            return searchResults;
        }

        private DataTable defineResultTable(string tableName)
        {
            DataTable result = new DataTable(tableName);            // Erzeugt ein neues DataTable-Objekt für die Suchergebnisse
            DataColumn column;                                      // Deklariert eine Hilfsvariable für die Spaltendefinition.



            column = new DataColumn                                 // Erzeugz eine neue DataColumn, und legt Folgendes fest:
            {
                DataType = System.Type.GetType("System.Int32"),     // Setzt Datentyp auf "int".
                ColumnName = "Eintrag",                             // Der Spaltenname ist "Eintrag".
                ReadOnly = true,                                    // Die Inhalte dieser Spalte können nur gelesen werden.
            };
            result.Columns.Add(column);                             // Fügt die definierte Spalte "Eintrag" zur DataColumnCollection result.Columns hinzu.



            column = new DataColumn                                 // Erzeugz eine neue DataColumn, und legt Folgendes fest:
            {
                DataType = System.Type.GetType("System.String"),    // Setzt Datentyp auf "String".
                ColumnName = "Schreibweise",                        // Der Spaltenname ist "Schreibweise".
                ReadOnly = true,                                    // Die Inhalte dieser Spalte können nur gelesen werden.
            };
            result.Columns.Add(column);                             // Fügt die definierte Spalte "Schreibweise" zur DataColumnCollection result.Columns hinzu.



            column = new DataColumn                                 // Erzeugz eine neue DataColumn, und legt Folgendes fest:
            {
                DataType = System.Type.GetType("System.String"),    // Setzt Datentyp auf "String".
                ColumnName = "Wortart",                             // Der Spaltenname ist "Wortart".
                ReadOnly = true,                                    // Die Inhalte dieser Spalte können nur gelesen werden.
            };
            result.Columns.Add(column);                             // Fügt die definierte Spalte "Wortart" zur DataColumnCollection result.Columns hinzu.



            column = new DataColumn                                 // Erzeugz eine neue DataColumn, und legt Folgendes fest:
            {
                DataType = System.Type.GetType("System.String"),    // Setzt Datentyp auf "String".
                ColumnName = "Umschrift",                           // Der Spaltenname ist "Umschrift".
                ReadOnly = true,                                    // Die Inhalte dieser Spalte können nur gelesen werden.
            };
            result.Columns.Add(column);                             // Fügt die definierte Spalte "Umschrift" zur DataColumnCollection result.Columns hinzu.



            column = new DataColumn                                 // Erzeugz eine neue DataColumn, und legt Folgendes fest:
            {
                DataType = System.Type.GetType("System.String"),    // Setzt Datentyp auf "String".
                ColumnName = "Bedeutungen",                         // Der Spaltenname ist "Bedeutungen".
                ReadOnly = true,                                    // Die Inhalte dieser Spalte können nur gelesen werden.
            };
            result.Columns.Add(column);                             // Fügt die definierte Spalte "Bedeutungen" zur DataColumnCollection result.Columns hinzu.



            DataColumn[] PrimaryKeyColumns = new DataColumn[1];     // Erzeugt einen Hilfsarray, der die Schlüsselspalte(n) aufnehmen soll.
            PrimaryKeyColumns[0] = result.Columns["Eintrag"];       // Speichert die "Eintrag"-Spalte im Hilfsarray ab.
            result.PrimaryKey = PrimaryKeyColumns;                  // Markiert die "Eintrag"-Spalte als Primärschlüssel.

            return result;                                          // Gibt die fertig definierte Tabelle "result" zurück.
        }

        private string manage_orths(XElement orths)
        {
            List<String> orthsData = new List<String>();
            List<String> deviatingOrthsData = new List<String>();

            int orthCount = 0;
            string orthsString = "";

            foreach (XElement orth in orths.Elements())
            {
                string orthData = "";
                bool variant = false;

                if (orth.Attribute("midashigo") == null)
                {
                    if (orth.Attribute("irr") != null)
                    {
                        if (orth.Value != "") orthData = orth.Value;
                        variant = true;
                    }
                    else
                    {
                        if (orth.Value != "") orthData = orth.Value;
                    }
                }

                if (orthData != "")
                {
                    if (variant)
                    {
                        deviatingOrthsData.Add("(" + ++orthCount + ") " + orthData);
                    }
                    else
                    {
                        orthsData.Add("(" + ++orthCount + ") " + orthData);
                    }
                }
            }
            orthsString = String.Join("; ", orthsData) + ((deviatingOrthsData.Count > 0) ? ("; [auch: " + String.Join("; ", deviatingOrthsData) + "]") : "");
            return orthsString;
        }

        private string manage_gramGrpData(XElement gramGrp)
        {
            List<String> gramGrpData = new List<String>();
            string gramGrpString = "";

            XElement trDummy = new XElement("tr");
            string posData = "";

            if (gramGrp != null)
            {
                foreach (XElement elem in gramGrp.Elements())
                {
                    posData = "";
                    if (elem.Name == xns + "pos")
                    {
                        if (trDummy.Elements().Count() > 0)
                        {
                            if ((posData = manage_trContent(trDummy)) != "")
                            {
                                gramGrpData.Add(posData);
                                posData = "";
                                trDummy = new XElement("tr");
                            }
                        }
                        if (elem.HasAttributes)
                        {
                            if (elem.Attribute("type") != null)
                            {
                                posData = (string)posDeciphering[elem.Attribute("type").Value];
                            }
                            if (elem.HasElements)
                            {
                                string posDescendantsData = "";
                                posData += ((posDescendantsData = manage_trContent(elem)) != "")
                                            ? (": " + posDescendantsData) : "";
                            }
                        }
                        if (posData != "") gramGrpData.Add(posData);
                    }
                    else
                    {
                        trDummy.Add(elem);
                    }
                }
                if (trDummy.Elements().Count() > 0)
                {
                    if ((posData = manage_trContent(trDummy)) != "") gramGrpData.Add(posData);
                }
            }
            return ((gramGrpString = String.Join("; ", gramGrpData)) != "") ? (gramGrpString + ";") : "";
        }

        private string manage_prons(XElement prons)
        {
            List<String> pronsData = new List<String>();
            List<String> deviatingPronsData = new List<String>();

            int pronCount = 0;
            string pronsString = "";

            foreach (XElement pron in prons.Elements())
            {
                string pronData = "";
                bool variant = false;

                if (pron.Attribute("type") != null)
                {
                    if (pron.Attribute("type").Value == "romaji")
                    {
                        if (pron.Value != "") pronData = pron.Value;
                        variant = true;
                    }
                }
                else
                {
                    if (pron.Value != "") pronData = pron.Value;
                }

                if (pronData != "")
                {
                    if (variant)
                    {
                        deviatingPronsData.Add("(" + ++pronCount + ") " + pronData);
                    }
                    else
                    {
                        pronsData.Add("(" + ++pronCount + ") " + pronData);
                    }
                }
            }
            pronsString = String.Join("; ", pronsData) + ((deviatingPronsData.Count > 0) ? ("; [Romaji: " + String.Join("; ", deviatingPronsData) + "]") : "");
            return pronsString;
        }

        private string manage_trContent(XElement tr)
        {
            //Console.WriteLine(tr.ToString());
            bool first = true;
            bool followingSpaceOld = false;
            bool followingSpaceNew = false;
            bool preceedingSpace = false;

            List<String> elemPerTr = new List<String>();
            string trData = "";

            foreach (XElement elem in tr.Elements())
            {
                //Console.WriteLine(elem.ToString());
                string elemData = "";


                if (elem.Name == xns + "text")
                {
                    if (elem.Attribute("hasPrecedingSpace") != null)
                    {
                        if (elem.Attribute("hasPrecedingSpace").Value == "true")
                        {
                            preceedingSpace = true;
                        }
                    }
                    if (elem.Attribute("hasFollowingSpace") != null)
                    {
                        if (elem.Attribute("hasFollowingSpace").Value == "true")
                        {
                            followingSpaceNew = true;
                        }
                    }
                    elemData = elem.Value;
                }


                else if (elem.Name == xns + "token")
                {
                    string tokenInfo = "";

                    if (elem.Attribute("type") != null)
                    {

                        if (elem.Attribute("type").Value == "N")
                        {
                            string numerus = "";
                            string genus = "";

                            foreach (XAttribute attr in elem.Attributes())
                            {
                                switch (attr.Name.ToString())
                                {
                                    case "genus":
                                        genus = elem.Attribute("genus").Value; break;
                                    case "numerus":
                                        numerus = (elem.Attribute("numerus").Value == "pl") ? "Pl." : "Sg."; break;
                                }
                            }
                            if (genus != "")
                            {
                                tokenInfo = "{" + genus + ".";
                                if (numerus != "") tokenInfo += ", " + numerus;
                                tokenInfo += "}";
                            }
                            else
                            {
                                if (numerus != "") tokenInfo = "{" + numerus + "}";
                            }
                        }
                    }
                    elemData = elem.Value + tokenInfo;
                }


                else if (elem.Name == xns + "bracket")
                {
                    string bracketContent = manage_bracketData(elem);
                    if (bracketContent != "") elemData = "(" + bracketContent + ")";
                }


                else
                {
                    string otherContent = manage_otherData(elem);
                    if (otherContent != "") elemData = otherContent;
                }


                if (!(elemData == ""))
                {
                    if (first)
                    {
                        trData += elemData + ((followingSpaceNew) ? " " : "");
                        first = false;
                    }
                    else
                    {
                        if (!followingSpaceOld)         // Falls das vorherige Element kein Leerzeichen am Ende aufweist, ...
                        {
                            if (!preceedingSpace)           // ... und falls das gerade betrachtete Element auch kein führendes Leerzeichen erhalten soll, ...
                            {
                                trData += ((elemData.StartsWith(", ")) ? "" : ", ");        // ... so füge, falls nicht bereits vorhanden, ", " als Trennelement ein.
                            }
                            else                            // ... und falls das gerade betrachtete Element ein führendes Leerzeichen erhalten soll, ...
                            {
                                trData += " ";                                              // ... so füge das führende Leerzeichen ein.
                            }
                        }
                        trData += elemData + ((followingSpaceNew) ? " " : "");
                    }

                    followingSpaceOld = followingSpaceNew;
                    followingSpaceNew = false;
                    preceedingSpace = false;
                }
            }
            return trData;
        }

        private string manage_bracketData(XElement bracket)
        {
            List<String> elemsPerBracket = new List<String>();
            if (bracket.HasElements)
            {
                foreach (XElement bracketElem in bracket.Elements())
                {
                    string bracketElemContent = manage_otherData(bracketElem);
                    if (bracketElemContent != "") elemsPerBracket.Add(bracketElemContent);
                }
            }
            else
            {
                if (bracket.Value != "") elemsPerBracket.Add(bracket.Value);
            }
            return String.Join("; ", elemsPerBracket);
        }

        private string manage_otherData(XElement other)
        {
            List<String> elemsPerOther = new List<String>();
            if (other.HasElements)
            {
                foreach (XElement otherElem in other.Elements())
                {
                    if (otherElem.Name == xns + "bracket")
                    {
                        string bracketContent = manage_bracketData(otherElem);
                        if (bracketContent != "") elemsPerOther.Add("(" + bracketContent + ")");
                    }
                    string otherElemContent = manage_otherData(otherElem);
                    if (otherElemContent != "") elemsPerOther.Add(otherElemContent);
                }
            }
            else
            {
                if (other.Value != "") elemsPerOther.Add(other.Value);
            }
            return String.Join(", ", elemsPerOther);
        }
    }
}