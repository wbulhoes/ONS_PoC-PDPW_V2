<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frmCnsRecibo.aspx.vb" Inherits="pdpw.frmCnsRecibo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>frmCnsRecibo</title>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="../pdpw/images/style.css" rel="stylesheet">
    <style type="text/css">
        .auto-style1 {
            height: 27px;
        }

        .auto-style2 {
            height: 16px;
        }

        .auto-style3 {
            height: 19px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table style="width: 788px; height: 228px" height="228" cellspacing="0" cellpadding="0"
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
                                                            <img height="23" src="../pdpw/images/tit_CnsRecibo.gif" width="88" style="width: 88px; height: 23px"></div>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <form id="frmArquivo" name="frmArquivo" runat="server">
                            <table id="Table1" style="width: 476px; height: 104px; margin-bottom: 33px;" cellspacing="1" cellpadding="1" width="476"
                                border="0">
                                <tr>
                                    <td align="center" class="auto-style3" colspan="2">

                                        <asp:Label ID="lblMsg" Visible="False" ForeColor="Green" runat="server" Font-Bold="True" Font-Size="X-Small" ViewStateMode="Disabled">Desculpe, este dado não pôde ser carregado no momento.</asp:Label>

                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" class="auto-style3">
                                        <asp:Label ID="lblDataPdp" runat="server" Font-Names="Arial" Font-Size="X-Small" Font-Bold="True"
                                            Font-Name="Arial">Data do PDP:</asp:Label></td>
                                    <td class="auto-style3">
                                        <asp:DropDownList ID="drpDownDataPDP" runat="server" Font-Names="Arial" Font-Size="X-Small" AutoPostBack="True"
                                            Width="140px">
                                        </asp:DropDownList>&nbsp;&nbsp;&nbsp; </td>
                                </tr>
                                <tr>
                                    <td align="right" class="auto-style2">
                                        <asp:Label ID="lblEmpresa" runat="server" Font-Size="X-Small" Font-Bold="True" Font-Name="Arial">Empresa:</asp:Label></td>
                                    <td class="auto-style2">
                                        <asp:DropDownList ID="drpDownEmpresa" runat="server" Font-Names="Arial" Font-Size="X-Small" AutoPostBack="True"
                                            Width="140px">
                                        </asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblArquivo" runat="server" Font-Size="X-Small" Font-Bold="True" Font-Name="Arial">Arquivo:</asp:Label></td>
                                    <td>
                                        <asp:DropDownList ID="drpDownArquivo" runat="server" Font-Names="Arial" Font-Size="X-Small" AutoPostBack="True"
                                            Width="356px">
                                        </asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td align="right" class="auto-style1"></td>
                                    <td align="center" class="auto-style1">
                                        <asp:ImageButton ID="btnConfirmar" runat="server" ImageUrl="images/bt_confirmar.gif"></asp:ImageButton></td>
                                </tr>
                            </table>
                        </form>
                    </div>
                </td>
            </tr>
        </tbody>
    </table>
</asp:Content>

