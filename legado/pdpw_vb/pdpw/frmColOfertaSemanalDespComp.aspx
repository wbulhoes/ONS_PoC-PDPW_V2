<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frmColOfertaSemanalDespComp.aspx.vb" Inherits="pdpw.frmColOfertaSemanalDespComp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>ONS - Operador Nacional do Sistema Elétrico</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5" />
    <link href="../pdpw/images/style.css" rel="stylesheet">
    <link href="../pdpw/css/bootstrap.min.css" rel="stylesheet">
    <link href="../pdpw/css/bootstrap-theme.min.css" rel="stylesheet">
    <script src="../pdpw/js/jquery.min.js" type="text/javascript"></script>
    <script src="../pdpw/js/bootstrap.min.js" type="text/javascript"></script>
    <script src="js/jquery.mask.js"></script>

    <style type="text/css">
        .auto-style1 {
            height: 21px;
            width: 13%;
        }
    </style>

    <script type="text/javascript">
        $(document).ready(function () {
            //$('input[id*="ValorCVU_"]').mask("9,99");
            $('input[id*="PrdUGeLigada_"]').mask("999,9");
            $('input[id*="PrdUGeDesligada_"]').mask("999,9");
            $('input[id*="ValorGeracaoMinima_"]').mask("999999");
            $('input[id*="PrdRampaSubQuente_"]').mask("99,9");
            $('input[id*="PrdRampaSubMorno_"]').mask("99,9");
            $('input[id*="PrdRampaSubFrio_"]').mask("99,9");
            $('input[id*="PrdRampaDesc_"]').mask("99,9");

            $('.number').mask('000000000000');
            $('.decimal').mask('ZyZyZyZyZyZy0yZyZyZyZyZyZyZyZyZy',
                {
                    translation: {
                        'Z': { pattern: /[0-9]/, optional: true },
                        'y': { pattern: /./, optional: true }
                    }
                });


            var sessaoMsg = '<%=Session("strMensagem")%>';

            if (sessaoMsg != null && sessaoMsg != "") {
                alert(sessaoMsg);
                <%Session("strMensagem") = ""%>
            }

        });

        //function validaCampoHora(e, txt){
        //    var tecla = (window.event) ? event.keyCode : e.which;   

        //    var valorDecimal = $('#' + txt).val().split(",");
        //    if (valorDecimal) {
        //        //var valorInteiro = ((tecla > 47 && tecla < 58) && (valorDecimal[0] >= 0 && valorDecimal[0] <= 9));
        //        var valorDecimal = ((tecla === 48 || tecla === 53) && (valorDecimal[1] === 0 || valorDecimal[1] === 5));
        //        if (valorDecimal) return true;
        //        else {
        //            if (tecla === 8 || tecla === 44) return true;
        //         else  return false;
        //        }
        //    }
        //};

        function validaCampoHora(txt) {
            if ($('#' + txt).val() === '0' || $('#' + txt).val() === '0,0') {
                alert('O valor tem que ser maior do que zero.');
                return false;
            }

            return true
        };

        function validaCampoMonetário(txt) {
            if ($('#' + txt).val() === '0' || $('#' + txt).val() === '0,00') {
                alert('O valor tem que ser maior do que zero.');
                return false;
            }

            return true
        };


        //function maskIt(w, e, m, r, a)
        function maskIt(w, e, r, a) {
            m = '#########,##';
            // Cancela se o evento for Backspace
            if (!e) var e = window.event
            if (e.keyCode) code = e.keyCode;
            else if (e.which) code = e.which;
            // Variáveis da função
            var txt = (!r) ? w.value.replace(/[^\d]+/gi, '') : w.value.replace(/[^\d]+/gi, '').reverse();
            var mask = (!r) ? m : m.reverse();
            var pre = (a) ? a.pre : "";
            var pos = (a) ? a.pos : "";
            var ret = "";
            if (code == 9 || code == 8 || txt.length == mask.replace(/[^#]+/g, '').length) return false;
            // Loop na máscara para aplicar os caracteres
            for (var x = 0, y = 0, z = mask.length; x < z && y < txt.length;) {
                if (mask.charAt(x) != '#') {
                    ret += mask.charAt(x); x++;
                }
                else {
                    ret += txt.charAt(y); y++; x++;
                }
            }
            // Retorno da função
            ret = (!r) ? ret : ret.reverse()
            w.value = pre + ret + pos;
        }
        // Novo método para o objeto 'String'
        String.prototype.reverse = function () {
            return this.split('').reverse().join('');
        };


        function validaCampoInteiro(txt) {
            if ($('#' + txt).val() === '0') {
                alert('O valor tem que ser maior do que zero.');
                return false;
            }

            return true
        };

    </script>

    <script language="javascript" type="text/javascript" src="js/MSGAguarde.js"></script>
    <link href="css/MSGAguarde.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container row" style="width: 100%;">
        <table style="width: 784px; height: 256px" height="256" cellspacing="0" cellpadding="0"
            width="784" border="0">
            <tbody>
                <tr>
                    <td valign="top" width="55" style="width: 55px">
                        <br>
                    </td>
                    <td style="width: 781px" valign="top">
                        <div align="center">
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tbody>
                                    <tr>
                                        <td width="20%" height="5" style="height: 5px">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td height="33" style="height: 33px">
                                            <table cellspacing="0" cellpadding="0" width="765" background="../pdpw/images/back_tit_sistema.gif"
                                                border="0">
                                                <tbody>
                                                    <tr>
                                                        <td>
                                                            <img height="25" src="../pdpw/images/tit_sis_guideline.gif" width="179"></td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td height="2">
                                            <table cellspacing="0" cellpadding="0" width="765" background="../pdpw/images/back_titulo.gif"
                                                border="0">
                                                <tbody>
                                                    <tr>
                                                        <td>
                                                            <div align="left">
                                                                <img height="23" src="../pdpw/images/tit_colosdc.gif" width="88" style="width: auto; height: 23px"></div>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                            <br>
                            <form id="frmColOfertaSemanalDespComp" name="frmColOfertaSemanalDespComp" runat="server" style="width: 1350px;">
                                <table style="width: 726px; height: 137px" cellspacing="0" cellpadding="0" border="0" style="font-family: Times New Roman;" font-size="Smaller">
                                    <tr>
                                        <td>
                                            <b>Empresa:</b>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DdlEmpresa" runat="server" AutoPostBack="true" Width="219px"></asp:DropDownList>
                                        </td>
                                        <td>
                                            <span style="font-size: 12px;"><strong style="color: red;">*&nbsp;Campos obrigatórios</strong></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <b>Semana PMO:</b>
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="PMOConsultaRadioButton" runat="server" GroupName="PMO" AutoPostBack="True" OnCheckedChanged="PMOConsultaRadioButton_CheckedChanged" />
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="PMOEdicaoRadioButton" runat="server" GroupName="PMO" AutoPostBack="True" OnCheckedChanged="PMOConsultaRadioButton_CheckedChanged" Checked="True" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <b>Data limite de envio:</b>
                                        </td>
                                        <td>
                                            <asp:Label ID="DataLimiteEnvioConsultaLabel" runat="server" Text="Label"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="DataLimiteEnvioEdicaoLabel" runat="server" Text="Label"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" style="margin-left: 10px;">
                                            <asp:Table ID="tblOfertaSemanalDespComp" runat="server" BorderWidth="1px" GridLines="Both" BorderStyle="Ridge"
                                                CellPadding="2" CellSpacing="0" Width="1299px">
                                            </asp:Table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:ImageButton ID="btnSalvar" runat="server" ImageUrl="images/bt_salvar.gif"></asp:ImageButton>
                                        </td>
                                    </tr>
                                </table>
                            </form>

                        </div>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</asp:Content>




















