
Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports OnsClasses.OnsData
Imports pdpw

Public Class UsinaConversoraDAO
    Inherits BaseDAO(Of UsinaConversoraDTO)
    Public Overrides Function Listar(dataPDP As String) As List(Of UsinaConversoraDTO)
        Dim criterios As String = ""

        If Not String.IsNullOrEmpty(dataPDP) Then
            criterios += $" o.datpdp = '{dataPDP}' "
        Else
            Throw Me.TratarErro("UsinaConversoraDAO - Listar - Data PDP não informada")
        End If

        Return Me.ListarTodos(criterios)
    End Function

    Protected Overrides Function ListarTodos(criterioWhere As String) As List(Of UsinaConversoraDTO)

        Dim lista As List(Of UsinaConversoraDTO) = New List(Of UsinaConversoraDTO)
        Dim chaveCache As String = $"Listar-{criterioWhere}"

        Try
            Dim listaCache As List(Of UsinaConversoraDTO) = Me.CacheSelect(chaveCache)
            If Not IsNothing(listaCache) Then
                Return listaCache
            End If

            Dim sql As String = $"select 
                                    Distinct
                                uc.id_usinaconversora, 
                                Trim(uc.codusina) as CodUsina, 
                                Trim(uc.codusiconversora) as CodUsiConversora, 
                                uc.pct_perda,
                                ISNULL(usi.PotInstalada,-1) as PotInstaladaUsina,
                                ISNULL(conv.PotInstalada,-1) as PotInstaladaConversora,
                                uc.num_prioridade
                                from tb_usinaconversora uc
                                LEFT JOIN 
                                    tb_OfertaExportacao o 
                                    on o.CodUsina = uc.CodUsina and o.codusiconversora = uc.codusiconversora 
                                Join Usina usi on usi.CodUsina = uc.CodUsina
                                Join Usina conv on conv.CodUsina = uc.CodUsiConversora
                                "

            If Not IsNothing(criterioWhere) AndAlso Not String.IsNullOrEmpty(criterioWhere) Then
                sql += $" Where {criterioWhere} "
            End If

            Dim rs As SqlDataReader = Me.ConsultarSQL(sql)

            Do While rs.Read

                lista.Add(New UsinaConversoraDTO() With
                        {
                            .IdUsinaConversora = rs("id_usinaconversora"),
                            .CodUsina = rs("codusina"),
                            .CodUsiConversora = rs("codusiconversora"),
                            .PercentualPerda = rs("pct_perda"),
                            .PotInstaladaUsina = rs("PotInstaladaUsina"),
                            .PotInstaladaConversora = rs("PotInstaladaConversora"),
                            .NumeroPrioridade = rs("Num_Prioridade")
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

    <Obsolete("Utilizar métodos que executa ListarTodos(criterio)")>
    Public Function ListarUsinas(ByVal codEmpre As String, ByVal tipoUsina As String, Optional listaUsinaAprovadaOfertaExportacao As String = "") As List(Of UsiConversDTO)

        Dim listaUsina As List(Of UsiConversDTO) = New List(Of UsiConversDTO)

        Try
            If Not IsNothing(codEmpre) AndAlso Not String.IsNullOrEmpty(codEmpre.Trim()) Then

                Dim sql As String = $"select codusina, nomusina from usina where tpusina_id = '{tipoUsina}' and codempre = '{codEmpre}'"

                If Not IsNothing(listaUsinaAprovadaOfertaExportacao) AndAlso Not String.IsNullOrEmpty(listaUsinaAprovadaOfertaExportacao.Trim()) Then
                    sql = sql + $" and CodUsina in ({listaUsinaAprovadaOfertaExportacao}) "
                End If

                sql = sql + " and nomUsina not like '%_DESSEM' "

                Dim rsUsina As SqlDataReader = Me.ConsultarSQL(sql)

                Do While rsUsina.Read
                    Dim usina As UsiConversDTO = New UsiConversDTO()
                    usina.CodUsina = rsUsina("codusina")
                    usina.NomUsina = rsUsina("nomusina")

                    listaUsina.Add(usina)
                Loop

                rsUsina.Close()
                rsUsina = Nothing
            End If

        Catch ex As Exception
            Throw TratarErro(ex)
        Finally
            Me.FecharConexao()
        End Try

        Return listaUsina

    End Function

    <Obsolete("Utilizar métodos que executa ListarTodos(criterio)")>
    Public Function ListarUsinasConversoras(ByVal tipoUsina As String, ByVal codUsina As String) As List(Of UsiConversDTO)

        Dim listaUsinaConversora As List(Of UsiConversDTO) = New List(Of UsiConversDTO)

        Try
            Dim sql As String = $"select codusina, nomusina from usina 
                            where tpusina_id = '{tipoUsina}' 
                            and CodUsina not in ('COACY','IEGAI1','IEGAI2') "

            Dim reader As SqlDataReader = Me.ConsultarSQL(sql)

            Do While reader.Read
                Dim usina As UsiConversDTO = New UsiConversDTO()
                usina.CodUsina = reader("codusina")
                usina.NomUsina = reader("nomusina")

                listaUsinaConversora.Add(usina)
            Loop

        Catch ex As Exception
            Throw TratarErro(ex)
        Finally
            Me.FecharConexao()
        End Try

        Return listaUsinaConversora

    End Function

    <Obsolete("Utilizar métodos que executa ListarTodos(criterio)")>
    Public Function ListaUsinaConversoraPorUsina(ByVal codUsina As String, ByVal codEmpre As String) As List(Of UsiConversDTO)

        Dim listaUsinaConversora As List(Of UsiConversDTO) = New List(Of UsiConversDTO)

        Try
            Dim sql As String = "select uc.id_usinaconversora, 
                                        u.codusina, 
                                        u.nomusina, 
                                        uc.codusiconversora, 
                                        (select nomusina from usina where codusina= uc.codusiconversora) As nomusinaconversora, 
                                        uc.pct_perda,
                                        uc.num_prioridade
                                        from usina u 
                                        inner Join tb_usinaconversora uc On uc.codusina = u.codusina "

            If Not IsNothing(codEmpre) Then
                sql += "where u.codempre = '" & codEmpre & "' "
            End If

            If Not IsNothing(codUsina) Then
                sql += "and Trim(u.codusina) = '" & codUsina.Trim() & "' "
            End If

            sql += "order by u.codusina asc"

            Dim rsUsina As SqlDataReader = Me.ConsultarSQL(sql)

            Do While rsUsina.Read
                Dim usinaConversora As UsiConversDTO = New UsiConversDTO()
                usinaConversora.IdUsinaConversora = rsUsina("id_usinaconversora")
                usinaConversora.CodUsina = rsUsina("codusina")
                usinaConversora.NomUsina = rsUsina("nomusina")
                usinaConversora.CodUsinaConversora = rsUsina("codusiconversora")
                usinaConversora.nomConversora = rsUsina("nomusinaconversora")
                usinaConversora.PercentualPerda = rsUsina("pct_perda")
                usinaConversora.NumeroPrioridade = rsUsina("num_prioridade")

                listaUsinaConversora.Add(usinaConversora)
            Loop

            rsUsina.Close()
            rsUsina = Nothing

        Catch ex As Exception
            Throw TratarErro(ex)
        Finally
            Me.FecharConexao()
        End Try

        Return listaUsinaConversora
    End Function

    <Obsolete("Utilizar o DAO.Salvar que realiza o DTO.ObterComando()")>
    Public Overloads Sub Salvar(ByVal usinaConversora As UsiConversDTO)
        Dim sql As String = ""
        Dim percentualPerda As String = usinaConversora.PercentualPerda.ToString().Replace(",", ".")

        Try
            If Not Me.ExisteUsinaConversora(usinaConversora.CodUsina, usinaConversora.codConversora) Then
                sql = $"INSERT INTO tb_usinaconversora (codusina, codusiconversora, pct_perda, Num_Prioridade) 
                    VALUES('{usinaConversora.CodUsina}', '{usinaConversora.codConversora}', {percentualPerda}, {usinaConversora.NumeroPrioridade})"
            Else
                ' Atualiza o número de prioridade sempre e o percentual de perda apenas se for diferente de zero
                If usinaConversora.PercentualPerda <> 0 Then
                    sql = $"UPDATE tb_usinaconversora SET pct_perda = {percentualPerda}, num_prioridade = {usinaConversora.NumeroPrioridade}
                        WHERE codusina = '{usinaConversora.CodUsina}' AND codusiconversora = '{usinaConversora.codConversora}'"
                Else
                    sql = $"UPDATE tb_usinaconversora SET num_prioridade = {usinaConversora.NumeroPrioridade}
                        WHERE codusina = '{usinaConversora.CodUsina}' AND codusiconversora = '{usinaConversora.codConversora}'"
                End If
            End If

            Me.ExecutarSQL(sql)

        Catch ex As Exception
            Throw TratarErro(ex)
        Finally
            Me.FecharConexao()
        End Try
    End Sub


    <Obsolete("Utilizar métodos que executa ListarTodos(criterio)")>
    Public Function ExisteUsinaConversora(ByVal codUsina As String, ByVal codUsinaConversora As String) As Boolean
        Dim retorno As Boolean = False
        Dim IdUsinaConversora As Integer = 0

        Try
            Dim sql As String = $"select id_usinaconversora from tb_usinaconversora
                                    where codusina = '{codUsina}'
                                    and codusiconversora = '{codUsinaConversora}'"

            IdUsinaConversora = Me.ObterCommando(sql).ExecuteScalar()

            If IdUsinaConversora <> 0 Then
                retorno = True
            End If

            Return retorno

        Catch ex As Exception
            Throw TratarErro(ex)
        Finally
            Me.FecharConexao()
        End Try

    End Function

    <Obsolete("Utilizar métodos que executa ListarTodos(criterio)")>
    Public Function ExisteOfertaFutura_PendenteAnalise_UsinaConversora(ByVal codUsina As String, ByVal codUsinaConversora As String) As Boolean
        Dim retorno As Boolean = False
        Dim IdUsinaConversora As Integer = 0

        Try
            Dim sql As String = $"  select uc.id_usinaconversora from tb_usinaconversora uc
                                    inner join tb_OfertaExportacao o on o.codUsina = uc.CodUsina and o.CodUsiConversora = uc.CodUsiConversora
                                    where uc.codusina = '{codUsina}'
                                    and uc.codusiconversora = '{codUsinaConversora}' and
                                    o.DatPDP = '{DateTime.Now.AddDays(1).ToString("yyyyMMdd")}' 
                                    and (o.flg_aprovado_ons is null or (o.flg_aprovado_agente is null and o.flg_aprovado_ons = 'S' ) ) "

            IdUsinaConversora = Me.ObterCommando(sql).ExecuteScalar()

            If IdUsinaConversora <> 0 Then
                retorno = True
            End If

            Return retorno

        Catch ex As Exception
            Throw TratarErro(ex)
        Finally
            Me.FecharConexao()
        End Try

    End Function

    <Obsolete("Utilizar métodos que executa ListarTodos(criterio)")>
    Public Function ListarConversorasPorUsina(Optional ByVal codUsina As String = "", Optional ByVal listaCodEmpre As List(Of String) = Nothing) As List(Of UsiConversDTO)
        Dim UsinaConversora As New List(Of UsiConversDTO)()

        Try
            Dim sql As String = "select 
                                    Trim(con.codusina) as codusina , 
                                    us.nomusina, 
                                    Trim(con.codusiconversora) as codusiconversora, 
                                    uconv.nomusina as nomconversora
                                 from tb_usinaconversora con 
                                join usina us on us.codusina = con.codusina 
                                join usina uconv on uconv.codusina = con.codusiconversora "

            Dim criterio As String = ""

            If Not String.IsNullOrEmpty(codUsina) Or Not IsNothing(listaCodEmpre) And listaCodEmpre.Count > 0 Then
                criterio += " Where "
            End If

            Dim _and As String = ""
            Dim _virgula As String = ""
            If Not IsNothing(listaCodEmpre) AndAlso listaCodEmpre.Count > 0 Then

                criterio += " us.codempre in ( "

                For Each codEmpre As String In listaCodEmpre
                    criterio += _virgula & " '" & codEmpre.Trim() & "'"
                    _virgula = ","
                Next

                criterio += ")"
                _and = " and "
            End If

            If Not IsNothing(codUsina) AndAlso Not String.IsNullOrEmpty(codUsina.Trim()) Then
                criterio += _and & " Trim(us.codusina) = '" & codUsina.Trim() & "'"
            End If

            sql += criterio + " Order by con.codusina "

            Dim reader As SqlDataReader = Me.ConsultarSQL(sql)
            Dim uListaUsinaConversora As List(Of ConversoraValorOfertaDTO) = New List(Of ConversoraValorOfertaDTO)

            Do While reader.Read
                Dim uConversora As New ConversoraValorOfertaDTO
                uConversora.CodConversora = reader("codusiconversora")
                uConversora.CodUsina = reader("codusina")
                uConversora.nomConversora = reader("nomconversora")
                uConversora.NomUsina = reader("nomusina")
                uConversora.ValorSugeridoAgente = 0
                uConversora.ValorSugeridoOns = 0

                uListaUsinaConversora.Add(uConversora)
            Loop

            Dim sUsinas As String = ""
            Dim usConv As New UsiConversDTO

            usConv.ValoresOfertaUsiConversora = New List(Of ConversoraValorOfertaDTO)

            Dim tot As Integer
            Dim total = uListaUsinaConversora.Count

            For Each conv As ConversoraValorOfertaDTO In uListaUsinaConversora
                Dim uConversora As New ConversoraValorOfertaDTO
                tot = tot + 1
                uConversora.CodConversora = conv.CodConversora
                uConversora.CodUsina = conv.CodUsina
                uConversora.NomUsina = conv.NomUsina
                uConversora.nomConversora = conv.nomConversora
                uConversora.ValorSugeridoAgente = 0
                uConversora.ValorSugeridoOns = 0

                usConv.ValoresOfertaUsiConversora.Add(uConversora)

                If ((uListaUsinaConversora.Count - 1) >= tot) Then
                    sUsinas = uListaUsinaConversora.Item(tot).CodUsina
                End If

                If sUsinas <> "" And sUsinas <> conv.CodUsina Or total = tot Then
                    usConv.CodUsina = conv.CodUsina
                    usConv.codConversora = conv.CodConversora
                    usConv.CodUsinaConversora = conv.CodConversora
                    usConv.nomConversora = conv.nomConversora
                    usConv.NomUsina = conv.NomUsina

                    usConv.OrdemAgente = 0
                    usConv.OrdemOns = 0

                    UsinaConversora.Add(usConv)
                    usConv = New UsiConversDTO
                    usConv.ValoresOfertaUsiConversora = New List(Of ConversoraValorOfertaDTO)
                End If

            Next

        Catch ex As Exception
            Throw TratarErro(ex)
        Finally
            Me.FecharConexao()
        End Try

        Return UsinaConversora

    End Function

    <Obsolete("Utilizar métodos que executa ListarTodos(criterio)")>
    Public Function ListarUsinaConversora(Optional ByVal idUsinaConversora As Integer = 0, Optional ByVal codUsina As String = "", Optional ByVal codUsinaConversora As String = "") As List(Of UsinaConversoraDTO)

        Dim criterios As String = " 1=1 "

        If idUsinaConversora <> 0 Then
            criterios += "and id_usinaconversora = '" & idUsinaConversora & "'"
        End If

        If Not String.IsNullOrEmpty(codUsina) Then
            criterios += "and codusina = '" & codUsina & "'"
        End If

        If Not String.IsNullOrEmpty(codUsinaConversora) Then
            criterios += "and codusiconversora = '" & codUsinaConversora & "'"
        End If

        Return Me.ListarTodos(criterios)
    End Function


End Class
