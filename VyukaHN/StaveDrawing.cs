using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;

namespace VyukaHN
{
    public class StaveDrawing
    {     
        private int distanceOfFlatSharpSymbol = 0;

        public int LowestNotePositionY { get; private set; }
        public int DistanceBetweenLines { get; private set; } 

        public StaveDrawing(int lowestNotePositionY, int distanceBetweenLines)
        {
            this.LowestNotePositionY = lowestNotePositionY;
            this.DistanceBetweenLines = distanceBetweenLines;
        }

        public Bitmap DrawNote(Bitmap stave, Bitmap note, Tone tone, int notePositionX)
        {
            int index = ToneGraph.GetReferenceToneIndex(tone);
            Bitmap newBitmap = new Bitmap(stave.Width, stave.Height);

            using (Graphics graphics = Graphics.FromImage(newBitmap))
            {
                Rectangle ImageSize = new Rectangle(0, 0, stave.Width, stave.Height);
                graphics.FillRectangle(Brushes.White, ImageSize);

                if (tone.ChromaticChange != 0)
                {
                    DrawFlatSharpSymbol(newBitmap, tone, index, notePositionX);
                }

                graphics.DrawImage(note, new Point(notePositionX, this.LowestNotePositionY - 
                                                                 (this.DistanceBetweenLines / 2) * index));
                DrawLedgerLine(newBitmap, tone, index, notePositionX);
                graphics.CompositingMode = CompositingMode.SourceOver;
                stave.MakeTransparent(Color.White);
                graphics.DrawImage(stave, 0, 0);
            }

            return newBitmap;
        }

        private void DrawFlatSharpSymbol(Bitmap newBitmap, Tone tone, int index, int notePositionX)
        {
            Bitmap symbol = GetFlatSharpSymbol(tone.ChromaticChange);

            // ideal distance from note is 7/29 of height of note
            int symbolPositionX = notePositionX - (symbol.Width + (VyukaHN.Properties.Resources.nota.Height * 7 / 29)) 
                                  - this.distanceOfFlatSharpSymbol; //in case an interval (3, 4, 5, 6) is drawn, otherwise 0
            int lowestSymbolPositionY = GetLowestFlatSharpSymbolPositionY(symbol.Height, tone.ChromaticChange);
            int symbolPositionY = lowestSymbolPositionY - (this.DistanceBetweenLines / 2) * index;

            using (Graphics graphics = Graphics.FromImage(newBitmap))
            {
                graphics.DrawImage(symbol, new Point(symbolPositionX, symbolPositionY));
            }
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

        private void DrawLedgerLine(Bitmap newBitmap, Tone tone, int index, int XLowestNote)
        {
            int ledgerLinePositionY;

            switch (index)
            {
                case 0:
                case 1:
                    ledgerLinePositionY = this.LowestNotePositionY - 1;
                    break;
                case 13:
                    ledgerLinePositionY = (this.LowestNotePositionY - 1) - 12 * (this.DistanceBetweenLines / 2);
                    break;
                default:
                    return;
            }
           
            using (Graphics graphics = Graphics.FromImage(newBitmap))
            {
                graphics.DrawImage(VyukaHN.Properties.Resources.pomocnaLinka,
                                   new Point(XLowestNote - ((VyukaHN.Properties.Resources.pomocnaLinka.Width 
                                             - VyukaHN.Properties.Resources.nota.Width) / 2), ledgerLinePositionY));
            }
        }

        public Bitmap DrawInterval(Bitmap stave, Interval interval, int notePositionX)
        {
            int upperNotePositionX = this.GetUpperNotePositionX(interval, notePositionX);

            stave = DrawNote(stave, VyukaHN.Properties.Resources.nota, interval.Tone2, upperNotePositionX);

            if (interval.GeneralInterval.numeral > 2 
                && SharpFlatSymbolOverlapping(interval))
            {
                this.distanceOfFlatSharpSymbol = this.GetNoteFlatSharpSymbolWidth(interval.Tone2) + 5;
            }
            
            stave = DrawNote(stave, VyukaHN.Properties.Resources.nota, interval.Tone1, notePositionX);
            this.distanceOfFlatSharpSymbol = 0;
            return stave;
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