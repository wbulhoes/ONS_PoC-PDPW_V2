Partial Class frmCnsRestricao
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

    Private Sub Consulta()
        Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
        Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
        Cmd.Connection = Conn
        Dim objRow As TableRow
        Dim objCell As TableCell
        Dim intI, intCol, intTotal, intHora As Integer
        Dim arrIntervalo(48) As String

        intHora = 0
        For intI = 1 To 48
            arrIntervalo(intI) = IntParaHora(intI)
            'If intI Mod 2 <> 0 Then
            '    arrIntervalo(intI) = Format(intHora - 1, "00") & ":00" & "-" & Format(intHora, "00") & ":30"
            '    intHora = intHora + 1
            'Else
            '    arrIntervalo(intI) = Format(intHora - 1, "00") & ":30" & "-" & Format(intHora, "00") & ":00"
            'End If
        Next
        Try
            ', nvl(r.obsrestr,'') as obsrestr " & _

            Cmd.CommandText = "SELECT r.codrestr, u.codempre, u.codusina, u.nomusina, g.siggerad, r.datinirestr, " &
                              "r.intinirestr, r.datfimrestr, r.intfimrestr, r.valrestr, " &
                              "isnull(m.dsc_motivorestricao,'') as dsc_motivorestricao " &
                              "FROM " & optDados.SelectedValue & " r, gerad g, usina u, outer tb_motivorestricao m " &
                              "WHERE u.codusina = g.codusina " &
                              "AND g.codgerad = r.codgerad AND m.id_motivorestricao = r.id_motivorestricao " &
                              "AND u.flg_recebepdpage = 'S' "
            If cboEmpresa.SelectedIndex <> 0 Then
                Cmd.CommandText = Cmd.CommandText & "AND u.codempre = '" & cboEmpresa.SelectedItem.Value & "' "
            End If
            If cboDataInicial.SelectedIndex <> 0 Then
                Cmd.CommandText = Cmd.CommandText & "AND '" & cboDataInicial.SelectedItem.Value & "' BETWEEN datinirestr " &
                                                    "AND datfimrestr "
            End If

            ', nvl(r.obsrestr,'') as obsrestr " & _

            Cmd.CommandText &= "UNION " &
                               "SELECT r.codrestr, u.codempre, u.codusina, u.nomusina, '', r.datinirestr, " &
                               "r.intinirestr, r.datfimrestr, r.intfimrestr, r.valrestr, " &
                               "isnull(m.dsc_motivorestricao,'') as dsc_motivorestricao " &
                               "FROM "
            Select Case optDados.SelectedValue
                Case Is = "restrgerademp"
                    Cmd.CommandText &= "restrusinaemp"
                Case Is = "temprestrgerad"
                    Cmd.CommandText &= "temprestrusina"
                Case Is = "restrgerad"
                    Cmd.CommandText &= "restrusina"
            End Select
            Cmd.CommandText &= " r, usina u, outer tb_motivorestricao m " &
                               "WHERE u.codusina = r.codusina AND m.id_motivorestricao = r.id_motivorestricao " &
                               "AND u.flg_recebepdpage = 'S' "
            If cboEmpresa.SelectedIndex <> 0 Then
                Cmd.CommandText = Cmd.CommandText & "AND u.codempre = '" & cboEmpresa.SelectedItem.Value & "' "
            End If
            If cboDataInicial.SelectedIndex <> 0 Then
                Cmd.CommandText = Cmd.CommandText & "AND '" & cboDataInicial.SelectedItem.Value & "' BETWEEN datinirestr " & _
                                                    "AND datfimrestr "
            End If
            Cmd.CommandText = Cmd.CommandText & "ORDER BY 3, 5, 6"

            Conn.Open(strBase)
            Dim rsConsulta As OnsClasses.OnsData.OnsDataReader = Cmd.ExecuteReader
            intI = 0
            intTotal = 0

            Dim objTamanho As System.Web.UI.WebControls.Unit
            objTamanho = New Unit

            'Colocar cor na tabela (RGB(233, 244, 207) = cor padrão ONS)
            Dim Color As System.Drawing.Color
            Color = New System.Drawing.Color
            Color = System.Drawing.ColorTranslator.FromWin32(RGB(233, 244, 207))

            Dim strUsina As String = ""
            
            tblConsulta.Width = objTamanho.Pixel(820)
            Do While rsConsulta.Read

                If Page.Request.QueryString("strAcesso") <> "PDOC" Then
                    strUsina = rsConsulta("codusina")
                Else
                    strUsina = rsConsulta("nomusina")
                End If

                If intI = 0 Then
                    objRow = New TableRow
                    objRow.Font.Bold = True
                    objRow.BackColor = System.Drawing.Color.YellowGreen
                    'Código
                    objCell = New TableCell
                    objCell.Width = objTamanho.Pixel(50)
                    objCell.Text = "Código"
                    objRow.Controls.Add(objCell)

                    'Empresa
                    objCell = New TableCell
                    objCell.HorizontalAlign = HorizontalAlign.Center
                    objCell.Width = objTamanho.Pixel(70)
                    objCell.Text = "Empresa"
                    objRow.Controls.Add(objCell)

                    'Usina
                    objCell = New TableCell
                    objCell.Width = objTamanho.Pixel(70)
                    objCell.Text = "Usina"
                    objRow.Controls.Add(objCell)

                    'UG
                    objCell = New TableCell
                    objCell.Width = objTamanho.Pixel(50)
                    objCell.Text = "UG"
                    objRow.Controls.Add(objCell)

                    'Início
                    objCell = New TableCell
                    objCell.Width = objTamanho.Pixel(80)
                    objCell.Text = "Início"
                    objRow.Controls.Add(objCell)

                    'Interv. Inicial
                    objCell = New TableCell
                    objCell.Width = objTamanho.Pixel(100)
                    objCell.Text = "Interv. Inicial"
                    objRow.Controls.Add(objCell)

                    'Fim
                    objCell = New TableCell
                    objCell.Width = objTamanho.Pixel(80)
                    objCell.Text = "Fim"
                    objRow.Controls.Add(objCell)

                    'Interv. Final
                    objCell = New TableCell
                    objCell.Width = objTamanho.Pixel(100)
                    objCell.Text = "Interv. Final"
                    objRow.Controls.Add(objCell)

                    'Valor Restrição
                    objCell = New TableCell
                    objCell.HorizontalAlign = HorizontalAlign.Center
                    objCell.Width = objTamanho.Pixel(70)
                    objCell.Text = "Valor da Restrição"
                    objRow.Controls.Add(objCell)

                    'Motivo Restrição
                    objCell = New TableCell
                    objCell.Width = objTamanho.Pixel(180)
                    objCell.Text = "Motivo Restrição"
                    objRow.Controls.Add(objCell)

                    'Observação
                    'objCell = New TableCell
                    'objCell.Width = objTamanho.Pixel(230)
                    'objCell.Text = "Observação"
                    'objRow.Controls.Add(objCell)

                    tblConsulta.Rows.Add(objRow)
                    intI = intI + 1
                End If

                'Nova linha da tabela
                objRow = New TableRow
                If intI Mod 2 = 0 Then
                    'quando linha = par troca cor
                    objRow.BackColor = Color
                End If
                'Código
                objCell = New TableCell
                objCell.Width = objTamanho.Pixel(50)
                objCell.Text = rsConsulta("codrestr")
                objRow.Controls.Add(objCell)
                'Empresa
                objCell = New TableCell
                objCell.Width = objTamanho.Pixel(70)
                objCell.HorizontalAlign = HorizontalAlign.Center
                objCell.Text = rsConsulta("codempre")
                objRow.Controls.Add(objCell)
                'Usina
                objCell = New TableCell
                objCell.Width = objTamanho.Pixel(100)
                'objCell.Text = rsConsulta("codusina")
                objCell.Text = strUsina
                objRow.Controls.Add(objCell)
                'UG
                objCell = New TableCell
                objCell.Width = objTamanho.Pixel(50)
                objCell.Text = rsConsulta("siggerad")
                objRow.Controls.Add(objCell)
                'Início
                objCell = New TableCell
                objCell.Width = objTamanho.Pixel(80)
                objCell.Text = Mid(rsConsulta("datinirestr"), 7, 2) & "/" & _
                            Mid(rsConsulta("datinirestr"), 5, 2) & "/" & _
                            Mid(rsConsulta("datinirestr"), 1, 4)
                objRow.Controls.Add(objCell)
                'Interv. Inicial
                objCell = New TableCell
                objCell.Width = objTamanho.Pixel(70)
                objCell.Text = arrIntervalo(rsConsulta("intinirestr"))
                objRow.Controls.Add(objCell)
                'Fim
                objCell = New TableCell
                objCell.Width = objTamanho.Pixel(80)
                objCell.Text = Mid(rsConsulta("datfimrestr"), 7, 2) & "/" & _
                            Mid(rsConsulta("datfimrestr"), 5, 2) & "/" & _
                            Mid(rsConsulta("datfimrestr"), 1, 4)
                objRow.Controls.Add(objCell)
                'Interv. Final
                objCell = New TableCell
                objCell.Width = objTamanho.Pixel(70)
                objCell.Text = arrIntervalo(rsConsulta("intfimrestr"))
                objRow.Controls.Add(objCell)
                'Retorno
                objCell = New TableCell
                objCell.Width = objTamanho.Pixel(70)
                objCell.HorizontalAlign = HorizontalAlign.Right
                objCell.Text = rsConsulta("valrestr")
                objRow.Controls.Add(objCell)

                'Motivo restrição
                objCell = New TableCell
                objCell.Width = objTamanho.Pixel(120)
                objCell.Text = rsConsulta("dsc_motivorestricao")
                objRow.Controls.Add(objCell)

                'Observação
                'objCell = New TableCell
                'objCell.Width = objTamanho.Pixel(230)
                'objCell.Text = rsConsulta("obsrestr")
                'objRow.Controls.Add(objCell)

                'Adiciona a linha a tabela
                tblConsulta.Rows.Add(objRow)
                intI = intI + 1
            Loop
            rsConsulta.Close()
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

    Private Overloads Sub btnVisualizar_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnVisualizar.Click
        If cboDataInicial.SelectedIndex <> 0 Then
            Session("datEscolhida") = CDate(cboDataInicial.SelectedItem.Text)
        End If
        If cboEmpresa.SelectedIndex <> 0 Then
            Session("strCodEmpre") = cboEmpresa.SelectedItem.Value
        End If
        If optDados.SelectedItem.Text <> "" Then
            Consulta()

            'Chama uma nova janela para exibir a planilha e passa os parâmetros por query string
            imgPlanilha.Attributes.Add("onclick", _
                                       "window.open('frmPlanilha.aspx?" & _
                                       "strDataPDP=" & cboDataInicial.SelectedValue & "&" & _
                                       "strCampo=" & optDados.SelectedItem.Value & "&" & _
                                       "strEmpresa=" & cboEmpresa.SelectedValue.Trim & "|" & cboEmpresa.SelectedItem.Text.Trim & "&" & _
                                       "strTabela=RES&" & _
                                       "strBase=" & strBase & "&" & _
                                       "strAcesso=" & Page.Request.QueryString("strAcesso") & "'" & _
                                       ",'Planilha','height = 600, width = 850, toolbar=yes,location=no,status=no,menubar=yes,scrollbars=yes,scrolling=yes,resizebled=yes');")
            imgPlanilha.Visible = True

        End If
    End Sub
End Class
