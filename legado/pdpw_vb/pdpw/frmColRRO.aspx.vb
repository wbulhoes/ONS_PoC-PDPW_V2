Imports System.Data.SqlClient
Imports System.IO

Partial Class frmColRRO

    Inherits BaseWebUi
#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Dim _listaUsinaAprovada As String = ""

    Public Property ListaUsinaAprovada() As String
        Get
            If (_listaUsinaAprovada = "") Then
                _listaUsinaAprovada = ViewState("ListaUsinaAprovada")
            End If
            Return _listaUsinaAprovada
        End Get
        Set(ByVal value As String)
            _listaUsinaAprovada = value
            ViewState.Add("ListaUsinaAprovada", _listaUsinaAprovada)
        End Set
    End Property

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        'objMenu = CType(Session("onsmenu"), OnsWebControls.OnsMenu)

        If Not Page.IsPostBack Then

            ObterUsinasAutorizadas()

            Dim intI As Integer
            Dim Conn As SqlConnection = New SqlConnection
            Dim Cmd As SqlCommand = New SqlCommand
            If Session("datEscolhida") = Nothing Then
                'Inicializa a variável com data do próximo
                Session("datEscolhida") = Now.AddDays(1)
            End If
            Cmd.Connection = Conn

            Try
                Conn.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
                Conn.Open()
                Cmd.CommandText = "Select datpdp " &
                                "From pdp " &
                                "Order By datpdp Desc"
                Dim rsData As SqlDataReader = Cmd.ExecuteReader
                intI = 1
                Dim objItem As WebControls.ListItem
                objItem = New WebControls.ListItem
                objItem.Text = ""
                objItem.Value = "0"
                cboData.Items.Add(objItem)
                Do While rsData.Read
                    objItem = New System.Web.UI.WebControls.ListItem
                    objItem.Text = Mid(rsData("datpdp"), 7, 2) & "/" & Mid(rsData("datpdp"), 5, 2) & "/" & Mid(rsData("datpdp"), 1, 4)
                    objItem.Value = Mid(rsData("datpdp"), 7, 2) & "/" & Mid(rsData("datpdp"), 5, 2) & "/" & Mid(rsData("datpdp"), 1, 4)
                    cboData.Items.Add(objItem)
                    If Trim(cboData.Items(intI).Value) = Format(Session("datEscolhida"), "dd/MM/yyyy") Then
                        cboData.SelectedIndex = intI
                    End If
                    intI = intI + 1
                Loop

                rsData.Close()
                rsData = Nothing
                Cmd.Connection.Close()
                Conn.Close()
                PreencheComboEmpresaPOP(AgentesRepresentados, cboEmpresa, Session("strCodEmpre"))
                If cboData.SelectedIndex > 0 Then
                    cboData_SelectedIndexChanged(sender, e)
                End If
            Catch
                Session("strMensagem") = "Não foi possível acessar a Base de Dados. Ocorreu o seguinte erro: " & Chr(10) & Err.Description
                If Conn.State = ConnectionState.Open Then
                    Conn.Close()
                End If
                Response.Redirect("frmMensagem.aspx")
            Finally
                If Conn.State = ConnectionState.Open Then
                    Conn.Close()
                End If
            End Try
        End If
    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        If SessaoAtiva(Page.Session) Then
            MyBase.Render(writer)
            'objMenu.RenderControl(writer)
        End If
    End Sub

    Private Sub ObterUsinasAutorizadas()

        Dim caminhoArquivo As String = Server.MapPath("~") & "\Temp\UsinasAutorizadas.txt" 'ConfigurationManager.AppSettings.Get("PathUsinaAutorizada")
        Dim texto As String = ""
        texto = File.ReadAllText(caminhoArquivo)

        ListaUsinaAprovada = "'" & texto.Replace(",", "','") & "'"

    End Sub

    Private Sub cboEmpresa_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboEmpresa.SelectedIndexChanged
        Dim intI As Integer
        Dim Conn As SqlConnection = New SqlConnection
        Dim Cmd As SqlCommand = New SqlCommand
        Cmd.Connection = Conn
        Dim strCodUsina As String
        'tblTexto.Visible = False
        divValor.Visible = False
        btnSalvar.Visible = False
        If cboEmpresa.SelectedIndex > 0 Then
            Session("strCodEmpre") = cboEmpresa.SelectedItem.Value
        End If

        Try
            Cmd.CommandText = "Select d.codusina, " &
                              "       u.ordem " &
                              "From tb_rro d, " &
                              "     usina u " &
                              " inner join tpusina " &
                              " On u.tpusina_id = tpusina.tpusina_id " &
                              "    And tpusina.id_tpgeracao = 2 " &
                              "Where u.codempre = '" & Session("strCodEmpre") & "' And " &
                              "      u.codusina = d.codusina And " &
                              "      d.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' " &
                              "      And u.flg_recebepdpage = 'S' "
            If (ListaUsinaAprovada <> "") Then
                Cmd.CommandText = Cmd.CommandText & "      and u.codusina in (" + ListaUsinaAprovada + ")"
            End If

            Cmd.CommandText = Cmd.CommandText &
                              "Group By d.codusina, " &
                              "         u.ordem " &
                              "Order By u.ordem, " &
                              "         d.codusina"
            Conn.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
            Conn.Open()
            Dim rsUsina As SqlDataReader = Cmd.ExecuteReader
            Dim objItem As System.Web.UI.WebControls.ListItem
            cboUsina.Items.Clear()
            intI = 1
            Do While rsUsina.Read
                If intI = 1 Then
                    cboUsina.Items.Add("Selecione uma Usina")
                End If
                cboUsina.Items.Add(rsUsina.Item("codusina"))
                intI = intI + 1
            Loop
            If intI > 1 Then
                cboUsina.Items.Add("Todas as Usinas")
            End If
            rsUsina.Close()
            rsUsina = Nothing
            Conn.Close()
            PreencheTable()

            'Valida Limite de Envio
            Dim lRetorno As Integer = 0
            If Not ValidaLimiteEntradaDados(cboEmpresa.SelectedItem.Value, cboData.SelectedItem.Value, lRetorno, "RRO") Then
                btnSalvar.Visible = False
                cboUsina.Enabled = False

                If lRetorno = 1 Then
                    Response.Write("<SCRIPT>alert('" + strMsgInicioLimiteEnvioDados + "')</SCRIPT>")
                Else
                    Response.Write("<SCRIPT>alert('" + strMsgLimiteEnvioDados + "')</SCRIPT>")
                End If
                Exit Sub
            Else
                btnSalvar.Visible = True
                cboUsina.Enabled = True

            End If
        Catch
            Session("strMensagem") = "Não foi possível acessar a Base de Dados. Ocorreu o seguinte erro: " & Chr(10) & Err.Description
            If Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If
            Response.Redirect("frmMensagem.aspx")
        Finally
            If Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If
        End Try
    End Sub

    Private Sub cboUsina_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboUsina.SelectedIndexChanged
        Dim intI As Integer
        Dim Conn As SqlConnection = New SqlConnection
        Dim Cmd As SqlCommand = New SqlCommand
        Cmd.Connection = Conn
        Try
            If cboUsina.SelectedItem.Text = "Todas as Usinas" Then
                Cmd.CommandText = "Select d.valrrotran, " &
                                "       d.intrro, " &
                                "       d.codusina, " &
                                "       u.ordem " &
                                "From tb_rro d, " &
                                "     usina u " &
                                " inner join tpusina " &
                              " On u.tpusina_id = tpusina.tpusina_id " &
                              "    And tpusina.id_tpgeracao = 2 " &
                                "Where u.codempre = '" & cboEmpresa.SelectedItem.Value & "' And " &
                                "      u.codusina = d.codusina And " &
                                "      d.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' " &
                                "      And u.flg_recebepdpage = 'S' "
                If (ListaUsinaAprovada <> "") Then
                    Cmd.CommandText = Cmd.CommandText & "      and u.codusina in (" + ListaUsinaAprovada + ")"
                End If

                Cmd.CommandText = Cmd.CommandText &
                                  "Order By d.intrro, " &
                                  "         u.ordem, " &
                                  "         d.codusina"
            Else
                Cmd.CommandText = "Select d.valrrotran, " &
                                "       d.intrro " &
                                "From tb_rro d, " &
                                "     usina u " &
                                " inner join tpusina " &
                              " On u.tpusina_id = tpusina.tpusina_id " &
                              "    And tpusina.id_tpgeracao = 2 " &
                                "Where u.codempre = '" & cboEmpresa.SelectedItem.Value & "' And " &
                                "      u.codusina = d.codusina And " &
                                "      d.codusina = '" & cboUsina.SelectedItem.Value & "' And " &
                                "      d.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' " &
                                "      And u.flg_recebepdpage = 'S' "
                If (ListaUsinaAprovada <> "") Then
                    Cmd.CommandText = Cmd.CommandText & "      and u.codusina in (" + ListaUsinaAprovada + ")"
                End If

                Cmd.CommandText = Cmd.CommandText & "Order By d.intrro"
            End If
            Conn.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
            Conn.Open()
            Dim rsUsina As SqlDataReader = Cmd.ExecuteReader

            'Colocando os valores de carga no text para alteração
            Dim objTextArea As HtmlControls.HtmlTextArea
            objTextArea = New HtmlTextArea
            objTextArea.Rows = 48
            objTextArea.ID = "txtValor"
            objTextArea.Attributes.Item("onkeyup") = "RetiraEnter(event)"
            objTextArea.Attributes.Item("runat") = "server"
            If cboUsina.SelectedItem.Text <> "Todas as Usinas" Then
                objTextArea.Attributes.Item("style") = "font-size:XSmall;height:1001px;width:81px;line-height:20px"
            Else
                objTextArea.Attributes.Item("style") = "font-size:XSmall;height:1001px;width:" & (((cboUsina.Items.Count - 2) * 65) + 16) - Int(cboUsina.Items.Count / 2) & "px;line-height:20px"
            End If

            Dim intInstante As Integer = 1
            Dim blnPassou As Boolean = False
            Do While rsUsina.Read
                If blnPassou Then
                    If rsUsina("intrro") <> intInstante Then
                        intInstante = rsUsina("intrro")
                        objTextArea.Value = objTextArea.Value & Chr(13)
                    Else
                        objTextArea.Value = objTextArea.Value & Chr(9)
                    End If
                End If
                If Not IsDBNull(rsUsina.Item("valrrotran")) Then
                    objTextArea.Value = objTextArea.Value & rsUsina.Item("valrrotran")
                Else
                    objTextArea.Value = objTextArea.Value & 0
                End If
                'na primeira passagem não escreve TAB nem ENTER
                blnPassou = True
            Loop
            divValor.Controls.Add(objTextArea)
            divValor.Style.Item("TOP") = "269px"
            If cboUsina.SelectedItem.Text <> "Todas as Usinas" Then
                divValor.Style.Item("LEFT") = Trim(Str(90 + (cboUsina.SelectedIndex * 64))) & "px"
            Else
                divValor.Style.Item("LEFT") = "154px"
            End If

            If Trim(cboUsina.SelectedItem.Text) <> "Selecione uma Usina" Then
                divValor.Visible = True
            Else
                divValor.Visible = False
            End If
            btnSalvar.Visible = True
            rsUsina.Close()
            rsUsina = Nothing
            Cmd.Connection.Close()
            Conn.Close()
            PreencheTable()
        Catch
            Session("strMensagem") = "Não foi possível acessar a Base de Dados. Ocorreu o seguinte erro: " & Chr(10) & Err.Description
            If Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If
            Response.Redirect("frmMensagem.aspx")
        Finally
            If Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If
        End Try
    End Sub

    Private Sub PreencheTable()
        'Usinas
        Dim intI, intJ As Integer
        Dim intLin As Integer
        Dim dblMedia As Double
        Dim objCel As TableCell
        Dim objRow As TableRow
        Dim objTamanho As WebControls.Unit
        Dim Conn As SqlConnection = New SqlConnection
        Dim Cmd As SqlCommand = New SqlCommand
        Cmd.Connection = Conn
        Dim strCodUsina As String
        Try
            Cmd.CommandText = "Select d.codusina, " &
                                "       d.intrro, " &
                                "       d.valrrotran, " &
                                "       u.ordem " &
                                "From tb_rro d, " &
                                "     usina u " &
                                " inner join tpusina " &
                              " On u.tpusina_id = tpusina.tpusina_id " &
                              "    And tpusina.id_tpgeracao = 2 " &
                                "Where u.codempre = '" & Session("strCodEmpre") & "' And " &
                                "      u.codusina = d.codusina And " &
                                "      d.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' " &
                                "      And u.flg_recebepdpage = 'S' "
            If (ListaUsinaAprovada <> "") Then
                Cmd.CommandText = Cmd.CommandText & "      and u.codusina in (" + ListaUsinaAprovada + ")"
            End If

            Cmd.CommandText = Cmd.CommandText &
                                "Order By u.ordem, " &
                                "         d.codusina, " &
                                "         d.intrro"
            Conn.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
            Conn.Open()
            Dim rsUsina As SqlDataReader = Cmd.ExecuteReader
            tblRRO.Rows.Clear()
            objTamanho = New WebControls.Unit

            objRow = New TableRow
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
            tblRRO.Width = objTamanho.Pixel(132)
            tblRRO.Controls.Add(objRow)

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
                objCel.Text = 0
                objCel.BackColor = System.Drawing.Color.Beige
                objCel.Font.Bold = True
                objCel.Width = objTamanho.Pixel(62)
                objRow.Controls.Add(objCel)
                tblRRO.Controls.Add(objRow)
            Next

            objRow = New TableRow
            objRow.Width = objTamanho.Pixel(132)
            objCel = New TableCell
            objCel.Text = "Total"
            objCel.BackColor = System.Drawing.Color.Beige
            objCel.Font.Bold = True
            objCel.Width = objTamanho.Pixel(70)
            objCel.HorizontalAlign = HorizontalAlign.Center
            objRow.Controls.Add(objCel)
            objCel = New TableCell
            objCel.BackColor = System.Drawing.Color.Beige
            objCel.Font.Bold = True
            objCel.Width = objTamanho.Pixel(62)
            objRow.Controls.Add(objCel)
            tblRRO.Controls.Add(objRow)

            objRow = New TableRow
            objRow.Width = objTamanho.Pixel(132)
            objCel = New TableCell
            objCel.Text = "Média"
            objCel.BackColor = System.Drawing.Color.Beige
            objCel.Font.Bold = True
            objCel.Width = objTamanho.Pixel(70)
            objCel.HorizontalAlign = HorizontalAlign.Center
            objRow.Controls.Add(objCel)
            objCel = New TableCell
            objCel.BackColor = System.Drawing.Color.Beige
            objCel.Font.Bold = True
            objCel.Width = objTamanho.Pixel(62)
            objRow.Controls.Add(objCel)
            tblRRO.Controls.Add(objRow)

            intI = 1
            strCodUsina = ""
            dblMedia = 0

            Do While rsUsina.Read
                If strCodUsina <> rsUsina.Item("codusina") Then
                    'Cria uma nova coluna na tabela
                    objCel = New TableCell
                    objCel.Wrap = False
                    objCel.Width = objTamanho.Pixel(64)
                    objCel.Font.Bold = True
                    tblRRO.Rows(0).Width = objTamanho.Pixel(tblRRO.Rows(0).Width.Value + 64)
                    tblRRO.Width = objTamanho.Pixel(tblRRO.Width.Value + 64)

                    If Trim(rsUsina("codusina")) = Trim(cboUsina.SelectedItem.Text) Then
                        objCel.ForeColor = System.Drawing.Color.Red
                    End If
                    objCel.Text = rsUsina("codusina")
                    tblRRO.Rows(0).Controls.Add(objCel)
                    strCodUsina = rsUsina("codusina")
                    intI = intI + 1
                    intLin = 1
                End If
                'Inseri as celulas com os valores das usinas
                objCel = New TableCell
                objCel.Wrap = False
                objCel.Width = objTamanho.Pixel(64)
                If Not IsDBNull(rsUsina.Item("valrrotran")) Then
                    objCel.Text = rsUsina.Item("valrrotran")
                    dblMedia = dblMedia + rsUsina.Item("valrrotran")
                Else
                    objCel.Text = 0
                End If
                tblRRO.Rows(intLin).Width = objTamanho.Pixel(tblRRO.Rows(intLin).Width.Value + 63)
                tblRRO.Rows(intLin).Controls.Add(objCel)
                intLin = intLin + 1
                If intLin = 49 Then
                    'totalizador horizontal
                    objCel = New TableCell
                    objCel.Wrap = False
                    objCel.Width = objTamanho.Pixel(64)
                    objCel.Text = Trim(Str(dblMedia))
                    tblRRO.Rows(intLin).Width = objTamanho.Pixel(tblRRO.Rows(intLin).Width.Value + 64)
                    tblRRO.Rows(intLin).Controls.Add(objCel)
                    tblRRO.Rows(intLin).Cells(1).Text = Trim(Str(Val(tblRRO.Rows(intLin).Cells(1).Text) + dblMedia))

                    'media
                    objCel = New TableCell
                    objCel.Wrap = False
                    objCel.Width = objTamanho.Pixel(64)
                    objCel.Text = Trim(Str(Int(dblMedia / 48)))
                    tblRRO.Rows(intLin + 1).Width = objTamanho.Pixel(tblRRO.Rows(intLin + 1).Width.Value + 64)
                    tblRRO.Rows(intLin + 1).Controls.Add(objCel)
                    tblRRO.Rows(intLin + 1).Cells(1).Text = Trim(Str(Val(tblRRO.Rows(intLin + 1).Cells(1).Text) + Int(dblMedia / 48)))

                    dblMedia = 0
                End If
            Loop

            'totalizador vertical
            For intI = 1 To 50
                dblMedia = 0
                For intJ = 2 To tblRRO.Rows(0).Cells.Count - 1
                    dblMedia = dblMedia + Val(tblRRO.Rows(intI).Cells(intJ).Text)
                Next
                tblRRO.Rows(intI).Cells(1).Text = dblMedia '/ 2
            Next

            rsUsina.Close()
            rsUsina = Nothing
            'Cmd.Connection.Close()
            'Conn.Close()
        Catch
            Session("strMensagem") = "Não foi possível acessar a Base de Dados. Ocorreu o seguinte erro: " & Chr(10) & Err.Description
            If Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If
            Response.Redirect("frmMensagem.aspx")
        Finally
            If Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If
        End Try
    End Sub

    Private Sub cboData_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboData.SelectedIndexChanged
        Try
            If cboData.SelectedIndex <> 0 Then
                Session("datEscolhida") = CDate(cboData.SelectedItem.Value)
            End If
            'PreencheComboEmpresa(Session("usuarID"), cboEmpresa, Session("strCodEmpre"))
            If cboEmpresa.SelectedIndex > 0 Then
                cboEmpresa_SelectedIndexChanged(sender, e)
            End If
        Catch
            Session("strMensagem") = "Não foi possível acessar a Base de Dados. Ocorreu o seguinte erro: " & Chr(10) & Err.Description
            Response.Redirect("frmMensagem.aspx")
        End Try
    End Sub

    Private Sub btnSalvar_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSalvar.Click
        Dim intI As Integer
        Dim intJ As Integer
        Dim intColAtual As Integer
        Dim intColAnterior As Integer
        Dim intQtdReg As Integer
        Dim intCol As Integer
        Dim intValor As Integer
        Dim strValor As String

        'Dim objPermissao As OnsClasses.OnsInterfaceUnica.OnsPermissao
        'objPermissao = New OnsClasses.OnsInterfaceUnica.OnsPermissao("UPD", "PDPColGera", UsuarID)


        Dim Conn As SqlConnection = New SqlConnection
        Dim Cmd As SqlCommand = New SqlCommand
        Cmd.Connection = Conn

        Dim sql As String = ""

        'Verifica se o usuário tem permissão para salvar os registros
        If EstaAutorizado Then

            Dim datAtual As Date = Now
            strValor = Page.Request.Form("_ctl0:ContentPlaceHolder1:txtValor")

            Try
                'Atualiza a grid
                If strValor = "" Then
                    'Quando o txtValor estiver em branco, branqueia tabela e a grid
                    If cboUsina.SelectedItem.Text <> "Todas as Usinas" Then
                        For intI = 1 To 48
                            'Atualiza na BDT a Coluna Alterada
                            'Cmd.CommandText
                            sql += "Update tb_rro " &
                                                "Set valrrotran = 0 " &
                                                "Where datPdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' And " &
                                                "      codusina = '" & Trim(cboUsina.SelectedItem.Value) & "' And " &
                                                "      intrro = " & intI & ";"
                            'Cmd.ExecuteNonQuery()
                        Next
                    Else
                        For intI = 1 To 48
                            For intJ = 1 To cboUsina.Items.Count - 2
                                'Atualiza na BDT TODAS as Colunas Alteradas
                                'Cmd.CommandText
                                sql += "Update tb_rro " &
                                                "Set valrrotran = 0 " &
                                                "Where datPdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' And " &
                                                "      codusina = '" & Trim(cboUsina.Items(intJ).Value) & "' And " &
                                                "      intrro = " & intI & ";"
                                'Cmd.ExecuteNonQuery()
                            Next
                        Next
                    End If
                Else
                    If cboUsina.SelectedItem.Text <> "Todas as Usinas" Then
                        'Somente uma USINA
                        intI = 1
                        intColAnterior = 1
                        intColAtual = InStr(intColAnterior, strValor, Chr(13), CompareMethod.Binary)
                        For intI = 1 To 48
                            If intColAtual <> 0 Then
                                If Mid(strValor, intColAnterior, (intColAtual - intColAnterior) + 1) <> "" Then
                                    'Atualiza na BDT a Coluna Alterada
                                    intValor = Val(Mid(strValor, intColAnterior, (intColAtual - intColAnterior) + 1))
                                End If
                                intColAnterior = intColAtual
                            ElseIf intColAtual = 0 And Mid(strValor, intColAnterior) <> "" Then
                                'Não tem ENTER (chr(13)) no final do texto
                                intValor = Val(Mid(strValor, intColAnterior))
                                intColAnterior = intColAnterior + Trim(Mid(strValor, intColAnterior)).Length
                            Else
                                intValor = 0
                            End If
                            'Cmd.CommandText
                            sql += "Update tb_rro " &
                                                "Set valrrotran = " & intValor & " " &
                                                "Where datPdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' And " &
                                                "      codusina = '" & cboUsina.SelectedItem.Value.Trim() & "' And " &
                                                "      intrro = " & intI & ";"
                            'Cmd.ExecuteNonQuery()
                            intColAtual = InStr(intColAnterior + 1, strValor, Chr(13), CompareMethod.Binary)
                        Next
                    Else

                        Dim strLinha As String
                        Dim intCelula, intFim, intUsina As Integer

                        strLinha = ""
                        intColAtual = 1
                        For intI = 1 To 48

                            'Todas as USINA
                            intUsina = 1
                            If InStr(intColAtual, strValor, Chr(13), CompareMethod.Binary) <> 0 Then
                                strLinha = Mid(strValor, intColAtual, ((InStr(intColAtual, strValor, Chr(13), CompareMethod.Binary)) - intColAtual) + 1)
                            Else
                                strLinha = Mid(strValor, intColAtual)
                            End If

                            If InStr(strLinha, Chr(13)) <> 0 Then
                                strLinha = Left(strLinha, InStr(strLinha, Chr(13)) - 1)
                            End If
                            intColAtual = intColAtual + Len(Trim(strLinha)) + 2
                            intColAnterior = 1
                            intCelula = 2
                            Do While InStr(intColAnterior, strLinha, Chr(9), CompareMethod.Binary) <> 0
                                intFim = InStr(intColAnterior, strLinha, Chr(9), CompareMethod.Binary)

                                If Trim(Mid(strLinha, intColAnterior, intFim - intColAnterior)) <> "" Then
                                    intValor = Val(Mid(strLinha, intColAnterior, intFim - intColAnterior))
                                Else
                                    intValor = 0
                                End If

                                'Cmd.CommandText
                                sql += "Update tb_rro " &
                                                "Set valrrotran = " & intValor & " " &
                                                "Where datPdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' And " &
                                                "      codusina = '" & cboUsina.Items(intUsina).Value.Trim() & "' And " &
                                                "      intrro = " & intI & ";"
                                'Cmd.ExecuteNonQuery()
                                intUsina = intUsina + 1
                                intColAnterior = intFim + 1
                                intCelula = intCelula + 1
                            Loop

                            If Trim(Mid(strLinha, intColAnterior)) <> "" Then
                                intValor = Val(Mid(strLinha, intColAnterior))
                            Else
                                intValor = 0
                            End If
                            'Cmd.CommandText
                            sql += "Update tb_rro " &
                                                "Set valrrotran = " & intValor & " " &
                                                "Where datPdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' And " &
                                                "      codusina = '" & cboUsina.Items(intUsina).Value.Trim() & "' And " &
                                                "      intrro = " & intI & ";"
                            'Cmd.ExecuteNonQuery()
                        Next
                    End If
                    strValor = ""
                End If

                'Grava evento registrando o recebimento de Geração
                sql += GerarSqlGravaEventoPDP("64", Format(Session("datEscolhida"), "yyyyMMdd"), cboEmpresa.SelectedItem.Text, datAtual, "PDPColGera", UsuarID)

                'Caso tenha registro para ser atualizado.
                If (sql <> "") Then
                    Conn.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
                    Conn.Open()
                    Cmd.CommandText = sql
                    Cmd.ExecuteNonQuery()
                End If

            Catch
                'houve erro, aborta a transação e fecha a conexão
                Session("strMensagem") = "Não foi possível gravar os dados."

                If Conn.State = ConnectionState.Open Then
                    Conn.Close()
                End If
                'Chama a tela de mensagem
                Response.Redirect("frmMensagem.aspx")
            Finally
                If Conn.State = ConnectionState.Open Then
                    Conn.Close()
                End If
            End Try
            Try
                'fecha a conexão com o banco
                'Cmd.Connection.Close()
                'Conn.Close()
                'tblTexto.Visible = False
                divValor.Visible = False
                btnSalvar.Visible = False
                cboUsina.SelectedIndex = 0
                PreencheTable()
            Catch
                Session("strMensagem") = "Não foi possível acessar a Base de Dados. Ocorreu o seguinte erro: " & Chr(10) & Err.Description
                'Conn.Close()
                Response.Redirect("frmMensagem.aspx")
            End Try
        Else
            Session("strMensagem") = "Usuário não tem permissão para alterar os valores."
            Response.Redirect("frmMensagem.aspx")
        End If
    End Sub

End Class
