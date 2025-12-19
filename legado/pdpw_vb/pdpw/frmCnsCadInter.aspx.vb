Partial Class frmCnsCadInter
    Inherits System.Web.UI.Page
    Dim indice_inicial As Integer = 0         ' Índice inicial para paginação.
    Private oCommand As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
    Private oConnection As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
    Private oAdapter As OnsClasses.OnsData.OnsDataAdapter ' OleDb.OleDbDataAdapter
    Private oDataSet As DataSet

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        objMenu = CType(Session("onsmenu"), OnsWebControls.OnsMenu)
        If Not Page.IsPostBack Then
            Dim intI As Integer
            Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
            Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
            Cmd.Connection = Conn
            Try
                Cmd.CommandText = "Select codempre, " & _
                                "       sigempre " & _
                                "From empre " & _
                                "Order By sigempre"
                Conn.Open("pdp")
                Dim rsEmpresa As OnsClasses.OnsData.OnsDataReader = Cmd.ExecuteReader
                intI = 1
                Dim objItem As System.Web.UI.WebControls.ListItem
                objItem = New System.Web.UI.WebControls.ListItem
                objItem.Text = ""
                objItem.Value = "0"
                cboEmpresa.Items.Add(objItem)
                Do While rsEmpresa.Read
                    objItem = New System.Web.UI.WebControls.ListItem
                    objItem.Text = rsEmpresa.Item("sigempre")
                    objItem.Value = rsEmpresa.Item("codempre")
                    cboEmpresa.Items.Add(objItem)
                    If Trim(cboEmpresa.Items(intI).Value) = Trim(Session("strCodEmpre")) Then
                        cboEmpresa.SelectedIndex = intI
                    End If
                    intI = intI + 1
                Loop
                rsEmpresa.Close()
                rsEmpresa = Nothing

                'Cmd.Connection.Close()
                'Conn.Close()
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
        End If
    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        If SessaoAtiva(Page.Session) Then
            MyBase.Render(writer)
            objMenu.RenderControl(writer)
        End If
    End Sub

    Private Function Carregar_Dados() As DataSet
        Dim objRow As TableRow
        Dim objCell As TableCell
        Dim intI, intCol, intTotal As Integer

        oConnection.Servico = "PDPCnsCadInter"
        oConnection.Usuario = Session("usuarID")
        Try
            oConnection.Open("pdp")
            oCommand.Connection = oConnection
            oCommand.CommandText = "select empre.sigempre, interempre.codcontade || '-' || interempre.codcontapara as contabil, modal.desmodal " & _
                                   "from interempre, empre, modal " & _
                                   "where interempre.codemprede = '" & cboEmpresa.SelectedItem.Value & "' " & _
                                   "and interempre.codemprepara = empre.codempre " & _
                                   "and interempre.codcontamodal = modal.codmodal"
            oAdapter = New OnsClasses.OnsData.OnsDataAdapter(oCommand.CommandText, oConnection)
            oDataSet = New DataSet
            oAdapter.Fill(oDataSet)
            'oConnection.Close()
        Catch ex As Exception
            Session("strMensagem") = ex.Message
            If oConnection.State = ConnectionState.Open Then
                oConnection.Close()
            End If
            Response.Redirect("frmMensagem.aspx")
        Finally
            If oConnection.State = ConnectionState.Open Then
                oConnection.Close()
            End If
        End Try

        Return oDataSet

    End Function

    Private Function InterComSelecao_Paginacao(ByVal bCargaDados As Boolean) As DataTable
        ' Carrega as linhas levando em conta o esquema de paginação associado.
        ' informar novo critério.
        Dim oDataTable As DataTable                 ' DataTable com linhas do grid.
        Dim oRow As DataRow                         ' Linha auxiliar.
        Dim i As Integer                            ' Contador.

        ' Criar a tabela com os dados.
        oDataTable = New DataTable
        ' Deve carregar o dataset?
        If bCargaDados = True Then
            ' Aplicar os critérios e carrega o dataset. 
            CnsInter.oDataSet = Carregar_Dados()
            For i = 0 To CnsInter.oDataSet.Tables(0).Columns.Count - 1
                oDataTable.Columns.Add(CnsInter.oDataSet.Tables(0).Columns(i).ColumnName, _
                                    System.Type.GetType("System.String"))
            Next
            ' Contar o número de linhas desta tabela.
            dtgInter.VirtualItemCount = oDataSet.Tables(0).Rows.Count
        Else
            ' Adicionar as colunas correspondentes. 
            oDataTable.Columns.Add("sigempre", System.Type.GetType("System.String"))
            oDataTable.Columns.Add("contabil", System.Type.GetType("System.String"))
            oDataTable.Columns.Add("desmodal", System.Type.GetType("System.String"))
        End If
        ' Copia o número de linhas que cabe em uma página para a tabela que será usada.
        For i = indice_inicial To (indice_inicial + dtgInter.PageSize) - 1
            ' Verificar se não atingimos a última linha da tabela.
            If (i <= CnsInter.oDataSet.Tables(0).Rows.Count - 1) Then
                oRow = oDataTable.NewRow

                oRow(0) = CnsInter.oDataSet.Tables(0).Rows(i)(0)
                oRow(1) = CnsInter.oDataSet.Tables(0).Rows(i)(1)
                oRow(2) = CnsInter.oDataSet.Tables(0).Rows(i)(2)
                oDataTable.Rows.Add(oRow)
            End If
        Next
        Return oDataTable
    End Function


    Private Sub Carregar_Grid()
        dtgInter.DataSource = InterComSelecao_Paginacao(True)
        dtgInter.DataBind()
        ' Coloca a form em estado Idle.
    End Sub


    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        indice_inicial = 0
        dtgInter.CurrentPageIndex = 0
        Carregar_Grid()
        'dtginter.Visible = True
        'imbPesquisar.Visible = False
    End Sub

    Private Sub dtgInter_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgInter.PageIndexChanged
        ' Evento que controla alteração de página no grid.
        'Session("strMensagem") = "TESTE"
        ' Atualizar a página em que o grid estará posicionado.
        dtgInter.CurrentPageIndex = e.NewPageIndex
        ' Calcular a posição inicial de onde se iniciará a cópia dos elementos.
        indice_inicial = dtgInter.CurrentPageIndex * dtgInter.PageSize
        ' Carga dos itens.
        dtgInter.DataSource = InterComSelecao_Paginacao(False)
        ' Realiza o bind do grid. 
        dtgInter.DataBind()
        dtgInter.Visible = True

    End Sub
End Class

Module CnsInter
    Public oDataSet As New DataSet
End Module
