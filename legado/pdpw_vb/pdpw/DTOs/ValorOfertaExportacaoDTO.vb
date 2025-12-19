Public Class ValorOfertaExportacaoDTO
    Inherits BaseDTO

    Private _IdValoresOfertaExportacao As Integer
    Private _Datpdp As String
    Private _CodUsina As String
    Private _CodUsiConversora As String
    Private _NumPatamar As Integer
    Private _ValSugeridoAgente As Integer
    Private _ValSugeridoONS As Integer
    Private _ValDespaUsinaRef As Integer?
    Private _ValDespaUsiConversoraRef As Integer?
    Private _ValExportaRef As Integer?
    Private _ValPerdaRef As Integer?
    Private _ValCargaUsiConversoraRef As Integer?

    Public Property IdValoresOfertaExportacao As Integer
        Get
            Return _IdValoresOfertaExportacao
        End Get
        Set
            _IdValoresOfertaExportacao = Value
        End Set
    End Property

    Public Property Datpdp As String
        Get
            Return _Datpdp
        End Get
        Set
            _Datpdp = Value
        End Set
    End Property

    Public Property CodUsina As String
        Get
            Return _CodUsina
        End Get
        Set
            _CodUsina = Value
        End Set
    End Property

    Public Property CodUsiConversora As String
        Get
            Return _CodUsiConversora
        End Get
        Set
            _CodUsiConversora = Value
        End Set
    End Property

    Public Property NumPatamar As Integer
        Get
            Return _NumPatamar
        End Get
        Set
            _NumPatamar = Value
        End Set
    End Property

    Public Property ValSugeridoAgente As Integer
        Get
            Return _ValSugeridoAgente
        End Get
        Set
            _ValSugeridoAgente = Value
        End Set
    End Property

    Public Property ValSugeridoONS As Integer
        Get
            Return _ValSugeridoONS
        End Get
        Set
            _ValSugeridoONS = Value
        End Set
    End Property

    Public Property ValDespaUsinaRef As Nullable(Of Integer)
        Get
            Return _ValDespaUsinaRef
        End Get
        Set
            _ValDespaUsinaRef = Value
        End Set
    End Property

    Public Property ValDespaUsiConversoraRef As Nullable(Of Integer)
        Get
            Return _ValDespaUsiConversoraRef
        End Get
        Set
            _ValDespaUsiConversoraRef = Value
        End Set
    End Property

    Public Property ValExportaRef As Nullable(Of Integer)
        Get
            Return _ValExportaRef
        End Get
        Set
            _ValExportaRef = Value
        End Set
    End Property

    Public Property ValPerdaRef As Nullable(Of Integer)
        Get
            Return _ValPerdaRef
        End Get
        Set
            _ValPerdaRef = Value
        End Set
    End Property

    Public Property ValCargaUsiConversoraRef As Integer?
        Get
            Return _ValCargaUsiConversoraRef
        End Get
        Set(value As Integer?)
            _ValCargaUsiConversoraRef = value
        End Set
    End Property

    Public Overrides Function ObterComando() As String
        Select Case State
            Case StateDTO.Added
                Throw New NotImplementedException()
                Return ""
            Case StateDTO.Modified
                Return $" UPDATE tb_valoresofertaexportacao SET 
	                        num_patamar = {Me.TrataNothing(NumPatamar)}, 
	                        val_sugeridoagente = {Me.TrataNothing(ValSugeridoAgente)}, 
	                        val_sugeridoons = {Me.TrataNothing(ValSugeridoONS)}, 
	                        val_refusina = {Me.TrataNothing(ValDespaUsinaRef)}, 
	                        val_refusiconversora = {Me.TrataNothing(ValDespaUsiConversoraRef)}, 
	                        val_refexporta = {Me.TrataNothing(ValExportaRef)}, 
	                        val_refperda = {Me.TrataNothing(ValPerdaRef)},
                            val_refCargaUsiConversora = {Me.TrataNothing(ValCargaUsiConversoraRef)}
                        WHERE id_valoresofertaexportacao = {IdValoresOfertaExportacao}; "
            Case StateDTO.Removed
                Throw New NotImplementedException()
                Return ""
        End Select

        Return ""
    End Function
End Class
