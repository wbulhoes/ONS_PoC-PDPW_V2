<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frmCnsVazao.aspx.vb" Inherits="pdpw.frmCnsVazao" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>ONS - Operador Nacional do Sistema Elétrico</title>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <meta http-equiv="Content-Type" content="text/html; charset=windows-1252">
    <link href="../pdpw/images/style.css" rel="stylesheet">
    <script language="javascript" src="Lib.js"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table style="width: 788px; height: 216px" height="216" cellspacing="0" cellpadding="0"
        width="788" border="0">
        <tbody>
            <tr>
                <td style="width: 19px" valign="top" width="19">
                    <br>
                </td>
                <td style="width: 741px" valign="top">
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
                                                        <script>MontaCabecalho();</script>
                                                    </td>
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
                                                            <img style="width: 88px; height: 23px" height="23" src="../pdpw/images/tit_ColVazao.gif"
                                                                width="88">
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
                        <form id="frmCnsVazao" name="frmCnsVazao" runat="server">
                            <div id="divtabela" style="left: 45px; position: absolute; top: 120px">
                                <table class="modulo" cellspacing="0" cellpadding="0" border="0">
                                    <tr valign="top">
                                        <td style="width: 143px; height: 9px"><%if Request.QueryString("strAcesso") <> "PDOC" Then
                                                                                      Response.Write("Dados:")
                                                                                  End If%></td>
                                        <td style="width: 99px; height: 9px">&nbsp;&nbsp;&nbsp;&nbsp;Visualizar Por:</td>
                                        <td style="width: 288px; height: 9px">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
												&nbsp;Selecionar:</td>
                                        <td style="width: 77px; height: 9px"></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 143px">
                                            <asp:RadioButtonList ID="optDados" runat="server" Width="146px" AutoPostBack="True" Font-Size="XX-Small">
                                                <asp:ListItem Value="0" Selected="True">&#193;rea Transfer&#234;ncia</asp:ListItem>
                                                <asp:ListItem Value="1">Enviados / Consolidados</asp:ListItem>
                                            </asp:RadioButtonList></td>
                                        <td style="width: 99px">
                                            <asp:RadioButtonList ID="optVisualiza" runat="server" Width="87px" AutoPostBack="True" Font-Size="XX-Small">
                                                <asp:ListItem Value="0" Selected="True">Data do PDP</asp:ListItem>
                                                <asp:ListItem Value="1">Empresa</asp:ListItem>
                                            </asp:RadioButtonList></td>
                                        <td style="width: 288px">
                                            <table class="formulario_texto" style="width: 291px; height: 58px" cellspacing="0" cellpadding="0"
                                                border="0">
                                                <tr>
                                                    <td style="width: 76px; height: 27px" align="right">
                                                        <asp:Label Style="z-index: 0" ID="lblData" runat="server" Width="70px">Data do PDP</asp:Label>&nbsp;</td>
                                                    <td style="width: 203px; height: 27px">
                                                        <asp:DropDownList ID="cboDataInicial" runat="server" Width="90px" AutoPostBack="True"></asp:DropDownList>&nbsp;
															<asp:Label ID="lblLetra" runat="server" Width="11px">  a</asp:Label><asp:DropDownList ID="cboDataFinal" runat="server" Width="90px" AutoPostBack="True"></asp:DropDownList></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 68px; height: 27px" align="right">Empresa&nbsp;</td>
                                                    <td style="width: 203px; height: 27px">
                                                        <asp:DropDownList ID="cboEmpresa" runat="server" Width="149px" AutoPostBack="True"></asp:DropDownList></td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style="width: 77px" align="center">
                                            <asp:ImageButton ID="btnVisualizar" runat="server" ImageUrl="images/bt_Visualizar.gif"></asp:ImageButton></td>
                                        <td align="center">
                                            <img id="imgPlanilha" onmouseover="this.style.cursor='hand'" onclick="javascript:botao();"
                                                alt="" src="images/bt_planilha.gif" runat="server"></td>
                                    </tr>
                                </table>
                            </div>
                        </form>
                    </div>
                </td>
            </tr>
        </tbody>
    </table>
    <div style="display: inline; left: 44px; width: 696px; position: absolute; top: 230px; height: 36px"
        align="left" ms_positioning="FlowLayout">
        <asp:Table ID="tblConsulta" runat="server" Width="561px" Font-Size="X-Small" CellSpacing="0"></asp:Table>
    </div>
</asp:Content>

