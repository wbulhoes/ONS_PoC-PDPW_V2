<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frmMensagemValidacaoEnvio.aspx.vb" Inherits="pdpw.frmMensagemValidacaoEnvio" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>ONS - Operador Nacional do Sistema Elétrico</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <meta content="MSHTML 6.00.2600.0" name="GENERATOR">
    <meta content="C#" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="../pdpw/images/style.css" rel="stylesheet">
    <form id="MensagemErro" name="MensagemErro" action="MensagemErro.aspx" method="post">
        <p>&nbsp;</p>
        <p>
            <asp:DataGrid ID="dtgMensagem" runat="server"
                Font-Size="Small"
                Font-Names="Arial" AutoGenerateColumns="False" AllowPaging="False">
                <SelectedItemStyle BackColor="Lavender"></SelectedItemStyle>
                <AlternatingItemStyle BackColor="White"></AlternatingItemStyle>
                <ItemStyle BackColor="#E9F4CF" Height="25px"></ItemStyle>
                <HeaderStyle Font-Bold="True" BackColor="YellowGreen"></HeaderStyle>
                <Columns>
                    <asp:BoundColumn DataField="mensagem" HeaderText="Mensagens de erro da Validação de Envio">
                        <HeaderStyle Width="600px" Height="30px"></HeaderStyle>
                    </asp:BoundColumn>
                </Columns>
            </asp:DataGrid>

            <br />
            &nbsp;
      
            <input id="ibt_voltar" value="Back" onclick="history.back(1);return false; " type="image" src="../pdpw/images/bt_voltar.gif" border="0" name="ibt_voltar">
    </form>
    <!--a href="JavaScript:history.back()" onMouseOut="SwapImgRestore()" onMouseOver="SwapImage('Image2','','../WebColeta/imagens/voltar02.gif',1)">
<img name="Image2" border="0" src="../WebColeta/imagens/voltar01.gif" width="75" height="27"></a-->
</asp:Content>

