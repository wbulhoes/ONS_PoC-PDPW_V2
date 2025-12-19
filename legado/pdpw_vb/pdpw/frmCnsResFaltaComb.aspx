<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frmCnsResFaltaComb.aspx.vb" Inherits="pdpw.frmCnsResFaltaComb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>ONS - Operador Nacional do Sistema Elétrico</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <meta http-equiv="Content-Type" content="text/html; charset=windows-1252">
    <link href="images/style.css" rel="stylesheet">
    <script language="javascript" src="Lib.js"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table style="width: 788px; height: 256px" height="256" cellspacing="0" cellpadding="0"
        width="788" border="0">
        <tbody>
            <tr>
                <td style="width: 19px" valign="top" width="19">
                    <br>
                </td>
                <td valign="top">
                    <div align="center">
                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                            <tbody>
                                <tr>
                                    <td width="20%" height="2">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td style="height: 17px" height="17">
                                        <table cellspacing="0" cellpadding="0" width="765" background="../pdpw/images/back_tit_sistema.gif"
                                            border="0">
                                            <tbody>
                                                <tr>
                                                    <td style="height: 12px">
                                                        <script>MontaCabecalho();</script>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 10px" height="10">
                                        <table cellspacing="0" cellpadding="0" width="765" background="../pdpw/images/back_titulo.gif"
                                            border="0">
                                            <tbody>
                                                <tr>
                                                    <td style="height: 8px">
                                                        <div align="left">
                                                            <img height="23" src="../pdpw/images/tit_ResFaltaComb.gif" width="280" style="width: 280px; height: 23px"></div>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <br>
                        <form id="frmCnsResFaltaComb" name="frmCnsResFaltaComb" runat="server">
                            <div id="divtabela" style="left: 80px; width: 584px; position: absolute; top: 120px; height: 146px">
                                <table class="modulo" style="width: 607px; height: 102px" cellspacing="0" cellpadding="0"
                                    border="0">
                                    <tr valign="top">
                                        <td style="width: 123px; height: 11px"><%if Request.QueryString("strAcesso") <> "PDOC" Then
                                                                                       Response.Write("Dados:")
                                                                                   End If%></td>
                                        <td style="width: 306px; height: 11px">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
												Selecionar:</td>
                                        <td style="height: 11px"></td>
                                        <td style="width: 87px"></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 123px">
                                            <asp:RadioButtonList ID="optDados" runat="server" Font-Size="XX-Small" Width="118px" AutoPostBack="True">
                                                <asp:ListItem Value="0" Selected="True">&#193;rea Transfer&#234;ncia</asp:ListItem>
                                                <asp:ListItem Value="1">Enviados</asp:ListItem>
                                                <asp:ListItem Enabled="false" Value="2">Consolidados</asp:ListItem>
                                                <asp:ListItem Value="3">Recebidos DESSEM</asp:ListItem>
                                                <asp:ListItem Value="4">Consistidos DESSEM</asp:ListItem>
                                            </asp:RadioButtonList></td>
                                        <td style="width: 306px">
                                            <table class="formulario_texto" style="width: 311px; height: 40px" cellspacing="0" cellpadding="0"
                                                border="0">
                                                <tr>
                                                    <td style="width: 93px; height: 27px" align="right">
                                                        <asp:Label Style="z-index: 0" ID="lblData" runat="server">Data do PDP</asp:Label>&nbsp;</td>
                                                    <td style="width: 209px; height: 27px">
                                                        <asp:DropDownList ID="cboDataInicial" runat="server" Width="90px" AutoPostBack="True"></asp:DropDownList>&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 93px; height: 27px" align="right">Empresa&nbsp;</td>
                                                    <td style="width: 209px; height: 27px">
                                                        <asp:DropDownList ID="cboEmpresa" runat="server" Width="212px" AutoPostBack="True"></asp:DropDownList></td>
                                                </tr>
                                            </table>
                                            <p>&nbsp;</p>
                                        </td>
                                        <td align="center">
                                            <asp:ImageButton ID="btnVisualizar" runat="server" ImageUrl="images/bt_visualizar.gif"></asp:ImageButton></td>
                                        <td align="center" style="width: 87px">
                                            <img id="imgPlanilha" onmouseover="this.style.cursor='hand'" onclick="javascript:botao();"
                                                alt="" src="images/bt_planilha.gif" runat="server"></td>
                                    </tr>
                                </table>
                                <table style="width: 368px; height: 25px" align="center">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lblMensagem" runat="server" Visible="False" Font-Size="X-Small" Font-Bold="True"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </form>
                    </div>
                </td>
            </tr>
        </tbody>
    </table>
    <div style="display: inline; left: 68px; width: 641px; position: absolute; top: 260px; height: 43px"
        align="left" ms_positioning="FlowLayout">
        <asp:Table ID="tblConsulta" runat="server" Font-Size="X-Small" Width="607px" CellSpacing="0"
            Height="24px">
        </asp:Table>
    </div>
</asp:Content>
