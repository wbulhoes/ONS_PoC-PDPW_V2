
Imports System.Collections.Generic
Imports System.Globalization
Imports System.IO
Imports System.Linq
Imports System.Text
Imports OnsClasses.OnsData

Public Class PDPProgSemanal
    Inherits System.Web.UI.Page
    Private logger As log4net.ILog = log4net.LogManager.GetLogger(Me.GetType())
    Private provider As NumberFormatInfo = New NumberFormatInfo
    Dim util As New Util

    Public ofertaDaSemana As New List(Of OfertaSemanal)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsNothing(Page.Request.Form("btnExcluir")) Then
            DeleteProgSemanal(Integer.Parse(Page.Request.Form("btnExcluir")))
            ofertaDaSemana = ConsultaOferta()

            LblSemanaPMO.Text = GetDescProximaSemanaPMO()
            lblID.Text = "0"

            PreencheComboEmpresaPOP(AgentesRepresentados, DdlEmpresa, Session("strCodEmpre"))
            DdlEmpresa_SelectedIndexChanged(sender, e)

            ViewState("Editando") = "N"

            AtualizarControles()

            Return
        End If

        If Page.Request.Form("btnEditar") <> Nothing Then

            Dim ofertaSemanal As OfertaSemanal = ConsultaOfertaPorId(Integer.Parse(Page.Request.Form("btnEditar")))

            EdtVolumeProgramacao.Text = ofertaSemanal.VolumeProgramacao.ToString()
            EdtVolumeTempoReal.Text = ofertaSemanal.VolumeTempoReal.ToString()
            EdtPrecoTempoReal.Text = ofertaSemanal.PrecoTempoReal.ToString()
            EdtPrecoProgramacao.Text = ofertaSemanal.PrecoProgramacao.ToString()
            lblID.Text = ofertaSemanal.OfertaSemanalId
            ChkDependentes.Checked = ofertaSemanal.Dependentes

            LblSemanaPMO.Text = GetDescProximaSemanaPMO()

            PreencheComboEmpresaPOP(AgentesRepresentados, DdlEmpresa, Session("strCodEmpre"))
            DdlEmpresa_SelectedIndexChanged(sender, e)

            DdlUsinaProgramacao.SelectedValue = ofertaSemanal.CodUsinaProgramacao
            DdlUsinaTempoReal.SelectedValue = ofertaSemanal.CodUsinaTempoReal

            ViewState("Editando") = "S"
            AtualizarControles()

            Return
        End If

        provider.NumberDecimalSeparator = "."

        'objMenu = CType(Session("onsmenu"), OnsWebControls.OnsMenu)

        If Not Page.IsPostBack Then
            'ValidarAberturaDaSemanaPMO()

            Session("strCodEmpre") = ""

            DdlUsinaProgramacao.Items.Clear()
            DdlUsinaTempoReal.Items.Clear()

            LblSemanaPMO.Text = GetDescProximaSemanaPMO()

            PreencheComboEmpresaPOP(AgentesRepresentados, DdlEmpresa, Session("strCodEmpre"))

            DdlEmpresa_SelectedIndexChanged(sender, e)

            ViewState("Editando") = "N"

            AtualizarControles()

        Else
            AtualizarControles()
        End If
    End Sub

    Private Sub AtualizarControles()
        If ViewState("Editando") = "S" Then
            BtnSalvar.Visible = True
            BtnIncluirNovo.Visible = False
        Else
            BtnSalvar.Visible = False
            BtnIncluirNovo.Visible = ConsultaOferta().Count = 0
        End If
    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        If SessaoAtiva(Page.Session) Then
            MyBase.Render(writer)
            'objMenu.RenderControl(writer)
        End If
    End Sub

    Private Function GetDescProximaSemanaPMO() As String
        Dim retorno As String

        Dim pmo As List(Of SemanaPMO) = GetProximaSemanaPMO(DateTime.Today, Nothing, Nothing)
        retorno = "Semana PMO - " + pmo(0).Datas_SemanaPmo(0).ToString("dd/MM/yyyy") + " até " + pmo(0).Datas_SemanaPmo((pmo(0).Datas_SemanaPmo.Count - 1)).ToString("dd/MM/yyyy")
        Return retorno
    End Function

    Protected Sub DdlEmpresa_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DdlEmpresa.SelectedIndexChanged

        Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
        Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
        Cmd.Connection = Conn

        If DdlEmpresa.SelectedIndex > 0 Then
            Session("strCodEmpre") = DdlEmpresa.SelectedItem.Value
        Else
            Session("strCodEmpre") = 0
        End If

        DdlUsinaProgramacao.Items.Clear()
        DdlUsinaTempoReal.Items.Clear()
        ViewState("Editando") = "N"
        AtualizarControles()

        Dim usinas As List(Of Usina) = ListarUsinasDeDemanda()

        Dim item As Usina
        For Each item In usinas
            DdlUsinaProgramacao.Items.Add(New ListItem(item.NomUsina, item.CodUsina))
            DdlUsinaTempoReal.Items.Add(New ListItem(item.NomUsina, item.CodUsina))
        Next


        ofertaDaSemana = ConsultaOferta()


    End Sub


    Private Sub ChecarDependencia()

        'Comentado por solicitação do cliente.
        'DdlUsinaTempoReal.Enabled = Not ChkDependentes.Checked

        'If ChkDependentes.Checked Then
        '    DdlUsinaTempoReal.SelectedValue = DdlUsinaProgramacao.SelectedValue
        'End If

    End Sub

    Private Function ListarUsinasDeDemanda() As List(Of Usina)
        Dim I As Integer
        Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
        Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
        Cmd.Connection = Conn

        Try
            Cmd.CommandText = "select u.codusina, u.nomusina" &
                              "  from usina u " &
                              " where u.codempre = '" & Session("strCodEmpre") & "'" &
                              "   and u.tpusina_id = 'UTD' " &
                              " order by u.ordem, u.codusina"

            Conn.Open("pdp")
            Dim rsUsina As OnsClasses.OnsData.OnsDataReader = Cmd.ExecuteReader
            Dim item As Usina

            Dim itens As List(Of Usina) = New List(Of Usina)

            itens.Add(New Usina("", "Selecione uma Usina"))

            Do While rsUsina.Read
                item = New Usina(rsUsina.Item("codusina"), rsUsina.Item("nomusina"))
                itens.Add(item)
            Loop

            rsUsina.Close()
            rsUsina = Nothing
            Conn.Close()
            Return itens
        Catch ex As Exception
            RegistrarLogErro(ex)
            Session("strMensagem") = "Não foi possível acessar a Base de Dados."
            If Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If
            Response.Redirect("frmMensagem.aspx")
        Finally
            If Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If
        End Try

    End Function

    Private Sub RegistrarLog(ByVal mensagem As String)
        'log4net.Config.XmlConfigurator.Configure()
        logger.Debug(mensagem)
    End Sub

    Private Sub RegistrarLogErro(ByVal ex As Exception)
        'log4net.Config.XmlConfigurator.Configure()
        logger.Error(ex.Message, ex)
    End Sub

    Protected Sub BtnIncluirNovo_Click(sender As Object, e As ImageClickEventArgs) Handles BtnIncluirNovo.Click

        'Dim objPermissao As OnsClasses.OnsInterfaceUnica.OnsPermissao
        'objPermissao = New OnsClasses.OnsInterfaceUnica.OnsPermissao("UPD", "PDPProgSemanal", UsuarID)

        Try
            If Not EstaAutorizado Then
                Throw New Exception("Usuário não tem permissão para alterar os valores.")
            End If

            ValidarEntradas()

            SalvarProgramacaoSemanal(0)

            DdlUsinaProgramacao.SelectedIndex = 0
            DdlUsinaTempoReal.SelectedIndex = 0
            EdtVolumeProgramacao.Text = ""
            EdtVolumeTempoReal.Text = ""
            EdtPrecoProgramacao.Text = ""
            EdtPrecoTempoReal.Text = ""
            ChkDependentes.Checked = False

            ViewState("Editando") = "N"
            AtualizarControles()

        Catch ex As Exception
            RegistrarLogErro(ex)
            Session("strMensagem") = "" + ex.Message
            Response.Redirect("frmMensagem.aspx")
        End Try


    End Sub
    Protected Sub BtnSalvar_Click(sender As Object, e As ImageClickEventArgs) Handles BtnSalvar.Click
        'Dim objPermissao As OnsClasses.OnsInterfaceUnica.OnsPermissao
        'objPermissao = New OnsClasses.OnsInterfaceUnica.OnsPermissao("UPD", "PDPProgSemanal", UsuarID)

        Try
            If Not EstaAutorizado Then
                Throw New Exception("Usuário não tem permissão para alterar os valores.")
            End If

            ValidarEntradas()

            SalvarProgramacaoSemanal(Integer.Parse(lblID.Text))

            DdlUsinaProgramacao.SelectedIndex = 0
            DdlUsinaTempoReal.SelectedIndex = 0
            EdtVolumeProgramacao.Text = ""
            EdtVolumeTempoReal.Text = ""
            EdtPrecoProgramacao.Text = ""
            EdtPrecoTempoReal.Text = ""
            lblID.Text = ""

            ViewState("Editando") = "N"
            AtualizarControles()

        Catch ex As Exception
            RegistrarLogErro(ex)
            Session("strMensagem") = "" + ex.Message
            Response.Redirect("frmMensagem.aspx")
        End Try

    End Sub

    Private Sub SalvarProgramacaoSemanal(id As Integer)

        Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
        Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
        Dim transaction As OnsTransaction
        Cmd.Connection = Conn

        Dim pmo As List(Of SemanaPMO) = GetProximaSemanaPMO(DateTime.Today, Nothing, Nothing)

        Dim penultimoDia As DateTime = ConsultaDataLimite(pmo(0).IdSemanapmo)

        'Validação de retorno "Nothing" do método "ConsultaDataLimite" indicando que não há registro de "Data Limite de Envio de Dados" na tabela
        If Convert.ToDecimal(CDate(penultimoDia).ToString("yyyyMMdd")) = Convert.ToDecimal(CDate("0001-01-01").ToString("yyyyMMdd")) Then
            penultimoDia = CDate(pmo(0).Datas_SemanaPmo(0).AddDays(-2)).ToString("yyyy-MM-dd 11:00")
        End If

        Conn.Open("pdp")
        transaction = Conn.BeginTransaction()

        Try
            Cmd.Transaction = transaction

            Dim mensagem As String = ""
            Dim sucesso As Boolean = False

            If (id > 0) Then
                Dim oferta As OfertaSemanal = ConsultaOfertaPorId(id)

                If (VerificarSeExisteOfertaDiaria()) Then
                    mensagem = "Tendo em vista já ter sido realizada uma ou mais oferta(s) diária(s) e conforme alertado anteriormente, estes dados não podem mais ser alterados. Em caso de dúvida, contactar a equipe de programação diária do ONS."
                Else
                    AtualizaOferta(Cmd, pmo(0), oferta)
                    mensagem = "Salvo com sucesso.\n O prazo limite para fazer alterações é: " & CDate(penultimoDia).ToString("dd/MM/yyyy HH:mm") & " desde que não tenha feito a oferta diária relativa a essa semana."
                    sucesso = True
                End If
            Else
                'valida se ja existe um registro
                If (ValidarProgSemanal(pmo(0).IdSemanapmo, pmo(0).IdAnomes, DdlUsinaProgramacao.SelectedItem.Value, DdlUsinaTempoReal.SelectedItem.Value)) Then
                    mensagem = "Já existe oferta para essa semana."
                Else
                    CadastrarOfertaSemanal(Cmd, pmo(0))
                    mensagem = "Salvo com sucesso.\n O prazo limite para fazer alterações é: " & CDate(penultimoDia).ToString("dd/MM/yyyy HH:mm") & " desde que não tenha feito a oferta diária relativa a essa semana."
                    sucesso = True
                End If
            End If

            transaction.Commit()

            If Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If

            If sucesso Then
                ExibirMensagem(mensagem)
                DdlEmpresa_SelectedIndexChanged(Nothing, Nothing)
            Else
                RedirecionarParaTelaDeMensagem(mensagem)
            End If



        Catch ex As Exception
            RegistrarLogErro(ex)
            transaction.Rollback()
            If Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If
            RedirecionarParaTelaDeMensagem(ex.Message)
        End Try
    End Sub

    Private Function VerificarSeExisteOfertaDiaria() As Boolean
        Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
        Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
        Cmd.Connection = Conn

        Try
            Cmd.CommandText = " select * from tb_respdemanda WHERE id_respdemandasemanal = " & lblID.Text & ""

            Conn.Open("pdp")
            Dim obj As OnsClasses.OnsData.OnsDataReader = Cmd.ExecuteReader

            Do While obj.Read
                Return True
            Loop

            Return False

            Conn.Close()
        Catch ex As Exception
            RegistrarLogErro(ex)
            Session("strMensagem") = "Não foi possível acessar a Base de Dados."
            If Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If
            Response.Redirect("frmMensagem.aspx")
        Finally
            If Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If
        End Try
    End Function

    Private Function ValidarProgSemanal(idsemanapmo As Integer, idanomes As Integer, cod_usinaprog As String, cod_usinatr As String) As Boolean

        Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
        Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
        Cmd.Connection = Conn

        Try

            Cmd.CommandText = "select * from tb_respdemandasemanal where id_semanapmo = " & idsemanapmo & " and id_anomes = '" & idanomes & "' and cod_usinaprog = '" & cod_usinaprog & "'"

            Conn.Open("pdp")
            Dim obj As OnsClasses.OnsData.OnsDataReader = Cmd.ExecuteReader

            Do While obj.Read
                Return True
            Loop

            Return False

            Conn.Close()

        Catch ex As Exception
            RegistrarLogErro(ex)
            Session("strMensagem") = "Não foi possível acessar a Base de Dados."
            If Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If
            Response.Redirect("frmMensagem.aspx")
        Finally
            If Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If
        End Try


    End Function

    Private Function CadastrarOfertaSemanal(cmd As OnsCommand, pmo As SemanaPMO)
        Try

            Dim sql As StringBuilder = New StringBuilder()


            Dim precoTempoReal As Decimal
            Dim precoProgramacao As Decimal
            Dim volumeTempoReal As Integer
            Dim volumeProgramacao As Integer
            Dim dependentes As String
            Dim usinaProgramacao As String = DdlUsinaProgramacao.SelectedValue
            Dim usinaTempoReal As String = DdlUsinaTempoReal.SelectedValue

            If Trim(EdtPrecoTempoReal.Text) <> "" Then
                precoTempoReal = Convert.ToDecimal(EdtPrecoTempoReal.Text.Trim())
            Else
                precoTempoReal = 0
            End If

            If Trim(EdtPrecoProgramacao.Text) <> "" Then

                precoProgramacao = Convert.ToDecimal(EdtPrecoProgramacao.Text.Trim())
            Else
                precoProgramacao = 0
            End If

            If Trim(EdtVolumeTempoReal.Text) <> "" Then
                volumeTempoReal = CInt(Trim(EdtVolumeTempoReal.Text))
            Else
                volumeTempoReal = 0
            End If

            If Trim(EdtVolumeProgramacao.Text) <> "" Then
                volumeProgramacao = CInt(Trim(EdtVolumeProgramacao.Text))
            Else
                volumeProgramacao = 0
            End If

            If ChkDependentes.Checked Then
                dependentes = "S"
            Else
                dependentes = "N"
            End If


            sql.Append("insert into tb_respdemandasemanal(")
            sql.Append("id_semanapmo,")
            sql.Append("id_anomes,")
            sql.Append("cod_usinatr,")
            sql.Append("cod_usinaprog,")
            sql.Append("val_cvutr,")
            sql.Append("val_cvuprog,")
            sql.Append("val_volumetr,")
            sql.Append("val_volumeprog,")
            sql.Append("flg_dependente)")
            sql.Append("values(")
            sql.Append("" & pmo.IdSemanapmo & ",")
            sql.Append("'" & pmo.IdAnomes & "',")
            If (Trim(usinaTempoReal) = "") Then
                sql.Append("null,")
            Else
                sql.Append("'" & Trim(usinaTempoReal) & "',")
            End If
            If (Trim(usinaProgramacao) = "") Then
                sql.Append("null,")
            Else
                sql.Append("'" & Trim(usinaProgramacao) & "',")
            End If
            sql.Append("'" & precoTempoReal & "',")
            sql.Append("'" & precoProgramacao & "',")
            sql.Append(" " & volumeTempoReal & ",")
            sql.Append(" " & volumeProgramacao & ",")
            sql.Append("'" & dependentes & "');")

            cmd.CommandText = sql.ToString()
            cmd.CommandType = CommandType.Text
            cmd.ExecuteNonQuery()

            cmd.CommandText = "SELECT MAX(id_respdemandasemanal) FROM tb_respdemandasemanal"
            Dim reader As OnsDataReader = cmd.ExecuteReader()

            Dim ret As Integer

            While reader.Read()
                ret = reader(0)
            End While

            reader.Close()
            Return ret

        Catch ex As Exception

        End Try
    End Function

    Private Sub ValidarAberturaDaSemanaPMO()
        Dim pmo As SemanaPMO = GetProximaSemanaPMO(DateTime.Today, Nothing, Nothing)(0)
        Dim diasAbertos As List(Of DateTime) = ListarDiasAbertos(pmo.Datas_SemanaPmo(0), pmo.Datas_SemanaPmo(6))

        If pmo.Datas_SemanaPmo.Count <> diasAbertos.Count Then
            Dim diasFechados As String = ""

            For Each dia As DateTime In pmo.Datas_SemanaPmo
                If Not diasAbertos.Contains(dia) Then
                    If diasFechados.Length > 1 Then
                        diasFechados &= ", "
                    End If
                    diasFechados &= dia.ToString("dd/MM/yyyy")
                End If
            Next

            Response.Write("<SCRIPT>alert('Os seguintes dias precisam ser abertos: " & diasFechados & ".')</SCRIPT>")

            BtnSalvar.Enabled = False
            DdlEmpresa.Enabled = False
        Else
            BtnSalvar.Enabled = True
            DdlEmpresa.Enabled = True
        End If
    End Sub

    Private Function FormatarNumerico(valor As String) As String
        Dim retorno As String = ""

        If Not String.IsNullOrEmpty(valor) Then
            retorno = String.Format(provider, "{0:#####################0.00}", Convert.ToDecimal(valor, provider))
        Else
            retorno = String.Format(provider, "{0:#####################0.00}", 0)
        End If

        Return Right("00000000" & retorno, 8)
    End Function

    Private Function ConsultaOferta() As List(Of OfertaSemanal)

        Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
        Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand

        Dim pmo As SemanaPMO = GetProximaSemanaPMO(DateTime.Today, Nothing, Nothing)(0)

        Cmd.Connection = Conn
        Conn.Open("pdp")

        Dim sql As StringBuilder = New StringBuilder()

        Dim existeOferta As Boolean = False

        sql.Append(" SELECT r.id_respdemandasemanal, r.cod_usinaprog, round(r.val_volumeprog)::int val_volumeprog, r.val_cvuprog, r.cod_usinatr, round(r.val_volumetr)::int val_volumetr, r.val_cvutr, r.flg_dependente  ")
        sql.Append(" FROM tb_respdemandasemanal r ")
        sql.Append(" join usina u on u.codusina = r.cod_usinaprog  ")
        sql.Append(" where u.codempre = '" & Session("strCodEmpre") & "' and r.id_semanapmo = " & pmo.IdSemanapmo)
        sql.Append(" union")
        sql.Append(" SELECT r.id_respdemandasemanal, r.cod_usinaprog, round(r.val_volumeprog)::int val_volumeprog, r.val_cvuprog, r.cod_usinatr, round(r.val_volumetr)::int val_volumetr, r.val_cvutr, r.flg_dependente  ")
        sql.Append(" FROM tb_respdemandasemanal r ")
        sql.Append(" join usina u on u.codusina = r.cod_usinatr  ")
        sql.Append(" where u.codempre = '" & Session("strCodEmpre") & "' and r.id_semanapmo = " & pmo.IdSemanapmo)

        Cmd.CommandText = sql.ToString()

        Dim drOfertas As OnsClasses.OnsData.OnsDataReader = Cmd.ExecuteReader

        Dim listOfertaSemanal As New List(Of OfertaSemanal)
        Do While drOfertas.Read

            Dim oferta As OfertaSemanal = New OfertaSemanal()

            oferta.OfertaSemanalId = drOfertas.Item("id_respdemandasemanal")
            oferta.CodUsinaProgramacao = drOfertas.Item("cod_usinaprog").ToString()
            oferta.VolumeProgramacao = drOfertas.Item("val_volumeprog")
            oferta.PrecoProgramacao = drOfertas.Item("val_cvuprog")
            oferta.CodUsinaTempoReal = drOfertas.Item("cod_usinatr").ToString()
            oferta.VolumeTempoReal = drOfertas.Item("val_volumetr")
            oferta.PrecoTempoReal = drOfertas.Item("val_cvutr")

            oferta.Dependentes = IIf(drOfertas.Item("flg_dependente") = "N", False, True)

            listOfertaSemanal.Add(oferta)
        Loop

        Return listOfertaSemanal

    End Function

    Private Function ConsultaOfertaPorId(id As Integer) As OfertaSemanal

        Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
        Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand

        Cmd.Connection = Conn
        Conn.Open("pdp")

        Dim sql As StringBuilder = New StringBuilder()

        sql.Append(" SELECT r.id_respdemandasemanal, r.cod_usinaprog, round(r.val_volumeprog)::int val_volumeprog, r.val_cvuprog, r.cod_usinatr, round(r.val_volumetr)::int val_volumetr, r.val_cvutr, r.flg_dependente  ")
        sql.Append(" FROM tb_respdemandasemanal r ")
        sql.Append(" where  r.id_respdemandasemanal = " & id)

        Cmd.CommandText = sql.ToString()

        Dim drOfertas As OnsClasses.OnsData.OnsDataReader = Cmd.ExecuteReader

        Dim listOfertaSemanal As New List(Of OfertaSemanal)
        Do While drOfertas.Read

            Dim oferta As OfertaSemanal = New OfertaSemanal()

            oferta.OfertaSemanalId = drOfertas.Item("id_respdemandasemanal")
            oferta.CodUsinaProgramacao = drOfertas.Item("cod_usinaprog").ToString()
            oferta.VolumeProgramacao = drOfertas.Item("val_volumeprog")
            oferta.PrecoProgramacao = drOfertas.Item("val_cvuprog")

            oferta.CodUsinaTempoReal = drOfertas.Item("cod_usinatr").ToString()
            oferta.VolumeTempoReal = drOfertas.Item("val_volumetr")
            oferta.PrecoTempoReal = drOfertas.Item("val_cvutr")

            oferta.Dependentes = IIf(drOfertas.Item("flg_dependente") = "N", False, True)

            listOfertaSemanal.Add(oferta)
        Loop

        Return listOfertaSemanal(0)

    End Function

    'Cadastrar oferta diaria
    Private Sub CadastrarOferta(cmd As OnsCommand, dia As DateTime, id As Integer)

        Dim pdp As String = dia.ToString("yyyyMMdd")
        Dim sql As StringBuilder = New StringBuilder()


        sql.Append("insert into tb_respdemanda (")
        sql.Append("datpdp")
        sql.Append(", cod_usinatr")
        sql.Append(", cod_usinaprog")
        sql.Append(", val_volumetr")
        sql.Append(", val_volumeprog")
        sql.Append(", id_respdemandasemanal")
        sql.Append(")")
        sql.Append("values (")
        sql.Append("'" & pdp & "'")
        sql.Append(",'" & DdlUsinaTempoReal.SelectedValue & "'")
        sql.Append(",'" & DdlUsinaProgramacao.SelectedValue & "'")



        If Trim(EdtVolumeTempoReal.Text) <> "" Then
            sql.Append("," & CInt(Trim(EdtVolumeTempoReal.Text)))
        Else
            sql.Append(", 0")
        End If

        If Trim(EdtVolumeProgramacao.Text) <> "" Then
            sql.Append("," & CInt(Trim(EdtVolumeProgramacao.Text)))
        Else
            sql.Append(", 0")
        End If



        sql.Append(", " & id)
        sql.Append(");")

        Dim I As Integer

        For I = 1 To 48
            sql.AppendLine("")

            sql.Append("insert into disponibilidade (datpdp, codusina, intdsp, valdsptran, valdspemp) values (")

            sql.Append("'" & pdp & "'")

            sql.Append(", '" & DdlUsinaProgramacao.SelectedValue & "'")

            sql.Append(", " & I)

            If Trim(EdtVolumeProgramacao.Text) <> "" Then
                sql.Append("," & CInt(Trim(EdtVolumeProgramacao.Text)))
            Else
                sql.Append(", 0")
            End If

            If Trim(EdtVolumeProgramacao.Text) <> "" Then
                sql.Append("," & CInt(Trim(EdtVolumeProgramacao.Text)))
            Else
                sql.Append(", 0")
            End If

            sql.Append(");")
        Next

        cmd.Parameters.Clear()
        cmd.CommandText = sql.ToString()
        cmd.CommandType = CommandType.Text
        cmd.ExecuteNonQuery()

    End Sub

    Private Sub AtualizaOferta(cmd As OnsCommand, semana As SemanaPMO, ofertas As OfertaSemanal)

        Dim sql As StringBuilder = New StringBuilder()

        'persiste na tabela todo
        sql.Clear()

        sql.Append("UPDATE tb_respdemandasemanal rs")
        If (String.IsNullOrEmpty(DdlUsinaTempoReal.SelectedValue)) Then
            sql.Append(" SET cod_usinatr = null, ")
        Else
            sql.Append(" SET cod_usinatr = '" & DdlUsinaTempoReal.SelectedValue & "', ")
        End If

        If (String.IsNullOrEmpty(DdlUsinaProgramacao.SelectedValue)) Then
            sql.Append("cod_usinaprog = null,")
        Else
            sql.Append("cod_usinaprog = '" & DdlUsinaProgramacao.SelectedValue & "',")
        End If

        If Trim(EdtPrecoTempoReal.Text.Trim()) <> "" Then
            sql.Append("val_cvutr = '" & Convert.ToDecimal(EdtPrecoTempoReal.Text.Trim()) & "',")
        Else
            sql.Append("val_cvutr = 0,")
        End If

        If Trim(EdtVolumeTempoReal.Text.Trim()) <> "" Then
            sql.Append("val_volumetr = " & CInt(Trim(EdtVolumeTempoReal.Text)) & ",")
        Else
            sql.Append("val_volumetr = 0,")
        End If

        If Trim(EdtVolumeProgramacao.Text.Trim()) <> "" Then
            sql.Append("val_volumeprog = " & CInt(Trim(EdtVolumeProgramacao.Text)) & ",")
        Else
            sql.Append("val_volumeprog = 0,")
        End If

        sql.Append("flg_dependente = '" & IIf(ChkDependentes.Checked, "S", "N") & "',")

        If Trim(EdtPrecoProgramacao.Text.Trim()) <> "" Then
            sql.Append("val_cvuprog = '" & Convert.ToDecimal(EdtPrecoProgramacao.Text.Trim()) & "' ")
        Else
            sql.Append("val_cvuprog = 0 ")
        End If

        sql.Append(" WHERE id_respdemandasemanal = " & lblID.Text & "")

        cmd.Parameters.Clear()
        cmd.CommandText = sql.ToString()
        cmd.CommandType = CommandType.Text
        cmd.ExecuteNonQuery()
    End Sub

    Private Sub ValidarEntradas()
        'VERIFICA EMPRESA
        If (DdlEmpresa.SelectedIndex = 0) Then
            Throw New ArgumentException("Selecione uma empresa.")
        End If

        'VERIFICA USINAS
        If (DdlUsinaProgramacao.SelectedIndex = 0 AndAlso DdlUsinaTempoReal.SelectedIndex = 0) Then
            Throw New ArgumentException("Selecione uma usina para a Programação ou Tempo Real.")
        Else

            'PROGRAMACAO
            If (DdlUsinaProgramacao.SelectedIndex > 0) Then
                If (Not String.IsNullOrEmpty(EdtVolumeProgramacao.Text)) Then
                    Try
                        If (Convert.ToInt32(EdtVolumeProgramacao.Text) < 5) Then
                            Throw New ArgumentException("O Volume da Programação precisa ser maior ou igual a 5.")
                        End If
                    Catch
                        Throw New ArgumentException("O campo Volume da Programação é somente número.")
                    End Try
                Else
                    Throw New ArgumentException("Informe o Volume da programação.")
                End If

                If (Not String.IsNullOrEmpty(EdtPrecoProgramacao.Text)) Then
                    If (Convert.ToDouble(EdtPrecoProgramacao.Text) < 1D) Then
                        Throw New ArgumentException("O Preço da programação precisa ser maior que 0.")
                    End If
                Else
                    Throw New ArgumentException("Informe o preço da programação.")
                End If
            End If

            'TEMPO REAL
            If (DdlUsinaTempoReal.SelectedIndex > 0) Then
                If (Not String.IsNullOrEmpty(EdtVolumeTempoReal.Text)) Then
                    Try
                        If (Convert.ToInt32(EdtVolumeTempoReal.Text) < 5) Then
                            Throw New ArgumentException("O Volume do Tempo Real precisa ser maior ou igual a 5.")
                        End If
                    Catch
                        Throw New ArgumentException("O campo Volume do Tempo Real é somente número.")
                    End Try
                Else
                    Throw New ArgumentException("Informe o Volume do Tempo Real.")
                End If

                If (Not String.IsNullOrEmpty(EdtPrecoTempoReal.Text)) Then
                    If (Convert.ToDouble(EdtPrecoTempoReal.Text) < 1D) Then
                        Throw New ArgumentException("O Preço do Tempo Real precisa ser maior que 0.")
                    End If
                Else
                    Throw New ArgumentException("Informe o preço do tempo real.")
                End If
            End If

        End If

        'Comentado por solicitação do cliente.
        'DEPENDENTES
        'If ChkDependentes.Checked Then
        '    If (DdlUsinaProgramacao.SelectedIndex <> DdlUsinaTempoReal.SelectedIndex) Then
        '        Throw New ArgumentException("Em caso de dependencia, as Usinas selecionadas devem ser iguais.")
        '    End If
        'End If

        'DIA LIMITE PARA O USUARIO FAZER ALTERACOES
        Dim SemanaPMO As List(Of SemanaPMO) = GetProximaSemanaPMO(DateTime.Today, Nothing, Nothing)
        Dim penultimoDia As DateTime

        penultimoDia = ConsultaDataLimite(SemanaPMO(0).IdSemanapmo)

        'Validação de retorno "Nothing" do método "ConsultaDataLimite" indicando que não há registro de "Data Limite de Envio de Dados" na tabela
        If Convert.ToDecimal(CDate(penultimoDia).ToString("yyyyMMdd")) = Convert.ToDecimal(CDate("0001-01-01").ToString("yyyyMMdd")) Then
            penultimoDia = CDate(SemanaPMO(0).Datas_SemanaPmo(0).AddDays(-2)).ToString("yyyy-MM-dd 11:00")

            'While util.VerificaFeriado(CStr(penultimoDia.ToString("ddMMyyyy")))
            '    penultimoDia = penultimoDia.AddDays(-1)
            'End While

            'PRAZO DE ENVIO
            If PerfilID <> "ADM_PDPW" Then
                If (Convert.ToDecimal(DateTime.Now.ToString("yyyyMMddHHmm")) > Convert.ToDecimal(CDate(penultimoDia).ToString("yyyyMMdd1100"))) Then
                    Throw New ArgumentException("Fora do prazo de envio.")
                End If
            End If
        End If

        'PRAZO DE ENVIO
        If PerfilID <> "ADM_PDPW" Then
            If (Convert.ToDecimal(DateTime.Now.ToString("yyyyMMddHHmm")) > Convert.ToDecimal(CDate(penultimoDia).ToString("yyyyMMddHHmm"))) Then
                Throw New ArgumentException("Fora do prazo de envio.")
            End If
        End If
    End Sub

    Private Function ConsultaDataLimite(id_semanapmo As Integer) As Date
        Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
        Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand

        Cmd.Connection = Conn
        Conn.Open("pdp")

        Dim sql As StringBuilder = New StringBuilder()
        sql.Append(" SELECT dat_limite, hor_limite ")
        sql.Append(" FROM tb_limiteenvioofertasage ")
        sql.Append(" where id_semanapmo = " & id_semanapmo)

        Cmd.CommandText = sql.ToString()

        Dim drLimite As OnsClasses.OnsData.OnsDataReader = Cmd.ExecuteReader
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

    Private Function ValidarVolume(componente As TextBox) As Boolean
        Try
            Convert.ToInt32(componente.Text)
            Return True
        Catch ex As Exception
            RegistrarLogErro(ex)
            componente.Text = ""
            componente.Focus()
            Return False
        End Try
    End Function

    Private Function ValidarPreco(componente As TextBox) As Boolean
        Dim valor As String = componente.Text
        If valor.Contains(".") And Not valor.Contains(",") Then
            Return False
        End If
        valor = valor.Trim().Replace(".", "").Replace(",", ".")
        Try
            valor = FormatarNumerico(valor)
            Return True
        Catch ex As Exception
            RegistrarLogErro(ex)
            componente.Text = ""
            componente.Focus()
            Return False
        End Try
    End Function

    Protected Sub EdtVolumeProgramacao_TextChanged(sender As Object, e As EventArgs) Handles EdtVolumeProgramacao.TextChanged
        If String.IsNullOrEmpty(EdtVolumeProgramacao.Text) And Not ValidarPreco(EdtVolumeProgramacao) Then
            Response.Write("<SCRIPT>alert('Informe um volume válido! Ex.: 123456789')</SCRIPT>")
        End If
        ChecarDependencia()

        ofertaDaSemana = ConsultaOferta()

    End Sub

    Protected Sub EdtVolumeTempoReal_TextChanged(sender As Object, e As EventArgs) Handles EdtVolumeTempoReal.TextChanged
        If String.IsNullOrEmpty(EdtVolumeTempoReal.Text) And Not ValidarPreco(EdtVolumeTempoReal) Then
            Response.Write("<SCRIPT>alert('Informe um volume válido! Ex.: 123456789')</SCRIPT>")
        End If
        ChecarDependencia()

        ofertaDaSemana = ConsultaOferta()

    End Sub

    Protected Sub EdtPrecoTempoReal_TextChanged(sender As Object, e As EventArgs) Handles EdtPrecoTempoReal.TextChanged
        If String.IsNullOrEmpty(EdtPrecoTempoReal.Text) And Not ValidarPreco(EdtPrecoTempoReal) Then
            Response.Write("<SCRIPT>alert('Informe um preço válido! Ex.: 1.234.567,89')</SCRIPT>")
        End If
        ChecarDependencia()

        ofertaDaSemana = ConsultaOferta()

    End Sub

    Protected Sub EdtPrecoProgramacao_TextChanged(sender As Object, e As EventArgs) Handles EdtPrecoProgramacao.TextChanged
        If String.IsNullOrEmpty(EdtPrecoProgramacao.Text) And Not ValidarPreco(EdtPrecoProgramacao) Then
            Response.Write("<SCRIPT>alert('Informe um preço válido! Ex.: 1.234.567,89')</SCRIPT>")
        End If
        ChecarDependencia()

        ofertaDaSemana = ConsultaOferta()

    End Sub

    Public Sub DeleteProgSemanal(id As Integer)

        Try
            Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
            Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
            Dim sql As StringBuilder = New StringBuilder()

            Cmd.Connection = Conn
            Conn.Open("pdp")

            sql.Append(" delete from tb_respdemandasemanal where id_respdemandasemanal = " & id & "")

            Cmd.CommandText = sql.ToString()
            Cmd.CommandType = CommandType.Text
            Cmd.ExecuteNonQuery()

            Conn.Close()

            Response.Write("<SCRIPT>alert('Registro deletado com sucesso.')</SCRIPT>")

        Catch ex As Exception
            RedirecionarParaTelaDeMensagem("Tendo em vista já ter sido realizada uma ou mais oferta(s) diária(s) e conforme alertado anteriormente, estes dados não podem mais ser alterados. Em caso de dúvida, contactar a equipe de programação diária do ONS.")
        End Try

    End Sub

    Public Sub RedirecionarParaTelaDeMensagem(mensagem As String)
        Session("strMensagem") = mensagem
        Response.Redirect("frmMensagem.aspx", False)
    End Sub

    Public Sub ExibirMensagem(mensagem As String)
        Response.Write(String.Format("<SCRIPT>alert('{0}')</SCRIPT>", mensagem))
    End Sub

End Class