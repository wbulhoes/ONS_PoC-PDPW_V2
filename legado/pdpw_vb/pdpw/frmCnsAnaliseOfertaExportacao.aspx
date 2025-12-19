<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frmCnsAnaliseOfertaExportacao.aspx.vb" Inherits="pdpw.frmCnsAnaliseOfertaExportacao" %>

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
        function AprovarReprovarOfertaSelecionada(flgAprovado) {

            if (flgAprovado == '') {
                MarcarDesmarcarTodos(true);
            }

            var ObjsCheckBox = document.getElementsByClassName("cheUsinaConv");
            var DatPDP = document.getElementById("_ctl0_ContentPlaceHolder1_DataPdpDropDown").value;
            var codEmpresa = document.getElementById("_ctl0_ContentPlaceHolder1_cboEmpresa").value;

            var Patamares = [];
            var OfertaOrdem = [];
            var OfertasDetalhe = [];
            var convSelec = false;

            var urlParams = new URLSearchParams(window.location.search);
            var tblGeracao = document.getElementById("_ctl0_ContentPlaceHolder1_tblGeracao").innerHTML;

            var AnaliseONS = urlParams.get('AnaliseONS');

            var msg = ""

            if (flgAprovado == 'S') {
                msg = "aprovação";
            }
            else if (flgAprovado == 'N') {
                msg = "reprovação";
            }

            for (var i = 0; i < ObjsCheckBox.length; i++) {

                if (ObjsCheckBox[i].checked == true || flgAprovado == "O") {

                    console.log('ObjCheck Name: ' + ObjsCheckBox[i].name);
                    var UsConv = ObjsCheckBox[i].name.replace("_ctl0:ContentPlaceHolder1:","").split("_");
                    console.log('UsConv: ' + UsConv);

                    var Ordem = 0
                    var OfOrdem = [];

                    if (AnaliseONS == 'S') {
                        var txtArea = "_ctl0_ContentPlaceHolder1_txt_" + UsConv[1] + "_" + UsConv[2];
                        console.log('txtArea: ' + txtArea);
                        var ObjstextArea = document.getElementById(txtArea);
                        console.log('ObjstextArea: ' + ObjstextArea);
                        var txtAreaOrdem = "_ctl0_ContentPlaceHolder1_txt_ordem_" + UsConv[1];
                        console.log('txtAreaOrdem: ' + txtAreaOrdem);
                        var ObjstextAreaOrdem = document.getElementById(txtAreaOrdem);
                        console.log('ObjstextAreaOrdem: ' + ObjstextAreaOrdem);
                        var valores = ObjstextArea.value.split("\n");
                        console.log('valores: ' + valores);

                        Ordem = ObjstextAreaOrdem.value
                        console.log('Ordem: ' + Ordem);
                        Patamares.push(UsConv[1]);
                        console.log('Patamares[1]: ' + Patamares);
                        Patamares.push(UsConv[2]);
                        console.log('Patamares[2]: ' + Patamares);
                        Patamares.push(...valores);
                        console.log('Patamares[valores]: ' + Patamares);

                        OfertasDetalhe.push(Patamares);
                        console.log('OfertasDetalhe: ' + OfertasDetalhe);
                        Patamares = []
                    }

                    convSelec = true;
                    OfOrdem.push(UsConv[1], UsConv[2], Ordem, flgAprovado);
                    console.log('OfOrdem: ' + OfOrdem);
                    OfertaOrdem.push(OfOrdem)
                    console.log('OfertaOrdem: ' + OfertaOrdem);
                    OfOrdem = []
                }
            }


            if (msg != "") {

                if (!convSelec) {
                    alert("Selecione uma oferta para " + msg);
                    return;
                }

                if (flgAprovado == 'S' || flgAprovado == 'N') {

                    if (!confirm("Após a confirmação da " + msg + " as ofertas não poderão mais sofrer alterações." +
                        " Confirma a " + msg + " da análise?")) {
                        return;
                    }
                }

                if (flgAprovado == 'E') {

                    if (!confirm("Confirma a exclusão das ofertas selecionadas?")) {
                        return;
                    }
                }
            }

            if (flgAprovado == '') {
                MarcarDesmarcarTodos(false);
            }

            PageMethods.AprovarReprovarOfertaSelecionada(OfertaOrdem, OfertasDetalhe, DatPDP, codEmpresa, AnaliseONS, tblGeracao, ObterUsuarioLogado(), flgAprovado, ObterAgentesRepresentados(), OnSuccess);
        }

        function ObterUsuarioLogado() {
            console.log("Recuperando Usuário Logado...");
            var username = '<%= Session("usuarID") %>';
            console.log("Usuário logado recuperado: " + username);
            return username;
        }
        function ObterAgentesRepresentados() {
            console.log("Recuperando Agentes Representados da Sessão...");
            var retorno = '<%= If(IsNothing(Session("AgentesRepresentados")), String.Empty, Session("AgentesRepresentados"))  %>';
            console.log("Agentes Representados recuperado: (" + retorno + ")");
            return retorno;
        }

        function OnSuccess(response, userContext, methodName) {
            alert(response);
            //window.location.href = 'frmMensagem.aspx?strMensagem=' + response;
            //window.open('frmMensagem.aspx?strMensagem=' + response);
        }

        function OnSuccessDadosPercentual(result) {
            var ObjstextAreaTotLiquido = document.getElementsByTagName("TextArea");
            var total = new Array(48);
            let resultados = [];


            for (var j = 0; j < ObjstextAreaTotLiquido.length; j++) {

                for (var i = 0; i < total.length; i++) {
                    total[i] = parseInt(0);
                }

                var valores = ObjstextAreaTotLiquido[j].value.split("\n");

                const regex = /txt_([^_]+)_([^_]+)/;
                const resultado = ObjstextAreaTotLiquido[j].id.match(regex);

                const usina = resultado[1];
                const conversora = resultado[2];


                const valoresUsinaConversora = result.filter(v =>
                    v.CodUsina.trim() === usina.trim() &&
                    v.CodConversora.trim() === conversora.trim()
                );

                let valorConcatenacaoIdTotalLiquido = "";

                switch (conversora) {
                    case 'IEGBI1':
                        valorConcatenacaoIdTotalLiquido = "2";
                        break;
                    case 'IEGBI2':
                        valorConcatenacaoIdTotalLiquido = "3";
                        break;
                    case 'ESCMEL':
                        valorConcatenacaoIdTotalLiquido = "4";
                        break;
                    case 'ESRIV':
                        valorConcatenacaoIdTotalLiquido = "5";
                        break;
                }

                for (var x = 0; x < valores.length; x++) {

                    const valorPatamar = valoresUsinaConversora.filter(v => v.Num_Patamar === x + 1);

                    if (valores[x] == "") {
                        valores[x] = 0;
                    }

                    if (valorPatamar.length > 0) {
                        const valorComPerda = parseInt(valores[x]) - Math.round(valores[x] * (valorPatamar[0].valorPerdas / 100).toFixed(2), 0)
                        total[x] = parseInt(total[x]) + valorComPerda;
                    }

                    resultados.push({
                        valorConcatenacaoIdTotalLiquido: valorConcatenacaoIdTotalLiquido,
                        patamar: x + 1, // Considera que o patamar começa em 1
                        valorTotal: total[x]
                    });
                }
 
            }

            let agrupado = {};

            // Agrupamento e soma
            resultados.forEach(item => {
                // Cria a chave para o agrupamento
                let chave = `${item.patamar}_${item.valorConcatenacaoIdTotalLiquido}`;

                // Se a chave ainda não existe, cria uma nova entrada
                if (!agrupado[chave]) {
                    agrupado[chave] = {
                        valorConcatenacaoIdTotalLiquido: item.valorConcatenacaoIdTotalLiquido,
                        patamar: item.patamar,
                        valorTotal: 0
                    };
                }

                // Adiciona o valor ao total
                agrupado[chave].valorTotal += item.valorTotal;
            });

            // Converte o objeto agrupado de volta para um array
            let resultadosAgrupados = Object.values(agrupado);

            for (var p = 0; p < resultadosAgrupados.length; p++) {

                const stringConcatenada = "_ctl0_ContentPlaceHolder1_Num" + resultadosAgrupados[p].valorConcatenacaoIdTotalLiquido + "_Patamar_ONS_" + resultadosAgrupados[p].patamar.toString();

                var labelText = document.getElementById(stringConcatenada);

                if (labelText != undefined) {
                    labelText.innerText = resultadosAgrupados[p].valorTotal * (-1);
                }

            }
        }

        function Totalizar() {
            var ObjstextArea = document.getElementsByTagName("TextArea");
            var total = new Array(48);

            for (var i = 0; i < total.length; i++) {
                total[i] = parseInt(0);
            }

            for (var j = 0; j < ObjstextArea.length; j++) {

                var valores = ObjstextArea[j].value.split("\n");

                for (var x = 0; x < valores.length; x++) {

                    if (valores[x] == "") {
                        valores[x] = 0;
                    }

                    total[x] = parseInt(total[x]) + parseInt(valores[x]);

                }

            }

            for (var p = 0; p < total.length; p++) {
                var labelText = document.getElementById("_ctl0_ContentPlaceHolder1_Num_Patamar_ONS_" + (p + 1).toString());

                if (labelText != undefined) {
                    labelText.innerText = total[p].toString();
                }

            }

            var DatPDP = document.getElementById("_ctl0_ContentPlaceHolder1_DataPdpDropDown");

            if (DatPDP !== null) {
                var valoresUsinas = PageMethods.ObterDadosPercentualPercaUsinaParaAtualizacao(DatPDP.value, OnSuccessDadosPercentual);
            }

        }

        function MarcarTodosUsina(campo) {
            console.log("[MarcarTodosUsina] Parametro campo: " + campo)
            var objCheck = document.getElementsByTagName("Input");
            var ObjCheckPrincipal = document.getElementById("_ctl0_ContentPlaceHolder1_Chk_" + campo);

            for (var i = 0; i < objCheck.length; i++) {
                var ordem = objCheck[i].value

                if (objCheck[i].name.includes("Chk_" + campo + "_")) {
                    objCheck[i].checked = ObjCheckPrincipal.checked
                }
            }

        }

        function MarcarDesmarcarTodos(checked) {
            var objCheck = document.getElementsByTagName("Input");

            for (var i = 0; i < objCheck.length; i++) {
                var ordem = objCheck[i].value

                if (objCheck[i].name.includes("Chk_")) {
                    objCheck[i].checked = checked
                }
            }

        }

        function MarcarTodosZerados() {
            var valoresTextArea = document.getElementsByTagName("TextArea");

            for (var i = 0; i < valoresTextArea.length; i++) {
                const linhas = valoresTextArea[i].value.split('\n'); // Divide o valor por quebra de linha
                const todosZeros = linhas.every(linha => linha.trim() === "0" || linha.trim() === "");

                if (todosZeros) {
                    const stringConcat = "_ctl0_ContentPlaceHolder1_Chk" + valoresTextArea[i].id.substring(valoresTextArea[i].id.lastIndexOf('_', valoresTextArea[i].id.lastIndexOf('_') - 1));
                    var valorCheckUsina = document.getElementById(stringConcat);

                    valorCheckUsina.checked = true;
                }
            }
        }

        Totalizar();

    </script>



    <style type="text/css">
        .auto-style3 {
            width: 220px;
            height: 46px;
        }

        .auto-style4 {
            width: 750px;
            height: 46px;
        }

        .button_style1 {
            background-color: lightgreen;
            height: 25px;
            cursor: pointer;
            font-family: Arial;
            font-size: small;
            width: 160px;
            text-align: left;
            font-style: oblique;
            font-weight: bold;
        }

        .auto-style10 {
            background-color: none;
            height: 25px;
            cursor: pointer;
            font-family: Arial;
            font-size: small;
            width: 160px;
            text-align: left;
            font-style: oblique;
            font-weight: bold;
            cursor: pointer;
            border: none;
        }

        .auto-style11 {
            width: 220px
        }
        .auto-style12 {
            position: relative;
            min-height: 1px;
            float: left;
            width: 50%;
            left: 35px;
            top: 6px;
            padding-left: 15px;
            padding-right: 15px;
        }
        .auto-style13 {
            width: 1100px;
            height: 266px;
        }
        .auto-style14 {
            background-color: none;
            height: 25px;
            cursor: pointer;
            font-family: Arial;
            font-size: small;
            width: 245px;
            text-align: left;
            font-style: oblique;
            font-weight: bold;
            cursor: pointer;
            border: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table height="266" cellspacing="0" cellpadding="0"
        width="1100" border="0" class="auto-style13">
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
                                                            <table style="width: 867px; height: 26px" cellspacing="0" cellpadding="0" width="867" background="images/back_tit_sistema.gif"
                                                                border="0">
                                                                <tbody>
                                                                    <tr>
                                                                        <td style="height: 26px">
                                                                            <img height="25" src="images/tit_sis_guideline.gif" width="179"></td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="height: 10px" height="10">
                                                            <table style="width: 868px; height: 23px" cellspacing="0" cellpadding="0" width="868" background="images/back_titulo.gif"
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
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;

                                    <asp:Label ID="lblMsg" Visible="False" ForeColor="Green" runat="server" Font-Size="X-Small" Font-Bold="True">Desculpe, este dado não pôde ser carregado no momento.</asp:Label>

                            <br />
                            <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>
                            <table style="width: 1078px; height: 100px" cellspacing="0" cellpadding="0" width="1078" border="0">
                                <tbody>
                                    <tr>
                                        <td align="right" class="auto-style3"><b><span style="color: red">*</span>Data:</b></td>
                                        <td class="auto-style4">
                                            <p>
                                                &nbsp;<asp:DropDownList ID="DataPdpDropDown" runat="server" Width="141px" AutoPostBack="true" Height="26px"></asp:DropDownList>&nbsp;&nbsp;&nbsp;
                                            </p>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td align="right" class="auto-style3"><b><span style="color: red">*</span>Empresa:</b></td>
                                        <td class="auto-style4">&nbsp;<asp:DropDownList ID="cboEmpresa" runat="server" Width="200px" AutoPostBack="True" Height="27px"></asp:DropDownList>&nbsp;&nbsp;
                                <asp:ImageButton ID="btn_ExportaDadosParaExportacao" OnClientClick="AprovarReprovarOfertaSelecionada('');" runat="server" ImageUrl="images/bt_exportacao_balanco.gif" Visible="False" Height="27px" Style="position: relative; top: 15px;"></asp:ImageButton>
                                            <asp:ImageButton ID="btn_Iniciar_Analise_ONS" runat="server" ImageUrl="images/bt_iniciar_analise_ONS.gif" Visible="False" Height="27px" Style="position: relative; top: 15px;"></asp:ImageButton>
                                            <asp:ImageButton ImageUrl="images/bt_Reinicia_Valores_referencia.gif" ID="btnReiniciaValoresReferencia" runat="server" OnClientClick="return confirm('Os dados de referências das Usinas para a programação serão zerados no processo de Exportação. Deseja continuar?');" OnClick="reiniciarvaloresreferencia_Click" Visible="false" Width="151px" Height="27px" Style="position: relative; top: 15px;"></asp:ImageButton>
                                            <asp:ImageButton ImageUrl="images/bt_salvar_exportacao.gif" ID="btnSalvarExportacao" runat="server" OnClientClick="AprovarReprovarOfertaSelecionada('O');" Visible="false" Width="151px" Height="27px" Style="position: relative; top: -15px; left: 720px;"></asp:ImageButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <%--<td></td>--%>
                                        <td align="left" colspan="2">
                                            <input type="button" style="background: url(images/bt_selecionar_todas.gif) no-repeat;" class="auto-style10" id="btnSelecionarTodos" name="btnSelecionarTodos" runat="server" onclick="MarcarDesmarcarTodos(true);" /> 
                                            <input type="button" style="background: url(images/bt_Desmarcar_todas.gif) no-repeat;" class="auto-style10" id="btnDesmarcarTodos" name="btnDesmarcarTodos" runat="server" onclick="MarcarDesmarcarTodos(false);" />

                                       <%--     <input type="button" style="background: url(images/bt_selecionar_todas.gif) no-repeat; position: relative; top: -30px; left: -10px;" class="auto-style14" id="btnSelecionarTodos" name="btnSelecionarTodos" runat="server" Visible="false"
                                             onclick="MarcarDesmarcarTodos(true);" />
 
                                            <input type="button" style="background: url(images/bt_Desmarcar_todas.gif) no-repeat; position: relative; top: -30px; left: -100px;" class="auto-style14" id="btnDesmarcarTodos" name="btnDesmarcarTodos" runat="server" Visible="false"
                                             onclick="MarcarDesmarcarTodos(false);" />  --%>

                                            <asp:ImageButton ImageUrl="images/bt_aprovar_selecionadas.gif" ID="btnAprovarSelecionados" runat="server" Visible="false" OnClientClick="AprovarReprovarOfertaSelecionada('S');"></asp:ImageButton>
                                            <asp:ImageButton ImageUrl="images/bt_reprovar_selecionadas.gif" ID="btnReprovarSelcionados" runat="server" Visible="false" OnClientClick="AprovarReprovarOfertaSelecionada('N');"></asp:ImageButton>
                                            <asp:ImageButton ImageUrl="images/bt_reiniciar_analise.gif" ID="btnReiniciarDecisaoAnalise" runat="server" OnClientClick="return confirm('Deseja reaelmente reiniciar as análises de todas as ofertas?');" OnClick="reiniciardecisaoanalise_Click" Visible="false" Width="265px"></asp:ImageButton>
                                            <asp:ImageButton ImageUrl="images/bt_excluir_selecionadas.gif" ID="btn_excluir_ofertas_selecionadas" runat="server" Visible="false" OnClientClick="AprovarReprovarOfertaSelecionada('E');"></asp:ImageButton>

                                            <input type="button" 
                                             style="background: url(images/bt_seleciona_zerado_ons.gif) no-repeat; 
                                                 position: relative; 
                                                 top: -30px; 
                                                    left: 930px;" 
                                                    class="auto-style14" 
                                             id="BtnSelecionarZerados" 
                                             name="btnDesmarcarTodos" 
                                             runat="server"
                                             Visible="false"
                                             onclick="MarcarTodosZerados();" />

                                            <div style="display: inline-block; vertical-align: middle; margin-left: 10px;">
                                                <asp:FileUpload ID="fl_upload_planilha" runat="server" accept=".xlsx, .xls, .xlsm" Height="24px" Width="400px" CssClass="auto-style22" Visible="False" Style="position: relative; top: -145px; left: 150px;" />
                                            </div>
                                            <div style="display: inline-block; vertical-align: middle;">
                                                <asp:ImageButton ID="btn_Importar_Planilha" runat="server" ImageUrl="images/bt_importar_planilha.gif" Visible="False" Height="22px" Width="139px" Style="position: relative; top: -140px; left: 65px;" />
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style11">
                                            <!-- <asp:Label ID="lblMensagemAviso" runat="server" Style="color: red">Hora Limite para Análise 18:15</asp:Label> -->
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style11"><b></b></td>
                                    </tr>
                                </tbody>
                            </table>
                            <b>
                                <asp:Label runat="server" ID="AvisoSincroniaExportacao" Style="height: 27px;"></asp:Label></b>
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

                    <button onclick="exceller()">Exportar</button>

                </td>
            </tr>
        </tbody>
    </table>
    <br />
    <div class="row">
        <script>
            function exceller() {
                var uri = 'data:application/vnd.ms-excel;base64,',
                    template = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40"><head><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>{worksheet}</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--></head><body><table>{table}</table></body></html>',
                    base64 = function (s) {
                        return window.btoa(unescape(encodeURIComponent(s)))
                    },
                    format = function (s, c) {
                        return s.replace(/{(\w+)}/g, function (m, p) {
                            return c[p];
                        })
                    }
                var toExcel = document.getElementById("_ctl0_ContentPlaceHolder1_tblGeracao").innerHTML;
                var ctx = {
                    worksheet: name || '',
                    table: toExcel
                };
                var link = document.createElement("a");
                link.download = "export.xls";
                link.href = uri + base64(format(template, ctx))
                link.click();
            }
        </script>
    </div>
    <div class="row">
        <div class="row">
            <div class="auto-style12">
                <div class="col-sm-12">
                    <div class="row">
                        <div class="justify-content-start">
                            <p><b>Legenda:</b></p>
                        </div>
                        <div class="col-sm-1" style="background-color: beige; height: 15px; border: 1px solid black;"></div>
                        <div class="col-sm-10">Pendente análise ONS</div>
                        <div class="col-sm-1"></div>
                    </div>
                    <div class="row">
                        <div class="col-sm-1" style="background-color: yellow; height: 15px; border: 1px solid black;"></div>
                        <!--<div class="col-sm-10">Aguardando análise do Agente</div> -->
                        <div class="col-sm-10">Aprovado sem alterações</div>
                        <div class="col-sm-1"></div>
                    </div>
                    <div class="row">
                        <div class="col-sm-1" style="background-color: orange; height: 15px; border: 1px solid black;"></div>
                        <!-- <div class="col-sm-10">Aguardando análise do Agente (alterado pelo ONS)</div> -->
                        <div class="col-sm-10">Aprovado com alterações</div>
                        <div class="col-sm-1"></div>
                    </div>
                    <div class="row">
                        <div class="col-sm-1" style="background-color: LightGray; height: 15px; border: 1px solid black;"></div>
                        <div class="col-sm-10">Reprovado pelo ONS</div>
                        <div class="col-sm-1"></div>
                    </div>
                    <!--
                <div class="row">
                    <div class="col-sm-1" style="background-color: red; height: 15px; border: 1px solid black;"></div>
                    <div class="col-sm-10">Reprovado pelo Agente</div>
                    <div class="col-sm-1"></div>
                </div>
                <div class="row">
                    <div class="col-sm-1" style="background-color: lightgreen; height: 15px; border: 1px solid black;"></div>
                    <div class="col-sm-10">Aprovado pelo ONS e Agente</div>
                    <div class="col-sm-1"></div>
                </div>
                -->
                </div>
            </div>

        </div>
</asp:Content>
