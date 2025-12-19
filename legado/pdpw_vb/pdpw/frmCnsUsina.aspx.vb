Imports System.Data.SqlClient
Partial Class frmCnsUsina
    Inherits System.Web.UI.Page
    Private indice_inicial As Integer = 0         ' Índice inicial para paginação.
    Private oCommand As SqlCommand = New SqlCommand
    Private oConnection As SqlConnection = New SqlConnection
    Private oAdapter As SqlDataAdapter ' OleDb.OleDbDataAdapter
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
        Dim bContinuar As Boolean
        'objMenu = CType(Session("onsmenu"), OnsWebControls.OnsMenu)

        If Not Page.IsPostBack Then
            ' Caso a página tenha sido chamada por um click na coluna servico_id do grid,
            ' nós queremos realizar uma alteração no usuário escolhido.
            If Request.QueryString("strOper") = "A" Then
                ' Chamar a página de entrada de dados.
                Server.Transfer("frmCnsUsiDados.aspx?strCodUsina=" &
                Request.QueryString("id").Trim() & "&strCodEmpresa=" & Session("strCodEmpresa"))
            Else

                Dim intI As Integer
                Dim Conn As SqlConnection = New SqlConnection
                Conn.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
                Dim Cmd As SqlCommand = New SqlCommand
                Cmd.Connection = Conn
                Try
                    Cmd.CommandText = "Select codempre, " &
                                        "       sigempre " &
                                        "From empre " &
                                        "Order By sigempre"
                    Conn.Open()
                    Dim rsEmpresa As SqlDataReader = Cmd.ExecuteReader
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

                    Cmd.Connection.Close()
                    Conn.Close()

                    If Request.QueryString("strOper") = "V" Then
                        cboEmpresa.SelectedIndex = 0
                        bContinuar = True
                        While bContinuar = True
                            If cboEmpresa.SelectedItem.Value.Trim() = Session("strCodEmpresa") Then
                                bContinuar = False
                            Else
                                cboEmpresa.SelectedIndex = cboEmpresa.SelectedIndex + 1
                            End If
                        End While
                        Carregar_Grid()
                    End If

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
        End If
    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        If SessaoAtiva(Page.Session) Then
            MyBase.Render(writer)
            'objMenu.RenderControl(writer)
        End If
    End Sub

    Private Function Carregar_Dados() As DataSet
        'Dim oCommand As OleDb.OleDbCommand = New OleDb.OleDbCommand()
        'Dim oConnection As OleDb.OleDbConnection = New OleDb.OleDbConnection()
        'Dim oAdapter As OleDb.OleDbDataAdapter
        'Dim oDataSet As DataSet
        Dim objRow As TableRow
        Dim objCell As TableCell
        Dim intI, intCol, intTotal As Integer

        'oConnection.Servico = "PDPCnsUsina"
        'oConnection.Usuario = UsuarID
        oConnection.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
        oConnection.Open()
        oCommand.Connection = oConnection
        Try
            oCommand.CommandText = "select codusina, sigusina, nomusina, tipusina " &
                                    "from usina " &
                                    "where codempre = '" & cboEmpresa.SelectedItem.Value & "' "
            'oCommand.ExecuteNonQuery()

            oAdapter = New SqlDataAdapter(oCommand.CommandText, oConnection)
            oDataSet = New DataSet
            oAdapter.Fill(oDataSet)
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

    Private Function UsinaComSelecao_Paginacao(ByVal bCargaDados As Boolean) As DataTable
        ' Carrega as linhas levando em conta o esquema de paginação associado.
        'Dim oDataSet As DataSet                     ' Data Set carregado quando o usuário
        ' informar novo critério.
        Dim oDataTable As DataTable                 ' DataTable com linhas do grid.
        Dim oRow As DataRow                         ' Linha auxiliar.
        Dim i As Integer                            ' Contador.

        ' Criar a tabela com os dados.
        oDataTable = New DataTable
        ' Deve carregar o dataset?
        If bCargaDados = True Then
            ' Aplicar os critérios e carrega o dataset. 
            CnsUsina.oDataSet = Carregar_Dados()
            For i = 0 To CnsUsina.oDataSet.Tables(0).Columns.Count - 1
                oDataTable.Columns.Add(CnsUsina.oDataSet.Tables(0).Columns(i).ColumnName,
                                    System.Type.GetType("System.String"))
            Next
            ' Contar o número de linhas desta tabela.
            dtgUsina.VirtualItemCount = oDataSet.Tables(0).Rows.Count
        Else
            ' Adicionar as colunas correspondentes. 
            oDataTable.Columns.Add("codusina", System.Type.GetType("System.String"))
            oDataTable.Columns.Add("sigusina", System.Type.GetType("System.String"))
            oDataTable.Columns.Add("nomusina", System.Type.GetType("System.String"))
            oDataTable.Columns.Add("tipusina", System.Type.GetType("System.String"))
        End If
        ' Copia o número de linhas que cabe em uma página para a tabela que será usada.
        For i = indice_inicial To (indice_inicial + dtgUsina.PageSize) - 1
            ' Verificar se não atingimos a última linha da tabela.
            If (i <= CnsUsina.oDataSet.Tables(0).Rows.Count - 1) Then
                oRow = oDataTable.NewRow

                oRow(0) = CnsUsina.oDataSet.Tables(0).Rows(i)(0)
                oRow(1) = CnsUsina.oDataSet.Tables(0).Rows(i)(1)
                oRow(2) = CnsUsina.oDataSet.Tables(0).Rows(i)(2)
                oRow(3) = CnsUsina.oDataSet.Tables(0).Rows(i)(3)
                oDataTable.Rows.Add(oRow)
            End If
        Next
        Return oDataTable
    End Function


    Private Sub Carregar_Grid()
        dtgUsina.DataSource = UsinaComSelecao_Paginacao(True)
        dtgUsina.DataBind()
        ' Coloca a form em estado Idle.
    End Sub


    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        indice_inicial = 0
        dtgUsina.CurrentPageIndex = 0
        Session("strCodEmpresa") = cboEmpresa.SelectedItem.Value.Trim()
        Carregar_Grid()
    End Sub



    Private Sub dtgusina_PageIndexChanged(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgUsina.PageIndexChanged
        ' Evento que controla alteração de página no grid.
        'Session("strMensagem") = "TESTE"
        ' Atualizar a página em que o grid estará posicionado.
        dtgUsina.CurrentPageIndex = e.NewPageIndex
        ' Calcular a posição inicial de onde se iniciará a cópia dos elementos.
        indice_inicial = dtgUsina.CurrentPageIndex * dtgUsina.PageSize
        ' Carga dos itens.
        dtgUsina.DataSource = UsinaComSelecao_Paginacao(False)
        ' Realiza o bind do grid. 
        dtgUsina.DataBind()
        dtgUsina.Visible = True

    End Sub
End Class

Module CnsUsina
    Public oDataSet As New DataSet
End Module
