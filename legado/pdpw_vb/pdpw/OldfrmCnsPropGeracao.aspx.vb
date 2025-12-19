Partial Class frmCnsPropGeracao
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
            optDados.SelectedIndex = 0
            strBase = "rpdp"
        Else
            strBase = "pdp"
        End If
        If Not Page.IsPostBack Then
            Dim intI As Integer
            Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
            Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
            Cmd.Connection = Conn
            Try
                Cmd.CommandText = "SELECT codempre, sigempre " & _
                                  "FROM empre " & _
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
                Cmd.CommandText = "SELECT datpdp " & _
                                  "FROM pdp " & _
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
        Dim intI, intCol, intTotal, intHora, intTamanho, intQtdReg, intJ As Integer
        Try
            'Intervalo
            Cmd.CommandText = "SELECT n.ipat, n.patini, n.patfim, n.nivcarga " & _
                              "FROM pdp p, nivcarga_ppg n " & _
                              "WHERE p.datpdp = '" & cboDataInicial.SelectedItem.Value & "' " & _
                              "AND p.codtipdia = n.codtipdia " & _
                              "ORDER BY n.ipat"
            Conn.Open(strBase)
            Dim rsConsulta As OnsClasses.OnsData.OnsDataReader = Cmd.ExecuteReader
            Dim objTamanho As System.Web.UI.WebControls.Unit
            objTamanho = New Unit

            'Colocar cor na tabela (RGB(233, 244, 207) = cor padrão ONS)
            Dim Color As System.Drawing.Color
            Color = New System.Drawing.Color
            Color = System.Drawing.ColorTranslator.FromWin32(RGB(233, 244, 207))
            intI = 0
            tblConsulta.Width = objTamanho.Pixel(360)
            Do While rsConsulta.Read
                If intI = 0 Then
                    objRow = New TableRow
                    objRow.BackColor = System.Drawing.Color.YellowGreen
                    'Patamar
                    objCell = New TableCell
                    objCell.Font.Bold = True
                    objCell.Width = objTamanho.Pixel(70)
                    objCell.Text = "Patamar"
                    objCell.HorizontalAlign = HorizontalAlign.Center
                    objRow.Controls.Add(objCell)
                    'Hora Início
                    objCell = New TableCell
                    objCell.Font.Bold = True
                    objCell.Width = objTamanho.Pixel(70)
                    objCell.Text = "Hora Início"
                    objCell.HorizontalAlign = HorizontalAlign.Center
                    objRow.Controls.Add(objCell)
                    'Hora Fim
                    objCell = New TableCell
                    objCell.Font.Bold = True
                    objCell.Width = objTamanho.Pixel(70)
                    objCell.Text = "Hora Fim"
                    objCell.HorizontalAlign = HorizontalAlign.Center
                    objRow.Controls.Add(objCell)
                    'Perfil Carga
                    objCell = New TableCell
                    objCell.Font.Bold = True
                    objCell.Width = objTamanho.Pixel(80)
                    objCell.Text = "Perfil Carga"
                    objCell.HorizontalAlign = HorizontalAlign.Center
                    objRow.Controls.Add(objCell)
                    'Total
                    objCell = New TableCell
                    objCell.Font.Bold = True
                    objCell.Width = objTamanho.Pixel(70)
                    objCell.Text = "Total"
                    objCell.HorizontalAlign = HorizontalAlign.Right
                    objRow.Controls.Add(objCell)
                    tblConsulta.Rows.Add(objRow)
                    intI = intI + 1
                End If
                'Nova linha da tabela somente na primeira passagem
                objRow = New TableRow
                If intI Mod 2 = 0 Then
                    'quando linha = par troca cor
                    objRow.BackColor = Color
                End If
                'Patamar
                objCell = New TableCell
                objCell.Wrap = False
                objCell.Text = rsConsulta("ipat")
                objCell.HorizontalAlign = HorizontalAlign.Center
                objRow.Controls.Add(objCell)

                'Hora Início
                objCell = New TableCell
                objCell.Wrap = False
                objCell.Text = rsConsulta("patini")
                objCell.HorizontalAlign = HorizontalAlign.Center
                objRow.Controls.Add(objCell)

                'Hora Fim
                objCell = New TableCell
                objCell.Wrap = False
                objCell.Text = rsConsulta("patfim")
                objCell.HorizontalAlign = HorizontalAlign.Center
                objRow.Controls.Add(objCell)

                'Perfil Carga
                objCell = New TableCell
                objCell.Wrap = False
                objCell.Text = rsConsulta("nivcarga")
                objCell.HorizontalAlign = HorizontalAlign.Center
                objRow.Controls.Add(objCell)

                'Total
                objCell = New TableCell
                objRow.Controls.Add(objCell)
                objCell.HorizontalAlign = HorizontalAlign.Right
                tblConsulta.Rows.Add(objRow)
                intI = intI + 1
            Loop
            rsConsulta.Close()
            intQtdReg = intI - 1

            Cmd.CommandText = "SELECT n.ipat, n.patini, n.patfim, n.nivcarga, d.codusina, d.valppgpro, u.ordem " &
                              "FROM usina u, despa_ppg d, pdp p, nivcarga_ppg n " &
                              "WHERE u.codempre = '" & cboEmpresa.SelectedItem.Value & "' " &
                              "AND u.codusina = d.codusina " &
                              "AND d.datpdp = '" & cboDataInicial.SelectedItem.Value & "' " &
                              "AND d.datpdp = p.datpdp " &
                              "AND p.codtipdia = n.codtipdia " &
                              "AND n.ipat = d.ipat " &
                              "AND u.flg_recebepdpage = 'S' " &
                              "ORDER BY u.ordem, d.codusina, n.ipat"
            rsConsulta = Cmd.ExecuteReader
            intTamanho = 360
            Do While rsConsulta.Read
                If strCodUsina <> rsConsulta("codusina") Then
                    If intI > 0 Then
                        Do While intI <= intQtdReg
                            objCell = New TableCell
                            tblConsulta.Rows(intI).Controls.Add(objCell)
                            intI = intI + 1
                        Loop
                    End If
                    intTamanho = intTamanho + 70
                    tblConsulta.Width = objTamanho.Pixel(intTamanho)
                    objCell = New TableCell
                    objCell.Font.Bold = True
                    objCell.Width = objTamanho.Pixel(70)
                    objCell.Text = rsConsulta("codusina")
                    objCell.HorizontalAlign = HorizontalAlign.Right
                    tblConsulta.Rows(0).Controls.Add(objCell)
                    strCodUsina = rsConsulta("codusina")
                    intTotal = 0
                    intI = 1
                End If
                Do While Trim(tblConsulta.Rows(intI).Cells(0).Text) <> rsConsulta("ipat")
                    objCell = New TableCell
                    tblConsulta.Rows(intI).Controls.Add(objCell)
                    intI = intI + 1
                Loop
                objCell = New TableCell
                objCell.Wrap = False
                objCell.Text = IIf(Not IsDBNull(rsConsulta("valppgpro")), rsConsulta("valppgpro"), 0)
                objCell.HorizontalAlign = HorizontalAlign.Right
                tblConsulta.Rows(intI).Controls.Add(objCell)
                'intTotal = intTotal + objCell.Text
                intI = intI + 1
            Loop

            If Trim(optDados.SelectedItem.Value) = "2" Then
                rsConsulta.Close()

                Cmd.CommandText = "SELECT DECODE(codstatu,99,'" & strMsgValidado & "','" & strMsgNaoValidado & "') AS msg " & _
                                  "FROM pdp " & _
                                  "WHERE datpdp = '" & cboDataInicial.SelectedItem.Value & "'"

                rsConsulta = Cmd.ExecuteReader
                Do While rsConsulta.Read
                    lblMensagem.Text = rsConsulta("msg")
                    lblMensagem.Visible = True
                Loop
                rsConsulta.Close()
            End If

            rsConsulta.Close()
            Conn.Close()

            'Dim intTotalT As Integer = 0
            For intI = 1 To tblConsulta.Rows.Count - 1
                intTotal = 0
                For intJ = 5 To tblConsulta.Rows(0).Cells.Count - 1
                    intTotal = intTotal + tblConsulta.Rows(intI).Cells(intJ).Text
                Next
                tblConsulta.Rows(intI).Cells(4).Text = intTotal
            Next
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
        Dim strCampo As String
        lblMensagem.Visible = False
        If cboDataInicial.SelectedIndex <> 0 Then
            Session("datEscolhida") = CDate(cboDataInicial.SelectedItem.Text)
        End If
        If cboEmpresa.SelectedIndex <> 0 Then
            Session("strCodEmpre") = cboEmpresa.SelectedItem.Value
        End If
        'Somente serão consultados os dados consolidados
        strCampo = "pro"
        'intervalo
        ConsultaIntervalo(strCampo)

        'Chama uma nova janela para exibir a planilha e passa os parâmetros por query string
        imgPlanilha.Attributes.Add("onclick", _
                                   "window.open('frmPlanilha.aspx?" & _
                                   "strDataPDP=" & cboDataInicial.SelectedValue & "&" & _
                                   "strCampo=" & strCampo & "&" & _
                                   "strEmpresa=" & cboEmpresa.SelectedValue.Trim & "|" & cboEmpresa.SelectedItem.Text.Trim & "&" & _
                                   "strTabela=PGA&" & _
                                   "strBase=" & strBase & "&" & _
                                   "strAcesso=" & Page.Request.QueryString("strAcesso") & "'" & _
                                   ",'Planilha','height = 600, width = 850, toolbar=yes,location=no,status=no,menubar=yes,scrollbars=yes,scrolling=yes,resizebled=yes');")
        imgPlanilha.Visible = True
    End Sub
End Class
