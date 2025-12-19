Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports System.Linq
Imports OnsClasses.OnsData
Imports pdpw

Public Class UsinaDAO
    Inherits BaseDAO(Of UsinaDTO)

    Public Overrides Function Listar(dataPDP As String) As List(Of UsinaDTO)
        Throw New NotImplementedException()
    End Function

    Public Function ListarUsina(ByVal codUsina As String) As List(Of UsinaDTO)
        If String.IsNullOrEmpty(codUsina) Then
            Throw New NullReferenceException("UsinaDAO - Listar - Código Usina não informado")
        End If

        Return Me.ListarTodos($" Codusina = '{codUsina}'")
    End Function

    Public Function ListarUsinasPorEmpresas(ByVal listaCodEmpre As List(Of String)) As List(Of UsinaDTO)
        Dim lista As List(Of UsinaDTO) = New List(Of UsinaDTO)()

        Dim distinctValidCodEmpre As List(Of String) = listaCodEmpre _
                                                    .Where(Function(c) Not String.IsNullOrEmpty(c.Trim())) _
                                                    .Distinct() _
                                                    .ToList()

        If distinctValidCodEmpre.Count > 0 Then
            Dim codEmpreInClause As String = String.Join(",", distinctValidCodEmpre.Select(Function(c) $"'{c}'"))
            lista = Me.ListarTodos($" CodEmpre IN ({codEmpreInClause}) ")
        End If

        Return lista
    End Function

    Public Function ListarUsinaPorEmpresa(ByVal codEmpre As String) As List(Of UsinaDTO)
        If String.IsNullOrEmpty(codEmpre) Then
            Throw New NullReferenceException("UsinaDAO - Listar - Código Empresa não informado")
        End If

        Return Me.ListarTodos($" CodEmpre = '{codEmpre}' ")
    End Function

    Protected Overrides Function ListarTodos(criterioWhere As String) As List(Of UsinaDTO)
        Dim lista As List(Of UsinaDTO) = New List(Of UsinaDTO)
        Dim chaveCache As String = $"Listar-{criterioWhere}"

        Try
            Dim listaCache As List(Of UsinaDTO) = Me.CacheSelect(chaveCache)
            If Not IsNothing(listaCache) Then
                Return listaCache
            End If

            Dim sql As String = "SELECT Trim(codusina) as CodUsina, 
                                        nomusina, 
                                        codempre, 
                                        potinstalada, 
                                        usi_bdt_id, 
                                        dpp_id, 
                                        sigsme,
                                        tpusina_id
                                    FROM usina "

            If Not IsNothing(criterioWhere) AndAlso Not String.IsNullOrEmpty(criterioWhere) Then
                sql += $" Where {criterioWhere} "
            End If

            Dim rs As SqlDataReader = Me.ConsultarSQL(sql)

            Do While rs.Read
                Dim dto As New UsinaDTO
                dto.CodUsina = rs.GetString(rs.GetOrdinal("codusina"))

                dto.NomeUsina = rs.GetString(rs.GetOrdinal("nomusina"))

                If (Not rs.IsDBNull(rs.GetOrdinal("codempre"))) Then
                    dto.CodEmpre = rs.GetString(rs.GetOrdinal("codempre"))
                Else
                    dto.CodEmpre = String.Empty
                End If

                If (Not rs.IsDBNull(rs.GetOrdinal("potinstalada"))) Then
                    dto.Potinstalada = rs.GetInt32(rs.GetOrdinal("potinstalada"))
                Else
                    dto.Potinstalada = 0
                End If

                If (Not rs.IsDBNull(rs.GetOrdinal("usi_bdt_id"))) Then
                    dto.UsiBdtId = rs.GetString(rs.GetOrdinal("usi_bdt_id"))
                Else
                    dto.UsiBdtId = String.Empty
                End If

                If (Not rs.IsDBNull(rs.GetOrdinal("dpp_id"))) Then
                    dto.DppId = rs.GetDouble(rs.GetOrdinal("dpp_id"))
                Else
                    dto.DppId = 0
                End If

                If (Not rs.IsDBNull(rs.GetOrdinal("sigsme"))) Then
                    dto.Sigsme = rs.GetString(rs.GetOrdinal("sigsme"))
                Else
                    dto.Sigsme = String.Empty
                End If

                If (Not rs.IsDBNull(rs.GetOrdinal("tpusina_id"))) Then
                    dto.TpusinaId = rs.GetString(rs.GetOrdinal("tpusina_id"))
                Else
                    dto.TpusinaId = String.Empty
                End If

                lista.Add(dto)
            Loop

            rs.Close()
            rs = Nothing
        Catch ex As Exception
            Throw TratarErro("Erro ListarUsina - " + ex.Message, ex)
        Finally
            Me.FecharConexao()
        End Try

        Return Me.CacheSave(chaveCache, lista)
    End Function
End Class
