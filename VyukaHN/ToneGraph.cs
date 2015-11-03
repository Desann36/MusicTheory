using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VyukaHN
{
    public static class ToneGraph
    {
        private static List<string> basicTones;
        private static List<Tone> toneList;

        public static int Count
        {
            get { return toneList.Count; }
        }

        static ToneGraph()
        {
            toneList = new List<Tone>();
            basicTones = new List<string>();
            ReadTones();
            ReadBasicTones();
        }

        private static void ReadTones()
        {
            string[] lines = VyukaHN.Properties.Resources.Tones.Split(null);

            foreach (var line in lines)
            {
                if (line.Equals(""))
                {
                    continue;
                }

                var items = line.Split(';');
                Tone tone = new Tone(items[0], items[1], Convert.ToInt32(items[2]), ReadDistancesOfTones(items[0]));
                toneList.Add(tone);
            }
        }

        private static List<ToneRelationship> ReadDistancesOfTones(string toneName)
        {
            List<ToneRelationship> neighbors = new List<ToneRelationship>();
            string[] lines = VyukaHN.Properties.Resources.DistancesOfTones.Split(null);

            foreach (var line in lines)
            {
                if (line.Equals("")) { continue; }

                var items = line.Split(';');

                if (items[0].Equals(toneName))
                {
                    neighbors.Add(new ToneRelationship(toneName, items[1], Convert.ToInt32(items[2])));
                }
            }

            return neighbors;
        }

        private static void ReadBasicTones()
        {
            var lines = VyukaHN.Properties.Resources.BasicTones.Split(null);

            foreach (var line in lines)
            {
                if (line.Equals("")) { continue; }

                basicTones.Add(line);
            }
        }

        public static int GetReferenceToneIndex(Tone tone)
        {
            return basicTones.FindIndex(x => x.Contains(tone.ReferenceTone.Name));
        }

        public static Tone GetBasicToneByIndex(int index)
        {
            if (index >= basicTones.Count || index < 0)
            {
                return null;
            }

            return GetToneByName(basicTones.ElementAt(index));
        }

        public static Tone GetToneByReferenceToneAndChromaticChange(Tone referenceTone, int chromaticChange)
        {
            if(referenceTone.ChromaticChange != 0 || referenceTone == null || chromaticChange > 3 || chromaticChange < -3)
            {
                return null;
            }

            if (chromaticChange == 0)
            {
                return referenceTone;
            }

            Tone tone = null;
            foreach (Tone t in toneList)
            {
                if (t.ReferenceTone.Name.Equals(referenceTone.Name) && t.ChromaticChange == chromaticChange)
                {
                    tone = t;
                }
            }

            return tone;
        }

        public static bool IsBasicTone(Tone tone)
        {
            return basicTones.Any(item => item.Equals(tone.Name));
        }

        public static Tone GetToneByName(string name)
        {
            return toneList.Find(tone => tone.Name.Equals(name));
        }
    }
}