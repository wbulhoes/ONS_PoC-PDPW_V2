<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frmColRestricaoUG.aspx.vb" Inherits="pdpw.frmColRestricaoUG" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>ONS - Operador Nacional do Sistema Elétrico</title>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <meta http-equiv="Content-Type" content="text/html; charset=windows-1252">
    <link href="../pdpw/images/style.css" rel="stylesheet">
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
                                                            <img height="23" src="../pdpw/images/tit_ColRestricaoUG.gif" width="280" style="width: 280px; height: 23px"></div>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <br>
                        <form id="frmColRestricaoUG" name="frmColRestricaoUG" runat="server">
                            <table class="modulo" style="width: 726px; height: 341px" cellspacing="0" cellpadding="0"
                                border="0">
                                <tr>
                                    <td style="width: 719px; height: 31px" colspan="3">&nbsp;&nbsp; Código:
											<asp:TextBox ID="txtCod" runat="server" Width="73px" Height="20px" Enabled="False"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr align="left">
                                    <td style="width: 719px; height: 40px" colspan="3">
                                        <table class="modulo" style="width: 775px; height: 58px">
                                            <tr>
                                                <td style="width: 320px; height: 30px" colspan="3">&nbsp;Gerador:&nbsp;<asp:DropDownList ID="cboGerador" runat="server" Width="320px" Height="20px" AutoPostBack="True"></asp:DropDownList>
                                                </td>
                                                <td style="width: 195px">Capacidade de Geração
														<asp:TextBox ID="txtValGer" runat="server" Width="58px" Enabled="False"></asp:TextBox>
                                                </td>
                                                <td style="width: 195px">Valor da Restrição
														<asp:TextBox ID="txtValRestr" runat="server" Width="58px"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 719px; height: 14px" colspan="3">
                                        <table height="18" cellspacing="0" cellpadding="0" width="724" background="../pdpw/images/back_titulo.gif">
                                            <tr>
                                                <td height="15"></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 719px; height: 31px" colspan="3">
                                        <table class="modulo" style="width: 495px; height: 65px">
                                            <tr>
                                                <td colspan="2">Período (dd/mm/aaaa):</td>
                                            </tr>
                                            <tr>
                                                <td style="width: 254px">De:</td>
                                                <td>Até:</td>
                                            </tr>
                                            <tr>
                                                <td>Data:&nbsp;
														<asp:TextBox ID="txtDataInicial" runat="server" Height="21px" Width="91px" MaxLength="10"></asp:TextBox>&nbsp;&nbsp; 
														Hora:&nbsp;
														<asp:DropDownList ID="cboHoraInicial" runat="server" Height="20px" Width="63px">
                                                            <asp:ListItem></asp:ListItem>
                                                            <asp:ListItem Value="00:30">00:30</asp:ListItem>
                                                            <asp:ListItem Value="01:00">01:00</asp:ListItem>
                                                            <asp:ListItem Value="01:30">01:30</asp:ListItem>
                                                            <asp:ListItem Value="02:00">02:00</asp:ListItem>
                                                            <asp:ListItem Value="02:30">02:30</asp:ListItem>
                                                            <asp:ListItem Value="03:00">03:00</asp:ListItem>
                                                            <asp:ListItem Value="03:30">03:30</asp:ListItem>
                                                            <asp:ListItem Value="04:00">04:00</asp:ListItem>
                                                            <asp:ListItem Value="04:30">04:30</asp:ListItem>
                                                            <asp:ListItem Value="05:00">05:00</asp:ListItem>
                                                            <asp:ListItem Value="05:30">05:30</asp:ListItem>
                                                            <asp:ListItem Value="06:00">06:00</asp:ListItem>
                                                            <asp:ListItem Value="06:30">06:30</asp:ListItem>
                                                            <asp:ListItem Value="07:00">07:00</asp:ListItem>
                                                            <asp:ListItem Value="07:30">07:30</asp:ListItem>
                                                            <asp:ListItem Value="08:00">08:00</asp:ListItem>
                                                            <asp:ListItem Value="08:30">08:30</asp:ListItem>
                                                            <asp:ListItem Value="09:00">09:00</asp:ListItem>
                                                            <asp:ListItem Value="09:30">09:30</asp:ListItem>
                                                            <asp:ListItem Value="10:00">10:00</asp:ListItem>
                                                            <asp:ListItem Value="10:30">10:30</asp:ListItem>
                                                            <asp:ListItem Value="11:00">11:00</asp:ListItem>
                                                            <asp:ListItem Value="11:30">11:30</asp:ListItem>
                                                            <asp:ListItem Value="12:00">12:00</asp:ListItem>
                                                            <asp:ListItem Value="12:30">12:30</asp:ListItem>
                                                            <asp:ListItem Value="13:00">13:00</asp:ListItem>
                                                            <asp:ListItem Value="13:30">13:30</asp:ListItem>
                                                            <asp:ListItem Value="14:00">14:00</asp:ListItem>
                                                            <asp:ListItem Value="14:30">14:30</asp:ListItem>
                                                            <asp:ListItem Value="15:00">15:00</asp:ListItem>
                                                            <asp:ListItem Value="15:30">15:30</asp:ListItem>
                                                            <asp:ListItem Value="16:00">16:00</asp:ListItem>
                                                            <asp:ListItem Value="16:30">16:30</asp:ListItem>
                                                            <asp:ListItem Value="17:00">17:00</asp:ListItem>
                                                            <asp:ListItem Value="17:30">17:30</asp:ListItem>
                                                            <asp:ListItem Value="18:00">18:00</asp:ListItem>
                                                            <asp:ListItem Value="18:30">18:30</asp:ListItem>
                                                            <asp:ListItem Value="19:00">19:00</asp:ListItem>
                                                            <asp:ListItem Value="19:30">19:30</asp:ListItem>
                                                            <asp:ListItem Value="20:00">20:00</asp:ListItem>
                                                            <asp:ListItem Value="20:30">20:30</asp:ListItem>
                                                            <asp:ListItem Value="21:00">21:00</asp:ListItem>
                                                            <asp:ListItem Value="21:30">21:30</asp:ListItem>
                                                            <asp:ListItem Value="22:00">22:00</asp:ListItem>
                                                            <asp:ListItem Value="22:30">22:30</asp:ListItem>
                                                            <asp:ListItem Value="23:00">23:00</asp:ListItem>
                                                            <asp:ListItem Value="23:30">23:30</asp:ListItem>
                                                            <asp:ListItem Value="24:00">24:00</asp:ListItem>
                                                        </asp:DropDownList>
                                                </td>
                                                <td>Data:&nbsp;
														<asp:TextBox ID="txtDataFinal" runat="server" Height="21px" Width="89px" MaxLength="10"></asp:TextBox>&nbsp;&nbsp; 
														Hora:&nbsp;
														<asp:DropDownList ID="cboHoraFinal" runat="server" Height="20px" Width="62px">
                                                            <asp:ListItem></asp:ListItem>
                                                            <asp:ListItem Value="00:30">00:30</asp:ListItem>
                                                            <asp:ListItem Value="01:00">01:00</asp:ListItem>
                                                            <asp:ListItem Value="01:30">01:30</asp:ListItem>
                                                            <asp:ListItem Value="02:00">02:00</asp:ListItem>
                                                            <asp:ListItem Value="02:30">02:30</asp:ListItem>
                                                            <asp:ListItem Value="03:00">03:00</asp:ListItem>
                                                            <asp:ListItem Value="03:30">03:30</asp:ListItem>
                                                            <asp:ListItem Value="04:00">04:00</asp:ListItem>
                                                            <asp:ListItem Value="04:30">04:30</asp:ListItem>
                                                            <asp:ListItem Value="05:00">05:00</asp:ListItem>
                                                            <asp:ListItem Value="05:30">05:30</asp:ListItem>
                                                            <asp:ListItem Value="06:00">06:00</asp:ListItem>
                                                            <asp:ListItem Value="06:30">06:30</asp:ListItem>
                                                            <asp:ListItem Value="07:00">07:00</asp:ListItem>
                                                            <asp:ListItem Value="07:30">07:30</asp:ListItem>
                                                            <asp:ListItem Value="08:00">08:00</asp:ListItem>
                                                            <asp:ListItem Value="08:30">08:30</asp:ListItem>
                                                            <asp:ListItem Value="09:00">09:00</asp:ListItem>
                                                            <asp:ListItem Value="09:30">09:30</asp:ListItem>
                                                            <asp:ListItem Value="10:00">10:00</asp:ListItem>
                                                            <asp:ListItem Value="10:30">10:30</asp:ListItem>
                                                            <asp:ListItem Value="11:00">11:00</asp:ListItem>
                                                            <asp:ListItem Value="11:30">11:30</asp:ListItem>
                                                            <asp:ListItem Value="12:00">12:00</asp:ListItem>
                                                            <asp:ListItem Value="12:30">12:30</asp:ListItem>
                                                            <asp:ListItem Value="13:00">13:00</asp:ListItem>
                                                            <asp:ListItem Value="13:30">13:30</asp:ListItem>
                                                            <asp:ListItem Value="14:00">14:00</asp:ListItem>
                                                            <asp:ListItem Value="14:30">14:30</asp:ListItem>
                                                            <asp:ListItem Value="15:00">15:00</asp:ListItem>
                                                            <asp:ListItem Value="15:30">15:30</asp:ListItem>
                                                            <asp:ListItem Value="16:00">16:00</asp:ListItem>
                                                            <asp:ListItem Value="16:30">16:30</asp:ListItem>
                                                            <asp:ListItem Value="17:00">17:00</asp:ListItem>
                                                            <asp:ListItem Value="17:30">17:30</asp:ListItem>
                                                            <asp:ListItem Value="18:00">18:00</asp:ListItem>
                                                            <asp:ListItem Value="18:30">18:30</asp:ListItem>
                                                            <asp:ListItem Value="19:00">19:00</asp:ListItem>
                                                            <asp:ListItem Value="19:30">19:30</asp:ListItem>
                                                            <asp:ListItem Value="20:00">20:00</asp:ListItem>
                                                            <asp:ListItem Value="20:30">20:30</asp:ListItem>
                                                            <asp:ListItem Value="21:00">21:00</asp:ListItem>
                                                            <asp:ListItem Value="21:30">21:30</asp:ListItem>
                                                            <asp:ListItem Value="22:00">22:00</asp:ListItem>
                                                            <asp:ListItem Value="22:30">22:30</asp:ListItem>
                                                            <asp:ListItem Value="23:00">23:00</asp:ListItem>
                                                            <asp:ListItem Value="23:30">23:30</asp:ListItem>
                                                            <asp:ListItem Value="24:00">24:00</asp:ListItem>
                                                        </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 719px; height: 14px" colspan="3">
                                        <table height="18" cellspacing="0" cellpadding="0" width="724" background="../pdpw/images/back_titulo.gif"
                                            style="width: 724px; height: 18px">
                                            <tr>
                                                <td height="15"></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    -
										<td style="width: 195px">Motivo de Restrição
											<asp:DropDownList ID="cboMotivo" runat="server" Width="195px" Height="20px" AutoPostBack="True"></asp:DropDownList>
                                        </td>
                                    <td style="width: 491px">Observação
											<asp:TextBox ID="txtObsRestr" runat="server" Width="513px" MaxLength="255" TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 719px; height: 14px" colspan="3">
                                        <table height="18" cellspacing="0" cellpadding="0" width="724" background="../pdpw/images/back_titulo.gif">
                                            <tr>
                                                <td height="15"></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 719px; height: 8px" colspan="3">
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
