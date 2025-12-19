Imports System.Collections.Generic

<Obsolete("Utilizar a classe 'PDPW/DTOs/UsinaConversoraDTO'")>
Public Class UsiConversDTO

    Public Overrides Function ToString() As String
        Return $"{_datPDP} - {_codUsina} - {_codConversora} - Valores: {_valoresOfertaUsiConversora.Count}"
    End Function

    Private _numeroPrioridade As Integer

    Private _datPDP As String
    Public Property DatPdp() As String
        Get
            Return _datPDP
        End Get
        Set(ByVal value As String)
            _datPDP = value
        End Set
    End Property

    Private _codUsina As String
    Public Property CodUsina() As String
        Get
            Return _codUsina
        End Get
        Set(ByVal value As String)
            _codUsina = value
        End Set
    End Property

    Private _codConversora As String
    Public Property codConversora() As String
        Get
            Return _codConversora
        End Get
        Set(ByVal value As String)
            _codConversora = value
        End Set
    End Property

    Private _nomUsina As String
    Public Property NomUsina() As String
        Get
            Return _nomUsina
        End Get
        Set(ByVal value As String)
            _nomUsina = value
        End Set
    End Property

    Private _nomConversora As String
    Public Property nomConversora() As String
        Get
            Return _nomConversora
        End Get
        Set(ByVal value As String)
            _nomConversora = value
        End Set
    End Property

    Private _OrdemAgente As Integer
    Public Property OrdemAgente() As Integer
        Get
            Return _OrdemAgente
        End Get
        Set(ByVal value As Integer)
            _OrdemAgente = value
        End Set
    End Property

    Private _OrdemOns As Integer
    Public Property OrdemOns() As Integer
        Get
            Return _OrdemOns
        End Get
        Set(ByVal value As Integer)
            _OrdemOns = value
        End Set
    End Property

    Private _dinofertaexportacao As String
    Public Property dinofertaexportacao() As String
        Get
            Return _dinofertaexportacao
        End Get
        Set(ByVal value As String)
            _dinofertaexportacao = value
        End Set
    End Property

    Private _flgaprovadoons As String
    Public Property flgaprovadoons() As String
        Get
            Return _flgaprovadoons
        End Get
        Set(value As String)
            _flgaprovadoons = value
        End Set
    End Property

    Private _dinanaliseons As String
    Public Property dinanaliseons() As String
        Get
            Return _dinanaliseons
        End Get
        Set(ByVal value As String)
            _dinanaliseons = value
        End Set
    End Property

    Private _flgaprovadoagente As String
    Public Property flgaprovadoagente() As String
        Get
            Return _flgaprovadoagente
        End Get
        Set(value As String)
            _flgaprovadoagente = value
        End Set
    End Property

    Private _dinanaliseagente As String
    Public Property dinanaliseagente() As String
        Get
            Return _dinanaliseagente
        End Get
        Set(ByVal value As String)
            _dinanaliseagente = value
        End Set
    End Property

    Private _valoresOfertaUsiConversora As List(Of ConversoraValorOfertaDTO)
    Public Property ValoresOfertaUsiConversora() As List(Of ConversoraValorOfertaDTO)
        Get
            Return _valoresOfertaUsiConversora
        End Get
        Set(ByVal value As List(Of ConversoraValorOfertaDTO))
            _valoresOfertaUsiConversora = value
        End Set
    End Property

    Private _idUsinaConversora As String
    Public Property IdUsinaConversora() As String
        Get
            Return _idUsinaConversora
        End Get
        Set(ByVal value As String)
            _idUsinaConversora = value
        End Set
    End Property

    Private _codUsinaConversora As String
    Public Property CodUsinaConversora() As String
        Get
            Return _codUsinaConversora
        End Get
        Set(ByVal value As String)
            _codUsinaConversora = value
        End Set
    End Property

    Private _percentualPerda As Decimal
    Public Property PercentualPerda() As Decimal
        Get
            Return _percentualPerda
        End Get
        Set(ByVal value As Decimal)
            _percentualPerda = value
        End Set
    End Property

    Private _login_AgenteOferta As String = ""
    Public Property Login_AgenteOferta() As String
        Get
            Return _login_AgenteOferta
        End Get
        Set(ByVal value As String)
            _login_AgenteOferta = value
        End Set
    End Property

    Public Property NumeroPrioridade As Integer
        Get
            Return _numeroPrioridade
        End Get
        Set(value As Integer)
            _numeroPrioridade = value
        End Set
    End Property
End Class
