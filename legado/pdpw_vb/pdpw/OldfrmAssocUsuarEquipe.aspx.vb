Partial Class frmAssocUsuarEquipe
    Inherits System.Web.UI.Page
    Dim objTable As New System.Data.DataTable
    Dim objColumn0 As New System.Data.DataColumn("id_equipepdp", System.Type.GetType("System.String"))
    Dim objColumn1 As New System.Data.DataColumn("usuar_id", System.Type.GetType("System.String"))

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents imgExcluir As System.Web.UI.WebControls.ImageButton

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load, Me.Load
        'objMenu = CType(Session("onsmenu"), OnsWebControls.OnsMenu)
        'Put user code to initialize the page here
        If Not Page.IsPostBack Then
            Dim strSql As String
            Dim objItem As ListItem

            Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection

            Dim daEquipe As OnsClasses.OnsData.OnsDataAdapter
            Dim dsEquipe As DataSet

            Dim daUsuar As OnsClasses.OnsData.OnsDataAdapter
            Dim dsUsuar As DataSet

            Try

                objItem = New ListItem
                objItem.Text = ""
                objItem.Value = "0"

                Conn.Open("pdp")

                strSql = "SELECT id_equipepdp, UPPER(nom_equipepdp) as nom_equipepdp " & _
                         "FROM tb_equipepdp " & _
                         "ORDER BY id_equipepdp"

                daEquipe = New OnsClasses.OnsData.OnsDataAdapter(strSql, Conn)
                dsEquipe = New DataSet
                daEquipe.Fill(dsEquipe, "Equipe")
                cboEquipe.DataTextField = "nom_equipepdp"
                cboEquipe.DataValueField = "id_equipepdp"
                cboEquipe.DataSource = dsEquipe.Tables("Equipe").DefaultView
                cboEquipe.DataBind()
                cboEquipe.Items.Add(objItem)
                cboEquipe.Items.FindByValue("0")

                cboEquipe.SelectedIndex = cboEquipe.Items.Count - 1

                strSql = "SELECT usuar_id, UPPER(usuar_nome) AS usuar_nome " & _
                         "FROM usuar " & _
                         "ORDER BY usuar_nome"

                daUsuar = New OnsClasses.OnsData.OnsDataAdapter(strSql, Conn)
                dsUsuar = New DataSet
                daUsuar.Fill(dsUsuar, "Usuar")
                cboUsuario.DataTextField = "usuar_nome"
                cboUsuario.DataValueField = "usuar_id"
                cboUsuario.DataSource = dsUsuar.Tables("Usuar").DefaultView
                cboUsuario.DataBind()
                cboUsuario.Items.Add(objItem)

                cboUsuario.SelectedIndex = cboUsuario.Items.Count - 1

                DataGridBind()

            Catch ex As Exception

            End Try
            
        End If
    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        If SessaoAtiva(Page.Session) Then
            MyBase.Render(writer)
            'objMenu.RenderControl(writer)
        End If
    End Sub

    Sub dtgAssociar_Paged(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtgAssociar.PageIndexChanged
        dtgAssociar.CurrentPageIndex = e.NewPageIndex
        DataGridBind()
    End Sub

    Sub DataGridBind()
        Dim strSql As String
        Dim daAssociar As OnsClasses.OnsData.OnsDataAdapter
        Dim dsAssociar As DataSet
        Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
        Conn.Open("pdp")
        Try
            strSql = "SELECT ue.id_usuarequipepdp, e.id_equipepdp, UPPER(e.nom_equipepdp) AS nom_equipepdp, u.usuar_id, UPPER(u.usuar_nome) AS usuar_nome " & _
                     "FROM tb_equipepdp e, usuar u, tb_usuarequipepdp ue " & _
                     "WHERE ue.id_equipepdp = e.id_equipepdp " & _
                     "AND ue.usuar_id = u.usuar_id "
            If cboEquipe.SelectedValue = "0" And cboUsuario.SelectedValue = "0" Then
                strSql &= "AND ue.id_equipepdp = ''"
            Else
                If cboEquipe.SelectedValue <> "0" Then
                    strSql &= "AND ue.id_equipepdp = '" & cboEquipe.SelectedValue & "' "
                End If
                If cboUsuario.SelectedValue <> "0" Then
                    strSql &= "AND ue.usuar_id = '" & cboUsuario.SelectedValue & "'"
                End If
            End If
            daAssociar = New OnsClasses.OnsData.OnsDataAdapter(strSql, Conn)
            dsAssociar = New DataSet
            daAssociar.Fill(dsAssociar, "Associar")
            dtgAssociar.DataSource = dsAssociar.Tables("Associar").DefaultView
            dtgAssociar.DataBind()
            'Conn.Close()
        Catch ex As Exception
            If Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If
            Response.Write("<script lang='javascript'>")
            Response.Write("  window.alert('Não foi possível recuperar os registro)!')")
            Response.Write("</script>")
        Finally
            If Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If
        End Try
    End Sub

    Private Sub cboUsuario_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboUsuario.SelectedIndexChanged
        dtgAssociar.CurrentPageIndex = 0
        DataGridBind()
    End Sub

    Private Sub btnExcluir_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnExcluir.Click
        Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
        Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
        Cmd.Connection = Conn

        Dim lstExclui As String = ""

        Dim item As DataGridItem
        For Each item In dtgAssociar.Items

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
                Cmd.CommandText = "Delete From tb_usuarequipepdp Where id_usuarequipepdp in (" & lstExclui & ")"
                Cmd.ExecuteNonQuery()
            End If

            dtgAssociar.CurrentPageIndex = 0

        Catch ex As Exception
            If ex.Message.ToLower().IndexOf("key value for constraint") > -1 Then
                Session("strMensagem") = "Não é possivel excluir, o usuário está associado a um estudo"
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

    Private Sub imgIncluir_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgIncluir.Click
        If cboEquipe.SelectedValue <> "0" And cboUsuario.SelectedValue <> "0" And dtgAssociar.Items.Count = 0 Then

            Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
            Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
            Dim objTrans As OnsClasses.OnsData.OnsTransaction

            Cmd.Connection = Conn
            Conn.Open("pdp")

            ' Inicializa uma transação.
            objTrans = Conn.BeginTransaction()
            Cmd.Transaction = objTrans
            Try
                ' Compor a declaração de inclusão.
                Cmd.CommandText = "INSERT INTO tb_usuarequipepdp " & _
                                  "(id_equipepdp, usuar_id) VALUES " & _
                                  "('" & cboEquipe.SelectedValue & "', '" & cboUsuario.SelectedValue & "')"
                Cmd.ExecuteNonQuery()

                ' Fecha a transação.
                objTrans.Commit()
                ' Liberar recursos.
                'Cmd.Connection.Close()
                'Conn.Close()
                DataGridBind()
            Catch ex As Exception
                'Cmd.Connection.Close()
                objTrans.Rollback()
                If Conn.State = ConnectionState.Open Then
                    Conn.Close()
                End If
                Response.Write("<script lang='javascript'>")
                Response.Write("  window.alert('Não foi possível incluir a associação!')")
                Response.Write("</script>")
            Finally
                If Conn.State = ConnectionState.Open Then
                    Conn.Close()
                End If
            End Try
        Else
            Response.Write("<script lang='javascript'>")
            Response.Write("  window.alert('Não foi possível incluir a associação!.');")
            Response.Write("</script>")
        End If

    End Sub

    Protected Sub cboEquipe_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboEquipe.SelectedIndexChanged
        dtgAssociar.CurrentPageIndex = 0
        DataGridBind()
    End Sub
End Class
