<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frmRecibo.aspx.vb" Inherits="pdpw.frmRecibo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>ONS - Operador Nacional do Sistema Elétrico</title>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="../pdpw/images/style.css" rel="stylesheet">
    <style type="text/css">
        .auto-style1 {
            height: 19px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="frmRecibo" name="frmRecibo" runat="server">

        <table style="border-style: double; margin: 5px 5px 5px 5px;">
            <tr>
                <td>
                    <asp:Image ID="Image1" runat="server" Width="272px" Height="41px" ImageUrl="/IntUnica/images/logo_top.jpg" ImageAlign="Bottom"></asp:Image>
                </td>
                <td align="center">
                    <asp:Label ID="lblRecibo" runat="server" Width="230px" Font-Size="X-Small" Font-Bold="True" Font-Names="Arial">RECIBO DO ENVIO DOS DADOS</asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblempresa" runat="server" Width="45px" Font-Size="xx-small" Font-Names="arial">Empresa: </asp:Label>
                    <asp:Label ID="lblempresavalor" runat="server" Width="210px" Font-Size="xx-small"
                        Font-Names="arial"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lbldatapdp" runat="server" Width="64px" Font-Size="xx-small" Font-Names="arial">Data do PDP: </asp:Label>
                    <asp:Label ID="lbldatapdpvalor" runat="server" Width="103px" Font-Size="xx-small"
                        Font-Names="arial"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblusuario" runat="server" Width="39px" Font-Size="xx-small" Font-Names="arial">Usuário:</asp:Label>
                    <asp:Label ID="lblusuariovalor" runat="server" Font-Size="xx-small" Font-Names="arial"></asp:Label>
                </td>
                <td style="height: 21px">
                    <asp:Label ID="lbldataenvio" runat="server" Width="104px" Font-Size="xx-small" Font-Names="arial">Data e hora do envio:</asp:Label>
                    <asp:Label ID="lbldataenviovalor" runat="server" Width="85px" Font-Size="xx-small"
                        Font-Names="arial"></asp:Label>&nbsp;
                        <asp:Label ID="lblhoraenviovalor" runat="server" Width="33px" Font-Size="xx-small"
                            Font-Names="arial"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblarquivo" runat="server" Font-Size="xx-small" Font-Names="arial">Arquivo:</asp:Label>
                    <asp:Label ID="lblarquivovalor" runat="server" Width="217px" Font-Size="xx-small"
                        Font-Names="arial"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblsituacao" runat="server" Font-Size="xx-small" Font-Names="arial">Situação:</asp:Label>
                    <asp:Label ID="lblsituacaovalor" runat="server" Width="150px" Font-Size="xx-small"
                        Font-Names="arial"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>&nbsp;
                </td>
                <td>&nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblgeracao" runat="server" Font-Size="xx-small" Font-Names="arial">Geração: </asp:Label>
                    <asp:Label ID="lblgeracaovalor" runat="server" Width="174px" Font-Size="xx-small"
                        Font-Names="arial"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblcarga" runat="server" Font-Size="xx-small" Font-Names="arial">Carga:</asp:Label>
                    <asp:Label ID="lblcargavalor" runat="server" Width="150px" Font-Size="xx-small" Font-Names="arial"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblinter" runat="server" Font-Size="xx-small" Font-Names="arial">Intercâmbio:</asp:Label>
                    <asp:Label ID="lblintervalor" runat="server" Width="106px" Font-Size="xx-small" Font-Names="arial"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblvazao" runat="server" Font-Size="xx-small" Font-Names="arial">Vazão:</asp:Label>
                    <asp:Label ID="lblvazaovalor" runat="server" Width="150px" Font-Size="xx-small" Font-Names="arial"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblrestr" runat="server" Font-Size="xx-small" Font-Names="arial">Restrição:</asp:Label>
                    <asp:Label ID="lblrestrvalor" runat="server" Width="194px" Font-Size="xx-small" Font-Names="arial"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblmanu" runat="server" Font-Size="xx-small" Font-Names="arial">Manutenção:</asp:Label>
                    <asp:Label ID="lblmanuvalor" runat="server" Width="150px" Font-Size="xx-small" Font-Names="arial"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblinflexi" runat="server" Font-Size="xx-small" Font-Names="arial">Inflexibilidade: </asp:Label>
                    <asp:Label ID="lblinflexivalor" runat="server" Width="138px" Font-Size="xx-small"
                        Font-Names="arial"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblrazener" runat="server" Font-Size="xx-small" Font-Names="arial">Razão energética:</asp:Label>
                    <asp:Label ID="lblrazenervalor" runat="server" Width="130px" Font-Size="xx-small"
                        Font-Names="arial"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblrazelet" runat="server" Font-Size="xx-small" Font-Names="arial">Razão elétrica:</asp:Label>
                    <asp:Label ID="lblrazeletvalor" runat="server" Width="130px" Font-Size="xx-small"
                        Font-Names="arial"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="label2" runat="server" Width="51px" Font-Size="xx-small" Font-Names="arial">Exportação:</asp:Label>
                    <asp:Label ID="lblexporta" runat="server" Width="130px" Font-Size="xx-small" Font-Names="arial"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="label3" runat="server" Font-Size="xx-small" Font-Names="arial">Importação:</asp:Label>
                    <asp:Label ID="lblimporta" runat="server" Width="130px" Font-Size="xx-small" Font-Names="arial"></asp:Label>
                </td>
                <td style="width: 301px; height: 9px">
                    <asp:Label ID="label1" runat="server" Font-Size="xx-small" Font-Names="arial">Motivo despacho razão elétrica:</asp:Label>
                    <asp:Label ID="lblmre" runat="server" Width="43px" Font-Size="xx-small" Font-Names="arial"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="label12" runat="server" Font-Size="xx-small" Font-Names="arial">Motivo despacho por inflexibilidade: </asp:Label>
                    <asp:Label ID="lblmif" runat="server" Width="43px" Font-Size="xx-small" Font-Names="arial"></asp:Label>
                </td>
                <td style="width: 301px; height: 16px">
                    <asp:Label ID="label4" runat="server" Font-Size="xx-small" Font-Names="arial">Perdas consumo interno e compensação:</asp:Label>
                    <asp:Label ID="lblpcc" runat="server" Width="58px" Font-Size="xx-small" Font-Names="arial"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="label6" runat="server" Width="249px" Font-Size="xx-small" Font-Names="arial">Número máquinas paradas conveniência operativa:</asp:Label>
                    <asp:Label ID="lblmco" runat="server" Width="39px" Font-Size="xx-small" Font-Names="arial"></asp:Label>
                </td>
                <td style="width: 301px; height: 1px">
                    <asp:Label ID="label5" runat="server" Font-Size="xx-small" Font-Names="arial">Número máquinas operando como síncrono:</asp:Label>
                    <asp:Label ID="lblmos" runat="server" Width="45px" Font-Size="xx-small" Font-Names="arial"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="label8" runat="server" Font-Size="xx-small" Font-Names="arial">Número máquinas gerando:</asp:Label>
                    <asp:Label ID="lblmeg" runat="server" Width="43px" Font-Size="xx-small" Font-Names="arial"></asp:Label>
                </td>
                <td style="width: 301px; height: 1px">
                    <asp:Label ID="label7" runat="server" Font-Size="xx-small" Font-Names="arial">Energia de reposição e perdas:</asp:Label>
                    <asp:Label ID="lblerp" runat="server" Width="45px" Font-Size="xx-small" Font-Names="arial"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="label10" runat="server" Font-Size="xx-small" Font-Names="arial">Disponibilidade:</asp:Label>
                    <asp:Label ID="lbldsp" runat="server" Width="43px" Font-Size="xx-small" Font-Names="arial"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="label9" runat="server" Font-Size="xx-small" Font-Names="arial">Motivo de Despacho Unit Commitment:</asp:Label>
                    <asp:Label ID="lblclf" runat="server" Width="45px" Font-Size="xx-small" Font-Names="arial"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="label11" runat="server" Font-Size="xx-small" Font-Names="arial">Parada de máquinas por conveniência operativa:</asp:Label>
                    <asp:Label ID="lblpco" runat="server" Width="43px" Font-Size="xx-small" Font-Names="arial"></asp:Label>
                </td>
                <td style="width: 301px; height: 1px">
                    <asp:Label ID="label13" runat="server" Font-Size="xx-small" Font-Names="arial">Cota inicial:</asp:Label>
                    <asp:Label ID="lblcota" runat="server" Width="43px" Font-Size="xx-small" Font-Names="arial"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="label14" runat="server" Font-Size="xx-small" Font-Names="arial">Restrição por falta de combustível:</asp:Label>
                    <asp:Label ID="lblrfc" runat="server" Width="43px" Font-Size="xx-small" Font-Names="arial"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="label15" runat="server" Font-Names="arial" Font-Size="xx-small">Garantia energética:</asp:Label>
                    <asp:Label ID="lblrmp" runat="server" Width="43px" Font-Names="arial" Font-Size="xx-small"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="label16" runat="server" Font-Names="arial" Font-Size="xx-small">Geração fora de mérito:</asp:Label>
                    <asp:Label ID="lblgfm" runat="server" Width="43px" Font-Names="arial" Font-Size="xx-small"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="label17" runat="server" Font-Names="arial" Font-Size="xx-small">Crédito por Substituição:</asp:Label>
                    <asp:Label ID="lblcfm" runat="server" Width="43px" Font-Names="arial" Font-Size="xx-small"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="label18" runat="server" Font-Names="arial" Font-Size="xx-small">Geração Substituta:</asp:Label>
                    <asp:Label ID="lblsom" runat="server" Width="43px" Font-Names="arial" Font-Size="xx-small"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="label19" runat="server" Font-Names="arial" Font-Size="xx-small">GE substituição:</asp:Label>
                    <asp:Label ID="lblges" runat="server" Width="43px" Font-Names="arial" Font-Size="xx-small"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="label20" runat="server" Font-Names="arial" Font-Size="xx-small">GE crédito:</asp:Label>
                    <asp:Label ID="lblgec" runat="server" Width="43px" Font-Names="arial" Font-Size="xx-small"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="label21" runat="server" Font-Names="arial" Font-Size="xx-small">Despacho ciclo aberto:</asp:Label>
                    <asp:Label ID="lbldca" runat="server" Width="43px" Font-Names="arial" Font-Size="xx-small"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="label22" runat="server" Font-Names="arial" Font-Size="xx-small">Despacho ciclo reduzido:</asp:Label>
                    <asp:Label ID="lbldcr" runat="server" Width="43px" Font-Names="arial" Font-Size="xx-small"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="label29" runat="server" Font-Names="arial" Font-Size="xx-small">Nível de Partida:</asp:Label>
                    <asp:Label ID="lblIR1" runat="server" Width="43px" Font-Names="arial" Font-Size="xx-small"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="label23" runat="server" Font-Names="arial" Font-Size="xx-small">Dia -1:</asp:Label>
                    <asp:Label ID="lblIR2" runat="server" Width="43px" Font-Names="arial" Font-Size="xx-small"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="label25" runat="server" Font-Names="arial" Font-Size="xx-small">Dia -2:</asp:Label>
                    <asp:Label ID="lblIR3" runat="server" Width="43px" Font-Names="arial" Font-Size="xx-small"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="label27" runat="server" Font-Names="arial" Font-Size="xx-small">Carga da Ande:</asp:Label>
                    <asp:Label ID="lblIR4" runat="server" Width="43px" Font-Names="arial" Font-Size="xx-small"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="label26" runat="server" Font-Names="arial" Font-Size="xx-small">Recomposição de Reserva Operativa:</asp:Label>
                    <asp:Label ID="lblRRO" runat="server" Width="43px" Font-Names="arial" Font-Size="xx-small"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="auto-style1">
                    <asp:Label ID="label24" runat="server" Font-Names="arial" Font-Size="xx-small">Custo Variável Unitário (CVU):</asp:Label>
                    <asp:Label ID="lblCvu" runat="server" Width="43px" Font-Names="arial" Font-Size="xx-small"></asp:Label>
                </td>
                <td class="auto-style1">&nbsp;</td>
            </tr>
            <tr>
                <td align="center" class="auto-style1" colspan="2">

                    <asp:Label ID="lblMsg" Visible="False" ForeColor="Green" runat="server" Font-Bold="True" Font-Size="Medium">Desculpe, este dado não pôde ser carregado no momento.</asp:Label>

                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <asp:ImageButton ID="btnimprimir" runat="server" ImageUrl="images/bt_imprimir.gif"></asp:ImageButton>
                </td>
            </tr>

        </table>
    </form>
</asp:Content>

