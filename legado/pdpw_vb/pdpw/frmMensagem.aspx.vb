Partial Class frmMensagem
    Inherits System.Web.UI.Page

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

        Dim msg As String = Session("strMensagem")

        If Not IsNothing(Page.Request.QueryString("strMensagem")) And Page.Request.QueryString("strMensagem") <> "" Then
            msg = Page.Request.QueryString("strMensagem")
        End If

        TxtMensagem.Text = msg.Replace("|", Environment.NewLine) 'Fazendo quebra de linha

        Session("strMensagem") = ""
    End Sub

End Class
