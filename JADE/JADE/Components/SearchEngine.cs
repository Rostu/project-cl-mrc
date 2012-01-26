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

        private static XNamespace rss = "http://www.wadoku.de/xml/entry";
        private static XDocument wadoku;


        private SearchEngine()
        {
            if (engine != null)
            {
                throw new InvalidOperationException("SearchEngine can only be instantiated once.");
            }
            if (!File.Exists("../../wadoku.xml"))
            {
                throw new ArgumentException("The dictionary file could not be found.");
            }
            wadoku = XDocument.Load("../../wadoku.xml");
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

        public DataTable search(string token, int satzNr, int tokenNr)
        {
            XElement res = new XElement("results",
                            (from e in wadoku.Root.Elements().OfType<XElement>()

                             where ((string)e.Element(rss + "form").Element(rss + "orth").Element(rss + "text")).StartsWith(token)

                             select new XElement("result", e.Element(rss + "form").Element(rss + "pron").Element(rss + "text"),
                                                            new XElement("senses", e.Elements(rss + "sense")))));
            //Console.Write(res.ToString());
            return createResultTable(res, token, satzNr, tokenNr);
        }

        //public string transform(XElement results)
        //{
        //    DataTable res
        //    return results;
        //}


        private DataTable createResultTable(XElement resEntries, string token, int satzNr, int tokenNr)
        {
            DataTable res;
            // Create a new DataTable as the result of the search.
            DataTable result = new DataTable("results_" + token + "_" + satzNr + "_" + tokenNr);
            // Declare variables for DataColumn and DataRow objects.
            DataColumn column;
            DataRow row;


            // Create new DataColumn, set DataType, 
            // ColumnName and add to DataTable.    
            column = new DataColumn
            {
                DataType = System.Type.GetType("System.Int32"),
                ColumnName = "Eintrag",
                ReadOnly = true,
            };
            // Add the Column to the DataColumnCollection.
            result.Columns.Add(column);


            // Create new DataColumn, set DataType, 
            // ColumnName and add to DataTable.    
            column = new DataColumn
            {
                DataType = System.Type.GetType("System.String"),
                ColumnName = "Umschrift",
                ReadOnly = true,
            };
            // Add the Column to the DataColumnCollection.
            result.Columns.Add(column);


            // Create new DataColumn, set DataType, 
            // ColumnName and add to DataTable.    
            column = new DataColumn
            {
                DataType = System.Type.GetType("System.String"),
                ColumnName = "Bedeutungen",
                ReadOnly = true,
            };
            // Add the Column to the DataColumnCollection.
            result.Columns.Add(column);

            //// Create second column.
            //column = new DataColumn();
            //column.DataType = System.Type.GetType("System.String");
            //column.ColumnName = "ParentItem";
            //column.AutoIncrement = false;
            //column.Caption = "ParentItem";
            //column.ReadOnly = false;
            //column.Unique = false;
            //// Add the column to the table.
            //result.Columns.Add(column);

            // Make the ID column the primary key column.
            DataColumn[] PrimaryKeyColumns = new DataColumn[1];
            PrimaryKeyColumns[0] = result.Columns["Eintrag"];
            result.PrimaryKey = PrimaryKeyColumns;

            // Instantiate the DataSet variable.
            dataSet = new DataSet();
            // Add the new DataTable to the DataSet.
            dataSet.Tables.Add(result);

            // Create three new DataRow objects and add 
            // them to the DataTable

            int resultCount = 0;

            foreach (XElement resultEntry in resEntries.Nodes())
            {
                List<String> senses = new List<String>();

                foreach (XElement sense in resultEntry.Element("senses").Elements(rss + "sense"))
                {
                    //Console.WriteLine(sense.ToString());
                    List<String> transPerSense = new List<String>();
                    string senseData = "";

                    #region Einzelne_<trans>'s_auflisten
                    foreach (XElement trans in sense.Elements(rss + "trans"))
                    {
                        //Console.WriteLine(trans.ToString());
                        List<String> trPerTrans = new List<String>();
                        string transData = "";

                        #region Einzelne_<tr>'s_auflisten
                        foreach (XElement tr in trans.Elements(rss + "tr"))
                        {
                            //Console.WriteLine(tr.ToString());
                            bool first = true;
                            bool followingSpaceOld = false;
                            bool followingSpaceNew = false;
                            bool preceedingSpace = false;

                            List<String> elemPerTr = new List<String>();
                            string trData = "";

                            #region Elemente_fuer_tr_konkatenieren

                            foreach (XElement elem in tr.Elements())
                            {
                                //Console.WriteLine(elem.ToString());
                                string elemData = "";

                                if (elem.Name == rss + "text")
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
                                    //a
                                }
                                else if (elem.Name == rss + "token")
                                {
                                    string tokenInfo = "";

                                    if (elem.Attribute("type") != null)
                                    {
                                        tokenInfo = tokenInfo + " [";

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
                                            if (genus != "") tokenInfo = tokenInfo + genus + ".";
                                            if (numerus != "") tokenInfo = tokenInfo + ", " + numerus;
                                            tokenInfo = tokenInfo + "]";
                                        }
                                    }
                                    elemData = elem.Value + tokenInfo;
                                    //a
                                }
                                Console.WriteLine(elemData);
                                if (!(elemData == ""))
                                {
                                    if (first)
                                    {
                                        trData = trData + elemData + ((followingSpaceNew) ? " " : "");
                                        first = false;
                                    }
                                    else
                                    {
                                        if (!followingSpaceOld)         // Falls das vorherige Element kein Leerzeichen am Ende aufweist, ...
                                        {
                                            if (!preceedingSpace)           // ... und falls das gerade betrachtete Element auch kein führendes Leerzeichen erhalten soll, ...
                                            {
                                                trData = trData + ((elemData.StartsWith(", ")) ? "" : ", ");        // ... so füge, falls nicht bereits vorhanden, ", " als Trennelement ein.
                                            }
                                            else                            // ... und falls das gerade betrachtete Element ein führendes Leerzeichen erhalten soll, ...
                                            {
                                                trData = trData + " ";                                              // ... so füge das führende Leerzeichen ein.
                                            }
                                        }
                                        trData = trData + elemData + ((followingSpaceNew) ? " " : "");
                                    }

                                    followingSpaceOld = followingSpaceNew;
                                    followingSpaceNew = false;
                                    preceedingSpace = false;
                                }
                            }
                            #endregion

                            trPerTrans.Add(trData);
                        }
                        #endregion

                        transData = String.Join(", ", trPerTrans.ToArray());
                        transPerSense.Add(transData);

                    } 
                    #endregion

                    senseData = String.Join("; ", transPerSense.ToArray());
                    senses.Add(senseData);
                    //translations.Add(senseData + String.Join(", ", translationsPerSense.ToArray()));
                }
               
                int senseCount = 1;

                result.Rows.Add(++resultCount, resultEntry.Element(rss + "text").Value, "(1) " + String.Join((". (" + ++senseCount + ") "), senses) + ".");
            }
            res = result;
            return res;
        }
    }
}
