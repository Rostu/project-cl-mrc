using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml.Linq;
using System.Windows.Forms;
using System.IO;
using System.Collections;

namespace JADE
{
    /// <summary>
    ///*SearchEngine-Klasse enthält die Funktionen zum Suchen eines Token im Wörterbuch. Mit Hilfe von Linq to XML wird wadoku.xml durchsucht, um für ein gesuchtes Token entsprechende Einträge zu finden. Dabei gibt es die Möglichkeit, nach genau übereinstimmenden Einträgen zu suchen oder aber nach allen Einträgen, die wie das Token beginnen. So können eventuelle falsche Zerlegungen des TinySegmenters mit Hilfe von Wadoku und der Bearbeiten-Funktion verbessert werden.
    ///*<para>Benutzt wurde eine XML-Dump des frei verfügbaren Wörterbuch-Projektes Wadoku</para>
    ///*<para><a href="http://www.wadoku.de/wiki/x/ZQE">http://www.wadoku.de/wiki/x/ZQE</a></para>
    /// </summary>
    public class SearchEngine
    {
        /**Diese Variable dient der Klasse SearchEngine als Referenz auf ein DataSet, das die einzelnen Suchergebnisse, die bereits angefordert wurden, speichert. */
        private static System.Data.DataSet dataSet;
        /// @cond
        private static SearchEngine engine; //Platzhalter, der zur Umsetzung des Singleton-Entwurfsmusters dient.
        private static XNamespace xns; //Namespace-Angabe für Linq to XML wird hier gespeichert.
        private static XDocument wadoku; //Variable für die Referenz auf das Wörterbuch.
        private static Hashtable posDeciphering; // Hier wird beim Initialisieren eine Referenz auf eine Hashtable gespeichert, die bei der Erstellung von Einträgen für das Suchergebnis POS-Tag-Abkürzungen im Wörterbuch leserlich darstellbar macht. 
        /// @endcond
        /**
         * Private Konstruktor für die SearchEngine.
         */ 
        private SearchEngine()
        {
            if (engine != null) //Falls bereits eine Instanz dieser Klasse erzeugt worden ist, ... => siehe Exception-Nachricht
            {
                throw new InvalidOperationException("Es ist unmöglich eine weitere Instanz der Singleton-Klasse SearchEngine zu erzeugen.");
            }
            if (!File.Exists("wadoku.xml")) //Dient dazu, Probleme beim Lesezugriff auf das Wörterbuch abzufangen. => siehe Exception-Nachricht
            {
                throw new ArgumentException("Die Wörterbuchdatei konnte nicht gefunden werden.");
            }

            dataSet = new DataSet(); //Initialisiere das benötigte DataSet dataSet.
            xns = "http://www.wadoku.de/xml/entry"; //Angabe des Namespaces für wadoku.xml
            wadoku = XDocument.Load("wadoku.xml"); //Relativer Pfad zu wadoku.xml wird hier angegeben

            posDeciphering = new Hashtable() //Hashtable, die bei der Erstellung von Einträgen für das Suchergebnis POS-Tag-Abkürzungen im Wörterbuch leserlich darstellbar macht
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
        /**
         * Get-Funktion, die das Singelton-Entwurfsmuster für die Klasse SearchEngine umsetzt. Dadurch wird gewährleistet, dass nur eine und immer die selbe Instanz der SearchEngine Klasse verwendet wird.
         * Überprüft, ob es schon eine Instanz der Klasse SearchEngine gibt und erstellt falls nicht eine solche.
        */ 
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


        /**
        * Funktion zum Leeren der DataTable, in der sich die Suchergebnisse befinden.
        */ 
        public void clearDataSet()
        {
            while (dataSet.Tables.Count > 0) // Solange noch Tabellen in dataSet gespeichert sind ...
            {
                DataTable table = dataSet.Tables[0]; // ... betrachte die die Tabelle mit Indexwert 0 und ...
                if (dataSet.Tables.CanRemove(table)) dataSet.Tables.Remove(table); // ... falls die Tabelle entfernt werden kann, entferne sie auch.
            }
        }

        /**
         * Funktion zum Löschen einer Table aus dem Dataset. Dies kann notwendig sein wenn, ein Token geändert wurde. Der bereits bestehende (alte) Eintrag muss nun aus der Datatable gelöscht werden.
         * Damit bei einer erneuten Suchanfrage nicht das alte Table-Objekt aufgerufen wird, sondern eine neue Suche initiiert wird.
         * @param[in] satzNr Int-Wert des Satzes in dem sich der Token befindet dessen Table-Objekt in dem Dataset gelöscht werden soll.
         * @param[in] tokenNr Int-Wert des Tokens dessen Table-Objekt in dem Dataset gelöscht werden soll.
        */
        public static void DisposeTable(int satzNr, int tokenNr)
        {
            //Die Variablen speichern (u.U. auch null-Referenz) Referenzen auf evtl. vorhandenen Tabellen mit Suchergebnissen (jeweils für genaue Übereinstimmung und 
            //für Einträge, die wie das Token beginnen) für das durch die Parameter eindeutig bestimmtes Token.
            DataTable tableToDispose = dataSet.Tables["results_" + satzNr + tokenNr];
            DataTable tableToDispose_absolute = dataSet.Tables["results_" + satzNr + tokenNr + "_absolute"];

            //Falls die gefundenen Tabellen gefunden wurden (keine null-Referenz) und sie gelöscht werden können, werden sie entfernt.
            if (dataSet.Tables.CanRemove(tableToDispose)) dataSet.Tables.Remove(tableToDispose);
            if (dataSet.Tables.CanRemove(tableToDispose_absolute)) dataSet.Tables.Remove(tableToDispose_absolute);
        }

        /**
         * Suchfunktion. Erhält Informationen über das gesuchte Token, sucht in wadoku.xml und liefert eine DataTable mit Ergebnissen zurück. 
         * @param[in] satzNr Int-Wert des Satzes in dem sich das Token befindet, der im Wörterbuch gesucht werden soll.
         * @param[in] tokenNr Int-Wert des Tokens der im Wörterbuch gesucht werden soll.
         * @param[in] token String-Repräsentation des gesuchten Token.
         * @param[in] absolute bool-Wert der angibt, ob nach genauer Übereinstimmung oder extensiv gesucht werden soll.
         * @param[out] outputTable DataTable-Objekt mit den gefundenen Suchergebnissen.
        */
        public DataTable search(string token, int satzNr, int tokenNr, bool absolute)
        {
            //Wenn im dataSet schoneinmal dasselbe Token mit/ohne genauer Übereinstimmung gesucht wurde, ...
            if (dataSet.Tables.Contains("results_" + satzNr + tokenNr + ((absolute) ? "_absolute" : "")))
            {
                //gib die entsprechende Tabelle zurück.
                return dataSet.Tables["results_" + satzNr + tokenNr + ((absolute) ? "_absolute" : "")];
            }
            //Falls noch nicht nach dem Token gesucht wurde, führe eine Suche in Wadoku aus.
            else
            {
                //Erzeuge einen Knoten, der alle Suchergebnisknoten enthält,
                XElement results = new XElement("results",
                                //Durchsuche alle Knoten, die der Wurzelknoten in wadoku.xml enthält, ...
                                (from entry in wadoku.Root.Elements().OfType<XElement>()
                                 //lasse validateEntry den <form>-Knoten auf die durch absolute bestimmte Weise nach dem String token durchsuchen 
                                 where validateEntry(entry.Element(xns + "form"), token, absolute)
                                 //und wähle für einen neu erstellten <result>-Knoten die folgenden Elemente aus dem Wadoku-Eintrag aus:
                                 select new XElement("result", new XElement("orths", entry.Element(xns + "form").Elements(xns + "orth")),   //neues xml-Element, das aus dem <form>-Knoten alle japanischen Schreibweisen aufnimmt
                                                               entry.Element(xns + "gramGrp"),                                              //xml-Element mit grammatischen (u.a. POS-Infos) Informationen 
                                                               new XElement("prons", entry.Element(xns + "form").Elements(xns + "pron")),   //neues xml-Element, das aus dem <form>-Knoten alle japanischen Umschriften aufnimmt
                                                               new XElement("senses", entry.Elements(xns + "sense")))));                    //neues xml-Element, das aus dem Eintrag alle <sense>-Knoten mit deutschen Bedeutungen aufnimmt      
                //Wurden <result>-Knoten für gefundene Einträge erstellt, ...
                DataTable outputTable = (results.HasElements) ?
                                            //So erstelle eine DataTable mit entsprechend formatierten Ergenissen. Erstelle ansonsten eine leere DataTable.
                                            (createResultTable(results, satzNr, tokenNr, absolute)) : (new DataTable());
                //Gib die der Variable outputTable zugewiesene Tabelle zurück.
                return outputTable;
            }

        }

        /**
         * Die Funktion durchsucht einen Wadokueintrag nach japanischen Zeichenketten, die je nach Ausprägung von absolute genau mit token übereinstimmen oder mit token beginnen.
         * @param[in] form XElement-Objekt, welches das Form-Element eines Wadoku-Eintrages beinhaltet.
         * @param[in] token String-Repräsentation des gesuchten Token.
         * @param[in] absolute bool-Wert, der angibt ob nach genauer Übereinstimmung oder Extensiv gesucht wird.
         * @param[out] entryIsValid bool-Wert der angibt, ob Übereinstimmung vorliegt oder nicht.
        */
        private bool validateEntry(XElement form, string token, bool absolute)
        {
            bool entryIsValid = false; //Nimm an, dass der Eintrag nicht passend ist.
            bool currentFormDescendantIsValid = false; //Nimm an, dass der gerade in der Schleife betrachtete Knoten innerhalb des <form>-Knotens nicht passend ist.

            foreach (XElement formEntry in form.Elements())//Durchlaufe alle Elemente im <form>-Knoten.
            {
                //Falls der <text>-Knoten im gerade betrachteten Element denselben String wie token enthält oder ... 
                if ((absolute && (currentFormDescendantIsValid = formEntry.Element(xns + "text").Value == token)) ||
                    // ... falls der <text>-Knoten im gerade betrachteten Element einen String enthält, der mit dem String token beginnt ... 
                    (!absolute && (currentFormDescendantIsValid = formEntry.Element(xns + "text").Value.StartsWith(token)))) 
                    // ... so wird currentFormDescendantIsValid auf true gesetzt ...
                {
                    // ... und auch entryIsValid wird auf true gesetzt.
                    entryIsValid = currentFormDescendantIsValid;
                }
                //Ansonsten wird currentFormDescendantIsValid auf false gesetzt und entryIsValid wird nicht verändert (bleibt also ggf. true).
            }
            return entryIsValid; //Der Wert von entryIsValid wird nun zurückgegeben.
        }

        /**
         * Diese Funktion wird von search dazu verwendet um eine DataTable für die Anzeige zu erstellen. Mit Hilfe von defineResultTable wird dem dataSet eine Tabelle hinzugefügt und innerhalb der Funktion createResultTable selbst wird die Tabelle mit formatierten Daten aus der Wadoku-XML-Datei gefüllt.
         * @param[in] resEntries Suchergebnisse
         * @param[in] satzNr int-Wert des Satzes, in dem sich der Token befindet.
         * @param[in] tokenNr int-Wert des Tokens.
         * @param[in] absolute bool-Wert der angibt, ob nach genauer Übereinstimmung oder Extensiv gesucht wird.
         * @param[out] searchResults DataTable-Objekt mit den Suchergebnissen.
        */
        private DataTable createResultTable(XElement resEntries, int satzNr, int tokenNr, bool absolute)
        {
            //Speichere die von defineResultTable definierte und zurückgegebene Tabelle in searchResults.
            DataTable searchResults = defineResultTable("results_" + satzNr + tokenNr + ((absolute) ? "_absolute" : ""));
            dataSet.Tables.Add(searchResults); // Add the new DataTable to the DataSet.

            //Zähler für die Spalte "Eintrag"            
            int resultCount = 0;

            //Durchlaufe alle Elemente, die im XElement resEntries enthalten sind
            foreach (XElement resultEntry in resEntries.Nodes())
            {
                //Erzeuge eine Instanz der generischen Klasse List vom Typ String, um die einzeln formatierten deutschen Bedeutungen zu speichern.
                List<String> senses = new List<String>();

                //Zähler für die Bedeutungen eines Ergebniseintrags
                int senseCount = 0;

                //Durchlaufe alle <sense>-Knoten im Element <senses> des Eintrags
                foreach (XElement sense in resultEntry.Element("senses").Elements(xns + "sense"))
                {
                    //Erzeuge eine Instanz der generischen Klasse List vom Typ String, um die einzelnen Alternativen einer deutschen Bedeutung zu speichern.
                    List<String> transPerSense = new List<String>();
                    //Diese Variable dient dem Zwischenspeichern von formatiertem Text für eine der Bedeutungen eines Eintrags.
                    string senseData = "";

                    #region Einzelne <trans>-Knoten-Inhalte durchlaufen
                    foreach (XElement trans in sense.Elements(xns + "trans"))
                    {
                        //Erzeuge eine Instanz der generischen Klasse List vom Typ String, um die einzeln formatierten Elemente eines <trans>-Knotens zu speichern.
                        List<String> trPerTrans = new List<String>();
                        //Diese Variable dient dem Zwischenspeichern von formatiertem Text für eine Alternative einer der Bedeutungen eines Eintrags.
                        string transData = "";

                        #region Einzelne <tr>-Knoten-Inhalte durchlaufen
                        foreach (XElement tr in trans.Elements(xns + "tr"))
                        {
                            //Durchsuche rekursiv die <tr>-Knoten nach weiteren formatierbaren Elementen und speichere den formatierten String in trData.
                            string trData = manage_trContent(tr);
                            //Falls trData, nachdem Leerzeichen am Ende entfernt wurden, keinen leeren String enthält, so füge ihn der Liste trPerTrans hinzu. 
                            if ((trData = trData.TrimEnd()) != "") trPerTrans.Add(trData);
                        }
                        #endregion
                        //Verbinde die Elemente der Liste trPerTrans mit ", " und speichere den sich ergebenden String in transData.
                        transData = String.Join(", ", trPerTrans.ToArray());
                        //Falls transData keinen leeren String enthält, dann füge den enthaltenen String zur Liste transPerSense hinzu.
                        if (transData != "") transPerSense.Add(transData);
                    }
                    #endregion
                    //Füge die Alternativen einer Bedeutung (enthalten in der Liste transPerSense) mit dem Trennzeichen "; " zusammen und speichere den formatierten String in senseData.
                    senseData = String.Join("; ", transPerSense.ToArray());
                    ////Falls senseData keinen leeren String enthält, dann füge den enthaltenen String zusammen mit einer vorangestellten Bedeutungsnummer zur Liste senses hinzu.
                    if (senseData != "") senses.Add("(" + ++senseCount + ") " + senseData);
                }
                //Füge der in defineResultTable definierten und dem dataSet hinzugefügten Tabelle eine neue Zeile mit formatierten Daten hinzu.
                searchResults.Rows.Add
                (
                    ++resultCount, //Inkrementiere den Zähler für die Spalte "Eintrag" um 1 und gib die so erhaltene Zahl zurück.
                    manage_orths(resultEntry.Element("orths")), //Gib die verschiedenen Schreibweisen formatiert aus.
                    manage_gramGrpData(resultEntry.Element(xns + "gramGrp")), //Gib die Grammatikinformationen formatiert aus.
                    manage_prons(resultEntry.Element("prons")), //Gib die Umschriften formatiert aus.
                    (String.Join(". ", senses) + ".") //Füge die formatierten deutschen Bedeutungen in der Liste senses zusammen. Die Trennzeichen sind ". ".
                );
            }
            return searchResults;
        }

        /**
         * Diese Funktion definiert für die gefundenen Wörterbucheinträge eine DataTable mit den Spalten für die Eintragsnummer, die Schreibweisen, die Wortart, die Umschrift und die Bedeutungen. Der Name der definierten Tabelle wird durch tableName bestimmt.
         * @param[in] tableName Name der zu erstellenden Table.
         * @param[out] result Die vollständig definierte Tabelle.
         */
        private DataTable defineResultTable(string tableName)
        {
            DataTable result = new DataTable(tableName);            // Erzeugt ein neues DataTable-Objekt für die Suchergebnisse
            DataColumn column;                                      // Deklariert eine Hilfsvariable für die Spaltendefinition.



            column = new DataColumn                                 // Erzeugt eine neue DataColumn, und legt Folgendes fest:
            {
                DataType = System.Type.GetType("System.Int32"),     // Setzt Datentyp auf "int".
                ColumnName = "Eintrag",                             // Der Spaltenname ist "Eintrag".
                ReadOnly = true,                                    // Die Inhalte dieser Spalte können nur gelesen werden.
            };
            result.Columns.Add(column);                             // Fügt die definierte Spalte "Eintrag" zur DataColumnCollection result.Columns hinzu.



            column = new DataColumn                                 // Erzeugt eine neue DataColumn, und legt Folgendes fest:
            {
                DataType = System.Type.GetType("System.String"),    // Setzt Datentyp auf "String".
                ColumnName = "Schreibweise",                        // Der Spaltenname ist "Schreibweise".
                ReadOnly = true,                                    // Die Inhalte dieser Spalte können nur gelesen werden.
            };
            result.Columns.Add(column);                             // Fügt die definierte Spalte "Schreibweise" zur DataColumnCollection result.Columns hinzu.



            column = new DataColumn                                 // Erzeugt eine neue DataColumn, und legt Folgendes fest:
            {
                DataType = System.Type.GetType("System.String"),    // Setzt Datentyp auf "String".
                ColumnName = "Wortart",                             // Der Spaltenname ist "Wortart".
                ReadOnly = true,                                    // Die Inhalte dieser Spalte können nur gelesen werden.
            };
            result.Columns.Add(column);                             // Fügt die definierte Spalte "Wortart" zur DataColumnCollection result.Columns hinzu.



            column = new DataColumn                                 // Erzeugt eine neue DataColumn, und legt Folgendes fest:
            {
                DataType = System.Type.GetType("System.String"),    // Setzt Datentyp auf "String".
                ColumnName = "Umschrift",                           // Der Spaltenname ist "Umschrift".
                ReadOnly = true,                                    // Die Inhalte dieser Spalte können nur gelesen werden.
            };
            result.Columns.Add(column);                             // Fügt die definierte Spalte "Umschrift" zur DataColumnCollection result.Columns hinzu.



            column = new DataColumn                                 // Erzeugt eine neue DataColumn, und legt Folgendes fest:
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


        /// @cond

        /**
         * Diese Funktion dient der Formatierung der Schreibweisen.
         * @param[in] orth XElement-Objekt, das die Schreibweise-Daten aus einem Eintrag in Wadoku enthält
         * @param[out] orthsString string-Wert mit formatierten Schreibweise-Daten
         */
        private string manage_orths(XElement orths)
        {
            //Liste für Schreibweise-Textlemente ohne besondere Attribute
            List<String> orthsData = new List<String>();
            //Liste für Schreibweise-Textlemente mit besonderen Attributen, die für Redundanz in der Anzeige sorgen
            List<String> deviatingOrthsData = new List<String>();

            ////Zähler für die einzelnen Schreibweisen
            int orthCount = 0;

            //Zwischenspeicher für die formatierte Schreibweise-Zeichenkette
            string orthsString = "";

            //Durchlaufe alle Schreibweise-Elemente 
            foreach (XElement orth in orths.Elements())
            {
                //Zwischenspeicher für die Zeichenketten in den Schreibweise-Elementen
                string orthData = "";
                //Diese Variable wird auf true gesetzt, wenn eine Zeichenkette in die Liste deviatingOrthsData gehört.
                bool variant = false;

                //Diese Verzweigungen bereiten die Sortierung der Schreibweise-Elemente in die Listen orthsData und deviatingOrthsData vor. 
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

                //Falls verwendbare orthData gefunden wurde, ... 
                if (orthData != "")
                {
                    if (variant)
                    {
                        // ... werden Schreibweise-Elemente mit besonderen Attributen zunächst in deviatingOrthsData gesammelt
                        deviatingOrthsData.Add(orthData);
                    }
                    else
                    {
                        // ... werden Schreibweise-Elemente ohne besondere Attribute nummeriert in orthsData gesammelt
                        orthsData.Add("(" + ++orthCount + ") " + orthData);
                    }
                }
            }

            //Schreibweise-Elemente mit besonderen Attributen werden jetzt durchnummeriert in der Liste NumberedDeviatingOrthsData gespeichert
            List<String> NumberedDeviatingOrthsData = new List<String>();

            foreach (var deviatingOrth in deviatingOrthsData)
            {
                NumberedDeviatingOrthsData.Add("(" + ++orthCount + ") " + deviatingOrth);
            }

            //zuerst werden die Schreibweise-Elemente ohne besondere Attribute zusammengefügt, dann folgen die Schreibweise-Elemente mit besonderen Attributen (falls welche vorhanden sind) in eckigen Klammern. 
            //Trennzeichen sind in beiden Fällen "; ".
            orthsString = String.Join("; ", orthsData) + ((NumberedDeviatingOrthsData.Count > 0) ? ("; [auch: " + String.Join("; ", NumberedDeviatingOrthsData) + "]") : "");
            
            //Die mit dieser Formattierung in orthsString gespeicherte Zeichenkette wird nun zurückgegeben.
            return orthsString;
        }

        /**
         * Diese Funktion dient der Formatierung der grammatischen Informationen
         * @param[in] gramGrp orth XElement-Objekt, das die grammatischen Informationen aus einem Eintrag in Wadoku enthält
         * @param[out] string-Wert mit formatierten Schreibweise-Daten
         */
        private string manage_gramGrpData(XElement gramGrp)
        {
            //Liste für Schreibweise-Textelemente ohne besondere Attribute
            List<String> gramGrpData = new List<String>();
            //Zwischenspeicher für die formatierte grammatische Informationen
            string gramGrpString = "";

            //Dummy-Element, das XML-Elemente aufsammelt, die (noch) nicht gesondert bei der Formatierung behandelt werden (Wadoku-Eintragsstruktur teilweise unklar, oder aber explizit als in Zukunft erweiterbar beschrieben)
            XElement trDummy = new XElement("tr");

            //Zwischenspeicher für die formatierte grammatische Informationen eines Elements im <gramGrp>-Knoten
            string posData = "";

            //Falls <gramGrp>-Knoten vorhanden ist ... 
            if (gramGrp != null)
            {
                // ... durchlaufe alle Elemente auf der nächsttieferen Ebene, ...
                foreach (XElement elem in gramGrp.Elements())
                {
                    //Setze posData auf den leeren String
                    posData = "";

                    //Falls es sich um POS-Informationen handelt, ...
                    if (elem.Name == xns + "pos")
                    {
                        // ... formatiere, falls vorhanden, die Inhalte des trDummy-Elements mittels manage_trContent.
                        if (trDummy.Elements().Count() > 0)
                        {
                            if ((posData = manage_trContent(trDummy)) != "")
                            {
                                gramGrpData.Add(posData); //Füge formatierte Daten der Liste hinzu

                                //Setze Hilfsvariablen zurück
                                posData = "";
                                trDummy = new XElement("tr");
                            }
                        }
                        // ... und falls das POS-Element Attribute besitzt ...
                        if (elem.HasAttributes)
                        {
                            // .... und das Attribut type vorhanden ist, ...
                            if (elem.Attribute("type") != null)
                            {
                                // ... so formatiere die POS-Infos mit der Hashtable posDeciphering.
                                posData = (string)posDeciphering[elem.Attribute("type").Value];
                            }
                            //Überprüfung des POS-Elements auf untergeordnete Elemente
                            if (elem.HasElements)
                            {
                                //Füge eventuell vorhandene ergänzende Daten nach einem Doppelpunkt den POS-Infos hinzu
                                string posDescendantsData = "";
                                posData += ((posDescendantsData = manage_trContent(elem)) != "")
                                            ? (": " + posDescendantsData) : "";
                            }
                        }
                        //Falls posData nicht leer ist, sammle die Daten in der Liste gramGrpData
                        if (posData != "") gramGrpData.Add(posData);
                    }
                    //Aufsammlung der XML-Elemente, die nicht POS-Elemente sind und (noch) nicht gesondert bei der Formatierung behandelt werden
                    else
                    {
                        trDummy.Add(elem);
                    }
                }
                //Falls trDummy nach dem Ende der foreach-Schleife nicht leer ist, formatiere die noch vorhandenen Daten
                if (trDummy.Elements().Count() > 0)
                {
                    if ((posData = manage_trContent(trDummy)) != "") gramGrpData.Add(posData);
                }
            }
            //Falls gramGrpData Strings enthält, füge sie getrennt durch "; " und hänge ein weiteres ";" hinten an und gib diesen String zurück. gib ansonsten einen leeren String zurück.
            return ((gramGrpString = String.Join("; ", gramGrpData)) != "") ? (gramGrpString + ";") : "";
        }

        /**
         * Diese Funktion dient der Formatierung der Umschriften.
         * @param[in] prons XElement-Objekt, das die Umschriften aus einem Eintrag in Wadoku enthält
         * @param[out] pronsString string-Wert mit formatierten Umschrift-Daten
         */
        private string manage_prons(XElement prons)
        {

            //Der Aufbau dieser Funktion ist analog zu der für die Schreibweise-Daten

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
                        deviatingPronsData.Add(pronData);
                    }
                    else
                    {
                        pronsData.Add("(" + ++pronCount + ") " + pronData);
                    }
                }
            }

            List<String> NumberedDeviatingPronsData = new List<String>();

            foreach (var deviatingPron in deviatingPronsData)
            {
                NumberedDeviatingPronsData.Add("(" + ++pronCount + ") " + deviatingPron);
            }

            pronsString = String.Join("; ", pronsData) + ((deviatingPronsData.Count > 0) ? ("; [Romaji: " + String.Join("; ", NumberedDeviatingPronsData) + "]") : "");
            return pronsString;
        }

        /**
         * Diese Funktion dient der Formatierung der Informationen in <tr>-Elementen. Da sie eine rekursive Funktion aufruft, die flexibel auch tiefere Ebenen in der XML-Struktur durchsucht, wird sie auch stellenweise für Knoten verwendet, die keine <tr>-Knoten sind.
         * @param[in] tr XElement-Objekt, dessen Text-Inhalte formatiert werden sollen
         * @param[out] trData string-Wert, der die formatierten Texte für das <tr>-Element enthält
         */
        private string manage_trContent(XElement tr)
        {
            //verschiedene bool-Varaiblen, die dazu dienen, Attribute für Whitespace aus den Wadoku-Einträgen richtig zu beachten
            bool first = true; //hält fest, ob das gerade betrachtete Element das erste innerhalb des <tr>-Elements ist
            bool followingSpaceOld = false;
            bool followingSpaceNew = false;
            bool preceedingSpace = false;

            //Liste für Textelemente in einem <tr>-Element
            List<String> elemPerTr = new List<String>();
            //Zwischenspeicher für die formatierten Texte des <tr>-Elements
            string trData = "";

            //Durchlaufe alle dem <tr>-Element untergeordneten Elemente
            foreach (XElement elem in tr.Elements())
            {
                //Zwischenspeicher für die formatierten Texte von einem dem <tr>-Element untergeordneten Element
                string elemData = "";

                //Bei <text>-Elementen müssen die Whitespace-Attribute behandelt werden
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
                    elemData = elem.Value; //Übernehme den Text-Inhalt des XML-Elements
                }

                //Token-Elemente sind die zentralen Elemente einer Alternative innerhalb einer Bedeutung eines Eintrags, daher ... 
                else if (elem.Name == xns + "token")
                {
                    string tokenInfo = "";

                    // ... sind eventuell Genus- und Numerusinformationen in Attributen vorhanden, ...
                    if (elem.Attribute("type") != null)
                    {

                        if (elem.Attribute("type").Value == "N")
                        {
                            string numerus = "";
                            string genus = "";

                            // ... die in dieser Schleife herausgefiltert werden.
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

                            //Diese Verzweigung formatiert die gefundenen Informationen entsprechend in einem Klammerausdruck, ...
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

                    //... der dann dem Text im gerade betrachteten <token>-Element angehängt wird.
                    elemData = elem.Value + tokenInfo;
                }

                //Hier werden mit der rekursiven Funktion manage_bracketData Daten formatiert (falls vorhanden, werden sie in Klammern eingefasst), die in einem <bracket>-Element auftauchen.
                else if (elem.Name == xns + "bracket")
                {
                    string bracketContent = manage_bracketData(elem);
                    if (bracketContent != "") elemData = "(" + bracketContent + ")";
                }

                //eine flexible, rekursive Funktion, durchsucht Elemente anderen Typs auf weitere Informationen
                else
                {
                    string otherContent = manage_otherData(elem);
                    if (otherContent != "") elemData = otherContent;
                }

                //In den folgenden Verzweigungen wird trData die gesammelte elemData (falls vorhanden) angehängt. Dabei werden eventuell beobachtete Whitespace-Anforderungen umgesetzt.
                if (!(elemData == ""))
                {
                    //Hier wird für das erste dem <tr>-Element untergeordnete Element im Bezug auf Whitespace-Attribute nur das hasFollowingSpace-Attribut umgesetzt, falls vorhanden.
                    if (first)
                    {
                        trData += elemData + ((followingSpaceNew) ? " " : "");
                        first = false;
                    }

                    //Mit allen folgenden Elementen wird so verfahren:
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
                        trData += elemData + ((followingSpaceNew) ? " " : ""); //Falls nötig, füge ein folgendes Leerzeichen hinzu.
                    }

                    //Aktualisiere/Setze die Flaggen für folgende Elemente zurück. first wird natürlich nicht zurück auf true gesetzt. 
                    followingSpaceOld = followingSpaceNew;
                    followingSpaceNew = false;
                    preceedingSpace = false;
                }
            }
            //Gib die gesammelte trData-Zeichenkette zurück.
            return trData;
        }

        /**
         * Diese Funktion sammelt flexibel und rekursiv auf tieferen Ebenen formatierte Textdaten innerhalb von <bracket>-Elementen
         * @param[in] bracket XElement-Objekt, das das zu durchsuchende <bracket>-Element darstellt
         * @param[out] string-Wert, der die Daten innerhalb der Klammern zurückliefert
         */
        private string manage_bracketData(XElement bracket)
        {
            //Liste für Texte aus Elementen, die dem <bracket>-Element direkt untergeordnet sind. 
            List<String> elemsPerBracket = new List<String>();

            //Falls Elemente in der Klammer vorhanden sind, ...
            if (bracket.HasElements)
            {
                // ... so durchlaufe sie ...
                foreach (XElement bracketElem in bracket.Elements())
                {
                    // ... und sammle mit manage_otherData alle enthaltenen Texte, auch auf tieferen Ebenen in der XML-Struktur.
                    string bracketElemContent = manage_otherData(bracketElem);
                    //Wenn Texte gefunden wurden, sammle sie in der dafür vorgesehenen Liste elemsPerBracket.
                    if (bracketElemContent != "") elemsPerBracket.Add(bracketElemContent);
                }
            }
            //Hier wird der Text des <bracket>-Elements gesammelt, falls es keine untergeordneten Elemente besitzt und falls ein solcher Text vorhanden ist.
            else
            {
                if (bracket.Value != "") elemsPerBracket.Add(bracket.Value);
            }
            //Kombiniere die Texte in elemsPerBracket mit "; " und gib den so erhaltenen String zurück.
            return String.Join("; ", elemsPerBracket);
        }

        /**
        * Diese Funktion sammelt flexibel und rekursiv auf tieferen Ebenen formatierte Textdaten innerhalb von XML-Elementen, die nicht gesondert in anderen Funktionen behandelt werden.
        * @param[in] other XElement-Objekt, das das zu durchsuchende XML-Element darstellt
        * @param[out] string-Wert, der die Daten innerhalb des übergebenen Elements zurückliefert
        */
        private string manage_otherData(XElement other)
        {
            //Liste für Texte aus Elementen, die dem Element other direkt untergeordnet sind.
            List<String> elemsPerOther = new List<String>();

            //Falls untergeordnete Elemente im Element other vorhanden sind, ...
            if (other.HasElements)
            {
                // ... so durchlaufe sie ...
                foreach (XElement otherElem in other.Elements())
                {
                    // ... und falls ein Klammerausdruck benötigt wird, ...
                    if (otherElem.Name == xns + "bracket")
                    {
                        // ... ermittle die Texte in der Klammer mit manage_bracketData.
                        string bracketContent = manage_bracketData(otherElem);
                        //Falls etwas gefunden wurde, setze die Klammern entsprechend und sammle den String mit elemsPerOther auf.
                        if (bracketContent != "") elemsPerOther.Add("(" + bracketContent + ")");
                    }
                    //Ansonsten rufe manage_otherData für otherElem auf (Rekursivität). Wird Text zurückgegeben, so sammle in in der Liste elemsPerOther auf.
                    else
                    {
                        string otherElemContent = manage_otherData(otherElem);
                        if (otherElemContent != "") elemsPerOther.Add(otherElemContent);
                    }
                }
            }
            //Hier wird der Text des Elements other gesammelt, falls es keine untergeordneten Elemente besitzt und falls ein solcher Text vorhanden ist.
            else
            {
                if (other.Value != "") elemsPerOther.Add(other.Value);
            }
            //Kombiniere die Texte in elemsPerOther mit ", " und gib den so erhaltenen String zurück.
            return String.Join(", ", elemsPerOther);
        }

        /// @endcond 
    }
}