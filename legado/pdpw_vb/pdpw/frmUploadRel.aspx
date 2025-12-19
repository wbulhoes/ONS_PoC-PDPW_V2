<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frmUploadRel.aspx.vb" Inherits="pdpw.frmUploadRel" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>ONS - Operador Nacional do Sistema Elétrico</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table style="z-index: 101; left: 0px; width: 803px; position: absolute; top: 0px; height: 304px"
        height="304" cellspacing="0" cellpadding="0" width="803" border="0">
        <tbody>
            <tr>
                <td style="width: 15px" valign="top" width="15">
                    <br>
                </td>
                <td valign="top">
                    <div align="center">
                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                            <tbody>
                                <tr>
                                    <td width="20%" height="4" style="height: 4px">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td style="height: 17px" height="17">
                                        <table cellspacing="0" cellpadding="0" width="789" background="../pdpw/images/back_tit_sistema.gif"
                                            border="0" style="width: 789px; height: 25px">
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
                                        <table cellspacing="0" cellpadding="0" width="789" background="../pdpw/images/back_titulo.gif"
                                            border="0" style="width: 789px; height: 23px">
                                            <tbody>
                                                <tr>
                                                    <td style="height: 8px">
                                                        <div align="left">
                                                            <img height="23" src="../pdpw/images/tit_Upload.gif" width="88" style="width: 88px; height: 23px"></div>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <form id="frmUploadRel" name="frmUploadRel" method="post" enctype="multipart/form-data"
                            runat="server">
                            <table id="Table1" style="width: 535px; height: 152px" cellspacing="1" cellpadding="1"
                                width="535" border="0">
                                <tr>
                                    <td style="width: 91px; height: 19px" align="right">
                                        <asp:Label ID="lblDataPdp" runat="server" Font-Size="X-Small" Font-Bold="True" Font-Name="Arial"
                                            Font-Names="Arial">Data do PDP:</asp:Label></td>
                                    <td style="height: 19px">
                                        <asp:DropDownList ID="drpDownDataPDP" runat="server" Font-Size="X-Small" Font-Names="Arial" Width="140px"></asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td align="right" style="width: 91px; height: 26px">
                                        <asp:Label ID="lblArquivo" runat="server" Font-Name="Arial" Font-Bold="True" Font-Size="X-Small">Arquivo:</asp:Label></td>
                                    <td style="height: 26px">
                                        <input class="formulario_texto" onkeypress="return false;" id="fl_Arquivo" style="width: 434px; height: 22px"
                                            type="file" onchange="return false;" size="53" name="fl_Arquivo" runat="server"></td>
                                </tr>
                                <tr>
                                    <td align="right" style="width: 91px"></td>
                                    <td align="center">
                                        <asp:ImageButton ID="btnConfirmar" runat="server" ImageUrl="images/bt_confirmar.gif"></asp:ImageButton>
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
