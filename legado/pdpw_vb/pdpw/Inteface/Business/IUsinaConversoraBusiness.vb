Public Interface IUsinaConversoraBusiness
    Inherits IBaseBusiness

    Sub Excluir(ByVal id_usinaConversora As String)
    Function ExisteUsinaConversora(ByVal codUsina As String, ByVal codUsinaConversora As String) As Boolean
    Function ExisteOfertaFutura_PendenteAnalise_UsinaConversora(ByVal codUsina As String, ByVal codUsinaConversora As String) As Boolean

End Interface
