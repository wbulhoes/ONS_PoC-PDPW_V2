<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frmColCarga.aspx.vb" Inherits="pdpw.frmColCarga" %>

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
            vr = escape(document.frmColCarga.txtValor.value);
            //alert(vr);
            if (tecla == 13) {
                // Retira ENTER quando existirem 2 seguidos em qualquer lugar do texto
                vr = unescape(vr.replace('%0D%0A%0D%0A', '%0D%0A'));
                document.frmColCarga.txtValor.value = vr;
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
                        <form id="frmColCarga" name="frmColCarga" runat="server">

                            <asp:Label ID="lblMsg" Visible="False" ForeColor="Green" runat="server" Font-Bold="True" Font-Size="X-Small">Desculpe, este dado não pôde ser carregado no momento.</asp:Label>

                            <br>
                            <table height="100%" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr height="30">
                                    <td style="width: 180px; height: 42px" align="right"><b>Data PDP:</b></td>
                                    <td style="width: 257px; height: 42px">&nbsp;<asp:DropDownList ID="cboData" runat="server" AutoPostBack="True" Width="98px"></asp:DropDownList></td>
                                    <td style="height: 42px" valign="middle">&nbsp;</td>
                                </tr>
                                <tr height="30">
                                    <td style="width: 180px; height: 42px" align="right">
                                        <p><b>Empresa:</b></p>
                                    </td>
                                    <td style="width: 231px; height: 42px">
                                        <p>
                                            &nbsp;<asp:DropDownList ID="cboEmpresa" runat="server" AutoPostBack="True" Width="219px"></asp:DropDownList>
                                        </p>
                                    </td>
                                    <td style="height: 42px" valign="middle">
                                        <p>
                                            <asp:ImageButton ID="btnSalvar" runat="server" Visible="False" ImageUrl="images/bt_salvar.gif"></asp:ImageButton><asp:ImageButton ID="btnAlterar" runat="server" Visible="False" ImageUrl="images/bt_alterar.gif"></asp:ImageButton></p>
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
                                    <td style="width: 235px" valign="top" colspan="2">
                                        <p>
                                            &nbsp;<asp:Table ID="tblCarga" runat="server" Width="235px" BorderStyle="Ridge" BorderWidth="1px"
                                                CellPadding="2" CellSpacing="0" GridLines="Both" Height="260px" Font-Size="Smaller">
                                                <asp:TableRow HorizontalAlign="Center" BackColor="#EAECD4" Height="10px">
                                                    <asp:TableCell Width="100px" Font-Bold="True" Text="Hora"></asp:TableCell>
                                                    <asp:TableCell Width="135px" Font-Bold="True" Text="Valor"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="00:00 - 00:30"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="00:30 - 01:00"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="01:00 - 01:30"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="01:30 - 02:00"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="02:00 - 02:30"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="02:30 - 03:00"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="03:00 - 03:30"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="03:30 - 04:00"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="04:00 - 04:30"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="04:30 - 05:00"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="05:00 - 05:30"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="05:30 - 06:00"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="06:00 - 06:30"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="06:30 - 07:00"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="07:00 - 07:30"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="07:30 - 08:00"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="08:00 - 08:30"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="08:30 - 09:00"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="09:00 - 09:30"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="09:30 - 10:00"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="10:00 - 10:30"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="10:30 - 11:00"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="11:00 - 11:30"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="11:30 - 12:00"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="12:00 - 12:30"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="12:30 - 13:00"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="13:00 - 13:30"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="13:30 - 14:00"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="14:00 - 14:30"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="14:30 - 15:00"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="15:00 - 15:30"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="15:30 - 16:00"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="16:00 - 16:30"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="16:30 - 17:00"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="17:00 - 17:30"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="17:30 - 18:00"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="18:00 - 18:30"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="18:30 - 19:00"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="19:00 - 19:30"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="19:30 - 20:00"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="20:00 - 20:30"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="20:30 - 21:00"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="21:00 - 21:30"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="21:30 - 22:00"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="22:00 - 22:30"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="22:30 - 23:00"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="23:00 - 23:30"></asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Right"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell BackColor="#EAECD4" Font-Bold="True" HorizontalAlign="Center" Text="23:30 - 23:59"></asp:TableCell>
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

