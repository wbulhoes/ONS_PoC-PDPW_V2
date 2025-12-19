Partial Class frmCnsBalanco

    Inherits System.Web.UI.Page
    Private strBase As String

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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
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
                                  "FROM pdp " & GeraClausulaWHERE_DataPDP_PDO(Page.Request.QueryString("strAcesso")) & _
                                  "ORDER BY datpdp DESC"
                Dim rsData As OnsClasses.OnsData.OnsDataReader = Cmd.ExecuteReader
                intI = 1
                objItem = New System.Web.UI.WebControls.ListItem
                objItem.Text = ""
                objItem.Value = "0"
                cboDataInicial.Items.Add(objItem)
                Do While rsData.Read
                    objItem = New System.Web.UI.WebControls.ListItem
                    objItem.Text = Mid(rsData("datpdp"), 7, 2) & "/" & _
                                Mid(rsData("datpdp"), 5, 2) & "/" & _
                                Mid(rsData("datpdp"), 1, 4)
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
                'houve erro, aborta a transação e fecha a conexão
                Session("strMensagem") = "Não foi possível o acesso a Base de Dados."
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
        'Dim strCodUsina As String
        Dim intI, intCol, intTotal, intHora, intTamanho, intJ As Integer

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
            'objCell.Text = Format(intHora - 1, "00") & ":00" & "-" & Format(intHora, "00") & ":30"
            '  intHora = intHora + 1
            '  Else
            ' objCell.Text = Format(intHora - 1, "00") & ":30" & "-" & Format(intHora, "00") & ":00"
            ' End If
            objRow.Controls.Add(objCell)
            tblConsulta.Controls.Add(objRow)
        Next

        'Total
        objRow = New TableRow
        objRow.Font.Bold = True
        If intI Mod 2 = 0 Then
            'quando linha = par troca cor
            objRow.BackColor = Color
        End If
        objCell = New TableCell
        objCell.Text = "Total"
        objRow.Controls.Add(objCell)
        tblConsulta.Controls.Add(objRow)
        intI = intI + 1
        'Média
        objRow = New TableRow
        objRow.Font.Bold = True
        If intI Mod 2 = 0 Then
            'quando linha = par troca cor
            objRow.BackColor = Color
        End If
        objCell = New TableCell
        objCell.Text = "Média"
        objRow.Controls.Add(objCell)
        tblConsulta.Controls.Add(objRow)
        Try
            ' ###### GERAÇÃO ######
            Cmd.CommandText = "SELECT SUM(g.valdespa" & strcampo & ") AS valdespaemp, g.intdespa " &
                              "FROM usina u, despa g " &
                              "WHERE u.codempre = '" & cboEmpresa.SelectedItem.Value & "' " &
                              "AND u.codusina = g.codusina " &
                              "AND g.datpdp = '" & cboDataInicial.SelectedItem.Value & "' " &
                              "AND u.flg_recebepdpage = 'S' " &
                              "GROUP BY intdespa " &
                              "ORDER BY intdespa"
            Conn.Open(strBase)
            Dim rsConsulta As OnsClasses.OnsData.OnsDataReader = Cmd.ExecuteReader
            intTamanho = intTamanho + 100
            tblConsulta.Width = objTamanho.Pixel(intTamanho)
            objCell = New TableCell
            objCell.Width = objTamanho.Pixel(100)
            objCell.Font.Bold = True
            objCell.Text = "Geração"
            tblConsulta.Rows(0).Controls.Add(objCell)
            intI = 1
            intTotal = 0
            Do While rsConsulta.Read
                objCell = New TableCell
                objCell.Wrap = False
                objCell.Text = IIf(Not IsDBNull(rsConsulta("valdespaemp")), rsConsulta("valdespaemp"), 0)
                intTotal = intTotal + objCell.Text
                tblConsulta.Rows(intI).Controls.Add(objCell)
                intI = intI + 1
            Loop

            If intI > 0 Then
                For intI = intI To 48
                    objCell = New TableCell
                    objCell.Text = 0
                    tblConsulta.Rows(intI).Controls.Add(objCell)
                Next
                objCell = New TableCell
                objCell.Font.Bold = True
                objCell.Text = intTotal / 2
                tblConsulta.Rows(49).Controls.Add(objCell)

                objCell = New TableCell
                objCell.Font.Bold = True
                objCell.Text = Int(intTotal / 48)
                tblConsulta.Rows(50).Controls.Add(objCell)
            End If
            rsConsulta.Close()

            ' ###### CARGA ######
            Cmd.CommandText = "SELECT valcarga" & strcampo & " AS valor, intcarga " & _
                              "FROM carga " & _
                              "WHERE codempre = '" & cboEmpresa.SelectedItem.Value & "' " & _
                              "AND datpdp = '" & cboDataInicial.SelectedItem.Value & "' " & _
                              "ORDER BY intcarga"
            rsConsulta = Cmd.ExecuteReader
            intTamanho = intTamanho + 100
            tblConsulta.Width = objTamanho.Pixel(intTamanho)
            objCell = New TableCell
            objCell.Width = objTamanho.Pixel(100)
            objCell.Font.Bold = True
            objCell.Text = "Carga"
            tblConsulta.Rows(0).Controls.Add(objCell)
            intI = 1
            intTotal = 0
            Do While rsConsulta.Read
                objCell = New TableCell
                objCell.Wrap = False
                objCell.Text = IIf(Not IsDBNull(rsConsulta("valor")), rsConsulta("valor"), 0)
                intTotal = intTotal + objCell.Text
                tblConsulta.Rows(intI).Controls.Add(objCell)
                intI = intI + 1
            Loop

            If intI > 0 Then
                For intI = intI To 48
                    objCell = New TableCell
                    objCell.Text = 0
                    tblConsulta.Rows(intI).Controls.Add(objCell)
                Next
                'Total
                objCell = New TableCell
                objCell.Font.Bold = True
                objCell.Text = intTotal / 2
                tblConsulta.Rows(intI).Controls.Add(objCell)
                intI = intI + 1
                'Média
                objCell = New TableCell
                objCell.Font.Bold = True
                objCell.Text = Int(intTotal / 48)
                tblConsulta.Rows(intI).Controls.Add(objCell)
            End If
            rsConsulta.Close()

            ' ###### INTERCÂMBIO ######
            Cmd.CommandText = "SELECT SUM(valinter" & strcampo & ") AS valinteremp, intinter " & _
                              "FROM inter " & _
                              "WHERE codemprede = '" & cboEmpresa.SelectedItem.Value & "' " & _
                              "AND datpdp = '" & cboDataInicial.SelectedItem.Value & "' " & _
                              "GROUP BY intinter " & _
                              "ORDER BY intinter"
            rsConsulta = Cmd.ExecuteReader
            intTamanho = intTamanho + 100
            tblConsulta.Width = objTamanho.Pixel(intTamanho)
            objCell = New TableCell
            objCell.Width = objTamanho.Pixel(100)
            objCell.Font.Bold = True
            objCell.Text = "Intercâmbio"
            tblConsulta.Rows(0).Controls.Add(objCell)
            intI = 1
            intTotal = 0
            Do While rsConsulta.Read
                objCell = New TableCell
                objCell.Wrap = False
                objCell.Text = IIf(Not IsDBNull(rsConsulta("valinteremp")), rsConsulta("valinteremp"), 0)
                intTotal = intTotal + objCell.Text
                tblConsulta.Rows(intI).Controls.Add(objCell)
                'Adiciona a linha a tabela
                intI = intI + 1
            Loop

            If intI > 0 Then
                For intI = intI To 48
                    objCell = New TableCell
                    objCell.Text = 0
                    tblConsulta.Rows(intI).Controls.Add(objCell)
                Next
                'Total
                objCell = New TableCell
                objCell.Font.Bold = True
                objCell.Text = intTotal / 2
                tblConsulta.Rows(intI).Controls.Add(objCell)
                intI = intI + 1
                'Média
                objCell = New TableCell
                objCell.Font.Bold = True
                objCell.Text = Int(intTotal / 48)
                tblConsulta.Rows(intI).Controls.Add(objCell)
            End If
            rsConsulta.Close()
            Conn.Close()
        Catch
            Session("strMensagem") = "Não foi possível acessar a Base de Dados."
            Conn.Close()
            Response.Redirect("frmMensagem.aspx")
        End Try
        'Fechamento
        FechamentoBalanco(intTamanho)

        If Page.Request.QueryString("strAcesso") <> "PDOC" Then
            Try
                Conn.Open(strBase)
                Dim rsConsulta As OnsClasses.OnsData.OnsDataReader = Cmd.ExecuteReader

                If Trim(optDados.SelectedItem.Value) = "2" Then
                    rsConsulta.Close()

                    Cmd.CommandText = "SELECT DECODE(codstatu,99,'" & strMsgValidado & "', '" & strMsgNaoValidado & "') AS msg " & _
                                      "FROM pdp " & _
                                      "WHERE datpdp = '" & cboDataInicial.SelectedItem.Value & "'"
                    rsConsulta = Cmd.ExecuteReader
                    Do While rsConsulta.Read
                        lblMensagem.Text = rsconsulta("msg")
                        lblMensagem.Visible = True
                    Loop
                    rsConsulta.Close()
                End If
                Conn.Close()

            Catch
                Session("strMensagem") = "Não foi possível acessar a Base de Dados."
                Conn.Close()
                Response.Redirect("frmMensagem.aspx")
            End Try

        End If

    End Sub

    Private Sub FechamentoBalanco(ByVal intTamanho As Integer)
        'Verficando Balanço (Fechamento)
        Dim intTotal, inti As Integer
        Dim objRow As TableRow
        Dim objCell As TableCell

        Dim objTamanho As System.Web.UI.WebControls.Unit
        objTamanho = New Unit
        inti = 0
        intTotal = 0
        intTamanho = intTamanho + 100
        tblConsulta.Width = objTamanho.Pixel(intTamanho)
        objCell = New TableCell
        objCell.Font.Bold = True
        objCell.Width = objTamanho.Pixel(100)
        objCell.Text = "Fechamento"
        tblConsulta.Rows(0).Controls.Add(objCell)
        For inti = 1 To tblConsulta.Rows.Count - 3
            objCell = New TableCell
            objCell.Text = (Val(tblConsulta.Rows(inti).Cells(2).Text) + _
                            Val(tblConsulta.Rows(inti).Cells(3).Text)) - _
                            Val(tblConsulta.Rows(inti).Cells(1).Text)
            tblConsulta.Rows(inti).Controls.Add(objCell)
            intTotal = intTotal + Val(tblConsulta.Rows(inti).Cells(4).Text)
        Next
        'Total
        objCell = New TableCell
        objCell.Font.Bold = True

        'intervalo
        objCell.Text = intTotal / 2
        tblConsulta.Rows(inti).Controls.Add(objCell)
        'Média
        objCell = New TableCell
        objCell.Font.Bold = True
        objCell.Text = IIf(tblConsulta.Rows.Count <> 0, Int(intTotal / tblConsulta.Rows.Count), 0)
        tblConsulta.Rows(inti + 1).Controls.Add(objCell)
    End Sub

    Private Overloads Sub btnVisualizar_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnVisualizar.Click
        Dim strcampo As String
        lblMensagem.Visible = False
        If cboDataInicial.SelectedIndex <> 0 Then
            Session("datEscolhida") = CDate(cboDataInicial.SelectedItem.Text)
        End If
        If cboEmpresa.SelectedIndex <> 0 Then
            Session("strCodEmpre") = cboEmpresa.SelectedItem.Value
        End If

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

        'Consulta por intervalo
        ConsultaIntervalo(strcampo)

        'Chama uma nova janela para exibir o gráfico e passa os parâmetros por query string
        imgPlanilha.Attributes.Add("onclick", _
                                   "window.open('frmPlanilha.aspx?" & _
                                   "strDataPDP=" & cboDataInicial.SelectedValue & "&" & _
                                   "strCampo=" & strcampo & "&" & _
                                   "strEmpresa=" & cboEmpresa.SelectedValue.Trim & "|" & cboEmpresa.SelectedItem.Text.Trim & "&" & _
                                   "strTabela=balanco&" & _
                                   "strBase=" & strBase & "&" & _
                                   "strAcesso=" & Page.Request.QueryString("strAcesso") & "'" & _
                                   ",'Planilha','height = 600, width = 850, toolbar=yes,location=no,status=no,menubar=yes,scrollbars=yes,scrolling=yes,resizebled=yes');")
        imgPlanilha.Visible = True

    End Sub
End Class
