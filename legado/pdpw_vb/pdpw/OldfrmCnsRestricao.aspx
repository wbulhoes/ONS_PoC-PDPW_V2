<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="OldfrmCnsRestricao.aspx.vb" Inherits="pdpw.frmCnsRestricao" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>ONS - Operador Nacional do Sistema Elétrico</title>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <meta http-equiv="Content-Type" content="text/html; charset=windows-1252">
    <link href="../pdpw/images/style.css" rel="stylesheet">
    <script language="javascript" src="Lib.js"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table style="width: 788px; height: 216px" height="216" cellspacing="0" cellpadding="0"
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
                                                            <img height="23" src="../pdpw/images/tit_CnsRestricao.gif" width="120" style="width: 120px; height: 23px"></div>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <br>
                        <form id="frmCnsRestricao" name="frmCnsRestricao" runat="server">
                            <div id="divtabela" style="left: 80px; width: 560px; position: absolute; top: 120px; height: 121px">
                                <table class="modulo" style="width: 584px; height: 102px" cellspacing="0" cellpadding="0"
                                    border="0">
                                    <tr valign="top">
                                        <td style="width: 125px; height: 10px">&nbsp;&nbsp;&nbsp;<%if Request.QueryString("strAcesso") <> "PDOC" Then
                                                                                                         Response.Write("Dados:")
                                                                                                     End If%></td>
                                        <td style="width: 173px; height: 10px">&nbsp;&nbsp;&nbsp;Selecionar:</td>
                                        <td style="width: 96px; height: 10px"></td>
                                        <td style="width: 85px"></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 125px" valign="top" align="right">
                                            <asp:RadioButtonList ID="optDados" runat="server" Width="118px" Font-Size="XX-Small" AutoPostBack="True">
                                                <asp:ListItem Value="restrgerademp" Selected="True">&#193;rea Transfer&#234;ncia</asp:ListItem>
                                                <asp:ListItem Value="temprestrgerad">Enviados</asp:ListItem>
                                                <asp:ListItem Value="restrgerad">Consolidados</asp:ListItem>
                                            </asp:RadioButtonList></td>
                                        <td style="width: 173px" valign="top">
                                            <table class="formulario_texto" style="width: 277px; height: 52px" cellspacing="0" cellpadding="0"
                                                border="0">
                                                <tr>
                                                    <td style="width: 93px; height: 27px" align="right">
                                                        <asp:Label Style="z-index: 0" ID="lblData" runat="server">Data do PDP</asp:Label>&nbsp;</td>
                                                    <td style="width: 211px; height: 27px">
                                                        <asp:DropDownList ID="cboDataInicial" runat="server" Width="90px" AutoPostBack="True"></asp:DropDownList>&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 93px" align="right">Empresa&nbsp;</td>
                                                    <td style="width: 211px">
                                                        <asp:DropDownList ID="cboEmpresa" runat="server" Width="199px" AutoPostBack="True"></asp:DropDownList></td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td align="center" style="width: 96px">
                                            <asp:ImageButton ID="btnVisualizar" runat="server" ImageUrl="images/bt_visualizar.gif"></asp:ImageButton></td>
                                        <td align="center" style="width: 85px">
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
    <div style="display: inline; left: 76px; width: 602px; position: absolute; top: 240px; height: 36px"
        align="left" ms_positioning="FlowLayout">
        <asp:Table ID="tblConsulta" runat="server" Width="537px" Font-Size="X-Small" CellSpacing="0"></asp:Table>
    </div>
</asp:Content>
