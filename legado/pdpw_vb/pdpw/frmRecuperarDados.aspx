<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frmRecuperarDados.aspx.vb" Inherits="pdpw.frmRecuperarDados" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>ONS - Operador Nacional do Sistema Elétrico</title>
    <meta http-equiv="Content-Type" content="text/html; charset=windows-1252">
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="../pdpw/images/style.css" rel="stylesheet">
    <script>
        
        function MarcaCHK() {
            if (document.getElementById("_ctl0_ContentPlaceHolder1_chkTODOS").checked == true) {
                //document.frmRecuperarDados("chkGER").checked = false;
                //document.frmRecuperarDados("chkCAR").checked = false;
                //document.frmRecuperarDados("chkINT").checked = false;
                //document.frmRecuperarDados("chkVAZ").checked = false;
                document.getElementById("_ctl0_ContentPlaceHolder1_chkINF").checked = true;
                //document.frmRecuperarDados("chkENE").checked = false;
                //document.frmRecuperarDados("chkELE").checked = false;
                //document.frmRecuperarDados("chkEXP").checked = false;
                //document.frmRecuperarDados("chkMRE").checked = false;
                //document.frmRecuperarDados("chkMIF").checked = false;
                //document.frmRecuperarDados("chkIMP").checked = false;
                //document.frmRecuperarDados("chkPCC").checked = false;
                //document.frmRecuperarDados("chkMCO").checked = false;
                //document.frmRecuperarDados("chkMOS").checked = false;
                //document.frmRecuperarDados("chkMEG").checked = false;
                //document.frmRecuperarDados("chkERP").checked = false;
                document.getElementById("_ctl0_ContentPlaceHolder1_chkDSP").checked = true;
                //document.frmRecuperarDados("chkCLF").checked = false;
                //document.frmRecuperarDados("chkRES").checked = false;
                //document.frmRecuperarDados("chkMAN").checked = false;
                //document.frmRecuperarDados("chkPCO").checked = false;
                //document.frmRecuperarDados("chkRFC").checked = false;
                //document.frmRecuperarDados("chkRMP").checked = false;
                //document.frmRecuperarDados("chkGFM").checked = false;
                //document.frmRecuperarDados("chkCFM").checked = false;
                //document.frmRecuperarDados("chkSOM").checked = false;
                //document.frmRecuperarDados("chkGES").checked = false;
                //document.frmRecuperarDados("chkGEC").checked = false;
                //document.frmRecuperarDados("chkDCA").checked = false;
                //document.frmRecuperarDados("chkDCR").checked = false;
                //document.frmRecuperarDados("chkIR1").checked = false;
                //document.frmRecuperarDados("chkIR2").checked = false;
                //document.frmRecuperarDados("chkIR3").checked = false;
                //document.frmRecuperarDados("chkIR4").checked = false;
            } else {
                //document.frmRecuperarDados("chkGER").checked = false;
                //document.frmRecuperarDados("chkCAR").checked = false;
                //document.frmRecuperarDados("chkINT").checked = false;
                //document.frmRecuperarDados("chkVAZ").checked = false;
                document.getElementById("_ctl0_ContentPlaceHolder1_chkINF").checked = false;
                //document.frmRecuperarDados("chkENE").checked = false;
                //document.frmRecuperarDados("chkELE").checked = false;
                //document.frmRecuperarDados("chkEXP").checked = false;
                //document.frmRecuperarDados("chkMRE").checked = false;
                //document.frmRecuperarDados("chkMIF").checked = false;
                //document.frmRecuperarDados("chkIMP").checked = false;
                //document.frmRecuperarDados("chkPCC").checked = false;
                //document.frmRecuperarDados("chkMCO").checked = false;
                //document.frmRecuperarDados("chkMOS").checked = false;
                //document.frmRecuperarDados("chkMEG").checked = false;
                //document.frmRecuperarDados("chkERP").checked = false;
                document.getElementById("_ctl0_ContentPlaceHolder1_chkDSP").checked = false;
                //document.frmRecuperarDados("chkCLF").checked = false;
                //document.frmRecuperarDados("chkMAN").checked = false;
                //document.frmRecuperarDados("chkRES").checked = false;
                //document.frmRecuperarDados("chkPCO").checked = false;
                //document.frmRecuperarDados("chkRFC").checked = false;
                //document.frmRecuperarDados("chkRMP").checked = false;
                //document.frmRecuperarDados("chkGFM").checked = false;
                //document.frmRecuperarDados("chkCFM").checked = false;
                //document.frmRecuperarDados("chkSOM").checked = false;
                //document.frmRecuperarDados("chkGES").checked = false;
                //document.frmRecuperarDados("chkGEC").checked = false;
                //document.frmRecuperarDados("chkDCA").checked = false;
                //document.frmRecuperarDados("chkDCR").checked = false;
                //document.frmRecuperarDados("chkIR1").checked = false;
                //document.frmRecuperarDados("chkIR2").checked = false;
                //document.frmRecuperarDados("chkIR3").checked = false;
                //document.frmRecuperarDados("chkIR4").checked = false;
            }
        }

    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table style="width: 882px; height: 582px" height="582" cellspacing="0" cellpadding="0"
        width="882" border="0">
        <tbody>
            <tr>
                <td style="width: 19px" valign="top" width="19">
                    <br>
                </td>
                <td style="width: 797px" valign="top">
                    <div align="center">
                        <table style="width: 874px; height: 72px" cellspacing="0" cellpadding="0" width="874" border="0">
                            <tbody>
                                <tr>
                                    <td style="height: 6px" width="20%" height="6"></td>
                                </tr>
                                <tr>
                                    <td style="height: 17px" height="17">
                                        <table style="width: 872px; height: 25px" cellspacing="0" cellpadding="0" width="872" background="../pdpw/images/back_tit_sistema.gif"
                                            border="0">
                                            <tbody>
                                                <tr>
                                                    <td style="width: 766px; height: 12px">
                                                        <img height="25" src="../pdpw/images/tit_sis_guideline.gif" width="179"></td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 10px" height="10">
                                        <table style="width: 873px; height: 23px" cellspacing="0" cellpadding="0" width="873" background="../pdpw/images/back_titulo.gif"
                                            border="0">
                                            <tbody>
                                                <tr>
                                                    <td style="height: 8px">
                                                        <div align="left">
                                                            <img style="width: 152px; height: 23px" height="23" src="../pdpw/images/tit_RecuperarDados.gif"
                                                                width="152">
                                                        </div>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <div align="center">
                        <form id="frmRecuperarDados" name="frmRecuperarDados" runat="server">
                            <table class="modulo" style="width: 562px; height: 412px" cellspacing="0" cellpadding="0"
                                border="0">
                                <tr>
                                    <td>
                                        <table class="modulo" style="width: 561px; height: 396px" cellspacing="0" cellpadding="0"
                                            border="0">
                                            <tr>
                                                <td align="center">
                                                    <table class="formulario_texto" style="width: 560px; height: 404px" cellspacing="0" cellpadding="0"
                                                        border="0">
                                                        <!--TBODY-->
                                                        <tr align="center">
                                                            <td style="width: 532px; height: 22px" bordercolor="white" bgcolor="#99cc00" colspan="2">Marque 
																	somente os itens a serem recuperados
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 340px; height: 21px">
                                                                <asp:CheckBox ID="chkGER" runat="server" Width="50px" Text="Geração" Checked="False" Enabled="False"></asp:CheckBox></td>
                                                            <td style="width: 220px; height: 67px" align="center" rowspan="4"></td>
                                                            <tr>
                                                                <td style="width: 340px; height: 3px">
                                                                    <asp:CheckBox ID="chkCAR" runat="server" Width="39px" Text="Carga" Checked="False" Enabled="False"></asp:CheckBox></td>
                                                            </tr>
                                                        <tr>
                                                            <td style="width: 340px; height: 20px">
                                                                <asp:CheckBox ID="chkINT" runat="server" Width="73px" Text="Intercâmbio" Checked="False" Enabled="False"></asp:CheckBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 340px; height: 3px">
                                                                <asp:CheckBox ID="chkVAZ" runat="server" Width="41px" Text="Vazão" Checked="False" Enabled="False"></asp:CheckBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 340px; height: 13px">
                                                                <asp:CheckBox ID="chkINF" runat="server" Width="89px" Text="Inflexibilidade" Checked="True" Height="4px"></asp:CheckBox></td>
                                                            <td style="width: 220px; height: 32px" align="left" rowspan="2">Empresa:<br>
                                                                <asp:DropDownList ID="cboEmpresa" runat="server" Width="169px"></asp:DropDownList><br>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 340px; height: 8px">
                                                                <asp:CheckBox ID="chkENE" runat="server" Width="107px" Text="Razão Energética" Checked="False" Enabled="False"></asp:CheckBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 340px">
                                                                <asp:CheckBox ID="chkELE" runat="server" Width="89px" Text="Razão Elétrica" Checked="False" Enabled="False"></asp:CheckBox></td>
                                                            <td style="width: 220px" align="left" rowspan="2">Data do PDP:<br>
                                                                <asp:DropDownList ID="cboDataPDP" runat="server" Width="102px" AutoPostBack="True"></asp:DropDownList></td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 340px; height: 3px">
                                                                <asp:CheckBox ID="chkEXP" runat="server" Width="72px" Text="Exportação" Checked="False" Enabled="False"></asp:CheckBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 340px; height: 3px">
                                                                <asp:CheckBox ID="chkIMP" runat="server" Width="61px" Text="Importação" Checked="False" Enabled="False"></asp:CheckBox></td>
                                                            <td style="width: 220px; height: 3px" rowspan="3">
                                                                <input id="chkTODOS" onclick="MarcaCHK();" type="checkbox" checked name="chkEnviaTodos"
                                                                    runat="server">Marcar ou desmarcar Todos</td>
                                                            <tr>
                                                                <td style="width: 340px; height: 3px">
                                                                    <asp:CheckBox ID="chkMRE" runat="server" Width="195px" Text="Motivo de Despacho Razão Elétrica"
                                                                        Checked="False" Enabled="False"></asp:CheckBox></td>
                                                            </tr>
                                                        <tr>
                                                            <td style="width: 340px; height: 3px">
                                                                <asp:CheckBox ID="chkMIF" runat="server" Width="211px" Text="Motivo de Despacho por Inflexibilidade"
                                                                    Checked="False" Enabled="False"></asp:CheckBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 340px; height: 3px">
                                                                <asp:CheckBox ID="chkPCC" runat="server" Width="221px" Text="Perdas Consumo Interno e Compensação"
                                                                    Checked="False" Enabled="False"></asp:CheckBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 340px; height: 3px">
                                                                <asp:CheckBox ID="chkMCO" runat="server" Width="284px" Text="Número Máquinas Paradas por Conveniência Operativa"
                                                                    Checked="False" Enabled="False"></asp:CheckBox></td>
                                                            <td style="width: 220px; height: 3px" align="left" rowspan="4">
                                                                <asp:Label ID="Label2" runat="server" Width="161px" Font-Bold="True" ForeColor="OliveDrab"
                                                                    Font-Names="Arial" Font-Size="X-Small">Data a ser recuperada</asp:Label><asp:DropDownList ID="cboDataAnterior" runat="server" Width="102px" AutoPostBack="True"></asp:DropDownList></td>
                                                            <tr>
                                                                <td style="width: 340px; height: 3px">
                                                                    <asp:CheckBox ID="chkMOS" runat="server" Width="235px" Text="Número Máquinas Operando como Síncrono"
                                                                        Checked="False" Enabled="False"></asp:CheckBox></td>
                                                            </tr>
                                                        <tr>
                                                            <td style="width: 340px; height: 3px">
                                                                <asp:CheckBox ID="chkMEG" runat="server" Width="156px" Text="Número Máquinas Gerando" Checked="False" Enabled="False"></asp:CheckBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 340px; height: 3px">
                                                                <asp:CheckBox ID="chkERP" runat="server" Width="173px" Text="Energia de Reposição e Perdas" Checked="False" Enabled="False"></asp:CheckBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 340px; height: 3px">
                                                                <asp:CheckBox ID="chkDSP" runat="server" Width="94px" Text="Disponibilidade" Checked="True"></asp:CheckBox></td>
                                                            <td style="width: 220px; height: 80px" align="center" rowspan="10">
                                                                <asp:Label ID="lblMsg" runat="server" Width="191px" Height="160px" Font-Bold="True" Font-Size="X-Small"></asp:Label></td>
                                                            <tr>
                                                                <td style="width: 340px; height: 3px">
                                                                    <asp:CheckBox ID="chkCLF" runat="server" Width="163px" Text="Motivo de Despacho Unit Commitment" Checked="False" Enabled="False"></asp:CheckBox></td>
                                                            </tr>
                                                        <tr>
                                                            <td style="width: 340px; height: 3px">
                                                                <asp:CheckBox ID="chkRES" runat="server" Width="65px" Text="Restrição" Checked="False" Enabled="False"></asp:CheckBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 340px; height: 20px">
                                                                <asp:CheckBox ID="chkMAN" runat="server" Width="77px" Text="Manutenção" Checked="False" Enabled="False"></asp:CheckBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 340px; height: 20px">
                                                                <asp:CheckBox ID="chkPCO" runat="server" Width="260px" Text="Parada de Máquinas por Conveniência Operativa"
                                                                    Checked="False" Enabled="False"></asp:CheckBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 340px; height: 20px">
                                                                <asp:CheckBox ID="chkRFC" runat="server" Width="184px" Text="Restrição por Falta de Combustível"
                                                                    Checked="False" Enabled="False"></asp:CheckBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 340px; height: 20px">
                                                                <asp:CheckBox ID="chkRMP" runat="server" Checked="False" Enabled="False" Text="Garantia Energética" Width="154px"></asp:CheckBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 340px; height: 20px">
                                                                <asp:CheckBox ID="chkGFM" runat="server" Checked="False" Enabled="False" Text="Geração Fora de Mérito" Width="132px"></asp:CheckBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 340px; height: 20px">
                                                                <asp:CheckBox ID="chkCFM" runat="server" Checked="False" Enabled="False" Text="Geração Substituta" Width="133px"></asp:CheckBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 340px; height: 20px">
                                                                <asp:CheckBox ID="chkSOM" runat="server" Checked="False" Enabled="False" Text="Geração Substituta" Width="163px"></asp:CheckBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 340px; height: 20px">
                                                                <asp:CheckBox ID="chkGES" runat="server" Checked="False" Enabled="False" Text="GE Substituiçao" Width="163px"></asp:CheckBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 340px; height: 20px">
                                                                <asp:CheckBox ID="chkGEC" runat="server" Checked="False" Enabled="False" Text="GE Crédito" Width="163px"></asp:CheckBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 340px; height: 20px">
                                                                <asp:CheckBox ID="chkDCA" runat="server" Checked="False" Enabled="False" Text="Dispacho Ciclo Aberto" Width="163px"></asp:CheckBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 340px; height: 20px">
                                                                <asp:CheckBox ID="chkDCR" runat="server" Checked="False" Enabled="False" Text="Dispacho Ciclo Reduzido" Width="163px"></asp:CheckBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 340px; height: 20px">
                                                                <asp:CheckBox ID="chkIR1" runat="server" Checked="False" Enabled="False" Text="Nível de Partida" Width="163px"></asp:CheckBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 340px; height: 20px">
                                                                <asp:CheckBox ID="chkIR2" runat="server" Checked="False" Enabled="False" Text="Dia -1" Width="163px"></asp:CheckBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 340px; height: 20px">
                                                                <asp:CheckBox ID="chkIR3" runat="server" Checked="False" Enabled="False" Text="Dia - 2" Width="163px"></asp:CheckBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 340px; height: 20px">
                                                                <asp:CheckBox ID="chkIR4" runat="server" Checked="False" Enabled="False" Text="Carga da Ande" Width="163px"></asp:CheckBox></td>
                                                        </tr>
                                                        <!--/TBODY-->
                                                    </table>
                                                    <asp:ImageButton ID="btnEnviar" runat="server" ImageUrl="images/bt_Confirmar.gif"></asp:ImageButton></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </form>
                    </div>
                </td>
            </tr>
        </tbody>
    </table>
</asp:Content>
