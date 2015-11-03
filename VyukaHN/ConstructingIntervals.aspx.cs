using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace VyukaHN
{
    public partial class ConstructingIntervals : System.Web.UI.Page
    {
        public Interval ActualInterval
        {
            get
            {
                return Session["ActualInterval"] == null ? null : (Interval)Session["ActualInterval"];
            }

            set 
            { 
                Session["ActualInterval"] = value; 
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.NextExercise();
            }
        }

        protected void NextButton_Click(object sender, EventArgs e)
        {
            this.NextExercise();
        }

        private void NextExercise()
        {
            this.HideSecondNote();
            this.AddPointerToFirstNote();

            IntervalGenerator intervalGenerator = new IntervalGenerator();
            this.ActualInterval = intervalGenerator.GenerateIntervalWithTones();
            this.QuestionInterval.InnerText = this.ActualInterval.GeneralInterval.ToString();
            this.QuestionInterval.Attributes.Add("title", 
                this.WholeIntervalAdjective(this.ActualInterval.GeneralInterval.adjective) + " " +
                this.WholeIntervalNumeral(this.ActualInterval.GeneralInterval.numeral));
            this.Tone1Index.Value = ToneGraph.GetReferenceToneIndex(this.ActualInterval.Tone1).ToString();

            StaveObjectsPosition sop = new StaveObjectsPosition(226, 30);
            Tuple<Point, Point, Point> notePosition = sop.DrawNote(this.ActualInterval.Tone1, 240);

            this.ChangePositionOfNote(notePosition, this.Note1, this.LedgerLine1, this.SharpFlatSymbol1,
                                      this.ActualInterval.Tone1);
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
                SharpFlatSymbol.Src = this.SharpFlatSymbolSrc(tone.ChromaticChange);
            }

            SharpFlatSymbol.Style.Add("left", notePosition.Item3.X.ToString() + "px");
            SharpFlatSymbol.Style.Add("top", notePosition.Item3.Y.ToString() + "px");
        }

        private void HideSecondNote()
        {
            this.Note2.Style.Add("visibility", "hidden");
            this.LedgerLine2.Style.Add("visibility", "hidden");
            SharpFlatSymbol2.Style.Add("visibility", "hidden");
        }

        private void AddPointerToFirstNote()
        {
            Note1.Style.Add("cursor", "pointer");
            LedgerLine1.Style.Add("cursor", "pointer");
            SharpFlatSymbol1.Style.Add("cursor", "pointer");
        }

        private string WholeIntervalAdjective(string adj)
        {
            switch(adj)
            {
                case "zm": return "zmenšená";
                case "m": return "malá";
                case "v": return "veľká";
                case "č": return "čistá";
                case "zv": return "zväčšená";
            }

            return "prídavné meno";
        }

        private string WholeIntervalNumeral(int numeral)
        {
            switch (numeral)
            {
                case 1: return "prima";
                case 2: return "sekunda";
                case 3: return "tercia";
                case 4: return "kvarta";
                case 5: return "kvinta";
                case 6: return "sexta";
                case 7: return "septima";
                case 8: return "oktáva";
            }

            return "číslovka";
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

        protected void Krizik2Button_Click(object sender, ImageClickEventArgs e)
        {
            this.DetermineAnswer(2);
        }

        protected void Becko2Button_Click(object sender, ImageClickEventArgs e)
        {
            this.DetermineAnswer(-2);
        }

        protected void Krizik3Button_Click(object sender, ImageClickEventArgs e)
        {
            this.DetermineAnswer(3);
        }

        protected void Becko3Button_Click(object sender, ImageClickEventArgs e)
        {
            this.DetermineAnswer(-3);
        }

        private void DetermineAnswer(int chromaticChange)
        {
            Tone tone = ToneGraph.GetBasicToneByIndex(Convert.ToInt32(this.ToneClicked.Value));
            Tone toneWithChromaticChange = ToneGraph.GetToneByReferenceToneAndChromaticChange(tone, chromaticChange);
            if (toneWithChromaticChange.Name.Equals(this.ActualInterval.Tone2.Name))
            {
                this.SetQuestionAsAnswered();
            }
        }

        protected void RevealAnswer_Click(object sender, EventArgs e)
        {
            if (this.ActualInterval == null || this.Tone1Index.Value.Equals(""))
            {
                return;
            }

            this.SetQuestionAsAnswered();
        }

        private void SetQuestionAsAnswered()
        {
            StaveObjectsPosition sop = new StaveObjectsPosition(226, 30);
            Tuple<Point, Point, Point>[] notesPosition = sop.DrawInterval(this.ActualInterval, 240);

            this.ChangePositionOfNote(notesPosition[0], this.Note1, this.LedgerLine1, this.SharpFlatSymbol1,
                                      this.ActualInterval.Tone1);
            this.ChangePositionOfNote(notesPosition[1], this.Note2, this.LedgerLine2, this.SharpFlatSymbol2,
                                      this.ActualInterval.Tone2);

            this.Tone1Index.Value = "";

            Note1.Style.Add("cursor", "default");
            LedgerLine1.Style.Add("cursor", "default");
            SharpFlatSymbol1.Style.Add("cursor", "default");
        }
    }
}