Partial Class frmCnsCarga

    Inherits System.Web.UI.Page
    Protected WithEvents Button1 As System.Web.UI.WebControls.Button
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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load, Me.Load
        'objMenu = CType(Session("onsmenu"), OnsWebControls.OnsMenu)
        imgPlanilha.Visible = False
        lblMensagem.Visible = False
        If Page.Request.QueryString("strAcesso") = "PDOC" Then
            optDados.Visible = False
            optDados.SelectedIndex = 2
            strBase = "pdp"
            lblData.Text = "Data do PDP"
        Else
            strBase = "pdp"
            lblData.Text = "Data do PDP"
        End If

        If Not Page.IsPostBack Then
            Dim intI As Integer
            'Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
            'Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
            Dim Conn As System.Data.SqlClient.SqlConnection = New System.Data.SqlClient.SqlConnection
            Conn.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
            Dim Cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand
            Cmd.Connection = Conn
            Try
                Cmd.CommandText = "SELECT codempre, sigempre " &
                                  "FROM empre " &
                                  "ORDER BY sigempre"
                Conn.Open()
                'Dim rsEmpresa As OnsClasses.OnsData.OnsDataReader = Cmd.ExecuteReader
                Dim rsEmpresa As System.Data.SqlClient.SqlDataReader = Cmd.ExecuteReader
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
                'Dim rsData As OnsClasses.OnsData.OnsDataReader = Cmd.ExecuteReader
                Dim rsData As System.Data.SqlClient.SqlDataReader = Cmd.ExecuteReader
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
                lblMsg.Visible = False

                'Cmd.Connection.Close()
                'Conn.Close()
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

    Private Sub ConsultaIntervalo(ByVal strcampo As String)
        'Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
        'Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
        Dim Conn As System.Data.SqlClient.SqlConnection = New System.Data.SqlClient.SqlConnection
        Conn.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
        Dim Cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand
        Cmd.Connection = Conn
        Dim objRow As TableRow
        Dim objCell As TableCell
        Dim arrIntervalo(48) As String
        Dim intI, intCol, intTotal, intHora As Integer


        Session("datEscolhida") = cboDataInicial.SelectedItem.Value
        Session("strCodEmpre") = cboEmpresa.SelectedItem.Value

        Try

            Cmd.Parameters.Clear() ' Limpa os parâmetros anteriores
            Cmd.Parameters.AddWithValue("@codempre", cboEmpresa.SelectedItem.Value)
            Cmd.Parameters.AddWithValue("@datpdp", cboDataInicial.SelectedItem.Value)

            If Trim(optDados.SelectedItem.Value) = "2" Then
                Cmd.CommandText = "SELECT intcarga, valcarga" & strcampo & " AS valor, " &
                      "(SELECT CASE WHEN codstatu = 99 THEN @msgValidado ELSE @msgNaoValidado END " &
                      "FROM pdp " &
                      "WHERE datpdp = carga.datpdp) AS msg " &
                      "FROM carga " &
                      "WHERE codempre = @codempre " &
                      "AND datpdp = @datpdp " &
                      "ORDER BY intcarga"
                Cmd.Parameters.AddWithValue("@msgValidado", strMsgValidado)
                Cmd.Parameters.AddWithValue("@msgNaoValidado", strMsgNaoValidado)
            Else
                Cmd.CommandText = "SELECT intcarga, valcarga" & strcampo & " AS valor " &
                      "FROM carga " &
                      "WHERE codempre = @codempre " &
                      "AND datpdp = @datpdp " &
                      "ORDER BY intcarga"
            End If


            Conn.Open()
            'Dim rsConsulta As OnsClasses.OnsData.OnsDataReader = Cmd.ExecuteReader
            Dim rsConsulta As System.Data.SqlClient.SqlDataReader = Cmd.ExecuteReader


            intHora = 0
            For intI = 1 To 48
                arrIntervalo(intI) = IntParaHora(intI)
                'If intI Mod 2 <> 0 Then
                ' arrIntervalo(intI) = Format(intHora - 1, "00") & ":00" & "-" & Format(intHora, "00") & ":30"
                '  intHora = intHora + 1
                '  Else
                '  arrIntervalo(intI) = Format(intHora - 1, "00") & ":30" & "-" & Format(intHora, "00") & ":00"
                '  End If
            Next
            intI = 0

            Dim objTamanho As System.Web.UI.WebControls.Unit
            objTamanho = New Unit

            'Colocar cor na tabela (RGB(233, 244, 207) = cor padrão ONS)
            Dim Color As System.Drawing.Color
            Color = New System.Drawing.Color
            Color = System.Drawing.ColorTranslator.FromWin32(RGB(233, 244, 207))

            tblConsulta.Width = objTamanho.Pixel(170)
            Do While rsConsulta.Read
                If intI = 0 Then
                    objRow = New TableRow
                    objRow.Width = objTamanho.Pixel(170)
                    objRow.BackColor = System.Drawing.Color.YellowGreen
                    For intCol = 1 To 2
                        'nova Celula
                        objCell = New TableCell
                        objCell.Font.Bold = True
                        Select Case intCol
                            Case Is = 1
                                objCell.Width = objTamanho.Pixel(100)
                                objCell.Text = "Intervalo"
                            Case Is = 2
                                objCell.Width = objTamanho.Pixel(70)
                                objCell.Text = cboEmpresa.SelectedItem.Text
                                If Trim(optDados.SelectedItem.Value) = "2" Then
                                    lblMensagem.Text = rsConsulta("msg")
                                    If Page.Request.QueryString("strAcesso") <> "PDOC" Then
                                        lblMensagem.Visible = True
                                    End If

                                End If
                        End Select
                        objRow.Controls.Add(objCell)
                    Next
                    tblConsulta.Rows.Add(objRow)
                    intI = intI + 1
                End If

                'Nova linha da tabela
                objRow = New TableRow
                If intI Mod 2 = 0 Then
                    'quando linha = par troca cor
                    objRow.BackColor = Color
                End If
                For intCol = 1 To 2
                    'nova Celula
                    objCell = New TableCell
                    objCell.Wrap = False
                    Select Case intCol
                        Case Is = 1
                            objCell.Text = arrIntervalo(rsConsulta("intcarga"))
                        Case Is = 2
                            objCell.Text = IIf(Not IsDBNull(rsConsulta("valor")), rsConsulta("valor"), 0)
                            intTotal = intTotal + objCell.Text
                    End Select
                    objRow.Controls.Add(objCell)
                Next

                'Adiciona a linha a tabela
                tblConsulta.Rows.Add(objRow)
                intI = intI + 1
            Loop

            If intI > 0 Then
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
                objCell.Text = (intTotal / 2) 'Conforme definição da Marta o total deve ser div. por 2
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
                objCell.Text = Int(IIf(intTotal = 0, intTotal, intTotal / (intI - 2)))
                objRow.Controls.Add(objCell)
                tblConsulta.Controls.Add(objRow)
            End If
            rsConsulta.Close()
            lblMsg.Visible = False
            'Conn.Close()
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

    Private Overloads Sub btnVisualizar_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnVisualizar.Click
        Dim strCampo As String
        lblMensagem.Text = ""
        If cboDataInicial.SelectedIndex <> 0 Then
            Session("datEscolhida") = CDate(cboDataInicial.SelectedItem.Text)
        End If
        If cboEmpresa.SelectedIndex <> 0 Then
            Session("strCodEmpre") = cboEmpresa.SelectedItem.Value
        End If

        If Trim(optDados.SelectedItem.Value) = "0" Then
            strCampo = "tran"
        ElseIf Trim(optDados.SelectedItem.Value) = "1" Then
            strCampo = "emp"
        Else
            strCampo = "pro"
        End If
        'intervalo
        ConsultaIntervalo(strCampo)

        'Chama uma nova janela para exibir o gráfico e passa os parâmetros por query string
        imgPlanilha.Attributes.Add("onclick", _
                                   "window.open('frmPlanilha.aspx?" & _
                                   "strDataPDP=" & cboDataInicial.SelectedValue & "&" & _
                                   "strCampo=" & strCampo & "&" & _
                                   "strEmpresa=" & cboEmpresa.SelectedValue.Trim & "|" & cboEmpresa.SelectedItem.Text.Trim & "&" & _
                                   "strTabela=carga&" & _
                                   "strBase=" & strBase & "&" & _
                                   "strAcesso=" & Page.Request.QueryString("strAcesso") & "'" & _
                                   ",'Planilha','height = 600, width = 850, toolbar=yes,location=no,status=no,menubar=yes,scrollbars=yes,scrolling=yes,resizebled=yes');")
        imgPlanilha.Visible = True
    End Sub
End Class
