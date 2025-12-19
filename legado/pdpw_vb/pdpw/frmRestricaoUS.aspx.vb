Imports System.Data.OleDb

Partial Class frmRestricaoUS
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
        If cboEmpresa.SelectedIndex > 0 Then
            Session("strCodEmpre") = cboEmpresa.SelectedItem.Value
        End If
        PreencheTable()

        'Valida Limite de Envio
        Dim lRetorno As Integer = 0
        If Not ValidaLimiteEntradaDados(cboEmpresa.SelectedItem.Value, cboData.SelectedItem.Value, lRetorno) Then
            btnIncluir.Visible = False
            btnExcluir.Visible = False
            btnAlterar.Visible = False
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
        End If
    End Sub

    Private Sub btnIncluir_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnIncluir.Click
        Dim objPermissao As OnsClasses.OnsInterfaceUnica.OnsPermissao
        objPermissao = New OnsClasses.OnsInterfaceUnica.OnsPermissao("INS", "PDPColReUS", Session("usuarID"))
        'Verifica se o usuário tem permissão para salvar os registros
        If objPermissao.EstaAutorizado Then
            Session("strAcao") = "Incluir"
            Session("strSigEmpre") = cboEmpresa.SelectedItem.Text
            Server.Transfer("frmColRestricaoUS.aspx")
        Else
            Session("strMensagem") = "Usuário não tem permissão para Incluir os valores."
            Response.Redirect("frmMensagem.aspx")
        End If
    End Sub

    Private Sub btnExcluir_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnExcluir.Click
        Dim objPermissao As OnsClasses.OnsInterfaceUnica.OnsPermissao
        objPermissao = New OnsClasses.OnsInterfaceUnica.OnsPermissao("DEL", "PDPColReUS", Session("usuarID"))
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
                Conn.Servico = "PDPColReUS"
                Conn.Usuario = Session("usuarID")
                objCheckBox = New System.Web.UI.WebControls.CheckBox
                For intI = 1 To tblUsina.Rows.Count - 1
                    objCheckBox = tblUsina.Rows(intI).Cells(0).Controls(0)
                    If objCheckBox.Checked = True Then
                        Cmd.CommandText = "Delete From restrusinaemp " & _
                                        "Where codrestr = '" & objCheckBox.ID & "'"
                        Cmd.ExecuteNonQuery()
                        Session("blnTranfResUS") = True
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
        If cboEmpresa.SelectedItem.Value <> "0" Then
            Dim intRow, intCol As Integer
            Dim objCheck As CheckBox
            Dim objRow As TableRow
            Dim objCell As TableCell
            Dim arrHorario(48) As String
            Dim intI As Integer
            Dim intHora As Integer
            intHora = 0

            'limpa a tabela
            tblUsina.Rows.Clear()

            objRow = New TableRow
            objRow.BackColor = System.Drawing.Color.YellowGreen
            For intCol = 1 To 5
                'nova Celula
                objCell = New TableCell
                Select Case intCol
                    Case Is = 1
                        objCell.Controls.Add(New LiteralControl("Código"))
                    Case Is = 2
                        objCell.Controls.Add(New LiteralControl("Usina"))
                    Case Is = 3
                        objCell.Controls.Add(New LiteralControl("Sigla"))
                    Case Is = 4
                        objCell.Controls.Add(New LiteralControl("Data Início"))
                    Case Is = 5
                        objCell.Controls.Add(New LiteralControl("Hora Inicio"))
                End Select
                objRow.Controls.Add(objCell)
            Next
            tblUsina.Rows.Add(objRow)

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
                                    "       u.codusina, " & _
                                    "       u.sigusina, " & _
                                    "       p.datinirestr, " & _
                                    "       p.intinirestr " & _
                                    "From usina As u, " & _
                                    "     restrusinaemp As p " & _
                                    "Where u.codempre = '" & Trim(cboEmpresa.SelectedItem.Value) & "' And " & _
                                    "      u.codusina = p.codusina And " & _
                                    "      p.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' " & _
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

                    For intCol = 1 To 5
                        'nova Celula
                        objCell = New TableCell

                        Select Case intCol
                            Case Is = 1
                                objCell.Controls.Add(objCheck)
                                objCell.Controls.Add(New LiteralControl(rsTempRestr("codrestr")))
                            Case Is = 2
                                objCell.Controls.Add(New LiteralControl(rsTempRestr("codusina")))
                            Case Is = 3
                                objCell.Controls.Add(New LiteralControl(rsTempRestr("sigusina")))
                            Case Is = 4
                                objCell.Controls.Add(New LiteralControl(Right(rsTempRestr("datinirestr"), 2) & "/" & _
                                                                            Mid(rsTempRestr("datinirestr"), 5, 2) & "/" & _
                                                                            Left(rsTempRestr("datinirestr"), 4)))
                            Case Is = 5
                                objCell.Controls.Add(New LiteralControl(arrHorario(Val(rsTempRestr("intinirestr")))))
                        End Select
                        objRow.Controls.Add(objCell)
                    Next
                    'Adiciona a linha a tabela
                    tblUsina.Rows.Add(objRow)
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
    End Sub

    Private Sub btnAlterar_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnAlterar.Click
        Dim objPermissao As OnsClasses.OnsInterfaceUnica.OnsPermissao
        objPermissao = New OnsClasses.OnsInterfaceUnica.OnsPermissao("UPD", "PDPColReUS", Session("usuarID"))
        'Verifica se o usuário tem permissão para salvar os registros
        If objPermissao.EstaAutorizado Then
            Dim objCheckBox As System.Web.UI.WebControls.CheckBox
            Dim intI As Integer
            Session("strCodRestr") = ""
            Session("strSigEmpre") = cboEmpresa.SelectedItem.Text
            objCheckBox = New System.Web.UI.WebControls.CheckBox

            For intI = 1 To tblUsina.Rows.Count - 1
                objCheckBox = tblUsina.Rows(intI).Cells(0).Controls(0)
                If objCheckBox.Checked = True Then
                    Session("strCodRestr") = objCheckBox.ID
                    Session("strAcao") = "Alterar"
                    Server.Transfer("frmColRestricaoUS.aspx")
                End If
            Next
        Else
            Session("strMensagem") = "Usuário não tem permissão para Alterar os valores."
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
