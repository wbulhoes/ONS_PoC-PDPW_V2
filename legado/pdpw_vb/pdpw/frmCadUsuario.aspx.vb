Partial Class frmCadUsuario
    Inherits System.Web.UI.Page
    Dim objTable As New System.Data.DataTable
    Dim objColumn0 As New System.Data.DataColumn("usuar_id", System.Type.GetType("System.String"))
    Dim objColumn1 As New System.Data.DataColumn("usuar_nome", System.Type.GetType("System.String"))
    Dim objColumn2 As New System.Data.DataColumn("usuar_email", System.Type.GetType("System.String"))
    Dim objColumn3 As New System.Data.DataColumn("usuar_telefone", System.Type.GetType("System.String"))

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

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
        'Put user code to initialize the page here
        'objMenu = CType(Session("onsmenu"), OnsWebControls.OnsMenu)
        If Not Page.IsPostBack Then
            btnAlterar.Enabled = False
            btnExcluir.Enabled = False
            DataGridBind()
        Else
            Dim objRow As DataRow
            Dim objCol As DataColumn
            Dim objItem As DataGridItem
            Dim objCheckBox As CheckBox

            ' Antes de prosseguir, eliminar quaisquer linhas existentes na tabela
            ' auxiliar.
            For Each objRow In objTable.Rows
                objRow.Delete()
            Next

            'Inseri as três colunas na table
            objTable.Columns.Add(objColumn0)
            objTable.Columns.Add(objColumn1)
            objTable.Columns.Add(objColumn2)
            objTable.Columns.Add(objColumn3)

            ' Percorrer as linhas do grid.
            For Each objItem In dtgUsuario.Items
                ' Procuramos o controle denominado marca. Esta estratégia funciona,
                ' associada a um ItemTemplate. Este contém um CheckBox, que conseguimos
                ' associar um ID.
                objCheckBox = objItem.FindControl("chkMarca")
                ' Salva os IDs marcados para deleção.
                If (objCheckBox.Checked) Then
                    objRow = objTable.NewRow
                    'Colocando as datas na table
                    objRow("usuar_id") = CType(objItem.Cells(1).Text, String).Trim
                    objRow("usuar_nome") = CType(objItem.Cells(2).Text, String).Trim
                    objRow("usuar_email") = CType(objItem.Cells(3).Text, String).Trim
                    objRow("usuar_telefone") = CType(objItem.Cells(4).Text, String).Trim
                    'Acrescentar a linha na tabela.
                    objTable.Rows.Add(objRow)
                End If
            Next

        End If
    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        If SessaoAtiva(Page.Session) Then
            MyBase.Render(writer)
            'objMenu.RenderControl(writer)
        End If
    End Sub

    Private Sub btnPesquisar_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnPesquisar.Click, btnCancelar.Click
        Dim objBotao As ImageButton
        objBotao = CType(sender, ImageButton)

        If objBotao.ID = "btnCancelar" Then
            btnPesquisar.Enabled = True
            btnAlterar.Enabled = False
            btnExcluir.Enabled = False
            'btnSalvar.Enabled = True
            txtLogin.Enabled = True
            txtLogin.Text = ""
            txtNome.Text = ""
            txtEmail.Text = ""
            txtTelefone.Text = ""
        End If

        dtgUsuario.CurrentPageIndex = 0
        DataGridBind()
        If dtgUsuario.Items.Count > 0 Then
            btnAlterar.Enabled = True
            btnExcluir.Enabled = True
        End If
    End Sub

    Sub dtgUsuario_Paged(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs)
        dtgUsuario.CurrentPageIndex = e.NewPageIndex
        DataGridBind()
    End Sub

    Sub DataGridBind()
        Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
        Dim daPesq As OnsClasses.OnsData.OnsDataAdapter
        Dim dsPesq As DataSet
        Dim strSql As String

        strSql = "SELECT usuar_id, usuar_nome, usuar_email, usuar_telefone " & _
                 "FROM usuar "
        If txtLogin.Text <> "" Or txtNome.Text <> "" Or txtEmail.Text <> "" Or txtTelefone.Text <> "" Then
            If txtLogin.Text <> "" Then
                strSql &= "WHERE usuar_id LIKE '%" & txtLogin.Text & "%' "
            End If

            If txtNome.Text <> "" Then
                If strSql.LastIndexOf("WHERE") > 0 Then
                    strSql &= "AND usuar_nome LIKE '%" & txtNome.Text & "%' "
                Else
                    strSql &= "WHERE usuar_nome LIKE '%" & txtNome.Text & "%' "
                End If
            End If

            If txtEmail.Text <> "" Then
                If strSql.LastIndexOf("WHERE") > 0 Then
                    strSql &= "AND usuar_email LIKE '%" & txtEmail.Text & "%' "
                Else
                    strSql &= "WHERE usuar_email LIKE '%" & txtEmail.Text & "%' "
                End If
            End If

            If txtTelefone.Text <> "" Then
                If strSql.LastIndexOf("WHERE") > 0 Then
                    strSql &= "AND usuar_telefone LIKE '%" & txtTelefone.Text & "%' "
                Else
                    strSql &= "WHERE usuar_telefone LIKE '%" & txtTelefone.Text & "%' "
                End If
            End If
        Else
            strSql &= "WHERE usuar_id = ''"
        End If

        Conn.Open("pdp")
        daPesq = New OnsClasses.OnsData.OnsDataAdapter(strSql, Conn)
        dsPesq = New DataSet
        daPesq.Fill(dsPesq, "Usuario")
        dtgUsuario.DataSource = dsPesq.Tables("Usuario").DefaultView
        dtgUsuario.DataBind()
        Conn.Close()
    End Sub

    Private Sub btnAlterar_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnAlterar.Click
        btnAlterar.Enabled = False
        btnExcluir.Enabled = False
        btnPesquisar.Enabled = False

        Dim objRow As DataRow
        Dim objRowAux As DataRow
        Dim objRows As DataRow()
        Dim intI As Integer

        If objTable.Rows.Count <> 0 Then
            Try
                If objTable.Rows.Count > 1 Then
                    Response.Write("<script lang='javascript'>")
                    Response.Write("  window.alert('Marque somente um item para alteração!')")
                    Response.Write("</script>")
                Else
                    For Each objRow In objTable.Rows
                        'Coloca os dados do registro marcado nas text's para alterção
                        txtLogin.Text = objRow("usuar_id")
                        txtNome.Text = objRow("usuar_nome")
                        txtEmail.Text = objRow("usuar_email")
                        txtTelefone.Text = objRow("usuar_telefone")
                        txtLogin.Enabled = False
                        dtgUsuario.CurrentPageIndex = 0
                        DataGridBind()
                    Next
                End If
            Catch ex As Exception
                Response.Write("<script lang='javascript'>")
                Response.Write("  window.alert('Não foi possível alterar o registro!')")
                Response.Write("</script>")
            End Try
        Else
            Response.Write("<script lang='javascript'>")
            Response.Write("  window.alert('Selecione pelo menos um item para alteração.');")
            Response.Write("</script>")
        End If

    End Sub

    Private Sub btnExcluir_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnExcluir.Click
        ' Este botão executa a exclusão das linhas marcadas.
        ' Exclui as linhas marcadas e depois disso transfere para a mesma página.
        ' Com isso os dados da tabela base serão recarregados.
        Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
        Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
        Dim objTrans As OnsClasses.OnsData.OnsTransaction

        Cmd.Connection = Conn
        Conn.Open("pdp")

        Dim objRow As DataRow
        Dim objRowAux As DataRow
        Dim objRows As DataRow()
        Dim intI As Integer

        If objTable.Rows.Count <> 0 Then
            ' Inicializa uma transação.
            dtgUsuario.CurrentPageIndex = 0
            objTrans = Conn.BeginTransaction()
            Cmd.Transaction = objTrans
            Try
                ' Percorre a tabela, eliminando todas as linhas marcadas.
                For Each objRow In objTable.Rows
                    ' Compor a declaração de exclusão.
                    Cmd.CommandText = "DELETE FROM usuar " & _
                                      "WHERE usuar_id = '" & objRow.Item("usuar_id") & "'"
                    ' Executa a operação.
                    Cmd.ExecuteNonQuery()
                Next
                ' Eliminar as linhas marcadas.
                For intI = 0 To objTable.Rows.Count - 1
                    ' Excluir a linha. Sempre o item 0 porque ao eliminar a primeira linha, 
                    ' as outras serão renumeradas.
                    objTable.Rows(0).Delete()
                Next
                ' Fecha a transação.
                objTrans.Commit()
                ' Liberar recursos.
                Cmd.Connection.Close()
                Conn.Close()
                DataGridBind()
            Catch ex As Exception
                Cmd.Connection.Close()
                objTrans.Rollback()
                Conn.Close()
                Response.Write("<script lang='javascript'>")
                Response.Write("  window.alert('Não foi possível excluir o(s) registro(s)!')")
                Response.Write("</script>")
            End Try
        Else
            Response.Write("<script lang='javascript'>")
            Response.Write("  window.alert('Selecione pelo menos um item para exclusão.');")
            Response.Write("</script>")
        End If
    End Sub

    Private Sub btnSalvar_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSalvar.Click
        If txtLogin.Text <> "" And txtNome.Text <> "" And txtEmail.Text <> "" And txtTelefone.Text <> "" Then

            Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
            Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
            Dim objTrans As OnsClasses.OnsData.OnsTransaction

            Cmd.Connection = Conn
            Conn.Open("pdp")

            ' Inicializa uma transação.
            objTrans = Conn.BeginTransaction()
            Cmd.Transaction = objTrans
            Try
                If txtLogin.Enabled = False Then
                    ' Compor a declaração de Alteração.
                    Cmd.CommandText = "UPDATE usuar " & _
                                      "SET usuar_nome = '" & txtNome.Text & "', " & _
                                      "usuar_email = '" & txtEmail.Text & "', " & _
                                      "usuar_telefone = '" & txtTelefone.Text & "' " & _
                                      "WHERE usuar_id = '" & txtLogin.Text & "'"
                Else
                    ' Compor a declaração de inclusão.
                    Cmd.CommandText = "INSERT INTO usuar " & _
                                      "(usuar_id, usuar_nome, usuar_email, usuar_telefone) VALUES " & _
                                      "('" & txtLogin.Text & "','" & txtNome.Text & "','" & txtEmail.Text & "','" & txtTelefone.Text & "')"
                End If
                Cmd.ExecuteNonQuery()

                ' Fecha a transação.
                objTrans.Commit()
                ' Liberar recursos.
                Cmd.Connection.Close()
                Conn.Close()
                DataGridBind()
                txtLogin.Enabled = True
                txtLogin.Text = ""
                txtNome.Text = ""
                txtEmail.Text = ""
                txtTelefone.Text = ""
                btnPesquisar.Enabled = True
                btnExcluir.Enabled = True
                'btnSalvar.Enabled = False
            Catch ex As Exception
                objTrans.Rollback()
                Cmd.Connection.Close()
                Conn.Close()
                Response.Write("<script lang='javascript'>")
                Response.Write("  window.alert('Não foi possível salvar o usuário!')")
                Response.Write("</script>")
            End Try
        Else
            Response.Write("<script lang='javascript'>")
            Response.Write("  window.alert('Não foi possível incluir o usuário!.');")
            Response.Write("</script>")
        End If

    End Sub
End Class
