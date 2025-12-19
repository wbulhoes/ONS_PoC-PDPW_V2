<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="OldfrmManutencaoResponsaveis.aspx.vb" Inherits="pdpw.frmManutencaoResponsaveis" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>ONS - Operador Nacional do Sistema Elétrico</title>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <meta http-equiv="Content-Type" content="text/html; charset=windows-1252">
    <link href="../pdpw/images/style.css" rel="stylesheet">
    <script language="javascript">

        function Mascara_Data(CampoData) {
            var data = CampoData.value;

            if (data.length == 2) {
                data = data + '/';
                CampoData.value = data;
            }
            if (data.length == 5) {
                data = data + '/';
                CampoData.value = data;
            }
        }

        function Mascara_Hora(CampoHora) {
            var hora01 = '';
            hora01 = hora01 + CampoHora.value;
            if (hora01.length == 2) {
                hora01 = hora01 + ':';
                CampoHora.value = hora01;
            }
        }

    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table style="width: 831px; height: 584px" cellspacing="0" cellpadding="0" border="0">
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
                                    <td style="height: 24px" height="24">
                                        <table style="width: 764px; height: 23px" cellspacing="0" cellpadding="0" width="764" background="../pdpw/images/back_titulo.gif"
                                            border="0">
                                            <tbody>
                                                <tr>
                                                    <td style="width: 762px; height: 8px">
                                                        <div align="left">
                                                            <img style="width: 337px; height: 23px" height="23"
                                                                src="../pdpw/images/tit_CadastroRespPdp.GIF">
                                                        </div>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <br>
                        <form id="frmManutencaoResponsaveis" name="frmManutencaoResponsaveis" runat="server">
                            <table class="modulo" cellspacing="0" cellpadding="0" border="0" width="800">
                                <tr>
                                    <td style="width: 81px" align="right">Data PDP:&nbsp;</td>
                                    <td>
                                        <asp:DropDownList ID="cboData" runat="server" Width="97px" AutoPostBack="True"></asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td style="height: 20px" colspan="2"></td>
                                </tr>
                            </table>
                            <table class="modulo" cellspacing="0" cellpadding="0" border="0">
                                <tr>
                                    <td colspan="2">
                                        <asp:DataGrid ID="dtgResponsaveis" runat="server" Width="800px"
                                            Font-Size="XX-Small"
                                            Font-Names="Arial" AutoGenerateColumns="False" AllowPaging="True" PageSize="4">
                                            <SelectedItemStyle BackColor="Lavender"></SelectedItemStyle>
                                            <AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
                                            <ItemStyle BackColor="#E9F4CF"></ItemStyle>
                                            <HeaderStyle Font-Bold="True" BackColor="YellowGreen"></HeaderStyle>
                                            <Columns>
                                                <asp:TemplateColumn Visible="False">
                                                    <HeaderStyle Width="20px"></HeaderStyle>
                                                    <ItemStyle Width="10px"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkMarca" runat="server" />
                                                        <asp:Label ID="lblObjId" runat="server"
                                                            Text='<%# DataBinder.Eval(Container, "DataItem.id_responsavelprogpdp") %>'
                                                            Visible="False"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:BoundColumn DataField="usuar_nome" HeaderText="Usuário">
                                                    <HeaderStyle Width="280px"></HeaderStyle>
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="nom_equipepdp" HeaderText="Equipe">
                                                    <HeaderStyle Width="120px"></HeaderStyle>
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="tip_programacaoDescricao" HeaderText="Tipo Programação">
                                                    <HeaderStyle Width="100px" />
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="din_inicioprogpdp" HeaderText="Início">
                                                    <HeaderStyle Width="120px" />
                                                </asp:BoundColumn>
                                                <asp:TemplateColumn HeaderText="Fim">
                                                    <EditItemTemplate>
                                                        <asp:TextBox runat="server"
                                                            Text='<%# DataBinder.Eval(Container, "DataItem.din_fimprogpdp") %>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDataFim" runat="server"
                                                            Text='<%# DataBinder.Eval(Container, "DataItem.din_fimprogpdp") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:ButtonColumn Text="excluir" CommandName="Delete">
                                                    <HeaderStyle Width="40px"></HeaderStyle>
                                                    <ItemStyle ForeColor="Gray"></ItemStyle>
                                                </asp:ButtonColumn>
                                            </Columns>
                                            <PagerStyle NextPageText="Pr&#243;xima&amp;gt;" Font-Size="XX-Small" Font-Names="Arial" Font-Bold="True"
                                                PrevPageText="&amp;lt;Anterior"></PagerStyle>
                                        </asp:DataGrid>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 20px" colspan="2"></td>
                                </tr>
                            </table>
                            <table class="modulo" cellspacing="0" cellpadding="0" border="0" align="center" width="800px">
                                <tr>
                                    <td style="width: 798px" align="center">
                                        <asp:ImageButton ID="btnIncluir" runat="server" Width="71px" Height="25px" ImageUrl="../pdpw/images/bt_incluir.gif"></asp:ImageButton>
                                    </td>
                                    <td style="width: 1px">
                                        <asp:ImageButton ID="btnAlterar" runat="server" Width="71px" Height="25px" ImageUrl="../pdpw/images/bt_alterar.gif"
                                            Visible="False"></asp:ImageButton>
                                    </td>
                                    <td style="width: 1px">
                                        <asp:ImageButton ID="btnExcluir" runat="server"
                                            Width="71px" Height="25px" ImageUrl="../pdpw/images/bt_excluir.gif"
                                            Visible="False"></asp:ImageButton></td>
                                </tr>
                                <tr>
                                    <td style="height: 30px" colspan="2">
                                        <asp:HiddenField ID="hiddenAcao" runat="server" />
                                    </td>
                                </tr>
                            </table>
                            <table class="modulo" cellspacing="0" cellpadding="0" border="1" bordercolor="#e9f4cf">
                                <tr>
                                    <td style="height: 20px" colspan="2"></td>
                                </tr>
                                <tr>
                                    <td style="width: 140px" align="right">Equipe:&nbsp;</td>
                                    <td style="height: 25px">
                                        <asp:DropDownList ID="cboEquipe" runat="server" AutoPostBack="True"
                                            Width="219px" Font-Size="Smaller">
                                        </asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td style="width: 140px" align="right">Usuário Responsável:&nbsp;</td>
                                    <td style="height: 25px">
                                        <asp:DropDownList ID="cboUsuario" runat="server"
                                            Width="372px" Font-Size="Smaller">
                                        </asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td style="width: 140px" align="right">Tipo de Programação:&nbsp;</td>
                                    <td style="height: 25px">
                                        <asp:RadioButtonList ID="optTipoOperacao" runat="server" Width="163px"
                                            AutoPostBack="False" Height="22px" RepeatDirection="Horizontal"
                                            Font-Size="Smaller">
                                            <asp:ListItem Value="L">Elétrica</asp:ListItem>
                                            <asp:ListItem Value="N">Energética</asp:ListItem>
                                        </asp:RadioButtonList></td>
                                </tr>
                                <tr>
                                    <td style="width: 140px" align="right">Data Início Programação:&nbsp;</td>
                                    <td style="height: 25px">
                                        <asp:TextBox ID="txtDataInicio" onkeyup="Mascara_Data(this)" runat="server" Height="21px" Width="91px" MaxLength="10"></asp:TextBox>&nbsp;&nbsp; 
														<asp:TextBox ID="txtHoraInicio" onkeyup="Mascara_Hora(this)" runat="server" Height="21px" Width="60px"
                                                            MaxLength="5"></asp:TextBox>

                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 140px" align="right">&nbsp;</td>
                                    <td style="height: 25px">
                                        <asp:TextBox ID="txtDataFim" onkeyup="Mascara_Data(this)"
                                            runat="server" Height="21px" Width="91px" MaxLength="10" Visible="False"></asp:TextBox>&nbsp;&nbsp; 
														<asp:TextBox ID="txtHoraFim" onkeyup="Mascara_Hora(this)"
                                                            runat="server" Height="21px" Width="60px"
                                                            MaxLength="5" Visible="False"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 20px" colspan="2"></td>
                                </tr>
                            </table>
                            <table class="modulo" cellspacing="0" cellpadding="0" border="0" align="center">
                                <tr>
                                    <td style="width: 81px">
                                        <asp:ImageButton ID="btnSalvar" runat="server" Width="71px"
                                            Height="25px" ImageUrl="../pdpw/images/bt_salvar.gif" Visible="False"></asp:ImageButton></td>
                                    <td style="width: 81px">
                                        <asp:ImageButton ID="btnCancelar" runat="server"
                                            Width="71px" Height="25px" ImageUrl="../pdpw/images/bt_cancelar.gif"
                                            Visible="False"></asp:ImageButton></td>
                                </tr>
                            </table>

                        </form>
                    </div>
                </td>
            </tr>
        </tbody>
    </table>
</asp:Content>
