Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting

<TestClass()>
Public Class ArquivoDadgerValorServiceTest
    Inherits BaseServiceTest

    <TestMethod()>
    Public Sub Listar_PorDataPDP_ComSucesso()
        Dim lista = FactoryDAO.ArquivoDadgerValorDAO.Listar(_dataPDP_SugeridaTeste)
        Assert.IsTrue(Not IsNothing(lista))
    End Sub

End Class
