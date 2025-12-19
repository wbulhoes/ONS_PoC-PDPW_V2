<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frmUsinaConversora.aspx.vb" Inherits="pdpw.frmUsinaConversora" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Usinas Conversoras</title>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <meta http-equiv="Content-Type" content="text/html; charset=windows-1252">
    <link href="../pdpw/images/style.css" rel="stylesheet">
    <style type="text/css">
        .auto-style1 {
            width: 136px;
            height: 225px;
        }

        .auto-style2 {
            width: 1064px;
            height: 225px;
        }

        .auto-style3 {
            width: 170px;
            height: 46px;
        }

        .auto-style4 {
            width: 750px;
            height: 46px;
        }

        .auto-style5 {
            width: 170px;
            height: 45px;
        }

        .auto-style7 {
            width: 170px;
            height: 41px;
        }

        .auto-style8 {
            width: 881px;
            height: 41px;
        }

        .auto-style9 {
            width: 1078px;
            height: 100px;
        }

        .auto-style10 {
            width: 227px;
            height: 23px;
        }

    </style>
    <script language="javascript" type="text/javascript" src="js/MSGAguarde.js"></script>
    <link href="css/MSGAguarde.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <p>
        <table style="width: 1100px; height: 266px" height="266" cellspacing="0" cellpadding="0"
            width="1100" border="0">
            <tbody>
                <tr>
                    <td valign="top" width="136" class="auto-style1">
                        <br>
                    </td>
                    <td valign="top" class="auto-style2">
                        <div align="center">
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tbody>
                                    <tr>
                                        <td width="20%" height="2">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td height="2">
                                            <table cellspacing="0" cellpadding="0" width="765" background="../pdpw/images/back_tit_sistema.gif"
                                                border="0">
                                                <tbody>
                                                    <tr>
                                                        <td>
                                                            <img height="25" src="../pdpw/images/tit_sis_guideline.gif" width="179"></td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td height="2">
                                            <table cellspacing="0" cellpadding="0" width="765" background="../pdpw/images/back_titulo.gif"
                                                border="0">
                                                <tbody>
                                                    <tr>
                                                        <td>
                                                            <div align="left">
                                                                <img height="23" src="../pdpw/images/tit_ColAssociacao.gif" class="auto-style10">
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                            <form id="frmColGeracao" name="frmColGeracao" runat="server">

                                <asp:Label ID="lblMsg" Visible="False" ForeColor="Green" runat="server" Font-Bold="True" Font-Size="X-Small">Desculpe, este dado não pôde ser carregado no momento.</asp:Label>

                                <br>
                                <table cellspacing="0" cellpadding="0" width="1078" border="0" class="auto-style9">
                                    <tr>
                                        <td align="right" class="auto-style3"><b>Empresa:</b></td>
                                        <td class="auto-style4">
                                            <p>&nbsp;<asp:DropDownList ID="cboEmpresa" runat="server" Width="219px" AutoPostBack="True"></asp:DropDownList>&nbsp;&nbsp;&nbsp;</p>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" class="auto-style7"><b>Usinas:</b></td>
                                        <td class="auto-style8">
                                            <p>
                                                &nbsp;<asp:DropDownList ID="cboUsina" runat="server" Width="219px" AutoPostBack="True"></asp:DropDownList>&nbsp;&nbsp;&nbsp;
                                            </p>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" class="auto-style7"><b>Usina Conversora:</b></td>
                                        <td class="auto-style8">
                                            <p>
                                                &nbsp;<asp:DropDownList ID="cboUsinaConversora" runat="server" Width="264px" AutoPostBack="True" Height="18px"></asp:DropDownList>&nbsp;&nbsp;&nbsp;
													
                                            </p>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" class="auto-style5"><b>Percentual de Perda:</b></td>
                                        <td>
                                            <p>
                                                &nbsp;
                                                    <asp:TextBox ID="txtPercentualPerda" runat="server" Width="59px" ></asp:TextBox>&nbsp;&nbsp;<asp:Label ID="lblPercentual" Text="%" runat="server"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
                                            </p>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" class="auto-style5"><b>Prioridade:</b></td>
                                        <td>
                                            <p>
                                                &nbsp;
                                                    <asp:TextBox ID="txtPrioridade" runat="server" Width="59px"></asp:TextBox>&nbsp;&nbsp;<asp:Label ID="Label1" Text=" " runat="server"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:ImageButton ID="btnSalvar" runat="server" ImageUrl="images/bt_salvar.gif"></asp:ImageButton>&nbsp;&nbsp;
                                            </p>
                                        </td>
                                    </tr>
                                </table>
                        </div>
                        <tr>
                            <td style="width: 10px" valign="top" align="right">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            </td>
                            <td style="width: 750px; padding-left: 50px;" valign="top">
                                <%--<asp:table id="tblUsinaConversora" runat="server" Width="51px" BorderStyle="Ridge" BorderWidth="1px"
								CellPadding="2" CellSpacing="0" GridLines="Both" Height="26px" Font-Size="Smaller" BackColor="#99cc00"></asp:table>--%>
                                <div>
                                    <asp:GridView ID="UsinaConversoraGridView"
                                        AutoGenerateColumns="False"
                                        SelectedIndex="1"
                                        OnRowDataBound="UsinaConversoraGridView_RowDataBound"
                                        OnRowDeleting="UsinaConversoraGridView_RowDeleting"
                                        runat="server"
                                        DataKeyNames="id_usinaconversora" BorderStyle="None" CellPadding="6" CellSpacing="4" Font-Bold="False">

                                        <Columns>
                                            <asp:BoundField DataField="usina"
                                                HeaderText="Usina"
                                                ReadOnly="True"
                                                SortExpression="Usina" />
                                            <asp:BoundField DataField="conversora"
                                                HeaderText="Conversora"
                                                SortExpression="FirstName" />
                                            <asp:BoundField DataField="perda"
                                                HeaderText="Perda %"
                                                SortExpression="Perda" />
                                            <asp:BoundField DataField="prioridade" HeaderText="Prioridade" SortExpression="Prioridade" />
                                            <asp:CommandField ShowDeleteButton="true" ButtonType="Image" DeleteImageUrl="images/bt_excluir.gif" HeaderText="Deletar" />
                                        </Columns>


                                        <HeaderStyle BackColor="#99CC00" Font-Bold="True" />


                                        <PagerStyle Width="10px" />


                                    </asp:GridView>
                                </div>
                                <div id="divValor" style="display: inline; left: 180px; width: 82px; position: absolute; top: 300px; height: 21px"
                                    runat="server" ms_positioning="FlowLayout">
                                </div>
                            </td>
                        </tr>
            </tbody>
        </table>
        </FORM>
			<div></div>
        </TD></TR></TBODY></TABLE>
		<p></p>
</asp:Content>

