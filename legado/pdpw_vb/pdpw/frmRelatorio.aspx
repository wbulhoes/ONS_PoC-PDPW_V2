<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frmRelatorio.aspx.vb" Inherits="pdpw.frmRelatorio" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>ONS - Operador Nacional do Sistema Elétrico</title>
    <meta http-equiv="Content-Type" content="text/html; charset=windows-1252">
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="../pdpw/images/style.css" rel="stylesheet">
    <script>
        function PotSinc() {
            window.open('frmColPotSinc.aspx', '', 'left=300,top=50,height=700,width=550,status=no,scrollbars=yes,titlebar=no,menubar=no');
        }
        function Requisito() {
            window.open('frmCadRequisito.aspx?datPDP=' + frmArquivo.cboData.value, '', 'left=300,top=50,height=400,width=550,status=no,scrollbars=no,titlebar=no,menubar=no');
        }
        function ValidaData(source, arguments) {
            if (arguments.Value != 0)
                arguments.IsValid = true;
            else
                arguments.IsValid = false;
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table style="width: 788px; height: 418px" height="418" cellspacing="0" cellpadding="0"
        width="788" border="0">
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
                                        <table cellspacing="0" cellpadding="0" width="765" background="../pdpw/images/back_tit_sistema.gif"
                                            border="0">
                                            <tbody>
                                                <tr>
                                                    <td style="height: 12px">
                                                        <img height="25" src="../pdpw/images/tit_sis_guideline.gif" width="179"></td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 10px" height="10">
                                        <table cellspacing="0" cellpadding="0" width="765" background="../pdpw/images/back_titulo.gif"
                                            border="0">
                                            <tbody>
                                                <tr>
                                                    <td style="height: 8px">
                                                        <div align="left">
                                                            <img style="width: <%=iif(Trim(Request.Querystring("strRegional"))="S","230",iif(Trim(Request.Querystring("strRegional"))="SE","250",iif(Trim(Request.Querystring("strRegional"))="NE","250",iif(Trim(Request.Querystring("strRegional"))="CNOS" or Trim(Request.Querystring("strRegional"))="NCO","350","128"))))%>px; height: 23px" src="images/tit_Relatorio<%=Request.Querystring("strRegional")%>.gif"></div>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <form id="frmArquivo" name="frmArquivo" runat="server">
                            <table class="modulo" style="width: 727px; height: 68px" cellspacing="0" cellpadding="0"
                                border="0">
                                <tr>
                                    <td style="width: 64px; height: 33px" align="right">Relatório:
                                    </td>
                                    <td style="width: 450px; height: 33px" colspan="3">&nbsp;
											<asp:DropDownList ID="cboRelatorio" runat="server" Width="434px">
                                                <asp:ListItem Value="0">Selecione um Relat&#243;rio</asp:ListItem>
                                            </asp:DropDownList></td>
                                    <td style="width: 77px" align="center" rowspan="2">
                                        <asp:ImageButton ID="btnPesquisar" runat="server" ImageAlign="Top" ImageUrl="images/bt_visualizar.gif"></asp:ImageButton></td>
                                    <td align="center" rowspan="2">
                                        <img id="btnPotSinc" onmouseover="this.style.cursor='hand'" onclick="javascript:PotSinc();"
                                            alt="Cadastro de Potência Sincronizada - COSR-NE" src="images/bt_PotSinc.gif" runat="server"></td>
                                    <td align="center" rowspan="2">
                                        <img id="btnRequisito" onmouseover="this.style.cursor='hand'" onclick="javascript:Requisito();"
                                            alt="Cadastro de Requisito e Reserva - COSR-NE" src="images/bt_Requisito.gif" runat="server" height="25" width="70"></td>
                                </tr>
                                <tr>
                                    <td style="width: 64px" align="right">Data PDP:</td>
                                    <td style="width: 174px">&nbsp;
											<asp:DropDownList ID="cboData" runat="server" Width="92px"></asp:DropDownList><asp:CustomValidator ID="cvData" runat="server" ErrorMessage="Data Requerida" ControlToValidate="cboData"
                                                ClientValidationFunction="ValidaData">*</asp:CustomValidator></td>
                                    <td style="width: 8px">
                                        <asp:Label ID="lblAgrega" runat="server" Visible="False">Agregação:</asp:Label></td>
                                    <td style="width: 212px">
                                        <asp:DropDownList ID="cboAgrega" runat="server" Width="203px" Visible="False"></asp:DropDownList></td>
                                </tr>
                            </table>
                            <p></p>
                            <p></p>
                            <p>&nbsp;</p>
                            <p>
                                <asp:ValidationSummary ID="ValidationSummary1" runat="server" Width="467px" Height="40px" DisplayMode="List"
                                    ShowMessageBox="True" ShowSummary="False"></asp:ValidationSummary>
                            </p>
                            <p>&nbsp;</p>
                        </form>
                    </div>
                </td>
            </tr>
        </tbody>
    </table>
</asp:Content>
