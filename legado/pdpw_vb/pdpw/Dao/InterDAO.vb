Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports OnsClasses.OnsData
Imports pdpw

Public Class InterDAO
    Inherits BaseDAO(Of InterDTO)

    Public Overrides Function Listar(dataPDP As String) As List(Of InterDTO)
        If String.IsNullOrEmpty(dataPDP) Then
            Throw New NullReferenceException("InterDAO - Listar - Data PDP não informada")
        End If

        Return Me.ListarTodos($" datpdp = '{dataPDP}' ")
    End Function

    Protected Overrides Function ListarTodos(criterioWhere As String) As List(Of InterDTO)
        Dim lista As List(Of InterDTO) = New List(Of InterDTO)
        Dim chaveCache As String = $"Listar-{criterioWhere}"

        Try

            Dim listaCache As List(Of InterDTO) = Me.CacheSelect(chaveCache)
            If Not IsNothing(listaCache) Then
                Return listaCache
            End If

            Dim sql As String = "select datpdp, 
                                    Trim(codEmpreDe) as CodEmpreDe, 
                                    Trim(codEmprePara) as CodEmprePara, 
                                    Trim(codContaDe) as CodContaDe, 
                                    Trim(codContaPara) as CodContaPara, 
                                    Trim(codContaModal) as CodContaModal, 
                                    intInter, 
                                    TipInter, 
                                    valInteremp, 
                                    valInterpro, 
                                    valInterpre, 
                                    valIntersup, 
                                    valIntertran
                                    from Inter "

            If Not IsNothing(criterioWhere) AndAlso Not String.IsNullOrEmpty(criterioWhere) Then
                sql += $" Where {criterioWhere} "
            End If

            Dim rs As SqlDataReader = Me.ConsultarSQL(sql)

            Do While rs.Read
                Dim dto As New InterDTO
                dto.DataPDP = rs.GetString(rs.GetOrdinal("datpdp"))
                dto.CodEmpreDe = rs.GetString(rs.GetOrdinal("CodEmpreDe"))
                dto.CodEmprePara = rs.GetString(rs.GetOrdinal("CodEmprePara"))
                dto.CodContaDe = rs.GetString(rs.GetOrdinal("CodContaDe"))
                dto.CodContaPara = rs.GetString(rs.GetOrdinal("CodContaPara"))
                dto.CodContaModal = rs.GetString(rs.GetOrdinal("CodContaModal"))
                dto.Patamar = rs.GetInt32(rs.GetOrdinal("intInter"))
                dto.TipInter = rs.GetString(rs.GetOrdinal("TipInter"))
                dto.ValInterEmp = rs.GetInt32(rs.GetOrdinal("valInteremp"))
                dto.ValInterPro = rs.GetInt32(rs.GetOrdinal("valInterpro"))
                dto.ValInterPre = rs.GetInt32(rs.GetOrdinal("valInterpre"))
                dto.ValInterSup = rs.GetInt32(rs.GetOrdinal("valIntersup"))
                dto.ValInterTran = rs.GetInt32(rs.GetOrdinal("valIntertran"))

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
