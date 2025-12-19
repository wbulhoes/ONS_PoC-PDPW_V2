Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports OnsClasses.OnsData
Imports pdpw

Public Class PerdaDAO
    Inherits BaseDAO(Of PerdaDTO)

    Public Overrides Function Listar(dataPDP As String) As List(Of PerdaDTO)
        If String.IsNullOrEmpty(dataPDP) Then
            Throw New NullReferenceException("PerdaDAO - Listar - Data PDP não informada")
        End If

        Return Me.ListarTodos($" datpdp = '{dataPDP}' ")
    End Function

    Protected Overrides Function ListarTodos(criterioWhere As String) As List(Of PerdaDTO)
        Dim listaPerda As List(Of PerdaDTO) = New List(Of PerdaDTO)
        Dim chaveCache As String = $"Listar-{criterioWhere}"

        Try
            Dim listaCache As List(Of PerdaDTO) = Me.CacheSelect(chaveCache)
            If Not IsNothing(listaCache) Then
                Return listaCache
            End If

            Dim sql As String = "select 
                                       datpdp, 
                                        Trim(codusina) as CodUsina, 
                                        intpcc, 
                                        valpccemp, 
                                        valpccpro, 
                                        valpccpre, 
                                        valpccsup, 
                                        valpcctran
                                      from Perdascic "

            If Not IsNothing(criterioWhere) AndAlso Not String.IsNullOrEmpty(criterioWhere) Then
                sql += $" Where {criterioWhere} "
            End If

            Dim rsValPerda As SqlDataReader = Me.ConsultarSQL(sql)

            Do While rsValPerda.Read
                Dim perdaDto As New PerdaDTO
                perdaDto.DataPDP = rsValPerda.GetString(rsValPerda.GetOrdinal("datpdp"))
                perdaDto.CodUsina = rsValPerda.GetString(rsValPerda.GetOrdinal("CodUsina"))
                perdaDto.Patamar = rsValPerda.GetInt32(rsValPerda.GetOrdinal("Intpcc"))
                perdaDto.ValpccEmp = rsValPerda.GetInt32(rsValPerda.GetOrdinal("valpccemp"))
                perdaDto.ValpccPro = rsValPerda.GetInt32(rsValPerda.GetOrdinal("valpccpro"))
                perdaDto.ValpccPre = rsValPerda.GetInt32(rsValPerda.GetOrdinal("valpccpre"))
                perdaDto.Valpccsup = rsValPerda.GetInt32(rsValPerda.GetOrdinal("Valpccsup"))
                perdaDto.ValpccTran = rsValPerda.GetInt32(rsValPerda.GetOrdinal("valpcctran"))

                listaPerda.Add(perdaDto)
            Loop

            rsValPerda.Close()
            rsValPerda = Nothing

        Catch ex As Exception
            Throw TratarErro("Erro Listar - " + ex.Message, ex)
        Finally
            Me.FecharConexao()
        End Try

        Return Me.CacheSave(chaveCache, listaPerda)
    End Function
End Class
