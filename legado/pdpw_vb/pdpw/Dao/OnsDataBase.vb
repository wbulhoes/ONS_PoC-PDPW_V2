Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports System.Linq
Imports System.Text
Imports OnsClasses.OnsData

Public Class OnsDataBase
    Implements IDisposable

    Private conexao As SqlConnection
    Private transacao As SqlTransaction

    Public Sub New()
        AbrirConexao()
    End Sub

    Public Overridable Sub Dispose() Implements IDisposable.Dispose
        Me.FecharConexao()
    End Sub

    Public Sub AbrirConexao()
        If IsNothing(conexao) Then
            conexao = New SqlConnection()
        End If

        If Not conexao.State = ConnectionState.Open Then
            conexao.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
            conexao.Open()
        End If
    End Sub

    Public Sub IniciarTransacao()
        AbrirConexao()

        If IsNothing(transacao) Then
            transacao = conexao.BeginTransaction(IsolationLevel.ReadCommitted)
        End If
    End Sub

    Public Sub AplicarAlteracoes()
        If Not IsNothing(transacao) Then
            Try
                transacao.Commit()
            Catch ex As Exception
                Throw ex
            Finally
                transacao = Nothing
            End Try
        End If
    End Sub

    Public Sub CancelarAlteracoes()
        If Not IsNothing(transacao) Then
            Try
                transacao.Rollback()
            Catch ex As Exception
                Throw ex
            Finally
                transacao = Nothing
            End Try
        End If
    End Sub
    Public Function ExecutarSQL(comandoSQL As String, Optional ByVal comTransacao As Boolean = False, Optional qtdCorteExecucao As Integer = 1000) As Boolean

        Dim sucesso As Boolean = False

        Try
            If Not IsNothing(comandoSQL) And Not String.IsNullOrEmpty(comandoSQL) Then
                Me.AbrirConexao()

                If comTransacao Then
                    Me.IniciarTransacao()
                End If

                Dim comando As SqlCommand = Me.ObterCommando()
                ' WI 151539: Inserindo tempo de timeout para conexão com BD
                comando.CommandTimeout = 1200
                Dim listaComandos As List(Of String) = comandoSQL.Split(";").ToList()

                Dim sqlExec As New StringBuilder
                Dim qtdCmd As Integer = 0

                For Each cmd As String In listaComandos

                    If cmd.Trim().Length > 0 Then
                        sqlExec.AppendLine($"{cmd.Trim()};")
                        qtdCmd = qtdCmd + 1
                    End If

                    If qtdCmd Mod qtdCorteExecucao = 0 Then
                        comando.CommandText = ""
                        comando.CommandText = sqlExec.ToString()
                        comando.ExecuteNonQuery()

                        sqlExec.Clear()
                        qtdCmd = 0
                    End If
                Next

                If sqlExec.Length > 0 Then
                    comando.CommandText = ""
                    comando.CommandText = sqlExec.ToString()
                    comando.ExecuteNonQuery()
                End If

                Me.AplicarAlteracoes()

                sucesso = True
            End If

        Catch ex As Exception

            Me.CancelarAlteracoes()
            Throw ex
        Finally
            Me.FecharConexao()
        End Try

        Return sucesso

    End Function

    Public Function ConsultarSQL(comandoSQL As String) As SqlDataReader
        Dim resultado As SqlDataReader = Nothing
        Try
            If Not IsNothing(comandoSQL) And Not String.IsNullOrEmpty(comandoSQL) Then

                resultado = ObterCommando(comandoSQL).ExecuteReader()
            End If

        Catch ex As Exception
            resultado = Nothing
            Throw ex
        End Try

        Return resultado

    End Function

    Public Function ObterCommando(Optional ByVal comandoSQL As String = Nothing) As SqlCommand
        Me.AbrirConexao()

        Dim cmd As SqlCommand = conexao.CreateCommand()

        If Not IsNothing(transacao) Then 'Em caso de transação aberta
            cmd.Transaction = transacao
        End If

        cmd.CommandType = CommandType.Text

        If Not IsNothing(comandoSQL) Then
            cmd.CommandText = comandoSQL
        End If

        ' WI 151539: Inserindo tempo de timeout para conexão com BD
        cmd.CommandTimeout = 1200

        Return cmd
    End Function

    Public Sub FecharConexao()

        If Not IsNothing(conexao) Then
            Me.CancelarAlteracoes()

            If conexao.State = ConnectionState.Open Then
                conexao.Close()
            End If

            conexao = Nothing
        End If
    End Sub


End Class
