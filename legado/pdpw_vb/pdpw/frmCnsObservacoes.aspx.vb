Imports System.Collections.Generic
Imports System.Data.SqlClient

Partial Class frmCnsObservacoes
    Inherits BaseWebUi

    Dim indice_inicial As Integer = 0      ' Índice inicial para paginação.
    Dim Conn As SqlConnection = New SqlConnection
    Dim Cmd As SqlCommand = New SqlCommand
    Dim IdComentario As Integer
    Dim EVT, TPusina As String

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

        ''objMenu = CType(Session("onsmenu"), OnsWebControls.OnsMenu)
        Try

            If Not Page.IsPostBack Then
                Dim intI As Integer
                Dim Conn As SqlConnection = New SqlConnection
                Dim Cmd As SqlCommand = New SqlCommand
                Dim objitem As WebControls.ListItem
                Conn.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
                Cmd.Connection = Conn
                Conn.Open()
                Cmd.CommandText = "SELECT distinct pdp.datpdp " &
                                  "FROM pdp " &
                                  "INNER JOIN tb_statusimportacaodessem ON pdp.datpdp = REPLACE(CONVERT(VARCHAR, dat_processo, 112), '-', '') " &
                                  "ORDER BY datpdp DESC"
                Dim rsData As SqlDataReader = Cmd.ExecuteReader
                intI = 1

                objitem = New System.Web.UI.WebControls.ListItem
                objitem.Text = ""
                objitem.Value = ""

                DataPdpDropDown.Items.Clear()
                DataPdpDropDown.Items.Add(objitem)

                If Session("datEscolhida") = Nothing Then
                    Session("datEscolhida") = Now.AddDays(1)
                End If

                Do While rsData.Read
                    objitem = New System.Web.UI.WebControls.ListItem
                    objitem.Text = Mid(rsData("datpdp"), 7, 2) & "/" & Mid(rsData("datpdp"), 5, 2) & "/" & Mid(rsData("datpdp"), 1, 4)
                    objitem.Value = rsData("datpdp")
                    DataPdpDropDown.Items.Add(objitem)

                    If DataPdpDropDown.SelectedIndex = 0 Then
                        DataPdpDropDown.SelectedIndex = 1
                    End If

                    If Trim(DataPdpDropDown.Items(intI).Value) = Format(Session("datEscolhida"), "yyyyMMdd") Then
                        DataPdpDropDown.SelectedIndex = intI
                    End If
                    intI = intI + 1
                Loop

                rsData.Close()
                rsData = Nothing
                Cmd.Connection.Close()
                Conn.Close()

                PreencheComboEmpresaPOP(AgentesRepresentados, cboEmpresa, Session("strCodEmpre"))
                cboEmpresa.SelectedIndex = 0

                PopularComboInsumo()
                Atualizar_Dados_Usina()

                divValor.Visible = False
                tblGeracao.Visible = False

                indice_inicial = 0
                dtgEnvio.CurrentPageIndex = 0

                TxtComentarioONS.Visible = False
                lblComentarioONS.Visible = False
                btnImgSalvar.Visible = False
                divValor.Visible = False
                InsumoDropDown.Enabled = False
                ChkEVT.Enabled = False

                btnIncluirComSugestao.Attributes.Remove("Disabled")
                btnExcluirComSugestao.Attributes.Remove("Disabled")
                btnIncluirSemSugestao.Attributes.Remove("Disabled")
                btnExcluirSemSugestao.Attributes.Remove("Disabled")
            End If
            lblMsg.Visible = False
        Catch ex As Exception
            lblMsg.Visible = True
            If Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If

            'Session("strMensagem") = "Não foi possível visualizar os dados" + ".  " + ex.Message
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

    Private Sub PopularListBoxUsina()
        Dim Conn As SqlConnection = New SqlConnection
        Dim Cmd As SqlCommand = New SqlCommand
        Dim empresa As String

        UsinaListBox.Items.Clear()

        empresa = Session("strCodEmpre")

        If empresa = Nothing Or empresa = "" Then
            Exit Sub
        End If

        Conn.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
        Cmd.Connection = Conn
        Conn.Open()

        Cmd.CommandText = "SELECT Trim(us.codusina) as CodUsina, Trim(us.codusina) + ' - ' + us.nomusina as DescricaoUsina ,
                            us.nomusina
                           FROM usina us 
                           WHERE us.CodEmpre = '" + empresa + "'
                           and us.flg_recebepdpage = 'S' 
                           ORDER BY 2"

        Dim rsDataUsina As SqlDataReader = Cmd.ExecuteReader
        Dim objItemUsina As WebControls.ListItem

        Do While rsDataUsina.Read
            objItemUsina = New System.Web.UI.WebControls.ListItem
            objItemUsina.Text = rsDataUsina("DescricaoUsina")
            objItemUsina.Value = rsDataUsina("CodUsina")
            UsinaListBox.Items.Add(objItemUsina)
        Loop

        rsDataUsina.Close()
        rsDataUsina = Nothing
        ObterAnaliseSugestao()
        Conn.Close()
    End Sub

    Private Sub PopularComboInsumo()
        Dim Conn As SqlConnection = New SqlConnection
        Dim Cmd As SqlCommand = New SqlCommand
        Conn.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
        Cmd.Connection = Conn
        Conn.Open()

        Cmd.CommandText = "select id_insumo, dsc_mnemonico, dsc_insumo from tb_insumo order by dsc_mnemonico"

        Dim rsDataInsumo As SqlDataReader = Cmd.ExecuteReader
        Dim objItemInsumo As WebControls.ListItem

        objItemInsumo = New System.Web.UI.WebControls.ListItem
        'objItemInsumo.Text = ""
        'objItemInsumo.Value = ""

        InsumoDropDown.Items.Add(objItemInsumo)

        Do While rsDataInsumo.Read
            objItemInsumo = New System.Web.UI.WebControls.ListItem
            objItemInsumo.Text = rsDataInsumo("dsc_mnemonico") + " - " + rsDataInsumo("dsc_insumo")
            objItemInsumo.Value = rsDataInsumo("id_insumo")

            If objItemInsumo.Text.StartsWith("GER") Then
                InsumoDropDown.Items.Add(objItemInsumo)
                objItemInsumo.Selected = True
            End If

        Loop

        rsDataInsumo.Close()
        rsDataInsumo = Nothing

        Conn.Close()
    End Sub
    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        If SessaoAtiva(Page.Session) Then
            MyBase.Render(writer)
            'objMenu.RenderControl(writer)
        End If
    End Sub

    Protected Sub PreencherGrid(ByVal sender As Object, ByVal e As EventArgs)
        Atualiza_Table_Usinas()
    End Sub
    Private Sub Atualizar_Dados_Usina()
        Dim daObs As SqlDataAdapter
        Dim dsObs As DataSet
        Dim dt As DataTable

        Cmd = New SqlCommand()
        Cmd.Connection = Conn
        Try

            If Not String.IsNullOrEmpty(UsinaDropDown.SelectedValue) And
               Not String.IsNullOrEmpty(InsumoDropDown.SelectedValue) And
               Not String.IsNullOrEmpty(DataPdpDropDown.SelectedValue) Then

                If Conn.State <> ConnectionState.Open Then
                    Conn.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
                    Conn.Open()
                End If

                'Obtém o último comentários
                Cmd.CommandText = "select dsc_comentario, flg_evt from tb_comentario_dessem
                                   where id_comentario = '" + UsinaDropDown.SelectedValue + "'"

                Dim rsComentario As SqlDataReader = Cmd.ExecuteReader

                TxtDescricao.Text = ""
                TxtDescricao.Enabled = True

                Do While rsComentario.Read
                    If VarType(rsComentario("dsc_comentario")) <> vbNull Then
                        If (rsComentario("dsc_comentario").ToString().Trim() <> String.Empty) Then
                            TxtDescricao.Text = rsComentario("dsc_comentario").ToString()
                            TxtDescricao.Enabled = False
                            ChkEVT.Enabled = False
                        End If
                    End If
                    If rsComentario("flg_evt").ToString() = "S" Then
                        ChkEVT.Checked = True
                    Else
                        ChkEVT.Checked = False
                    End If
                Loop
                rsComentario.Close()

            Else
                TxtDescricao.Text = ""
                TxtDescricao.Enabled = False
                ChkEVT.Enabled = False
            End If

            PreencherComentario()

        Catch ex As Exception
            Throw (ex)
        Finally
            If Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If
        End Try
    End Sub

    Private Function ObterComando_DadosInsumo(Optional ByVal codUsina As String = "", Optional ByVal codEmpresa As String = "") As String
        Dim tabela As String = Nothing
        Dim apelido As String = Nothing

        ObterParametrosInsumo(tabela, apelido)

        'Obter dados de patamares para o insumo
        Dim comando As String = " select t.CodUsina, Cast(Int" + apelido + " as Varchar(15)) As patamar, val" + apelido + "PRE As vlr_dessem, p.Val_Sugerido As vlr_sugerido, c.flg_sugestaoAgente, c.flg_aprovado, Trim(c.Dsc_Comentario) as Descricao " +
                          " from " + tabela + " t " +
                          " Join Usina u on (u.codUsina = t.CodUsina) " +
                          " Left Join tb_comentario_Dessem c on (t.datpdp = c.dat_pdp and t.codUsina = c.Cod_Usina and c.Id_Insumo = '" + InsumoDropDown.SelectedValue + "') " +
                          " Left Join tb_comentario_dessem_patamar p on (c.id_comentario = p.id_comentario and t.Int" + apelido + " = p.Num_Patamar) " +
                          " where t.datpdp = '" + DataPdpDropDown.SelectedValue + "' " +
                          " {0} " +
                          " {1} " +
                          "  And u.flg_recebepdpage = 'S' " +
                          " and (
                                         c.id_comentario = (select max(c2.id_comentario) from tb_comentario_dessem c2 where c2.dat_pdp = c.dat_pdp and c2.Cod_Usina = c.Cod_Usina and c2.Id_Insumo = c.Id_Insumo) 
                                         or 
                                         c.id_comentario is null) " +
                          " Order by u.Ordem, t.Codusina, Int" + apelido

        Dim cmdUsina As String = ""
        Dim cmdEmpresa As String = ""

        If Not String.IsNullOrEmpty(codUsina) Then
            cmdUsina = $" and t.codusina = '{codUsina }' "
        End If

        If Not String.IsNullOrEmpty(codEmpresa) Then
            cmdEmpresa = $" and u.codEmpre = '{codEmpresa}' "
        End If

        comando = String.Format(comando, cmdUsina, cmdEmpresa)

        Return comando
    End Function

    Private Sub ObterParametrosInsumo(ByRef tabela As String, ByRef apelido As String)

        If Not Conn.State = ConnectionState.Open Then
            Conn.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
            Conn.Open()
        End If

        Cmd.Connection = Conn
        Cmd.CommandText = "select nom_tabela, dsc_mnemonicotabela from tb_insumo where id_insumo = '" + InsumoDropDown.SelectedValue + "'"

        Dim rsInsumo As SqlDataReader = Cmd.ExecuteReader
        Do While rsInsumo.Read
            tabela = rsInsumo("nom_tabela")
            apelido = rsInsumo("dsc_mnemonicotabela")
        Loop
        rsInsumo.Close()
    End Sub




    Protected Sub SalvarInformacoes(ByVal sender As Object, e As EventArgs)
        Me.SalvarComentario()
    End Sub


    Private Sub AplicarValoresSugeridos_DeAte()

        Dim dataGrid As DataTable
        Dim intDeVal As Int32
        Dim intAteVal As Int32
        Dim intValorSugerido As Int32
        Dim i As Int32

        If Not ViewState("dataGrid") Is Nothing Then

            If Int32.Parse(DropDownList_PatamarAte.SelectedValue) < Int32.Parse(DropDownList_PatamarDe.SelectedValue) Then
                AlertaMensagem("Intervalo (De - para) de Patamares é inválido para aplivar valor sugerido, verifique.")
                Exit Sub
            End If

            If String.IsNullOrEmpty(DropDownList_PatamarDe.SelectedValue) Then
                intDeVal = 1
            Else
                intDeVal = Convert.ToInt32(DropDownList_PatamarDe.SelectedValue)
            End If

            If String.IsNullOrEmpty(DropDownList_PatamarAte.SelectedValue) Then
                intAteVal = 1
            Else
                intAteVal = Convert.ToInt32(DropDownList_PatamarAte.SelectedValue)
            End If

            If String.IsNullOrEmpty(TextBoxValorSugerido.Text) Then
                'fazer a validação
                intValorSugerido = 1
            Else
                intValorSugerido = Convert.ToInt32(TextBoxValorSugerido.Text)
            End If

            dataGrid = ViewState("dataGrid")

            intDeVal = intDeVal - 1
            intAteVal = intAteVal - 1

            For i = intDeVal To intAteVal
                dataGrid.Rows(i)("vlr_sugerido") = intValorSugerido
                dataGrid.Rows(i)("alterado") = 1
            Next

            ViewState("dataGrid") = dataGrid

            Atualizar_Dados_Usina()
        Else
            AlertaMensagem("Valores para carregamento dos dados DESSEM não foram informados, verifique.")
            Exit Sub
        End If

    End Sub

    Private Function AplicarValoresSugeridos_EmLote() As List(Of Integer)

        Dim intI As Integer
        Dim intColAtual As Integer
        Dim intColAnterior As Integer
        Dim intValor As Integer
        Dim strValor As String
        Dim valorInserido As String
        Dim dataGrid As DataTable
        Dim listaPatamar As New List(Of Integer)

        If divValor.Visible = True And Not String.IsNullOrEmpty(ObterValoresLote()) Then

            strValor = ObterValoresLote()
            intI = 1
            intColAnterior = 1
            intColAtual = InStr(intColAnterior, strValor, Chr(13), CompareMethod.Binary)

            'dataGrid = ViewState("dataGrid")

            For intI = 1 To 48

                valorInserido = ""

                If intColAtual <> 0 Then
                    If Mid(strValor, intColAnterior, (intColAtual - intColAnterior) + 1) <> "" Then
                        intValor = Val(Mid(strValor, intColAnterior, (intColAtual - intColAnterior) + 1))
                        valorInserido = intValor.ToString()
                    End If
                    intColAnterior = intColAtual
                ElseIf intColAtual = 0 And Mid(strValor, intColAnterior) <> "" Then
                    intValor = Val(Mid(strValor, intColAnterior))
                    valorInserido = intValor.ToString()

                    intColAnterior = intColAnterior + Trim(Mid(strValor, intColAnterior)).Length
                Else
                    intValor = 0
                    valorInserido = ""
                End If

                If Not String.IsNullOrEmpty(valorInserido) Then
                    'dataGrid.Rows(intI - 1)("vlr_sugerido") = valorInserido
                    listaPatamar.Add(valorInserido)
                Else
                    'dataGrid.Rows(intI - 1)("vlr_sugerido") = DBNull.Value
                    listaPatamar.Add(0)
                End If


                intColAtual = InStr(intColAnterior + 1, strValor, Chr(13), CompareMethod.Binary)
            Next

            'ViewState("dataGrid") = dataGrid
            'dtgEnvio.DataSource = ViewState("dataGrid")
            'dtgEnvio.DataBind()
            Return listaPatamar
        End If
    End Function

    Protected Sub AplicarValorSugerido(ByVal sender As Object, e As EventArgs)


        AplicarValoresSugeridos_EmLote()
        AplicarCaixaTextoPatamares(Not divValor.Visible)

    End Sub

    Private Function ObterValoresLote() As String
        Return Page.Request.Form.Item("_ctl0:ContentPlaceHolder1:txtValorLote")
    End Function

    Private Sub AplicarCaixaTextoPatamares(Optional ByVal valores As String = "", Optional ByVal indiceColunaUsina As Integer = 0)

        Dim mostrar As Boolean = False

        If Not String.IsNullOrEmpty(UsinaDropDown.SelectedValue) And String.IsNullOrEmpty(TxtDescricao.Text.Trim()) Then
            mostrar = True
        End If

        If (mostrar) Then
            Dim objTextArea As HtmlControls.HtmlTextArea
            objTextArea = New HtmlTextArea
            objTextArea.Rows = 48
            objTextArea.ID = "txtValorLote"
            objTextArea.Attributes.Item("onkeyup") = "RetiraEnter(event)"
            objTextArea.Attributes.Item("runat") = "server"
            objTextArea.Attributes.Item("style") = "font-family:Arial;font-size:Smaller;height:851px;width:53px;line-height:17.6px"

            If Not String.IsNullOrEmpty(valores) Then
                objTextArea.Value = valores
            End If

            divValor.Controls.Add(objTextArea)
            divValor.Style.Item("TOP") = "499px"

            Dim left As Integer = 193

            If indiceColunaUsina <> 0 Then
                left = left + (101 * indiceColunaUsina)
            End If

            divValor.Style.Item("LEFT") = $"{left}px"

            divValor.Visible = True

            'btnSalvar.Visible = True
            'lblMsgSalvar.Visible = True
        Else
            divValor.Visible = False
            divValor.Controls.Clear()

            'btnSalvar.Visible = False
            'lblMsgSalvar.Visible = False
        End If

    End Sub

    Private Sub AlertaMensagem(ByVal mensagemTexto As String)
        Dim mensagem As String
        mensagem = "<script type='text/javascript'> showAlert('" + mensagemTexto.Replace("\n", "") + "'); </script>"
        ClientScript.RegisterClientScriptBlock(Me.GetType(), "myscript", mensagem)
    End Sub

    Protected Sub cboEmpresa_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboEmpresa.SelectedIndexChanged

        Session("strCodEmpre") = ""

        If cboEmpresa.SelectedIndex > 0 Then
            Session("strCodEmpre") = cboEmpresa.SelectedItem.Value
        End If

        Atualiza_Table_Usinas()

    End Sub

    Private Sub Atualiza_Table_Usinas()
        'Usinas
        Dim intI, intJ As Integer
        Dim intLin As Integer
        Dim intPat As Integer
        Dim dblMedia As Double
        Dim objCel As TableCell
        Dim objRow As TableRow
        Dim objTamanho As WebControls.Unit

        'Dim Conn As SqlConnection = New SqlConnection
        Cmd = New SqlCommand()

        Dim strCodUsina As String
        Try

            If Not String.IsNullOrEmpty(DataPdpDropDown.SelectedValue) And
               Not String.IsNullOrEmpty(cboEmpresa.SelectedValue) And
               Not String.IsNullOrEmpty(InsumoDropDown.SelectedValue) Then

                Session("datEscolhida") = DataPdpDropDown.SelectedValue
                Session("strCodEmpre") = cboEmpresa.SelectedValue
                Conn.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
                Conn.Open()
                Cmd.Connection = Conn

                Cmd.CommandText = ObterComando_DadosInsumo("", Session("strCodEmpre"))

                Dim rsUsina As SqlDataReader = Cmd.ExecuteReader

                tblGeracao.Visible = False
                tblGeracao.Rows.Clear()

                objTamanho = New WebControls.Unit

                objRow = New TableRow()
                objRow.BackColor = System.Drawing.Color.Beige
                objRow.Width = objTamanho.Pixel(100)
                objCel = New TableCell
                objCel.Wrap = False
                objCel.Text = "Intervalo"
                objCel.Font.Bold = True
                objCel.HorizontalAlign = HorizontalAlign.Center
                objCel.Width = objTamanho.Pixel(200)
                objCel.Height = objTamanho.Pixel(17)
                objRow.Controls.Add(objCel)

                objCel = New TableCell
                objCel.Text = "Total"
                objCel.Wrap = False
                objCel.Font.Bold = True
                objCel.Width = objTamanho.Pixel(62)
                objCel.HorizontalAlign = HorizontalAlign.Center
                objRow.Width = objTamanho.Pixel(132)
                objCel.Height = objTamanho.Pixel(17)
                objRow.Controls.Add(objCel)
                tblGeracao.Width = objTamanho.Pixel(132)
                tblGeracao.Controls.Add(objRow)

                'Cria linhas em branco para Intervalo e Total
                objRow = New TableRow
                objRow.Width = objTamanho.Pixel(132)

                objCel = New TableCell
                objCel.BackColor = System.Drawing.Color.Beige
                objCel.Font.Bold = True
                objCel.Wrap = False
                objCel.Width = objTamanho.Pixel(200)
                objCel.Height = objTamanho.Pixel(17)
                objCel.HorizontalAlign = HorizontalAlign.Center
                objCel.Text = ""
                objRow.Controls.Add(objCel)

                objCel = New TableCell
                objCel.Text = ""
                objCel.BackColor = System.Drawing.Color.Beige
                objCel.HorizontalAlign = HorizontalAlign.Center
                objCel.Font.Bold = True
                objCel.Width = objTamanho.Pixel(100)
                objCel.Height = objTamanho.Pixel(17)
                objRow.Controls.Add(objCel)

                tblGeracao.Controls.Add(objRow)

                Dim intHora As Integer = 0
                For intI = 1 To 48
                    objRow = New TableRow
                    objRow.Width = objTamanho.Pixel(132)

                    objCel = New TableCell
                    objCel.BackColor = System.Drawing.Color.Beige
                    objCel.Font.Bold = True
                    objCel.Wrap = False
                    objCel.Width = objTamanho.Pixel(200)
                    objCel.Height = objTamanho.Pixel(17)
                    objCel.HorizontalAlign = HorizontalAlign.Center
                    objCel.Text = IntParaHora(intI)

                    objRow.Controls.Add(objCel)
                    objCel = New TableCell
                    objCel.Text = 0
                    objCel.BackColor = System.Drawing.Color.Beige
                    objCel.Font.Bold = True
                    objCel.Width = objTamanho.Pixel(62)
                    objCel.Height = objTamanho.Pixel(17)
                    objRow.Controls.Add(objCel)
                    tblGeracao.Controls.Add(objRow)
                Next

                objRow = New TableRow
                objRow.Width = objTamanho.Pixel(132)
                objCel = New TableCell
                objCel.Text = "Total"
                objCel.BackColor = System.Drawing.Color.Beige
                objCel.Font.Bold = True
                objCel.Width = objTamanho.Pixel(70)
                objCel.Height = objTamanho.Pixel(17)
                objCel.HorizontalAlign = HorizontalAlign.Center
                objRow.Controls.Add(objCel)
                objCel = New TableCell
                objCel.BackColor = System.Drawing.Color.Beige
                objCel.Font.Bold = True
                objCel.Width = objTamanho.Pixel(62)
                objCel.Height = objTamanho.Pixel(17)
                objRow.Controls.Add(objCel)
                tblGeracao.Controls.Add(objRow)

                objRow = New TableRow
                objRow.Width = objTamanho.Pixel(132)
                objCel = New TableCell
                objCel.Text = "Média"
                objCel.BackColor = System.Drawing.Color.Beige
                objCel.Font.Bold = True
                objCel.Width = objTamanho.Pixel(70)
                objCel.Height = objTamanho.Pixel(17)
                objCel.HorizontalAlign = HorizontalAlign.Center
                objRow.Controls.Add(objCel)
                objCel = New TableCell
                objCel.BackColor = System.Drawing.Color.Beige
                objCel.Font.Bold = True
                objCel.Width = objTamanho.Pixel(62)
                objCel.Height = objTamanho.Pixel(17)
                objRow.Controls.Add(objCel)
                tblGeracao.Controls.Add(objRow)

                intI = 1
                strCodUsina = ""
                dblMedia = 0

                Dim listaUsina As List(Of UsinaComentarioDESSEM_DTO) = New List(Of UsinaComentarioDESSEM_DTO)

                Do While rsUsina.Read
                    Dim usina As UsinaComentarioDESSEM_DTO = New UsinaComentarioDESSEM_DTO()
                    usina.CodUsina = IIf(rsUsina.Item("codusina") Is DBNull.Value, "", rsUsina.Item("codusina").ToString())
                    usina.Num_Patamar = IIf(rsUsina.Item("Patamar") Is DBNull.Value, "", rsUsina.Item("Patamar").ToString())
                    usina.ValorDESSEM = IIf(rsUsina.Item("vlr_dessem") Is DBNull.Value, "", rsUsina.Item("vlr_dessem").ToString())
                    usina.ValorSugerido = IIf(rsUsina.Item("vlr_sugerido") Is DBNull.Value, "", rsUsina.Item("vlr_sugerido").ToString())
                    usina.Flg_SugestaoAgente = IIf(rsUsina.Item("Flg_SugestaoAgente") Is DBNull.Value, "", rsUsina.Item("Flg_SugestaoAgente").ToString())
                    usina.Flg_Aprovado = IIf(rsUsina.Item("flg_aprovado") Is DBNull.Value, "", rsUsina.Item("flg_aprovado").ToString())
                    usina.ExisteRegistrosPatamares = IIf(String.IsNullOrEmpty(rsUsina.Item("Descricao").ToString()), False, True)

                    listaUsina.Add(usina)
                Loop

                rsUsina.Close()
                rsUsina = Nothing

                For Each usina As UsinaComentarioDESSEM_DTO In listaUsina

                    If strCodUsina <> usina.CodUsina Then
                        'Cria uma nova coluna na tabela
                        objCel = New TableCell
                        objCel.Wrap = False
                        objCel.Width = objTamanho.Pixel(64)
                        objCel.Height = objTamanho.Pixel(17)
                        objCel.HorizontalAlign = HorizontalAlign.Center
                        objCel.Font.Bold = True

                        tblGeracao.Rows(0).Width = objTamanho.Pixel(tblGeracao.Rows(0).Width.Value + 64)
                        tblGeracao.Width = objTamanho.Pixel(tblGeracao.Width.Value + 64)


                        'If Trim(rsUsina("codusina")) = Trim(cboUsina.SelectedItem.Text) Then
                        '    objCel.ForeColor = System.Drawing.Color.Red
                        'End If
                        objCel.BackColor = RetornaCor(usina.Flg_SugestaoAgente, usina.Flg_Aprovado, usina.ExisteRegistrosPatamares)
                        objCel.Text = usina.CodUsina
                        objCel.ColumnSpan = 2

                        tblGeracao.Rows(0).Controls.Add(objCel)
                        strCodUsina = usina.CodUsina
                        intI = intI + 1
                        intPat = 1
                        intLin = 2

                        'Colunas ColSapn de usina
                        objCel = New TableCell
                        objCel.BackColor = System.Drawing.Color.Beige
                        objCel.Font.Bold = True
                        objCel.Width = objTamanho.Pixel(100)
                        objCel.Height = objTamanho.Pixel(17)
                        objCel.HorizontalAlign = HorizontalAlign.Center
                        objCel.Text = "DESSEM"
                        tblGeracao.Rows(1).Controls.Add(objCel)

                        objCel = New TableCell
                        objCel.Text = "Sugerido"
                        objCel.BackColor = System.Drawing.Color.Beige
                        objCel.Font.Bold = True
                        objCel.Width = objTamanho.Pixel(100)
                        objCel.Height = objTamanho.Pixel(17)
                        objCel.HorizontalAlign = HorizontalAlign.Center
                        tblGeracao.Rows(1).Controls.Add(objCel)

                    End If

                    'Inseri as celulas com os valores das usinas
                    objCel = New TableCell
                    objCel.Wrap = False
                    objCel.Width = objTamanho.Pixel(64)

                    If Not String.IsNullOrEmpty(usina.ValorDESSEM) Then
                        objCel.Text = usina.ValorDESSEM
                        dblMedia = dblMedia + Double.Parse(usina.ValorDESSEM)
                    Else
                        objCel.Text = 0
                    End If

                    tblGeracao.Rows(intLin).Width = objTamanho.Pixel(tblGeracao.Rows(intLin).Width.Value + 63)
                    tblGeracao.Rows(intLin).Controls.Add(objCel)

                    objCel = New TableCell
                    objCel.Wrap = False
                    objCel.Width = objTamanho.Pixel(64)
                    objCel.Height = objTamanho.Pixel(17)

                    If Not String.IsNullOrEmpty(usina.ValorSugerido) Then
                        objCel.Text = usina.ValorSugerido
                    Else
                        objCel.Text = ""
                    End If

                    tblGeracao.Rows(intLin).Width = objTamanho.Pixel(tblGeracao.Rows(intLin).Width.Value + 63)
                    tblGeracao.Rows(intLin).Controls.Add(objCel)

                    'Dim patamar = rsUsina.Item("patamar")

                    intLin = intLin + 1
                    intPat = intPat + 1

                    If intLin = 50 Then
                        Dim intLinhaNula As Integer = 3

                        objCel = New TableCell
                        objCel.Wrap = False
                        objCel.Width = objTamanho.Pixel(64)
                        objCel.Height = objTamanho.Pixel(17)
                        objCel.Text = Trim(Str(dblMedia)) '/2
                        tblGeracao.Rows(intLin).Width = objTamanho.Pixel(tblGeracao.Rows(intLin).Width.Value + 64)
                        tblGeracao.Rows(intLin).Controls.Add(objCel)
                        tblGeracao.Rows(intLin).Cells(1).Text = Trim(Str(Val(tblGeracao.Rows(intLin).Cells(1).Text) + dblMedia))

                        objCel = New TableCell
                        objCel.Wrap = False
                        objCel.Width = objTamanho.Pixel(64)
                        objCel.Height = objTamanho.Pixel(17)
                        objCel.Text = Trim(Str(Int(dblMedia / 48)))
                        tblGeracao.Rows(intLin + 1).Width = objTamanho.Pixel(tblGeracao.Rows(intLin + 1).Width.Value + 64)
                        tblGeracao.Rows(intLin + 1).Controls.Add(objCel)
                        tblGeracao.Rows(intLin + 1).Cells(1).Text = Trim(Str(Val(tblGeracao.Rows(intLin + 1).Cells(1).Text) + Int(dblMedia / 48)))

                        'tblGeracao.Rows(intLin).Cells(intLinhaNula).Text = "-"
                        'tblGeracao.Rows(intLin + 1).Cells(intLinhaNula).Text = "-"

                        objCel = New TableCell
                        objCel.BackColor = System.Drawing.Color.Gainsboro
                        objCel.Wrap = False
                        objCel.Width = objTamanho.Pixel(64)
                        objCel.Height = objTamanho.Pixel(17)
                        objCel.Text = "-"
                        objCel.HorizontalAlign = HorizontalAlign.Center
                        tblGeracao.Rows(intLin).Width = objTamanho.Pixel(tblGeracao.Rows(intLin).Width.Value + 64)
                        tblGeracao.Rows(intLin).Controls.Add(objCel)

                        objCel = New TableCell
                        objCel.BackColor = System.Drawing.Color.Gainsboro
                        objCel.Wrap = False
                        objCel.Width = objTamanho.Pixel(64)
                        objCel.Height = objTamanho.Pixel(17)
                        objCel.Text = "-"
                        objCel.HorizontalAlign = HorizontalAlign.Center
                        tblGeracao.Rows(intLin + 1).Width = objTamanho.Pixel(tblGeracao.Rows(intLin).Width.Value + 64)
                        tblGeracao.Rows(intLin + 1).Controls.Add(objCel)

                        dblMedia = 0
                    End If
                Next

                For intI = 2 To 50
                    dblMedia = 0
                    For intJ = 2 To tblGeracao.Rows(intI).Cells.Count - 1
                        dblMedia = dblMedia + Val(tblGeracao.Rows(intI).Cells(intJ).Text)
                        intJ = intJ + 1
                    Next

                    tblGeracao.Rows(intI).Cells(1).Text = dblMedia '/ 2

                Next

                Atualizar_Dados_Usina()

#Region "TEXTAREA"
                Dim indiceColunaUsina As Integer = 0
                If Not String.IsNullOrEmpty(UsinaDropDown.SelectedValue) Then

                    Dim usina As String = UsinaDropDown.SelectedItem.Text.Split("-").GetValue(0).ToString().Trim()
                    Dim usinaSelecionada As String = ""

                    For intJ = 2 To tblGeracao.Rows(0).Cells.Count - 1
                        usinaSelecionada = tblGeracao.Rows(0).Cells(intJ).Text.Trim()

                        If usina.Equals(usinaSelecionada) Then
                            Exit For
                        Else
                            indiceColunaUsina += 1
                        End If
                    Next
                End If

                Dim valoresLote As String = ObterValoresLote()
                AplicarCaixaTextoPatamares(valoresLote, indiceColunaUsina)
#End Region

                dtgEnvio.Visible = False
                tblGeracao.Visible = True
            Else
                Atualizar_Dados_Usina()
                dtgEnvio.Visible = True
                tblGeracao.Visible = False
            End If

            PopularListBoxUsina()
            ValidaPrazoParaSalvarDescricao()

            lblMsg.Visible = False
        Catch ex As Exception
            lblMsg.Visible = True
            'AlertaMensagem("Não foi possível acessar a Base de Dados. Ocorreu o seguinte erro: " & ex.Message)
        Finally

            If Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If

        End Try
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

    Private Sub ObterAnaliseSugestao()

        ComSugestaoListBox.Items.Clear()
        SemSugestaoListBox.Items.Clear()

        Dim selecionado As String = UsinaDropDown.SelectedValue
        Dim existeItemSelecionado As Boolean = False

        UsinaDropDown.Items.Clear()

        UsinaDropDown.Items.Add(New WebControls.ListItem("", ""))
        UsinaDropDown.SelectedIndex = 0


        Dim comando As String = " select  
                            Max(id_comentario) as id_comentario, 
                            Trim(cod_usina) as Cod_Usina, 
                            flg_sugestaoagente, 
                            Trim(u.codusina) + ' - ' +  u.nomusina as DescricaoUsina 
                            from tb_comentario_dessem t " +
                          " join Usina u on u.codusina = t.cod_usina " +
                          " where (t.id_insumo " + IIf(InsumoDropDown.SelectedValue = "", " is null or t.id_insumo = ' ' ", " = " + InsumoDropDown.SelectedValue) + ") " +
                          " and t.dat_pdp = '" + DataPdpDropDown.SelectedValue + "'" +
                          " and u.codempre = '" + cboEmpresa.SelectedValue + "'
                          group by cod_usina, flg_sugestaoagente, u.codusina, u.nomusina  
                          order by 4"

        If Conn.State <> ConnectionState.Open Then
            Conn.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
            Conn.Open()
        End If

        Cmd.Connection = Conn

        Cmd.Parameters.Clear()
        Cmd.CommandText = comando

        Dim cmdDR As SqlDataReader = Cmd.ExecuteReader

        Do While cmdDR.Read
            If (cmdDR("flg_sugestaoagente").ToString() = "2") Then
                SemSugestaoListBox.Items.Add(New System.Web.UI.WebControls.ListItem(cmdDR("DescricaoUsina"), cmdDR("id_comentario")))
            End If
            If (cmdDR("flg_sugestaoagente").ToString() = "3") Then
                ComSugestaoListBox.Items.Add(New System.Web.UI.WebControls.ListItem(cmdDR("DescricaoUsina"), cmdDR("id_comentario")))
                UsinaDropDown.Items.Add(New System.Web.UI.WebControls.ListItem(cmdDR("DescricaoUsina"), cmdDR("id_comentario")))

                If cmdDR("id_comentario").ToString() = selecionado Then
                    existeItemSelecionado = True
                End If

            End If

            Dim itemRemove As ListItem = UsinaListBox.Items.FindByValue(cmdDR("cod_usina"))

            If (Not IsNothing(itemRemove)) Then
                UsinaListBox.Items.Remove(itemRemove)
            End If

        Loop

        If existeItemSelecionado Then
            UsinaDropDown.SelectedValue = selecionado
        End If

        cmdDR.Close()
    End Sub

    Private Sub IncluirComentarioDessem(sender As Object, e As EventArgs) Handles btnIncluirComSugestao.ServerClick, btnIncluirSemSugestao.ServerClick

        If Not Me.ValidaPrazoParaSalvarDescricao() Then
            AlertaMensagem("Fora do prazo para salvar o arquivo.")
            Exit Sub
        End If

        Dim objTrans As SqlTransaction

        If Conn.State <> ConnectionState.Open Then
            Conn.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
            Conn.Open()
        End If

        Cmd.Connection = Conn

        objTrans = Conn.BeginTransaction()
        Cmd.Transaction = objTrans

        Dim btn As System.Web.UI.HtmlControls.HtmlButton = sender

        Dim flgSugestaoAgente As String = btn.Attributes.Item("value")

        For Each item As ListItem In UsinaListBox.Items

            If item.Selected Then

                Dim idInsumo As String = InsumoDropDown.SelectedValue
                Dim strCodUsina As String = item.Value
                Dim strDataPdp As String = DataPdpDropDown.SelectedValue

                Cmd = RemoverComentariosDESSEM(strDataPdp, strCodUsina, idInsumo, Cmd)

                Cmd.Parameters.Clear()
                Cmd.CommandText = ""
                EVT = "N"

                Cmd.CommandText = "INSERT INTO tb_comentario_dessem (   id_insumo , dat_pdp, dsc_comentario, cod_usina, flg_sugestaoagente, flg_evt) " +
                                  " VALUES(" + IIf(idInsumo <> String.Empty, idInsumo, "' '") + ", '" + strDataPdp + "', '', '" + strCodUsina + "', " + flgSugestaoAgente + ",'" + EVT + "')"

                Cmd.ExecuteNonQuery()
            End If
        Next

        Cmd.Transaction.Commit()

        If Conn.State = ConnectionState.Open Then
            Conn.Close()
        End If

        PopularListBoxUsina()
        Atualizar_Dados_Usina()
        Atualiza_Table_Usinas()
    End Sub

    Private Sub ExcluirComentarioDessem(sender As Object, e As EventArgs) Handles btnExcluirComSugestao.ServerClick, btnExcluirSemSugestao.ServerClick

        If Not Me.ValidaPrazoParaSalvarDescricao() Then
            AlertaMensagem("Fora do prazo para salvar o arquivo.")
            Exit Sub
        End If

        If Conn.State <> ConnectionState.Open Then
            Conn.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
            Conn.Open()
        End If

        Try
            Cmd.Connection = Conn

            'Cmd.Transaction = Conn.BeginTransaction()

            Dim btn As System.Web.UI.HtmlControls.HtmlButton = sender

            Dim lista As ListBox

            Dim flgSugestaoAgente As String = btn.Attributes.Item("value")

            If flgSugestaoAgente = "3" Then
                lista = ComSugestaoListBox
            ElseIf flgSugestaoAgente = "2" Then
                lista = SemSugestaoListBox
            End If

            Dim lista_IDComentarios As String = ""
            Dim virgula As String = ""
            Dim existeSelecionado As Boolean = False
            For Each item As ListItem In lista.Items

                If item.Selected Then
                    Cmd.Parameters.Clear()

                    Cmd.CommandText =
                        " delete from tb_comentario_dessem_patamar 
                        Where id_comentario In (Select id_comentario from tb_comentario_dessem where flg_aprovado is null and id_comentario = " + item.Value + ")"
                    Cmd.ExecuteNonQuery()

                    Cmd.CommandText =
                        " delete from tb_comentario_dessem 
                        where flg_aprovado is null and id_comentario = " + item.Value
                    Cmd.ExecuteNonQuery()

                    lista_IDComentarios += virgula + item.Value
                    virgula = ","

                    existeSelecionado = True
                End If

            Next

            If existeSelecionado Then
                Cmd.CommandText =
                            "Select distinct Trim(c.Cod_Usina) + '-' + Trim(u.NomUsina) as Descricao From tb_comentario_dessem  c
                            Inner join Usina u on (u.codUsina = c.Cod_usina)
                            Where id_comentario in (" + lista_IDComentarios + ") and flg_aprovado is not null order by 1"

                Dim drComentario_ComDecisaoONS As SqlDataReader = Cmd.ExecuteReader

                Dim listaCodUsinas_ComDecicaoONS As String = ""
                Do While drComentario_ComDecisaoONS.Read
                    listaCodUsinas_ComDecicaoONS += drComentario_ComDecisaoONS("Descricao") + "\n"
                Loop

                If Not String.IsNullOrEmpty(listaCodUsinas_ComDecicaoONS) Then
                    AlertaMensagem("As usinas: \n" + listaCodUsinas_ComDecicaoONS + " não podem ser movidas porque já foram analisadas pelo ONS.")
                End If

            End If

            'Cmd.Transaction.Commit()
        Catch ex As Exception
            'Cmd.Transaction.Rollback()
            AlertaMensagem(ex.Message)
        End Try

        If Conn.State = ConnectionState.Open Then
            Conn.Close()
        End If

        PopularListBoxUsina()
        Atualizar_Dados_Usina()
        Atualiza_Table_Usinas()
    End Sub

    Private Function ValidaPrazoParaSalvarDescricao() As Boolean
        Dim dtPdp As Date
        Dim podeSalvar As Boolean = False
        btnImgSalvar.Visible = False

        If Not String.IsNullOrEmpty(DataPdpDropDown.SelectedItem.Value) And Not String.IsNullOrEmpty(cboEmpresa.SelectedItem.Value) Then

            dtPdp = Date.ParseExact(DataPdpDropDown.SelectedValue, "yyyyMMdd", Globalization.CultureInfo.InvariantCulture)
            Dim dataPdp As String = dtPdp.ToString("dd/MM/yyyy")

            If Not Me.FactoryBusiness.ComentarioDESSEMBusiness.ValidaLimiteEnvioComentarioDESSEM(cboEmpresa.SelectedItem.Value, dataPdp) Then
                btnImgSalvar.Visible = False
                'Label1.Visible = False

                btnIncluirComSugestao.Attributes.Add("Disabled", "true")
                btnExcluirComSugestao.Attributes.Add("Disabled", "true")
                btnIncluirSemSugestao.Attributes.Add("Disabled", "true")
                btnExcluirSemSugestao.Attributes.Add("Disabled", "true")

            Else
                btnImgSalvar.Visible = True
                podeSalvar = True
                'Label1.Visible = True

                btnIncluirComSugestao.Attributes.Remove("Disabled")
                btnExcluirComSugestao.Attributes.Remove("Disabled")
                btnIncluirSemSugestao.Attributes.Remove("Disabled")
                btnExcluirSemSugestao.Attributes.Remove("Disabled")
            End If
        Else
            btnImgSalvar.Visible = False
            'Label1.Visible = False

            btnIncluirComSugestao.Attributes.Add("Disabled", "true")
            btnExcluirComSugestao.Attributes.Add("Disabled", "true")
            btnIncluirSemSugestao.Attributes.Add("Disabled", "true")
            btnExcluirSemSugestao.Attributes.Add("Disabled", "true")
        End If

        ValidaPrazoParaSalvarDescricao = podeSalvar
    End Function

    Private Sub PreencherComentario()

        TxtComentarioONS.Text = ""
        lblComentarioONS.Visible = False
        TxtComentarioONS.Visible = False
        TxtComentarioONS.Enabled = False

        If Not String.IsNullOrEmpty(UsinaDropDown.SelectedValue) Then

            TxtComentarioONS.Text = ObterComentarioDESSEMOns(UsinaDropDown.SelectedValue)

            If Not String.IsNullOrEmpty(TxtComentarioONS.Text) Then
                lblComentarioONS.Visible = True
                TxtComentarioONS.Visible = True
            End If
        End If

    End Sub



    Private Sub SalvarComentario()
        Dim indiceTabela As Integer
        Dim patamar As Integer
        Dim dataGrid As DataTable
        Dim objTrans As SqlTransaction
        Dim idInsumo As String
        Dim dataPdp As String
        Dim codUsina As String
        Dim vlDessem As String
        Dim descricao As String
        Dim strDataPdp As String = ""
        Dim strCodUsina As String = ""
        Dim IdComentario As String
        Dim listaPatamar As New List(Of Integer)

        ' Fazer a pergunta se confirma ou não a transação
        Try
            ' Verificar se a descrição contém aspas simples
            If TxtDescricao.Text.Contains("'") Then
                AlertaMensagem($"O texto na descrição não permite aspas simples, por favor retirar do texto.")
                Exit Sub
            End If

            If tblGeracao.Visible = True And Not String.IsNullOrEmpty(TxtDescricao.Text.Trim()) And TxtDescricao.Text.Trim().Length <= 200 Then

                listaPatamar = AplicarValoresSugeridos_EmLote()

                If IsNothing(listaPatamar) Then
                    AlertaMensagem("O campo Sugerido é de preenchimento obrigatório, verifique.")
                    Exit Sub
                End If

                If Conn.State <> ConnectionState.Open Then
                    Conn.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
                    Conn.Open()
                End If

                Cmd.Connection = Conn

                objTrans = Conn.BeginTransaction()
                Cmd.Transaction = objTrans

                IdComentario = UsinaDropDown.SelectedItem.Value
                strCodUsina = UsinaDropDown.SelectedItem.Text.Split("-")(0).Trim()
                strDataPdp = DataPdpDropDown.SelectedItem.Value
                descricao = TxtDescricao.Text

                idInsumo = InsumoDropDown.SelectedValue
                dataPdp = DataPdpDropDown.SelectedValue
                codUsina = UsinaDropDown.SelectedValue

                If ChkEVT.Checked Then
                    EVT = "S"
                Else
                    EVT = "N"
                End If

                Cmd.CommandText = $"UPDATE tb_comentario_dessem
                                    SET dsc_comentario= '{descricao}', flg_evt = '{EVT}' 
                                    WHERE id_comentario={IdComentario};"

                Dim param As Common.DbParameter = Cmd.CreateParameter()
                param.ParameterName = "@descricao"
                param.Value = TxtDescricao.Text
                param.DbType = DbType.String
                param.Direction = ParameterDirection.Input
                Cmd.Parameters.Add(param)

                Cmd.ExecuteNonQuery()
                Cmd.Parameters.Clear()

                Cmd.CommandText = "delete from tb_comentario_dessem_patamar where id_comentario =" + IdComentario
                Cmd.ExecuteNonQuery()
                Cmd.Parameters.Clear()

                If listaPatamar.Count > 0 Then
                    patamar = 1
                    For Each item As Integer In listaPatamar
                        'Dim pat As String = HoraParaInt(item.ToString()).ToString() 
                        Dim pat As String = patamar.ToString()

                        Dim sqlVal_DESSEM As String = $"Select isnull(ValDespaPRE, 0) as Val_DESSEM from Despa Where
                                                        CodUsina = '{strCodUsina}'
                                                        and datpdp = '{strDataPdp}'
                                                        and intdespa = '{pat}'"

                        Cmd.CommandText = sqlVal_DESSEM
                        vlDessem = Cmd.ExecuteScalar()

                        If IsNothing(vlDessem) Then
                            vlDessem = "0"
                        End If

                        Cmd.Parameters.Clear()
                        Cmd.CommandText = ""
                        Cmd.CommandText = "insert into tb_comentario_dessem_patamar 
                                      ( id_comentario, num_patamar, val_sugerido, val_dessem) 
                                      values(" + IdComentario + ", '" + pat + "'," +
                                      " @valSugerido , " +
                                      " @valDessem )"

                        'Tratamento Valor Sugerido
                        Dim paramValSugerido As Common.DbParameter = Cmd.CreateParameter()
                        paramValSugerido.ParameterName = "@valSugerido"
                        paramValSugerido.Value = ""
                        paramValSugerido.DbType = DbType.String
                        paramValSugerido.Direction = ParameterDirection.Input

                        Dim valorSugerido As String = item.ToString()
                        If String.IsNullOrEmpty(valorSugerido.Trim()) Then
                            paramValSugerido.Value = DBNull.Value
                        Else
                            paramValSugerido.Value = valorSugerido
                        End If

                        'Tratamento Valor DESSEM
                        Dim paramValDessem As Common.DbParameter = Cmd.CreateParameter()
                        paramValDessem.ParameterName = "@valDessem"
                        paramValDessem.Value = ""
                        paramValDessem.DbType = DbType.String
                        paramValDessem.Direction = ParameterDirection.Input

                        Dim valorDessem As String = vlDessem
                        If String.IsNullOrEmpty(valorDessem) Then
                            paramValDessem.Value = "0"
                        Else
                            paramValDessem.Value = valorDessem
                        End If

                        Cmd.Parameters.Add(paramValSugerido)
                        Cmd.Parameters.Add(paramValDessem)

                        Cmd.ExecuteNonQuery()
                        patamar = patamar + 1
                    Next
                End If

                Cmd.Transaction.Commit()
                ChkEVT.Enabled = False
                ChkEVT.Checked = False
                EVT = ""
                UsinaDropDown.SelectedIndex = 0
                Atualizar_Dados_Usina()
                Atualiza_Table_Usinas()
            Else
                Dim valoresLote As String = ObterValoresLote()

                If (TxtDescricao.Text.Trim().Length > 200) Then
                    AlertaMensagem("Descrição está maior que 200 caracteres permitidos, verifique.")
                End If

                If (String.IsNullOrEmpty(TxtDescricao.Text.Trim())) Then
                    AlertaMensagem("O campo Descrição é de preenchimento obrigatório, verifique.")
                End If

                Atualiza_Table_Usinas()

            End If
            'ChkEVT.Enabled = False
            'ChkEVT.Checked = False
            'EVT = ""
        Catch ex As Exception
            If Not Cmd.Transaction Is Nothing Then
                Cmd.Transaction.Rollback()
            End If

            AlertaMensagem($"Erro ao gravar os dados. Favor contatar o administrador do sistema. Erro: {ex.Message.Replace("'", "")}")
        Finally

            If Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If
        End Try
    End Sub

    Protected Sub btnImgSalvar_Click(sender As Object, e As ImageClickEventArgs) Handles btnImgSalvar.Click

        'Valida Prazo para envio
        If Not Me.ValidaPrazoParaSalvarDescricao() Then
            AlertaMensagem("Fora do prazo para salvar o arquivo.")
        Else
            SalvarComentario()
        End If

    End Sub

    Protected Sub DataPdpDropDown_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DataPdpDropDown.SelectedIndexChanged
        Atualiza_Table_Usinas()
    End Sub

    Protected Sub InsumoDropDown_SelectedIndexChanged(sender As Object, e As EventArgs) Handles InsumoDropDown.SelectedIndexChanged
        Atualiza_Table_Usinas()
    End Sub

    Protected Sub UsinaDropDown_SelectedIndexChanged(sender As Object, e As EventArgs) Handles UsinaDropDown.SelectedIndexChanged
        Atualiza_Table_Usinas()

        Dim rsDataHeader As SqlDataReader

        Cmd.Connection = Conn

        If Not String.IsNullOrEmpty(UsinaDropDown.SelectedValue) And
              Not String.IsNullOrEmpty(InsumoDropDown.SelectedValue) And
              Not String.IsNullOrEmpty(DataPdpDropDown.SelectedValue) Then

            If Conn.State <> ConnectionState.Open Then
                Conn.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
                Conn.Open()
            End If
            Dim codusi As String = UsinaDropDown.SelectedItem.Text.Split("-").GetValue(0).ToString().Trim()
            Dim codemp = Session("strCodEmpre")

            Cmd.CommandText = "Select tpusina_id from usina where codusina = '" & codusi & "' and codempre = '" & codemp & "'"
            rsDataHeader = Cmd.ExecuteReader

            If Not IsNothing(rsDataHeader) Then
                rsDataHeader.Read()
                TPusina = Trim(rsDataHeader("tpusina_id"))
                If (TPusina = "UHE" Or TPusina = "PCH") And TxtDescricao.Enabled Then
                    ChkEVT.Enabled = True

                Else
                    ChkEVT.Enabled = False
                End If
            Else
                ChkEVT.Enabled = False
            End If
            rsDataHeader.Close()
        Else
            ChkEVT.Enabled = False
        End If
    End Sub
End Class
