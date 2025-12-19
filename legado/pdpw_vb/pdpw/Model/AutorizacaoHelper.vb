Option Explicit On
Option Strict On
Imports System.Collections.Generic
Imports System.Linq
Imports System.Runtime.CompilerServices
Imports System.Security.AccessControl
Imports ons.common.providers
Imports ons.common.providers.schemas

Public Module AutorizacaoHelper
    'Session("perfilID") = Ons.CryptoAux.TEncryption.DecryptString3DES(Page.Request.QueryString("IntUnicaperfilID")) - ok
    'Session("cosID") = Ons.CryptoAux.TEncryption.DecryptString3DES(Page.Request.QueryString("IntUnicacosID")) - ok
    'Session("ageID") = Ons.CryptoAux.TEncryption.DecryptString3DES(Page.Request.QueryString("IntUnicaageID")) - nok
    'Session("usuarID") = Ons.CryptoAux.TEncryption.DecryptString3DES(Page.Request.QueryString("IntUnicausuarID")) - ok
    'Session("sistemaID") = Ons.CryptoAux.TEncryption.DecryptString3DES(Page.Request.QueryString("IntUnicasistemaID"))- ok
    'Session("onsmenu")'
    Private logger As log4net.ILog = log4net.LogManager.GetLogger(GetType(AutorizacaoHelper))
    <Extension()>
    Public Function PerfilID(ByVal master As System.Web.UI.MasterPage) As String
        Return PerfilID()
    End Function

    <Extension()>
    Public Function PerfilID(ByVal page As Page) As String
        Return PerfilID()
    End Function

    Private Sub TrataUsuarioSemPermissao()
        Dim UserName As String = POPHelper.LoginUsuario()

        If IsNothing(UserName) Then
            UserName = "Usuário não identificado"
        End If
        logger.Warn("Usuário do contexto: " + UserName)

        Throw New Exception("O usuario não possui acesso a nenhum perfil do PDPW.")
    End Sub

    Private Function PerfilID() As String
        If POPHelper.VerificarAcessoQualquerEscopo("ADM_PDPW") Then
            Return "ADM_PDPW"
        ElseIf POPHelper.VerificarAcessoQualquerEscopo("ATUPDP") Then
            Return "ATUPDP"
        ElseIf POPHelper.VerificarAcessoQualquerEscopo("CNSPDPW") Then
            Return "CNSPDPW"
        End If

        TrataUsuarioSemPermissao()

    End Function

    <Extension()>
    Public Function AgentesRepresentados(ByVal page As Page) As String

        Dim perfil As String = PerfilID(page)

        If perfil.Equals("ADM_PDPW") OrElse perfil.Equals("CNSPDPW") Then
            Return ""
        ElseIf perfil.Equals("ATUPDP") Then
            Dim escopos As List(Of EscopoDTO) = POPHelper.ObterAcessos("ATUPDP")
            Return String.Join(", ", escopos.Select(Function(escopo) String.Concat("'", escopo.IdEscopo, "'")))
        End If

        TrataUsuarioSemPermissao()

    End Function


    <Extension()>
    Public Function EstaAutorizado(ByVal page As Page) As Boolean

        Dim perfil As String = PerfilID(page)

        If perfil.Equals("ADM_PDPW") OrElse perfil.Equals("ATUPDP") Then
            Return True
        End If

        Return False

    End Function
    <Extension()>
    Public Sub SetPerfilID(ByVal page As Page, perfilId As String)

        page.Session("perfilID") = perfilId

    End Sub

    <Extension()>
    Public Function CosID(ByVal page As Page) As String

        Return page.Session("cosID").ToString()

    End Function
    <Extension()>
    Public Sub SetCosID(ByVal page As Page, cosId As String)

        page.Session("cosID") = cosId

    End Sub

    <Extension()>
    Public Function AgeID(ByVal page As Page) As String

        Return page.Session("ageID")?.ToString()

    End Function
    <Extension()>
    Public Sub SetAgeID(ByVal page As Page, ageId As String)

        page.Session("ageID") = ageId

    End Sub

    <Extension()>
    Public Function UsuarID(ByVal page As Page) As String

        Return POPHelper.LoginUsuario

    End Function

    <Extension()>
    Public Sub SetUsuarID(ByVal page As Page, usuarId As String)

        page.Session("usuarID") = usuarId

    End Sub

    <Extension()>
    Public Function SistemaID(ByVal page As Page) As String

        Return page.Session("sistemaID").ToString()

    End Function
    <Extension()>
    Public Sub SetSistemaID(ByVal page As Page, sistemaId As String)

        page.Session("sistemaID") = sistemaId

    End Sub

    <Extension()>
    Public Function OnsMenu(ByVal page As Page) As OnsWebControls.OnsMenu

        Return CType(page.Session("onsmenu"), OnsWebControls.OnsMenu)

    End Function
    <Extension()>
    Public Sub SetOnsMenu(ByVal page As Page, onsmenu As OnsWebControls.OnsMenu)

        page.Session("onsmenu") = onsmenu

    End Sub

End Module
