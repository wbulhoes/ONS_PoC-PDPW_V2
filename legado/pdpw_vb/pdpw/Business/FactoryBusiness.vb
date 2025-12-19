Imports System.Collections.Generic
Imports pdpw
Imports pdpw.Ons.interface.business

Public Class FactoryBusiness
    Implements IDisposable

    Private _listaBusiness As New List(Of IBaseBusiness)

    Private _ofertaExportacao As IOfertaExportacaoBusiness
    Private _usinaConversora As IUsinaConversoraBusiness
    Private _saldoInflexibilidadeSemanaPMO As ISaldoInflexibilidadePMOBusiness
    Private _inflexibilidadeBusiness As IInflexibilidadeBusiness
    Private _ArquivoDadgerValorBusiness As IArquivoDadgerValorBusiness
    Private _usinaBusiness As IUsinaBusiness
    Private _limiteEnvioBusiness As ILimiteEnvioBusiness
    Private _comentarioDESSEMBusiness As IComentarioDESSEMBusiness
    Private _intercambioBusiness As IIntercambioBusiness

    Public Sub Dispose() Implements IDisposable.Dispose
        For Each business As IBaseBusiness In _listaBusiness
            business.Dispose()
            business = Nothing
        Next
    End Sub

    Public ReadOnly Property OfertaExportacao As IOfertaExportacaoBusiness
        Get
            If IsNothing(_ofertaExportacao) Then
                _ofertaExportacao = New OfertaExportacaoBusiness()
                _listaBusiness.Add(_ofertaExportacao)
            End If

            Return _ofertaExportacao
        End Get
    End Property

    Public ReadOnly Property UsinaConversora As IUsinaConversoraBusiness
        Get
            If IsNothing(_usinaConversora) Then
                _usinaConversora = New UsinaConversoraBusiness()
                _listaBusiness.Add(_usinaConversora)
            End If
            Return _usinaConversora
        End Get
    End Property

    Public ReadOnly Property SaldoInflexibilidadeSemanaPMO As ISaldoInflexibilidadePMOBusiness
        Get
            If IsNothing(_saldoInflexibilidadeSemanaPMO) Then
                _saldoInflexibilidadeSemanaPMO = New SaldoInflexibilidadePMOBusiness()
                _listaBusiness.Add(_saldoInflexibilidadeSemanaPMO)
            End If

            Return _saldoInflexibilidadeSemanaPMO
        End Get
    End Property

    Public ReadOnly Property InflexibilidadeBusiness As IInflexibilidadeBusiness
        Get
            If IsNothing(_inflexibilidadeBusiness) Then
                _inflexibilidadeBusiness = New InflexibilidadeBusiness()
                _listaBusiness.Add(_inflexibilidadeBusiness)
            End If

            Return _inflexibilidadeBusiness
        End Get
    End Property

    Public ReadOnly Property ArquivoDadgerValorBusiness As IArquivoDadgerValorBusiness
        Get
            If IsNothing(_ArquivoDadgerValorBusiness) Then
                _ArquivoDadgerValorBusiness = New ArquivoDadgerValorBusiness()
                _listaBusiness.Add(_ArquivoDadgerValorBusiness)
            End If

            Return _ArquivoDadgerValorBusiness
        End Get
    End Property

    Public ReadOnly Property UsinaBusiness() As IUsinaBusiness
        Get
            If IsNothing(_usinaBusiness) Then
                _usinaBusiness = New UsinaBusiness()
                _listaBusiness.Add(_usinaBusiness)
            End If
            Return _usinaBusiness
        End Get
    End Property

    Public ReadOnly Property LimiteEnvioBusiness() As ILimiteEnvioBusiness
        Get
            If IsNothing(_limiteEnvioBusiness) Then
                _limiteEnvioBusiness = New LimiteEnvioBusiness()
                _listaBusiness.Add(_limiteEnvioBusiness)
            End If
            Return _limiteEnvioBusiness
        End Get
    End Property

    Public ReadOnly Property ComentarioDESSEMBusiness As IComentarioDESSEMBusiness
        Get
            If IsNothing(Me._comentarioDESSEMBusiness) Then
                _comentarioDESSEMBusiness = New ComentarioDESSEMBusiness()
                _listaBusiness.Add(_comentarioDESSEMBusiness)
            End If
            Return _comentarioDESSEMBusiness
        End Get
    End Property

    Public ReadOnly Property IntercambioBusiness As IIntercambioBusiness
        Get
            If IsNothing(Me._intercambioBusiness) Then
                _intercambioBusiness = New IntercambioBusiness()
                _listaBusiness.Add(_intercambioBusiness)
            End If

            Return _intercambioBusiness
        End Get
    End Property
End Class
