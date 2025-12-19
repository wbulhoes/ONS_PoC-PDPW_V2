Imports System.Collections.Generic

Public Class MenuItem
    Public Property Title As String
    Public Property Description As String
    Public Property Url As String
    Public Property CodAplication As String
    Public Property NodeOrder As Integer
    Public Property Published As Boolean
    Public Property Enabled As Boolean
    Public Property UrlHelp As String
    Public Property PadraoBrowser As String
    Public Property FlgPublico As Boolean
    Public Property ClasseCSS As Object
    Public Property Childs As List(Of MenuItem)
    Public Property Definicoes As List(Of String)
    Public Property UrlIcone As Object
    Public Property TipoRequisicao As String
    Public Property TipoSitemap As String
End Class
