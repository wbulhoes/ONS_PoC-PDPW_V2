<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frmCnsRRO.aspx.vb" Inherits="pdpw.frmCnsRRO" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>ONS - Operador Nacional do Sistema Elétrico</title>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <meta http-equiv="Content-Type" content="text/html; charset=windows-1252">
    <script language="javascript" src="Lib.js"></script>
    <link href="../pdpw/images/style.css" rel="stylesheet">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table style="width: 788px; height: 240px" height="240" cellspacing="0" cellpadding="0"
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
                                                            <img height="23" src="../pdpw/images/tit_ColRRO.gif" width="104" style="width: 104px; height: 23px"></div>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <br>
                        <form id="frmCnsGeracao" name="frmCnsGeracao" runat="server">
                            <div id="divtabela" style="left: 72px; width: 567px; position: absolute; top: 120px; height: 133px">
                                <table class="modulo" cellspacing="0" cellpadding="0" border="0" style="width: 600px; height: 114px">
                                    <tr valign="top">
                                        <td style="width: 108px; height: 14px"><%if Request.QueryString("strAcesso") <> "PDOC" Then
                                                                                       Response.Write("Dados:")
                                                                                   End If%></td>
                                        <td style="width: 306px; height: 14px">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
												Selecionar:</td>
                                        <td style="width: 85px; height: 14px"></td>
                                        <td style="width: 89px"></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 108px">
                                            <asp:RadioButtonList ID="optDados" runat="server" Font-Size="XX-Small" Width="118px" AutoPostBack="True">
                                                <asp:ListItem Value="0" Selected="True">&#193;rea Transfer&#234;ncia</asp:ListItem>
                                                <asp:ListItem Value="1">Enviados</asp:ListItem>
                                                <asp:ListItem Enabled="false" Value="2">Consolidados</asp:ListItem>
                                                <asp:ListItem Value="3">Recebidos DESSEM</asp:ListItem>
                                                <asp:ListItem Value="4">Consistidos DESSEM</asp:ListItem>
                                            </asp:RadioButtonList></td>
                                        <td style="width: 306px">
                                            <table class="formulario_texto" style="width: 311px; height: 58px" cellspacing="0" cellpadding="0"
                                                border="0">
                                                <tr>
                                                    <td style="width: 93px; height: 27px" align="right">
                                                        <asp:Label ID="lblData" runat="server">Data do PDP</asp:Label>&nbsp;</td>
                                                    <td style="width: 209px; height: 27px">
                                                        <asp:DropDownList ID="cboDataInicial" runat="server" Width="90px" AutoPostBack="True"></asp:DropDownList>&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 93px" align="right">Empresa&nbsp;</td>
                                                    <td style="width: 209px">
                                                        <asp:DropDownList ID="cboEmpresa" runat="server" Width="199px" AutoPostBack="True"></asp:DropDownList></td>
                                                </tr>
                                            </table>
                                            <br>
                                            <br>
                                            <asp:Label ID="lblMensagem" runat="server" Visible="False"></asp:Label>
                                        </td>
                                        <td align="center" style="width: 85px">
                                            <asp:ImageButton ID="btnVisualizar" runat="server" ImageUrl="images/bt_visualizar.gif"></asp:ImageButton></td>
                                        <td align="center" style="width: 89px">
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
    <div style="display: inline; left: 76px; width: 635px; position: absolute; top: 260px; height: 36px"
        align="left" ms_positioning="FlowLayout">
        <asp:Table ID="tblConsulta" runat="server" Font-Size="X-Small" Width="609px" CellSpacing="0"></asp:Table>
    </div>
</asp:Content>

