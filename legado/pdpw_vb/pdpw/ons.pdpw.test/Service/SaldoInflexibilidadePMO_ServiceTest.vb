Imports System.Data.SqlClient
Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports OnsClasses.OnsData
Imports pdpw

<TestClass()> Public Class SaldoInflexibilidadePMO_ServiceTest
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

    <TestMethod()> Public Sub Listar_PorDataPDP_ComSucesso()
        Dim lista = FactoryDAO.SaldoInflexibilidadePMO_DAO.Listar(_dataPDP_SugeridaTeste)
        Assert.IsTrue(Not IsNothing(lista))
    End Sub

    <TestMethod>
    Public Sub CalcularInflexibilidadeSemanaPMO_ComSucesso()

        Dim dataPDP As String = _dataPDP_SugeridaTeste

        Dim sucesso = FactoryBusiness.SaldoInflexibilidadeSemanaPMO.CalcularSaldoInflexibilidadeSemanaPMO(dataPDP, "NU")
        Assert.IsTrue(sucesso)
    End Sub

    <TestMethod>
    Public Sub CalcularSaldoInflexibilidadeUsina_ValoresValidos_CalculadoComSucesso()

        Dim dataPDP As String = _dataPDP_SugeridaTeste
        Dim listaSaldoIFX As List(Of SaldoInflexibilidadePMO_DTO) = New List(Of SaldoInflexibilidadePMO_DTO)()

        With listaSaldoIFX
            Dim saldo As New SaldoInflexibilidadePMO_DTO()
            With saldo
                .DatPDP = dataPDP
                .CodUsina = ""
            End With


        End With



    End Sub

End Class
