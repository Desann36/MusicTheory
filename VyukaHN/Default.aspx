<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" MasterPageFile="~/MasterPage.Master" Inherits="VyukaHN.Default" %>

<asp:Content ID="Content3" ContentPlaceHolderID="header" runat="server">
    <nav>
		<ul>
			<li><a href="Default.aspx" class="active">Určovanie intervalov</a></li>
			<li><a href="ConstructingIntervals.aspx">Vytváranie intervalov</a></li>
            <li><a href="ConstructingScales.aspx">Vytváranie stupníc</a></li>
		</ul>
	</nav>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="margin-top: 70px;">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div id="stave" class="stave">
                    <asp:Image ID="StaveImage" runat="server" ImageUrl="~/Resources/osnovaBezCiar.png">
                    </asp:Image>

                    <img id="Note1" class="StaveObject" runat="server" alt="Nota" src="~/Resources/nota.png" />
                    <img id="SharpFlatSymbol1" class="StaveObject" runat="server" src="~/Resources/krizik.png" />
                    <div id="LedgerLine1" class="StaveObject AuxLedgerLine" runat="server" 
                        onclick="img_OnClick()" onmousemove="img_OnMouseMove(event)"></div>

                    <img id="Note2" class="StaveObject" runat="server" alt="Nota" src="~/Resources/nota.png" />
                    <img id="SharpFlatSymbol2" class="StaveObject" runat="server" src="~/Resources/krizik.png" />
                    <div id="LedgerLine2" class="StaveObject AuxLedgerLine" runat="server" 
                        onclick="img_OnClick()" onmousemove="img_OnMouseMove(event)"></div>

                    <div class="staveLine" style="top: 75px;"></div>
                    <div class="staveLine" style="top: 105px;"></div>
                    <div class="staveLine" style="top: 135px;"></div>
                    <div class="staveLine" style="top: 165px;"></div>
                    <div class="staveLine" style="top: 195px;"></div>
                </div>
                <div id="controlPanel">
                    <div id="mainControls">
                        <asp:Label ID="AnswerLabelAdjective" class="labelAnswer" runat="server" ToolTip="prídavné meno" Text=""></asp:Label>
                        <asp:Label ID="AnswerLabelNumeral" class="labelAnswer" runat="server" ToolTip="číslovka" Text=""></asp:Label>
                        <asp:Button ID="RevealAnswerButton" class="controlButton" runat="server" OnClick="RevealAnswer_Click" Text="Odkryť odpoveď" />
                        <asp:Button ID="NextButton" class="controlButton nextButton" runat="server" OnClick="NextButton_Click" Text="Ďalší" />
                    </div>
                    <div id="lineUnderControls" style="top: 5px;"></div>
                    <div id="answerPanel" style="bottom: 18px;">
                        <div>
                            <asp:Button ID="Button_zm" class="answerButton" runat="server" OnClick="Button_zm_Click" ToolTip="zmenšená" Text="zm" />
                            <asp:Button ID="Button_m" class="answerButton" runat="server" OnClick="Button_m_Click" ToolTip="malá" Text="m" />
                            <asp:Button ID="Button_v" class="answerButton" runat="server" OnClick="Button_v_Click" ToolTip="veľká" Text="v" />
                            <asp:Button ID="Button_č" class="answerButton" runat="server" OnClick="Button_č_Click" ToolTip="čistá" Text="č" />
                            <asp:Button ID="Button_zv" class="answerButton" runat="server" OnClick="Button_zv_Click" ToolTip="zväčšená" Text="zv" />
                        </div>
                        <div style="margin-top: 5px;">
                            <asp:Button ID="Button1" class="answerButton" runat="server" OnClick="Button1_Click" ToolTip="prima" Text="1" />
                            <asp:Button ID="Button2" class="answerButton" runat="server" OnClick="Button2_Click" ToolTip="sekunda" Text="2" />
                            <asp:Button ID="Button3" class="answerButton" runat="server" OnClick="Button3_Click" ToolTip="tercia" Text="3" />
                            <asp:Button ID="Button4" class="answerButton" runat="server" OnClick="Button4_Click" ToolTip="kvarta" Text="4" />
                            <asp:Button ID="Button5" class="answerButton" runat="server" OnClick="Button5_Click" ToolTip="kvinta" Text="5" />
                            <asp:Button ID="Button6" class="answerButton" runat="server" OnClick="Button6_Click" ToolTip="sexta" Text="6" />
                            <asp:Button ID="Button7" class="answerButton" runat="server" OnClick="Button7_Click" ToolTip="septima" Text="7" />
                            <asp:Button ID="Button8" class="answerButton" runat="server" OnClick="Button8_Click" ToolTip="oktáva" Text="8" />
                        </div>
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
