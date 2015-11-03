using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace VyukaHN
{
    public class Tone
    {
        public Tone(string name, string referenceToneName, int chromaticChange, List<ToneRelationship> neighbors)
        {
            this.Name = name;
            this.ChromaticChange = chromaticChange;
            this.referenceToneName = referenceToneName;

            if (neighbors == null)
            {
                this.neighbors = new List<ToneRelationship>();
            }
            else
            {
                this.neighbors = neighbors;
            }
        }

        public string Name{ get; private set; }

        //-3 = ♭♭♭, -2 = ♭♭, -1 = ♭, 1 = ♯, 2 = ♯♯, 3 = ♯♯♯
        private int chromaticChange;

        public int ChromaticChange
        {
            get { return chromaticChange; }
            private set
            {
                if (value >= -3 && value <= 3)
                {
                    chromaticChange = value;
                }
                else
                {
                    throw new System.ArgumentException("Chromatic change cannot be less than -3 and more than 3");
                }
            }
        }

        /*
        basic tones (c, d, e, f, g, a, h)
        e.g.: cis - referenceTone c, ces - c, dis - d, deses - d, d - d
        */
        private string referenceToneName;

        public Tone ReferenceTone
        {
            get { return ToneGraph.GetToneByName(referenceToneName); }
        }

        private List<ToneRelationship> neighbors = new List<ToneRelationship>();
        public ReadOnlyCollection<ToneRelationship> Neighbors 
        {
            get { return new ReadOnlyCollection<ToneRelationship>(neighbors); }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}