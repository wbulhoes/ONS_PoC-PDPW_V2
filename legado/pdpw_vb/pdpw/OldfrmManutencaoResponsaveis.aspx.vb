Public Class frmManutencaoResponsaveis
    Inherits System.Web.UI.Page
    Dim indice_inicial As Integer = 0         ' Índice inicial para paginação.
    Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
    Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Put user code to initialize the page here
        'objMenu = CType(Session("onsmenu"), OnsWebControls.OnsMenu)
        Try
            If Not Page.IsPostBack Then
                CarregaComboData()

                indice_inicial = 0
                dtgResponsaveis.CurrentPageIndex = 0
                DataGridBind()

                cboEquipe.Enabled = False
                cboUsuario.Enabled = False
                optTipoOperacao.Enabled = False
                txtDataInicio.Enabled = False
                txtDataInicio.Enabled = False
                txtDataFim.Enabled = False
                txtDataFim.Enabled = False

                btnSalvar.Visible = False
                btnCancelar.Visible = False

                hiddenAcao.Value = ""

                CarregaComboEquipe()
            End If
        Catch ex As Exception
            Session("strMensagem") = "Erro ao consultar. (" + ex.Message + ")"
            Response.Redirect("frmMensagem.aspx")
        End Try
    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        If SessaoAtiva(Page.Session) Then
            MyBase.Render(writer)
            'objMenu.RenderControl(writer)
        End If
    End Sub

    Private Sub DataGridBind()
        Dim daResponsavel As OnsClasses.OnsData.OnsDataAdapter
        Dim dsResponsavel As DataSet
        Cmd.Connection = Conn
        Try
            Conn.Open("pdp")
            Cmd.CommandText = "SELECT tb_responsavelprogpdp.id_responsavelprogpdp, tb_responsavelprogpdp.datpdp, " & _
                              "tb_responsavelprogpdp.id_usuarequipepdp, tb_responsavelprogpdp.tip_programacao, " & _
                              "usuar.usuar_nome, tb_equipepdp.nom_equipepdp, " & _
                              "tb_responsavelprogpdp.din_inicioprogpdp, tb_responsavelprogpdp.din_fimprogpdp, " & _
                              "CASE WHEN tb_responsavelprogpdp.tip_programacao = 'L' THEN" & _
                                            "'Elétrica' " & _
                                            "ELSE " & _
                                            "'Energética' END AS tip_programacaoDescricao " & _
                              "FROM tb_responsavelprogpdp, tb_usuarequipepdp, usuar, tb_equipepdp " & _
                              "WHERE datpdp = '" & cboData.SelectedItem.Value & "' " & _
                              "and tb_responsavelprogpdp.id_usuarequipepdp = tb_usuarequipepdp.id_usuarequipepdp " & _
                              "and tb_usuarequipepdp.usuar_id = usuar.usuar_id " & _
                              "and tb_usuarequipepdp.id_equipepdp = tb_equipepdp.id_equipepdp " & _
                              "ORDER BY id_responsavelprogpdp"

            daResponsavel = New OnsClasses.OnsData.OnsDataAdapter(Cmd.CommandText, Conn)
            dsResponsavel = New DataSet
            daResponsavel.Fill(dsResponsavel, "Responsavel")
            dtgResponsaveis.DataSource = dsResponsavel.Tables("Responsavel").DefaultView
            dtgResponsaveis.DataBind()
        Catch ex As Exception
            If Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If
            Session("strMensagem") = "Erro ao consultar. (" + ex.Message + ")"
            Response.Redirect("frmMensagem.aspx")
        Finally
            If Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If
        End Try
    End Sub

    Protected Sub btnIncluir_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnIncluir.Click
        hiddenAcao.Value = "I"

        cboEquipe.Enabled = True
        cboUsuario.Enabled = True
        optTipoOperacao.Enabled = True
        txtDataInicio.Enabled = True
        txtDataInicio.Enabled = True
        txtDataFim.Enabled = True
        txtDataFim.Enabled = True

        btnSalvar.Visible = True
        btnCancelar.Visible = True

        If cboUsuario.Items.Count > 0 Then
            cboUsuario.SelectedIndex = 0
        End If
        If cboUsuario.Items.Count > 0 Then
            cboUsuario.SelectedIndex = 0
        End If
        optTipoOperacao.SelectedIndex = -1

        txtDataInicio.Text = DateTime.Now.ToString("dd/MM/yyyy")
        txtHoraInicio.Text = DateTime.Now.ToString("HH:mm")

        txtDataFim.Text = ""
        txtHoraFim.Text = ""

        Page.ClientScript.RegisterStartupScript(Me.GetType(), "Foco", "document.forms[0]['cboEquipe'].focus();", True)
    End Sub

    Protected Sub btnAlterar_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnAlterar.Click
        '    hiddenAcao.Value = "A"

        '    Dim item As DataGridItem
        '    For Each item In dtgResponsaveis.Items
        '        If (CType(item.FindControl("chkMarca"), CheckBox)).Checked Then
        '            txtCodigo.Text = item.Cells(1).Text
        '            txtNome.Text = item.Cells(2).Text
        '            Exit For
        '        End If
        '    Next

        '    If (txtNome.Text = "") Then
        '        Response.Write("<SCRIPT>alert('Selecione um item.')</SCRIPT>")
        '        Exit Sub
        '    Else
        '        txtNome.Enabled = True
        '        btnSalvar.Visible = True
        '        btnCancelar.Visible = True

        '        Page.ClientScript.RegisterStartupScript(Me.GetType(), "Foco", "document.forms[0]['txtNome'].focus();", True)
        '    End If

    End Sub

    Protected Sub btnExcluir_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnExcluir.Click
        Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
        Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
        Cmd.Connection = Conn

        Dim lstExclui As String = ""

        Dim item As DataGridItem
        For Each item In dtgResponsaveis.Items

            If (CType(item.FindControl("chkMarca"), CheckBox)).Checked Then
                If lstExclui.Length > 0 Then
                    lstExclui += ","
                End If
                lstExclui += (CType(item.FindControl("lblObjId"), Label)).Text
            End If
        Next

        '
        Try
            Conn.Open("pdp")

            If lstExclui = "" Then
                Response.Write("<SCRIPT>alert('Selecione um item.')</SCRIPT>")
                Exit Sub
            Else
                Cmd.CommandText = "Delete From tb_equipepdp Where id_equipepdp in (" & lstExclui & ")"
                Cmd.ExecuteNonQuery()
            End If

            cboEquipe.Enabled = False
            cboUsuario.Enabled = False
            optTipoOperacao.Enabled = False
            txtDataInicio.Enabled = False
            txtDataInicio.Enabled = False
            txtDataFim.Enabled = False
            txtDataFim.Enabled = False

            btnSalvar.Visible = False
            btnCancelar.Visible = False

            cboEquipe.SelectedIndex = 0
            cboUsuario.SelectedIndex = 0
            optTipoOperacao.SelectedIndex = -1
            txtDataInicio.Text = ""
            txtHoraInicio.Text = ""
            txtDataFim.Text = ""
            txtHoraFim.Text = ""

            hiddenAcao.Value = ""

            dtgResponsaveis.CurrentPageIndex = 0

        Catch ex As Exception
            Session("strMensagem") = "Erro ao excluir. (" + ex.Message + ")"
            If Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If
            Response.Redirect("frmMensagem.aspx")
        Finally
            If Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If
        End Try

        DataGridBind()

    End Sub

    Protected Sub btnSalvar_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSalvar.Click

        If cboData.SelectedIndex = 0 Then
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "Foco", "document.forms[0]['cboData'].focus();alert('Selecione a Data PDP')", True)
            Exit Sub
        End If

        If cboEquipe.SelectedIndex = 0 Then
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "Foco", "document.forms[0]['cboEquipe'].focus();alert('Selecione a Equipe')", True)
            Exit Sub
        End If

        If cboUsuario.SelectedIndex = 0 Then
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "Foco", "document.forms[0]['cboUsuario'].focus();alert('Selecione o Usuário Responsável')", True)
            Exit Sub
        End If

        If optTipoOperacao.SelectedIndex = -1 Then
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "Foco", "document.forms[0]['optTipoOperacao_0'].focus();alert('Selecione o Tipo de Programação')", True)
            Exit Sub
        End If

        If (txtDataInicio.Text = "" Or txtHoraInicio.Text = "") Then
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "Foco", "document.forms[0]['txtDataInicio'].focus();alert('Informe Data Hora Inicio Programação')", True)
            Exit Sub
        End If

        Dim strDataHoraInicio As String = txtDataInicio.Text & " " & txtHoraInicio.Text
        If Not IsDate(strDataHoraInicio) Then
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "Foco", "document.forms[0]['txtDataInicio'].focus();alert('Data Hora Início Inválida')", True)
            Exit Sub
        End If

        Dim strDataHoraFim As String = ""
        If (txtDataFim.Text = "" Xor txtHoraFim.Text = "") Then
            strDataHoraFim = txtDataFim.Text & " " & txtHoraFim.Text
            If Not IsDate(strDataHoraFim) Then
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "Foco", "document.forms[0]['txtDataFim'].focus();alert('Data Hora Fim Inválida')", True)
                Exit Sub
            End If
        End If

        If (VerificaExistenciaAssociacao()) Then
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "Foco", "document.forms[0]['cboUsuario'].focus();alert('Usuário/TipoProgramação já associados a Data PDP informada')", True)
            Exit Sub
        End If

        Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
        Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
        Cmd.Connection = Conn
        Try
            Conn.Open("pdp")

            Dim dataHoraInicio As DateTime = Convert.ToDateTime(strDataHoraInicio)

            Dim dataHoraFim As New DateTime
            If (strDataHoraFim <> "") Then
                dataHoraFim = Convert.ToDateTime(strDataHoraFim)
            End If

            If hiddenAcao.Value = "I" Then

                Cmd.CommandText = "INSERT INTO tb_responsavelprogpdp " & _
                 "(datpdp, id_usuarequipepdp, tip_programacao, din_inicioprogpdp, din_fimprogpdp) VALUES " & _
                 "('" & cboData.SelectedValue & "', '" & cboUsuario.SelectedValue & "', '" & _
                 optTipoOperacao.SelectedValue & "', '" & _
                 dataHoraInicio.ToString("yyyy-MM-dd HH:mm") & "', " & _
                 IIf(strDataHoraFim = "", "null", "'" & dataHoraFim.ToString("yyyy-MM-dd HH:mm") & "'") & ")"

            Else

                'Cmd.CommandText = ""

            End If

            Cmd.ExecuteNonQuery()
            Conn.Close()

            cboEquipe.SelectedIndex = 0
            cboUsuario.SelectedIndex = 0
            optTipoOperacao.SelectedIndex = -1

            txtDataInicio.Text = DateTime.Now.ToString("dd/MM/yyyy")
            txtHoraInicio.Text = DateTime.Now.ToString("HH:mm")
            txtDataFim.Text = ""
            txtHoraFim.Text = ""


        Catch ex As Exception
            Conn.Close()

            'If ex.Message.ToUpper.IndexOf("UNIQUE CONSTRAINT") > 0 Then
            'Session("strMensagem") = "Empresa já cadastrada para o dia " + txtDataPDP.Text
            'Else
            Session("strMensagem") = "Erro ao salvar. (" + ex.Message + ")"
            'End If

            Response.Redirect("frmMensagem.aspx")
        End Try

        DataGridBind()
    End Sub

    Protected Sub btnCancelar_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnCancelar.Click
        cboEquipe.Enabled = False
        cboUsuario.Enabled = False
        optTipoOperacao.Enabled = False
        txtDataInicio.Enabled = False
        txtDataInicio.Enabled = False
        txtDataFim.Enabled = False
        txtDataFim.Enabled = False

        cboEquipe.SelectedIndex = 0
        cboUsuario.SelectedIndex = 0
        optTipoOperacao.SelectedIndex = -1
        txtDataInicio.Text = ""
        txtHoraInicio.Text = ""
        txtDataFim.Text = ""
        txtHoraFim.Text = ""

        hiddenAcao.Value = ""
    End Sub


    Private Sub CarregaComboData()
        Dim intI As Integer
        Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
        Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand

        Cmd.Connection = Conn

        Try
            cboData.Items.Clear()
            Conn.Open("pdp")

            Cmd.CommandText = "Select datpdp From pdp Order By datpdp Desc"

            Dim rsData As OnsClasses.OnsData.OnsDataReader = Cmd.ExecuteReader

            intI = 1
            Dim objItem As WebControls.ListItem
            objItem = New WebControls.ListItem
            objItem.Text = ""
            objItem.Value = "0"
            cboData.Items.Add(objItem)

            Do While rsData.Read
                objItem = New System.Web.UI.WebControls.ListItem
                objItem.Text = Mid(rsData("datpdp"), 7, 2) & "/" & Mid(rsData("datpdp"), 5, 2) & "/" & Mid(rsData("datpdp"), 1, 4)
                objItem.Value = rsData("datpdp").ToString()
                cboData.Items.Add(objItem)
            Loop

            rsData.Close()
            rsData = Nothing
            Cmd.Connection.Close()
            Conn.Close()
        Catch
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

    End Sub


    Protected Sub cboData_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboData.SelectedIndexChanged
        DataGridBind()
    End Sub

    Protected Sub dtgResponsaveis_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgResponsaveis.PageIndexChanged
        dtgResponsaveis.CurrentPageIndex = e.NewPageIndex
        DataGridBind()
    End Sub

    Protected Sub dtgResponsaveis_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dtgResponsaveis.ItemCommand
        If e.CommandName = "Delete" Then

            Dim id As String = (CType(e.Item.FindControl("lblObjId"), Label)).Text

            Dim dataFinal As String = (CType(e.Item.FindControl("lblDataFim"), Label)).Text

            If dataFinal = "" Then

                Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
                Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
                Cmd.Connection = Conn
                Try
                    Conn.Open("pdp")

                    Cmd.CommandText = "update tb_responsavelprogpdp set din_fimprogpdp = '" & DateTime.Now.ToString("yyyy-MM-dd HH:mm") & "' " & _
                                            "where id_responsavelprogpdp = " & id

                    Cmd.ExecuteNonQuery()
                    Conn.Close()

                Catch ex As Exception
                    Conn.Close()

                    Session("strMensagem") = "Erro ao salvar. (" + ex.Message + ")"
                    Response.Redirect("frmMensagem.aspx")
                End Try

                DataGridBind()
            Else
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "Foco", "alert('Responsável já excluído')", True)
            End If

        End If

    End Sub

    Private Sub CarregaComboEquipe()
        Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
        Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand

        Cmd.Connection = Conn

        Try
            cboEquipe.Items.Clear()
            Conn.Open("pdp")

            Cmd.CommandText = "SELECT id_equipepdp, UPPER(nom_equipepdp) as nom_equipepdp " & _
                        "FROM tb_equipepdp " & _
                        "ORDER BY id_equipepdp"

            Dim rsData As OnsClasses.OnsData.OnsDataReader = Cmd.ExecuteReader

            Dim objItem As WebControls.ListItem
            objItem = New WebControls.ListItem
            objItem.Text = ""
            objItem.Value = "0"
            cboEquipe.Items.Add(objItem)

            Do While rsData.Read
                objItem = New System.Web.UI.WebControls.ListItem
                objItem.Text = rsData("nom_equipepdp").ToString()
                objItem.Value = rsData("id_equipepdp").ToString()
                cboEquipe.Items.Add(objItem)
            Loop

            rsData.Close()
            rsData = Nothing
            Cmd.Connection.Close()
            Conn.Close()
        Catch
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

    End Sub

    Private Sub CarregaComboUsuarioPDP()
        Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
        Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand

        Cmd.Connection = Conn

        Try
            cboUsuario.Items.Clear()
            Conn.Open("pdp")

            Cmd.CommandText = "SELECT tb_usuarequipepdp.id_usuarequipepdp, UPPER(usuar.usuar_nome) as usuar_nome " & _
                        "FROM tb_equipepdp, tb_usuarequipepdp, usuar " & _
                        "WHERE tb_equipepdp.id_equipepdp = tb_usuarequipepdp.id_equipepdp " & _
                        "and usuar.usuar_id = tb_usuarequipepdp.usuar_id " & _
                        "and tb_equipepdp.id_equipepdp = " & cboEquipe.SelectedItem.Value & " " & _
                        "ORDER BY usuar.usuar_nome"

            Dim rsData As OnsClasses.OnsData.OnsDataReader = Cmd.ExecuteReader

            Dim objItem As WebControls.ListItem
            objItem = New WebControls.ListItem
            objItem.Text = ""
            objItem.Value = "0"
            cboUsuario.Items.Add(objItem)

            Do While rsData.Read
                objItem = New System.Web.UI.WebControls.ListItem
                objItem.Text = rsData("usuar_nome").ToString()
                objItem.Value = rsData("id_usuarequipepdp").ToString()
                cboUsuario.Items.Add(objItem)
            Loop

            rsData.Close()
            rsData = Nothing
            Cmd.Connection.Close()
            Conn.Close()
        Catch
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

    End Sub

    Protected Sub cboEquipe_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboEquipe.SelectedIndexChanged
        CarregaComboUsuarioPDP()
        Page.ClientScript.RegisterStartupScript(Me.GetType(), "Foco", "document.forms[0]['cboUsuario'].focus()", True)
    End Sub

    Private Function VerificaExistenciaAssociacao() As Boolean
        Dim daResp As OnsClasses.OnsData.OnsDataAdapter
        Dim dsResp As DataSet
        Cmd.Connection = Conn
        Try
            Conn.Open("pdp")
            Cmd.CommandText = "SELECT id_responsavelprogpdp, datpdp, id_usuarequipepdp, tip_programacao " & _
                              "FROM tb_responsavelprogpdp " & _
                              "WHERE datpdp = '" & cboData.SelectedItem.Value & "' " & _
                              "and id_usuarequipepdp = '" & cboUsuario.SelectedItem.Value & "' " & _
                              "and tip_programacao = '" & optTipoOperacao.SelectedValue & "'"

            daResp = New OnsClasses.OnsData.OnsDataAdapter(Cmd.CommandText, Conn)
            dsResp = New DataSet
            daResp.Fill(dsResp, "Resp")

            VerificaExistenciaAssociacao = dsResp.Tables("Resp").Rows.Count > 0

        Catch ex As Exception
            If Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If
            Session("strMensagem") = "Erro ao consultar. (" + ex.Message + ")"
            Response.Redirect("frmMensagem.aspx")
        Finally
            If Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If
        End Try
    End Function

End Class