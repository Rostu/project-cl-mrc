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

        }

        private void Tokenize_Click(object sender, EventArgs e)
        {
            Segmenter segtest = new Segmenter();                                // erstellt ein neues Segmenter Obejekt
            Instanzdaten = segtest.TinySegmenter(this.richTextBox1.Text);       // Weißt dem Objekt die Daten aus der richTextBox zu
            // this.richTextBox1.Text = Instanzdaten.getToken(3, 0);
            /* ArrayList Alist = new ArrayList();
            Alist = Instanzdaten.Zugriff; foreach (ArrayList a in Alist)
            {
                this.richTextBox1.Text += "Satz: ";
                foreach (String s in a)
                {
                    this.richTextBox1.Text += s + "  ";
                }
            }
             */
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
            ArrayList Alist = new ArrayList();
            for (int i = 0; i < Alist; i++)
            {
                TreeNode n = new TreeNode();
                treeView1.Nodes.Add(n);
            }

        }

    }
}




