Imports System.Text.RegularExpressions
Imports System.Web
Imports System.Web.SessionState

Public Class [Global]
    Inherits System.Web.HttpApplication

#Region " Component Designer Generated Code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Component Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Required by the Component Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Component Designer
    'It can be modified using the Component Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        components = New System.ComponentModel.Container()
    End Sub

#End Region

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the application is started
        log4net.Config.XmlConfigurator.Configure()
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the session is started
    End Sub

    Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires at the beginning of each request
    End Sub

    Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires upon attempting to authenticate the use
    End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        Dim exc As Exception = Server.GetLastError()
        Dim id_erro As String = " (Código do erro: '" & Guid.NewGuid().ToString() & "') "
        Dim texto_msg As String = " Ocorreu um erro na aplicação. Favor, entre em contato com o administrador." & id_erro
        Dim util As New Util
        util.RegistrarLogErro(texto_msg & exc.Message, exc)

        ' Handle specific exception.
        If TypeOf exc Is HttpUnhandledException Then
            Session("strMensagem") = texto_msg
            Response.Redirect("frmMensagem.aspx")
        End If

        ' Clear the error from the server.
        Server.ClearError()
    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the session ends
    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the application ends
    End Sub

    Protected Sub Application_EndRequest(sender As Object, e As EventArgs)
        Dim redirectUrl As String = Me.Response.RedirectLocation

        If Not String.IsNullOrEmpty(redirectUrl) Then
            If Not redirectUrl.ToLower().Contains("frmMensagem".ToLower()) Then

                Me.Response.RedirectLocation = Regex.Replace(redirectUrl, "ReturnUrl=(?'url'.*)", Function(m As Match)
                                                                                                      Dim url As String = HttpUtility.UrlDecode(m.Groups("url").Value)
                                                                                                      Dim u As New Uri(Me.Request.Url, url)
                                                                                                      Return String.Format("ReturnUrl={0}", HttpUtility.UrlEncode(u.ToString()))
                                                                                                  End Function, RegexOptions.Singleline Or RegexOptions.IgnoreCase Or RegexOptions.ExplicitCapture)
            End If
        End If
    End Sub

End Class
