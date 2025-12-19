Public Class PDP_DTO
    Inherits BaseDTO

    Private _dataPDP As String

    Public Property DataPDP As String
        Get
            Return _dataPDP
        End Get
        Set(value As String)
            _dataPDP = value
        End Set
    End Property

    Public ReadOnly Property DataPDP_Formatada As String
        Get
            Dim dataFormatada As String = ""
            If Not IsNothing(_dataPDP) AndAlso Not String.IsNullOrEmpty(_dataPDP) AndAlso _dataPDP.Length = 8 Then
                Dim ano = _dataPDP.Substring(0, 4)
                Dim mes = _dataPDP.Substring(4, 2)
                Dim dia = _dataPDP.Substring(6, 2)

                dataFormatada = dia + "/" + mes + "/" + ano
            End If

            Return dataFormatada
        End Get
    End Property

    Public Overrides Function ObterComando() As String
        Throw New NotImplementedException()
    End Function
End Class
