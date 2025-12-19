<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frmColIntercambioEst.aspx.vb" Inherits="pdpw.frmColIntercambioEst" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>ONS - Operador Nacional do Sistema Elétrico</title>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <meta http-equiv="Content-Type" content="text/html; charset=windows-1252">
    <link href="../pdpw/images/style.css" rel="stylesheet">
    <script language="JavaScript">
        function RetiraEnter(teclapres) {
            var tecla = teclapres.keyCode;
            if (tecla == 13) {
                // Retira ENTER quando existirem 2 seguidos em qualquer lugar do texto
                vr = escape(document.frmColIntercambioEst.txtValor.value);
                vr = unescape(vr.replace('%0D%0A%0D%0A', '%0D%0A'));
                document.frmColIntercambioEst.txtValor.value = vr;
            }
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <p>
        <table style="width: 904px; height: 318px" height="318" cellspacing="0" cellpadding="0"
            width="904" border="0">
            <tbody>
                <tr>
                    <td style="width: 4px; height: 250px" valign="top" width="4">
                        <br>
                    </td>
                    <td style="width: 1064px; height: 250px" valign="top">
                        <div align="center">
                            <table style="width: 898px; height: 105px" cellspacing="0" cellpadding="0" width="898"
                                border="0">
                                <tbody>
                                    <tr>
                                        <td width="20%" height="2">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td height="2">
                                            <table cellspacing="0" cellpadding="0" width="765" background="../pdpw/images/back_tit_sistema.gif"
                                                border="0">
                                                <tbody>
                                                    <tr>
                                                        <td>
                                                            <img height="25" src="images/tit_sis_guideline.gif" width="179"></td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td height="2">
                                            <table cellspacing="0" cellpadding="0" width="765" background="images/back_titulo.gif"
                                                border="0">
                                                <tbody>
                                                    <tr>
                                                        <td>
                                                            <div align="left">
                                                                <img style="width: 152px; height: 23px" height="23" src="images/tit_ColIntercambio.gif"
                                                                    width="152">
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
                            <form id="frmColIntercambioEst" name="frmColIntercambioEst" runat="server">
                                <table style="width: 849px; height: 194px" cellspacing="0" cellpadding="0" border="0">
                                    <tr>
                                        <td style="width: 994px; height: 87px">
                                            <table style="width: 487px; height: 74px" cellspacing="0" cellpadding="0" border="0">
                                                <tr>
                                                    <td style="font-size: xx-small; width: 143px; font-family: Arial" align="right"><b>Data 
																PDP:</b></td>
                                                    <td style="width: 250px">&nbsp;
															<asp:TextBox ID="txtData" runat="server" Width="79px" Font-Size="X-Small" Height="20px"></asp:TextBox><asp:Button ID="btnCalendario" runat="server" Width="21px" Height="20px" Text="..."></asp:Button></td>
                                                    <td style="width: 80px" rowspan="3">
                                                        <asp:ImageButton ID="btnSalvar" runat="server" ImageUrl="images/bt_salvar.gif"></asp:ImageButton></td>
                                                </tr>
                                                <tr valign="middle" height="30">
                                                    <td style="font-size: xx-small; width: 143px; font-family: Arial" align="right"><b>Empresa:</b></td>
                                                    <td style="width: 250px">&nbsp;
															<asp:DropDownList ID="cboEmpresa" runat="server" Width="219px" AutoPostBack="True"></asp:DropDownList></td>
                                                </tr>
                                                <tr>
                                                    <td style="font-size: xx-small; width: 143px; font-family: Arial" align="right"><b>Intercâmbios:</b></td>
                                                    <td style="width: 250px">&nbsp;
															<asp:DropDownList ID="cboIntercambio" runat="server" Width="219px" AutoPostBack="True"></asp:DropDownList></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table style="width: 157px; height: 60px" cellspacing="0" cellpadding="0" border="0">
                                                <tr>
                                                    <td width="15">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    </td>
                                                    <td>
                                                        <br>
                                                        <asp:Table ID="tblIntercambio" runat="server" Width="60px" Font-Size="Smaller" Height="22px"
                                                            BorderStyle="Ridge" BorderWidth="1px" CellPadding="2" CellSpacing="0" GridLines="Both">
                                                        </asp:Table>
                                                        <div id="divValor" style="display: inline; z-index: 100; left: 100px; width: 61px; position: absolute; top: 281px; height: 19px"
                                                            runat="server" ms_positioning="FlowLayout">
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                <div id="divCal" style="display: inline; z-index: 101; left: 280px; width: 220px; position: absolute; top: 120px; height: 200px"
                                    runat="server" ms_positioning="FlowLayout">
                                    <asp:Calendar ID="calData" runat="server" Width="220px" Font-Size="8pt" Height="200px" BorderWidth="1px"
                                        Font-Names="Arial" BorderColor="Black" ShowGridLines="True" BackColor="Beige" Font-Bold="True" ForeColor="DarkBlue">
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
                            </form>
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>
    </p>
    <div id="divMsg" style="display: inline; left: 190px; width: 216px; position: absolute; top: 210px; height: 24px"
        ms_positioning="FlowLayout">
        <asp:Label ID="lblMsg" runat="server" Width="239px" Font-Size="X-Small" Font-Names="Arial"
            Font-Bold="True" ForeColor="Red" Visible="False">Valores já enviados para estudo.</asp:Label>
    </div>
</asp:Content>

