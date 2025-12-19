Partial Class frmCnsMotivo
    Inherits System.Web.UI.Page
    Dim indice_inicial As Integer = 0         ' Índice inicial para paginação.
    Dim Conn As System.Data.SqlClient.SqlConnection = New System.Data.SqlClient.SqlConnection
    Dim Cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand

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
        Try
            If Not Page.IsPostBack Then
                indice_inicial = 0
                dtgMotivo.CurrentPageIndex = 0
                DataGridBind()
            End If
        Catch ex As Exception
            Session("strMensagem") = "Não foi possível visualizar os dados"
            Response.Redirect("frmMensagem.aspx")
        End Try
    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        If SessaoAtiva(Page.Session) Then
            MyBase.Render(writer)
            'objMenu.RenderControl(writer)
        End If
    End Sub

    Public Sub dtgMotivo_Paged(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs)
        dtgMotivo.CurrentPageIndex = e.NewPageIndex
        DataGridBind()
    End Sub

    Private Sub DataGridBind()
        Dim strSql As String
        Dim daMot As System.Data.SqlClient.SqlDataAdapter
        Dim dsMot As DataSet
        Cmd.Connection = Conn
        Try
            Conn.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
            Conn.Open()
            Cmd.CommandText = "Select codmotivo, motivo " &
                              "From motivoscad " &
                              "Order By codmotivo"
            daMot = New System.Data.SqlClient.SqlDataAdapter(Cmd.CommandText, Conn)
            dsMot = New DataSet
            daMot.Fill(dsMot, "Motivo")
            dtgMotivo.DataSource = dsMot.Tables("Motivo").DefaultView
            dtgMotivo.DataBind()
            'Conn.Close()
        Catch ex As Exception
            If Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If
            Session("strMensagem") = "Não foi possível visualizar os dados"
            Response.Redirect("frmMensagem.aspx")
        Finally
            If Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If
        End Try
    End Sub
End Class
