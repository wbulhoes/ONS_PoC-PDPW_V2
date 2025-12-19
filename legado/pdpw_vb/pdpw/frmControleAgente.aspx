<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frmControleAgente.aspx.vb" Inherits="pdpw.frmControleAgente" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>ONS - Operador Nacional do Sistema Elétrico</title>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <meta http-equiv="Content-Type" content="text/html; charset=windows-1252">
    <link href="../pdpw/images/style.css" rel="stylesheet">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table style="width: 788px; height: 418px" height="418" cellspacing="0" cellpadding="0"
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
                                        <table cellspacing="0" cellpadding="0" width="765" background="../pdpw/images/back_titulo.gif" border="0">
                                            <tr>
                                                <td style="height: 8px">
                                                    <div align="left">
                                                        <img src="../pdpw/images/tit_ControleAgentePDP.gif" style="width: 192px; height: 23px">
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <br>
                        <form id="frmControleAgente" name="frmControleAgente" runat="server">
                            <table style="width: 505px; height: 214px" height="214" cellspacing="0" cellpadding="0"
                                width="505" border="0">
                                <tr>
                                    <td style="width: 218px; height: 10px" align="right"><b>Data PDP:</b></td>
                                    <td style="width: 345px; height: 10px">&nbsp;<asp:DropDownList ID="cboData" runat="server" Width="100px" AutoPostBack="True"></asp:DropDownList></td>
                                </tr>
                                <tr height="42">
                                    <td style="width: 218px; height: 10px" align="right"><b>Empresa:</b></td>
                                    <td style="width: 345px; height: 10px">
                                        <p>
                                            &nbsp;<asp:DropDownList ID="cboEmpresa" runat="server" AutoPostBack="True" Width="219px"></asp:DropDownList>
                                        </p>
                                    </td>
                                </tr>
                                <tr valign="top" align="center">
                                    <td style="width: 539px; height: 8px" colspan="2">
                                        <br>
                                        <asp:Table ID="tblDados" runat="server" Width="472px" Font-Size="X-Small" CellSpacing="0">
                                            <asp:TableRow BackColor="#99CC00">
                                                <asp:TableCell Text="Empresa"></asp:TableCell>
                                                <asp:TableCell Text="Data Hora In&#237;cio"></asp:TableCell>
                                                <asp:TableCell Text="Data Hora Fim"></asp:TableCell>
                                            </asp:TableRow>
                                        </asp:Table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 539px; height: 8px" align="center" colspan="2">
                                        <asp:Button ID="Button1" runat="server" Height="16px" Text="Button"
                                            Visible="False" Width="16px" />
                                        <table>
                                            <tr>
                                                <td style="width: 71px">
                                                    <asp:ImageButton ID="btnIncluir" runat="server" Width="71px" Height="25px" ImageUrl="../pdpw/images/bt_incluir.gif"></asp:ImageButton></td>
                                                <td style="width: 71px">
                                                    <asp:ImageButton ID="btnExcluir" runat="server" Width="71px" Height="25px" ImageUrl="../pdpw/images/bt_excluir.gif"></asp:ImageButton></td>
                                                <td style="width: 71px">
                                                    <asp:ImageButton ID="btnAlterar" runat="server" Width="71px" Height="25px" ImageUrl="../pdpw/images/bt_alterar.gif"></asp:ImageButton></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </form>
                    </div>
                </td>
            </tr>
        </tbody>
    </table>
</asp:Content>

