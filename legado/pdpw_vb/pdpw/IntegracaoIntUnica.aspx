<%@ Page Language="vb" %>

<script runat="server">
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
        
        Session("perfilID") = Ons.CryptoAux.TEncryption.DecryptString3DES(Page.Request.QueryString("IntUnicaperfilID"))
        Session("cosID") = Ons.CryptoAux.TEncryption.DecryptString3DES(Page.Request.QueryString("IntUnicacosID"))
        Session("ageID") = Ons.CryptoAux.TEncryption.DecryptString3DES(Page.Request.QueryString("IntUnicaageID"))
        Session("usuarID") = Ons.CryptoAux.TEncryption.DecryptString3DES(Page.Request.QueryString("IntUnicausuarID"))
        Session("sistemaID") = Ons.CryptoAux.TEncryption.DecryptString3DES(Page.Request.QueryString("IntUnicasistemaID"))
        
        
        Dim cult As New System.Globalization.CultureInfo("pt-BR")
        Dim data As DateTime = Convert.ToDateTime(Ons.CryptoAux.TEncryption.DecryptString3DES(Page.Request.QueryString("IntUnicaData")), cult.DateTimeFormat)
        
        If (data.AddMinutes(1) < DateTime.Now) Then
            Dim mensagem As String = abreScript
            mensagem &= "alert('Sessão expirada. Favor realizar novo Login.'); top.location.replace('" & caminhoIntUnica & "');"
            mensagem &= fechaScript
            
            Response.Write(mensagem)
            Response.End()
        End If
        

        'Response.Write(abreScript & "alert('" & Session("usuarID").ToString() & "');" & fechaScript)
        If Session("usuarID") = Nothing Then
            Dim mensagem As String = abreScript
            mensagem &= "alert('Sessão expirada. Favor realizar novo Login.2');"
            mensagem &= fechaScript
            
            Response.Write(mensagem)
            Response.End()
        End If

        
        Dim OnsMenu As New OnsWebControls.OnsMenu()
        OnsMenu.UsuarID = Session("usuarID")
        OnsMenu.PerfilID = Session("perfilID")
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
        Session("IGS_Estacao") = Session("cosID")


        Dim interface_form As String

        interface_form = Page.Request.QueryString("form").Trim
        
        'Response.Write(abreScript & "alert('" & caminhoServidor & interface_form.Trim & "'); " & fechaScript)
        
        If (Response.IsClientConnected) Then
            Response.Redirect(caminhoServidor & interface_form.Trim, False)
        Else
            Response.End()
        End If
    End Sub
</script>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>Integração IntUnica</title>
</head>
<body>
    <form id="IntegracaoIntUnicaForm" method="post" runat="server">
    </form>
</body>
</html>
