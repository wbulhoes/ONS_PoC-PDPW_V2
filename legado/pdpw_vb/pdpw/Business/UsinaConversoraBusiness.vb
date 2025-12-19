Imports System.Collections.Generic
Imports System.Linq

Namespace Ons.interface.business

    Public Class UsinaConversoraBusiness
        Inherits BaseBusiness
        Implements IUsinaConversoraBusiness

        Public Sub Excluir(ByVal id_usinaConversora As String) Implements IUsinaConversoraBusiness.Excluir
            Dim dataPdp As String = DateTime.Now.ToString("yyyyMMdd")

            Dim usinaConversoraBD As UsinaConversoraDTO = Me.FactoryDao.UsinaConversoraDAO.ListarUsinaConversora(Int32.Parse(id_usinaConversora)).FirstOrDefault()

            Dim existeOfertaFutura As Boolean = Me.FactoryDao.OfertaExportacaoDAO.ExisteOfertaExportacaoFutura(dataPdp, usinaConversoraBD.CodUsina, usinaConversoraBD.CodUsiConversora)

            If existeOfertaFutura = True Then
                Throw New Exception($"Não é possivel excluir a associação Usina {usinaConversoraBD.CodUsina.Trim()} com Conversora {usinaConversoraBD.CodUsiConversora.Trim()} pois existem ofertas futuras.")
            End If

            Me.FactoryDao.UsinaConversoraDAO.Excluir(usinaConversoraBD)
            Me.FactoryDao.UsinaConversoraDAO.Salvar()

        End Sub

        Public Function ExisteUsinaConversora(codUsina As String, codUsinaConversora As String) As Boolean Implements IUsinaConversoraBusiness.ExisteUsinaConversora
            Return Me.FactoryDao.UsinaConversoraDAO.ExisteUsinaConversora(codUsina, codUsinaConversora)
        End Function

        Public Function ExisteOfertaFutura_PendenteAnalise_UsinaConversora(codUsina As String, codUsinaConversora As String) As Boolean Implements IUsinaConversoraBusiness.ExisteOfertaFutura_PendenteAnalise_UsinaConversora
            Return Me.FactoryDao.UsinaConversoraDAO.ExisteOfertaFutura_PendenteAnalise_UsinaConversora(codUsina, codUsinaConversora)
        End Function
    End Class

End Namespace
