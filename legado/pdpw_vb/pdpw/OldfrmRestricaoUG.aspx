<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="OldfrmRestricaoUG.aspx.vb" Inherits="pdpw.frmRestricaoUG" %>

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
                        <table border="0" cellspacing="0" cellpadding="0" width="100%">
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
                                                            <img height="23" src="../pdpw/images/tit_ColRestricaoUG.gif" width="280" style="width: 280px; height: 23px"></div>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <br>
                        <form id="frmRestricaoUG" name="frmRestricaoUG" runat="server">
                            <table style="width: 505px; height: 241px" height="241" cellspacing="0" cellpadding="0"
                                width="505" border="0">
                                <tr>
                                    <td style="width: 218px; height: 25px" align="right"><b>Data PDP:</b></td>
                                    <td style="width: 345px; height: 25px">&nbsp;<asp:DropDownList ID="cboData" runat="server" Width="97px" AutoPostBack="True"></asp:DropDownList></td>
                                </tr>
                                <tr height="42">
                                    <td style="width: 218px; height: 25px" align="right"><b>Empresa:</b></td>
                                    <td style="width: 345px; height: 25px">
                                        <p>
                                            &nbsp;<asp:DropDownList ID="cboEmpresa" runat="server" Width="219px" AutoPostBack="True"></asp:DropDownList>
                                        </p>
                                    </td>
                                </tr>
                                <tr height="42">
                                    <td style="width: 218px; height: 25px" align="right"><b>Usina:</b></td>
                                    <td style="width: 345px; height: 25px">
                                        <p>
                                            &nbsp;<asp:DropDownList ID="cboUsina" runat="server" Width="219px" AutoPostBack="True"></asp:DropDownList>
                                        </p>
                                    </td>
                                </tr>
                                <tr valign="top" align="center">
                                    <td style="width: 600px; height: 7px" colspan="2">
                                        <br>
                                        <asp:Table ID="tblGerador" runat="server" Width="560px" Font-Size="X-Small" CellSpacing="0">
                                            <asp:TableRow BackColor="#99CC00">
                                                <asp:TableCell Width="70" Text="C&#243;digo"></asp:TableCell>
                                                <asp:TableCell Width="80" Text="Gerador"></asp:TableCell>
                                                <asp:TableCell Width="60" Text="Sigla"></asp:TableCell>
                                                <asp:TableCell Width="80" Text="Data In&#237;cio"></asp:TableCell>
                                                <asp:TableCell Width="70" Text="Hora In&#237;cio"></asp:TableCell>
                                                <asp:TableCell Width="200" Text="Motivo Restrição"></asp:TableCell>
                                            </asp:TableRow>
                                        </asp:Table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 560px; height: 50px" colspan="2" align="center">
                                        <table>
                                            <tr>
                                                <td style="width: 71px">
                                                    <asp:ImageButton ID="btnIncluir" runat="server" Width="71px" ImageUrl="../pdpw/images/bt_incluir.gif"
                                                        Height="25px"></asp:ImageButton></td>
                                                <td style="width: 71px">
                                                    <asp:ImageButton ID="btnExcluir" runat="server" Width="71px" ImageUrl="../pdpw/images/bt_excluir.gif"
                                                        Height="25px"></asp:ImageButton></td>
                                                <td style="width: 71px">
                                                    <asp:ImageButton ID="btnAlterar" runat="server" Width="71px" ImageUrl="../pdpw/images/bt_alterar.gif"
                                                        Height="25px"></asp:ImageButton></td>
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

