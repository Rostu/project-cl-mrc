using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace JADE
{
    public partial class Form1 : Form
    {
        public static Daten Instanzdaten = new Daten();


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

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            ArrayList Satz = Instanzdaten.getSatz(e.Node.Index);                                                                            // Durch e.Node.Index wird der selektierte Satz ausgwählt
            String test = "";                                                                                                               // neue Variable
            this.flowLayoutPanel1.Controls.Clear();                                                                                         // leert das flowlayoutoutPanel vor dem Start
            foreach (String a in Satz)                                                                                                      // für jedes Token im Satz
            {
                test += (String)a + "  ";                                                                                                   // Variable test der Wert eines Tokens zugeteilt mit Leerzeichen
                if ((Equals((String)a, " ")) || (Equals((String)a, "。")) || (Equals((String)a, "！")) || (Equals((String)a, "？")))        // Ausnahmebehandulung: Satzenden werden keine Checkboxen zugeteilt, sondern einfach ingnoriert.
                    break;
                else                                                                                                                        // für jedes Token im Satz wird eine neue Checkbox angelegt
                {
                    CheckBox Box = new System.Windows.Forms.CheckBox();         
                    Box.Text = (String)a;
                    Box.Size = new Size(80, 30);
                    this.flowLayoutPanel1.Controls.Add(Box);
                }
                
            }
            this.textBox1.Text = test;                                                                                                      // schreibt den selektierten Satz in die TextBox
        }

        private void Tokenize_Click(object sender, EventArgs e)
        {
            this.treeView1.Nodes.Clear();                                       // leert das treeview Objekt
            Segmenter segtest = new Segmenter();                                // erstellt ein neues Segmenter Obejekt
            Instanzdaten = segtest.TinySegmenter(this.richTextBox1.Text);       // Weißt dem Objekt die Daten aus der richTextBox zu
            ArrayList Alist = Instanzdaten.Zugriff;                             // greift auf die Instanzdaten zu       
            int i = 0;                                                          // Initialiert neue Variable
            foreach (ArrayList a in Alist)                                      // Schleife für jedes Element im Array wird neues treeveiw Element angelegt. Satznummer fängt bei 1 an.
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

        }
    }
}




