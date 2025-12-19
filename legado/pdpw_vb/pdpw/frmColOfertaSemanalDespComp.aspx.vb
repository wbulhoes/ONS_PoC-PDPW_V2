Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports System.Globalization
Imports System.IO
Imports System.Linq
Imports System.Text
Imports OnsClasses.OnsData

Public Class frmColOfertaSemanalDespComp
    Inherits BaseWebUi
    Private logger As log4net.ILog = log4net.LogManager.GetLogger(Me.GetType())
    Private provider As NumberFormatInfo = New NumberFormatInfo
    Dim util As New Util

    Public listaOfertaReservaPotencia As New List(Of OfertaReservaPotencia)

    Dim _idAnoMesConsulta As Integer = 0
    Dim _idSemanaPmoConsulta As Integer = 0
    Dim _idAnoMesEdicao As Integer = 0
    Dim _idSemanaPmoEdicao As Integer = 0
    Dim _idRevisaoConsulta As Integer = 0
    Dim _idRevisaoEdicao As Integer = 0
    Dim _qtdLinhas As Integer = 0
    Dim _listaUsinaAprovada As String = ""

    Public Property ListaUsinaAprovada() As String
        Get
            If (_listaUsinaAprovada = "") Then
                _listaUsinaAprovada = ViewState("ListaUsinaAprovada")
            End If
            Return _listaUsinaAprovada
        End Get
        Set(ByVal value As String)
            _listaUsinaAprovada = value
            ViewState.Add("ListaUsinaAprovada", _listaUsinaAprovada)
        End Set
    End Property

    Public Property IdAnoMesConsulta() As Integer
        Get
            If (_idAnoMesConsulta = 0) Then
                _idAnoMesConsulta = ViewState("IdAnoMesConsulta")
            End If
            Return _idAnoMesConsulta
        End Get
        Set(ByVal value As Integer)
            _idAnoMesConsulta = value
            ViewState.Add("IdAnoMesConsulta", _idAnoMesConsulta)
        End Set
    End Property

    Public Property IdSemanaPmoConsulta() As Integer
        Get
            If (_idSemanaPmoConsulta = 0) Then
                _idSemanaPmoConsulta = ViewState("IdSemanaPmoConsulta")
            End If
            Return _idSemanaPmoConsulta
        End Get
        Set(ByVal value As Integer)
            _idSemanaPmoConsulta = value
            ViewState.Add("IdSemanaPmoConsulta", _idSemanaPmoConsulta)
        End Set
    End Property

    Public Property IdAnoMesEdicao() As Integer
        Get
            If (_idAnoMesEdicao = 0) Then
                _idAnoMesEdicao = ViewState("IdAnoMesEdicao")
            End If
            Return _idAnoMesEdicao
        End Get
        Set(ByVal value As Integer)
            _idAnoMesEdicao = value
            ViewState.Add("IdAnoMesEdicao", _idAnoMesEdicao)
        End Set
    End Property

    Public Property IdSemanaPmoEdicao() As Integer
        Get
            If (_idSemanaPmoEdicao = 0) Then
                _idSemanaPmoEdicao = ViewState("IdSemanaPmoEdicao")
            End If
            Return _idSemanaPmoEdicao
        End Get
        Set(ByVal value As Integer)
            _idSemanaPmoEdicao = value
            ViewState.Add("IdSemanaPmoEdicao", _idSemanaPmoEdicao)
        End Set
    End Property

    Public Property IdRevisaoConsulta() As Integer
        Get
            If (_idRevisaoConsulta = 0) Then
                _idRevisaoConsulta = ViewState("IdRevisaoConsulta")
            End If
            Return _idRevisaoConsulta
        End Get
        Set(ByVal value As Integer)
            _idRevisaoConsulta = value
            ViewState.Add("IdRevisaoConsulta", _idRevisaoConsulta)
        End Set
    End Property

    Public Property IdRevisaoEdicao() As Integer
        Get
            If (_idRevisaoEdicao = 0) Then
                _idRevisaoEdicao = ViewState("IdRevisaoEdicao")
            End If
            Return _idRevisaoEdicao
        End Get
        Set(ByVal value As Integer)
            _idRevisaoEdicao = value
            ViewState.Add("IdRevisaoEdicao", _idRevisaoEdicao)
        End Set
    End Property

    Public Property QtdLinhas() As Integer
        Get
            If (_qtdLinhas = 0) Then
                _qtdLinhas = ViewState("intQtdLinha")
            End If
            Return _qtdLinhas
        End Get
        Set(ByVal value As Integer)
            _qtdLinhas = value
            ViewState.Add("intQtdLinha", _qtdLinhas)
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        provider.NumberDecimalSeparator = "."

        'objMenu = CType(Session("onsmenu"), OnsWebControls.OnsMenu)

        If Not Page.IsPostBack Then
            ExibirDados()
            btnSalvar.Visible = ChecarPossibilidadeEdicao()
        End If
    End Sub

    ''' <summary>
    ''' Método que exibe os dados da tela
    ''' </summary>
    Private Sub ExibirDados()
        Dim dataAtual As DateTime = DateTime.Today
        Dim dataEdicao As DateTime = DateTime.Today.AddDays(7)

        Dim pmoConsulta As List(Of SemanaPMO) = GetSemanaPMO(dataAtual, Nothing, Nothing)
        Dim pmoEdicao As List(Of SemanaPMO) = GetSemanaPMO(dataEdicao, Nothing, Nothing)

        Session("strCodEmpre") = ""

        Dim indiceConsulta As Integer = 0
        Dim indiceEdicao As Integer = 0

        '[TFS_5468]: Removido. pois há SemanaPMO que começa em um mês e termina no outro. Caso isso aconteça deverá ser utilizado a primeira semana.
        'Esse ponto é importante, pois é como o PDP.IU faz a busca para esses mesmos casos.

        '[TFS_7319]: Retificado. Há SemanaPMO que começa em um mês e termina no outro. Caso isso aconteça deverá ser utilizado a segunda semana.
        'Esse ponto é importante, pois é como o PDP.IU faz a busca para esses mesmos casos.
        'ImportarDADGER e ImportarCMO utilizarão a mesma lógica indicada aqui.

        If (pmoConsulta.Count > 1) Then indiceConsulta = 1
        If (pmoEdicao.Count > 1) Then indiceEdicao = 1

        PMOConsultaRadioButton.Text = pmoConsulta(indiceConsulta).Datas_SemanaPmo(0).ToString("dd/MM/yyyy") + " até " + pmoConsulta(indiceConsulta).Datas_SemanaPmo((pmoConsulta(indiceConsulta).Datas_SemanaPmo.Count - 1)).ToString("dd/MM/yyyy") & " - " & pmoConsulta(indiceConsulta).Semana & " (Consulta)"
        PMOEdicaoRadioButton.Text = pmoEdicao(indiceEdicao).Datas_SemanaPmo(0).ToString("dd/MM/yyyy") + " até " + pmoEdicao(indiceEdicao).Datas_SemanaPmo((pmoEdicao(indiceEdicao).Datas_SemanaPmo.Count - 1)).ToString("dd/MM/yyyy") & " - " & pmoEdicao(indiceEdicao).Semana & " (Edição)"
        DataLimiteEnvioConsultaLabel.Text = ObterDiaEnvio(dataAtual, pmoConsulta)
        DataLimiteEnvioEdicaoLabel.Text = ObterDiaEnvio(dataEdicao, pmoEdicao)
        IdRevisaoConsulta = pmoConsulta(indiceConsulta).Semana.Substring(pmoConsulta(indiceConsulta).Semana.Length - 1, 1) - 1
        IdRevisaoEdicao = pmoEdicao(indiceEdicao).Semana.Substring(pmoEdicao(indiceEdicao).Semana.Length - 1, 1) - 1
        ObterUsinasAutorizadas()

        IdAnoMesConsulta = pmoConsulta(indiceConsulta).IdAnomes
        IdSemanaPmoConsulta = pmoConsulta(indiceConsulta).IdSemanapmo
        IdAnoMesEdicao = pmoEdicao(indiceEdicao).IdAnomes
        IdSemanaPmoEdicao = pmoEdicao(indiceEdicao).IdSemanapmo

        PreencheComboEmpresaPOP(AgentesRepresentados, DdlEmpresa, Session("strCodEmpre"))

        DdlEmpresa_SelectedIndexChanged(Nothing, Nothing)
        btnSalvar.Visible = True

    End Sub

    Private Sub ObterUsinasAutorizadas()

        Dim caminhoArquivo As String = Server.MapPath("~") & "\Temp\UsinasAutorizadas.txt" 'ConfigurationManager.AppSettings.Get("PathUsinaAutorizada")
        Dim texto As String = ""
        texto = File.ReadAllText(caminhoArquivo)

        ListaUsinaAprovada = "'" & texto.Replace(",", "','") & "'"

    End Sub

    Private Function ObterDiaEnvio(dataReferencia As DateTime, SemanaPMO As List(Of SemanaPMO)) As String
        'DIA LIMITE PARA O USUARIO FAZER ALTERACOES
        Dim penultimoDia As DateTime
        Dim indiceConsulta = 0

        If (SemanaPMO.Count > 1) Then
            indiceConsulta = 1
        End If


        'If (SemanaPMO.Count > 1) Then
        '    penultimoDia = ConsultaDataLimite(SemanaPMO(1).IdSemanapmo)
        'Else
        '    penultimoDia = ConsultaDataLimite(SemanaPMO(0).IdSemanapmo)
        'End If

        penultimoDia = ConsultaDataLimite(SemanaPMO(indiceConsulta).IdSemanapmo)

        'Validação de retorno "Nothing" do método "ConsultaDataLimite" indicando que não há registro de "Data Limite de Envio de Dados" na tabela
        If Convert.ToDecimal(CDate(penultimoDia).ToString("yyyyMMdd")) = Convert.ToDecimal(CDate("0001-01-01").ToString("yyyyMMdd")) Then

            'If (SemanaPMO.Count > 1) Then
            '    penultimoDia = CDate(SemanaPMO(1).DataInicio).AddDays(-1)
            'Else
            '    penultimoDia = CDate(SemanaPMO(0).DataInicio).AddDays(-1)
            'End If

            penultimoDia = CDate(SemanaPMO(indiceConsulta).DataInicio).AddDays(-1)

            'While util.VerificaFeriado(CStr(penultimoDia.ToString("ddMMyyyy")))
            '    penultimoDia = penultimoDia.AddDays(-1)
            'End While

            penultimoDia = penultimoDia.AddDays(-1)

            'While util.VerificaFeriado(CStr(penultimoDia.ToString("ddMMyyyy")))
            '    penultimoDia = penultimoDia.AddDays(-1)
            'End While

            Return penultimoDia.ToString("dd/MM/yyyy") & " 16:00:00"
        End If

        Return penultimoDia.ToString("dd/MM/yyyy HH:mm:ss")
    End Function

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        If SessaoAtiva(Page.Session) Then
            MyBase.Render(writer)
            'objMenu.RenderControl(writer)
        End If
    End Sub

    Private Function GetDescSemanaPMOAnterior() As String
        Dim retorno As String

        Dim pmo As List(Of SemanaPMO) = GetSemanaPMO(DateTime.Today.AddDays(-7), Nothing, Nothing)
        retorno = pmo(0).Datas_SemanaPmo(0).ToString("dd/MM/yyyy") + " até " + pmo(0).Datas_SemanaPmo((pmo(0).Datas_SemanaPmo.Count - 1)).ToString("dd/MM/yyyy") & "(Edição)"
        Return retorno
    End Function

    Protected Sub DdlEmpresa_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DdlEmpresa.SelectedIndexChanged
        'AtualizarControles()
        'ViewState("Editando") = "S"

        Session("strCodEmpre") = IIf(DdlEmpresa.SelectedIndex > 0, DdlEmpresa.SelectedItem.Value, 0)

        PreencheTable()
    End Sub

    Private Sub RegistrarLog(ByVal mensagem As String)
        'log4net.Config.XmlConfigurator.Configure()
        logger.Debug(mensagem)
    End Sub

    Private Sub RegistrarLogErro(ByVal ex As Exception)
        'log4net.Config.XmlConfigurator.Configure()
        logger.Error(ex.Message, ex)
    End Sub

    Protected Sub BtnSalvar_Click(sender As Object, e As ImageClickEventArgs) Handles btnSalvar.Click
        Try
            'Dim objPermissao As OnsClasses.OnsInterfaceUnica.OnsPermissao = New OnsClasses.OnsInterfaceUnica.OnsPermissao("UPD", "PDPProgSemanal", UsuarID)
            If Not EstaAutorizado Then
                Throw New Exception("Usuário não tem permissão para alterar os valores.")
            End If

            'VERIFICA EMPRESA
            If (DdlEmpresa.SelectedIndex = 0) Then
                Throw New ArgumentException("Selecione uma empresa.")
            End If

            SalvarOfertaSemanalDespComp()
            PreencheTable()

        Catch ex As Exception
            RegistrarLogErro(ex)
            Session("strMensagem") = "" + ex.Message
            Response.Redirect("frmMensagem.aspx")
        End Try
    End Sub

    Private Sub SalvarOfertaSemanalDespComp()
        Dim qndLinhasGrid As Int32 = 0
        Dim Conn As SqlConnection = New SqlConnection
        Dim Cmd As SqlCommand = New SqlCommand
        Dim mensagem As StringBuilder = New StringBuilder()
        Dim limparTabela As Boolean = False

        qndLinhasGrid = QtdLinhas

        Dim sql As String = ""

        Try

            If (QtdLinhas = 0) Then
                PreencheTable()
                RedirecionarParaTelaDeMensagem("Não foi possível gravar os dados. Nenhum dado informado.")
                Exit Sub
            End If

            If Not (ChecarPossibilidadeEdicao()) Then
                PreencheTable()
                RedirecionarParaTelaDeMensagem("Não foi possível gravar os dados. O período de envio está esgotado.")
                Exit Sub
            End If

            For i As Integer = 0 To qndLinhasGrid - 1

                'Obtem os dados da tela
                Dim codusina As String = "", potInstalada As String = "", valorCVU As String,
                    prdUGELigada As String = "", prdUGEDesligada As String = "", valorGeracaoMinima As String = "", prdRampaSubQuente As String = "",
                    prdRampaSubMorno As String = "", prdRampaSubFrio As String = "", prdRampaDesc As String = ""

                codusina = Page.Request.Form("_ctl0:ContentPlaceHolder1:CodUsina_" & i).Trim()
                potInstalada = Page.Request.Form("_ctl0:ContentPlaceHolder1:Potinstalada_" & i).Trim()

                valorCVU = Page.Request.Form("_ctl0:ContentPlaceHolder1:ValorCVU_" & i).Trim()
                prdUGELigada = Page.Request.Form("_ctl0:ContentPlaceHolder1:PrdUGeLigada_" & i).Trim()
                prdUGEDesligada = Page.Request.Form("_ctl0:ContentPlaceHolder1:PrdUGeDesligada_" & i).Trim()
                valorGeracaoMinima = Page.Request.Form("_ctl0:ContentPlaceHolder1:ValorGeracaoMinima_" & i).Trim()
                prdRampaSubQuente = Page.Request.Form("_ctl0:ContentPlaceHolder1:PrdRampaSubQuente_" & i).Trim()
                prdRampaSubMorno = Page.Request.Form("_ctl0:ContentPlaceHolder1:PrdRampaSubMorno_" & i).Trim()
                prdRampaSubFrio = Page.Request.Form("_ctl0:ContentPlaceHolder1:PrdRampaSubFrio_" & i).Trim()
                prdRampaDesc = Page.Request.Form("_ctl0:ContentPlaceHolder1:PrdRampaDesc_" & i).Trim()

                'Valida se há valores sem preenchimento
                valorCVU = IIf(String.IsNullOrEmpty(valorCVU), "0", valorCVU)
                prdUGELigada = IIf(String.IsNullOrEmpty(prdUGELigada), "0", prdUGELigada)
                prdUGEDesligada = IIf(String.IsNullOrEmpty(prdUGEDesligada), "0", prdUGEDesligada)
                valorGeracaoMinima = IIf(String.IsNullOrEmpty(valorGeracaoMinima), "0", valorGeracaoMinima)
                prdRampaSubQuente = IIf(String.IsNullOrEmpty(prdRampaSubQuente), "0", prdRampaSubQuente)
                prdRampaSubMorno = IIf(String.IsNullOrEmpty(prdRampaSubMorno), "0", prdRampaSubMorno)
                prdRampaSubFrio = IIf(String.IsNullOrEmpty(prdRampaSubFrio), "0", prdRampaSubFrio)
                prdRampaDesc = IIf(String.IsNullOrEmpty(prdRampaDesc), "0", prdRampaDesc)

                Dim algumItemPreenchido As Boolean = False

                'Verificar se existe algum valor informado para uma usina, caso exista, os dados da linha deverão ser validados
                If Not (String.IsNullOrEmpty(valorCVU) Or Double.Parse(valorCVU) = 0) Then algumItemPreenchido = True
                If Not (String.IsNullOrEmpty(prdUGELigada) Or Double.Parse(prdUGELigada) = 0) Then algumItemPreenchido = True
                If Not (String.IsNullOrEmpty(prdUGEDesligada) Or Double.Parse(prdUGEDesligada) = 0) Then algumItemPreenchido = True
                If Not (String.IsNullOrEmpty(valorGeracaoMinima) Or Double.Parse(valorGeracaoMinima) = 0) Then algumItemPreenchido = True
                If Not (String.IsNullOrEmpty(prdRampaSubQuente) Or Double.Parse(prdRampaSubQuente) = 0) Then algumItemPreenchido = True
                If Not (String.IsNullOrEmpty(prdRampaSubMorno) Or Double.Parse(prdRampaSubMorno) = 0) Then algumItemPreenchido = True
                If Not (String.IsNullOrEmpty(prdRampaSubFrio) Or Double.Parse(prdRampaSubFrio) = 0) Then algumItemPreenchido = True
                If Not (String.IsNullOrEmpty(prdRampaDesc) Or Double.Parse(prdRampaDesc) = 0) Then algumItemPreenchido = True

                'Se nenhum item foi preenchido essa linha deve ser apagada, caso exista
                If Not (algumItemPreenchido) Then

                    sql += " delete from tb_ofertareservapotencia  where " &
                        " codusina = '" & codusina & "' " &
                        " and id_anomes = " & IdAnoMesEdicao &
                        " and id_semanapmo = " & IdSemanaPmoEdicao & ";"
                Else
                    'Valida dos campos inseridos na tela
                    mensagem.Append(ValidarDadosLinhaGrid(codusina.Trim, potInstalada, valorCVU,
                                           prdUGELigada, prdUGEDesligada, valorGeracaoMinima, prdRampaSubQuente,
                                           prdRampaSubMorno, prdRampaSubFrio, prdRampaDesc))

                    'Como todos os campos são obrigatórios, basta verificar se algum campo está preenchido e se não retornou mensagem com erro
                    If (String.IsNullOrEmpty(mensagem.ToString()) And Not String.IsNullOrEmpty(valorCVU)) Then

                        'apaga os dados antes de inserir
                        sql += " delete from tb_ofertareservapotencia  where " &
                                " codusina = '" & codusina & "' " &
                                " and id_anomes = " & IdAnoMesEdicao &
                                " and id_semanapmo = " & IdSemanaPmoEdicao & ";"

                        sql += " INSERT INTO tb_ofertareservapotencia (codusina, id_anomes, id_semanapmo, val_cvu, prd_ugeligada, prd_ugdesligada, val_geracaominima, 
                                prd_rampasubquente, prd_rampasubmorno, prd_rampasubfrio, prd_rampadesc) VALUES ('" &
                                codusina & "', " &
                                IdAnoMesEdicao & ", " &
                                IdSemanaPmoEdicao & ", " &
                                valorCVU.Replace(",", ".") & ", " &
                                prdUGELigada.Replace(",", ".") & ", " &
                                prdUGEDesligada.Replace(",", ".") & ", " &
                                valorGeracaoMinima.Replace(",", ".") & ", " &
                                prdRampaSubQuente.Replace(",", ".") & ", " &
                                prdRampaSubMorno.Replace(",", ".") & ", " &
                                prdRampaSubFrio.Replace(",", ".") & ", " &
                                prdRampaDesc.Replace(",", ".") & ");"
                    End If
                End If
            Next

            'Verifica se alguma mensagem de erro foi lançada e algum sql foi escrito
            If (String.IsNullOrEmpty(mensagem.ToString()) And Not String.IsNullOrEmpty(sql)) Then

                Cmd.Connection = Conn
                Conn.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
                Conn.Open()

                Cmd.CommandText = sql
                Cmd.ExecuteNonQuery()

                If Conn.State = ConnectionState.Open Then
                    Conn.Close()
                End If



                ExibirMensagem("Dados gravados com sucesso!")
                DdlEmpresa_SelectedIndexChanged(Nothing, Nothing)
            Else
                RedirecionarParaTelaDeMensagem("Não foi possível gravar os dados." & mensagem.ToString())
            End If

        Catch ex As Exception
            RegistrarLogErro(ex)
            RedirecionarParaTelaDeMensagem("Não foi possível gravar os dados." & mensagem.ToString())
        Finally
            If Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If
        End Try
    End Sub

    Private Function ValidarDadosLinhaGrid(codusina As String, potInstalada As String, valorCVU As String,
                                           prdUGELigada As String, prdUGEDesligada As String, valorGeracaoMinima As String, prdRampaSubQuente As String,
                                           prdRampaSubMorno As String, prdRampaSubFrio As String, prdRampaDesc As String) As String

        Dim mensagem As StringBuilder = New StringBuilder()

        'Verificar se todos os valores foram informados
        Dim valorCVUTipado As Decimal = 0, valorGeracaoMinimaTipado As Int64 = 0

        'Valor CVU
        If (String.IsNullOrEmpty(valorCVU)) Then
            mensagem.AppendLine("- O valor do campo Custo Variável Unitário da Usina " & codusina & " é obrigatório e maior do que zero.")
        Else
            valorCVUTipado = Decimal.Parse(valorCVU)
            If (valorCVUTipado <= 0) Then
                mensagem.AppendLine("- O valor do campo Custo Variável Unitário da Usina " & codusina & " é obrigatório e maior do que zero.")
            End If
        End If

        'Valida campos de hora
        mensagem.Append(ValidarCampoHora(prdUGELigada, "Tempo mínimo UG ligada", codusina, False))
        mensagem.Append(ValidarCampoHora(prdUGEDesligada, "Tempo mínimo UG desligada", codusina, True))
        mensagem.Append(ValidarCampoHora(prdRampaSubQuente, "Tempo rampa subida partida a quente", codusina, True))
        mensagem.Append(ValidarCampoHora(prdRampaSubMorno, "Tempo rampa subida partida a morno", codusina, True))
        mensagem.Append(ValidarCampoHora(prdRampaSubFrio, "Tempo rampa subida partida a frio", codusina, True))
        mensagem.Append(ValidarCampoHora(prdRampaDesc, "Tempo rampa descida", codusina, True))


        'Geração mínima
        If (String.IsNullOrEmpty(valorGeracaoMinima)) Then
            mensagem.AppendLine("- O valor do campo Geração mínima da Usina " & codusina & " é obrigatório e deve ser menor ou igual a potência nominal.")
        Else
            If Not (Int64.TryParse(valorGeracaoMinima, valorGeracaoMinimaTipado)) Then
                mensagem.AppendLine("- O valor do campo Geração mínima da Usina " & codusina & " é obrigatório e deve ser menor ou igual a potência nominal.")
            Else
                If (valorGeracaoMinimaTipado > potInstalada) Then
                    mensagem.AppendLine("- O valor do campo Geração mínima da Usina " & codusina & " é obrigatório e deve ser menor ou igual a potência nominal.")
                End If
            End If
        End If

        Return mensagem.ToString()
    End Function

    ''' <summary>
    ''' Valida os campos de hora.
    ''' </summary>
    ''' <param name="hora"></param>
    ''' <param name="nomeCampo"></param>
    ''' <param name="codUsina"></param>
    ''' <param name="podeSerZero"></param>
    ''' <returns></returns>
    Private Function ValidarCampoHora(hora As String, nomeCampo As String, codUsina As String, podeSerZero As Boolean) As String
        Dim mensagem As StringBuilder = New StringBuilder()

        Dim horaTipado As Decimal = 0

        'Verifica se está vazio
        If (String.IsNullOrEmpty(hora)) Then
            If Not (podeSerZero) Then
                mensagem.AppendLine("- O valor do campo " & nomeCampo & " da Usina " & codUsina & " é obrigatório. Deve ser maior que zero. Esse campo só aceita hora inteira ou fração de meia hora (xx,0 ou xx,5).")
            Else
                mensagem.AppendLine("- O valor do campo " & nomeCampo & " da Usina " & codUsina & " é obrigatório. Deve ser maior ou igual a zero. Esse campo só aceita hora inteira ou fração de meia hora (xx,0 ou xx,5).")
            End If
        Else
            'Caso não consiga converter para decimal
            If Not (Decimal.TryParse(hora, horaTipado)) Then
                mensagem.AppendLine(-"O valor do campo " & nomeCampo & " da Usina " & codUsina & " é obrigatório. Deve ser maior que zero. Esse campo só aceita hora inteira ou fração de meia hora (xx,0 ou xx,5).")
            Else
                If Not (podeSerZero) Then
                    'Se for menor ou igual a zero
                    If (horaTipado <= 0) Then
                        mensagem.AppendLine("- O valor do campo " & nomeCampo & " da Usina " & codUsina & " é obrigatório. Deve ser maior que zero. Esse campo só aceita hora inteira ou fração de meia hora (xx,0 ou xx,5).")
                    Else
                        'Verifica a parte decimal da hora
                        If hora.Split(",").Length > 1 Then
                            If (hora.Split(",")(1) <> 0 AndAlso hora.Split(",")(1) <> 5) Then
                                mensagem.AppendLine("- O valor do campo " & nomeCampo & " da Usina " & codUsina & " é obrigatório. Deve ser maior que zero. Esse campo só aceita hora inteira ou fração de meia hora (xx,0 ou xx,5).")
                            End If
                        End If
                    End If
                Else
                    'Pode ser zero, só validar o campo decimal e mudar a mensagem
                    'Verifica a parte decimal da hora
                    If hora.Split(",").Length > 1 Then
                        If (hora.Split(",")(1) <> 0 AndAlso hora.Split(",")(1) <> 5) Then
                            mensagem.AppendLine("- O valor do campo " & nomeCampo & " da Usina " & codUsina & " é obrigatório. Deve ser maior ou igual a zero. Esse campo só aceita hora inteira ou fração de meia hora (xx,0 ou xx,5).")
                        End If
                    End If
                End If
            End If
        End If


        Return mensagem.ToString()

    End Function

    Private Function FormatarNumerico(valor As String) As String
        Dim retorno As String = ""

        If Not String.IsNullOrEmpty(valor) Then
            retorno = String.Format(provider, "{0:#####################0.00}", Convert.ToDecimal(valor, provider))
        Else
            retorno = String.Format(provider, "{0:#####################0.00}", 0)
        End If

        Return Right("00000000" & retorno, 8)
    End Function

    Private Function ConsultaOfertaReservaPotencia(IdAnomes As Integer, IdSemanapmo As Integer, IdRevisao As Integer) As List(Of OfertaReservaPotencia)

        Dim sql As StringBuilder = New StringBuilder()
        sql.Append(" Select u.codusina, ")
        sql.Append("     o.id_anomes,")
        sql.Append("     o.id_semanapmo,")
        sql.Append("     o.val_cvu,")
        sql.Append("     o.prd_ugeligada,")
        sql.Append("     o.prd_ugdesligada,")
        sql.Append("     o.val_geracaominima,")
        sql.Append("     o.prd_rampasubquente,")
        sql.Append("     o.prd_rampasubmorno,")
        sql.Append("     o.prd_rampasubfrio,")
        sql.Append("     o.prd_rampadesc,")
        sql.Append("     u.potinstalada            ")
        sql.Append(" From usina u                            ")
        sql.Append("     inner Join tpusina                            ")
        sql.Append("        On u.tpusina_id = tpusina.tpusina_id                ")
        sql.Append("        And tpusina.id_tpgeracao = 2                            ")
        'sql.Append("     inner Join tb_arquivodadger arq ")
        'sql.Append("        on arq.id_anomes = " & IdAnomes.ToString())
        'sql.Append(" 		and arq.id_semanapmo = " & IdSemanapmo.ToString())
        'sql.Append(" 		and arq.num_revisao = " & IdRevisao.ToString())
        'sql.Append(" 	 inner Join tb_arquivodadgercvu cvu ")
        'sql.Append("        On arq.id_arquivodadger = cvu.id_arquivodadger ")
        'sql.Append("        and cvu.dpp_id = u.dpp_id ")
        sql.Append("    left Join tb_ofertareservapotencia o  ")
        sql.Append("       On o.codusina = u.codusina  ")
        sql.Append("       and o.id_anomes = " & IdAnomes.ToString())
        sql.Append("       and o.id_semanapmo = " & IdSemanapmo.ToString())
        sql.Append(" where ")
        sql.Append("      u.flg_recebepdpage = 'S'  ")
        sql.Append("      and u.codempre = '" & Session("strCodEmpre") & "'")
        'sql.Append("      and cvu.val_cvu > 0 ")

        If (ListaUsinaAprovada <> "") Then
            sql.Append("      and u.codusina in (" + ListaUsinaAprovada + ")")
        End If




        Dim Conn As SqlConnection = New SqlConnection
        Dim Cmd As SqlCommand = New SqlCommand
        Cmd.Connection = Conn
        Cmd.CommandText = sql.ToString()
        Conn.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
        Conn.Open()
        Dim drOfertaReservaPotencia As SqlDataReader = Cmd.ExecuteReader

        Dim listOfertaReservaPotencia As New List(Of OfertaReservaPotencia)
        Do While drOfertaReservaPotencia.Read
            Dim oferta As OfertaReservaPotencia = New OfertaReservaPotencia()
            oferta.CodUsina = If(drOfertaReservaPotencia.Item("codusina") Is DBNull.Value, "", drOfertaReservaPotencia.Item("codusina"))
            oferta.IdAmones = If(drOfertaReservaPotencia.Item("id_anomes") Is DBNull.Value, 0, drOfertaReservaPotencia.Item("id_anomes"))
            oferta.IdSemanaPMO = If(drOfertaReservaPotencia.Item("id_semanapmo") Is DBNull.Value, 0, drOfertaReservaPotencia.Item("id_semanapmo"))
            oferta.Potinstalada = If(drOfertaReservaPotencia.Item("potinstalada") Is DBNull.Value, 0, drOfertaReservaPotencia.Item("potinstalada"))

            If (drOfertaReservaPotencia.Item("val_cvu") Is DBNull.Value) Then
                oferta.ValorCVU = 0
            Else
                oferta.ValorCVU = Format(drOfertaReservaPotencia.Item("val_cvu"), "######0.00")
            End If

            If (drOfertaReservaPotencia.Item("prd_ugeligada") Is DBNull.Value) Then
                oferta.PrdUGeLigada = 0
            Else
                oferta.PrdUGeLigada = Format(drOfertaReservaPotencia.Item("prd_ugeligada"), "######0.0")
            End If

            If (drOfertaReservaPotencia.Item("prd_ugdesligada") Is DBNull.Value) Then
                oferta.PrdUGeDesligada = 0
            Else
                oferta.PrdUGeDesligada = Format(drOfertaReservaPotencia.Item("prd_ugdesligada"), "######0.0")
            End If

            If (drOfertaReservaPotencia.Item("val_geracaominima") Is DBNull.Value) Then
                oferta.ValorGeracaoMinima = 0
            Else
                oferta.ValorGeracaoMinima = Format(drOfertaReservaPotencia.Item("val_geracaominima"), "######0")
            End If

            If (drOfertaReservaPotencia.Item("prd_rampasubquente") Is DBNull.Value) Then
                oferta.PrdRampaSubQuente = -1
            Else
                oferta.PrdRampaSubQuente = Format(drOfertaReservaPotencia.Item("prd_rampasubquente"), "######0.0")
            End If

            If (drOfertaReservaPotencia.Item("prd_rampasubmorno") Is DBNull.Value) Then
                oferta.PrdRampaSubMorno = -1
            Else
                oferta.PrdRampaSubMorno = Format(drOfertaReservaPotencia.Item("prd_rampasubmorno"), "######0.0")
            End If

            If (drOfertaReservaPotencia.Item("prd_rampasubfrio") Is DBNull.Value) Then
                oferta.PrdRampaSubFrio = -1
            Else
                oferta.PrdRampaSubFrio = Format(drOfertaReservaPotencia.Item("prd_rampasubfrio"), "######0.0")
            End If

            If (drOfertaReservaPotencia.Item("prd_rampadesc") Is DBNull.Value) Then
                oferta.PrdRampaDesc = -1
            Else
                oferta.PrdRampaDesc = Format(drOfertaReservaPotencia.Item("prd_rampadesc"), "######0.0")
            End If

            oferta.CodEmpresa = If(Session("strCodEmpre") <> Nothing, Session("strCodEmpre").ToString(), "")
            listOfertaReservaPotencia.Add(oferta)
        Loop

        Conn.Close()

        Return listOfertaReservaPotencia
    End Function


    Public Sub RedirecionarParaTelaDeMensagem(mensagem As String)
        Session("strMensagem") = mensagem.Replace(vbCrLf, "") 'Replace com remção de caracter "Enter"
        'Response.Redirect("frmMensagem.aspx") 'Problema na abertura da tela de Mensagem. aspx com Alert no Document.Ready
    End Sub

    Public Sub ExibirMensagem(mensagem As String)
        Response.Write(String.Format("<SCRIPT>alert('{0}')</SCRIPT>", mensagem))
    End Sub

    Private Sub PreencheTable()


        Dim IdAnoMes As Integer = 0
        Dim IdSemanaPmo As Integer = 0
        Dim IdRevisao As Integer = 0

        If (PMOConsultaRadioButton.Checked) Then
            IdAnoMes = IdAnoMesConsulta
            IdSemanaPmo = IdSemanaPmoConsulta
            IdRevisao = IdRevisaoConsulta
        Else
            IdAnoMes = IdAnoMesEdicao
            IdSemanaPmo = IdSemanaPmoEdicao
            IdRevisao = IdRevisaoEdicao
        End If

        listaOfertaReservaPotencia = ConsultaOfertaReservaPotencia(IdAnoMes, IdSemanaPmo, IdRevisao)

        'MÉDIA NO RODAPÉ
        Dim mediaCVU, mediaPrdUGeLigada, mediaPrdUGeDesligada, mediaValorGeracaoMinima, mediaPrdRampaSubQuente, mediaPrdRampaSubMorno,
            mediaPrdRampaSubFrio, mediaPrdRampaDesc As Decimal

        mediaCVU = 0
        mediaPrdUGeLigada = 0
        mediaPrdUGeDesligada = 0
        mediaValorGeracaoMinima = 0
        mediaPrdRampaSubQuente = 0
        mediaPrdRampaSubMorno = 0
        mediaPrdRampaSubFrio = 0
        mediaPrdRampaDesc = 0

        'MONTAGEM DA TABELA
        tblOfertaSemanalDespComp.Rows.Clear()
        Dim txtValor As TextBox
        Dim strCel As TableCell = New TableCell
        Dim strRow As TableRow = New TableRow
        Dim Tamanho As Unit = New Unit(500)

#Region "MONTAGEM DAS COLUNAS"
        Dim Color As Color = New Color
        Color = ColorTranslator.FromWin32(RGB(233, 244, 207))

        'Usina
        strCel.BackColor = Color.Beige
        strCel.Font.Bold = True
        strCel.Width = New Unit(200)
        strCel.Font.Size.Unit.Point(10)
        strCel.Text = "Usina"
        strRow.Controls.Add(strCel)

        'Potencia Nominal
        strCel = New TableCell
        strCel.BackColor = Color.Beige
        strCel.Font.Bold = True
        strCel.Width = New Unit(200)
        strCel.Font.Size.Unit.Point(10)
        strCel.Text = "Pot. Nominal"
        strRow.Controls.Add(strCel)

        'Valor CVU
        strCel = New TableCell
        strCel.BackColor = Color.Beige
        strCel.Font.Bold = True
        strCel.Width = New Unit(380)
        strCel.Font.Size.Unit.Point(100)
        strCel.Text = "Custo Variável Unitário (R$/MWh) <strong style='color: red;'>*</strong>"
        strRow.Controls.Add(strCel)

        'Tempo Mínimo UG Ligada
        strCel = New TableCell
        strCel.BackColor = Color.Beige
        strCel.Font.Bold = True
        strCel.Width = New Unit(350)
        strCel.Font.Size.Unit.Point(100)
        strCel.Text = "Tempo mínimo UG ligada (horas) <strong style='color: red;'>*</strong>"
        strRow.Controls.Add(strCel)

        'Tempo Mínimo UG Desligada
        strCel = New TableCell
        strCel.BackColor = Color.Beige
        strCel.Font.Bold = True
        strCel.Width = New Unit(390)
        strCel.Font.Size.Unit.Point(100)
        strCel.Text = "Tempo mínimo UG desligada (horas) <strong style='color: red;'>*</strong>"
        strRow.Controls.Add(strCel)

        'Geração Mínima
        strCel = New TableCell
        strCel.BackColor = Color.Beige
        strCel.Font.Bold = True
        strCel.Width = New Unit(310)
        strCel.Font.Size.Unit.Point(100)
        strCel.Text = "Geração mínima (MW) <strong style='color: red;'>*</strong>"
        strRow.Controls.Add(strCel)

        'Tempo Rampa Subida - Partida a Quente (horas)
        strCel = New TableCell
        strCel.BackColor = Color.Beige
        strCel.Font.Bold = True
        strCel.Width = New Unit(515)
        strCel.Font.Size.Unit.Point(100)
        strCel.Text = "Tempo rampa subida - <br> Partida a Quente (horas) <strong style='color: red;'>*</strong>"
        strRow.Controls.Add(strCel)

        'Tempo Rampa Subida - Partida a Morno (horas)
        strCel = New TableCell
        strCel.BackColor = Color.Beige
        strCel.Font.Bold = True
        strCel.Width = Tamanho
        strCel.Font.Size.Unit.Point(100)
        strCel.Text = "Tempo rampa subida - <br> Partida a Morno (horas) <strong style='color: red;'>*</strong>"
        strRow.Controls.Add(strCel)

        'Tempo Rampa Subida - Partida a Frio (horas)
        strCel = New TableCell
        strCel.BackColor = Color.Beige
        strCel.Font.Bold = True
        strCel.Width = Tamanho
        strCel.Font.Size.Unit.Point(100)
        strCel.Text = "Tempo rampa subida - <br> Partida a Frio (horas) <strong style='color: red;'>*</strong>"
        strRow.Controls.Add(strCel)

        'Tempo Rampa Descida (horas)
        strCel = New TableCell
        strCel.BackColor = Color.Beige
        strCel.Font.Bold = True
        strCel.Width = Tamanho
        strCel.Font.Size.Unit.Point(100)
        strCel.Text = "Tempo rampa descida (horas) <strong style='color: red;'>*</strong>"
        strRow.Controls.Add(strCel)

        'Geração Mínima
        strCel = New TableCell
        strCel.Visible = False
        strCel.BackColor = Color.Beige
        strCel.Font.Bold = True
        strCel.Width = New Unit(325)
        strCel.Font.Size.Unit.Point(100)
        strCel.Text = "Potência disponivel"
        strRow.Controls.Add(strCel)

        tblOfertaSemanalDespComp.Controls.Add(strRow)
#End Region

#Region "MONTAGEM DAS LINHAS"

        QtdLinhas = listaOfertaReservaPotencia.Count

        For i As Integer = 0 To listaOfertaReservaPotencia.Count - 1
            'Usina
            txtValor = New TextBox
            txtValor.BorderStyle = BorderStyle.None
            txtValor.Width = Tamanho.Pixel(70)
            txtValor.BackColor = Color.Beige
            txtValor.Font.Bold = True
            txtValor.ID = "CodUsina_" & i
            txtValor.Text = listaOfertaReservaPotencia(i).CodUsina
            txtValor.ReadOnly = True

            strCel = New TableCell
            strCel.BackColor = Color.Beige
            strCel.Controls.Add(txtValor)

            strRow = New TableRow
            strRow.Controls.Add(strCel)

            'Potencia Nominal
            txtValor = New TextBox
            txtValor.BorderStyle = BorderStyle.None
            txtValor.Width = Tamanho.Pixel(70)
            txtValor.BackColor = Color.Beige
            txtValor.Font.Bold = True
            txtValor.ID = "Potinstalada_" & i
            txtValor.Text = listaOfertaReservaPotencia(i).Potinstalada
            txtValor.ReadOnly = True

            strCel = New TableCell
            strCel.BackColor = Color.Beige
            strCel.Controls.Add(txtValor)
            strRow.Controls.Add(strCel)

            'Valor CVU
            txtValor = New TextBox

            txtValor.BorderStyle = BorderStyle.None
            'txtValor.Width = "100%"
            txtValor.ID = "ValorCVU_" & i
            txtValor.Text = IIf(listaOfertaReservaPotencia(i).ValorCVU.HasValue AndAlso listaOfertaReservaPotencia(i).ValorCVU.Value > 0, listaOfertaReservaPotencia(i).ValorCVU.Value, "")
            'txtValor.Attributes.Add("onblur", "validaCampoMonetário('" & "ValorCVU_" & i & "')")
            txtValor.Attributes.Add("onKeyUp", "maskIt(this,event,true)")


            strCel = New TableCell
            strCel.Controls.Add(txtValor)

            strRow.Controls.Add(strCel)

            If Not String.IsNullOrEmpty(txtValor.Text) Then
                mediaCVU += txtValor.Text
            End If

            'Tempo Mínimo UG Ligada
            txtValor = New TextBox
            txtValor.BorderStyle = BorderStyle.None
            txtValor.Width = Tamanho.Pixel(70)
            txtValor.ID = "PrdUGeLigada_" & i
            txtValor.Text = IIf(listaOfertaReservaPotencia(i).PrdUGeLigada.HasValue AndAlso listaOfertaReservaPotencia(i).PrdUGeLigada.Value > 0, listaOfertaReservaPotencia(i).PrdUGeLigada.Value, "")
            'txtValor.Attributes.Add("onblur", "validaCampoHora('" & "PrdUGeLigada_" & i & "')")

            strCel = New TableCell
            strCel.Controls.Add(txtValor)

            strRow.Controls.Add(strCel)

            If Not String.IsNullOrEmpty(txtValor.Text) Then
                mediaPrdUGeLigada += txtValor.Text
            End If

            'Tempo Mínimo UG Desligada
            txtValor = New TextBox
            txtValor.BorderStyle = BorderStyle.None
            txtValor.Width = Tamanho.Pixel(70)
            txtValor.ID = "PrdUGeDesligada_" & i
            txtValor.Text = IIf((listaOfertaReservaPotencia(i).PrdUGeDesligada.HasValue AndAlso listaOfertaReservaPotencia(i).PrdUGeDesligada.Value > -1) AndAlso
                                (listaOfertaReservaPotencia(i).ValorCVU.HasValue AndAlso listaOfertaReservaPotencia(i).ValorCVU.Value > 0),
                                listaOfertaReservaPotencia(i).PrdUGeDesligada.Value, "")
            'txtValor.Attributes.Add("onblur", "validaCampoHora('" & "PrdUGeDesligada_" & i & "')")

            strCel = New TableCell
            strCel.Controls.Add(txtValor)

            strRow.Controls.Add(strCel)

            If Not String.IsNullOrEmpty(txtValor.Text) Then
                mediaPrdUGeDesligada += txtValor.Text
            End If

            'Geração Mínima
            txtValor = New TextBox
            txtValor.BorderStyle = BorderStyle.None
            txtValor.Width = Tamanho.Pixel(70)
            txtValor.ID = "ValorGeracaoMinima_" & i
            txtValor.Text = IIf((listaOfertaReservaPotencia(i).ValorGeracaoMinima.HasValue AndAlso listaOfertaReservaPotencia(i).ValorGeracaoMinima.Value > -1) AndAlso
                                (listaOfertaReservaPotencia(i).ValorCVU.HasValue AndAlso listaOfertaReservaPotencia(i).ValorCVU.Value > 0),
                                 listaOfertaReservaPotencia(i).ValorGeracaoMinima.Value, "")
            'txtValor.Attributes.Add("onblur", "validaCampoInteiro('" & "ValorGeracaoMinima_" & i & "')")

            strCel = New TableCell
            strCel.Controls.Add(txtValor)

            strRow.Controls.Add(strCel)

            If Not String.IsNullOrEmpty(txtValor.Text) Then
                mediaValorGeracaoMinima += txtValor.Text
            End If

            'Tempo Rampa Subida - Partida a Quente (horas)
            txtValor = New TextBox
            txtValor.BorderStyle = BorderStyle.None
            txtValor.Width = Tamanho.Pixel(70)
            txtValor.ID = "PrdRampaSubQuente_" & i
            txtValor.Text = IIf(listaOfertaReservaPotencia(i).PrdRampaSubQuente.HasValue AndAlso listaOfertaReservaPotencia(i).PrdRampaSubQuente.Value > -1, listaOfertaReservaPotencia(i).PrdRampaSubQuente.Value, "")
            'txtValor.Attributes.Add("onblur", "validaCampoHora('" & "PrdRampaSubQuente_" & i & "')")

            strCel = New TableCell
            strCel.Controls.Add(txtValor)

            strRow.Controls.Add(strCel)

            If Not String.IsNullOrEmpty(txtValor.Text) Then
                mediaPrdRampaSubQuente += txtValor.Text
            End If

            'Tempo Rampa Subida - Partida a Morno (horas)
            txtValor = New TextBox
            txtValor.BorderStyle = BorderStyle.None
            txtValor.Width = Tamanho.Pixel(70)
            txtValor.ID = "PrdRampaSubMorno_" & i
            txtValor.Text = IIf(listaOfertaReservaPotencia(i).PrdRampaSubMorno.HasValue AndAlso listaOfertaReservaPotencia(i).PrdRampaSubMorno.Value > -1, listaOfertaReservaPotencia(i).PrdRampaSubMorno.Value, "")
            'txtValor.Attributes.Add("onblur", "validaCampoHora('" & "PrdRampaSubMorno_" & i & "')")

            strCel = New TableCell
            strCel.Controls.Add(txtValor)

            strRow.Controls.Add(strCel)

            If Not String.IsNullOrEmpty(txtValor.Text) Then
                mediaPrdRampaSubMorno += txtValor.Text
            End If

            'Tempo Rampa Subida - Partida a Frio (horas)
            txtValor = New TextBox
            txtValor.BorderStyle = BorderStyle.None
            txtValor.Width = Tamanho.Pixel(70)
            txtValor.ID = "PrdRampaSubFrio_" & i
            txtValor.Text = IIf(listaOfertaReservaPotencia(i).PrdRampaSubFrio.HasValue AndAlso listaOfertaReservaPotencia(i).PrdRampaSubFrio.Value > -1, listaOfertaReservaPotencia(i).PrdRampaSubFrio.Value, "")
            'txtValor.Attributes.Add("onblur", "validaCampoHora('" & "PrdRampaSubFrio_" & i & "')")

            strCel = New TableCell
            strCel.Controls.Add(txtValor)

            strRow.Controls.Add(strCel)

            If Not String.IsNullOrEmpty(txtValor.Text) Then
                mediaPrdRampaSubFrio += txtValor.Text
            End If

            'Tempo Rampa Descida (horas)
            txtValor = New TextBox
            txtValor.BorderStyle = BorderStyle.None
            txtValor.Width = Tamanho.Pixel(70)
            txtValor.ID = "PrdRampaDesc_" & i
            txtValor.Text = IIf(listaOfertaReservaPotencia(i).PrdRampaDesc.HasValue AndAlso listaOfertaReservaPotencia(i).PrdRampaDesc.Value > -1, listaOfertaReservaPotencia(i).PrdRampaDesc.Value, "")
            'txtValor.Attributes.Add("onblur", "validaCampoHora('" & "PrdRampaDesc_" & i & "')")

            strCel = New TableCell
            strCel.Controls.Add(txtValor)

            strRow.Controls.Add(strCel)

            If Not String.IsNullOrEmpty(txtValor.Text) Then
                mediaPrdRampaDesc += txtValor.Text
            End If

            'Potencia instalada
            txtValor = New TextBox
            txtValor.Visible = False
            txtValor.BorderStyle = BorderStyle.None
            txtValor.Width = Tamanho.Pixel(70)
            txtValor.ID = "ValorPotinstalada_" & i
            txtValor.Text = IIf(listaOfertaReservaPotencia(i).Potinstalada.HasValue AndAlso listaOfertaReservaPotencia(i).Potinstalada.Value > 0, listaOfertaReservaPotencia(i).ValorGeracaoMinima.Value, "")

            strCel = New TableCell
            strCel.Controls.Add(txtValor)

            strRow.Controls.Add(strCel)

            'Incluindo a linha na tabela
            tblOfertaSemanalDespComp.Controls.Add(strRow)
        Next
#End Region


    End Sub

    Protected Sub PMOConsultaRadioButton_CheckedChanged(sender As Object, e As EventArgs) Handles PMOConsultaRadioButton.CheckedChanged

        btnSalvar.Visible = ChecarPossibilidadeEdicao()

        PreencheTable()

    End Sub

    Private Function ChecarPossibilidadeEdicao() As Boolean
        Dim dataLimite As DateTime

        If PMOConsultaRadioButton.Checked Then
            dataLimite = ConsultaDataLimite(IdSemanaPmoConsulta)
        Else
            dataLimite = ConsultaDataLimite(IdSemanaPmoEdicao)
        End If

        If Convert.ToDecimal(CDate(dataLimite).ToString("yyyyMMdd")) = Convert.ToDecimal(CDate("0001-01-01").ToString("yyyyMMdd")) Then
            dataLimite = DateTime.Parse(DataLimiteEnvioEdicaoLabel.Text)
        End If

        'O administrador pode alterar o dado a qualquer momento
        If (PerfilID = "ADM_PDPW") Then
            Return True
        End If

        If (DateTime.Now <= dataLimite) Then
            If (PMOConsultaRadioButton.Checked) Then
                Return False
            Else
                Return True
            End If
        Else
            Return False
        End If

    End Function

    Private Function ConsultaDataLimite(id_semanapmo As Integer) As DateTime
        Dim Conn As SqlConnection = New SqlConnection
        Dim Cmd As SqlCommand = New SqlCommand

        Cmd.Connection = Conn
        Conn.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
        Conn.Open()

        Dim sql As StringBuilder = New StringBuilder()
        sql.Append(" SELECT dat_limite, hor_limite ")
        sql.Append(" FROM tb_limiteenvioofertasage ")
        sql.Append(" where id_semanapmo = " & id_semanapmo)

        Cmd.CommandText = sql.ToString()

        Dim drLimite As SqlDataReader = Cmd.ExecuteReader
        If drLimite.Read Then
            If Not IsDBNull(drLimite.Item("dat_limite")) AndAlso Not IsDBNull(drLimite.Item("hor_limite")) Then
                Dim limite As LimiteEnvioOferta = New LimiteEnvioOferta()
                limite.DataLimite = Convert.ToDateTime(drLimite.Item("dat_limite").ToString())
                limite.HoraLimite = Convert.ToDateTime(drLimite.Item("hor_limite").ToString())

                Return String.Format("{0} {1}", limite.DataLimite.ToString("dd/MM/yyyy"), limite.HoraLimite.ToString("HH:mm"))
            Else
                Return Nothing
            End If
        End If

        Return Nothing
    End Function

    Private Sub PMOEdicaoRadioButton_Load(sender As Object, e As EventArgs) Handles PMOEdicaoRadioButton.Load

    End Sub
End Class