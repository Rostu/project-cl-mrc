using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Serialization;                     
using System.Runtime.Serialization.Formatters.Binary;

// eine extra Klasse für die Arraylist mit unséren Daten also den Sätzen und Token.
// habe ich extra angelegt weil es dann einfacher ist etwas ab zu aendern wenn wir später vielleicht auch noch Zusatzinformationen wie Uebersetzungen speichern wollen

namespace JADE
{
    [Serializable()]
    public class Daten : ISerializable
    {
        private ArrayList data = new ArrayList();
        public Form1 main;

        //leerer Konstruktor
        public Daten() { }

        // set und get Methode zum lesen und Schreiben 
        public ArrayList Zugriff
        {
            get
            {
                return data;
            }
            set
            {
                data = value;
            }
        }

        //Spezial Konstruktor fuer deserialisation 
        public Daten(SerializationInfo info, StreamingContext ctxt)
        {
            this.data = (ArrayList)info.GetValue("Daten", typeof(ArrayList));
        }

        //Funktion zur Rückgabe eines einzelnen Token(String)  
        public String getToken(int Satznummer, int Tok)
        {
            String rueckgabe;
            ArrayList hilf = new ArrayList();
            hilf = Form1.Instanzdaten.Zugriff;      
            ArrayList hilf2 = (ArrayList)hilf[Satznummer];
            rueckgabe = (String)hilf2[Tok];
            return rueckgabe;
        }

        //Funktion zur Rückgabe eines Satzes(ArrayList)
        public ArrayList getSatz(int Satznummer)
        {
            ArrayList hilf = new ArrayList();
            hilf = Form1.Instanzdaten.Zugriff;
            ArrayList rueckgabe = (ArrayList)hilf[Satznummer];
            return rueckgabe;
        }

        //Benötigt die Serializer Funktion
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("Daten", this.data);
        }
  }
}
