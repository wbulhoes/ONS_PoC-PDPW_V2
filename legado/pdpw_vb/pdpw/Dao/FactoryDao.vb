Imports System.Collections.Generic
Imports pdpw

Public Class FactoryDAO
    Implements IDisposable

    Private _PDP_DAO As PDP_DAO
    Private _despDAO As DespaDAO
    Private _exportaDAO As ExportaDAO
    Private _perdaDAO As PerdaDAO
    Private _valorOfertaExportacaoDAO As ValorOfertaExportacaoDAO
    Private _ofertaExportacaoDAO As OfertaExportacaoDAO
    Private _usinaConversoraDAO As UsinaConversoraDAO
    Private _saldoInflexibilidadePMO_DAO As SaldoInflexibilidadePMO_DAO
    Private _inflexibilidadeDao As InflexibilidadeDao
    Private _arquivoDadgerValorDAO As ArquivoDadgerValorDAO
    Private _usinaDAO As UsinaDAO
    Private _limiteEnvioDAO As LimiteEnvioDAO
    Private _interDAO As InterDAO
    Private _cargaDAO As CargaDAO

    Public Sub Dispose() Implements IDisposable.Dispose

        If Not IsNothing(_interDAO) Then
            _interDAO.Dispose()
            _interDAO = Nothing
        End If
        If Not IsNothing(_despDAO) Then
            _despDAO.Dispose()
            _despDAO = Nothing
        End If
        If Not IsNothing(_exportaDAO) Then
            _exportaDAO.Dispose()
            _exportaDAO = Nothing
        End If
        If Not IsNothing(_perdaDAO) Then
            _perdaDAO.Dispose()
            _perdaDAO = Nothing
        End If
        If Not IsNothing(_valorOfertaExportacaoDAO) Then
            _valorOfertaExportacaoDAO.Dispose()
            _valorOfertaExportacaoDAO = Nothing
        End If

        If Not IsNothing(_PDP_DAO) Then
            _PDP_DAO.Dispose()
            _PDP_DAO = Nothing
        End If

        If Not IsNothing(_saldoInflexibilidadePMO_DAO) Then
            _saldoInflexibilidadePMO_DAO.Dispose()
            _saldoInflexibilidadePMO_DAO = Nothing
        End If

    End Sub

    Public ReadOnly Property DespaDAO() As DespaDAO
        Get
            If IsNothing(_despDAO) Then
                _despDAO = New DespaDAO()
            End If

            Return _despDAO
        End Get
    End Property

    Public ReadOnly Property ExportaDAO() As ExportaDAO
        Get
            If IsNothing(_exportaDAO) Then
                _exportaDAO = New ExportaDAO()
            End If

            Return _exportaDAO
        End Get
    End Property

    Public ReadOnly Property PerdaDAO() As PerdaDAO
        Get
            If IsNothing(_perdaDAO) Then
                _perdaDAO = New PerdaDAO
            End If

            Return _perdaDAO
        End Get
    End Property

    Public ReadOnly Property ValoresOfertaExportacaoDAO As ValorOfertaExportacaoDAO
        Get
            If IsNothing(_valorOfertaExportacaoDAO) Then
                _valorOfertaExportacaoDAO = New ValorOfertaExportacaoDAO
            End If

            Return _valorOfertaExportacaoDAO
        End Get
    End Property

    Public ReadOnly Property PDP_DAO As PDP_DAO
        Get
            If IsNothing(_PDP_DAO) Then
                _PDP_DAO = New PDP_DAO
            End If

            Return _PDP_DAO
        End Get
    End Property

    Public ReadOnly Property OfertaExportacaoDAO As OfertaExportacaoDAO
        Get
            If IsNothing(_ofertaExportacaoDAO) Then
                _ofertaExportacaoDAO = New OfertaExportacaoDAO()
            End If

            Return _ofertaExportacaoDAO
        End Get
    End Property

    Public ReadOnly Property UsinaConversoraDAO As UsinaConversoraDAO
        Get
            If IsNothing(_usinaConversoraDAO) Then
                _usinaConversoraDAO = New UsinaConversoraDAO()
            End If
            Return _usinaConversoraDAO
        End Get
    End Property

    Public ReadOnly Property SaldoInflexibilidadePMO_DAO As SaldoInflexibilidadePMO_DAO
        Get
            If IsNothing(_saldoInflexibilidadePMO_DAO) Then
                _saldoInflexibilidadePMO_DAO = New SaldoInflexibilidadePMO_DAO()
            End If
            Return _saldoInflexibilidadePMO_DAO

        End Get
    End Property

    Public ReadOnly Property InflexibilidadeDao As InflexibilidadeDao
        Get
            If IsNothing(_inflexibilidadeDao) Then
                _inflexibilidadeDao = New InflexibilidadeDao()
            End If
            Return _inflexibilidadeDao
        End Get
    End Property

    Public ReadOnly Property ArquivoDadgerValorDAO As ArquivoDadgerValorDAO
        Get
            If IsNothing(_arquivoDadgerValorDAO) Then
                _arquivoDadgerValorDAO = New ArquivoDadgerValorDAO()
            End If
            Return _arquivoDadgerValorDAO
        End Get
    End Property

    Public ReadOnly Property UsinaDAO() As UsinaDAO
        Get
            If IsNothing(_usinaDAO) Then
                _usinaDAO = New UsinaDAO()
            End If
            Return _usinaDAO
        End Get
    End Property

    Public ReadOnly Property LimiteEnvioDAO() As LimiteEnvioDAO
        Get
            If IsNothing(_limiteEnvioDAO) Then
                _limiteEnvioDAO = New LimiteEnvioDAO()
            End If
            Return _limiteEnvioDAO
        End Get
    End Property

    Public ReadOnly Property InterDAO As InterDAO
        Get
            If IsNothing(_interDAO) Then
                _interDAO = New InterDAO()
            End If
            Return _interDAO
        End Get
    End Property

    Public ReadOnly Property CargaDAO As CargaDAO
        Get
            If IsNothing(_cargaDAO) Then
                _cargaDAO = New CargaDAO()
            End If
            Return _cargaDAO
        End Get

    End Property
End Class
