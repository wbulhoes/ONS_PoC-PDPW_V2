Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports pdpw

<TestClass()> Public Class LimiteEnvioServiceTest
    Inherits BaseServiceTest

    <TestMethod()> Public Sub Listar_Todos_ComSucesso()
        Dim lista As List(Of LimiteEnvioDTO) = FactoryDAO.LimiteEnvioDAO.ListarTodos()
        Assert.IsTrue(lista.Count > 0)
    End Sub

End Class
