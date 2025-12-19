Imports System.Data.OleDb

Partial Class frmManutencaoUG
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.ID = "frmManutencaoUG"

    End Sub

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
                PreencheComboEmpresaPOP(AgentesRepresentados, cboEmpresa, Session("strCodEmpre"))
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
            'objMenu.RenderControl(writer)
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
                            "Where codempre = '" & Session("strCodEmpre") & "' " & _
                            "Order By nomusina"
            Conn.Open("pdp")
            Dim rsUsina As OnsClasses.OnsData.OnsDataReader = Cmd.ExecuteReader
            Dim objItem As System.Web.UI.WebControls.ListItem
            cboUsina.Items.Clear()
            blnUsina = True
            Dim intI As Integer = 1
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
                    cboUsina.SelectedIndex = intI
                End If
                intI = intI + 1
            Loop
            rsUsina.Close()
            rsUsina = Nothing
            'Cmd.Connection.Close()
            'Conn.Close()
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

    Private Sub btnIncluir_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnIncluir.Click
        'Dim objPermissao As OnsClasses.OnsInterfaceUnica.OnsPermissao
        'objPermissao = New OnsClasses.OnsInterfaceUnica.OnsPermissao("INS", "PDPColMaUG", UsuarID)
        'Verifica se o usuário tem permissão para salvar os registros
        If EstaAutorizado Then
            Session("strAcao") = "Incluir"
            Session("strCodEmpre") = cboEmpresa.SelectedItem.Value
            Session("strSigEmpre") = cboEmpresa.SelectedItem.Text
            Session("strCodUsina") = cboUsina.SelectedItem.Value
            Server.Transfer("frmColManutencaoUG.aspx")
        Else
            Session("strMensagem") = "Usuário não tem permissão para Incluir os valores."
            Response.Redirect("frmMensagem.aspx")
        End If
    End Sub

    Private Sub btnExcluir_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnExcluir.Click
        'Dim objPermissao As OnsClasses.OnsInterfaceUnica.OnsPermissao
        'objPermissao = New OnsClasses.OnsInterfaceUnica.OnsPermissao("DEL", "PDPColMaUG", UsuarID)
        'Verifica se o usuário tem permissão para salvar os registros
        If EstaAutorizado Then
            Dim intI As Integer
            Dim a As String
            Dim objCheckBox As System.Web.UI.WebControls.CheckBox
            Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
            Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
            Cmd.Connection = Conn
            Try
                Conn.Open("pdp")
                Conn.Servico = "PDPColMaUG"
                Conn.Usuario = UsuarID
                objCheckBox = New System.Web.UI.WebControls.CheckBox
                For intI = 1 To tblGerador.Rows.Count - 1
                    objCheckBox = tblGerador.Rows(intI).Cells(0).Controls(0)
                    If objCheckBox.Checked = True Then
                        Cmd.CommandText = "Delete From paralemp " & _
                                        "Where codparal = '" & objCheckBox.ID & "'"
                        Cmd.ExecuteNonQuery()
                    End If
                Next
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
                For intCol = 1 To 5
                    'nova Celula
                    objCell = New TableCell
                    Select Case intCol
                        Case Is = 1
                            objCell.Controls.Add(New LiteralControl("Código"))
                        Case Is = 2
                            objCell.Controls.Add(New LiteralControl("Equipamento"))
                        Case Is = 3
                            objCell.Controls.Add(New LiteralControl("Sigla"))
                        Case Is = 4
                            objCell.Controls.Add(New LiteralControl("Data Início"))
                        Case Is = 5
                            objCell.Controls.Add(New LiteralControl("Hora Inicio"))
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
                    Cmd.CommandText = "Select p.codparal, " & _
                                    "       g.codgerad, " & _
                                    "       g.siggerad, " & _
                                    "       p.datiniparal, " & _
                                    "       p.intiniparal " & _
                                    "From gerad As g, " & _
                                    "     paralemp As p " & _
                                    "Where g.codusina = '" & Trim(cboUsina.SelectedItem.Value) & "' And " & _
                                    "      g.codgerad = p.codequip And " & _
                                    "      p.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' " & _
                                    "Order By p.datiniparal"
                    Conn.Open("pdp")
                    Dim rsParalEmp As OnsClasses.OnsData.OnsDataReader = Cmd.ExecuteReader

                    'Colocar cor na tabela (RGB(233, 244, 207) = cor padrão ONS)
                    Dim Color As System.Drawing.Color
                    Color = New System.Drawing.Color
                    Color = System.Drawing.ColorTranslator.FromWin32(RGB(233, 244, 207))
                    intRow = 1
                    Do While rsParalEmp.Read
                        'Cria um novo check box
                        objCheck = New CheckBox
                        objCheck.ID = rsParalEmp("codparal")

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
                                    objCell.Controls.Add(New LiteralControl(rsParalEmp("codparal")))
                                Case Is = 2
                                    objCell.Controls.Add(New LiteralControl(rsParalEmp("codgerad")))
                                Case Is = 3
                                    objCell.Controls.Add(New LiteralControl(rsParalEmp("siggerad")))
                                Case Is = 4
                                    objCell.Controls.Add(New LiteralControl(Right(rsParalEmp("datiniparal"), 2) & "/" & _
                                                                            Mid(rsParalEmp("datiniparal"), 5, 2) & "/" & _
                                                                            Left(rsParalEmp("datiniparal"), 4)))
                                Case Is = 5
                                    objCell.Controls.Add(New LiteralControl(arrHorario(Val(rsParalEmp("intiniparal")))))
                            End Select
                            objRow.Controls.Add(objCell)
                        Next

                        'Adiciona a linha a tabela
                        tblGerador.Rows.Add(objRow)
                        intRow = intRow + 1
                    Loop
                    rsParalEmp.Close()
                    rsParalEmp = Nothing
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
        PreencheTable()
    End Sub

    Private Sub btnAlterar_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnAlterar.Click
        'Dim objPermissao As OnsClasses.OnsInterfaceUnica.OnsPermissao
        'objPermissao = New OnsClasses.OnsInterfaceUnica.OnsPermissao("UPD", "PDPColMaUG", UsuarID)
        'Verifica se o usuário tem permissão para salvar os registros
        If EstaAutorizado Then
            Dim objCheckBox As System.Web.UI.WebControls.CheckBox
            Dim intI As Integer
            Session("strSigEmpre") = cboEmpresa.SelectedItem.Text
            Session("strCodParal") = ""
            objCheckBox = New System.Web.UI.WebControls.CheckBox
            For intI = 1 To tblGerador.Rows.Count - 1
                objCheckBox = tblGerador.Rows(intI).Cells(0).Controls(0)
                If objCheckBox.Checked = True Then
                    Session("strCodEmpre") = cboEmpresa.SelectedItem.Value
                    Session("strCodUsina") = cboUsina.SelectedItem.Value
                    Session("strCodParal") = objCheckBox.ID
                    Session("strAcao") = "Alterar"
                    Server.Transfer("frmColManutencaoUG.aspx")
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
