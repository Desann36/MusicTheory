using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VyukaHN
{
    public class ToneRelationship
    {
        private string tone1Name;
        private string tone2Name;

        public Tone Tone1
        {
            get { return ToneGraph.GetToneByName(this.tone1Name); }
        }

        public Tone Tone2
        {
            get { return ToneGraph.GetToneByName(this.tone2Name); }
        }

        public int NumberOfHalftones { get; private set; }

        public ToneRelationship(string tone1Name, string tone2Name, int numberOfHalftones)
        {
            this.tone1Name = tone1Name;
            this.tone2Name = tone2Name;
            this.NumberOfHalftones = numberOfHalftones;
        }
    }
}