'Classes base para manipulação de texto
Imports System.Text
'Classes base para manipulação do ambiente corrente
Imports System.Environment

Partial Class frmRelatorio
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents CrystalReportViewer1 As CrystalDecisions.Web.CrystalReportViewer

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        'objMenu = CType(Session("onsmenu"), OnsWebControls.OnsMenu)
        If Request.QueryString("strRegional") = "NE" Then
            'Botão para cadastro da potência sincronizada do Regional NE
            btnPotSinc.Visible = True
            btnRequisito.Visible = True
        Else
            btnPotSinc.Visible = False
            btnRequisito.Visible = False
        End If
        If Not Page.IsPostBack Then
            Dim objConn As New OnsClasses.OnsData.OnsConnection
            Dim objCmd As New OnsClasses.OnsData.OnsCommand
            Dim intI As Integer

            If Request.QueryString("strRegional") = "CNOS" Or Request.QueryString("strRegional") = "NE" Then
                cboRelatorio.AutoPostBack = True
            Else
                cboRelatorio.AutoPostBack = False
            End If

            objConn.Open("rpdp")
            objCmd.Connection = objConn
            objCmd.CommandText = "SELECT datpdp " & _
                                 "FROM pdp " & _
                                 "ORDER BY datpdp DESC"

            '& "WHERE datpdp = 20140815 " & _

            Dim rsData As OnsClasses.OnsData.OnsDataReader = objCmd.ExecuteReader
            intI = 1

            'cboData.Items.Clear()
            Dim objItem As WebControls.ListItem
            objItem = New WebControls.ListItem
            objItem.Text = ""
            objItem.Value = "0"
            cboData.Items.Add(objItem)

            Do While rsData.Read
                objItem = New System.Web.UI.WebControls.ListItem
                objItem.Text = Mid(rsData("datpdp"), 7, 2) & "/" & Mid(rsData("datpdp"), 5, 2) & "/" & Mid(rsData("datpdp"), 1, 4)
                objItem.Value = rsData("datpdp")
                cboData.Items.Add(objItem)
                If cboData.Items(intI).Value = Format(Session("datEscolhida"), "yyyyMMdd") Then
                    cboData.SelectedIndex = intI
                End If
                intI = intI + 1
            Loop

            rsData.Close()
            rsData = Nothing
            objCmd.Connection.Close()
            objConn.Close()

            'cboRelatorio.Items.Clear()

            Select Case Request.QueryString("strRegional")
                Case Is = "S"
                    addItem("Intercâmbios Prog/Reprog (MW)", "IntProgReprog", cboRelatorio)
                    addItem("Geração Hidro (MW)", "GerHidro", cboRelatorio)
                    addItem("Geração Termo (MW)", "GerTermo", cboRelatorio)
                    addItem("Cargas e Intercâmbios (MW)", "CargaInter", cboRelatorio)
                    addItem("Carga e Geração da COPEL", "GeracaoCargaCOPEL", cboRelatorio)
                    addItem("Geração da CEEE e Carga RGS", "GeracaoCargaS", cboRelatorio)
                    addItem("Inflexibilidade por Usina (MW)", "InflexUsina", cboRelatorio)
                    addItem("Relação de Usinas por Empresa", "UsiPorEmpre", cboRelatorio) '-- IM44972 -- Relatório de Usina por Empresa
                    addItem("Todos os Relatórios do SUL", "TodosS", cboRelatorio)
                    'Retirado em 19/07/2007 a pedido da Zélia (Núcleo Sul)
                    'addItem("Inflexibilidades Totais (MW)", "InflexTotal", cboRelatorio)
                Case Is = "SE"
                    addItem("Previsão de Carga", "CargaSE", cboRelatorio)
                    addItem("Geração por Usina", "GeracaoSE", cboRelatorio)
                    addItem("Programa de Defluência por Usina Hidrelétrica", "VazaoSE", cboRelatorio)
                    addItem("Intercâmbio Líquido", "InterSE", cboRelatorio)
                    addItem("Transferência de Energia entre Regiões", "TransfSE", cboRelatorio)
                    addItem("Restrições Operativas das Usinas Hidrelétricas e Termelétricas", "RestrSE", cboRelatorio)
                    addItem("Folga de Potência por Usina Hidrelétrica", "FolgaSE", cboRelatorio)
                    addItem("Balanço de Energia por Agente", "BalancoSE", cboRelatorio)
                    addItem("Reserva de Potência por Agente", "ReservaSE", cboRelatorio)
                    addItem("Razao do Despacho das Usinas Térmicas", "RazaoSE", cboRelatorio)
                    addItem("Configuração de Unidades Geradoras das Usinas Hidráulicas", "ConfigSE", cboRelatorio)
                    addItem("Todos", "TodosSE", cboRelatorio)
                Case Is = "NE"
                    addItem("Geração Horária das Usinas Hidráulicas", "GerHidroNE", cboRelatorio)
                    addItem("Geração Horária das Usinas Térmicas e Eólicas", "GerTermoNE", cboRelatorio)
                    addItem("Cronograma de Parada de Máquinas Hidráulicas e Térmicas", "ParadaNE_H", cboRelatorio)
                    'Foi retirado a partir de julho/08 e CAMACARI foi incluida no relatório HIDRO.
                    'addItem("Cronograma de Parada de Máquinas Térmicas e Eólicas", "ParadaNE_T", cboRelatorio)
                    addItem("Requisito por Áreas", "CargaNE", cboRelatorio)
                    addItem("Requisito / Geração", "ReqGerNE", cboRelatorio)
                    addItem("Todos os Relatórios em PDF", "TodosNE", cboRelatorio)
                    addItem("Todos os Relatórios em Arquivo Texto", "TodosNEtxt", cboRelatorio)
                Case Is = "NCO"
                    addItem("Geração Horária das Usinas Hidráulicas", "GerHidroNCO", cboRelatorio)
                    addItem("Geração Horária das Usinas Térmicas", "GerTermoNCO", cboRelatorio)
                    addItem("Previsão de Carga e Intercâmbios", "CargaNCO", cboRelatorio)
                    'addItem("Intercâmbios", "InterNCO", cboRelatorio)
                    addItem("Todos os Relatórios do Norte/Centro-Oeste", "TodosNCO", cboRelatorio)
                Case Is = "CNOS"
                    addItem("Previsão de Carga", "CargaCNOS", cboRelatorio)
                    addItem("Programa de Geração das Usinas", "GeracaoCNOS", cboRelatorio)
                    addItem("Programa de Defluência nas usinas", "VazaoCNOS", cboRelatorio)
                    addItem("Intercâmbio Líquido por Centro", "InterCNOS", cboRelatorio)
                    addItem("Transferência de Energia entre Regiões", "TransfCNOS", cboRelatorio)
                    addItem("Restrições Operativas das Usinas Hidrelétricas e Termelétricas", "RestrCNOS", cboRelatorio)
                    addItem("Folga de Potência por usina Hidrelétrica", "FolgaCNOS", cboRelatorio)
                    addItem("Balanço de Energia", "BalancoCNOS", cboRelatorio)
                    addItem("Reserva de Potência", "ReservaCNOS", cboRelatorio)
                    addItem("Razões do Despacho das Usinas Térmicas", "RazaoCNOS", cboRelatorio)
                    addItem("Configuração de Unidades Geradoras Hidráulicas", "ConfigCNOS", cboRelatorio)
                Case Is = "EC" '-- CRQ3713 (14/01/13)
                    '-- Descomentar linhas abaixo se necessitar testar isoladamente os subreportss de relTodosDeflux.rpt
                    'addItem("Programa Diário de Defluências das Bacias do SIN - PDF", "PDDefluxObs", cboRelatorio) '-- CRQ3713 (14/01/13)
                    'addItem("Programa Diário de Defluências das Bacias do SIN - PDF", "PDDefluxSIN", cboRelatorio) '-- CRQ3713 (14/01/13)
                    addItem("Programa Diário de Defluências das Bacias do SIN - PDF", "TodosDeflux", cboRelatorio) '-- CRQ3713 (14/01/13)
            End Select
            MarcaCombo()
        End If
    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        '-- Em caso de Debug, BreakPoint abaixo e pula essa parte
        MyBase.Render(writer)
        'objMenu.RenderControl(writer)
    End Sub

    Private Sub btnPesquisar_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnPesquisar.Click
        Select Case cboRelatorio.SelectedItem.Value
            Case Is = "GerHidro"
                '### SUL ###
                'Programação Diária - Geração Hidro (MW)
                Response.Redirect("frmResultado.aspx?strData=" & cboData.SelectedItem.Value & "&strRelatorio=relGeracao" & "&strTipoGer=Hidro&strRegional=S")
            Case Is = "GerTermo"
                'Programação Diária - Geração Termo (MW)
                Response.Redirect("frmResultado.aspx?strData=" & cboData.SelectedItem.Value & "&strRelatorio=relGeracao" & "&strTipoGer=Termo&strRegional=S")
            Case Is = "GeracaoCargaS"
                'Programação Diária - Geração CEEE e Carga RGS (MW)
                Response.Redirect("frmResultado.aspx?strData=" & cboData.SelectedItem.Value & "&strRelatorio=relGeracao" & "&strTipoGer=GeracaoCargaS&strRegional=S")
            Case Is = "GeracaoCargaCOPEL"
                'Programação Diária - Carga e Geração da COPEL (MW)
                Response.Redirect("frmResultado.aspx?strData=" & cboData.SelectedItem.Value & "&strRelatorio=relGeracao" & "&strTipoGer=GeracaoCargaCOPEL&strRegional=S")
            Case Is = "IntProgReprog"
                'Programação Diária - Intercâmbios Prog/Reprog (MW)
                Response.Redirect("frmResultado.aspx?strData=" & cboData.SelectedItem.Value & "&strRelatorio=relInterS" & "&strTipoGer=Inter&strRegional=S")
            Case Is = "CargaInter"
                'Programação Diária - Cargas e Intercâmbios (MW)
                Response.Redirect("frmResultado.aspx?strData=" & cboData.SelectedItem.Value & "&strRelatorio=relCargaInterS" & "&strTipoGer=CargaInter&strRegional=S")
            Case Is = "InflexTotal"
                'Programação Diária - Inflexibilidades Totais (MW)
                Response.Redirect("frmResultado.aspx?strData=" & cboData.SelectedItem.Value & "&strRelatorio=relInflexTotalS" & "&strTipoGer=InflexTotal&strRegional=S")
            Case Is = "InflexUsina"
                'Programação Diária - Inflexibilidades por Usina (MW)
                Response.Redirect("frmResultado.aspx?strData=" & cboData.SelectedItem.Value & "&strRelatorio=relInflexUsinaS" & "&strTipoGer=InflexUsina&strRegional=S")
                '-- Adicionado por conta da IM44972 (Início)
            Case Is = "UsiPorEmpre"
                'Relação de Usinas por Empresa -- IM44972
                Response.Redirect("frmResultado.aspx?strData=" & cboData.SelectedItem.Value & "&strRelatorio=relUsiPorEmpre" & "&strTipoGer=UsiPorEmpre&strRegional=S")
                '-- Adicionado por conta da IM44972 (Fim)
            Case Is = "TodosS"
                'Todos os dados do Centro Regional Sul em um único relatório
                Response.Redirect("frmResultado.aspx?strData=" & cboData.SelectedItem.Value & "&strRelatorio=relTodosS" & "&strTipoGer=TodosS&strRegional=S")

                '### SUDESTE ###'
            Case Is = "CargaSE"
                'Programação Diária - Previsão de Carga SE
                Response.Redirect("frmResultado.aspx?strData=" & cboData.SelectedItem.Value & "&strRelatorio=relCarga" & "&strTipoGer=CargaSE&strRegional=SE")
            Case Is = "GeracaoSE"
                'Programação Diária - Geração por Usina SE
                Response.Redirect("frmResultado.aspx?strData=" & cboData.SelectedItem.Value & "&strRelatorio=relGeracao" & "&strTipoGer=GeracaoSE&strRegional=SE")
            Case Is = "VazaoSE"
                'Programação Diária - Defluência por Usina Hidrelétrica (Vazão) SE
                Response.Redirect("frmResultado.aspx?strData=" & cboData.SelectedItem.Value & "&strRelatorio=relVazao" & "&strTipoGer=VazaoSE&strRegional=SE")
            Case Is = "InterSE"
                'Programação Diária - Intercâmbio Líquido SE
                Response.Redirect("frmResultado.aspx?strData=" & cboData.SelectedItem.Value & "&strRelatorio=relInter" & "&strTipoGer=InterSE&strRegional=SE")
            Case Is = "TransfSE"
                'Programação Diária - Tranferência de Energia entre Regiões SE
                Response.Redirect("frmResultado.aspx?strData=" & cboData.SelectedItem.Value & "&strRelatorio=relTransf" & "&strTipoGer=TransfSE&strRegional=SE")
            Case Is = "RestrSE"
                'Programação Diária - Restrição Operativa de Usinas SE
                Response.Redirect("frmResultado.aspx?strData=" & cboData.SelectedItem.Value & "&strRelatorio=relRestr" & "&strTipoGer=RestrSE&strRegional=SE")
            Case Is = "FolgaSE"
                'Programação Diária - Folga de Potência por Usina Hidrelétrica SE
                Response.Redirect("frmResultado.aspx?strData=" & cboData.SelectedItem.Value & "&strRelatorio=relFolga" & "&strTipoGer=FolgaSE&strRegional=SE")
            Case Is = "BalancoSE"
                'Programação Diária - Balanço de Energia por Agente SE
                Response.Redirect("frmResultado.aspx?strData=" & cboData.SelectedItem.Value & "&strRelatorio=relBalanco" & "&strTipoGer=BalancoSE&strRegional=SE")
            Case Is = "ReservaSE"
                'Programação Diária - Reserva de Potência por Agente SE
                Response.Redirect("frmResultado.aspx?strData=" & cboData.SelectedItem.Value & "&strRelatorio=relReserva" & "&strTipoGer=ReservaSE&strRegional=SE")
            Case Is = "RazaoSE"
                'Programação Diária - Razão do Despacho das Usinas Térmicas SE
                Response.Redirect("frmResultado.aspx?strData=" & cboData.SelectedItem.Value & "&strRelatorio=relRazao" & "&strTipoGer=RazaoSE&strRegional=SE")
            Case Is = "ConfigSE"
                'Programação Diária - Configuração de Unidades Geradoras das Usinas Hidrelétricas SE
                Response.Redirect("frmResultado.aspx?strData=" & cboData.SelectedItem.Value & "&strRelatorio=relConfig" & "&strTipoGer=ConfigSE&strRegional=SE")
            Case Is = "TodosSE"
                'Todos os itens dos relatórios anteriores - Arquivo Texto
                Response.Redirect("frmResultado.aspx?strData=" & cboData.SelectedItem.Value & "&strRelatorio=relTodos" & "&strTipoGer=TodosSE&strRegional=SE")

                '### NORDESTE ###'
            Case Is = "GerHidroNE"
                'Programação Diária - Geração Horária das Usinas Hidráulicas NE
                Response.Redirect("frmResultado.aspx?strData=" & cboData.SelectedItem.Value & "&strRelatorio=relGeracao" & "&strTipoGer=GerHidroNE&strRegional=NE")
            Case Is = "GerTermoNE"
                'Programação Diária - Geração Horária das Usinas Térmicas NE
                Response.Redirect("frmResultado.aspx?strData=" & cboData.SelectedItem.Value & "&strRelatorio=relGeracao" & "&strTipoGer=GerTermoNE&strRegional=NE")
            Case Is = "ParadaNE_H"
                'Programação Diária - Cronograma de Parada de Máquinas Hidráulicas - NE
                Response.Redirect("frmResultado.aspx?strData=" & cboData.SelectedItem.Value & "&strRelatorio=relParada" & "&strTipoGer=ParadaNE_H&strRegional=NE")
                'Case Is = "ParadaNE_T"
                '    'Programação Diária - Cronograma de Parada de Máquinas Térmicas e Eólicas - NE
                '    Response.Redirect("frmResultado.aspx?strData=" & cboData.SelectedItem.Value & "&strRelatorio=relParada" & "&strTipoGer=ParadaNE_T&strRegional=NE")
            Case Is = "CargaNE"
                'Programação Diária - Requisitos por Áreas NE
                Response.Redirect("frmResultado.aspx?strData=" & cboData.SelectedItem.Value & "&strRelatorio=relCarga" & "&strTipoGer=CargaNE&strRegional=NE")
            Case Is = "ReqGerNE"
                'Programação Diária - Requisitos / Geração NE
                Response.Redirect("frmResultado.aspx?strData=" & cboData.SelectedItem.Value & "&strRelatorio=relReqGer" & "&strTipoGer=ReqGerNE&strRegional=NE")
            Case Is = "TodosNE"
                'Todos os itens dos relatórios anteriores - PDF
                Response.Redirect("frmResultado.aspx?strData=" & cboData.SelectedItem.Value & "&strRelatorio=relTodosNE" & "&strTipoGer=TodosNE&strRegional=NE")
            Case Is = "TodosNEtxt"
                'Todos os itens dos relatórios anteriores - Arquivo Texto
                Response.Redirect("frmResultado.aspx?strData=" & cboData.SelectedItem.Value & "&strRelatorio=relTodos" & "&strTipoGer=TodosNEtxt&strRegional=NE")

                '### NORTE/CENTRO-OESTE ###'
            Case Is = "GerHidroNCO"
                'Programação Diária - Geração Horária das Usinas Hidráulicas N/CO
                Response.Redirect("frmResultado.aspx?strData=" & cboData.SelectedItem.Value & "&strRelatorio=relGeracaoNCO" & "&strTipoGer=GerHidroNCO&strRegional=NCO")
            Case Is = "GerTermoNCO"
                'Programação Diária - Geração Horária das Usinas Térmicas N/CO
                Response.Redirect("frmResultado.aspx?strData=" & cboData.SelectedItem.Value & "&strRelatorio=relGeracaoNCO" & "&strTipoGer=GerTermoNCO&strRegional=NCO")
            Case Is = "CargaNCO"
                'Programação Diária - Previsão de Carga e Intercâmbios - NCO
                Response.Redirect("frmResultado.aspx?strData=" & cboData.SelectedItem.Value & "&strRelatorio=relGeracaoNCO" & "&strTipoGer=CargaNCO&strRegional=NCO")
                'Case Is = "InterNCO"
                '    'Programação Diária - Intercâmbios - NCO
                '    Response.Redirect("frmResultado.aspx?strData=" & cboData.SelectedItem.Value & "&strRelatorio=relCargaNCO" & "&strTipoGer=InterNCO&strRegional=NCO")
            Case Is = "TodosNCO"
                'Programação Diária - Todos os Relatórios - NCO
                Response.Redirect("frmResultado.aspx?strData=" & cboData.SelectedItem.Value & "&strRelatorio=relTodosNCO" & "&strTipoGer=TodosNCO&strRegional=NCO")

                '### CNOS ###'
            Case Is = "CargaCNOS"
                'Programação Diária - Previsão de Carga - CNOS
                Response.Redirect("frmResultado.aspx?strData=" & cboData.SelectedItem.Value & "&strRelatorio=relCargaCNOS" & "&strTipoGer=CargaCNOS&strRegional=CNOS&strAgrega=" & cboAgrega.SelectedItem.Value)
            Case Is = "GeracaoCNOS"
                'Programação Diária - Programa de Geração das Usinas - CNOS
                Response.Redirect("frmResultado.aspx?strData=" & cboData.SelectedItem.Value & "&strRelatorio=relGeracaoCNOS" & "&strTipoGer=GeracaoCNOS&strRegional=CNOS&strAgrega=" & cboAgrega.SelectedItem.Value)
            Case Is = "VazaoCNOS"
                'Programação Diária - Programa de Defluência nas Usinas - CNOS
                Response.Redirect("frmResultado.aspx?strData=" & cboData.SelectedItem.Value & "&strRelatorio=relVazaoCNOS" & "&strTipoGer=VazaoCNOS&strRegional=CNOS&strAgrega=" & cboAgrega.SelectedItem.Value)
            Case Is = "InterCNOS"
                'Programação Diária - Intercâmbio Líquido por Área - CNOS
                Response.Redirect("frmResultado.aspx?strData=" & cboData.SelectedItem.Value & "&strRelatorio=relInterCNOS" & "&strTipoGer=InterCNOS&strRegional=CNOS")
            Case Is = "TransfCNOS"
                'Programação Diária - Transferência de Energia entre Regiões - CNOS
                Response.Redirect("frmResultado.aspx?strData=" & cboData.SelectedItem.Value & "&strRelatorio=relTransfCNOS" & "&strTipoGer=TransfCNOS&strRegional=CNOS")
            Case Is = "RestrCNOS"
                'Programação Diária - Restrições Operativas das Usinas Hidrelétricas e Termelétricas - CNOS
                Response.Redirect("frmResultado.aspx?strData=" & cboData.SelectedItem.Value & "&strRelatorio=relRestrCNOS" & "&strTipoGer=RestrCNOS&strRegional=CNOS&strAgrega=" & cboAgrega.SelectedItem.Value)
            Case Is = "FolgaCNOS"
                'Programação Diária - Folga de Potência por Usina Hidroelétrica - CNOS
                Response.Redirect("frmResultado.aspx?strData=" & cboData.SelectedItem.Value & "&strRelatorio=relFolgaCNOS" & "&strTipoGer=FolgaCNOS&strRegional=CNOS&strAgrega=" & cboAgrega.SelectedItem.Value)
            Case Is = "BalancoCNOS"
                'Programação Diária - Balanço de Energia por Agente - CNOS
                Response.Redirect("frmResultado.aspx?strData=" & cboData.SelectedItem.Value & "&strRelatorio=relBalancoCNOS" & "&strTipoGer=BalancoCNOS&strRegional=CNOS&strAgrega=" & cboAgrega.SelectedItem.Value)
            Case Is = "ReservaCNOS"
                'Programação Diária - Reserva de Potência por Agente - CNOS
                Response.Redirect("frmResultado.aspx?strData=" & cboData.SelectedItem.Value & "&strRelatorio=relReservaCNOS" & "&strTipoGer=ReservaCNOS&strRegional=CNOS&strAgrega=" & cboAgrega.SelectedItem.Value)
            Case Is = "RazaoCNOS"
                'Programação Diária - Razões do despacho das Usinas Térmicas - CNOS
                Response.Redirect("frmResultado.aspx?strData=" & cboData.SelectedItem.Value & "&strRelatorio=relRazaoCNOS" & "&strTipoGer=RazaoCNOS&strRegional=CNOS&strAgrega=" & cboAgrega.SelectedItem.Value)
            Case Is = "ConfigCNOS"
                'Programação Diária - Configuração de Unidades Geradoras Hidráulicas - CNOS
                Response.Redirect("frmResultado.aspx?strData=" & cboData.SelectedItem.Value & "&strRelatorio=relConfigCNOS" & "&strTipoGer=ConfigCNOS&strRegional=CNOS&strAgrega=" & cboAgrega.SelectedItem.Value)

                '### EC ###'
                'Programação Diária - Programa Diário de Defluências das Bacias do SIN - PDF
            Case Is = "TodosDeflux" '-- CRQ3713 (14/01/13)                
                'Response.Redirect("frmResultado.aspx?strData=" & cboData.SelectedItem.Value & "&strRelatorio=relTodosDeflux" & "&strTipoGer=TodosDeflux&strRegional=EC")
                Response.Redirect("frmResultado.aspx?strData=" & cboData.SelectedItem.Value & "&strRelatorio=relPDDefluxSIN" & "&strTipoGer=TodosDeflux&strRegional=EC")
            Case Is = "PDDefluxSIN" '-- CRQ3713 (14/01/13)
                'Programação Diária - Programa Diário de Defluências das Bacias do SIN - PDF
                Response.Redirect("frmResultado.aspx?strData=" & cboData.SelectedItem.Value & "&strRelatorio=relPDDefluxSIN" & "&strTipoGer=PDDefluxSIN&strRegional=EC")
            Case Is = "PDDefluxObs" '-- CRQ3713 (14/01/13)
                'Programação Diária - Programa Diário de Defluências das Bacias do SIN - PDF
                Response.Redirect("frmResultado.aspx?strData=" & cboData.SelectedItem.Value & "&strRelatorio=relPDDefluxObs" & "&strTipoGer=PDDefluxObs&strRegional=EC")

        End Select
    End Sub

    Private Sub cboRelatorio_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboRelatorio.SelectedIndexChanged
        Dim blnAgrega As Boolean
        lblAgrega.Visible = False
        cboAgrega.Visible = False
        Session("strRelatorio") = cboRelatorio.SelectedValue
        cboAgrega.Items.Clear()
        If cboRelatorio.SelectedIndex > 0 Then
            Select Case cboRelatorio.SelectedItem.Value
                Case Is = "CargaCNOS"
                    addItem("Agente", "age", cboAgrega)
                    addItem("Área", "are", cboAgrega)
                    addItem("Região/Subsistema/SIN", "reg", cboAgrega)
                    blnAgrega = True
                Case Is = "GeracaoCNOS", "RestrCNOS", "ReservaCNOS", "RazaoCNOS", "BalancoCNOS", "ConfigCNOS", "FolgaCNOS"
                    addItem("Agente", "age", cboAgrega)
                    addItem("Área", "are", cboAgrega)
                    addItem("Região", "reg", cboAgrega)
                    addItem("Subsistema", "sis", cboAgrega)
                    addItem("SIN", "sbr", cboAgrega)
                    blnAgrega = True
                Case Is = "VazaoCNOS"
                    addItem("Agente", "age", cboAgrega)
                    addItem("Bacia", "bac", cboAgrega)
                    blnAgrega = True
            End Select
            If blnAgrega Then
                lblAgrega.Visible = True
                cboAgrega.Visible = True
            End If
        End If
    End Sub

    Private Sub addItem(ByVal strTexto As String, ByVal strValor As String, ByRef objCombo As DropDownList)
        Dim objItem As WebControls.ListItem
        objItem = New System.Web.UI.WebControls.ListItem
        objItem.Text = strTexto
        objItem.Value = strValor
        objCombo.Items.Add(objItem)
    End Sub

    Private Sub MarcaCombo()
        Dim sender As System.Web.UI.WebControls.DropDownList
        Dim e As System.EventArgs
        Dim intI As Integer
        For intI = 0 To cboRelatorio.Items.Count - 1
            If Session("strRelatorio") = cboRelatorio.Items(intI).Value Then
                cboRelatorio.SelectedIndex = intI
                cboRelatorio_SelectedIndexChanged(sender, e)
                Exit For
            End If
        Next
    End Sub

    Private Sub cboData_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboData.SelectedIndexChanged
        Session("datEscolhida") = cboData.SelectedValue
    End Sub
End Class

