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
            XElement res =  new XElement("results", 
                            (from e in wadoku.Root.Elements().OfType<XElement>()

                            where (string) e.Element(rss + "form").Element(rss + "orth").Element(rss + "text") == token

                            select new XElement("result",   e.Element(rss + "form").Element(rss + "pron").Element(rss + "text"),
                                                            e.Element(rss + "sense").Element(rss + "trans").Element(rss + "tr"))));
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
            DataTable result = new DataTable("Suchergebnisse - " + token + " (Satz " + satzNr + ", Token " + tokenNr + ")");
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

            int count = 0;

            foreach (XElement r in resEntries.Nodes())
            {   
                //ArrayList tokens = new ArrayList((ICollection) );
                //MessageBox.Show("" + tokens.Count);
                result.Rows.Add(++count, r.Element(rss + "text").Value, String.Join(", ", ((from t in r.Element(rss + "tr").Elements() select ((string)t)).ToArray())));
            }
            res = result;
            return res;
        }
    }
}
