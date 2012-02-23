using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

// Klasse welche zum speichern unserer Datenstruktur verwendet wird. Alles sehr allgemein gehalten, zum speichern wird später ein string für den Pfad und ein saveobjekt gebraucht.
//Fuer das Saveobjekt git es eine eigene Klasse welche spaeter unsere Datenstruktur erhaelt und so nur als "Container" fungiert.

namespace JADE
{
    class Saver
    {
        public Saver() {}                                               //Leerer Konstruktor.

        //Funktion zum Speichern. 
        //Erhaelt einen String für den Filenamen und ein Objekt der SaveObjekt Klasse.
        public void Save(string filename, SaveObjekt saveObjekt)
        {
            Stream stream = File.Open(filename, FileMode.Create);       //Erstellt ein Filestream-Objekt (write). 
            BinaryFormatter bFormatter = new BinaryFormatter();         //Erstellt ein BinaryFormatter-Objekt.
            bFormatter.Serialize(stream, saveObjekt);                   //Erstellt die Save-Datei und schreibt das SaveObjekt binär hinein.
            stream.Close();                                             //schließt den Stream.
        }

        //Funktion zum Laden aus einer Savedatei. 
        //Erhaelt String der zu ladenden Datei (Dateinamen) und gibt ein SaveObjekt-Objekt zurueck.
        public SaveObjekt DeSave(string filename)
        {
            SaveObjekt saveObjekt;
            Stream stream = File.Open(filename, FileMode.Open);         //Erstellt ein Filestream-Objekt (read). 
            BinaryFormatter bFormatter = new BinaryFormatter();         //Erstellt ein BinaryFormatter-Objekt.
            saveObjekt = (SaveObjekt)bFormatter.Deserialize(stream);    //Erstellt ein SaveObjekt-Objekt und schreibt die gelesenen SaveDaten hinein.    
            stream.Close();                                             //schließt den Stream.
            return saveObjekt;                                          //gibt das SaveObjekt-Objekt zurück. 
        }
    }
}
