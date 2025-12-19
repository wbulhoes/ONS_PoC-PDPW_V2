Imports pdpw

Public MustInherit Class BaseBusiness
    Implements IBaseBusiness

    Private _factoryBusiness As FactoryBusiness

    Protected ReadOnly Property FactoryBusiness As FactoryBusiness
        Get
            If IsNothing(_factoryBusiness) Then
                _factoryBusiness = New FactoryBusiness()
            End If

            Return _factoryBusiness
        End Get
    End Property

    Private _factoryDAO As FactoryDAO

    Protected ReadOnly Property FactoryDao As FactoryDAO
        Get
            If IsNothing(_factoryDAO) Then
                _factoryDAO = New FactoryDAO
            End If

            Return _factoryDAO
        End Get
    End Property
    Public Overridable Sub Dispose() Implements IDisposable.Dispose

        If Not IsNothing(_factoryDAO) Then
            _factoryDAO.Dispose()
        End If

        _factoryDAO = Nothing
    End Sub

    Protected Function TratarErro(ByVal mensagem As String) As BusinessException
        Return TratarErro(mensagem, New Exception(mensagem))
    End Function

    Protected Function TratarErro(ByVal exception As Exception) As BusinessException
        Return TratarErro(exception.Message, exception)
    End Function

    Protected Function TratarErro(ByVal mensagem As String, ByVal exception As Exception) As BusinessException

        Dim erro As BusinessException = New BusinessException($" {mensagem.Replace("'", "").Replace("\n", "")}", exception)

        'TODO: Ponto de interceptação para gerar LOG - Business
        Diagnostics.Debug.WriteLine($"Erro na camada Business {mensagem} {exception.Message}")

        Return erro
    End Function


End Class
