<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frmCnsUsina.aspx.vb" Inherits="pdpw.frmCnsUsina" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>frmCnsUsina</title>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="images/style.css" rel="stylesheet">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table style="width: 788px; height: 384px" height="384" cellspacing="0" cellpadding="0"
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
                                                            <img height="23" src="../pdpw/images/tit_CnsUsina.gif" width="88" style="width: 88px; height: 23px"></div>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <form id="frmArquivo" name="frmArquivo" runat="server">
                            <table class="modulo" id="Table1" style="width: 369px; height: 32px" cellspacing="1" cellpadding="1"
                                width="369" border="0">
                                <tr>
                                    <td style="height: 16px" align="right">Empresa:</td>
                                    <td style="width: 178px; height: 16px">
                                        <asp:DropDownList ID="cboEmpresa" runat="server" Font-Size="X-Small" Height="28px" Width="140px" Font-Names="Arial"></asp:DropDownList></td>
                                    <td>
                                        <asp:ImageButton ID="ImageButton1" runat="server" ImageAlign="Top" ImageUrl="images/bt_pesquisar.gif"></asp:ImageButton></td>
                                </tr>
                            </table>
                            <div class="modulo" style="display: inline; left: 204px; width: 370px; position: absolute; top: 160px; height: 223px"
                                ms_positioning="FlowLayout">
                                <asp:DataGrid ID="dtgUsina" runat="server" AllowPaging="True" AllowCustomPaging="True" AutoGenerateColumns="False"
                                    Height="20px" Width="390px">
                                    <SelectedItemStyle BackColor="Lavender"></SelectedItemStyle>
                                    <AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
                                    <ItemStyle Font-Size="8pt" Font-Names="Arial" BackColor="#E9F4CF"></ItemStyle>
                                    <HeaderStyle Font-Size="9pt" Font-Names="Arial" Font-Bold="True" HorizontalAlign="Center" ForeColor="Black"
                                        BackColor="#99CC00"></HeaderStyle>
                                    <Columns>
                                        <asp:HyperLinkColumn DataNavigateUrlField="codusina" DataNavigateUrlFormatString="frmCnsUsina.aspx?strOper=A&amp;id={0}"
                                            DataTextField="codusina" HeaderText="C&#243;digo" NavigateUrl="frmCnsUsiDados.aspx">
                                            <HeaderStyle Width="70px"></HeaderStyle>
                                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                        </asp:HyperLinkColumn>
                                        <asp:BoundColumn DataField="sigusina" ReadOnly="True" HeaderText="Sigla">
                                            <HeaderStyle Width="80px"></HeaderStyle>
                                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="nomusina" ReadOnly="True" HeaderText="Nome">
                                            <HeaderStyle Width="170px"></HeaderStyle>
                                            <ItemStyle Wrap="False"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="tipusina" ReadOnly="True" HeaderText="Tipo">
                                            <HeaderStyle Width="60px"></HeaderStyle>
                                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
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

