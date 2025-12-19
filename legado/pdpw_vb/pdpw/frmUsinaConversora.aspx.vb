Imports System.Collections.Generic
Imports System.IO

Public Class frmUsinaConversora
    Inherits BaseWebUi

    Dim _listaUsinaAprovadaOfertaExportacao As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then


                PreencheComboEmpresaPOP(AgentesRepresentados, cboEmpresa, Session("strCodEmpre"))

                ObterUsinasAutorizadasOfertaExportacao()

                If Not IsNothing(Session("strCodEmpre")) Then
                    Me.ListaUsinas(Session("strCodEmpre"))
                    Me.Carregar_Grid(Session("strCodEmpre"))
                End If

                '' Bloquear edição do campo Percentual de Perda se o usuário não for ADM_PDPW
                If UsuarioTemPermissaoADM() Then
                    txtPercentualPerda.Enabled = True
                Else
                    txtPercentualPerda.Enabled = False
                End If
            End If

        Catch ex As Exception
            AlertaMensagem(ex.Message)
        End Try
    End Sub



    Private Function UsuarioTemPermissaoADM() As Boolean

        ' Dim PerfilID As String = "ADM_PDPW" ' Simulando um perfil com permissão ATUPDP

        If PerfilID.Equals("ADM_PDPW") Then
            Return True
        Else
            Return False
        End If

    End Function


#Region "Eventos"

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        If SessaoAtiva(Page.Session) Then
            MyBase.Render(writer)
            'objMenu.RenderControl(writer)
        End If
    End Sub

    Protected Sub btnSalvar_Click(sender As Object, e As ImageClickEventArgs) Handles btnSalvar.Click
        Me.SalvarUsinaConversora()
    End Sub

    Protected Sub cboEmpresa_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboEmpresa.SelectedIndexChanged

        If cboEmpresa.SelectedIndex > 0 Then
            Session("strCodEmpre") = cboEmpresa.SelectedItem.Value
            Me.ListaUsinas(Session("strCodEmpre"))
            Me.Carregar_Grid(Session("strCodEmpre"))
        Else
            cboUsina.Items.Clear()
            cboUsinaConversora.Items.Clear()
        End If

    End Sub

    Protected Sub cboUsina_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboUsina.SelectedIndexChanged
        Dim codEmpre As String
        Dim codUsina As String

        If cboEmpresa.SelectedIndex > 0 And cboUsina.SelectedIndex > 0 Then
            codEmpre = cboEmpresa.SelectedItem.Value
            codUsina = cboUsina.SelectedItem.Value

            Me.ListarUsinasConversoras(codUsina)
            Me.Carregar_Grid(codEmpre, codUsina)
        Else
            cboUsinaConversora.Items.Clear()

            If Not IsNothing(Session("strCodEmpre")) Then
                Me.Carregar_Grid(Session("strCodEmpre"))
            End If

        End If

    End Sub

    Protected Sub UsinaConversoraGridView_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles UsinaConversoraGridView.RowDataBound

        If e.Row.RowState = (DataControlRowState.Edit Or DataControlRowState.Alternate) Or e.Row.RowState = DataControlRowState.Edit Then
            Exit Sub
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim deleteButton As ImageButton = DirectCast(e.Row.Cells(4).Controls(0), ImageButton)
            deleteButton.OnClientClick = "if (!window.confirm('Confirma a exclusão deste registro ?')) return false;"
        End If

    End Sub

    Protected Sub UsinaConversoraGridView_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles UsinaConversoraGridView.RowDeleting
        Dim id As String = Convert.ToInt32(UsinaConversoraGridView.DataKeys(e.RowIndex).Value.ToString())
        Me.ExcluirUsinaConversora(id)
    End Sub

#End Region

#Region "Métodos"

    Private Sub ValidaRegraEdicaoPorcentagemperda(codUsina As String, codConversora As String, codEmpresa As String)

        If Me.FactoryBusiness.UsinaConversora.ExisteUsinaConversora(codUsina, codConversora) Then 'Edição da Usina/Conversora

            Dim listaCodEmpresas As List(Of String) = New List(Of String)({codEmpresa})

            Dim dentro_prazo_envio As Boolean =
                    Me.FactoryBusiness.
                        OfertaExportacao.
                        ValidarPrazoEnvioOfertaAgente(DateTime.Now.AddDays(1).ToString("dd/MM/yyyy"), listaCodEmpresas)

            If Not dentro_prazo_envio Then

                Dim existeOfertasFuturas_AnalisePendente As Boolean =
                                    Me.FactoryBusiness.
                                        UsinaConversora.
                                        ExisteOfertaFutura_PendenteAnalise_UsinaConversora(codUsina, codConversora)

                If existeOfertasFuturas_AnalisePendente Then
                    Throw New Exception($"A porcentagem de perda cadastrada para Usina {codUsina.Trim()} e Conversora {codConversora.Trim()} não pode ser alterada, pois existem Ofertas para data de programação {DateTime.Now.AddDays(1).ToString("dd/MM/yyyy")} com pendência de decisão (aprovação ou reprovação) pelo ONS e/ou Agente, verifique.")
                End If
            End If
        End If
    End Sub

    Private Sub ListaUsinas(ByVal codEmpre As String)
        Dim usinaConversoraDao As UsinaConversoraDAO = New UsinaConversoraDAO

        Try
            Dim listaUsinas As List(Of UsiConversDTO) =
                usinaConversoraDao.
                ListarUsinas(
                codEmpre,
                "UTE",
                Me.ListaUsinaAprovadaOfertaExportacao)

            cboUsina.Items.Clear()

            Dim obj As New WebControls.ListItem
            obj.Text = $" Selecione... "
            obj.Value = ""

            cboUsina.Items.Add(obj)

            For Each usina As UsiConversDTO In listaUsinas

                Dim objUsina As New WebControls.ListItem
                objUsina.Text = $"{usina.CodUsina.Trim()} - {usina.NomUsina.Trim()}"
                objUsina.Value = usina.CodUsina

                cboUsina.Items.Add(objUsina)
            Next

        Catch ex As Exception

        Finally
            usinaConversoraDao = Nothing
        End Try

    End Sub

    Private Sub ListarUsinasConversoras(ByVal codUsina As String)
        Dim usinaConversoraDao As UsinaConversoraDAO = New UsinaConversoraDAO

        Try
            Dim listaUsinaConversora As List(Of UsiConversDTO) = usinaConversoraDao.ListarUsinasConversoras("CNV", codUsina)

            cboUsinaConversora.Items.Clear()

            Dim obj As New WebControls.ListItem
            obj.Text = $" Selecione... "
            obj.Value = ""

            cboUsinaConversora.Items.Add(obj)

            For Each usinaConversora As UsiConversDTO In listaUsinaConversora
                Dim objUsinaConversora As New WebControls.ListItem
                objUsinaConversora.Text = $"{usinaConversora.CodUsina.Trim()} - {usinaConversora.NomUsina.Trim()}"
                objUsinaConversora.Value = usinaConversora.CodUsina

                cboUsinaConversora.Items.Add(objUsinaConversora)
            Next

        Catch ex As Exception

        Finally
            usinaConversoraDao = Nothing
        End Try

    End Sub

    Private Sub SalvarUsinaConversora()
        Dim codEmpreSelecionada As String
        Dim usinaConversoraDao As UsinaConversoraDAO = New UsinaConversoraDAO

        ' Validação da Empresa
        If cboEmpresa.SelectedIndex = 0 Then
            AlertaMensagem("Deve-se obrigatoriamente selecionar a Empresa.")
            Exit Sub
        Else
            codEmpreSelecionada = cboEmpresa.SelectedItem.Value
        End If

        ' Validação da Usina
        If cboUsina.SelectedIndex = 0 Then
            AlertaMensagem("Deve-se obrigatoriamente selecionar a Usina.")
            Exit Sub
        End If

        ' Validação da Usina Conversora
        If cboUsinaConversora.SelectedIndex = 0 Then
            AlertaMensagem("Deve-se obrigatoriamente selecionar a Usina Conversora.")
            Exit Sub
        End If

        ' Validação do Percentual de Perda
        Dim pctPerda As Decimal = 0
        Dim valorNumerico_pct As Boolean = Decimal.TryParse(txtPercentualPerda.Text, pctPerda)

        If UsuarioTemPermissaoADM() Then
            ' Garantir que, se o valor for 0 ou não numérico, ele não seja considerado na atualização
            If Not valorNumerico_pct OrElse pctPerda < 0 Then
                AlertaMensagem("O Percentual da Perda deve ser um valor numérico decimal positivo.")
                Exit Sub
            End If
        End If



        ' Validação da Prioridade
        Dim numPrioridade As Decimal = 0
        Dim valorNumerico_prioridade As Boolean = Int32.TryParse(txtPrioridade.Text, numPrioridade)

        If String.IsNullOrEmpty(txtPrioridade.Text) Or Not valorNumerico_prioridade Then
            AlertaMensagem("Deve-se obrigatoriamente inserir o número da prioridade para Usina/Conversora (valor numérico inteiro).")
            Exit Sub
        End If

        Try
            ' Prepara o objeto para salvar
            Dim usinaConversora As UsiConversDTO = New UsiConversDTO()
            usinaConversora.CodUsina = cboUsina.SelectedItem.Value.ToString()
            usinaConversora.codConversora = cboUsinaConversora.SelectedItem.Value.ToString()
            usinaConversora.PercentualPerda = pctPerda
            usinaConversora.NumeroPrioridade = numPrioridade

            ' Validação de regra de edição
            ValidaRegraEdicaoPorcentagemperda(usinaConversora.CodUsina, usinaConversora.codConversora, codEmpreSelecionada)

            ' Chama o método de salvar no banco de dados
            usinaConversoraDao.Salvar(usinaConversora)

            ' Atualiza a grid e limpa os campos
            Me.Carregar_Grid(codEmpreSelecionada, usinaConversora.CodUsina)
            txtPercentualPerda.Text = ""
            Me.ListarUsinasConversoras(usinaConversora.CodUsina)

            ' Mensagem de sucesso
            Me.AlertaMensagem("Salvo com sucesso.")
            lblMsg.Visible = False
        Catch ex As Exception
            ' Exibe mensagem de erro
            lblMsg.Visible = True
            AlertaMensagem($"Erro ao salvar: {ex.Message}")
        Finally
            usinaConversoraDao = Nothing
        End Try
    End Sub



    Private Sub ExcluirUsinaConversora(ByVal idUsinaConversora As String)

        Try
            Dim usinaConversora As UsinaConversoraDTO = New UsinaConversoraDTO()
            usinaConversora.IdUsinaConversora = CInt(idUsinaConversora)

            Me.FactoryBusiness.UsinaConversora.Excluir(idUsinaConversora)

            If cboUsina.SelectedIndex > 0 Then
                usinaConversora.CodUsina = cboUsina.SelectedItem.Value.ToString()
                Me.ListarUsinasConversoras(usinaConversora.CodUsina)
            End If

            Me.Carregar_Grid(Session("strCodEmpre"), usinaConversora.CodUsina)
            AlertaMensagem($"Exclusão realizada com sucesso.")

            lblMsg.Visible = False
        Catch ex As Exception
            lblMsg.Visible = True
            'Session("strMensagem") = "Não foi possível acessar a Base de Dados. Ocorreu o seguinte erro: " & Chr(10) & Err.Description
            'Response.Redirect("frmMensagem.aspx")
        End Try

    End Sub

    Private Function PreencherTabela(ByVal strCodEmpre As String, Optional ByVal strCodUsina As String = Nothing) As DataTable
        Dim usinaConversoraDao As UsinaConversoraDAO = New UsinaConversoraDAO
        Dim oDataTable As DataTable                 ' DataTable com linhas do grid.
        Dim oRow As DataRow                         ' Linha auxiliar.

        Dim listaUsinaConversora As List(Of UsiConversDTO) = usinaConversoraDao.ListaUsinaConversoraPorUsina(strCodUsina, strCodEmpre)
        oDataTable = New DataTable

        oDataTable.Columns.Add("usina", System.Type.GetType("System.String"))
        oDataTable.Columns.Add("conversora", System.Type.GetType("System.String"))
        oDataTable.Columns.Add("perda", System.Type.GetType("System.String"))
        oDataTable.Columns.Add("id_usinaconversora", System.Type.GetType("System.String"))
        oDataTable.Columns.Add("prioridade", System.Type.GetType("System.String"))
        Dim intI As Integer = 1

        For Each usinaConversora As UsiConversDTO In listaUsinaConversora
            oRow = oDataTable.NewRow

            oRow(0) = $"{usinaConversora.CodUsina} - {usinaConversora.NomUsina}"
            oRow(1) = $"{usinaConversora.CodUsinaConversora} - {usinaConversora.nomConversora}"
            oRow(2) = usinaConversora.PercentualPerda
            oRow(3) = usinaConversora.IdUsinaConversora
            oRow(4) = usinaConversora.NumeroPrioridade

            oDataTable.Rows.Add(oRow)
            intI = intI + 1
        Next

        Return oDataTable
    End Function


    Private Sub Carregar_Grid(ByVal strCodEmpre As String, Optional ByVal strCodUsina As String = Nothing)
        UsinaConversoraGridView.DataSource = Me.PreencherTabela(strCodEmpre, strCodUsina)
        UsinaConversoraGridView.DataBind()
        UsinaConversoraGridView.Visible = True

    End Sub

    Private Sub AlertaMensagem(ByVal mensagemTexto As String)
        Response.Write("<SCRIPT>alert('" + mensagemTexto.Replace("\n", "") + "')</SCRIPT>")
    End Sub

    Private Property ListaUsinaAprovadaOfertaExportacao() As String
        Get
            If (_listaUsinaAprovadaOfertaExportacao = "") Then
                _listaUsinaAprovadaOfertaExportacao = ViewState("ListaUsinaAprovadaOfertaExportacao")
            End If
            Return _listaUsinaAprovadaOfertaExportacao
        End Get
        Set(ByVal value As String)
            _listaUsinaAprovadaOfertaExportacao = value
            ViewState.Add("ListaUsinaAprovadaOfertaExportacao", _listaUsinaAprovadaOfertaExportacao)
        End Set
    End Property

    Private Sub ObterUsinasAutorizadasOfertaExportacao()

        Dim caminhoArquivo As String = Server.MapPath("~") & "\Temp\UsinasAutorizadasOfertaExportacao.txt"
        Dim texto As String = ""
        texto = File.ReadAllText(caminhoArquivo)

        If Not String.IsNullOrEmpty(texto) Then
            ListaUsinaAprovadaOfertaExportacao = "'" & texto.Replace(",", "','") & "'"
        End If


    End Sub

#End Region



End Class