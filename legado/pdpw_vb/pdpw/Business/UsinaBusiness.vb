Imports System.Collections.Generic
Imports System.Linq

Namespace Ons.interface.business
    Public Class UsinaBusiness
        Inherits BaseBusiness
        Implements IUsinaBusiness

        Public Function ListarUsinasPorEmpresas(listaCodEmpre As List(Of String)) As List(Of UsinaDTO) Implements IUsinaBusiness.ListarUsinasPorEmpresas

            Return Me.FactoryDao.UsinaDAO.ListarUsinasPorEmpresas(listaCodEmpre)
        End Function

    End Class

End Namespace

