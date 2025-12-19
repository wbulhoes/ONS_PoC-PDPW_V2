Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports pdpw

<TestClass()> Public MustInherit Class BaseServiceTest

    Protected FactoryDAO As FactoryDAO = Nothing
    Protected FactoryBusiness As FactoryBusiness = Nothing
    Protected _dataPDP_SugeridaTeste As String = ""

    <TestInitialize()> Public Overridable Sub Inicializacao()
        If IsNothing(FactoryDAO) Then
            FactoryDAO = New FactoryDAO()
        End If

        If IsNothing(FactoryBusiness) Then
            FactoryBusiness = New FactoryBusiness()
        End If

        _dataPDP_SugeridaTeste = Now.AddDays(-45).ToString("yyyyMMdd")
    End Sub

    <TestCleanup()> Public Overridable Sub Limpeza()
        FactoryDAO = Nothing

        FactoryBusiness.Dispose()
        FactoryBusiness = Nothing
    End Sub

End Class