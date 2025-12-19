Imports System.Collections.Generic

Public Interface IOfertaExportacaoBusiness
    Inherits IBaseBusiness

    Function ValidarPrazoEnvioOfertaAgente(ByVal dataSelecionada As String, ByVal listaCodEmpresa As List(Of String)) As Boolean
    Function ValidarPrazoAnaliseOfertaAgente(dataSelecionada As String, listaCodEmpresa As List(Of String)) As Boolean
    Sub ExportarDadosParaBalanco(ByVal dataPDP As String, ByVal loginUsuario As String, ByVal forcarExportacao As Boolean, ListaAnalise As List(Of OfertaExportacaoDTO))
    Function Calculo_Exportacao_Geracao_PorUsinaPorConversora(ByVal dataPDP As String, ByVal ofertas As List(Of ValorOfertaExportacaoDTO), ByRef mensagensErro As String) As String
    Sub AtualizarDadosDeReferencia(ByVal dataPDP As String, ByVal ofertas As List(Of OfertaExportacaoDTO), Optional ByVal forcarAtualizacao As Boolean = False)
    Function ObterProximoNumeroOrdem(dataPDP As String, Optional codEmpre As String = "") As Integer
    Function ValidarExiste_OfertasNaoAnalisadasONSOriginal(dataPDP As String) As Boolean
    Function ValidarExiste_OfertasNaoAnalisadasONS(Lista As List(Of OfertaExportacaoDTO)) As Boolean
    Function ObterUltimaExportacaoBalancao(dataPDP As String) As OfertaExportacaoDTO
    Function ReiniciarDecisaoDeAnalise(ByVal dataPDP As String) As Boolean
    Function ReiniciarValoresReferencia(ByVal dataPDP As String) As Boolean
    Function ObterLimiteCadastrado(dataSelecionada As String, listaCodEmpresa As List(Of String), tipoEnvio As TipoEnvio, hora_padrao As String) As DateTime
    Function ListaTodasOfertasAnalises(dataPDP As String) As List(Of OfertaExportacaoDTO)
    Function Permitir_ExclusaoOfertas(dataPDP As String) As Boolean
    Sub IniciarAnaliseOfertasONS(dataPDP As String)
    Function RealizarLeituraPlanilhaValoresOfertaExportacaoONS(dataPDP As String, caminhoArquivo As String) As List(Of String)
    Function VerificaUsinasJaAprovadasOuReprovadas(dataPDP As String, checaFlgNula As Boolean) As List(Of Tuple(Of String, String))
    Function VerificaSeExisteUsinaEConversora(codUsina As String, codUsinaConversora As String) As Boolean
End Interface
