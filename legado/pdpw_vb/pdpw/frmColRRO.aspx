<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frmColRRO.aspx.vb" Inherits="pdpw.frmColRRO" %>

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
    <script language="javascript" type="text/javascript" src="js/MSGAguarde.js"></script>
    <link href="css/MSGAguarde.css" rel="stylesheet" />
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
                                                                <img height="23" src="../pdpw/images/tit_ColRRO.gif" width="96" style="width: 96px; height: 23px"></div>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                            <br>
                            <form id="frmColGeracao" name="frmColGeracao" runat="server">
                                <table style="width: 1078px; height: 124px" height="124" cellspacing="0" cellpadding="0"
                                    width="1078" border="0">
                                    <tr height="30">
                                        <td style="width: 170px; height: 22px" align="right"><b>Data PDP:</b></td>
                                        <td style="width: 257px; height: 22px">&nbsp;<asp:DropDownList ID="cboData" runat="server" Width="98px" AutoPostBack="True"></asp:DropDownList></td>
                                    </tr>
                                    <tr height="30">
                                        <td style="width: 170px; height: 21px" align="right"><b>Empresa:</b></td>
                                        <td style="width: 750px; height: 21px">
                                            <p>
                                                &nbsp;<asp:DropDownList ID="cboEmpresa" runat="server" Width="219px" AutoPostBack="True"></asp:DropDownList>
                                            &nbsp;&nbsp;&nbsp;
                                        </td>
                                    </tr>
                                    <tr height="30">
                                        <td style="width: 170px; height: 2px" align="right"><b>Usinas:</b></td>
                                        <td style="width: 881px; height: 2px">
                                            <p>
                                                &nbsp;<asp:DropDownList ID="cboUsina" runat="server" Width="219px" AutoPostBack="True"></asp:DropDownList>&nbsp;&nbsp;&nbsp;
													<asp:ImageButton ID="btnSalvar" runat="server" ImageUrl="images/bt_salvar.gif"></asp:ImageButton>
                                            </p>
                                        </td>
                                    </tr>
                                </table>
                        </div>
                        <tr>
                            <td style="width: 10px" valign="top" align="right">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            </td>
                            <td style="width: 750px" valign="top">
                                <asp:Table ID="tblRRO" runat="server" Width="51px" BorderStyle="Ridge" BorderWidth="1px"
                                    CellPadding="2" CellSpacing="0" GridLines="Both" Height="26px" Font-Size="Smaller">
                                </asp:Table>
                                <div id="divValor" style="display: inline; left: 180px; width: 82px; position: absolute; top: 270px; height: 21px"
                                    runat="server" ms_positioning="FlowLayout">
                                </div>
                            </td>
                        </tr>
            </tbody>
        </table>
        </FORM>
			<div></div>
        </TD></TR></TBODY></TABLE>
		<p></p>
</asp:Content>
