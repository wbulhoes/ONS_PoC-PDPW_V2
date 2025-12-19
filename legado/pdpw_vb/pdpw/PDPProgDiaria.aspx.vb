Imports System.Collections.Generic
Imports System.Globalization

Imports System.IO
Imports System.Text
Imports OnsClasses.OnsData
Imports pdpw

Public Class PDPProgDiaria
    Inherits System.Web.UI.Page

    Private logger As log4net.ILog = log4net.LogManager.GetLogger(Me.GetType())
    Private provider As NumberFormatInfo = New NumberFormatInfo
    Dim util As New Util
    Public ofertaDiaria As New OfertaDiaria

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load



        'objMenu = CType(Session("onsmenu"), OnsWebControls.OnsMenu)

        If Not Page.IsPostBack Then

            Try
                Dim semanaTemp As List(Of SemanaPMO)

                Dim dataInicio As DateTime
                Dim dataFim As DateTime

                semanaTemp = GetSemanaPMO(DateTime.Today, Nothing, Nothing)

                If semanaTemp.Count > 0 Then
                    dataInicio = semanaTemp(0).DataInicio
                    dataFim = semanaTemp(0).DataFim
                Else
                    Throw New Exception("Não foi encontrado registro de semana PMO para a data de hoje.")
                End If

                semanaTemp = GetSemanaPMO(dataFim.AddDays(1), Nothing, Nothing)

                If semanaTemp.Count > 0 Then
                    dataFim = semanaTemp(0).DataFim
                Else
                    Throw New Exception("Não foi encontrado registro da próxima semana PMO.")
                End If

                'pegar os dias aberta do primeiro dia da semana operativa corrente ate o ultimo dia da prox. semana operativa
                Dim diasAbertos As List(Of DateTime) = ListarDiasAbertos(dataInicio, dataFim)

                Dim itens As List(Of ListItem) = New List(Of ListItem)

                'For Each dia As DateTime In diasSemanaPMO
                For Each dia As DateTime In diasAbertos

                    If itens.Count = 0 Then
                        itens.Add(New ListItem("Selecione uma data.", CStr(-1)))
                    End If

                    itens.Add(New ListItem(CStr(dia)))
                Next

                cboData.Items.AddRange(itens.ToArray())

                PreencheComboEmpresaPOP(AgentesRepresentados, cboEmpresa, Session("strCodEmpre"))
                cboData_SelectedIndexChanged(sender, e)
            Catch ex As Exception
                RegistrarLogErro(ex)
            Finally
                
            End Try
        End If
    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        If SessaoAtiva(Page.Session) Then
            MyBase.Render(writer)
            'objMenu.RenderControl(writer)
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
    Protected Sub cboData_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboData.SelectedIndexChanged
        Try
            If cboData.SelectedIndex <> 0 Then
                Session("datEscolhida") = CDate(cboData.SelectedItem.Value)
            Else
                Session("datEscolhida") = Nothing
            End If

            cboEmpresa_SelectedIndexChanged(sender, e)
        Catch ex As Exception
            RegistrarLogErro(ex)
            Session("strMensagem") = "Não foi possível acessar a Base de Dados."
            Response.Redirect("frmMensagem.aspx")
        End Try
    End Sub

    Private Sub CarregarOfertaDiaria()

        ofertaDiaria = ConsultaOferta(cboData.SelectedItem.Value)

        Session("id_respdemanda") = ofertaDiaria.Id

        If ofertaDiaria.Id <> 0 Then
            Dim oferta As OfertaDiaria = ConsultaOfertaPorId(ofertaDiaria.Id)
            EdtUsina.Value = oferta.CodUsinaProgramacao
            EdtUsinaTempoReal.Value = oferta.CodUsinaTempoReal
            EdtVolume.Value = IIf(oferta.VolumeProgramacao Is Nothing, "", oferta.VolumeProgramacao)
            EtdPreco.Value = IIf(oferta.PrecoProgramacao Is Nothing, "", oferta.PrecoProgramacao)
            EdtVolumeTempoReal.Value = IIf(oferta.VolumeTempoReal Is Nothing, "", oferta.VolumeTempoReal)
            EtdPrecoTempoReal.Value = IIf(oferta.PrecoTempoReal Is Nothing, "", oferta.PrecoTempoReal)
            ChkDependentes.Checked = ofertaDiaria.Dependentes

            If (String.IsNullOrEmpty(oferta.CodUsinaProgramacao)) Then
                EdtVolume.Disabled = True
                EdtVolume.Value = ""
                EtdPreco.Value = ""
            End If

            If (String.IsNullOrEmpty(oferta.CodUsinaTempoReal)) Then
                EdtVolumeTempoReal.Disabled = True
                EdtVolumeTempoReal.Value = ""
                EtdPrecoTempoReal.Value = ""
            End If

        Else

            If (cboEmpresa.SelectedItem.Value = "") Then
                EdtUsina.Value = ""
                EdtUsinaTempoReal.Value = ""
                EdtVolume.Value = ""
                EtdPreco.Value = ""
                EdtVolumeTempoReal.Value = ""
                EtdPrecoTempoReal.Value = ""
                ChkDependentes.Checked = False
            Else
                Dim ofertaSugerida As OfertaDiaria = ConsultaOfertaSugerida(cboData.SelectedItem.Value)

                If (ofertaSugerida.IdOfertaSemanal <> 0) Then
                    EdtUsina.Value = ofertaSugerida.CodUsinaProgramacao
                    EdtUsinaTempoReal.Value = ofertaSugerida.CodUsinaTempoReal
                    EdtVolume.Value = IIf(ofertaSugerida.VolumeProgramacao Is Nothing, "", ofertaSugerida.VolumeProgramacao)
                    EtdPreco.Value = IIf(ofertaSugerida.PrecoProgramacao Is Nothing, "", ofertaSugerida.PrecoProgramacao)
                    EdtVolumeTempoReal.Value = IIf(ofertaSugerida.VolumeTempoReal Is Nothing, "", ofertaSugerida.VolumeTempoReal)
                    EtdPrecoTempoReal.Value = IIf(ofertaSugerida.PrecoTempoReal Is Nothing, "", ofertaSugerida.PrecoTempoReal)
                    ChkDependentes.Checked = ofertaSugerida.Dependentes

                    If (String.IsNullOrEmpty(ofertaSugerida.CodUsinaProgramacao)) Then
                        EdtVolume.Disabled = True
                        EdtVolume.Value = ""
                        EtdPreco.Value = ""
                    End If

                    If (String.IsNullOrEmpty(ofertaSugerida.CodUsinaTempoReal)) Then
                        EdtVolumeTempoReal.Disabled = True
                        EdtVolumeTempoReal.Value = ""
                        EtdPrecoTempoReal.Value = ""
                    End If

                Else
                    EdtUsina.Value = ""
                    EdtUsinaTempoReal.Value = ""
                    EdtVolume.Value = ""
                    EtdPreco.Value = ""
                    EdtVolumeTempoReal.Value = ""
                    EtdPrecoTempoReal.Value = ""
                    ChkDependentes.Checked = False
                End If

            End If

        End If

    End Sub

    Private Function ConsultaOfertaSugerida(dia As DateTime) As OfertaDiaria

        Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
        Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
        Cmd.Connection = Conn
        Conn.Open("pdp")


        Dim semanaTemp As List(Of SemanaPMO)

        semanaTemp = GetSemanaPMO(dia, Nothing, Nothing)

        Dim oferta As OfertaDiaria = New OfertaDiaria()

        Dim sql As StringBuilder = New StringBuilder()

        sql.Append(" SELECT u.codempre, r.datpdp, r.id_respdemanda, rs.id_respdemandasemanal, rs.cod_usinaprog, rs.cod_usinatr, rs.val_volumeprog, rs.val_volumetr, rs.val_cvuprog,rs.val_cvutr, rs.flg_dependente ")
        sql.Append(" FROM tb_respdemandasemanal rs ")
        sql.Append(" LEFT JOIN tb_respdemanda r on rs.id_respdemandasemanal = r.id_respdemandasemanal ")
        sql.Append(" LEFT JOIN usina u on u.codusina = rs.cod_usinaprog ")
        sql.Append(" WHERE u.codempre = '" & cboEmpresa.SelectedItem.Value & "' ")
        sql.Append(" and rs.id_semanapmo = '" & semanaTemp(0).IdSemanapmo.ToString & "' ")
        sql.Append(" union ")
        sql.Append(" SELECT u.codempre, r.datpdp, r.id_respdemanda, rs.id_respdemandasemanal, rs.cod_usinaprog, rs.cod_usinatr, rs.val_volumeprog, rs.val_volumetr, rs.val_cvuprog,rs.val_cvutr, rs.flg_dependente ")
        sql.Append(" FROM tb_respdemandasemanal rs ")
        sql.Append(" LEFT JOIN tb_respdemanda r on rs.id_respdemandasemanal = r.id_respdemandasemanal ")
        sql.Append(" LEFT JOIN usina u on u.codusina = rs.cod_usinatr ")
        sql.Append(" WHERE u.codempre = '" & cboEmpresa.SelectedItem.Value & "' ")
        sql.Append(" and rs.id_semanapmo = '" & semanaTemp(0).IdSemanapmo.ToString & "' ")

        Cmd.CommandText = sql.ToString()
        Dim drOfertas As OnsClasses.OnsData.OnsDataReader = Cmd.ExecuteReader()
        sql.Clear()

        If (drOfertas.Read()) Then

            oferta.IdOfertaSemanal = drOfertas.Item("id_respdemandasemanal")
            oferta.CodUsinaProgramacao = drOfertas.Item("cod_usinaprog").ToString()
            Try
                oferta.VolumeProgramacao = Convert.ToInt32(drOfertas.Item("val_volumeprog"))
            Catch
                oferta.VolumeProgramacao = Nothing
            End Try
            Try
                oferta.VolumeTempoReal = Convert.ToInt32(drOfertas.Item("val_volumetr"))
            Catch
                oferta.VolumeTempoReal = Nothing
            End Try
            Try
                oferta.PrecoProgramacao = drOfertas.Item("val_cvuprog")
            Catch
                oferta.PrecoProgramacao = Nothing
            End Try
            Try
                oferta.PrecoTempoReal = drOfertas.Item("val_cvutr")
            Catch
                oferta.PrecoTempoReal = Nothing
            End Try

            oferta.Dependentes = IIf(drOfertas.Item("flg_dependente") = "N", False, True)
            oferta.CodUsinaTempoReal = drOfertas.Item("cod_usinatr").ToString()

        End If

        If Conn.State = ConnectionState.Open Then
            Conn.Close()
        End If

        Return oferta

    End Function

    Protected Sub cboEmpresa_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboEmpresa.SelectedIndexChanged

        Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
        Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
        Cmd.Connection = Conn

        If Page.IsPostBack Then
            If cboEmpresa.SelectedIndex > 0 Then
                Session("strCodEmpre") = cboEmpresa.SelectedItem.Value
            Else
                Session("strCodEmpre") = 0
            End If

            Try
                Conn.Open("pdp")
                If (cboData.SelectedIndex > 0) Then

                    CarregarOfertaDiaria()

                Else
                    Response.Write("<SCRIPT>alert('Selecione a dataPDP!')</SCRIPT>")
                    cboEmpresa.SelectedIndex = 0
                End If

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

        End If
    End Sub

    Private Function ListarUsinasDeDemanda() As List(Of Usina)

        Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
        Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
        Cmd.Connection = Conn

        Try

            Cmd.CommandText = "select u.codusina, u.nomusina  from tb_respdemandasemanal r join usina u on u.codusina = r.cod_usinaprog   where u.codempre = '" & Session("strCodEmpre") & "'   and u.tpusina_id = 'UTD'  union select u.codusina, u.nomusina  from tb_respdemandasemanal r join usina u on u.codusina = r.cod_usinatr   where u.codempre = '" & Session("strCodEmpre") & "'   and u.tpusina_id = 'UTD'"

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
    Protected Sub btnSalvar_Click(sender As Object, e As ImageClickEventArgs) Handles BtnSalvar.Click

        Try

            SalvarProgramacaoSemanal()

        Catch ex As Exception
            RegistrarLogErro(ex)

        End Try

    End Sub

    Private Sub ValidarEntradas()
        'Dim objPermissao As OnsClasses.OnsInterfaceUnica.OnsPermissao
        'objPermissao = New OnsClasses.OnsInterfaceUnica.OnsPermissao("UPD", "PDPProgSemanal", UsuarID)

        If (cboData.SelectedValue.Equals("-1")) Or (cboEmpresa.SelectedValue.Equals("")) Then
            Throw New Exception("Selecione uma Data e Empresa.")
        End If
        Dim validarDia As DateTime = CDate(cboData.SelectedItem.Value).Date

        If Not EstaAutorizado Then
            Throw New Exception("Usuário não tem permissão para alterar os valores.")
        End If

        'prazo
        If PerfilID <> "ADM_PDPW" Then
            If (CDate(validarDia).ToString("yyyyMMdd1100") < DateTime.Now.ToString("yyyyMMddHHmm")) Then
                Throw New Exception("Fora do prazo para validação dos dados.")
            End If
        End If


        If (String.IsNullOrEmpty(EdtVolume.Value.ToString()) AndAlso String.IsNullOrEmpty(EdtVolumeTempoReal.Value.ToString())) Then
            Throw New Exception("Informe o Volume da programação ou tempo real.")
        Else
            If (EdtVolume.Value <> "") Then
                If (Convert.ToInt32(EdtVolume.Value) < 5) Then
                    Throw New Exception("O Volume da Programação precisa ser maior ou igual a 5.")
                End If
            End If
            If (EdtVolumeTempoReal.Value <> "") Then
                If (Convert.ToInt32(EdtVolumeTempoReal.Value) < 5) Then
                    Throw New Exception("O Volume de Tempo Real precisa ser maior ou igual a 5.")
                End If
            End If

        End If

    End Sub

    Private Sub SalvarProgramacaoSemanal()

        Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
        Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
        Dim transaction As OnsTransaction
        Cmd.Connection = Conn

        Dim pmo As SemanaPMO = GetSemanaPMO(DateTime.Today, Nothing, Nothing)(0)

        Try
            Conn.Open("pdp")
            transaction = Conn.BeginTransaction()
            Cmd.Transaction = transaction

            ValidarEntradas()

            Dim ofertaDiaria As OfertaDiaria = ConsultaOferta(cboData.SelectedItem.Value)

            If (ofertaDiaria.Id <> 0) Then
                If (Not ofertaDiaria.Id.Equals(Nothing)) Then
                    AtualizaOferta(Cmd, ofertaDiaria)
                End If
            Else
                ofertaDiaria = ConsultaOfertaPorId(0)
                If (ofertaDiaria.IdOfertaSemanal = 0) Then
                    Throw New Exception("Não existe programação semanal para associar.")
                Else
                    SalvaOferta(Cmd, ofertaDiaria)
                End If

            End If

            'Grava evento registrando o recebimento de Disponibilidade (DSP) 
            GravaEventoPDP("46", Convert.ToDateTime(cboData.SelectedItem.Value).ToString("yyyyMMdd"), cboEmpresa.SelectedItem.Text, DateTime.Now, "PDPProgDiaria", UsuarID)

            transaction.Commit()

            Response.Write("<SCRIPT>alert('Salvo com sucesso!')</SCRIPT>")

        Catch ex As Exception
            RegistrarLogErro(ex)
            transaction.Rollback()
            Session("strMensagem") = "" & ex.Message
            Response.Redirect("frmMensagem.aspx")

        Finally
            If Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If

            EdtVolume.Value = ""
            EtdPreco.Value = ""
            EdtVolumeTempoReal.Value = ""
            EtdPrecoTempoReal.Value = ""
            EdtUsina.Value = ""
            EdtUsinaTempoReal.Value = ""
            ChkDependentes.Checked = False
            cboData.SelectedIndex = 0
            cboEmpresa.SelectedIndex = 0

        End Try
    End Sub

    Private Sub SalvaOferta(cmd As OnsCommand, ofertaDaSemana As OfertaDiaria)

        Dim sql As StringBuilder = New StringBuilder()

        sql.Clear()

        Dim pdp As String = cboData.SelectedItem.Value
        pdp = pdp.Substring(6, 4) & pdp.Substring(3, 2) & pdp.Substring(0, 2)

        If (String.IsNullOrEmpty(EdtVolumeTempoReal.Value)) Then
            ofertaDaSemana.VolumeTempoReal = Nothing
        Else
            ofertaDaSemana.VolumeTempoReal = Integer.Parse(EdtVolumeTempoReal.Value)
        End If

        If (String.IsNullOrEmpty(EdtVolume.Value)) Then
            ofertaDaSemana.VolumeProgramacao = Nothing
        Else
            ofertaDaSemana.VolumeProgramacao = Integer.Parse(EdtVolume.Value)
        End If

        If (String.IsNullOrEmpty(ofertaDaSemana.CodUsinaProgramacao)) Then
            ofertaDaSemana.CodUsinaProgramacao = "null"
        Else
            ofertaDaSemana.CodUsinaProgramacao = "'" & ofertaDaSemana.CodUsinaProgramacao & "'"
        End If

        If (String.IsNullOrEmpty(ofertaDaSemana.CodUsinaTempoReal)) Then
            ofertaDaSemana.CodUsinaTempoReal = "null"
        Else
            ofertaDaSemana.CodUsinaTempoReal = "'" & ofertaDaSemana.CodUsinaTempoReal & "'"
        End If

        sql.Append(" insert into tb_respdemanda(    ")
        sql.Append(" datpdp,  ")
        sql.Append(" cod_usinatr, ")
        sql.Append(" cod_usinaprog, ")
        sql.Append(" val_volumetr,  ")
        sql.Append(" val_volumeprog, ")
        sql.Append(" id_respdemandasemanal) ")
        sql.Append(" values('" & pdp & "', " & ofertaDaSemana.CodUsinaTempoReal & ", " & ofertaDaSemana.CodUsinaProgramacao & ", " & IIf(ofertaDaSemana.VolumeTempoReal Is Nothing, "null", ofertaDaSemana.VolumeTempoReal) & ", " & IIf(ofertaDaSemana.VolumeProgramacao Is Nothing, "null", ofertaDaSemana.VolumeProgramacao) & ", " & ofertaDaSemana.IdOfertaSemanal & ") ;")

        If (Not String.IsNullOrEmpty(ofertaDaSemana.CodUsinaProgramacao) AndAlso ofertaDaSemana.CodUsinaProgramacao <> "null") Then

            Dim I As Integer

            sql.AppendLine("delete from disponibilidade where datpdp = '" & pdp & "' and codusina = " & ofertaDaSemana.CodUsinaProgramacao & ";")

            For I = 1 To 48

                sql.AppendLine("insert into disponibilidade(datpdp, codusina, intdsp, valdsptran)")
                sql.AppendLine("values('" & pdp & "', " & ofertaDaSemana.CodUsinaProgramacao & ", " & I & ", " & CInt(Trim(EdtVolume.Value)) & ");")

                If I Then

                End If

            Next

        End If

        cmd.CommandText = sql.ToString()
        cmd.CommandType = CommandType.Text
        cmd.ExecuteNonQuery()

    End Sub

    Private Function VerificaOfertaExistente(id As Integer) As Boolean

        Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
        Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
        Cmd.Connection = Conn
        Conn.Open("pdp")

        Try

            Dim sql As StringBuilder = New StringBuilder()

            Dim pdp As String = cboData.SelectedItem.Value
            pdp = pdp.Substring(6, 4) & pdp.Substring(3, 2) & pdp.Substring(0, 2)

            sql.Append(" select 1 from tb_respdemanda where datpdp = '" & pdp & "' and id_respdemandasemanal = " & id)

            Cmd.CommandText = sql.ToString()

            Dim drOfertas As OnsClasses.OnsData.OnsDataReader = Cmd.ExecuteReader

            If (drOfertas.Read) Then
                Return True
            Else
                Return False
            End If

        Catch ex As Exception
            Throw
        Finally
            If Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If
        End Try

    End Function

    Private Sub AtualizaOferta(cmd As OnsCommand, ofertas As OfertaDiaria)

        Dim sql As StringBuilder = New StringBuilder()

        sql.Clear()

        Dim pdp As String = cboData.SelectedItem.Value
        pdp = pdp.Substring(6, 4) & pdp.Substring(3, 2) & pdp.Substring(0, 2)

        If (Not String.IsNullOrEmpty(ofertas.CodUsinaProgramacao)) Then

            Dim volume As Integer?
            Try
                volume = CInt(Trim(EdtVolume.Value))
            Catch
                volume = Nothing
            End Try

            sql.Append(" UPDATE tb_respdemanda ")
            sql.Append("   SET ")
            sql.Append("    val_volumeprog = " & IIf(volume Is Nothing, "null", volume) & " ")
            sql.Append(" WHERE ")
            sql.Append(" id_respdemanda = " & ofertas.Id & "; ")

            Dim I As Integer

            sql.AppendLine("delete from disponibilidade where datpdp = '" & pdp & "' and codusina = '" & ofertas.CodUsinaProgramacao & "';")

            For I = 1 To 48

                sql.AppendLine("insert into disponibilidade(datpdp, codusina, intdsp, valdsptran)")
                sql.AppendLine("values('" & pdp & "', '" & ofertas.CodUsinaProgramacao & "', " & I & ", " & CInt(Trim(EdtVolume.Value)) & ");")

                If I Then

                End If

            Next


        End If

        If (Not String.IsNullOrEmpty(ofertas.CodUsinaTempoReal)) Then

            Dim volume As Integer?
            Try
                volume = CInt(Trim(EdtVolumeTempoReal.Value))
            Catch
                volume = Nothing
            End Try

            sql.Append(" UPDATE tb_respdemanda ")
            sql.Append("   SET ")
            sql.Append("    val_volumetr = " & IIf(volume Is Nothing, "null", volume) & " ")
            sql.Append(" WHERE ")
            sql.Append(" id_respdemanda = " & ofertas.Id & "; ")
        End If


        cmd.CommandText = sql.ToString()
        cmd.CommandType = CommandType.Text
        cmd.ExecuteNonQuery()

    End Sub

    Private Function ConsultaOferta(dia As DateTime) As OfertaDiaria

        Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
        Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
        Cmd.Connection = Conn
        Conn.Open("pdp")

        Dim oferta As OfertaDiaria = New OfertaDiaria()

        Dim sql As StringBuilder = New StringBuilder()

        sql.Append(" SELECT r.datpdp, r.id_respdemanda, r.id_respdemandasemanal, r.cod_usinaprog, r.cod_usinatr, r.val_volumeprog,r.val_volumetr, rs.val_cvuprog,rs.val_cvutr, rs.flg_dependente ")
        sql.Append("  FROM tb_respdemanda r")
        sql.Append(" INNER JOIN tb_respdemandasemanal rs on rs.id_respdemandasemanal = r.id_respdemandasemanal")
        sql.Append(" INNER JOIN usina u on u.codusina = r.cod_usinaprog  ")
        sql.Append(" WHERE r.datpdp = '" & dia.ToString("yyyyMMdd") & "'")
        sql.Append("   and u.codempre = '" & cboEmpresa.SelectedItem.Value & "'")
        sql.Append("  union ")
        sql.Append(" SELECT r.datpdp, r.id_respdemanda, r.id_respdemandasemanal, r.cod_usinaprog, r.cod_usinatr, r.val_volumeprog,r.val_volumetr, rs.val_cvuprog,rs.val_cvutr, rs.flg_dependente ")
        sql.Append("  FROM tb_respdemanda r")
        sql.Append(" INNER JOIN tb_respdemandasemanal rs on rs.id_respdemandasemanal = r.id_respdemandasemanal")
        sql.Append(" INNER JOIN usina u on u.codusina = r.cod_usinatr  ")
        sql.Append(" WHERE r.datpdp = '" & dia.ToString("yyyyMMdd") & "'")
        sql.Append("   and u.codempre = '" & cboEmpresa.SelectedItem.Value & "'")

        Cmd.CommandText = sql.ToString()
        Dim drOfertas As OnsClasses.OnsData.OnsDataReader = Cmd.ExecuteReader()
        sql.Clear()

        If (drOfertas.Read()) Then

            oferta.DataPDP = drOfertas.Item("datpdp")
            oferta.Id = drOfertas.Item("id_respdemanda")
            oferta.IdOfertaSemanal = drOfertas.Item("id_respdemandasemanal")
            oferta.CodUsinaProgramacao = drOfertas.Item("cod_usinaprog").ToString()
            oferta.CodUsinaTempoReal = drOfertas.Item("cod_usinatr").ToString()

            Try
                oferta.VolumeProgramacao = Convert.ToInt32(drOfertas.Item("val_volumeprog"))
            Catch
                oferta.VolumeProgramacao = Nothing
            End Try
            Try
                oferta.VolumeTempoReal = Convert.ToInt32(drOfertas.Item("val_volumetr"))
            Catch
                oferta.VolumeTempoReal = Nothing
            End Try
            Try
                oferta.PrecoProgramacao = drOfertas.Item("val_cvuprog")
            Catch
                oferta.PrecoProgramacao = Nothing
            End Try
            Try
                oferta.PrecoTempoReal = drOfertas.Item("val_cvutr")
            Catch
                oferta.PrecoTempoReal = Nothing
            End Try

            oferta.Dependentes = IIf(drOfertas.Item("flg_dependente") = "N", False, True)

        End If

        If Conn.State = ConnectionState.Open Then
            Conn.Close()
        End If

        Return oferta

    End Function

    Private Function ConsultaOferta(dia As DateTime, codUsinaProg As String) As OfertaDiaria

        Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
        Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
        Cmd.Connection = Conn
        Conn.Open("pdp")

        Dim oferta As OfertaDiaria = New OfertaDiaria()

        Dim sql As StringBuilder = New StringBuilder()

        sql.Append(" SELECT r.datpdp, r.id_respdemanda, r.id_respdemandasemanal, r.cod_usinaprog, r.val_volumeprog, rs.val_cvuprog, rs.flg_dependente ")
        sql.Append("  FROM tb_respdemanda r ")
        sql.Append("  left join tb_respdemandasemanal rs on rs.id_respdemandasemanal = r.id_respdemandasemanal ")
        sql.Append(" WHERE r.datpdp = '" & dia.ToString("yyyyMMdd") & "'")
        sql.Append("   and r.cod_usinaprog = '" & codUsinaProg & "';")

        Cmd.CommandText = sql.ToString()
        Dim drOfertas As OnsClasses.OnsData.OnsDataReader = Cmd.ExecuteReader()
        sql.Clear()

        If (drOfertas.Read()) Then

            oferta.DataPDP = drOfertas.Item("datpdp")
            oferta.Id = drOfertas.Item("id_respdemanda")
            oferta.IdOfertaSemanal = drOfertas.Item("id_respdemandasemanal")
            oferta.CodUsinaProgramacao = drOfertas.Item("cod_usinaprog")


            Try
                oferta.VolumeProgramacao = Convert.ToInt32(drOfertas.Item("val_volumeprog"))
            Catch
                oferta.VolumeProgramacao = Nothing
            End Try
            Try
                oferta.PrecoProgramacao = drOfertas.Item("val_cvuprog")
            Catch
                oferta.PrecoProgramacao = Nothing
            End Try

            oferta.Dependentes = IIf(drOfertas.Item("flg_dependente").ToString() = "N", False, True)

        End If

        If Conn.State = ConnectionState.Open Then
            Conn.Close()
        End If

        Return oferta

    End Function

    Private Function ConsultaOfertaPorId(id As Integer) As OfertaDiaria

        Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
        Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
        Cmd.Connection = Conn
        Conn.Open("pdp")

        Dim oferta As OfertaDiaria = New OfertaDiaria()

        Dim sql As StringBuilder = New StringBuilder()

        Dim semanaTemp As List(Of SemanaPMO)
        semanaTemp = GetSemanaPMO(cboData.SelectedItem.Value, Nothing, Nothing)



        If id <> 0 Then
            sql.Append(" select r.datpdp, r.id_respdemanda, r.id_respdemandasemanal, r.cod_usinaprog, r.cod_usinatr, r.val_volumeprog,r.val_volumetr, rs.val_cvuprog,rs.val_cvutr, rs.flg_dependente ")
            sql.Append("  FROM tb_respdemanda r")
            sql.Append(" INNER JOIN tb_respdemandasemanal rs on rs.id_respdemandasemanal = r.id_respdemandasemanal")
            sql.Append(" WHERE r.id_respdemanda = '" & id & "';")
        Else
            sql.Append("SELECT '" & cboData.SelectedItem.Value & "' as datpdp, 0 as id_respdemanda, r.id_respdemandasemanal, r.cod_usinaprog, r.cod_usinatr, r.val_volumeprog,r.val_volumetr, r.val_cvuprog,r.val_cvutr, r.flg_dependente ")
            sql.Append("  FROM tb_respdemandasemanal r")
            sql.Append(" WHERE (r.cod_usinatr = '" & EdtUsinaTempoReal.Value & "' or r.cod_usinatr is null) and (r.cod_usinaprog = '" & EdtUsina.Value & "' or r.cod_usinaprog is null)")
            sql.Append(" and r.id_semanapmo = '" & semanaTemp(0).IdSemanapmo.ToString & "';")
        End If

        Cmd.CommandText = sql.ToString()
        Dim drOfertas As OnsClasses.OnsData.OnsDataReader = Cmd.ExecuteReader()
        sql.Clear()

        If (drOfertas.Read()) Then

            oferta.DataPDP = drOfertas.Item("datpdp")
            oferta.Id = drOfertas.Item("id_respdemanda")
            oferta.IdOfertaSemanal = drOfertas.Item("id_respdemandasemanal")
            oferta.CodUsinaProgramacao = drOfertas.Item("cod_usinaprog").ToString()
            'oferta.VolumeProgramacao = drOfertas.Item("val_volumeprog")
            oferta.CodUsinaTempoReal = drOfertas.Item("cod_usinatr").ToString()
            'oferta.VolumeTempoReal = drOfertas.Item("val_volumetr")
            oferta.Dependentes = IIf(drOfertas.Item("flg_dependente") = "N", False, True)

            Try
                oferta.VolumeProgramacao = Convert.ToInt32(drOfertas.Item("val_volumeprog"))
            Catch
                oferta.VolumeProgramacao = Nothing
            End Try
            Try
                oferta.VolumeTempoReal = Convert.ToInt32(drOfertas.Item("val_volumetr"))
            Catch
                oferta.VolumeTempoReal = Nothing
            End Try
            Try
                oferta.PrecoProgramacao = drOfertas.Item("val_cvuprog")
            Catch
                oferta.PrecoProgramacao = Nothing
            End Try
            Try
                oferta.PrecoTempoReal = drOfertas.Item("val_cvutr")
            Catch
                oferta.PrecoTempoReal = Nothing
            End Try


        End If

        If Conn.State = ConnectionState.Open Then
            Conn.Close()
        End If

        Return oferta

    End Function

End Class