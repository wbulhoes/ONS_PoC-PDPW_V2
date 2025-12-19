Partial Class frmCnsIR3
    Inherits System.Web.UI.Page
    Private strBase As String

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
        imgPlanilha.Visible = False
        lblMensagem.Visible = False
        If Page.Request.QueryString("strAcesso") = "PDOC" Then
            optDados.Visible = False
            optDados.SelectedIndex = 2
            strBase = "rpdp"
            lblData.Text = "Data do PDP"
        Else
            strBase = "pdp"
            lblData.Text = "Data do PDP"
        End If

        If Not Page.IsPostBack Then
            Dim intI As Integer
            Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
            Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
            Cmd.Connection = Conn
            Try
                Cmd.CommandText = "SELECT codempre, sigempre " &
                                  "FROM empre " &
                                  "ORDER BY sigempre"
                Conn.Open(strBase)
                Dim rsEmpresa As OnsClasses.OnsData.OnsDataReader = Cmd.ExecuteReader
                intI = 1
                Dim objItem As System.Web.UI.WebControls.ListItem
                objItem = New System.Web.UI.WebControls.ListItem
                objItem.Text = ""
                objItem.Value = "0"
                cboEmpresa.Items.Add(objItem)
                Do While rsEmpresa.Read
                    objItem = New System.Web.UI.WebControls.ListItem
                    objItem.Text = rsEmpresa.Item("sigempre")
                    objItem.Value = rsEmpresa.Item("codempre")
                    cboEmpresa.Items.Add(objItem)
                    If Trim(cboEmpresa.Items(intI).Value) = Trim(Session("strCodEmpre")) Then
                        cboEmpresa.SelectedIndex = intI
                    End If
                    intI = intI + 1
                Loop
                rsEmpresa.Close()
                rsEmpresa = Nothing
                Cmd.CommandText = "SELECT datpdp " &
                                  "FROM pdp " & GeraClausulaWHERE_DataPDP_PDO(Page.Request.QueryString("strAcesso")) &
                                  "ORDER BY datpdp DESC"
                Dim rsData As OnsClasses.OnsData.OnsDataReader = Cmd.ExecuteReader
                intI = 1
                objItem = New System.Web.UI.WebControls.ListItem
                objItem.Text = ""
                objItem.Value = "0"
                cboDataInicial.Items.Add(objItem)
                Do While rsData.Read
                    objItem = New System.Web.UI.WebControls.ListItem
                    objItem.Text = Mid(rsData("datpdp"), 7, 2) & "/" & Mid(rsData("datpdp"), 5, 2) & "/" & Mid(rsData("datpdp"), 1, 4)
                    objItem.Value = rsData("datpdp")
                    cboDataInicial.Items.Add(objItem)
                    If Trim(cboDataInicial.Items(intI).Value) = Format(Session("datEscolhida"), "yyyyMMdd") Then
                        cboDataInicial.SelectedIndex = intI
                    End If
                    intI = intI + 1
                Loop
                rsData.Close()
                rsData = Nothing

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
        End If
    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        If SessaoAtiva(Page.Session) Then
            MyBase.Render(writer)
            'objMenu.RenderControl(writer)
        End If
    End Sub

    Private Sub ConsultaIntervalo(ByVal strcampo As String)
        Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
        Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
        Cmd.Connection = Conn
        Dim objRow As TableRow
        Dim objCell As TableCell
        Dim strCodUsina As String
        Dim intI, intCol, intTotal, intHora, intTamanho, intJ As Integer
        Try
            'data pdp
            Cmd.CommandText = "SELECT d.codusina, d.intir3, d.valir3" & strcampo & " AS valor, u.ordem, u.nomusina " &
                              "FROM tb_IR3 d, usina u " &
                              "WHERE u.codempre = '" & cboEmpresa.SelectedItem.Value & "' " &
                              "AND u.codusina = d.codusina " &
                              "AND d.datpdp = '" & cboDataInicial.SelectedItem.Value & "' " &
                              "AND u.flg_recebepdpage = 'S' " &
                              "ORDER BY u.ordem, d.codusina, d.intir3"
            Conn.Open(strBase)
            Dim rsConsulta As OnsClasses.OnsData.OnsDataReader = Cmd.ExecuteReader

            Dim objTamanho As System.Web.UI.WebControls.Unit
            objTamanho = New Unit

            'Colocar cor na tabela (RGB(233, 244, 207) = cor padrão ONS)
            Dim Color As System.Drawing.Color
            Color = New System.Drawing.Color
            Color = System.Drawing.ColorTranslator.FromWin32(RGB(233, 244, 207))

            'nova Celula
            tblConsulta.Width = objTamanho.Pixel(100)
            intTamanho = 100
            objRow = New TableRow
            objRow.BackColor = System.Drawing.Color.YellowGreen
            objCell = New TableCell
            objCell.Font.Bold = True
            objCell.Width = objTamanho.Pixel(100)
            objCell.Text = "Intervalo"
            objRow.Controls.Add(objCell)

            objCell = New TableCell
            objCell.Font.Bold = True
            objCell.Width = objTamanho.Pixel(100)
            objCell.Text = "Total"
            objRow.Controls.Add(objCell)

            tblConsulta.Controls.Add(objRow)

            intHora = 0
            For intI = 1 To 48
                objRow = New TableRow
                If intI Mod 2 = 0 Then
                    'quando linha = par troca cor
                    objRow.BackColor = Color
                End If

                objCell = New TableCell
                objCell.Text = IntParaHora(intI)
                'If intI Mod 2 <> 0 Then
                '    objCell.Text = Format(intHora - 1, "00") & ":00" & "-" & Format(intHora, "00") & ":30"
                '    intHora = intHora + 1
                'Else
                '    objCell.Text = Format(intHora - 1, "00") & ":30" & "-" & Format(intHora, "00") & ":00"
                'End If
                objRow.Controls.Add(objCell)
                objCell = New TableCell
                objRow.Controls.Add(objCell)
                tblConsulta.Controls.Add(objRow)
            Next
            'Total
            objRow = New TableRow
            If intI Mod 2 = 0 Then
                'quando linha = par troca cor
                objRow.BackColor = Color
            End If
            objCell = New TableCell
            objCell.Text = "Total"
            objCell.Font.Bold = True
            objRow.Controls.Add(objCell)
            objCell = New TableCell
            objCell.Font.Bold = True
            objRow.Controls.Add(objCell)
            tblConsulta.Controls.Add(objRow)
            intI = intI + 1
            'Média
            objRow = New TableRow
            If intI Mod 2 = 0 Then
                'quando linha = par troca cor
                objRow.BackColor = Color
            End If
            objCell = New TableCell
            objCell.Text = "Média"
            objCell.Font.Bold = True
            objRow.Controls.Add(objCell)
            objCell = New TableCell
            objCell.Font.Bold = True
            objRow.Controls.Add(objCell)
            tblConsulta.Controls.Add(objRow)
            intI = 0
            strCodUsina = ""

            Dim strUsina As String = ""
            Dim intTamanhoColunaUsina As Integer = 0

            Do While rsConsulta.Read

                If Page.Request.QueryString("strAcesso") <> "PDOC" Then
                    strUsina = rsConsulta("codusina")
                    intTamanhoColunaUsina = 70
                Else
                    strUsina = rsConsulta("nomusina")
                    intTamanhoColunaUsina = 100
                End If

                If strCodUsina <> rsConsulta("codusina") Then
                    If intI <> 0 Then
                        objCell = New TableCell
                        objCell.Font.Bold = True
                        objCell.Text = intTotal / 2
                        tblConsulta.Rows(49).Controls.Add(objCell)

                        objCell = New TableCell
                        objCell.Font.Bold = True
                        objCell.Text = Int(IIf(intTotal = 0, intTotal, intTotal / 48))
                        tblConsulta.Rows(50).Controls.Add(objCell)
                        intTotal = 0
                    End If
                    intTamanho = intTamanho + intTamanhoColunaUsina
                    tblConsulta.Width = objTamanho.Pixel(intTamanho)
                    objCell = New TableCell
                    objCell.Font.Bold = True
                    objCell.Width = objTamanho.Pixel(intTamanhoColunaUsina)
                    'objCell.Text = rsConsulta("codusina")
                    objCell.Text = strUsina
                    tblConsulta.Rows(0).Controls.Add(objCell)
                    strCodUsina = rsConsulta("codusina")
                    intI = 1
                End If

                'Nova coluna da tabela
                objCell = New TableCell
                objCell.Wrap = False
                objCell.Text = IIf(Not IsDBNull(rsConsulta("valor")), rsConsulta("valor"), 0)
                intTotal = intTotal + objCell.Text
                tblConsulta.Rows(intI).Controls.Add(objCell)
                intI = intI + 1
            Loop

            If intI > 0 Then
                objCell = New TableCell
                objCell.Font.Bold = True
                objCell.Text = intTotal / 2
                tblConsulta.Rows(49).Controls.Add(objCell)

                objCell = New TableCell
                objCell.Font.Bold = True
                objCell.Text = Int(IIf(intTotal = 0, intTotal, intTotal / 48))
                tblConsulta.Rows(50).Controls.Add(objCell)
            End If
            rsConsulta.Close()

            If Page.Request.QueryString("strAcesso") <> "PDOC" Then
                If Trim(optDados.SelectedItem.Value) = "2" Then
                    rsConsulta.Close()
                    Cmd.CommandText = "SELECT DECODE(codstatu,99,'" & strMsgValidado & "','" & strMsgNaoValidado & "') AS msg " &
                                      "FROM pdp " &
                                      "WHERE datpdp = '" & cboDataInicial.SelectedItem.Value & "'"

                    rsConsulta = Cmd.ExecuteReader
                    Do While rsConsulta.Read
                        lblMensagem.Text = rsConsulta("msg")
                        lblMensagem.Visible = True
                    Loop
                    rsConsulta.Close()
                End If
            End If

            Conn.Close()

            intTotal = 0
            Dim intTotalT As Integer = 0
            For intI = 1 To 48
                For intJ = 2 To tblConsulta.Rows(0).Cells.Count - 1
                    intTotal = intTotal + tblConsulta.Rows(intI).Cells(intJ).Text
                Next
                tblConsulta.Rows(intI).Cells(1).Text = intTotal ' / 2
                intTotalT = intTotalT + intTotal
                intTotal = 0
            Next
            tblConsulta.Rows(49).Cells(1).Text = intTotalT / 2
            tblConsulta.Rows(50).Cells(1).Text = Int(intTotalT / 48)
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

    Private Overloads Sub btnVisualizar_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnVisualizar.Click
        Dim strcampo As String
        lblMensagem.Text = ""
        If cboDataInicial.SelectedIndex <> 0 Then
            Session("datEscolhida") = CDate(cboDataInicial.SelectedItem.Text)
        End If
        If cboEmpresa.SelectedIndex <> 0 Then
            Session("strCodEmpre") = cboEmpresa.SelectedItem.Value
        End If

        ' Verifica qual dados deverá ser consultado
        If Trim(optDados.SelectedItem.Value) = "0" Then
            strcampo = "tran"
        ElseIf Trim(optDados.SelectedItem.Value) = "1" Then
            strcampo = "emp"
        ElseIf Trim(optDados.SelectedItem.Value) = "2" Then
            strcampo = "pro"
        ElseIf Trim(optDados.SelectedItem.Value) = "3" Then
            strcampo = "pre"
        ElseIf Trim(optDados.SelectedItem.Value) = "4" Then
            strcampo = "sup"
        End If

        'intervalo
        ConsultaIntervalo(strcampo)

        'Chama uma nova janela para exibir a planilha e passa os parâmetros por query string
        imgPlanilha.Attributes.Add("onclick",
                                   "window.open('frmPlanilha.aspx?" &
                                   "strDataPDP=" & cboDataInicial.SelectedValue & "&" &
                                   "strCampo=" & strcampo & "&" &
                                   "strEmpresa=" & cboEmpresa.SelectedValue.Trim & "|" & cboEmpresa.SelectedItem.Text.Trim & "&" &
                                   "strTabela=IR3&" &
                                   "strBase=" & strBase & "&" &
                                   "strAcesso=" & Page.Request.QueryString("strAcesso") & "'" &
                                   ",'Planilha','height = 600, width = 850, toolbar=yes,location=no,status=no,menubar=yes,scrollbars=yes,scrolling=yes,resizebled=yes');")
        imgPlanilha.Visible = True
    End Sub
End Class

