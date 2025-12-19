Imports pdpw

Public Class DespaDTO
    Inherits BaseDTO

    Private _dataPDP As String
    Private _codUsina As String
    Private _patamar As String
    Private _valDespaSup As Nullable(Of Integer)
    Private _valDespaEmp As Nullable(Of Integer)
    Private _valDespaPre As Nullable(Of Integer)
    Private _valDespaPro As Nullable(Of Integer)
    Private _valDespaTran As Nullable(Of Integer)


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

    Public Property ValDespaSup() As Nullable(Of Integer)
        Get
            Return _valDespaSup
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _valDespaSup = value
        End Set
    End Property

    Public Property ValDespaEmp() As Nullable(Of Integer)
        Get
            Return _valDespaEmp
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _valDespaEmp = value
        End Set
    End Property

    Public Property ValDespaPre() As Nullable(Of Integer)
        Get
            Return _valDespaPre
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _valDespaPre = value
        End Set
    End Property

    Public Property ValDespaPro() As Nullable(Of Integer)
        Get
            Return _valDespaPro
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _valDespaPro = value
        End Set
    End Property

    Public Property ValDespaTran() As Nullable(Of Integer)
        Get
            Return _valDespaTran
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _valDespaTran = value
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

        Return ""
    End Function

End Class
