﻿using System;
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
            ArrayList Satz = Instanzdaten.getSatz(e.Node.Index);
            String test = "";
            this.flowLayoutPanel1.Controls.Clear();
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
                    this.flowLayoutPanel1.Controls.Add(Box);
                }
            }
            this.textBox1.Text = test;
        }

        private void Tokenize_Click(object sender, EventArgs e)
        {
            this.treeView1.Nodes.Clear();
            Segmenter segtest = new Segmenter();                                // erstellt ein neues Segmenter Obejekt
            Instanzdaten = segtest.TinySegmenter(this.richTextBox1.Text);       // Weißt dem Objekt die Daten aus der richTextBox zu
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

        }
    }
}




