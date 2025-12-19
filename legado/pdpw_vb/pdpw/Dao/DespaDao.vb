Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports OnsClasses.OnsData
Imports pdpw

Public Class DespaDAO
    Inherits BaseDAO(Of DespaDTO)

    Public Overrides Function Listar(dataPDP As String) As List(Of DespaDTO)
        If String.IsNullOrEmpty(dataPDP) Then
            Throw New NullReferenceException("DespaDAO - Listar - Data PDP não informada")
        End If

        Return Me.ListarTodos($" datpdp = '{dataPDP}' ")
    End Function

    Protected Overrides Function ListarTodos(criterioWhere As String) As List(Of DespaDTO)
        Dim lista As List(Of DespaDTO) = New List(Of DespaDTO)
        Dim chaveCache As String = $"Listar-{criterioWhere}"

        Try

            Dim listaCache As List(Of DespaDTO) = Me.CacheSelect(chaveCache)
            If Not IsNothing(listaCache) Then
                Return listaCache
            End If

            Dim sql As String = "select datpdp, 
                                    Trim(codusina) as CodUsina, 
                                    intdespa, 
                                    valdespaemp, 
                                    valdespapro, 
                                    valdespapre, 
                                    valdespasup, 
                                    valdespatran
                                    from despa "

            If Not IsNothing(criterioWhere) AndAlso Not String.IsNullOrEmpty(criterioWhere) Then
                sql += $" Where {criterioWhere} "
            End If

            Dim rs As SqlDataReader = Me.ConsultarSQL(sql)

            Do While rs.Read
                Dim dto As New DespaDTO
                dto.DataPDP = rs.GetString(rs.GetOrdinal("datpdp"))
                dto.CodUsina = rs.GetString(rs.GetOrdinal("codusina"))
                dto.Patamar = rs.GetInt32(rs.GetOrdinal("intdespa"))
                dto.ValDespaEmp = rs.GetInt32(rs.GetOrdinal("valdespaemp"))
                dto.ValDespaPro = rs.GetInt32(rs.GetOrdinal("valdespapro"))
                dto.ValDespaPre = rs.GetInt32(rs.GetOrdinal("valdespapre"))
                dto.ValDespaSup = rs.GetInt32(rs.GetOrdinal("valdespasup"))
                dto.ValDespaTran = rs.GetInt32(rs.GetOrdinal("valdespatran"))

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
