Partial Class frmColIntercambio

    Inherits System.Web.UI.Page

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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load, Me.Load
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
                lblMsg.Visible = False
            Catch
                lblMsg.Visible = True
                'Session("strMensagem") = "Não foi possível acessar a Base de Dados."
                If Conn.State = ConnectionState.Open Then
                    Conn.Close()
                End If
                'Response.Redirect("frmMensagem.aspx")
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
        divValor.Visible = False
        btnSalvar.Visible = False
        If cboEmpresa.SelectedIndex > 0 Then
            Session("strCodEmpre") = cboEmpresa.SelectedItem.Value
        End If
        cboIntercambio.Items.Clear()
        Try
            If optModalidade.Checked = True Then
                Conn.Open("pdp")
                Cmd.CommandText = "Select codemprede, " & _
                                "       codemprepara, " & _
                                "       codcontade, " & _
                                "       codcontapara, " & _
                                "       codcontamodal, " & _
                                "       tipinter " & _
                                "From inter " & _
                                "Where codemprede = '" & Session("strCodEmpre") & "' And " & _
                                "      datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' " & _
                                "Group By codemprede, codemprepara, codcontade, codcontapara, codcontamodal, tipinter " & _
                                "Order By codemprepara, codcontade, codcontapara, codcontamodal, tipinter"
                Dim rsIntercambio As OnsClasses.OnsData.OnsDataReader = Cmd.ExecuteReader
                Dim objItem As System.Web.UI.WebControls.ListItem
                intI = 1
                Do While rsIntercambio.Read
                    If intI = 1 Then
                        objItem = New System.Web.UI.WebControls.ListItem
                        objItem.Text = "Selecione um Intercâmbio"
                        objItem.Value = "0"
                        cboIntercambio.Items.Add(objItem)
                    End If
                    objItem = New System.Web.UI.WebControls.ListItem
                    objItem.Text = rsIntercambio("tipinter")
                    objItem.Value = Trim(rsIntercambio("codemprede")) & "|" & Trim(rsIntercambio("codemprepara")) & "|" & Trim(rsIntercambio("codcontade")) & "|" & Trim(rsIntercambio("codcontapara")) & "|" & Trim(rsIntercambio("codcontamodal"))
                    cboIntercambio.Items.Add(objItem)
                    intI = intI + 1
                Loop
                If intI > 1 Then
                    objItem = New System.Web.UI.WebControls.ListItem
                    objItem.Text = "Todos os Intercâmbios"
                    objItem.Value = "Todos"
                    cboIntercambio.Items.Add(objItem)
                End If
                rsIntercambio.Close()
                rsIntercambio = Nothing
                Conn.Close()
            End If
            PreencheTable()

            'Valida Limite de Envio
            Dim lRetorno As Integer = 0
            If Not ValidaLimiteEntradaDados(cboEmpresa.SelectedItem.Value, cboData.SelectedItem.Value, lRetorno) Then
                btnSalvar.Visible = False
                cboIntercambio.Enabled = False
                If lRetorno = 1 Then
                    Response.Write("<SCRIPT>alert('" + strMsgInicioLimiteEnvioDados + "')</SCRIPT>")
                Else
                    Response.Write("<SCRIPT>alert('" + strMsgLimiteEnvioDados + "')</SCRIPT>")
                End If
                Exit Sub
            Else
                btnSalvar.Visible = True
                cboIntercambio.Enabled = True
            End If
            lblMsg.Visible = False
        Catch
            lblMsg.Visible = True
            'Session("strMensagem") = "Não foi possível acessar a Base de Dados."
            If Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If
            'Response.Redirect("frmMensagem.aspx")
        Finally
            If Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If
        End Try
    End Sub

    Private Sub PreencheTable()
        Dim intI, intJ As Integer
        Dim intLin As Integer
        Dim dblMedia As Double
        Dim objCel As TableCell
        Dim objRow As TableRow
        Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
        Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
        Cmd.Connection = Conn
        Dim strCodEmp As String
        Dim strCodContaDe As String
        Dim strCodContaPara As String
        Dim strCodModal As String
        Try
            If optModalidade.Checked = True Then
                Cmd.CommandText = "Select codemprede, " & _
                                "       codemprepara, " & _
                                "       codcontade, " & _
                                "       codcontapara, " & _
                                "       codcontamodal, " & _
                                "       intinter, " & _
                                "       valintertran, " & _
                                "       tipinter " & _
                                "From inter " & _
                                "Where codemprede = '" & Session("strCodEmpre") & "' And " & _
                                "      datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' " & _
                                "Order By codemprepara, codcontade, codcontapara, codcontamodal, tipinter, intinter"
            Else
                Cmd.CommandText = "Select codemprede, " & _
                                "       codemprepara, " & _
                                "       intinter, " & _
                                "       Sum(valintertran) As valintertran " & _
                                "From inter " & _
                                "Where codemprede = '" & Session("strCodEmpre") & "' And " & _
                                "      datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' " & _
                                "Group By codemprede, codemprepara, intinter " & _
                                "Order By codemprepara, intinter"
            End If

            Conn.Open("pdp")
            Dim rsIntercambio As OnsClasses.OnsData.OnsDataReader = Cmd.ExecuteReader

            tblIntercambio.Rows.Clear()
            objRow = New TableRow
            objRow.BackColor = System.Drawing.Color.Beige
            objRow.Width = WebControls.Unit.Pixel(100)
            objCel = New TableCell
            objCel.Wrap = False
            objCel.Text = "Intervalo"
            objCel.Font.Bold = True
            objCel.HorizontalAlign = HorizontalAlign.Center
            objCel.Width = WebControls.Unit.Pixel(100)
            objRow.Controls.Add(objCel)

            objCel = New TableCell
            objCel.Text = "Total"
            objCel.Wrap = False
            objCel.Font.Bold = True
            objCel.Width = WebControls.Unit.Pixel(62)
            objCel.HorizontalAlign = HorizontalAlign.Center
            objRow.Width = WebControls.Unit.Pixel(132)
            objRow.Controls.Add(objCel)
            tblIntercambio.Width = WebControls.Unit.Pixel(132)
            tblIntercambio.Controls.Add(objRow)

            Dim intHora As Integer = 0
            For intI = 1 To 48
                objRow = New TableRow
                objRow.Width = WebControls.Unit.Pixel(132)
                objCel = New TableCell
                objCel.BackColor = System.Drawing.Color.Beige
                objCel.Font.Bold = True
                objCel.Width = WebControls.Unit.Pixel(100)
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
                objCel.Width = WebControls.Unit.Pixel(62)
                objRow.Controls.Add(objCel)
                tblIntercambio.Controls.Add(objRow)
            Next

            objRow = New TableRow
            objRow.Width = WebControls.Unit.Pixel(132)
            objCel = New TableCell
            objCel.Text = "Total"
            objCel.BackColor = System.Drawing.Color.Beige
            objCel.Font.Bold = True
            objCel.Width = WebControls.Unit.Pixel(70)
            objCel.HorizontalAlign = HorizontalAlign.Center
            objRow.Controls.Add(objCel)
            objCel = New TableCell
            objCel.BackColor = System.Drawing.Color.Beige
            objCel.Font.Bold = True
            objCel.Width = WebControls.Unit.Pixel(62)
            objRow.Controls.Add(objCel)
            tblIntercambio.Controls.Add(objRow)

            objRow = New TableRow
            objRow.Width = WebControls.Unit.Pixel(132)
            objCel = New TableCell
            objCel.Text = "Média"
            objCel.BackColor = System.Drawing.Color.Beige
            objCel.Font.Bold = True
            objCel.Width = WebControls.Unit.Pixel(70)
            objCel.HorizontalAlign = HorizontalAlign.Center
            objRow.Controls.Add(objCel)
            objCel = New TableCell
            objCel.BackColor = System.Drawing.Color.Beige
            objCel.Font.Bold = True
            objCel.Width = WebControls.Unit.Pixel(62)
            objRow.Controls.Add(objCel)
            tblIntercambio.Controls.Add(objRow)

            intI = 1
            strCodEmp = ""
            strCodContaDe = ""
            strCodContaPara = ""
            strCodModal = ""
            dblMedia = 0

            Do While rsIntercambio.Read
                If optModalidade.Checked = True Then
                    If (strCodEmp & strCodContaDe & strCodContaPara & strCodModal) <> _
                        Trim(rsIntercambio("codemprepara")) & Trim(rsIntercambio("codcontade")) & _
                        Trim(rsIntercambio("codcontapara")) & Trim(rsIntercambio("codcontamodal")) Then
                        'Cria uma nova coluna na tabela
                        objCel = New TableCell
                        objCel.Width = WebControls.Unit.Pixel(64)
                        objCel.Wrap = False
                        objCel.Font.Size = WebControls.FontUnit.XXSmall
                        objCel.HorizontalAlign = HorizontalAlign.Center
                        objCel.Font.Bold = True

                        If Trim(rsIntercambio.Item("tipinter")) = Trim(cboIntercambio.SelectedItem.Text) Then
                            objCel.ForeColor = System.Drawing.Color.Red
                        End If

                        objCel.Text = Trim(rsIntercambio("codemprede")) & "-" & Trim(rsIntercambio("codemprepara")) & "/" & Trim(rsIntercambio("codcontamodal"))
                        tblIntercambio.Width = WebControls.Unit.Pixel(tblIntercambio.Width.Value + 64)
                        tblIntercambio.Rows(0).Controls.Add(objCel)

                        strCodEmp = Trim(rsIntercambio("codemprepara"))
                        strCodContaDe = Trim(rsIntercambio("codcontade"))
                        strCodContaPara = Trim(rsIntercambio("codcontapara"))
                        strCodModal = Trim(rsIntercambio("codcontamodal"))
                        intI = intI + 1
                        intLin = 1
                    End If
                Else
                    If strCodEmp <> Trim(rsIntercambio("codemprepara")) Then
                        'Cria uma nova coluna na tabela
                        objCel = New TableCell
                        objCel.Width = WebControls.Unit.Pixel(64)
                        objCel.Wrap = False
                        objCel.Font.Bold = True
                        objCel.Text = rsIntercambio("codemprede") & " - " & rsIntercambio("codemprepara")
                        tblIntercambio.Width = WebControls.Unit.Pixel(tblIntercambio.Width.Value + 64)
                        tblIntercambio.Rows(0).Controls.Add(objCel)
                        strCodEmp = Trim(rsIntercambio("codemprepara"))
                        intI = intI + 1
                        intLin = 1
                    End If
                End If

                'Inseri as celulas com os valores dos intercâmbios
                objCel = New TableCell
                objCel.Width = WebControls.Unit.Pixel(64)
                If Not IsDBNull(rsIntercambio("valintertran")) Then
                    objCel.Text = rsIntercambio("valintertran")
                    dblMedia = dblMedia + rsIntercambio.Item("valintertran")
                Else
                    objCel.Text = 0
                End If
                tblIntercambio.Rows(intLin).Controls.Add(objCel)
                intLin = intLin + 1
                If intLin = 49 Then
                    'Total dos intercâmbios
                    objCel = New TableCell
                    objCel.Width = WebControls.Unit.Pixel(64)
                    objCel.Text = dblMedia / 2
                    tblIntercambio.Rows(intLin).Controls.Add(objCel)
                    tblIntercambio.Rows(intLin).Cells(1).Text = Val(tblIntercambio.Rows(intLin).Cells(1).Text) + dblMedia
                    'Média dos intercâmbios
                    objCel = New TableCell
                    objCel.Width = WebControls.Unit.Pixel(64)
                    objCel.Text = Int(dblMedia / 48)
                    tblIntercambio.Rows(intLin + 1).Controls.Add(objCel)
                    tblIntercambio.Rows(intLin + 1).Cells(1).Text = Val(tblIntercambio.Rows(intLin + 1).Cells(1).Text) + Int(dblMedia / 48)
                    dblMedia = 0
                End If
            Loop

            For intI = 1 To 50
                dblMedia = 0
                For intJ = 2 To tblIntercambio.Rows(0).Cells.Count - 1
                    dblMedia = dblMedia + Val(tblIntercambio.Rows(intI).Cells(intJ).Text)
                Next
                tblIntercambio.Rows(intI).Cells(1).Text = dblMedia '/ 2
            Next
            rsIntercambio.Close()
            rsIntercambio = Nothing
            'Cmd.Connection.Close()
            'Conn.Close()
            lblMsg.Visible = False
        Catch
            lblMsg.Visible = True
            'Session("strMensagem") = "Não foi possível acessar a Base de Dados."
            If Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If
            'Response.Redirect("frmMensagem.aspx")
        Finally
            If Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If
        End Try
    End Sub

    Private Sub cboIntercambio_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboIntercambio.SelectedIndexChanged

        Dim intI As Integer
        Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
        Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
        Cmd.Connection = Conn
        Try
            If cboIntercambio.SelectedIndex > 0 Then
                If cboIntercambio.SelectedItem.Value <> "Todos" Then
                    Cmd.CommandText = "Select valintertran, " & _
                                        "       intinter " & _
                                        "From inter " & _
                                        "Where codemprede = '" & Mid(cboIntercambio.SelectedItem.Value, 1, 2) & "' And " & _
                                        "      codemprepara = '" & Mid(cboIntercambio.SelectedItem.Value, 4, 2) & "' And " & _
                                        "      codcontade = '" & Mid(cboIntercambio.SelectedItem.Value, 7, 2) & "' And " & _
                                        "      codcontapara = '" & Mid(cboIntercambio.SelectedItem.Value, 10, 2) & "' And " & _
                                        "      codcontamodal = '" & Mid(cboIntercambio.SelectedItem.Value, 13, 2) & "' And " & _
                                        "      datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' " & _
                                        "Order By intinter"
                Else
                    Cmd.CommandText = "Select valintertran, " & _
                                        "       intinter, " & _
                                        "       codemprede, " & _
                                        "       codemprepara, " & _
                                        "       codcontade, " & _
                                        "       codcontapara, " & _
                                        "       codcontamodal, " & _
                                        "       tipinter " & _
                                        "From inter " & _
                                        "Where datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' And " & _
                                        "      codemprede = '" & cboEmpresa.SelectedItem.Value & "' " & _
                                        "Order By intinter, " & _
                                        "         codemprede, " & _
                                        "         codemprepara, " & _
                                        "         codcontade, " & _
                                        "         codcontapara, " & _
                                        "         codcontamodal, " & _
                                        "         tipinter"
                End If

                Conn.Open("pdp")
                Dim rsIntercambio As OnsClasses.OnsData.OnsDataReader = Cmd.ExecuteReader
                Dim objTextArea As HtmlControls.HtmlTextArea
                objTextArea = New HtmlTextArea
                objTextArea.Rows = 48
                objTextArea.ID = "txtValor"
                objTextArea.Attributes.Item("onkeyup") = "RetiraEnter(event)"
                objTextArea.Attributes.Item("runat") = "server"

                If cboIntercambio.SelectedItem.Value <> "Todos" Then
                    objTextArea.Attributes.Item("style") = "font-size:XSmall;height:1001px;width:88px;line-height:20px"
                Else
                    objTextArea.Attributes.Item("style") = "font-size:XSmall;height:1001px;width:" & (((cboIntercambio.Items.Count - 2) * 65) + 16) - Int(cboIntercambio.Items.Count / 2) & "px;line-height:20px"
                End If

                Dim intInstante As Integer = 1
                Dim blnPassou As Boolean = False
                Do While rsIntercambio.Read
                    If blnPassou Then
                        If rsIntercambio("intinter") <> intInstante Then
                            intInstante = rsIntercambio("intinter")
                            objTextArea.Value = objTextArea.Value & Chr(13)
                        Else
                            objTextArea.Value = objTextArea.Value & Chr(9)
                        End If
                    End If
                    If Not IsDBNull(rsIntercambio("valintertran")) Then
                        objTextArea.Value = objTextArea.Value & rsIntercambio("valintertran")
                    Else
                        objTextArea.Value = objTextArea.Value & 0
                    End If
                    'na primeira passagem não escreve TAB nem ENTER
                    blnPassou = True
                Loop
                divValor.Controls.Add(objTextArea)
                divValor.Style.Item("TOP") = "210px"
                If cboIntercambio.SelectedItem.Value <> "Todos" Then
                    divValor.Style.Item("LEFT") = Trim(Str(102 + (cboIntercambio.SelectedIndex * 64))) & "px"
                Else
                    divValor.Style.Item("LEFT") = "166px"
                End If
                divValor.Controls.Add(objTextArea)
                divValor.Visible = True
                btnSalvar.Visible = True
                rsIntercambio.Close()
                rsIntercambio = Nothing
                Cmd.Connection.Close()
                Conn.Close()
            Else
                divValor.Visible = False
                btnSalvar.Visible = False
            End If
            PreencheTable()
            lblMsg.Visible = False
        Catch
            lblMsg.Visible = True
            'Session("strMensagem") = "Não foi possível acessar a Base de Dados."
            If Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If
            'Response.Redirect("frmMensagem.aspx")
        Finally
            If Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If
        End Try
    End Sub

    Private Sub optModalidade_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optModalidade.CheckedChanged
        If optEmpresa.Checked = True Then
            optEmpresa.Checked = False
        End If
        If cboEmpresa.SelectedIndex <> 0 Then
            cboEmpresa_SelectedIndexChanged(sender, e)
        End If
    End Sub

    Private Sub optEmpresa_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optEmpresa.CheckedChanged
        If optModalidade.Checked = True Then
            optModalidade.Checked = False
        End If
        If cboEmpresa.SelectedIndex <> 0 Then
            cboEmpresa_SelectedIndexChanged(sender, e)
        End If
    End Sub

    Private Sub cboData_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboData.SelectedIndexChanged
        Try
            If cboData.SelectedIndex <> 0 Then
                Session("datEscolhida") = CDate(cboData.SelectedItem.Value)
            End If
            If cboEmpresa.SelectedIndex > 0 Then
                cboEmpresa_SelectedIndexChanged(sender, e)
            End If
            lblMsg.Visible = False
        Catch
            lblMsg.Visible = True
            'Session("strMensagem") = "Não foi possível acessar a Base de Dados."
            'Response.Redirect("frmMensagem.aspx")
        End Try
    End Sub

    Private Overloads Sub btnSalvar_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSalvar.Click
        Dim intI As Integer
        Dim intJ As Integer
        Dim intColAtual As Integer
        Dim intColAnterior As Integer
        Dim intQtdReg As Integer
        Dim intCol As Integer
        Dim intValor As Integer
        Dim strValor As String
        'Dim objPermissao As OnsClasses.OnsInterfaceUnica.OnsPermissao
        'objPermissao = New OnsClasses.OnsInterfaceUnica.OnsPermissao("UPD", "PDPColInte", UsuarID)
        'Verifica se o usuário tem permissão para salvar os registros
        If EstaAutorizado Then

            Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
            Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
            Cmd.Connection = Conn
            Dim objTrans As OnsClasses.OnsData.OnsTransaction
            Dim datAtual As Date = Now
            Try
                Conn.Open("pdp")
                Conn.Servico = "PDPColInte"
                Conn.Usuario = UsuarID

                'Alterando os valores de carga na BDT
                objTrans = Conn.BeginTransaction()
                Cmd.Transaction = objTrans
                strValor = Page.Request.Form.Item("txtValor")
                'Atualiza a grid
                If strValor = "" Then
                    'Quando o txtValor estiver em branco, branqueia tabela e a grid
                    If cboIntercambio.SelectedItem.Value <> "Todos" Then
                        For intI = 1 To 48
                            'Atualiza na BDT a Coluna Alterada
                            Cmd.CommandText = "Update inter " &
                                                "Set valintertran = 0 " &
                                                "Where datPdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' And " &
                                                "      codemprede = '" & Mid(cboIntercambio.SelectedItem.Value, 1, 2) & "' And " &
                                                "      codemprepara = '" & Mid(cboIntercambio.SelectedItem.Value, 4, 2) & "' And " &
                                                "      codcontade = '" & Mid(cboIntercambio.SelectedItem.Value, 7, 2) & "' And " &
                                                "      codcontapara = '" & Mid(cboIntercambio.SelectedItem.Value, 10, 2) & "' And " &
                                                "      codcontamodal = '" & Mid(cboIntercambio.SelectedItem.Value, 13, 2) & "' And " &
                                                "      intinter = " & intI
                            Cmd.ExecuteNonQuery()
                        Next
                    Else
                        For intI = 1 To 48
                            For intJ = 1 To cboIntercambio.Items.Count - 2
                                'Atualiza na BDT TODAS as Colunas Alteradas
                                Cmd.CommandText = "Update inter " &
                                                "Set valintertran = 0 " &
                                                "Where datPdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' And " &
                                                "      codemprede = '" & Mid(cboIntercambio.Items(intJ).Value, 1, 2) & "' And " &
                                                "      codemprepara = '" & Mid(cboIntercambio.Items(intJ).Value, 4, 2) & "' And " &
                                                "      codcontade = '" & Mid(cboIntercambio.Items(intJ).Value, 7, 2) & "' And " &
                                                "      codcontapara = '" & Mid(cboIntercambio.Items(intJ).Value, 10, 2) & "' And " &
                                                "      codcontamodal = '" & Mid(cboIntercambio.Items(intJ).Value, 13, 2) & "' And " &
                                                "      intinter = " & intI
                                Cmd.ExecuteNonQuery()
                            Next
                        Next
                    End If
                Else
                    If cboIntercambio.SelectedItem.Value <> "Todos" Then
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
                            Cmd.CommandText = "Update inter " &
                                                "Set valintertran = " & intValor &
                                                "Where datPdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' And " &
                                                "      codemprede = '" & Mid(cboIntercambio.SelectedItem.Value, 1, 2) & "' And " &
                                                "      codemprepara = '" & Mid(cboIntercambio.SelectedItem.Value, 4, 2) & "' And " &
                                                "      codcontade = '" & Mid(cboIntercambio.SelectedItem.Value, 7, 2) & "' And " &
                                                "      codcontapara = '" & Mid(cboIntercambio.SelectedItem.Value, 10, 2) & "' And " &
                                                "      codcontamodal = '" & Mid(cboIntercambio.SelectedItem.Value, 13, 2) & "' And " &
                                                "      intinter = " & intI
                            Cmd.ExecuteNonQuery()
                            intColAtual = InStr(intColAnterior + 1, strValor, Chr(13), CompareMethod.Binary)
                        Next
                        strValor = ""
                    Else

                        Dim strLinha As String
                        Dim intCelula, intFim, intInter As Integer

                        strLinha = ""
                        intColAtual = 1
                        For intI = 1 To 48

                            'Todas as USINA
                            intInter = 1
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

                                Cmd.CommandText = "Update inter " &
                                                "Set valintertran = " & intValor &
                                                "Where datPdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' And " &
                                                "      codemprede = '" & Mid(cboIntercambio.Items(intInter).Value, 1, 2) & "' And " &
                                                "      codemprepara = '" & Mid(cboIntercambio.Items(intInter).Value, 4, 2) & "' And " &
                                                "      codcontade = '" & Mid(cboIntercambio.Items(intInter).Value, 7, 2) & "' And " &
                                                "      codcontapara = '" & Mid(cboIntercambio.Items(intInter).Value, 10, 2) & "' And " &
                                                "      codcontamodal = '" & Mid(cboIntercambio.Items(intInter).Value, 13, 2) & "' And " &
                                                "      intinter = " & intI
                                Cmd.ExecuteNonQuery()
                                intInter = intInter + 1
                                intColAnterior = intFim + 1
                                intCelula = intCelula + 1
                            Loop

                            If Trim(Mid(strLinha, intColAnterior)) <> "" Then
                                intValor = Val(Mid(strLinha, intColAnterior))
                            Else
                                intValor = 0
                            End If

                            Cmd.CommandText = "Update inter " &
                                                "Set valintertran = " & intValor &
                                                "Where datPdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' And " &
                                                "      codemprede = '" & Mid(cboIntercambio.Items(intInter).Value, 1, 2) & "' And " &
                                                "      codemprepara = '" & Mid(cboIntercambio.Items(intInter).Value, 4, 2) & "' And " &
                                                "      codcontade = '" & Mid(cboIntercambio.Items(intInter).Value, 7, 2) & "' And " &
                                                "      codcontapara = '" & Mid(cboIntercambio.Items(intInter).Value, 10, 2) & "' And " &
                                                "      codcontamodal = '" & Mid(cboIntercambio.Items(intInter).Value, 13, 2) & "' And " &
                                                "      intinter = " & intI
                            Cmd.ExecuteNonQuery()
                        Next
                    End If
                End If

                'Grava evento registrando o recebimento de Intercâmbio
                GravaEventoPDP("9", Format(Session("datEscolhida"), "yyyyMMdd"), cboEmpresa.SelectedItem.Text, datAtual, "PDPColInte", UsuarID)

                'Grava toda a transação
                objTrans.Commit()
                lblMsg.Visible = False
            Catch
                lblMsg.Visible = True
                'houve erro, aborta a transação e fecha a conexão
                'Session("strMensagem") = "Não foi possível gravar os dados."
                objTrans.Rollback()
                If Conn.State = ConnectionState.Open Then
                    Conn.Close()
                End If
                'Chama a tela de mensagem
                'Response.Redirect("frmMensagem.aspx")
            Finally
                If Conn.State = ConnectionState.Open Then
                    Conn.Close()
                End If
            End Try
            Try
                'fecha a conexão com o banco
                'Cmd.Connection.Close()
                'Conn.Close()
                divValor.Visible = False
                btnSalvar.Visible = False
                cboIntercambio.SelectedIndex = 0
                PreencheTable()
                lblMsg.Visible = False
            Catch
                lblMsg.Visible = True
                'Session("strMensagem") = "Não foi possível acessar a Base de Dados."
                'Response.Redirect("frmMensagem.aspx")
            End Try
        Else
            Session("strMensagem") = "Usuário não tem permissão para alterar os valores."
            Response.Redirect("frmMensagem.aspx")
        End If
    End Sub
End Class
