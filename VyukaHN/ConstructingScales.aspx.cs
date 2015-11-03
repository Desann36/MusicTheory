using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace VyukaHN
{
    public partial class ConstructingScales : System.Web.UI.Page
    {
        private static List<Scale> listOfScales;
        private static int[] durIntervals = { 2, 2, 1, 2, 2, 2, 1 };
        private static int[] molIntervals = { 2, 1, 2, 2, 1, 2, 2 };

        static ConstructingScales()
        {
            listOfScales = new List<Scale>();
            string[] lines = VyukaHN.Properties.Resources.Scales.Split(null);

            foreach (var line in lines)
            {
                if (line.Equals(""))
                {
                    continue;
                }

                var items = line.Split(';');
                Scale scale = new Scale(items[0] + " " + items[1], items[1], ToneGraph.GetToneByName(items[2]), Convert.ToInt32(items[3]));

                listOfScales.Add(scale);
            }
        }

        public Scale ActualScale
        {
            get
            {
                return Session["ActualScale"] == null ? null : (Scale)Session["ActualScale"];
            }

            set 
            {
                Session["ActualScale"] = value; 
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.ActualNoteIndex.Value = "1";
                this.NextExercise();
            }
        }

        protected void NextButton_Click(object sender, EventArgs e)
        {
            this.NextExercise();
        }

        private void NextExercise()
        {
            this.ChangeCSSAtributeOfNotes("visibility", "hidden");
            //this.AddPointerToFirstNote();
            this.ActualScale = listOfScales.ElementAt(this.RandomNumber(0, listOfScales.Count - 1));
            this.ActualNoteIndex.Value = "1";

            this.QuestionTone.InnerText = this.ActualScale.StartingTone.ToString();
            this.QuestionScale.InnerText = this.ActualScale.Name.ToString();
        }

        private void ChangePositionOfNote(Tuple<Point, Point, Point> notePosition, HtmlImage Note, 
                                          HtmlGenericControl LedgerLine, HtmlImage SharpFlatSymbol, Tone tone)
        {
            Note.Style.Add("visibility", "visible");
            Note.Style.Add("left", notePosition.Item1.X.ToString() + "px");
            Note.Style.Add("top", notePosition.Item1.Y.ToString() + "px");

            LedgerLine.Style.Add("visibility", "visible");
            LedgerLine.Style.Add("left", notePosition.Item2.X.ToString() + "px");
            LedgerLine.Style.Add("top", notePosition.Item2.Y.ToString() + "px");

            string sharpFlatSymbolSrc = this.SharpFlatSymbolSrc(tone.ChromaticChange);

            if (sharpFlatSymbolSrc.Equals(""))
            {
                SharpFlatSymbol.Style.Add("visibility", "hidden");
            }
            else
            {
                SharpFlatSymbol.Style.Add("visibility", "visible");
                SharpFlatSymbol.Src = sharpFlatSymbolSrc;
            }

            SharpFlatSymbol.Style.Add("left", notePosition.Item3.X.ToString() + "px");
            SharpFlatSymbol.Style.Add("top", notePosition.Item3.Y.ToString() + "px");
        }

        private void ChangeCSSAtributeOfNotes(string attribute, string value)
        {
            this.ChangeCSSAtributeOfNote(this.Note1, this.LedgerLine1, this.SharpFlatSymbol1, attribute, value);
            this.ChangeCSSAtributeOfNote(this.Note2, this.LedgerLine2, this.SharpFlatSymbol2, attribute, value);
            this.ChangeCSSAtributeOfNote(this.Note3, this.LedgerLine3, this.SharpFlatSymbol3, attribute, value);
            this.ChangeCSSAtributeOfNote(this.Note4, this.LedgerLine4, this.SharpFlatSymbol4, attribute, value);
            this.ChangeCSSAtributeOfNote(this.Note5, this.LedgerLine5, this.SharpFlatSymbol5, attribute, value);
            this.ChangeCSSAtributeOfNote(this.Note6, this.LedgerLine6, this.SharpFlatSymbol6, attribute, value);
            this.ChangeCSSAtributeOfNote(this.Note7, this.LedgerLine7, this.SharpFlatSymbol7, attribute, value);
            this.ChangeCSSAtributeOfNote(this.Note8, this.LedgerLine8, this.SharpFlatSymbol8, attribute, value);
        }

        private void ChangeCSSAtributeOfNote(HtmlImage Note, HtmlGenericControl LedgerLine, HtmlImage SharpFlatSymbol, 
                                             string attribute, string value)
        {
            Note.Style.Add(attribute, value);
            LedgerLine.Style.Add(attribute, value);
            SharpFlatSymbol.Style.Add(attribute, value);
        }

        private void AddPointerToFirstNote()
        {
            Note1.Style.Add("cursor", "pointer");
            LedgerLine1.Style.Add("cursor", "pointer");
            SharpFlatSymbol1.Style.Add("cursor", "pointer");
        }

        private string SharpFlatSymbolSrc(int chromaticChange)
        {
            switch (chromaticChange)
            {
                case -3:
                    return "~/Resources/becko3.png";
                case -2:
                    return "~/Resources/becko2.png";
                case -1:
                    return "~/Resources/becko.png";
                case 1:
                    return "~/Resources/krizik.png";
                case 2:
                    return "~/Resources/krizik2.png";
                case 3:
                    return "~/Resources/krizik3.png";
            }

            return "";
        }

        protected void BezPosuvkyButton_Click(object sender, EventArgs e)
        {
            this.DetermineAnswer(0);
        }

        protected void KrizikButton_Click(object sender, ImageClickEventArgs e)
        {
            this.DetermineAnswer(1);
        }

        protected void BeckoButton_Click(object sender, ImageClickEventArgs e)
        {
            this.DetermineAnswer(-1);
        }

        private void DetermineAnswer(int chromaticChange)
        {
            int actualNoteIndex = Convert.ToInt32(this.ActualNoteIndex.Value);

            Tone toneAux = ToneGraph.GetBasicToneByIndex(Convert.ToInt32(this.ToneClicked.Value));
            Tone toneClicked = ToneGraph.GetToneByReferenceToneAndChromaticChange(toneAux, chromaticChange);

            Tone nextTone = this.GetNextToneToDisplay();

            if (toneClicked.Name.Equals(nextTone.Name))
            {
                this.DisplayNextNote();
            }
        }

        private void DisplayNextNote()
        {
            int actualNoteIndex = Convert.ToInt32(this.ActualNoteIndex.Value);
            Tone nextTone = this.GetNextToneToDisplay();

            if (nextTone != null)
            {
                StaveObjectsPosition sop = new StaveObjectsPosition(226, 30);
                Tuple<Point, Point, Point> notePosition = sop.DrawNote(nextTone, 150 + (117 * (actualNoteIndex - 1)));

                this.ActualNoteIndex.Value = (Convert.ToInt32(this.ActualNoteIndex.Value) + 1).ToString();
                this.PreviousTone.Value = nextTone.Name;

                switch (actualNoteIndex)
                {
                    case 1: this.ChangePositionOfNote(notePosition, this.Note1, this.LedgerLine1, this.SharpFlatSymbol1,
                                 nextTone);
                            break;
                    case 2: this.ChangePositionOfNote(notePosition, this.Note2, this.LedgerLine2, this.SharpFlatSymbol2,
                                nextTone);
                             break;
                    case 3: this.ChangePositionOfNote(notePosition, this.Note3, this.LedgerLine3, this.SharpFlatSymbol3,
                                nextTone);
                             break;
                    case 4: this.ChangePositionOfNote(notePosition, this.Note4, this.LedgerLine4, this.SharpFlatSymbol4,
                                nextTone);
                            break;
                    case 5: this.ChangePositionOfNote(notePosition, this.Note5, this.LedgerLine5, this.SharpFlatSymbol5,
                                nextTone);
                            break;
                    case 6: this.ChangePositionOfNote(notePosition, this.Note6, this.LedgerLine6, this.SharpFlatSymbol6,
                                nextTone);
                            break;
                    case 7: this.ChangePositionOfNote(notePosition, this.Note7, this.LedgerLine7, this.SharpFlatSymbol7,
                                nextTone);
                            break;
                    case 8: this.ChangePositionOfNote(notePosition, this.Note8, this.LedgerLine8, this.SharpFlatSymbol8,
                                nextTone);
                            this.SetQuestionAsAnswered();
                            break;
                }
            }
        }

        private Tone GetNextToneToDisplay()
        {
            int actualNoteIndex = Convert.ToInt32(this.ActualNoteIndex.Value);

            if (this.ActualNoteIndex.Value.Equals(""))
            {
                return null;
            }

            if (this.PreviousTone.Value.Equals(""))
            {
                return this.ActualScale.StartingTone;
            }

            Tone tonePrev = ToneGraph.GetToneByName(this.PreviousTone.Value);

            int halftoneNumber;
            if (this.ActualScale.Type.Equals("dur"))
            {
                halftoneNumber = durIntervals[actualNoteIndex - 2];
            }
            else
            {
                halftoneNumber = molIntervals[actualNoteIndex - 2];
            }

            IntervalGenerator intervalGenerator = new IntervalGenerator();
            GeneralInterval interval = intervalGenerator.GetIntervalByNumeralAndNumberOfHalftones(2, halftoneNumber);
            return intervalGenerator.ComputeSecondToneOfInterval(tonePrev, interval); 
        }

        protected void RevealNextNoteButton_Click(object sender, EventArgs e)
        {
            if (this.ActualScale == null || this.ActualNoteIndex.Value.Equals(""))
            {
                return;
            }

            this.DisplayNextNote();
        }

        protected void RevealAnswer_Click(object sender, EventArgs e)
        {
            if (this.ActualScale == null || this.ActualNoteIndex.Value.Equals(""))
            {
                return;
            }

            int actualNoteIndex = Convert.ToInt32(this.ActualNoteIndex.Value);

            for (int i = actualNoteIndex; i <= 8; i++)
            {
                this.DisplayNextNote();
            }
        }

        private void SetQuestionAsAnswered()
        {
            this.ActualNoteIndex.Value = "";
            this.PreviousTone.Value = "";
            this.ChangeCSSAtributeOfNotes("cursor", "default");
        }

        private int RandomNumber(int minValue, int maxValue)
        {
            return StaticRandom.Instance.Next(minValue, maxValue);
        }
    }
}