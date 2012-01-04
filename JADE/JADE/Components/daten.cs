using System;
using System.Collections.Generic;
using System.Collections;                               // für die Arraylist
using System.Linq;
using System.Text;
using System.Runtime.Serialization;                     
using System.Runtime.Serialization.Formatters.Binary;

// eine extra Klasse für die Arraylist mit unséren Daten also den Sätzen und Token.
// habe ich extra angelegt weil es dann einfacher ist etwas ab zu aendern wenn wir später vielleicht auch noch Zusatzinformationen wie Uebersetzungen  speichern wollen
//Außerdem enthält die Klasse Funktionen zum trennen und zusammenfuegen von Token, sowie Funktionen zur Rückgabe einzelner Token und Saetze 
namespace JADE
{
    [Serializable()]
    public class Daten : ISerializable
    {
        private ArrayList data = new ArrayList();

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

        //Zusammenfuegen zweier Token 
        public void zusammen(int Satznummer, int Tok1, int Tok2)
        {
            ArrayList Alist = this.Zugriff;                             //Zugriff auf Datenstruktur
            ArrayList Satz = (ArrayList)Alist[Satznummer];              //Heraus suchen des Satzes in welchem sich das zu aendernde Token befindet   
                  
            Satz[Tok1] = ((String)Satz[Tok1] + (String)Satz[Tok2]);     //Zusammenfügen der Token tok1 und tok2 an Position von tok1
            Satz.RemoveAt(Tok2);                                        //Loeschen des nun überfluessigen tok2

            Alist[Satznummer] = Satz;                                   //Schreiben des geaenderten Satzes in die Arraylist
            this.Zugriff = Alist;                                       //Schreiben der geaenderten ARRAYLIST zurueck in die Datenstruktur
        }

        //Trennen eines Token 
        public void trennen(int Satznummer, int Tok)
        {
            ArrayList Alist = this.Zugriff;                                 //Zugriff auf Datenstruktur
            ArrayList Satz = (ArrayList)Alist[Satznummer];                  //Heraus suchen des Satzes in welchem sich das zu aendernde Token befindet  
            String str = (String)Satz[Tok];
            BearbeitenFenster neumit = new BearbeitenFenster(this,Satznummer,Tok,str);         //Erzeugt bearbeiten Fenster und ruft dieses auf
            neumit.Show();
        }
        //Trennen eines Token 
        public void trennen2(int Satznummer, int Tok, String neu1, String neu2)
        {
            ArrayList Alist = this.Zugriff;                                 //Zugriff auf Datenstruktur
            ArrayList Satz = (ArrayList)Alist[Satznummer];                  //Heraus suchen des Satzes in welchem sich das zu aendernde Token befindet  
            Satz.Insert(Tok, neu2);
            Satz.Insert(Tok, neu1);
            Satz.RemoveAt(Tok+2);
            Alist[Satznummer] = Satz;                                   //Schreiben des geaenderten Satzes in die Arraylist
            this.Zugriff = Alist;                                       //Schreiben der geaenderten ARRAYLIST zurueck in die Datenstruktur
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
