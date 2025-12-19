Partial Class frmColParadaUG
    Inherits System.Web.UI.Page

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
        If SessaoAtiva(Page.Session) Then
            If Not Page.IsPostBack Then
                Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
                Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
                If Session("datEscolhida") = Nothing Then
                    'Inicializa a variável com data do próximo
                    Session("datEscolhida") = Now.AddDays(1)
                End If
                Cmd.Connection = Conn

                Try
                    Cmd.CommandText = "SELECT codgerad " & _
                                      "FROM gerad " & _
                                      "WHERE codusina = '" & Session("strCodUsina") & "'"
                    Conn.Open("pdp")
                    Dim rsGerador As OnsClasses.OnsData.OnsDataReader = Cmd.ExecuteReader
                    Do While rsGerador.Read
                        cboGerador.Items.Add(rsGerador("codgerad"))
                    Loop
                    rsGerador.Close()
                    rsGerador = Nothing
                    Cmd.Connection.Close()
                    Conn.Close()

                    If Session("strAcao") = "Alterar" Then
                        InicializaControles()
                    Else
                        txtDataInicial.Text = Format(Session("datEscolhida"), "dd/MM/yyyy")
                        txtDataFinal.Text = Format(Session("datEscolhida"), "dd/MM/yyyy")
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
            End If
        End If
    End Sub

    Private Sub btnVoltar_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnVoltar.Click
        Server.Transfer("frmParadaUG.aspx")
    End Sub

    Private Sub InicializaControles()
        Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
        Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
        Cmd.Connection = Conn
        Try
            Cmd.CommandText = "SELECT codparal, codequip, tipequip, datiniparal, datfimparal, intiniparal, intfimparal, datpdp " & _
                              "FROM paralemp_co " & _
                              "WHERE codparal = '" & Session("strcodparal") & "'"
            Conn.Open("pdp")
            Dim rsParalEmp As OnsClasses.OnsData.OnsDataReader = Cmd.ExecuteReader
            Dim intI As Integer
            Do While rsParalEmp.Read
                txtCod.Text = rsParalEmp("codparal")
                For intI = 0 To cboGerador.Items.Count - 1
                    If Trim(cboGerador.Items.Item(intI).Text) = Trim(rsParalEmp("codequip")) Then
                        cboGerador.SelectedIndex = intI
                    End If
                Next
                txtDataInicial.Text = IIf(Not IsDBNull(rsParalEmp("datiniparal")), (Mid(rsParalEmp("datiniparal"), 7, 2) & "/" & Mid(rsParalEmp("datiniparal"), 5, 2) & "/" & Mid(rsParalEmp("datiniparal"), 1, 4)), "")
                cboHoraInicial.SelectedIndex = IIf(Not IsDBNull(rsParalEmp("intiniparal")), rsParalEmp("intiniparal"), 0)
                If Not IsDBNull(rsParalEmp("datfimparal")) Then
                    txtDataFinal.Text = Mid(rsParalEmp("datfimparal"), 7, 2) & "/" & Mid(rsParalEmp("datfimparal"), 5, 2) & "/" & Mid(rsParalEmp("datfimparal"), 1, 4)
                End If
                cboHoraFinal.SelectedIndex = IIf(Not IsDBNull(rsParalEmp("intfimparal")), rsParalEmp("intfimparal"), 0)
            Loop
            rsParalEmp.Close()
            rsParalEmp = Nothing
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

    Private Sub btnSalvar_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSalvar.Click
        'Dim objPermissao As OnsClasses.OnsInterfaceUnica.OnsPermissao
        If Session("strAcao") = "Alterar" Then
            'objPermissao = New OnsClasses.OnsInterfaceUnica.OnsPermissao("UPD", "PDPColPaUG", UsuarID)
        Else
            'objPermissao = New OnsClasses.OnsInterfaceUnica.OnsPermissao("INS", "PDPColPaUG", UsuarID)
        End If
        'Verifica se o usuário tem permissão para salvar os registros
        If EstaAutorizado Then
            Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
            Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
            Cmd.Connection = Conn
            Dim intI As Integer
            If IsDate(txtDataInicial.Text) And _
                IsDate(txtDataFinal.Text) And _
                cboHoraInicial.SelectedItem.Text <> "" And _
                cboHoraFinal.SelectedItem.Text <> "" Then
                If (CDate(txtDataInicial.Text) < CDate(txtDataFinal.Text)) Or _
                ((CDate(txtDataInicial.Text) = CDate(txtDataFinal.Text)) And _
                (cboHoraInicial.SelectedIndex <= cboHoraFinal.SelectedIndex)) Then
                    Try
                        If Session("strAcao") = "Alterar" Then
                            'Alteração
                            Cmd.CommandText = "UPDATE paralemp_co " & _
                                              "SET codequip = '" & cboGerador.SelectedItem.Value & "', " & _
                                              "datiniparal = '" & Format(CDate(txtDataInicial.Text), "yyyyMMdd") & "',  " & _
                                              "datfimparal = '" & Format(CDate(txtDataFinal.Text), "yyyyMMdd") & "', " & _
                                              "intiniparal = " & cboHoraInicial.SelectedIndex & ", " & _
                                              "intfimparal = " & cboHoraFinal.SelectedIndex & " " & _
                                              "WHERE codparal = '" & Session("strCodParal") & "'"

                        Else
                            'Inclusão
                            Cmd.CommandText = "INSERT INTO paralemp_co (" & _
                                              "codequip, " & _
                                              "tipequip, " & _
                                              "datpdp, " & _
                                              "datiniparal, " & _
                                              "datfimparal, " & _
                                              "intiniparal, " & _
                                              "intfimparal " & _
                                              ") Values (" & _
                                              "'" & cboGerador.SelectedItem.Value & "', " & _
                                              "'" & "GD" & "', " & _
                                              "'" & Format(Session("datEscolhida"), "yyyyMMdd") & "', " & _
                                              "'" & Format(CDate(txtDataInicial.Text), "yyyyMMdd") & "',  " & _
                                              "'" & Format(CDate(txtDataFinal.Text), "yyyyMMdd") & "', " & _
                                              "" & cboHoraInicial.SelectedIndex & ", " & _
                                              "" & cboHoraFinal.SelectedIndex & ")"
                        End If
                        Conn.Open("pdp")
                        Conn.Servico = "PDPColPaUG"
                        Conn.Usuario = UsuarID
                        Cmd.ExecuteNonQuery()
                        Cmd.Connection.Close()
                        Conn.Close()

                        'Grava evento registrando o recebimento da Parada por Conveniência Operativa
                        GravaEventoPDP("49", Format(Session("datEscolhida"), "yyyyMMdd"), Session("strSigEmpre"), Now, "PDPColPaUG", UsuarID)
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
                    Server.Transfer("frmParadaUG.aspx")
                Else
                    Session("strMensagem") = "Data inicial deve ser menor que a data final !"
                    Response.Redirect("frmMensagem.aspx")
                End If
            Else
                Session("strMensagem") = "Data Inválida !"
                Response.Redirect("frmMensagem.aspx")
            End If
        Else
            Session("strMensagem") = "Usuário não tem permissão para Salvar os valores."
            Response.Redirect("frmMensagem.aspx")
        End If
    End Sub

End Class
