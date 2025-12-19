Public Class frmModalidadeOpTermica
    Inherits System.Web.UI.Page
    Dim indice_inicial As Integer = 0         ' Índice inicial para paginação.
    Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
    Protected WithEvents frmCnsModalOperTerm As System.Web.UI.HtmlControls.HtmlForm
    Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents dtgModalidade As System.Web.UI.WebControls.DataGrid

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
        Try
            If Not Page.IsPostBack Then
                indice_inicial = 0
                dtgModalidade.CurrentPageIndex = 0
                DataGridBind()
            End If
        Catch ex As Exception
            Session("strMensagem") = "Não foi possível visualizar os dados"
            Response.Redirect("frmMensagem.aspx")
        End Try
    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        MyBase.Render(writer)
        'objMenu.RenderControl(writer)
    End Sub

    Public Sub dtgmodalidade_Paged(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs)
        dtgModalidade.CurrentPageIndex = e.NewPageIndex
        DataGridBind()
    End Sub

    Private Sub DataGridBind()
        Dim strSql As String
        Dim daMot As OnsClasses.OnsData.OnsDataAdapter
        Dim dsMot As DataSet
        Cmd.Connection = Conn
        Try
            Conn.Open("pdp")
            Cmd.CommandText = "SELECT codmodalidade, modalidade " & _
                              "FROM modalidadeoperterm_cad " & _
                              "ORDER BY codmodalidade"
            daMot = New OnsClasses.OnsData.OnsDataAdapter(Cmd.CommandText, Conn)
            dsMot = New DataSet
            daMot.Fill(dsMot, "modalidade")
            dtgModalidade.DataSource = dsMot.Tables("modalidade").DefaultView
            dtgModalidade.DataBind()
            Conn.Close()
        Catch ex As Exception
            If Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If
            Session("strMensagem") = "Não foi possível visualizar os dados"
            Response.Redirect("frmMensagem.aspx")
        End Try
    End Sub
End Class

