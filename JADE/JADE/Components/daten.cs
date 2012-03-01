﻿
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
    ///Eine extra Klasse für die Arraylist mit unseren Daten also den Sätzen und Token.
    ///Wurde extra angelegt, weil es dann einfacher ist, etwas zu ändern wenn wir später vielleicht auch noch Zusatzinformationen wie Übersetzungen speichern wollen.
    /// </summary>
    [Serializable()]
    public class Daten : ISerializable                              //Mit Zusatz (ISerializable) ist die Klasse später Serialisierbar.
    {
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
         * Konstruktor für die Serialisation/DeSerilisation.
         * @param[in] info  SerilizationInfo
         * @param[in] ctxt  StreamingContext 
         */
        public Daten(SerializationInfo info, StreamingContext ctxt)
        {
            this.data = (ArrayList)info.GetValue("Daten", typeof(ArrayList));
        }

        
        /**
         * Funktion zur Rückgabe eines einzelnen Token (String) aus unseren Daten.
         * Erhält zu diesem Zweck zwei Int-Werte für Satznummer und Tokennummer, des gewünschen Token.
         * @param[in] Satznummer Int-Wert des Satzes in dem sich der gewünschte Token befindet.
         * @param[in] Tok Int-Wert des gewünschten Token. 
         * @param[out] String Repräsentation eines Token aus der Daten Struktur.
         */
        public String getToken(int Satznummer, int Tok)
        {
            ArrayList hilf = this.getSatz(Satznummer);              //Erzeugt eine Variable vom Typ Arraylist und füllt diese mit den Token aus dem Zielsatz.
            return (String)hilf[Tok];                               //Konvertiert den Arraylisteintrag an der gewünschten Position zu String und gibt diesen zurück.
        }

        /**
         * Funktion zur Rückgabe eines Satzes(ArrayList) aus unseren Daten.
         * Erhält zu diesem Zweck einen Int-Werte für Satznummer des gewünschten Satzes.
         * @param[in] Satznummer Int-Wert des Satzes in dem sich der gewünschte Token befindet.
         * @param[out] Arraylist(mit String Elementen) die den gewünschten Satz repräsentiert.
         */
        public ArrayList getSatz(int Satznummer)
        {
            ArrayList hilf = new ArrayList();                       //Erzeugt Variable vom Typ Arraylist. 
            hilf = this.Zugriff;                                    //Weist der hilfsarralist die Daten zu.  
            ArrayList rueckgabe = (ArrayList)hilf[Satznummer];      //Erzeugt eine Variable vom Typ Arraylist und weist dieser den gewünschten Satz zu.
            return rueckgabe;                                       //Gibt den Satz zurück.
        }
       

        /**
        * Benötigt die Serialize Funktion.
        * Fügt der SerializeInfo informationen über das zu serialisierende Objekt hinzu.
        * @param[in] info Objekt vom Typ SerializationInfo
        * @param[in] ctxt Objekt vom Typ StreamingContext
        */
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("Daten", this.data);
        }
        
  }
}
