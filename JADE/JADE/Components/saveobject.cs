using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace JADE
{
// Wird unsere Datenstruktur zum abspeichern bekommen, kann aber später auch andere Daten zum Speichern bekommen.
    
    [Serializable()]
    public class SaveObjekt : ISerializable
    {
        private Daten[] list;

        public Daten[] Transfer
        {
            get { return this.list; }
            set { this.list = value; }
        }

        public SaveObjekt()
        {
        }
        public SaveObjekt(SerializationInfo info, StreamingContext ctxt)
        {
            this.list = (Daten[])info.GetValue("LIST", typeof(Daten[]));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("LIST", this.list);
        }
    }
}
