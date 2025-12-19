Imports System.Collections.Generic
Imports System.Data.SqlClient

Public Class CacheDataPDP
    Private Shared cacheDictionary As New Dictionary(Of String, CacheEntry)()

    ' Classe para armazenar os itens do cache e o tempo de expiração
    Private Class CacheEntry
        Public Property Data As List(Of ListItem)
        Public Property Expiration As DateTime
    End Class

    ' Método para obter dados do cache ou do banco de dados
    Public Shared Function GetPdpData(ByVal valueConcatenado As System.Boolean) As List(Of ListItem)

        Dim cacheKey As String = ""

        If valueConcatenado Then
            cacheKey = "pdpDataConcat"
        Else
            cacheKey = "pdpData"
        End If

        Dim sqlQuery As String = "SELECT datpdp FROM pdp ORDER BY datpdp DESC"
        Dim connectionString As String = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
        Dim duracaoCache As Integer = ConfigurationManager.AppSettings("cacheDataPDP")

        ' Verifica se o cache já existe e se ainda está válido
        If cacheDictionary.ContainsKey(cacheKey) Then
            Dim cacheEntry = cacheDictionary(cacheKey)
            If DateTime.Now <= cacheEntry.Expiration Then
                Return cacheEntry.Data
            End If
        End If

        ' Se não existe ou o cache expirou, carrega os dados do banco de dados
        Dim dataList As New List(Of ListItem)()
        Using conn As New SqlConnection(connectionString)
            Using cmd As New SqlCommand(sqlQuery, conn)
                conn.Open()
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        Dim item As New WebControls.ListItem
                        item.Text = Mid(reader("datpdp"), 7, 2) & "/" & Mid(reader("datpdp"), 5, 2) & "/" & Mid(reader("datpdp"), 1, 4)

                        If valueConcatenado Then
                            item.Value = Mid(reader("datpdp"), 7, 2) & "/" & Mid(reader("datpdp"), 5, 2) & "/" & Mid(reader("datpdp"), 1, 4)
                        Else
                            item.Value = reader("datpdp").ToString()
                        End If

                        dataList.Add(item)
                    End While
                End Using
            End Using
        End Using

        ' Armazena os dados no cache
        cacheDictionary(cacheKey) = New CacheEntry() With {
        .Data = dataList,
        .Expiration = DateTime.Now.AddMinutes(duracaoCache)
    }

        Return dataList
    End Function

End Class
