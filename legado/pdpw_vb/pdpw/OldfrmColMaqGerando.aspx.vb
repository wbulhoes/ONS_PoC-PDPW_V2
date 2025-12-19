Partial Class frmColMaqGerando
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        'objMenu = CType(Session("onsmenu"), OnsWebControls.OnsMenu)

        If Not Page.IsPostBack Then
            Dim intI As Integer
            Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
            Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
            If Session("datEscolhida") = Nothing Then
                'Inicializa a variável com data do próximo
                Session("datEscolhida") = Now.AddDays(1)
            End If
            Cmd.Connection = Conn

            Try
                Conn.Open("pdp")
                Cmd.CommandText = "Select datpdp " & _
                                "From pdp " & _
                                "Order By datpdp Desc"
                Dim rsData As OnsClasses.OnsData.OnsDataReader = Cmd.ExecuteReader
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
                Session("strMensagem") = "Não foi possível acessar a Base de Dados."
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

    Private Sub cboEmpresa_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboEmpresa.SelectedIndexChanged
        Dim intI As Integer
        Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
        Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
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
                            "From maq_gerando d, " &
                            "     usina u " &
                            "Where u.codempre = '" & Session("strCodEmpre") & "' And " &
                            "     (u.tipusina = 'T' or u.tipusina = 'H') And " &
                            "      u.codusina = d.codusina And " &
                            "      d.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' " &
                            "      And u.flg_recebepdpage = 'S' " &
                            "Group By u.ordem, " &
                            "         d.codusina " &
                            "Order By u.ordem, " &
                            "         d.codusina"
            Conn.Open("pdp")
            Dim rsUsina As OnsClasses.OnsData.OnsDataReader = Cmd.ExecuteReader
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
            If Not ValidaLimiteEntradaDados(cboEmpresa.SelectedItem.Value, cboData.SelectedItem.Value, lRetorno, "MEG") Then
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
            Session("strMensagem") = "Não foi possível acessar a Base de Dados."
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
        Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
        Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
        Cmd.Connection = Conn
        Try
            If cboUsina.SelectedItem.Text = "Todas as Usinas" Then
                Cmd.CommandText = "Select d.valmegtran, " &
                                "       d.intmeg, " &
                                "       d.codusina, " &
                                "       u.ordem " &
                                "From maq_gerando d, " &
                                "     usina u " &
                                "Where u.codempre = '" & cboEmpresa.SelectedItem.Value & "' And " &
                                "     (u.tipusina = 'T' or u.tipusina = 'H') And " &
                                "      u.codusina = d.codusina And " &
                                "      d.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' " &
                                "      And u.flg_recebepdpage = 'S' " &
                                "Order By d.intmeg, " &
                                "         u.ordem, " &
                                "         d.codusina"
            Else
                Cmd.CommandText = "Select d.valmegtran, " &
                                "       d.intmeg " &
                                "From maq_gerando d, " &
                                "     usina u " &
                                "Where u.codempre = '" & cboEmpresa.SelectedItem.Value & "' And " &
                                "     (u.tipusina = 'T' or u.tipusina = 'H') And " &
                                "      u.codusina = d.codusina And " &
                                "      d.codusina = '" & cboUsina.SelectedItem.Value & "' And " &
                                "      d.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' " &
                                "      And u.flg_recebepdpage = 'S' " &
                                "Order By d.intmeg"
            End If
            Conn.Open("pdp")
            Dim rsUsina As OnsClasses.OnsData.OnsDataReader = Cmd.ExecuteReader

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
                    If rsUsina("intmeg") <> intInstante Then
                        intInstante = rsUsina("intmeg")
                        objTextArea.Value = objTextArea.Value & Chr(13)
                    Else
                        objTextArea.Value = objTextArea.Value & Chr(9)
                    End If
                End If
                If Not IsDBNull(rsUsina.Item("valmegtran")) Then
                    objTextArea.Value = objTextArea.Value & rsUsina.Item("valmegtran")
                Else
                    objTextArea.Value = objTextArea.Value & 0
                End If
                'na primeira passagem não escreve TAB nem ENTER
                blnPassou = True
            Loop
            divValor.Controls.Add(objTextArea)
            divValor.Style.Item("TOP") = "249px"
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
            Session("strMensagem") = "Não foi possível acessar a Base de Dados."
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
        Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
        Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
        Cmd.Connection = Conn
        Dim strCodUsina As String
        Try
            Cmd.CommandText = "Select d.codusina, " &
                            "       d.intmeg, " &
                            "       d.valmegtran, " &
                            "       u.ordem " &
                            "From maq_gerando d, " &
                            "     usina u " &
                            "Where u.codempre = '" & Session("strCodEmpre") & "' And " &
                            "     (u.tipusina = 'T' or u.tipusina = 'H') And " &
                            "      u.codusina = d.codusina And " &
                            "      d.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' " &
                            "      And u.flg_recebepdpage = 'S' " &
                            "Order By u.ordem, " &
                            "         d.codusina, " &
                            "         d.intmeg"
            Conn.Open("pdp")
            Dim rsUsina As OnsClasses.OnsData.OnsDataReader = Cmd.ExecuteReader
            tblMaqGerando.Rows.Clear()
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
            tblMaqGerando.Width = objTamanho.Pixel(132)
            tblMaqGerando.Controls.Add(objRow)

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
                'If intI Mod 2 <> 0 Then
                '    objCel.Text = Format(intHora - 1, "00") & ":00" & "-" & Format(intHora, "00") & ":30"
                '    intHora = intHora + 1
                'Else
                '    objCel.Text = Format(intHora - 1, "00") & ":30" & "-" & Format(intHora, "00") & ":00"
                'End If
                objRow.Controls.Add(objCel)
                objCel = New TableCell
                objCel.Text = 0
                objCel.BackColor = System.Drawing.Color.Beige
                objCel.Font.Bold = True
                objCel.Width = objTamanho.Pixel(62)
                objRow.Controls.Add(objCel)
                tblMaqGerando.Controls.Add(objRow)
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
            tblMaqGerando.Controls.Add(objRow)

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
            tblMaqGerando.Controls.Add(objRow)

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
                    tblMaqGerando.Rows(0).Width = objTamanho.Pixel(tblMaqGerando.Rows(0).Width.Value + 64)
                    tblMaqGerando.Width = objTamanho.Pixel(tblMaqGerando.Width.Value + 64)

                    If Trim(rsUsina("codusina")) = Trim(cboUsina.SelectedItem.Text) Then
                        objCel.ForeColor = System.Drawing.Color.Red
                    End If
                    objCel.Text = rsUsina("codusina")
                    tblMaqGerando.Rows(0).Controls.Add(objCel)
                    strCodUsina = rsUsina("codusina")
                    intI = intI + 1
                    intLin = 1
                End If
                'Inseri as celulas com os valores das usinas
                objCel = New TableCell
                objCel.Wrap = False
                objCel.Width = objTamanho.Pixel(64)
                If Not IsDBNull(rsUsina.Item("valmegtran")) Then
                    objCel.Text = rsUsina.Item("valmegtran")
                    dblMedia = dblMedia + rsUsina.Item("valmegtran")
                Else
                    objCel.Text = 0
                End If
                tblMaqGerando.Rows(intLin).Width = objTamanho.Pixel(tblMaqGerando.Rows(intLin).Width.Value + 63)
                tblMaqGerando.Rows(intLin).Controls.Add(objCel)
                intLin = intLin + 1
                If intLin = 49 Then
                    objCel = New TableCell
                    objCel.Wrap = False
                    objCel.Width = objTamanho.Pixel(64)
                    objCel.Text = Trim(Str(dblMedia / 2))
                    tblMaqGerando.Rows(intLin).Width = objTamanho.Pixel(tblMaqGerando.Rows(intLin).Width.Value + 64)
                    tblMaqGerando.Rows(intLin).Controls.Add(objCel)
                    tblMaqGerando.Rows(intLin).Cells(1).Text = Trim(Str(Val(tblMaqGerando.Rows(intLin).Cells(1).Text) + dblMedia))  ' (dblMedia / 2)))

                    objCel = New TableCell
                    objCel.Wrap = False
                    objCel.Width = objTamanho.Pixel(64)
                    objCel.Text = Trim(Str(Int(dblMedia / 48)))
                    tblMaqGerando.Rows(intLin + 1).Width = objTamanho.Pixel(tblMaqGerando.Rows(intLin + 1).Width.Value + 64)
                    tblMaqGerando.Rows(intLin + 1).Controls.Add(objCel)
                    tblMaqGerando.Rows(intLin + 1).Cells(1).Text = Trim(Str(Val(tblMaqGerando.Rows(intLin + 1).Cells(1).Text) + Int(dblMedia / 48)))

                    dblMedia = 0
                End If
            Loop

            For intI = 1 To 50
                dblMedia = 0
                For intJ = 2 To tblMaqGerando.Rows(0).Cells.Count - 1
                    dblMedia = dblMedia + Val(tblMaqGerando.Rows(intI).Cells(intJ).Text)
                Next
                tblMaqGerando.Rows(intI).Cells(1).Text = dblMedia '/ 2
            Next

            rsUsina.Close()
            rsUsina = Nothing
            'Cmd.Connection.Close()
            'Conn.Close()
        Catch
            Session("strMensagem") = "Não foi possível acessar a Base de Dados."
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
            If cboEmpresa.SelectedIndex > 0 Then
                cboEmpresa_SelectedIndexChanged(sender, e)
            End If
        Catch
            Session("strMensagem") = "Não foi possível acessar a Base de Dados."
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

        Dim objPermissao As OnsClasses.OnsInterfaceUnica.OnsPermissao
        objPermissao = New OnsClasses.OnsInterfaceUnica.OnsPermissao("UPD", "PDPColMaqGerand", UsuarID)
        'Verifica se o usuário tem permissão para salvar os registros
        If EstaAutorizado Then
            Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
            Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
            Cmd.Connection = Conn
            Dim objTrans As OnsClasses.OnsData.OnsTransaction
            Conn.Open("pdp")
            Conn.Servico = "PDPColMaqGerand"
            Conn.Usuario = UsuarID

            'Alterando os valores de carga na BDT
            objTrans = Conn.BeginTransaction()
            Cmd.Transaction = objTrans
            Dim datAtual As Date = Now
            strValor = Page.Request.Form.Item("txtValor")

            Try
                'Atualiza a grid
                If strValor = "" Then
                    'Quando o txtValor estiver em branco, branqueia tabela e a grid
                    If cboUsina.SelectedItem.Text <> "Todas as Usinas" Then
                        For intI = 1 To 48
                            'Atualiza na BDT a Coluna Alterada
                            Cmd.CommandText = "Update maq_gerando " & _
                                                "Set valmegtran = 0 " & _
                                                "Where datPdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' And " & _
                                                "      codusina = '" & Trim(cboUsina.SelectedItem.Value) & "' And " & _
                                                "      intmeg = " & intI
                            Cmd.ExecuteNonQuery()
                        Next
                    Else
                        For intI = 1 To 48
                            For intJ = 1 To cboUsina.Items.Count - 2
                                'Atualiza na BDT TODAS as Colunas Alteradas
                                Cmd.CommandText = "Update maq_gerando " & _
                                                "Set valmegtran = 0 " & _
                                                "Where datPdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' And " & _
                                                "      codusina = '" & Trim(cboUsina.Items(intJ).Value) & "' And " & _
                                                "      intmeg = " & intI
                                Cmd.ExecuteNonQuery()
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
                            Cmd.CommandText = "Update maq_gerando " & _
                                                "Set valmegtran = " & intValor & " " & _
                                                "Where datPdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' And " & _
                                                "      codusina = '" & cboUsina.SelectedItem.Value & "' And " & _
                                                "      intmeg = " & intI
                            Cmd.ExecuteNonQuery()
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

                                Cmd.CommandText = "Update maq_gerando " & _
                                                "Set valmegtran = " & intValor & " " & _
                                                "Where datPdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' And " & _
                                                "      codusina = '" & cboUsina.Items(intUsina).Value & "' And " & _
                                                "      intmeg = " & intI
                                Cmd.ExecuteNonQuery()
                                intUsina = intUsina + 1
                                intColAnterior = intFim + 1
                                intCelula = intCelula + 1
                            Loop

                            If Trim(Mid(strLinha, intColAnterior)) <> "" Then
                                intValor = Val(Mid(strLinha, intColAnterior))
                            Else
                                intValor = 0
                            End If
                            Cmd.CommandText = "Update maq_gerando " & _
                                              "Set valmegtran = " & intValor & " " & _
                                              "Where datPdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' And " & _
                                              "      codusina = '" & cboUsina.Items(intUsina).Value & "' And " & _
                                              "      intmeg = " & intI
                            Cmd.ExecuteNonQuery()
                        Next
                    End If
                    strValor = ""
                End If

                'Grava evento registrando o recebimento de Número de Máquinas Gerando
                GravaEventoPDP("38", Format(Session("datEscolhida"), "yyyyMMdd"), cboEmpresa.SelectedItem.Text, datAtual, "PDPColMaqGerand", UsuarID)

                'Grava toda a transação
                objTrans.Commit()
            Catch
                'houve erro, aborta a transação e fecha a conexão
                Session("strMensagem") = "Não foi possível gravar os dados."
                objTrans.Rollback()
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
                Session("strMensagem") = "Não foi possível acessar a Base de Dados."
                'Conn.Close()
                Response.Redirect("frmMensagem.aspx")
            End Try
        Else
            Session("strMensagem") = "Usuário não tem permissão para alterar os valores."
            Response.Redirect("frmMensagem.aspx")
        End If
    End Sub
End Class
