<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="PDPProgDiaria.aspx.vb" Inherits="pdpw.PDPProgDiaria" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>ONS - Operador Nacional do Sistema Elétrico</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <link href="../pdpw/images/style.css" rel="stylesheet">
    <link href="../pdpw/css/bootstrap.min.css" rel="stylesheet">
    <link href="../pdpw/css/bootstrap-theme.min.css" rel="stylesheet">
    <script src="../pdpw/js/jquery.min.js" type="text/javascript"></script>
    <script src="../pdpw/js/bootstrap.min.js" type="text/javascript"></script>
    <script src="js/jquery.mask.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            $('.number').mask('000000000000');
            $('.decimal').mask('ZyZyZyZyZyZy0yZyZyZyZyZyZyZyZyZy',
                {
                    translation: {
                        'Z': { pattern: /[0-9]/, optional: true },
                        'y': { pattern: /./, optional: true }
                    }
                });
        });

    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container row" style="width: 100%;">
        <div class="col-md-12">
            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                <tr>
                    <td width="20%" height="2">&nbsp;</td>
                </tr>
                <tr>
                    <td style="height: 17px" height="17">
                        <table cellspacing="0" cellpadding="0" width="765" background="../pdpw/images/back_tit_sistema.gif"
                            border="0">
                            <tr>
                                <td style="height: 12px">
                                    <img height="25" src="../pdpw/images/tit_sis_guideline.gif" width="179"></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="height: 10px" height="10">
                        <table cellspacing="0" cellpadding="0" width="765"
                            border="0">
                            <tr>
                                <td style="height: 8px">
                                    <div align="left">
                                        <p class="titulo">Programação Diária</p>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <div class="col-md-12">
            <form id="PDPProgDiaria" name="PDPProgDiaria" runat="server">
                <div class="col-md-12">
                    <div class="col-md-2">
                        <label><b>Data PDP</b></label>
                        <asp:DropDownList ID="cboData" runat="server" AutoPostBack="True" class="form-control"></asp:DropDownList>
                    </div>
                    <div class="col-md-3">
                        <label><b>Empresa</b></label>
                        <asp:DropDownList ID="cboEmpresa" runat="server" AutoPostBack="True" class="form-control"></asp:DropDownList>
                    </div>
                </div>

                <div class="col-md-12">
                    <div class="col-md-3">
                        <h5>
                            <label><b>Programação (D-1)</b></label></h5>
                    </div>
                    <div class="col-md-1"></div>
                    <div class="col-md-1"></div>
                    <div class="col-md-1"></div>
                    <div class="col-md-3">
                        <h5>
                            <label><b>Tempo Real (D-0)</b></label></h5>
                    </div>
                    <div class="col-md-1"></div>
                    <div class="col-md-1"></div>
                    <div class="col-md-1"></div>
                </div>

                <div class="col-md-12">

                    <div class="col-md-2">

                        <label><b>Produto</b></label>
                        <input type="text" id="EdtUsina" runat="server" class="form-control" disabled="disabled" />
                        <%--<asp:DropDownList ID="DdlUsinaProgramacao" runat="server" AutoPostBack="True" class="form-control"></asp:DropDownList>--%>
                    </div>
                    <div class="col-md-2">
                        <label><b>Volume (MW)</b></label>
                        <input type="text" id="EdtVolume" runat="server" class="form-control number" />

                    </div>
                    <div class="col-md-2">
                        <label><b>Preço (R$/MWh)</b></label>
                        <input type="text" id="EtdPreco" runat="server" class="form-control" disabled="disabled" />
                    </div>
                    <%--<div class="col-md-1" style="margin-top: 10px;">
                        
                    </div>--%>

                    <div class="col-md-2">
                        <label><b>Produto</b></label>
                        <input type="text" id="EdtUsinaTempoReal" runat="server" class="form-control" disabled="disabled" />
                        <%--<asp:DropDownList ID="DdlUsinaTempoReal" runat="server" AutoPostBack="True" class="form-control"></asp:DropDownList>--%>
                    </div>
                    <div class="col-md-2">
                        <label><b>Volume (MW)</b></label>
                        <input type="text" id="EdtVolumeTempoReal" runat="server" class="form-control number" />

                    </div>
                    <div class="col-md-2">
                        <label><b>Preço (R$/MWh)</b></label>
                        <input type="text" id="EtdPrecoTempoReal" runat="server" class="form-control" disabled="disabled" />
                    </div>
                    <%--<div class="col-md-1" style="margin-top: 10px;">
                       
                    </div>--%>
                </div>
                <div class="col-md-12" style="padding-left: 2%; padding-top: 1%;">
                    <asp:CheckBox ID="ChkDependentes" runat="server" Text=" Os Produtos Ofertados em (D-1) e (D-0) são dependentes." Enabled="false" />
                </div>
                <div class="col-md-12">
                    <div class="col-md-12" style="padding-left: 2%; padding-top: 1%;">
                        <asp:ImageButton ID="BtnSalvar" runat="server" ImageUrl="images/bt_salvar.gif"></asp:ImageButton>
                    </div>
                </div>
            </form>
        </div>
    </div>
</asp:Content>
