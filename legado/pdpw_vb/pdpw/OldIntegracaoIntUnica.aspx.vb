Imports TEncryption = Ons.CryptoAux.TEncryption

Partial Class IntegracaoIntUnica
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
        Dim abreScript As String = "<"
        abreScript &= "script"
        abreScript &= ">"

        Dim fechaScript As String = "</"
        fechaScript &= "script"
        fechaScript &= ">"

        Dim caminhoServidor = Request.Url.GetLeftPart(UriPartial.Authority)
        Dim caminhoIntUnica = "/intunica/menu.aspx"

        'Response.Write(abreScript & "alert('Teste');" & fechaScript)
        SetPerfilID(TEncryption.DecryptString3DES(Page.Request.QueryString("IntUnicaperfilID")))
        SetCosID(TEncryption.DecryptString3DES(Page.Request.QueryString("IntUnicacosID")))
        SetAgeID(TEncryption.DecryptString3DES(Page.Request.QueryString("IntUnicaageID")))
        SetUsuarID(TEncryption.DecryptString3DES(Page.Request.QueryString("IntUnicausuarID")))
        SetSistemaID(TEncryption.DecryptString3DES(Page.Request.QueryString("IntUnicasistemaID")))

        Dim cult As New System.Globalization.CultureInfo("pt-BR")
        Dim data As DateTime = Convert.ToDateTime(TEncryption.DecryptString3DES(Page.Request.QueryString("IntUnicaData")), cult.DateTimeFormat)

        If (data.AddMinutes(1) < DateTime.Now) Then
            Dim mensagem As String = abreScript
            mensagem &= "alert('Sessão expirada. Favor realizar novo Login.'); top.location.replace('" & caminhoIntUnica & "');"
            mensagem &= fechaScript

            Response.Write(mensagem)
            Response.End()
        End If


        'Response.Write(abreScript & "alert('" & Session("usuarID").ToString() & "');" & fechaScript)
        If UsuarID = Nothing Then
            Dim mensagem As String = abreScript
            mensagem &= "alert('Sessão expirada. Favor realizar novo Login.2');"
            mensagem &= fechaScript

            Response.Write(mensagem)
            Response.End()
        End If


        Dim OnsMenu As New OnsWebControls.OnsMenu()
        OnsMenu.UsuarID = UsuarID
        OnsMenu.PerfilID = PerfilID
        ' Atribuir path base.
        OnsMenu.BasePath = Request.ApplicationPath
        'Response.Write(abreScript & "alert('" & Request.ApplicationPath & "');" & fechaScript)
        Session("onsmenu") = OnsMenu


        Select Case CType(OnsMenu.PerfilID, System.String).Trim()
            Case Is = "sadstgit"
                Session("IGS_Perfil") = 2
            Case Is = "sadstsuper"
                Session("IGS_Perfil") = 1
            Case Is = "sadstcentr"
                Session("IGS_Perfil") = 0
        End Select
        Session("IGS_Estacao") = CosID


        Dim interface_form As String

        interface_form = Page.Request.QueryString("form").Trim

        'Response.Write(abreScript & "alert('" & caminhoServidor & interface_form.Trim & "'); " & fechaScript)

        If (Response.IsClientConnected) Then
            Response.Redirect(caminhoServidor & interface_form.Trim, False)
        Else
            Response.End()
        End If
    End Sub


End Class
