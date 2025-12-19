Partial Class frmColIntercambioEst
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
            divCal.Visible = False
            Cmd.Connection = Conn

            Try

                PreencheComboEmpresaPOP(AgentesRepresentados, cboEmpresa, Session("strCodEmpre"), "S")
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
        divValor.Visible = False
        btnSalvar.Visible = False
        If cboEmpresa.SelectedIndex > 0 Then
            Session("strCodEmpre") = cboEmpresa.SelectedItem.Value
        Else
            Session("strCodEmpre") = ""
            cboIntercambio.Items.Clear()
            divValor.Visible = False
            Exit Sub
        End If
        cboIntercambio.Items.Clear()
        Try
            Conn.Open("pdp")
            Cmd.CommandText = "SELECT codemprede, codemprepara, codcontamodal " & _
                              "FROM interempre " & _
                              "WHERE codemprede = '" & Session("strCodEmpre") & "' " & _
                              "GROUP BY codemprede, codemprepara, codcontamodal " & _
                              "Order By codemprepara, codcontamodal"
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

                objItem.Text = Trim(rsIntercambio("codemprede")) & "-" & Trim(rsIntercambio("codemprepara")) & "-" & Trim(rsIntercambio("codcontamodal"))
                objItem.Value = Trim(rsIntercambio("codemprede")) & "|" & Trim(rsIntercambio("codemprepara")) & "|" & Trim(rsIntercambio("codcontamodal"))
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
        Dim intI, intJ, intCol As Integer
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
            Cmd.CommandText = "SELECT codemprede, codemprepara, codcontamodal " & _
                              "FROM interempre " & _
                              "WHERE codemprede = '" & Session("strCodEmpre") & "' " & _
                              "ORDER BY codemprepara, codcontamodal"

            Conn.Open("pdp")
            Dim rsIntercambio As OnsClasses.OnsData.OnsDataReader = Cmd.ExecuteReader

            tblIntercambio.Controls.Clear()
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

            intCol = 2
            Do While rsIntercambio.Read
                'Cria uma nova coluna na tabela
                objCel = New TableCell
                objCel.Width = WebControls.Unit.Pixel(64)
                objCel.Wrap = False
                objCel.Font.Size = WebControls.FontUnit.XXSmall
                objCel.HorizontalAlign = HorizontalAlign.Center
                objCel.Font.Bold = True

                objCel.Text = Trim(rsIntercambio("codemprede")) & "-" & Trim(rsIntercambio("codemprepara")) & "/" & Trim(rsIntercambio("codcontamodal"))
                tblIntercambio.Width = WebControls.Unit.Pixel(tblIntercambio.Width.Value + 64)
                tblIntercambio.Rows(0).Controls.Add(objCel)

                For intI = 1 To 50
                    objCel = New TableCell
                    objCel.Width = WebControls.Unit.Pixel(64)
                    tblIntercambio.Rows(intI).Controls.Add(objCel)
                    tblIntercambio.Rows(intI).Cells(intCol).Text = ""
                Next
                intCol += 1
            Loop

            rsIntercambio.Close()
            rsIntercambio = Nothing

            intI = 2
            strCodEmp = ""
            strCodModal = ""
            dblMedia = 0

            Cmd.CommandText = "SELECT codemprede, codemprepara, codcontamodal, intinter, valinter " & _
                              "FROM tb_interestudo " & _
                              "WHERE codemprede = '" & Session("strCodEmpre") & "' " & _
                              "AND datpdp = '" & Format(CDate(txtData.Text), "yyyyMMdd") & "' " & _
                              "ORDER BY codemprepara, codcontamodal, intinter"

            rsIntercambio = Cmd.ExecuteReader

            blnIncluir = True
            intCol = 1
            Do While rsIntercambio.Read
                If (strCodEmp & strCodModal) <> _
                    Trim(rsIntercambio("codemprepara")) & Trim(rsIntercambio("codcontamodal")) Then
                    '    'Cria uma nova coluna na tabela
                    '    objCel = New TableCell
                    '    objCel.Width = WebControls.Unit.Pixel(64)
                    '    objCel.Wrap = False
                    '    objCel.Font.Size = WebControls.FontUnit.XXSmall
                    '    objCel.HorizontalAlign = HorizontalAlign.Center
                    '    objCel.Font.Bold = True

                    '    'If Trim(rsIntercambio.Item("tipinter")) = Trim(cboIntercambio.SelectedItem.Text) Then
                    '    '    objCel.ForeColor = System.Drawing.Color.Red
                    '    'End If

                    '    objCel.Text = Trim(rsIntercambio("codemprede")) & "-" & Trim(rsIntercambio("codemprepara")) & "/" & Trim(rsIntercambio("codcontamodal"))
                    '    tblIntercambio.Width = WebControls.Unit.Pixel(tblIntercambio.Width.Value + 64)
                    '    tblIntercambio.Rows(0).Controls.Add(objCel)

                    strCodEmp = Trim(rsIntercambio("codemprepara"))
                    strCodModal = Trim(rsIntercambio("codcontamodal"))
                    '    intI = intI + 1
                    intLin = 1
                    blnIncluir = False
                    intCol += 1
                    Do While tblIntercambio.Rows(0).Cells(intCol).Text <> Trim(Session("strCodEmpre")) & "-" & Trim(rsIntercambio("codemprepara")) & "/" & Trim(rsIntercambio("codcontamodal"))
                        intCol += 1
                    Loop
                End If

                'Inseri as celulas com os valores dos intercâmbios
                'objCel = New TableCell
                'objCel.Width = WebControls.Unit.Pixel(64)
                If Not IsDBNull(rsIntercambio("valinter")) Then
                    'objCel.Text = rsIntercambio("valinter")
                    tblIntercambio.Rows(intLin).Cells(intCol).Text = rsIntercambio("valinter")
                    dblMedia += rsIntercambio.Item("valinter")
                Else
                    tblIntercambio.Rows(intLin).Cells(intCol).Text = 0
                    'objCel.Text = 0
                End If
                intLin = intLin + 1
                If intLin = 49 Then
                    'Total dos intercâmbios
                    tblIntercambio.Rows(intLin).Cells(intCol).Text = dblMedia
                    'Média dos intercâmbios
                    tblIntercambio.Rows(intLin + 1).Cells(intCol).Text = CInt(dblMedia / 48)
                    dblMedia = 0
                End If
            Loop

            For intI = 1 To 50
                dblMedia = 0
                For intJ = 2 To tblIntercambio.Rows(0).Cells.Count - 1
                    dblMedia += Val(tblIntercambio.Rows(intI).Cells(intJ).Text)
                Next
                tblIntercambio.Rows(intI).Cells(1).Text = dblMedia '/ 2
            Next
            rsIntercambio.Close()
            rsIntercambio = Nothing

            Cmd.CommandText = "SELECT datpdp, codempre " & _
                              "FROM mensa " & _
                              "WHERE datpdp = '" & Format(CDate(txtData.Text), "yyyyMMdd") & "' " & _
                              "AND codempre = '" & cboEmpresa.SelectedItem.Value & "'"
            Dim rsMensa As OnsClasses.OnsData.OnsDataReader = Cmd.ExecuteReader
            If rsMensa.Read Then
                lblMsg.Visible = True
            Else
                lblMsg.Visible = False
            End If
            rsMensa.Close()
            rsMensa = Nothing

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

    Private Sub cboIntercambio_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboIntercambio.SelectedIndexChanged

        Dim intI As Integer
        Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
        Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
        Cmd.Connection = Conn
        Try
            If cboIntercambio.SelectedIndex > 0 Then
                If cboIntercambio.SelectedItem.Value <> "Todos" Then
                    Cmd.CommandText = "SELECT valinter, intinter " & _
                                      "FROM tb_interestudo " & _
                                      "WHERE codemprede = '" & Mid(cboIntercambio.SelectedItem.Value, 1, 2) & "' " & _
                                      "AND codemprepara = '" & Mid(cboIntercambio.SelectedItem.Value, 4, 2) & "' " & _
                                      "AND codcontamodal = '" & Mid(cboIntercambio.SelectedItem.Value, 7, 2) & "' " & _
                                      "AND datpdp = '" & Format(CDate(txtData.Text), "yyyyMMdd") & "' " & _
                                      "ORDER BY intinter"
                Else
                    Cmd.CommandText = "SELECT valinter, intinter, codemprede, codemprepara, codcontamodal " & _
                                      "FROM tb_interestudo " & _
                                      "WHERE datpdp = '" & Format(CDate(txtData.Text), "yyyyMMdd") & "' " & _
                                      "AND codemprede = '" & cboEmpresa.SelectedItem.Value & "' " & _
                                      "ORDER BY intinter, codemprede, codemprepara, codcontamodal"
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
                    objTextArea.Attributes.Item("style") = "font-size:XSmall;height:1001px;width:80px;line-height:20px"
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
                    If Not IsDBNull(rsIntercambio("valinter")) Then
                        objTextArea.Value = objTextArea.Value & rsIntercambio("valinter")
                    Else
                        objTextArea.Value = objTextArea.Value & 0
                    End If
                    'na primeira passagem não escreve TAB nem ENTER
                    blnPassou = True
                Loop
                divValor.Controls.Add(objTextArea)
                divValor.Style.Item("TOP") = "250px"
                If cboIntercambio.SelectedItem.Value <> "Todos" Then
                    divValor.Style.Item("LEFT") = Trim(Str(121 + (cboIntercambio.SelectedIndex * 64))) & "px"
                Else
                    divValor.Style.Item("LEFT") = "185px"
                End If
                divValor.Controls.Add(objTextArea)
                divValor.Visible = True
                btnSalvar.Visible = True
                rsIntercambio.Close()
                rsIntercambio = Nothing
                'Cmd.Connection.Close()
                'Conn.Close()
            Else
                divValor.Visible = False
                btnSalvar.Visible = False
            End If
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

    Private Overloads Sub btnSalvar_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSalvar.Click
        Dim intI As Integer
        Dim intJ As Integer
        Dim intColAtual As Integer
        Dim intColAnterior As Integer
        Dim intQtdReg As Integer
        Dim intCol As Integer
        Dim intValor As Integer
        Dim strValor As String
        Dim strSql As String
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
                        strSql = "DELETE FROM tb_interestudo " & _
                                 "WHERE datpdp = '" & Format(CDate(txtData.Text), "yyyyMMdd") & "' " & _
                                 "AND codemprede = '" & Mid(cboIntercambio.SelectedItem.Value, 1, 2) & "' " & _
                                 "AND codemprepara = '" & Mid(cboIntercambio.SelectedItem.Value, 4, 2) & "' " & _
                                 "AND codcontamodal = '" & Mid(cboIntercambio.SelectedItem.Value, 7, 2) & "'"
                        Cmd.CommandText = strSql
                        Cmd.ExecuteNonQuery()
                        For intI = 1 To 48
                            'Atualiza na BDT a Coluna Alterada
                            'If blnIncluir Then

                            strSql = "INSERT INTO tb_interestudo " & _
                                     "(datpdp, codemprede, codemprepara, codcontamodal, intinter, valinter) " & _
                                     "VALUES (" & _
                                     "'" & Format(CDate(txtData.Text), "yyyyMMdd") & "', " & _
                                     "'" & Mid(cboIntercambio.SelectedItem.Value, 1, 2) & "', " & _
                                     "'" & Mid(cboIntercambio.SelectedItem.Value, 4, 2) & "', " & _
                                     "'" & Mid(cboIntercambio.SelectedItem.Value, 7, 2) & "', " & _
                                     "" & intI & ", " & _
                                     "" & "0" & ")"
                            Cmd.CommandText = strSql
                            Cmd.ExecuteNonQuery()
                        Next
                    Else
                        For intJ = 1 To cboIntercambio.Items.Count - 2
                            strSql = "DELETE FROM tb_interestudo " & _
                                     "WHERE datpdp = '" & Format(CDate(txtData.Text), "yyyyMMdd") & "' " & _
                                     "AND codemprede = '" & Mid(cboIntercambio.Items(intJ).Value, 1, 2) & "' " & _
                                     "AND codemprepara = '" & Mid(cboIntercambio.Items(intJ).Value, 4, 2) & "' " & _
                                     "AND codcontamodal = '" & Mid(cboIntercambio.Items(intJ).Value, 7, 2) & "'"
                            Cmd.CommandText = strSql
                            Cmd.ExecuteNonQuery()

                            For intI = 1 To 48
                                'Atualiza na BDT TODAS as Colunas Alteradas
                                'If blnIncluir Then

                                strSql = "INSERT INTO tb_interestudo " & _
                                         "(datpdp, codemprede, codemprepara, codcontamodal, intinter, valinter) " & _
                                         "VALUES (" & _
                                         "'" & Format(CDate(txtData.Text), "yyyyMMdd") & "', " & _
                                         "'" & Mid(cboIntercambio.Items(intJ).Value, 1, 2) & "', " & _
                                         "'" & Mid(cboIntercambio.Items(intJ).Value, 4, 2) & "', " & _
                                         "'" & Mid(cboIntercambio.Items(intJ).Value, 7, 2) & "', " & _
                                         "" & intI & ", " & _
                                         "" & "0" & ")"
                                Cmd.CommandText = strSql
                                Cmd.ExecuteNonQuery()
                            Next
                        Next
                    End If
                Else
                    If cboIntercambio.SelectedItem.Value <> "Todos" Then

                        'Eliminando os intercâmbios existentes
                        strSql = "DELETE FROM tb_interestudo " & _
                                 "WHERE datpdp = '" & Format(CDate(txtData.Text), "yyyyMMdd") & "' " & _
                                 "AND codemprede = '" & Mid(cboIntercambio.SelectedItem.Value, 1, 2) & "' " & _
                                 "AND codemprepara = '" & Mid(cboIntercambio.SelectedItem.Value, 4, 2) & "' " & _
                                 "AND codcontamodal = '" & Mid(cboIntercambio.SelectedItem.Value, 7, 2) & "'"
                        Cmd.CommandText = strSql
                        Cmd.ExecuteNonQuery()

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
                            'If blnIncluir Then

                            strSql = "INSERT INTO tb_interestudo " & _
                                    "(datpdp, codemprede, codemprepara, codcontamodal, intinter, valinter) " & _
                                    "VALUES (" & _
                                    "'" & Format(CDate(txtData.Text), "yyyyMMdd") & "', " & _
                                    "'" & Mid(cboIntercambio.SelectedItem.Value, 1, 2) & "', " & _
                                    "'" & Mid(cboIntercambio.SelectedItem.Value, 4, 2) & "', " & _
                                    "'" & Mid(cboIntercambio.SelectedItem.Value, 7, 2) & "', " & _
                                    "" & intI & ", " & _
                                    "" & intValor & ")"
                            Cmd.CommandText = strSql
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
                            intJ = 1
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
                            intInter = 1
                            Do While InStr(intColAnterior, strLinha, Chr(9), CompareMethod.Binary) <> 0
                                intFim = InStr(intColAnterior, strLinha, Chr(9), CompareMethod.Binary)

                                If Trim(Mid(strLinha, intColAnterior, intFim - intColAnterior)) <> "" Then
                                    intValor = Val(Mid(strLinha, intColAnterior, intFim - intColAnterior))
                                Else
                                    intValor = 0
                                End If
                                'If blnIncluir Then

                                strSql = "DELETE FROM tb_interestudo " & _
                                         "WHERE datpdp = '" & Format(CDate(txtData.Text), "yyyyMMdd") & "' " & _
                                         "AND codemprede = '" & Mid(cboIntercambio.Items(intInter).Value, 1, 2) & "' " & _
                                         "AND codemprepara = '" & Mid(cboIntercambio.Items(intInter).Value, 4, 2) & "' " & _
                                         "AND codcontamodal = '" & Mid(cboIntercambio.Items(intInter).Value, 7, 2) & "' " & _
                                         "AND intinter = " & intI

                                Cmd.CommandText = strSql
                                Cmd.ExecuteNonQuery()

                                strSql = "INSERT INTO tb_interestudo " & _
                                        "(datpdp, codemprede, codemprepara, codcontamodal, intinter, valinter) " & _
                                        "VALUES (" & _
                                        "'" & Format(CDate(txtData.Text), "yyyyMMdd") & "', " & _
                                        "'" & Mid(cboIntercambio.Items(intInter).Value, 1, 2) & "', " & _
                                        "'" & Mid(cboIntercambio.Items(intInter).Value, 4, 2) & "', " & _
                                        "'" & Mid(cboIntercambio.Items(intInter).Value, 7, 2) & "', " & _
                                        "" & intI & ", " & _
                                        "" & intValor & ")"
                                Cmd.CommandText = strSql
                                Cmd.ExecuteNonQuery()
                                intColAnterior = intFim + 1
                                intCelula += 1
                                intInter += 1
                            Loop

                            If Trim(Mid(strLinha, intColAnterior)) <> "" Then
                                intValor = Val(Mid(strLinha, intColAnterior))
                            Else
                                intValor = 0
                            End If
                            'If blnIncluir Then
                            strSql = "DELETE FROM tb_interestudo " & _
                                     "WHERE datpdp = '" & Format(CDate(txtData.Text), "yyyyMMdd") & "' " & _
                                     "AND codemprede = '" & Mid(cboIntercambio.Items(intInter).Value, 1, 2) & "' " & _
                                     "AND codemprepara = '" & Mid(cboIntercambio.Items(intInter).Value, 4, 2) & "' " & _
                                     "AND codcontamodal = '" & Mid(cboIntercambio.Items(intInter).Value, 7, 2) & "' " & _
                                     "AND intinter = " & intI

                            Cmd.CommandText = strSql
                            Cmd.ExecuteNonQuery()

                            strSql = "INSERT INTO tb_interestudo " & _
                                    "(datpdp, codemprede, codemprepara, codcontamodal, intinter, valinter) " & _
                                    "VALUES (" & _
                                    "'" & Format(CDate(txtData.Text), "yyyyMMdd") & "', " & _
                                    "'" & Mid(cboIntercambio.Items(intInter).Value, 1, 2) & "', " & _
                                    "'" & Mid(cboIntercambio.Items(intInter).Value, 4, 2) & "', " & _
                                    "'" & Mid(cboIntercambio.Items(intInter).Value, 7, 2) & "', " & _
                                    "" & intI & ", " & _
                                    "" & intValor & ")"
                            Cmd.CommandText = strSql
                            Cmd.ExecuteNonQuery()
                        Next
                    End If
                End If

                'Grava evento registrando o recebimento de Intercâmbio
                GravaEventoPDP("9", Format(CDate(txtData.Text), "yyyyMMdd"), cboEmpresa.SelectedItem.Text, datAtual, "PDPColInte", UsuarID)

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
                divValor.Visible = False
                btnSalvar.Visible = False
                cboIntercambio.SelectedIndex = 0
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

    Private Sub btnCalendario_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCalendario.Click
        calData.SelectedDate = CDate("01/01/1900")
        'If txtData.Text <> "" Then
        '    calData.SelectedDate = CType(txtData.Text, Date)
        'Else
        '    calData.SelectedDate = System.DateTime.Today.Date
        'End If
        divCal.Visible = True
    End Sub

    Private Sub calData_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calData.SelectionChanged
        txtData.Text = Format(calData.SelectedDate.Date, "dd/MM/yyyy")
        divCal.Visible = False
        If cboEmpresa.SelectedIndex > 0 Then
            cboEmpresa_SelectedIndexChanged(sender, e)
        End If
    End Sub

End Class


