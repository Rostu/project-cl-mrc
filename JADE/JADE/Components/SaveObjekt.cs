using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace JADE
{
// Eine allgemein gehaltene Klasse die als Container fuer zu speichernde(serilisation) Daten fungiert.
//Wird unsere Datenstruktur zum abspeichern bekommen, kann aber später auch andere Daten zum Speichern bekommen.
    
    [Serializable()]
    public class SaveObjekt : ISerializable                                         //Mit Zusatz (ISerializable) ist die Klasse später Serialisierbar.
    {
        private Daten[] list;                                                       //Erzeugt Array in dem zu Serialisierte Daten(in diesem fall ein Objekt unserer DatenKlasse) gespeichert werden.
        public Daten[] Transfer                                                     //Get und Set Methoden um Zugriff zu gewaehrleisten.
        {
            get { return this.list; }
            set { this.list = value; }
        }

        public SaveObjekt(){}                                                       //Leerer Konstruktor.

        //Konstruktor fuer die Serialisation
        public SaveObjekt(SerializationInfo info, StreamingContext ctxt)            
        {
            this.list = (Daten[])info.GetValue("LIST", typeof(Daten[]));            //Versieht die Klassenvariable mit den fuer die Serilisation benoetigten Informationen. 
        }

        //Funktion die den SerilisationsInformationen die infos ueber unsere Daten hinzufuegt.
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)    
        {
            info.AddValue("LIST", this.list);
        }
    }
}
