Imports System.IO.Directory
Partial Class frmManDiretorio
    Inherits System.Web.UI.Page

    Dim objTable As New System.Data.DataTable
    Dim objColumn0 As New System.Data.DataColumn("diretorio", System.Type.GetType("System.String"))

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
            DataGridBind()
        Else
            Dim objRow As DataRow
            Dim objCol As DataColumn
            Dim objItem As DataGridItem
            Dim intI As Integer
            Dim objCheckBox As CheckBox

            ' Antes de prosseguir, eliminar quaisquer linhas existentes na tabela
            ' auxiliar.
            For Each objRow In objTable.Rows
                objRow.Delete()
            Next

            'Inseri as duas colunas na table
            objTable.Columns.Add(objColumn0)

            ' Percorreremos as linhas do grid.
            For Each objItem In dtgDiretorio.Items
                ' Procuramos o controle denominado marca. Esta estratégia funciona,
                ' associada a um ItemTemplate. Este contém um CheckBox, que conseguimos
                ' associar um ID.
                objCheckBox = objItem.FindControl("chkMarca")
                ' Salva os IDs marcados para deleção.
                If (objCheckBox.Checked) Then
                    objRow = objTable.NewRow
                    'Colocando as datas na table
                    objRow("diretorio") = Request.PhysicalApplicationPath() & "Temp\" & CType(objItem.Cells(1).Text.Substring(objItem.Cells(1).Text.IndexOf("###") + 4), String) & "\"
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

    Sub dtgDiretorio_Paged(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs)
        dtgDiretorio.CurrentPageIndex = e.NewPageIndex
        DataGridBind()
    End Sub

    Private Sub DataGridBind()
        Dim arrDiretorio() As String
        Dim arrDiretorioNovo() As String
        Dim intI As Integer
        Dim datDiretorio As Date
        Dim strPath As String = Request.PhysicalApplicationPath() & "Temp"
        arrDiretorio = IO.Directory.GetDirectories(strPath)

        For intI = 0 To arrDiretorio.GetUpperBound(0)
            datDiretorio = IO.Directory.GetCreationTime(arrDiretorio(intI))
            ReDim Preserve arrDiretorioNovo(intI)
            arrDiretorioNovo.SetValue(Format(datDiretorio, "dd/MM/yyyy HH:mm") & " ### " & arrDiretorio(intI).Replace(Request.PhysicalApplicationPath() & "Temp\", ""), intI)
        Next
        If intI > 0 Then
            arrDiretorioNovo.Sort(CType(arrDiretorioNovo, System.Array))
        End If
        dtgDiretorio.DataSource = arrDiretorioNovo
        dtgDiretorio.DataBind()
        'dtgDiretorio.Columns.Item(0).HeaderText = "Data de Criação ### Descrição"
    End Sub

    Private Sub btnExcluir_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnExcluir.Click
        Dim objRow As DataRow
        Dim objRowAux As DataRow
        Dim objRows As DataRow()
        Dim strArq As String
        Dim intI As Integer

        If objTable.Rows.Count <> 0 Then
            ' Inicializa uma transação.
            Try
                ' Percorre a tabela, eliminando todas as linhas marcadas.
                For Each objRow In objTable.Rows
                    ' Compor a declaração de exclusão.
                    strArq = Dir(objRow("Diretorio"))
                    Do Until strArq = ""
                        IO.File.Delete(objRow("Diretorio") & strArq)
                        strArq = Dir()
                    Loop
                    IO.Directory.Delete(objRow("Diretorio"))
                Next
                ' Eliminar as linhas marcadas.
                For intI = 0 To objTable.Rows.Count - 1
                    ' Excluir a linha. Sempre o item 0 porque ao eliminar a primeira linha, 
                    ' as outras serão renumeradas.
                    objTable.Rows(0).Delete()
                Next
            Catch ex As Exception
                Response.Write("<script lang='javascript'>")
                Response.Write("  window.alert('Não foi possível excluir o(s) Diretório(s)!')")
                Response.Write("</script>")
            End Try
            Response.Write("<script lang='javascript'>")
            Response.Write("  window.alert('Exclusão(ões) realizada(s) com sucesso!');")
            Response.Write("</script>")
            'Transfere para a página principal.
            Server.Transfer("frmManDiretorio.aspx")
        Else
            Response.Write("<script lang='javascript'>")
            Response.Write("  window.alert('Selecione pelo menos um item para exclusão.');")
            Response.Write("</script>")
        End If
    End Sub

End Class
