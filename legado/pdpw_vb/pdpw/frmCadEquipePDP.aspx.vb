Public Class frmCadEquipePDP
    Inherits System.Web.UI.Page
    Dim indice_inicial As Integer = 0         ' Índice inicial para paginação.
    Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
    Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Put user code to initialize the page here
        objMenu = CType(Session("onsmenu"), OnsWebControls.OnsMenu)
        Try
            If Not Page.IsPostBack Then
                indice_inicial = 0
                dtgEquipePDP.CurrentPageIndex = 0
                DataGridBind()

                txtNome.Enabled = False
                btnSalvar.Visible = False
                btnCancelar.Visible = False

                hiddenAcao.Value = ""
            End If
        Catch ex As Exception
            Session("strMensagem") = "Erro ao consultar. (" + ex.Message + ")"
            Response.Redirect("frmMensagem.aspx")
        End Try
    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        If SessaoAtiva(Page.Session) Then
            MyBase.Render(writer)
            objMenu.RenderControl(writer)
        End If
    End Sub

    Protected Sub dtgEquipePDP_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgEquipePDP.PageIndexChanged
        dtgEquipePDP.CurrentPageIndex = e.NewPageIndex
        DataGridBind()
    End Sub

    Private Sub DataGridBind()
        Dim daEquipe As OnsClasses.OnsData.OnsDataAdapter
        Dim dsEquipe As DataSet
        Cmd.Connection = Conn
        Try
            Conn.Open("pdp")
            Cmd.CommandText = "SELECT id_equipepdp, nom_equipepdp " & _
                              "FROM tb_equipepdp " & _
                              "ORDER BY id_equipepdp"
            daEquipe = New OnsClasses.OnsData.OnsDataAdapter(Cmd.CommandText, Conn)
            dsEquipe = New DataSet
            daEquipe.Fill(dsEquipe, "Equipe")
            dtgEquipePDP.DataSource = dsEquipe.Tables("Equipe").DefaultView
            dtgEquipePDP.DataBind()
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

        txtNome.Enabled = True
        btnSalvar.Visible = True
        btnCancelar.Visible = True

        txtCodigo.Text = ""
        txtNome.Text = ""

        Page.ClientScript.RegisterStartupScript(Me.GetType(), "Foco", "document.forms[0]['txtNome'].focus();", True)
    End Sub

    Protected Sub btnAlterar_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnAlterar.Click
        hiddenAcao.Value = "A"

        Dim item As DataGridItem
        For Each item In dtgEquipePDP.Items
            If (CType(item.FindControl("chkMarca"), CheckBox)).Checked Then
                txtCodigo.Text = item.Cells(1).Text
                txtNome.Text = item.Cells(2).Text
                Exit For
            End If
        Next

        If (txtNome.Text = "") Then
            Response.Write("<SCRIPT>alert('Selecione um item.')</SCRIPT>")
            Exit Sub
        Else
            txtNome.Enabled = True
            btnSalvar.Visible = True
            btnCancelar.Visible = True

            Page.ClientScript.RegisterStartupScript(Me.GetType(), "Foco", "document.forms[0]['txtNome'].focus();", True)
        End If
        
    End Sub

    Protected Sub btnExcluir_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnExcluir.Click
        Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
        Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
        Cmd.Connection = Conn

        Dim lstExclui As String = ""

        Dim item As DataGridItem
        For Each item In dtgEquipePDP.Items

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

            txtNome.Enabled = False
            btnSalvar.Visible = False
            btnCancelar.Visible = False

            txtCodigo.Text = ""
            txtNome.Text = ""

            hiddenAcao.Value = ""

            dtgEquipePDP.CurrentPageIndex = 0

        Catch ex As Exception

            If ex.Message.ToLower().IndexOf("key value for constraint") > -1 Then
                Session("strMensagem") = "Não é possivel excluir, a equipe está associada a um usuário"
            Else
                Session("strMensagem") = "Erro ao excluir. (" + ex.Message + ")"
            End If
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

        If txtNome.Text = "" Then
            Response.Write("<SCRIPT>document.forms[0]['txtNome'].focus();alert('Informe a Nome.')</SCRIPT>")
            Exit Sub
        End If

        Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
        Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
        Cmd.Connection = Conn
        Try
            Conn.Open("pdp")

            If hiddenAcao.Value = "I" Then

                Cmd.CommandText = "insert into tb_equipepdp (nom_equipepdp) values ('" & txtNome.Text & "')"

            Else

                Cmd.CommandText = "update tb_equipepdp set nom_equipepdp = '" & txtNome.Text & "' " & _
                                    "where id_equipepdp = " & txtCodigo.Text

            End If

            Cmd.ExecuteNonQuery()
            Conn.Close()

            txtCodigo.Text = ""
            txtNome.Text = ""


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
        txtNome.Enabled = False
        btnSalvar.Visible = False
        btnCancelar.Visible = False

        txtCodigo.Text = ""
        txtNome.Text = ""

        hiddenAcao.Value = ""
    End Sub

End Class