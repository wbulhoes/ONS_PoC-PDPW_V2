<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="PDPProgSemanal.aspx.vb" Inherits="pdpw.PDPProgSemanal" %>

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
        function dependentes() {
            if (document.getElementById("ChkDependentes").checked === true) {
                //document.getElementById("DdlUsinaTempoReal").removeAttribute("enabled", "");
                //document.getElementById("DdlUsinaTempoReal").setAttribute("disabled", "");
                //document.getElementById("DdlUsinaTempoReal").selectedIndex = document.getElementById("DdlUsinaProgramacao").selectedIndex;
            }

            if (document.getElementById("ChkDependentes").checked === false) {
                //document.getElementById("DdlUsinaTempoReal").removeAttribute("disabled", "");
                //document.getElementById("DdlUsinaTempoReal").setAttribute("enabled", "");
            }
        }



    </script>
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
                                        <p class="titulo">Programação Semanal</p>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <form id="PDPProgSemanal" name="PDPProgSemanal" runat="server">
            <div class="col-md-12" style="margin-bottom: 1%;">
                <div class="col-md-12">
                    <div class="col-md-2">
                        <label><b>Empresa:</b></label>
                        <div>
                            <asp:DropDownList ID="DdlEmpresa" runat="server" AutoPostBack="true" class="form-control"></asp:DropDownList>
                        </div>
                    </div>

                    <div class="col-md-8">
                        <h3 style="margin-top: 2%;">
                            <asp:Label ID="LblSemanaPMO" runat="server" Text="Semana PMO - "></asp:Label></h3>
                    </div>
                </div>
            </div>
            <div class="col-md-12">
                <div class="col-md-6" style="text-align: center;">
                    <h4 style="text-align: left; padding-left: 15px;"><b>Programação (D-1)</b></h4>
                </div>
                <div class="col-md-6" style="text-align: center;">
                    <h4 style="text-align: left; padding-left: 15px;"><b>Tempo Real (D-0)</b></h4>
                </div>
            </div>


            <div class="col-md-12">
                <div class="col-md-6" style="border-right: 1px solid #ccc;">
                    <div class="col-md-6">
                        <label><b>Produto</b></label>
                        <asp:Label ID="lblID" runat="server" Text="0" CssClass="hidden"></asp:Label>
                        <asp:DropDownList ID="DdlUsinaProgramacao" runat="server" AutoPostBack="False" class="form-control" onchange="dependentes()"></asp:DropDownList>
                    </div>
                    <div class="col-md-3">
                        <label><b>Volume (MW)</b></label>
                        <asp:TextBox ID="EdtVolumeProgramacao" runat="server" AutoPostBack="True" class="form-control number"></asp:TextBox>
                    </div>
                    <div class="col-md-3">
                        <label><b>Preço (R$/MWh)</b></label>
                        <asp:TextBox ID="EdtPrecoProgramacao" runat="server" AutoPostBack="True" class="form-control decimal" MaxLength="8"></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-6" style="border-left: 1px solid #ccc;">
                    <div class="col-md-6">
                        <label><b>Produto</b></label>
                        <asp:DropDownList ID="DdlUsinaTempoReal" runat="server" AutoPostBack="False" class="form-control"></asp:DropDownList>
                    </div>
                    <div class="col-md-3">
                        <label><b>Volume (MW)</b></label>
                        <asp:TextBox ID="EdtVolumeTempoReal" runat="server" AutoPostBack="True" class="form-control number"></asp:TextBox>
                    </div>
                    <div class="col-md-3">
                        <label><b>Preço (R$/MWh)</b></label>
                        <asp:TextBox ID="EdtPrecoTempoReal" runat="server" AutoPostBack="True" class="form-control decimal" MaxLength="8"></asp:TextBox>
                    </div>
                </div>
            </div>



            <div class="col-md-12">
                <div class="col-md-12" style="padding-left: 2%; padding-top: 1%;">
                    <asp:CheckBox ID="ChkDependentes" runat="server" Text=" Os Produtos Ofertados em (D-1) e (D-0) são dependentes." Style="font-size: 11px;" />
                </div>
            </div>
            <div class="col-md-12">
                <div class="col-md-12" style="padding-left: 2%; padding-top: 1%;">
                    <asp:ImageButton ID="BtnSalvar" runat="server" ImageUrl="images/bt_salvar.gif"></asp:ImageButton>
                    <asp:ImageButton ID="BtnIncluirNovo" runat="server" ImageUrl="images/bt_incluir.gif" />
                </div>
            </div>
        </form>

    </div>

    <% If (ofertaDaSemana.Count > 0) Then %>
    <div class="row" style="padding: 40px;">
        <div class="col-md-12">
            <table class="table table-hover">
                <thead>
                    <tr>
                        <%--<th scope="col" class="col-md-1"></th>--%>
                        <th scope="col" class="col-md-2">Produto</th>
                        <th scope="col" class="col-md-1">Volume (MW)</th>
                        <th scope="col" class="col-md-1">Preço (R$/MWh)</th>
                        <th scope="col" class="col-md-1">|</th>
                        <th scope="col" class="col-md-2">Produto</th>
                        <th scope="col" class="col-md-1">Volume (MW)</th>
                        <th scope="col" class="col-md-1">Preço (R$/MWh)</th>
                        <th scope="col" class="col-md-3"></th>
                    </tr>
                </thead>
                <tbody>
                    <%  If (ofertaDaSemana.Count > 0) Then
                            For Each item In ofertaDaSemana
                                Response.Write("<tr>")
                                'Response.Write("<th class='row'>" & item.OfertaSemanalId & "</th>")
                                Response.Write("<td>" & item.CodUsinaProgramacao & "</td>")
                                Response.Write("<td>" & item.VolumeProgramacao & "</td>")
                                Response.Write("<td>" & item.PrecoProgramacao & "</td>")
                                Response.Write("<td></td>")
                                Response.Write("<td>" & item.CodUsinaTempoReal & "</td>")
                                Response.Write("<td>" & item.VolumeTempoReal & "</td>")
                                Response.Write("<td>" & item.PrecoTempoReal & "</td>")
                                Response.Write("<td>")
                                Response.Write("<form name='PDPProgSemanalDelEdit' runat='server' method='post'>")
                                Response.Write("<button name='btnExcluir' value='" & item.OfertaSemanalId & "' class='btn btn-sm btn-default' id='" & item.OfertaSemanalId & "' runat='server' >Excluir</button>")
                                Response.Write("<button style='margin-left:5px;' name='btnEditar' value='" & item.OfertaSemanalId & "' class='btn btn-sm btn-default' id='" & item.OfertaSemanalId & "' runat='server' >Editar</button>")
                                Response.Write("</form>")
                                Response.Write("</td>")
                                Response.Write("</tr>")
                            Next
                        End If %>
                </tbody>
            </table>
        </div>

    </div>
    <% End If %>
</asp:Content>



















