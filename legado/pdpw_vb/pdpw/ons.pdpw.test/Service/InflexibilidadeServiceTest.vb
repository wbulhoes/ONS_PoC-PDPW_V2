Imports System.Data.SqlClient
Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports OnsClasses.OnsData

<TestClass()> Public Class InflexibilidadeServiceTest
    Inherits BaseServiceTest

    <TestInitialize()>
    Public Sub Inicializar()
        Dim rs As SqlDataReader = Me.FactoryDAO.InflexibilidadeDao.
            ConsultarSQL("Select 
            TOP 1 DatPDP 
            From Inflexibilidade 
            Where ValflexiPRE > 0 and ValflexiPRE > 0 
            Order By DatPDP Desc")

        rs.Read()

        _dataPDP_SugeridaTeste = rs("DatPDP")

    End Sub

    <TestMethod()>
    Public Sub Listar_PorDataPDP_ComSucesso()
        Dim dataPDP As String = _dataPDP_SugeridaTeste
        Dim lista = FactoryDAO.InflexibilidadeDao.Listar(dataPDP)
        Assert.IsTrue(Not IsNothing(lista))
    End Sub

    <TestMethod>
    Public Sub EnviaLimiteDADGERPorUsina_UsinasComSaldoNegativo()
        Dim dataPDP As String = _dataPDP_SugeridaTeste
        Dim lista As List(Of String) = FactoryBusiness.InflexibilidadeBusiness.EnviaLimiteDADGER(dataPDP, "UN")
        Assert.IsTrue(Not IsNothing(lista))
    End Sub

End Class
