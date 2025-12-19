Public Class SaldoInflexibilidadePMO_DTO
    Inherits BaseDTO

    Private _id_SaldoInflexibilidadePMO As Integer
    Private _datPDP As String
    Private _codUsina As String
    Private _valAcumuladoDESSEM_Semana As Integer
    Private _valProgramado As Integer
    Private _valEnviadodessem As Integer
    Private _valSaldo As Integer

    Public Property IdSaldoInflexibilidadePMO() As Integer
        Get
            Return _id_SaldoInflexibilidadePMO
        End Get
        Set(ByVal value As Integer)
            _id_SaldoInflexibilidadePMO = value
        End Set
    End Property

    Public Property DatPDP() As String
        Get
            Return _datPDP
        End Get
        Set(ByVal value As String)
            _datPDP = value
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

    Public Property ValAcumuladoDESSEM_Semana() As Integer
        Get
            Return _valAcumuladoDESSEM_Semana
        End Get
        Set(ByVal value As Integer)
            _valAcumuladoDESSEM_Semana = value
        End Set
    End Property

    Public Property ValProgramado() As Integer
        Get
            Return _valProgramado
        End Get
        Set(ByVal value As Integer)
            _valProgramado = value
        End Set
    End Property

    Public Property ValEnviadoDESSEM() As Integer
        Get
            Return _valEnviadodessem
        End Get
        Set(ByVal value As Integer)
            _valEnviadodessem = value
        End Set
    End Property

    Public Property ValSaldo() As Integer
        Get
            Return _valSaldo
        End Get
        Set(ByVal value As Integer)
            _valSaldo = value
        End Set
    End Property

    Public Overrides Function ObterComando() As String
        Select Case State
            Case StateDTO.Added
                Return "INSERT INTO tb_saldoinflexibilidadepmo 
                        (id_saldoinflexibilidadepmo, DatPDP, CodUsina, val_acumuladodessemsemana, val_programado, val_enviadodessem, val_saldo) " &
                       $" VALUES(0, '{ _datPDP }', 
                       '{_codUsina }', 
                       { _valAcumuladoDESSEM_Semana }, 
                       { _valProgramado }, 
                       { _valEnviadodessem }, 
                       { _valSaldo }); "

            Case StateDTO.Modified
                Return " UPDATE tb_saldoinflexibilidadepmo" &
                      $" SET datpdp = '{ _datPDP }', 
                      codusina = '{_codUsina}', 
                      val_acumuladodessemsemana = { _valAcumuladoDESSEM_Semana } , 
                      val_programado = { _valProgramado } , 
                      val_enviadodessem = { _valEnviadodessem } , 
                      val_saldo = { _valSaldo }  
                      WHERE id_saldoinflexibilidadepmo = '{ _id_SaldoInflexibilidadePMO }'; "
            Case StateDTO.Removed
                Return $"DELETE FROM tb_saldoinflexibilidadepmo WHERE id_saldoinflexibilidadepmo = { _id_SaldoInflexibilidadePMO }; "

        End Select
    End Function
End Class
