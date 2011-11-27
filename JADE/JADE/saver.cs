using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

// Klasse welche zum speichern unserer Datenstruktur verwendet wird. Alles sehr allgemein gehalten, zum speichern wird später ein string für den Pfad und ein saveobjekt gebraucht.
//Für das Saveobjekt git es eine eigene Klasse welche später unsere Datenstruktur erhält und so nur als "Container" fungiert.

namespace JADE
{
    class Saver
    {
        public Saver()
        {
        }
        //Funktion zum Speichern
        public void Save(string filename, SaveObjekt saveObjekt)
        {
            Stream stream = File.Open(filename, FileMode.Create);
            BinaryFormatter bFormatter = new BinaryFormatter();
            bFormatter.Serialize(stream, saveObjekt);
            stream.Close();
        }
        //Funktion zum Laden
        public SaveObjekt DeSave(string filename)
        {
            SaveObjekt saveObjekt;
            Stream stream = File.Open(filename, FileMode.Open);
            BinaryFormatter bFormatter = new BinaryFormatter();
            saveObjekt = (SaveObjekt)bFormatter.Deserialize(stream);
            stream.Close();
            return saveObjekt;
        }
    }
}
