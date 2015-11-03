using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;

namespace VyukaHN
{
    public class StaveObjectsPosition
    {
        private int distanceOfFlatSharpSymbol = 0;

        public int LowestNotePositionY { get; private set; }
        public int DistanceBetweenLines { get; private set; }

        public StaveObjectsPosition(int lowestNotePositionY, int distanceBetweenLines)
        {
            this.LowestNotePositionY = lowestNotePositionY;
            this.DistanceBetweenLines = distanceBetweenLines;
        }

        public Tuple<Point, Point, Point> DrawNote(Tone tone, int notePositionX)
        {
            int toneIndex = ToneGraph.GetReferenceToneIndex(tone);
            Point notePosition = new Point();
            notePosition.X = notePositionX;
            notePosition.Y = 226 - 15 * toneIndex;

            int ledgerLineTop;

            if (toneIndex < 12)
            {
                ledgerLineTop = (LowestNotePositionY - 1) - (int)(toneIndex / 2) * DistanceBetweenLines;
            }
            else
            {
                ledgerLineTop = (LowestNotePositionY - 1) - (int)((toneIndex - 1) / 2) * DistanceBetweenLines;
            }

            Point ledgerLinePosition = new Point();
            ledgerLinePosition.X = notePositionX - 5;
            ledgerLinePosition.Y = ledgerLineTop;

            Point sharpFlatSymbolPosition = SharpFlatSymbolPosition(tone, notePositionX);

            return new Tuple<Point, Point, Point>(notePosition, ledgerLinePosition, sharpFlatSymbolPosition);
        }

        private Point SharpFlatSymbolPosition(Tone tone, int notePositionX)
        {
            if (tone.ChromaticChange == 0)
            {
                return new Point();
            }

            Bitmap symbol = GetFlatSharpSymbol(tone.ChromaticChange);

            // ideal distance from note is 7/29 of height of note
            int symbolPositionX = notePositionX - (symbol.Width + (VyukaHN.Properties.Resources.nota.Height * 7 / 29))
                                  - this.distanceOfFlatSharpSymbol; //in case an interval (3, 4, 5, 6) is drawn, otherwise 0
            int lowestSymbolPositionY = GetLowestFlatSharpSymbolPositionY(symbol.Height, tone.ChromaticChange);
            int symbolPositionY = lowestSymbolPositionY - (this.DistanceBetweenLines / 2) * ToneGraph.GetReferenceToneIndex(tone);

            return new Point(symbolPositionX, symbolPositionY);
        }

        private int GetLowestFlatSharpSymbolPositionY(int symbolHeight, int chromaticChange)
        {
            switch (chromaticChange)
            {
                case 1:
                case 3:
                    //(chromaticChangeSymbol.Height - distanceBetweenLines) divided according to ratio 22:27
                    return this.LowestNotePositionY - ((symbolHeight - this.DistanceBetweenLines) / 49) * 22;
                case 2:
                    return this.LowestNotePositionY;
                default:
                    return this.LowestNotePositionY - this.DistanceBetweenLines;
            }
        }

        private Bitmap GetFlatSharpSymbol(int chromaticChange)
        {
            switch (chromaticChange)
            {
                case -3:
                    return VyukaHN.Properties.Resources.becko3;
                case -2:
                    return VyukaHN.Properties.Resources.becko2;
                case -1:
                    return VyukaHN.Properties.Resources.becko;
                case 1:
                    return VyukaHN.Properties.Resources.krizik;
                case 2:
                    return VyukaHN.Properties.Resources.krizik2;
                case 3:
                    return VyukaHN.Properties.Resources.krizik3;
            }

            return null;
        }

        private int GetNoteFlatSharpSymbolWidth(Tone tone)
        {
            return this.GetFlatSharpSymbol(tone.ChromaticChange) == null ?
                                           0 : this.GetFlatSharpSymbol(tone.ChromaticChange).Width;
        }

        private int GetNoteFlatSharpSymbolHeight(Tone tone)
        {
            return this.GetFlatSharpSymbol(tone.ChromaticChange) == null ?
                                           0 : this.GetFlatSharpSymbol(tone.ChromaticChange).Height;
        }

        public Tuple<Point, Point, Point>[] DrawInterval(Interval interval, int notePositionX)
        {
            int upperNotePositionX = this.GetUpperNotePositionX(interval, notePositionX);

            Tuple<Point, Point, Point> note2 = DrawNote(interval.Tone2, upperNotePositionX);

            if (interval.GeneralInterval.numeral > 2
                && SharpFlatSymbolOverlapping(interval))
            {
                this.distanceOfFlatSharpSymbol = this.GetNoteFlatSharpSymbolWidth(interval.Tone2) + 5;
            }

            Tuple<Point, Point, Point> note1 = DrawNote(interval.Tone1, notePositionX);
            this.distanceOfFlatSharpSymbol = 0;
            return new Tuple<Point, Point, Point>[] { note1, note2 };
        }

        private int GetUpperNotePositionX(Interval interval, int notePositionX)
        {
            int flatSharpSymbolWidth = GetNoteFlatSharpSymbolWidth(interval.Tone2);

            if (interval.GeneralInterval.numeral == 1 || interval.GeneralInterval.numeral == 2)
            {
                return notePositionX + flatSharpSymbolWidth + (int)(1.7 * VyukaHN.Properties.Resources.nota.Width);
            }
            else
            {
                return notePositionX;
            }
        }

        private bool SharpFlatSymbolOverlapping(Interval interval)
        {
            int lowerNoteSymbolLowestPositionY =
                this.GetLowestFlatSharpSymbolPositionY(this.GetNoteFlatSharpSymbolHeight(interval.Tone1),
                    interval.Tone1.ChromaticChange);

            int higerNoteSymbolLowestPositionY =
                this.GetLowestFlatSharpSymbolPositionY(this.GetNoteFlatSharpSymbolHeight(interval.Tone2),
                    interval.Tone2.ChromaticChange);

            int lowerNoteSymbolPositionY = lowerNoteSymbolLowestPositionY - ToneGraph.GetReferenceToneIndex(interval.Tone1)
                                           * (this.DistanceBetweenLines / 2);

            int higherNoteSymbolPositionY = higerNoteSymbolLowestPositionY - ToneGraph.GetReferenceToneIndex(interval.Tone2)
                                            * (this.DistanceBetweenLines / 2);

            int distanceBetweenSymbols = lowerNoteSymbolPositionY - higherNoteSymbolPositionY
                                         - this.GetNoteFlatSharpSymbolHeight(interval.Tone2);

            return distanceBetweenSymbols <= 0 ? true : false;
        }
    }
}