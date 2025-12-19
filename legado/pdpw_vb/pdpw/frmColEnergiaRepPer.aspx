<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frmColEnergiaRepPer.aspx.vb" Inherits="pdpw.frmColEnergiaRepPer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>ONS - Operador Nacional do Sistema Elétrico</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="../pdpw/images/style.css" rel="stylesheet">
    <script language="JavaScript">
        function RetiraEnter(teclapres) {
            var tecla = teclapres.keyCode;
            if (tecla == 13) {
                // Retira Caracteres especiais (ENTER, TAB, etc...) quando existirem 2 seguidos em qualquer lugar do texto
                vr = escape(document.frmColEnergiaRepPer.txtValor.value);
                //TAB + ENTER
                vr = vr.replace('%09%0D%0A', '%09')
                //ENTER + TAB
                vr = vr.replace('%0D%0A%09', '%09')
                //ENTER + ENTER
                vr = unescape(vr.replace('%0D%0A%0D%0A', '%0D%0A'));
                document.frmColEnergiaRepPer.txtValor.value = vr;
            }
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table style="width: 1098px; height: 112px" height="112" cellspacing="0" cellpadding="0"
        width="1098" border="0">
        <tr>
            <td style="width: 27px; height: 250px" valign="top" width="27">
                <br>
            </td>
            <td style="width: 1064px; height: 250px" valign="top">
                <div align="center">
                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                        <tr>
                            <td width="20%" height="2">&nbsp;</td>
                        </tr>
                        <tr>
                            <td style="height: 17px" height="17">
                                <table cellspacing="0" cellpadding="0" width="765" background="../pdpw/images/back_tit_sistema.gif"
                                    border="0">
                                    <tr>
                                        <td style="height: 12px">
                                            <img height="25" src="../pdpw/images/tit_sis_guideline.gif" width="179"></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 10px" height="10">
                                <table cellspacing="0" cellpadding="0" width="765" background="../pdpw/images/back_titulo.gif"
                                    border="0">
                                    <tr>
                                        <td style="height: 8px">
                                            <div align="left">
                                                <img style="width: 272px; height: 23px" height="23" src="images/tit_ColErp.gif"
                                                    width="272">
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <br>
                    <form id="frmColEnergiaRepPer" name="frmColEnergiaRepPer" runat="server">
                        <table style="width: 1078px; height: 64px" height="64" cellspacing="0" cellpadding="0"
                            width="1078" border="0">
                            <tr height="30">
                                <td style="width: 170px; height: 2px" align="right"><b>Data PDP:</b></td>
                                <td style="width: 257px; height: 2px">&nbsp;<asp:DropDownList ID="cboData" runat="server" AutoPostBack="True" Width="98px"></asp:DropDownList></td>
                            </tr>
                            <tr height="30">
                                <td style="width: 170px; height: 21px" align="right"><b>Empresa:</b></td>
                                <td style="width: 750px; height: 21px">&nbsp;<asp:DropDownList ID="cboEmpresa" runat="server" AutoPostBack="True" Width="219px"></asp:DropDownList></td>
                            </tr>
                            <tr height="30">
                                <td style="width: 170px; height: 24px" align="right"><b>Usinas:</b></td>
                                <td style="width: 881px; height: 24px">&nbsp;<asp:DropDownList ID="cboUsina" runat="server" AutoPostBack="True" Width="219px"></asp:DropDownList>&nbsp;&nbsp;&nbsp;
										<asp:ImageButton ID="btnSalvar" runat="server" ImageUrl="images/bt_salvar.gif"></asp:ImageButton></td>
                            </tr>
                        </table>
                        <div style="left: 34px; width: 1027px; position: absolute; top: 220px; height: 8px">
                            <table style="width: 1026px; height: 51px">
                                <tr>
                                    <td style="width: 750px" valign="top">
                                        <asp:Table ID="tblErp" runat="server" Width="51px" Font-Size="Smaller" Height="26px" GridLines="Both"
                                            CellSpacing="0" CellPadding="2" BorderWidth="1px" BorderStyle="Ridge">
                                        </asp:Table>
                                        <div id="divValor" style="display: inline; left: 180px; width: 82px; position: absolute; top: 270px; height: 21px"
                                            runat="server" ms_positioning="FlowLayout">
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </form>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>

