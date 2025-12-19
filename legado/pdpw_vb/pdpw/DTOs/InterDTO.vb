Public Class InterDTO
    Inherits BaseDTO

    Private _dataPDP As String
    Private _tipInter As String
    Private _codEmpreDe As String
    Private _codEmprePara As String
    Private _codContaDe As String
    Private _codContaPara As String
    Private _codContaModal As String
    Private _patamar As String
    Private _valInterSup As Nullable(Of Integer)
    Private _valInterEmp As Nullable(Of Integer)
    Private _valInterPre As Nullable(Of Integer)
    Private _valInterPro As Nullable(Of Integer)
    Private _valInterTran As Nullable(Of Integer)

    Public Property DataPDP As String
        Get
            Return _dataPDP
        End Get
        Set(value As String)
            _dataPDP = value
        End Set
    End Property

    Public Property TipInter As String
        Get
            Return _tipInter
        End Get
        Set(value As String)
            _tipInter = value
        End Set
    End Property

    Public Property CodEmpreDe As String
        Get
            Return _codEmpreDe
        End Get
        Set(value As String)
            _codEmpreDe = value
        End Set
    End Property

    Public Property CodEmprePara As String
        Get
            Return _codEmprePara
        End Get
        Set(value As String)
            _codEmprePara = value
        End Set
    End Property

    Public Property CodContaDe As String
        Get
            Return _codContaDe
        End Get
        Set(value As String)
            _codContaDe = value
        End Set
    End Property

    Public Property CodContaPara As String
        Get
            Return _codContaPara
        End Get
        Set(value As String)
            _codContaPara = value
        End Set
    End Property

    Public Property CodContaModal As String
        Get
            Return _codContaModal
        End Get
        Set(value As String)
            _codContaModal = value
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

    Public Property ValInterSup As Integer?
        Get
            Return _valInterSup
        End Get
        Set(value As Integer?)
            _valInterSup = value
        End Set
    End Property

    Public Property ValInterEmp As Integer?
        Get
            Return _valInterEmp
        End Get
        Set(value As Integer?)
            _valInterEmp = value
        End Set
    End Property

    Public Property ValInterPre As Integer?
        Get
            Return _valInterPre
        End Get
        Set(value As Integer?)
            _valInterPre = value
        End Set
    End Property

    Public Property ValInterPro As Integer?
        Get
            Return _valInterPro
        End Get
        Set(value As Integer?)
            _valInterPro = value
        End Set
    End Property

    Public Property ValInterTran As Integer?
        Get
            Return _valInterTran
        End Get
        Set(value As Integer?)
            _valInterTran = value
        End Set
    End Property

    Public Overrides Function ObterComando() As String
        Select Case State
            Case StateDTO.Added
                Throw New NotImplementedException()
                Return ""
            Case StateDTO.Modified
                Return $" Update Inter Set 
                          ValInterTran = {_valInterTran},
                          ValInterEmp = {_valInterEmp},
                          ValInterPro = {_valInterPro},
                          ValInterPre = {_valInterPre},
                          ValInterSup = {_valInterSup},
                          TipInter = '{_tipInter}'
                          Where 
                            DatPDP = '{_dataPDP}' and
                            CodEmpreDe = '{_codEmpreDe}' and 
                            CodEmprePara = '{_codEmprePara}' and 
                            CodContaDe = '{_codContaDe}' and 
                            CodContaPara = '{_codContaPara}' and 
                            CodContaModal = '{_codContaModal}' and 
                            IntInter = {_patamar} ; "
            Case StateDTO.Removed
                Throw New NotImplementedException()
                Return ""
        End Select

        Return ""
    End Function
End Class
