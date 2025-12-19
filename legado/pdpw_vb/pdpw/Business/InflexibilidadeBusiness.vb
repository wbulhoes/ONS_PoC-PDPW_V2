Imports System.Collections.Generic
Imports System.Linq

Namespace Ons.interface.business


    Public Class InflexibilidadeBusiness
        Inherits BaseBusiness
        Implements IInflexibilidadeBusiness

        Public Function EnviaLimiteDADGER(dataPDP As String, codEmpresa As String) As List(Of String) Implements IInflexibilidadeBusiness.EnviaLimiteDADGER

            Dim listaUsinasComValorNegativo As List(Of String) = New List(Of String)

            Try
                'Obter lista de Usina por empresa
                Dim listaUsina As List(Of String) = Me.FactoryDao.UsinaDAO.
                    ListarUsinaPorEmpresa(codEmpresa).Select(Function(u) u.CodUsina).ToList()

                Dim listaUsinas_ComIFX As List(Of String) =
                    Me.FactoryDao.InflexibilidadeDao.ListarPor_DataPDP_Empresa(dataPDP, codEmpresa).
                    Where(Function(ifx) ifx.ValFlexiTran.HasValue And ifx.ValFlexiTran.Value > 0).
                    Select(Function(ifx) ifx.CodUsina).
                    Distinct().
                    ToList()

                listaUsina = listaUsina.
                    Where(Function(codUsina) listaUsinas_ComIFX.Any(Function(codUsina_comIFX) codUsina = codUsina_comIFX)).
                    ToList()

                For Each usina As String In listaUsina
                    Dim usinaComValorNegativo As String = Me.EnviaLimiteDADGERPorUsina(dataPDP, usina)

                    If Not String.IsNullOrEmpty(usinaComValorNegativo) Then
                        listaUsinasComValorNegativo.Add(usinaComValorNegativo)
                    End If

                Next

            Catch ex As Exception
                Throw TratarErro(ex)
            End Try

            Return listaUsinasComValorNegativo

        End Function

        Public Function EnviaLimiteDADGERPorUsina(dataPDP As String, codUsina As String) As String Implements IInflexibilidadeBusiness.EnviaLimiteDADGERPorUsina

            Dim msg As String = ""

            Try
                Dim saldoIFX As SaldoInflexibilidadePMO_DTO =
                    Me.FactoryDao.SaldoInflexibilidadePMO_DAO.Listar(dataPDP).
                    Where(Function(i) i.CodUsina.Trim().Equals(codUsina.Trim())).
                    FirstOrDefault()

                If Not IsNothing(saldoIFX) Then

                    Dim totalValInflexibilidadeTran As Integer = 0

                    'totalValInflexibilidadeTran =
                    'Me.FactoryDao.InflexibilidadeDao.
                    '    Listar(dataPDP).
                    '    Where(Function(i) i.CodUsina.Trim().Equals(codUsina.Trim())).
                    '    Sum(Function(i) i.ValFlexiTran)

                    Dim limiteIFX_Dia As Integer = saldoIFX.ValSaldo - totalValInflexibilidadeTran

                    If limiteIFX_Dia < 0 Then 'Ngativo
                        msg = $"{codUsina.Trim()} - Saldo IFX: {saldoIFX.ValSaldo}"
                    End If
                End If

                Dim arqDADGERValor As ArquivoDadgerValorDTO =
                    Me.FactoryDao.ArquivoDadgerValorDAO.Listar(dataPDP).
                    Where(Function(i) i.CodUsina.Trim().Equals(codUsina.Trim())).
                    FirstOrDefault()

                If Not IsNothing(arqDADGERValor) Then
                    If arqDADGERValor.ValorLimiteIfxPMO.HasValue AndAlso arqDADGERValor.ValorLimiteIfxPMO = 0 Then
                        msg = $"{codUsina.Trim()} - Limite IFX Semana PMO = 0"
                    End If
                End If

            Catch ex As Exception
                Throw TratarErro(ex)
            End Try

            Return msg
        End Function
    End Class

End Namespace

