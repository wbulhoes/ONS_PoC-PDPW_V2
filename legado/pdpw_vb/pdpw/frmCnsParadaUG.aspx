<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frmCnsParadaUG.aspx.vb" Inherits="pdpw.frmCnsParadaUG" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>ONS - Operador Nacional do Sistema Elétrico</title>
    <meta http-equiv="Content-Type" content="text/html; charset=windows-1252">
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <link href="images/style.css" rel="stylesheet">
    <script language="javascript" src="Lib.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table style="width: 788px; height: 242px" height="242" cellspacing="0" cellpadding="0"
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
                                        <table cellspacing="0" cellpadding="0" width="765" background="images/back_tit_sistema.gif"
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
                                        <table cellspacing="0" cellpadding="0" width="765" background="images/back_titulo.gif"
                                            border="0">
                                            <tbody>
                                                <tr>
                                                    <td style="height: 8px">
                                                        <div align="left">
                                                            <img height="23" src="images/tit_ColParadaUG.gif" width="408" style="width: 408px; height: 23px"></div>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <br>
                        <form id="frmCnsParadaUG" name="frmCnsParadaUG" runat="server">
                            <div id="divtabela" style="left: 80px; width: 598px; position: absolute; top: 120px; height: 121px">
                                <table class="modulo" style="width: 615px; height: 102px" cellspacing="0" cellpadding="0"
                                    border="0">
                                    <tr valign="top">
                                        <td style="width: 134px; height: 10px">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<%if Request.QueryString("strAcesso") <> "PDOC" Then
                                                                                                                                       Response.Write("Dados:")
                                                                                                                                   End If%></td>
                                        <td style="width: 228px; height: 10px">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
												Selecionar:</td>
                                        <td style="width: 96px; height: 10px"></td>
                                        <td style="width: 94px"></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 134px" valign="top" align="right">
                                            <asp:RadioButtonList ID="optDados" runat="server" Width="118px" Font-Size="XX-Small" AutoPostBack="True">
                                                <asp:ListItem Value="paralemp_co" Selected="True">&#193;rea Transfer&#234;ncia</asp:ListItem>
                                                <asp:ListItem Value="tempparal_co">Enviados</asp:ListItem>
                                                <asp:ListItem Value="paral_co">Consolidados</asp:ListItem>
                                            </asp:RadioButtonList></td>
                                        <td style="width: 228px" valign="top">
                                            <table class="formulario_texto" style="width: 287px; height: 52px" cellspacing="0" cellpadding="0"
                                                border="0">
                                                <tr>
                                                    <td style="width: 93px; height: 27px" align="right">
                                                        <asp:Label Style="z-index: 0" ID="lblData" runat="server">Data do PDP</asp:Label>&nbsp;</td>
                                                    <td style="width: 209px; height: 27px">
                                                        <asp:DropDownList ID="cboDataInicial" runat="server" Width="90px"></asp:DropDownList>&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 93px" align="right">Empresa&nbsp;</td>
                                                    <td>
                                                        <asp:DropDownList ID="cboEmpresa" runat="server" Width="200px"></asp:DropDownList></td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td align="center" style="width: 96px">
                                            <asp:ImageButton ID="btnVisualizar" runat="server" ImageUrl="images/bt_visualizar.gif"></asp:ImageButton></td>
                                        <td align="center" style="width: 94px">
                                            <img id="imgPlanilha" onmouseover="this.style.cursor='hand'" onclick="javascript:botao();"
                                                alt="" src="images/bt_planilha.gif" runat="server"></td>
                                    </tr>
                                </table>
                            </div>
                        </form>
                    </div>
                </td>
            </tr>
        </tbody>
    </table>
    <div style="display: inline; left: 76px; width: 626px; position: absolute; top: 240px; height: 36px"
        align="left" ms_positioning="FlowLayout">
        <asp:Table ID="tblConsulta" runat="server" Width="598px" Font-Size="X-Small" CellSpacing="0"></asp:Table>
    </div>

</asp:Content>

