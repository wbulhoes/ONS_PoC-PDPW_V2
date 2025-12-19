Imports System.Collections.Generic

Public Interface IBaseDAO(Of DTO)
    Inherits IDisposable

    Function ListarTodos() As List(Of DTO)

End Interface
