<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frmManDiretorio.aspx.vb" Inherits="pdpw.frmManDiretorio" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>ONS - Operador Nacional do Sistema Elétrico</title>
    <meta http-equiv="Content-Type" content="text/html; charset=windows-1252">
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="../pdpw/images/style.css" rel="stylesheet">
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
                                                            <img style="width: 192px; height: 23px" height="23" src="images/tit_ManDiretorio.gif"
                                                                width="192">
                                                        </div>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <form id="frmDiretorio" name="frmDiretorio" runat="server">
                            <table class="modulo" style="width: 154px; height: 36px" cellspacing="0" cellpadding="0"
                                border="0">
                                <tr align="center">
                                    <td style="width: 190px">
                                        <asp:ImageButton ID="btnExcluir" runat="server" ImageUrl="images/bt_excluir.gif" style="height: 24px"></asp:ImageButton></td>
                                </tr>
                            </table>
                            <table style="width: 576px; height: 270px" cellspacing="0" cellpadding="0" border="0">
                                <tr valign="top" align="center">
                                    <td style="width: 539px; height: 8px">
                                        <br>
                                        <asp:DataGrid ID="dtgDiretorio" runat="server" Font-Size="XX-Small" Width="598px" PageSize="8"
                                            AllowPaging="True" Font-Names="Arial" OnPageIndexChanged="dtgDiretorio_Paged">
                                            <SelectedItemStyle BackColor="Lavender"></SelectedItemStyle>
                                            <AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
                                            <ItemStyle BackColor="#E9F4CF"></ItemStyle>
                                            <HeaderStyle Font-Bold="True" BackColor="YellowGreen"></HeaderStyle>
                                            <Columns>
                                                <asp:TemplateColumn>
                                                    <ItemStyle Width="10px"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkMarca" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                            </Columns>
                                            <PagerStyle NextPageText="Pr&#243;xima&amp;gt;" Font-Size="XX-Small" Font-Names="Arial" Font-Bold="True"
                                                PrevPageText="&amp;lt;Anterior"></PagerStyle>
                                        </asp:DataGrid></td>
                                </tr>
                            </table>
                        </form>
                    </div>
                </td>
            </tr>
        </tbody>
    </table>
</asp:Content>

