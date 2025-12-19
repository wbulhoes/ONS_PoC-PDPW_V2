Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace Ons.interface.business
    Public Class LimiteEnvioBusiness
        Inherits BaseBusiness
        Implements ILimiteEnvioBusiness


        Public Function ObterLimiteEnvio(ByVal codEmpresa As String, dtPdp As String, ByVal tipoEnvio As TipoEnvio) As LimiteEnvioDTO Implements ILimiteEnvioBusiness.ObterLimiteEnvio

            Dim dataPDP As String = Format(CDate(dtPdp), "yyyyMMdd")
            Return Me.FactoryDao.LimiteEnvioDAO.ObterPor_DataPDP_Empresa_TipoEnvio(dataPDP, codEmpresa, tipoEnvio)

        End Function

    End Class

End Namespace
