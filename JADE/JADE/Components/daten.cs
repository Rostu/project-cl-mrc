
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


namespace JADE
{
    /// <summary>
    ///Eine Klasse, die für das Speichern der Arraylist mit unseren Daten für den eingegebenen Text (die einzelnen Sätze und Token) gedacht ist.
    ///Sie wurde auch für den Zweck angelegt, spätere Änderungen wie das Speichern von Zusatzinformationen wie Übersetzungsmöglichkeiten zu erleichtern.
    /// </summary>
    [Serializable()]
    public class Daten : ISerializable                              //Mit Zusatz (ISerializable) ist die Klasse später Serialisierbar.
    {
        /**Private Variable vom Typ ArrayList. Hier werden die Daten abgespeichert.*/
        private ArrayList data = new ArrayList();                   //Erzeugt die Variable data(Arraylist) in der unsere Daten gespeichert werden.

        /**leerer Konstruktor*/
        public Daten() { }

        /**Zugriffs-Methode mit get und set um einfach auf die Datenstruktur zugreifen zu können.*/
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

        /**
         * Konstruktor für die Serialisation/Deserialisation.
        * @param[in] info Objekt vom Typ SerializationInfo
        * @param[in] ctxt Objekt vom Typ StreamingContext
         */
        public Daten(SerializationInfo info, StreamingContext ctxt)
        {
            this.data = (ArrayList)info.GetValue("Daten", typeof(ArrayList));
        }




        /**
         * Funktion zur Rückgabe der Länge (Anzahl der Elemente) eines Satzes.
         * @param[in] Satznummer Int-Wert des Satzes in dem sich der gewünschte Token befindet.
         * @param[out] Int-Wert der die Anzahl der Elemente eines Satzes repräsentiert.
          */
        public int getSatzLaenge(int Satznummer)
        {
            ArrayList hilf = this.getSatz(Satznummer);              //Erzeugt eine Variable vom Typ Arraylist und füllt diese mit den Token aus dem Zielsatz.
            return hilf.Count;                                      //Rückgabe eines Int-Wertes der die Anzahl der Elemente eines Satzes repräsentiert.
        }

        /**
         * Funktion zur Rückgabe eines einzelnen Token (String) aus unseren Daten.
         * Erhält zu diesem Zweck zwei int-Werte für Satznummer und Tokennummer, des gewünschen Token.
         * @param[in] Satznummer int-Wert des Satzes, in dem sich der gewünschte Token befindet.
         * @param[in] Tok int-Wert des gewünschten Tokens. 
         * @param[out] string-Repräsentation eines Tokens aus der Datenstruktur.
         */
        public String getToken(int Satznummer, int Tok)
        {
            ArrayList hilf = this.getSatz(Satznummer);              //Erzeugt eine Variable vom Typ Arraylist und füllt diese mit den Token aus dem Zielsatz.
            return (String)hilf[Tok];                               //Konvertiert den Arraylisteintrag an der gewünschten Position zu String und gibt diesen zurück.
        }

        /**
         * Funktion zur Rückgabe eines Satzes (als ArrayList) aus unseren Daten.
         * Erhält zu diesem Zweck einen int-Wert für Satznummer des gewünschten Satzes.
         * @param[in] Satznummer int-Wert des Satzes, in dem sich der gewünschte Token befindet.
         * @param[out] Arraylist(mit string-Elementen), die den gewünschten Satz repräsentiert.
         */
        public ArrayList getSatz(int Satznummer)
        {
            ArrayList hilf = new ArrayList();                       //Erzeugt Variable vom Typ Arraylist. 
            hilf = this.Zugriff;                                    //Weist der hilfsarralist die Daten zu.  
            ArrayList rueckgabe = (ArrayList)hilf[Satznummer];      //Erzeugt eine Variable vom Typ Arraylist und weist dieser den gewünschten Satz zu.
            return rueckgabe;                                       //Gibt den Satz zurück.
        }
       

        /**
        * Benötigt die Serialize-Funktion.
        * Fügt der SerializeInfo Informationen über das zu serialisierende Objekt hinzu.
        * @param[in] info Objekt vom Typ SerializationInfo
        * @param[in] ctxt Objekt vom Typ StreamingContext
        */
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("Daten", this.data);
        }
        
  }
}
