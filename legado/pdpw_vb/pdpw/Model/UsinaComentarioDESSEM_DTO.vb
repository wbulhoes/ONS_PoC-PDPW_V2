Public Class UsinaComentarioDESSEM_DTO

    Private _codUsina As String
    Public Property CodUsina() As String
        Get
            Return _codUsina
        End Get
        Set(ByVal value As String)
            _codUsina = value
        End Set
    End Property

    Private _num_Patamar As String
    Public Property Num_Patamar() As String
        Get
            Return _num_Patamar
        End Get
        Set(ByVal value As String)
            _num_Patamar = value
        End Set
    End Property

    Private _valorDESSEM As String
    Public Property ValorDESSEM() As String
        Get
            Return _valorDESSEM
        End Get
        Set(ByVal value As String)
            _valorDESSEM = value
        End Set
    End Property

    Private _valorSugerido As String
    Public Property ValorSugerido() As String
        Get
            Return _valorSugerido
        End Get
        Set(ByVal value As String)
            _valorSugerido = value
        End Set
    End Property

    Private _flg_SugestaoAgente As String
    Public Property Flg_SugestaoAgente() As String
        Get
            Return _flg_SugestaoAgente
        End Get
        Set(ByVal value As String)
            _flg_SugestaoAgente = value
        End Set
    End Property

    Private _flg_Aprovado As String

    Public Sub New()
    End Sub

    Public Property Flg_Aprovado() As String
        Get
            Return _flg_Aprovado
        End Get
        Set(ByVal value As String)
            _flg_Aprovado = value
        End Set
    End Property

    Private _existeRegistrosPatamares As Boolean
    Public Property ExisteRegistrosPatamares() As Boolean
        Get
            Return _existeRegistrosPatamares
        End Get
        Set(ByVal value As Boolean)
            _existeRegistrosPatamares = value
        End Set
    End Property


End Class

