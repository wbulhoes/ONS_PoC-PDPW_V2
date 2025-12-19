<%@ Page Language="vb" MasterPageFile="~/Site.Master" AutoEventWireup="true" %>

<script runat="server">
    Protected Overrides Sub Render(ByVal writer As HtmlTextWriter)
        ' Exibe o menu nesta página.
        Dim OnsMenu As OnsWebControls.OnsMenu = Session("onsmenu")
        OnsMenu.RenderControl(writer)
    End Sub
</script>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Início</title>

    <script type="text/javascript">
        if (window == top) {
            top.location.replace("/intunica/menu.aspx");
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="TelaInicialVaziaForm" method="post" runat="server">
    </form>
</asp:Content>
