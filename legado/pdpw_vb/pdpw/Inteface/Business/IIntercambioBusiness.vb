Public Interface IIntercambioBusiness
    Inherits IBaseBusiness

    Function AtualizaIntercambio(dataPDP As String) As String

    Function AtualizaIntercambioTodasOfertas(dataPDP As String) As String

End Interface
