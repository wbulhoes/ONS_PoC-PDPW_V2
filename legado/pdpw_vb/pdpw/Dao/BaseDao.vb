
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports pdpw

Public MustInherit Class BaseDAO(Of DTO As {BaseDTO})
    Inherits OnsDataBase
    Implements IBaseDAO(Of DTO)
    Private logger As log4net.ILog = log4net.LogManager.GetLogger(Me.GetType())
    Private _listaDTO_Persistencia As List(Of DTO) = New List(Of DTO)
    Private _listaCache As Dictionary(Of String, List(Of DTO)) = New Dictionary(Of String, List(Of DTO))()
    Private Shared _cacheTraking As Boolean = True

    Public Sub CacheTrakingDisabled()
        _cacheTraking = False
    End Sub

    Public Sub CacheTrakingEnabled()
        _cacheTraking = True
    End Sub

    ''' <summary>
    ''' Salve (incluindo ou atualizando) um resultado de busca de lista de DTO
    ''' </summary>
    ''' <param name="chave">Chave de Identificação no CHACE</param>
    ''' <param name="listaDTO">Lista de DTO a ser salva em CACHE</param>
    ''' <returns>Lista de DTO salva em CACHE</returns>
    Protected Function CacheSave(chave As String, listaDTO As List(Of DTO)) As List(Of DTO)

        If _cacheTraking Then
            _listaCache.Remove(chave)
            _listaCache.Add(chave, listaDTO)
        End If

        Return listaDTO
    End Function

    ''' <summary>
    ''' Obtém um lista de DTO salvo em CACHE
    ''' </summary>
    ''' <param name="chave">Chave de Identificação no CHACE</param>
    ''' <returns>Lista de DTO em CACHE</returns>
    Protected Function CacheSelect(chave As String) As List(Of DTO)
        Dim lista As List(Of DTO) = Nothing

        If _cacheTraking Then
            If _listaCache.ContainsKey(chave) Then
                lista = _listaCache(chave)
            End If
        End If

        Return lista

    End Function

    Protected Function TratarErro(ByVal mensagem As String) As DAOException
        Return TratarErro(mensagem, New Exception(mensagem))
    End Function

    Protected Function TratarErro(ByVal exception As Exception) As DAOException
        Return TratarErro(exception.Message, exception)
    End Function

    Protected Function TratarErro(ByVal mensagem As String, ByVal exception As Exception) As DAOException
        logger.Error(exception.Message, exception)
        Dim erro As DAOException = New DAOException(mensagem, exception)

        'TODO: Ponto de interceptação para gerar LOG - DAO
        Diagnostics.Debug.WriteLine($"Erro na camada DAO '{mensagem} {exception.Message}'")

        Return erro
    End Function

    Public Function Salvar() As Boolean
        Return Me.Salvar(_listaDTO_Persistencia)
    End Function

    Public Function Salvar(ByVal entidades As List(Of DTO)) As Boolean
        Dim sucesso As Boolean = False
        Try
            If entidades.Count > 0 Then

                Dim sql As StringBuilder = New StringBuilder()

                For Each entidade As DTO In entidades
                    sql.Append(entidade.ObterComando())
                Next

                sucesso = Me.ExecutarSQL(sql.ToString(), False)
            End If

        Catch ex As Exception
            Throw TratarErro(ex)
        End Try

        Return sucesso

    End Function

    Public Function Inserir(ByVal entidade As BaseDTO) As BaseDTO
        entidade.State = StateDTO.Added
        _listaDTO_Persistencia.Add(entidade)

        Return entidade
    End Function

    Public Function Atualizar(ByVal entidade As BaseDTO) As BaseDTO
        entidade.State = StateDTO.Modified
        _listaDTO_Persistencia.Add(entidade)

        Return entidade
    End Function

    Public Function Excluir(ByVal entidade As BaseDTO) As BaseDTO
        entidade.State = StateDTO.Removed
        _listaDTO_Persistencia.Add(entidade)

        Return entidade
    End Function

    ''' <summary>
    ''' Lista todos os objetos do banco sem filtro.
    ''' Importante: Em caso de tabelas muito grandes deverá utilizar ou implementar métodos com critérios específicos
    ''' </summary>
    ''' <returns></returns>
    Public Overloads Function ListarTodos() As List(Of DTO) Implements IBaseDAO(Of DTO).ListarTodos
        Try

            Return Me.ListarTodos("")

        Catch ex As Exception
            Throw Me.TratarErro(ex)
        End Try
    End Function


    Public MustOverride Overloads Function Listar(ByVal dataPDP As String) As List(Of DTO)

    Protected MustOverride Overloads Function ListarTodos(ByVal criterioWhere As String) As List(Of DTO)

End Class
