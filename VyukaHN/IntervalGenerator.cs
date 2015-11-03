using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace VyukaHN
{
    public struct GeneralInterval {
        public readonly string adjective;
        public readonly int numeral;
        public readonly int halftoneNumber;

        public GeneralInterval(string adjective, int numeral, int halftoneNumber) 
        {
            this.adjective = adjective;
            this.numeral = numeral;
            this.halftoneNumber = halftoneNumber;
        }

        public override string ToString()
        {
            return adjective + numeral.ToString();
        }
    }

    public class IntervalGenerator
    {
        private List<GeneralInterval> intervals;

        public IntervalGenerator()
        {
            this.ReadIntervals();
        }

        private void ReadIntervals()
        {
            this.intervals = new List<GeneralInterval>();

            string[] lines = VyukaHN.Properties.Resources.Intervals.Split(null);

            foreach (var line in lines)
            {
                if (line.Equals(""))
                {
                    continue;
                }

                var items = line.Split(';');

                GeneralInterval interval = new GeneralInterval(items[0], Convert.ToInt32(items[1]), Convert.ToInt32(items[2]));
                this.intervals.Add(interval);
            }
        }

        public Interval GenerateIntervalWithTones()
        {
            GeneralInterval itv = this.intervals[this.RandomNumber(0, this.intervals.Count)];
            Tone startingTone = this.GenerateStartingTone(itv);
            Tone targetTone = this.ComputeSecondToneOfInterval(startingTone, itv);

            return new Interval(startingTone, targetTone, itv);
        }

        private Tone GenerateStartingTone(GeneralInterval itv)
        {
            int randomNumber = this.RandomNumber(0, 14 - (itv.numeral - 1));
            Tone tone = ToneGraph.GetBasicToneByIndex(randomNumber);

            int randomNumber2 = this.RandomNumber(-1, 3);
            
            if (randomNumber2 != 0) 
            { 
                if (tone.Name != "h")
                {
                    tone = this.GetDerivedToneRelationship(tone, randomNumber2).Tone2;
                }
                else
                {
                    tone = this.GetDerivedToneRelationship(tone, Math.Abs(randomNumber2)).Tone2;
                }
            }

            return tone;
        }

        public Tone ComputeSecondToneOfInterval(Tone firstTone, GeneralInterval itv)
        {
            int sumOfHalftones = 0;
            Tone targetTone = firstTone;
            Tone startingToneReference = firstTone.ReferenceTone;

            if (firstTone.ChromaticChange == -1)
            {
                targetTone = startingToneReference;
                sumOfHalftones += 1;
            }
            else if (firstTone.ChromaticChange == 1)
            {
                targetTone = startingToneReference;
                sumOfHalftones -= 1;
            }

            for (int i = 1; i < itv.numeral; i++)
            {
                ToneRelationship nextBasicToneRelationship = this.GetBasicToneRelationship(targetTone);
                targetTone = nextBasicToneRelationship.Tone2;
                sumOfHalftones += nextBasicToneRelationship.NumberOfHalftones;
            }

            while (sumOfHalftones != itv.halftoneNumber)
            {
                ToneRelationship derivedToneRelationship = this.GetDerivedToneRelationship(targetTone, itv.halftoneNumber - sumOfHalftones);
                targetTone = derivedToneRelationship.Tone2;
                sumOfHalftones += derivedToneRelationship.NumberOfHalftones;
            }

            return targetTone;
        }

        private ToneRelationship GetBasicToneRelationship(Tone tone)
        {
            for (int i = 0; i < tone.Neighbors.Count; i++)
            {
                ToneRelationship toneRelationship = tone.Neighbors.ElementAt(i);

                if (ToneGraph.IsBasicTone(toneRelationship.Tone2))
                {
                    return toneRelationship;
                }
            }

            return null;
        }

        private ToneRelationship GetDerivedToneRelationship(Tone tone, int halftoneDifference)
        {
            for (int i = 0; i < tone.Neighbors.Count; i++)
            {
                ToneRelationship toneRelationship = tone.Neighbors.ElementAt(i);

                if (!ToneGraph.IsBasicTone(toneRelationship.Tone2)
                    && Math.Abs(halftoneDifference - toneRelationship.NumberOfHalftones) < Math.Abs(halftoneDifference))
                {
                    return toneRelationship;
                }
            }

            return null;
        }

        public GeneralInterval GetIntervalByAdjAndNumeral(string adjective, int numeral)
        {
            return intervals.Find(interval => interval.adjective.Equals(adjective) && interval.numeral == numeral);
        }

        public GeneralInterval GetIntervalByAdjAndNumberOfHalftones(string adjective, int numberOfHalftones)
        {
            return intervals.Find(interval => interval.adjective.Equals(adjective) 
                                              && interval.halftoneNumber == numberOfHalftones);
        }

        public GeneralInterval GetIntervalByNumeralAndNumberOfHalftones(int numeral, int numberOfHalftones)
        {
            return intervals.Find(interval => interval.numeral == numeral && interval.halftoneNumber == numberOfHalftones);
        }

        private int RandomNumber(int minValue, int maxValue)
        {
            return StaticRandom.Instance.Next(minValue, maxValue);
        }
    }

    public static class StaticRandom
    {
        private static int seed;

        private static ThreadLocal<Random> threadLocal = new ThreadLocal<Random>
            (() => new Random(Interlocked.Increment(ref seed)));

        static StaticRandom()
        {
            seed = Environment.TickCount;
        }

        public static Random Instance { get { return threadLocal.Value; } }
    }
}