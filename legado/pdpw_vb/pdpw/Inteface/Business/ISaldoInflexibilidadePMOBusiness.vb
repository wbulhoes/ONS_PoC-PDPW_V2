Imports System.Collections.Generic

Public Interface ISaldoInflexibilidadePMOBusiness
    Inherits IBaseBusiness

    ''' <summary>
    ''' Realiza calculo de Saldo para uma Usina em uma Data de Programação com dados importados do arquivo DADGER
    ''' 
    ''' BusinessException : "Data PDP, CodUsina ou Valor de Inflexiblidade Programado inválidos"
    ''' 
    ''' </summary>
    ''' <param name="dataPDP">Data de Programação</param>
    ''' <param name="codUsina">Código da usina</param>
    ''' <param name="total_IFX_Programado">Valor de Inflexibilidade Programado</param>
    ''' <param name="listaValoresSalvo_IFX_SemanaPMO">Lista de registros de Saldo Inflexibilidade para Semana PMO da Data PDP Indicada</param>
    ''' <returns>Retorna a lista de Saldo Inflexibilidade para Semana PMO atualizada com o cálculo realizado de Saldo</returns>
    Function CalcularSaldoInflexibilidadeSemanaPMO(ByVal dataPDP As String,
                                                              ByVal codUsina As String,
                                                              ByVal listaValoresSalvo_IFX_SemanaPMO As List(Of SaldoInflexibilidadePMO_DTO)
                                                              ) As List(Of SaldoInflexibilidadePMO_DTO)

    ''' <summary>
    ''' Realiza o cálculo de Saldo de Inflexibilidade para TODAS as Usinas com oferta de Inflexibilidade para Data PDP indicada
    ''' 
    ''' BusinessException : "Data PDP inválida"
    ''' 
    ''' </summary>
    ''' <param name="dataPDP">Data de Programação</param>
    ''' <returns>Retorna se houve sucesso na atualização dos cálculos de saldo</returns>
    Function CalcularSaldoInflexibilidadeSemanaPMO(ByVal dataPDP As String, ByVal codEmpresa As String) As Boolean



End Interface
