using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VyukaHN
{
    public class Scale
    {
        public Scale(string name, string type, Tone startingTone, int keySignature)
        {
            this.Name = name;
            this.Type = type;
            this.StartingTone = startingTone;
            this.KeySignature = keySignature;
        }

        public String Name { get; private set; }

        public Tone StartingTone { get; private set; }

        public Int32 KeySignature { get; private set; }

        public String Type { get; private set; }
    }
}