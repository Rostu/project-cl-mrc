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
    ///Form-Objekt(Windows Fenster), in dem das Trennen eines Token in zwei neue Token realisiert wird.
    /// </summary>
    public partial class BearbeitenFenster : Form
    {
        /**int-Variable, erhält bei Aufruf des Fensters die Satznummer des zu bearbeitenden Token. */ 
        private int Satznummer;
        /**int-Variable, erhält bei Aufruf des Fensters die Tokennummer des zu bearbeitenden Token. */ 
        private int Tok;
        /**Daten Objekt, erhält bei Aufruf des Fensters die Satz- und Token-Daten aus dem HauptFenster. */ 
        private Daten obj;

        /**
         * Konstruktor für das Bearbeiten-Fenster.
         * @param obj Ein Objekt der Klasse Daten.
         * @param[in] Satznummer int-Wert des Satzes, in dem sich das gewünschte Token befindet.
         * @param[in] str string-Wert, der das zu trennenden Token repräsentiert. 
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
         * Funktion für den TrennenButton. Überprüft, ob sich die beiden neuen Token aus den Chars des Ursprungs-Token zusammensetzen und trennt diese dann. 
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

        /**
         * Funktion für den AbbrechenButton. Ein Klick auf den Button schließt das BearbeitenFenster.
         */
        private void AbbrechenButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
