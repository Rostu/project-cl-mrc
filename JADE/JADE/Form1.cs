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

        }
    }
}
