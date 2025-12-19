<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frmResultado.aspx.vb" Inherits="pdpw.frmResultado" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Frame</title>
    <meta http-equiv="Content-Type" content="text/html; charset=windows-1252">
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <frameset runat="server" rows="61,89%" border="0" framespacing="0" frameborder="0">
        <frame name="header" src="frmExporta.aspx?strData=<%=Request.Querystring("strData")%>&strRelatorio=<%=Request.Querystring("strRelatorio")%>&strTipoGer=<%=Request.Querystring("strTipoGer")%>&strRegional=<%=Request.Querystring("strRegional")%>&strAgrega=<%=Request.Querystring("strAgrega")%>&vlrRequisito=<%=Request.Querystring("vlrRequisito")%>&horRequisito=<%=Request.Querystring("horRequisito")%>&vlrReserva=<%=Request.Querystring("vlrReserva")%>&horReserva=<%=Request.Querystring("horReserva")%>" scrolling="no" noresize>
            <frame name="viewframe" src="" scrolling="no" noresize>
                <noframes>
                    <pre id="p2">
			</pre>
                    <p id="p1">
                    </p>
                </noframes>
    </frameset>
</asp:Content>
