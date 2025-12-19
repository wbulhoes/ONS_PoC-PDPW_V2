<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="OldfrmCnsPropGeracao.aspx.vb" Inherits="pdpw.frmCnsPropGeracao" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>ONS - Operador Nacional do Sistema Elétrico</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <meta http-equiv="Content-Type" content="text/html; charset=windows-1252">
    <link href="../pdpw/images/style.css" rel="stylesheet">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table style="width: 788px; height: 232px" height="232" cellspacing="0" cellpadding="0"
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
                                                        <img height="25" src="../pdpw/images/tit_sis_guideline.gif" width="179"></td>
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
                                                            <img height="23" src="../pdpw/images/tit_PropGeracao.gif" width="176" style="width: 176px; height: 23px"></div>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <br>
                        <form id="frmCnsPropGeracao" name="frmCnsPropGeracao" runat="server">
                            <div id="divtabela" style="left: 80px; width: 617px; position: absolute; top: 120px; height: 48px">
                                <table class="modulo" style="width: 616px; height: 144px" cellspacing="0" cellpadding="0"
                                    border="0">
                                    <tr valign="top">
                                        <td style="width: 42px; height: 1px"><%if Request.QueryString("strAcesso") <> "PDOC" Then
                                                                                     Response.Write("Dados:")
                                                                                 End If%></td>
                                        <td style="width: 306px; height: 1px">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
												Selecionar:</td>
                                        <td style="height: 1px"></td>
                                        <td style="width: 81px"></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 42px; height: 76px" valign="top">
                                            <asp:RadioButtonList ID="optDados" runat="server" Font-Size="XX-Small" Width="118px">
                                                <asp:ListItem Value="2" Selected="True">Consolidados</asp:ListItem>
                                            </asp:RadioButtonList></td>
                                        <td style="width: 306px; height: 76px">
                                            <table class="formulario_texto" style="width: 311px; height: 58px" cellspacing="0" cellpadding="0"
                                                border="0">
                                                <tr>
                                                    <td style="width: 93px; height: 27px" align="right">Data do PDP&nbsp;</td>
                                                    <td style="width: 213px; height: 27px">
                                                        <asp:DropDownList ID="cboDataInicial" runat="server" Width="90px" AutoPostBack="True"></asp:DropDownList></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 93px" align="right">Empresa&nbsp;</td>
                                                    <td style="width: 213px">
                                                        <asp:DropDownList ID="cboEmpresa" runat="server" Width="199px" AutoPostBack="True"></asp:DropDownList></td>
                                                </tr>
                                            </table>
                                            <p>&nbsp;</p>
                                        </td>
                                        <td align="center">
                                            <asp:ImageButton ID="btnVisualizar" runat="server" ImageUrl="images/bt_visualizar.gif"></asp:ImageButton></td>
                                        <td align="center" style="width: 81px">
                                            <img id="imgPlanilha" onmouseover="this.style.cursor='hand'" onclick="javascript:botao();"
                                                alt="" src="images/bt_planilha.gif" runat="server"></td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" align="center" style="width: 607px">
                                            <asp:Label ID="lblMensagem" runat="server" Visible="False"></asp:Label>
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
    <div style="display: inline; left: 20px; width: 768px; position: absolute; top: 248px; height: 24px"
        align="left" ms_positioning="FlowLayout">
        <br>
        <asp:Table ID="tblConsulta" runat="server" Font-Size="X-Small" Width="70px" CellSpacing="0"></asp:Table>
    </div>
</asp:Content>
