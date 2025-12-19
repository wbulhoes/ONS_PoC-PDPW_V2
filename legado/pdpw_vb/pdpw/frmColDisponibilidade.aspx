<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frmColDisponibilidade.aspx.vb" Inherits="pdpw.frmColDisponibilidade" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>ONS - Operador Nacional do Sistema Elétrico</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <link href="../pdpw/images/style.css" rel="stylesheet">
    <script language="JavaScript">
        function RetiraEnter(teclapres) {
            var tecla = teclapres.keyCode;
            if (tecla == 13) {
                // Retira Caracteres especiais (ENTER, TAB, etc...) quando existirem 2 seguidos em qualquer lugar do texto
                vr = escape(document.frmColEnergiaRepPer.txtValor.value);
                //TAB + ENTER
                vr = vr.replace('%09%0D%0A', '%09')
                //ENTER + TAB
                vr = vr.replace('%0D%0A%09', '%09')
                //ENTER + ENTER
                vr = unescape(vr.replace('%0D%0A%0D%0A', '%0D%0A'));
                document.frmColEnergiaRepPer.txtValor.value = vr;
            }
        }
    </script>
    <style type="text/css">
        .auto-style1 {
            height: 250px;
            width: 27px;
        }

        .auto-style2 {
            width: 257px;
            height: 40px;
        }

        .auto-style7 {
            width: 257px;
            height: 79px;
        }

        .auto-style8 {
            width: 170px;
            height: 2px;
        }

        .auto-style9 {
            width: 257px;
            height: 2px;
        }

        .auto-style10 {
            left: 19px;
            width: 1028px;
            position: absolute;
            top: 416px;
            height: 8px;
        }

        .auto-style11 {
            height: 21px;
            width: 170px;
        }

        .auto-style12 {
            height: 24px;
            width: 170px;
        }

        .auto-style13 {
            width: 750px;
            height: 21px;
        }

        .auto-style14 {
            width: 170px;
            height: 79px;
        }

        .auto-style15 {
            width: 750px;
            height: 49px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table style="width: 1098px; height: 112px" height="112" cellspacing="0" cellpadding="0"
        width="1098" border="0">
        <tr>
            <td valign="top" class="auto-style1">
                <br>
            </td>
            <td style="width: 1064px; height: 250px" valign="top">
                <div align="center">
                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                        <tr>
                            <td width="20%" height="2">&nbsp;</td>
                        </tr>
                        <tr>
                            <td style="height: 17px" height="17">
                                <table cellspacing="0" cellpadding="0" width="765" background="../pdpw/images/back_tit_sistema.gif"
                                    border="0">
                                    <tr>
                                        <td style="height: 12px">
                                            <img height="25" src="../pdpw/images/tit_sis_guideline.gif" width="179"></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 10px" height="10">
                                <table cellspacing="0" cellpadding="0" width="765" background="../pdpw/images/back_titulo.gif"
                                    border="0">
                                    <tr>
                                        <td style="height: 8px">
                                            <div align="left">
                                                <img style="width: 152px; height: 23px" height="23" src="../pdpw/images/tit_ColDSP.gif"
                                                    width="152">
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <form id="frmColEnergiaRepPer" name="frmColEnergiaRepPer" runat="server">

                        <asp:Label ID="lblMsg" Visible="False" ForeColor="Green" runat="server" Font-Size="X-Small" Font-Bold="True">Desculpe, este dado não pôde ser carregado no momento.</asp:Label>

                        <br>
                        <table style="width: 1078px; height: 64px" height="64" cellspacing="0" cellpadding="0"
                            width="1078" border="0">
                            <tr height="30">
                                <td align="right" class="auto-style8">&nbsp;</td>
                                <td class="auto-style2">
                                    <asp:Label ID="lblUsinaSelecionada" runat="server" Font-Bold="True" Font-Size="Medium" ForeColor="#FF3300" Visible="False" Width="533px"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" class="auto-style14">&nbsp;</td>
                                <td class="auto-style7">
                                    <asp:Panel ID="Panel1" runat="server" BorderColor="#BFDEB6" BorderStyle="Solid" BorderWidth="2px" Height="74px" Width="404px">
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:Label ID="Label1" runat="server" Font-Bold="True" Text="Selecione o Tipo de Usina para enviar os dados"></asp:Label>
                                        <br />
                                        <br />
                                        &nbsp;&nbsp;
                                            <asp:RadioButton ID="OptTermica" runat="server" GroupName="1" Text="Usinas Térmicas" />
                                        &nbsp;&nbsp;&nbsp;
                                            <asp:RadioButton ID="OptHidraulica" runat="server" GroupName="1" Text="Usinas Hidráulicas" />
                                        &nbsp;&nbsp;&nbsp; &nbsp;<asp:ImageButton ID="btnSelecionarUsina" runat="server" ImageUrl="~/images/bt_confirmar.gif" />
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr height="30">
                                <td align="right" class="auto-style8">&nbsp;</td>
                                <td style="width: 257px; height: 2px">&nbsp;</td>
                            </tr>
                            <tr>
                                <td align="right" class="auto-style8"><b>Data PDP:</b></td>
                                <td class="auto-style9">&nbsp;<asp:DropDownList ID="cboData" runat="server" AutoPostBack="True" Width="98px" Enabled="False"></asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td align="right" class="auto-style11"><b>Empresa:</b></td>
                                <td class="auto-style13">&nbsp;<asp:DropDownList ID="cboEmpresa" runat="server" AutoPostBack="True" Width="219px" Enabled="False"></asp:DropDownList></td>
                            </tr>
                            <tr height="30">
                                <td align="right" class="auto-style12"><b>Usinas:</b></td>
                                <td style="width: 881px; height: 24px">&nbsp;<asp:DropDownList ID="cboUsina" runat="server" AutoPostBack="True" Width="219px" Enabled="False"></asp:DropDownList>&nbsp;&nbsp;&nbsp;
										<asp:ImageButton ID="btnSalvar" runat="server" ImageUrl="images/bt_salvar.gif"></asp:ImageButton></td>
                            </tr>
                        </table>
                        <div class="auto-style10">
                            <table style="width: 1026px; height: 51px">
                                <tr>
                                    <td valign="top" class="auto-style15">
                                        <asp:Table ID="tblDSP" runat="server" Width="51px" Font-Size="Smaller" Height="26px" GridLines="Both"
                                            CellSpacing="0" CellPadding="2" BorderWidth="1px" BorderStyle="Ridge">
                                        </asp:Table>
                                        <div id="divValor" style="display: inline; left: 180px; width: 82px; position: absolute; top: 270px; height: 21px"
                                            runat="server" ms_positioning="FlowLayout">
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </form>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>

