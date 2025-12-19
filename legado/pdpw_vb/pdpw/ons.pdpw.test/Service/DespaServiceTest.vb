Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting

<TestClass()> Public Class DespaServiceTest
    Inherits BaseServiceTest

    <TestMethod()> Public Sub Listar_PorDataPDP_ComSucesso()
        Dim lista = FactoryDAO.DespaDAO.Listar(_dataPDP_SugeridaTeste)
        Assert.IsTrue(lista.Count > 0)
    End Sub

End Class