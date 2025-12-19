Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports System.Linq
Imports System.Web.Script.Services
Imports Microsoft.VisualBasic
Imports pdpw.Ons.interface.business

Partial Class frmCnsOfertaExportacao
    Inherits BaseWebUi

    Dim indice_inicial As Integer = 0      ' Índice inicial para paginação.
    Dim Conn As SqlConnection = New SqlConnection
    Dim Cmd As SqlCommand = New SqlCommand
    Dim IdComentario As Integer
    Dim DentroPrazoEnvio As Boolean = False


    Protected tbResponsaveis As DataTable = New DataTable("Resposaveis")
    Protected coluna As DataColumn
    Protected linha As DataRow
    Protected WithEvents dtgResponsaveis As Global.System.Web.UI.WebControls.DataGrid

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object



    Private Sub Page_DataBinding(sender As Object, e As System.EventArgs) Handles Me.DataBinding

    End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        'Put user code to initialize the page here

        'objMenu = CType(Session("onsmenu"), OnsWebControls.OnsMenu)
        Try

            If Not Page.IsPostBack Then

                Session("datEscolhida") = Now.AddDays(1).ToString("yyyyMMdd")

                PreencheComboData(DataPdpDropDown, Session("datEscolhida"))
                PreencheComboEmpresaPOP(AgentesRepresentados, cboEmpresa, Session("strCodEmpre"))

                cboEmpresa.SelectedIndex = 0
            Else

                AtualizaValoresSessao()
                Me.Atualiza_Table_Usinas()

            End If
            lblMsg.Visible = False
        Catch ex As Exception
            lblMsg.Visible = True

            If Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If

            'Session("strMensagem") = "Não foi possível visualizar os dados" + ".  " + ex.Message + "____" + ex.StackTrace
            'If Conn.State = ConnectionState.Open Then
            '    Conn.Close()
            'End If
            'Response.Redirect("frmMensagem.aspx")
        Finally
            If Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If
        End Try
    End Sub

    Private Sub PopularDropDownUsina()
        Dim Conn As SqlConnection = New SqlConnection
        Dim Cmd As SqlCommand = New SqlCommand
        Dim empresa As String

        UsinaDropDown.SelectedIndex = -1
        UsinaDropDown.Items.Clear()

        empresa = Session("strCodEmpre")

        If empresa = Nothing Or empresa = "" Then
            Exit Sub
        End If
        Conn.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
        Cmd.Connection = Conn
        Conn.Open()

        Cmd.CommandText = $"SELECT
                            Distinct
                            Trim(us.codusina) as CodUsina, 
                            Trim(us.codusina) + ' - ' + Trim(us.nomusina)
                                as DescricaoUsina ,
                            us.nomusina
                           FROM usina us
                           inner join tb_usinaconversora uc on Trim(uc.codUsina) = Trim(us.CodUsina)
                           WHERE us.CodEmpre = '{ empresa }'
                           ORDER BY 2"

        Dim objItemUsina As WebControls.ListItem
        objItemUsina = New System.Web.UI.WebControls.ListItem
        objItemUsina.Text = ""
        objItemUsina.Value = Nothing
        UsinaDropDown.Items.Add(objItemUsina)

        Dim rsDataUsina As SqlDataReader = Cmd.ExecuteReader
        Do While rsDataUsina.Read
            objItemUsina = New System.Web.UI.WebControls.ListItem
            objItemUsina.Text = rsDataUsina("DescricaoUsina")
            objItemUsina.Value = rsDataUsina("CodUsina")
            UsinaDropDown.Items.Add(objItemUsina)
        Loop

        rsDataUsina.Close()
        rsDataUsina = Nothing
        ' ObterAnaliseSugestao()
        Conn.Close()
    End Sub


    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        If SessaoAtiva(Page.Session) Then
            Funcoes.RetirarItensSelecionadosComDuplicidade(DataPdpDropDown)
            Funcoes.RetirarItensSelecionadosComDuplicidade(cboEmpresa)

            MyBase.Render(writer)
            'objMenu.RenderControl(writer)
        End If
    End Sub

    Protected Sub PreencherGrid(ByVal sender As Object, ByVal e As EventArgs)
        Atualiza_Table_Usinas()
    End Sub

    Private Sub AplicarCaixaTextoPatamares(ByVal valores As Nullable(Of Integer)(), Optional ByVal indiceColunaUsina As Integer = 0, Optional ByVal codConversora As String = "", Optional ByVal CodUsina As String = "")
        Dim replaceRegex As System.Text.RegularExpressions.Regex = New System.Text.RegularExpressions.Regex("\n")
        Dim objTextArea As HtmlControls.HtmlTextArea
        objTextArea = New HtmlTextArea
        objTextArea.Rows = 48
        objTextArea.ID = "txt_" + CodUsina.Trim() + "_" + codConversora.Trim()
        objTextArea.Attributes.Item("onkeyup") = "RetiraEnter(event,this)"
        '  objTextArea.Attributes.Item("runat") = "server"
        objTextArea.Attributes.Item("style") = "font-family:Arial;font-size:Smaller;height:800px;width:50px;line-height:16px"
        objTextArea.Attributes.Item("oninput") = "Totalizar()"
        objTextArea.Attributes.Item("onkeypress") = "somenteNumeros(event)"


        If valores.Length > 0 Then
            Dim replacedString As String
            For index As Integer = 0 To (valores.Length - 1)
                replacedString += valores(index).ToString() + vbNewLine
            Next
            objTextArea.Value = replacedString ' valores(Index).ToString() + "\n"
        End If
        Dim divValores As HtmlGenericControl

        divValores = New HtmlGenericControl("div")

        divValores.Attributes.Add("runat", "server")
        divValores.Attributes.Add("ms_positioning", "FlowLayout")
        divValores.Style.Item("TOP") = "400px"
        divValores.Style.Item("DISPLAY") = "inline"
        divValores.Style.Item("LEFT") = "180px"
        divValores.Style.Item("WIDTH") = "82px"
        divValores.Style.Item("POSITION") = "absolute"
        divValores.Style.Item("HEIGHT") = "21px"

        divValores.ID = "div_" + CodUsina.Trim() + "_" + codConversora.Trim()

        Dim left As Integer = 150

        If indiceColunaUsina <> 1 Then
            left = left + (50 * (indiceColunaUsina - 1))
        End If

        divValores.Style.Item("LEFT") = $"{left}px"

        divValores.Controls.Add(objTextArea)

        divValores.Visible = True

        frmCnsOfertaExportacao.Controls.Add(divValores)

    End Sub

    Private Sub AlertaMensagem(ByVal mensagemTexto As String)
        Dim mensagem As String
        mensagem = "<script type='text/javascript'> showAlert('" + mensagemTexto.Replace("'", "").Replace("\n", "") + "'); </script>"
        ClientScript.RegisterClientScriptBlock(Me.GetType(), "myscript", mensagem)
    End Sub

    Protected Sub cboEmpresa_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboEmpresa.SelectedIndexChanged

        PopularDropDownUsina()

        AtualizaValoresSessao()

        PrazoEnvio()
        'Atualiza_Table_Usinas()
    End Sub

    Private Sub AtualizaValoresSessao()
        Session("strCodUsina") = ""
        Session("datEscolhida") = ""
        Session("strCodEmpre") = ""

        If UsinaDropDown.SelectedIndex > 0 Then
            Session("strCodUsina") = UsinaDropDown.SelectedValue
        End If

        If DataPdpDropDown.SelectedIndex > 0 Then
            Session("datEscolhida") = DataPdpDropDown.SelectedValue
        End If

        If cboEmpresa.SelectedIndex > 0 Then
            Session("strCodEmpre") = cboEmpresa.SelectedValue
        End If
    End Sub

    Private Sub Atualiza_Table_Usinas()

        Dim intI As Integer
        Dim objCel As TableCell
        Dim objRow As TableRow
        Dim objLabel As Label
        Dim objTamanho As WebControls.Unit

        Try

            Dim controlesRemocao As List(Of Control) = New List(Of Control)()
            For Each textAreaValores As Control In frmCnsOfertaExportacao.Controls
                If Not IsNothing(textAreaValores.ID) AndAlso
                    (textAreaValores.ID.StartsWith("div_") Or textAreaValores.ID.StartsWith("txt_")) Then
                    controlesRemocao.Add(textAreaValores)
                End If
            Next
            For Each textAreaValores As Control In controlesRemocao
                frmCnsOfertaExportacao.Controls.Remove(textAreaValores)
            Next

            If Not String.IsNullOrEmpty(Session("datEscolhida")) Then

                AtualizaValoresSessao()

                tblGeracao.Visible = False
                tblGeracao.Rows.Clear()
                tblGeracao.Controls.Clear()

                Dim usConv As New UsinaConversoraDAO
                Dim OfertaExp As New OfertaExportacaoDAO

                Dim Conversora As List(Of UsiConversDTO) = New List(Of UsiConversDTO)()
                Dim listaCodEmpre As List(Of String) = New List(Of String)

                If Not IsNothing(Session("datEscolhida")) And
                    Not String.IsNullOrEmpty(Session("datEscolhida")) And
                    Session("datEscolhida") > 0 Then

                    If Not String.IsNullOrEmpty(Session("strCodEmpre")) And Not IsNothing(Session("strCodEmpre")) Then
                        listaCodEmpre.Add(Session("strCodEmpre")) 'Empresa Selecionada na combo
                    Else
                        For Each codEmpre As ListItem In cboEmpresa.Items
                            If codEmpre.Value.Length > 0 Then
                                listaCodEmpre.Add(codEmpre.Value)
                            End If
                        Next
                    End If

                    Me.AtualizarMensagemAviso(DataPdpDropDown.SelectedItem.Text, listaCodEmpre)

                    Conversora = usConv.ListarConversorasPorUsina(Session("strCodUsina"), listaCodEmpre)

                    Dim ofertas_UsiConv As List(Of UsiConversDTO) =
                        OfertaExp.ListarOfertarAgente(If(Not String.IsNullOrEmpty(Session("datEscolhida")?.ToString()), Session("datEscolhida").ToString(), Now.AddDays(1).ToString("yyyyMMdd")), Session("strCodEmpre"), Session("strCodUsina"))

                    Dim valoresOfertas_UsiConv As List(Of ConversoraValorOfertaDTO) = New List(Of ConversoraValorOfertaDTO)()
                    For Each usiConv As UsiConversDTO In ofertas_UsiConv
                        valoresOfertas_UsiConv.AddRange(usiConv.ValoresOfertaUsiConversora)
                    Next
                    valoresOfertas_UsiConv = valoresOfertas_UsiConv.Distinct().ToList()

                    Dim listaConv As List(Of ConversoraValorOfertaDTO) = New List(Of ConversoraValorOfertaDTO)()
                    For Each usiConv As UsiConversDTO In Conversora
                        For Each conv As ConversoraValorOfertaDTO In usiConv.ValoresOfertaUsiConversora
                            listaConv.Add(conv)
                        Next
                    Next

                    For Each usiConv As UsiConversDTO In Conversora

                        Dim uc As UsiConversDTO =
                            ofertas_UsiConv.
                            FirstOrDefault(Function(o) o.CodUsina.Trim() = usiConv.CodUsina.Trim())

                        If Not IsNothing(uc) Then
                            usiConv.OrdemAgente = uc.OrdemAgente
                            usiConv.OrdemOns = uc.OrdemOns
                        End If

                        For Each conv As ConversoraValorOfertaDTO In listaConv

                            If conv.CodUsina = usiConv.CodUsina Then
                                Dim oferta As ConversoraValorOfertaDTO =
                                    valoresOfertas_UsiConv.
                                    FirstOrDefault(Function(o)
                                                       Return o.CodUsina.Trim() = conv.CodUsina.Trim() And
                                                        o.CodConversora.Trim() = conv.CodConversora.Trim()
                                                   End Function)

                                If Not IsNothing(oferta) Then

                                    usiConv.ValoresOfertaUsiConversora.
                                            RemoveAll(Function(c)
                                                          Return c.CodUsina.Trim() = conv.CodUsina.Trim() And
                                                          c.CodConversora.Trim() = conv.CodConversora.Trim()
                                                      End Function)

                                    usiConv.ValoresOfertaUsiConversora.AddRange(
                                            valoresOfertas_UsiConv.
                                            Where(Function(c)
                                                      Return c.CodUsina.Trim() = conv.CodUsina.Trim() And
                                                      c.CodConversora.Trim() = conv.CodConversora.Trim()
                                                  End Function).
                                            ToList()
                                            )
                                End If
                            End If
                        Next
                    Next

                End If

                Dim ordemConv As Integer = 1

                If Not String.IsNullOrEmpty(Session("strCodEmpre")) And Not IsNothing(Session("strCodEmpre")) Then
                    ordemConv =
                        Me.FactoryBusiness.
                        OfertaExportacao.
                        ObterProximoNumeroOrdem(Session("datEscolhida"), Session("strCodEmpre"))
                Else
                    ordemConv =
                        Me.FactoryBusiness.
                        OfertaExportacao.
                        ObterProximoNumeroOrdem(Session("datEscolhida"), "")
                End If

                Conversora.
                    OrderBy(Function(c) c.CodUsina).
                    ToList().
                    ForEach(Sub(c)
                                If (c.OrdemAgente = 0) Then
                                    c.OrdemAgente = ordemConv
                                    c.OrdemOns = c.OrdemAgente

                                    ordemConv = ordemConv + 1
                                End If
                            End Sub)

                Conversora = Conversora.OrderBy(Function(x) x.OrdemAgente).ToList()

                If (Conversora.Count > 0) Then
                    PrazoEnvio()
                End If

                Dim ListaConversoras As List(Of ConversoraValorOfertaDTO) = New List(Of ConversoraValorOfertaDTO)

                objTamanho = New WebControls.Unit

                objRow = New TableRow()
                objRow.BackColor = System.Drawing.Color.Beige
                objRow.Width = objTamanho.Pixel(100)
                objCel = New TableCell
                objCel.Wrap = False
                objCel.ColumnSpan = 2
                objCel.Text = "Ordem"
                objCel.Font.Bold = True
                objCel.HorizontalAlign = HorizontalAlign.Center
                objCel.Width = objTamanho.Pixel(100)
                objRow.Controls.Add(objCel)

                For ord As Integer = 1 To Conversora.Count
                    Dim objText As HtmlInputText
                    objText = New HtmlInputText
                    objText.ID = "txt_ordem_" + Conversora.Item(ord - 1).CodUsina.Trim() + "_" + Conversora.Item(ord - 1).codConversora.Trim()

                    objText.Value = IIf((Conversora.Item(ord - 1).OrdemAgente = Nothing), ord, Conversora.Item(ord - 1).OrdemAgente)
                    ord.ToString()
                    objText.Style.Item("width") = "40px"
                    objText.Style.Item("margin-left") = "0px"
                    objText.Style.Item("text-align") = "center"
                    objText.Attributes.Item("onkeypress") = "somenteNumeros(event)"

                    If Not DentroPrazoEnvio Then
                        objText.Attributes.Add("disabled", "")
                    End If

                    objCel = New TableCell
                    objCel.Wrap = False

                    objCel.ColumnSpan = Conversora.Item(ord - 1).ValoresOfertaUsiConversora.GroupBy(Function(n) n.CodConversora).Count
                    objCel.Controls.Add(objText)
                    objCel.Font.Bold = True
                    objCel.HorizontalAlign = HorizontalAlign.Center
                    objCel.Width = objTamanho.Pixel(62)
                    objRow.Controls.Add(objCel)

                Next


                tblGeracao.Width = objTamanho.Pixel(132)
                tblGeracao.Controls.Add(objRow)

                objRow = New TableRow()
                objRow.BackColor = System.Drawing.Color.Beige
                objRow.Width = objTamanho.Pixel(100)
                objCel = New TableCell
                objCel.Wrap = False
                objCel.Font.Bold = True
                objCel.ColumnSpan = 2
                objCel.Text = "    "
                objCel.HorizontalAlign = HorizontalAlign.Center
                objCel.Width = objTamanho.Pixel(100)
                objRow.Controls.Add(objCel)

                For Each uConv As UsiConversDTO In Conversora

                    objCel = New TableCell
                    objCel.Wrap = False
                    objCel.ColumnSpan = uConv.ValoresOfertaUsiConversora.GroupBy(Function(n) n.CodConversora).Count
                    objCel.Text = uConv.CodUsina

                    objCel.Font.Bold = True
                    objCel.HorizontalAlign = HorizontalAlign.Center
                    objCel.Width = objTamanho.Pixel(62)
                    objRow.Controls.Add(objCel)
                    ListaConversoras.AddRange(uConv.ValoresOfertaUsiConversora)

                Next

                tblGeracao.Width = objTamanho.Pixel(132)
                tblGeracao.Controls.Add(objRow)

                objRow = New TableRow()
                objRow.BackColor = System.Drawing.Color.Beige
                objRow.Width = objTamanho.Pixel(100)
                objCel = New TableCell
                objCel.Wrap = False
                objCel.Text = "Intervalo"
                objCel.Font.Bold = True
                objCel.HorizontalAlign = HorizontalAlign.Center
                objCel.Width = objTamanho.Pixel(100)
                objRow.Controls.Add(objCel)

                objCel = New TableCell
                objCel.Text = "Total"
                objCel.Wrap = False
                objCel.Font.Bold = True
                objCel.Width = objTamanho.Pixel(62)
                objCel.HorizontalAlign = HorizontalAlign.Center
                objRow.Width = objTamanho.Pixel(132)
                objRow.Controls.Add(objCel)

                Dim ListConv = (From I In ListaConversoras Select I.CodUsina, I.CodConversora Distinct).ToList

                For Each Conv As Object In ListConv
                    objCel = New TableCell
                    objCel.Text = Conv.CodConversora


                    objCel.Wrap = False
                    objCel.Font.Bold = True

                    'objCel.Width = objTamanho.Pixel(62)
                    objCel.Style.Item("width") = "62px"

                    objCel.HorizontalAlign = HorizontalAlign.Center
                    'objRow.Width = objTamanho.Pixel(132)
                    objRow.Controls.Add(objCel)

                Next

                tblGeracao.Width = objTamanho.Pixel(132)
                tblGeracao.Controls.Add(objRow)

                'Cria linhas em branco para Intervalo e Total
                objRow = New TableRow
                objRow.Width = objTamanho.Pixel(132)

                objCel = New TableCell
                objCel.BackColor = System.Drawing.Color.Beige
                objCel.Font.Bold = True
                'objCel.Width = objTamanho.Pixel(100)
                objCel.HorizontalAlign = HorizontalAlign.Center
                objCel.Text = "    "
                objCel.Attributes.Add("style", "min-width: 80px; width: 80px;")
                objRow.Controls.Add(objCel)

                objCel = New TableCell
                objCel.Text = "    "
                objCel.BackColor = System.Drawing.Color.Beige
                objCel.HorizontalAlign = HorizontalAlign.Center
                objCel.Font.Bold = True
                'objCel.Width = objTamanho.Pixel(100)
                objCel.Attributes.Add("style", "min-width: 50px; max-width: 50px;")
                objRow.Controls.Add(objCel)

                tblGeracao.Controls.Add(objRow)

                Dim intHora As Integer = 0
                For intI = 1 To 48
                    objRow = New TableRow
                    objRow.Width = objTamanho.Pixel(132)

                    objCel = New TableCell
                    objCel.BackColor = System.Drawing.Color.Beige
                    objCel.Font.Bold = True
                    objCel.Width = objTamanho.Pixel(100)
                    objCel.HorizontalAlign = HorizontalAlign.Center
                    objCel.Text = IntParaHora(intI)
                    objRow.Controls.Add(objCel)

                    objCel = New TableCell
                    objLabel = New Label
                    objLabel.Text = ListaConversoras.Where(Function(b) b.Num_Patamar = intI).Sum(Function(n) n.ValorSugeridoAgente)
                    objLabel.ID = "Num_Patamar_" + intI.ToString()
                    objCel.Text = 0
                    objCel.BackColor = System.Drawing.Color.Beige
                    objCel.Font.Bold = True
                    objCel.Width = objTamanho.Pixel(62)
                    objCel.Controls.Add(objLabel)

                    For Index As Integer = 1 To ListConv.Count

                        Dim valor = (From I In ListaConversoras Where I.CodConversora = ListConv.Item(Index - 1).CodConversora And I.CodUsina = ListConv.Item(Index - 1).CodUsina And I.Num_Patamar = intI Select I.ValorSugeridoAgente).Sum()

                        objRow.Controls.Add(objCel)
                        objCel = New TableCell
                        objCel.Text = valor
                        objCel.BackColor = System.Drawing.Color.Beige
                        objCel.Font.Bold = True
                        'objCel.Width = objTamanho.Pixel(62)
                        objCel.Attributes.Add("style", "min-width: 50px; width: 50px;")

                        If intI = 1 And DentroPrazoEnvio Then
                            Dim valores = (From I In ListaConversoras Where I.CodConversora = ListConv.Item(Index - 1).CodConversora And I.CodUsina = ListConv.Item(Index - 1).CodUsina Order By I.Num_Patamar Select I.ValorSugeridoAgente).ToArray

                            AplicarCaixaTextoPatamares(valores, Index, ListConv.Item(Index - 1).CodConversora, ListConv.Item(Index - 1).CodUsina)
                        End If
                    Next

                    objRow.Controls.Add(objCel)
                    tblGeracao.Controls.Add(objRow)
                Next

                tblGeracao.Visible = True
            End If

        Catch ex As Exception
            AlertaMensagem("Erro: " & ex.Message)
        Finally

            If Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If

        End Try
    End Sub

    Private Sub AtualizarMensagemAviso(dataPDP As String, listaCodEmpre As List(Of String))
        lblMensagemAviso.Text = "Hora limite para envio de oferta 10:00."

        If listaCodEmpre.Count > 0 And Not IsNothing(dataPDP) And Not String.IsNullOrEmpty(dataPDP) And dataPDP <> "0" Then

            Dim dataHora_limite As DateTime =
                Me.FactoryBusiness.OfertaExportacao.
                ObterLimiteCadastrado(dataPDP, listaCodEmpre, TipoEnvio.EnviarOfertaExportacao, "10:00:00")

            lblMensagemAviso.Text = $"Limite para envio de oferta: {dataHora_limite.ToString("dd/MM/yyyy HH:mm:ss")}."

        End If
    End Sub

    Private Function RetornaCor(flg_sugestaoAgente As String, flg_aprovado As String, existeRegistrosPatamares As Boolean) As Color

        Dim Cor As Color = Color.Beige

        If flg_sugestaoAgente = "2" Then
            Cor = Color.Green
        ElseIf flg_sugestaoAgente = "3" And String.IsNullOrEmpty(flg_aprovado) And existeRegistrosPatamares Then
            Cor = Color.Yellow
        ElseIf flg_sugestaoAgente = "3" And String.IsNullOrEmpty(flg_aprovado) And Not existeRegistrosPatamares Then
            Cor = Color.Coral
        ElseIf flg_sugestaoAgente = "3" And flg_aprovado = "S" Then
            Cor = Color.Green
        ElseIf flg_sugestaoAgente = "3" And flg_aprovado = "N" Then
            Cor = Color.Red
        End If

        Return Cor

    End Function



    <System.Web.Services.WebMethod()>
    Public Shared Function SalvarOferta(ByVal OfertaOrdem As Object, ByVal OfertaOrdemDetalhe As Object, ByVal DatPDP As String, ByVal loginUsuario As String) As String

        Dim OfertaExportacao As List(Of UsiConversDTO) = New List(Of UsiConversDTO)
        Dim ValoresOfertaExportacao As List(Of ConversoraValorOfertaDTO) = New List(Of ConversoraValorOfertaDTO)

        Dim Retorno As String = ""
        Try

            Dim _dataHoraOferta As String = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")

            For Index As Integer = 0 To (OfertaOrdem.Length - 1)
                Dim ofExport As UsiConversDTO = New UsiConversDTO

                ofExport.CodUsina = OfertaOrdem(Index)(0).ToString()
                ofExport.codConversora = ""
                ofExport.DatPdp = DatPDP
                ofExport.dinofertaexportacao = _dataHoraOferta
                ofExport.OrdemAgente = CType(OfertaOrdem(Index)(1).ToString(), Integer)
                ofExport.OrdemOns = ofExport.OrdemAgente
                OfertaExportacao.Add(ofExport)
            Next

            For IndexDetalhe As Integer = 0 To (OfertaOrdemDetalhe.Length - 1)
                For I As Integer = 2 To 49
                    Dim Valores As ConversoraValorOfertaDTO = New ConversoraValorOfertaDTO
                    Valores.CodUsina = OfertaOrdemDetalhe(IndexDetalhe)(0).ToString()
                    Valores.CodConversora = OfertaOrdemDetalhe(IndexDetalhe)(1).ToString()
                    Valores.DatPdp = DatPDP
                    Valores.Num_Patamar = I - 1

                    Valores.ValorSugeridoAgente = Nothing
                    Valores.ValorSugeridoOns = Nothing

                    If OfertaOrdemDetalhe(IndexDetalhe).Length - 1 >= I AndAlso Not IsNothing(OfertaOrdemDetalhe(IndexDetalhe)(I)) Then

                        If Not String.IsNullOrEmpty(OfertaOrdemDetalhe(IndexDetalhe)(I).ToString()) Then
                            Valores.ValorSugeridoAgente = OfertaOrdemDetalhe(IndexDetalhe)(I).ToString()
                            Valores.ValorSugeridoOns = Valores.ValorSugeridoAgente
                        End If

                    End If

                    ValoresOfertaExportacao.Add(Valores)

                Next
            Next

            Dim ofertaExportacaoBusiness As OfertaExportacaoBusiness = New OfertaExportacaoBusiness()
            Retorno = ofertaExportacaoBusiness.GravarOfertas(OfertaExportacao, ValoresOfertaExportacao, loginUsuario, DatPDP)

            If Retorno.Length = 0 Then
                Retorno = "Dados gravados com sucesso."
            End If

        Catch ex As Exception
            Retorno = $"Problema ao gravar ofertas:{ex.Message.Replace("'", "").Replace("\n", "")}"
        End Try

        Return Retorno

    End Function

    Protected Sub DataPdpDropDown_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DataPdpDropDown.SelectedIndexChanged
        AtualizaValoresSessao()

        PrazoEnvio()
        'Atualiza_Table_Usinas()

    End Sub



    Protected Sub UsinaDropDown_SelectedIndexChanged(sender As Object, e As EventArgs) Handles UsinaDropDown.SelectedIndexChanged

        AtualizaValoresSessao()

        PrazoEnvio()
        'Atualiza_Table_Usinas()

    End Sub

    Protected Sub PrazoEnvio()

        DentroPrazoEnvio = False

        If Not String.IsNullOrEmpty(DataPdpDropDown.SelectedItem.Text.Trim()) Then

            Dim listaCodEmpresas As List(Of String) = New List(Of String)()
            For Each empre As ListItem In cboEmpresa.Items
                If empre.Value.Length > 0 Then
                    listaCodEmpresas.Add(empre.Value)
                End If
            Next

            DentroPrazoEnvio =
                Me.FactoryBusiness.
                    OfertaExportacao.
                    ValidarPrazoEnvioOfertaAgente(DataPdpDropDown.SelectedItem.Text.Trim(), listaCodEmpresas)

            If Not DentroPrazoEnvio Then
                AlertaMensagem("Prazo esgotado para envio de Oferta")
                btnImgSalvar.Visible = False
            Else
                btnImgSalvar.Visible = True
            End If

        End If

    End Sub

    Protected Sub btnImgSalvar_Click(sender As Object, e As ImageClickEventArgs) Handles btnImgSalvar.Click
        Atualiza_Table_Usinas()
    End Sub
End Class

