
Public Class OfertaExportacaoDTO
    Inherits BaseDTO

    Public Property IdOfertaExportacao As Integer
    Public Property Datpdp As String
    Public Property CodUsina As String
    Public Property CodUsiConversora As String
    Public Property NumOrdemAgente As Integer
    Public Property NumOrdemONS As Integer
    Public Property LgnAgenteOferta As String
    Public Property DinOfertaExportacao As DateTime?
    Public Property LgnOnsAnalise As String
    Public Property FlgAprovadoONS As String
    Public Property DinAnaliseONS As DateTime?
    Public Property LgnAgenteAnalise As String
    Public Property FlgAprovadoAgente As String
    Public Property DinAnaliseAgente As DateTime?
    Public Property FlgExportadoBalanco As String
    Public Property LgnOnsExportadoBalanco As String
    Public Property DinOnsExportadoBalanco As DateTime?



    Public Overrides Function ObterComando() As String
        Select Case State
            Case StateDTO.Added
                Throw New NotImplementedException()
                Return ""

                'Dim Din_OnsExportadoBalanco
                '
                'If (IsNothing(DinOnsExportadoBalanco)) Then
                '    Din_OnsExportadoBalanco = "NULL"
                'Else
                '
                'End If
            Case StateDTO.Modified
                Return $" Update tb_OfertaExportacao Set
                              Num_OrdemAgente = {Me.TrataNothing(NumOrdemAgente)}
                            , Num_OrdemONS = {Me.TrataNothing(NumOrdemONS)}
                            , Lgn_AgenteOferta = {Me.TrataNothing(LgnAgenteOferta)}
                            , Din_Oferta_Exportacao = {Me.TratarDataFormat(DinOfertaExportacao)}
                            , Lgn_ONSAnalise = {Me.TrataNothing(LgnOnsAnalise)}
                            , flg_Aprovado_ONS = {Me.TrataNothing(FlgAprovadoONS)}
                            , Din_Analise_ONS = {Me.TratarDataFormat(DinAnaliseONS)}
                            , Lgn_AgenteAnalise = {Me.TrataNothing(LgnAgenteAnalise) }
                            , Flg_Aprovado_Agente = {Me.TrataNothing(FlgAprovadoAgente)}
                            , Din_Analise_Agente = {Me.TratarDataFormat(DinAnaliseAgente)}
                            , Flg_ExportaBalanco = {Me.TrataNothing(FlgExportadoBalanco)}
                            , Lgn_ONSExportaBalanco = {Me.TrataNothing(LgnOnsExportadoBalanco)}
                            , Din_ExportaBalanco = {If(IsNothing(DinOnsExportadoBalanco), Me.TrataNothing(DinOnsExportadoBalanco), Me.TratarDataFormat(DinOnsExportadoBalanco))}
                            Where Id_OfertaExportacao = {IdOfertaExportacao}; "
            Case StateDTO.Removed
                Throw New NotImplementedException()
                Return ""
        End Select

        Return ""
    End Function
End Class
