<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frmReciboEst.aspx.vb" Inherits="pdpw.frmReciboEst" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>ONS - Operador Nacional do Sistema Elétrico</title>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="../pdpw/images/style.css" rel="stylesheet">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table style="width: 696px; height: 200px" height="200" cellspacing="0" cellpadding="0"
        width="696" border="0">
        <tbody>
            <tr>
                <td style="width: 15px; height: 100px" valign="top" width="15">
                    <br>
                </td>
                <td valign="top" style="width: 655px">
                    <div align="center">
                    </div>
                    <div align="center">
                        <br>
                        <form id="frmReciboEst" name="frmReciboEst" runat="server">
                            <asp:Panel ID="Panel1" runat="server" BorderStyle="Double" Height="88px" Width="674px">
                                <table id="Table1" style="width: 665px; height: 112px" cellspacing="1" cellpadding="1"
                                    width="665" border="0">
                                    <tr>
                                        <td style="width: 296px; height: 20px">
                                            <asp:Image ID="Image1" runat="server" Width="272px" Height="41px" ImageUrl="/IntUnica/images/logo_top.jpg"
                                                ImageAlign="Bottom"></asp:Image></td>
                                        <td style="height: 20px" align="center">
                                            <asp:Label ID="lblRecibo" runat="server" Width="230px" Font-Size="X-Small" Font-Bold="True"
                                                Font-Names="Arial">RECIBO DO ENVIO DOS DADOS</asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 296px; height: 20px">
                                            <asp:Label ID="lblEmpresa" runat="server" Width="45px" Font-Size="XX-Small" Font-Names="Arial">Empresa: </asp:Label>
                                            <asp:Label ID="lblEmpresaValor" runat="server" Width="210px" Font-Size="XX-Small" Font-Names="Arial"></asp:Label></td>
                                        <td style="height: 17px">
                                            <asp:Label ID="lblDataPdp" runat="server" Width="64px" Font-Size="XX-Small" Font-Names="Arial">Data do PDP: </asp:Label>
                                            <asp:Label ID="lblDataPdpValor" runat="server" Width="103px" Font-Size="XX-Small" Font-Names="Arial"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 296px; height: 21px">
                                            <asp:Label ID="lblUsuario" runat="server" Width="39px" Font-Size="XX-Small" Font-Names="Arial">Usuário:</asp:Label>
                                            <asp:Label ID="lblUsuarioValor" runat="server" Font-Size="XX-Small" Font-Names="Arial"></asp:Label></td>
                                        <td style="height: 21px">
                                            <asp:Label ID="lblDataEnvio" runat="server" Width="104px" Font-Size="XX-Small" Font-Names="Arial">Data e hora do Envio:</asp:Label>
                                            <asp:Label ID="lblDataEnvioValor" runat="server" Width="85px" Font-Size="XX-Small" Font-Names="Arial"></asp:Label>&nbsp;
												<asp:Label ID="lblHoraEnvioValor" runat="server" Width="33px" Font-Size="XX-Small" Font-Names="Arial"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 296px; height: 20px">
                                            <asp:Label ID="lblArquivo" runat="server" Font-Size="XX-Small" Font-Names="Arial">Arquivo:</asp:Label>
                                            <asp:Label ID="lblArquivoValor" runat="server" Width="217px" Font-Size="XX-Small" Font-Names="Arial"></asp:Label></td>
                                        <td style="height: 19px">
                                            <asp:Label ID="lblSituacao" runat="server" Font-Size="XX-Small" Font-Names="Arial">Situação:</asp:Label>
                                            <asp:Label ID="lblSituacaoValor" runat="server" Width="150px" Font-Size="XX-Small" Font-Names="Arial"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 296px; height: 24px"></td>
                                        <td style="height: 24px"></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 296px; height: 20px">
                                            <asp:Label ID="lblCarga" runat="server" Font-Size="XX-Small" Font-Names="Arial">Carga:</asp:Label>
                                            <asp:Label ID="lblCargaValor" runat="server" Width="150px" Font-Size="XX-Small" Font-Names="Arial"></asp:Label></td>
                                        <td style="height: 17px">
                                            <asp:Label ID="lblInter" runat="server" Font-Size="XX-Small" Font-Names="Arial">Intercâmbio:</asp:Label>
                                            <asp:Label ID="lblInterValor" runat="server" Width="106px" Font-Size="XX-Small" Font-Names="Arial"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 296px; height: 20px">
                                            <asp:Label ID="lblDespa" runat="server" Font-Size="XX-Small" Font-Names="Arial">Geração:</asp:Label>
                                            <asp:Label ID="lblDespaValor" runat="server" Width="150px" Font-Size="XX-Small" Font-Names="Arial"></asp:Label>
                                        </td>
                                        <td style="height: 17px"></td>
                                    </tr>
                                    <tr>
                                        <td style="height: 60px" align="center" colspan="2">
                                            <asp:ImageButton ID="btnImprimir" runat="server" ImageUrl="images/bt_imprimir.gif"></asp:ImageButton></td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </form>
                    </div>
                </td>
            </tr>
        </tbody>
    </table>
</asp:Content>

