Public Class InflexibilidadeDTO
    Inherits BaseDTO

    Private _dataPdp As String
    Private _codUsina As String
    Private _intFlexi As String
    Private _valFlexiEmp As Nullable(Of Integer)
    Private _valFlexiPre As Nullable(Of Integer)
    Private _valFlexiPro As Nullable(Of Integer)
    Private _valFlexiSup As Nullable(Of Integer)
    Private _valFlexiTran As Nullable(Of Integer)

    Public Property DataPDP() As String
        Get
            Return _dataPdp
        End Get
        Set(ByVal value As String)
            _dataPdp = value
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

    Public Property IntFlexi() As String
        Get
            Return _intFlexi
        End Get
        Set(value As String)
            _intFlexi = value
        End Set
    End Property

    Public Property ValFlexiEmp() As Nullable(Of Integer)
        Get
            Return _valFlexiEmp
        End Get
        Set(value As Nullable(Of Integer))
            _valFlexiEmp = value
        End Set
    End Property

    Public Property ValFlexiPre() As Nullable(Of Integer)
        Get
            Return _valFlexiPre
        End Get
        Set(value As Nullable(Of Integer))
            _valFlexiPre = value
        End Set
    End Property

    Public Property ValFlexiPro() As Nullable(Of Integer)
        Get
            Return _valFlexiPro
        End Get
        Set(value As Nullable(Of Integer))
            _valFlexiPro = value
        End Set
    End Property

    Public Property ValFlexiSup() As Nullable(Of Integer)
        Get
            Return _valFlexiSup
        End Get
        Set(value As Nullable(Of Integer))
            _valFlexiSup = value
        End Set
    End Property

    Public Property ValFlexiTran() As Integer?
        Get
            Return _valFlexiTran
        End Get
        Set(value As Integer?)
            _valFlexiTran = value
        End Set
    End Property

    Public Overrides Function ObterComando() As String
        Select Case State
            Case StateDTO.Added
                Return "INSERT INTO inflexibilidade " &
                       " (datpdp, codusina, intflexi, valflexiemp, valflexipro, valflexipre, valflexisup, valflexitran) " &
                       $" VALUES ('{_dataPdp}', '{_codUsina}', {_intFlexi}, {_valFlexiEmp}, {_valFlexiPro}, {_valFlexiPre}, {_valFlexiSup}, {_valFlexiTran}) ;"
            Case StateDTO.Modified
                Return "UPDATE inflexibilidade " &
                        $" SET valflexiemp = {_valFlexiEmp}, valflexipro= {_valFlexiPro}, valflexipre= {_valFlexiPre}, valflexisup= {_valFlexiSup}, valflexitran= {_valFlexiTran} " &
                        $" WHERE datpdp ='{_dataPdp}' AND codusina='{_codUsina}' AND intflexi={_intFlexi}; "
            Case StateDTO.Removed
                Return $"DELETE From inflexibilidade WHERE datpdp='{_dataPdp}' AND codusina='{_codUsina}' AND intflexi={_intFlexi};"
        End Select
    End Function
End Class
