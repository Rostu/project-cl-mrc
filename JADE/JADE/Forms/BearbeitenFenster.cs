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

        public BearbeitenFenster(Daten obj, int Satznummer, int Tok, String str)    //
        {
            this.Satznummer = Satznummer;
            this.Tok = Tok;
            this.obj = obj;
            InitializeComponent();
            this.textBox3.Text = str;
            this.Icon = new System.Drawing.Icon("jade.ico");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ArrayList Alist = obj.Zugriff;                                     //Zugriff auf Datenstruktur
            ArrayList Satz = (ArrayList)Alist[Satznummer];                     //Heraus suchen des Satzes in welchem sich das zu aendernde Token befindet  
            String test = this.textBox1.Text + this.textBox2.Text;             //Hilfs-String bestehend aus der Summe der Inhalt von Textbox1+2 (String) 

            if ((Equals(test, (String)Satz[Tok])) && !((Equals(this.textBox1.Text, "")) || (Equals(this.textBox2.Text, ""))))                               //Um unsinniges trennen zu vermeiden wird geprüft ob sich die getrennten neuen Token aus den Zeichen des urspruenglichen Token zusammen setzen 
            {
                Satz.Insert(Tok, this.textBox2.Text);                          //Fuegt den Inhalt der 2ten Textbox in die Arraylist ein(an der Stelle Tok)
                Satz.Insert(Tok, this.textBox1.Text);                          //Fuegt den Inhalt der 1ten Textbox in die Arraylist ein(an der Stelle Tok), Tok2 rückt dadurch nach hinten
                Satz.RemoveAt(Tok + 2);                                        //Loescht den zu trennenden ursprünglichen Token 
                Alist[Satznummer] = Satz;                                      //Schreiben des geaenderten Satzes in die Arraylist
                obj.Zugriff = Alist;                                           //Schreiben der geaenderten ARRAYLIST zurueck in die Datenstruktur
                SearchEngine.DisposeTable(Satznummer, Tok);
                this.Close();
            }
            else
            {
                MessageBox.Show("Die 2 neuen Token müssen aus den Zeichen \ndes zu bearbeitenden Token bestehen.", "Fehler bei der Eingabe", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void BearbeitenFenster_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

    }
}
