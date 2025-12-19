<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frmControleAgenteCad.aspx.vb" Inherits="pdpw.frmControleAgenteCad" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>ONS - Operador Nacional do Sistema Elétrico</title>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <meta http-equiv="Content-Type" content="text/html; charset=windows-1252">
    <link href="../pdpw/images/style.css" rel="stylesheet">
    <script language="javascript">

        function Mascara_Data(CampoData) {
            var data = CampoData.value;

            if (data.length == 2) {
                data = data + '/';
                CampoData.value = data;
            }
            if (data.length == 5) {
                data = data + '/';
                CampoData.value = data;
            }
        }

        function Mascara_Hora(CampoHora) {
            var hora01 = '';
            hora01 = hora01 + CampoHora.value;
            if (hora01.length == 2) {
                hora01 = hora01 + ':';
                CampoHora.value = hora01;
            }
        }

        function confirmaOperacao() {
            if (frmColRestricaoUS.cboEmpresa.value == "Todas") {
                if (confirm("As datas limite de todas as empresas serão alteradas. Confirma?"))
                    return true;
                else
                    return false;
            }
            else {
                return true;
            }
        }

    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table style="width: 831px; height: 486px" cellspacing="0" cellpadding="0" border="0">
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
                                    <td style="height: 24px" height="24">
                                        <table style="width: 764px; height: 23px" cellspacing="0" cellpadding="0" width="764" background="../pdpw/images/back_titulo.gif"
                                            border="0">
                                            <tbody>
                                                <tr>
                                                    <td style="width: 762px; height: 8px">
                                                        <div align="left">
                                                            <img height="23" src="../pdpw/images/tit_ControleAgentePDP.gif" width="184" style="width: 184px; height: 23px"></div>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <br>
                        <form id="frmColRestricaoUS" name="frmColRestricaoUS" runat="server">
                            <table class="modulo" style="width: 726px; height: 141px" cellspacing="0" cellpadding="0"
                                border="0">
                                <tr align="left">
                                    <td style="width: 700px; height: 30px" colspan="3">
                                        <table class="modulo" style="width: 775px; height: 58px">
                                            <tr>
                                                <td style="width: 100px; height: 30px" colspan="3">&nbsp;Data PDP:&nbsp;<asp:TextBox
                                                    ID="txtDataPDP" runat="server" Height="21px" Width="89px" MaxLength="10"
                                                    Enabled="False"></asp:TextBox>
                                                </td>
                                                <td style="width: 300px; height: 30px" colspan="3">&nbsp;Empresa:&nbsp;<asp:DropDownList
                                                    ID="cboEmpresa" runat="server" Width="303px" Height="22px"
                                                    AutoPostBack="True">
                                                </asp:DropDownList>
                                                </td>
                                                <td style="width: 300px; height: 30px" colspan="3">&nbsp;</td>
                                            </tr>

                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 700px; height: 31px" colspan="3">
                                        <table class="modulo" style="width: 495px; height: 65px">
                                            <tr>
                                                <td colspan="2">Limites</td>
                                            </tr>
                                            <tr>
                                                <td style="width: 254px">De:</td>
                                                <td>Até:</td>
                                            </tr>
                                            <tr>
                                                <td>Data:&nbsp;
														<asp:TextBox ID="txtDataInicial" onkeyup="Mascara_Data(this)" runat="server" Height="21px" Width="91px" MaxLength="10"></asp:TextBox>&nbsp;&nbsp; 
														Hora:&nbsp;
														<asp:TextBox ID="txtHoraInicial" onkeyup="Mascara_Hora(this)" runat="server" Height="21px" Width="60px"
                                                            MaxLength="5"></asp:TextBox>
                                                </td>
                                                <td>Data:&nbsp;
														<asp:TextBox ID="txtDataFinal" onkeyup="Mascara_Data(this)" runat="server" Height="21px" Width="91px" MaxLength="10"></asp:TextBox>&nbsp;&nbsp; 
														Hora:&nbsp;
														<asp:TextBox ID="txtHoraFinal" onkeyup="Mascara_Hora(this)" runat="server" Height="21px" Width="60px"
                                                            MaxLength="5"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 719px; height: 19px" colspan="3">
                                        <br>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 719px; height: 30px" align="center" colspan="3" height="30">
                                        <table width="71">
                                            <tr>
                                                <td style="width: 71px">
                                                    <asp:ImageButton ID="btnSalvar" runat="server" Width="71px" Height="25px" ImageUrl="../pdpw/images/bt_salvar.gif"></asp:ImageButton></td>
                                                <td style="width: 71px">
                                                    <asp:ImageButton ID="btnVoltar" runat="server" Width="71px" Height="25px" ImageUrl="../pdpw/images/bt_voltar.gif"></asp:ImageButton></td>
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

