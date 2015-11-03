using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VyukaHN
{
    public class Interval
    {
        public Interval(Tone tone1, Tone tone2, GeneralInterval generalInterval)
        {
            this.Tone1 = tone1;
            this.Tone2 = tone2;
            this.GeneralInterval = generalInterval;
        }

        public Tone Tone1{ get; private set; }

        public Tone Tone2{ get; private set; }

        public GeneralInterval GeneralInterval{ get; private set; }

        public override string ToString()
        {
            return this.Tone1.ToString() + ", " + this.Tone2.ToString() + ", " + this.GeneralInterval.ToString();
        }
    }
}