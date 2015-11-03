using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace VyukaHN
{
    public partial class Default : System.Web.UI.Page
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
            this.clearAnswerLabels();
            this.ChangeBackgroundOfAnswerButtons("#8C8C8C");
            IntervalGenerator intervalGenerator = new IntervalGenerator();
            ActualInterval = intervalGenerator.GenerateIntervalWithTones();

            StaveObjectsPosition sop = new StaveObjectsPosition(226, 30);
            Tuple<Point, Point, Point>[] notesPosition = sop.DrawInterval(ActualInterval, 240);

            this.ChangePositionOfNote(notesPosition[0], this.Note1, this.LedgerLine1, this.SharpFlatSymbol1,
                                      this.ActualInterval.Tone1);
            this.ChangePositionOfNote(notesPosition[1], this.Note2, this.LedgerLine2, this.SharpFlatSymbol2,
                                      this.ActualInterval.Tone2);
        }

        private void clearAnswerLabels()
        {
            this.AnswerLabelAdjective.Text = "";
            this.AnswerLabelNumeral.Text = "";
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

        protected void RevealAnswer_Click(object sender, EventArgs e)
        {
            if (this.ActualInterval == null)
            {
                return;
            }

            this.AnswerLabelAdjective.Text = this.ActualInterval.GeneralInterval.adjective;
            this.AnswerLabelNumeral.Text = this.ActualInterval.GeneralInterval.numeral.ToString();

            this.ChangeBackgroundOfAnswerButtons("url(/Resources/lockedButtonBackground.png)");
        }

        private void ChangeBackgroundOfAnswerButtons(string background)
        {
            ChangeBackgroundOfAdjectiveButtons(background);
            ChangeBackgroundOfNumeralButtons(background);
        }

        private void ChangeBackgroundOfAdjectiveButtons(string background)
        {
            this.AddBackgroundToButtonOnCondition(this.Button_zm, background);
            this.AddBackgroundToButtonOnCondition(this.Button_m, background);
            this.AddBackgroundToButtonOnCondition(this.Button_v, background);
            this.AddBackgroundToButtonOnCondition(this.Button_č, background);
            this.AddBackgroundToButtonOnCondition(this.Button_zv, background);
        }

        private void ChangeBackgroundOfNumeralButtons(string background)
        {
            this.AddBackgroundToButtonOnCondition(this.Button1, background);
            this.AddBackgroundToButtonOnCondition(this.Button2, background);
            this.AddBackgroundToButtonOnCondition(this.Button3, background);
            this.AddBackgroundToButtonOnCondition(this.Button4, background);
            this.AddBackgroundToButtonOnCondition(this.Button5, background);
            this.AddBackgroundToButtonOnCondition(this.Button6, background);
            this.AddBackgroundToButtonOnCondition(this.Button7, background);
            this.AddBackgroundToButtonOnCondition(this.Button8, background);
        }

        private void AddBackgroundToButtonOnCondition(Button button, string background)
        {
            if (background.Contains("url") &&
                button.Text != this.ActualInterval.GeneralInterval.adjective &&
                button.Text != this.ActualInterval.GeneralInterval.numeral.ToString())
            {
                button.Style.Add("background-image", background);
                button.Style.Add("color", "#acacac");
            }

            if (!background.Contains("url"))
            {
                button.Style.Add("background-image", "none");
                button.Style.Add("background-color", background);
                button.Style.Add("color", "white");
            }
        }

        protected void Button_zm_Click(object sender, EventArgs e)
        {
            this.DetermineAdjectiveAnswer("zm");
        }

        protected void Button_m_Click(object sender, EventArgs e)
        {
            this.DetermineAdjectiveAnswer("m");
        }

        protected void Button_v_Click(object sender, EventArgs e)
        {
            this.DetermineAdjectiveAnswer("v");
        }

        protected void Button_č_Click(object sender, EventArgs e)
        {
            this.DetermineAdjectiveAnswer("č");
        }

        protected void Button_zv_Click(object sender, EventArgs e)
        {
            this.DetermineAdjectiveAnswer("zv");
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            this.DetermineNumeralAnswer(1);
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            this.DetermineNumeralAnswer(2);
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            this.DetermineNumeralAnswer(3);
        }

        protected void Button4_Click(object sender, EventArgs e)
        {
            this.DetermineNumeralAnswer(4);
        }

        protected void Button5_Click(object sender, EventArgs e)
        {
            this.DetermineNumeralAnswer(5);
        }

        protected void Button6_Click(object sender, EventArgs e)
        {
            this.DetermineNumeralAnswer(6);
        }

        protected void Button7_Click(object sender, EventArgs e)
        {
            this.DetermineNumeralAnswer(7);
        }

        protected void Button8_Click(object sender, EventArgs e)
        {
            this.DetermineNumeralAnswer(8);
        }

        private void DetermineAdjectiveAnswer(string adjective)
        {
            if (adjective.Equals(this.ActualInterval.GeneralInterval.adjective))
            {
                this.AnswerLabelAdjective.Text = adjective;
                ChangeBackgroundOfAdjectiveButtons("url(/Resources/lockedButtonBackground.png)");
            }
        }

        private void DetermineNumeralAnswer(int numeral)
        {
            if (numeral == this.ActualInterval.GeneralInterval.numeral)
            {
                this.AnswerLabelNumeral.Text = numeral.ToString();
                ChangeBackgroundOfNumeralButtons("url(/Resources/lockedButtonBackground.png)");
            }
        }
    }
}