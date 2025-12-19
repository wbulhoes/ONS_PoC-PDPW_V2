Partial Class frmAssocUsuar
    Inherits System.Web.UI.Page
    Dim objTable As New System.Data.DataTable
    Dim objColumn0 As New System.Data.DataColumn("codempre", System.Type.GetType("System.String"))
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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'objMenu = CType(Session("onsmenu"), OnsWebControls.OnsMenu)
        'Put user code to initialize the page here
        If Not Page.IsPostBack Then
            Dim intI As Integer
            Dim strSql As String
            Dim objItem As ListItem

            Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection

            Dim daEmpresa As OnsClasses.OnsData.OnsDataAdapter
            Dim dsEmpresa As DataSet

            Dim daUsuar As OnsClasses.OnsData.OnsDataAdapter
            Dim dsUsuar As DataSet

            Try

                objItem = New ListItem
                objItem.Text = ""
                objItem.Value = "0"

                Conn.Open("pdp")

                strSql = "SELECT codempre, UPPER(sigempre) AS sigempre " & _
                         "FROM empre " & _
                         "ORDER BY sigempre"

                daEmpresa = New OnsClasses.OnsData.OnsDataAdapter(strSql, Conn)
                dsEmpresa = New DataSet
                daEmpresa.Fill(dsEmpresa, "Empresa")
                cboEmpresa.DataTextField = "sigempre"
                cboEmpresa.DataValueField = "codempre"
                cboEmpresa.DataSource = dsEmpresa.Tables("Empresa").DefaultView
                cboEmpresa.DataBind()
                cboEmpresa.Items.Add(objItem)
                cboEmpresa.Items.FindByValue("0")

                cboEmpresa.SelectedIndex = cboEmpresa.Items.Count - 1

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

            ' Percorrer as linhas do grid.
            For Each objItem In dtgAssociar.Items
                ' Procuramos o controle denominado marca. Esta estratégia funciona,
                ' associada a um ItemTemplate. Este contém um CheckBox, que conseguimos
                ' associar um ID.
                objCheckBox = objItem.FindControl("chkMarca")
                ' Salva os IDs marcados para deleção.
                If (objCheckBox.Checked) Then
                    objRow = objTable.NewRow
                    'Colocando as datas na table
                    objRow("codempre") = CType(objItem.Cells(1).Text, String)
                    objRow("usuar_id") = CType(objItem.Cells(3).Text, String)
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

    Sub dtgAssociar_Paged(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs)
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
            strSql = "SELECT e.codempre, UPPER(e.sigempre) AS sigempre, u.usuar_id, UPPER(u.usuar_nome) AS usuar_nome " & _
                     "FROM empre e, usuar u, usuarempre ue " & _
                     "WHERE ue.codempre = e.codempre " & _
                     "AND ue.usuar_id = u.usuar_id "
            If cboEmpresa.SelectedValue = "0" And cboUsuario.SelectedValue = "0" Then
                strSql &= "AND ue.codempre = ''"
            Else
                If cboEmpresa.SelectedValue <> "0" Then
                    strSql &= "AND ue.codempre = '" & cboEmpresa.SelectedValue & "' "
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

    Private Sub cboEmpresa_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboEmpresa.SelectedIndexChanged
        dtgAssociar.CurrentPageIndex = 0
        DataGridBind()
    End Sub

    Private Sub cboUsuario_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboUsuario.SelectedIndexChanged
        dtgAssociar.CurrentPageIndex = 0
        DataGridBind()
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
            dtgAssociar.CurrentPageIndex = 0
            objTrans = Conn.BeginTransaction()
            Cmd.Transaction = objTrans
            Try
                ' Percorre a tabela, eliminando todas as linhas marcadas.
                For Each objRow In objTable.Rows
                    ' Compor a declaração de exclusão.
                    Cmd.CommandText = "DELETE FROM usuarempre " & _
                                      "WHERE codempre = '" & objRow.Item("codempre") & "' " & _
                                      "AND usuar_id = '" & objRow.Item("usuar_id") & "' "
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
                Response.Write("  window.alert('Não foi possível excluir o(s) registro(s)!')")
                Response.Write("</script>")
            Finally
                If Conn.State = ConnectionState.Open Then
                    Conn.Close()
                End If
            End Try
        Else
            Response.Write("<script lang='javascript'>")
            Response.Write("  window.alert('Selecione pelo menos um item para exclusão.');")
            Response.Write("</script>")
        End If
    End Sub

    Private Sub imgIncluir_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgIncluir.Click
        If cboEmpresa.SelectedValue <> "0" And cboUsuario.SelectedValue <> "0" And dtgAssociar.Items.Count = 0 Then

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
                Cmd.CommandText = "INSERT INTO usuarempre " & _
                                  "(codempre, usuar_id) VALUES " & _
                                  "('" & cboEmpresa.SelectedValue & "', '" & cboUsuario.SelectedValue & "')"
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
End Class
