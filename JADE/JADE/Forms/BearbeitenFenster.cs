using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

//hello ther

namespace JADE
{
    public partial class BearbeitenFenster : Form
    {
        private int Satznummer;
        private int Tok;
        private Daten obj;

        public BearbeitenFenster(Daten obj,int Satznummer,int Tok,String str)
        {
            this.Satznummer = Satznummer;
            this.Tok = Tok;
            this.obj = obj;
            InitializeComponent();
            this.textBox3.Text = str;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1.Instanzdaten.trennen2(Satznummer, Tok, this.textBox1.Text, this.textBox2.Text);
            this.Close();
        }
      

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
