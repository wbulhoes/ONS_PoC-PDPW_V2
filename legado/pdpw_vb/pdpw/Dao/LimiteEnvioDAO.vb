Imports System.Collections.Generic
Imports OnsClasses.OnsData
Imports System.Linq
Imports pdpw
Imports System.Data.SqlClient

Public Class LimiteEnvioDAO
    Inherits BaseDAO(Of LimiteEnvioDTO)

    Public Overrides Function Listar(dataPDP As String) As List(Of LimiteEnvioDTO)
        Return Me.ListarTodos($" datpdp = '{dataPDP.Trim()}' ")
    End Function

    Public Function ListarPor_DataPDP_Empresa(dataPDP As String, codEmpresa As String) As List(Of LimiteEnvioDTO)
        Return Me.ListarTodos($" datpdp = '{dataPDP}' and CodEmpre = '{codEmpresa}'")
    End Function

    Public Function ObterPor_DataPDP_Empresa_TipoEnvio(dataPDP As String, codEmpresa As String, ByVal tipoEnvio As TipoEnvio) As LimiteEnvioDTO
        Dim tipStatus As Integer = CInt(tipoEnvio)
        Return Me.ListarTodos($" datpdp = '{dataPDP}' and CodEmpre = '{codEmpresa}' and tip_status = {tipStatus}").FirstOrDefault()
    End Function

    Protected Overrides Function ListarTodos(criterioWhere As String) As List(Of LimiteEnvioDTO)

        Dim lista As List(Of LimiteEnvioDTO) = New List(Of LimiteEnvioDTO)
        Dim chaveCache As String = $"Listar-{criterioWhere}"

        Try
            Dim listaCache As List(Of LimiteEnvioDTO) = Me.CacheSelect(chaveCache)
            If Not IsNothing(listaCache) Then
                Return listaCache
            End If

            Dim sql As String = "select 
                                    datpdp,
	                                codempre,
	                                hor_limite,
	                                tip_status,
	                                dat_limite
                                from tb_limiteenviocomentariodessemage "

            If Not IsNothing(criterioWhere) AndAlso Not String.IsNullOrEmpty(criterioWhere) Then
                sql += $" Where {criterioWhere} "
            End If

            Dim rs As SqlDataReader = Me.ConsultarSQL(sql)

            Do While rs.Read

                Dim newLimite As New LimiteEnvioDTO()

                newLimite.DataPDP = rs("datpdp")

                newLimite.CodEmpre = rs("codempre")

                newLimite.Hora_Limite = Convert.ToDateTime(Date.Now.ToString("dd/MM/yyyy") + " " + IIf(Not String.IsNullOrEmpty(rs("hor_limite")),
                                                                                                  Convert.ToDateTime(rs("hor_limite")).ToShortTimeString(),
                                                                                                  "23:59"))
                newLimite.TipoStatus = rs("tip_status")

                If (Not rs("dat_limite") Is Nothing) Then
                    If (rs("dat_limite").ToString() <> "") Then
                        newLimite.Data_Limite = Convert.ToDateTime(rs("dat_limite"))
                    End If
                End If
                lista.Add(newLimite)
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
