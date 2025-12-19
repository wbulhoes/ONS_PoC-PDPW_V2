Imports System.Collections.Generic
Imports System.IO
Imports System.Web.Script.Serialization

Public Class SemanaPMOBusiness
    Implements ISemanaPMOBusiness

    Private Shared _listaSemanaPMO_DTO As List(Of SemanaPMO_DTO) = Nothing

    Private Sub PreencherLista()
        If IsNothing(_listaSemanaPMO_DTO) Then
            Dim caminho As String = System.AppDomain.CurrentDomain.BaseDirectory.ToString()
            Dim jsonString As String = File.ReadAllText(caminho & "\Bin\Temp\ListaSemanaPMO.json")
            Dim serializer As JavaScriptSerializer = New JavaScriptSerializer With {
                .MaxJsonLength = Integer.MaxValue
            }
            _listaSemanaPMO_DTO = serializer.Deserialize(Of List(Of SemanaPMO_DTO))(jsonString)
        End If
    End Sub

    Public Function ListarTodas() As List(Of SemanaPMO_DTO)
        PreencherLista()
        Return _listaSemanaPMO_DTO
    End Function

End Class
