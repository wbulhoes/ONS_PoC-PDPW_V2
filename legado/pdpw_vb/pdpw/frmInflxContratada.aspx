<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frmInflxContratada.aspx.vb" Inherits="pdpw.frmInflxContratada" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>ONS - Operador Nacional do Sistema Elétrico</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <link href="../pdpw/images/style.css" rel="stylesheet">
    <style type="text/css">
        .auto-style1 {
            width: 702px;
            height: 30px;
        }

        .auto-style3 {
            width: 304px;
            height: 23px;
            margin-top: 0px;
        }
    </style>
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
                                        <table cellspacing="0" cellpadding="0" width="765" background="../pdpw/images/back_tit_sistem0.gif"
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
                                                            <img height="23" src="../pdpw/images/tit_InflxContratada.gif"
                                                                width="304" class="auto-style3" style="top: 1px">

                                                            <%--                                                            		<DIV align="left"><IMG height="23" src="../pdpw/images/tit_CnsArquivo.gif" width="192" style="WIDTH: 192px; HEIGHT: 23px"></DIV>--%>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <form id="frmInflxContratada" name="frmInflxContratada" runat="server">
                            <table border="1" left="20" width="700">
                                <tr>
                                    <td class="auto-style1" style="border-width: thin thin 0px thin; witdh: 702px; border-left-color: green; border-right-color: green; border-top-color: green">

                                        <script type="text/javascript">
                                            var escolhido = "";


                                            function showpopup() {
                                                var selecionado = document.getElementById('HiddenFieldSelecionado').value;
                                                window.open("frmInflxContratadaModal.aspx?tipo=Alterar&selecionado=" + selecionado, "mywindow", "top=200,left=450,width=680,height=500");
                                            }
                                        </script>
                                        <script type="text/javascript">
                                            function incluir() {
                                                window.open("frmInflxContratadaModal.aspx?tipo=Incluir", "mywindow", "top=200,left=450,width=680,height=500");
                                            }
                                        </script>
                                        <asp:ImageButton ID="Button1" Text="Incluir" runat="server" ImageUrl="images/bt_incluir.gif" OnClick="btnIncluir_Click" OnClientClick="incluir()" />
                                        <asp:ImageButton ID="Button2" Text="Alterar" runat="server" ImageUrl="images/bt_alterar.gif" OnClick="btnAlterar_Click" OnClientClick="showpopup()" />
                                        <asp:ImageButton ID="Button3" Text="Excluir" runat="server" ImageUrl="images/bt_excluir.gif" OnClick="btnExcluir_Click" />
                                        <input id="selecionado" type="hidden" />
                                        <asp:HiddenField ID="HiddenFieldSelecionado" runat="Server" />
                                        <input type="hidden" id="ControleTipo" runat="server" enabled="False" visible="False">
                                    </td>
                                </tr>
                            </table>
                            <div class="modulo" id="divGrid" style="border-width: thin; width: 702px; display: inline; height: 184px"
                                ms_positioning="FlowLayout" dir="ltr">
                                <asp:DataGrid ID="dtgMotivo" runat="server" Width="700px" AllowPaging="True" AutoGenerateColumns="False"
                                    PageSize="8" OnPageIndexChanged="dtgMotivo_Paged" BorderColor="#006600" BorderStyle="Solid" BorderWidth="1px">
                                    <SelectedItemStyle Wrap="False" BackColor="Lavender"></SelectedItemStyle>
                                    <EditItemStyle Wrap="False"></EditItemStyle>
                                    <AlternatingItemStyle Font-Size="9pt" Font-Names="Arial" Wrap="False" BackColor="#F7F7F7"></AlternatingItemStyle>
                                    <ItemStyle Font-Size="9pt" Font-Names="Arial" Wrap="False" BackColor="#E9F4CF"></ItemStyle>
                                    <HeaderStyle Font-Size="X-Small" Font-Names="Arial" Font-Bold="True" ForeColor="Black" BackColor="#99CC00"></HeaderStyle>
                                    <Columns>
                                        <asp:TemplateColumn ItemStyle-HorizontalAlign="Center">
                                            <HeaderStyle Width="3"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkMarca" OnCheckedChanged="dtgMotivo_SelectedIndexChanged" AutoPostBack="true" runat="server" ControlStyle-Width="3px" />
                                            </ItemTemplate>

                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:TemplateColumn>
                                        <asp:BoundColumn DataField="codusina" ReadOnly="True" Visible="false" HeaderText="CodUsina">
                                            <HeaderStyle Width="20"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="nomusina" ReadOnly="True" HeaderText="Usina">
                                            <HeaderStyle Width="20"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="din_iniciovigencia" ReadOnly="True" HeaderText="Inicio Vigencia">
                                            <HeaderStyle HorizontalAlign="Center" Width="70"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="din_fimvigencia" ReadOnly="True" HeaderText="Fim Vigencia">
                                            <HeaderStyle HorizontalAlign="Center" Width="70"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="val_inflexcontratada" ReadOnly="True" HeaderText="Valor" DataFormatString="{0:N0}">
                                            <HeaderStyle HorizontalAlign="Center" Width="30"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="flg_registroativo" ReadOnly="True" HeaderText="Habilitado">
                                            <HeaderStyle HorizontalAlign="Center" Width="10"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="flg_modalidadecontrato" ReadOnly="True" HeaderText="Modalidade">
                                            <HeaderStyle HorizontalAlign="Center" Width="30"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
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

