<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="OldfrmColMaqOperando.aspx.vb" Inherits="pdpw.frmColMaqOperando" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>ONS - Operador Nacional do Sistema Elétrico</title>
    <meta http-equiv="Content-Type" content="text/html; charset=windows-1252">
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <link href="../pdpw/images/style.css" rel="stylesheet">
    <script language="JavaScript">
        function RetiraEnter(teclapres) {
            var tecla = teclapres.keyCode;
            if (tecla == 13) {
                // Retira Caracteres especiais (ENTER, TAB, etc...) quando existirem 2 seguidos em qualquer lugar do texto
                vr = escape(document.frmColMaqOperando.txtValor.value);
                //TAB + ENTER
                vr = vr.replace('%09%0D%0A', '%09')
                //ENTER + TAB
                vr = vr.replace('%0D%0A%09', '%09')
                //ENTER + ENTER
                vr = unescape(vr.replace('%0D%0A%0D%0A', '%0D%0A'));
                document.frmColMaqOperando.txtValor.value = vr;
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
                    <td style="width: 28px; height: 250px" valign="top" width="28">
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
                                                                <img style="width: 368px; height: 23px" height="23" src="../pdpw/images/tit_ColMaqOperando.gif"
                                                                    width="368">
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
                            <form id="frmColMaqOperando" name="frmColMaqOperando" runat="server">
                                <table style="width: 1078px; height: 124px" height="124" cellspacing="0" cellpadding="0"
                                    width="1078" border="0">
                                    <tr height="30">
                                        <td style="width: 170px; height: 22px" align="right"><b>Data PDP:</b></td>
                                        <td style="width: 257px; height: 22px">&nbsp;<asp:DropDownList ID="cboData" runat="server" Width="98px" AutoPostBack="True"></asp:DropDownList></td>
                                    </tr>
                                    <tr height="30">
                                        <td style="width: 170px; height: 39px" align="right"><b>Empresa:</b></td>
                                        <td style="width: 750px; height: 39px">
                                            <p>&nbsp;<asp:DropDownList ID="cboEmpresa" runat="server" Width="219px" AutoPostBack="True"></asp:DropDownList>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</p>
                                        </td>
                                    </tr>
                                    <tr height="30">
                                        <td style="width: 170px; height: 28px" align="right"><b>Usinas:</b></td>
                                        <td style="width: 881px; height: 28px">
                                            <p>
                                                &nbsp;<asp:DropDownList ID="cboUsina" runat="server" Width="219px" AutoPostBack="True"></asp:DropDownList>&nbsp;&nbsp;&nbsp;
													<asp:ImageButton ID="btnSalvar" runat="server" ImageUrl="images/bt_salvar.gif"></asp:ImageButton>
                                            </p>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 10px" valign="top" align="right">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        </td>
                                        <td style="width: 750px" valign="top">
                                            <div id="divValor" style="display: inline; z-index: 200; left: 180px; width: 82px; position: absolute; top: 270px; height: 21px"
                                                runat="server" ms_positioning="FlowLayout">
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <div style="display: inline; z-index: 1; left: 20px; width: 141px; position: absolute; top: 230px; height: 45px"
                                                ms_positioning="FlowLayout">
                                                <asp:Table ID="tblMaqOperando" runat="server" Width="47px" BorderStyle="Ridge" BorderWidth="1px"
                                                    CellPadding="2" CellSpacing="0" GridLines="Both" Height="26px" Font-Size="Smaller">
                                                </asp:Table>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </form>
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>
        <p></p>
</asp:Content>
