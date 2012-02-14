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

        private void Form1_Load(object sender, EventArgs e)
        {
            // Segmenter segtest = new Segmenter();
            // Instanzdaten = segtest.TinySegmenter(this.richTextBox1.Text);
            // TreeNode Instanz = new TreeNode();
            // treeView1.Nodes.Add(Instanz);
            /* ArrayList Alist = new ArrayList();
             for (int i = 0; i < Alist; i++)
             {
                 TreeNode n = new TreeNode();
                 treeView1.Nodes.Add(n);
             }
             */
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            this.dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
        }

        public void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            flowupdate();
        }

        //Funktion zum Updaten der Token Anzeige
        public void flowupdate()
        {
            TreeNode tn = treeView1.SelectedNode;
            if (tn != null)
            {

                ArrayList Satz = Instanzdaten.getSatz(this.treeView1.SelectedNode.Index);
                String test = "";
                this.flowLayoutPanel1.Controls.Clear();
                int i = 0;
                foreach (String a in Satz)
                {
                    test += (String)a + "  ";
                    if (!((Equals((String)a, "、")) || (Equals((String)a, " ")) || (Equals((String)a, "。")) || (Equals((String)a, "！")) || (Equals((String)a, "？"))))
                    {
                        CheckBox Box = new System.Windows.Forms.CheckBox();
                        Console.WriteLine((String)a);
                        Box.Text = ((String)a)/*.PadRight(4, '　')*/;
                        Box.AutoSize = true;
                        Box.Name = "checkBox" + i++;
                        this.flowLayoutPanel1.Controls.Add(Box);
                    }
                }
                this.textBox1.Text = test;
            }
        }

        private void Tokenize_Click(object sender, EventArgs e)
        {
            //Segmenter segtest = new Segmenter();
            String toTok = this.richTextBox1.Text;
            toTok = toTok.Replace("\n", "");
            toTok = toTok.Replace("\t", "");
            toTok = toTok.Replace(" ", "");
            toTok = toTok.Replace("　", "");
            this.richTextBox1.Text = toTok;
            segtest.TextTest(this.richTextBox1.Text);
            
            if (!(Equals(richTextBox1.Text, "")))
            {
                suche.clearDataSet();
                this.treeView1.Nodes.Clear();
                Instanzdaten = segtest.TinySegmenter(toTok);       // Weißt dem Objekt die Daten aus der richTextBox zu
                ArrayList Alist = Instanzdaten.Zugriff;
                int i = 0;
                foreach (ArrayList a in Alist)
                {
                    TreeNode Instanz = new TreeNode("Satz " +  ("" + (i + 1)).PadLeft(2,'0') + ": " + Instanzdaten.getToken(i, 0));
                    i++;
                    treeView1.Nodes.Add(Instanz);
                }
            }
            else
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
                    if ((openFileStream = new StreamReader(openFileDialog1.FileName)) != null)
                    {
                        using (openFileStream)
                        {
                            // Make sure you read from the file or it won't be able
                            // to guess the encoding
                            var file = openFileStream.ReadToEnd();

                            // Create two different encodings.
                            Encoding utf8 = Encoding.UTF8;
                            Encoding detectedEncoding = openFileStream.CurrentEncoding;

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
            //if (openFileDialog1.ShowDialog() == DialogResult.OK)
            //{
            //    if (File.Exists(openFileDialog1.FileName))
            //    {
            //        using (StreamReader myFile = new StreamReader(openFileDialog1.FileName))
            //        {

            //        }
            //    }
            //    using (StreamReader myFile = new StreamReader(openFileDialog1.FileName))
            //    {

            //    }
            //    StreamReader myFile = new StreamReader(openFileDialog1.FileName);
            //    String myString = myFile.ReadToEnd();
            //    richTextBox1.Text = myString;
            //}
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            int count = 0;
            int first = 0;
            int second = 0;
            foreach (Control con in this.flowLayoutPanel1.Controls)
            {
                CheckBox box = (CheckBox)con;
                int index = this.flowLayoutPanel1.Controls.IndexOf(con);
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
            if (count > 2)
            {
                MessageBox.Show("Bitte Maximal 2 (nebeneinander liegende) Token auswählen", "Fehler bei der Eingabe", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                switch (count)
                {
                    case 1:
                        trennen(this.treeView1.SelectedNode.Index, first);
                        break;
                    case 2:
                        if (first + 1 != second)
                        {
                            MessageBox.Show("Bitte Maximal 2 (nebeneinander liegende) Token auswählen", "Fehler bei der Eingabe", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            break;
                        }
                        else
                        {
                            zusammen(this.treeView1.SelectedNode.Index, first, second);
                            break;
                        }
                }
            }
            flowupdate();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            int count = 0;
            int first = 0;
            foreach (Control con in this.flowLayoutPanel1.Controls)
            {
                CheckBox box = (CheckBox)con;
                int index = this.flowLayoutPanel1.Controls.IndexOf(con);
                if (box.Checked == true)
                {
                    if (count == 0)
                    {
                        first = index;
                    }
                    count++;
                }
            }
            if (count > 1)
            {
                MessageBox.Show("Bitte wählen Sie maximal ein Token aus.", "Fehler bei der Eingabe", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            if (count == 1)
            {
                DataTable result = suche.search(Instanzdaten.getToken(this.treeView1.SelectedNode.Index, first), this.treeView1.SelectedNode.Index, first, this.checkBox1.Checked);
                if (result.Rows.Count > 0)
                {
                    this.dataGridView1.DataSource = result;
                }
                else
                {
                    MessageBox.Show("Es wurden keine Einträge gefunden.", "Fehler bei der Eingabe", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void beendenToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        public void TableDel(int satznummer,int tok)
        {
            SearchEngine.DisposeTable(satznummer, tok);
        }

      /*  private void speichernToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamWriter File = new StreamWriter(saveFileDialog1.FileName);
                File.WriteLine(richTextBox1.Text);
                File.Close();
            }
        } */
    }
}




