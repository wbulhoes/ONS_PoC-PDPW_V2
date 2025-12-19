<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frmEnviaDados.aspx.vb" Inherits="pdpw.frmEnviaDados" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>ONS - Operador Nacional do Sistema Elétrico</title>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <meta http-equiv="Content-Type" content="text/html; charset=windows-1252">
    <link href="../pdpw/images/style.css" rel="stylesheet">
    <script language="javascript" type="text/javascript" src="js/MSGAguarde.js"></script>
    <link href="css/MSGAguarde.css" rel="stylesheet" />
    <script>
        function MarcaCHK() {
            if (document.getElementById("_ctl0_ContentPlaceHolder1_chkEnviaTodos").checked == true) {
                if (document.getElementById("_ctl0_ContentPlaceHolder1_chkVazao").checked == true) {
                    document.getElementById("_ctl0_ContentPlaceHolder1_chkVazE").checked = true;
                }
                if (document.getElementById("_ctl0_ContentPlaceHolder1_chkInflexibilidade").checked == true) {
                    document.getElementById("_ctl0_ContentPlaceHolder1_chkIfxE").checked = true;
                }
                /*if (document.frmEnviarDados("chkEnergetica").checked == true) {
                    document.frmEnviarDados("chkZenE").checked = true;
                }
                if (document.frmEnviarDados("chkEletrica").checked == true) {
                    document.frmEnviarDados("chkZelE").checked = true;
                }
                if (document.frmEnviarDados("chkExporta").checked == true) {
                    document.frmEnviarDados("chkExpE").checked = true;
                }
                if (document.frmEnviarDados("chkImporta").checked == true) {
                    document.frmEnviarDados("chkImpE").checked = true;
                }
                if (document.frmEnviarDados("chkMRE").checked == true) {
                    document.frmEnviarDados("chkMreE").checked = true;
                }
                if (document.frmEnviarDados("chkMIF").checked == true) {
                    document.frmEnviarDados("chkMifE").checked = true;
                }
                if (document.frmEnviarDados("chkPCC").checked == true) {
                    document.frmEnviarDados("chkPccE").checked = true;
                }
                if (document.frmEnviarDados("chkMCO").checked == true) {
                    document.frmEnviarDados("chkMcoE").checked = true;
                }
                if (document.frmEnviarDados("chkMOS").checked == true) {
                    document.frmEnviarDados("chkMosE").checked = true;
                }
                if (document.frmEnviarDados("chkMEG").checked == true) {
                    document.frmEnviarDados("chkMegE").checked = true;
                }
                if (document.frmEnviarDados("chkERP").checked == true) {
                    document.frmEnviarDados("chkErpE").checked = true;
                }*/
                if (document.getElementById("_ctl0_ContentPlaceHolder1_chkDSP").checked == true) {
                    document.getElementById("_ctl0_ContentPlaceHolder1_chkDspE").checked = true;
                }
                /*if (document.frmEnviarDados("chkCLF").checked == true) {
                    document.frmEnviarDados("chkClfE").checked = true;
                }
                if (document.frmEnviarDados("chkPCO").checked == true) {
                    document.frmEnviarDados("chkPcoE").checked = true;
                }*/

                if (document.getElementById("_ctl0_ContentPlaceHolder1_chkRFC").checked == true) {
                    document.getElementById("_ctl0_ContentPlaceHolder1_chkRfcE").checked = true;
                }
                /*
                if (document.frmEnviarDados("chkRMP").checked == true) {
                    document.frmEnviarDados("chkRmpE").checked = true;
                }
                if (document.frmEnviarDados("chkGFM").checked == true) {
                    document.frmEnviarDados("chkGfmE").checked = true;
                }
                if (document.frmEnviarDados("chkCFM").checked == true) {
                    document.frmEnviarDados("chkCfmE").checked = true;
                }
                if (document.frmEnviarDados("chkSOM").checked == true) {
                    document.frmEnviarDados("chkSomE").checked = true;
                }
                if (document.frmEnviarDados("chkGES").checked == true) {
                    document.frmEnviarDados("chkGesE").checked = true;
                }
                if (document.frmEnviarDados("chkGEC").checked == true) {
                    document.frmEnviarDados("chkGecE").checked = true;
                }
                if (document.frmEnviarDados("chkDCA").checked == true) {
                    document.frmEnviarDados("chkDcaE").checked = true;
                }
                if (document.frmEnviarDados("chkDCR").checked == true) {
                    document.frmEnviarDados("chkDcrE").checked = true;
                }
                if (document.frmEnviarDados("chkIR1").checked == true) {
                    document.frmEnviarDados("chkIr1E").checked = true;
                }
                if (document.frmEnviarDados("chkIR2").checked == true) {
                    document.frmEnviarDados("chkIr2E").checked = true;
                }
                if (document.frmEnviarDados("chkIR3").checked == true) {
                    document.frmEnviarDados("chkIr3E").checked = true;
                }
                if (document.frmEnviarDados("chkIR4").checked == true) {
                    document.frmEnviarDados("chkIr4E").checked = true;
                }
                if (document.frmEnviarDados("chkRRO").checked == true) {
                    document.frmEnviarDados("chkRROE").checked = true;
                }*/

            } else {
                document.getElementById("_ctl0_ContentPlaceHolder1_chkVazE").checked = false;
                document.getElementById("_ctl0_ContentPlaceHolder1_chkIfxE").checked = false;
                /*document.frmEnviarDados("chkZenE").checked = false;
                document.frmEnviarDados("chkZelE").checked = false;
                document.frmEnviarDados("chkExpE").checked = false;
                document.frmEnviarDados("chkMreE").checked = false;
                document.frmEnviarDados("chkMifE").checked = false;
                document.frmEnviarDados("chkImpE").checked = false;
                document.frmEnviarDados("chkPccE").checked = false;
                document.frmEnviarDados("chkMcoE").checked = false;
                document.frmEnviarDados("chkMosE").checked = false;
                document.frmEnviarDados("chkMegE").checked = false;
                document.frmEnviarDados("chkErpE").checked = false;*/
                document.getElementById("_ctl0_ContentPlaceHolder1_chkDspE").checked = false;
                /*document.frmEnviarDados("chkClfE").checked = false;
                document.frmEnviarDados("chkPcoE").checked = false;*/
                document.getElementById("_ctl0_ContentPlaceHolder1_chkrfcE").checked = false;
                /*
                document.frmEnviarDados("chkRfcE").checked = false;
                document.frmEnviarDados("chkRmpE").checked = false;
                document.frmEnviarDados("chkGfmE").checked = false;
                document.frmEnviarDados("chkCfmE").checked = false;
                document.frmEnviarDados("chkSomE").checked = false;
                document.frmEnviarDados("chkGesE").checked = false;
                document.frmEnviarDados("chkGecE").checked = false;
                document.frmEnviarDados("chkDcaE").checked = false;
                document.frmEnviarDados("chkDcrE").checked = false;
                document.frmEnviarDados("chkIr1E").checked = false;
                document.frmEnviarDados("chkIr2E").checked = false;
                document.frmEnviarDados("chkIr3E").checked = false;
                document.frmEnviarDados("chkIr4E").checked = false;
                document.frmEnviarDados("chkRROE").checked = false;*/
            }
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table style="width: 1154px; height: 620px" height="620" cellspacing="0" cellpadding="0"
        width="1154" border="0">
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
                                        <table style="width: 1356px; height: 25px" cellspacing="0" cellpadding="0" width="1356"
                                            background="../pdpw/images/back_tit_sistema.gif" border="0">
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
                                        <table style="width: 1356px; height: 23px" cellspacing="0" cellpadding="0" width="1356"
                                            background="../pdpw/images/back_titulo.gif" border="0">
                                            <tbody>
                                                <tr>
                                                    <td style="height: 8px">
                                                        <div align="left">
                                                            <img style="width: 136px; height: 23px" height="23" src="../pdpw/images/tit_EnviarDados.gif"
                                                                width="136">
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
                        <form id="frmEnviarDados" name="frmEnviarDados" runat="server">
                            <table class="modulo" style="width: 748px; height: 296px" cellspacing="0" cellpadding="0"
                                border="0">
                                <tr>
                                    <td style="width: 1082px">
                                        <table class="modulo" style="width: 745px; height: 466px" cellspacing="0" cellpadding="0"
                                            border="0">
                                            <tbody>
                                                <tr>
                                                    <td style="width: 259px" valign="top">
                                                        <br>
                                                        <table class="modulo" style="width: 257px; height: 62px" cellspacing="0" cellpadding="0"
                                                            border="0">
                                                            <tr>
                                                                <td style="width: 93px; height: 30px" align="right">
                                                                    <p>Data do PDP&nbsp;</p>
                                                                </td>
                                                                <td style="width: 163px; height: 30px">
                                                                    <asp:DropDownList ID="cboData" runat="server" Width="148px" AutoPostBack="True"></asp:DropDownList></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 93px" align="right">Empresa&nbsp;</td>
                                                                <td style="width: 163px">
                                                                    <asp:DropDownList ID="cboEmpresa" runat="server" Width="148px" AutoPostBack="True"></asp:DropDownList></td>
                                                            </tr>
                                                        </table>
                                                        <table style="width: 256px; height: 402px">
                                                            <tr align="center">
                                                                <td>
                                                                    <asp:ImageButton ID="btnEnviar" runat="server" ImageUrl="images/bt_Enviar.gif"></asp:ImageButton><br>
                                                                    <br>
                                                                    <br>
                                                                    <asp:Label ID="lblMensagem" runat="server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td style="width: 5px">&nbsp;&nbsp;&nbsp;<font size="1"> </font>
                                                    </td>
                                                    <td style="width: 810px">
                                                        <table class="formulario_texto" style="width: 486px; height: 452px" cellspacing="0" cellpadding="0"
                                                            border="0">
                                                            <tr>
                                                                <td style="width: 278px; height: 23px" bordercolor="white" bgcolor="#99cc00">
                                                                    <p align="center">
                                                                        <font size="1"><strong>Dados Digitados</strong></font>
                                                                    </p>
                                                                </td>
                                                                <td style="width: 207px; height: 23px" bgcolor="#99cc00" colspan="2">
                                                                    <p align="center">
                                                                        <font size="1"><strong>Envio dos dados </strong></font>
                                                                    </p>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 278px; height: 24px">
                                                                    <asp:CheckBox ID="chkGeracao" runat="server" Width="122px" Text="Geração" Enabled="False"></asp:CheckBox></td>
                                                                <td style="width: 207px; height: 67px" colspan="2" rowspan="3">
                                                                    <asp:CheckBox ID="chkEnvia1" runat="server" Width="186px" Text="Geração, Carga, Intercâmbio."></asp:CheckBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 278px; height: 19px">
                                                                    <asp:CheckBox ID="chkCarga" runat="server" Width="119px" Text="Carga" Enabled="False"></asp:CheckBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 278px; height: 3px">
                                                                    <asp:CheckBox ID="chkIntercambio" runat="server" Width="121px" Text="Intercâmbio" Enabled="False"></asp:CheckBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 498px; height: 25px" valign="middle" background="../pdpw/images/back_titulo.gif"
                                                                    colspan="3"></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 278px; height: 19px">
                                                                    <asp:CheckBox ID="chkVazao" runat="server" Width="54px" Text="Vazão" Enabled="False"></asp:CheckBox></td>
                                                                <td style="width: 2px; height: 19px">
                                                                    <asp:CheckBox ID="chkVazE" runat="server" Width="77px" Enabled="False"></asp:CheckBox></td>
                                                                <td rowspan="21" style="width: 90px">
                                                                    <input type="checkbox" runat="server" id="chkEnviaTodos" onclick="MarcaCHK();">&nbsp;Marcar 
																		Todos</td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 278px; height: 20px">
                                                                    <asp:CheckBox ID="chkInflexibilidade" runat="server" Width="91px" Text="Inflexibilidade" Enabled="False"></asp:CheckBox></td>
                                                                <td style="width: 2px; height: 20px">
                                                                    <asp:CheckBox ID="chkIfxE" runat="server" Width="59px" Enabled="False"></asp:CheckBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 278px; height: 3px">
                                                                    <asp:CheckBox ID="chkEnergetica" runat="server" Width="107px" Text="Razão Energética" Enabled="False"></asp:CheckBox></td>
                                                                <td style="width: 2px">
                                                                    <asp:CheckBox ID="chkZenE" runat="server" Width="71px" Enabled="False"></asp:CheckBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 278px; height: 3px">
                                                                    <asp:CheckBox ID="chkEletrica" runat="server" Width="91px" Text="Razão Elétrica" Enabled="False"></asp:CheckBox></td>
                                                                <td style="width: 2px">
                                                                    <asp:CheckBox ID="chkZelE" runat="server" Width="67px" Enabled="False"></asp:CheckBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 278px; height: 3px">
                                                                    <asp:CheckBox ID="chkExporta" runat="server" Width="75px" Text="Exportação" Enabled="False"></asp:CheckBox></td>
                                                                <td style="width: 2px">
                                                                    <asp:CheckBox ID="chkExpE" runat="server" Width="70px" Enabled="False"></asp:CheckBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 278px; height: 3px">
                                                                    <asp:CheckBox ID="chkImporta" runat="server" Width="77px" Text="Importação" Enabled="False"></asp:CheckBox></td>
                                                                <td style="width: 2px">
                                                                    <asp:CheckBox ID="chkImpE" runat="server" Width="71px" Enabled="False"></asp:CheckBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 278px; height: 19px">
                                                                    <asp:CheckBox ID="chkMRE" runat="server" Width="200px" Text="Motivo de Despacho Razão Elétrica"
                                                                        Enabled="False"></asp:CheckBox></td>
                                                                <td style="width: 2px">
                                                                    <asp:CheckBox ID="chkMreE" runat="server" Width="70px" Enabled="False"></asp:CheckBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 278px; height: 19px">
                                                                    <asp:CheckBox ID="chkMIF" runat="server" Width="217px" Text="Motivo de Despacho por Inflexibilidade"
                                                                        Enabled="False"></asp:CheckBox></td>
                                                                <td style="width: 2px">
                                                                    <asp:CheckBox ID="chkMifE" runat="server" Width="70px" Enabled="False"></asp:CheckBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 278px; height: 3px">
                                                                    <asp:CheckBox ID="chkPCC" runat="server" Width="223px" Text="Perdas Consumo Interno e Compensação"
                                                                        Enabled="False"></asp:CheckBox></td>
                                                                <td style="width: 2px">
                                                                    <asp:CheckBox ID="chkPccE" runat="server" Width="70px" Enabled="False"></asp:CheckBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 278px; height: 19px">
                                                                    <asp:CheckBox ID="chkMCO" runat="server" Width="284px" Text="Número Máquinas Paradas por Conveniência Operativa"
                                                                        Enabled="False"></asp:CheckBox></td>
                                                                <td style="width: 2px; height: 19px">
                                                                    <asp:CheckBox ID="chkMcoE" runat="server" Width="70px" Enabled="False"></asp:CheckBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 278px; height: 20px">
                                                                    <asp:CheckBox ID="chkMOS" runat="server" Width="235px" Text="Número Máquinas Operando como Síncrono"
                                                                        Enabled="False"></asp:CheckBox></td>
                                                                <td style="width: 2px; height: 20px">
                                                                    <asp:CheckBox ID="chkMosE" runat="server" Width="69px" Enabled="False"></asp:CheckBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 278px; height: 3px">
                                                                    <asp:CheckBox ID="chkMEG" runat="server" Width="157px" Text="Número Máquinas Gerando" Enabled="False"></asp:CheckBox></td>
                                                                <td style="width: 2px">
                                                                    <asp:CheckBox ID="chkMegE" runat="server" Width="71px" Enabled="False"></asp:CheckBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 278px; height: 19px">
                                                                    <asp:CheckBox ID="chkERP" runat="server" Width="173px" Text="Energia de Reposição e Perdas" Enabled="False"></asp:CheckBox></td>
                                                                <td style="width: 2px; height: 19px">
                                                                    <asp:CheckBox ID="chkErpE" runat="server" Width="69px" Enabled="False"></asp:CheckBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 278px; height: 19px">
                                                                    <asp:CheckBox ID="chkDSP" runat="server" Width="97px" Text="Disponibilidade" Enabled="False"></asp:CheckBox></td>
                                                                <td style="width: 2px; height: 19px">
                                                                    <asp:CheckBox ID="chkDspE" runat="server" Width="69px" Enabled="False"></asp:CheckBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 278px; height: 19px">
                                                                    <asp:CheckBox ID="chkCLF" runat="server" Width="163px" Text="Compensação de Lastro Físico" Enabled="False"></asp:CheckBox></td>
                                                                <td style="width: 2px; height: 19px">
                                                                    <asp:CheckBox ID="chkClfE" runat="server" Width="69px" Enabled="False"></asp:CheckBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 278px; height: 19px">
                                                                    <asp:CheckBox ID="chkPCO" runat="server" Width="257px" Text="Parada de Máquinas por Conveniência Operativa"
                                                                        Enabled="False"></asp:CheckBox></td>
                                                                <td style="width: 2px; height: 19px">
                                                                    <asp:CheckBox ID="chkPcoE" runat="server" Width="69px" Enabled="False"></asp:CheckBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 278px; height: 19px">
                                                                    <asp:CheckBox ID="chkRFC" runat="server" Width="189px" Text="Restrição por Falta de Combustível"
                                                                        Enabled="False"></asp:CheckBox></td>
                                                                <td style="width: 2px; height: 19px">
                                                                    <asp:CheckBox ID="chkRfcE" runat="server" Width="69px" Enabled="True"></asp:CheckBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 278px; height: 19px">
                                                                    <asp:CheckBox ID="chkRMP" runat="server" Width="157px" Enabled="False" Text="Garantia Energética"></asp:CheckBox></td>
                                                                <td style="width: 2px; height: 19px">
                                                                    <asp:CheckBox ID="chkRmpE" runat="server" Width="79px" Enabled="False"></asp:CheckBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 278px; height: 19px">
                                                                    <asp:CheckBox ID="chkGFM" runat="server" Width="132px" Enabled="False" Text="Geração Fora de Mérito"></asp:CheckBox></td>
                                                                <td style="width: 2px; height: 19px">
                                                                    <asp:CheckBox ID="chkGfmE" runat="server" Width="79px" Enabled="False"></asp:CheckBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 278px; height: 19px">
                                                                    <asp:CheckBox ID="chkCFM" runat="server" Width="129px" Enabled="False" Text="Crédito por Substituição"></asp:CheckBox></td>
                                                                <td style="width: 2px; height: 19px">
                                                                    <asp:CheckBox ID="chkCfmE" runat="server" Width="79px" Enabled="False"></asp:CheckBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 278px; height: 19px">
                                                                    <asp:CheckBox ID="chkSOM" runat="server" Width="161px" Enabled="False" Text="Geração Substituta"></asp:CheckBox></td>
                                                                <td style="width: 2px; height: 19px">
                                                                    <asp:CheckBox ID="chkSomE" runat="server" Width="79px" Enabled="False"></asp:CheckBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 278px; height: 19px">
                                                                    <asp:CheckBox ID="chkGES" runat="server" Width="161px" Enabled="False" Text="GE Substituição"></asp:CheckBox></td>
                                                                <td style="width: 2px; height: 19px">
                                                                    <asp:CheckBox ID="chkGesE" runat="server" Width="79px" Enabled="False"></asp:CheckBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 278px; height: 19px">
                                                                    <asp:CheckBox ID="chkGEC" runat="server" Width="161px" Enabled="False" Text="GE Crédito"></asp:CheckBox></td>
                                                                <td style="width: 2px; height: 19px">
                                                                    <asp:CheckBox ID="chkGecE" runat="server" Width="79px" Enabled="False"></asp:CheckBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 278px; height: 19px">
                                                                    <asp:CheckBox ID="chkDCA" runat="server" Width="161px" Enabled="False" Text="Despacho Ciclo Aberto"></asp:CheckBox></td>
                                                                <td style="width: 2px; height: 19px">
                                                                    <asp:CheckBox ID="chkDcaE" runat="server" Width="79px" Enabled="False"></asp:CheckBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 278px; height: 19px">
                                                                    <asp:CheckBox ID="chkDCR" runat="server" Width="161px" Enabled="False" Text="Despacho Ciclo Reduzido"></asp:CheckBox></td>
                                                                <td style="width: 2px; height: 19px">
                                                                    <asp:CheckBox ID="chkDcrE" runat="server" Width="79px" Enabled="False"></asp:CheckBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 278px; height: 19px">
                                                                    <asp:CheckBox ID="chkIR1" runat="server" Width="161px" Enabled="False" Text="Nível de Partida"></asp:CheckBox></td>
                                                                <td style="width: 2px; height: 19px">
                                                                    <asp:CheckBox ID="chkIr1E" runat="server" Width="79px" Enabled="False"></asp:CheckBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 278px; height: 19px">
                                                                    <asp:CheckBox ID="chkIR2" runat="server" Width="161px" Enabled="False" Text="Dia -1"></asp:CheckBox></td>
                                                                <td style="width: 2px; height: 19px">
                                                                    <asp:CheckBox ID="chkIr2E" runat="server" Width="79px" Enabled="False"></asp:CheckBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 278px; height: 19px">
                                                                    <asp:CheckBox ID="chkIR3" runat="server" Width="161px" Enabled="False" Text="Dia - 2"></asp:CheckBox></td>
                                                                <td style="width: 2px; height: 19px">
                                                                    <asp:CheckBox ID="chkIr3E" runat="server" Width="79px" Enabled="False"></asp:CheckBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 278px; height: 19px">
                                                                    <asp:CheckBox ID="chkIR4" runat="server" Width="161px" Enabled="False" Text="Carga da Ande"></asp:CheckBox></td>
                                                                <td style="width: 2px; height: 19px">
                                                                    <asp:CheckBox ID="chkIr4E" runat="server" Width="79px" Enabled="False"></asp:CheckBox></td>
                                                            </tr>

                                                            <tr>
                                                                <td style="width: 278px; height: 19px">
                                                                    <asp:CheckBox ID="chkRRO" runat="server" Width="200px" Enabled="False" Text="Recomposição de Reserva Operativa (RRO)"></asp:CheckBox></td>
                                                                <td style="width: 2px; height: 19px">
                                                                    <asp:CheckBox ID="chkRROE" runat="server" Width="70px" Enabled="False"></asp:CheckBox></td>
                                                            </tr>

                                                            <tr>
                                                                <td style="width: 498px; height: 24px" valign="middle" background="../pdpw/images/back_titulo.gif"
                                                                    colspan="3"></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 278px; height: 12px">
                                                                    <asp:CheckBox ID="chkRestricao" runat="server" Width="65px" Text="Restrição" Enabled="False"></asp:CheckBox></td>
                                                                <td style="width: 207px; height: 41px" colspan="2" rowspan="2">
                                                                    <asp:CheckBox ID="chkEnvia2" runat="server" Text="Manutenções e Restrições"></asp:CheckBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 278px; height: 20px">
                                                                    <asp:CheckBox ID="chkManutencao" runat="server" Width="78px" Text="Manutenção" Enabled="False"></asp:CheckBox></td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 1082px">
                                        <table>
                                            <tr>
                                                <td align="center" colspan="3">
                                                    <asp:Table ID="tblMensa" runat="server" Width="1376px" Font-Size="XX-Small">
                                                        <asp:TableRow BackColor="#99CC00" Font-Size="XX-Small" Font-Bold="True">
                                                            <asp:TableCell Text="Descri&#231;&#227;o"></asp:TableCell>
                                                            <asp:TableCell Text="Situa&#231;&#227;o"></asp:TableCell>
                                                            <asp:TableCell Text="Gera&#231;&#227;o"></asp:TableCell>
                                                            <asp:TableCell Text="Carga"></asp:TableCell>
                                                            <asp:TableCell Text="Interc&#226;mbio"></asp:TableCell>
                                                            <asp:TableCell Text="Vaz&#227;o"></asp:TableCell>
                                                            <asp:TableCell Text="Restri&#231;&#245;es"></asp:TableCell>
                                                            <asp:TableCell Text="Manuten&#231;&#245;es"></asp:TableCell>
                                                            <asp:TableCell Text="Inflexibilidade"></asp:TableCell>
                                                            <asp:TableCell Text="Raz&#227;o Energ&#233;tica"></asp:TableCell>
                                                            <asp:TableCell Text="Raz&#227;o El&#233;trica"></asp:TableCell>
                                                            <asp:TableCell Text="Exporta&#231;&#227;o"></asp:TableCell>
                                                            <asp:TableCell Text="Importa&#231;&#227;o"></asp:TableCell>
                                                            <asp:TableCell Text="Mot Desp Raz&#227;o El&#233;trica"></asp:TableCell>
                                                            <asp:TableCell Text="Mot Desp Inflexibilidade"></asp:TableCell>
                                                            <asp:TableCell Text="Perdas Consumo"></asp:TableCell>
                                                            <asp:TableCell Text="M&#225;quinas Paradas"></asp:TableCell>
                                                            <asp:TableCell Text="M&#225;quinas Operando"></asp:TableCell>
                                                            <asp:TableCell Text="M&#225;quinas Gerando"></asp:TableCell>
                                                            <asp:TableCell Text="Energ Repos e Perdas"></asp:TableCell>
                                                            <asp:TableCell Text="Maq por Conv Operativa"></asp:TableCell>
                                                            <asp:TableCell Text="Cota Inicial"></asp:TableCell>
                                                            <asp:TableCell Text="Rest Falta Comb"></asp:TableCell>
                                                            <asp:TableCell Text="Garantia Energética"></asp:TableCell>
                                                            <asp:TableCell Text="Gera&#231;&#227;o Fora de M&#233;rito"></asp:TableCell>
                                                            <asp:TableCell Text="Cr&#233;dito por Substitui&#231"></asp:TableCell>
                                                            <asp:TableCell Text="Gera&#231;&#227;o Substituta"></asp:TableCell>
                                                            <asp:TableCell Text="GE Substituição"></asp:TableCell>
                                                            <asp:TableCell Text="GE Crédito"></asp:TableCell>
                                                            <asp:TableCell Text="Despacho ciclo aberto"></asp:TableCell>
                                                            <asp:TableCell Text="Despacho ciclo reduzido"></asp:TableCell>
                                                            <asp:TableCell Text="Nível de Partida"></asp:TableCell>
                                                            <asp:TableCell Text="Dia -1"></asp:TableCell>
                                                            <asp:TableCell Text="Dia -2"></asp:TableCell>
                                                            <asp:TableCell Text="Carga da Ande"></asp:TableCell>
                                                            <asp:TableCell Text="Recomposi&#231;&#227;o de Reserva Operativa"></asp:TableCell>
                                                            <asp:TableCell Text="Programação Semanal"></asp:TableCell>
                                                        </asp:TableRow>
                                                    </asp:Table>
                                                </td>
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

