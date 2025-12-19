<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frmPDOC.aspx.vb" Inherits="pdpw.frmPDOC" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>frmPDOC</title>
    <meta content="text/html; charset=windows-1252" http-equiv="Content-Type">
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="Form1" method="post" runat="server">
        <asp:Calendar Style="z-index: 100; position: absolute; top: 8px; left: 56px" ID="dtPDO" runat="server"
            Width="200px" Height="190px" BorderWidth="1px" NextPrevFormat="FullMonth" BackColor="Transparent"
            ForeColor="Black" Font-Size="XX-Small" Font-Names="Arial" BorderColor="White" BorderStyle="Solid">
            <TodayDayStyle BackColor="#CCCCCC"></TodayDayStyle>
            <NextPrevStyle Font-Size="8pt" Font-Bold="True" ForeColor="#333333" VerticalAlign="Bottom"></NextPrevStyle>
            <DayHeaderStyle Font-Size="8pt" Font-Bold="True"></DayHeaderStyle>
            <SelectedDayStyle ForeColor="White" BackColor="#333399"></SelectedDayStyle>
            <TitleStyle Font-Size="12pt" Font-Bold="True" BorderWidth="4px" ForeColor="#333399" BorderColor="Black"
                BackColor="White"></TitleStyle>
            <OtherMonthDayStyle ForeColor="#999999"></OtherMonthDayStyle>
        </asp:Calendar>
        <asp:Button Style="z-index: 121; position: absolute; top: 424px; left: 400px" ID="CmdVeOk" runat="server"
            Width="56px" Text="Ok" Visible="False"></asp:Button>
        <asp:RadioButtonList Style="z-index: 120; position: absolute; top: 312px; left: 360px" ID="RbListValElet"
            runat="server" Width="224px" Height="104px" Font-Size="XX-Small" Font-Names="Arial" BorderStyle="Inset" Visible="False" Font-Bold="True">
            <asp:ListItem Value="VE" Selected="True">Relat&#243;rio Valida&#231;&#227;o El&#233;trica</asp:ListItem>
            <asp:ListItem Value="VEV">Coment&#225;rios da Valida&#231;&#227;o</asp:ListItem>
        </asp:RadioButtonList>
        <asp:LinkButton Style="z-index: 119; position: absolute; top: 232px; left: 328px" ID="lblValElet"
            runat="server" Width="128px" Height="16px" ForeColor="DimGray" Font-Size="XX-Small" Font-Names="Arial" Font-Bold="True">VALIDAÇÃO ELÉTRICA</asp:LinkButton>
        <asp:Button Style="z-index: 116; position: absolute; top: 472px; left: 400px" ID="cmdOk1" runat="server"
            Width="56px" Text="Ok" Visible="False"></asp:Button>
        <asp:RadioButtonList Style="z-index: 115; position: absolute; top: 312px; left: 400px" ID="Rblist1" runat="server"
            Width="119px" Height="150px" Font-Size="XX-Small" Font-Names="Arial" BorderStyle="Inset" Visible="False" Font-Bold="True">
            <asp:ListItem Value="ON" Selected="True">S I N</asp:ListItem>
            <asp:ListItem Value="N">COSR-NCO</asp:ListItem>
            <asp:ListItem Value="S">COSR-S</asp:ListItem>
            <asp:ListItem Value="NE">COSR-NE</asp:ListItem>
            <asp:ListItem Value="SE">COSR-SE</asp:ListItem>
        </asp:RadioButtonList>
        <div style="margin-left: 40px">
            <asp:LinkButton Style="z-index: 110; position: absolute; top: 208px; left: 328px" ID="lnkPDPW" runat="server"
                Width="360px" Height="16px" ForeColor="DimGray" Font-Size="XX-Small" Font-Names="Arial" Font-Bold="True">PROGRAMA DIÁRIO DE PRODUÇÃO CONSOLIDADO - PDP</asp:LinkButton>
        </div>
        <asp:LinkButton Style="z-index: 109; position: absolute; top: 184px; left: 328px" ID="lblLimObser1"
            runat="server" Width="360px" Height="16px" ForeColor="DimGray" Font-Size="XX-Small" Font-Names="Arial" Font-Bold="True">LIMITAÇÕES E OBSERVAÇÕES - LimObs</asp:LinkButton>
        <asp:LinkButton Style="z-index: 108; position: absolute; top: 160px; left: 328px" ID="lblInfMet1"
            runat="server" Width="360px" Height="16px" ForeColor="DimGray" Font-Size="XX-Small" Font-Names="Arial" Font-Bold="True">INFORMAÇÕES METEOROLÓGICAS - Infmet</asp:LinkButton>
        <asp:LinkButton Style="z-index: 107; position: absolute; top: 136px; left: 328px" ID="lblRecomend1"
            runat="server" Width="360px" Height="16px" ForeColor="DimGray" Font-Size="XX-Small" Font-Names="Arial" Font-Bold="True">RECOMENDAÇÕES ELETROENERGÉTICAS CONSOLIDADAS - RDEc</asp:LinkButton>
        <asp:LinkButton Style="z-index: 106; position: absolute; top: 112px; left: 328px" ID="lblDefluConsol1"
            runat="server" Width="360px" Height="16px" ForeColor="DimGray" Font-Size="XX-Small" Font-Names="Arial" Font-Bold="True">PROGRAMA DIÁRIO DE DEFLUÊNCIAS CONSOLIDADO - PDFc</asp:LinkButton>
        <asp:LinkButton Style="z-index: 105; position: absolute; top: 88px; left: 328px" ID="lblCargFreq1"
            runat="server" Width="360px" Height="16px" ForeColor="DimGray" Font-Size="XX-Small" Font-Names="Arial" Font-Bold="True">PROGRAMA DIÁRIO DE CARGA E FREQUÊNCIA - PDCF</asp:LinkButton>
        <asp:Label Style="z-index: 101; position: absolute; top: 216px; left: 72px" ID="lblDtPDO" runat="server"
            Width="176px" Height="16px" Font-Size="X-Small" Font-Names="Arial" Font-Bold="True">Data do PDO: dd/MM/aaaa</asp:Label>
        <asp:Label Style="z-index: 102; position: absolute; top: 280px; left: 80px" ID="lblError" runat="server"
            Width="176px" Height="16px" ForeColor="Red" Font-Bold="True">Não há relatório disponível</asp:Label>
        <asp:Label Style="z-index: 103; position: absolute; top: 24px; left: 360px" ID="Label1" runat="server"
            Width="368px" Height="24px" Font-Size="Smaller" Font-Names="Arial Black">PROGRAMA DIÁRIO DA OPERAÇÃO</asp:Label>
        <asp:LinkButton Style="z-index: 104; position: absolute; top: 64px; left: 328px" ID="lbIntervencoes1"
            runat="server" Width="360px" Height="16px" ForeColor="DimGray" Font-Size="XX-Small" Font-Names="Arial" Font-Bold="True">PROGRAMA DIÁRIO DE INTERVENÇÕES CONSOLIDADO - PDIc</asp:LinkButton>
        <asp:RadioButtonList Style="z-index: 111; position: absolute; top: 304px; left: 384px" ID="Rblist" runat="server"
            Width="119px" Height="150px" Font-Size="XX-Small" Font-Names="Arial" BorderStyle="Inset" Visible="False" Font-Bold="True">
            <asp:ListItem Value="ON" Selected="True">S I N</asp:ListItem>
            <asp:ListItem Value="N">COSR-NCO</asp:ListItem>
            <asp:ListItem Value="S">COSR-S</asp:ListItem>
            <asp:ListItem Value="NE">COSR-NE</asp:ListItem>
            <asp:ListItem Value="SE">COSR-SE</asp:ListItem>
            <asp:ListItem Value="AG">AGENTE</asp:ListItem>
        </asp:RadioButtonList>
        <asp:Button Style="z-index: 112; position: absolute; top: 472px; left: 384px" ID="cmdOk" runat="server"
            Width="56px" Text="Ok" Visible="False"></asp:Button>
    </form>
</asp:Content>

