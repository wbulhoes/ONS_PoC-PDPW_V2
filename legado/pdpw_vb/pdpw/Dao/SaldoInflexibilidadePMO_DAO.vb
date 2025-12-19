Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports OnsClasses.OnsData
Imports pdpw

Public Class SaldoInflexibilidadePMO_DAO
    Inherits BaseDAO(Of SaldoInflexibilidadePMO_DTO)

    Public Function ListarPor_DataPDP_Usina(dataPDP As String, codUsina As String) As List(Of SaldoInflexibilidadePMO_DTO)

        Dim criterios As String = ""

        If Not String.IsNullOrEmpty(dataPDP) Then
            criterios += $" s.datpdp = '{dataPDP}' "
        Else
            Throw New NullReferenceException("SaldoInflexibilidadePMODAO - Listar - Data PDP não informada")
        End If

        If Not String.IsNullOrEmpty(codUsina) Then
            criterios += $" and s.CodUsina = '{codUsina}' "
        Else
            Throw New NullReferenceException("SaldoInflexibilidadePMODAO - Listar - Cod Usina não informada")
        End If

        Return Me.ListarTodos(criterios)
    End Function

    Public Overrides Function Listar(dataPDP As String) As List(Of SaldoInflexibilidadePMO_DTO)
        If String.IsNullOrEmpty(dataPDP) Then
            Throw New NullReferenceException("SaldoInflexibilidadePMODAO - Listar - Data PDP não informada")
        End If

        Return Me.ListarTodos($" s.datpdp = '{dataPDP}' ")
    End Function

    Public Overloads Function ListarPor_DataPDP_Empresa(dataPDP As String, ByVal codEmpresa As String) As List(Of SaldoInflexibilidadePMO_DTO)
        If String.IsNullOrEmpty(dataPDP) Then
            Throw New NullReferenceException("SaldoInflexibilidadePMODAO - Listar - Data PDP não informada")
        End If

        If String.IsNullOrEmpty(codEmpresa) Then
            Throw New NullReferenceException("SaldoInflexibilidadePMODAO - Listar - Código Empresa não informado")
        End If

        Return Me.ListarTodos($" s.datpdp = '{dataPDP}' and u.CodEmpre = '{codEmpresa}' ")

    End Function

    Protected Overrides Function ListarTodos(criterioWhere As String) As List(Of SaldoInflexibilidadePMO_DTO)
        Dim lista As List(Of SaldoInflexibilidadePMO_DTO) = New List(Of SaldoInflexibilidadePMO_DTO)
        Dim chaveCache As String = $"Listar-{criterioWhere}"

        Try
            Dim listaCache As List(Of SaldoInflexibilidadePMO_DTO) = Me.CacheSelect(chaveCache)
            If Not IsNothing(listaCache) Then
                Return listaCache
            End If

            Dim sql As String = "SELECT s.id_saldoinflexibilidadepmo, 
                                        s.datpdp, 
                                        Trim(s.codusina) as CodUsina, 
                                        s.val_acumuladodessemsemana, 
                                        s.val_programado, 
                                        s.val_enviadodessem, 
                                        s.val_saldo
                                    FROM tb_saldoinflexibilidadepmo s join usina u on u.CodUsina = s.CodUsina "

            If Not IsNothing(criterioWhere) AndAlso Not String.IsNullOrEmpty(criterioWhere) Then
                sql += $" Where {criterioWhere} "
            End If

            Dim rs As SqlDataReader = Me.ConsultarSQL(sql)

            Do While rs.Read
                Dim dto As SaldoInflexibilidadePMO_DTO = New SaldoInflexibilidadePMO_DTO()
                dto.IdSaldoInflexibilidadePMO = rs("id_saldoinflexibilidadepmo")
                dto.DatPDP = rs("datpdp")
                dto.CodUsina = rs("codusina")
                dto.ValAcumuladoDESSEM_Semana = rs.GetInt32(rs.GetOrdinal("val_acumuladodessemsemana"))
                dto.ValProgramado = rs.GetInt32(rs.GetOrdinal("val_programado"))
                dto.ValEnviadoDESSEM = rs.GetInt32(rs.GetOrdinal("val_enviadodessem"))
                dto.ValSaldo = rs.GetInt32(rs.GetOrdinal("val_saldo"))

                lista.Add(dto)
            Loop

            rs.Close()
            rs = Nothing

        Catch ex As Exception
            Throw TratarErro(ex)
        Finally
            Me.FecharConexao()
        End Try

        Return Me.CacheSave(chaveCache, lista)
    End Function
End Class
