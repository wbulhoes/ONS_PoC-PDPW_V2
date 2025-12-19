Imports System.Collections.Generic
Imports System.IO
Imports System.Linq
Imports System.Text
Imports System.Web.Script.Serialization
Imports Newtonsoft.Json

Public Class Site
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Itens.Text = menu()
        head.DataBind()
    End Sub

    Public Function menu() As String
        Dim filePath As String = Server.MapPath("Menu\Menu.json")
        Dim jsonString As String = File.ReadAllText(filePath, Encoding.UTF8)
        Dim serializer As New JavaScriptSerializer()
        Dim menuData As List(Of MenuItem) = serializer.Deserialize(Of List(Of MenuItem))(jsonString)
        Dim menuDataFiltrado As List(Of MenuItem) = VerificarPerfilID(menuData, PerfilID)
        Dim jsonSerializado As String = serializer.Serialize(menuDataFiltrado)
        Return jsonSerializado
    End Function

    ''' <summary>
    ''' Função recursiva que verifica se o MenuItem permite o PerfilID
    ''' </summary>
    ''' <param name="menuItems"></param>
    ''' <param name="perfilID"></param>
    ''' <returns></returns>
    Function VerificarPerfilID(menuItems As List(Of MenuItem), perfilID As String) As List(Of MenuItem)
        Dim menuDataFiltrado As New List(Of MenuItem)()

        For Each item As MenuItem In menuItems
            If item.Definicoes IsNot Nothing AndAlso item.Definicoes.Contains(perfilID) Then
                item.Url = item.Url.Replace("{URL_BASE}", ConfigurationManager.AppSettings("urlbase"))
                menuDataFiltrado.Add(item)
                If item.Childs IsNot Nothing AndAlso item.Childs.Count > 0 Then
                    Dim childItems As List(Of MenuItem) = VerificarPerfilID(item.Childs, perfilID)
                    item.Childs.Clear()
                    If childItems.Count > 0 Then
                        item.Childs.AddRange(childItems)
                    End If
                End If
            End If
        Next

        Return menuDataFiltrado
    End Function

End Class