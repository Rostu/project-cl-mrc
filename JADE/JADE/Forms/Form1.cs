using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using href.Utils;

namespace JADE
{
    public partial class Form1 : Form
    {
        public static Daten Instanzdaten;
        private static SearchEngine suche;
        private static Segmenter segtest;
        
        public Form1()
        {
            InitializeComponent();

            this.Icon = new System.Drawing.Icon("jade.ico");

            flowLayoutPanel1.HorizontalScroll.Enabled = false;
            flowLayoutPanel1.HorizontalScroll.Visible = false;

            Instanzdaten = new Daten();
            suche = SearchEngine.Engine;
            segtest = new Segmenter();

        }

        //In Dieser Funktion wird unsere Form 1 geladen.
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        //Zeigt die Eintraege aus unsere Worterbuch an.
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            this.dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);   //Automatische Spaltenskalierung.
        }

        //Funktion zum Anzeigen der Saetze in einer Baumstruktur.
        public void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            flowupdate();                                                                           //Ruft Funktion flowupdate auf.
        }

        //Funktion zum Updaten der Token Anzeige
        public void flowupdate()
        {   
            TreeNode selectedTreeNode = treeView1.SelectedNode;                                     //Erstellt TreeNodeobjekt.
            int satzIndex = this.treeView1.SelectedNode.Index;                                      //Gibt verweis auf aktuellen TreeNodeobjekt.
            
            if (selectedTreeNode != null)                                                           //Wenn Treeview Saetze enhaelt, werden diese Eintraege hier geloescht.
            {
                this.flowLayoutPanel1.Controls.Clear();                                             //Loescht alle Checkboxen aus dem Flowlayoutpannel.
                string satzText = "";                                                               //Erstellt leeren String.
                List<Control> checkBoxes = new List<Control>();                                     //Erstellt ein Control fuer Checkboxes.
                ArrayList satz = Instanzdaten.getSatz(satzIndex);                                   //Erstellt eine Arraylist und erhaelt den Satz der markiert wurde.
                int i = 0;                                                                          //Erstellt Hilfsvariable.
                string token = "";                                                                  //Erstellt Hilfsvariable.
                    
                foreach (var tokenObj in satz)                                                      //Jedes Token im Satz wird mit Leerzeichen an string angehangen.
                {
                    token = (String)tokenObj;
                    satzText += token + "  ";
                    
                    if (!("、？！。".Contains(token)))                                              //Falls "?", ",", "!" oder "." enhalten wird keine Checkbox fuer dieser erstellt.
                    {
                        checkBoxes.Add
                            (new System.Windows.Forms.CheckBox                                      //Fuer Jedes Token wird neue Checkbox erstellt.
                            { 
                                Name = "" + i,
                                Text = token,
                                AutoSize = true                                                     //Autosize ist an.
                            });
                    }
                    i++;
                }

                this.textBox1.Text = satzText;                                                      //Uebergibt der Textbox den String mit dem Token.
                this.flowLayoutPanel1.Controls.AddRange(checkBoxes.ToArray());                      //Uebergibt die Checkboxen an ein Array.
            }
        }

        //Funktion die den bei klick auf den Tokenize Button aufgerufen wird.
        private void Tokenize_Click(object sender, EventArgs e)
        {
            String toTok = this.richTextBox1.Text;                                                  //Fuegt Text aus Richtextbox in ein String.
            toTok = toTok.Replace("\n", "");                                                        //Ignoriert Zeilvorschub.
            toTok = toTok.Replace("\t", "");
            toTok = toTok.Replace(" ", "");                                                         //Ignoriert Leerzeichen.
            toTok = toTok.Replace("　", "");                                                        //Ignoriert doppeltes Leerzeichen,
            this.richTextBox1.Text = toTok;                                                         //Fuegt text von Richtextbox erneunt an String an.
            segtest.TextTest(this.richTextBox1.Text);
            
            if (!(Equals(richTextBox1.Text, "")))                                                   //Wenn die Textbox nicht leer ist, laeuft Programm weiter.
            {
                suche.clearDataSet();
                dataGridView1.DataSource = null;
                this.treeView1.Nodes.Clear();
                this.textBox1.Text = "";
                this.flowLayoutPanel1.Controls.Clear();

                Instanzdaten = segtest.TinySegmenter(toTok);                                        //Weißt dem Objekt die Daten aus der richTextBox zu
                ArrayList Alist = Instanzdaten.Zugriff;                                             //Erstellt eine Arraylist.

                int i = 0;
                foreach (ArrayList a in Alist)                                                      //Fuer Jeden Eintrag im Array wird ein neuer Eintrag im Treeview erstellt.
                {
                    TreeNode Instanz = new TreeNode("Satz " +  ("" + (i + 1)).PadLeft(2,'0') + ": " + Instanzdaten.getToken(i, 0));
                    treeView1.Nodes.Add(Instanz);
                    i++;
                }
            }
            else                                                                                    //Falls Textnox leer ist, wird eine Fehlermeldung ausgegeben.
            {
                MessageBox.Show("Kein Text zum Tokenisieren gegeben", "Fehler bei der Eingabe", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }
     
        //Trennen eines Token 
        public void trennen(int Satznummer, int Tok)
        {
            String str = Instanzdaten.getToken(Satznummer, Tok);
            BearbeitenFenster neumit = new BearbeitenFenster(Instanzdaten, Satznummer, Tok, str);           //Erzeugt bearbeiten Fenster und ruft dieses auf
            neumit.FormClosing += new FormClosingEventHandler(frm2_FormClosing);                            //Erzeugt für das neue Fenster einen Fenster schließen Eventhandler
            neumit.Show();
        }

        //EventHandler der bei Schließen des Bearbeiten-Fensters den Listener ausschaltet und das FlowlayoutPanel mit den Token(die Anzeige der Tokenliste) updatet.
        void frm2_FormClosing(object sender, FormClosingEventArgs e)
        {
            BearbeitenFenster neumit = (BearbeitenFenster)sender;
            neumit.FormClosing -= new FormClosingEventHandler(frm2_FormClosing);
            flowupdate();
        }

        //Zusammenfuegen zweier Token 
        public void zusammen(int Satznummer, int Tok1, int Tok2)
        {
            ArrayList Alist = Instanzdaten.Zugriff;                                                         //Zugriff auf Datenstruktur
            ArrayList Satz = Instanzdaten.getSatz(Satznummer);                                              //Heraus suchen des Satzes in welchem sich das zu aendernde Token befindet   
            Satz[Tok1] = ((String)Satz[Tok1] + (String)Satz[Tok2]);                                         //Zusammenfügen der Token tok1 und tok2 an Position von tok1
            Satz.RemoveAt(Tok2);                                                                            //Loeschen des nun ueberfluessigen tok2
            Alist[Satznummer] = Satz;                                                                       //Schreiben des geaenderten Satzes in die Arraylist
            Instanzdaten.Zugriff = Alist;                                                                   //Schreiben der geaenderten ARRAYLIST zurueck in die Datenstruktur
            SearchEngine.DisposeTable(Satznummer, Tok1);
            SearchEngine.DisposeTable(Satznummer, Tok2);
        }

        private void öffnenToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.RestoreDirectory = true;

            StreamReader openFileStream;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((openFileStream = EncodingTools.OpenTextFile(openFileDialog1.FileName)) != null)
                    {
                        using (openFileStream)
                        {
                            // Make sure you read from the file or it won't be able
                            // to guess the encoding
                            var file = openFileStream.ReadToEnd();

                            // Create two different encodings.
                            Encoding utf8 = Encoding.UTF8;
                            Encoding detectedEncoding = EncodingTools.GetMostEfficientEncoding(file);

                            // Convert the string into a byte array.
                            byte[] detectedEncodingBytes = detectedEncoding.GetBytes(file);

                            // Perform the conversion from one encoding to the other.
                            byte[] utf8Bytes = Encoding.Convert(detectedEncoding, utf8, detectedEncodingBytes);

                            // Convert the new byte[] into a char[] and then into a string.
                            char[] utf8Chars = new char[utf8.GetCharCount(utf8Bytes, 0, utf8Bytes.Length)];
                            utf8.GetChars(utf8Bytes, 0, utf8Bytes.Length, utf8Chars, 0);

                            richTextBox1.Text = new string(utf8Chars);
                        }
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Es ist ein Fehler beim Öffnen der angegebenen Datei aufgetreten.\n\nFehlermeldung:\n" + exception.Message, "Fehler bei der Eingabe", MessageBoxButtons.OK, MessageBoxIcon.Warning); 
                }
            }

        }

        //Funktion die ausgefuehrt werden muss um zwei Token zu trennen oder zusammen zu fuehren.
        private void button2_Click_1(object sender, EventArgs e)
        {
            int count = 0;                                                          //Variable "count" wird eingefuehrt.
            int first = 0;                                                          //Variable "first" wird eingefuehrt.
            int second = 0;                                                         //Variable "second" wird eingefuehrt.
            foreach (Control con in this.flowLayoutPanel1.Controls)                 //Zaehlt wie viel Checkboxen ausgewahelt wurden.
            {
                CheckBox box = (CheckBox)con;
                int index = Int32.Parse(box.Name); 
                if (box.Checked == true)
                {
                    if (count == 0)
                    {
                        first = index;
                    }
                    if (count > 0)
                    {
                        second = index;
                    }
                    count++;
                }
            }
            if (count > 2)                                                          //Fehlerausgabe wenn mehr als 2 Checkboxen ausgewaehlt wurden.
            {
                MessageBox.Show("Bitte Maximal 2 (nebeneinander liegende) Token auswählen.", "Fehler bei der Eingabe", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                switch (count)
                {
                    case 1:                                                         //Trennt die ausgewaehlten Token.
                        trennen(this.treeView1.SelectedNode.Index, first);
                        break;
                    case 2:
                        if (first + 1 != second)                                    //Fehlerausgabe wenn Checkboxen nicht nebeneinader liegen.
                        {
                            MessageBox.Show("Bitte Maximal 2 (nebeneinander liegende) Token auswählen.", "Fehler bei der Eingabe", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            break;
                        }
                        else                                                        //Fuegt Token zusammen.
                        {
                            zusammen(this.treeView1.SelectedNode.Index, first, second);
                            flowupdate();                                           //Ruft flowupdate-Funktion auf.
                            break;                                                  //beendet diese Funktion.
                        }
                }
            }
        }

        //Funktion zum Suchen von Token im Woerterbuch.
        private void button1_Click_1(object sender, EventArgs e)
        {
            int count = 0;                                                                  //Erzeugt die Variable "Count" mit dem Wert Null.
            int first = 0;                                                                  //Erzeugt die Variable "first" mit dem Wert Null.
            foreach (Control con in this.flowLayoutPanel1.Controls)                         
            {
                CheckBox box = (CheckBox)con;
                int index = Int32.Parse(box.Name);
                if (box.Checked == true)
                {
                    if (count == 0)
                    {
                        first = index;
                    }
                    count++;
                }
            }
            if (count > 1)                                                                  //Eventhaendler, wenn mehr als eine Checkbox ausgewaehlt ist, wird eine Fehlermeldung ausgegeben.
            {
                MessageBox.Show("Bitte wählen Sie maximal ein Token aus.", "Fehler bei der Eingabe", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            if (count == 1)                                                                 //Eventhaendler, wenn genau eine Checkbox ausgewaehlt ist.
            {
                DataTable result = suche.search(Instanzdaten.getToken(this.treeView1.SelectedNode.Index, first), this.treeView1.SelectedNode.Index, first, this.checkBox1.Checked);     //Sucht nun ueber den Index, ob es im Woerterbuch passenden Eintraege gibt.
                if (result.Rows.Count > 0)                                                  //Wenn etwas gefunden wurde, wird es in Datagridview geschrieben.
                {
                    this.dataGridView1.DataSource = result;
                }
                else                                                                        //Wenn kein Eintrag gefunden wurde, wird eine Meldung ausgegeben.
                {
                    MessageBox.Show("Es wurden keine Einträge gefunden.", "Fehler bei der Eingabe", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        //Funktion die das Programm beendet, wenn es der User wuenscht.
        private void beendenToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        //Funktion erhaelt Satznummer und Tokennummer und loescht aus der Woerterbuchergebnisliste den gefunden Eintrag. Dies ist erforderlich da bei einer Aenderung des Tokens aus der Ergebnisliste, nicht der alte Eintrag wieder aufgerufen werden soll, sondern der neue.
        public void TableDel(int satznummer,int tok)
        {
            SearchEngine.DisposeTable(satznummer, tok);
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}




