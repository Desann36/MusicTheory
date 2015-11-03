<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConstructingIntervals.aspx.cs" MasterPageFile="~/MasterPage.Master"  Inherits="VyukaHN.ConstructingIntervals" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.3.2/jquery.min.js"></script>
    <script type="text/javascript">
        function img_OnMouseMove(event) {
            if (document.getElementById('<%=Tone1Index.ClientID%>').value != "") {
                var top = (document.documentElement && document.documentElement.scrollTop) ||
                            document.body.scrollTop
                var left = (document.documentElement && document.documentElement.scrollLeft) ||
                            document.body.scrollLeft
                var pos_x = Math.floor(event.clientX + left - document.getElementById("stave").offsetLeft);
                var pos_y = Math.floor(event.clientY + top - document.getElementById("stave").offsetTop);

                var tone1Index = document.getElementById('<%=Tone1Index.ClientID%>').value;
                var tone2Index = GetNoteIndex(pos_y);

                if (tone1Index > tone2Index) {
                    img_OnMouseOut();
                    return;
                }

                if (tone2Index != -1) {
                    DrawOnStave(PositionNoteX(tone1Index, tone2Index),
                                PositionNoteY(tone1Index, tone2Index),
                                PositionLedgerLineX(tone1Index, tone2Index),
                                PositionLedgerLineY(tone1Index, tone2Index));
                }
            }
        }

        function GetNoteIndex(pos_y) {
            if (pos_y >= 249 || pos_y <= 38) return -1;
            if (pos_y >= 232 && pos_y <= 248) return 0;
            if (pos_y >= 219 && pos_y <= 231) return 1;
            if (pos_y >= 202 && pos_y <= 218) return 2;
            if (pos_y >= 189 && pos_y <= 201) return 3;
            if (pos_y >= 172 && pos_y <= 188) return 4;
            if (pos_y >= 159 && pos_y <= 171) return 5;
            if (pos_y >= 142 && pos_y <= 158) return 6;
            if (pos_y >= 129 && pos_y <= 141) return 7;
            if (pos_y >= 112 && pos_y <= 128) return 8;
            if (pos_y >= 99 && pos_y <= 111) return 9;
            if (pos_y >= 82 && pos_y <= 98) return 10;
            if (pos_y >= 69 && pos_y <= 81) return 11;
            if (pos_y >= 52 && pos_y <= 68) return 12;
            if (pos_y >= 39 && pos_y <= 51) return 13;
        }

        function PositionNoteX(tone1Index, tone2Index) {
            return document.getElementById('<%=StaveImage.ClientID%>').offsetLeft + 240 + Shift(tone1Index, tone2Index) + "px";
        }

        function PositionNoteY(tone1Index, tone2Index) {
            var auxNoteTop = 226 - 15 * tone2Index;
            return document.getElementById('<%=StaveImage.ClientID%>').offsetTop + auxNoteTop + "px";
        }

        function PositionLedgerLineX(tone1Index, tone2Index) {
            return document.getElementById('<%=StaveImage.ClientID%>').offsetLeft + 235 + Shift(tone1Index, tone2Index) + "px";
        }

        function PositionLedgerLineY(tone1Index, tone2Index) {
            var ledgerLineTop;

            if (tone2Index < 12) {
                ledgerLineTop = 225 - Math.floor(tone2Index / 2) * 30;
            } else {
                ledgerLineTop = 225 - Math.floor((tone2Index - 1) / 2) * 30;
            }

            return document.getElementById('<%=StaveImage.ClientID%>').offsetTop + ledgerLineTop + "px";
        }

        function Shift(tone1Index, tone2Index) {
            var shift = 0;

            if (tone1Index == tone2Index || tone1Index == tone2Index - 1) {
                shift = 76;
            }
            return shift;
        }

        function DrawOnStave(posNote_x, posNote_y, positionLedgerLine_x, positionLedgerLine_y) {
            AuxNote_MouseOver();
            document.getElementById('<%=AuxLedgerLine2.ClientID%>').style.visibility = 'visible';
            $("[id$='AuxNote']").css("top", posNote_y);
            $("[id$='AuxNote']").css("left", posNote_x);
            $("[id$='AuxLedgerLine2']").css("top", positionLedgerLine_y);
            $("[id$='AuxLedgerLine2']").css("left", positionLedgerLine_x);
            $("[id$='StaveImage']").css({ "cursor": "pointer" });
            $(".staveLine").css({ "cursor": "pointer" });
        }

        function img_OnMouseOut() {
            $("[id$='StaveImage']").css({ "cursor": "default" });
            $("[id$='AuxNote']").css({ "cursor": "default" });
            $(".staveLine").css({ "cursor": "default" });
            if (document.getElementById('<%=AuxNote.ClientID%>').style.visibility == 'visible') {
                document.getElementById('<%=AuxNote.ClientID%>').style.visibility = 'hidden';
            }
            if (document.getElementById('<%=AuxLedgerLine2.ClientID%>').style.visibility == 'visible') {
                document.getElementById('<%=AuxLedgerLine2.ClientID%>').style.visibility = 'hidden';
            }
        }

        function AuxNote_MouseOver() {
            $("[id$='AuxNote']").css({ "cursor": "pointer" });
            if (document.getElementById('<%=AuxNote.ClientID%>').style.visibility != 'visible') {
                document.getElementById('<%=AuxNote.ClientID%>').style.visibility = 'visible';
            }
            if (document.getElementById('<%=AuxLedgerLine2.ClientID%>').style.visibility != 'visible') {
                document.getElementById('<%=AuxLedgerLine2.ClientID%>').style.visibility = 'visible';
            }
        }

        function img_OnClick(event) {
            if (document.getElementById('<%=Tone1Index.ClientID%>').value != "") {
                var top = (document.documentElement && document.documentElement.scrollTop) ||
                            document.body.scrollTop
                var left = (document.documentElement && document.documentElement.scrollLeft) ||
                            document.body.scrollLeft

                var pos_x = Math.floor(event.clientX + left - document.getElementById("stave").offsetLeft);
                var pos_y = Math.floor(event.clientY + top - document.getElementById("stave").offsetTop);

                var tone1Index = document.getElementById('<%=Tone1Index.ClientID%>').value;
                var tone2Index = GetNoteIndex(pos_y);

                if (pos_y <= 248 && pos_y >= 39 && GetNoteIndex(pos_y) != -1 && tone2Index >= tone1Index) {
                    $("[id$='posuvky']").css("top", pos_y - 80 + "px");
                    $("[id$='posuvky']").show(200);
                    document.getElementById('<%=ToneClicked.ClientID%>').value = tone2Index;
                    $("[id$='ClickNote']").css("top", PositionNoteY(tone1Index, tone2Index));
                    $("[id$='ClickNote']").css("left", PositionNoteX(tone1Index, tone2Index));
                    $("[id$='AuxLedgerLine3']").css("top", PositionLedgerLineY(tone1Index, tone2Index));
                    $("[id$='AuxLedgerLine3']").css("left", PositionLedgerLineX(tone1Index, tone2Index));
                    document.getElementById('<%=ClickNote.ClientID%>').style.visibility = 'visible';
                    document.getElementById('<%=AuxLedgerLine3.ClientID%>').style.visibility = 'visible';
                }
            }
        }

        function closePosuvky_Click() {

        }
    </script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="header" runat="server">
    <nav>
		<ul>
			<li><a href="Default.aspx">Určovanie intervalov</a></li>
			<li><a href="ConstructingIntervals.aspx" class="active">Vytváranie intervalov</a></li>
            <li><a href="ConstructingScales.aspx">Vytváranie stupníc</a></li>
		</ul>
	</nav>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
    </asp:ScriptManager>
    <div style="margin-top:20px;">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <br />
                <br />
                <asp:HiddenField ID="Tone1Index" runat="server" Value=""/>
                <asp:HiddenField ID="ToneClicked" runat="server" Value=""/>
                <asp:HiddenField ID="StaveImageOffset" runat="server" Value=""/>

                <div id="stave" class="stave">
                    <asp:Image ID="StaveImage" runat="server" onmousemove="img_OnMouseMove(event)" 
                        onmouseout="img_OnMouseOut()" onclick="img_OnClick(event)" ImageUrl="~/Resources/osnovaBezCiar.png">
                    </asp:Image>

                    <img id="Note1" class="StaveObject StaveObjectWithPointer" runat="server" 
                        onmousemove="img_OnMouseMove(event)" onclick="img_OnClick(event)" 
                        alt="Nota" src="~/Resources/nota.png" />
                    <img id="SharpFlatSymbol1" class="StaveObject StaveObjectWithPointer" runat="server" 
                        onmousemove="img_OnMouseMove(event)" onclick="img_OnClick(event)" src="~/Resources/krizik.png" />
                    <div id="LedgerLine1" class="StaveObject StaveObjectWithPointer AuxLedgerLine" runat="server" 
                        onclick="img_OnClick()" onmousemove="img_OnMouseMove(event)"></div>

                    <img id="Note2" class="StaveObject" runat="server" 
                        onmousemove="img_OnMouseMove(event)" onclick="img_OnClick(event)" 
                        alt="Nota" src="~/Resources/nota.png" />
                    <img id="SharpFlatSymbol2" class="StaveObject" runat="server" 
                        onmousemove="img_OnMouseMove(event)" onclick="img_OnClick(event)" src="~/Resources/krizik.png" />
                    <div id="LedgerLine2" class="StaveObject AuxLedgerLine" runat="server" 
                        onclick="img_OnClick()" onmousemove="img_OnMouseMove(event)"></div>

                    <img id="AuxNote" class="StaveObject StaveObjectWithPointer" runat="server" 
                        onmousemove="img_OnMouseMove(event)" onclick="img_OnClick(event)" 
                        onmouseover="AuxNote_MouseOver()" alt="Pomocna nota" src="Resources/notaPomocna.png" />
                    <div id="AuxLedgerLine2" class="StaveObject StaveObjectWithPointer AuxLedgerLine" runat="server" 
                        onclick="img_OnClick()" onmousemove="img_OnMouseMove(event)"></div>
                    
                    <img id="ClickNote" class="StaveObject StaveObjectWithPointer" onclick="img_OnClick(event)" 
                        onmousemove="img_OnMouseMove(event)" runat="server" alt="Pomocna nota" src="~/Resources/notaPomocna2.png" />
                    <div id="AuxLedgerLine3" class="StaveObject StaveObjectWithPointer AuxLedgerLine" runat="server" 
                        onclick="img_OnClick()" onmousemove="img_OnMouseMove(event)"></div>
                    
                    <div class="staveLine" style="top: 75px;" runat="server" onclick="img_OnClick(event)" 
                        onmousemove="img_OnMouseMove(event)"></div>
                    <div class="staveLine" style="top: 105px;" onclick="img_OnClick(event)" 
                        onmousemove="img_OnMouseMove(event)"></div>
                    <div class="staveLine" style="top: 135px;" onclick="img_OnClick(event)" 
                        onmousemove="img_OnMouseMove(event)"></div>
                    <div class="staveLine" style="top: 165px;" onclick="img_OnClick(event)" 
                        onmousemove="img_OnMouseMove(event)"></div>
                    <div class="staveLine" style="top: 195px;" onclick="img_OnClick(event)" 
                        onmousemove="img_OnMouseMove(event)"></div>

                    <div id="posuvky" class="posuvky" runat="server">
                        <button id="closePosuvky" class="closePosuvky" runat="server" onclick="closePosuvky_Click()">✖</button>
                        <asp:Button ID="BezPosuvkyButton" class="BezPosuvkyButton" OnClick="BezPosuvkyButton_Click" runat="server" Text="bez posuvky" />
                        <asp:ImageButton ID="KrizikButton" class="posuvkyButtons" runat="server" ToolTip="krížik"
                            OnClick="KrizikButton_Click" ImageUrl="~/Resources/krizikButton.png"/>
                        <asp:ImageButton ID="BeckoButton" class="posuvkyButtons" runat="server" ToolTip="béčko"
                            OnClick="BeckoButton_Click" ImageUrl="~/Resources/beckoButton.png"/>
                        <asp:ImageButton ID="Krizik2Button" class="posuvkyButtons" runat="server" ToolTip="dva krížiky"
                            OnClick="Krizik2Button_Click" ImageUrl="~/Resources/krizik2Button.png"/>
                        <asp:ImageButton ID="Becko2Button" class="posuvkyButtons" runat="server" ToolTip="dve béčka"
                            OnClick="Becko2Button_Click" ImageUrl="~/Resources/becko2Button.png"/>
                        <asp:ImageButton ID="Krizik3Button" class="posuvkyButtons" runat="server" ToolTip="tri krížiky"
                            OnClick="Krizik3Button_Click" ImageUrl="~/Resources/krizik3Button.png"/>
                        <asp:ImageButton ID="Becko3Button" class="posuvkyButtons" runat="server" ToolTip="tri béčka"
                            OnClick="Becko3Button_Click" ImageUrl="~/Resources/becko3Button.png"/>
                    </div>
                </div>
                <div id="controlPanel">
                    <div id="answerPanel">
                        <pre style="margin-top: 30px; font-family: 'Times New Roman', Times, serif; font-size: 16px;">Od udaného tónu utvorte:      <span id="QuestionInterval" class="labelQuestion" runat="server"></span></pre>
                    </div>
                    <div id="lineUnderControls"></div>
                    <div id="mainControls">
                        <asp:Button ID="RevealAnswerButton" class="controlButton" runat="server" OnClick="RevealAnswer_Click" 
                            Text="Odkryť odpoveď" />
                        <asp:Button ID="NextButton" class="controlButton nextButton" runat="server" OnClick="NextButton_Click" 
                            Text="Ďalší" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="preload">
        <img alt="becko" width="1" height="1" src="Resources/becko.png" />
        <img alt="becko2" width="1" height="1" src="Resources/becko2.png" />
        <img alt="becko3" width="1" height="1" src="Resources/becko3.png" />
        <img alt="krizik" width="1" height="1" src="Resources/krizik.png" />
        <img alt="krizik2" width="1" height="1" src="Resources/krizik2.png" />
        <img alt="krizik3" width="1" height="1" src="Resources/krizik3.png" />
    </div>
</asp:Content>
