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

// eine extra Klasse für die Arraylist mit unseren Daten also den Saetzen und Token.
// habe ich extra angelegt weil es dann einfacher ist etwas abzuaendern wenn wir spaeter vielleicht auch noch Zusatzinformationen wie Uebersetzungen speichern wollen.

namespace JADE
{
    [Serializable()]
    public class Daten : ISerializable                              //Mit Zusatz (ISerializable) ist die Klasse später Serialisierbar.
    {
        private ArrayList data = new ArrayList();                   //Erzeugt die Variable data(Arraylist) in der unsere Daten gespeichert werden.


        public Daten() { }                                          //leerer Konstruktor

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
        
        //Spezial Konstruktor fuer die Deserialisation 
        public Daten(SerializationInfo info, StreamingContext ctxt)
        {
            this.data = (ArrayList)info.GetValue("Daten", typeof(ArrayList));
        }

        //Funktion zur Rueckgabe eines einzelnen Token(String) aus unseren Daten.
        //Erhällt zu diesem Zweck zwei Int-Werte für Satznummer und Tokennummer, des gewuenschen Token.
        public String getToken(int Satznummer, int Tok)
        {
            ArrayList hilf = this.getSatz(Satznummer);              //Erzeugt eine Variable vom Typ Arraylist und fuellt diese mit den Token aus dem Zielsatz.
            return (String)hilf[Tok];                               //Konvertiert den Arraylisteintrag an der gewuenschten Position zu String und gibt diesen zurueck.
        }

        //Funktion zur Rückgabe eines Satzes(ArrayList) aus unseren Daten.
        //Erhällt zu diesem Zweck einen Int-Werte für Satznummer des gewuenschen Satzes.
        public ArrayList getSatz(int Satznummer)
        {
            ArrayList hilf = new ArrayList();                       //Erzeugt Variable vom Typ Arraylist. 
            hilf = this.Zugriff;                                    //Weist der hilfsarralist die Daten zu.  
            ArrayList rueckgabe = (ArrayList)hilf[Satznummer];      //Erzeugt eine Variable vom Typ Arraylist und weist dieser den gewuenschten Satz zu.
            return rueckgabe;                                       //gibt den Satz zurueck.
        }

        //Benoetigt die Serialize Funktion (liefert spaeter eine info an die Serialize Funktion ueber die zu serialisierenden Daten)
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("Daten", this.data);
        }
  }
}
