Imports System.Collections.Generic

Public Interface IInflexibilidadeBusiness
    Inherits IBaseBusiness

    Function EnviaLimiteDADGER(ByVal dataPDP As String, ByVal codEmpresa As String) As List(Of String)
    Function EnviaLimiteDADGERPorUsina(ByVal dataPDP As String, ByVal codUsina As String) As String

End Interface
