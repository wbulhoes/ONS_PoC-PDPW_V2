<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="frmCnsIR1.aspx.vb" Inherits="pdpw.frmCnsIR1" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>ONS - Operador Nacional do Sistema Elétrico</title>
    <meta http-equiv="Content-Type" content="text/html; charset=windows-1252">
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <link href="../pdpw/images/style.css" rel="stylesheet">
    <script language="javascript" src="Lib.js"></script>
</head>
<body bgcolor="#ffffff" leftmargin="0" topmargin="0">
    <table style="width: 792px; height: 249px" height="249" cellspacing="0" cellpadding="0"
        width="792" border="0">
        <tr>
            <td style="width: 17px" width="17">
                <br>
            </td>
            <td style="height: 248px" valign="top">
                <div align="center">
                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                        <tr>
                            <td width="20%" height="2">&nbsp;</td>
                        </tr>
                        <tr>
                            <td style="height: 17px" height="17">
                                <table style="width: 772px; height: 25px" cellspacing="0" cellpadding="0" width="772" background="../pdpw/images/back_tit_sistema.gif"
                                    border="0">
                                    <tr>
                                        <td style="height: 12px">
                                            <script>MontaCabecalho();</script>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 10px" height="10">
                                <table style="width: 771px; height: 27px" cellspacing="0" cellpadding="0" width="771"
                                    border="0">
                                    <tr>
                                        <td style="height: 8px">
                                            <div align="left">
                                                <p class="titulo">Nível de Partida</p>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <br>
                    <form id="frmCnsIR1" name="frmCnsIR1" runat="server">
                        <div id="divtabela" style="left: 80px; width: 599px; position: absolute; top: 120px; height: 134px">
                            <table class="modulo" style="width: 606px; height: 115px" cellspacing="0" cellpadding="0"
                                border="0">
                                <tr valign="top">
                                    <td style="width: 108px; height: 7px"><%if Request.QueryString("strAcesso") <> "PDOC" Then
                                                                                  Response.Write("Dados:")
                                                                              End If%></td>
                                    <td style="width: 306px; height: 7px">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Selecionar:</td>
                                    <td style="height: 7px"></td>
                                    <td style="width: 85px"></td>
                                </tr>
                                <tr>
                                    <td style="width: 108px">
                                        <asp:RadioButtonList ID="optDados" runat="server" Width="118px" Font-Size="XX-Small" AutoPostBack="True">
													<asp:ListItem Value="0" Selected="True">&#193;rea Transfer&#234;ncia</asp:ListItem>
													<asp:ListItem Value="1">Enviados</asp:ListItem>
													<asp:ListItem Enabled="false" Value="2">Consolidados</asp:ListItem>
													<asp:ListItem Value="3">Recebidos DESSEM</asp:ListItem>
													<asp:ListItem Value="4">Consistidos DESSEM</asp:ListItem>
                                        </asp:RadioButtonList></td>
                                    <td style="width: 306px">
                                        <table class="formulario_texto" style="width: 311px; height: 59px" cellspacing="0" cellpadding="0"
                                            border="0">
                                            <tr>
                                                <td style="width: 93px; height: 27px" align="right">
                                                    <asp:Label Style="z-index: 0" ID="lblData" runat="server">Data do PDP</asp:Label>&nbsp;</td>
                                                <td style="width: 209px; height: 27px">
                                                    <asp:DropDownList ID="cboDataInicial" runat="server" Width="90px" AutoPostBack="True"></asp:DropDownList>&nbsp;</td>
                                            </tr>

                                            <tr>
                                                <td style="width: 93px" align="right">Empresa&nbsp;</td>
                                                <td style="width: 209px">
                                                    <asp:DropDownList ID="cboEmpresa" runat="server" Width="199px" AutoPostBack="True"></asp:DropDownList></td>
                                            </tr>
                                            <tr>
                                                <td style="width: 93px" align="right">Valor&nbsp;</td>
                                                <td>
                                                    <input type="text" runat="server" id="vlNvlPartida" disabled/></td>
                                            </tr>
                                        </table>
                                        <br>
                                        <br>
                                        <asp:Label ID="lblMensagem" runat="server" Visible="False"></asp:Label></td>
                                    <td align="center">
                                        <asp:ImageButton ID="btnVisualizar" runat="server" ImageUrl="images/bt_visualizar.gif"></asp:ImageButton></td>
                                    <td align="center" style="width: 85px">
                                        <img id="imgPlanilha" onmouseover="this.style.cursor='hand'" onclick="javascript:botao();"
                                            alt="" src="images/bt_planilha.gif" runat="server"></td>
                                </tr>
                            </table>
                        </div>
                    </form>
                </div>
            </td>
        </tr>
    </table>
    <div style="display: inline; left: 76px; width: 635px; position: absolute; top: 260px; height: 36px"
        align="left" ms_positioning="FlowLayout">
    </div>
</body>
</html>
