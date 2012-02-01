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
        public static Daten Instanzdaten = new Daten();
        private static SearchEngine suche = SearchEngine.Engine;

        public Form1()
        {
            InitializeComponent();
            //Segmenter segtest = new Segmenter();  
            //Instanzdaten.Zugriff = segtest.TinySegmenter(this.textBox1.Text);
            //Instanzdaten.trennen(0,4);
            //Instanzdaten.zusammen(0, 1, 2);
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        public void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            flowupdate();
        }

        //Funktion zum updaten der Token Anzeige
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
                    if ((Equals((String)a, " ")) || (Equals((String)a, "。")) || (Equals((String)a, "！")) || (Equals((String)a, "？")))
                        break;
                    else
                    {
                        CheckBox Box = new System.Windows.Forms.CheckBox();
                        Box.Text = (String)a;
                        Box.Size = new Size(80, 30);
                        Box.Name = "checkBox" + i; i++;
                        this.flowLayoutPanel1.Controls.Add(Box);
                    }
                }
                this.textBox1.Text = test;
            }
        }

        private void Tokenize_Click(object sender, EventArgs e)
        {
            suche.clearDataSet();
            this.treeView1.Nodes.Clear();
            Segmenter segtest = new Segmenter();     // erstellt ein neues Segmenter Obejekt
            String toTok = this.richTextBox1.Text;
            toTok = toTok.Replace("\n", "");
            toTok = toTok.Replace("\t", "");
            toTok = toTok.Replace(" ", "");
            this.richTextBox1.Text = toTok;
            Instanzdaten = segtest.TinySegmenter(toTok);       // Weißt dem Objekt die Daten aus der richTextBox zu
            ArrayList Alist = Instanzdaten.Zugriff;
            int i = 0;
            foreach (ArrayList a in Alist)
            {
                TreeNode Instanz = new TreeNode("Satz" + (i + 1) + ": " + Instanzdaten.getToken(i, 0));
                i++;
                treeView1.Nodes.Add(Instanz);
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
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

        //Trennen eines Token 
        public void trennen(int Satznummer, int Tok)
        {
            String str = Instanzdaten.getToken(Satznummer, Tok);
            BearbeitenFenster neumit = new BearbeitenFenster(Instanzdaten, Satznummer, Tok, str);           //Erzeugt bearbeiten Fenster und ruft dieses auf
            neumit.FormClosing += new FormClosingEventHandler(frm2_FormClosing);                            //Erzeugt für das neue Fenster einen Fenster schließen Eventhandler
            neumit.Show();
        }

        //EventHandler der bei schließen des bearbeiten Fensters den Listener ausschaltet und die FlowlayoutPanel mit den Token(die Anzeige der Tokenliste) updatet
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
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void öffnenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamReader myFile = new StreamReader(openFileDialog1.FileName);
                String myString = myFile.ReadToEnd();
                richTextBox1.Text = myString;
            }
        }

        private void button1_Click(object sender, EventArgs e)
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
                MessageBox.Show("Bitte Maximal 1 Token auswählen", "Fehler bei der Eingabe", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                       MessageBox.Show("Keine Einträge gefunden", "Fehler bei der Eingabe", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                   }
                }
            }

            private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
            {
                this.dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
            }

            private void beendenToolStripMenuItem_Click(object sender, EventArgs e)
            {
                System.Windows.Forms.Application.Exit();
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




