Public Class UsinaConversoraDTO
    Inherits BaseDTO

    Private _idUsinaConversora As Integer
    Public Property IdUsinaConversora() As Integer
        Get
            Return _idUsinaConversora
        End Get
        Set(value As Integer)
            _idUsinaConversora = value
        End Set
    End Property

    Private _codUsina As String
    Public Property CodUsina() As String
        Get
            Return _codUsina
        End Get
        Set(value As String)
            _codUsina = value
        End Set
    End Property

    Private _codConversora As String
    Public Property CodUsiConversora() As String
        Get
            Return _codConversora
        End Get
        Set(value As String)
            _codConversora = value
        End Set
    End Property

    Private _percentualPerda As Integer
    Public Property PercentualPerda() As Integer
        Get
            Return _percentualPerda
        End Get
        Set(ByVal value As Integer)
            _percentualPerda = value
        End Set
    End Property

    Private _potInstaladaUsina As Integer
    Public Property PotInstaladaUsina() As Integer
        Get
            Return _potInstaladaUsina
        End Get
        Set(ByVal value As Integer)
            _potInstaladaUsina = value
        End Set
    End Property

    Private _potInstaladaConversora As Integer
    Public Property PotInstaladaConversora() As Integer
        Get
            Return _potInstaladaConversora
        End Get
        Set(ByVal value As Integer)
            _potInstaladaConversora = value
        End Set
    End Property

    Public Property NumeroPrioridade As Integer
        Get
            Return _numeroPrioridade
        End Get
        Set(value As Integer)
            _numeroPrioridade = value
        End Set
    End Property

    Private _numeroPrioridade As Integer

    Public Overrides Function ObterComando() As String
        Select Case State
            Case StateDTO.Added
                Throw New NotImplementedException()
                Return ""
            Case StateDTO.Modified
                Throw New NotImplementedException()
                Return ""
            Case StateDTO.Removed
                Return $"delete from tb_usinaconversora where id_usinaconversora = {Me.IdUsinaConversora}"
        End Select
    End Function
End Class
