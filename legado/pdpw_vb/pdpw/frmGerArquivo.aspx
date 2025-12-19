<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frmGerArquivo.aspx.vb" Inherits="pdpw.frmGerArquivo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>ONS - Operador Nacional do Sistema Elétrico</title>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <meta http-equiv="Content-Type" content="text/html; charset=windows-1252">
    <link href="../pdpw/images/style.css" rel="stylesheet">
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
                                    <td style="height: 10px" height="10">
                                        <table cellspacing="0" cellpadding="0" width="765" background="../pdpw/images/back_titulo.gif"
                                            border="0">
                                            <tbody>
                                                <tr>
                                                    <td style="height: 8px">
                                                        <div align="left">
                                                            <img style="width: 112px; height: 23px" height="23" src="../pdpw/images/tit_Arquivo.gif"
                                                                width="112">
                                                        </div>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <form id="frmGerArquivo" name="frmGerArquivo" runat="server">

                            <asp:Label ID="lblMsg" Visible="False" ForeColor="Green" runat="server" Font-Bold="True" Font-Size="X-Small">Desculpe, este dado não pôde ser carregado no momento.</asp:Label>

                            <br />
                            <table>
                                <tr>
                                    <td>
                                        <label>Obs.: Os arquivos gerados devem ser recuperados em até 24 horas.</label>
                                    </td>
                                </tr>
                            </table>
                            <table class="modulo" style="width: 481px; height: 105px" cellspacing="0" cellpadding="0"
                                border="0">
                                <tr valign="top">
                                    <td style="width: 241px; height: 4px">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
											Selecionar:</td>
                                    <td style="width: 158px; height: 4px">&nbsp;&nbsp; Origem dos dados:</td>
                                    <td style="height: 4px"></td>
                                </tr>
                                <tr>
                                    <td style="width: 241px; height: 36px">
                                        <table class="formulario_texto" style="width: 232px; height: 96px" cellspacing="0" cellpadding="0"
                                            border="0">
                                            <tr>
                                                <td style="width: 90px; height: 24px" valign="top" align="right">
                                                    <br>
                                                    Seleção por&nbsp;</td>
                                                <td style="height: 24px">
                                                    <asp:RadioButtonList ID="optSelecao" runat="server" AutoPostBack="True" Width="143px" RepeatDirection="Horizontal"
                                                        Height="40px" Font-Size="XX-Small">
                                                        <asp:ListItem Value="E" Selected="True">Empresa</asp:ListItem>
                                                        <asp:ListItem Value="A">&#193;rea</asp:ListItem>
                                                    </asp:RadioButtonList></td>
                                            </tr>
                                            <tr>
                                                <td style="width: 90px" align="right">Data do PDP&nbsp;</td>
                                                <td>
                                                    <asp:DropDownList ID="cboDataPdp" runat="server" Width="148px"></asp:DropDownList></td>
                                            </tr>
                                            <tr>
                                                <td style="width: 90px" align="right">Empresa&nbsp;</td>
                                                <td>
                                                    <asp:DropDownList ID="cboEmpresa" runat="server" Width="148px"></asp:DropDownList></td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td style="width: 158px">
                                        <asp:RadioButtonList ID="optEnvia" runat="server" Height="50px" Font-Size="XX-Small" Width="170px">
                                            <asp:ListItem Value="0">&#193;rea de Transfer&#234;ncia</asp:ListItem>
                                            <asp:ListItem Value="1">Dados Enviados</asp:ListItem>
                                            <asp:ListItem Value="2">Dados Consolidados</asp:ListItem>
                                            <asp:ListItem Value="4">Dados Modelo DESSEM</asp:ListItem>
                                            <asp:ListItem Value="5">Dados Consolidados DESSEM</asp:ListItem>
                                        </asp:RadioButtonList><br>
                                    </td>
                                    <td style="height: 36px" align="center">
                                        <asp:ImageButton ID="btnGravar" runat="server" ImageUrl="images/bt_Gravar.gif"></asp:ImageButton></td>
                                </tr>
                            </table>
                        </form>
                    </div>
                </td>
            </tr>
        </tbody>
    </table>
</asp:Content>

