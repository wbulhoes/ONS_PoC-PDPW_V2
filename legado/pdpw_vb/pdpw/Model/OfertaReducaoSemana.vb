Public Class OfertaReducaoSemana

    <CsvAttribute("Ponto de Medição")>
    Public Property PontoMedicao As String
    <CsvAttribute("Consumidor")>
    Public Property Consumidor As String
    <CsvAttribute("Dia Inicio")>
    Public Property DiaInicio As String
    <CsvAttribute("Dia Fim")>
    Public Property DiaFim As String
    <CsvAttribute("Produto")>
    Public Property Produto As String
    <CsvAttribute("Aviso Previo")>
    Public Property AvisoPrevio As String
    <CsvAttribute("MW")>
    Public Property MW As Decimal
    <CsvAttribute("R$/MWh")>
    Public Property MWh As Decimal
    Public Property CodigoDoProduto As String
End Class
