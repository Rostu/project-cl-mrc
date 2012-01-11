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
    public partial class BearbeitenFenster : Form
    {
        private int Satznummer;
        private int Tok;
        private Daten obj;

        public BearbeitenFenster(Daten obj, int Satznummer, int Tok, String str)
        {
            this.Satznummer = Satznummer;
            this.Tok = Tok;
            this.obj = obj;
            InitializeComponent();
            this.textBox3.Text = str;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ArrayList Alist = obj.Zugriff;                                 //Zugriff auf Datenstruktur
            ArrayList Satz = (ArrayList)Alist[Satznummer];                 //Heraus suchen des Satzes in welchem sich das zu aendernde Token befindet  
            Satz.Insert(Tok, this.textBox1.Text);                          //Fügt die getrennten Teile in die Arraylist ein(an der Stelle Tok)
            Satz.Insert(Tok, this.textBox2.Text);
            Satz.RemoveAt(Tok + 2);                                        //Löscht den zu trennenden Token 
            Alist[Satznummer] = Satz;                                      //Schreiben des geaenderten Satzes in die Arraylist
            obj.Zugriff = Alist;                                           //Schreiben der geaenderten ARRAYLIST zurueck in die Datenstruktur
            this.Close();
        }
      

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BearbeitenFenster_Load(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

    }
}
