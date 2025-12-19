Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports System.Linq
Imports OnsClasses.OnsData
Imports pdpw

Public Class PDP_DAO
    Inherits BaseDAO(Of PDP_DTO)
    Public Overrides Function Listar(dataPDP As String) As List(Of PDP_DTO)

        Return Me.ListarTodos($" datpdp = '{dataPDP}' ")
    End Function

    Protected Overrides Function ListarTodos(criterioWhere As String) As List(Of PDP_DTO)
        Dim lista As List(Of PDP_DTO) = New List(Of PDP_DTO)
        Dim chaveCache As String = $"Listar-{criterioWhere}"

        Try

            Dim listaCache As List(Of PDP_DTO) = Me.CacheSelect(chaveCache)
            If Not IsNothing(listaCache) Then
                Return listaCache
            End If

            Dim sql As String = "select datpdp from pdp "

            If Not IsNothing(criterioWhere) AndAlso Not String.IsNullOrEmpty(criterioWhere) Then
                sql += $" Where {criterioWhere} "
            End If

            Dim rsData As SqlDataReader = Me.ConsultarSQL(sql)

            Do While rsData.Read
                Dim dto As New PDP_DTO
                dto.DataPDP = rsData.GetString(rsData.GetOrdinal("datpdp"))
                lista.Add(dto)
            Loop

            rsData.Close()
            rsData = Nothing

        Catch ex As Exception
            Throw TratarErro("Erro Listar - " + ex.Message, ex)
        Finally
            Me.FecharConexao()
        End Try

        Return Me.CacheSave(chaveCache, lista)
    End Function


End Class
