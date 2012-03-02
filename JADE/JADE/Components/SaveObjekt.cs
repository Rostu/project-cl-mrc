using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace JADE
{
    /// <summary>
    ///Eine Klasse, die als Container für zu speichernde Daten fungiert.
    ///Sie wird um Datenstruktur zum Abspeichern erweitert werden, kann aber potentiell auch andere Daten zum Speichern erhalten.
    /// </summary>
    [Serializable()]
    public class SaveObjekt : ISerializable                                         //Mit Zusatz (ISerializable) ist die Klasse später Serialisierbar.
    {
        /**Variable vom Typ Daten[].*/
        private Daten[] list;                                                       //Erzeugt Array in dem zu Serialisierte Daten(in diesem Fall ein Objekt unserer DatenKlasse) gespeichert werden.
        
        /*Get- und Set-Methoden um Lese- und Schreibzugriff zu gewährleisten.*/
        public Daten[] Transfer                                                     
        {
            get { return this.list; }
            set { this.list = value; }
        }
        /*Leerer Konstruktor.*/
        public SaveObjekt(){}

        /**
         * Konstruktor für die Serialisation/Deserialisation.
        * @param[in] info Objekt vom Typ SerializationInfo
        * @param[in] ctxt Objekt vom Typ StreamingContext
         */
        public SaveObjekt(SerializationInfo info, StreamingContext ctxt)            
        {
            this.list = (Daten[])info.GetValue("LIST", typeof(Daten[]));            //Versieht die Klassenvariable mit den für die Serilisation benötigten Informationen. 
        }

        /**
        * Benötigt die Serialize-Funktion.
        * Fügt der SerializeInfo Informationen über das zu serialisierende Objekt hinzu.
        * @param[in] info Objekt vom Typ SerializationInfo
        * @param[in] ctxt Objekt vom Typ StreamingContext
        */
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)    
        {
            info.AddValue("LIST", this.list);
        }
    }
}
