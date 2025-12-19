<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frmMsgUpload.aspx.vb" Inherits="pdpw.frmMsgUpload" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>frmMsgUpload</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio.NET 7.0">
    <meta name="CODE_LANGUAGE" content="Visual Basic 7.0">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
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
                                                        <div align="left">&nbsp;</div>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <form id="frmArquivo" name="frmArquivo" method="post" enctype="multipart/form-data" runat="server">
                            <table id="Table1" style="width: 535px; height: 153px" cellspacing="1" cellpadding="1"
                                width="535" border="0">
                                <tr>
                                    <td style="width: 91px; height: 90px" align="right"></td>
                                    <td style="height: 90px">
                                        <table id="Table2" bordercolor="#969696" cellspacing="0" cellpadding="0" width="600" align="center"
                                            border="1">
                                            <tr bgcolor="#e3e3e3">
                                                <td class="texto_bold" height="80">
                                                    <div align="center">
                                                        <span class="texto_bold" id="lb_erro">
                                                            <asp:Label ID="lblMensagem" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="X-Small"></asp:Label>
                                                        </span>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" style="width: 91px"></td>
                                    <td align="center">
                                        <asp:ImageButton ID="btnVoltar" runat="server" ImageUrl="images/bt_voltar.gif"></asp:ImageButton>
                                        <p></p>
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

