
<TestClass()>
Public Class OfertaExportacaoServiceTest
    Inherits BaseServiceTest

    <TestMethod>
    Public Sub ReiniciarDecisaoDeAnalise()
        Dim dataPDP As String = Me.FactoryDAO.OfertaExportacaoDAO.ListarTodos().FirstOrDefault().Datpdp

        Dim sucesso As Boolean = Me.FactoryBusiness.OfertaExportacao.ReiniciarDecisaoDeAnalise(dataPDP)

        Assert.IsTrue(sucesso)

    End Sub

End Class
