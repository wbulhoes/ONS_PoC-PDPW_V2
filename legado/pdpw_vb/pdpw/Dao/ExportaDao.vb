Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports OnsClasses.OnsData
Imports pdpw

Public Class ExportaDAO
    Inherits BaseDAO(Of ExportaDTO)

    Public Overrides Function Listar(dataPDP As String) As List(Of ExportaDTO)

        If String.IsNullOrEmpty(dataPDP) Then
            Throw New NullReferenceException("ExportaDAO - Listar - Data PDP não informada")
        End If

        Return Me.ListarTodos($" datpdp = '{dataPDP}' ")
    End Function

    Protected Overrides Function ListarTodos(criterioWhere As String) As List(Of ExportaDTO)
        Dim listaExporta As List(Of ExportaDTO) = New List(Of ExportaDTO)
        Dim chaveCache As String = $"Listar-{criterioWhere}"

        Try

            Dim listaCache As List(Of ExportaDTO) = Me.CacheSelect(chaveCache)
            If Not IsNothing(listaCache) Then
                Return listaCache
            End If

            Dim sql As String = "select datpdp, 
                                        Trim(codusina) as CodUsina, 
                                        intexporta, 
                                        valexportatran, 
                                        valexportaemp, 
                                        valexportapro, 
                                        valexportapre, 
                                        valexportasup
                                      from exporta "

            If Not IsNothing(criterioWhere) AndAlso Not String.IsNullOrEmpty(criterioWhere) Then
                sql += $" Where {criterioWhere} "
            End If

            Dim rsValExporta As SqlDataReader = Me.ConsultarSQL(sql)

            Do While rsValExporta.Read
                Dim exportaDTO As New ExportaDTO
                exportaDTO.DataPDP = rsValExporta.GetString(rsValExporta.GetOrdinal("datpdp"))
                exportaDTO.CodUsina = rsValExporta.GetString(rsValExporta.GetOrdinal("codusina"))
                exportaDTO.Patamar = rsValExporta.GetInt32(rsValExporta.GetOrdinal("intexporta"))
                exportaDTO.ValExportaEmp = rsValExporta.GetInt32(rsValExporta.GetOrdinal("valexportaemp"))
                exportaDTO.ValExportaPro = rsValExporta.GetInt32(rsValExporta.GetOrdinal("valexportapro"))
                exportaDTO.ValExportaPre = rsValExporta.GetInt32(rsValExporta.GetOrdinal("valexportapre"))
                exportaDTO.ValExportaTran = rsValExporta.GetInt32(rsValExporta.GetOrdinal("valexportatran"))
                exportaDTO.ValExportaSUP = rsValExporta.GetInt32(rsValExporta.GetOrdinal("valexportasup"))

                listaExporta.Add(exportaDTO)
            Loop

            rsValExporta.Close()
            rsValExporta = Nothing

        Catch ex As Exception
            Throw TratarErro("Erro Listar - " + ex.Message, ex)
        Finally
            Me.FecharConexao()
        End Try

        Return Me.CacheSave(chaveCache, listaExporta)
    End Function

End Class
