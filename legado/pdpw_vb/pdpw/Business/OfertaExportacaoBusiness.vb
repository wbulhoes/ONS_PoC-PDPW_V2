Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports System.Linq
Imports System.Text
Imports DocumentFormat.OpenXml.Packaging
Imports DocumentFormat.OpenXml.Spreadsheet

Namespace Ons.interface.business

    Public Class OfertaExportacaoBusiness
        Inherits BaseBusiness
        Implements IOfertaExportacaoBusiness
        Private logger As log4net.ILog = log4net.LogManager.GetLogger(Me.GetType())
        Public Function ValidarExiste_OfertasNaoAnalisadasONS(Lista As List(Of OfertaExportacaoDTO)) As Boolean Implements IOfertaExportacaoBusiness.ValidarExiste_OfertasNaoAnalisadasONS
            Dim existeOFertas_NaoAnalisadasONS As Boolean = False


            existeOFertas_NaoAnalisadasONS = IIf(Lista.Where(Function(x) x.FlgAprovadoONS = Nothing).Count > 0, True, False)



            'Me.FactoryDao.OfertaExportacaoDAO.CacheTrakingDisabled()

            'existeOFertas_NaoAnalisadasONS =
            '    Me.FactoryDao.OfertaExportacaoDAO.ValiarExisteOfertaPendenteAnaliseONS(dataPDP)

            'Me.FactoryDao.OfertaExportacaoDAO.CacheTrakingEnabled()

            Return existeOFertas_NaoAnalisadasONS
        End Function

        Public Function ValidarExiste_OfertasNaoAnalisadasONSOriginal(dataPDP As String) As Boolean Implements IOfertaExportacaoBusiness.ValidarExiste_OfertasNaoAnalisadasONSOriginal
            Dim existeOFertas_NaoAnalisadasONS As Boolean = False


            Me.FactoryDao.OfertaExportacaoDAO.CacheTrakingDisabled()

            existeOFertas_NaoAnalisadasONS =
                Me.FactoryDao.OfertaExportacaoDAO.ValiarExisteOfertaPendenteAnaliseONS(dataPDP)

            Me.FactoryDao.OfertaExportacaoDAO.CacheTrakingEnabled()

            Return existeOFertas_NaoAnalisadasONS
        End Function

        Public Function Permitir_ExclusaoOfertas(dataPDP As String) As Boolean Implements IOfertaExportacaoBusiness.Permitir_ExclusaoOfertas
            Dim permitir As Boolean = False

            If Get_DataPDP_DateTime(dataPDP).Date >= DateTime.Now.AddDays(1).Date Then

                permitir =
                    Not Me.FactoryDao.OfertaExportacaoDAO.
                    ValiarExisteOferta_AnaliseONS_Iniciada(dataPDP)
            End If

            Return permitir
        End Function

        Public Function ListaTodasOfertasAnalises(dataPDP As String) As List(Of OfertaExportacaoDTO) Implements IOfertaExportacaoBusiness.ListaTodasOfertasAnalises
            Dim lista As List(Of OfertaExportacaoDTO) = New List(Of OfertaExportacaoDTO)
            Me.FactoryDao.OfertaExportacaoDAO.CacheTrakingDisabled()

            lista = Me.FactoryDao.OfertaExportacaoDAO.ListarTodasAnalises(dataPDP)

            Me.FactoryDao.OfertaExportacaoDAO.CacheTrakingEnabled()

            Return lista
        End Function

        Public Function ObterLimiteCadastrado(dataSelecionada As String, listaCodEmpresa As List(Of String), tipoEnvio As TipoEnvio, hora_padrao As String) As DateTime Implements IOfertaExportacaoBusiness.ObterLimiteCadastrado

            Dim DataHora_Limite As DateTime =
                CType(CType(dataSelecionada, Date).AddDays(-1).ToString($"dd/MM/yyyy {hora_padrao}"), Date)

            If listaCodEmpresa.Count > 0 Then 'Valida se existe alguma empresa fora do limite

                Dim listaLimitesEmpresas As List(Of LimiteEnvioDTO) =
                    Me.FactoryDao.LimiteEnvioDAO.
                    Listar(CType(dataSelecionada, Date).ToString("yyyyMMdd"))

                Dim listaLimites_PorEmpresa As List(Of LimiteEnvioDTO) = New List(Of LimiteEnvioDTO)()
                For Each codEmpresa As String In listaCodEmpresa

                    Dim limiteCadastrado As LimiteEnvioDTO =
                            listaLimitesEmpresas.Where(Function(l) l.CodEmpre.Trim() = codEmpresa.Trim() And l.TipoEnvio = tipoEnvio).
                            FirstOrDefault()

                    'SE existe limite cadastrado para essa empresa: será cumprido o prazo cadastrado.
                    If Not IsNothing(limiteCadastrado) Then
                        listaLimites_PorEmpresa.Add(limiteCadastrado)
                    End If
                Next

                If listaLimites_PorEmpresa.Count > 0 Then
                    DataHora_Limite = listaLimites_PorEmpresa.Min(Function(l) l.DataHora_Limite)
                End If
            End If

            Return DataHora_Limite

        End Function

        Public Function ValidarPrazoEnvioOfertaAgente(ByVal dataSelecionada As String, ByVal listaCodEmpresa As List(Of String)) As Boolean Implements IOfertaExportacaoBusiness.ValidarPrazoEnvioOfertaAgente

            Dim DentroPrazoEnvio As Boolean = False
            Dim DataHora_Atual As DateTime = Date.Now

            Dim DataHora_Limite As DateTime =
                CType(CType(dataSelecionada, Date).AddDays(-1).ToString("dd/MM/yyyy 10:00:00"), Date)

            If listaCodEmpresa.Count > 0 Then 'Valida se existe alguma empresa fora do limite

                Dim listaLimites_dia As List(Of LimiteEnvioDTO) =
                    Me.FactoryDao.LimiteEnvioDAO.Listar(CType(dataSelecionada, Date).ToString("yyyyMMdd"))

                Dim listaLimites_PorEmpresa As List(Of LimiteEnvioDTO) = New List(Of LimiteEnvioDTO)()
                For Each codEmpresa As String In listaCodEmpresa

                    Dim limiteCadastrado As LimiteEnvioDTO =
                        listaLimites_dia.
                            Where(Function(l) l.CodEmpre.Trim() = codEmpresa.Trim() And
                                              l.TipoEnvio = TipoEnvio.EnviarOfertaExportacao).
                            FirstOrDefault()

                    'SE existe limite cadastrado para essa empresa: será cumprido o prazo cadastrado.
                    If Not IsNothing(limiteCadastrado) Then
                        listaLimites_PorEmpresa.Add(limiteCadastrado)
                    End If
                Next

                If listaLimites_PorEmpresa.Count > 0 Then
                    DataHora_Limite = listaLimites_PorEmpresa.Min(Function(l) l.DataHora_Limite)
                End If
            End If

            If (DataHora_Atual <= DataHora_Limite) Then
                DentroPrazoEnvio = True
            End If

            Return DentroPrazoEnvio

        End Function

        Public Function ValidarPrazoAnaliseOfertaAgente(dataSelecionada As String, listaCodEmpresa As List(Of String)) As Boolean Implements IOfertaExportacaoBusiness.ValidarPrazoAnaliseOfertaAgente

            Dim DentroPrazoEnvio As Boolean = False
            Dim DataHora_Atual As DateTime = Date.Now

            If Not IsNothing(dataSelecionada) And Not String.IsNullOrEmpty(dataSelecionada) Then

                Dim DataHora_Limite As DateTime =
                CType(CType(dataSelecionada, Date).AddDays(-1).ToString("dd/MM/yyyy 18:15:00"), Date)

                If listaCodEmpresa.Count > 0 Then 'Valida se existe alguma empresa fora do limite

                    Dim listaEmpresas_PorDia As List(Of LimiteEnvioDTO) =
                    Me.FactoryDao.LimiteEnvioDAO.Listar(CType(dataSelecionada, Date).ToString("yyyyMMdd"))

                    Dim listaLimites_PorEmpresa As List(Of LimiteEnvioDTO) = New List(Of LimiteEnvioDTO)()
                    For Each codEmpresa As String In listaCodEmpresa

                        Dim limiteCadastrado As LimiteEnvioDTO =
                            listaEmpresas_PorDia.
                                Where(Function(l) l.CodEmpre.Trim() = codEmpresa.Trim() And
                                                  l.TipoEnvio = TipoEnvio.AnalisarOfertaExportacao).
                                FirstOrDefault()

                        'SE existe limite cadastrado para essa empresa: será cumprido o prazo cadastrado.
                        If Not IsNothing(limiteCadastrado) Then
                            listaLimites_PorEmpresa.Add(limiteCadastrado)
                        End If
                    Next

                    If listaLimites_PorEmpresa.Count > 0 Then
                        DataHora_Limite = listaLimites_PorEmpresa.Min(Function(l) l.DataHora_Limite)
                    End If
                End If

                If (DataHora_Atual <= DataHora_Limite) Then
                    DentroPrazoEnvio = True
                End If
            End If

            Return DentroPrazoEnvio

        End Function

        Public Function ObterProximoNumeroOrdem(dataPDP As String, Optional codEmpre As String = "") As Integer Implements IOfertaExportacaoBusiness.ObterProximoNumeroOrdem
            Try

                Dim proxOrdem As Integer =
                    Me.FactoryDao.OfertaExportacaoDAO.
                    ObterUltimoNumeroOrdem(dataPDP, codEmpre) + 1

                Return proxOrdem

            Catch ex As Exception
                Throw TratarErro(ex)
            End Try
        End Function

        Public Function ObterUltimaExportacaoBalancao(dataPDP As String) As OfertaExportacaoDTO Implements IOfertaExportacaoBusiness.ObterUltimaExportacaoBalancao

            Dim ultimaOfertaDia As OfertaExportacaoDTO = Nothing

            Me.FactoryDao.OfertaExportacaoDAO.CacheTrakingDisabled()

            Dim ofertas As List(Of OfertaExportacaoDTO) =
                Me.FactoryDao.OfertaExportacaoDAO.Listar(dataPDP)

            Me.FactoryDao.OfertaExportacaoDAO.CacheTrakingEnabled()

            If Not IsNothing(ofertas) AndAlso ofertas.Count > 0 Then
                ultimaOfertaDia = ofertas.OrderByDescending(Function(o) o.DinOnsExportadoBalanco).FirstOrDefault()
            End If

            Return ultimaOfertaDia

        End Function

        <Obsolete("Método implementado fora do padrão do uso das camadass DAO e Business. 
                    Não criar novas execuções em código desse método.")>
        Public Function GravarAnalise(ByVal ListUsinaOferta As List(Of UsiConversDTO), ListValoresOferta As List(Of ConversoraValorOfertaDTO), ByVal AnaliseOns As Boolean, ByVal loginUsuario As String, dataPDP As String, codEmpresa As String, flgAprovado As String, ByVal agentesRepresentados As String) As Boolean

            Dim sucesso As Boolean = False
            Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
            Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
            Dim houveReprovacaoAgente As Boolean = False
            Dim forcarExportacao As Boolean = False
            Dim ListaAnalise As List(Of OfertaExportacaoDTO) = Me.ListaTodasOfertasAnalises(dataPDP)


            Try
                Dim sql As StringBuilder = New StringBuilder()
                If AnaliseOns Then
                    '
                    ' WI 189663 - Incluído mais um botão (Salvar Exportação: flgAprovado = "O")
                    '
                    If flgAprovado = "S" Or flgAprovado = "N" Or flgAprovado = "" Or flgAprovado = "O" Then

                        Dim usinasDict As Dictionary(Of String, UsiConversDTO) = ListUsinaOferta.ToDictionary(Function(o) o.CodUsina & "-" & o.CodUsinaConversora)

                        'Atualiza Valores de Patamares (antes de atualizar os flg de decisão) 
                        'e faz a EXPORTAÇÃO para BALANÇO somente das Ofertas que NÃO tem decisão (flg...)"

                        With "Ação: UPDATE de Valores (sem UPDATE de Ofertas.Flg... ) e Exportar para Balanço"

                            Dim groupedValues = (From conv In ListValoresOferta
                                                 Group By key = New With {
                                                         Key conv.DatPdp,
                                                         Key conv.CodUsina,
                                                         Key conv.CodConversora,
                                                         Key conv.ValorSugeridoOns
                                                     }
                                                     Into Group).ToList()

                            For Each group As Object In groupedValues
                                Dim datPdp As String = group.Key.DatPdp
                                Dim codUsina As String = group.Key.CodUsina
                                Dim codConversora As String = group.Key.CodConversora
                                Dim valorSugeridoOns As Double = group.Key.ValorSugeridoOns

                                ' Verifique se algum item do grupo precisa ter o ValorSugeridoOns ajustado para 0
                                If usinasDict.ContainsKey(codUsina & "-" & codConversora) AndAlso usinasDict(codUsina & "-" & codConversora).flgaprovadoons = "N" Then
                                    valorSugeridoOns = 0
                                End If


                                ' Converter o grupo para uma lista enumerável para garantir que o Select funcione
                                Dim groupList As IEnumerable(Of ConversoraValorOfertaDTO) = CType(group.Group, IEnumerable(Of ConversoraValorOfertaDTO))

                                ' Obter os valores distintos para Num_Patamar
                                Dim patamaresList As List(Of String) = groupList _
                                    .Select(Function(c) c.Num_Patamar.ToString()) _
                                    .Distinct() _
                                    .ToList()

                                ' Criar uma string separada por vírgulas para a cláusula IN
                                Dim patamares As String = String.Join(",", patamaresList)


                                sql.AppendLine($"UPDATE tb_valoresofertaexportacao")
                                sql.AppendLine($"SET val_sugeridoons = {valorSugeridoOns}")
                                sql.AppendLine($"WHERE codusina = '{codUsina}'")
                                sql.AppendLine($"AND codusiconversora = '{codConversora}'")
                                sql.AppendLine($"AND datpdp = '{datPdp}'")
                                sql.AppendLine($"AND Num_Patamar IN ({patamares});")
                            Next

                        End With

                        With "Atualiza os flg de decisão das Ofertas (Perfil ONS)"

                            ' Consulta fora do loop para evitar repetição desnecessária
                            Dim consultasConverg As Dictionary(Of String, List(Of String)) = ListValoresOferta _
                                    .GroupBy(Function(I) I.CodUsina) _
                                    .ToDictionary(Function(g) g.Key, Function(g) g.Select(Function(I) I.CodConversora).Distinct().ToList())

                            forcarExportacao = True

                            For Each item As UsiConversDTO In ListUsinaOferta
                                If item.flgaprovadoons = "S" Or item.flgaprovadoons = "N" Then

                                    If consultasConverg.ContainsKey(item.CodUsina) Then
                                        For Each ConverItem As String In consultasConverg(item.CodUsina)
                                            sql.AppendLine($" update tb_ofertaexportacao 
                                                set lgn_onsanalise = '{ loginUsuario }',
                                                din_analise_ons = '{ item.dinanaliseons }',
                                                flg_aprovado_ons = '{ item.flgaprovadoons }', 
                                                num_ordemONS = { item.OrdemOns } ,
                                                num_ordemAgente = { item.OrdemAgente }
                                                where codusina = '{ item.CodUsina }' 
                                                and codusiconversora = '{ ConverItem }' 
                                                and datpdp = '{ item.DatPdp }'; ")
                                        Next
                                    End If

                                    forcarExportacao = False
                                End If
                            Next

                        End With
                    End If
                Else
                    '
                    ' WI 189663 - Incluído mais um botão (Salvar Exportação: flgAprovado = "O")
                    '
                    If flgAprovado = "S" Or flgAprovado = "N" Or flgAprovado = "" Or flgAprovado = "O" Then
                        '
                        ' WI 189663 - A funcionalidade não deve mais ter prazo. O agente pode entrar a qualquer momento para visualizar informações
                        '
                        'With "Valição de Prazo de Envio"

                        '    Dim listaCodEmpresa As New List(Of String)({codEmpresa})

                        '    If codEmpresa.Length = 0 Then
                        '        Dim comboEmpre As New DropDownList
                        '        PreencheComboEmpresaPOP(agentesRepresentados, comboEmpre, "")
                        '        listaCodEmpresa.Clear()
                        '        For i As Integer = 0 To comboEmpre.Items.Count - 1
                        '            If comboEmpre.Items(i).Value.Trim().Length > 0 Then
                        '                listaCodEmpresa.Add(comboEmpre.Items(i).Value)
                        '            End If
                        '        Next
                        '    End If

                        '    Dim dentroPrazo As Boolean =
                        '     Me.ValidarPrazoAnaliseOfertaAgente(Get_DataPDP_DateTime(dataPDP).ToString("dd/MM/yyyy"), listaCodEmpresa)

                        '    If (Not dentroPrazo) Then
                        '        Throw Me.TratarErro("Prazo esgotado para envio de Ofertas.")
                        '    End If
                        'End With

                        With "Valida se ainda existe ofertas pendente de Análise pelo ONS"
                            Dim existeOfertasPendenteAnaliseONS As Boolean =
                            Me.ValidarExiste_OfertasNaoAnalisadasONS(ListaAnalise)

                            If existeOfertasPendenteAnaliseONS Then
                                Throw Me.TratarErro("Nenhuma oferta pode ser aprovada ou reprovada enquanto ainda existir ofertas pendente de análise pelo ONS.")
                            End If
                        End With

                        With "Atualiza os flg de decisão das Ofertas (Perfil AGENTE)"

                            For Each item As UsiConversDTO In ListUsinaOferta
                                If item.flgaprovadoagente = "S" Or item.flgaprovadoagente = "N" Then

                                    sql.AppendLine($" update tb_ofertaexportacao 
                                        set lgn_agenteanalise ='{ loginUsuario }',
                                        din_analise_agente = '{ item.dinanaliseagente }',
                                        flg_aprovado_agente = '{ item.flgaprovadoagente }'
                                        where codusina = '{ item.CodUsina }' 
                                        and codusiconversora = '{ item.CodUsinaConversora }' 
                                        and datpdp = '{ item.DatPdp }'; ")

                                    If item.flgaprovadoagente = "N" Then
                                        houveReprovacaoAgente = True
                                        forcarExportacao = True
                                    End If

                                End If
                            Next

                        End With
                    Else
                        If flgAprovado = "E" Then
                            With "Valida se as análises de ofertas já foram iniciadas"
                                Dim permitirExclusao As Boolean =
                                    Me.Permitir_ExclusaoOfertas(dataPDP)

                                If Not permitirExclusao Then
                                    Throw Me.TratarErro("Não pode haver exclusões após o início de análise das ofertas pelo ONS.")
                                End If
                            End With

                            With "Exclusão de Ofertas"
                                '
                                ' WI 189663 - As exclusões só podem ser feitas no dia da programação até as 10hs ou dias posteriores ao dia da programação
                                '
                                Dim DataHoraAtual As DateTime = Now
                                Dim DataHoraLimite As DateTime = Today.AddMinutes(600)
                                Dim Amanha As DateTime = DateTime.Today.AddDays(1)
                                Dim DataProg As DateTime = CDate(Mid(dataPDP, 1, 4) & "-" & Mid(dataPDP, 5, 2) & "-" & Mid(dataPDP, 7, 2))

                                If DataProg = Amanha And DataHoraAtual > DataHoraLimite Then
                                    Throw Me.TratarErro("Não pode haver exclusões para a programação de amanhã após as 10h00.")
                                Else
                                    For Each item As UsiConversDTO In ListUsinaOferta

                                        sql.AppendLine($" Delete From tb_valoresofertaexportacao  
                                    where codusina = '{ item.CodUsina}' 
                                    and codusiconversora = '{ item.CodUsinaConversora}' 
                                    and datpdp = '{ item.DatPdp }'; ")

                                        sql.AppendLine($" Delete From tb_ofertaexportacao 
                                            where codusina = '{ item.CodUsina }' 
                                            and codusiconversora = '{ item.CodUsinaConversora }' 
                                            and datpdp = '{ item.DatPdp }'; ")
                                    Next
                                End If
                            End With

                        End If
                    End If
                End If

                If Not String.IsNullOrEmpty(sql.ToString().Trim()) Then
                    Me.ExecutarSQL(sql.ToString(), False)
                End If

                '
                ' WI 153017 e WI 189663 - A exportação para balanço só é executada quando o flgAprovado for diferente de S ou N ou O
                ' ou seja, omente será executado se o botão Exportar Para Balanço for executado.
                '
                If Not String.IsNullOrEmpty(dataPDP) And flgAprovado = "" Then

                    If AnaliseOns Then
                        Me.ExportarDadosParaBalanco(dataPDP, loginUsuario, forcarExportacao, ListaAnalise)
                    Else
                        If houveReprovacaoAgente Then
                            Me.ExportarDadosParaBalanco(dataPDP, loginUsuario, forcarExportacao, ListaAnalise)
                        End If
                    End If

                End If

                sucesso = True

            Catch ex As Exception
                Throw TratarErro(ex)
            End Try

            Return sucesso

        End Function

        Private Function ExecutarSQL(sql As String, comTransacao As Boolean) As String
            If Not sql.Length = 0 Then
                Me.FactoryDao.OfertaExportacaoDAO.ExecutarSQL(sql, comTransacao)
                sql = ""
            End If

            Return sql
        End Function

        Private Function Remover_Exportacao(ByVal dataPDP As String, ByVal ofertas As List(Of ValorOfertaExportacaoDTO)) As String

            Dim sql As StringBuilder = New StringBuilder()

            If (ofertas.Count > 0) Then

                For Each ofertaValor As ValorOfertaExportacaoDTO In ofertas.ToList()

                    Dim valDespaSup As Integer = ofertaValor.ValSugeridoONS

                    Dim usinaConversora As UsinaConversoraDTO =
                        Me.FactoryDao.UsinaConversoraDAO.Listar(dataPDP).
                        FirstOrDefault(Function(us) us.CodUsina = ofertaValor.CodUsina And
                                                    us.CodUsiConversora = ofertaValor.CodUsiConversora)

                    Dim valPerda As Integer = 0
                    If Not IsNothing(usinaConversora) Then
                        '
                        ' WI 152098 - Arredondamento equivocado
                        '
                        '                       valPerda = Int32.Parse(
                        '                      Math.Round(
                        '                     ofertaValor.ValSugeridoONS * (usinaConversora.PercentualPerda / 100),
                        '0))
                        valPerda = Int32.Parse(Math.Round(ofertaValor.ValSugeridoONS * (usinaConversora.PercentualPerda / 100), 0, MidpointRounding.AwayFromZero))
                    End If

                    Dim codEmpresa As String = ofertaValor.CodUsiConversora.Substring(0, 2)

                    Dim valGeracaoExportacaoConversora As Integer = (ofertaValor.ValSugeridoONS - valPerda) ' * (-1) 'Tem que ser o valor Positivo
                    Dim valExportaSUP As Integer = ofertaValor.ValSugeridoONS - valPerda

                    sql.AppendLine($" Update PerdasCIC set 
                                    ValPCCSup = ValPCCSup - {valPerda} 
                                    where datpdp = '{dataPDP}' 
                                    and codusina = '{ofertaValor.CodUsina}' 
                                    and intPCC = {ofertaValor.NumPatamar}; ")

                    sql.AppendLine($" Update Despa set 
                                    ValDespaSup = ValDespaSup + {valGeracaoExportacaoConversora} 
                                    where datpdp = '{dataPDP}' 
                                    and codusina = '{ofertaValor.CodUsiConversora}' 
                                    and intdespa = {ofertaValor.NumPatamar}; ")

                    sql.AppendLine($" Update Carga set 
                                    ValCargaSup = ValCargaSup - {valPerda} 
                                    where datpdp = '{dataPDP}' 
                                    and CodEmpre = '{codEmpresa}' 
                                    and intCarga = {ofertaValor.NumPatamar}; ")

                    sql.AppendLine($" Update Exporta Set 
                                     ValExportaSUP = ValExportaSUP - {valExportaSUP} 
                                     where datpdp = '{dataPDP}' 
                                     and codusina = '{ofertaValor.CodUsina}' 
                                     and intexporta = {ofertaValor.NumPatamar}; ")

                    sql.AppendLine($" Update Despa set 
                                        ValDespaSup = ValDespaSup - {valDespaSup} 
                                        where datpdp = '{dataPDP}' 
                                        and codusina = '{ofertaValor.CodUsina}' 
                                        and intdespa = {ofertaValor.NumPatamar};  ")
                Next

                Return sql.ToString()

            End If

        End Function

        ''' <summary>
        ''' Exportação de dados para Balanço
        ''' </summary>
        ''' <param name="dataPDP">Data da Programação</param>
        Public Sub ExportarDadosParaBalanco(ByVal dataPDP As String, ByVal loginUsuario As String, ByVal forcarExportacao As Boolean, ListaAnalise As List(Of OfertaExportacaoDTO)) Implements IOfertaExportacaoBusiness.ExportarDadosParaBalanco

            Dim sql As StringBuilder = New StringBuilder()
            Dim ofertas As List(Of OfertaExportacaoDTO) = Me.FactoryDao.OfertaExportacaoDAO.ListarOfertaExportacao(0, dataPDP) ' metodo de busca extraido da funcoes abaixo
            Dim ListaValorExportacao As List(Of ValorOfertaExportacaoDTO) = Me.FactoryDao.ValoresOfertaExportacaoDAO.Listar(dataPDP) ' reduzindo busca na tabela tb_valoresofertaexportacao
            'Dim ListaAnalise As List(Of OfertaExportacaoDTO) = Me.ListaTodasOfertasAnalises(dataPDP)
            Try
                logger.Info("Inicio da Exportação para Balanço...")
                Dim existeOferta As Boolean = Me.FactoryDao.ValoresOfertaExportacaoDAO.ExisteOfertaParaDataPdpSelecionada(dataPDP, ListaValorExportacao)

                If Not existeOferta Then
                    Throw New Exception($"Não possui ofertas para a data PDP selecionada.")
                End If
                logger.Info("Método de verificação de Ofertas para a data da programação executada: ExisteOfertaParaDataPdpSelecionada ...")

                Dim existeValoresRef As Boolean = Me.FactoryDao.ValoresOfertaExportacaoDAO.ExisteValorDeReferencia(dataPDP, ListaValorExportacao)
                If Not existeValoresRef Then
                    Me.AtualizarDadosDeReferencia(dataPDP, ofertas) 'passando lista de oferta via parametro
                    'Me.AtualizarDadosDeReferencia(dataPDP)

                End If
                logger.Info("Atualização dos dados de referência executada: AtualizarDadosDeReferencia ...")
                'Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
                'Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
                'Conn.Open("pdp")

                Dim existeOfertaPendenteAnaliseONS As Boolean =
                Me.ValidarExiste_OfertasNaoAnalisadasONS(ListaAnalise)
                logger.Info("Validação se existe ofertas não analisadas executada: ValidarExiste_OfertasNaoAnalisadasONS ...")

                If Not existeOfertaPendenteAnaliseONS Or forcarExportacao Then
                    Dim msgErro As String = ""

                    'Validar regras de limites de potência
                    msgErro = Me.FactoryDao.OfertaExportacaoDAO.ValidarUsinasComLimiteDePotenciaViolado(dataPDP)
                    If msgErro.Length = 0 Then
                        msgErro = Me.FactoryDao.OfertaExportacaoDAO.ValidarUsinasConversorasComLimiteDePotenciaViolado(dataPDP)
                    End If
                    logger.Info("Validação das ofertas com os limites das Usinas executada ...")
                    If msgErro.Length > 0 Then
                        msgErro = $"|Alguma(s) oferta(s) ultrapassa(m) o limite de potência de Usina ou Conversora:{msgErro}"
                        Throw TratarErro(msgErro)
                    Else
                        sql.AppendLine(
                            Me.AtualizaUsuarioExportacaoBalanco(dataPDP, loginUsuario, ofertas)) 'passado ofertas lista por parametro
                        'Me.AtualizaUsuarioExportacaoBalanco(dataPDP, loginUsuario)) '
                        logger.Info("Atualização o usuário que faz a exportação executada: AtualizaUsuarioExportacaoBalanco ...")
                        Dim listaValoresOfertaExportacao As List(Of ValorOfertaExportacaoDTO) =
                        Me.FactoryDao.ValoresOfertaExportacaoDAO.Listar(dataPDP)

                        If listaValoresOfertaExportacao.Count > 0 Then

                            sql.AppendLine(
                            Me.Calculo_Exportacao_Geracao_PorUsinaPorConversora(dataPDP, listaValoresOfertaExportacao, msgErro))

                        End If
                        logger.Info("Calculo das exportações por Usina e Conversora executado: Calculo_Exportacao_Geracao_PorUsinaPorConversora ...")
                        With "Remover Ofertas Reprovadas (pelo ONS ou pelo Agente)"
                            Dim listaOfertas_Reprovadas As List(Of OfertaExportacaoDTO) =
                                Me.FactoryDao.OfertaExportacaoDAO.ListarOfertas_Reprovadas_ONS_ou_Agente(ListaAnalise)

                            Dim listaValores_Remocao As List(Of ValorOfertaExportacaoDTO) = New List(Of ValorOfertaExportacaoDTO)()
                            If listaValores_Remocao.Count > 0 Then
                                For Each ofertaRemocao As OfertaExportacaoDTO In listaOfertas_Reprovadas
                                    listaValores_Remocao.AddRange(
                                    listaValoresOfertaExportacao.
                                        Where(Function(v) v.CodUsina = ofertaRemocao.CodUsina And
                                                          v.CodUsiConversora = ofertaRemocao.CodUsiConversora).
                                        ToList()
                                    )
                                Next
                                sql.AppendLine(Me.Remover_Exportacao(dataPDP, listaValores_Remocao))
                                logger.Info("Remover Ofertas Reprovadas (pelo ONS ou pelo Agente) ...")
                            End If
                        End With

                        sql.AppendLine(Me.Preparar_Inicializar_AnaliseONS(dataPDP))
                        logger.Info("Incluindo a inicialização da Analise de Ofertas no Script de BD ...")

                        sql.AppendLine(Me.FactoryBusiness.IntercambioBusiness.AtualizaIntercambioTodasOfertas(dataPDP))
                        logger.Info("Incluindo a Atualização de Intercambios no Script de BD ...")

                        logger.Info("Iniciar a execução do Script de BD ...")
                        Me.ExecutarSQL(sql.ToString(), True)
                        logger.Info("Fim da execução do Script de BD ...")
                    End If
                End If

            Catch ex As Exception
                logger.Error(ex.Message, ex)
                Throw TratarErro(ex)
            End Try
        End Sub

        Private Function Preparar_Inicializar_AnaliseONS(dataPDP As String) As String

            Return $" Update tb_OfertaExportacao Set flg_analiseONSIniciada = 'S' Where DatPDP = '{dataPDP}'; "
        End Function

        ''' <summary>
        ''' Cálculo de Exportação e Geração Por Usina
        ''' </summary>
        ''' <param name="dataPDP">Data da programação</param>
        ''' <param name="ofertas">Lista de Ofertas</param>
        Public Function Calculo_Exportacao_Geracao_PorUsinaPorConversora(ByVal dataPDP As String, ByVal ofertas As List(Of ValorOfertaExportacaoDTO), ByRef mensagensErro As String) As String Implements IOfertaExportacaoBusiness.Calculo_Exportacao_Geracao_PorUsinaPorConversora

            Dim sql_completo As StringBuilder = New StringBuilder()

            Dim listaUsinaConversora As List(Of UsinaConversoraDTO) =
                Me.FactoryDao.UsinaConversoraDAO.ListarTodos()

            Dim listaValGeracaoExportacao_PorConversora As List(Of Object) = New List(Of Object)
            'Dim quebralinha As String = "|"

            'Usinas Ofertadas
            For Each codUsina As String In ofertas.Select(Function(o) o.CodUsina).Distinct().ToList()

                Dim sql_porUsina As StringBuilder = New StringBuilder()

                For patamar As Integer = 1 To 48

                    Dim sql_porUsina_PorPatamar As StringBuilder = New StringBuilder()

                    Dim valGeracaoPerdaPorUsina As Integer = 0

                    'Valores Ofertados para Usina
                    Dim listaOfertaPorPatamar As List(Of ValorOfertaExportacaoDTO) =
                        ofertas.Where(Function(oft) oft.CodUsina.Trim() = codUsina.Trim() And
                        oft.NumPatamar = patamar).
                        ToList()

                    For Each itemValorOferta As ValorOfertaExportacaoDTO In listaOfertaPorPatamar.OrderBy(Function(v) v.CodUsiConversora).ToList()

                        Dim valPerda As Integer = 0

                        Dim valSugeridoONS As Integer = itemValorOferta.ValSugeridoONS
                        Dim codUsina_valorOferta As String = itemValorOferta.CodUsina.Trim()
                        Dim codUsiConversora_valorOferta As String = itemValorOferta.CodUsiConversora.Trim()

                        valPerda = CalcularPerda(listaUsinaConversora, valSugeridoONS, codUsina_valorOferta, codUsiConversora_valorOferta)

                        If valGeracaoPerdaPorUsina = 0 Then
                            valGeracaoPerdaPorUsina = itemValorOferta.ValPerdaRef
                        End If
                        valGeracaoPerdaPorUsina += valPerda 'Soma valores de perda por Usina

                        Dim valGeracaoExportacaoConversora As Integer =
                                (itemValorOferta.ValSugeridoONS - valPerda) * (-1) 'Valor negativo da Exportação da Conversora menos a perda

                        Dim valorGeracaoExportacao_PorConversora As Object = New With 'dynamic
                                {
                                    .CodUsiConversora = itemValorOferta.CodUsiConversora.Trim(),
                                    valGeracaoExportacaoConversora,
                                    .valGeracaoPerdaPorConversora = valPerda,
                                    patamar
                                }

                        listaValGeracaoExportacao_PorConversora.Add(valorGeracaoExportacao_PorConversora)
                    Next

                    sql_porUsina_PorPatamar.AppendLine($" Update PerdasCIC set 
                                    ValPCCSup = {valGeracaoPerdaPorUsina} 
                                    where datpdp = '{dataPDP}' 
                                    and codusina = '{codUsina}' 
                                    and intPCC = {patamar}; ")

                    Dim somaValSugeridoONS As Integer = listaOfertaPorPatamar.Sum(Function(o) o.ValSugeridoONS)

                    Dim valExportaRef As Integer? = ofertas.
                        Where(Function(o) o.Datpdp = dataPDP And o.CodUsina = codUsina And o.NumPatamar = patamar).
                        FirstOrDefault().ValExportaRef

                    Dim valDespaUsinaRef As Integer? = ofertas.
                        Where(Function(o) o.Datpdp = dataPDP And o.CodUsina = codUsina And o.NumPatamar = patamar).
                        FirstOrDefault().ValDespaUsinaRef

                    Dim valExportaSUP As Integer = valExportaRef + somaValSugeridoONS - valGeracaoPerdaPorUsina
                    Dim valDespaSup As Integer = valDespaUsinaRef + somaValSugeridoONS
                    Dim codEmpresa As String = codUsina.Substring(0, 2)

                    sql_porUsina_PorPatamar.AppendLine($" Update Exporta Set 
                                     ValExportaSUP = {valExportaSUP} 
                                     where datpdp = '{dataPDP}' 
                                     and codusina = '{codUsina}' 
                                     and intexporta = {patamar}; ")

                    sql_porUsina_PorPatamar.AppendLine($" Update Despa set 
                                    ValDespaSup = {valDespaSup} 
                                    where datpdp = '{dataPDP}' 
                                    and codusina = '{codUsina}' 
                                    and intdespa = {patamar};  ")

                    'Dim limite_PotInstalada_Usina As Nullable(Of Integer) = Nothing
                    'With "Obter PotInstalada Usina"
                    '    Dim usina As UsinaConversoraDTO =
                    '    listaUsinaConversora.
                    '    FirstOrDefault(Function(u) u.CodUsina.Trim() = codUsina)

                    '    If Not IsNothing(usina) Then
                    '        If usina.PotInstaladaUsina > 0 Then
                    '            limite_PotInstalada_Usina = usina.PotInstaladaUsina
                    '        End If
                    '    End If
                    'End With

                    'Dim temErro_Patamar_Usina As Boolean = False

                    'If valDespaSup > limite_PotInstalada_Usina Then
                    '    mensagensErro += $"{quebralinha} (Usina {codUsina}, Patamar {patamar}, [Valor Ref. {valDespaUsinaRef} Valor Exportação {somaValSugeridoONS}], Limite potência {limite_PotInstalada_Usina})"
                    '    'Exit For
                    '    temErro_Patamar_Usina = True
                    'End If

                    'If temErro_Patamar_Usina Then

                    '    'Cancela as atualizações para o Patamar
                    '    sql_porUsina_PorPatamar.Clear()

                    '    'Cancela qualquer decisão de Oferta feita para Usina
                    '    sql_porUsina_PorPatamar.AppendLine($" Update tb_OfertaExportacao set 
                    '                    Flg_aprovado_ONS = NULL,
                    '                    Flg_aprovado_Agente = NULL
                    '                    where datpdp = '{dataPDP}' 
                    '                    and codusina = '{codUsina}';  ")
                    'End If

                    sql_porUsina.AppendLine(sql_porUsina_PorPatamar.ToString())
                Next

                sql_completo.AppendLine(sql_porUsina.ToString())
            Next

            Dim ultimoCodEmpresa As String = ""
            Dim ultimoCodConversora As String = ""
            Dim flgUtilizarValorREF As Boolean = True
            Dim listaValperdaExportacao_PorEmpresa As List(Of Object) = New List(Of Object)

            'Conversoras Ofertas
            For Each CodUsiConversora As String In ofertas.OrderBy(Function(o) o.CodUsiConversora).Select(Function(o) o.CodUsiConversora).Distinct().ToList()
                Dim sql_porConversora As StringBuilder = New StringBuilder()

                For patamar As Integer = 1 To 48

                    Dim sql_porConversora_porPatamar As StringBuilder = New StringBuilder()

                    Dim totalExportacaoSemPerda_porConversora As Integer =
                        listaValGeracaoExportacao_PorConversora.
                        Where(Function(o) o.CodUsiConversora.ToString().Trim() = CodUsiConversora.Trim() And
                                          o.Patamar = patamar).
                        Sum(Function(o) o.valGeracaoExportacaoConversora)

                    Dim totalPerda_porConversora As Integer =
                        listaValGeracaoExportacao_PorConversora.
                        Where(Function(o) o.CodUsiConversora.ToString().Trim() = CodUsiConversora.Trim() And
                                                   o.Patamar = patamar).
                         Sum(Function(o) o.valGeracaoPerdaPorConversora)

                    'Obter os valores de Referência de Conversora
                    Dim ref_Conversora As ValorOfertaExportacaoDTO =
                        ofertas.
                        Where(Function(o) o.Datpdp = dataPDP And
                            o.CodUsiConversora.Trim() = CodUsiConversora.Trim() And
                           o.NumPatamar = patamar).
                        FirstOrDefault()

                    Dim ValDespa_UsiConversoraRef As Integer = 0
                    Dim ValCarga_UsiConversoraRef As Integer = 0

                    Dim codEmpresaConversora As String = CodUsiConversora.Substring(0, 2)

                    If Not IsNothing(ref_Conversora) Then
                        ValDespa_UsiConversoraRef = ref_Conversora.ValDespaUsiConversoraRef
                        ValCarga_UsiConversoraRef = ref_Conversora.ValCargaUsiConversoraRef

                        'Não mudou de empresa, mas mudou de Conversora, então não usar valor de REF 
                        'pois já foi somado para empresa quando calculou a primeira Conversa
                        If ultimoCodEmpresa = codEmpresaConversora And ultimoCodConversora <> CodUsiConversora Then
                            flgUtilizarValorREF = False
                        End If

                        'na mudança de empresa, volta a considerar a utilização do valor
                        If ultimoCodEmpresa <> codEmpresaConversora Then
                            flgUtilizarValorREF = True
                        End If

                        If Not flgUtilizarValorREF Then 'Não utilizar valore de referência
                            ValCarga_UsiConversoraRef = 0
                        End If

                    End If

                    Dim ValDespaSup As Integer = ValDespa_UsiConversoraRef + totalExportacaoSemPerda_porConversora
                    Dim ValCargaSup As Integer = ValCarga_UsiConversoraRef + totalPerda_porConversora

                    sql_porConversora_porPatamar.AppendLine($" Update Despa set 
                                    ValDespaSup = {ValDespaSup} 
                                    where datpdp = '{dataPDP}' 
                                    and codusina = '{CodUsiConversora}' 
                                    and intdespa = {patamar}; ")

                    If ultimoCodEmpresa <> codEmpresaConversora Or ultimoCodConversora <> CodUsiConversora Then
                        ultimoCodEmpresa = codEmpresaConversora
                        ultimoCodConversora = CodUsiConversora
                    End If

                    Dim valorperdaExportacao_PorEmpresa As Object = New With 'dynamic
                                {
                                    codEmpresaConversora,
                                    patamar,
                                    ValCargaSup
                                }

                    listaValperdaExportacao_PorEmpresa.Add(valorperdaExportacao_PorEmpresa)

                    'Dim limite_PotInstalada_Conversora As Nullable(Of Integer) = Nothing
                    'With "Obter PotInstalada Conversora"
                    '    Dim conversora As UsinaConversoraDTO =
                    '    listaUsinaConversora.
                    '    FirstOrDefault(Function(u) u.CodUsiConversora.Trim() = CodUsiConversora)

                    '    If Not IsNothing(conversora) Then
                    '        If conversora.PotInstaladaConversora > 0 Then
                    '            limite_PotInstalada_Conversora = conversora.PotInstaladaConversora
                    '        End If
                    '    End If
                    'End With

                    If ValDespaSup < 0 Then
                        ValDespaSup = ValDespaSup * (-1)
                    End If

                    'Dim temErro_Patamar_Conversora As Boolean = False

                    'If ValDespaSup > limite_PotInstalada_Conversora Then

                    '    mensagensErro += $"{quebralinha} (Conversora {CodUsiConversora}, Patamar {patamar}, [Valor Ref. {ValDespa_UsiConversoraRef} Valor Exportação {totalExportacaoSemPerda_porConversora}], Limite potência {limite_PotInstalada_Conversora})"
                    '    temErro_Patamar_Conversora = True
                    'End If

                    'If temErro_Patamar_Conversora Then

                    '    'Cancela as atualizações para o Patamar
                    '    sql_porConversora_porPatamar.Clear()

                    '    'Cancela qualquer decisão de Oferta feita para Usina
                    '    sql_porConversora_porPatamar.AppendLine($" Update tb_OfertaExportacao set 
                    '                    Flg_aprovado_ONS = NULL,
                    '                    Flg_aprovado_Agente = NULL
                    '                    where datpdp = '{dataPDP}' 
                    '                    and codusiConversora = '{CodUsiConversora}'; ")
                    'End If

                    sql_porConversora.AppendLine(sql_porConversora_porPatamar.ToString())
                Next

                sql_completo.AppendLine(sql_porConversora.ToString())
            Next

            'Empresas Ofertas
            For Each codEmpresa As String In listaValperdaExportacao_PorEmpresa.OrderBy(Function(o) o.codEmpresaConversora).Select(Function(o) o.codEmpresaConversora).Distinct().ToList()

                For patamar As Integer = 1 To 48

                    Dim totalPerda_porEmpresa As Integer =
                        listaValperdaExportacao_PorEmpresa.
                        Where(Function(o) o.codEmpresaConversora = codEmpresa And
                                                   o.Patamar = patamar).
                         Sum(Function(o) o.ValCargaSup)

                    'Inclui ou Atualiza as PERDAS das Conversoras como Geração negativa das Usinas criadas para receber as perdas
                    If codEmpresa = "IE" Then
                        sql_completo.AppendLine($"MERGE into DESPA as t
                        using (                    
                                Select '{dataPDP}' as DatPDP, 
                                'IEPRD' as CodUsinaPerdas, 
                                '{patamar}' as Patamar, 
                                (-1 * {totalPerda_porEmpresa}) as ValorSUP  
                                From (VALUES (1)) AS dummyTable(dummyColumn)                               
                              )  as s 
                                     on t.datpdp = s.DatPDP
                                     and t.CodUsina = s.CodUsinaPerdas
                                     and t.IntDespa = s.Patamar
                        WHEN MATCHED THEN
	                        update set 
                                t.valdespasup = s.ValorSUP
                        WHEN NOT MATCHED THEN
	                        insert (DatPDP, CodUsina, IntDespa, valdespasup)
                            values (s.DatPDP, s.CodUsinaPerdas, s.Patamar, s.ValorSUP) ; ")
                    ElseIf codEmpresa = "ES" Then
                        sql_completo.AppendLine($"MERGE into DESPA as t
                        using (                    
                                Select '{dataPDP}' as DatPDP, 
                                'ESPRD' as CodUsinaPerdas, 
                                '{patamar}' as Patamar, 
                                (-1 * {totalPerda_porEmpresa}) as ValorSUP  
                                From (VALUES (1)) AS dummyTable(dummyColumn)                               
                              )  as s 
                                     on t.datpdp = s.DatPDP
                                     and t.CodUsina = s.CodUsinaPerdas
                                     and t.IntDespa = s.Patamar
                        WHEN MATCHED THEN
	                        update set 
                                t.valdespasup = s.ValorSUP
                        WHEN NOT MATCHED THEN
	                        insert (DatPDP, CodUsina, IntDespa, valdespasup)
                            values (s.DatPDP, s.CodUsinaPerdas, s.Patamar, s.ValorSUP) ; ")
                    End If
                Next
            Next

            Return sql_completo.ToString()

        End Function

        Private Function CalcularPerda(listaUsinaConversora As List(Of UsinaConversoraDTO), valSugeridoONS As Integer, codUsina_valorOferta As String, codUsiConversora_valorOferta As String) As Integer
            Dim usinaConversora As UsinaConversoraDTO =
                listaUsinaConversora.
                FirstOrDefault(Function(us) us.CodUsina.Trim() = codUsina_valorOferta And
                                us.CodUsiConversora.Trim() = codUsiConversora_valorOferta)

            Dim valPerda As Integer = 0

            If Not IsNothing(usinaConversora) Then
                '
                ' WI 152098 - Arredondamento equivocado
                '
                ' valPerda = Int32.Parse(
                'Math.Round(
                'valSugeridoONS * (Math.Round(usinaConversora.PercentualPerda / 100, 2)),
                '0))

                valPerda = Int32.Parse(Math.Round(valSugeridoONS * (usinaConversora.PercentualPerda / 100), 0, MidpointRounding.AwayFromZero))
            End If

            Return valPerda
        End Function

        Public Sub AtualizarDadosDeReferencia(ByVal dataPDP As String, ByVal ofertas As List(Of OfertaExportacaoDTO), Optional ByVal forcarAtualizacao As Boolean = False) Implements IOfertaExportacaoBusiness.AtualizarDadosDeReferencia
            Dim sql As StringBuilder = New StringBuilder()
            Try
                Dim listaValoresRef As List(Of ValorOfertaExportacaoDTO) = New List(Of ValorOfertaExportacaoDTO)

                'Dim ofertas As List(Of OfertaExportacaoDTO) = Me.FactoryDao.OfertaExportacaoDAO.ListarOfertaExportacao(0, dataPDP)

                Dim codigo_controle As String = ""
                'Atualiza dados de referencia USINA  
                For Each oferta As OfertaExportacaoDTO In ofertas

                    sql.AppendLine($" merge into tb_valoresofertaexportacao as dest 
	                                    using (
		                                    select
			                                    du.datpdp,
			                                    du.codusina,
			                                    dc.codusina as codusiconversora,
			                                    du.intdespa,
			                                    du.valdespasup as gerusi,
			                                    dc.valdespasup as gerconv,
			                                    c.valcargasup as cargconv,
			                                    ISNULL(e.valexportasup, 0) as expusi,
			                                    ISNULL(p.valpccsup, 0) as perdusi
		                                    from despa du inner join despa dc on (du.datpdp = dc.datpdp and du.intdespa = dc.intdespa)
			                                     inner join carga c on (du.datpdp = c.datpdp and du.intdespa = c.intcarga and c.codempre = substring(dc.codusina, 0, 2))
			                                     left join exporta e on (du.datpdp = e.datpdp and du.intdespa = e.intexporta and du.codusina = e.codusina)
			                                     left join perdascic p on (du.datpdp = p.datpdp and du.intdespa = p.intpcc and du.codusina = p.codusina)
		                                    where 
			                                    du.datpdp = '{ dataPDP }' and
			                                    du.codusina = '{ oferta.CodUsina }' and 
			                                    dc.codusina = '{ oferta.CodUsiConversora }'
	                                    ) as orig
                                    on dest.datpdp = orig.datpdp
                                       and dest.num_patamar = orig.intdespa
                                       and dest.codusina = orig.codusina
                                       and dest.codusiconversora = orig.codusiconversora
                                    WHEN MATCHED THEN
                                           update set 
                                               dest.val_refusina = orig.gerusi,
		                                       dest.val_refusiconversora = orig.gerconv,
		                                       dest.val_refexporta = orig.expusi,
		                                       dest.val_refperda = orig.perdusi,
		                                       dest.val_refcargausiconversora = orig.cargconv; ")

                    'If codigo_controle <> oferta.CodUsina + oferta.CodUsiConversora Then
                    '    codigo_controle = oferta.CodUsina + oferta.CodUsiConversora

                    '    Dim codUsi As String = oferta.CodUsina
                    '    Dim codUsiConversora As String = oferta.CodUsiConversora

                    '    'Obter valores de refencia
                    '    Dim listaDespaReferencia As List(Of DespaDTO) = Me.FactoryDao.DespaDAO.Listar(dataPDP).Where(Function(d) d.CodUsina = codUsi.Trim()).ToList()
                    '    Dim listaExportaReferencia As List(Of ExportaDTO) = Me.FactoryDao.ExportaDAO.Listar(dataPDP).Where(Function(e) e.CodUsina = codUsi.Trim()).ToList()
                    '    Dim listaPerdaReferencia As List(Of PerdaDTO) = Me.FactoryDao.PerdaDAO.Listar(dataPDP).Where(Function(p) p.CodUsina = codUsi.Trim()).ToList()


                    '    For patamar As Integer = 1 To 48

                    '        Dim usinaDespa As DespaDTO = listaDespaReferencia.FirstOrDefault(Function(despa) despa.Patamar = patamar)
                    '        Dim usinaExporta As ExportaDTO = listaExportaReferencia.FirstOrDefault(Function(despa) despa.Patamar = patamar)
                    '        Dim usinaPerda As PerdaDTO = listaPerdaReferencia.FirstOrDefault(Function(despa) despa.Patamar = patamar)

                    '        Dim valorReferencia As ValorOfertaExportacaoDTO = New ValorOfertaExportacaoDTO
                    '        valorReferencia.Datpdp = dataPDP
                    '        valorReferencia.CodUsina = codUsi
                    '        valorReferencia.CodUsiConversora = codUsiConversora
                    '        valorReferencia.NumPatamar = patamar

                    '        If Not IsNothing(usinaDespa) Then
                    '            valorReferencia.ValDespaUsinaRef = usinaDespa.ValDespaSup
                    '        End If

                    '        If Not IsNothing(usinaExporta) Then
                    '            valorReferencia.ValExportaRef = usinaExporta.ValExportaSUP
                    '        End If

                    '        If Not IsNothing(usinaPerda) Then
                    '            valorReferencia.ValPerdaRef = usinaPerda.Valpccsup
                    '        End If

                    '        listaValoresRef.Add(valorReferencia)
                    '    Next

                    'End If
                Next

                'Atualiza dados de referencia CONVERSORA  
                'codigo_controle = ""
                'For Each oferta As OfertaExportacaoDTO In ofertas

                '    If codigo_controle <> oferta.CodUsina + oferta.CodUsiConversora Then
                '        codigo_controle = oferta.CodUsina + oferta.CodUsiConversora

                '        Dim codUsiConversora As String = oferta.CodUsiConversora

                '        Dim listaGeracao_Conversora As List(Of DespaDTO) =
                '            Me.FactoryDao.DespaDAO.Listar(dataPDP).
                '            Where(Function(d) d.CodUsina.Trim() = codUsiConversora.Trim()).
                '            ToList()

                '        Dim listaCarga_Conversora As List(Of CargaDTO) =
                '            Me.FactoryDao.CargaDAO.Listar(dataPDP).
                '            Where(Function(p) p.CodEmpre = codUsiConversora.Substring(0, 2).Trim()).
                '            ToList()

                '        If listaGeracao_Conversora.Any() Then
                '            For patamar As Integer = 1 To 48

                '                Dim geracao_Conversora As DespaDTO =
                '                    listaGeracao_Conversora.
                '                    FirstOrDefault(Function(despa) despa.Patamar = patamar)

                '                Dim carga_Conversora As CargaDTO =
                '                    listaCarga_Conversora.
                '                    FirstOrDefault(Function(carga) carga.Patamar = patamar)

                '                If Not IsNothing(geracao_Conversora) Then

                '                    Dim geracaoConversora As Integer = geracao_Conversora.ValDespaSup
                '                    Dim cargaConversora As Integer = 0

                '                    If Not IsNothing(carga_Conversora) Then
                '                        cargaConversora = carga_Conversora.ValCargaSup
                '                    End If

                '                    listaValoresRef.
                '                        Where(Function(vo) vo.CodUsiConversora.Trim() = codUsiConversora.Trim() And
                '                            vo.NumPatamar = patamar).
                '                        ToList().
                '                        ForEach(Function(vo)
                '                                    vo.ValDespaUsiConversoraRef = geracaoConversora
                '                                    vo.ValCargaUsiConversoraRef = cargaConversora
                '                                    Return vo
                '                                End Function)
                '                End If

                '            Next
                '        End If
                '    End If
                'Next

                'For Each valorOfertaExportacao As ValorOfertaExportacaoDTO In listaValoresRef

                '    Dim valDespa_Usina As Integer = 0
                '    If Not IsNothing(valorOfertaExportacao.ValDespaUsinaRef) Then
                '        valDespa_Usina = valorOfertaExportacao.ValDespaUsinaRef
                '    End If

                '    Dim valDespa_Conversora As Integer = 0
                '    If Not IsNothing(valorOfertaExportacao.ValDespaUsiConversoraRef) Then
                '        valDespa_Conversora = valorOfertaExportacao.ValDespaUsiConversoraRef
                '    End If

                '    Dim valExporta_Usina As Integer = 0
                '    If Not IsNothing(valorOfertaExportacao.ValExportaRef) Then
                '        valExporta_Usina = valorOfertaExportacao.ValExportaRef
                '    End If

                '    Dim valPerda_Usina As Integer = 0
                '    If Not IsNothing(valorOfertaExportacao.ValPerdaRef) Then
                '        valPerda_Usina = valorOfertaExportacao.ValPerdaRef
                '    End If

                '    Dim valCarga_Conversora As Integer = 0
                '    If Not IsNothing(valorOfertaExportacao.ValCargaUsiConversoraRef) Then
                '        valCarga_Conversora = valorOfertaExportacao.ValCargaUsiConversoraRef
                '    End If

                '    sql.AppendLine($" Update tb_valoresofertaexportacao set 
                '                val_refUsina = {valDespa_Usina}, 
                '                val_RefUsiConversora = {valDespa_Conversora}, 
                '                val_RefExporta = {valExporta_Usina}, 
                '                val_RefPerda = {valPerda_Usina}, 
                '                val_RefCargaUsiConversora = {valCarga_Conversora} 
                '                where datpdp = '{valorOfertaExportacao.Datpdp}' 
                '                and codusina = '{valorOfertaExportacao.CodUsina}' 
                '                and codUsiConversora = '{valorOfertaExportacao.CodUsiConversora}' 
                '                and num_patamar = {valorOfertaExportacao.NumPatamar}; ")
                'Next

                Me.FactoryDao.ValoresOfertaExportacaoDAO.ExecutarSQL(sql.ToString(), True)

            Catch ex As Exception
                Throw TratarErro("Erro AtualizarDadosDeReferencia - " + ex.Message)
            End Try

        End Sub

        Private Function AtualizaUsuarioExportacaoBalanco(ByVal dataPDP As String, ByVal loginUsuario As String, ofertas As List(Of OfertaExportacaoDTO)) As String

            Try
                Dim listaOfertaExportacao As List(Of OfertaExportacaoDTO) = ofertas

                Dim sql As StringBuilder = New StringBuilder()

                For Each ofertaExportacao As OfertaExportacaoDTO In listaOfertaExportacao

                    sql.AppendLine($" Update tb_ofertaexportacao Set
                                            flg_exportaBalanco =  'S',
                                            lgn_ONSExportaBalanco = '{loginUsuario.Trim()}',
                                            din_ExportaBalanco = '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}'
                                            Where id_ofertaexportacao = '{ofertaExportacao.IdOfertaExportacao}'; ")

                Next

                Return sql.ToString()

            Catch ex As Exception
                Throw TratarErro("Erro AtualizaUsuarioExportacaoBalanco - " + ex.Message)
            End Try

        End Function

        <Obsolete("Método implementado fora do padrão do uso das camadass DAO e Business. 
                    Não criar novas execuções em código desse método.")>
        Public Function GravarOfertas(ByVal ListUsinaOferta As List(Of UsiConversDTO), ListValoresOferta As List(Of ConversoraValorOfertaDTO), ByVal loginUsuario As String, dataPDP As String) As String

            Me.ValidarOfertasOrdemRepetidas(ListUsinaOferta)

            Dim mensagem As String =
                Me.Remover_OfertasComUsinaConversora_ForaDaPotInstalada(ListUsinaOferta, ListValoresOferta)

            Dim sucesso As Boolean =
            Me.FactoryDao.OfertaExportacaoDAO.
                GravarOfertas(ListUsinaOferta, ListValoresOferta, loginUsuario)

            Return mensagem

        End Function

        Private Sub ValidarOfertasOrdemRepetidas(ByRef listaValoresOferta As List(Of UsiConversDTO))

            Dim existeOfertaOrdemRepedita As Boolean = False

            Dim qtdUsinasOrdenadas As Integer =
                (From item In listaValoresOferta Select item.CodUsina, item.OrdemAgente Distinct).ToList().Count

            Dim qtdOrdens As Integer =
                (From item In listaValoresOferta Select item.OrdemAgente Distinct).ToList().Count

            existeOfertaOrdemRepedita = qtdUsinasOrdenadas <> qtdOrdens

            If existeOfertaOrdemRepedita Then
                Throw TratarErro("Existem ofertas com ordem repetida, verifique.")
            End If

        End Sub

        Public Function Remover_OfertasComUsinaConversora_ForaDaPotInstalada(ByRef listaOfertas As List(Of UsiConversDTO), ByRef listaOfertasValores As List(Of ConversoraValorOfertaDTO)) As String

            Dim mensagemRetorno As String = ""

            Dim listaUsinasConversoras As List(Of UsinaConversoraDTO) =
                Me.FactoryDao.UsinaConversoraDAO.ListarTodos()

            Dim listaOfertas_Remocao As List(Of ConversoraValorOfertaDTO) = New List(Of ConversoraValorOfertaDTO)()

            If listaUsinasConversoras.Count > 0 And listaOfertas.Count > 0 Then

                Dim codigo_controle As String = ""
                For Each oferta As ConversoraValorOfertaDTO In listaOfertasValores

                    If codigo_controle <> oferta.CodUsina + oferta.CodConversora Then
                        codigo_controle = oferta.CodUsina + oferta.CodConversora

                        Dim codUsina_oferta As String = oferta.CodUsina.Trim()
                        Dim codUsiConversora As String = oferta.CodConversora.Trim()

                        Dim limite_PotInstalada_Usina As Nullable(Of Integer) = Nothing
                        With "Obter PotInstalada Usina"
                            Dim usina As UsinaConversoraDTO =
                            listaUsinasConversoras.
                            FirstOrDefault(Function(u) u.CodUsina.Trim() = codUsina_oferta)

                            If Not IsNothing(usina) Then
                                If usina.PotInstaladaUsina > 0 Then
                                    limite_PotInstalada_Usina = usina.PotInstaladaUsina
                                End If
                            End If
                        End With

                        Dim limite_PotInstalada_Conversora As Nullable(Of Integer) = Nothing
                        With "Obter PotInstalada Conversora"
                            Dim conversora As UsinaConversoraDTO =
                            listaUsinasConversoras.
                            FirstOrDefault(Function(u) u.CodUsiConversora.Trim() = codUsiConversora)

                            If Not IsNothing(conversora) Then
                                If conversora.PotInstaladaConversora > 0 Then
                                    limite_PotInstalada_Conversora = conversora.PotInstaladaConversora
                                End If
                            End If
                        End With

                        Dim perdaPorcentagem As Nullable(Of Integer) = 0
                        With "Obter Percentual Perda da oferta"
                            Dim usina_converora As UsinaConversoraDTO =
                            listaUsinasConversoras.
                            FirstOrDefault(Function(u) u.CodUsina.Trim() = codUsina_oferta And
                                                       u.CodUsiConversora.Trim() = codUsiConversora)

                            If Not IsNothing(usina_converora) Then
                                perdaPorcentagem = usina_converora.PercentualPerda
                            End If
                        End With

                        With "Validação do Limite para Usina e Conversora"
                            If Not IsNothing(limite_PotInstalada_Usina) Or Not IsNothing(limite_PotInstalada_Conversora) Then

                                'Valida se algum patamar ultrapassa o limite de PotInstalada
                                For patamar As Integer = 1 To 48
                                    Dim total_exportacaoOferta As Integer =
                                            listaOfertasValores.
                                            Where(Function(v) v.CodUsina.Trim() = codUsina_oferta And
                                                              v.CodConversora.Trim() = codUsiConversora And
                                                              v.Num_Patamar = patamar).
                                            Sum(Function(o) o.ValorSugeridoAgente)

                                    Dim total_exportacaoUsina As Integer = total_exportacaoOferta
                                    Dim total_exportacaoConversora As Integer = total_exportacaoOferta - ((total_exportacaoOferta * perdaPorcentagem) / 100)

                                    If total_exportacaoUsina > limite_PotInstalada_Usina Or
                                       total_exportacaoConversora > limite_PotInstalada_Conversora Then

                                        listaOfertas_Remocao.Add(oferta)

                                    End If

                                Next
                            End If
                        End With
                    End If

                Next

                If listaOfertas_Remocao.Count > 0 Then

                    mensagemRetorno = "As seguintes ofertas de exportação excederam o limite máximo de geração da usina ou da conversora, verifique:"
                    Dim separador As String = ""

                    codigo_controle = ""
                    For Each ofertaRemocao As ConversoraValorOfertaDTO In listaOfertas_Remocao

                        Dim codUsina As String = ofertaRemocao.CodUsina.Trim()
                        Dim codConversora As String = ofertaRemocao.CodConversora.Trim()

                        If codigo_controle <> codUsina + codConversora Then
                            codigo_controle = codUsina + codConversora

                            listaOfertasValores.RemoveAll(Function(o) o.CodUsina.Trim() = codUsina And
                                                           o.CodConversora.Trim() = codConversora)

                            mensagemRetorno += $"{separador} ({codUsina} - {codConversora})"
                            separador = ","
                        End If
                    Next

                    If listaOfertasValores.Count > 0 Then 'Ainda restam ofertas para persistir...
                        mensagemRetorno += " Demais ofertas foram gravadas com sucesso."
                    End If
                End If

            End If

            Return mensagemRetorno
        End Function

        Public Function ReiniciarDecisaoDeAnalise(dataPDP As String) As Boolean Implements IOfertaExportacaoBusiness.ReiniciarDecisaoDeAnalise
            Dim sucesso As Boolean = False

            Try
                Dim listaOfertaExportacao As List(Of OfertaExportacaoDTO) =
                    Me.FactoryDao.OfertaExportacaoDAO.Listar(dataPDP)

                For Each oferta As OfertaExportacaoDTO In listaOfertaExportacao
                    oferta.FlgAprovadoONS = Nothing
                    oferta.FlgAprovadoAgente = Nothing

                    Me.FactoryDao.OfertaExportacaoDAO.Atualizar(oferta)
                Next

                Me.FactoryDao.OfertaExportacaoDAO.Salvar()

                Dim listaValorOfertaExportacao As List(Of ValorOfertaExportacaoDTO) =
                    Me.FactoryDao.ValoresOfertaExportacaoDAO.Listar(dataPDP)

                For Each valorOferta As ValorOfertaExportacaoDTO In listaValorOfertaExportacao
                    valorOferta.ValDespaUsiConversoraRef = Nothing
                    valorOferta.ValDespaUsinaRef = Nothing
                    valorOferta.ValExportaRef = Nothing
                    valorOferta.ValPerdaRef = Nothing
                    valorOferta.ValCargaUsiConversoraRef = Nothing

                    Me.FactoryDao.ValoresOfertaExportacaoDAO.Atualizar(valorOferta)
                Next

                Me.FactoryDao.ValoresOfertaExportacaoDAO.Salvar()

                sucesso = True
            Catch ex As Exception
                Throw TratarErro(ex)
            End Try

            Return sucesso
        End Function

        Public Function ReiniciarValoresReferencia(dataPDP As String) As Boolean Implements IOfertaExportacaoBusiness.ReiniciarValoresReferencia
            Dim sucesso As Boolean = False

            Try
                Dim sql As StringBuilder = New StringBuilder()
                sql.AppendLine($" UPDATE tb_valoresofertaexportacao
                                  SET val_refusina =0,
                                  val_refusiconversora = 0 ,
                                  val_refexporta = 0,
                                  val_refperda = 0,
                                  val_refcargausiconversora = 0
                                  WHERE datpdp = '{dataPDP}'; ")

                Me.FactoryDao.ValoresOfertaExportacaoDAO.ExecutarSQL(sql.ToString(), True)
            Catch ex As Exception
                Throw TratarErro(ex)
            End Try

            Return sucesso
        End Function

        Public Sub IniciarAnaliseOfertasONS(dataPDP As String) Implements IOfertaExportacaoBusiness.IniciarAnaliseOfertasONS

            Me.ExecutarSQL(Me.Preparar_Inicializar_AnaliseONS(dataPDP), False)

        End Sub

        Public Function RealizarLeituraPlanilhaValoresOfertaExportacaoONS(dataPDP As String, caminhoArquivo As String) As List(Of String) Implements IOfertaExportacaoBusiness.RealizarLeituraPlanilhaValoresOfertaExportacaoONS
            Dim mensagemRetorno As New List(Of String)

            Using document As SpreadsheetDocument = SpreadsheetDocument.Open(caminhoArquivo, False)
                Dim workbookPart As WorkbookPart = document.WorkbookPart
                Dim sheet As Sheet = workbookPart.Workbook.Sheets.GetFirstChild(Of Sheet)()
                Dim worksheetPart As WorksheetPart = CType(workbookPart.GetPartById(sheet.Id.Value), WorksheetPart)
                Dim rows As IEnumerable(Of Row) = worksheetPart.Worksheet.GetFirstChild(Of SheetData)().Descendants(Of Row)()

                ' Obtenha todas as células na planilha
                Dim cells As IEnumerable(Of Cell) = worksheetPart.Worksheet.Descendants(Of Cell)()

                ' Obter todas as colunas existentes
                Dim columnLetters As List(Of String) = cells.Select(Function(c) GetColumnName(c.CellReference)).Distinct().OrderBy(Function(c) c).ToList()
                Dim codigoUsina As String = ""

                Dim PrimeiraColuna As Cell = rows.ElementAt(0).Elements(Of Cell)().Where(Function(c) GetColumnName(c.CellReference) = "A").FirstOrDefault()
                Dim valorData As String = GetCellValue(workbookPart, PrimeiraColuna)

                If dataPDP <> valorData Then
                    mensagemRetorno.Add("A data da planilha está diferente da data escolhida, por favor ajustar e clicar novamente em Importar. ")
                    Return mensagemRetorno
                End If

                Dim valoresUsinasConversoras As New Dictionary(Of Tuple(Of String, String), List(Of Integer))

                ' Para cada coluna, percorra todas as linhas
                For Each columnLetter As String In columnLetters
                    Console.WriteLine($"Coluna: {columnLetter}")

                    ' Diferente de A, porque não há necessidade de ler a coluna de horários de patamares
                    If columnLetter <> "A" Then
                        ' Percorre as linhas na coluna atual em ordem por patamar
                        ' obtém o valor da usina e das conversoras

                        Dim colunaUsina As Cell = rows.ElementAt(0).Elements(Of Cell)().Where(Function(c) GetColumnName(c.CellReference) = columnLetter).FirstOrDefault()
                        Dim valor As String = GetCellValue(workbookPart, colunaUsina)

                        If valor <> "" Then
                            codigoUsina = valor
                        End If

                        '1a linha: cod usina
                        '2a linha: cod usina conversora
                        '3a linha até a 50a linha: valores para cada patamar daquela usina / conversora
                        Dim colunaConversora As Cell = rows.ElementAt(1).Elements(Of Cell)().Where(Function(c) GetColumnName(c.CellReference) = columnLetter).FirstOrDefault()
                        Dim valorConversora As String = GetCellValue(workbookPart, colunaConversora)

                        Dim arrayDeValores As New List(Of Integer)

                        If codigoUsina <> "" And valorConversora <> "" Then

                            For Each row As Row In rows.Skip(2)

                                Dim cell As Cell = row.Elements(Of Cell)().Where(Function(c) GetColumnName(c.CellReference) = columnLetter).FirstOrDefault()

                                If cell IsNot Nothing Then
                                    Dim cellValue As String = GetCellValue(workbookPart, cell)

                                    Dim valorConvertido As Integer
                                    If Integer.TryParse(cellValue, valorConvertido) Then
                                        arrayDeValores.Add(valorConvertido)
                                    Else
                                        arrayDeValores.Add(0) ' para não inserir valores nulos
                                    End If
                                End If
                            Next

                            Dim chave As New Tuple(Of String, String)(codigoUsina, valorConversora)

                            valoresUsinasConversoras.Add(chave, arrayDeValores)

                        End If
                    End If
                Next

                If valoresUsinasConversoras IsNot Nothing Then

                    Dim usinasAprovadasOuReprovadas As List(Of Tuple(Of String, String)) = VerificaUsinasJaAprovadasOuReprovadas(dataPDP, False)

                    ' Mantém somente as usinas que não possuem aprovação ou reprovação para ler a planilha e inserir na base
                    If usinasAprovadasOuReprovadas IsNot Nothing Then

                        Dim chavesParaRemover As List(Of Tuple(Of String, String)) = valoresUsinasConversoras.Keys.Where(Function(key)
                                                                                                                             Return usinasAprovadasOuReprovadas.Any(Function(usina)
                                                                                                                                                                        Return usina.Item1.Trim() = key.Item1 AndAlso
                                                                                                                                                                                usina.Item2.Trim() = key.Item2
                                                                                                                                                                    End Function)
                                                                                                                         End Function).ToList()

                        ' Remove as chaves duplicadas do dicionário original
                        For Each chave As Tuple(Of String, String) In chavesParaRemover
                            mensagemRetorno.Add($"A usina {chave.Item1} / conversora {chave.Item2} não foi alterada, pois existe oferta [aprovada/reprovada]. ”)
                            valoresUsinasConversoras.Remove(chave)
                        Next
                    End If

                    If valoresUsinasConversoras IsNot Nothing Then
                        InserirValoresUsinasConversoras(valoresUsinasConversoras, dataPDP, mensagemRetorno)
                    End If

                End If

            End Using

            Return mensagemRetorno

        End Function

        Public Sub InserirValoresUsinasConversoras(valoresUsinasConversoras As Dictionary(Of Tuple(Of String, String), List(Of Integer)), dataPDP As String, mensagemRetorno As List(Of String))
            Dim dtValoresOfertaExportacao As New DataTable("tb_valoresofertaexportacao")

            ' Definir colunas para o DataTable de tb_valoresofertaexportacao
            dtValoresOfertaExportacao.Columns.Add("datpdp", GetType(String))
            dtValoresOfertaExportacao.Columns.Add("codusina", GetType(String))
            dtValoresOfertaExportacao.Columns.Add("codusiconversora", GetType(String))
            dtValoresOfertaExportacao.Columns.Add("num_patamar", GetType(Integer))
            dtValoresOfertaExportacao.Columns.Add("val_sugeridoons", GetType(Decimal))

            Dim usinasValidasParaAtualizacao As New List(Of Tuple(Of String, String))
            ' Popular os DataTables com os valores das usinas que ainda não passaram por aprovação ou reprovação e estão aptas a ter dado inserido na base
            For Each valor As KeyValuePair(Of Tuple(Of String, String), List(Of Integer)) In valoresUsinasConversoras
                Dim codUsina As String = valor.Key.Item1.ToUpper()
                Dim codUsinaConversora As String = valor.Key.Item2.ToUpper()

                If VerificaSeExisteUsinaEConversora(codUsina, codUsinaConversora) Then
                    ' Popula tb_valoresofertaexportacao
                    For i As Integer = 0 To valor.Value.Count - 1
                        dtValoresOfertaExportacao.Rows.Add(dataPDP, codUsina, codUsinaConversora, i + 1, valor.Value(i))
                    Next

                    usinasValidasParaAtualizacao.Add(New Tuple(Of String, String)(codUsina, codUsinaConversora))
                Else
                    mensagemRetorno.Add($"A usina: {codUsina} / conversora: {codUsinaConversora} existente na planilha não foi encontrada em nossa base de dados, insira uma usina/conversora válida. ")
                End If
            Next

            '' BUSCA USINA/CONVERSORA QUE NAO TA NA PLANILHA E QUE NAO TEVE OFERTA APROVADA OU REPROVADA AINDA
            If usinasValidasParaAtualizacao IsNot Nothing Then
                Dim usinasForaPlanilhaENaoTiveramOfertaAprovRev As List(Of Tuple(Of String, String)) = VerificaUsinasJaAprovadasOuReprovadas(dataPDP, True)

                If usinasForaPlanilhaENaoTiveramOfertaAprovRev IsNot Nothing Then
                    Dim diferencas As List(Of Tuple(Of String, String)) = usinasForaPlanilhaENaoTiveramOfertaAprovRev.
                                                                            Where(Function(key)
                                                                                      Return usinasValidasParaAtualizacao.All(Function(usina)
                                                                                                                                  Return usina.Item1.Trim() <> key.Item1.Trim() OrElse
                                                                                                                                                        usina.Item2.Trim() <> key.Item2.Trim()
                                                                                                                              End Function)
                                                                                  End Function).ToList()
                    ' Remove as chaves duplicadas do dicionário original
                    For Each chave As Tuple(Of String, String) In diferencas
                        mensagemRetorno.Add($"A usina {chave.Item1.Trim()} / conversora {chave.Item2.Trim()} que ainda não possui oferta aprovada/reprovada para o dia, não está na planilha importada. Sendo assim, não haverá dado alterado. ”)
                    Next
                End If
            End If



            If dtValoresOfertaExportacao.Rows IsNot Nothing And mensagemRetorno.Count() > 0 Then
                mensagemRetorno.Add("Fora as exceções registradas, as demais ofertas inseridas na planilha foram atualizadas! Confira a seguir para conferência. ")
            End If

            ' Inserir os DataTables no banco de dados usando SqlBulkCopy
            Using Conn As New SqlConnection(ConfigurationManager.AppSettings("pdpSQL").ToString())
                Conn.Open()

                ' Criar a tabela temporária no banco de dados
                Using command As New SqlCommand("CREATE TABLE #TempValoresAtualizacao (datpdp CHAR(8), codusina CHAR(12), codusiconversora CHAR(12), num_patamar INT, val_sugeridoons FLOAT)", Conn)
                    command.ExecuteNonQuery()
                End Using

                ' Inserir os dados no DataTable temporário
                Using bulkCopy As New SqlBulkCopy(Conn)
                    bulkCopy.DestinationTableName = "#TempValoresAtualizacao"
                    bulkCopy.WriteToServer(dtValoresOfertaExportacao)
                End Using

                ' Executar o comando de atualização usando um JOIN com a tabela temporária
                Dim sqlUpdate As New StringBuilder()
                sqlUpdate.AppendLine("UPDATE tb_valoresofertaexportacao")
                sqlUpdate.AppendLine("SET tb_valoresofertaexportacao.val_sugeridoons = Temp.val_sugeridoons")
                sqlUpdate.AppendLine("FROM tb_valoresofertaexportacao")
                sqlUpdate.AppendLine("JOIN #TempValoresAtualizacao AS Temp ON tb_valoresofertaexportacao.datpdp = Temp.datpdp")
                sqlUpdate.AppendLine("AND tb_valoresofertaexportacao.codusina = Temp.codusina")
                sqlUpdate.AppendLine("AND tb_valoresofertaexportacao.codusiconversora = Temp.codusiconversora")
                sqlUpdate.AppendLine("AND tb_valoresofertaexportacao.num_patamar = Temp.num_patamar")

                Using commandUpdate As New SqlCommand(sqlUpdate.ToString(), Conn)
                    commandUpdate.ExecuteNonQuery()
                End Using

                ' Remover a tabela temporária
                Using commandDrop As New SqlCommand("DROP TABLE #TempValoresAtualizacao", Conn)
                    commandDrop.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function VerificaSeExisteUsinaEConversora(codUsina As String, codUsinaConversora As String) As Boolean Implements IOfertaExportacaoBusiness.VerificaSeExisteUsinaEConversora
            Dim resultado As Boolean = False

            Using Conn As New SqlConnection(ConfigurationManager.AppSettings.Get("pdpSQL").ToString())
                Try
                    Conn.Open()
                    Dim sql As New StringBuilder()
                    sql.AppendLine("SELECT CASE WHEN id_usinaconversora IS NOT NULL THEN 1 ELSE 0 END AS Existe FROM tb_usinaconversora WHERE codusina = @codUsina AND codusiconversora = @codUsinaConversora")

                    Using adapter As New SqlDataAdapter(sql.ToString(), Conn)
                        adapter.SelectCommand.Parameters.AddWithValue("@codUsina", codUsina)
                        adapter.SelectCommand.Parameters.AddWithValue("@codUsinaConversora", codUsinaConversora)

                        Dim resultadoConsulta As New DataTable()
                        adapter.Fill(resultadoConsulta)

                        ' Verificar se o DataTable contém linhas
                        If resultadoConsulta.Rows.Count > 0 Then
                            ' Ler o valor da primeira coluna como inteiro e converter para booleano
                            resultado = Convert.ToBoolean(resultadoConsulta.Rows(0)("Existe"))
                        End If
                    End Using

                Catch ex As Exception
                    Throw TratarErro(ex)
                Finally
                    If Conn.State = ConnectionState.Open Then
                        Conn.Close()
                    End If
                End Try
            End Using

            Return resultado
        End Function


        Public Function VerificaUsinasJaAprovadasOuReprovadas(dataPDP As String, checaFlgNula As Boolean) As List(Of Tuple(Of String, String)) Implements IOfertaExportacaoBusiness.VerificaUsinasJaAprovadasOuReprovadas
            Dim resultados As New List(Of Tuple(Of String, String))()
            Dim resultadoConsulta As DataSet = New DataSet()

            Using Conn As New SqlConnection(ConfigurationManager.AppSettings.Get("pdpSQL").ToString())
                Try
                    Conn.Open()
                    Dim sql As New StringBuilder()
                    If checaFlgNula Then
                        sql.AppendLine("SELECT codusina, codusiconversora FROM tb_ofertaexportacao WHERE datpdp = @dataPDP AND flg_aprovado_ons IS NULL")
                    Else
                        sql.AppendLine("SELECT codusina, codusiconversora FROM tb_ofertaexportacao WHERE datpdp = @dataPDP AND flg_aprovado_ons IS NOT NULL")
                    End If


                    Using adapter As New SqlDataAdapter(sql.ToString(), Conn)
                        adapter.SelectCommand.Parameters.AddWithValue("@dataPDP", dataPDP)
                        adapter.Fill(resultadoConsulta)
                    End Using

                    ' Verificar se o DataSet contém linhas e preencher a lista de tuplas
                    If resultadoConsulta.Tables.Count > 0 AndAlso resultadoConsulta.Tables(0).Rows.Count > 0 Then
                        For Each row As DataRow In resultadoConsulta.Tables(0).Rows
                            Dim codusi As String = row("codusina").ToString()
                            Dim codusiconversora As String = row("codusiconversora").ToString()
                            resultados.Add(New Tuple(Of String, String)(codusi, codusiconversora))
                        Next
                    End If

                Catch ex As Exception
                    Throw TratarErro(ex)
                Finally
                    If Conn.State = ConnectionState.Open Then
                        Conn.Close()
                    End If
                End Try
            End Using

            Return resultados
        End Function


        Private Function GetCellValue(workbookPart As WorkbookPart, cell As Cell) As String
            Dim value As String = cell.InnerText

            ' Se a célula usa uma referência para o SharedStringTable, pegue o valor correto
            If (cell.DataType IsNot Nothing) AndAlso (cell.DataType.Value = CellValues.SharedString) Then
                Dim sstPart As SharedStringTablePart = workbookPart.GetPartsOfType(Of SharedStringTablePart).FirstOrDefault()
                If sstPart IsNot Nothing Then
                    value = sstPart.SharedStringTable.ChildElements(Int32.Parse(value)).InnerText
                End If
            ElseIf cell.StyleIndex IsNot Nothing Then
                ' Se o estilo da célula indica que é um valor de data/hora
                Dim cellFormat = workbookPart.WorkbookStylesPart.Stylesheet.CellFormats.Elements(Of CellFormat).ElementAt(Convert.ToInt32(cell.StyleIndex.Value))

                ' Se o formato indica data/hora, converta o valor de volta para a hora
                If cellFormat.NumberFormatId.Value >= 14 AndAlso cellFormat.NumberFormatId.Value <= 22 Then
                    Dim doubleValue As Double
                    If Double.TryParse(value, doubleValue) Then
                        Try
                            Dim dateValue As DateTime = DateTime.FromOADate(doubleValue)
                            value = dateValue.ToString("yyyyMMdd") ' Formata como vem no pdp "ex:20241023"
                        Catch ex As Exception
                            ' Se não conseguir converter, apenas retorna o valor numérico original
                            value = doubleValue.ToString()
                        End Try
                    End If
                End If
            End If

            Return value
        End Function

        ' Função para obter o nome da coluna de uma célula (por exemplo, A1 retorna A)
        Private Function GetColumnName(cellReference As String) As String
            Dim regex As New System.Text.RegularExpressions.Regex("[A-Za-z]+")
            Dim match As System.Text.RegularExpressions.Match = regex.Match(cellReference)
            Return match.Value
        End Function
    End Class



End Namespace
