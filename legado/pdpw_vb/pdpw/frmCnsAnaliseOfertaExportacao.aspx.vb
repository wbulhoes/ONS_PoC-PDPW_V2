Imports System.Collections.Generic
Imports System.Linq
Imports System.Runtime.Serialization
Imports System.Web.Script.Services
Imports Microsoft.VisualBasic
Imports pdpw.Ons.interface.business
Imports ons.common.providers
Imports System.IO


Partial Class frmCnsAnaliseOfertaExportacao
    Inherits BaseWebUi
    Private logger As log4net.ILog = log4net.LogManager.GetLogger(Me.GetType())
    Dim DentroPrazoEnvio As Boolean = False
    Dim AnaliseONS As Boolean
    Private Shared ValoresOfertasPercPercaCache As List(Of ConversoraValorOfertaDTO)
    Private Shared UltimaAlteracaoCacheValorsOfertasPerc As DateTime = DateTime.MinValue
    Dim ConversorasMarcadas() As String
    Dim BotaoSelZerados As Boolean

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
        If Page.Request.QueryString("AnaliseONS") = "S" Then
            AnaliseONS = True
        Else
            AnaliseONS = False
        End If

        AtualizaImagemBotoes(AnaliseONS)

        lblMensagemAviso.Visible = Not AnaliseONS
        btnReiniciarDecisaoAnalise.Visible = AnaliseONS

        '
        ' WI 189663 - Suprimir botões de Manter/Aprovar ofertas e mostrar/ocultar novo botão de selecionar zerados
        '
        If AnaliseONS Then
            btnReprovarSelcionados.Visible = True
            btnAprovarSelecionados.Visible = True
            BtnSelecionarZerados.Visible = True

        Else
            btnReprovarSelcionados.Visible = False
            btnAprovarSelecionados.Visible = False
            BtnSelecionarZerados.Visible = False
            PrazoEnvio()
        End If

        Try

            If Not Page.IsPostBack Then
                If Session("datEscolhida") Is Nothing Then
                    Session("datEscolhida") = Now.AddDays(1).ToString("yyyyMMdd")
                End If

                If POPHelper.LoginUsuario.Equals(String.Empty) Then
                    Session("usuarID") = String.Empty
                Else
                    If POPHelper.LoginUsuario.Contains("\") Then
                        Session("usuarID") = POPHelper.LoginUsuario.Split("\").GetValue(1)
                    Else
                        Session("usuarID") = POPHelper.LoginUsuario
                    End If
                End If

                PreencheComboData(DataPdpDropDown, Session("datEscolhida"))
                PreencheComboEmpresaPOP(AgentesRepresentados, cboEmpresa, Session("strCodEmpre"))

                cboEmpresa.SelectedIndex = 0
            End If
            lblMsg.Visible = False
        Catch ex As Exception
            lblMsg.Visible = True
            'Session("strMensagem") = "Não foi possível visualizar os dados" + ".  " + ex.Message + "____" + ex.StackTrace
            'Response.Redirect("frmMensagem.aspx")
        End Try

    End Sub

    Private Sub AtualizaImagemBotoes(analiseONS As Boolean)

        If Not analiseONS Then
            btnAprovarSelecionados.ImageUrl = "images/bt_manter_ofertas.gif"
            btnReprovarSelcionados.ImageUrl = "images/bt_retirar_ofertas.gif"
            '
            ' WI 189663 - Suprimir botões de Manter e aprovar ofertas
            '
            btnAprovarSelecionados.Visible = False
            btnReprovarSelcionados.Visible = False
        End If
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

    Private Sub AplicarCaixaTextoPatamares(ByVal valores As Object(), Optional ByVal indiceColunaUsina As Integer = 0, Optional ByVal codConversora As String = "", Optional ByVal CodUsina As String = "", Optional analiseONS As Boolean = False, Optional ocultarRivera As Boolean = False, Optional aplicarDisplayNoneTextArea As Boolean = False)
        Dim replaceRegex As System.Text.RegularExpressions.Regex = New System.Text.RegularExpressions.Regex("\n")
        Dim objTextArea As HtmlControls.HtmlTextArea
        objTextArea = New HtmlTextArea
        objTextArea.Rows = 48
        objTextArea.ID = "txt_" + CodUsina.Trim() + "_" + codConversora.Trim()
        objTextArea.Attributes.Item("onkeyup") = "RetiraEnter(event,this)"
        '  objTextArea.Attributes.Item("runat") = "server"                                                              ' era 16

        If aplicarDisplayNoneTextArea Then
            objTextArea.Attributes.Item("style") = "display:none;font-family:Arial;font-size:Smaller;height:858.5px;width:50px;line-height:17.4px"
        Else
            objTextArea.Attributes.Item("style") = "font-family:Arial;font-size:Smaller;height:858.5px;width:50px;line-height:17.4px"
        End If

        objTextArea.Attributes.Item("oninput") = "Totalizar()"
        objTextArea.Attributes.Item("onkeypress") = "somenteNumeros(event)"

        If valores.Length > 0 Then
            Dim replacedString As String
            For index As Integer = 0 To (valores.Length - 1)
                replacedString += IIf(Not analiseONS, valores(index).ValorSugeridoAgente.ToString(), valores(index).ValorSugeridoOns.ToString()) + vbNewLine
            Next
            objTextArea.Value = replacedString ' valores(Index).ToString() + "\n"
        End If
        Dim divValores As HtmlGenericControl

        divValores = New HtmlGenericControl("div")

        divValores.Attributes.Add("runat", "server")
        divValores.Attributes.Add("ms_positioning", "FlowLayout")

        Dim _top As Integer = 415

        If lblMensagemAviso.Visible Then
            divValores.Style.Item("TOP") = $"{_top}px"
        Else
            divValores.Style.Item("TOP") = $"{_top - 15}px"
        End If

        divValores.Style.Item("DISPLAY") = "inline"
        divValores.Style.Item("LEFT") = "180px"
        divValores.Style.Item("WIDTH") = "82px"
        divValores.Style.Item("POSITION") = "absolute"
        divValores.Style.Item("HEIGHT") = "35px"
        divValores.Style.Item("TOP") = "450px"   ' Altura da caixa de texto de valores estava 445

        divValores.ID = "div_" + CodUsina.Trim() + "_" + codConversora.Trim()

        'Dim left As Integer = 267

        Dim left As Integer = 460

        If ocultarRivera Then
            left = 410
        End If

        If indiceColunaUsina <> 1 Then
            left = left + (110 * (indiceColunaUsina - 1))
        End If

        divValores.Style.Item("LEFT") = $"{left}px"
        divValores.Controls.Add(objTextArea)
        divValores.Visible = True

        frmCnsOfertaExportacao.Controls.Add(divValores)
    End Sub

    Private Sub AlertaMensagem(ByVal mensagemTexto As String)
        Dim mensagem As String
        mensagem = "<script type='text/javascript'> showAlert('" + mensagemTexto.Replace("\n", "") + "'); </script>"
        ClientScript.RegisterClientScriptBlock(Me.GetType(), "myscript", mensagem)
    End Sub

    Protected Sub cboEmpresa_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboEmpresa.SelectedIndexChanged

        Session("strCodEmpre") = ""
        Session("datEscolhida") = DataPdpDropDown.SelectedValue
        Session("strCodEmpre") = cboEmpresa.SelectedValue
        PrazoEnvio()
        Atualiza_Table_Usinas()

    End Sub

    Private Sub AtualizaInformacoes_Exportacao_Sincronia()

        Dim _mensagem As String = "Status de análise de ofertas: Selecione uma data..."
        Dim _cor As Color = Color.Green

        Try
            If VerificarECorrigirDataPDPSelecionada() Then

                If Session("datEscolhida").ToString() = "0" Then
                    _mensagem = "Status de análise de ofertas: Selecione uma data..."
                Else

                    'Obtém dados para mostrar se ainda há ou não Ofertas pendente de análise.
                    Dim exiteOfertaPendenteAnaliseONS As Boolean =
                        Me.FactoryBusiness.OfertaExportacao.ValidarExiste_OfertasNaoAnalisadasONSOriginal(Session("datEscolhida"))

                    If exiteOfertaPendenteAnaliseONS Then
                        _mensagem = "Ainda existem ofertas com pendência de análise pelo ONS."
                        _cor = Color.Red
                    Else
                        _mensagem = "Todas as ofertas foram analisadas pelo ONS nessa data."
                        _cor = Color.Blue
                    End If

                    If AnaliseONS Then
                        'Obtém último usuário que realizou exportação
                        Dim ultimaOfertaExportada As OfertaExportacaoDTO = Me.FactoryBusiness.OfertaExportacao.ObterUltimaExportacaoBalancao(Session("datEscolhida"))

                        _mensagem += $" (Última exportação para balanço: "

                        If (Not IsNothing(ultimaOfertaExportada)) Then
                            If (Not IsNothing(ultimaOfertaExportada.LgnOnsExportadoBalanco) And (Not IsNothing(ultimaOfertaExportada.DinOnsExportadoBalanco))) Then

                                Dim login As String = ultimaOfertaExportada.LgnOnsExportadoBalanco.Trim()
                                Dim datahora As String = ultimaOfertaExportada.DinOnsExportadoBalanco.Value.ToString("dd/MM/yyyy HH:mm:ss")

                                _mensagem += $" {datahora}"
                            Else
                                _mensagem += " Ainda não houve exportação para balanço."
                            End If
                        Else
                            _mensagem += " Ainda não houve exportação para balanço."
                        End If
                        _mensagem += ")"
                    End If
                End If
            End If

        Catch ex As Exception
            _cor = Color.Yellow
            _mensagem =
                $"Problema na exibição de status das análises: {ex.Message.PadRight(70).Substring(0, 69).Trim()}..."
            logger.Error(ex.Message, ex)
        End Try

        AvisoSincroniaExportacao.Text = _mensagem
        AvisoSincroniaExportacao.ForeColor = _cor

    End Sub

    Private Sub AtualizarMensagemAviso(dataPDP As String, listaCodEmpre As List(Of String))
        lblMensagemAviso.Text = "Hora limite para análise oferta 18:15."

        If listaCodEmpre.Count > 0 And Not IsNothing(dataPDP) And Not String.IsNullOrEmpty(dataPDP) And dataPDP <> "0" Then

            Dim dataHora_limite As DateTime =
                Me.FactoryBusiness.OfertaExportacao.
                ObterLimiteCadastrado(dataPDP, listaCodEmpre, TipoEnvio.AnalisarOfertaExportacao, "18:15:00")

            lblMensagemAviso.Text = $"Limite para análise de oferta: {dataHora_limite.ToString("dd/MM/yyyy HH:mm:ss")}."

        End If
    End Sub

    ' Função para criar células padrão
    Sub AddTableCell(ByRef objRow As TableRow, ByVal text As String, ByVal width As Integer, ByVal objTamanho As WebControls.Unit, Optional ByVal isBold As Boolean = True, Optional ByVal labelId As String = "")
        Dim objCel As New TableCell()
        objCel.BackColor = System.Drawing.Color.Beige
        objCel.Font.Bold = isBold
        objCel.Width = objTamanho.Pixel(width)
        objCel.HorizontalAlign = HorizontalAlign.Center
        If Not String.IsNullOrEmpty(labelId) Then
            Dim objLabel As New Label()
            objLabel.Text = text
            objLabel.ID = labelId
            objCel.Controls.Add(objLabel)
        Else
            objCel.Text = text
        End If
        objRow.Controls.Add(objCel)
    End Sub

    ' Função para criar Label padrão
    Function CreateLabel(ByVal text As String, ByVal id As String) As Label
        Dim objLabel As New Label()
        objLabel.Text = text
        objLabel.ID = id
        Return objLabel
    End Function

    Private Sub Atualiza_Table_Usinas()

        Dim intI As Integer
        Dim objCel As TableCell
        Dim objRow As TableRow
        Dim objLabel As Label
        Dim objTamanho As WebControls.Unit
        Dim ObjCheckBox As HtmlInputCheckBox
        Dim VisualizarButtons As Boolean = False
        Dim codEmpre_ultima As String = ""
        Dim ultima_cor_aplicada As Color
        Dim identificacaoPage As String = "_ctl0_ContentPlaceHolder1_"

        Try

            Dim controlesRemocao As List(Of Control) = New List(Of Control)()
            For Each textAreaValores As Control In frmCnsOfertaExportacao.Controls
                If Not IsNothing(textAreaValores.ID) AndAlso
                    (textAreaValores.ID.StartsWith(identificacaoPage + "div_") Or textAreaValores.ID.StartsWith(identificacaoPage + "txt_")) Then
                    controlesRemocao.Add(textAreaValores)
                End If
            Next
            For Each textAreaValores As Control In controlesRemocao
                frmCnsOfertaExportacao.Controls.Remove(textAreaValores)
            Next

            btn_ExportaDadosParaExportacao.Visible = False
            btnSalvarExportacao.Visible = False
            btn_Iniciar_Analise_ONS.Visible = False
            btnReiniciaValoresReferencia.Visible = False
            AtualizaInformacoes_Exportacao_Sincronia()

            tblGeracao.Visible = False
            tblGeracao.Rows.Clear()
            tblGeracao.Controls.Clear()

            If VerificarECorrigirDataPDPSelecionada() Then

                Dim msgErro As String = Session("strMensagem")

                If Not IsNothing(msgErro) AndAlso msgErro.ToLowerInvariant().Contains("falha") Then
                    Dim util As New Util
                    util.MensagemFormulario(msgErro)
                End If

                Dim usConv As New UsinaConversoraDAO
                Dim Conversora As List(Of UsiConversDTO)

                Dim OfertaExp As New OfertaExportacaoDAO

                If Not AnaliseONS Then
                    btn_excluir_ofertas_selecionadas.Visible = False
                    If Me.FactoryBusiness.OfertaExportacao.Permitir_ExclusaoOfertas(Session("datEscolhida")) Then
                        btn_excluir_ofertas_selecionadas.Visible = True
                    End If
                End If

                Conversora = OfertaExp.ListarOfertarAgente(If(Not String.IsNullOrEmpty(Session("datEscolhida")?.ToString()), Session("datEscolhida").ToString(), Now.AddDays(1).ToString("yyyyMMdd")), Session("strCodEmpre"))

                Dim listaCodEmpre As List(Of String) = New List(Of String)
                If Not String.IsNullOrEmpty(Session("strCodEmpre")) And Not IsNothing(Session("strCodEmpre")) Then
                    listaCodEmpre.Add(Session("strCodEmpre")) 'Empresa Selecionada na combo
                Else
                    For Each codEmpre As ListItem In cboEmpresa.Items
                        If codEmpre.Value.Length > 0 Then
                            listaCodEmpre.Add(codEmpre.Value)
                        End If
                    Next
                End If

                '
                ' WI 189663 - Retirada da mensagem de limite para análise de oferta
                '
                'Me.AtualizarMensagemAviso(DataPdpDropDown.SelectedItem.Text, listaCodEmpre)

                Dim usinas_filtro As List(Of UsinaDTO) = Me.FactoryBusiness.UsinaBusiness.ListarUsinasPorEmpresas(listaCodEmpre)

                Conversora = Conversora.Where(Function(c) usinas_filtro.Any(Function(u) u.CodUsina.Trim() = c.CodUsina.Trim())).ToList()

                If Conversora.Count = 0 Then
                    Throw New Exception("Não existe Ofertas para os filtros aplicados.")
                End If

                Dim ListaConversoras As List(Of ConversoraValorOfertaDTO) = New List(Of ConversoraValorOfertaDTO)
                Dim ListaSomaConversoras As List(Of ConversoraValorOfertaDTO) = OfertaExp.ListarOfertasAgrupadasPorConversoras(Session("datEscolhida"))
                objTamanho = New WebControls.Unit

                Dim colunasTabelaLiquido As Integer = 3
                If ListaConversoras.Any(Function(i) i.CodConversora = "ESRIV") Then
                    colunasTabelaLiquido = 4
                End If

                objRow = New TableRow()
                objRow.BackColor = System.Drawing.Color.Beige
                objRow.Width = objTamanho.Pixel(100)
                objCel = New TableCell
                objCel.Wrap = False
                objCel.ColumnSpan = 3
                objCel.Text = "Prioridade Agente"
                objCel.Font.Bold = True
                objCel.HorizontalAlign = HorizontalAlign.Center
                objCel.Width = objTamanho.Pixel(100)
                objRow.Controls.Add(objCel)

                objRow.BackColor = System.Drawing.Color.Beige
                objRow.Width = objTamanho.Pixel(100)
                objCel = New TableCell
                objCel.Wrap = False
                objCel.ColumnSpan = colunasTabelaLiquido
                objCel.Text = " "
                objCel.Font.Bold = True
                objCel.HorizontalAlign = HorizontalAlign.Center
                objCel.Width = objTamanho.Pixel(100)
                objRow.Controls.Add(objCel)

                Dim codUsina_grid As String = ""

                For ord As Integer = 1 To Conversora.Count

                    If codUsina_grid <> Conversora.Item(ord - 1).CodUsina Then
                        codUsina_grid = Conversora.Item(ord - 1).CodUsina

                        objCel = New TableCell

                        Dim objText As HtmlInputText
                        objText = New HtmlInputText
                        objText.ID = "txt_ordem_" + Conversora.Item(ord - 1).CodUsina.Trim()
                        objText.Value = IIf((Conversora.Item(ord - 1).OrdemAgente = Nothing), ord, Conversora.Item(ord - 1).OrdemAgente)
                        ord.ToString()
                        objText.Attributes.Item("onkeypress") = "somenteNumeros(event)"
                        objText.Style.Item("width") = "65px"
                        objText.Style.Item("text-align") = "center"
                        objText.Disabled = IIf(AnaliseONS = True, False, True)

                        objCel.Wrap = False
                        objCel.Text = IIf((Conversora.Item(ord - 1).OrdemAgente = Nothing), ord, Conversora.Item(ord - 1).OrdemAgente)
                        objCel.ColumnSpan = Conversora.Where(Function(c) c.CodUsina = Conversora.Item(ord - 1).CodUsina).Count * 2 'Conversora.Item(ord - 1).ValoresOfertaUsiConversora.GroupBy(Function(n) n.CodConversora).Count * 2
                        objCel.Font.Bold = True
                        objCel.HorizontalAlign = HorizontalAlign.Center
                        objCel.Width = objTamanho.Pixel(100)

                        objCel.BackColor =
                            RetornaCor_Empresa(
                            Conversora.Item(ord - 1).CodUsina.Trim().Substring(0, 2),
                            codEmpre_ultima,
                            ultima_cor_aplicada)

                        objCel.Controls.Add(objText)
                        objRow.Controls.Add(objCel)

                    End If

                Next

                tblGeracao.Width = objTamanho.Pixel(132)
                tblGeracao.Controls.Add(objRow)

                objRow = New TableRow()
                objRow.BackColor = System.Drawing.Color.Beige
                objRow.Width = objTamanho.Pixel(100)
                objCel = New TableCell
                objCel.Wrap = False
                objCel.Font.Bold = True
                objCel.ColumnSpan = 3
                objCel.Text = "Ordem de chegada <span style='font-size:20px;'>&#8680;</span>"
                objCel.HorizontalAlign = HorizontalAlign.Center
                objCel.Width = objTamanho.Pixel(100)
                objRow.Controls.Add(objCel)

                objRow.BackColor = System.Drawing.Color.Beige
                objRow.Width = objTamanho.Pixel(100)
                objCel = New TableCell
                objCel.Wrap = False
                objCel.Font.Bold = True
                objCel.ColumnSpan = colunasTabelaLiquido
                objCel.Text = " "
                objCel.HorizontalAlign = HorizontalAlign.Center
                objCel.Width = objTamanho.Pixel(100)
                objRow.Controls.Add(objCel)

                codUsina_grid = ""
                codEmpre_ultima = ""
                ultima_cor_aplicada = Nothing

                For Each uConv As UsiConversDTO In Conversora

                    If codUsina_grid <> uConv.CodUsina Then

                        codUsina_grid = uConv.CodUsina

                        Dim VisualizaCheck As Boolean = True

                        If AnaliseONS Then
                            VisualizaCheck = (From I In uConv.ValoresOfertaUsiConversora Where I.CodUsina = uConv.CodUsina And I.Flgaprovadoons = "" Select I.CodUsina, I.CodConversora Distinct).Count > 0
                        Else
                            VisualizaCheck =
                                (From I In uConv.ValoresOfertaUsiConversora
                                 Where I.CodUsina = uConv.CodUsina And I.Flgaprovadoons = "S" And I.Flgaprovadoagente = ""
                                 Select I.CodUsina, I.CodConversora Distinct).Count > 0

                            If Not VisualizaCheck Then
                                VisualizaCheck = (AnaliseONS = False And btn_excluir_ofertas_selecionadas.Visible)
                            End If

                        End If

                        If Not VisualizarButtons Then
                            VisualizarButtons = VisualizaCheck
                        End If

                        objCel = New TableCell
                        objLabel = New Label
                        objCel.Wrap = False
                        objCel.ColumnSpan = Conversora.Where(Function(c) c.CodUsina = uConv.CodUsina).Count * 2 'uConv.ValoresOfertaUsiConversora.GroupBy(Function(n) n.CodConversora).Count * 2
                        objCel.Text = uConv.CodUsina
                        objLabel.Text = uConv.CodUsina
                        objCel.Font.Bold = True
                        objCel.HorizontalAlign = HorizontalAlign.Center
                        objCel.Width = objTamanho.Pixel(62)

                        objCel.BackColor =
                            RetornaCor_Empresa(
                            uConv.CodUsina.Substring(0, 2),
                            codEmpre_ultima,
                            ultima_cor_aplicada)

                        If VisualizaCheck Then
                            ObjCheckBox = New HtmlInputCheckBox
                            ObjCheckBox.ID = "Chk_" + uConv.CodUsina.Trim
                            ObjCheckBox.Attributes.Item("onclick") = "MarcarTodosUsina('" + uConv.CodUsina.Trim + "')"
                            objCel.Controls.Add(ObjCheckBox)

                        End If

                        objCel.Controls.Add(objLabel)
                        objRow.Controls.Add(objCel)
                    End If

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
                objCel.Text = "Total "
                objCel.Wrap = False
                objCel.ColumnSpan = 2
                objCel.Font.Bold = True
                objCel.Width = objTamanho.Pixel(100)
                objCel.HorizontalAlign = HorizontalAlign.Center
                objRow.Width = objTamanho.Pixel(132)
                objRow.Controls.Add(objCel)


                objCel = New TableCell
                objCel.Text = "Total Líquido"
                objCel.Wrap = False
                objCel.ColumnSpan = colunasTabelaLiquido
                objCel.Font.Bold = True
                objCel.Width = objTamanho.Pixel(100)
                objCel.HorizontalAlign = HorizontalAlign.Center
                objRow.Width = objTamanho.Pixel(132)
                objRow.Controls.Add(objCel)

                Dim ListConv = (From I In ListaConversoras Select I.CodUsina, I.CodConversora, I.Flgaprovadoagente, I.Flgaprovadoons, I.NumeroPrioridade Distinct).ToList

                For Each Conv As Object In ListConv
                    objCel = New TableCell
                    objCel.Text = Conv.CodConversora
                    objLabel = New Label

                    If ((AnaliseONS = True And Conv.Flgaprovadoons = "") Or (AnaliseONS = False And Conv.Flgaprovadoons = "S" And Conv.Flgaprovadoagente = "") Or (AnaliseONS = False And btn_excluir_ofertas_selecionadas.Visible)) Then
                        ObjCheckBox = New HtmlInputCheckBox
                        ObjCheckBox.ID = "Chk_" + Conv.CodUsina.Trim + "_" + Conv.CodConversora.Trim
                        ObjCheckBox.Attributes.Item("class") = "cheUsinaConv"
                        objCel.Controls.Add(ObjCheckBox)
                        '
                        ' WI 189663 - Caso o botão de selecionar os valores zerados tenha sido executado e efetivamente
                        ' existam todos os valores = 0 para a conversora, vai marcar o seu check box correspondente.
                        '
                        If BotaoSelZerados Then
                            Dim i As Integer
                            For i = 0 To UBound(ConversorasMarcadas) - 1
                                If ObjCheckBox.ID = ConversorasMarcadas(i) Then
                                    ObjCheckBox.Checked = True
                                    Exit For
                                End If
                            Next i
                        End If
                    End If

                    objLabel.Text = $"  ({Conv.NumeroPrioridade}°) " + Conv.CodConversora.Trim()
                    objCel.Wrap = False
                    objCel.ColumnSpan = 2

                    objCel.BackColor =
                        RetornaCor(
                            Conv.Flgaprovadoagente,
                            Conv.Flgaprovadoons,
                            ListaConversoras.Where(Function(v) v.CodUsina = Conv.CodUsina And
                                                               v.CodConversora = Conv.CodConversora).
                                              ToList())

                    objCel.Controls.Add(objLabel)
                    objCel.Font.Bold = True
                    objCel.Width = objTamanho.Pixel(62)
                    objCel.HorizontalAlign = HorizontalAlign.Center
                    objRow.Width = objTamanho.Pixel(132)

                    objRow.Controls.Add(objCel)

                Next

                Dim listaConversorasAgrupadas As Dictionary(Of Integer, List(Of ConversoraValorOfertaDTO)) = ListaConversoras.GroupBy(Function(b) b.Num_Patamar).ToDictionary(Function(g) g.Key, Function(g) g.ToList())
                Dim listaSomaConversorasAgrupadas As Dictionary(Of Integer, List(Of ConversoraValorOfertaDTO)) = ListaSomaConversoras.GroupBy(Function(i) i.Num_Patamar).ToDictionary(Function(g) g.Key, Function(g) g.ToList())


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
                objCel.Text = "  "
                objCel.Attributes.Add("style", "min-width: 80px; width: 80px;")
                objRow.Controls.Add(objCel)

                objCel = New TableCell
                objCel.Text = "Agente"
                objCel.BackColor = System.Drawing.Color.Beige
                objCel.HorizontalAlign = HorizontalAlign.Center
                objCel.Font.Bold = True
                'objCel.Width = objTamanho.Pixel(132)
                objCel.Attributes.Add("style", "min-width: 50px; width: 50px;")
                objRow.Controls.Add(objCel)

                objCel = New TableCell
                objCel.Text = "ONS"
                objCel.BackColor = System.Drawing.Color.Beige
                objCel.HorizontalAlign = HorizontalAlign.Center
                objCel.Font.Bold = True
                'objCel.Width = objTamanho.Pixel(132)
                objCel.Attributes.Add("style", "min-width: 50px; width: 50px;")
                objRow.Controls.Add(objCel)

                objCel = New TableCell
                objCel.Text = "GarabiI"
                objCel.BackColor = System.Drawing.Color.Beige
                objCel.HorizontalAlign = HorizontalAlign.Center
                objCel.Font.Bold = True
                'objCel.Width = objTamanho.Pixel(132)
                objCel.Attributes.Add("style", "min-width: 50px; width: 50px;")
                objRow.Controls.Add(objCel)

                objCel = New TableCell
                objCel.Text = "Garabi2"
                objCel.BackColor = System.Drawing.Color.Beige
                objCel.HorizontalAlign = HorizontalAlign.Center
                objCel.Font.Bold = True
                'objCel.Width = objTamanho.Pixel(132)
                objCel.Attributes.Add("style", "min-width: 50px; width: 50px;")
                objRow.Controls.Add(objCel)

                objCel = New TableCell
                objCel.Text = "Melo"
                objCel.BackColor = System.Drawing.Color.Beige
                objCel.HorizontalAlign = HorizontalAlign.Center
                objCel.Font.Bold = True
                'objCel.Width = objTamanho.Pixel(132)
                objCel.Attributes.Add("style", "min-width: 50px; width: 50px;")
                objRow.Controls.Add(objCel)

                If ListaConversoras.Any(Function(i) i.CodConversora = "ESRIV") Then
                    objCel = New TableCell
                    objCel.Text = "Rivera"
                    objCel.BackColor = System.Drawing.Color.Beige
                    objCel.HorizontalAlign = HorizontalAlign.Center
                    objCel.Font.Bold = True
                    'objCel.Width = objTamanho.Pixel(132)
                    objCel.Attributes.Add("style", "min-width: 50px; width: 50px;")
                    objRow.Controls.Add(objCel)
                End If

                tblGeracao.Controls.Add(objRow)

                For Each Conv As Object In ListConv
                    objCel = New TableCell
                    objCel.Text = "Agente"
                    objCel.Wrap = False
                    objCel.Font.Bold = True
                    'objCel.Width = objTamanho.Pixel(62)
                    objCel.HorizontalAlign = HorizontalAlign.Center
                    'objRow.Width = objTamanho.Pixel(132)
                    objCel.Attributes.Add("style", "min-width: 60px; width: 60px;")
                    objRow.Controls.Add(objCel)

                    objCel = New TableCell
                    objCel.Text = "ONS"
                    objCel.Wrap = False
                    objCel.Font.Bold = True
                    'objCel.Width = objTamanho.Pixel(62)
                    objCel.HorizontalAlign = HorizontalAlign.Center
                    'objRow.Width = objTamanho.Pixel(132)
                    objCel.Attributes.Add("style", "min-width: 50px; width: 50px;")
                    objRow.Controls.Add(objCel)

                Next

                Dim intHora As Integer = 0
                Dim objRows As New List(Of TableRow)()

                For patamar As Integer = 1 To 48
                    Dim objRowp As New TableRow()
                    objRowp.Width = objTamanho.Pixel(132)

                    ' Adicionar célula de hora
                    AddTableCell(objRowp, IntParaHora(patamar), 100, objTamanho)

                    ' Recuperar valores sugeridos
                    Dim valorSugeridoAgente As Decimal = If(listaConversorasAgrupadas.ContainsKey(patamar), listaConversorasAgrupadas(patamar).Sum(Function(n) n.ValorSugeridoAgente), 0)
                    Dim valorSugeridoOns As Decimal = If(listaConversorasAgrupadas.ContainsKey(patamar), listaConversorasAgrupadas(patamar).Sum(Function(n) n.ValorSugeridoOns), 0)
                    'Dim labelText As String = If(valorSugeridoOns = 0, valorSugeridoAgente.ToString(), valorSugeridoOns.ToString())

                    ' Criar labels e células para os valores
                    Dim ids As String()
                    Dim codConversoras As String()
                    Dim texts As String()
                    Dim ocultarRivera As Boolean

                    'Valor negativo no somatório corresponde a forma que é gravado o valor da Exportação da Conversora menos a perda (tabela Despa)

                    If listaSomaConversorasAgrupadas(patamar).Any(Function(i) i.CodConversora = "ESRIV") Then
                        codConversoras = {"", "", "IEGBI1", "IEGBI2", "ESCMEL", "ESRIV"}
                        ids = {"Num_Patamar_", "Num_Patamar_ONS_", "Num2_Patamar_ONS_", "Num3_Patamar_ONS_", "Num4_Patamar_ONS_", "Num5_Patamar_ONS_"}
                        texts = {
       valorSugeridoAgente.ToString(),
       valorSugeridoOns.ToString(),
       If(listaSomaConversorasAgrupadas.ContainsKey(patamar), listaSomaConversorasAgrupadas(patamar).Where(Function(i) i.CodConversora = "IEGBI1").Select(Function(i) i.ValorLiquidoOns).FirstOrDefault() * (-1), 0).ToString(),
       If(listaSomaConversorasAgrupadas.ContainsKey(patamar), listaSomaConversorasAgrupadas(patamar).Where(Function(i) i.CodConversora = "IEGBI2").Select(Function(i) i.ValorLiquidoOns).FirstOrDefault() * (-1), 0).ToString(),
       If(listaSomaConversorasAgrupadas.ContainsKey(patamar), listaSomaConversorasAgrupadas(patamar).Where(Function(i) i.CodConversora = "ESCMEL").Select(Function(i) i.ValorLiquidoOns).FirstOrDefault() * (-1), 0).ToString(),
       If(listaSomaConversorasAgrupadas.ContainsKey(patamar), listaSomaConversorasAgrupadas(patamar).Where(Function(i) i.CodConversora = "ESRIV").Select(Function(i) i.ValorLiquidoOns).FirstOrDefault() * (-1), 0).ToString()
   }
                    Else
                        ocultarRivera = True
                        codConversoras = {"", "", "IEGBI1", "IEGBI2", "ESCMEL"}
                        ids = {"Num_Patamar_", "Num_Patamar_ONS_", "Num2_Patamar_ONS_", "Num3_Patamar_ONS_", "Num4_Patamar_ONS_"}
                        texts = {
       valorSugeridoAgente.ToString(),
       valorSugeridoOns.ToString(),
       If(listaSomaConversorasAgrupadas.ContainsKey(patamar), listaSomaConversorasAgrupadas(patamar).Where(Function(i) i.CodConversora = "IEGBI1").Select(Function(i) i.ValorLiquidoOns).FirstOrDefault() * (-1), 0).ToString(),
       If(listaSomaConversorasAgrupadas.ContainsKey(patamar), listaSomaConversorasAgrupadas(patamar).Where(Function(i) i.CodConversora = "IEGBI2").Select(Function(i) i.ValorLiquidoOns).FirstOrDefault() * (-1), 0).ToString(),
       If(listaSomaConversorasAgrupadas.ContainsKey(patamar), listaSomaConversorasAgrupadas(patamar).Where(Function(i) i.CodConversora = "ESCMEL").Select(Function(i) i.ValorLiquidoOns).FirstOrDefault() * (-1), 0).ToString()
   }
                    End If

                    For j As Integer = 0 To ids.Length - 1
                        AddTableCell(objRowp, texts(j), 62, objTamanho, True, ids(j) & patamar.ToString())
                    Next

                    ' Adicionar valores das listas de conversoras
                    For Each conv As Object In ListConv
                        Dim valoresConversora As IEnumerable(Of ConversoraValorOfertaDTO) = If(listaConversorasAgrupadas.ContainsKey(patamar), listaConversorasAgrupadas(patamar).Where(Function(i) i.CodConversora = conv.CodConversora AndAlso i.CodUsina = conv.CodUsina), Enumerable.Empty(Of ConversoraValorOfertaDTO)())
                        Dim valor As Decimal = valoresConversora.Sum(Function(i) i.ValorSugeridoAgente)
                        AddTableCell(objRowp, valor.ToString(), 62, objTamanho)

                        Dim valorONS As Decimal = valoresConversora.Sum(Function(i) i.ValorSugeridoOns)
                        AddTableCell(objRowp, valorONS.ToString(), 62, objTamanho)

                        If patamar = 1 AndAlso AnaliseONS Then
                            btn_ExportaDadosParaExportacao.Visible = True
                            btnSalvarExportacao.Visible = True
                            btn_Iniciar_Analise_ONS.Visible = True
                            btnReiniciaValoresReferencia.Visible = True

                            Dim valores = listaConversorasAgrupadas.Values.SelectMany(Function(g) g).Where(Function(i) i.CodConversora = conv.CodConversora AndAlso i.CodUsina = conv.CodUsina).OrderBy(Function(i) i.Num_Patamar).Select(Function(i) New With {i.ValorSugeridoOns, i.ValorSugeridoAgente}).ToArray()
                            Dim aplicarDisplayNoneTextArea As Boolean = Not String.IsNullOrEmpty(conv.Flgaprovadoons)
                            AplicarCaixaTextoPatamares(valores, ListConv.IndexOf(conv) + 1, conv.CodConversora, conv.CodUsina, AnaliseONS, ocultarRivera, aplicarDisplayNoneTextArea)
                        End If
                    Next

                    objRows.Add(objRowp)
                Next

                For Each row As TableRow In objRows
                    tblGeracao.Controls.Add(row)
                Next

                tblGeracao.Visible = Conversora.Count > 0
                BotaoSelZerados = False

            End If

        Catch ex As Exception
            AlertaMensagem(ex.Message)
        End Try
    End Sub

    Private Sub HabilitarDesabilitarbotoes(ByVal habilta As Boolean)
        btnAprovarSelecionados.Visible = habilta
        btnDesmarcarTodos.Visible = habilta
        btnReprovarSelcionados.Visible = habilta
        btnSelecionarTodos.Visible = habilta
    End Sub

    Private Function RetornaCor_Empresa(codEmpre_Atual As String, ByRef codEmpre_Ultima As String, ByRef ultima_cor_aplicada As Color) As Color
        Dim cor1 As Color = Color.LightBlue
        Dim cor2 As Color = Color.DodgerBlue

        If codEmpre_Atual <> codEmpre_Ultima Then
            ultima_cor_aplicada = IIf(ultima_cor_aplicada = cor1, cor2, cor1)
            codEmpre_Ultima = codEmpre_Atual
        End If

        Return ultima_cor_aplicada
    End Function

    Private Function RetornaCor(flg_aprovadoAgente As String, flg_aprovadoOns As String, valores As List(Of ConversoraValorOfertaDTO)) As Color

        Dim Cor As Color = Color.Beige

        'If AnaliseONS = True Then
        'Return Cor
        'End If

        If flg_aprovadoOns = "S" And String.IsNullOrEmpty(flg_aprovadoAgente) Then
            Cor = Color.Yellow

            If valores.Any(Function(v) v.ValorSugeridoAgente <> v.ValorSugeridoOns) Then
                Cor = Color.Orange
            End If

        ElseIf flg_aprovadoOns = "N" Then
            Cor = Color.LightGray
        ElseIf flg_aprovadoOns = "S" And flg_aprovadoAgente = "S" Then
            Cor = Color.LightGreen
        ElseIf flg_aprovadoOns = "S" And flg_aprovadoAgente = "N" Then
            Cor = Color.Red
        End If

        Return Cor
    End Function


    <System.Web.Services.WebMethod()>
    Public Shared Function ObterDadosPercentualPercaUsinaParaAtualizacao(ByVal datPdp As String) As List(Of ConversoraValorOfertaDTO)
        Dim OfertaExp As New OfertaExportacaoDAO

        If IsNothing(ValoresOfertasPercPercaCache) And UltimaAlteracaoCacheValorsOfertasPerc.Equals(DateTime.MinValue) Or UltimaAlteracaoCacheValorsOfertasPerc.AddHours(3) < DateTime.Now Then
            Dim ValoresOfertas As List(Of ConversoraValorOfertaDTO) = OfertaExp.ListarOfertasAgrupadasPorConversorasUsinasEPercentualPerca(datPdp)

            If Not IsNothing(ValoresOfertas) Then
                ValoresOfertasPercPercaCache = ValoresOfertas
                UltimaAlteracaoCacheValorsOfertasPerc = DateTime.Now
            End If

            Return ValoresOfertas
        End If

        Return ValoresOfertasPercPercaCache

    End Function

    <System.Web.Services.WebMethod()>
    Public Shared Function AprovarReprovarOfertaSelecionada(ByVal OfertaOrdem As Object, ByVal OfertaOrdemDetalhe As Object, ByVal DatPDP As String, ByVal codEmpresa As String, ByVal AnaliseONS As String, ByVal tblGeracao As String, ByVal loginUsuario As String, ByVal flgAprovado As String, ByVal AgentesRepresentados As String) As String
        Dim OfertaExportacao As List(Of UsiConversDTO) = New List(Of UsiConversDTO)
        Dim ValoresOfertaExportacao As List(Of ConversoraValorOfertaDTO) = New List(Of ConversoraValorOfertaDTO)
        Dim Retorno As String
        Dim Classe As New frmCnsAnaliseOfertaExportacao
        Dim Ret As String = ""
        Try
            If AnaliseONS = "S" Then
                Classe.AnaliseONS = True
            Else
                Classe.AnaliseONS = False
            End If

            Classe.logger.Info("Iniciando o laço for para separar as ofertas e análise se são da ONS ou não")

            For Index As Integer = 0 To (OfertaOrdem.Length - 1)
                Dim ofExport As UsiConversDTO = New UsiConversDTO

                ofExport.CodUsina = OfertaOrdem(Index)(0).ToString()
                ofExport.CodUsinaConversora = OfertaOrdem(Index)(1).ToString()
                ofExport.DatPdp = DatPDP
                If Classe.AnaliseONS Then
                    ofExport.flgaprovadoons = OfertaOrdem(Index)(3).ToString()
                    ofExport.dinanaliseons = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                    ofExport.OrdemOns = CType(OfertaOrdem(Index)(2).ToString(), Integer)
                    ofExport.OrdemAgente = ofExport.OrdemOns
                Else
                    If Ret = "" Then
                        Ret = OfertaOrdem(Index)(3).ToString()
                    End If
                    ofExport.flgaprovadoagente = OfertaOrdem(Index)(3).ToString()
                    ofExport.dinanaliseagente = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                End If

                OfertaExportacao.Add(ofExport)
            Next

            For IndexDetalhe As Integer = 0 To (OfertaOrdemDetalhe.Length - 1)
                For I As Integer = 2 To 49
                    Dim Valores As ConversoraValorOfertaDTO = New ConversoraValorOfertaDTO
                    Valores.CodUsina = OfertaOrdemDetalhe(IndexDetalhe)(0).ToString()
                    Valores.CodConversora = OfertaOrdemDetalhe(IndexDetalhe)(1).ToString()
                    Valores.DatPdp = DatPDP
                    Valores.Num_Patamar = I - 1
                    Valores.ValorSugeridoOns = OfertaOrdemDetalhe(IndexDetalhe)(I).ToString()

                    ValoresOfertaExportacao.Add(Valores)

                Next
            Next
            '
            ' Chamada para a gravação da exportação para o balanço
            '
            Dim ofertaExportacaoBusiness As OfertaExportacaoBusiness = New Ons.interface.business.OfertaExportacaoBusiness()
            Classe.logger.Info("Iniciando o método de ofertaExportacaoBusiness.GravarAnalise")
            ofertaExportacaoBusiness.GravarAnalise(OfertaExportacao, ValoresOfertaExportacao, Classe.AnaliseONS, loginUsuario, DatPDP, codEmpresa, flgAprovado, AgentesRepresentados)
            Classe.logger.Info("Fin do método de ofertaExportacaoBusiness.GravarAnalise")

            If Ret = "" Or Ret = "S" Then
                '
                ' WI 189663 - Separar as mensagens dependendo da escolha dos botões (Salvar exportação ou Exportação para Balanço)
                '
                If flgAprovado = "O" Then
                    Retorno = "Dados gravados com sucesso na tabela de ofertas."
                Else
                    Retorno = "Dados gravados com sucesso e exportados para balanço."
                End If
            Else
                Retorno = "Dados retirados com sucesso."
            End If

        Catch ex As Exception
            Dim msgErro As String = ""

            For Each msg As String In ex.Message.Split("|").Take(100).ToList()
                msgErro += $"|{msg}"
            Next

            If ex.Message.Split("|").ToList().Count > 100 Then
                msgErro += $"|..."
            End If

            Retorno = $"Problema ao gravar análise:{msgErro.Replace("'", "")}"
        End Try

        Return Retorno

    End Function

    Protected Sub DataPdpDropDown_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DataPdpDropDown.SelectedIndexChanged

        Session("datEscolhida") = DataPdpDropDown.SelectedValue
        Session("strCodEmpre") = cboEmpresa.SelectedValue
        PrazoEnvio()
        Atualiza_Table_Usinas()

        If DataPdpDropDown.SelectedValue <> "0" And AnaliseONS Then
            btn_Importar_Planilha.Visible = True
            fl_upload_planilha.Visible = True
        Else
            btn_Importar_Planilha.Visible = False
            fl_upload_planilha.Visible = False
        End If
    End Sub


    Protected Sub PrazoEnvio()

        DentroPrazoEnvio = False

        If AnaliseONS = True Then
            DentroPrazoEnvio = True
            '
            ' WI 189663 - Suprimir botões de Manter e aprovar ofertas - Esse método estava após o EndIF
            '
            HabilitarDesabilitarbotoes(DentroPrazoEnvio)
        Else
            Dim listaCodEmpresas As List(Of String) = New List(Of String)()
            For Each empre As ListItem In cboEmpresa.Items
                If empre.Value.Length > 0 Then
                    listaCodEmpresas.Add(empre.Value)
                End If
            Next

            If Not IsNothing(DataPdpDropDown.SelectedItem) Then
                DentroPrazoEnvio =
                    Me.FactoryBusiness.OfertaExportacao.
                    ValidarPrazoAnaliseOfertaAgente(DataPdpDropDown.SelectedItem.Text, listaCodEmpresas)
            End If
        End If

        HabilitarDesabilitarbotoes(DentroPrazoEnvio)

        If Not DentroPrazoEnvio AndAlso Not IsNothing(DataPdpDropDown.SelectedItem) AndAlso DataPdpDropDown.SelectedItem.Text <> "" Then
            AlertaMensagem("Prazo esgotado para envio de Oferta")
        End If

    End Sub

    Protected Sub btn_excluir_ofertas_selecionadas_Click(sender As Object, e As ImageClickEventArgs) Handles btn_excluir_ofertas_selecionadas.Click
        Atualiza_Table_Usinas()
    End Sub

    Protected Sub btnAprovarSelecionados_Click(sender As Object, e As ImageClickEventArgs) Handles btnAprovarSelecionados.Click
        Atualiza_Table_Usinas()
    End Sub

    Protected Sub btnReprovarSelcionados_Click(sender As Object, e As ImageClickEventArgs) Handles btnReprovarSelcionados.Click
        Atualiza_Table_Usinas()
    End Sub

    Protected Sub btn_ExportaDadosParaExportacao_Click(sender As Object, e As ImageClickEventArgs) Handles btn_ExportaDadosParaExportacao.Click
        ' Me.ExportarDadosParaBalanco() - Já é feito quando faz o "GravarAnalise"
        Atualiza_Table_Usinas()
    End Sub

    Protected Sub btn_Iniciar_Analise_ONS_Click(sender As Object, e As ImageClickEventArgs) Handles btn_Iniciar_Analise_ONS.Click
        Iniciar_AnaliseOfertas_ONS()
        Atualiza_Table_Usinas()
    End Sub

    Protected Sub btnbtnSalvarExportacao_Click(sender As Object, e As ImageClickEventArgs) Handles btnSalvarExportacao.Click
        Atualiza_Table_Usinas()
    End Sub

    Private Sub Iniciar_AnaliseOfertas_ONS()
        Me.FactoryBusiness.OfertaExportacao.IniciarAnaliseOfertasONS(Session("datEscolhida"))
    End Sub

    Public Sub ReiniciarDecisaoDeAnalise()

        Try
            If Not String.IsNullOrEmpty(DataPdpDropDown.SelectedItem.Text.Trim()) Then
                Dim dataPDP As String = DataPdpDropDown.SelectedValue
                Me.FactoryBusiness.OfertaExportacao.ReiniciarDecisaoDeAnalise(dataPDP)
                Me.Atualiza_Table_Usinas()
                Me.AlertaMensagem($"As ofertas da data {DataPdpDropDown.SelectedItem.Text.Trim()} foram reiniciadas com sucesso.")
            Else
                Me.AlertaMensagem($"Selecione uma data PDP.")
            End If
            lblMsg.Visible = False
        Catch ex As Exception
            lblMsg.Visible = True
            'Session("strMensagem") = ex.Message
            'Response.Redirect("frmMensagem.aspx")
        End Try


    End Sub

    Public Sub ReiniciarValoresReferencia()

        Try
            If Not String.IsNullOrEmpty(DataPdpDropDown.SelectedItem.Text.Trim()) Then
                Dim dataPDP As String = DataPdpDropDown.SelectedValue
                Me.FactoryBusiness.OfertaExportacao.ReiniciarValoresReferencia(dataPDP)
                Me.Atualiza_Table_Usinas()
                Me.AlertaMensagem($"Os Valores de Referência de Programação da data {DataPdpDropDown.SelectedItem.Text.Trim()} foram reiniciadas com sucesso.")
            Else
                Me.AlertaMensagem($"Selecione uma data PDP.")
            End If
            lblMsg.Visible = False
        Catch ex As Exception
            lblMsg.Visible = True
            'Session("strMensagem") = ex.Message
            'Response.Redirect("frmMensagem.aspx")
        End Try


    End Sub

    Private Function VerificarECorrigirDataPDPSelecionada() As Boolean
        If IsNothing(Session("datEscolhida")) Or String.IsNullOrEmpty(Session("datEscolhida")) Then
            Session("datEscolhida") = DataPdpDropDown.SelectedValue
        End If

        If Int32.Parse(Session("datEscolhida")).Equals(0) Then
            Return False
        Else
            Return True
        End If
    End Function

    Protected Sub reiniciardecisaoanalise_Click(sender As Object, e As ImageClickEventArgs)
        Me.ReiniciarDecisaoDeAnalise()
    End Sub

    Protected Sub reiniciarvaloresreferencia_Click(sender As Object, e As ImageClickEventArgs)
        Me.ReiniciarValoresReferencia()
    End Sub

    Protected Sub btn_Importar_Planilha_Click(sender As Object, e As ImageClickEventArgs) Handles btn_Importar_Planilha.Click
        Dim dataPDP As String = DataPdpDropDown.SelectedValue
        If fl_upload_planilha.HasFile Then
            ' Obtém o nome do arquivo
            Dim fileName As String = fl_upload_planilha.FileName

            Dim extensaoArquivo As String = System.IO.Path.GetExtension(fileName).ToLower()
            If extensaoArquivo.Equals(".xls") Or extensaoArquivo.Equals(".xlsx") Then
                Dim caminhoArquivo As String = Path.Combine(ConfigurationManager.AppSettings.Get("PathArquivosGerados"), fileName)
                fl_upload_planilha.SaveAs(caminhoArquivo)

                Dim resultado As List(Of String) = Me.FactoryBusiness.OfertaExportacao.RealizarLeituraPlanilhaValoresOfertaExportacaoONS(dataPDP, caminhoArquivo)

                If resultado.Any() Then
                    Dim mensagem As String = "Resultado da importação da planilha: " & "\n" & String.Join("\n", resultado)

                    ' Exibe a mensagem formatada com as exceções listadas
                    Me.AlertaMensagem(mensagem)

                Else
                    Me.AlertaMensagem($"Planilha: ({fileName}) carregada com sucesso.")
                End If

                ' Exclui a planilha do diretório para liberar espaço
                File.Delete(caminhoArquivo)

                Me.Atualiza_Table_Usinas()
            Else
                Me.AlertaMensagem($"Arquivo selecionado está no formato inválido, selecione somente arquivos .xls ou .xlsx para importação! ")
            End If
        Else
            ' Se nenhum arquivo foi selecionado
            Me.Atualiza_Table_Usinas()
            Me.AlertaMensagem($"Selecione um arquivo válido para importar.")
        End If
    End Sub
End Class

