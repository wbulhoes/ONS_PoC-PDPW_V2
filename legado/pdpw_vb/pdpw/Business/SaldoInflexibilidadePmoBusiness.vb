Imports System.Collections.Generic
Imports System.Linq
Imports pdpw

Namespace Ons.interface.business

    Public Class SaldoInflexibilidadePMOBusiness
        Inherits BaseBusiness
        Implements ISaldoInflexibilidadePMOBusiness

        Public Function CalcularSaldoInflexibilidadeSemanaPMO(ByVal dataPDP As String,
                                                              ByVal codUsina As String,
                                                              ByVal listaValoresSalvo_IFX_SemanaPMO As List(Of SaldoInflexibilidadePMO_DTO)
                                                              ) As List(Of SaldoInflexibilidadePMO_DTO) Implements ISaldoInflexibilidadePMOBusiness.CalcularSaldoInflexibilidadeSemanaPMO

            Dim listaRetorno As List(Of SaldoInflexibilidadePMO_DTO) = New List(Of SaldoInflexibilidadePMO_DTO)()

            Try

                'Validação de parâmetros de entrada
                If (IsNothing(dataPDP) OrElse (String.IsNullOrEmpty(dataPDP))) Or
                (IsNothing(codUsina) OrElse (String.IsNullOrEmpty(codUsina))) Then
                    Throw New BusinessException("Data PDP, CodUsina ou Valor de Inflexiblidade Programado inválidos")
                End If

                'Regra de Validação:
                'Validar se os registros em "Saldo Inflexibilidade Semana PMO" foram abertos para Semana PMO na importação do arquivo DADGER
                Dim semanaPMO As SemanaPMO = GetSemanaPMO(Get_DataPDP_DateTime(dataPDP), Nothing, Nothing).FirstOrDefault()

                If Not IsNothing(semanaPMO) Then

                    Dim listaValoresSaldo_Usina As List(Of SaldoInflexibilidadePMO_DTO) =
                        listaValoresSalvo_IFX_SemanaPMO.
                        Where(Function(s) s.CodUsina.Trim() = codUsina.Trim() And
                        s.DatPDP >= semanaPMO.DataInicio.Replace("-", "") And
                        s.DatPDP <= semanaPMO.DataFim.Replace("-", "")).
                        ToList()

                    If Not IsNothing(listaValoresSaldo_Usina) AndAlso listaValoresSaldo_Usina.Count = 7 Then

                        Dim saldoDiaPDP As SaldoInflexibilidadePMO_DTO =
                        listaValoresSaldo_Usina.
                            Where(Function(s) Get_DataPDP_DateTime(s.DatPDP) = Get_DataPDP_DateTime(dataPDP)).
                            FirstOrDefault()

                        If Not IsNothing(saldoDiaPDP) Then

                            Dim arquivoDADGERValor As ArquivoDadgerValorDTO =
                            Me.FactoryDao.ArquivoDadgerValorDAO.Listar(dataPDP).
                                Where(Function(a) a.CodUsina.Trim() = codUsina.Trim()).
                                FirstOrDefault()

                            If Not IsNothing(arquivoDADGERValor) Then

                                Dim valorLimite As Integer = arquivoDADGERValor.ValorLimiteIfxPMO


                                Dim dataInicio As DateTime = Get_DataPDP_DateTime(semanaPMO.DataInicio.Replace("-", ""))
                                Dim dataFim As DateTime = Get_DataPDP_DateTime(dataPDP).AddDays(-1)

                                Dim total_IFX_DESSEM_DiasAnteriores_Usina As Integer =
                                Me.FactoryDao.InflexibilidadeDao.ListarPor_Data(dataInicio, dataFim).
                                    Where(Function(i) i.CodUsina.Trim() = codUsina.Trim()).
                                    ToList().
                                    Sum(Function(i) IIf(i.ValFlexiPre.HasValue, i.ValFlexiPre, 0))

                                Dim valorSaldo As Integer = valorLimite - total_IFX_DESSEM_DiasAnteriores_Usina

                                saldoDiaPDP.ValAcumuladoDESSEM_Semana = total_IFX_DESSEM_DiasAnteriores_Usina
                                saldoDiaPDP.ValEnviadoDESSEM = 0
                                saldoDiaPDP.ValProgramado = 0
                                saldoDiaPDP.ValSaldo = valorSaldo

                                listaRetorno.Add(saldoDiaPDP)

                            End If
                        End If

                    End If
                End If

            Catch ex As Exception
                Throw TratarErro(ex)
            End Try

            Return listaRetorno
        End Function

        Public Function CalcularSaldoInflexibilidadeSemanaPMO(dataPDP As String, ByVal codEmpresa As String) As Boolean Implements ISaldoInflexibilidadePMOBusiness.CalcularSaldoInflexibilidadeSemanaPMO

            Dim sucesso As Boolean = False
            Try
                'Validação de parâmetros de entrada
                If (IsNothing(dataPDP) OrElse (String.IsNullOrEmpty(dataPDP))) Then
                    Throw New BusinessException("Data PDP inválido")
                End If

                Dim semanaPMO As SemanaPMO = GetSemanaPMO(Get_DataPDP_DateTime(dataPDP), Nothing, Nothing).FirstOrDefault()
                Dim listaValoresSaldo As List(Of SaldoInflexibilidadePMO_DTO) = New List(Of SaldoInflexibilidadePMO_DTO)()
                Dim lista_CodUsina_ComInflexibilidade As New List(Of String)()

                If Not IsNothing(semanaPMO) Then

                    For Each dia As DateTime In semanaPMO.Datas_Inicio_Fim

                        Dim listaSaldoDia As List(Of SaldoInflexibilidadePMO_DTO) =
                            Me.FactoryDao.SaldoInflexibilidadePMO_DAO.ListarPor_DataPDP_Empresa(Get_DataPDP_Format(dia), codEmpresa).
                            ToList()

                        If Not IsNothing(listaSaldoDia) AndAlso listaSaldoDia.Count > 0 Then
                            'Guarda Valores de Saldo
                            listaValoresSaldo.AddRange(listaSaldoDia)

                            Dim usinasSaldo As List(Of String) =
                                listaSaldoDia.Select(Function(s) s.CodUsina).
                                Distinct().
                                ToList()

                            Dim lista_IFX_Dia As List(Of InflexibilidadeDTO) =
                                Me.FactoryDao.InflexibilidadeDao.ListarPor_DataPDP_Empresa(Get_DataPDP_Format(dia), codEmpresa)

                            For Each codUsinaSaldo As String In usinasSaldo
                                'Verifica se a Usina que está em Saldo tem IFX no mesmo dia
                                Dim existeIFX As Boolean =
                                    lista_IFX_Dia.Where(Function(i) i.CodUsina = codUsinaSaldo).Any()

                                If existeIFX Then
                                    'Guarda CodUsina das Usinas que precisando ser calculadas
                                    lista_CodUsina_ComInflexibilidade.Add(codUsinaSaldo)
                                End If
                            Next

                        End If
                    Next
                End If

                'Remove repetições de Codusina
                lista_CodUsina_ComInflexibilidade = lista_CodUsina_ComInflexibilidade.Distinct().ToList()
                Dim listaSaldoCalculado As List(Of SaldoInflexibilidadePMO_DTO) = New List(Of SaldoInflexibilidadePMO_DTO)()

                For Each codUsina_IFX As String In lista_CodUsina_ComInflexibilidade
                    listaSaldoCalculado.AddRange(
                    Me.CalcularSaldoInflexibilidadeSemanaPMO(dataPDP, codUsina_IFX, listaValoresSaldo))
                Next

                For Each saldoCalculado As SaldoInflexibilidadePMO_DTO In listaSaldoCalculado
                    Me.FactoryDao.SaldoInflexibilidadePMO_DAO.Atualizar(saldoCalculado)
                Next

                Me.FactoryDao.SaldoInflexibilidadePMO_DAO.Salvar()

                sucesso = True
            Catch ex As Exception
                Throw TratarErro(ex)
            End Try

            Return sucesso
        End Function
    End Class

End Namespace
