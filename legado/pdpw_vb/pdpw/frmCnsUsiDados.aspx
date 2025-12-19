<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frmCnsUsiDados.aspx.vb" Inherits="pdpw.frmCnsUsiDados" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>ONS - Operador Nacional do Sistema Elétrico</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio.NET 7.0">
    <meta name="CODE_LANGUAGE" content="Visual Basic 7.0">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <link href="images/style.css" rel="stylesheet">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table style="width: 776px; height: 418px" height="418" cellspacing="0" cellpadding="0"
        width="776" border="0">
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
                                                            <img height="23" src="../pdpw/images/tit_CnsUsiDados.gif" width="130"></div>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <form id="frmArquivo" name="frmArquivo" runat="server">
                            <table class="formulario_texto" cellspacing="0" cellpadding="0" border="0" style="width: 763px; height: 110px">
                                <tr>
                                    <td align="right" style="width: 85px; height: 7px">Código:&nbsp;</td>
                                    <td style="width: 186px; height: 20px" colspan="2">
                                        <asp:TextBox ID="txtCodigo" runat="server" ReadOnly="True" Width="82px" BorderStyle="Groove"></asp:TextBox></td>
                                    <td align="right" style="width: 94px; height: 7px">Bacia:&nbsp;</td>
                                    <td style="width: 124px; height: 7px" colspan="2">
                                        <asp:TextBox ID="txtBacia" runat="server" ReadOnly="True" BorderStyle="Groove"></asp:TextBox></td>
                                    <td align="right" style="width: 118px; height: 7px">Cod. GTPO:&nbsp;</td>
                                    <td style="width: 97px; height: 7px">
                                        <asp:TextBox ID="txtGtpo" runat="server" ReadOnly="True" Width="42px" BorderStyle="Groove"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td align="right" style="width: 85px; height: 23px">Sigla:&nbsp;</td>
                                    <td style="width: 186px; height: 23px" colspan="2">
                                        <asp:TextBox ID="txtSigla" runat="server" ReadOnly="True" Width="82px" BorderStyle="Groove"></asp:TextBox></td>
                                    <td align="right" style="width: 94px; height: 23px">Usina a Jusante:&nbsp;</td>
                                    <td style="width: 124px; height: 23px" colspan="2">
                                        <asp:TextBox ID="txtUsiJusante" runat="server" ReadOnly="True" BorderStyle="Groove"></asp:TextBox></td>
                                    <td align="right" style="width: 118px; height: 23px">Potência Instalada:&nbsp;</td>
                                    <td style="width: 97px; height: 23px">
                                        <asp:TextBox ID="txtPotInstalada" runat="server" ReadOnly="True" Width="54px" BorderStyle="Groove"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td align="right" style="width: 85px; height: 17px">Nome:&nbsp;</td>
                                    <td style="width: 186px; height: 17px" colspan="2">
                                        <asp:TextBox ID="txtNome" runat="server" Width="180px" ReadOnly="True" BorderStyle="Groove"></asp:TextBox></td>
                                    <td align="right" style="width: 94px; height: 17px">Instalação:&nbsp;</td>
                                    <td style="width: 124px; height: 17px" colspan="2">
                                        <asp:TextBox ID="txtIns" runat="server" ReadOnly="True" BorderStyle="Groove"></asp:TextBox></td>
                                    <td colspan="2" style="width: 187px; height: 17px"></td>
                                </tr>
                                <tr>
                                    <td colspan="8" style="width: 764px; height: 9px">
                                        <img height="8" src="images/back_titulo.gif" width="759" style="width: 101.84%; height: 8px"></td>
                                </tr>
                                <tr>
                                    <td align="right" style="width: 85px; height: 3px">Tipo:&nbsp;</td>
                                    <td style="width: 51px; height: 3px">
                                        <asp:CheckBox ID="chkHidraulica" runat="server" Enabled="False" Text="Hidráulica" Height="16px"></asp:CheckBox></td>
                                    <td style="width: 101px; height: 3px">
                                        <asp:CheckBox ID="chkTermica" runat="server" Enabled="False" Text="Térmica"></asp:CheckBox></td>
                                    <td align="right" style="width: 94px; height: 3px"></td>
                                    <td style="width: 122px; height: 3px" align="right">Classificação:</td>
                                    <td style="width: 19px; height: 3px">
                                        <asp:CheckBox ID="chkCag" runat="server" Enabled="False" Text="CAG"></asp:CheckBox></td>
                                    <td colspan="2" style="width: 187px; height: 3px">
                                        <asp:CheckBox ID="chkPqu" runat="server" Enabled="False" Text="Pequenas Usinas"></asp:CheckBox></td>
                                </tr>
                                <tr>
                                    <td colspan="8" style="width: 764px; height: 9px">
                                        <img height="8" src="images/back_titulo.gif" width="786" style="width: 100%; height: 8px"></td>
                                </tr>
                            </table>
                            <div id="divGrid" style="left: 250px; width: 314px; position: absolute; top: 205px; height: 116px">
                                <asp:DataGrid ID="dtgUsiDados" runat="server" Width="312px" PageSize="5" AutoGenerateColumns="False"
                                    AllowCustomPaging="True" AllowPaging="True">
                                    <SelectedItemStyle BackColor="Lavender"></SelectedItemStyle>
                                    <AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
                                    <ItemStyle Font-Size="Smaller" Font-Names="Arial" BackColor="#E9F4CF"></ItemStyle>
                                    <HeaderStyle Font-Size="9pt" Font-Names="Arial" Font-Bold="True" HorizontalAlign="Center" ForeColor="Black"
                                        BackColor="#99CC00"></HeaderStyle>
                                    <Columns>
                                        <asp:BoundColumn DataField="codgerad" ReadOnly="True" HeaderText="C&#243;digo da Geradora">
                                            <ItemStyle Font-Size="8pt"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="siggerad" ReadOnly="True" HeaderText="Sigla">
                                            <ItemStyle Font-Size="8pt"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="capgerad" ReadOnly="True" HeaderText="Capacidade">
                                            <ItemStyle Font-Size="8pt" HorizontalAlign="Right"></ItemStyle>
                                        </asp:BoundColumn>
                                    </Columns>
                                    <PagerStyle NextPageText="Pr&#243;xima&amp;gt;" Font-Size="7pt" PrevPageText="&amp;lt;Anterior"></PagerStyle>
                                </asp:DataGrid>
                            </div>
                            <div style="left: 370px; position: absolute; top: 335px">
                                <asp:ImageButton ID="imgBtnVoltar" runat="server" ImageUrl="images/bt_voltar.gif" Width="68px"></asp:ImageButton>
                            </div>
                        </form>
                    </div>
                </td>
            </tr>
        </tbody>
    </table>
</asp:Content>

