<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frmColPotSinc.aspx.vb" Inherits="pdpw.frmColPotSinc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>ONS - Operador Nacional do Sistema Elétrico</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="images/style.css" rel="stylesheet">
    <script language="JavaScript">
        function RetiraEnter(teclapres) {
            var tecla = teclapres.keyCode;
            if (tecla == 13) {
                // Retira Caracteres especiais (ENTER, TAB, etc...) quando existirem 2 seguidos em qualquer lugar do texto
                vr = escape(document.frmColGeracao.txtValor.value);
                //TAB + ENTER
                vr = vr.replace('%09%0D%0A', '%09')
                //ENTER + TAB
                vr = vr.replace('%0D%0A%09', '%09')
                //ENTER + ENTER
                vr = unescape(vr.replace('%0D%0A%0D%0A', '%0D%0A'));
                document.frmColGeracao.txtValor.value = vr;
            }
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table style="width: 504px; height: 363px" height="363" cellspacing="0" cellpadding="0"
        width="504" border="0">
        <tbody>
            <tr>
                <td style="width: 41px; height: 157px" valign="top" width="41">
                    <br>
                </td>
                <td style="width: 693px; height: 157px" valign="top">
                    <div align="center">
                        <table style="width: 503px; height: 105px" cellspacing="0" cellpadding="0" width="503"
                            border="0">
                            <tbody>
                                <tr>
                                    <td style="width: 1063px; height: 18px" width="1063" height="18">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td style="width: 1063px" height="2">
                                        <table style="width: 510px; height: 25px" cellspacing="0" cellpadding="0" width="510" background="images/back_tit_sistema.gif"
                                            border="0">
                                            <tbody>
                                                <tr>
                                                    <td>
                                                        <img style="width: 179px; height: 25px" height="25" src="images/tit_sis_guideline.gif"
                                                            width="179"></td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 1063px; height: 26px" height="26">
                                        <table style="width: 510px; height: 23px" cellspacing="0" cellpadding="0" width="510" background="images/back_titulo.gif"
                                            border="0">
                                            <tbody>
                                                <tr>
                                                    <td>
                                                        <div align="left">
                                                            <img style="width: 200px; height: 23px" height="23" src="images/tit_ColPotSinc.gif" width="200"></div>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <form id="frmColPotSinc" name="frmPotSinc" method="post" enctype="multipart/form-data"
                            runat="server">
                            <table class="modulo" style="width: 505px; height: 29px" cellspacing="0" cellpadding="0"
                                border="0">
                                <tr>
                                    <td style="width: 62px; height: 29px" align="right">Data:</td>
                                    <td style="width: 125px; height: 29px">&nbsp;
											<asp:DropDownList ID="cboData" runat="server" AutoPostBack="True" Width="98px"></asp:DropDownList></td>
                                    <td style="width: 87px">
                                        <asp:ImageButton ID="btnSalvar" runat="server" ImageUrl="images/bt_salvar.gif" Visible="False"></asp:ImageButton>
                                    </td>
                                    <td>
                                        <img onmouseover="this.style.cursor='hand'" onclick="javascript:window.close();" alt=""
                                            src="images\bt_fechar.gif">
                                    </td>
                                </tr>
                            </table>
                            <div id="divValor" style="display: inline; left: 120px; width: 82px; position: absolute; top: 200px; height: 21px"
                                runat="server" ms_positioning="FlowLayout">
                            </div>
                        </form>
                    </div>
                    <tr>
                        <td style="width: 41px" valign="top" align="right">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                        <td style="width: 693px" valign="top">
                            <br>
                            <p>
                                <asp:Table ID="tblPotSinc" runat="server" Width="51px" BorderStyle="Ridge" BorderWidth="1px"
                                    CellPadding="2" GridLines="Both" CellSpacing="0" Font-Size="Smaller" Height="26px">
                                </asp:Table>
                            </p>
                        </td>
                    </tr>
                </td>
            </tr>
        </tbody>
    </table>
</asp:Content>

