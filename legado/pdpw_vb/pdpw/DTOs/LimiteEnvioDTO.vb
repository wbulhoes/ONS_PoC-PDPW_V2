Public Class LimiteEnvioDTO
    Inherits BaseDTO

    Private _dataPDP As String
    Private _codEmpre As String
    Private _tipoStatus As Integer
    Private _data_Limite As DateTime
    Private _hora_Limite As DateTime

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

    Public Property TipoStatus As Integer
        Get
            Return _tipoStatus
        End Get
        Set(value As Integer)
            _tipoStatus = value
        End Set
    End Property

    Public ReadOnly Property TipoEnvio As TipoEnvio
        Get
            Dim _tipoEnvio As TipoEnvio = Nothing

            If Not IsNothing(Me._tipoStatus) Then
                _tipoEnvio = [Enum].Parse(GetType(TipoEnvio), Me._tipoStatus.ToString())
            End If

            Return _tipoEnvio
        End Get
    End Property

    Public Property Data_Limite As DateTime
        Get
            Return _data_Limite
        End Get
        Set(value As Date)
            _data_Limite = value
        End Set
    End Property
    Public Property Hora_Limite As DateTime
        Get
            Return _hora_Limite
        End Get
        Set(value As DateTime)
            _hora_Limite = value
        End Set
    End Property

    Public ReadOnly Property DataHora_Limite As DateTime
        Get
            Dim _dataHora_limite As DateTime = DateTime.MinValue

            If Not IsNothing(Me._data_Limite) Then
                _dataHora_limite = DateTime.Parse(Me._data_Limite.ToString("dd/MM/yyyy"))
            End If

            If Not IsNothing(Me._hora_Limite) Then
                _dataHora_limite = DateTime.Parse($"{_dataHora_limite.ToString("dd/MM/yyyy")} {Me._hora_Limite.ToString("HH:mm:ss")}")
            End If

            Return _dataHora_limite
        End Get
    End Property

    Public Overrides Function ObterComando() As String
        Throw New NotImplementedException()
    End Function
End Class
