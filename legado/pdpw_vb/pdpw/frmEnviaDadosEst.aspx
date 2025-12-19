<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frmEnviaDadosEst.aspx.vb" Inherits="pdpw.frmEnviaDadosEst" %>

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
    <table style="width: 801px; height: 620px" height="620" cellspacing="0" cellpadding="0"
        width="801" border="0">
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
                                        <table style="width: 1034px; height: 25px" cellspacing="0" cellpadding="0" width="1034"
                                            background="../pdpw/images/back_tit_sistema.gif" border="0">
                                            <tbody>
                                                <tr>
                                                    <td style="width: 766px; height: 12px">
                                                        <img height="25" src="../pdpw/images/tit_sis_guideline.gif" width="179"></td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 10px" height="10">
                                        <table style="width: 1035px; height: 23px" cellspacing="0" cellpadding="0" width="1035"
                                            background="../pdpw/images/back_titulo.gif" border="0">
                                            <tbody>
                                                <tr>
                                                    <td style="height: 8px">
                                                        <div align="left">
                                                            <img style="width: 136px; height: 23px" height="23" src="../pdpw/images/tit_EnviarDados.gif"
                                                                width="136">
                                                        </div>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <div align="center">
                        <form id="frmEnviarDadosEst" name="frmEnviarDadosEst" runat="server">
                            <table class="modulo" style="width: 954px; height: 280px" cellspacing="0" cellpadding="0"
                                border="0">
                                <tr>
                                    <td style="width: 1082px; height: 194px">
                                        <table class="modulo" style="width: 327px; height: 192px" cellspacing="0" cellpadding="0"
                                            border="0">
                                            <tbody>
                                                <tr>
                                                    <td style="width: 231px; height: 169px" valign="top">
                                                        <br>
                                                        <table class="modulo" style="width: 242px; height: 62px" cellspacing="0" cellpadding="0"
                                                            border="0">
                                                            <tr>
                                                                <td style="font-weight: normal; font-size: 10px; width: 74px; color: #000000; font-style: normal; font-family: Arial, Helvetica, sans-serif; height: 16px"
                                                                    align="right">
                                                                    <p>Data do PDP&nbsp;</p>
                                                                </td>
                                                                <td style="width: 163px; height: 30px">
                                                                    <asp:TextBox ID="txtData" runat="server" Width="79px" Font-Size="X-Small" Height="20px"></asp:TextBox><asp:Button ID="btnCalendario" runat="server" Width="21px" Height="20px" Text="..."></asp:Button></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="font-weight: normal; font-size: 10px; width: 74px; color: #000000; font-style: normal; font-family: Arial, Helvetica, sans-serif; height: 16px"
                                                                    align="right">Empresa&nbsp;</td>
                                                                <td style="width: 163px">
                                                                    <asp:DropDownList ID="cboEmpresa" runat="server" Width="148px" AutoPostBack="True"></asp:DropDownList></td>
                                                            </tr>
                                                        </table>
                                                        <br>
                                                        <table class="formulario_texto" id="Table1" style="width: 242px; height: 88px" cellspacing="0"
                                                            cellpadding="0" border="0">
                                                            <tr>
                                                                <td style="width: 287px; height: 23px" bordercolor="white" bgcolor="#99cc00"><font size="1"><strong>Dados 
																				Digitados</strong></font>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 287px; height: 3px"></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 287px; height: 19px">
                                                                    <asp:CheckBox ID="chkCarga" runat="server" Width="119px" Text="Carga"></asp:CheckBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 287px; height: 3px">
                                                                    <asp:CheckBox ID="chkIntercambio" runat="server" Width="121px" Text="Intercâmbio"></asp:CheckBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 287px; height: 3px">
                                                                    <asp:CheckBox ID="chkGeracao" runat="server" Width="121px" Text="Geração"></asp:CheckBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 498px; height: 25px" valign="middle" background="../pdpw/images/back_titulo.gif"
                                                                    colspan="3"></td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td style="width: 5px; height: 169px">&nbsp;<asp:ImageButton ID="btnEnviar" runat="server" ImageUrl="images/bt_Enviar.gif"></asp:ImageButton>&nbsp;&nbsp;<font size="1">
                                                    </font>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                        <div id="divCal" style="display: inline; left: 300px; width: 220px; position: absolute; top: 110px; height: 200px"
                                            runat="server" ms_positioning="FlowLayout">
                                            <asp:Calendar ID="calData" runat="server" Width="220px" Font-Size="8pt" Height="200px" Font-Bold="True"
                                                BackColor="Beige" ShowGridLines="True" BorderColor="Black" ForeColor="DarkBlue" BorderWidth="1px" Font-Names="Arial">
                                                <TodayDayStyle BackColor="YellowGreen"></TodayDayStyle>
                                                <SelectorStyle BackColor="YellowGreen"></SelectorStyle>
                                                <NextPrevStyle Font-Size="9pt" ForeColor="Black"></NextPrevStyle>
                                                <DayHeaderStyle Height="1px" BorderColor="Black" BackColor="PaleGoldenrod"></DayHeaderStyle>
                                                <SelectedDayStyle Font-Bold="True" BackColor="YellowGreen"></SelectedDayStyle>
                                                <TitleStyle Font-Size="9pt" Font-Bold="True" ForeColor="Black" BorderColor="#404040" BackColor="YellowGreen"></TitleStyle>
                                                <WeekendDayStyle BackColor="LemonChiffon"></WeekendDayStyle>
                                                <OtherMonthDayStyle ForeColor="Silver"></OtherMonthDayStyle>
                                            </asp:Calendar>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 1082px">
                                        <table style="width: 588px; height: 42px">
                                            <tr>
                                                <td valign="top" align="left" colspan="3">
                                                    <asp:Table ID="tblMensa" runat="server" Font-Size="XX-Small" Width="580px">
                                                        <asp:TableRow BackColor="#99CC00" Font-Size="XX-Small" Font-Bold="True">
                                                            <asp:TableCell Width="260px" Text="Descri&#231;&#227;o"></asp:TableCell>
                                                            <asp:TableCell Width="80px" Text="Situa&#231;&#227;o"></asp:TableCell>
                                                            <asp:TableCell Width="80px" Text="Carga"></asp:TableCell>
                                                            <asp:TableCell Width="80px" Text="Interc&#226;mbio"></asp:TableCell>
                                                            <asp:TableCell Width="80px" Text="Gera&#231;&#227;o"></asp:TableCell>
                                                        </asp:TableRow>
                                                    </asp:Table>
                                                </td>
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

