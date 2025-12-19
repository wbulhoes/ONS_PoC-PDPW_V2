Public Interface ILimiteEnvioBusiness
    Inherits IBaseBusiness

    Function ObterLimiteEnvio(ByVal codEmpresa As String, dtPdp As String, ByVal tipoEnvio As TipoEnvio) As LimiteEnvioDTO

End Interface
