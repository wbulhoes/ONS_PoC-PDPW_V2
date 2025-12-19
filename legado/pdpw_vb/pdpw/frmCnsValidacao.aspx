<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frmCnsValidacao.aspx.vb" Inherits="pdpw.frmCnsValidacao" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>ONS - Operador Nacional do Sistema Elétrico</title>
    <meta http-equiv="Content-Type" content="text/html; charset=windows-1252">
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="images/style.css" rel="stylesheet">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table style="width: 1032px; height: 354px" height="354" cellspacing="0" cellpadding="0"
        width="1032" border="0">
        <tbody>
            <tr>
                <td style="width: 75px; height: 100px" valign="top" width="75">
                    <br>
                </td>
                <td style="width: 1231px" valign="top">
                    <div align="center">
                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                            <tbody>
                                <tr>
                                    <td width="20%" height="2">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td style="height: 17px" height="17">
                                        <table style="width: 966px; height: 26px" cellspacing="0" cellpadding="0" width="966" background="../pdpw/images/back_tit_sistema.gif"
                                            border="0">
                                            <tbody>
                                                <tr>
                                                    <td style="height: 26px">
                                                        <img height="25" src="../pdpw/images/tit_sis_guideline.gif" width="179"></td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 10px" height="10">
                                        <table style="width: 967px; height: 23px" cellspacing="0" cellpadding="0" width="967" background="../pdpw/images/back_titulo.gif"
                                            border="0">
                                            <tbody>
                                                <tr>
                                                    <td style="height: 8px">
                                                        <div align="left">
                                                            <img style="width: 232px; height: 23px" height="23" src="../pdpw/images/tit_ValidHidro.gif"
                                                                width="232">
                                                        </div>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <form id="frmCnsValidacao" name="frmCnsValidacao" runat="server">
                            <div style="display: inline; left: 152px; width: 407px; position: absolute; top: 110px; height: 80px"
                                ms_positioning="FlowLayout">
                                <table class="modulo" id="Table1" style="width: 378px; height: 56px" cellspacing="0" cellpadding="0"
                                    border="0">
                                    <tr>
                                        <td style="width: 61px" align="right">Data PDP:
                                        </td>
                                        <td style="width: 233px">&nbsp;
												<asp:DropDownList ID="cboData" runat="server" Width="97px"></asp:DropDownList></td>
                                        <td rowspan="3">
                                            <asp:ImageButton ID="btnPesquisar" runat="server" ImageUrl="images/bt_visualizar.gif" ImageAlign="Top"></asp:ImageButton></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 61px" align="right">Empresa:
                                        </td>
                                        <td style="width: 233px">&nbsp;
												<asp:DropDownList ID="cboEmpresa" runat="server" Width="216px"></asp:DropDownList></td>
                                        <tr>
                                            <td style="width: 61px" align="right">Usina:
                                            </td>
                                            <td style="width: 233px">&nbsp;
												<asp:DropDownList ID="cboUsina" runat="server" Width="216px"></asp:DropDownList></td>
                                        </tr>
                                    <tr>
                                        <td style="width: 61px" align="center" colspan="2" rowspan="1">
                                            <asp:CheckBox ID="chkTodos" runat="server" Width="288px" Text="Todas as Validações"></asp:CheckBox></td>
                                    </tr>
                                </table>
                            </div>
                            <div class="modulo" style="display: inline; left: 45px; width: 913px; position: absolute; top: 220px; height: 303px"
                                ms_positioning="FlowLayout">
                                <asp:DataGrid ID="dtgValidacao" runat="server" Width="940px" OnPageIndexChanged="dtgValidacao_Paged"
                                    AutoGenerateColumns="False" AllowPaging="True" PageSize="15">
                                    <SelectedItemStyle BackColor="Lavender"></SelectedItemStyle>
                                    <AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
                                    <ItemStyle Font-Size="8pt" Font-Names="Arial" BackColor="#E9F4CF"></ItemStyle>
                                    <HeaderStyle Font-Size="9pt" Font-Names="Arial" Font-Bold="True" HorizontalAlign="Center" ForeColor="Black"
                                        BackColor="#99CC00"></HeaderStyle>
                                    <Columns>
                                        <asp:BoundColumn DataField="nomempre" HeaderText="Empresa">
                                            <HeaderStyle Width="100px"></HeaderStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="codusina" HeaderText="Usina">
                                            <HeaderStyle Width="60px"></HeaderStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="datvalid" HeaderText="Dt Valida&#231;&#227;o" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}">
                                            <HeaderStyle Width="130px"></HeaderStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="status" HeaderText="Status">
                                            <HeaderStyle Width="50px"></HeaderStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="obs" HeaderText="Observa&#231;&#245;es">
                                            <HeaderStyle Width="570px"></HeaderStyle>
                                        </asp:BoundColumn>
                                    </Columns>
                                    <PagerStyle NextPageText="Pr&#243;xima&amp;gt;" Font-Size="XX-Small" Font-Names="Arial" Font-Bold="True"
                                        PrevPageText="&amp;lt;Anterior"></PagerStyle>
                                </asp:DataGrid>
                            </div>
                        </form>
                    </div>
                </td>
            </tr>
        </tbody>
    </table>
</asp:Content>

