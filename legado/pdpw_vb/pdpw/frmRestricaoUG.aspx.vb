Imports System.Data.OleDb

Partial Class frmRestricaoUG
    Inherits System.Web.UI.Page
    Protected WithEvents Imagebutton1 As System.Web.UI.WebControls.ImageButton
    Protected WithEvents Imagebutton3 As System.Web.UI.WebControls.ImageButton

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
        'Put user code to initialize the page here
        objMenu = CType(Session("onsmenu"), OnsWebControls.OnsMenu)
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
                PreencheComboEmpresa(Session("usuarID"), cboEmpresa, Session("strCodEmpre"))
                cboEmpresa_SelectedIndexChanged(sender, e)
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
        PreencheTable()
    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        If SessaoAtiva(Page.Session) Then
            MyBase.Render(writer)
            objMenu.RenderControl(writer)
        End If
    End Sub

    Private Sub cboEmpresa_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboEmpresa.SelectedIndexChanged
        Dim blnUsina As Boolean
        Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
        Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
        Cmd.Connection = Conn
        If cboEmpresa.SelectedIndex > 0 Then
            Session("strCodEmpre") = cboEmpresa.SelectedItem.Value
        End If
        Try
            Cmd.CommandText = "Select codusina, nomusina " & _
                            "From usina " & _
                            "Where codempre = '" & Trim(Session("strCodEmpre")) & "' " & _
                            "Order By nomusina"
            Conn.Open("pdp")
            Dim rsUsina As OnsClasses.OnsData.OnsDataReader = Cmd.ExecuteReader
            Dim objItem As System.Web.UI.WebControls.ListItem
            cboUsina.Items.Clear()
            blnUsina = True
            Dim intI As Integer = 0
            Do While rsUsina.Read
                If blnUsina = True Then
                    objItem = New System.Web.UI.WebControls.ListItem
                    objItem.Value = "0"
                    objItem.Text = ""
                    cboUsina.Items.Add(objItem)
                    blnUsina = False
                End If
                objItem = New System.Web.UI.WebControls.ListItem
                objItem.Value = rsUsina.Item("codusina")
                objItem.Text = rsUsina.Item("nomusina")
                cboUsina.Items.Add(objItem)
                If Session("strCodUsina") = rsUsina.Item("codusina") Then
                    cboUsina.SelectedIndex = intI + 1
                End If
                intI = intI + 1
            Loop
            rsUsina.Close()
            rsUsina = Nothing
            'Cmd.Connection.Close()
            'Conn.Close()

            'Valida Limite de Envio
            Dim lRetorno As Integer = 0
            If Not ValidaLimiteEntradaDados(cboEmpresa.SelectedItem.Value, cboData.SelectedItem.Value, lRetorno) Then
                btnIncluir.Visible = False
                btnExcluir.Visible = False
                btnAlterar.Visible = False
                cboUsina.Enabled = False
                If lRetorno = 1 Then
                    Response.Write("<SCRIPT>alert('" + strMsgInicioLimiteEnvioDados + "')</SCRIPT>")
                Else
                    Response.Write("<SCRIPT>alert('" + strMsgLimiteEnvioDados + "')</SCRIPT>")
                End If
                Exit Sub
            Else
                btnIncluir.Visible = True
                btnExcluir.Visible = True
                btnAlterar.Visible = True
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

    Private Sub btnExcluir_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnExcluir.Click
        Dim objPermissao As OnsClasses.OnsInterfaceUnica.OnsPermissao
        objPermissao = New OnsClasses.OnsInterfaceUnica.OnsPermissao("DEL", "PDPColReUG", Session("usuarID"))
        'Verifica se o usuário tem permissão para salvar os registros
        If objPermissao.EstaAutorizado Then
            Dim intI As Integer
            Dim a As String
            Dim objCheckBox As System.Web.UI.WebControls.CheckBox
            Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
            Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
            Cmd.Connection = Conn
            Try
                Conn.Open("pdp")
                Conn.Servico = "PDPColReUG"
                Conn.Usuario = Session("usuarID")
                objCheckBox = New System.Web.UI.WebControls.CheckBox
                For intI = 1 To tblGerador.Rows.Count - 1
                    objCheckBox = tblGerador.Rows(intI).Cells(0).Controls(0)
                    If objCheckBox.Checked = True Then
                        Cmd.CommandText = "Delete From restrgerademp " & _
                                        "Where codrestr = '" & objCheckBox.ID & "'"
                        Cmd.ExecuteNonQuery()
                    End If
                Next
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
        Else
            Session("strMensagem") = "Usuário não tem permissão para Excluir os valores."
            Response.Redirect("frmMensagem.aspx")
        End If
    End Sub

    Private Sub PreencheTable()
        If cboEmpresa.SelectedItem.Value <> "0" And cboUsina.Items.Count <> "0" Then
            If cboUsina.SelectedItem.Value <> "0" Then
                Dim intRow, intCol As Integer
                Dim objCheck As CheckBox
                Dim objRow As TableRow
                Dim objCell As TableCell
                Dim arrHorario(48) As String
                Dim intI As Integer
                Dim intHora As Integer
                intHora = 0

                'limpa a tabela
                tblGerador.Rows.Clear()

                objRow = New TableRow
                objRow.BackColor = System.Drawing.Color.YellowGreen
                For intCol = 1 To 6
                    'nova Celula
                    objCell = New TableCell
                    Select Case intCol
                        Case Is = 1
                            objCell.Controls.Add(New LiteralControl("Código"))
                        Case Is = 2
                            objCell.Controls.Add(New LiteralControl("Gerador"))
                        Case Is = 3
                            objCell.Controls.Add(New LiteralControl("Sigla"))
                        Case Is = 4
                            objCell.Controls.Add(New LiteralControl("Data Início"))
                        Case Is = 5
                            objCell.Controls.Add(New LiteralControl("Hora Inicio"))
                        Case Is = 6
                            objCell.Controls.Add(New LiteralControl("Motivo Restrição"))
                            'Case Is = 7
                            '    objCell.Controls.Add(New LiteralControl("Observação"))
                    End Select
                    objRow.Controls.Add(objCell)
                Next
                tblGerador.Rows.Add(objRow)

                'inicializando vetor com intervalos de hora
                For intI = 1 To 48
                    If intI Mod 2 = 0 Then
                        arrHorario(intI) = Right("00" & Trim(Str(intHora)), 2) & ":00"
                    Else
                        arrHorario(intI) = Right("00" & Trim(Str(intHora)), 2) & ":30"
                        intHora = intHora + 1
                    End If
                Next
                Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
                Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
                Cmd.Connection = Conn
                Try
                    Cmd.CommandText = "Select p.codrestr, " & _
                                    "       g.codgerad, " & _
                                    "       g.siggerad, " & _
                                    "       p.datinirestr, " & _
                                    "       p.intinirestr, " & _
                                    "       nvl(m.dsc_motivorestricao,'') as dsc_motivorestricao " & _
                                    "From gerad As g, " & _
                                    "     restrgerademp As p, " & _
                                    "     outer tb_motivorestricao As m " & _
                                    "Where g.codusina = '" & Trim(cboUsina.SelectedItem.Value) & "' And " & _
                                    "      g.codgerad = p.codgerad And " & _
                                    "      p.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' And " & _
                                    "      m.id_motivorestricao = p.id_motivorestricao " & _
                                    "Order By p.datinirestr"
                    Conn.Open("pdp")
                    Dim rsTempRestr As OnsClasses.OnsData.OnsDataReader = Cmd.ExecuteReader

                    'Colocar cor na tabela (RGB(233, 244, 207) = cor padrão ONS)
                    Dim Color As System.Drawing.Color
                    Color = New System.Drawing.Color
                    Color = System.Drawing.ColorTranslator.FromWin32(RGB(233, 244, 207))
                    intRow = 1
                    Do While rsTempRestr.Read
                        'Cria um novo check box
                        objCheck = New CheckBox
                        objCheck.ID = rsTempRestr("codrestr")

                        'Nova linha da tabela
                        objRow = New TableRow
                        If intRow Mod 2 = 0 Then
                            'quanto linha = par troca cor
                            objRow.BackColor = Color
                        End If

                        Dim objTamanho As System.Web.UI.WebControls.Unit
                        objTamanho = New Unit

                        For intCol = 1 To 6
                            'nova Celula
                            objCell = New TableCell

                            Select Case intCol
                                Case Is = 1
                                    objCell.Width = objTamanho.Pixel(70)
                                    objCell.Controls.Add(objCheck)
                                    objCell.Controls.Add(New LiteralControl(rsTempRestr("codrestr")))
                                Case Is = 2
                                    objCell.Width = objTamanho.Pixel(80)
                                    objCell.Controls.Add(New LiteralControl(rsTempRestr("codgerad")))
                                Case Is = 3
                                    objCell.Width = objTamanho.Pixel(60)
                                    objCell.Controls.Add(New LiteralControl(rsTempRestr("siggerad")))
                                Case Is = 4
                                    objCell.Width = objTamanho.Pixel(80)
                                    objCell.Controls.Add(New LiteralControl(Right(rsTempRestr("datinirestr"), 2) & "/" & _
                                                                            Mid(rsTempRestr("datinirestr"), 5, 2) & "/" & _
                                                                            Left(rsTempRestr("datinirestr"), 4)))
                                Case Is = 5
                                    objCell.Width = objTamanho.Pixel(70)
                                    objCell.Controls.Add(New LiteralControl(arrHorario(Val(rsTempRestr("intinirestr")))))
                                Case Is = 6
                                    objCell.Width = objTamanho.Pixel(200)
                                    objCell.Controls.Add(New LiteralControl(rsTempRestr("dsc_motivorestricao")))
                                    'Case Is = 7
                                    '    objCell.Width = objTamanho.Pixel(170)
                                    '    objCell.Controls.Add(New LiteralControl(rsTempRestr("obsrestr")))
                            End Select
                            objRow.Controls.Add(objCell)
                        Next
                        'Adiciona a linha a tabela
                        tblGerador.Rows.Add(objRow)
                        intRow = intRow + 1
                    Loop
                    rsTempRestr.Close()
                    rsTempRestr = Nothing
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
        End If
    End Sub

    Private Sub cboUsina_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboUsina.SelectedIndexChanged
        If cboUsina.SelectedIndex > 0 Then
            Session("strCodUsina") = cboUsina.SelectedItem.Value
        End If
        PreencheTable()
    End Sub

    Private Sub btnAlterar_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnAlterar.Click
        Dim objPermissao As OnsClasses.OnsInterfaceUnica.OnsPermissao
        objPermissao = New OnsClasses.OnsInterfaceUnica.OnsPermissao("UPD", "PDPColReUG", Session("usuarID"))
        'Verifica se o usuário tem permissão para salvar os registros
        If objPermissao.EstaAutorizado Then
            Dim objCheckBox As System.Web.UI.WebControls.CheckBox
            Dim intI As Integer
            Session("strSigEmpre") = cboEmpresa.SelectedItem.Text
            Session("strCodRestr") = ""
            objCheckBox = New System.Web.UI.WebControls.CheckBox
            For intI = 1 To tblGerador.Rows.Count - 1
                objCheckBox = tblGerador.Rows(intI).Cells(0).Controls(0)
                If objCheckBox.Checked = True Then
                    Session("strCodRestr") = objCheckBox.ID
                    Session("strAcao") = "Alterar"
                    Server.Transfer("frmColRestricaoUG.aspx")
                End If
            Next
        Else
            Session("strMensagem") = "Usuário não tem permissão para Alterar os valores."
            Response.Redirect("frmMensagem.aspx")
        End If
    End Sub

    Private Sub btnIncluir_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnIncluir.Click
        Dim objPermissao As OnsClasses.OnsInterfaceUnica.OnsPermissao
        objPermissao = New OnsClasses.OnsInterfaceUnica.OnsPermissao("INS", "PDPColReUG", Session("usuarID"))
        'Verifica se o usuário tem permissão para salvar os registros
        If objPermissao.EstaAutorizado Then
            Session("strAcao") = "Incluir"
            Session("strSigEmpre") = cboEmpresa.SelectedItem.Text
            Server.Transfer("frmColRestricaoUG.aspx")
        Else
            Session("strMensagem") = "Usuário não tem permissão para Incluir os valores."
            Response.Redirect("frmMensagem.aspx")
        End If
    End Sub

    Private Sub cboData_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboData.SelectedIndexChanged
        If cboData.SelectedIndex > 0 Then
            Session("datEscolhida") = CDate(cboData.SelectedItem.Value)
        End If
        cboEmpresa_SelectedIndexChanged(sender, e)
    End Sub
End Class
