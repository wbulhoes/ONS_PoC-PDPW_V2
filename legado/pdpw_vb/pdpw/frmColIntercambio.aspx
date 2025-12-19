<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frmColIntercambio.aspx.vb" Inherits="pdpw.frmColIntercambio" %>

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
                vr = escape(document.frmColIntercambio.txtValor.value);
                vr = unescape(vr.replace('%0D%0A%0D%0A', '%0D%0A'));
                document.frmColIntercambio.txtValor.value = vr;
            }
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <p>
        <table style="width: 1100px; height: 266px" height="266" cellspacing="0" cellpadding="0"
            width="1100" border="0">
            <tbody>
                <tr>
                    <td style="width: 136px; height: 250px" valign="top" width="136">
                        <br>
                    </td>
                    <td style="width: 1064px; height: 250px" valign="top">
                        <div align="center">
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
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
                                                                <img height="23" src="images/tit_ColIntercambio.gif" width="152" style="width: 152px; height: 23px"></div>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                            <form id="frmColIntercambio" name="frmColIntercambio" runat="server">

                                <asp:Label ID="lblMsg" Visible="False" ForeColor="Green" runat="server" Font-Bold="True" Font-Size="X-Small">Desculpe, este dado não pôde ser carregado no momento.</asp:Label>

                                <br>

                                <table style="width: 1091px; height: 194px" cellspacing="0" cellpadding="0" border="0">
                                    <tr>
                                        <td style="width: 994px; height: 87px">
                                            <table style="width: 958px; height: 80px" height="80" cellspacing="0" cellpadding="0" width="958"
                                                border="0">
                                                <tr height="30">
                                                    <td style="width: 177px; height: 24px" align="right"><b>Data PDP:</b></td>
                                                    <td style="width: 865px; height: 24px">&nbsp;<asp:DropDownList ID="cboData" runat="server" Width="100px" AutoPostBack="True"></asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr valign="middle" height="30">
                                                    <td style="width: 177px; height: 24px" align="right"><b>Empresa:</b></td>
                                                    <td style="width: 865px; height: 24px">
                                                        <p>
                                                            &nbsp;<asp:DropDownList ID="cboEmpresa" runat="server" Width="219px" AutoPostBack="True"></asp:DropDownList>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
																<asp:RadioButton ID="optModalidade" runat="server" AutoPostBack="True" Checked="True" Text="Por Modalidade"></asp:RadioButton>&nbsp;&nbsp;
																<asp:RadioButton ID="optEmpresa" runat="server" AutoPostBack="True" Text="Por Empresa"></asp:RadioButton>
                                                        </p>
                                                    </td>
                                                </tr>
                                                <tr height="30">
                                                    <td style="width: 177px; height: 17px" align="right"><b>Intercâmbios:</b></td>
                                                    <td style="width: 865px; height: 17px">
                                                        <p>
                                                            &nbsp;<asp:DropDownList ID="cboIntercambio" runat="server" Width="219px" AutoPostBack="True"></asp:DropDownList>&nbsp;&nbsp;&nbsp;&nbsp;
																<asp:ImageButton ID="btnSalvar" runat="server" ImageUrl="images/bt_salvar.gif"></asp:ImageButton>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                        </p>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table style="width: 169px; height: 23px" cellspacing="0" cellpadding="0" border="0">
                                                <tr>
                                                    <td width="15">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    </td>
                                                    <td>
                                                        <br>
                                                        <asp:Table ID="tblIntercambio" runat="server" Width="70px" BorderStyle="Ridge" BorderWidth="1px"
                                                            CellPadding="2" CellSpacing="0" GridLines="Both" Height="22px" Font-Size="Smaller">
                                                        </asp:Table>
                                                        <div id="divValor" style="display: inline; left: 100px; width: 70px; position: absolute; top: 271px; height: 15px"
                                                            runat="server" ms_positioning="FlowLayout">
                                                        </div>
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
    </p>
</asp:Content>
