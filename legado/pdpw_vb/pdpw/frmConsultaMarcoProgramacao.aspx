<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frmConsultaMarcoProgramacao.aspx.vb" Inherits="pdpw.frmConsultaMarcoProgramacao" %>

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
                                                            <img style="width: 277px; height: 23px" height="23"
                                                                src="../pdpw/images/tit_ConsultaMarcos.GIF">
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
                        <form id="frmConsultaMarcoProgramacao" name="frmConsultaMarcoProgramacao" runat="server">
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
                            <table class="modulo" cellspacing="0" cellpadding="0" border="0" align="center" width="800px">
                                <tr>
                                    <td style="height: 100px" align="center">
                                        <div id="divCabecalho">
                                            <asp:Literal ID="litCabecalho" runat="server" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <table class="modulo" cellspacing="0" cellpadding="0" border="0" align="center" width="800px">
                                <tr>
                                    <td style="height: 20px" colspan="2"></td>
                                </tr>
                            </table>

                            <div id="divMarco">
                                <asp:Literal ID="litMarcos" runat="server" />
                            </div>

                            <table class="modulo" cellspacing="0" cellpadding="0" border="0" align="center" width="800px">
                                <tr>
                                    <td style="height: 30px" colspan="2"></td>
                                </tr>
                            </table>

                            <table class="modulo" cellspacing="0" cellpadding="0" border="0">
                                <tr>
                                    <td colspan="2">
                                        <asp:DataGrid ID="dtgMarcosProgramacao" runat="server" Width="800px"
                                            Font-Size="XX-Small"
                                            Font-Names="Arial" AutoGenerateColumns="False" AllowPaging="True" PageSize="4" Visible="False">
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
                                                            Text='<%# DataBinder.Eval(Container, "DataItem.id_marcopdp") %>'
                                                            Visible="False"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:BoundColumn DataField="dsc_marcoprogpdp" HeaderText="Marco">
                                                    <HeaderStyle Width="180px"></HeaderStyle>
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="id_usuarsolicitante" HeaderText="Usuário Solicitante">
                                                    <HeaderStyle Width="250px"></HeaderStyle>
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="id_usuarresponsavel" HeaderText="Usuário Responsável">
                                                    <HeaderStyle Width="250px" />
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="din_marcopdp" HeaderText="Data Ocorrência">
                                                    <HeaderStyle Width="100px" />
                                                </asp:BoundColumn>
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

                        </form>
                    </div>
                </td>
            </tr>
        </tbody>
    </table>
</asp:Content>

