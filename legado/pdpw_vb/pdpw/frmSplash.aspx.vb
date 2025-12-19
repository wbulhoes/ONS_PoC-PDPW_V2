Partial Class frmSplash
    Inherits System.Web.UI.Page

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
        'Dim OnsMenu As New OnsWebControls.OnsMenu()
        'OnsMenu.UsuarID = Session("usuarID")
        'OnsMenu.PerfilID = Session("perfilID")
        '' Atribuir path base.
        'OnsMenu.BasePath = Request.ApplicationPath
        ''Response.Write(abreScript & "alert('" & Request.ApplicationPath & "');" & fechaScript)
        'Session("onsmenu") = OnsMenu

        'objMenu = CType(Session("onsmenu"), OnsWebControls.OnsMenu)
        'Put user code to initialize the page here
    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        MyBase.Render(writer)
        'objMenu.RenderControl(writer)
    End Sub

End Class

