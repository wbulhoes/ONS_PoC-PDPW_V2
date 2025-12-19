Public Class UsinaDTO
    Inherits BaseDTO

    Private _codUsina As String
    Private _nomeUsina As String
    Private _codEmpre As String
    Private _potinstalada As Nullable(Of Integer)
    Private _dppId As Nullable(Of Double)
    Private _sigsme As String
    Private _tpusinaId As String
    Private _usiBdtId As String

    Public Overrides Function ToString() As String
        'Return MyBase.ToString()
        Return $"{Me.CodEmpre} - {Me.CodUsina} - {Me.NomeUsina}"
    End Function

    Public Property CodUsina() As String
        Get
            Return _codUsina
        End Get
        Set(value As String)
            _codUsina = value
        End Set
    End Property

    Public Property NomeUsina() As String
        Get
            Return _nomeUsina
        End Get
        Set(value As String)
            _nomeUsina = value
        End Set
    End Property

    Public Property CodEmpre() As String
        Get
            Return _codEmpre
        End Get
        Set(value As String)
            _codEmpre = value
        End Set
    End Property

    Public Property Potinstalada() As Nullable(Of Integer)
        Get
            Return _potinstalada
        End Get
        Set(value As Nullable(Of Integer))
            _potinstalada = value
        End Set
    End Property

    Public Property DppId() As Nullable(Of Double)
        Get
            Return _dppId
        End Get
        Set(value As Nullable(Of Double))
            _dppId = value
        End Set
    End Property

    Public Property Sigsme() As String
        Get
            Return _sigsme
        End Get
        Set(value As String)
            _sigsme = value
        End Set
    End Property

    Public Property TpusinaId() As String
        Get
            Return _tpusinaId
        End Get
        Set(value As String)
            _tpusinaId = value
        End Set
    End Property

    Public Property UsiBdtId As String
        Get
            Return _usiBdtId
        End Get
        Set(value As String)
            _usiBdtId = value
        End Set
    End Property

    Public Overrides Function ObterComando() As String
        Select Case State
            Case StateDTO.Added
                Throw New NotImplementedException()
            Case StateDTO.Modified
                Throw New NotImplementedException()
            Case StateDTO.Removed
                Throw New NotImplementedException()
        End Select
    End Function
End Class
