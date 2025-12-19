<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frmInflxContratadaModal.aspx.vb" Inherits="pdpw.frmInflxContratadaModal" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>ONS - Operador Nacional do Sistema Elétrico</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="../pdpw/images/style.css" rel="stylesheet">

    <style type="text/css">
        .auto-style1 {
            height: 23px;
        }

        .auto-style4 {
            height: 50px;
        }

        .auto-style11 {
            height: 20px;
        }

        .auto-style12 {
            width: 80px;
        }

        .auto-style13 {
            width: 181px;
        }

        .auto-style15 {
            height: 15px;
            width: 33px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="frmInflxContratadaModal" name="frmInflxContratadaModal" runat="server">


        <div class="row">
            <script>
                function btnSalvar() {
                    alert("Registro salvo com sucesso!");
                }
            </script>
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <h3 class="panel-title" bgcolor="#99CC00">
                        <meta charset="utf-8">
                        <table border="0.5px" left="20" width="654" bgcolor="#99CC00">
                            <tr>

                                <td class="auto-style12">
                                    <asp:Label ID="LabelTipo" runat="server" Text=""></asp:Label></td>
                                <td class="auto-style13">Inflexbilidade Contratada: </td>
                                <td>
                                    <asp:Label ID="LabelUsina" runat="server" Text=""></asp:Label></td>
                            </tr>
                        </table>
                    </h3>
                </div>
                <div class="panel-body">
                    <table class="table table-bordered">
                        <tr>
                            <td class="auto-style1">
                                <font color="#AA0000">*</font>
                                <label class="editor-label" title="">
                                    Nome Usina:
                                </label>
                            </td>
                            <td class="auto-style1">
                                <font color="#AA0000">
                                    <asp:DropDownList ID="cboUsina" runat="server" Height="16px" Width="221px">
                                    </asp:DropDownList>
                                    *</font>
                            </td>
                        </tr>
                        <td>
                            <font color="#AA0000">*</font>
                            <label class="editor-label" title="">
                                Início de Vigência:
                            </label>
                            <td>
                                <%--<input type="date" id="data_inicial" required>--%>
                                <asp:TextBox ID="data_inicial" runat="server"></asp:TextBox>
                                <asp:Button ID="btnCalendario" runat="server" Width="21px" Height="20px" Text="..."></asp:Button>
                                <div id="divCal" style="display: inline; left: 385px; width: 220px; position: absolute; top: 88px; height: 200px"
                                    runat="server" ms_positioning="FlowLayout">
                                    <asp:Calendar ID="calData" runat="server" Width="220px" Font-Size="8pt" Height="200px" Font-Bold="True"
                                        BackColor="Beige" ShowGridLines="True" BorderColor="Black" ForeColor="DarkBlue" BorderWidth="1px"
                                        Font-Names="Arial">
                                        <%-- <TodayDayStyle BackColor="LightGreen"></TodayDayStyle>--%>
                                        <SelectorStyle BackColor="YellowGreen"></SelectorStyle>
                                        <NextPrevStyle Font-Size="9pt" ForeColor="Black"></NextPrevStyle>
                                        <DayHeaderStyle Height="1px" BorderColor="Black" BackColor="PaleGoldenrod"></DayHeaderStyle>
                                        <SelectedDayStyle Font-Bold="True" BackColor="YellowGreen"></SelectedDayStyle>
                                        <TitleStyle Font-Size="9pt" Font-Bold="True" ForeColor="Black" BorderColor="#404040" BackColor="YellowGreen"></TitleStyle>
                                        <WeekendDayStyle BackColor="LemonChiffon"></WeekendDayStyle>
                                        <OtherMonthDayStyle ForeColor="Silver"></OtherMonthDayStyle>
                                    </asp:Calendar>
                                </div>
                            </td>
                        </td>
                        <tr>
                            <td class="auto-style1">
                                <font color="#AA0000">*</font>
                                <label class="editor-label" title="">
                                    Fim de Vigência:
                                </label>
                                <td>
                                    <asp:TextBox ID="data_final" runat="server"></asp:TextBox>
                                    <asp:Button ID="btnCalendario2" runat="server" Width="21px" Height="20px" Text="..."></asp:Button>
                                    <div id="divCal2" style="display: inline-table; left: 385px; width: 220px; position: absolute; top: 110px; height: 200px"
                                        runat="server" ms_positioning="FlowLayout">
                                        <asp:Calendar ID="calData2" runat="server" Width="220px" Font-Size="8pt" Height="200px" Font-Bold="True"
                                            BackColor="Beige" ShowGridLines="True" BorderColor="Black" ForeColor="DarkBlue" BorderWidth="1px"
                                            Font-Names="Arial">
                                            <%-- <TodayDayStyle BackColor="LightGreen"></TodayDayStyle>--%>
                                            <SelectorStyle BackColor="YellowGreen"></SelectorStyle>
                                            <NextPrevStyle Font-Size="9pt" ForeColor="Black"></NextPrevStyle>
                                            <DayHeaderStyle Height="1px" BorderColor="Black" BackColor="PaleGoldenrod"></DayHeaderStyle>
                                            <SelectedDayStyle Font-Bold="True" BackColor="YellowGreen"></SelectedDayStyle>
                                            <TitleStyle Font-Size="9pt" Font-Bold="True" ForeColor="Black" BorderColor="#404040" BackColor="YellowGreen"></TitleStyle>
                                            <WeekendDayStyle BackColor="LemonChiffon"></WeekendDayStyle>
                                            <OtherMonthDayStyle ForeColor="Silver"></OtherMonthDayStyle>
                                        </asp:Calendar>
                                    </div>
                                </td>
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td><font color="#AA0000">*</font> Valor da Inflexibilidade Contratada:</td>
                            <td colspan="2">
                                <asp:TextBox ID="valorcontratado" runat="server" type="text"></asp:TextBox>
                            </td>
                        </tr>
                        <td>
                            <font color="#AA0000">*</font>
                            <label class="editor-label" title="">
                                Modelo de Contrato:
                            </label>
                        </td>
                        <td>&nbsp;<asp:DropDownList class="col-sm-3 form-control" DataValueField="" ID="contrato" runat="server">
                            <asp:ListItem DataValueField="" Selected="True">Selecione... </asp:ListItem>
                            <asp:ListItem DataValueField="1">Anteriores a 2011 </asp:ListItem>
                            <asp:ListItem DataValueField="2">Posteriores a 2011 </asp:ListItem>
                        </asp:DropDownList>

                        </td>
                        <tr>
                            <td colspan="1">

                                <font color="#AA0000">*</font>
                                <label class="editor-label" title="">
                                    Registro habilitado:
                                </label>

                            </td>
                            <td>
                                <asp:CheckBox type="checkbox" runat="server" ID="habilitado" /></td>
                        </tr>
                        <table border="0" left="20" width="654" bgcolor="#FFFFF" class="auto-style4">
                            <tr>
                                <br></br>
                                <td colspan="3">
                                    <div class="auto-style11">
                                        <asp:ImageButton ID="Button1" ImageAlign="Left" runat="server" ImageUrl="images/bt_salvar.gif" OnClick="btnSalvar_Click" />

                                        <asp:ImageButton ID="Button2" ImageAlign="Right" Text=" " runat="server" ImageUrl="images/bt_cancelar.gif" OnClientClick="window.close(); return false" />

                                    </div>
                                </td>
                            </tr>
                        </table>
                </div>
                <table border="0.5px" left="20" width="654" bgcolor="#99CC00">
                    <td></td>
                </table>
            </div>
        </div>
    </form>
</asp:Content>

