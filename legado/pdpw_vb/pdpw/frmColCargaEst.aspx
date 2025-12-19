<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frmColCargaEst.aspx.vb" Inherits="pdpw.frmColCargaEst" %>

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
            vr = escape(document.frmColCargaEst.txtValor.value);
            //alert(vr);
            if (tecla == 13) {
                // Retira ENTER quando existirem 2 seguidos em qualquer lugar do texto
                vr = unescape(vr.replace('%0D%0A%0D%0A', '%0D%0A'));
                document.frmColCargaEst.txtValor.value = vr;
            }
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table height="159" cellspacing="0" cellpadding="0" width="780" border="0">
        <tbody>
            <tr>
                <td style="width: 14px; height: 1242px" valign="top" width="14">
                    <br>
                </td>
                <td style="height: 1242px" valign="top">
                    <div align="center">
                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                            <tbody>
                                <tr>
                                    <td width="20%" height="2">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td style="height: 23px" height="23">
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
                                                            <img style="width: 88px; height: 23px" height="23" src="../pdpw/images/tit_ColCarga.gif"
                                                                width="88">
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
                        <form id="frmColCargaEst" name="frmColCargaEst" runat="server">
                            <table height="100%" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr height="30">
                                    <td style="font-weight: normal; font-size: 10px; color: #000000; font-style: normal; font-family: Arial, Helvetica, sans-serif; height: 23px"
                                        align="right"><b>Data PDP:</b></td>
                                    <td style="width: 257px; height: 23px">&nbsp;<asp:TextBox ID="txtData" runat="server" Height="20px" Width="79px" Font-Size="X-Small"></asp:TextBox><asp:Button ID="btnCalendario" runat="server" Height="20px" Width="21px" Text="..."></asp:Button></td>
                                    <td style="height: 23px" valign="middle">
                                        <div id="divCal" style="display: inline; z-index: 5; left: 330px; width: 220px; position: absolute; top: 100px; height: 200px"
                                            runat="server" ms_positioning="FlowLayout">
                                            <asp:Calendar ID="calData" runat="server" Height="200px" Width="220px" Font-Size="8pt" Font-Names="Arial"
                                                BorderWidth="1px" ForeColor="DarkBlue" BorderColor="Black" ShowGridLines="True" BackColor="Beige" Font-Bold="True">
                                                <TodayDayStyle BackColor="YellowGreen"></TodayDayStyle>
                                                <SelectorStyle BackColor="YellowGreen"></SelectorStyle>
                                                <NextPrevStyle Font-Size="9pt" ForeColor="Black"></NextPrevStyle>
                                                <DayHeaderStyle Height="1px" BorderColor="Black" BackColor="PaleGoldenrod"></DayHeaderStyle>
                                                <SelectedDayStyle Font-Bold="True" BackColor="GreenYellow"></SelectedDayStyle>
                                                <TitleStyle Font-Size="9pt" Font-Bold="True" ForeColor="Black" BorderColor="#404040" BackColor="YellowGreen"></TitleStyle>
                                                <WeekendDayStyle BackColor="LemonChiffon"></WeekendDayStyle>
                                                <OtherMonthDayStyle ForeColor="Silver"></OtherMonthDayStyle>
                                            </asp:Calendar>
                                        </div>
                                    </td>
                                </tr>
                                <tr height="30">
                                    <td style="font-size: xx-small; width: 180px; font-family: Arial; height: 30px" align="right"><b>Empresa:</b>
                                    </td>
                                    <td style="width: 231px; height: 30px">&nbsp;<asp:DropDownList ID="cboEmpresa" runat="server" Height="20px" Width="219px" Font-Size="X-Small" AutoPostBack="True"></asp:DropDownList>
                                    </td>
                                    <td style="height: 30px" valign="middle">
                                        <p>
                                            <asp:ImageButton ID="btnSalvar" runat="server" ImageUrl="images/bt_salvar.gif"></asp:ImageButton><asp:ImageButton ID="btnAlterar" runat="server" ImageUrl="images/bt_alterar.gif"></asp:ImageButton><asp:ImageButton ID="btnIncluir" runat="server" ImageUrl="images/bt_incluir.gif" Visible="False"></asp:ImageButton></p>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 180px" valign="top" align="right" width="180">
                                        <p>&nbsp;</p>
                                        <div id="divValor" style="display: inline; left: 245px; width: 90px; position: absolute; top: 228px; height: 21px"
                                            runat="server" ms_positioning="FlowLayout">
                                        </div>
                                        <p>&nbsp;</p>
                                    </td>
                                    <td style="width: 231px" valign="top" colspan="2">
                                        <p>
                                            &nbsp;<asp:Table ID="tblCarga" runat="server" Height="260px" Width="120px" Font-Size="Smaller" BorderWidth="1px"
                                                GridLines="Both" CellSpacing="0" CellPadding="2" BorderStyle="Ridge">
                                                <asp:TableRow HorizontalAlign="Center" BackColor="#EAECD4" Height="10px">
                                                    <asp:TableCell Width="65px" Font-Bold="True" Text="Hora"></asp:TableCell>
                                                    <asp:TableCell Width="135px" Font-Bold="True" Text="Valor"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="00:30"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="01:00"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="01:30"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="02:00"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="02:30"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="03:00"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="03:30"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="04:00"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="04:30"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="05:00"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="05:30"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="06:00"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="06:30"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="07:00"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="07:30"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="08:00"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="08:30"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="09:00"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="09:30"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="10:00"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="10:30"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="11:00"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="11:30"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="12:00"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="12:30"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="13:00"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="13:30"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="14:00"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="14:30"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="15:00"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="15:30"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="16:00"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="16:30"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="17:00"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="17:30"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="18:00"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="18:30"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="19:00"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="19:30"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="20:00"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="20:30"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="21:00"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="21:30"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="22:00"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="22:30"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="23:00"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="23:30"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="24:00"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="Total"></asp:TableCell>
                                                    <asp:TableCell BackColor="#EAECD4" HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="M&#233;dia"></asp:TableCell>
                                                    <asp:TableCell BackColor="#EAECD4" HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                            </asp:Table>
                                        </p>
                                        <div id="divMsg" style="display: inline; left: 195px; width: 216px; position: absolute; top: 135px; height: 24px"
                                            ms_positioning="FlowLayout">
                                            <asp:Label ID="lblMsg" runat="server" Width="239px" Font-Size="X-Small" Font-Names="Arial"
                                                ForeColor="Red" Font-Bold="True" Visible="False">Valores já enviados para estudo.</asp:Label>
                                        </div>
                                        <p></p>
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

