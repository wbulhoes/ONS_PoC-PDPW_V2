Public Class PerdaDTO
    Inherits BaseDTO

    Private _dataPDP As String
    Private _codUsina As String
    Private _patamar As String
    Private _valpccsup As Nullable(Of Integer)
    Private _valpccemp As Nullable(Of Integer)
    Private _valpccpre As Nullable(Of Integer)
    Private _valpccpro As Nullable(Of Integer)
    Private _valpcctran As Nullable(Of Integer)

    Public Property DataPDP() As String
        Get
            Return _dataPDP
        End Get
        Set(ByVal value As String)
            _dataPDP = value
        End Set
    End Property

    Public Property CodUsina() As String
        Get
            Return _codUsina
        End Get
        Set(ByVal value As String)
            _codUsina = value
        End Set
    End Property

    Public Property Patamar() As String
        Get
            Return _patamar
        End Get
        Set(ByVal value As String)
            _patamar = value
        End Set
    End Property

    Public Property Valpccsup() As Nullable(Of Integer)
        Get
            Return _valpccsup
        End Get
        Set(ByVal Value As Nullable(Of Integer))
            _valpccsup = Value
        End Set
    End Property

    Public Property ValpccEmp() As Nullable(Of Integer)
        Get
            Return _valpccemp
        End Get
        Set(ByVal Value As Nullable(Of Integer))
            _valpccemp = Value
        End Set
    End Property

    Public Property ValpccPre() As Nullable(Of Integer)
        Get
            Return _valpccpre
        End Get
        Set(ByVal Value As Nullable(Of Integer))
            _valpccpre = Value
        End Set
    End Property

    Public Property ValpccPro() As Nullable(Of Integer)
        Get
            Return _valpccpro
        End Get
        Set(ByVal Value As Nullable(Of Integer))
            _valpccpro = Value
        End Set
    End Property

    Public Property ValpccTran() As Nullable(Of Integer)
        Get
            Return _valpcctran
        End Get
        Set(ByVal Value As Nullable(Of Integer))
            _valpcctran = Value
        End Set
    End Property

    Public Overrides Function ObterComando() As String
        Select Case State
            Case StateDTO.Added
                Throw New NotImplementedException()
                Return ""
            Case StateDTO.Modified
                Throw New NotImplementedException()
                Return ""
            Case StateDTO.Removed
                Throw New NotImplementedException()
                Return ""
        End Select
    End Function

End Class
