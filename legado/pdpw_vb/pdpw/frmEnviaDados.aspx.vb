Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports System.Text

Partial Class frmEnviaDados
    Inherits BaseWebUi
    Private logger As log4net.ILog = log4net.LogManager.GetLogger(Me.GetType())

    Private strNomeArquivo As String 'Variável que será transportada para o recibo
    Protected strOpcao As String
    Protected strListErro As String = ""
    Protected tbMensagem As DataTable = New DataTable()

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        'objMenu = CType(Session("onsmenu"), OnsWebControls.OnsMenu)
        If Not Page.IsPostBack Then
            Try
                If Session("datEscolhida") = Nothing Then
                    'Inicializa a variável com data do próximo
                    Session("datEscolhida") = Now.AddDays(1)
                End If

                If Session("strCodEmpre") = Nothing Then
                    'Inicializa a variável com data do próximo
                    Session("strCodEmpre") = ""
                End If

                PreencheComboData(cboData, Format(Session("datEscolhida")), True)
                PreencheComboEmpresaPOP(AgentesRepresentados, cboEmpresa, Session("strCodEmpre"))
                If cboEmpresa.SelectedIndex > 0 Then
                    cboEmpresa_SelectedIndexChanged(sender, e)
                End If
            Catch
                Session("strMensagem") = "Não foi possível acessar os dados, tente novamente."
                Response.Redirect("frmMensagem.aspx")
            End Try
        End If
    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        If SessaoAtiva(Page.Session) Then
            Funcoes.RetirarItensSelecionadosComDuplicidade(cboData)
            Funcoes.RetirarItensSelecionadosComDuplicidade(cboEmpresa)

            MyBase.Render(writer)
            'objMenu.RenderControl(writer)
        End If
    End Sub


#Region "Demanda 27513"
    '#4 
    Private Sub ExibirAlerta(ByVal mensagem As String)
        Response.Write("<SCRIPT>alert('" + mensagem + "')</SCRIPT>")
    End Sub



    '#3 
    Private Sub ProcessarMensagem(ByVal listaUsinasComValorNegativo As List(Of String))

        Dim mensagem As StringBuilder = New StringBuilder()
        Dim msg As String = "Existem Usinas que passaram do limite de inflexibilidade para Semana PMO ou com declaração zerada."

        lblMensagem.Text = ""
        lblMensagem.Visible = False

        If listaUsinasComValorNegativo.Count > 0 Then
            mensagem.Append("O somatório de inflexibilidade declarado para a(s) usina(s) abaixo excedeu o valor declarado para a semana PMO e não será considerado no DESSEM:<br /><br />")

            For Each usina As String In listaUsinasComValorNegativo
                mensagem.Append($"{usina}<br />")
            Next

            lblMensagem.Style.Add("text-align", "justify")
            lblMensagem.ForeColor = Color.Red
            lblMensagem.Text = mensagem.ToString()
            lblMensagem.Visible = True
            Me.ExibirAlerta(msg)

        End If

    End Sub

    '#2 
    Private Sub CalculaSaldoLimitePMO(ByVal dataPDP As String, ByVal codEmpresa As String)

        Dim data As String = DateTime.Parse(dataPDP + " 00:00:00").ToString("yyyyMMdd")

        Me.FactoryBusiness.SaldoInflexibilidadeSemanaPMO.CalcularSaldoInflexibilidadeSemanaPMO(data, codEmpresa)

        Dim listaUsinaSaldoNegativo As List(Of String) = Me.FactoryBusiness.InflexibilidadeBusiness.EnviaLimiteDADGER(data, codEmpresa)
        Me.ProcessarMensagem(listaUsinaSaldoNegativo)

    End Sub

    '#1 
    Private Sub ValidarCriticaInflexibilidade()

        If cboData.SelectedIndex <> 0 And cboEmpresa.SelectedIndex <> 0 And chkInflexibilidade.Checked = True Then
            CalculaSaldoLimitePMO(cboData.SelectedItem.Value.Trim(), cboEmpresa.SelectedItem.Value.Trim())
        End If

    End Sub


#End Region

    Private Sub cboEmpresa_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboEmpresa.SelectedIndexChanged
        If cboData.SelectedIndex <> 0 Then
            Session("datEscolhida") = CDate(cboData.SelectedItem.Text)
        End If
        If cboEmpresa.SelectedIndex <> 0 Then
            Session("strCodEmpre") = cboEmpresa.SelectedItem.Value
        End If
        If cboData.SelectedIndex > 0 And cboEmpresa.SelectedIndex > 0 Then
            PreencheCabecalho()
            PreencheTable()
            VerificaDigitacao()

            ValidarCriticaInflexibilidade()

            Dim lRetorno As Integer = 0
            If Not ValidaLimiteEntradaDados(cboEmpresa.SelectedItem.Value, cboData.SelectedItem.Value, lRetorno) Then
                btnEnviar.Visible = False
                If lRetorno = 1 Then
                    Response.Write("<SCRIPT>alert('" + strMsgInicioLimiteEnvioDadosAux + "')</SCRIPT>")
                Else
                    Response.Write("<SCRIPT>alert('" + strMsgLimiteEnvioDadosAux + "')</SCRIPT>")
                End If
                Exit Sub
            Else
                btnEnviar.Visible = True
            End If
        End If
    End Sub

    Private Sub PreencheCabecalho()
        Dim objRow As TableRow
        Dim objCell As TableCell

        'limpa a tabela
        tblMensa.Rows.Clear()
        Dim objTamanho As System.Web.UI.WebControls.Unit
        objTamanho = New Unit

        objRow = New TableRow
        objRow.BackColor = System.Drawing.Color.YellowGreen
        objRow.Font.Bold = True

        'Descrição
        objCell = New TableCell
        objCell.Width = objTamanho.Pixel(100)
        objCell.Text = "Descrição"
        objRow.Controls.Add(objCell)

        'Situação
        objCell = New TableCell
        objCell.Width = objTamanho.Pixel(100)
        objCell.Text = "Situação"
        objRow.Controls.Add(objCell)

        'Geração
        objCell = New TableCell
        objCell.Width = objTamanho.Pixel(100)
        objCell.Text = "Geração"
        objRow.Controls.Add(objCell)

        'Recomposição de Reserva Operativa
        objCell = New TableCell
        objCell.Width = objTamanho.Pixel(100)
        objCell.Text = "RRO"
        objRow.Controls.Add(objCell)

        'Carga
        objCell = New TableCell
        objCell.Width = objTamanho.Pixel(100)
        objCell.Text = "Carga"
        objRow.Controls.Add(objCell)

        'Intercâmbio
        objCell = New TableCell
        objCell.Width = objTamanho.Pixel(100)
        objCell.Text = "Intercâmbio"
        objRow.Controls.Add(objCell)

        'Vazão
        objCell = New TableCell
        objCell.Width = objTamanho.Pixel(100)
        objCell.Text = "Vazão"
        objRow.Controls.Add(objCell)

        'Restrição
        objCell = New TableCell
        objCell.Width = objTamanho.Pixel(100)
        objCell.Text = "Restrição"
        objRow.Controls.Add(objCell)

        'Manutenção
        objCell = New TableCell
        objCell.Width = objTamanho.Pixel(100)
        objCell.Text = "Manutenção"
        objRow.Controls.Add(objCell)

        'Inflexibilidade
        objCell = New TableCell
        objCell.Width = objTamanho.Pixel(100)
        objCell.Text = "Inflexibilidade"
        objRow.Controls.Add(objCell)

        'Razão Energética
        objCell = New TableCell
        objCell.Width = objTamanho.Pixel(100)
        objCell.Text = "Razão Energética"
        objRow.Controls.Add(objCell)

        'Razão Elétrica
        objCell = New TableCell
        objCell.Width = objTamanho.Pixel(100)
        objCell.Text = "Razão Elétrica"
        objRow.Controls.Add(objCell)

        'Exportação
        objCell = New TableCell
        objCell.Width = objTamanho.Pixel(100)
        objCell.Text = "Exportação"
        objRow.Controls.Add(objCell)

        'Importação
        objCell = New TableCell
        objCell.Width = objTamanho.Pixel(100)
        objCell.Text = "Importação"
        objRow.Controls.Add(objCell)

        'Motivo de Despacho Razão Elétrica
        objCell = New TableCell
        objCell.Width = objTamanho.Pixel(100)
        objCell.Text = "Mot Desp Razão Elétrica"
        objRow.Controls.Add(objCell)

        'Motivo de Despacho por Inflexibilidade
        objCell = New TableCell
        objCell.Width = objTamanho.Pixel(100)
        objCell.Text = "Mot Desp Inflexibilidade"
        objRow.Controls.Add(objCell)

        'Perdas Consumo Interno e Compensação
        objCell = New TableCell
        objCell.Width = objTamanho.Pixel(100)
        objCell.Text = "Perdas Consumo"
        objRow.Controls.Add(objCell)

        'Número Máquinas Paradas por Conveniência Operativa
        objCell = New TableCell
        objCell.Width = objTamanho.Pixel(100)
        objCell.Text = "Máquinas Paradas"
        objRow.Controls.Add(objCell)

        'Número Máquinas Operando como Síncrono
        objCell = New TableCell
        objCell.Width = objTamanho.Pixel(100)
        objCell.Text = "Máquinas Operando"
        objRow.Controls.Add(objCell)

        'Número Máquinas Gerando
        objCell = New TableCell
        objCell.Width = objTamanho.Pixel(100)
        objCell.Text = "Máquinas Gerando"
        objRow.Controls.Add(objCell)

        'Energia de Reposição e Perdas
        objCell = New TableCell
        objCell.Width = objTamanho.Pixel(100)
        objCell.Text = "Energia Reposição"
        objRow.Controls.Add(objCell)

        'Disponibilidade
        objCell = New TableCell
        objCell.Width = objTamanho.Pixel(100)
        objCell.Text = "Disponibilidade"
        objRow.Controls.Add(objCell)

        'Compensação de Lastro Físico
        objCell = New TableCell
        objCell.Width = objTamanho.Pixel(100)
        objCell.Text = "Compensação Lastro"
        objRow.Controls.Add(objCell)

        'Parada de Máquinas por Conveniência Operativa
        objCell = New TableCell
        objCell.Width = objTamanho.Pixel(100)
        objCell.Text = "Maq por Conv Operativa"
        objRow.Controls.Add(objCell)

        'Cota Inicial
        objCell = New TableCell
        objCell.Width = objTamanho.Pixel(100)
        objCell.Text = "Cota Inicial"
        objRow.Controls.Add(objCell)

        'Restrição Falta Combustível
        objCell = New TableCell
        objCell.Width = objTamanho.Pixel(100)
        objCell.Text = "Rest Falta Comb"
        objRow.Controls.Add(objCell)

        'Garantia Energética
        objCell = New TableCell
        objCell.Width = objTamanho.Pixel(100)
        objCell.Text = "Garantia Energética"
        objRow.Controls.Add(objCell)

        'Geração Fora de Mérito
        objCell = New TableCell
        objCell.Width = objTamanho.Pixel(100)
        objCell.Text = "Geração Fora de Mérito"
        objRow.Controls.Add(objCell)

        'Crédito por Substituição
        objCell = New TableCell
        objCell.Width = objTamanho.Pixel(100)
        objCell.Text = "Crédito por Substituição"
        objRow.Controls.Add(objCell)

        'Geração Substituta
        objCell = New TableCell
        objCell.Width = objTamanho.Pixel(100)
        objCell.Text = "Geração Substituta"
        objRow.Controls.Add(objCell)

        'GE Substituição
        objCell = New TableCell
        objCell.Wrap = False
        objCell.Text = "GE Substituição"
        objRow.Controls.Add(objCell)

        'GE Crédito
        objCell = New TableCell
        objCell.Wrap = False
        objCell.Text = "GE Crédito"
        objRow.Controls.Add(objCell)

        'Despacho Ciclo Aberto
        objCell = New TableCell
        objCell.Wrap = False
        objCell.Text = "Despacho Ciclo Aberto"
        objRow.Controls.Add(objCell)

        ' Despacho Ciclo Reduzido
        objCell = New TableCell
        objCell.Wrap = False
        objCell.Text = "Despacho Ciclo Reduzido"
        objRow.Controls.Add(objCell)

        'Insumo Reserva 1
        objCell = New TableCell
        objCell.Wrap = False
        objCell.Text = " Nível de Partida"
        objRow.Controls.Add(objCell)
        'Insumo Reserva 2
        objCell = New TableCell
        objCell.Wrap = False
        objCell.Text = "Dia -1"
        objRow.Controls.Add(objCell)
        ' Insumo Reserva 3
        objCell = New TableCell
        objCell.Wrap = False
        objCell.Text = " Dia -2"
        objRow.Controls.Add(objCell)
        ' Insumo Reserva 4
        objCell = New TableCell
        objCell.Wrap = False
        objCell.Text = "Carga da Ande"
        objRow.Controls.Add(objCell)
        'Programacao semmanal
        objCell = New TableCell
        objCell.Wrap = False
        objCell.Text = "Programação Semanal"
        objRow.Controls.Add(objCell)

        tblMensa.Rows.Add(objRow)
    End Sub

    Private Sub cboData_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboData.SelectedIndexChanged
        If cboData.SelectedIndex > 0 Then
            Session("datEscolhida") = CDate(cboData.SelectedItem.Value)
        End If
        cboEmpresa_SelectedIndexChanged(sender, e)
    End Sub

    Private Sub VerificaDigitacao()
        Dim Conn As System.Data.SqlClient.SqlConnection = New System.Data.SqlClient.SqlConnection
        Conn.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
        Dim Cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand
        Cmd.Connection = Conn
        Conn.Open()
        Try
            chkInflexibilidade.Checked = False
            chkRestricao.Checked = False
            chkManutencao.Checked = False
            chkVazao.Checked = False
            chkGeracao.Checked = False
            chkCarga.Checked = False
            chkIntercambio.Checked = False
            chkEnergetica.Checked = False
            chkEletrica.Checked = False
            chkEnvia1.Checked = False
            chkEnvia2.Checked = False
            chkExporta.Checked = False
            chkImporta.Checked = False
            chkMRE.Checked = False
            chkMIF.Checked = False
            chkPCC.Checked = False
            chkMCO.Checked = False
            chkMOS.Checked = False
            chkMEG.Checked = False
            chkERP.Checked = False
            chkDSP.Checked = False
            chkCLF.Checked = False
            chkPCO.Checked = False
            chkRFC.Checked = False
            chkRMP.Checked = False
            chkGFM.Checked = False
            chkCFM.Checked = False
            chkSOM.Checked = False
            chkGES.Checked = False
            chkGEC.Checked = False
            chkDCA.Checked = False
            chkDCR.Checked = False
            chkIR1.Checked = False
            chkIR2.Checked = False
            chkIR3.Checked = False
            chkIR4.Checked = False
            'RRO
            chkRRO.Checked = False

            'Verifica se exitem os eventos 
            Cmd.CommandText = "SELECT codstatu " &
                              "FROM eventpdp " &
                              "WHERE datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' " &
                              "AND cmpevent = '" & cboEmpresa.SelectedItem.Text & "' " &
                              "AND (codstatu = '2' " &
                              "OR codstatu = '6' " &
                              "OR codstatu = '46' " &
                              "OR codstatu = '51') " &
                              "GROUP BY codstatu"
            '"OR codstatu = '4' " &
            '"OR codstatu = '5' " &
            '"OR codstatu = '6' " &
            '"OR codstatu = '7' " &
            '"OR codstatu = '8' " &
            '"OR codstatu = '9' " &
            '"OR codstatu = '17' " &
            '"OR codstatu = '18' " &
            '"OR codstatu = '32' " &
            '"OR codstatu = '33' " &
            '"OR codstatu = '34' " &
            '"OR codstatu = '35' " &
            '"OR codstatu = '36' " &
            '"OR codstatu = '37' " &
            '"OR codstatu = '38' " &
            '"OR codstatu = '39' " &

            '"OR codstatu = '47' " &
            '"OR codstatu = '48' " &
            '"OR codstatu = '49' " &
            '"OR codstatu = '51' " &
            '"OR codstatu = '52' " &
            '"OR codstatu = '53' " &
            '"OR codstatu = '54' " &
            '"OR codstatu = '55' " &
            '"OR codstatu = '56' " &
            '"OR codstatu = '57' " &
            '"OR codstatu = '58' " &
            '"OR codstatu = '59' " &
            '"OR codstatu = '60' " &
            '"OR codstatu = '61' " &
            '"OR codstatu = '62' " &
            '"OR codstatu = '63' " &
            '"OR codstatu = '64') " &

            'Dim rsEvento As OnsClasses.OnsData.OnsDataReader = Cmd.ExecuteReader
            Dim rsEvento As System.Data.SqlClient.SqlDataReader = Cmd.ExecuteReader

            chkEnvia1.Enabled = True
            chkEnvia1.Checked = False
            chkEnvia2.Enabled = True
            chkEnvia2.Checked = False
            chkIfxE.Enabled = False
            chkIfxE.Checked = False
            chkVazE.Enabled = False
            chkVazE.Checked = False
            chkZenE.Enabled = False
            chkZenE.Checked = False
            chkZelE.Enabled = False
            chkZelE.Checked = False
            chkImpE.Enabled = False
            chkImpE.Checked = False
            chkExpE.Enabled = False
            chkExpE.Checked = False
            chkMreE.Enabled = False
            chkMreE.Checked = False
            chkMifE.Enabled = False
            chkMifE.Checked = False
            chkPccE.Enabled = False
            chkPccE.Checked = False
            chkMcoE.Enabled = False
            chkMcoE.Checked = False
            chkMosE.Enabled = False
            chkMosE.Checked = False
            chkMegE.Enabled = False
            chkMegE.Checked = False
            chkErpE.Enabled = False
            chkErpE.Checked = False
            chkDspE.Enabled = False
            chkDspE.Checked = False
            chkClfE.Enabled = False
            chkClfE.Checked = False
            chkPcoE.Enabled = False
            chkPcoE.Checked = False
            chkRfcE.Enabled = False
            chkRfcE.Checked = False
            chkRmpE.Enabled = False
            chkRmpE.Checked = False
            chkGfmE.Enabled = False
            chkGfmE.Checked = False
            chkCfmE.Enabled = False
            chkCfmE.Checked = False
            chkSomE.Enabled = False
            chkSomE.Checked = False
            chkEnviaTodos.Checked = False
            'RRO
            chkRROE.Checked = False

            Do While rsEvento.Read
                Select Case rsEvento("codstatu")
                    Case Is = 2
                        chkInflexibilidade.Checked = True
                        chkIfxE.Checked = True
                        chkIfxE.Enabled = True
                    Case Is = 4
                        chkRestricao.Checked = True
                    Case Is = 5
                        chkManutencao.Checked = True
                    Case Is = 6
                        chkVazao.Checked = True
                        chkVazE.Checked = True
                        chkVazE.Enabled = True
                    Case Is = 7
                        chkGeracao.Checked = True
                    Case Is = 8
                        chkCarga.Checked = True
                    Case Is = 9
                        chkIntercambio.Checked = True
                    Case Is = 17
                        chkEnergetica.Checked = True
                        chkZenE.Checked = True
                        chkZenE.Enabled = True
                    Case Is = 18
                        chkEletrica.Checked = True
                        chkZelE.Checked = True
                        chkZelE.Enabled = True
                    Case Is = 32
                        chkImporta.Checked = True
                        chkImpE.Checked = True
                        chkImpE.Enabled = True
                    Case Is = 33
                        chkExporta.Checked = True
                        chkExpE.Checked = True
                        chkExpE.Enabled = True
                    Case Is = 34
                        chkMRE.Checked = True
                        chkMreE.Checked = True
                        chkMreE.Enabled = True
                    Case Is = 35
                        chkPCC.Checked = True
                        chkPccE.Checked = True
                        chkPccE.Enabled = True
                    Case Is = 36
                        chkMCO.Checked = True
                        chkMcoE.Checked = True
                        chkMcoE.Enabled = True
                    Case Is = 37
                        chkMOS.Checked = True
                        chkMosE.Checked = True
                        chkMosE.Enabled = True
                    Case Is = 38
                        chkMEG.Checked = True
                        chkMegE.Checked = True
                        chkMegE.Enabled = True
                    Case Is = 39
                        chkERP.Checked = True
                        chkErpE.Checked = True
                        chkErpE.Enabled = True
                    Case Is = 46
                        chkDSP.Checked = True
                        chkDspE.Checked = True
                        chkDspE.Enabled = True
                    Case Is = 47
                        chkCLF.Checked = True
                        chkClfE.Checked = True
                        chkClfE.Enabled = True
                    Case Is = 48
                        chkMIF.Checked = True
                        chkMifE.Checked = True
                        chkMifE.Enabled = True
                    Case Is = 49
                        chkPCO.Checked = True
                        chkPcoE.Checked = True
                        chkPcoE.Enabled = True
                    Case Is = 51
                        chkRFC.Checked = True
                        chkRfcE.Checked = True
                        chkRfcE.Enabled = True
                    Case Is = 52
                        chkRMP.Checked = True
                        chkRmpE.Checked = True
                        chkRmpE.Enabled = True
                    Case Is = 53
                        chkGFM.Checked = True
                        chkGfmE.Checked = True
                        chkGfmE.Enabled = True
                    Case Is = 54
                        chkCFM.Checked = True
                        chkCfmE.Checked = True
                        chkCfmE.Enabled = True
                    Case Is = 55
                        chkSOM.Checked = True
                        chkSomE.Checked = True
                        chkSomE.Enabled = True
                    Case Is = 56
                        chkGES.Checked = True
                        chkGesE.Checked = True
                        chkGesE.Enabled = True
                    Case Is = 57
                        chkGEC.Checked = True
                        chkGecE.Checked = True
                        chkGecE.Enabled = True
                    Case Is = 58
                        chkDCA.Checked = True
                        chkDcaE.Checked = True
                        chkDcaE.Enabled = True
                    Case Is = 59
                        chkDCR.Checked = True
                        chkDcrE.Checked = True
                        chkDcrE.Enabled = True
                    Case Is = 60
                        chkIR1.Checked = True
                        chkIr1E.Checked = True
                        chkIr1E.Enabled = True
                    Case Is = 61
                        chkIR2.Checked = True
                        chkIr2E.Checked = True
                        chkIr2E.Enabled = True
                    Case Is = 62
                        chkIR3.Checked = True
                        chkIr3E.Checked = True
                        chkIr3E.Enabled = True
                    Case Is = 63
                        chkIR4.Checked = True
                        chkIr4E.Checked = True
                        chkIr4E.Enabled = True
                    Case Is = 64 'RRO
                        chkRRO.Checked = True
                        chkRROE.Checked = True
                        chkRROE.Enabled = True
                End Select
            Loop
            If chkCarga.Checked Or chkGeracao.Checked Or chkIntercambio.Checked Then
                chkEnvia1.Checked = True
            End If
            If chkCarga.Checked = False And chkGeracao.Checked = False And chkIntercambio.Checked = False Then
                chkEnvia1.Enabled = False
            End If
            If chkManutencao.Checked Or chkRestricao.Checked Then
                chkEnvia2.Checked = True
            End If
            If chkManutencao.Checked = False And chkRestricao.Checked = False Then
                chkEnvia2.Enabled = False
            End If
            rsEvento.Close()

        Catch ex As Exception
            If Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If
            '
            ' WI 158532 - Log para tentar ver qual o problema pois ele não aparece em teste
            '
            logger.Error("Erro no envio de dados: " + ex.Message, ex)

            Session("strMensagem") = "Não foi possivel enviar os dados (1), por favor tente novamente ou comunique a ocorrência ao ONS." +
                                               " (" + ex.Message + "  -  " + ex.StackTrace + ")."
            Response.Redirect("frmMensagem.aspx")
        Finally
            If Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If
        End Try
    End Sub

    Private Function RetornaUsinaFromBdtId(pConn As OnsClasses.OnsData.OnsConnection, pBdtId As String) As String
        Dim retorno As String = ""
        Dim tb As DataTable = New DataTable()
        tb = RetornaDadosSQL(pConn, "select codusina from usina where usi_bdt_id = '" & pBdtId & "'", "Usina")
        For Each item As DataRow In tb.Rows
            retorno = retorno & Trim(item("codusina")) + ", "
        Next
        Return retorno.Remove(retorno.Length - 2, 2)
    End Function


    ''' <summary>
    ''' Método que valida o intercâmbio da empresa.
    ''' O intercâmbio deve ser igual a (soma de toda geração da empresa menos sua carga declarada)
    ''' </summary>
    ''' <param name="Conn"></param>
    ''' <param name="Cmd"></param>
    ''' <param name="empresa"></param>
    ''' <param name="dataPDP"></param>
    ''' <param name="patamar"></param>
    ''' <returns>bool</returns>
    ''' <remarks>true para validado e false para não validado</remarks>
    Private Function ValidarIntercambio(Conn As System.Data.SqlClient.SqlConnection, Cmd As System.Data.SqlClient.SqlCommand, empresa As String, dataPDP As String, patamar As String) As Boolean

        Dim tbIntercambio As DataTable = New DataTable()
        Dim sql As String
        'Conn.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()

        sql = "select " &
"   (select ISNULL(sum(valintertran),0) from inter where datpdp = '" & dataPDP & "' and intinter = " & patamar & " and codemprede = empre.codempre) as totInter " &
", (select ISNULL(sum(valcargatran),0) from carga where datpdp = '" & dataPDP & "' and intcarga = " & patamar & " and codempre = empre.codempre) as totCarga" &
", ( " &
"     Select ISNULL(sum(valdespatran), 0) " &
"     from despa " &
"        inner join usina " &
"           on despa.codusina = usina.codusina " &
"     where " &
"     despa.datpdp = '" & dataPDP & "' " &
"     and despa.intdespa = " & patamar &
"     and usina.codempre = empre.codempre " &
"  ) as totGeracao " &
" from empre " &
" where codempre = '" & empresa & "' "

        tbIntercambio = RetornaDadosSQLServer(Conn, sql, "Intercambio")

        If (tbIntercambio.Rows.Count > 0) Then
            Dim totInter As Int64 = Int64.Parse(tbIntercambio.Rows(0).Item("totInter").ToString())
            Dim totCarga As Int64 = Int64.Parse(tbIntercambio.Rows(0).Item("totCarga").ToString())
            Dim totGeracao As Int64 = Int64.Parse(tbIntercambio.Rows(0).Item("totGeracao").ToString())

            If (totInter <> totGeracao - totCarga) Then
                AddMensagemErro("Intervalo " & PatamarToIntervalo(patamar) & ": " & "Intercâmbio líquido (" + totInter.ToString() + ") diferente do somatório da geração (" + totGeracao.ToString() + ") menos a carga (" + totCarga.ToString() + ").")
                Return False
            End If
        End If

        Return True

    End Function


    Private Function ValidaEnvioDados() As Boolean

        Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
        'Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
        Dim ConnSql As System.Data.SqlClient.SqlConnection = New System.Data.SqlClient.SqlConnection
        ConnSql.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
        Dim Cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand

        Dim daDados As OnsClasses.OnsData.OnsDataAdapter
        Dim dsDados As DataSet

        Dim retorno As Boolean = True

        Dim strCodEmpresa = cboEmpresa.SelectedValue  'Session("strCodEmpre")
        Dim strDataPDP = Format(CDate(cboData.SelectedValue), "yyyyMMdd")  'Format(Session("datEscolhida"), "yyyyMMdd")

        'Agente/Empresa
        Dim strSqlAgente = "Select codempre, sigempre, empre_bdt_id from empre where codempre = '" & strCodEmpresa & "'"

        Dim strSqlCountParcelas = "Select usi_bdt_id, count(usi_bdt_id) as count " &
        "From usina where usi_bdt_id <> 'xxxxxx' And usi_bdt_id Is Not null and trim(usi_bdt_id) <> '' and codempre = '" & strCodEmpresa & "' " &
        "and flg_recebepdpage <> 'N' " &
        "group by usi_bdt_id "

        '{0} - usi_bdt_id
        'RETIRADO DO SQL //'{1} - age_bdt_id "item_intervencao.age_id_prin = '{1}' and " & _
        '{1} - Data Hora Inicio (2015-09-26 00:00:00)
        '{2} - Data Hora Fim (2015-09-26 00:00:00)
        'Dim varRegistry As Microsoft.Win32.RegistryKey
        'varRegistry = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\ONS\IU\pdp", False)
        Dim DataSourceBDT As String = ConfigurationManager.AppSettings.Get("DataSourceBDT") 'varRegistry.GetValue("DataSourceBDT")

        Dim strSqlSGI = "Select Distinct " &
                        "intervencao.object_id as IdIntervencao, " &
                        "intervencao.tipo_periodo, " &
                        "periodo.data_hora_inicio, periodo.data_hora_fim, " &
                        "'A' as patamarIni, 'B' as patamarFim, " &
                        "item_intervencao.eqpsgi_id as uge_id, " &
                        "item_intervencao.tpeqpsgi_id, " &
                        "item_intervencao.ins_id, uge.usi_id, sum(gerad.capgerad) as capgerad " &
                        "From " &
                        DataSourceBDT & ":item_intervencao as item_intervencao, " & DataSourceBDT & ":intervencao as intervencao, " &
                        DataSourceBDT & ":periodo as periodo, " & DataSourceBDT & ":uge as uge, gerad " &
                        "Where " &
                        "uge.usi_id = '{0}' and " &
                        "gerad.uge_bdt_id = uge.eqp_id and " &
                        "item_intervencao.eqpsgi_id = gerad.uge_bdt_id and " &
                        "item_intervencao.famgen_id = 18 and " &
                        "intervencao.estado = 4 and " &
                        "intervencao.caracterizacao = 1 and " &
                        "periodo.data_hora_fim >= '{1}' and " &
                        "periodo.data_hora_inicio < '{2}' and " &
                        "periodo.situacao = 1 And " &
                        "item_intervencao.intervencao = intervencao.object_id And " &
                        "periodo.intervencao = intervencao.object_id And " &
                        "item_intervencao.eqpsgi_id = uge.eqp_id " &
                        "group by intervencao.object_id,  " &
                        "intervencao.tipo_periodo, " &
                        "periodo.data_hora_inicio, periodo.data_hora_fim, " &
                        "item_intervencao.eqpsgi_id, " &
                        "item_intervencao.tpeqpsgi_id, " &
                        "item_intervencao.ins_id, uge.usi_id "

        '"intervencao.estado in ( 4, 1, 12, 3, 5, 11, 8 ) and " & _

        Dim strSqlPeriodoIntervencaoSGI = "Select periodo.object_id, periodo.data_hora_inicio, periodo.data_hora_fim " &
                        "From " &
                        DataSourceBDT & ":periodo as periodo " &
                        "Where " &
                        "periodo.intervencao = {0} and " &
                        "periodo.situacao <> 2 and " &
                        "periodo.situacao <> 4 " &
                        "Order by periodo.data_hora_inicio"


        Dim strCodUsina As String = ""
        Dim strNomeUsina As String = ""
        Dim strUsiBdtId As String = ""
        Dim potinstalada As Integer = 0
        Dim valPotInstaladaConj As Integer = 0
        Dim strFlgRecebePdpAge As String = ""

        Dim valInflex As Integer = -1
        Dim valOrdemMerito As Integer = -1
        Dim valRazaoEletrica As Integer = -1
        Dim valMotivoRazaoEletrica As Integer = -1
        Dim valGarantiaEnergetica As Integer = -1
        Dim valGeracaoForaOrdemMerito As Integer = -1
        Dim valGeracaoSubstituta As Integer = -1
        Dim valCreditoSubstituicao As Integer = -1
        Dim valRestricaoFaltaCombstivel As Integer = -1
        Dim valEnergiaReposicao As Integer = -1
        Dim valRRO As Integer = -1

        Dim valDisponibilidade As Integer = -1
        Dim valExportacao As Integer = -1
        Dim countParcelas As Integer = 0

        strListErro = ""

        InicializarDataTableMensagemErro()

        Cmd.Connection = ConnSql
        Try

            ConnSql.Open()

            'Verifica se exitem dados na tela de bloqueio
            Dim tbBloqueioEnvio As DataTable = New DataTable()
            Dim sqlBloqueiEnvio = "select count(*) count from tb_bloqueioenvio"
            tbBloqueioEnvio = RetornaDadosSQLServer(ConnSql, sqlBloqueiEnvio, "tb_bloqueioenvio")
            Dim totcount As Int64 = Int64.Parse(tbBloqueioEnvio.Rows(0).Item("count").ToString())

            If (totcount > 0) Then
                retorno = True
                Return retorno

            End If


            'Recupera age_bdt_id
            Dim tbAgente As DataTable = New DataTable()
            tbAgente = RetornaDadosSQLServer(ConnSql, strSqlAgente, "Agente")
            Dim age_bdt_id As String = tbAgente.Rows(0).Item("empre_bdt_id").ToString()

            Dim tbCountParcelas As DataTable = New DataTable()
            tbCountParcelas = RetornaDadosSQLServer(ConnSql, strSqlCountParcelas, "Usina")

            Dim strSqlGenericoDados As String = ""
            Dim strSqlGenericoDadosConj As String = ""
            Dim strSqlUsinas As String = ""

            Dim validacaoIntercambio As Boolean = True

            For Each parcela As DataRow In tbCountParcelas.Rows
                Dim tbUsinas As DataTable = New DataTable()

                'Usinas
                strSqlUsinas = " Select codusina, nomusina, codempre, codinsta, potinstalada, usi_bdt_id, tpusina_id, flg_recebepdpage, ordem, indpeq, flg_abatercarga " &
                               " From usina Where codempre = '" & strCodEmpresa & "' " &
                               " and usi_bdt_id = '" & parcela("usi_bdt_id") & "' " &
                               " Order by ordem "

                strSqlGenericoDados = "Select u.potinstalada, u.tpusina_id, d.{0} As valor, d.{1} As patamar " &
                                            "From {2} d, usina u " &
                                            "Where u.codempre = '{3}' And " &
                                            "d.codusina = '{4}' And " &
                                            "u.codusina = d.codusina And " &
                                            "d.datpdp = '{5}' " &
                                            "and flg_recebepdpage <> 'N' " &
                                            "and indpeq <> 1 " &
                                            "Order By d.codusina, d.{1}, u.tpusina_id "

                strSqlGenericoDadosConj = "Select sum(u.potinstalada) as potinstalada, u.tpusina_id, sum(d.{0}) As valor, d.{1} As patamar " &
                                      "From {2} d, usina u " &
                                      "Where u.codempre = '{3}' And " &
                                      "u.usi_bdt_id = '{4}' And " &
                                      "u.codusina = d.codusina And " &
                                      "d.datpdp = '{5}' " &
                                      "and flg_recebepdpage <> 'N' " &
                                      "and indpeq <> 1 " &
                                      "Group By u.tpusina_id, d.{1} " &
                                      "Order By u.tpusina_id, d.{1} "

                tbUsinas = RetornaDadosSQLServer(ConnSql, strSqlUsinas, "Usina")

                Dim dataPDPCompleta As DateTime

                ' For de Usinas
                For Each linha As DataRow In tbUsinas.Rows
                    strCodUsina = linha("codusina")
                    strNomeUsina = linha("nomusina")

                    Dim strParcelas As String
                    strParcelas = strCodUsina

                    strFlgRecebePdpAge = ""
                    If (Not DBNull.Value.Equals("flg_recebepdpage")) Then
                        strFlgRecebePdpAge = linha("flg_recebepdpage")
                    Else
                        strFlgRecebePdpAge = "S"
                    End If

                    strUsiBdtId = ""
                    If (Not DBNull.Value.Equals(linha("usi_bdt_id"))) Then
                        strUsiBdtId = linha("usi_bdt_id")
                    End If

                    potinstalada = 0
                    If (Not DBNull.Value.Equals(linha("potinstalada"))) Then
                        potinstalada = Integer.Parse(linha("potinstalada").ToString())
                    End If

                    '------------------
                    'Potencia instalada do conjunto e a geração do conjunto
                    '------------------
                    Dim tbPotInstaladaConj As DataTable = New DataTable()
                    If parcela("count") > 1 Then
                        Dim strSqlDadosPotInstaladaConj = String.Format(strSqlGenericoDadosConj, "valdespatran", "intdespa", "despa", strCodEmpresa, strUsiBdtId, strDataPDP)
                        tbPotInstaladaConj = RetornaDadosSQLServer(ConnSql, strSqlDadosPotInstaladaConj, "PotInstaladaEGeracaoConjunto")

                        'Obtem o valor da potencia instalada do conjunto
                        If (tbPotInstaladaConj.Rows.Count > 0) Then
                            If Not DBNull.Value.Equals(tbPotInstaladaConj.Rows(0).Item("PotInstalada")) Then
                                valPotInstaladaConj = Integer.Parse(tbPotInstaladaConj.Rows(0).Item("PotInstalada").ToString())
                            End If
                        End If
                    End If


                    '--------------
                    'GERACAO
                    '--------------
                    Dim strSqlDadosGeracao = String.Format(strSqlGenericoDados, "valdespatran", "intdespa", "despa", strCodEmpresa, strCodUsina, strDataPDP)

                    Dim tbGeracao As DataTable = New DataTable()
                    If chkGeracao.Checked Then
                        tbGeracao = RetornaDadosSQLServer(ConnSql, strSqlDadosGeracao, "Geracao")
                    End If


                    '----------------
                    'INFLEXIBILIDADE
                    '----------------
                    Dim strSqlDadosInflexibilidade = String.Format(strSqlGenericoDados, "valflexitran", "intflexi", "inflexibilidade", strCodEmpresa, strCodUsina, strDataPDP)
                    Dim tbInflex As DataTable = New DataTable()
                    If chkIfxE.Checked Then
                        tbInflex = RetornaDadosSQLServer(ConnSql, strSqlDadosInflexibilidade, "Inflex")
                    End If


                    '----------------
                    'Ordem de Mérito    (A geração por ordem de mérito, é quando o mnemônico ZEN vem com valor 999, na tabela razaoener)
                    '----------------
                    'Se possuir parcelas, o valor do mérito nao deve ser agrupado.
                    Dim tbOrdemMerito As DataTable = New DataTable()

                    Dim strSqlDadosOrdemMerito = String.Format(strSqlGenericoDados, "valrazenertran", "intrazener", "razaoener", strCodEmpresa, strCodUsina, strDataPDP)
                    If chkZenE.Checked Then
                        tbOrdemMerito = RetornaDadosSQLServer(ConnSql, strSqlDadosOrdemMerito, "OrdemMerito")
                    End If

                    '" "
                    '----------------
                    'Razão Elétrica   
                    '----------------
                    Dim strSqlDadosRazaoEletrica = String.Format(strSqlGenericoDados, "valrazelettran", "intrazelet", "razaoelet", strCodEmpresa, strCodUsina, strDataPDP)
                    Dim tbRazaoEletrica As DataTable = New DataTable()
                    If chkZelE.Checked Then
                        tbRazaoEletrica = RetornaDadosSQLServer(ConnSql, strSqlDadosRazaoEletrica, "RazaoEletrica")
                    End If


                    '---------------------------------
                    'Motivo de Despacho Razão Elétrica   
                    '---------------------------------
                    Dim strSqlDadosMotivoRazaoEletrica = String.Format(strSqlGenericoDados, "valmretran", "intmre", "motivorel", strCodEmpresa, strCodUsina, strDataPDP)
                    Dim tbMotivoRazaoEletrica As DataTable = New DataTable()
                    If chkMreE.Checked Then
                        tbMotivoRazaoEletrica = RetornaDadosSQLServer(ConnSql, strSqlDadosMotivoRazaoEletrica, "MotivoRazaoEletrica")
                    End If


                    '------------------------
                    'GE-Garantia Energética
                    '------------------------
                    Dim strSqlDadosGarantiaEnergetica = String.Format(strSqlGenericoDados, "valrmptran", "intrmp", "tb_rmp", strCodEmpresa, strCodUsina, strDataPDP)
                    Dim tbGarantiaEnergetica As DataTable = New DataTable()
                    If chkRmpE.Checked Then
                        tbGarantiaEnergetica = RetornaDadosSQLServer(ConnSql, strSqlDadosGarantiaEnergetica, "GarantiaEnergetica")
                    End If


                    '---------------------------------------
                    'Geração Fora da Ordem de Mérito (GFOM)
                    '---------------------------------------
                    Dim strSqlDadosGeracaoForaOrdemMerito = String.Format(strSqlGenericoDados, "valgfmtran", "intgfm", "tb_gfm", strCodEmpresa, strCodUsina, strDataPDP)
                    Dim tbGeracaoForaOrdemMerito As DataTable = New DataTable()
                    If chkGfmE.Checked Then
                        tbGeracaoForaOrdemMerito = RetornaDadosSQLServer(ConnSql, strSqlDadosGeracaoForaOrdemMerito, "GeracaoForaOrdemMerito")
                    End If


                    '---------------------------------------
                    'Geração Substituta (GSUB)
                    '---------------------------------------
                    Dim strSqlDadosGeracaoSubstituta = String.Format(strSqlGenericoDados, "valsomtran", "intsom", "tb_som", strCodEmpresa, strCodUsina, strDataPDP)
                    Dim tbGeracaoSubstituta As DataTable = New DataTable()
                    If chkSomE.Checked Then
                        tbGeracaoSubstituta = RetornaDadosSQLServer(ConnSql, strSqlDadosGeracaoSubstituta, "GeracaoSubstituta")
                    End If

                    '---------------------------------------
                    'GE Substituição (GES)
                    '---------------------------------------
                    Dim strSqlDadosGES = String.Format(strSqlGenericoDados, "valGEStran", "intGES", "tb_GES", strCodEmpresa, strCodUsina, strDataPDP)
                    Dim tbGES As DataTable = New DataTable()
                    If chkGesE.Checked Then
                        tbGES = RetornaDadosSQLServer(ConnSql, strSqlDadosGES, "GES")
                    End If

                    '---------------------------------------
                    'GE Crédito (GEC)
                    '---------------------------------------
                    Dim strSqlDadosGEC = String.Format(strSqlGenericoDados, "valGECtran", "intGEC", "tb_GEC", strCodEmpresa, strCodUsina, strDataPDP)
                    Dim tbGEC As DataTable = New DataTable()
                    If chkGecE.Checked Then
                        tbGEC = RetornaDadosSQLServer(ConnSql, strSqlDadosGEC, "GEC")
                    End If

                    '---------------------------------------
                    'Despacho Ciclo Aberto (DCA)
                    '---------------------------------------
                    Dim strSqlDadosDCA = String.Format(strSqlGenericoDados, "valDCAtran", "intDCA", "tb_DCA", strCodEmpresa, strCodUsina, strDataPDP)
                    Dim tbDCA As DataTable = New DataTable()
                    If chkDcaE.Checked Then
                        tbDCA = RetornaDadosSQLServer(ConnSql, strSqlDadosDCA, "DCA")
                    End If

                    '---------------------------------------
                    'Despacho Ciclo Reduzido (DCR)
                    '---------------------------------------
                    Dim strSqlDadosDCR = String.Format(strSqlGenericoDados, "valDCRtran", "intDCR", "tb_DCR", strCodEmpresa, strCodUsina, strDataPDP)
                    Dim tbDCR As DataTable = New DataTable()
                    If chkDcrE.Checked Then
                        tbDCR = RetornaDadosSQLServer(ConnSql, strSqlDadosDCR, "DCR")
                    End If

                    '---------------------------------------
                    'Insumo Reserva 1 (IR1)
                    '---------------------------------------
                    Dim strSqlDadosIR1 = "Select u.potinstalada, u.tpusina_id, d.valIR1tran As valor " &
                                         "From tb_IR1 d, usina u " &
                                         "Where u.codempre = '" & strCodEmpresa & "' And " &
                                         "d.codusina = '" & strCodUsina & "' And " &
                                         "u.codusina = d.codusina And " &
                                         "d.datpdp = '" & strDataPDP & "' " &
                                         "and flg_recebepdpage <> 'N' " &
                                         "and indpeq <> 1 " &
                                         "Order By d.codusina, u.tpusina_id "
                    Dim tbIR1 As DataTable = New DataTable()
                    If chkIr1E.Checked Then
                        tbIR1 = RetornaDadosSQLServer(ConnSql, strSqlDadosIR1, "IR1")
                    End If

                    '---------------------------------------
                    'Insumo Reserva 2 (IR2)
                    '---------------------------------------
                    Dim strSqlDadosIR2 = String.Format(strSqlGenericoDados, "valIR2tran", "intIR2", "tb_IR2", strCodEmpresa, strCodUsina, strDataPDP)
                    Dim tbIR2 As DataTable = New DataTable()
                    If chkIr2E.Checked Then
                        tbIR2 = RetornaDadosSQLServer(ConnSql, strSqlDadosIR2, "IR2")
                    End If

                    '---------------------------------------
                    'Insumo Reserva 3 (IR3)
                    '---------------------------------------
                    Dim strSqlDadosIR3 = String.Format(strSqlGenericoDados, "valIR3tran", "intIR3", "tb_IR3", strCodEmpresa, strCodUsina, strDataPDP)
                    Dim tbIR3 As DataTable = New DataTable()
                    If chkIr3E.Checked Then
                        tbIR3 = RetornaDadosSQLServer(ConnSql, strSqlDadosIR3, "IR3")
                    End If

                    '---------------------------------------
                    'Insumo Reserva 4 (IR4)
                    '---------------------------------------
                    Dim strSqlDadosIR4 = String.Format(strSqlGenericoDados, "valIR4tran", "intIR4", "tb_IR4", strCodEmpresa, strCodUsina, strDataPDP)
                    Dim tbIR4 As DataTable = New DataTable()
                    If chkIr4E.Checked Then
                        tbIR4 = RetornaDadosSQLServer(ConnSql, strSqlDadosIR4, "IR4")
                    End If

                    '---------------------------------------
                    'Insumo Recomposição de Reserva Operativa (RRO)
                    '---------------------------------------
                    Dim strSqlDadosRRO = String.Format(strSqlGenericoDados, "valrrotran", "intrro", "tb_rro", strCodEmpresa, strCodUsina, strDataPDP)
                    Dim tbRRO As DataTable = New DataTable()
                    If chkRROE.Checked Then
                        tbRRO = RetornaDadosSQLServer(ConnSql, strSqlDadosRRO, "RRO")
                    End If

                    '----------------------------------------------------------------
                    'Crédito de Substituição (CSUB)    (CEFOM-Crédito por Substituição)
                    '----------------------------------------------------------------
                    Dim strSqlDadosCreditoSubstituicao = String.Format(strSqlGenericoDados, "valcfmtran", "intcfm", "tb_cfm", strCodEmpresa, strCodUsina, strDataPDP)
                    Dim tbCreditoSubstituicao As DataTable = New DataTable()
                    If chkCfmE.Checked Then
                        tbCreditoSubstituicao = RetornaDadosSQLServer(ConnSql, strSqlDadosCreditoSubstituicao, "CreditoSubstituicao")
                    End If


                    '-----------------------------------
                    'Restrição por Falta de combustível 
                    '-----------------------------------
                    Dim strSqlDadosRestricaoFaltaCombstivel = String.Format(strSqlGenericoDados, "valrfctran", "intrfc", "rest_falta_comb", strCodEmpresa, strCodUsina, strDataPDP)
                    Dim tbRestricaoFaltaCombstivel As DataTable = New DataTable()
                    If chkRfcE.Checked Then
                        tbRestricaoFaltaCombstivel = RetornaDadosSQLServer(ConnSql, strSqlDadosRestricaoFaltaCombstivel, "RestricaoFaltaCombstivel")
                    End If



                    '-----------------------------------
                    'Energia de Reposição 
                    '-----------------------------------
                    Dim strSqlDadosEnergiaReposicao = String.Format(strSqlGenericoDados, "valerptran", "interp", "energia_reposicao", strCodEmpresa, strCodUsina, strDataPDP)
                    Dim tbEnergiaReposicao As DataTable = New DataTable()
                    If chkErpE.Checked Then
                        tbEnergiaReposicao = RetornaDadosSQLServer(ConnSql, strSqlDadosEnergiaReposicao, "EnergiaReposicao")
                    End If

                    '------------------------------------------------------------------------------


                    '-----------------------------------
                    'Disponibilidade 
                    '-----------------------------------
                    Dim strSqlDadosDisponibilidade = String.Format(strSqlGenericoDados, "valdsptran", "intdsp", "disponibilidade", strCodEmpresa, strCodUsina, strDataPDP)
                    Dim tbDisponibilidade As DataTable = New DataTable()

                    If chkDspE.Checked Then
                        tbDisponibilidade = RetornaDadosSQLServer(ConnSql, strSqlDadosDisponibilidade, "Disponibilidade")
                    End If


                    Dim dataPDP As DateTime = Convert.ToDateTime(cboData.SelectedItem.Text)
                    Dim dataPDPAux As DateTime = dataPDP.AddDays(1)

                    'Dim tbDadosPatamarSGI As New DataTable()
                    Dim tbDadosSGI As DataTable = New DataTable()

                    '-----------------------------------
                    'Exportação 
                    '-----------------------------------
                    Dim strSqlDadosExportacao = String.Format(strSqlGenericoDados, "valexportatran", "intexporta", "exporta", strCodEmpresa, strCodUsina, strDataPDP)
                    Dim tbExportacao As DataTable = New DataTable()
                    If chkExpE.Checked Then
                        tbExportacao = RetornaDadosSQLServer(ConnSql, strSqlDadosExportacao, "Exportacao")
                    End If



                    Dim index As Integer = 0
                    Dim patamar As Integer = 0

                    For index = 0 To 47

                        'Formata a data do PDP de acordo com o Patamar
                        If (index = 0) Then
                            dataPDPCompleta = DateTime.Parse(dataPDP + " 00:00:00")
                        Else
                            dataPDPCompleta = dataPDPCompleta.AddMinutes(30)
                        End If


                        patamar = index + 1

                        If (strFlgRecebePdpAge = "S") Then


                            'Validação do Intercâmbio
                            If (validacaoIntercambio) Then
                                If Not ValidarIntercambio(ConnSql, Cmd, strCodEmpresa, strDataPDP, patamar) Then
                                    'retorno = False
                                End If

                                'Só deverá validar o intercâmbio uma vez por empresa
                                If patamar = 48 Then validacaoIntercambio = False
                            End If

                            If linha("indpeq") = 0 Then

                                'Geração do conjunto
                                Dim valGeracaoConj As Integer = -1
                                'PotInstalada e Geracao do Conjunto
                                If (tbPotInstaladaConj.Rows.Count > 0) Then
                                    If Not DBNull.Value.Equals(tbPotInstaladaConj.Rows(index).Item("valor")) Then
                                        valGeracaoConj = Integer.Parse(tbPotInstaladaConj.Rows(index).Item("valor").ToString())
                                    End If

                                End If



                                'GERAÇÃO
                                Dim valGeracao As Integer = -1
                                'If (tbGeracao.Rows.Count = 0) Then
                                '    AddMensagemErro("Geração não Informada para a Usina " & strCodUsina & ".")
                                '    retorno = False
                                'Else
                                If (tbGeracao.Rows.Count > 0) Then
                                    If DBNull.Value.Equals(tbGeracao.Rows(index).Item("valor")) Then
                                        'AddMensagemErro("Intervalo " & PatamarToIntervalo(patamar) & ": " & "Geração não Informada para a(s) Usina(s) " & strParcelas & ".")
                                        'retorno = False
                                    Else
                                        valGeracao = Integer.Parse(tbGeracao.Rows(index).Item("valor").ToString())
                                    End If
                                End If


                                'INFLEX
                                valInflex = -1
                                If (tbInflex.Rows.Count > 0) Then
                                    If (Not DBNull.Value.Equals(tbInflex.Rows(index).Item("valor"))) Then
                                        valInflex = Integer.Parse(tbInflex.Rows(index).Item("valor").ToString())
                                    End If
                                End If

                                'RRO
                                valRRO = -1
                                If (tbRRO.Rows.Count > 0) Then
                                    If (Not DBNull.Value.Equals(tbRRO.Rows(index).Item("valor"))) Then
                                        valRRO = Integer.Parse(tbRRO.Rows(index).Item("valor").ToString())
                                    End If
                                End If

                                'ORDEM MERITO
                                valOrdemMerito = -1
                                If (tbOrdemMerito.Rows.Count > 0) Then
                                    If (Not DBNull.Value.Equals(tbOrdemMerito.Rows(index).Item("valor"))) Then
                                        valOrdemMerito = Integer.Parse(tbOrdemMerito.Rows(index).Item("valor").ToString())
                                    End If
                                End If


                                'RAZAO ELETRICA
                                valRazaoEletrica = -1
                                If (tbRazaoEletrica.Rows.Count > 0) Then
                                    If (Not DBNull.Value.Equals(tbRazaoEletrica.Rows(index).Item("valor"))) Then
                                        valRazaoEletrica = Integer.Parse(tbRazaoEletrica.Rows(index).Item("valor").ToString())
                                    End If
                                End If


                                'MOTIVO RAZAO ELETRICA
                                valMotivoRazaoEletrica = -1
                                If (tbMotivoRazaoEletrica.Rows.Count > 0) Then
                                    If (Not DBNull.Value.Equals(tbMotivoRazaoEletrica.Rows(index).Item("valor"))) Then
                                        valMotivoRazaoEletrica = Integer.Parse(tbMotivoRazaoEletrica.Rows(index).Item("valor").ToString())
                                    End If
                                End If


                                'GARANTIA ENERGETICA
                                valGarantiaEnergetica = -1
                                If (tbGarantiaEnergetica.Rows.Count > 0) Then
                                    If (Not DBNull.Value.Equals(tbGarantiaEnergetica.Rows(index).Item("valor"))) Then
                                        valGarantiaEnergetica = Integer.Parse(tbGarantiaEnergetica.Rows(index).Item("valor").ToString())
                                    End If
                                End If


                                'Geração Fora da Ordem de Mérito (GFOM)
                                valGeracaoForaOrdemMerito = -1
                                If (tbGeracaoForaOrdemMerito.Rows.Count > 0) Then
                                    If (Not DBNull.Value.Equals(tbGeracaoForaOrdemMerito.Rows(index).Item("valor"))) Then
                                        valGeracaoForaOrdemMerito = Integer.Parse(tbGeracaoForaOrdemMerito.Rows(index).Item("valor").ToString())
                                    End If
                                End If


                                'Geração Substituta (GSUB)
                                valGeracaoSubstituta = -1
                                If (tbGeracaoSubstituta.Rows.Count > 0) Then
                                    If (Not DBNull.Value.Equals(tbGeracaoSubstituta.Rows(index).Item("valor"))) Then
                                        valGeracaoSubstituta = Integer.Parse(tbGeracaoSubstituta.Rows(index).Item("valor").ToString())
                                    End If
                                End If


                                'Crédito de Substituição (CSUB)    (CEFOM-Crédito por Substituição)
                                valCreditoSubstituicao = -1
                                If (tbCreditoSubstituicao.Rows.Count > 0) Then
                                    If (Not DBNull.Value.Equals(tbCreditoSubstituicao.Rows(index).Item("valor"))) Then
                                        valCreditoSubstituicao = Integer.Parse(tbCreditoSubstituicao.Rows(index).Item("valor").ToString())
                                    End If
                                End If


                                'Restrição por Falta de combustível 
                                valRestricaoFaltaCombstivel = -1
                                If (tbRestricaoFaltaCombstivel.Rows.Count > 0) Then
                                    If (Not DBNull.Value.Equals(tbRestricaoFaltaCombstivel.Rows(index).Item("valor"))) Then
                                        valRestricaoFaltaCombstivel = Integer.Parse(tbRestricaoFaltaCombstivel.Rows(index).Item("valor").ToString())
                                    End If
                                End If


                                'Energia de Reposição 
                                valEnergiaReposicao = -1
                                If (tbEnergiaReposicao.Rows.Count > 0) Then
                                    If (Not DBNull.Value.Equals(tbEnergiaReposicao.Rows(index).Item("valor"))) Then
                                        valEnergiaReposicao = Integer.Parse(tbEnergiaReposicao.Rows(index).Item("valor").ToString())
                                    End If
                                End If


                                'Disponibilidade
                                valDisponibilidade = 0
                                If (tbDisponibilidade.Rows.Count > 0) Then
                                    If (Not DBNull.Value.Equals(tbDisponibilidade.Rows(index).Item("valor"))) Then
                                        valDisponibilidade = Integer.Parse(tbDisponibilidade.Rows(index).Item("valor").ToString())
                                    End If
                                End If


                                'Exportação
                                valExportacao = -1
                                If (tbExportacao.Rows.Count > 0) Then
                                    If (Not DBNull.Value.Equals(tbExportacao.Rows(index).Item("valor"))) Then
                                        valExportacao = Integer.Parse(tbExportacao.Rows(index).Item("valor").ToString())
                                    End If
                                End If

                                'Críticas somente para térmicas
                                If (linha("tpusina_id") = "UTE") Then


                                    '--Regra 1
                                    'Qualquer geração térmica deve ter, obrigatoriamente, uma Titulação*- Inflexibilidade (I), Ordem de Mérito (OM), 
                                    'Razão Elétrica (RE), Motivo da Razão Elétrica (MRE), Garantia Energética (GE), Geração Fora da Ordem de Mérito (GFOM), 
                                    'Geração Substituta (GSUB), Crédito de Substituição (CSUB), Restrição por Falta de combustível (RFC), Energia de Reposição (ERP);
                                    If valGeracao > 0 Then
                                        If (valInflex <= 0 And valOrdemMerito <> 999 And valRazaoEletrica <= 0 And valMotivoRazaoEletrica <= 0 And
                                        valGarantiaEnergetica <= 0 And valGeracaoForaOrdemMerito <= 0 And valGeracaoSubstituta <= 0 And
                                        valCreditoSubstituicao <= 0 And valRestricaoFaltaCombstivel <= 0 And valEnergiaReposicao <= 0 And valRRO <= 0) Then

                                            'AddMensagemErro("Intervalo " & PatamarToIntervalo(patamar) & ": " & "Nenhuma titulação foi informada para a(s) Usina(s) " & strParcelas & ".")
                                            'retorno = False
                                        End If
                                    End If

                                    '-- Regra 2
                                    'Toda Geração Térmica deve ter sempre uma Disponibilidade (D), que por sua vez, nunca pode ser inferior a sua Geração(G);
                                    If (String.IsNullOrEmpty(valDisponibilidade)) Then
                                        AddMensagemErro("Intervalo " & PatamarToIntervalo(patamar) & ": " & "A Disponibilidade não foi informada para as Usina(s) " & strParcelas & ".")
                                        retorno = False
                                    Else
                                        If (valDisponibilidade < valGeracao) Then
                                            'AddMensagemErro("Intervalo " & PatamarToIntervalo(patamar) & ": " & "A Disponiblidade da(s) Usina(s) " & strParcelas & "(" & valDisponibilidade & ") não pode ser menor que a Geração(" & valGeracao & ").")
                                            'retorno = False
                                        End If

                                    End If

                                    '-- Regra 3
                                    'A Inflexibilidade nunca deve ser maior do que a Geração Inflexibilidade <= Geração;
                                    If (valInflex > valGeracao) Then
                                        'AddMensagemErro("Intervalo " & PatamarToIntervalo(patamar) & ": " & "A Inflexibilidade da(s) Usina(s) " & strParcelas & "(" & valInflex & ") não pode ser maior quer a Geração(" & valGeracao & ").")
                                        'retorno = False
                                    End If

                                    '--Regra 4
                                    ' Toda geração titulada com Razão Elétrica (RE) deve ter obrigatoriamente um Motivo de razão elétrica (MRE);
                                    If (valRazaoEletrica > 0 And valMotivoRazaoEletrica <= 0) Then
                                        'AddMensagemErro("Intervalo " & PatamarToIntervalo(patamar) & ": " & "A(s) Usina(s) " & strParcelas & " teve Geração titulada com Razão Elétrica(RE) e obrigatoriamente deve ter um Motivo de razão elétrica(MRE).")
                                        'retorno = False
                                    End If

                                End If

                                'Críticas para todas as usinas

                                '--Regra 5 
                                'a) 1º - Verificar se o valor informado de disponibilidade (DSP) está de acordo com a POTINSTALADA DA USINA menos os SGIs.
                                ' Se for uma usina de parcela, deverá obter a potinstalada da usina toda (mesmo usi_bdt_id)

                                Dim valIndisponiblidadeSGI As Integer = 0
                                Dim dicionario As System.Collections.Specialized.StringDictionary
                                dicionario = New System.Collections.Specialized.StringDictionary()


                                Dim dataInicioIntervencao As DateTime
                                Dim dataFimIntervencao As DateTime
                                Dim dataPatamar As DateTime
                                Dim dataInicioPatamar As DateTime
                                Dim dataFimPatamar As DateTime


                                If tbDadosSGI.Rows.Count > 0 Then

                                    For Each rowSGI As DataRow In tbDadosSGI.Rows
                                        dataInicioIntervencao = DateTime.Parse(rowSGI("data_hora_inicio").ToString())
                                        dataFimIntervencao = DateTime.Parse(rowSGI("data_hora_fim").ToString())
                                        dataPatamar = dataPDPCompleta.Date
                                        dataInicioPatamar = dataPDPCompleta
                                        dataFimPatamar = dataPDPCompleta.AddMinutes(30)


                                        'Intervenção começa e termina no mesmo dia
                                        'Diária
                                        If (dataInicioIntervencao.Date = dataFimIntervencao.Date) Then

                                            '1º horário
                                            If (dataInicioIntervencao >= dataInicioPatamar And dataInicioIntervencao < dataFimPatamar) Then
                                                If (Not dicionario.ContainsKey(rowSGI("uge_id").ToString())) Then
                                                    valIndisponiblidadeSGI = valIndisponiblidadeSGI + Int32.Parse(rowSGI("capgerad").ToString())
                                                    dicionario.Add(rowSGI("uge_id").ToString(), "contabilizada")
                                                End If
                                            End If

                                            'Demais horários
                                            If (dataInicioPatamar > dataInicioIntervencao And dataFimPatamar < dataFimIntervencao) Then
                                                If (Not dicionario.ContainsKey(rowSGI("uge_id").ToString())) Then
                                                    valIndisponiblidadeSGI = valIndisponiblidadeSGI + Int32.Parse(rowSGI("capgerad").ToString())
                                                    dicionario.Add(rowSGI("uge_id").ToString(), "contabilizada")
                                                End If
                                            End If

                                            'último horário
                                            If (dataFimIntervencao > dataInicioPatamar And dataFimIntervencao <= dataFimPatamar) Then
                                                If (Not dicionario.ContainsKey(rowSGI("uge_id").ToString())) Then
                                                    valIndisponiblidadeSGI = valIndisponiblidadeSGI + Int32.Parse(rowSGI("capgerad").ToString())
                                                    dicionario.Add(rowSGI("uge_id").ToString(), "contabilizada")
                                                End If
                                            End If

                                        Else 'Contínua

                                            'dia intermediário
                                            If (dataInicioIntervencao.Date < dataPatamar.Date And dataFimIntervencao.Date > dataPatamar.Date) Then
                                                If (Not dicionario.ContainsKey(rowSGI("uge_id").ToString())) Then
                                                    valIndisponiblidadeSGI = valIndisponiblidadeSGI + Int32.Parse(rowSGI("capgerad").ToString())
                                                    dicionario.Add(rowSGI("uge_id").ToString(), "contabilizada")
                                                End If
                                            End If

                                            '1º dia
                                            If (dataInicioIntervencao.Date = dataPatamar.Date) Then
                                                If (dataInicioPatamar >= dataInicioIntervencao) Then
                                                    If (Not dicionario.ContainsKey(rowSGI("uge_id").ToString())) Then
                                                        valIndisponiblidadeSGI = valIndisponiblidadeSGI + Int32.Parse(rowSGI("capgerad").ToString())
                                                        dicionario.Add(rowSGI("uge_id").ToString(), "contabilizada")
                                                    End If
                                                End If
                                            End If

                                            'último dia
                                            If (dataFimIntervencao.Date = dataPatamar.Date) Then
                                                If (dataFimPatamar <= dataFimIntervencao) Then
                                                    If (Not dicionario.ContainsKey(rowSGI("uge_id").ToString())) Then
                                                        valIndisponiblidadeSGI = valIndisponiblidadeSGI + Int32.Parse(rowSGI("capgerad").ToString())
                                                        dicionario.Add(rowSGI("uge_id").ToString(), "contabilizada")
                                                    End If
                                                End If
                                            End If

                                        End If

                                    Next
                                End If




                                Dim potenciaInstaladaVerificacao As Integer = 0
                                Dim geracaoVerificacao As Integer = 0

                                'Se for usina de parcela
                                If parcela("count") > 1 Then
                                    potenciaInstaladaVerificacao = valPotInstaladaConj
                                    geracaoVerificacao = valGeracaoConj
                                Else
                                    potenciaInstaladaVerificacao = potinstalada
                                    geracaoVerificacao = valGeracao
                                End If



                                If (valDisponibilidade > (potenciaInstaladaVerificacao - valIndisponiblidadeSGI)) Then
                                    AddMensagemErro("Intervalo " & PatamarToIntervalo(patamar) & ": " & "A Disponibilidade da(s) Usina(s) " & strParcelas & "(" & valDisponibilidade & ")" & " deve ser menor ou igual a Potencia Instalada (" & potenciaInstaladaVerificacao & ") menos as intervenções do SGI(" & valIndisponiblidadeSGI & ").")
                                    retorno = False
                                End If


                                'b) 2º - Verificar se a disponibilidade menos os SGIs é maior ou igual a geração.
                                'Para descobrir quando um SGI impacta uma disponibilidade:
                                'O SGI é por unidade geradora, tabela gerad no PDP. Nessa tabela temos o uge_bdt_id para ligarmos com a BDT.
                                'Na tabela gerad temos o campo capgerad, que deve ser deduzido da 	usina.potinstalada caso a máquina esteja em manutenção.
                                ' Se for uma usina de parcela, deverá obter a potinstalada da usina toda (mesmo usi_bdt_id)

                                If (geracaoVerificacao > (potenciaInstaladaVerificacao - valIndisponiblidadeSGI)) Then
                                    'AddMensagemErro("Intervalo " & PatamarToIntervalo(patamar) & ": " & "A Geração (" & geracaoVerificacao & ") não pode ser maior que a Capacidade instalada da(s) Usina(s) " & strParcelas & " (" & potenciaInstaladaVerificacao & ") menos as intervenções do SGI (" & valIndisponiblidadeSGI & ").")
                                    'retorno = False
                                End If



                                'Críticas somente para térmicas
                                If (linha("tpusina_id") = "UTE") Then
                                    '--Regra 6
                                    'A geração térmica por ordem de mérito somente pode ter a titulação; 
                                    'Ordem de mérito, Inflexibilidade; Falta de Combustível; Razão Elétrica; 
                                    Dim strMsgRegra6 As String = ""
                                    If (valOrdemMerito = 999) Then

                                        'Garantia Energética (GE), Geração Fora da Ordem de Mérito (GFOM), 
                                        'Geração Substituta (GSUB), Crédito de Substituição (CSUB), Energia de Reposição (ERP);

                                        If (valGarantiaEnergetica > 0) Then
                                            'AddMensagemErro("Intervalo " & PatamarToIntervalo(patamar) & ": " & "Não é permitido informar Garantia Energética(" & valGarantiaEnergetica & ") para a(s) Usina(s) " & strParcelas & ", pois a mesma foi informada com Geração Térmica por Ordem de Mérito e somente pode ter titulação " &
                                            '            "Inflexibilidade, Falta de Combustível e Razão Elétrica.")
                                            'retorno = False
                                        End If

                                        If (valGeracaoForaOrdemMerito > 0) Then
                                            'AddMensagemErro("Intervalo " & PatamarToIntervalo(patamar) & ": " & "Não é permitido informar Geração Fora da Ordem de Mérito(" & valGeracaoForaOrdemMerito & ") para a(s) Usina(s) " & strParcelas & ", pois a mesma foi informada com Geração Térmica por Ordem de Mérito e somente pode ter titulação " &
                                            '            "Inflexibilidade, Falta de Combustível e Razão Elétrica.")
                                            'retorno = False
                                        End If

                                        If (valGeracaoSubstituta > 0) Then
                                            'AddMensagemErro("Intervalo " & PatamarToIntervalo(patamar) & ": " & "Não é permitido informar Geração Substituta(" & valGeracaoSubstituta & ") para a(s) Usina(s) " & strParcelas & ", pois a mesma foi informada com Geração Térmica por Ordem de Mérito e somente pode ter titulação " &
                                            '           "Inflexibilidade, Falta de Combustível e Razão Elétrica.")
                                            'retorno = False
                                        End If

                                        '10/08/2018 - O crédito de substituição pode ser informado junto quando estiver por ordem de mérito, desde que respeite a disponibilidade e tenha falta de combustível
                                        If (valCreditoSubstituicao > 0) Then
                                            'AddMensagemErro("Intervalo " & PatamarToIntervalo(patamar) & ": " & "Não é permitido informar Crédito de Substituição(" & valCreditoSubstituicao & ") para a(s) Usina(s) " & strParcelas & "(Intervalo: " & PatamarToIntervalo(patamar) & ")" & ", pois a mesma foi informada com Geração Térmica por Ordem de Mérito e somente pode ter titulação " &
                                            '            "Inflexibilidade, Falta de Combustível e Razão Elétrica.")

                                            '--Regra 7 (Se geração Ordem de Mérito)
                                            'a) Toda vez que tiver Crédito de Substituição (CSUB) deverá ter, obrigatoriamente, 
                                            'declaração por Falta de Combustível (FC) igual ou superior ao valor desse crédito.

                                            If (valRestricaoFaltaCombstivel < valCreditoSubstituicao) Then
                                                'AddMensagemErro("Intervalo " & PatamarToIntervalo(patamar) & ": " & "Foi informado para a(s) usina(s) " & strParcelas & " Crédito de Substituição(" & valCreditoSubstituicao & ") e nesse caso é obrigatório informar Falta de Combustível(" & valRestricaoFaltaCombstivel & ") com valor igual ou superior ao valor do Crédito(" & valCreditoSubstituicao & ").")
                                                'retorno = False
                                            End If

                                            '(Se geração Ordem de Mérito)
                                            'b) Toda vez que tiver Crédito de Substituição (CSUB) deverá ter disponibilidade igual ou superior
                                            If (valDisponibilidade < valCreditoSubstituicao) Then
                                                'AddMensagemErro("Intervalo " & PatamarToIntervalo(patamar) & ": " & "Foi informado para a(s) usina(s) " & strParcelas & " Crédito de Substituição(" & valCreditoSubstituicao & ") com valor maior que a Disponibilidade (" & valDisponibilidade & ").")
                                                'retorno = False
                                            End If

                                        End If

                                        If (valEnergiaReposicao > 0) Then
                                            'AddMensagemErro("Intervalo " & PatamarToIntervalo(patamar) & ": " & "Não é permitido informar Energia de Reposição(" & valEnergiaReposicao & ") para a(s) Usina(s) " & strParcelas & ", pois a mesma foi informada com Geração Térmica por Ordem de Mérito e somente pode ter titulação " &
                                            '            "Inflexibilidade, Falta de Combustível e Razão Elétrica.")
                                            'retorno = False
                                        End If



                                        'b) 'Disponibilidade = Geração + Razão Elétrica + Falta de Combustível 
                                        Dim valDisponibilidadeAux As Integer = IIf(valDisponibilidade = -1, 0, valDisponibilidade)
                                        Dim valGeracaoAux As Integer = IIf(valGeracao = -1, 0, valGeracao)
                                        Dim valRazaoEletricaAux As Integer = IIf(valRazaoEletrica = -1, 0, valRazaoEletrica)
                                        Dim valRestricaoFaltaCombstivelAux As Integer = IIf(valRestricaoFaltaCombstivel = -1, 0, valRestricaoFaltaCombstivel)

                                        If (valDisponibilidadeAux <> (valGeracaoAux + valRazaoEletricaAux + valRestricaoFaltaCombstivelAux)) Then
                                            'AddMensagemErro("Intervalo " & PatamarToIntervalo(patamar) & ": " & "Disponibilidade da(s) Usina(s) " & strParcelas & "(" & valDisponibilidadeAux & ") deve ser o igual somatório de Geração(" & valGeracaoAux & ") + Razão Elétrica(" & valRazaoEletricaAux & ") + Falta de Combustível(" & valRestricaoFaltaCombstivelAux & ").")
                                            'retorno = False
                                        End If

                                    End If


                                    '--Regra 8
                                    'Uma térmica fora da ordem de mérito não poderá ter titulação de ordem de mérito (OM);
                                    If (valGeracaoForaOrdemMerito > 0 And valOrdemMerito = 999) Then
                                        'AddMensagemErro("Intervalo " & PatamarToIntervalo(patamar) & ": " & "Foi informada para a(s) Usina(s) " & strParcelas & " Térmica Fora da Ordem de Mérito(" & valGeracaoForaOrdemMerito & "), a mesma não pode ter titulação de Ordem de Mérito(" & valOrdemMerito & ").")
                                        'retorno = False
                                    End If


                                    '--Regra 9
                                    'Qualquer geração térmica fora da ordem de mérito deve ter, obrigatoriamente, uma Titulação* 
                                    'Inflexibilidade (I), Razão Elétrica (RE), Motivo da Razão Elétrica (MRE), Garantia Energética (GE), 
                                    'Geração Fora da Ordem de Mérito (GFOM), Geração Substituta (GSUB), Crédito de Substituição (CSUB), 
                                    'Restrição por Falta de combustível (RFC), Energia de Reposição (ERP);
                                    '>>>>> Idem regra 1 <<<<<<


                                    '--Regra 10 (Se não Ordem de Merito)
                                    'Geração = Inflexibilidade + Razão Elétrica + Garantia Energética + Geração Fora da Ordem de Mérito + 
                                    'Geração Substituta +Energia de Reposição + Energia de exportação
                                    If (valOrdemMerito <> 999) Then

                                        Dim valGeracaoAux As Integer = IIf(valGeracao = -1, 0, valGeracao)
                                        Dim valInflexAux As Integer = IIf(valInflex = -1, 0, valInflex)
                                        Dim valRazaoEletricaAux As Integer = IIf(valRazaoEletrica = -1, 0, valRazaoEletrica)
                                        Dim valGarantiaEnergeticaAux As Integer = IIf(valGarantiaEnergetica = -1, 0, valGarantiaEnergetica)
                                        Dim valGeracaoForaOrdemMeritoAux As Integer = IIf(valGeracaoForaOrdemMerito = -1, 0, valGeracaoForaOrdemMerito)
                                        Dim valGeracaoSubstitutaAux As Integer = IIf(valGeracaoSubstituta = -1, 0, valGeracaoSubstituta)
                                        Dim valEnergiaReposicaoAux As Integer = IIf(valEnergiaReposicao = -1, 0, valEnergiaReposicao)
                                        Dim valExportacaoAux As Integer = IIf(valExportacao = -1, 0, valExportacao)
                                        Dim valRROAux As Integer = IIf(valRRO = -1, 0, valRRO)

                                        If (valGeracaoAux <> (valInflexAux + valRazaoEletricaAux + valGarantiaEnergeticaAux + valGeracaoForaOrdemMeritoAux + valGeracaoSubstitutaAux + valEnergiaReposicaoAux + valExportacaoAux + valRROAux)) Then
                                            'AddMensagemErro("Intervalo " & PatamarToIntervalo(patamar) & ": " & "A(s) Usina(s) " & strParcelas & " deve ter Geração(" & valGeracaoAux & ") igual somatório de Inflexibilidade(" & valInflexAux & ") + Razão Elétrica(" & valRazaoEletricaAux & ") + Garantia Energética(" & valGarantiaEnergeticaAux & ") + Geração Fora da Ordem de Mérito(" & valGeracaoForaOrdemMeritoAux & ") + " &
                                            '"Geração Substituta(" & valGeracaoSubstitutaAux & ") + Energia de Reposição(" & valEnergiaReposicaoAux & ") + Energia de exportação(" & valExportacaoAux & ") + RRO(" & valRROAux & ").")
                                            'retorno = False
                                        End If

                                    End If

                                    'O somatório da geração informada para a Usina deverá ser menor ou igual à sua capacidade instalada. 
                                    If (valGeracao > potinstalada) Then
                                        'AddMensagemErro("Intervalo " & PatamarToIntervalo(patamar) & ": " & "A Geração da(s) Usina(s) " & strParcelas & "(" & valGeracao & ")" & " deve ser menor ou igual a Potência Instalada(" & potinstalada & ").")
                                        'retorno = False
                                    End If

                                End If
                            End If
                        End If
                    Next ' For 0 to 47

                Next ' For lista de Usinas
            Next ' For lista por usi_bdt_id

            Session("tbMensagem") = tbMensagem

        Catch ex As Exception

            retorno = False

            If Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If

            Session("strMensagem") = "Erro ao consultar. (" + ex.Message + ") " + ex.StackTrace
            Response.Redirect("frmMensagem.aspx")

        Finally
            If Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If
        End Try

        ValidaEnvioDados = retorno

    End Function

    Private Function RetornaDadosSQL(ByVal pConn As OnsClasses.OnsData.OnsConnection, ByVal pSql As String, ByVal pNomeDado As String) As DataTable

        Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
        Dim daDados As OnsClasses.OnsData.OnsDataAdapter
        Dim dsDados As New DataSet

        Cmd.CommandText = pSql

        daDados = New OnsClasses.OnsData.OnsDataAdapter(Cmd.CommandText, pConn)

        daDados.Fill(dsDados, pNomeDado)

        RetornaDadosSQL = dsDados.Tables(pNomeDado)

    End Function

    Private Function RetornaDadosSQLServer(ByVal pConn As System.Data.SqlClient.SqlConnection, ByVal pSql As String, ByVal pNomeDado As String) As DataTable

        'pConn.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
        Dim Cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand
        'Dim daDados As OnsClasses.OnsData.OnsDataAdapter
        Dim daDados As SqlDataAdapter
        Dim dsDados As New DataSet

        Cmd.CommandText = pSql

        daDados = New SqlDataAdapter(Cmd.CommandText, pConn)

        daDados.Fill(dsDados, pNomeDado)

        RetornaDadosSQLServer = dsDados.Tables(pNomeDado)

    End Function

    Private Sub EnviarDados(ByVal strDataHora As String)

        Dim envioDeDados As New EnvioDeDados

        envioDeDados.ChkVazE = chkVazE.Checked
        envioDeDados.ChkEnvia1 = chkEnvia1.Checked
        envioDeDados.ChkIfxE = chkIfxE.Checked
        envioDeDados.ChkZenE = chkZenE.Checked
        envioDeDados.ChkZelE = chkZelE.Checked
        envioDeDados.ChkEnvia2 = chkEnvia2.Checked
        envioDeDados.ChkPcoE = chkPcoE.Checked
        envioDeDados.ChkExpE = chkExpE.Checked
        envioDeDados.ChkImpE = chkImpE.Checked
        envioDeDados.ChkMreE = chkMreE.Checked
        envioDeDados.ChkMifE = chkMifE.Checked
        envioDeDados.ChkPccE = chkPccE.Checked
        envioDeDados.ChkMcoE = chkMcoE.Checked
        envioDeDados.ChkMosE = chkMosE.Checked
        envioDeDados.ChkMegE = chkMegE.Checked
        envioDeDados.ChkErpE = chkErpE.Checked
        envioDeDados.ChkDspE = chkDspE.Checked
        envioDeDados.ChkClfE = chkClfE.Checked
        envioDeDados.ChkRfcE = chkRfcE.Checked
        envioDeDados.ChkRmpE = chkRmpE.Checked
        envioDeDados.ChkGfmE = chkGfmE.Checked
        envioDeDados.ChkCfmE = chkCfmE.Checked
        envioDeDados.ChkSomE = chkSomE.Checked
        envioDeDados.ChkGesE = chkGesE.Checked
        envioDeDados.ChkGecE = chkGecE.Checked
        envioDeDados.ChkDcaE = chkDcaE.Checked
        envioDeDados.ChkDcrE = chkDcrE.Checked
        envioDeDados.ChkIr1E = chkIr1E.Checked
        envioDeDados.ChkIr2E = chkIr2E.Checked
        envioDeDados.ChkIr3E = chkIr3E.Checked
        envioDeDados.ChkIr4E = chkIr4E.Checked
        envioDeDados.ChkRROE = chkRROE.Checked
        envioDeDados.ChkGeracao = chkGeracao.Checked
        envioDeDados.ChkCarga = chkCarga.Checked
        envioDeDados.ChkIntercambio = chkIntercambio.Checked
        envioDeDados.ChkRestricao = chkRestricao.Checked
        envioDeDados.ChkManutencao = chkManutencao.Checked

        envioDeDados.ChkCVU = chkDSP.Checked

        envioDeDados.CboDataValue = cboData.SelectedItem.Value
        envioDeDados.CboEmpresaValue = cboEmpresa.SelectedItem.Value
        envioDeDados.CboEmpresaText = cboEmpresa.SelectedItem.Text

        Dim util As New Util

        If (util.EnviarDadosOtimizado(strDataHora, envioDeDados)) Then
            PreencheCabecalho()
            PreencheTable()
        End If

    End Sub

    Private Sub RegistrarLog(ByVal mensagem As String)
        'log4net.Config.XmlConfigurator.Configure()
        logger.Debug(mensagem)
    End Sub
    Private Sub RegistrarLogErro(ByVal ex As Exception)
        'log4net.Config.XmlConfigurator.Configure()
        logger.Error(ex.Message, ex)
    End Sub
    Private Sub PreencheTable()
        Dim intRow, intCol, intArq As Integer
        Dim objRow As TableRow
        Dim objCell As TableCell

        Dim Conn As System.Data.SqlClient.SqlConnection = New System.Data.SqlClient.SqlConnection
        Conn.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
        Dim Cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand
        Cmd.Connection = Conn

        Cmd.CommandText = "SELECT nomarq, dthmensa, sitmensa, totdespa, totcarga, totinter, " &
                          "totvazao, qtdrestr, qtdparal, totinflexi, totrazener, totrazelet, " &
                          "totexporta, totimporta, totmre, totmif, totpcc, totmco, totmos, " &
                          "totmeg, toterp, totdsp, totclf, qtdparalco, totcota, totrfc, totrmp, " &
                          "totgfm, totcfm, totsom, totges, totgec, totdca, totdcr, totir1, totir2, totir3, totir4, totrro, totcvu " &
                          "FROM mensa " &
                          "WHERE codempre = '" & Session("strCodEmpre") & "' " &
                          "AND datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' " &
                          "ORDER BY dthmensa DESC"
        Conn.Open()
        Try
            'Dim rsMensa As OnsClasses.OnsData.OnsDataReader = Cmd.ExecuteReader
            Dim rsMensa As System.Data.SqlClient.SqlDataReader = Cmd.ExecuteReader

            'Colocar cor na tabela (RGB(233, 244, 207) = cor padrão ONS)
            Dim Color As System.Drawing.Color
            Color = New System.Drawing.Color
            Color = System.Drawing.ColorTranslator.FromWin32(RGB(233, 244, 207))
            intRow = 1

            Do While rsMensa.Read
                'Nova linha da tabela
                objRow = New TableRow
                If intRow Mod 2 = 0 Then
                    'quando linha = par troca cor
                    objRow.BackColor = Color
                End If

                'Descrição
                objCell = New TableCell
                objCell.Wrap = False
                objCell.Text = rsMensa("nomarq")
                objRow.Controls.Add(objCell)

                'Situação
                objCell = New TableCell
                objCell.Wrap = False
                objCell.Text = rsMensa("sitmensa")
                objRow.Controls.Add(objCell)

                'Geração
                objCell = New TableCell
                objCell.Wrap = False
                objCell.Text = IIf(Not IsDBNull(rsMensa("totdespa")), rsMensa("totdespa"), 0)
                objRow.Controls.Add(objCell)

                'RRO
                objCell = New TableCell
                objCell.Wrap = False
                objCell.Text = IIf(Not IsDBNull(rsMensa("totrro")), rsMensa("totrro"), 0)
                objRow.Controls.Add(objCell)

                'Carga
                objCell = New TableCell
                objCell.Wrap = False
                objCell.Text = IIf(Not IsDBNull(rsMensa("totcarga")), rsMensa("totcarga"), 0)
                objRow.Controls.Add(objCell)

                'Intercâmbio
                objCell = New TableCell
                objCell.Wrap = False
                objCell.Text = IIf(Not IsDBNull(rsMensa("totinter")), rsMensa("totinter"), 0)
                objRow.Controls.Add(objCell)

                'Vazão
                objCell = New TableCell
                objCell.Wrap = False
                objCell.Text = IIf(Not IsDBNull(rsMensa("totvazao")), rsMensa("totvazao"), 0)
                objRow.Controls.Add(objCell)

                'Restrição
                objCell = New TableCell
                objCell.Wrap = False
                objCell.Text = IIf(Not IsDBNull(rsMensa("qtdrestr")), rsMensa("qtdrestr"), 0)
                objRow.Controls.Add(objCell)

                'Manutenção
                objCell = New TableCell
                objCell.Wrap = False
                objCell.Text = IIf(Not IsDBNull(rsMensa("qtdparal")), rsMensa("qtdparal"), 0)
                objRow.Controls.Add(objCell)

                'Inflexibilidade
                objCell = New TableCell
                objCell.Wrap = False
                objCell.Text = IIf(Not IsDBNull(rsMensa("totinflexi")), rsMensa("totinflexi"), 0)
                objRow.Controls.Add(objCell)

                'Razão Energética
                objCell = New TableCell
                objCell.Wrap = False
                objCell.Text = IIf(Not IsDBNull(rsMensa("totrazener")), rsMensa("totrazener"), 0)
                objRow.Controls.Add(objCell)

                'Razão Elétrica
                objCell = New TableCell
                objCell.Wrap = False
                objCell.Text = IIf(Not IsDBNull(rsMensa("totrazelet")), rsMensa("totrazelet"), 0)
                objRow.Controls.Add(objCell)

                'Exportação
                objCell = New TableCell
                objCell.Wrap = False
                objCell.Text = IIf(Not IsDBNull(rsMensa("totexporta")), rsMensa("totexporta"), 0)
                objRow.Controls.Add(objCell)

                'Importação
                objCell = New TableCell
                objCell.Wrap = False
                objCell.Text = IIf(Not IsDBNull(rsMensa("totimporta")), rsMensa("totimporta"), 0)
                objRow.Controls.Add(objCell)

                'Motivo Despacho Razão Elétrica
                objCell = New TableCell
                objCell.Wrap = False
                objCell.Text = IIf(Not IsDBNull(rsMensa("totmre")), rsMensa("totmre"), 0)
                objRow.Controls.Add(objCell)

                'Motivo Despacho por Inflexibilidade
                objCell = New TableCell
                objCell.Wrap = False
                objCell.Text = IIf(Not IsDBNull(rsMensa("totmif")), rsMensa("totmif"), 0)
                objRow.Controls.Add(objCell)

                'Perdas Consumo Interno e Compensação
                objCell = New TableCell
                objCell.Wrap = False
                objCell.Text = IIf(Not IsDBNull(rsMensa("totpcc")), rsMensa("totpcc"), 0)
                objRow.Controls.Add(objCell)

                'Número Máquinas Paradas por Conveniência Operativa
                objCell = New TableCell
                objCell.Wrap = False
                objCell.Text = IIf(Not IsDBNull(rsMensa("totmco")), rsMensa("totmco"), 0)
                objRow.Controls.Add(objCell)

                'Número Máquinas Operando como Síncrono
                objCell = New TableCell
                objCell.Wrap = False
                objCell.Text = IIf(Not IsDBNull(rsMensa("totmos")), rsMensa("totmos"), 0)
                objRow.Controls.Add(objCell)

                'Número Máquinas Gerando
                objCell = New TableCell
                objCell.Wrap = False
                objCell.Text = IIf(Not IsDBNull(rsMensa("totmeg")), rsMensa("totmeg"), 0)
                objRow.Controls.Add(objCell)

                'Energia de Reposição e Perdas
                objCell = New TableCell
                objCell.Wrap = False
                objCell.Text = IIf(Not IsDBNull(rsMensa("toterp")), rsMensa("toterp"), 0)
                objRow.Controls.Add(objCell)

                'Disponibilidade
                objCell = New TableCell
                objCell.Wrap = False
                objCell.Text = IIf(Not IsDBNull(rsMensa("totdsp")), rsMensa("totdsp"), 0)
                objRow.Controls.Add(objCell)

                'Compensação de Lastro Físico
                objCell = New TableCell
                objCell.Wrap = False
                objCell.Text = IIf(Not IsDBNull(rsMensa("totclf")), rsMensa("totclf"), 0)
                objRow.Controls.Add(objCell)

                'Parada de Máquinas por Conveniência Operativa
                objCell = New TableCell
                objCell.Wrap = False
                objCell.Text = IIf(Not IsDBNull(rsMensa("qtdparalco")), rsMensa("qtdparalco"), 0)
                objRow.Controls.Add(objCell)

                'Cota Inicial
                objCell = New TableCell
                objCell.Wrap = False
                objCell.Text = IIf(Not IsDBNull(rsMensa("totcota")), rsMensa("totcota"), 0)
                objRow.Controls.Add(objCell)

                'Restrição por Falta de Combustível
                objCell = New TableCell
                objCell.Wrap = False
                objCell.Text = IIf(Not IsDBNull(rsMensa("totrfc")), rsMensa("totrfc"), 0)
                objRow.Controls.Add(objCell)

                'Garantia Energética
                objCell = New TableCell
                objCell.Wrap = False
                objCell.Text = IIf(Not IsDBNull(rsMensa("totrmp")), rsMensa("totrmp"), 0)
                objRow.Controls.Add(objCell)

                'Geração Fora de Mérito
                objCell = New TableCell
                objCell.Wrap = False
                objCell.Text = IIf(Not IsDBNull(rsMensa("totgfm")), rsMensa("totgfm"), 0)
                objRow.Controls.Add(objCell)

                'Crédito por Substituição
                objCell = New TableCell
                objCell.Wrap = False
                objCell.Text = IIf(Not IsDBNull(rsMensa("totcfm")), rsMensa("totcfm"), 0)
                objRow.Controls.Add(objCell)

                'Geração Substituta
                objCell = New TableCell
                objCell.Wrap = False
                objCell.Text = IIf(Not IsDBNull(rsMensa("totsom")), rsMensa("totsom"), 0)
                objRow.Controls.Add(objCell)

                'GE Substituição
                objCell = New TableCell
                objCell.Wrap = False
                objCell.Text = IIf(Not IsDBNull(rsMensa("totges")), rsMensa("totges"), 0)
                objRow.Controls.Add(objCell)

                'GE Crédito
                objCell = New TableCell
                objCell.Wrap = False
                objCell.Text = IIf(Not IsDBNull(rsMensa("totgec")), rsMensa("totgec"), 0)
                objRow.Controls.Add(objCell)

                'Despacho Ciclo Aberto
                objCell = New TableCell
                objCell.Wrap = False
                objCell.Text = IIf(Not IsDBNull(rsMensa("totdca")), rsMensa("totdca"), 0)
                objRow.Controls.Add(objCell)

                ' Despacho Ciclo Reduzido
                objCell = New TableCell
                objCell.Wrap = False
                objCell.Text = IIf(Not IsDBNull(rsMensa("totdcr")), rsMensa("totdcr"), 0)
                objRow.Controls.Add(objCell)

                'Insumo Reserva 1
                objCell = New TableCell
                objCell.Wrap = False
                objCell.Text = IIf(Not IsDBNull(rsMensa("totir1")), rsMensa("totir1"), 0)
                objRow.Controls.Add(objCell)

                'Insumo Reserva 2
                objCell = New TableCell
                objCell.Wrap = False
                objCell.Text = IIf(Not IsDBNull(rsMensa("totir2")), rsMensa("totir2"), 0)
                objRow.Controls.Add(objCell)

                'Insumo Reserva 3
                objCell = New TableCell
                objCell.Wrap = False
                objCell.Text = IIf(Not IsDBNull(rsMensa("totir3")), rsMensa("totir3"), 0)
                objRow.Controls.Add(objCell)

                'Insumo Reserva 4
                objCell = New TableCell
                objCell.Wrap = False
                objCell.Text = IIf(Not IsDBNull(rsMensa("totir4")), rsMensa("totir4"), 0)
                objRow.Controls.Add(objCell)

                'Insumo Recomposição de Reserva Operativa
                objCell = New TableCell
                objCell.Wrap = False
                objCell.Text = IIf(Not IsDBNull(rsMensa("totrro")), rsMensa("totrro"), 0)
                objRow.Controls.Add(objCell)

                'Programacao semanal
                objCell = New TableCell
                objCell.Wrap = False
                objCell.Text = IIf(Not IsDBNull(rsMensa("totcvu")), rsMensa("totcvu"), 0)
                objRow.Controls.Add(objCell)

                'Adiciona a linha a tabela
                tblMensa.Rows.Add(objRow)
                intRow = intRow + 1
            Loop
            rsMensa.Close()

        Catch ex As Exception
            If Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If
            logger.Error("Erro no envio de dados: " + ex.Message, ex)
            Session("strMensagem") = "Não foi possivel enviar os dados (2), por favor tente novamente ou comunique a ocorrência ao ONS." +
                        " (" + ex.Message + "  -  " + ex.StackTrace + ")."
            Response.Redirect("frmMensagem.aspx")
        Finally
            If Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If
        End Try
    End Sub

    Private Sub btnEnviar_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnEnviar.Click
        Dim strDataHora As String
        VerificaFechamento()
        If chkEnvia1.Checked Or chkEnvia2.Checked Or chkIfxE.Checked Or
           chkVazE.Checked Or chkZenE.Checked Or chkZelE.Checked Or
           chkImpE.Checked Or chkExpE.Checked Or chkMreE.Checked Or
           chkPccE.Checked Or chkMcoE.Checked Or chkMosE.Checked Or
           chkMegE.Checked Or chkErpE.Checked Or chkDspE.Checked Or
           chkClfE.Checked Or chkMifE.Checked Or chkPcoE.Checked Or
           chkRfcE.Checked Or chkRmpE.Checked Or chkGfmE.Checked Or
           chkCfmE.Checked Or chkSomE.Checked Or chkGesE.Checked Or
           chkGecE.Checked Or chkDcaE.Checked Or chkDcrE.Checked Or
           chkIr1E.Checked Or chkIr2E.Checked Or chkIr3E.Checked Or
           chkIr4E.Checked Or chkRROE.Checked Then


            'Validação dos dados 
            If (ValidaEnvioDados()) Then
                'O Horário deve ser o mesmo na hora de criar o registro na mensa e gerar o arquivo texto
                strDataHora = Format(Now, "yyyyMMddHHmmss")
                EnviarDados(strDataHora)
            Else
                'Session("strMensagem") = "" & strListErro
                Response.Redirect("frmMensagemValidacaoEnvio.aspx")
            End If

            '####### Alterado para que a emissão do arquivo e o FTP fique dentro da transação ########
            ''Grava arquivo para a empresa selecionada na data selecionada
            '' Conforme definição da Marta, será necessário gerar
            '' o arquivo para dados da área de transferência também,
            '' foi por isso que isolei o if abaixo e estou passando o valor
            '' da opção selecionada para a rotina gravaarquivotexto
            'strData = Mid(cboData.SelectedItem.Value, 7, 4) & Mid(cboData.SelectedItem.Value, 4, 2) & Mid(cboData.SelectedItem.Value, 1, 2)
            'If Not GravaArquivoTexto(cboEmpresa.SelectedItem.Value, strData, strArq, "1", True, bRetorno, strOpcao, strDataHora, "") Then
            '    'Session("strMensagem") = "Não foi possível gravar o arquivo texto, os dados podem estar incorretos. "
            '    Session("strMensagem") = "Não foi possivel enviar os dados, por favor tente novamente ou comunique a ocorrência ao ONS."
            '    Response.Redirect("frmMensagem.aspx")
            'Else
            '    ' Conforme definição da Marta, após enviar o arquivo para área de FTP
            '    ' apagá-lo do diretório Download

            '    If bRetorno = True Then
            '        Response.Write("<script language=JavaScript>")
            '        Response.Write("window.open('frmRecibo.aspx?strNomeArquivo=" & Trim(strNomeArquivo) & "' , '', 'width=700,height=540,status=no,scrollbars=no,titlebar=yes,menubar=no')")
            '        Response.Write("</script>")
            '    Else
            '        'Session("strMensagem") = "Não foi possível gravar o arquivo na área de FTP!"
            '        Session("strMensagem") = "Não foi possivel enviar os dados, por favor tente novamente ou comunique a ocorrência ao ONS."
            '        Response.Redirect("frmMensagem.aspx")
            '    End If
            'End If


        Else
            Session("strMensagem") = "Para enviar os dados o usuário deve selecionar pelo menos 1 envio"
            Response.Redirect("frmMensagem.aspx")
        End If
    End Sub

    Private Sub VerificaFechamento()
        'Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
        'Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
        Dim Conn As System.Data.SqlClient.SqlConnection = New System.Data.SqlClient.SqlConnection
        Conn.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
        Dim Cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand
        Dim blnFechado As Boolean = False
        Cmd.Connection = Conn
        Try
            Conn.Open()
            Cmd.CommandText = "SELECT codstatu " &
                              "FROM pdp " &
                              "WHERE codstatu = 99 " &
                              "AND datpdp = '" & Mid(cboData.SelectedItem.Value, 7, 4) & Mid(cboData.SelectedItem.Value, 4, 2) & Mid(cboData.SelectedItem.Value, 1, 2) & "'"
            'Dim drStatus As OnsClasses.OnsData.OnsDataReader = Cmd.ExecuteReader
            Dim drStatus As System.Data.SqlClient.SqlDataReader = Cmd.ExecuteReader
            If drStatus.Read Then
                blnFechado = True
            End If
            drStatus.Close()
            'Conn.Close()
        Catch ex As Exception
            If Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If
            logger.Error("Erro no envio de dados: " + ex.Message, ex)

            'Session("strMensagem") = "Não foi possível enviar os dados."
            Session("strMensagem") = "Não foi possivel enviar os dados  (3), por favor tente novamente ou comunique a ocorrência ao ONS." +
                        " (" + ex.Message + "  -  " + ex.StackTrace + ")."
            Response.Redirect("frmMensagem.aspx")


        Finally
            If Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If
        End Try
        If blnFechado Then
            Session("strMensagem") = "Os dados não podem ser enviados, a programação está encerrada."    'o dia já foi validado energeticamente."
            Response.Redirect("frmMensagem.aspx")
        End If
    End Sub

    Private Sub InicializarDataTableMensagemErro()
        Dim coluna As DataColumn

        ' cria coluna Mensagem.    
        coluna = New DataColumn()
        coluna.DataType = System.Type.GetType("System.String")
        coluna.ColumnName = "mensagem"
        tbMensagem.Columns.Add(coluna)

    End Sub

    Private Sub AddMensagemErro(ByVal Msg As String)
        Dim row As DataRow = tbMensagem.NewRow()
        row("mensagem") = Msg
        tbMensagem.Rows.Add(row)
    End Sub

    Private Function PatamarToIntervalo(ByVal intPatamar As Integer)

        Select Case intPatamar
            Case 1 : Return "00:00 - 00:30"
            Case 2 : Return "00:30 - 01:00"
            Case 3 : Return "01:00 - 01:30"
            Case 4 : Return "01:30 - 02:00"
            Case 5 : Return "02:00 - 02:30"
            Case 6 : Return "02:30 - 03:00"
            Case 7 : Return "03:00 - 03:30"
            Case 8 : Return "03:30 - 04:00"
            Case 9 : Return "04:00 - 04:30"
            Case 10 : Return "04:30 - 05:00"
            Case 11 : Return "05:00 - 05:30"
            Case 12 : Return "05:30 - 06:00"
            Case 13 : Return "06:00 - 06:30"
            Case 14 : Return "06:30 - 07:00"
            Case 15 : Return "07:00 - 07:30"
            Case 16 : Return "07:30 - 08:00"
            Case 17 : Return "08:00 - 08:30"
            Case 18 : Return "08:30 - 09:00"
            Case 19 : Return "09:00 - 09:30"
            Case 20 : Return "09:30 - 10:00"
            Case 21 : Return "10:00 - 10:30"
            Case 22 : Return "10:30 - 11:00"
            Case 23 : Return "11:00 - 11:30"
            Case 24 : Return "11:30 - 12:00"
            Case 25 : Return "12:00 - 12:30"
            Case 26 : Return "12:30 - 13:00"
            Case 27 : Return "13:00 - 13:30"
            Case 28 : Return "13:30 - 14:00"
            Case 29 : Return "14:00 - 14:30"
            Case 30 : Return "14:30 - 15:00"
            Case 31 : Return "15:00 - 15:30"
            Case 32 : Return "15:30 - 16:00"
            Case 33 : Return "16:00 - 16:30"
            Case 34 : Return "16:30 - 17:00"
            Case 35 : Return "17:00 - 17:30"
            Case 36 : Return "17:30 - 18:00"
            Case 37 : Return "18:00 - 18:30"
            Case 38 : Return "18:30 - 19:00"
            Case 39 : Return "19:00 - 19:30"
            Case 40 : Return "19:30 - 20:00"
            Case 41 : Return "20:00 - 20:30"
            Case 42 : Return "20:30 - 21:00"
            Case 43 : Return "21:00 - 21:30"
            Case 44 : Return "21:30 - 22:00"
            Case 45 : Return "22:00 - 22:30"
            Case 46 : Return "22:30 - 23:00"
            Case 47 : Return "23:00 - 23:30"
            Case 48 : Return "23:30 - 24:00"
        End Select


    End Function

End Class
