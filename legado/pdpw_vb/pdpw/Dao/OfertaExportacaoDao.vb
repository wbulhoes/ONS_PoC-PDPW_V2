
Imports System.Collections.Generic
Imports OnsClasses.OnsData
Imports System.Linq
Imports pdpw
Imports System.Text
Imports System.Data.SqlClient

Public Class OfertaExportacaoDAO
    Inherits BaseDAO(Of OfertaExportacaoDTO)

    Public Function ListarOfertas_Reprovadas_ONS_ou_Agente(ListaAnalise As List(Of OfertaExportacaoDTO)) As List(Of OfertaExportacaoDTO)

        Dim criterios As String = ""
        Dim ListaRetorno As List(Of OfertaExportacaoDTO)

        ListaRetorno = TryCast(CObj(ListaAnalise.Where(Function(x) x.FlgAprovadoAgente = "N" Or x.FlgAprovadoONS = "N")), List(Of OfertaExportacaoDTO))





        Return ListaRetorno
        'If Not String.IsNullOrEmpty(dataPDP) Then
        '    criterios += $" datpdp = '{dataPDP}' and (flg_aprovado_ons = 'N' or flg_aprovado_agente = 'N') "
        'Else
        '    Throw TratarErro("OfertaExportacao DAO - Listar - Data PDP não informada")
        'End If

        'Return Me.ListarTodos(criterios)

    End Function

    Public Function ValiarExisteOfertaPendenteAnaliseONS(dataPDP As String) As Boolean
        Dim criterios As String = ""

        If Not String.IsNullOrEmpty(dataPDP) Then
            criterios += $" datpdp = '{dataPDP}' and flg_aprovado_ons is null "
        Else
            Throw TratarErro("OfertaExportacao DAO - Listar - Data PDP não informada")
        End If

        Return Me.ListarTodos(criterios).Count > 0
    End Function

    Public Function ListarTodasAnalises(dataPDP As String) As List(Of OfertaExportacaoDTO)

        Dim criterios As String = ""

        If Not String.IsNullOrEmpty(dataPDP) Then
            criterios += $" datpdp = '{dataPDP}' "
        Else
            Throw TratarErro("OfertaExportacao DAO - Listar - Data PDP não informada")
        End If

        Return Me.ListarTodos(criterios)

    End Function

    Public Function ValiarExisteOferta_AnaliseONS_Iniciada(dataPDP As String) As Boolean
        Dim criterios As String = ""

        If Not String.IsNullOrEmpty(dataPDP) Then
            criterios += $" datpdp = '{dataPDP}' and flg_analiseONSIniciada = 'S' "
        Else
            Throw TratarErro("OfertaExportacao DAO - Listar - Data PDP não informada")
        End If

        Me.CacheTrakingDisabled()
        Dim lista As List(Of OfertaExportacaoDTO) = Me.ListarTodos(criterios)
        Me.CacheTrakingEnabled()

        Dim existe As Boolean = lista.Count > 0

        Return existe
    End Function



    Public Overrides Function Listar(dataPDP As String) As List(Of OfertaExportacaoDTO)
        Dim criterios As String = ""

        If Not String.IsNullOrEmpty(dataPDP) Then
            criterios += $" datpdp = '{dataPDP}' "
        Else
            Throw TratarErro("OfertaExportacao DAO - Listar - Data PDP não informada")
        End If

        Return Me.ListarTodos(criterios)
    End Function

    Protected Overrides Function ListarTodos(criterioWhere As String) As List(Of OfertaExportacaoDTO)

        Dim lista As List(Of OfertaExportacaoDTO) = New List(Of OfertaExportacaoDTO)
        Dim chaveCache As String = $"Listar-{criterioWhere}"

        Try
            Dim listaCache As List(Of OfertaExportacaoDTO) = Me.CacheSelect(chaveCache)
            If Not IsNothing(listaCache) Then
                Return listaCache
            End If

            Dim sql As String =
                    "SELECT  
                    id_ofertaexportacao,  
                    datpdp, 
                    Trim(CodUsina) as Codusina,
                    Trim(codusiconversora) as CodUsiConversora, 
                    num_ordemagente,
                    num_ordemons, 
                    lgn_agenteoferta, 
                    din_oferta_exportacao, 
                    lgn_onsanalise, 
                    flg_aprovado_ons, 
                    din_analise_ons, 
                    lgn_agenteanalise, 
                    flg_aprovado_agente, 
                    din_analise_agente,
                    Lgn_ONSExportaBalanco,
                    Din_ExportaBalanco,
                    Flg_ExportaBalanco
                    FROM tb_ofertaexportacao "

            If Not IsNothing(criterioWhere) AndAlso Not String.IsNullOrEmpty(criterioWhere) Then
                sql += $" Where {criterioWhere} "
            End If

            Dim rs As SqlDataReader = Me.ConsultarSQL(sql)

            Do While rs.Read

                lista.Add(New OfertaExportacaoDTO() With
                          {
                            .IdOfertaExportacao = rs("id_ofertaexportacao"),
                            .Datpdp = rs("datpdp"),
                            .CodUsina = rs("codusina"),
                            .CodUsiConversora = rs("codusiconversora"),
                            .NumOrdemAgente = rs("num_ordemagente"),
                            .NumOrdemONS = rs("num_ordemons"),
                            .LgnAgenteOferta = rs.GetString(rs.GetOrdinal("lgn_agenteoferta")),
                            .DinOfertaExportacao = IIf(Not IsDBNull(rs("din_oferta_exportacao")), rs.GetDateTime(rs.GetOrdinal("din_oferta_exportacao")), DateTime.MinValue.AddYears(1899)),
                            .LgnOnsAnalise = IIf(Not IsDBNull(rs.GetOrdinal("lgn_onsanalise")), Convert.ToString(rs.GetOrdinal("lgn_onsanalise")), String.Empty),
                            .FlgAprovadoONS = IIf(String.IsNullOrEmpty(Convert.ToString(rs.GetOrdinal("flg_aprovado_ons"))), String.Empty, Convert.ToString(rs.GetOrdinal("flg_aprovado_ons"))),
                            .DinAnaliseONS = IIf(Not IsDBNull(rs("din_analise_ons")), rs("din_analise_ons"), DateTime.MinValue.AddYears(1899)),
                            .LgnAgenteAnalise = IIf(String.IsNullOrEmpty(Convert.ToString(rs.GetOrdinal("lgn_agenteanalise"))), String.Empty, Convert.ToString(rs.GetOrdinal("lgn_agenteanalise"))),
                            .FlgAprovadoAgente = IIf(String.IsNullOrEmpty(Convert.ToString(rs.GetOrdinal("flg_aprovado_agente"))), String.Empty, Convert.ToString(rs.GetOrdinal("flg_aprovado_agente"))),
                            .DinAnaliseAgente = IIf(Not IsDBNull(rs("din_analise_agente")), rs("din_analise_agente"), DateTime.MinValue.AddYears(1899)),
                            .LgnOnsExportadoBalanco = IIf(String.IsNullOrEmpty(Convert.ToString(rs.GetOrdinal("Lgn_ONSExportaBalanco"))), String.Empty, Convert.ToString(rs.GetOrdinal("Lgn_ONSExportaBalanco"))),
                            .DinOnsExportadoBalanco = IIf(Not IsDBNull(rs("Din_ExportaBalanco")), rs("Din_ExportaBalanco"), Nothing), ' DateTime.MinValue.AddYears(1899)),
                            .FlgExportadoBalanco = IIf(String.IsNullOrEmpty(Convert.ToString(rs("Flg_ExportaBalanco"))), String.Empty, Convert.ToString(rs("Flg_ExportaBalanco")))
                })

            Loop

            rs.Close()
            rs = Nothing

        Catch ex As Exception
            Throw TratarErro("Erro Listar OfertaExportaçãoDAO - " + ex.Message, ex)
        Finally
            Me.FecharConexao()
        End Try

        Return Me.CacheSave(chaveCache, lista)
    End Function

    Public Function ObterUltimoNumeroOrdem(dataPDP As String, Optional codEmpre As String = "") As Integer
        Dim numOrdem As Integer = 0

        Try
            Dim sql As String = $"select isNull(max(num_ordemagente),0) from tb_ofertaexportacao o
                                    join Usina u on o.CodUsina = u.CodUsina
                                    Where DatPDP = '{dataPDP}' "

            If Not IsNothing(codEmpre) And Not String.IsNullOrEmpty(codEmpre) Then
                sql += $" and u.CodEmpre = '{codEmpre}' "
            End If

            numOrdem = Me.ObterCommando(sql).ExecuteScalar()

        Catch ex As Exception
            Throw TratarErro(ex)
        Finally
            Me.FecharConexao()
        End Try

        Return numOrdem
    End Function



    <Obsolete("Utilizar métodos que executa ListarTodos(criterio)")>
    Public Function GravarOfertas(ByVal ListUsinaOferta As List(Of UsiConversDTO), ListValoresOferta As List(Of ConversoraValorOfertaDTO), ByVal loginUsuario As String) As Boolean
        Dim sucesso As Boolean = False

        Try
            Dim sql As StringBuilder = New StringBuilder()

            For Each item As UsiConversDTO In ListUsinaOferta

                Dim conver As List(Of String) = (From I In ListValoresOferta Where I.CodUsina = item.CodUsina Select I.CodConversora Distinct).ToList()

                If conver.Count > 0 Then
                    For Each ConverItem As String In conver
                        sql.AppendLine($" DELETE from tb_ofertaexportacao 
                                       where datpdp = '{ item.DatPdp }' and 
                                             codusina = '{ item.CodUsina }' and 
                                             CodUsiConversora = '{ConverItem}'; ")

                        sql.AppendLine($" DELETE from tb_valoresofertaexportacao 
                                        where datpdp = '{ item.DatPdp }' and 
                                              codusina = '{ item.CodUsina }' and 
                                              CodUsiConversora = '{ConverItem}'; ")

                        sql.AppendLine($" INSERT INTO tb_ofertaexportacao 
                                            (datpdp, 
                                             codusina, 
                                             codusiconversora, 
                                             num_ordemagente, 
                                             num_ordemons, 
                                             lgn_agenteoferta,
                                             din_oferta_exportacao) 
                                             VALUES 
                                               ('{ item.DatPdp }', 
                                                '{ item.CodUsina }', 
                                                '{ ConverItem }', 
                                                 { item.OrdemAgente }, 
                                                 { item.OrdemOns }, 
                                                '{ loginUsuario }', 
                                                '{ item.dinofertaexportacao }'); ")
                    Next
                End If
            Next

            For Each conv As ConversoraValorOfertaDTO In ListValoresOferta

                Dim valorAgente As String = "NULL"
                If Not IsNothing(conv.ValorSugeridoAgente) Then
                    valorAgente = "'" & conv.ValorSugeridoAgente & "'"
                End If

                Dim valorONS As String = "NULL"
                If Not IsNothing(conv.ValorSugeridoOns) Then
                    valorONS = "'" & conv.ValorSugeridoOns & "'"
                End If

                sql.AppendLine($" INSERT INTO tb_valoresofertaexportacao
                                (   datpdp, 
                                    codusina, 
                                    codusiconversora, 
                                    num_patamar, 
                                    val_sugeridoagente, 
                                    val_sugeridoONS)
                                    VALUES
                                   ('{ conv.DatPdp }', 
                                    '{ conv.CodUsina}', 
                                    '{ conv.CodConversora }', 
                                     { conv.Num_Patamar }, 
                                     { valorAgente }, 
                                     { valorONS } 
                                     ); ")
            Next

            If Not String.IsNullOrEmpty(sql.ToString()) Then
                Me.ExecutarSQL(sql.ToString(), False)
            End If

            sucesso = True

        Catch ex As Exception
            Throw TratarErro(ex)
        End Try

        Return sucesso

    End Function

    <Obsolete("Utilizar métodos que executa ListarTodos(criterio)")>
    Public Function ListarOfertarAgente(ByVal datPdp As String, Optional ByVal codempre As String = "", Optional ByVal _codUsina As String = "", Optional ByVal _codUsinaConversora As String = "") As List(Of UsiConversDTO)
        Dim _UsinaConversora As New List(Of UsiConversDTO)()
        Try

            Dim sql As String = ""
            '
            '  WI 103013 - Mudança da ordenção a pedido da Yasmin
            '
            sql = $"Select 
                        oferta.datpdp, 
                        Trim(oferta.codusina) as codusina , 
                        Trim(oferta.codusiconversora) as codusiconversora, 
                        oferta.num_ordemagente, 
                        oferta.num_ordemONS, 
                        Cast(isNull(valores.num_patamar,0) as int)        as num_patamar, 
                        Cast(isNull(valores.val_sugeridoagente,0) as int) as val_sugeridoagente,
                        Cast(isNull(valores.val_sugeridoons,0) as int)    as val_sugeridoons,
                        oferta.flg_aprovado_ons, 
                        oferta.flg_aprovado_agente,
                        oferta.lgn_agenteOferta,
                        us.Num_Prioridade
                        from tb_usinaConversora us
                        join usina u on u.CodUsina = us.CodUsina
                        Join tb_ofertaexportacao oferta 
                            on us.codusina = oferta.codusina
                                and us.CodUsiConversora = oferta.CodUsiConversora
                        Join tb_valoresofertaexportacao valores 
                            on oferta.codusina = valores.codusina 
                                And oferta.codusiconversora = valores.codusiconversora 
                                And oferta.datpdp = valores.datpdp 
                        where 
                        oferta.datpdp = '{ datPdp }' 
                        {IIf(String.IsNullOrEmpty(codempre.Trim()), "", " and Trim(u.codempre) = '" & codempre.Trim() & "' ") }
                        {IIf(String.IsNullOrEmpty(_codUsina.Trim()), "", " and Trim(us.codusina) = '" & _codUsina.Trim() & "' ") }
                        {IIf(String.IsNullOrEmpty(_codUsinaConversora.Trim()), "", " and Trim(us.codusiConversora) = '" & _codUsinaConversora.Trim() & "' ") }
                        order by valores.num_patamar, oferta.din_oferta_exportacao, oferta.num_ordemONS, oferta.codusina, us.Num_Prioridade "

            'Ordenação anterior: order by oferta.din_oferta_exportacao, oferta.codusina, oferta.num_ordemagente, us.Num_Prioridade "

            Dim reader As SqlDataReader = Me.ConsultarSQL(sql)

            Dim uListaUsinaConversora As List(Of ConversoraValorOfertaDTO) = New List(Of ConversoraValorOfertaDTO)
            Dim ListUsinaOferta As New List(Of UsiConversDTO)

            Do While reader.Read
                Dim uConversora As New ConversoraValorOfertaDTO
                uConversora.CodConversora = reader.GetString(reader.GetOrdinal("codusiconversora"))
                uConversora.CodUsina = reader.GetString(reader.GetOrdinal("codusina"))
                uConversora.DatPdp = reader.GetString(reader.GetOrdinal("datpdp"))

                If (Not reader.IsDBNull(reader.GetOrdinal("num_patamar"))) Then
                    uConversora.Num_Patamar = reader("num_patamar")
                Else
                    uConversora.Num_Patamar = 0
                End If

                If (Not reader.IsDBNull(reader.GetOrdinal("val_sugeridoagente"))) Then
                    uConversora.ValorSugeridoAgente = reader("val_sugeridoagente")
                Else
                    uConversora.ValorSugeridoAgente = 0
                End If

                If (Not reader.IsDBNull(reader.GetOrdinal("val_sugeridoons"))) Then
                    uConversora.ValorSugeridoOns = reader("val_sugeridoons")
                Else
                    uConversora.ValorSugeridoOns = 0
                End If

                If (Not reader.IsDBNull(reader.GetOrdinal("flg_aprovado_ons"))) Then
                    uConversora.Flgaprovadoons = reader.GetString(reader.GetOrdinal("flg_aprovado_ons"))
                Else
                    uConversora.Flgaprovadoons = String.Empty
                End If

                If (Not reader.IsDBNull(reader.GetOrdinal("flg_aprovado_agente"))) Then
                    uConversora.Flgaprovadoagente = reader.GetString(reader.GetOrdinal("flg_aprovado_agente"))
                Else
                    uConversora.Flgaprovadoagente = String.Empty
                End If

                If (Not reader.IsDBNull(reader.GetOrdinal("Lgn_AgenteOferta"))) Then
                    uConversora.Login_AgenteOferta = reader.GetString(reader.GetOrdinal("Lgn_AgenteOferta"))
                Else
                    uConversora.Login_AgenteOferta = String.Empty
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("Num_Prioridade"))) Then
                    uConversora.NumeroPrioridade = reader("Num_Prioridade")
                Else
                    uConversora.NumeroPrioridade = 0
                End If
                uListaUsinaConversora.Add(uConversora)

                Dim UsinaOferta As New UsiConversDTO

                If (Not reader.IsDBNull(reader.GetOrdinal("codusina"))) Then
                    UsinaOferta.CodUsina = reader.GetString(reader.GetOrdinal("codusina"))
                Else
                    UsinaOferta.CodUsina = String.Empty
                End If

                If (Not reader.IsDBNull(reader.GetOrdinal("codusiconversora"))) Then
                    UsinaOferta.codConversora = reader.GetString(reader.GetOrdinal("codusiconversora"))
                Else
                    UsinaOferta.codConversora = String.Empty
                End If

                If (Not reader.IsDBNull(reader.GetOrdinal("codusiconversora"))) Then
                    UsinaOferta.CodUsinaConversora = reader.GetString(reader.GetOrdinal("codusiconversora"))
                Else
                    UsinaOferta.CodUsinaConversora = String.Empty
                End If

                If (Not reader.IsDBNull(reader.GetOrdinal("datpdp"))) Then
                    UsinaOferta.DatPdp = reader.GetString(reader.GetOrdinal("datpdp"))
                Else
                    UsinaOferta.DatPdp = String.Empty
                End If

                If (Not reader.IsDBNull(reader.GetOrdinal("num_ordemagente"))) Then
                    UsinaOferta.OrdemAgente = reader("num_ordemagente")
                Else
                    UsinaOferta.OrdemAgente = 0
                End If

                If (Not reader.IsDBNull(reader.GetOrdinal("num_ordemONS"))) Then
                    UsinaOferta.OrdemOns = reader("num_ordemONS")
                Else
                    UsinaOferta.OrdemOns = 0
                End If

                If (Not reader.IsDBNull(reader.GetOrdinal("Lgn_AgenteOferta"))) Then
                    UsinaOferta.Login_AgenteOferta = reader.GetString(reader.GetOrdinal("Lgn_AgenteOferta"))
                Else
                    UsinaOferta.Login_AgenteOferta = String.Empty
                End If

                If (Not reader.IsDBNull(reader.GetOrdinal("Num_Prioridade"))) Then
                    UsinaOferta.NumeroPrioridade = reader("Num_Prioridade")
                Else
                    UsinaOferta.NumeroPrioridade = 0
                End If

                ListUsinaOferta.Add(UsinaOferta)
            Loop

            'UsinaConversora 
            Dim UvConv As Object = New Object
            UvConv = (From I In ListUsinaOferta Select I.CodUsina, I.codConversora, I.OrdemAgente, I.OrdemOns, I.flgaprovadoagente, I.flgaprovadoons, I.Login_AgenteOferta Distinct).ToList

            For Each item As Object In UvConv
                Dim Oferta As UsiConversDTO = New UsiConversDTO
                Oferta.ValoresOfertaUsiConversora = New List(Of ConversoraValorOfertaDTO)
                Oferta.CodUsina = item.CodUsina
                Oferta.codConversora = item.codConversora
                Oferta.CodUsinaConversora = item.codConversora
                Oferta.OrdemAgente = item.OrdemAgente
                Oferta.OrdemOns = item.OrdemOns
                Oferta.flgaprovadoons = item.flgaprovadoons
                Oferta.flgaprovadoagente = item.flgaprovadoagente
                Oferta.Login_AgenteOferta = item.Login_AgenteOferta
                Oferta.ValoresOfertaUsiConversora = (From I In uListaUsinaConversora Where I.CodUsina = Oferta.CodUsina And I.CodConversora = Oferta.codConversora Select I).ToList

                _UsinaConversora.Add(Oferta)
            Next

        Catch ex As Exception
            Throw TratarErro(ex)
        Finally
            Me.FecharConexao()
        End Try

        Return _UsinaConversora

    End Function

    Public Function ListarOfertasAgrupadasPorConversoras(ByVal datPdp As String) As List(Of ConversoraValorOfertaDTO)
        Dim _ListaUsinaConversora As New List(Of ConversoraValorOfertaDTO)()
        Try

            Dim sql As String = ""
            '
            ' WI 92105 e WI 100386: Fazer o 1º e 2º arredondamento dos valores das conversoras na query
            '
            sql = $"Select 
                        oferta.datpdp, 
                        Trim(oferta.codusiconversora) as codusiconversora, 
                        isNull(valores.num_patamar,0) as num_patamar, 
                        SUM(ISNULL(valores.val_sugeridoons,0) -
                            ROUND((ISNULL(valores.val_sugeridoons,0) * ROUND(us.pct_perda/100,2)),0)) as soma_sugeridoons
                        from tb_usinaConversora us
                        join usina u on u.CodUsina = us.CodUsina
                        Join tb_ofertaexportacao oferta 
                            on us.codusina = oferta.codusina
                                and us.CodUsiConversora = oferta.CodUsiConversora
                        Join tb_valoresofertaexportacao valores 
                            on oferta.codusina = valores.codusina 
                                And oferta.codusiconversora = valores.codusiconversora 
                                And oferta.datpdp = valores.datpdp 
                        where 
                            oferta.datpdp = '{ datPdp }'
                        group by oferta.datpdp, oferta.codusiconversora, valores.num_patamar
                        order by oferta.datpdp, oferta.codusiconversora, valores.num_patamar "

            Dim reader As SqlDataReader = Me.ConsultarSQL(sql)

            Do While reader.Read
                Dim uConversora As New ConversoraValorOfertaDTO
                uConversora.CodConversora = reader("codusiconversora")
                uConversora.DatPdp = reader.GetString(reader.GetOrdinal("datpdp"))

                If (Not reader.IsDBNull(reader.GetOrdinal("num_patamar"))) Then
                    uConversora.Num_Patamar = reader("num_patamar")
                Else
                    uConversora.Num_Patamar = 0
                End If

                If (Not reader.IsDBNull(reader.GetOrdinal("soma_sugeridoons"))) Then
                    uConversora.ValorLiquidoOns = reader.GetDouble(reader.GetOrdinal("soma_sugeridoons"))
                Else
                    uConversora.ValorLiquidoOns = 0
                End If

                _ListaUsinaConversora.Add(uConversora)
            Loop

        Catch ex As Exception
            Throw TratarErro(ex)
        Finally
            Me.FecharConexao()
        End Try

        Return _ListaUsinaConversora

    End Function

    Public Function ListarOfertasAgrupadasPorConversorasUsinasEPercentualPerca(ByVal datPdp As String) As List(Of ConversoraValorOfertaDTO)
        Dim _ListaUsinaConversora As New List(Of ConversoraValorOfertaDTO)()
        Try

            Dim sql As String = ""
            '
            ' WI 92105 e WI 100386: Fazer o 1º e 2º arredondamento dos valores das conversoras na query
            '
            sql = $"Select 
                        oferta.datpdp, 
                        Trim(oferta.codusiconversora) as codusiconversora, 
                        u.codusina as codusina,
                        isNull(valores.num_patamar,0) as num_patamar, 
                        SUM(ISNULL(valores.val_sugeridoons,0) -
                            ROUND((ISNULL(valores.val_sugeridoons,0) * ROUND(us.pct_perda/100,2)),0)) as soma_sugeridoons,
                        us.pct_perda
                        from tb_usinaConversora us
                        join usina u on u.CodUsina = us.CodUsina
                        Join tb_ofertaexportacao oferta 
                            on us.codusina = oferta.codusina
                                and us.CodUsiConversora = oferta.CodUsiConversora
                        Join tb_valoresofertaexportacao valores 
                            on oferta.codusina = valores.codusina 
                                And oferta.codusiconversora = valores.codusiconversora 
                                And oferta.datpdp = valores.datpdp 
                        where 
                            oferta.datpdp = '{ datPdp }'
                        group by oferta.datpdp, oferta.codusiconversora, valores.num_patamar, u.codusina, us.pct_perda
                        order by oferta.datpdp, oferta.codusiconversora, valores.num_patamar "

            Dim reader As SqlDataReader = Me.ConsultarSQL(sql)

            Do While reader.Read
                Dim uConversora As New ConversoraValorOfertaDTO
                uConversora.CodConversora = reader("codusiconversora")
                uConversora.CodUsina = reader("codusina")
                uConversora.DatPdp = reader.GetString(reader.GetOrdinal("datpdp"))

                If (Not reader.IsDBNull(reader.GetOrdinal("num_patamar"))) Then
                    uConversora.Num_Patamar = reader("num_patamar")
                Else
                    uConversora.Num_Patamar = 0
                End If

                If (Not reader.IsDBNull(reader.GetOrdinal("soma_sugeridoons"))) Then
                    uConversora.ValorLiquidoOns = reader.GetDouble(reader.GetOrdinal("soma_sugeridoons"))
                Else
                    uConversora.ValorLiquidoOns = 0
                End If

                If (Not reader.IsDBNull(reader.GetOrdinal("pct_perda"))) Then
                    uConversora.valorPerdas = reader.GetDouble(reader.GetOrdinal("pct_perda"))
                Else
                    uConversora.valorPerdas = 0
                End If

                _ListaUsinaConversora.Add(uConversora)
            Loop

        Catch ex As Exception
            Throw TratarErro(ex)
        Finally
            Me.FecharConexao()
        End Try

        Return _ListaUsinaConversora

    End Function

    <Obsolete("Utilizar métodos que executa ListarTodos(criterio)")>
    Public Function ListarOfertaExportacao(Optional ByVal idOfertaExportacao As Integer = 0, Optional ByVal dataPDP As String = "", Optional ByVal codUsina As String = "", Optional ByVal codUsinaConversora As String = "") As List(Of OfertaExportacaoDTO)
        Dim listaOfertaExportacao As List(Of OfertaExportacaoDTO) = New List(Of OfertaExportacaoDTO)

        Try
            Dim sql As String = ""

            sql += "SELECT " &
                    "id_ofertaexportacao, " &
                    "datpdp, " &
                    "codusina, " &
                    "codusiconversora, " &
                    "num_ordemagente, " &
                    "num_ordemons, " &
                    "lgn_agenteoferta, " &
                    "din_oferta_exportacao, " &
                    "lgn_onsanalise, " &
                    "flg_aprovado_ons, " &
                    "din_analise_ons, " &
                    "lgn_agenteanalise, " &
                    "flg_aprovado_agente, " &
                    "din_analise_agente " &
                "FROM tb_ofertaexportacao " &
                "where 1=1 "

            If idOfertaExportacao <> 0 Then
                sql += " and id_ofertaexportacao = " & idOfertaExportacao & ""
            End If

            If Not String.IsNullOrEmpty(dataPDP) Then
                sql += " and datpdp = '" & dataPDP & "'"
            End If

            If Not String.IsNullOrEmpty(codUsina) Then
                sql += " and codusina = '" & codUsina & "'"
            End If

            If Not String.IsNullOrEmpty(codUsinaConversora) Then
                sql += " and codusiconversora = '" & codUsinaConversora & "'"
            End If

            Dim rsOfertaExportacao As SqlDataReader = Me.ConsultarSQL(sql)

            Do While rsOfertaExportacao.Read
                Dim ofertaExportacao As OfertaExportacaoDTO = New OfertaExportacaoDTO()
                ofertaExportacao.IdOfertaExportacao = rsOfertaExportacao("id_ofertaexportacao")
                ofertaExportacao.Datpdp = rsOfertaExportacao("datpdp")
                ofertaExportacao.CodUsina = rsOfertaExportacao("codusina")

                If (Not IsNothing(rsOfertaExportacao("codusiconversora"))) Then
                    ofertaExportacao.CodUsiConversora = rsOfertaExportacao("codusiconversora")
                Else
                    ofertaExportacao.CodUsiConversora = String.Empty
                End If

                If (Not rsOfertaExportacao.IsDBNull(rsOfertaExportacao.GetOrdinal("num_ordemagente"))) Then
                    ofertaExportacao.NumOrdemAgente = rsOfertaExportacao("num_ordemagente")
                Else
                    ofertaExportacao.NumOrdemAgente = 0
                End If

                If (Not rsOfertaExportacao.IsDBNull(rsOfertaExportacao.GetOrdinal("num_ordemons"))) Then
                    ofertaExportacao.NumOrdemONS = rsOfertaExportacao("num_ordemons")
                Else
                    ofertaExportacao.NumOrdemONS = 0
                End If

                If (Not rsOfertaExportacao.IsDBNull(rsOfertaExportacao.GetOrdinal("lgn_agenteoferta"))) Then
                    ofertaExportacao.LgnAgenteOferta = rsOfertaExportacao.GetString(rsOfertaExportacao.GetOrdinal("lgn_agenteoferta"))
                Else
                    ofertaExportacao.LgnAgenteOferta = String.Empty
                End If

                If (Not rsOfertaExportacao.IsDBNull(rsOfertaExportacao.GetOrdinal("din_oferta_exportacao"))) Then
                    ofertaExportacao.DinOfertaExportacao = rsOfertaExportacao.GetDateTime(rsOfertaExportacao.GetOrdinal("din_oferta_exportacao"))
                End If

                If (Not rsOfertaExportacao.IsDBNull(rsOfertaExportacao.GetOrdinal("lgn_onsanalise"))) Then
                    ofertaExportacao.LgnOnsAnalise = rsOfertaExportacao.GetString(rsOfertaExportacao.GetOrdinal("lgn_onsanalise"))
                End If

                If (Not IsNothing(rsOfertaExportacao("flg_aprovado_ons"))) Then
                    ofertaExportacao.FlgAprovadoONS = rsOfertaExportacao("flg_aprovado_ons").ToString()
                Else
                    ofertaExportacao.FlgAprovadoONS = String.Empty
                End If


                If (Not rsOfertaExportacao.IsDBNull(rsOfertaExportacao.GetOrdinal("din_analise_ons"))) Then
                    ofertaExportacao.DinAnaliseONS = rsOfertaExportacao.GetDateTime(rsOfertaExportacao.GetOrdinal("din_analise_ons"))
                End If

                If (Not rsOfertaExportacao.IsDBNull(rsOfertaExportacao.GetOrdinal("lgn_agenteanalise"))) Then
                    ofertaExportacao.LgnAgenteAnalise = rsOfertaExportacao.GetString(rsOfertaExportacao.GetOrdinal("lgn_agenteanalise"))
                Else
                    ofertaExportacao.LgnAgenteAnalise = String.Empty
                End If

                If (Not rsOfertaExportacao.IsDBNull(rsOfertaExportacao.GetOrdinal("flg_aprovado_agente"))) Then
                    ofertaExportacao.FlgAprovadoAgente = rsOfertaExportacao.GetString(rsOfertaExportacao.GetOrdinal("flg_aprovado_agente"))
                Else
                    ofertaExportacao.FlgAprovadoAgente = String.Empty
                End If

                If (Not rsOfertaExportacao.IsDBNull(rsOfertaExportacao.GetOrdinal("din_analise_agente"))) Then
                    ofertaExportacao.DinAnaliseAgente = rsOfertaExportacao.GetDateTime(rsOfertaExportacao.GetOrdinal("din_analise_agente"))
                End If

                listaOfertaExportacao.Add(ofertaExportacao)
            Loop

            rsOfertaExportacao.Close()
            rsOfertaExportacao = Nothing

        Catch ex As Exception
            Throw TratarErro("Erro Listar OfertaExportação - " + ex.Message, ex)
        Finally
            Me.FecharConexao()
        End Try

        Return listaOfertaExportacao
    End Function

    Public Function ValidarUsinasComLimiteDePotenciaViolado(ByVal datPdp As String) As String
        Dim msgsErro As String = ""
        Try

            Dim sql As String = ""

            sql = $"select u.codusina, d.intdespa, u.potinstalada, d.valdespasup,v.soma_sugeridoons, v.soma_sugeridoagente 
                    from despa d, 
	                     (select v.datpdp, v.codusina, v.num_patamar, sum(v.val_sugeridoons) soma_sugeridoons, sum(v.val_sugeridoons) soma_sugeridoagente
		                    from tb_valoresofertaexportacao v,
			                     tb_ofertaexportacao o
		                    where v.datpdp=o.datpdp and v.codusina=o.codusina and v.codusiconversora = o.codusiconversora and
			                      v.datpdp = '{ datPdp }' and 
			                      (isNull(o.flg_aprovado_ons,'S') <> 'N') 
		                    group by v.datpdp, v.codusina, v.num_patamar) as v, 
	                     usina u
                    where d.codusina = v.codusina and d.datpdp = v.datpdp and d.intdespa = v.num_patamar and 
	                      u.codusina  = d.codusina and
	                      d.datpdp = '{ datPdp }' and 
	                      v.soma_sugeridoons > u.potinstalada
                    order by u.codusina, d.intdespa"

            Dim reader As SqlDataReader = Me.ConsultarSQL(sql)

            Do While reader.Read
                msgsErro += $" | (Usina {reader("codusina")}, Patamar {reader("intdespa")}, [Valor Ref. {reader("valdespasup")} Valor Exportação {reader("soma_sugeridoons")}], Limite potência {reader("potinstalada")})"
            Loop

        Catch ex As Exception
            Throw TratarErro(ex)
        Finally
            Me.FecharConexao()
        End Try

        Return msgsErro

    End Function

    Public Function ValidarUsinasConversorasComLimiteDePotenciaViolado(ByVal datPdp As String) As String
        Dim msgsErro As String = ""
        Try

            Dim sql As String = ""

            sql = $"select v.datpdp, u.codusina, u.potinstalada, v.num_patamar, d.valdespasup, 
						v.soma_sugeridoons_sem_perdas
                    from despa d, 
                         (select v.datpdp, v.codusiconversora, v.num_patamar, 
                         		sum(cast(v.val_sugeridoons - 
                         			round((v.val_sugeridoons * (round(uc.pct_perda, 0)/100)),0) as int)) as soma_sugeridoons_sem_perdas 
		                    from tb_valoresofertaexportacao v,
			                     tb_ofertaexportacao o,
	                     		 tb_usinaconversora uc
		                    where v.datpdp=o.datpdp and v.codusina=o.codusina and v.codusiconversora = o.codusiconversora and
		                    	  uc.codusina = o.codusina and uc.codusiconversora = o.codusiconversora and
			                      v.datpdp = '{ datPdp }' and 
			                      (isNull(o.flg_aprovado_ons,'S') <> 'N')
		                    group by v.datpdp, v.codusiconversora, v.num_patamar) as v, 
	                     usina u
                    where d.codusina = v.codusiconversora and d.datpdp = v.datpdp and d.intdespa = v.num_patamar and 
	                      u.codusina  = d.codusina and
	                      d.datpdp = '{ datPdp }' and
	                   	  v.soma_sugeridoons_sem_perdas > u.potinstalada
					order by u.codusina, v.num_patamar"

            Dim reader As SqlDataReader = Me.ConsultarSQL(sql)

            Do While reader.Read
                msgsErro += $" | (Conversora {reader("codusina")}, Patamar {reader("num_patamar")}, [Valor Ref. {reader("valdespasup")} Valor Exportação {reader("soma_sugeridoons_sem_perdas")}], Limite potência {reader("potinstalada")})"
            Loop

        Catch ex As Exception
            Throw TratarErro(ex)
        Finally
            Me.FecharConexao()
        End Try

        Return msgsErro

    End Function

    <Obsolete("Utilizar métodos que executa ListarTodos(criterio)")>
    Public Function ExisteOfertaExportacaoFutura(ByVal dataPdp As String, ByVal codUsina As String, ByVal codUsiConversora As String) As Boolean
        Dim quantidadeOferta As Integer = 0
        Dim existeOferta As Boolean = False

        Try
            Dim sql As String = $"Select count(*) from tb_ofertaexportacao where datpdp > '{dataPdp}' and codusina = '{codUsina}' and codusiconversora = '{codUsiConversora}' "

            quantidadeOferta = Me.ObterCommando(sql).ExecuteScalar()

            If quantidadeOferta > 0 Then
                existeOferta = True
            End If

        Catch ex As Exception
            Throw TratarErro(ex)
        Finally
            Me.FecharConexao()
        End Try

        Return existeOferta

    End Function

End Class
