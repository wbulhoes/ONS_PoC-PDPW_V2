Public Class OfertaDiaria
    Public Id As Integer
    Public IdOfertaSemanal As Integer
    Public DataPDP As String
    Public CodUsinaProgramacao As String
    Public VolumeProgramacao As Integer?
    Public PrecoProgramacao As Decimal?
    Public CodUsinaTempoReal As String
    Public VolumeTempoReal As Integer?
    Public PrecoTempoReal As Decimal?
    Public Dependentes As Boolean


    Public Sub New()
    End Sub
End Class
