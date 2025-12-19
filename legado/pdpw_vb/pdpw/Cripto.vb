Public Class Cripto
    Inherits System.ComponentModel.Component

#Region " Component Designer generated code "

    Public Sub New(Container As System.ComponentModel.IContainer)
        MyClass.New()

        'Required for Windows.Forms Class Composition Designer support
        Container.Add(me)
    End Sub

    Public Sub New()
        MyBase.New()

        'This call is required by the Component Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Component overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Component Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Component Designer
    'It can be modified using the Component Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        components = New System.ComponentModel.Container()
    End Sub

#End Region
    Public Function Encripta(ByVal texto As String) As String
        Dim Letra, i, j
        For i = 1 To Len(texto)
            Letra = Asc(Mid(texto, i, 1)) * 15

Verifica:
            If Len(Letra) < 5 Then
                Letra = "0" & Letra
                GoTo Verifica
            End If

            Encripta = Encripta & Letra
        Next

    End Function

    Public Function Decripta(ByVal texto As String) As String

        Dim Letra, i, j

        For i = 1 To Len(texto) Step 5
            Letra = Chr(Mid(texto, i, 5) / 15)
            Decripta = Decripta & Letra
        Next

        Exit Function

    End Function

End Class
