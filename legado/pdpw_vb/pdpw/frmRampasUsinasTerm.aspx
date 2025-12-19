<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frmRampasUsinasTerm.aspx.vb" Inherits="pdpw.frmRampasUsinasTerm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>ONS - Operador Nacional do Sistema Elétrico</title>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <meta http-equiv="Content-Type" content="text/html; charset=windows-1252">
    <link href="../pdpw/images/style.css" rel="stylesheet">
    <script language="JavaScript">

</script>
    <style type="text/css">
        .auto-style1 {
            height: 26px;
        }

        .auto-style4 {
            width: 154px;
        }

        .auto-style5 {
            width: 902px;
        }

        .auto-style6 {
            height: 26px;
            width: 1099px;
        }

        .auto-style7 {
            width: 812px;
            height: 26px;
        }

        .auto-style8 {
            width: 901px;
        }

        .auto-style10 {
            width: 644px;
        }

        .auto-style13 {
            height: 26px;
            margin-left: 40px;
            width: 711px;
        }

        .auto-style16 {
            height: 33px;
            width: 388px;
        }

        .auto-style18 {
            height: 33px;
            width: 220px;
        }

        .auto-style19 {
            width: 812px;
            height: 30px;
        }

        .auto-style20 {
            width: 388px;
        }

        .auto-style24 {
            width: 5px;
        }

        .auto-style26 {
            width: 7px;
            height: 26px;
        }

        .auto-style27 {
            height: 26px;
            margin-left: 40px;
            width: 309px;
        }

        .auto-style28 {
            width: 308px;
            height: 26px;
        }

        .auto-style29 {
            width: 243px;
            height: 31px;
        }

        .auto-style32 {
            width: 322px;
        }

        .auto-style33 {
            height: 332px;
            width: 1106px;
        }

        .auto-style36 {
            height: 3px;
        }

        .auto-style40 {
            height: 5px;
            width: 220px;
        }

        .auto-style42 {
            width: 220px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="frmRampasUsinasTerm" runat="server" title="ONS - Operador Nacional do Sistema Elétrico">
        <div>

            <table cellspacing="0" cellpadding="0" border="0" class="auto-style8">
                <tbody>
                    <tr>
                        <td class="auto-style1"></td>
                    </tr>
                    <tr>
                        <td height="2">
                            <table cellspacing="0" cellpadding="0" background="../pdpw/images/back_tit_sistema.gif"
                                border="0" class="auto-style5">
                                <tbody>
                                    <tr>
                                        <td class="auto-style10">
                                            <img height="25" src="../pdpw/images/tit_sis_guideline.gif" class="auto-style4"></td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="font-size: 18px; font-family: 'Arial Narrow'; font-style: italic; color: #99CC00; font-weight: bold;">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                Rampas Usinas Térmicas ____________________________________________________________________________________</td>
                    </tr>
                </tbody>
            </table>

        </div>
        <table cellspacing="0" cellpadding="0" border="0" class="auto-style6">
            <tr>
                <td align="right" class="auto-style1" style="padding: inherit; margin: auto; font-family: Arial, Helvetica, sans-serif; font-size: x-small;" colspan="2"><b>Usina:&nbsp;&nbsp; </b></td>
                <td class="auto-style1" colspan="6" style="padding: inherit; margin: auto; font-family: Arial, Helvetica, sans-serif; font-size: x-small;">
                    <asp:DropDownList ID="cboUsina" runat="server" AutoPostBack="True" Height="20px" Width="200px" Font-Size="X-Small" ForeColor="Black">
                    </asp:DropDownList>
                    <asp:TextBox ID="txtCodUsina" runat="server" Enabled="False" Font-Size="Small" Width="16px" BorderStyle="None" Height="16px" Visible="False"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right" class="auto-style1" style="padding: inherit; margin: auto; font-family: Arial, Helvetica, sans-serif; font-size: x-small;" colspan="2"><b>Unidade geradora equivalente DESSEM:</b></td>
                <td class="auto-style1" colspan="6" style="padding: inherit; margin: auto; font-family: Arial, Helvetica, sans-serif; font-size: x-small;">
                    <asp:DropDownList ID="cboUGDessem" runat="server" AutoPostBack="True" Height="20px" Width="200px" Font-Size="X-Small" TabIndex="1">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td align="right" class="auto-style1" style="padding: inherit; margin: auto; font-family: Arial, Helvetica, sans-serif; font-size: x-small;" colspan="2"><b>Tipo forma rampa:&nbsp;&nbsp; </b></td>
                <td class="auto-style1" colspan="6" style="padding: inherit; margin: auto; font-family: Arial, Helvetica, sans-serif; font-size: x-small;">
                    <asp:DropDownList ID="cboFormaRampa" runat="server" AutoPostBack="True" Height="20px" Width="200px" Font-Size="X-Small" TabIndex="2">
                    </asp:DropDownList>
                    <asp:TextBox ID="txtIDFormaRampa" runat="server" Enabled="False" Font-Size="Small" Width="16px" BorderStyle="None" Height="16px" Visible="False"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td valign="top" align="right" class="auto-style1" style="padding: inherit; margin: auto; font-family: Arial, Helvetica, sans-serif; font-size: x-small;" colspan="2"><b>Tipo rampa:&nbsp;&nbsp; </b>
                </td>
                <td valign="top" class="auto-style1" colspan="6" style="padding: inherit; margin: auto; font-family: Arial, Helvetica, sans-serif; font-size: x-small;">
                    <asp:DropDownList ID="cboTipoRampa" runat="server" AutoPostBack="True" Height="20px" Width="285px" Font-Size="X-Small" TabIndex="3">
                    </asp:DropDownList>
                    <asp:TextBox ID="txtIDTipoRampa" runat="server" Enabled="False" Font-Size="Small" Width="16px" BorderStyle="None" Height="16px" Visible="False"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right" class="auto-style1" style="padding: inherit; margin: auto; font-family: Arial, Helvetica, sans-serif; font-size: x-small;" colspan="2"><b>Tempo:</b></td>
                <td class="auto-style1" colspan="6" style="padding: inherit; margin: auto; font-family: Arial, Helvetica, sans-serif; font-size: x-small;">
                    <asp:DropDownList ID="cboTempo" runat="server" AutoPostBack="True" Width="83px" Font-Size="X-Small" Height="20px" TabIndex="4">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td align="right" class="auto-style1" style="padding: inherit; margin: auto; font-family: Arial, Helvetica, sans-serif; font-size: x-small;" colspan="2"><b>Potência (MW):&nbsp;</b></td>
                <td class="auto-style1" colspan="6" style="padding: inherit; margin: auto; font-family: Arial, Helvetica, sans-serif; font-size: x-small;">
                    <asp:TextBox ID="txtPotencia" runat="server" Font-Size="X-Small" Width="80px" Font-Names="Arial" Height="20px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right" class="auto-style1">
                    <asp:Panel ID="Panel2" runat="server" BorderColor="#009900" BorderStyle="Solid" BorderWidth="3px" Font-Bold="True" Font-Overline="False" Font-Size="X-Small" Height="42px" Width="269px" HorizontalAlign="Left">
                        <b style="line-height: normal">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Potência Mínina (MW)</b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; Ton(h)
                                                <br />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                <asp:TextBox ID="txtPotenciaMin" runat="server" Enabled="False" Font-Size="X-Small" Width="43px"></asp:TextBox>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                <asp:TextBox ID="txtTon" runat="server" Enabled="False" Font-Size="X-Small" Width="47px"></asp:TextBox>
                    </asp:Panel>
                </td>
                <td align="right" class="auto-style1"></td>
                <td class="auto-style28">&nbsp;&nbsp;<asp:TextBox ID="txtFormaCad" runat="server" Enabled="False" Font-Size="Small" Width="16px" BorderStyle="None" Height="16px" Visible="False"></asp:TextBox>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                <td class="auto-style26"></td>
                <td class="auto-style27">&nbsp;&nbsp; &nbsp;<asp:Panel ID="Panel3" runat="server" BorderColor="#CCCCCC" BorderStyle="Groove" BorderWidth="3px" Font-Bold="True" Font-Overline="False" Font-Size="Small" Height="29px" Width="345px">
                    &nbsp;&nbsp;&nbsp;<asp:ImageButton ID="btnIncluir" runat="server" Height="25px" ImageUrl="../pdpw/images/bt_incluir.gif" TabIndex="6" Width="71px" />
                    &nbsp;&nbsp;&nbsp;<asp:ImageButton ID="btnSalvar" runat="server" ImageUrl="images/bt_salvar.gif" TabIndex="7" />
                    &nbsp;&nbsp;
                                                    <asp:ImageButton ID="btnExcluir" runat="server" ImageUrl="~/images/bt_excluir.gif" TabIndex="7" />
                    &nbsp;&nbsp;
                                                    <asp:ImageButton ID="btnCancelar" runat="server" ImageUrl="~/images/bt_limpar.gif" TabIndex="8" />
                    &nbsp;
                    </asp:Panel>
                </td>
                <td class="auto-style13">&nbsp; 
                </td>
                <td class="auto-style1"></td>
                <td class="auto-style1"></td>
            </tr>
            <tr>
                <td colspan="8" class="auto-style1">
                    <p class="auto-style7" style="background-color: #E2F1CD; font-variant: normal; text-transform: none; font-weight: bold; text-align: justify; font-size: small; border-top-style: groove; border-top-color: #99CC00; border-top-width: 2px; border-bottom-style: groove; border-bottom-color: #99CC00; border-bottom-width: 2px;">
                        Dados conjunturais rampas DESSEM
                    </p>
                </td>
            </tr>
            <tr>
                <td class="auto-style1" colspan="8">
                    <asp:DataGrid ID="GridDessem" runat="server" Width="812px" PageSize="6" AllowPaging="True" AutoGenerateColumns="False"
                        Font-Names="Arial" Font-Size="XX-Small" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False">
                        <SelectedItemStyle BackColor="Lavender"></SelectedItemStyle>
                        <AlternatingItemStyle BackColor="#F7F7F7" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" Wrap="False"></AlternatingItemStyle>
                        <ItemStyle BackColor="#E9F4CF"></ItemStyle>
                        <HeaderStyle Font-Bold="True" BackColor="YellowGreen" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" VerticalAlign="Middle"></HeaderStyle>
                        <Columns>
                            <asp:TemplateColumn>
                                <HeaderStyle Width="20px"></HeaderStyle>
                                <ItemStyle Width="10px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkMarca" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:BoundColumn DataField="UGE" HeaderText="UGE Equivalente">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" Width="150px" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="FormaRampa" HeaderText="Tipo Forma Rampa">
                                <HeaderStyle Width="120px"></HeaderStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="TipoRampa" HeaderText="Tipo Rampa">
                                <HeaderStyle Width="300px" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="Tempo" HeaderText="Tempo">
                                <HeaderStyle Width="60px"></HeaderStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="Pot" HeaderText="Potência (MW)">
                                <HeaderStyle Width="70px" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="ton" HeaderText="Ton (h)" Visible="False"></asp:BoundColumn>
                            <asp:BoundColumn DataField="PotMinUGE" HeaderText="Potência mínima" Visible="False"></asp:BoundColumn>
                            <asp:BoundColumn DataField="IDFormaRampa" HeaderText="ID Forma Rampa" Visible="False"></asp:BoundColumn>
                            <asp:BoundColumn DataField="IDTipoRampa" HeaderText="ID Tipo Rampa" Visible="False"></asp:BoundColumn>
                            <asp:BoundColumn DataField="Lgn_Usuario" HeaderText="Lgn_Usuario" Visible="False"></asp:BoundColumn>
                            <asp:BoundColumn DataField="id_rampaugeconjuntural" HeaderText="id_rampaugeconjuntural" Visible="False"></asp:BoundColumn>
                        </Columns>
                        <PagerStyle NextPageText="Pr&#243;xima&amp;gt;" Font-Size="XX-Small" Font-Names="Arial" Font-Bold="True"
                            PrevPageText="&amp;lt;Anterior" PageButtonCount="6"></PagerStyle>
                    </asp:DataGrid>
                </td>
            </tr>
        </table>
        <p class="auto-style36">
        </p>
        <table cellspacing="0" cellpadding="0" border="0" class="auto-style33">
            <tr>
                <td colspan="2" class="auto-style16">
                    <p class="auto-style19" style="background-color: #E2F1CD; font-variant: normal; text-transform: none; font-weight: bold; text-align: justify; font-size: small; border-top-style: groove; border-top-color: #99CC00; border-top-width: 2px; border-bottom-style: groove; border-bottom-color: #99CC00; border-bottom-width: 2px;">
                        Dados estruturais rampas
                    </p>
                </td>
                <td class="auto-style18" style="border: 1px solid #99CC00;">
                    <p class="auto-style29" style="background-color: #E2F1CD; font-variant: normal; text-transform: none; font-weight: bold; text-align: center; font-size: small; border-top-color: #99CC00; border-top-width: 2px; border-bottom-color: #99CC00; border-bottom-width: 2px;">
                        <asp:ImageButton ID="btnImportarGERCAD" runat="server" Enabled="False" ImageUrl="~/images/bt_importar_rampas_Gercad.gif" TabIndex="8" Width="192px" />
                    </p>
                </td>
            </tr>
            <tr>
                <td class="auto-style20" rowspan="7">
                    <asp:DataGrid ID="GridEstrutural" runat="server" Width="812px" PageSize="15" AllowPaging="True" AutoGenerateColumns="False"
                        Font-Names="Arial" Font-Size="XX-Small" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" Height="314px">
                        <SelectedItemStyle BackColor="Lavender"></SelectedItemStyle>
                        <AlternatingItemStyle BackColor="#F7F7F7" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" Wrap="False"></AlternatingItemStyle>
                        <ItemStyle BackColor="#E9F4CF"></ItemStyle>
                        <HeaderStyle Font-Bold="True" BackColor="YellowGreen" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" VerticalAlign="Middle"></HeaderStyle>
                        <Columns>
                            <asp:TemplateColumn Visible="False">
                                <HeaderStyle Width="20px"></HeaderStyle>
                                <ItemStyle Width="10px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkMarca1" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:BoundColumn DataField="UGE" HeaderText="UGE Equivalente">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" Width="150px" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="FormaRampa" HeaderText="Tipo Forma Rampa">
                                <HeaderStyle Width="120px"></HeaderStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="TipoRampa" HeaderText="Tipo Rampa">
                                <HeaderStyle Width="300px" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="Tempo" HeaderText="Tempo">
                                <HeaderStyle Width="60px"></HeaderStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="Pot" HeaderText="Potência (MW)">
                                <HeaderStyle Width="70px" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="ton" HeaderText="Ton (h)" Visible="False"></asp:BoundColumn>
                            <asp:BoundColumn DataField="PotMinUGE" HeaderText="Potência mínima" Visible="False"></asp:BoundColumn>
                            <asp:BoundColumn DataField="IDFormaRampa" HeaderText="ID Forma Rampa" Visible="False"></asp:BoundColumn>
                            <asp:BoundColumn DataField="IDTipoRampa" HeaderText="ID Tipo Rampa" Visible="False"></asp:BoundColumn>
                            <asp:BoundColumn DataField="Lgn_Usuario" HeaderText="Lgn_Usuario" Visible="False"></asp:BoundColumn>
                            <asp:BoundColumn DataField="id_rampaugeconjuntural" HeaderText="id_rampaugeconjuntural" Visible="False"></asp:BoundColumn>
                        </Columns>
                        <PagerStyle NextPageText="Pr&#243;xima&amp;gt;" Font-Size="XX-Small" Font-Names="Arial" Font-Bold="True"
                            PrevPageText="&amp;lt;Anterior"></PagerStyle>
                    </asp:DataGrid>
                </td>
                <td class="auto-style24" rowspan="7">&nbsp;</td>
                <td class="auto-style42" style="border: 2px solid #C0C0C0">&nbsp;<asp:Panel ID="Mostrar" runat="server" Enabled="False" Height="169px" Width="242px">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:Label ID="LblSelRampa1" runat="server" Font-Bold="True" Text="Selecione o Tipo de Rampa"></asp:Label>
                    <br />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:Label ID="LblSelRampa2" runat="server" Font-Bold="True" Text="que deseja importar."></asp:Label>
                    <br />
                    <br />
                    <asp:RadioButton ID="OptSubFrio" runat="server" CausesValidation="True" Font-Bold="False" Font-Names="Arial" Font-Size="X-Small" GroupName="1" Text="Rampa de Subida a Frio (Térmica)" />
                    <br />
                    <asp:RadioButton ID="OptSubQuente" runat="server" Font-Bold="False" Font-Names="Arial" Font-Size="X-Small" GroupName="1" Text="Rampa de Subida a Quente (Térmica)" />
                    <br />
                    <asp:RadioButton ID="OptDescida" runat="server" Font-Bold="False" Font-Names="Arial" Font-Size="X-Small" GroupName="1" Text="Rampa de Descida" />
                    <br />
                    <br />
                    &nbsp;<asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="X-Small" ForeColor="Black" Text="Obs: Ao importar as rampas do GERCAD, os dados do PDP serão substituídos"></asp:Label>
                    <br />
                    <br />
                </asp:Panel>
                </td>
            </tr>
            <tr>
                <td class="auto-style40"></td>
            </tr>
            <tr>
                <td class="auto-style42">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style42"></td>
                <td class="auto-style32" rowspan="4">
                    <br />
                    <br />
                    <br />
                </td>
            </tr>
            <tr>
                <td class="auto-style42" rowspan="4">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style42">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style42"></td>
            </tr>
        </table>
    </form>
</asp:Content>

