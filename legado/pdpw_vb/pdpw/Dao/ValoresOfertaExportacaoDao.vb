Imports System.Collections.Generic
Imports OnsClasses.OnsData
Imports pdpw
Imports System.Linq
Imports System.Data.SqlClient

Public Class ValorOfertaExportacaoDAO
    Inherits BaseDAO(Of ValorOfertaExportacaoDTO)

    Public Function ExisteOfertaParaDataPdpSelecionada(ByVal dataPdp As String, ListaValorExportacao As List(Of ValorOfertaExportacaoDTO)) As Boolean
        'Dim listaValoresOfertaExportacao As List(Of ValorOfertaExportacaoDTO) = New List(Of ValorOfertaExportacaoDTO)
        Dim existeOferta As Boolean = False

        Try
            'Dim sql As String = ""
            'sql += $" select count(id_valoresofertaexportacao) 
            '       from tb_valoresofertaexportacao where datPdp = '{dataPdp}' "

            'Dim resultado As Integer = Me.ObterCommando(sql).ExecuteScalar()
            Dim resultado As Integer = ListaValorExportacao.Count
            If resultado > 0 Then
                existeOferta = True
            End If

        Catch ex As Exception
            Throw TratarErro(ex)
        Finally
            Me.FecharConexao()
        End Try

        Return existeOferta

    End Function

    Public Function ExisteValorDeReferencia(ByVal dataPdp As String, ListaValorExportacao As List(Of ValorOfertaExportacaoDTO)) As Boolean

        'Dim listaValoresOfertaExportacao As List(Of ValorOfertaExportacaoDTO) = New List(Of ValorOfertaExportacaoDTO)
        Dim resultado As String = ""
        Dim possuiValorDeReferencia As Boolean = False

        Try
            'Dim sql As String = $"select 
            '                        first 1 val_Refusina
            '                    from tb_valoresofertaexportacao where datpdp = '{dataPdp}' "

            'Dim Cmd As OnsCommand = Me.ObterCommando(sql)
            'resultado = IIf(Not IsDBNull(Cmd.ExecuteScalar()), Cmd.ExecuteScalar(), Nothing)
            resultado = IIf(ListaValorExportacao.FirstOrDefault().ValDespaUsinaRef, ListaValorExportacao.FirstOrDefault().ValDespaUsinaRef, Nothing)
            If Not IsNothing(resultado) Then
                possuiValorDeReferencia = True
            End If

        Catch ex As Exception
            Throw TratarErro(ex)
        Finally
            Me.FecharConexao()
        End Try

        Return possuiValorDeReferencia

    End Function

    Public Overrides Function Listar(dataPDP As String) As List(Of ValorOfertaExportacaoDTO)
        Dim criterios As String = ""

        If Not String.IsNullOrEmpty(dataPDP) Then
            criterios += $" v.datpdp = '{dataPDP}'"
        Else
            Throw TratarErro("ValoresOfertaExportacaoDao - Listar - Data PDP não informada")
        End If

        Return Me.ListarTodos(criterios)
    End Function

    Protected Overrides Function ListarTodos(criterioWhere As String) As List(Of ValorOfertaExportacaoDTO)

        Dim lista As List(Of ValorOfertaExportacaoDTO) = New List(Of ValorOfertaExportacaoDTO)
        Dim chaveCache As String = $"Listar-{criterioWhere}"

        Try
            Dim listaCache As List(Of ValorOfertaExportacaoDTO) = Me.CacheSelect(chaveCache)
            If Not IsNothing(listaCache) Then
                Return listaCache
            End If

            Dim sql As String = "select 
                                v.id_valoresofertaexportacao, 
                                v.datpdp, 
                                Trim(v.codusina) as CodUsina, 
                                Trim(v.codusiconversora) as CodUsiConversora, 
                                v.num_patamar, 
                                v.val_sugeridoagente, 
                                v.val_sugeridoons, 
                                ISNULL(v.val_RefUsina, 0) as val_RefUsina, 
                                ISNULL(v.val_RefUsiConversora, 0) as val_RefUsiConversora, 
                                ISNULL(v.val_RefExporta, 0) as val_RefExporta, 
                                ISNULL(v.val_RefPerda, 0) as val_RefPerda,
                                ISNULL(v.Val_RefCargaUsiConversora, 0) as Val_RefCargaUsiConversora
                                from tb_valoresofertaexportacao v
                                inner join tb_OfertaExportacao o 
                                    on o.CodUsina = v.CodUsina and 
                                    o.CodUsiConversora = v.CodUsiConversora and 
                                    o.DatPDP = v.DatPDP "

            If Not IsNothing(criterioWhere) AndAlso Not String.IsNullOrEmpty(criterioWhere) Then
                sql += $" Where {criterioWhere} "
            End If

            Dim rs As SqlDataReader = Me.ConsultarSQL(sql)

            Do While rs.Read
                lista.Add(New ValorOfertaExportacaoDTO() With
                          {
                            .IdValoresOfertaExportacao = rs("id_valoresofertaexportacao"),
                            .Datpdp = rs("datpdp"),
                            .CodUsina = rs("codusina"),
                            .CodUsiConversora = rs("codusiconversora"),
                            .NumPatamar = rs("num_patamar"),
                            .ValSugeridoAgente = If(Not IsNothing(rs("val_sugeridoagente")) And Not (rs("val_sugeridoagente").ToString() = ""), Convert.ToInt32(rs("val_sugeridoagente")), 0),
                            .ValSugeridoONS = If(Not IsNothing(rs("val_sugeridoons")), Convert.ToInt32(rs("val_sugeridoons")), rs("val_sugeridoons")),
                            .ValDespaUsinaRef = If(Not IsNothing(rs("val_RefUsina")), Convert.ToInt32(rs("val_RefUsina")), rs("val_RefUsina")),
                            .ValDespaUsiConversoraRef = If(Not IsNothing(rs("val_RefUsiConversora")), Convert.ToInt32(rs("val_RefUsiConversora")), rs("val_RefUsiConversora")),
                            .ValExportaRef = If(Not IsNothing(rs("val_RefExporta")), Convert.ToInt32(rs("val_RefExporta")), rs("val_RefExporta")),
                            .ValPerdaRef = If(Not IsNothing(rs("val_RefPerda")), Convert.ToInt32(rs("val_RefPerda")), rs("val_RefPerda")),
                            .ValCargaUsiConversoraRef = If(Not IsNothing(rs("val_RefCargaUsiConversora")), Convert.ToInt32(rs("val_RefCargaUsiConversora")), rs("val_RefCargaUsiConversora"))
                          })
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
