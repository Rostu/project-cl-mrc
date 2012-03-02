using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace JADE
{
    /// <summary>
    /// Klasse welche zum Speichern unserer Datenstruktur verwendet wird. Es wurde auch hier Rücksicht auf Erweiterbarkeit genommen. Zum Speichern wird später ein String für den Pfad und ein SaveObjekt benötigt.
    /// </summary>
    class Saver
    {
        /**Leerer Konstruktor.*/
        public Saver() {}

        /**
         * Funktion zum Speichern in eine Save-Datei. 
         * Erhält einen String für den Dateipfad und ein Objekt der SaveObjekt Klasse.
         * @param[in] filename String (Pfad/Dateiname) der zu speichernden Datei.
         * @param[in] saveObjekt Objekt der SaveObjekt-Klasse. 
         */
        public void Save(string filename, SaveObjekt saveObjekt)
        {
            Stream stream = File.Open(filename, FileMode.Create);       //Erstellt ein Filestream-Objekt (write). 
            BinaryFormatter bFormatter = new BinaryFormatter();         //Erstellt ein BinaryFormatter-Objekt.
            bFormatter.Serialize(stream, saveObjekt);                   //Erstellt die Save-Datei und schreibt das SaveObjekt binär hinein.
            stream.Close();                                             //schließt den Stream.
        }

        /**
         * Funktion zum Laden aus einer Savedatei.
         * Erhält einen String mit dem Dateipfad der zu ladenden Datei und gibt ein SaveObjekt-Objekt zurück.
         * @param[in] filename Pfad der zu ladenden Datei.
         * @param[out] saveObjekt Objekt der Daten-Klasse, enthält die gelesenen Daten aus der Save-Datei.
        */
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
