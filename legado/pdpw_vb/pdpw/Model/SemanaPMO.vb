Imports System.Collections.Generic

Public Class SemanaPMO
    Public Sub New()
    End Sub

    Public Sub New(ano As Integer, mes As Integer, semana As String, dataInicio As String, dataFim As String, idSemanapmo As Integer, idAnomes As Integer)
        Me.Ano = ano
        Me.Mes = mes
        Me.Semana = semana
        Me.DataInicio = dataInicio
        Me.DataFim = dataFim
        Me.IdSemanapmo = idSemanapmo
        Me.IdAnomes = idAnomes
    End Sub

    Public Property Ano As Integer
    Public Property Mes As Integer
    Public Property Semana As String
    Public Property DataInicio As String
    Public Property DataFim As String
    Public Property IdSemanapmo As Integer
    Public Property IdAnomes As Integer
    Public Property Datas_SemanaPmo As List(Of DateTime)

    Public ReadOnly Property Datas_Inicio_Fim() As List(Of DateTime)
        Get
            Dim dias As List(Of DateTime) = New List(Of Date)()

            If Not String.IsNullOrEmpty(Me.DataInicio) And Not String.IsNullOrEmpty(Me.DataFim) Then
                Dim primeiroDia As DateTime = Get_DataPDP_DateTime(Me.DataInicio.Replace("-", ""))
                dias.Add(primeiroDia)

                For i As Integer = 1 To 6
                    dias.Add(primeiroDia.AddDays(i))
                Next
            End If

            Return dias
        End Get
    End Property


End Class
