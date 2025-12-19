<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frmColVazao.aspx.vb" Inherits="pdpw.frmColVazao" %>

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
    <table style="width: 784px; height: 256px" height="256" cellspacing="0" cellpadding="0"
        width="784" border="0">
        <tbody>
            <tr>
                <td valign="top" width="55" style="width: 55px">
                    <br>
                </td>
                <td style="width: 781px" valign="top">
                    <div align="center">
                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                            <tbody>
                                <tr>
                                    <td width="20%" height="5" style="height: 5px">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td height="33" style="height: 33px">
                                        <table cellspacing="0" cellpadding="0" width="765" background="../pdpw/images/back_tit_sistema.gif"
                                            border="0">
                                            <tbody>
                                                <tr>
                                                    <td>
                                                        <img height="25" src="../pdpw/images/tit_sis_guideline.gif" width="179"></td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td height="2">
                                        <table cellspacing="0" cellpadding="0" width="765" background="../pdpw/images/back_titulo.gif"
                                            border="0">
                                            <tbody>
                                                <tr>
                                                    <td>
                                                        <div align="left">
                                                            <img height="23" src="../pdpw/images/tit_ColVazao.gif" width="88" style="width: 88px; height: 23px"></div>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <br>
                        <form id="frmColVazao" name="frmColVazao" runat="server">
                            <table style="width: 726px; height: 137px" cellspacing="0" cellpadding="0" border="0" style="font-family: Times New Roman;" font-size="Smaller">
                                <tr height="30">
                                    <td style="width: 110px; height: 42px" align="right"><b>Data PDP:</b></td>
                                    <td style="width: 605px; height: 42px">&nbsp;<asp:DropDownList ID="cboData" runat="server" Width="97px" AutoPostBack="True"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr height="30">
                                    <td style="width: 110px; height: 51px" align="right"><b>Empresa:</b></td>
                                    <td style="width: 605px; height: 51px">
                                        <p>
                                            &nbsp;<asp:DropDownList ID="cboEmpresa" runat="server" Width="219px" AutoPostBack="True"></asp:DropDownList>&nbsp;&nbsp;
												<asp:ImageButton ID="btnSalvar" runat="server" ImageUrl="images/bt_salvar.gif"></asp:ImageButton>
                                        </p>
                                    </td>
                                </tr>
                                <tr align="left">
                                    <td colspan="2" style="width: 718px">
                                        <asp:Table ID="tblVazao" runat="server" BorderWidth="1px" GridLines="Both" BorderStyle="Ridge"
                                            CellPadding="2" CellSpacing="0" Width="724px" Style="margin-left: 60px;">
                                        </asp:Table>
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

