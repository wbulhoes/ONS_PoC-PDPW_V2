<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frmExporta.aspx.vb" Inherits="pdpw.frmExporta" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>frmExporta</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="Form1" method="post" runat="server">
        <asp:ImageButton Style="z-index: 101; position: absolute; top: 16px; left: 440px" ID="btnVoltar"
            runat="server" ImageUrl="images/bt_voltar.gif" Height="22px" Width="72px"></asp:ImageButton>
        <asp:ImageButton Style="z-index: 103; position: absolute; top: 16px; left: 336px" ID="btnExportar"
            runat="server" ImageUrl="images/bt_exportar.gif" Height="22px" Width="72px"></asp:ImageButton>
        <asp:DropDownList Style="z-index: 102; position: absolute; top: 16px; left: 184px" ID="cboTipo" runat="server"
            Height="16px" Width="120px">
        </asp:DropDownList>
        <asp:Table ID="tblDeflux1" runat="server" Width="51px" Font-Size="Smaller" Height="26px" GridLines="Both"
            CellSpacing="0" CellPadding="2" BorderWidth="1px" BorderStyle="Ridge" Visible="False" Style="z-index: 104; position: absolute; top: 168px; left: 48px">
        </asp:Table>
    </form>
</asp:Content>

