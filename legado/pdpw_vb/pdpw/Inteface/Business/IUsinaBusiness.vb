Imports System.Collections.Generic

Public Interface IUsinaBusiness
    Inherits IBaseBusiness

    Function ListarUsinasPorEmpresas(listaCodEmpre As List(Of String)) As List(Of UsinaDTO)

End Interface
