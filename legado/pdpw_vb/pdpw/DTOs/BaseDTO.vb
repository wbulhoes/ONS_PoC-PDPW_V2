
Imports pdpw

Public MustInherit Class BaseDTO
    Implements IBaseDTO

    Public Property State As StateDTO = StateDTO.Detached

    Public MustOverride Function ObterComando() As String Implements IBaseDTO.ObterComando

    Public Shared Widening Operator CType(v As BaseDAO(Of BaseDTO)) As BaseDTO
        Throw New NotImplementedException()
    End Operator

    Protected Function TrataNothing(valor As Object) As String
        Dim valorRetorno As String = "NULL"

        If Not IsNothing(valor) Then
            valorRetorno = $"'{valor.ToString()}'"
        End If

        Return valorRetorno
    End Function

    Protected Function TratarDataFormat(data As DateTime) As String
        Dim dataRetorno As String = "NULL"

        If Not IsNothing(data) Then
            dataRetorno = $"'{data.ToString("yyyy-MM-dd HH:mm:ss")}'"
        End If

        Return dataRetorno
    End Function
End Class
