<System.AttributeUsage(System.AttributeTargets.Property)>
Public Class CsvAttribute
    Inherits System.Attribute
    Public Property Descricao As String
    Sub New(ByVal value As String)
        Descricao = value
    End Sub
End Class
