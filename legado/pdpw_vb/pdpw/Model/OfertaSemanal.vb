Public Class OfertaSemanal

    Public Property OfertaSemanalId As Integer
    Public Property CodUsinaProgramacao As String
    Public Property VolumeProgramacao As Integer?
    Public Property PrecoProgramacao As Decimal?
    Public Property CodUsinaTempoReal As String
    Public Property VolumeTempoReal As Integer?
    Public Property PrecoTempoReal As Decimal?
    Public Property Dependentes As Boolean

    Public Property Id_SemanaPMO As Integer? = Nothing
    Public Property CodEmpresa As String = Nothing

End Class
