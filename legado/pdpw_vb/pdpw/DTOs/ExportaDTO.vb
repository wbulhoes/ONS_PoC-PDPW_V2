Public Class ExportaDTO
    Inherits BaseDTO

    Private _dataPDP As String
    Private _codUsina As String
    Private _patamar As Integer
    Private _valExportaSUP As Nullable(Of Integer)
    Private _valExportaEmp As Nullable(Of Integer)
    Private _valExportaPro As Nullable(Of Integer)
    Private _valExportaPre As Nullable(Of Integer)
    Private _valExportaTran As Nullable(Of Integer)

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

    Public Property Patamar() As Integer
        Get
            Return _patamar
        End Get
        Set(ByVal value As Integer)
            _patamar = value
        End Set
    End Property

    Public Property ValExportaSUP() As Nullable(Of Integer)
        Get
            Return _valExportaSUP
        End Get
        Set(ByVal Value As Nullable(Of Integer))
            _valExportaSUP = Value
        End Set
    End Property

    Public Property ValExportaEmp() As Nullable(Of Integer)
        Get
            Return _valExportaEmp
        End Get
        Set(ByVal Value As Nullable(Of Integer))
            _valExportaEmp = Value
        End Set
    End Property

    Public Property ValExportaPro() As Nullable(Of Integer)
        Get
            Return _valExportaPro
        End Get
        Set(ByVal Value As Nullable(Of Integer))
            _valExportaPro = Value
        End Set
    End Property

    Public Property ValExportaPre() As Nullable(Of Integer)
        Get
            Return _valExportaPre
        End Get
        Set(ByVal Value As Nullable(Of Integer))
            _valExportaPre = Value
        End Set
    End Property

    Public Property ValExportaTran() As Nullable(Of Integer)
        Get
            Return _valExportaTran
        End Get
        Set(ByVal Value As Nullable(Of Integer))
            _valExportaTran = Value
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
