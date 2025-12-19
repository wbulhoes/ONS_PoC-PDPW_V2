Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports System.Linq
Imports OnsClasses.OnsData
Imports pdpw

Public Class ArquivoDadgerValorDAO
    Inherits BaseDAO(Of ArquivoDadgerValorDTO)

    Public Function ListarPor_DataPDP_Usina(DataPDP As String, codUsina As String) As List(Of ArquivoDadgerValorDTO)

        Dim semanaPMO As SemanaPMO = GetSemanaPMO(Get_DataPDP_DateTime(DataPDP), Nothing, Nothing).FirstOrDefault()
        Dim criterios As String = ""

        If Not IsNothing(semanaPMO) Then
            'Problema com Importação de ArquivoDADGER com SemanaPMO que começa em mês e termina no outro mês. 
            'Será avaliado futura correção.
            'sql += " a.id_anomes = {semanaPMO.IdAnomes} and " 

            criterios += $" a.id_semanapmo = '{semanaPMO.IdSemanapmo}' "
        Else
            Throw TratarErro($"Não foi possível obter Semana PMO para Data PDP '{DataPDP}'")
        End If

        If Not IsNothing(codUsina) AndAlso Not String.IsNullOrEmpty(codUsina) Then
            criterios += $" and u.CodUsina = '{codUsina}' "
        Else
            Throw New NullReferenceException($"ArquivoDadgerValorDAO - Listar - Cod Usina não informado.")
        End If

        Return Me.ListarTodos(criterios)

    End Function

    Public Overrides Function Listar(DataPDP As String) As List(Of ArquivoDadgerValorDTO)

        Dim semanaPMO As SemanaPMO = GetSemanaPMO(Get_DataPDP_DateTime(DataPDP), Nothing, Nothing).LastOrDefault()
        Dim criterios As String = ""

        If Not IsNothing(semanaPMO) Then
            'Problema com Importação de ArquivoDADGER com SemanaPMO que começa em mês e termina no outro mês. 
            'Será avaliado futura correção.
            'sql += " a.id_anomes = {semanaPMO.IdAnomes} and " 

            criterios += $" a.id_semanapmo = '{semanaPMO.IdSemanapmo}' "
        Else
            Throw TratarErro($"Não foi possível obter Semana PMO para Data PDP '{DataPDP}'")
        End If

        Return Me.ListarTodos(criterios)

    End Function

    Protected Overrides Function ListarTodos(criterioWhere As String) As List(Of ArquivoDadgerValorDTO)
        Dim lista As List(Of ArquivoDadgerValorDTO) = New List(Of ArquivoDadgerValorDTO)
        Dim chaveCache As String = $"Listar-{criterioWhere}"

        Try
            Dim listaCache As List(Of ArquivoDadgerValorDTO) = Me.CacheSelect(chaveCache)
            If Not IsNothing(listaCache) Then
                Return listaCache
            End If

            Dim sql As String = $"SELECT id_arquivodadgervalor,
                                        v.id_arquivodadger, 
                                        v.dpp_id, 
                                        Trim(ISNULL(u.codusina,'')) as CodUsina,
                                        val_cvu, 
                                        val_inflexileve, 
                                        val_infleximedia, 
                                        val_inflexipesada, 
                                        val_inflexipmo
                                        FROM tb_arquivodadgervalor v
                                        join tb_arquivodadger a on a.id_arquivodadger = v.id_arquivodadger
                                        left join Usina u on u.Dpp_Id = v.Dpp_Id and u.tpusina_id = 'UTE' "

            If Not IsNothing(criterioWhere) AndAlso Not String.IsNullOrEmpty(criterioWhere) Then
                sql += $" Where {criterioWhere} "
            End If

            Dim rs As SqlDataReader = Me.ConsultarSQL(sql)

            Do While rs.Read
                Dim dto As ArquivoDadgerValorDTO = New ArquivoDadgerValorDTO()
                dto.IdArquivoDadgerValor = rs("id_arquivodadgervalor")
                dto.IdArquivoDadger = rs("id_arquivodadger")
                dto.DppId = rs.GetDouble(rs.GetOrdinal("dpp_id"))
                dto.CodUsina = rs.GetString(rs.GetOrdinal("codusina"))
                dto.ValorCVU = rs.GetDecimal(rs.GetOrdinal("val_cvu"))
                dto.ValorIfxLeve = rs.GetDecimal(rs.GetOrdinal("val_inflexileve"))
                dto.ValorIfxMedia = rs.GetDecimal(rs.GetOrdinal("val_infleximedia"))
                dto.ValorIfxPesada = rs.GetDecimal(rs.GetOrdinal("val_inflexipesada"))
                dto.ValorLimiteIfxPMO = rs.GetInt32(rs.GetOrdinal("val_inflexipmo"))

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
