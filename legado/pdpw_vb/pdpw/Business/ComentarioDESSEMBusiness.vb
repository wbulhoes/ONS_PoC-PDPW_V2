Public Class ComentarioDESSEMBusiness
    Inherits BaseBusiness
    Implements IComentarioDESSEMBusiness

    Public Function ValidaLimiteEnvioComentarioDESSEM(codEmpresa As String, dtPdp As String) As Boolean Implements IComentarioDESSEMBusiness.ValidaLimiteEnvioComentarioDESSEM

        Dim horaDefault As Date = Date.Parse(Date.Now.ToString("dd/MM/yyyy") + " " + "17:00:00")
        Dim fazerUpload As Boolean = True

        Dim dataPDP As String = Format(CDate(dtPdp), "yyyyMMdd")
        Dim diaSeguinte As String = DateTime.Now.Date.AddDays(1).ToString("yyyyMMdd")

        Dim limiteEnvio As LimiteEnvioDTO
        limiteEnvio = Me.FactoryDao.LimiteEnvioDAO.ObterPor_DataPDP_Empresa_TipoEnvio(dataPDP, codEmpresa, TipoEnvio.ComentarioDESSEM)

        If diaSeguinte.Equals(dataPDP) Then
            If IsNothing(limiteEnvio) Then 'Não existe registro cadastrado no banco
                If DateTime.Now > horaDefault Then 'Caso horário corrente ultrapassou horário padrão
                    fazerUpload = False
                End If
            Else
                If DateTime.Now > limiteEnvio.Hora_Limite Then 'Caso horário corrente ultrapassou horário definido no banco de dados
                    fazerUpload = False
                End If
            End If
        Else
            fazerUpload = False
        End If

        Return fazerUpload

    End Function


End Class
