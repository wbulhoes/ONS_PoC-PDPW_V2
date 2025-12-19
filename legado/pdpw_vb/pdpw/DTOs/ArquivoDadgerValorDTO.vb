Public Class ArquivoDadgerValorDTO
    Inherits BaseDTO

    Private _idArquivoDadgerValor As Integer
    Private _idArquivoDadger As Integer
    Private _dppId As Double
    Private _codUsina As String
    Private _valorCVU As Decimal
    Private _valorIfxLeve As Nullable(Of Decimal)
    Private _valorIfxMedia As Nullable(Of Decimal)
    Private _valorIfxPesada As Nullable(Of Decimal)
    Private _valorLimiteIfxPMO As Nullable(Of Integer)

    Public Property IdArquivoDadgerValor() As Integer
        Get
            Return _idArquivoDadgerValor
        End Get
        Set(value As Integer)
            _idArquivoDadgerValor = value
        End Set
    End Property

    Public Property IdArquivoDadger() As Integer
        Get
            Return _idArquivoDadger
        End Get
        Set(value As Integer)
            _idArquivoDadger = value
        End Set
    End Property

    Public Property DppId() As Double
        Get
            Return _dppId
        End Get
        Set(value As Double)
            _dppId = value
        End Set
    End Property

    Public Property ValorCVU() As Decimal
        Get
            Return _valorCVU
        End Get
        Set(value As Decimal)
            _valorCVU = value
        End Set
    End Property

    Public Property ValorIfxLeve() As Nullable(Of Decimal)
        Get
            Return _valorIfxLeve
        End Get
        Set(value As Nullable(Of Decimal))
            _valorIfxLeve = value
        End Set
    End Property

    Public Property ValorIfxMedia() As Nullable(Of Decimal)
        Get
            Return _valorIfxMedia
        End Get
        Set(value As Nullable(Of Decimal))
            _valorIfxMedia = value
        End Set
    End Property

    Public Property ValorIfxPesada() As Nullable(Of Decimal)
        Get
            Return _valorIfxPesada
        End Get
        Set(value As Nullable(Of Decimal))
            _valorIfxPesada = value
        End Set
    End Property

    Public Property ValorLimiteIfxPMO As Nullable(Of Integer)
        Get
            Return _valorLimiteIfxPMO
        End Get
        Set(value As Nullable(Of Integer))
            _valorLimiteIfxPMO = value
        End Set
    End Property

    Public Property CodUsina As String
        Get
            Return _codUsina
        End Get
        Set(value As String)
            _codUsina = value
        End Set
    End Property

    Public Overrides Function ObterComando() As String
        Select Case State
            Case StateDTO.Added
                Return "INSERT INTO tb_arquivodadgervalor" &
                      $" (id_arquivodadgervalor, id_arquivodadger, dpp_id, val_cvu, val_inflexileve, val_infleximedia, val_inflexipesada, val_inflexipmo) " &
                      $" VALUES(0, {_idArquivoDadger}, {_dppId}, {_valorCVU}, {_valorIfxLeve}, {_valorIfxMedia}, {_valorIfxPesada}, {_valorLimiteIfxPMO});"
            Case StateDTO.Modified
                Return " UPDATE tb_arquivodadgervalor" &
                      $" SET id_arquivodadger= {_idArquivoDadger}, dpp_id= {_dppId}, val_cvu= {_valorCVU}, val_inflexileve= {_valorIfxLeve}, val_infleximedia= {_valorIfxMedia}, val_inflexipesada= {_valorIfxPesada}, val_inflexipmo= {_valorLimiteIfxPMO} " &
                      $" WHERE id_arquivodadgervalor= {_idArquivoDadgerValor}; "
            Case StateDTO.Removed
                Return $"DELETE FROM tb_arquivodadgervalor WHERE id_arquivodadgervalor= {_idArquivoDadgerValor};"
        End Select
    End Function
End Class
