<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frmRelOfertaReducaoSemana.aspx.vb" Inherits="pdpw.frmRelOfertaReducaoSemana" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script language="javascript" type="text/javascript" src="js/MSGAguarde.js"></script>
    <link href="css/MSGAguarde.css" rel="stylesheet" />
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
                                                        <div align="left" style="height: 27px">
                                                            <img height="23"
                                                                src="../pdpw/images/tit_Abrir_Dia.gif" style="width: 676px; height: 23px">
                                                        </div>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <br>
                        <form id="Form1" name="" runat="server">
                            <table style="width: 726px; height: 45px" cellspacing="0" cellpadding="0" border="0" bordercolor="#e9f4cf">
                                <tr height="30">
                                    <td style="height: 42px" align="right"><b>Mês:&nbsp;&nbsp;&nbsp; </b></td>
                                    <td style="height: 42px;" align="left">
                                        <asp:DropDownList ID="cmbMes" runat="server" Height="16px" Width="185px">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="height: 42px" align="right"><b>Ano:&nbsp;&nbsp;&nbsp; </b></td>
                                    <td style="height: 42px" align="left">
                                        <asp:DropDownList ID="cmbAno" runat="server" Height="18px" Width="185px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr height="30">
                                    <td style="height: 42px" align="right"></td>
                                    <td style="height: 42px;" align="left">
                                        <asp:ImageButton ID="btnGerar" runat="server" Width="71px" Height="25px" ImageUrl="../pdpw/images/bt_confirmar.gif" />
                                    </td>
                                    <td style="height: 42px" align="right"></td>
                                    <td style="height: 42px" align="left"></td>
                                </tr>
                            </table>
                        </form>
                    </div>
                </td>
            </tr>
        </tbody>
    </table>
</asp:Content>
