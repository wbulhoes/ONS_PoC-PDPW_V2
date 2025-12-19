<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frmModalidadeOpTermica.aspx.vb" Inherits="pdpw.frmModalidadeOpTermica" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>ONS - Operador Nacional do Sistema Elétrico</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
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
                                                            <img style="width: 304px; height: 23px" height="23" src="../pdpw/images/tit_ModOpTermica.gif"
                                                                width="304">
                                                        </div>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <form id="frmCnsModalOperTerm" name="frmCnsModalOperTerm" runat="server">
                            <div class="modulo" id="divGrid" style="width: 702px; display: inline; height: 184px"
                                ms_positioning="FlowLayout">
                                <asp:DataGrid ID="dtgModalidade" runat="server" Width="700px" AllowPaging="True" AutoGenerateColumns="False"
                                    PageSize="8" OnPageIndexChanged="dtgModalidade_Paged">
                                    <SelectedItemStyle Wrap="False" BackColor="Lavender"></SelectedItemStyle>
                                    <EditItemStyle Wrap="False"></EditItemStyle>
                                    <AlternatingItemStyle Font-Size="9pt" Font-Names="Arial" Wrap="False" BackColor="#F7F7F7"></AlternatingItemStyle>
                                    <ItemStyle Font-Size="9pt" Font-Names="Arial" Wrap="False" BackColor="#E9F4CF"></ItemStyle>
                                    <HeaderStyle Font-Size="X-Small" Font-Names="Arial" Font-Bold="True" ForeColor="Black" BackColor="#99CC00"></HeaderStyle>
                                    <Columns>
                                        <asp:BoundColumn DataField="codmodalidade" ReadOnly="True" HeaderText="C&#243;digo">
                                            <HeaderStyle Width="30px"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="modalidade" ReadOnly="True" HeaderText="Descri&#231;&#227;o da Modalidade">
                                            <HeaderStyle HorizontalAlign="Center" Width="670px"></HeaderStyle>
                                            <FooterStyle HorizontalAlign="Center"></FooterStyle>
                                        </asp:BoundColumn>
                                    </Columns>
                                    <PagerStyle NextPageText="Pr&#243;xima&amp;gt;" Font-Size="7pt" Font-Names="Arial" Font-Bold="True"
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
