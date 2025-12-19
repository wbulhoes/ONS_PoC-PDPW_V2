
Partial Class frmColCargaEst
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load, Me.Load
        'Put user code to initialize the page here
        'objMenu = CType(Session("onsmenu"), OnsWebControls.OnsMenu)
        lblMsg.Visible = False
        If Not Page.IsPostBack Then
            Dim intI As Integer
            Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
            Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
            Cmd.Connection = Conn

            Try
                'No início coloca o calendário invisivel
                btnSalvar.Visible = False
                btnAlterar.Visible = False
                divCal.Visible = False
                'preenche a combo de empresas de acordo com o usuário logado
                PreencheComboEmpresaPOP(AgentesRepresentados, cboEmpresa, Session("strCodEmpre"), "S")
            Catch
                Session("strMensagem") = "Não foi possível acessar a Base de Dados. Ocorreu o seguinte erro: " & Chr(10) & Err.Description
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
        'CARGA - EMPRESA
        divCal.Visible = False
        AtualizaGrid()
    End Sub

    Private Overloads Sub btnSalvar_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSalvar.Click
        divCal.Visible = False
        SalvaRegistro()
    End Sub

    Private Sub SalvaRegistro()
        Dim intI As Integer = 1
        Dim intColAtual As Integer
        Dim intColAnterior As Integer
        Dim intMedia As Integer
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
                                intMedia += Val(tblCarga.Rows(intI).Cells(1).Text)
                            End If
                            intColAnterior = intColAtual
                        ElseIf intColAtual = 0 And Mid(strValor, intColAnterior) <> "" Then
                            'Não tem ENTER (chr(13)) no final do texto
                            tblCarga.Rows(intI).Cells(1).Text = Mid(strValor, intColAnterior)
                            intMedia += Val(tblCarga.Rows(intI).Cells(1).Text)
                            intColAnterior = intColAnterior + Trim(Mid(strValor, intColAnterior)).Length
                        Else
                            tblCarga.Rows(intI).Cells(1).Text = 0
                        End If
                        intColAtual = InStr(intColAnterior + 1, strValor, Chr(10), CompareMethod.Binary)
                    Next
                    tblCarga.Rows(49).Cells(1).Text = intMedia
                    tblCarga.Rows(50).Cells(1).Text = CInt(intMedia / 48)
                    strValor = ""
                End If
            End If
            'Select Case btnSalvar.Text
            'Case Is = "Alterar"
            If btnAlterar.Visible Or btnIncluir.Visible Then
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
                divValor.Style.Item("TOP") = "175px"
                divValor.Style.Item("LEFT") = "240px"
                divValor.Visible = True
                btnAlterar.Visible = False
                btnIncluir.Visible = False
                btnSalvar.Visible = True

                'btnSalvar.Text = "Salvar"
            Else
                'Case Is = "Salvar"
                'Alterando os valores de carga na BDT
                objTrans = Conn.BeginTransaction()
                Cmd.Transaction = objTrans
                Try
                    If blnIncluir = True Then
                        For intI = 1 To 48
                            Cmd.CommandText = "INSERT INTO tb_cargaestudo " & _
                                              "(datpdp, codempre, intcarga, valcarga) " & _
                                              "VALUES (" & _
                                              "'" & Format(CDate(txtData.Text), "yyyyMMdd") & "', " & _
                                              "'" & cboEmpresa.SelectedItem.Value & "', " & _
                                              "" & intI & ", " & _
                                              "" & Val(tblCarga.Rows(intI).Cells(1).Text) & ")"
                            Cmd.ExecuteNonQuery()
                        Next
                        blnIncluir = False
                    Else
                        For intI = 1 To 48
                            Cmd.CommandText = "UPDATE tb_cargaestudo " & _
                                              "SET valcarga = " & Val(tblCarga.Rows(intI).Cells(1).Text) & " " & _
                                              "WHERE datPdp = '" & Format(CDate(txtData.Text), "yyyyMMdd") & "' " & _
                                              "AND codempre = '" & cboEmpresa.SelectedItem.Value & "' " & _
                                              "AND intCarga = " & intI
                            Cmd.ExecuteNonQuery()
                        Next
                    End If

                    'Grava evento registrando o recebimento da carga
                    GravaEventoPDP("8", Format(CDate(txtData.Text), "yyyyMMdd"), cboEmpresa.SelectedItem.Text, datAtual, "PDPColCarg", UsuarID)

                    'Grava toda a transação
                    objTrans.Commit()
                    btnSalvar.Visible = False
                    btnIncluir.Visible = False
                    btnAlterar.Visible = True
                    AtualizaGrid()
                    'btnSalvar.Text = "Alterar"
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

    Private Sub btnCalendario_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCalendario.Click
        'Session("dtSelecao") = calData.SelectedDate
        'If txtData.Text <> "" Then
        '    calData.SelectedDate = CType(txtData.Text, Date)
        'Else
        '    calData.SelectedDate = System.DateTime.Today.Date
        'End If
        calData.SelectedDate = CDate("01/01/1900")
        'calData.SelectedDates
        divCal.Visible = True
    End Sub

    Private Sub calData_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calData.SelectionChanged
        txtData.Text = Format(calData.SelectedDate.Date, "dd/MM/yyyy")
        divCal.Visible = False
        AtualizaGrid()
    End Sub

    Private Sub btnIncluir_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnIncluir.Click
        divCal.Visible = False
        SalvaRegistro()
    End Sub

    Private Sub AtualizaGrid()
        Dim intI As Integer
        divValor.Visible = False
        btnSalvar.Visible = False
        btnAlterar.Visible = True
        'btnSalvar.Text = "Alterar"
        If cboEmpresa.SelectedIndex = 0 Then
            btnSalvar.Visible = False
            btnAlterar.Visible = False
            btnIncluir.Visible = False
            For intI = 1 To 50
                tblCarga.Rows(intI).Cells(1).Text = ""
            Next
        ElseIf cboEmpresa.SelectedIndex > 0 Then
            Session("strCodEmpre") = cboEmpresa.SelectedItem.Value
            Dim intMedia, intQtdReg As Integer
            Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
            Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
            Cmd.Connection = Conn
            Try
                Cmd.CommandText = "SELECT intcarga, valcarga " & _
                                  "FROM tb_cargaestudo " & _
                                  "WHERE codempre = '" & Session("strCodEmpre") & "' " & _
                                  "AND datpdp = '" & Format(CDate(txtData.Text), "yyyyMMdd") & "' " & _
                                  "ORDER BY intcarga"
                Conn.Open("pdp")
                Dim rsCarga As OnsClasses.OnsData.OnsDataReader = Cmd.ExecuteReader
                intMedia = 0
                intQtdReg = 0
                Do While rsCarga.Read
                    If Not IsDBNull(rsCarga.Item("valcarga")) Then
                        tblCarga.Rows(rsCarga.Item("intcarga")).Cells(1).Text = rsCarga.Item("valcarga")
                        intMedia += rsCarga.Item("valcarga")
                    Else
                        tblCarga.Rows(rsCarga.Item("intcarga")).Cells(1).Text = 0
                    End If
                    intQtdReg = intQtdReg + 1
                Loop
                If intQtdReg = 0 Then
                    btnAlterar.Visible = False
                    btnIncluir.Visible = True
                    blnIncluir = True
                    For intI = 1 To 50
                        tblCarga.Rows(intI).Cells(1).Text = ""
                    Next
                Else
                    btnAlterar.Visible = True
                    btnIncluir.Visible = False
                    blnIncluir = False
                    If intMedia <> 0 Then
                        tblCarga.Rows(49).Cells(1).Text = intMedia
                        tblCarga.Rows(50).Cells(1).Text = CInt(intMedia / 48)
                    Else
                        tblCarga.Rows(49).Cells(1).Text = 0
                        tblCarga.Rows(50).Cells(1).Text = 0
                    End If
                End If

                btnSalvar.Visible = False
                rsCarga.Close()
                rsCarga = Nothing

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

                'Conn.Close()
            Catch
                Session("strMensagem") = "Não foi possível acessar a Base de Dados. Ocorreu o seguinte erro: " & Chr(10) & Err.Description
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

End Class

Module Globais
    Public objMenu As OnsWebControls.OnsMenu
End Module
