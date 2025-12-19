Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports OnsClasses.OnsData
Imports pdpw

Public Class InflexibilidadeDao
    Inherits BaseDAO(Of InflexibilidadeDTO)

    Public Overrides Function Listar(dataPDP As String) As List(Of InflexibilidadeDTO)
        Return Me.ListarTodos($" i.datpdp = '{dataPDP}' ")
    End Function

    Public Function ListarPor_Data(dataPDP_Inicio As DateTime, dataPDP_Fim As DateTime) As List(Of InflexibilidadeDTO)
        Return Me.ListarTodos($" i.DatPdp >= '{dataPDP_Inicio.ToString("yyyyMMdd")}' and i.DatPDP <= '{dataPDP_Fim.ToString("yyyyMMdd")}' ")
    End Function

    Public Function ListarPor_DataPDP_Empresa(dataPDP As String, codEmpresa As String) As List(Of InflexibilidadeDTO)
        Return Me.ListarTodos($" i.datpdp = '{dataPDP}' and u.CodEmpre = '{codEmpresa}'")
    End Function

    Protected Overrides Function ListarTodos(ByVal criterioWhere As String) As List(Of InflexibilidadeDTO)

        Dim lista As List(Of InflexibilidadeDTO) = New List(Of InflexibilidadeDTO)
        Dim chaveCache As String = $"Listar-{criterioWhere}"

        Try
            Dim listaCache As List(Of InflexibilidadeDTO) = Me.CacheSelect(chaveCache)
            If Not IsNothing(listaCache) Then
                Return listaCache
            End If

            Dim sql As String = "SELECT i.datpdp, 
		                                Trim(i.codusina) as CodUsina, 
		                                i.intflexi, 
		                                i.valflexiemp, 
		                                i.valflexipro, 
		                                i.valflexipre, 
		                                i.valflexisup, 
		                                i.valflexitran
                                FROM inflexibilidade i join usina u on u.CodUsina = i.CodUsina "

            If Not IsNothing(criterioWhere) AndAlso Not String.IsNullOrEmpty(criterioWhere) Then
                sql += $" Where {criterioWhere} "
            End If

            Dim rs As SqlDataReader = Me.ConsultarSQL(sql)

            Do While rs.Read
                Dim dto As InflexibilidadeDTO = New InflexibilidadeDTO()
                dto.DataPDP = rs("datpdp")
                dto.CodUsina = rs("codusina")
                dto.IntFlexi = rs("intflexi")

                '
                ' WI 155507 - Com os comandos abaixo comentados, o valor que estava considerado era o indice da coluna 
                '
                '                dto.ValFlexiEmp = IIf(Not IsDBNull(rs.GetOrdinal("valflexiemp")), Int(rs.GetOrdinal("valflexiemp")), 0)
                '                dto.ValFlexiPro = IIf(Not IsDBNull(rs.GetOrdinal("valflexipro")), Int(rs.GetOrdinal("valflexipro")), 0)
                '                dto.ValFlexiPre = IIf(Not IsDBNull(rs.GetOrdinal("valflexipre")), Int(rs.GetOrdinal("valflexipre")), 0)
                '                dto.ValFlexiSup = IIf(Not IsDBNull(rs.GetOrdinal("valflexisup")), Int(rs.GetOrdinal("valflexisup")), 0)
                '                dto.ValFlexiTran = IIf(Not IsDBNull(rs.GetOrdinal("valflexitran")), Int(rs.GetOrdinal("valflexitran")), 0)

                dto.ValFlexiEmp = If(Not IsDBNull(rs("valflexiemp")), Convert.ToInt32(rs("valflexiemp")), 0)
                dto.ValFlexiPro = If(Not IsDBNull(rs("valflexipro")), Convert.ToInt32(rs("valflexipro")), 0)
                dto.ValFlexiPre = If(Not IsDBNull(rs("valflexipre")), Convert.ToInt32(rs("valflexipre")), 0)
                dto.ValFlexiSup = If(Not IsDBNull(rs("valflexisup")), Convert.ToInt32(rs("valflexisup")), 0)
                dto.ValFlexiTran = If(Not IsDBNull(rs("valflexitran")), Convert.ToInt32(rs("valflexitran")), 0)

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
