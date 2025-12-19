<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frmCnsOfertaExportacao.aspx.vb" Inherits="pdpw.frmCnsOfertaExportacao" %>

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

    <script type="text/javascript" language="javascript">
        function showConfirm(message) {
            return confirm(message);
        }

        function showAlert(message) {
            alert(message)
        }

        function somenteNumeros(e) {
            var charCode = e.charCode ? e.charCode : e.keyCode;
            // charCode 8 = backspace   
            // charCode 9 = tab
            // charCode 13= ENTER
            if (charCode != 8 && charCode != 9 && charCode != 13) {
                // charCode 48 equivale a 0   
                // charCode 57 equivale a 9
                if (charCode < 48 || charCode > 57) {
                    return e.preventDefault();
                }
            }
        }


        function RetiraEnter(teclapres, obj) {
            var tecla = teclapres.keyCode;
            if (tecla == 13) {
                // Retira Caracteres especiais (ENTER, TAB, etc...) quando existirem 2 seguidos em qualquer lugar do texto
                vr = escape(obj.value);
                //TAB + ENTER
                vr = vr.replace('%09%0D%0A', '%09')
                //ENTER + TAB
                vr = vr.replace('%0D%0A%09', '%09')
                //ENTER + ENTER
                vr = unescape(vr.replace('%0D%0A%0D%0A', '%0D%0A'));
                obj.value = vr;
            }
        }
        function SalvarOferta() {
            var ObjstextArea = document.getElementsByTagName("TextArea");
            var ObjstextBox = document.getElementsByTagName("Input");
            var DatPDP = document.getElementById("_ctl0_ContentPlaceHolder1_DataPdpDropDown").value
            var Patamares = []
            var OfertaOrdem = []
            var OfertasDetalhe = []



            for (var i = 0; i < ObjstextBox.length; i++) {
                var ordem = ObjstextBox[i].value;
                var UsConv = ObjstextBox[i].name.split("_");
                var OfOrdem = []
                if (UsConv.length > 1 && UsConv[2] == "ordem") {
                    OfOrdem.push(UsConv[3], ordem);
                    OfertaOrdem.push(OfOrdem)
                    OfOrdem = []
                }
            }



            for (var j = 0; j < ObjstextArea.length; j++) {
                var valores = ObjstextArea[j].value.split("\n");
                var UsConv = ObjstextArea[j].name.split("_");

                Patamares.push(UsConv[2]);
                Patamares.push(UsConv[3]);
                Patamares.push(...valores);

                OfertasDetalhe.push(Patamares);
                Patamares = []
            }

            var dataPdp = document.getElementById("_ctl0_ContentPlaceHolder1_DataPdpDropDown").value;

            if (dataPdp === "" || dataPdp == null) {
                alert("Deve-se obrigatoriamente selecionar uma Data PDP.")
                return;
            }

            if (OfertaOrdem.length == 0 || OfertasDetalhe.length == 0) {
                alert("Deve-se obrigatoriamente inserir uma oferta.")
                return;
            }
            PageMethods.SalvarOferta(OfertaOrdem, OfertasDetalhe, DatPDP, ObterUsuarioLogado(), OnSuccess);

        }

        function ObterUsuarioLogado() {
            var username = '<%= Session("usuarID") %>';
            return username;
        }

        function OnSuccess(response, userContext, methodName) {
            alert(response);
        }


        function Totalizar() {
            var ObjstextArea = document.getElementsByTagName("TextArea");
            var total = new Array(48);

            for (var i = 0; i < total.length; i++) {
                total[i] = parseInt(0);
            }

            for (var j = 0; j < ObjstextArea.length; j++) {
                var valores = ObjstextArea[j].value.split("\n")

                for (var x = 0; x < valores.length; x++) {

                    if (valores[x] > 0) {
                        total[x] = parseInt(total[x]) + parseInt(valores[x]);
                    }
                }
            }

            for (var p = 0; p < total.length; p++) {
                var labelText = document.getElementById("_ctl0_ContentPlaceHolder1_Num_Patamar_" + (p + 1).toString());

                if (labelText != undefined) {
                    labelText.innerText = total[p].toString();
                }

            }

        }


    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <p>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    </p>

    <table style="width: 1100px; height: 266px" height="266" cellspacing="0" cellpadding="0"
        width="1100" border="0">
        <tbody>
            <tr>
                <td style="width: 136px; height: 250px" valign="top" width="136">
                    <br>
                </td>
                <asp:Literal ID="PopupBox" runat="server"></asp:Literal>
                <td style="width: 1064px; height: 250px" valign="top">
                    <div align="left">
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
                                                                                <img style="width: 120px; height: 23px" height="23" src="images/tit_ColExportacao.gif"
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
                        <form id="frmCnsOfertaExportacao" name="frmCnsOfertaExportacao" runat="server">

                            <asp:Label ID="lblMsg" Visible="False" ForeColor="Green" runat="server" Font-Bold="True" Font-Size="X-Small">Desculpe, este dado não pôde ser carregado no momento.</asp:Label>

                            <br />
                            <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>
                            <table style="width: 1078px; height: 124px" height="124" cellspacing="0" cellpadding="0" width="1078" border="0">
                                <tbody>
                                    <tr height="30">
                                        <td style="width: 170px; height: 2px" align="right">
                                            <label><b><span style="color: red">*</span>Data:</b> </label>
                                        </td>
                                        <td style="width: 881px; height: 2px">
                                            <p>
                                                &nbsp;<asp:DropDownList ID="DataPdpDropDown" runat="server" Width="141px" AutoPostBack="true" Height="26px"></asp:DropDownList>&nbsp;&nbsp;&nbsp;
                                            </p>
                                        </td>
                                    </tr>

                                    <tr height="30">
                                        <td style="width: 170px; height: 22px" align="right">
                                            <label><b><span style="color: red">*</span>Empresa:</b> </label>
                                        </td>
                                        <td style="width: 257px; height: 22px">&nbsp;<asp:DropDownList ID="cboEmpresa" runat="server" Width="200px" AutoPostBack="True" Height="27px"></asp:DropDownList></td>
                                    </tr>
                                    <tr height="30">
                                        <td style="width: 170px; height: 22px" align="right">
                                            <label><b><span style="color: red">*</span>Usina:</b></label></td>
                                        <td style="width: 257px; height: 22px">&nbsp;<asp:DropDownList ID="UsinaDropDown" runat="server" Width="300px" AutoPostBack="true" Height="27px"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr height="30">
                                        <td style="width: 170px; height: 22px" align="right">
                                            <asp:ImageButton ID="btnImgSalvar" runat="server" Width="71px" OnClientClick="SalvarOferta();" ImageUrl="../pdpw/images/bt_salvar.gif" Height="25px"></asp:ImageButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <asp:Label ID="lblMensagemAviso" runat="server" Style="color: red">Hora Limite para Envio de Oferta 10:00</asp:Label>
                                    </tr>
                                </tbody>
                            </table>
                        </form>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="width: 10px" valign="top" align="right">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                </td>
                <td style="width: 750px" valign="top">

                    <asp:Table ID="tblGeracao" runat="server" Width="51px" BorderStyle="Ridge" BorderWidth="1px"
                        CellPadding="2" CellSpacing="0" GridLines="Both" Height="26px" Font-Size="Smaller">
                    </asp:Table>

                </td>
            </tr>
        </tbody>
    </table>

</asp:Content>

