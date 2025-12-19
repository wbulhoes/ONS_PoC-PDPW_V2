<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frmCnsObservacoes.aspx.vb" Inherits="pdpw.frmCnsObservacoes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>ONS - Operador Nacional do Sistema Elétrico</title>
    <meta http-equiv="Content-Type" content="text/html; charset=windows-1252">
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <%--<link href="images/style.css" rel="stylesheet">--%>
    <link href="../pdpw/images/style.css" rel="stylesheet" />
    <script language="javascript" type="text/javascript" src="js/MSGAguarde.js"></script>
    <link href="css/MSGAguarde.css" rel="stylesheet" />
    <link href="css/bootstrap.min.css" rel="stylesheet">
    <!--
    <script charset="UTF-8" type="text/javascript">
        window["adrum-start-time"] = new Date().getTime();
        (function (config) {
            config.appKey = "AD-AAB-ABA-HZF";
            config.adrumExtUrlHttp = "http://cdn.appdynamics.com";
            config.adrumExtUrlHttps = "https://cdn.appdynamics.com";
            config.beaconUrlHttp = "http://pdx-col.eum-appdynamics.com";
            config.beaconUrlHttps = "https://pdx-col.eum-appdynamics.com";
            config.resTiming = { "bufSize": 200, "clearResTimingOnBeaconSend": true };
            config.maxUrlLength = 512;
        })(window["adrum-config"] || (window["adrum-config"] = {}));
    </script>
    <script src="//cdn.appdynamics.com/adrum/adrum-20.9.0.3268.js"></script>
    -->
    <script type="text/javascript" language="javascript">
        function showConfirm(message) {
            return confirm(message);
        }

        function showAlert(message) {
            alert(message)
        }

        function RetiraEnter(teclapres) {
            var tecla = teclapres.keyCode;
            if (tecla == 13) {
                // Retira Caracteres especiais (ENTER, TAB, etc...) quando existirem 2 seguidos em qualquer lugar do texto
                vr = escape(document.frmColGeracao.txtValor.value);
                //TAB + ENTER
                vr = vr.replace('%09%0D%0A', '%09')
                //ENTER + TAB
                vr = vr.replace('%0D%0A%09', '%09')
                //ENTER + ENTER
                vr = unescape(vr.replace('%0D%0A%0D%0A', '%0D%0A'));
                document.frmColGeracao.txtValor.value = vr;
            }
        }

    </script>
    <style type="text/css">
        .auto-style1 {
            position: relative;
            min-height: 1px;
            float: left;
            width: 100%;
            left: 0px;
            top: 0px;
            height: 227px;
            padding-left: 15px;
            padding-right: 15px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <asp:Literal ID="PopupBox" runat="server"></asp:Literal>
        <table style="width: 966px; height: 93px" cellspacing="0" cellpadding="0"
            width="966" border="0">
            <tbody>
                <tr>
                    <td style="width: 55px; height: 100px" valign="top" width="75">
                        <br>
                    </td>
                    <td style="width: 1231px" valign="top">
                        <div align="center">
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tbody>
                                    <tr>
                                        <td width="20%" height="2">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td style="height: 17px" height="17">
                                            <table style="width: 867px; height: 26px" cellspacing="0" cellpadding="0" width="867" background="../pdpw/images/back_tit_sistema.gif"
                                                border="0">
                                                <tbody>
                                                    <tr>
                                                        <td style="height: 26px">
                                                            <img height="25" src="../pdpw/images/tit_sis_guideline.gif" width="179"></td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 10px" height="10">
                                            <table style="width: 868px; height: 23px" cellspacing="0" cellpadding="0" width="868" background="../pdpw/images/back_titulo.gif"
                                                border="0">
                                                <tbody>
                                                    <tr>
                                                        <td style="height: 8px">
                                                            <div align="left">
                                                                <img style="width: 120px; height: 23px" height="23" src="images/ComentariosDESSEM.gif"
                                                                    width="120">
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
                    </td>
                </tr>
            </tbody>
        </table>
        <form id="frmCnsObservacoes" name="frmCnsObservacoes" runat="server">
            <div class="row form-inline">
                <div class="form-group">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                    <label><b><span style="color: red">*</span>Data:</b> </label>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:DropDownList ID="DataPdpDropDown" runat="server" Width="141px" AutoPostBack="true" Height="26px"></asp:DropDownList></td>

                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                    <label><b><span style="color: red">*</span>Empresa:</b> </label>
                    &nbsp;&nbsp;&nbsp;
                    <asp:DropDownList ID="cboEmpresa" runat="server" Width="200px" AutoPostBack="True" Height="27px"></asp:DropDownList>

                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                    <label><b><span style="color: red">*</span>Insumo:</b></label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:DropDownList ID="InsumoDropDown" runat="server" Width="205px" AutoPostBack="true" Height="26px"></asp:DropDownList></td>
                </div>
            </div>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;

                                    <asp:Label ID="lblMsg" Visible="False" ForeColor="Green" runat="server" Font-Size="X-Small" Font-Bold="True">Desculpe, este dado não pôde ser carregado no momento.</asp:Label>

            <br />
            <div style="margin: 0px 50px 0px 50px;">
                <div class="row">
                    <div class="col-sm-7" style="border: 1px solid; height: 230px;">
                        <div class="justify-content-start">
                            <p><b>Análise DESSEM</b></p>
                        </div>
                        <div class="row">

                            <div class="col-sm-3">
                                <span style="font-family: Arial; font-size: X-Small; font-weight: bold;">Sem Sugestão</span>
                                <asp:ListBox runat="server" ID="SemSugestaoListBox" SelectionMode="Multiple" Style="width: 100%; height: 150px; margin: 0px 0px 0px 0px;"></asp:ListBox>
                            </div>
                            <div class="col-sm-1">
                                <br />
                                <br />
                                <br />

                                <button runat="server" id="btnIncluirSemSugestao" type="button" class="btn btn-default btn-sm" onclick="" value="2"><i class="glyphicon glyphicon-backward"></i></button>
                                <br />
                                <button runat="server" id="btnExcluirSemSugestao" type="button" class="btn btn-default btn-sm" onclick="" value="2"><i class="glyphicon glyphicon-forward"></i></button>

                            </div>
                            <div class="col-sm-4">
                                <span style="font-family: Arial; font-size: X-Small; font-weight: bold;">Lista de Usinas</span>
                                <asp:ListBox runat="server" ID="UsinaListBox" SelectionMode="Multiple" Style="width: 100%; height: 150px; margin: 0px 0px 0px 0px;"></asp:ListBox>
                            </div>
                            <div class="col-sm-1">
                                <br />
                                <br />
                                <br />
                                <button type="button" runat="server" id="btnIncluirComSugestao" class="btn btn-default btn-sm" onclick="" value="3"><i class="glyphicon glyphicon-forward"></i></button>
                                <br />
                                <button type="button" runat="server" id="btnExcluirComSugestao" class="btn btn-default btn-sm" onclick="" value="3"><i class="glyphicon glyphicon-backward"></i></button>
                            </div>
                            <div class="col-sm-3">
                                <span style="font-family: Arial; font-size: X-Small; font-weight: bold;">Com Sugestão</span>
                                <asp:ListBox runat="server" ID="ComSugestaoListBox" SelectionMode="Multiple" Style="width: 100%; height: 150px; margin: 0px 0px 0px 0px;"></asp:ListBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-5" style="border: 1px solid; height: 230px;">

                        <div class="auto-style1">
                            <br />
                            <label><b><span style="color: red">*</span>Usina:</b></label>
                            <asp:DropDownList ID="UsinaDropDown" runat="server" Width="222px" AutoPostBack="true" Height="28px"></asp:DropDownList></td>
                        
                            &nbsp;
                                                                    <asp:CheckBox ID="ChkEVT" runat="server" Width="214px" Text="Energia Vertida Turbinável" Visible="true"></asp:CheckBox>

                            <br />
                            <br />
                            <asp:Label runat="server" ID="lblDescricao"><b><span style="color:red">*</span>Descrição:</b></asp:Label>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Label runat="server" ID="lblComentarioONS"><b><span style="color:red">*</span>Comentário ONS:</b></asp:Label>
                            <br />
                            <asp:TextBox ID="TxtDescricao" runat="server" Style="border-style: groove; margin: 0px 0px 0px 0px;" Columns="1" TextMode="MultiLine"
                                ReadOnly="False" MaxLength="200" EnableViewState="False" Height="93px" Width="174px" Wrap="False"></asp:TextBox>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox ID="TxtComentarioONS" runat="server" Style="border-style: groove; margin: 0px 0px 0px 0px;" Columns="1" TextMode="MultiLine"
                                ReadOnly="False" MaxLength="200" Height="96px" Width="171px" Wrap="False"></asp:TextBox>


                            <br />
                            &nbsp;<label style="vertical-align: top"><b>(Até 200 caracteres)</b></label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        
                        </div>
                        <br />
                        <div class="pull-right">
                            <asp:ImageButton ID="btnImgSalvar" runat="server" Width="71px" ImageUrl="../pdpw/images/bt_salvar.gif" Height="25px"></asp:ImageButton>
                        </div>
                    </div>
                </div>
            </div>
            <br />
            <div class="row form-inline" style="display: none">
                <div class="form-group">
                    <label>Patamares:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; De:</label>
                    <asp:DropDownList ID="DropDownList_PatamarDe" runat="server" Width="150px" AutoPostBack="true" Height="26px">
                        <asp:ListItem Value="1" Selected="True">00:00 - 00:30</asp:ListItem>
                        <asp:ListItem Value="2">00:30 - 01:00</asp:ListItem>
                        <asp:ListItem Value="3">01:00 - 01:30</asp:ListItem>
                        <asp:ListItem Value="4">01:30 - 02:00</asp:ListItem>
                        <asp:ListItem Value="5">02:00 - 02:30</asp:ListItem>
                        <asp:ListItem Value="6">02:30 - 03:00</asp:ListItem>
                        <asp:ListItem Value="7">03:00 - 03:30</asp:ListItem>
                        <asp:ListItem Value="8">03:30 - 04:00</asp:ListItem>
                        <asp:ListItem Value="9">04:00 - 04:30</asp:ListItem>
                        <asp:ListItem Value="10">04:30 - 05:00</asp:ListItem>
                        <asp:ListItem Value="11">05:00 - 05:30</asp:ListItem>
                        <asp:ListItem Value="12">05:30 - 06:00</asp:ListItem>
                        <asp:ListItem Value="13">06:00 - 06:30</asp:ListItem>
                        <asp:ListItem Value="14">06:30 - 07:00</asp:ListItem>
                        <asp:ListItem Value="15">07:00 - 07:30</asp:ListItem>
                        <asp:ListItem Value="16">07:30 - 08:00</asp:ListItem>
                        <asp:ListItem Value="17">08:00 - 08:30</asp:ListItem>
                        <asp:ListItem Value="18">08:30 - 09:00</asp:ListItem>
                        <asp:ListItem Value="19">09:00 - 09:30</asp:ListItem>
                        <asp:ListItem Value="20">09:30 - 10:00</asp:ListItem>
                        <asp:ListItem Value="21">10:00 - 10:30</asp:ListItem>
                        <asp:ListItem Value="22">10:30 - 11:00</asp:ListItem>
                        <asp:ListItem Value="23">11:00 - 11:30</asp:ListItem>
                        <asp:ListItem Value="24">11:30 - 12:00</asp:ListItem>
                        <asp:ListItem Value="25">12:00 - 12:30</asp:ListItem>
                        <asp:ListItem Value="26">12:30 - 13:00</asp:ListItem>
                        <asp:ListItem Value="27">13:00 - 13:30</asp:ListItem>
                        <asp:ListItem Value="28">13:30 - 14:00</asp:ListItem>
                        <asp:ListItem Value="29">14:00 - 14:30</asp:ListItem>
                        <asp:ListItem Value="30">14:30 - 15:00</asp:ListItem>
                        <asp:ListItem Value="31">15:00 - 15:30</asp:ListItem>
                        <asp:ListItem Value="32">15:30 - 16:00</asp:ListItem>
                        <asp:ListItem Value="33">16:00 - 16:30</asp:ListItem>
                        <asp:ListItem Value="34">16:30 - 17:00</asp:ListItem>
                        <asp:ListItem Value="35">17:00 - 17:30</asp:ListItem>
                        <asp:ListItem Value="36">17:30 - 18:00</asp:ListItem>
                        <asp:ListItem Value="37">18:00 - 18:30</asp:ListItem>
                        <asp:ListItem Value="38">18:30 - 19:00</asp:ListItem>
                        <asp:ListItem Value="39">19:00 - 19:30</asp:ListItem>
                        <asp:ListItem Value="40">19:30 - 20:00</asp:ListItem>
                        <asp:ListItem Value="41">20:00 - 20:30</asp:ListItem>
                        <asp:ListItem Value="42">20:30 - 21:00</asp:ListItem>
                        <asp:ListItem Value="43">21:00 - 21:30</asp:ListItem>
                        <asp:ListItem Value="44">21:30 - 22:00</asp:ListItem>
                        <asp:ListItem Value="45">22:00 - 22:30</asp:ListItem>
                        <asp:ListItem Value="46">22:30 - 23:00</asp:ListItem>
                        <asp:ListItem Value="47">23:00 - 23:30</asp:ListItem>
                        <asp:ListItem Value="48">23:30 - 23:59</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="form-group">
                    <label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Até:</label>
                    <asp:DropDownList ID="DropDownList_PatamarAte" runat="server" Width="150px" AutoPostBack="true" Height="26px">
                        <asp:ListItem Value="1" Selected="True">00:00 - 00:30</asp:ListItem>
                        <asp:ListItem Value="2">00:30 - 01:00</asp:ListItem>
                        <asp:ListItem Value="3">01:00 - 01:30</asp:ListItem>
                        <asp:ListItem Value="4">01:30 - 02:00</asp:ListItem>
                        <asp:ListItem Value="5">02:00 - 02:30</asp:ListItem>
                        <asp:ListItem Value="6">02:30 - 03:00</asp:ListItem>
                        <asp:ListItem Value="7">03:00 - 03:30</asp:ListItem>
                        <asp:ListItem Value="8">03:30 - 04:00</asp:ListItem>
                        <asp:ListItem Value="9">04:00 - 04:30</asp:ListItem>
                        <asp:ListItem Value="10">04:30 - 05:00</asp:ListItem>
                        <asp:ListItem Value="11">05:00 - 05:30</asp:ListItem>
                        <asp:ListItem Value="12">05:30 - 06:00</asp:ListItem>
                        <asp:ListItem Value="13">06:00 - 06:30</asp:ListItem>
                        <asp:ListItem Value="14">06:30 - 07:00</asp:ListItem>
                        <asp:ListItem Value="15">07:00 - 07:30</asp:ListItem>
                        <asp:ListItem Value="16">07:30 - 08:00</asp:ListItem>
                        <asp:ListItem Value="17">08:00 - 08:30</asp:ListItem>
                        <asp:ListItem Value="18">08:30 - 09:00</asp:ListItem>
                        <asp:ListItem Value="19">09:00 - 09:30</asp:ListItem>
                        <asp:ListItem Value="20">09:30 - 10:00</asp:ListItem>
                        <asp:ListItem Value="21">10:00 - 10:30</asp:ListItem>
                        <asp:ListItem Value="22">10:30 - 11:00</asp:ListItem>
                        <asp:ListItem Value="23">11:00 - 11:30</asp:ListItem>
                        <asp:ListItem Value="24">11:30 - 12:00</asp:ListItem>
                        <asp:ListItem Value="25">12:00 - 12:30</asp:ListItem>
                        <asp:ListItem Value="26">12:30 - 13:00</asp:ListItem>
                        <asp:ListItem Value="27">13:00 - 13:30</asp:ListItem>
                        <asp:ListItem Value="28">13:30 - 14:00</asp:ListItem>
                        <asp:ListItem Value="29">14:00 - 14:30</asp:ListItem>
                        <asp:ListItem Value="30">14:30 - 15:00</asp:ListItem>
                        <asp:ListItem Value="31">15:00 - 15:30</asp:ListItem>
                        <asp:ListItem Value="32">15:30 - 16:00</asp:ListItem>
                        <asp:ListItem Value="33">16:00 - 16:30</asp:ListItem>
                        <asp:ListItem Value="34">16:30 - 17:00</asp:ListItem>
                        <asp:ListItem Value="35">17:00 - 17:30</asp:ListItem>
                        <asp:ListItem Value="36">17:30 - 18:00</asp:ListItem>
                        <asp:ListItem Value="37">18:00 - 18:30</asp:ListItem>
                        <asp:ListItem Value="38">18:30 - 19:00</asp:ListItem>
                        <asp:ListItem Value="39">19:00 - 19:30</asp:ListItem>
                        <asp:ListItem Value="40">19:30 - 20:00</asp:ListItem>
                        <asp:ListItem Value="41">20:00 - 20:30</asp:ListItem>
                        <asp:ListItem Value="42">20:30 - 21:00</asp:ListItem>
                        <asp:ListItem Value="43">21:00 - 21:30</asp:ListItem>
                        <asp:ListItem Value="44">21:30 - 22:00</asp:ListItem>
                        <asp:ListItem Value="45">22:00 - 22:30</asp:ListItem>
                        <asp:ListItem Value="46">22:30 - 23:00</asp:ListItem>
                        <asp:ListItem Value="47">23:00 - 23:30</asp:ListItem>
                        <asp:ListItem Value="48">23:30 - 23:59</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="form-group">
                    <label>Valor Sugerido:</label>
                    <asp:TextBox ID="TextBoxValorSugerido" runat="server" BorderStyle="Groove" TextMode="Number"></asp:TextBox>
                </div>
            </div>

            <div class="row form-inline">
            </div>
            <br />
            <div>
                <table style="width: 966px; height: 93px" cellspacing="0" cellpadding="0"
                    width="966" border="0">
                    <tbody>
                        <tr>
                            <td style="width: 75px; height: 100px" valign="top" width="75">
                                <br />
                                <p>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                </p>
                            </td>

                            <td style="height: 8px">
                                <div>
                                    <asp:Table ID="tblGeracao" runat="server" Width="51px" BorderStyle="Ridge" BorderWidth="1px"
                                        CellPadding="2" CellSpacing="0" GridLines="Both" Height="26px" Font-Size="Smaller" Visible="false">
                                    </asp:Table>
                                </div>

                                <div>
                                    <asp:DataGrid ID="dtgEnvio" runat="server"
                                        Width="476px" AutoGenerateColumns="False">
                                        <SelectedItemStyle BackColor="Lavender" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" VerticalAlign="Middle"></SelectedItemStyle>
                                        <AlternatingItemStyle BackColor="#F7F7F7" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" VerticalAlign="Middle"></AlternatingItemStyle>
                                        <ItemStyle Font-Size="Small" Font-Names="Arial" BackColor="#E9F4CF" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                        <EditItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" VerticalAlign="Middle" />
                                        <HeaderStyle Font-Size="12pt" Font-Names="Arial" Font-Bold="True" HorizontalAlign="Center" ForeColor="Black"
                                            BackColor="#99CC00"></HeaderStyle>
                                        <Columns>
                                            <asp:BoundColumn DataField="patamar" HeaderText="Patamar">
                                                <HeaderStyle Width="25%"></HeaderStyle>
                                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" VerticalAlign="Middle" />
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="vlr_dessem" HeaderText="Valor DESSEM" ReadOnly="True">
                                                <HeaderStyle Width="25%"></HeaderStyle>
                                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" VerticalAlign="Middle" />
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="vlr_sugerido" HeaderText="Valor Sugerido">
                                                <HeaderStyle Width="25%"></HeaderStyle>
                                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" VerticalAlign="Middle" />
                                            </asp:BoundColumn>
                                        </Columns>
                                    </asp:DataGrid>

                                    <div id="divValor" style="display: inline; left: 180px; width: 82px; position: absolute; top: 270px; height: 21px"
                                        runat="server" ms_positioning="FlowLayout">
                                    </div>

                                </div>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <br />
        </form>
        <div class="row">
            <div class="col-sm-4">
                <div class="col-sm-8">
                    <div class="row">
                        <div class="justify-content-start">
                            <p><b>Legenda:</b></p>
                        </div>
                        <div class="col-sm-1" style="background-color: beige; height: 10px; border: 1px solid black;"></div>
                        <div class="col-sm-6">Não analisado</div>
                        <div class="col-sm-5"></div>
                    </div>
                    <div class="row">
                        <div class="col-sm-1" style="background-color: coral; height: 10px; border: 1px solid black;"></div>
                        <div class="col-sm-6">Selecionado para análise</div>
                        <div class="col-sm-5"></div>
                    </div>
                    <div class="row">
                        <div class="col-sm-1" style="background-color: yellow; height: 10px; border: 1px solid black;"></div>
                        <div class="col-sm-6">Em análise pelo ONS</div>
                        <div class="col-sm-5"></div>
                    </div>
                    <div class="row">
                        <div class="col-sm-1" style="background-color: red; height: 10px; border: 1px solid black;"></div>
                        <div class="col-sm-6">Recusado pelo ONS</div>
                        <div class="col-sm-5"></div>
                    </div>
                    <div class="row">
                        <div class="col-sm-1" style="background-color: green; height: 10px; border: 1px solid black;"></div>
                        <div class="col-sm-6">Aceito pelo ONS ou sem sugestões</div>
                        <div class="col-sm-5"></div>
                    </div>
                </div>
            </div>

        </div>
</asp:Content>

