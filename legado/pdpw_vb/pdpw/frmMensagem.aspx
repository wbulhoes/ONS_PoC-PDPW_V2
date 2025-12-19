<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frmMensagem.aspx.vb" Inherits="pdpw.frmMensagem" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>ONS - Operador Nacional do Sistema Elétrico</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
    <meta content="MSHTML 6.00.2600.0" name="GENERATOR">
    <meta content="C#" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="../pdpw/images/style.css" rel="stylesheet">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="MensagemErro" runat="server">
        <table>
            <tr>
                <td style="text-align: center;">
                    <asp:TextBox ID="TxtMensagem" runat="server" Enabled="False" ReadOnly="True" TextMode="MultiLine" Width="1000px" Height="300px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <input id="ibt_voltar" value="Back" onclick="history.go(-1);return false; " type="image" src="../pdpw/images/bt_voltar.gif" border="0" name="ibt_voltar">
                </td>
            </tr>
        </table>
    </form>
    <!--a href="JavaScript:history.back()" onMouseOut="SwapImgRestore()" onMouseOver="SwapImage('Image2','','../WebColeta/imagens/voltar02.gif',1)">
<img name="Image2" border="0" src="../WebColeta/imagens/voltar01.gif" width="75" height="27"></a-->
</asp:Content>
