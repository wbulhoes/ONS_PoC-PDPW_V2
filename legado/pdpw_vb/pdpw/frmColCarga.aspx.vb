Partial Class frmColCarga

    Inherits System.Web.UI.Page
    Protected WithEvents txtValor As System.Web.UI.HtmlControls.HtmlTextArea
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
                Cmd.CommandText = "Select datpdp " &
                                  "From pdp " &
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
                'preenche a combo de empresas de acordo com o usuário logado
                PreencheComboEmpresaPOP(AgentesRepresentados, cboEmpresa, Session("strCodEmpre"))
                If cboData.SelectedIndex > 0 Then
                    cboData_SelectedIndexChanged(sender, e)
                End If
                lblMsg.Visible = False
            Catch
                lblMsg.Visible = True
                'Session("strMensagem") = "Não foi possível acessar a Base de Dados. Ocorreu o seguinte erro: " & Chr(10) & Err.Description
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
        'CARGA - EMPRESA
        Dim intI As Integer
        divValor.Visible = False
        btnSalvar.Visible = False
        btnAlterar.Visible = True
        'btnSalvar.Text = "Alterar"
        If cboEmpresa.SelectedIndex = 0 Then
            btnSalvar.Visible = False
            btnAlterar.Visible = False
            For intI = 1 To 50
                tblCarga.Rows(intI).Cells(1).Text = ""
            Next
        ElseIf cboEmpresa.SelectedIndex > 0 Then
            Session("strCodEmpre") = cboEmpresa.SelectedItem.Value
            Dim intMedia As Double, intQtdReg As Integer
            Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
            Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
            Cmd.Connection = Conn
            Try
                Cmd.CommandText = "Select intcarga, " &
                                "       valcargatran " &
                                "From carga " &
                                "Where codempre = '" & Session("strCodEmpre") & "' And " &
                                "      datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' " &
                                "Order By intcarga"
                Conn.Open("pdp")
                Dim rsCarga As OnsClasses.OnsData.OnsDataReader = Cmd.ExecuteReader
                intMedia = 0
                intQtdReg = 0
                Do While rsCarga.Read
                    If Not IsDBNull(rsCarga.Item("valcargatran")) Then
                        tblCarga.Rows(rsCarga.Item("intcarga")).Cells(1).Text = rsCarga.Item("valcargatran")
                        intMedia = intMedia + rsCarga.Item("valcargatran")
                    Else
                        tblCarga.Rows(rsCarga.Item("intcarga")).Cells(1).Text = 0
                    End If
                    intQtdReg = intQtdReg + 1
                Loop
                If intMedia <> 0 Then
                    tblCarga.Rows(49).Cells(1).Text = intMedia / 2
                    tblCarga.Rows(50).Cells(1).Text = Int(intMedia / 48)
                Else
                    tblCarga.Rows(49).Cells(1).Text = 0
                    tblCarga.Rows(50).Cells(1).Text = 0
                End If
                'If intQtdReg = 0 Then
                '    btnSalvar.Visible = False
                '    btnAlterar.Visible = True
                'End If
                btnSalvar.Visible = False
                btnAlterar.Visible = True
                rsCarga.Close()
                rsCarga = Nothing
                'Conn.Close()

                'Valida Limite de Envio
                Dim lRetorno As Integer = 0
                If Not ValidaLimiteEntradaDados(cboEmpresa.SelectedItem.Value, cboData.SelectedItem.Value, lRetorno, "CAR") Then
                    btnSalvar.Visible = False
                    btnAlterar.Visible = False
                    If lRetorno = 1 Then
                        Response.Write("<SCRIPT>alert('" + strMsgInicioLimiteEnvioDados + "')</SCRIPT>")
                    Else
                        Response.Write("<SCRIPT>alert('" + strMsgLimiteEnvioDados + "')</SCRIPT>")
                    End If
                    Exit Sub
                Else
                    btnSalvar.Visible = True
                    btnAlterar.Visible = True
                End If
                lblMsg.Visible = False
            Catch
                lblMsg.Visible = True
                'Session("strMensagem") = "Não foi possível acessar a Base de Dados. Ocorreu o seguinte erro: " & Chr(10) & Err.Description
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
            'Session("strMensagem") = "Não foi possível acessar a Base de Dados. Ocorreu o seguinte erro: " & Chr(10) & Err.Description
            'Response.Redirect("frmMensagem.aspx")
        End Try
    End Sub

    Private Overloads Sub btnSalvar_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSalvar.Click
        SalvaRegistro()
    End Sub

    Private Sub SalvaRegistro()
        Dim intI As Integer = 1
        Dim intColAtual As Integer
        Dim intColAnterior As Integer
        Dim dblMedia As Double
        Dim intQtdReg As Integer
        Dim intCol As Integer
        Dim strValor As String
        'Dim objPermissao As OnsClasses.OnsInterfaceUnica.OnsPermissao
        'objPermissao = New OnsClasses.OnsInterfaceUnica.OnsPermissao("UPD", "PDPColCarg", UsuarID)
        'Verifica se o usuário tem permissão para salvar os registros
        If EstaAutorizado Then
            Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
            Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
            Cmd.Connection = Conn
            Dim objTrans As OnsClasses.OnsData.OnsTransaction
            Conn.Servico = "PDPColCarg"
            Conn.Usuario = UsuarID
            Conn.Open("pdp")
            Dim datAtual As Date = Now
            strValor = Page.Request.Form.Item("txtValor")
            'If btnSalvar.Text = "Salvar" Then
            If btnSalvar.Visible = True Then
                'Atualiza a grid
                If strValor = "" Then
                    'Quando o strValor estiver em branco, branqueia tabela e a grid
                    For intI = 1 To 50
                        tblCarga.Rows(intI).Cells(1).Text = 0
                    Next
                Else
                    intI = 1
                    intColAnterior = 1
                    intColAtual = InStr(intColAnterior, strValor, Chr(10), CompareMethod.Binary)
                    For intI = 1 To 48
                        If intColAtual <> 0 Then
                            If Mid(strValor, intColAnterior, (intColAtual - intColAnterior) + 1) <> "" Then
                                tblCarga.Rows(intI).Cells(1).Text = Mid(strValor, intColAnterior, (intColAtual - intColAnterior) + 1)
                                dblMedia = dblMedia + Val(tblCarga.Rows(intI).Cells(1).Text)
                            End If
                            intColAnterior = intColAtual
                        ElseIf intColAtual = 0 And Mid(strValor, intColAnterior) <> "" Then
                            'Não tem ENTER (chr(13)) no final do texto
                            tblCarga.Rows(intI).Cells(1).Text = Mid(strValor, intColAnterior)
                            dblMedia = dblMedia + Val(tblCarga.Rows(intI).Cells(1).Text)
                            intColAnterior = intColAnterior + Trim(Mid(strValor, intColAnterior)).Length
                        Else
                            tblCarga.Rows(intI).Cells(1).Text = 0
                        End If
                        intColAtual = InStr(intColAnterior + 1, strValor, Chr(10), CompareMethod.Binary)
                    Next
                    tblCarga.Rows(49).Cells(1).Text = dblMedia / 2
                    tblCarga.Rows(50).Cells(1).Text = Int(dblMedia / 48)
                    strValor = ""
                End If
            End If
            'Select Case btnSalvar.Text
            'Case Is = "Alterar"
            If btnAlterar.Visible = True Then
                'Colocando os valores de carga no text para alteração
                Dim objTextArea As HtmlControls.HtmlTextArea
                objTextArea = New HtmlTextArea
                objTextArea.Rows = 48
                objTextArea.ID = "txtValor"

                objTextArea.Attributes.Item("onkeyup") = "RetiraEnter(event)"
                objTextArea.Attributes.Item("runat") = "server"
                objTextArea.Attributes.Item("style") = "font-size:XSmall;height:1001px;width:88px;line-height:20px"
                For intI = 1 To 48
                    objTextArea.Value = objTextArea.Value & Val(tblCarga.Rows(intI).Cells(1).Text()) & Chr(10)
                Next
                'Posiciona a DIV em cima da GRID para que os valores sejam alinhados
                divValor.Controls.Add(objTextArea)
                divValor.Style.Item("TOP") = "208px"
                divValor.Style.Item("LEFT") = "243px"
                divValor.Visible = True
                btnAlterar.Visible = False
                btnSalvar.Visible = True
                'btnSalvar.Text = "Salvar"
            Else
                'Case Is = "Salvar"
                'Alterando os valores de carga na BDT
                objTrans = Conn.BeginTransaction()
                Cmd.Transaction = objTrans
                Try
                    For intI = 1 To 48
                        Cmd.CommandText = "Update carga " &
                                            "Set valcargatran = " & Val(tblCarga.Rows(intI).Cells(1).Text) & " " &
                                            "Where datPdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' And " &
                                            "      codempre = '" & cboEmpresa.SelectedItem.Value & "' And " &
                                            "      intCarga = " & intI
                        Cmd.ExecuteNonQuery()
                    Next

                    'Grava evento registrando o recebimento da carga
                    GravaEventoPDP("8", Format(Session("datEscolhida"), "yyyyMMdd"), cboEmpresa.SelectedItem.Text, datAtual, "PDPColCarg", UsuarID)

                    'Grava toda a transação
                    objTrans.Commit()
                    btnSalvar.Visible = False
                    btnAlterar.Visible = True
                    'btnSalvar.Text = "Alterar"
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
                    ' Response.Redirect("frmMensagem.aspx")
                Finally
                    If Conn.State = ConnectionState.Open Then
                        Conn.Close()
                    End If
                End Try

                'fecha a conexão com o banco
                'Cmd.Connection.Close()
                'Conn.Close()
            End If
            'End Select
        Else
            Session("strMensagem") = "Usuário não tem permissão para alterar os valores."
            Response.Redirect("frmMensagem.aspx")
        End If
    End Sub

    Private Sub btnAlterar_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnAlterar.Click
        SalvaRegistro()
    End Sub
End Class

