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
    /// <summary>
    ///Form-Objekt(Windows Fenster) indem das Trennen eines Token in zwei neue Token realisiert wird.
    /// </summary>
    public partial class BearbeitenFenster : Form
    {
        private int Satznummer;
        private int Tok;
        private Daten obj;

        /**
         * Konstruktor für das Bearbeiten-Fenster.
         * @param obj Ein Objekt der Datenstruktur Daten.
         * @param[in] Satznummer Int-Wert des Satzes in dem sich der gewünschte Token befindet.
         * @param[in] str String der den zu trennenden Token repräsentiert. 
         */
        public BearbeitenFenster(Daten obj, int Satznummer, int Tok, String str)    //
        {
            this.Satznummer = Satznummer;
            this.Tok = Tok;
            this.obj = obj;
            InitializeComponent();
            this.textBox3.Text = str;
            this.Icon = new System.Drawing.Icon("jade.ico");
        }
        
        /**
         * Trennen-Button Funktion. Überprüft ob sich die beiden Neuen Token aus den Chars des Ursprungs-Token zusammensetzen und trennt diese dann. 
         */
        private void TrennenButton_Click(object sender, EventArgs e)
        {
            ArrayList Alist = obj.Zugriff;                                     //Zugriff auf Datenstruktur
            ArrayList Satz = (ArrayList)Alist[Satznummer];                     //Heraussuchen des Satzes in welchem sich das zu aendernde Token befindet  
            String test = this.textBox1.Text + this.textBox2.Text;             //Hilfs-String bestehend aus der Summe der Inhalt von Textbox1+2 (String) 

            if ((Equals(test, (String)Satz[Tok])) && !((Equals(this.textBox1.Text, "")) || (Equals(this.textBox2.Text, ""))))                               //Um unsinniges trennen zu vermeiden wird geprüft ob sich die getrennten neuen Token aus den Zeichen des urspruenglichen Token zusammen setzen 
            {
                Satz.Insert(Tok, this.textBox2.Text);                          //Fügt den Inhalt der 2ten Textbox in die Arraylist ein(an der Stelle Tok).
                Satz.Insert(Tok, this.textBox1.Text);                          //Fügt den Inhalt der 1ten Textbox in die Arraylist ein(an der Stelle Tok), Tok2 rückt dadurch nach hinten.
                Satz.RemoveAt(Tok + 2);                                        //Löscht den zu trennenden ursprünglichen Token.
                Alist[Satznummer] = Satz;                                      //Schreiben des geänderten Satzes in die Arraylist.
                obj.Zugriff = Alist;                                           //Schreiben der geänderten ARRAYLIST zurück in die Datenstruktur.
                SearchEngine.DisposeTable(Satznummer, Tok);
                this.Close();
            }
            else
            {
                MessageBox.Show("Die 2 neuen Token müssen aus den Zeichen \ndes zu bearbeitenden Token bestehen.", "Fehler bei der Eingabe", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        /// @cond
        /**
         * Abbrechen-Button Funktion. Schließt das BearbeitenFenster.
         */
        private void AbbrechenButton_Click(object sender, EventArgs e)
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

        private void BearbeitenFenster_Load(object sender, EventArgs e)
        {

        }
        /// @endcond
    }
}
