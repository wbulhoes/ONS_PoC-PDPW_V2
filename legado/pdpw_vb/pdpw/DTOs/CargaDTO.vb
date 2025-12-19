Public Class CargaDTO
    Inherits BaseDTO

    Private _dataPDP As String
    Private _codEmpre As String
    Private _patamar As String
    Private _valCargaSup As Nullable(Of Integer)
    Private _valCargaEmp As Nullable(Of Integer)
    Private _valCargaPre As Nullable(Of Integer)
    Private _valCargaPro As Nullable(Of Integer)
    Private _valCargaTran As Nullable(Of Integer)

    Public Property DataPDP As String
        Get
            Return _dataPDP
        End Get
        Set(value As String)
            _dataPDP = value
        End Set
    End Property

    Public Property CodEmpre As String
        Get
            Return _codEmpre
        End Get
        Set(value As String)
            _codEmpre = value
        End Set
    End Property

    Public Property Patamar As String
        Get
            Return _patamar
        End Get
        Set(value As String)
            _patamar = value
        End Set
    End Property

    Public Property ValCargaSup As Integer?
        Get
            Return _valCargaSup
        End Get
        Set(value As Integer?)
            _valCargaSup = value
        End Set
    End Property

    Public Property ValCargaEmp As Integer?
        Get
            Return _valCargaEmp
        End Get
        Set(value As Integer?)
            _valCargaEmp = value
        End Set
    End Property

    Public Property ValCargaPre As Integer?
        Get
            Return _valCargaPre
        End Get
        Set(value As Integer?)
            _valCargaPre = value
        End Set
    End Property

    Public Property ValCargaPro As Integer?
        Get
            Return _valCargaPro
        End Get
        Set(value As Integer?)
            _valCargaPro = value
        End Set
    End Property

    Public Property ValCargaTran As Integer?
        Get
            Return _valCargaTran
        End Get
        Set(value As Integer?)
            _valCargaTran = value
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
