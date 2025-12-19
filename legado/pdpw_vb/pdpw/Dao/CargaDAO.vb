Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports OnsClasses.OnsData
Imports pdpw

Public Class CargaDAO
    Inherits BaseDAO(Of CargaDTO)

    Public Overrides Function Listar(dataPDP As String) As List(Of CargaDTO)
        If String.IsNullOrEmpty(dataPDP) Then
            Throw New NullReferenceException("CargaDAO - Listar - Data PDP não informada")
        End If

        Return Me.ListarTodos($" datpdp = '{dataPDP}' ")
    End Function

    Protected Overrides Function ListarTodos(criterioWhere As String) As List(Of CargaDTO)
        Dim lista As List(Of CargaDTO) = New List(Of CargaDTO)
        Dim chaveCache As String = $"Listar-{criterioWhere}"

        Try

            Dim listaCache As List(Of CargaDTO) = Me.CacheSelect(chaveCache)
            If Not IsNothing(listaCache) Then
                Return listaCache
            End If

            Dim sql As String = "select datpdp, 
                                    Trim(codEmpre) as CodEmpre, 
                                    intCarga, 
                                    valCargaemp, 
                                    valCargapro, 
                                    valCargapre, 
                                    valCargasup, 
                                    valCargatran
                                    from Carga "

            If Not IsNothing(criterioWhere) AndAlso Not String.IsNullOrEmpty(criterioWhere) Then
                sql += $" Where {criterioWhere} "
            End If

            Dim rs As SqlDataReader = Me.ConsultarSQL(sql)

            Do While rs.Read
                Dim dto As New CargaDTO
                dto.DataPDP = rs.GetString(rs.GetOrdinal("datpdp"))
                dto.CodEmpre = rs.GetString(rs.GetOrdinal("CodEmpre"))
                dto.Patamar = rs.GetInt32(rs.GetOrdinal("intCarga"))
                dto.ValCargaEmp = rs.GetInt32(rs.GetOrdinal("valCargaemp"))
                dto.ValCargaPro = rs.GetInt32(rs.GetOrdinal("valCargapro"))
                dto.ValCargaPre = rs.GetInt32(rs.GetOrdinal("valCargapre"))
                dto.ValCargaSup = rs.GetInt32(rs.GetOrdinal("valCargasup"))
                dto.ValCargaTran = rs.GetInt32(rs.GetOrdinal("valCargatran"))

                lista.Add(dto)
            Loop

            rs.Close()
            rs = Nothing
        Catch ex As Exception
            Throw TratarErro("Erro Listar - " + ex.Message, ex)
        Finally
            Me.FecharConexao()
        End Try

        Return Me.CacheSave(chaveCache, lista)
    End Function
End Class
