Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting

<TestClass()>
Public Class UsinaServiceTest
    Inherits BaseServiceTest

    <TestMethod()>
    Public Sub Listar_Usina_ComSucesso()
        Dim lista = FactoryDAO.UsinaDAO.ListarTodos()
        Assert.IsTrue(Not IsNothing(lista))
    End Sub

    <TestMethod()>
    Public Sub Listar_Usina_Por_CodUsina_ComSucesso()
        Dim codUsina As String = "CMCPB1"
        Dim lista = FactoryDAO.UsinaDAO.ListarUsina(codUsina)
        Assert.IsTrue(Not IsNothing(lista))
    End Sub
End Class
