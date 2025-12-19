<Obsolete("Utilizar a classe 'PDPW/DTOs/UsinaConversoraDTO'")>
Public Class ConversoraValorOfertaDTO

    Public Overrides Function ToString() As String
        Return $"{_datPdp} - {_codUsina.Trim()} - {_codConversora.Trim()} - {_num_Patamar} - {_ValorSugeridoAgente} - {_valorSugeridoOns} - {_valPerdas}"
    End Function

    Private _datPdp As String
    Public Property DatPdp() As String
        Get
            Return _datPdp
        End Get
        Set(ByVal value As String)
            _datPdp = value
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
    Public Property CodConversora() As String
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

    Private _num_Patamar As Integer
    Public Property Num_Patamar() As Integer
        Get
            Return _num_Patamar
        End Get
        Set(ByVal value As Integer)
            _num_Patamar = value
        End Set
    End Property

    Private _ValorSugeridoAgente As Nullable(Of Integer)
    Public Property ValorSugeridoAgente() As Nullable(Of Integer)
        Get
            Return _ValorSugeridoAgente
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _ValorSugeridoAgente = value
        End Set
    End Property

    Private _valorSugeridoOns As Nullable(Of Integer)
    Public Property ValorSugeridoOns() As Nullable(Of Integer)
        Get
            Return _valorSugeridoOns
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _valorSugeridoOns = value
        End Set
    End Property

    Private _valorLiquidoOns As Nullable(Of Double)
    Public Property ValorLiquidoOns() As Nullable(Of Double)
        Get
            Return _valorLiquidoOns
        End Get
        Set(ByVal value As Nullable(Of Double))
            _valorLiquidoOns = value
        End Set
    End Property

    Private _flgaprovadoons As String
    Public Property Flgaprovadoons() As String
        Get
            Return _flgaprovadoons
        End Get
        Set(ByVal value As String)
            _flgaprovadoons = value
        End Set
    End Property

    Private _flgaprovadoagente As String
    Public Property Flgaprovadoagente() As String
        Get
            Return _flgaprovadoagente
        End Get
        Set(ByVal value As String)
            _flgaprovadoagente = value
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

    Private _valPerdas As Nullable(Of Double)
    Public Property valorPerdas() As Nullable(Of Double)
        Get
            Return _valPerdas
        End Get
        Set(ByVal value As Nullable(Of Double))
            _valPerdas = value
        End Set
    End Property


    Private _numeroPrioridade As Integer

End Class
