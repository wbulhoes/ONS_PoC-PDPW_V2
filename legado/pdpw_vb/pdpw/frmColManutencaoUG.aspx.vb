Imports System.Data.OleDb

Partial Class frmColManutencaoUG

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
                    Cmd.CommandText = "Select codgerad " & _
                                    "From gerad " & _
                                    "Where codusina = '" & Session("strCodUsina") & "'"
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
        Server.Transfer("frmManutencaoUG.aspx")
    End Sub

    Private Sub InicializaControles()
        Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
        Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
        Cmd.Connection = Conn
        Try
            Cmd.CommandText = "Select codparal, " & _
                                "       codequip, " & _
                                "       tipequip, " & _
                                "       datiniparal,  " & _
                                "       datfimparal, " & _
                                "       intiniparal, " & _
                                "       intfimparal, " & _
                                "       codnivel, " & _
                                "       refoutrosis, " & _
                                "       indcont, " & _
                                "       intinivoltaparal, " & _
                                "       intfimvoltaparal, " & _
                                "       datpdp " & _
                                "From   paralemp " & _
                                "Where  codparal = '" & Session("strcodparal") & "'"
            Conn.Open("pdp")
            Dim rsParalEmp As OnsClasses.OnsData.OnsDataReader = Cmd.ExecuteReader
            Dim intI As Integer
            Do While rsParalEmp.Read
                txtCod.Text = rsParalEmp("codparal")
                txtCodExterno.Text = IIf(Not IsDBNull(rsParalEmp("refoutrosis")), rsParalEmp("refoutrosis"), "")
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
                If Not IsDBNull(rsParalEmp("codnivel")) Then
                    Select Case Trim(rsParalEmp("codnivel"))
                        Case Is = "MP"
                            optNivel.Items.Item(0).Selected = True
                        Case Is = "ME"
                            optNivel.Items.Item(1).Selected = True
                        Case Is = "MU"
                            optNivel.Items.Item(2).Selected = True
                    End Select
                End If
                If Not IsDBNull(rsParalEmp("indCont")) Then
                    Select Case Trim(rsParalEmp("indcont"))
                        Case Is = "D"
                            cboContinuidade.SelectedIndex = 0
                        Case Is = "C"
                            cboContinuidade.SelectedIndex = 1
                        Case Is = "V"
                            cboContinuidade.SelectedIndex = 2
                    End Select
                    If rsParalEmp("indcont") = "V" Then
                        cboVoltaInicial.SelectedIndex = IIf(Not IsDBNull(rsParalEmp("intinivoltaparal")), rsParalEmp("intinivoltaparal"), 0)
                        cboVoltaFinal.SelectedIndex = IIf(Not IsDBNull(rsParalEmp("intfimvoltaparal")), rsParalEmp("intfimvoltaparal"), 0)
                        cboVoltaInicial.Enabled = True
                        cboVoltaFinal.Enabled = True
                    End If
                End If
                '''txtObservacao.Text = IIf(Not IsDBNull(rsParalEmp("obsparal")), rsParalEmp("obsparal"), "")
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
            'objPermissao = New OnsClasses.OnsInterfaceUnica.OnsPermissao("UPD", "PDPColMaUG", UsuarID)
        Else
            'objPermissao = New OnsClasses.OnsInterfaceUnica.OnsPermissao("INS", "PDPColMaUG", UsuarID)
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
                            Cmd.CommandText = "Update paralemp " & _
                                                "Set refoutrosis = " & IIf(txtCodExterno.Text <> "", txtCodExterno.Text, "Null") & ", " & _
                                                "    codequip = '" & cboGerador.SelectedItem.Value & "', " & _
                                                "    datiniparal = '" & Format(CDate(txtDataInicial.Text), "yyyyMMdd") & "',  " & _
                                                "    datfimparal = '" & Format(CDate(txtDataFinal.Text), "yyyyMMdd") & "', " & _
                                                "    intiniparal = " & cboHoraInicial.SelectedIndex & ", " & _
                                                "    intfimparal = " & cboHoraFinal.SelectedIndex & ", "
                            For intI = 0 To 2
                                If optNivel.Items.Item(intI).Selected = True Then
                                    Cmd.CommandText = Cmd.CommandText & _
                                    "    codnivel = '" & optNivel.Items.Item(intI).Value & "', "
                                End If
                            Next
                            Cmd.CommandText = Cmd.CommandText & _
                                                "    indcont = '" & cboContinuidade.SelectedItem.Value & "', " & _
                                                "    intinivoltaparal = " & IIf(cboVoltaInicial.SelectedItem.Text <> "", cboVoltaInicial.SelectedIndex, "Null") & ", " & _
                                                "    intfimvoltaparal = " & IIf(cboVoltaFinal.SelectedItem.Text <> "", cboVoltaFinal.SelectedIndex, "Null") & " " & _
                                                "Where codparal = '" & Session("strCodParal") & "'"

                            '''If txtObservacao.Text <> "" Then
                            '''    'Quando existir texto atualizar com parâmetro
                            '''    Cmd.CommandText = Cmd.CommandText & _
                            '''                    "    obsparal = ? " & _
                            '''                    "Where codparal = '" & Session("strCodParal") & "'"
                            '''    'Atualização do Arquivo BLOB através de parâmetro
                            '''    'Criar um array de bytes
                            '''    Dim arrByte(Len(Trim(txtObservacao.Text)) - 1) As Byte
                            '''    For intI = 0 To Len(Trim(txtObservacao.Text)) - 1
                            '''        arrByte(intI) = CByte(Asc(Mid(txtObservacao.Text, intI + 1, 1)))
                            '''    Next
                            '''    'Criar o parâmetro para receber o array de bytes
                            '''    'Dim objParam As New OleDbParameter("@obsparal", OleDbType.LongVarBinary, arrByte.Length, ParameterDirection.Input, False, 0, 0, Nothing, DataRowVersion.Current, arrByte)
                            '''    Dim objparam As New IBM.Data.Informix.IfxParameter("@obsparal", IBM.Data.Informix.IfxType.Byte, arrByte.Length, ParameterDirection.Input, False, 0, 0, Nothing, DataRowVersion.Current, arrByte)
                            '''    Cmd.Parameters.Add(objParam)
                            '''Else
                            '''    'se não existir texto atualizar com Null
                            '''    Cmd.CommandText = Cmd.CommandText & _
                            '''                    "    obsparal = Null " & _
                            '''                    "Where codparal = '" & Session("strCodParal") & "'"
                            '''End If
                        Else
                            'Inclusão
                            Cmd.CommandText = "Insert Into paralemp (" & _
                                                "refoutrosis, " & _
                                                "codequip, " & _
                                                "tipequip, " & _
                                                "datpdp, " & _
                                                "datiniparal, " & _
                                                "datfimparal, " & _
                                                "intiniparal, " & _
                                                "intfimparal, " & _
                                                "codnivel, " & _
                                                "indcont, " & _
                                                "intinivoltaparal, " & _
                                                "intfimvoltaparal " & _
                                                ") Values (" & _
                                                "" & IIf(txtCodExterno.Text <> "", txtCodExterno.Text, "Null") & ", " & _
                                                "'" & cboGerador.SelectedItem.Value & "', " & _
                                                "'" & "GD" & "', " & _
                                                "'" & Format(Session("datEscolhida"), "yyyyMMdd") & "', " & _
                                                "'" & Format(CDate(txtDataInicial.Text), "yyyyMMdd") & "',  " & _
                                                "'" & Format(CDate(txtDataFinal.Text), "yyyyMMdd") & "', " & _
                                                "" & cboHoraInicial.SelectedIndex & ", " & _
                                                "" & cboHoraFinal.SelectedIndex & ", "
                            For intI = 0 To 2
                                If optNivel.Items.Item(intI).Selected = True Then
                                    Cmd.CommandText = Cmd.CommandText & _
                                    "'" & optNivel.Items.Item(intI).Value & "', "
                                End If
                            Next
                            Cmd.CommandText = Cmd.CommandText & _
                                                "'" & cboContinuidade.SelectedItem.Value & "', " & _
                                                "" & IIf(cboVoltaInicial.SelectedItem.Text <> "", cboVoltaInicial.SelectedIndex, "Null") & ", " & _
                                                "" & IIf(cboVoltaFinal.SelectedItem.Text <> "", cboVoltaFinal.SelectedIndex, "Null") & ")"
                            '''If txtObservacao.Text <> "" Then
                            '''    Cmd.CommandText = Cmd.CommandText & _
                            '''    ", ?)"
                            '''    Dim arrByte(Len(Trim(txtObservacao.Text)) - 1) As Byte
                            '''    For intI = 0 To Len(Trim(txtObservacao.Text)) - 1
                            '''        arrByte(intI) = CByte(Asc(Mid(txtObservacao.Text, intI + 1, 1)))
                            '''    Next
                            '''    'Dim objParam As New OleDbParameter("@obsparal", OleDbType.LongVarBinary, arrByte.Length, ParameterDirection.Input, False, 0, 0, Nothing, DataRowVersion.Current, arrByte)
                            '''    Dim objparam As New IBM.Data.Informix.IfxParameter("@obsparal", IBM.Data.Informix.IfxType.Byte, arrByte.Length, ParameterDirection.Input, False, 0, 0, Nothing, DataRowVersion.Current, arrByte)
                            '''    Cmd.Parameters.Add(objParam)
                            '''Else
                            '''    Cmd.CommandText = Cmd.CommandText & _
                            '''    ", Null)"
                            '''End If
                        End If
                        Conn.Open("pdp")
                        Conn.Servico = "PDPColMaUG"
                        Conn.Usuario = UsuarID
                        Cmd.ExecuteNonQuery()
                        Cmd.Connection.Close()
                        Conn.Close()

                        'Grava evento registrando o recebimento de Manutenção
                        GravaEventoPDP("5", Format(Session("datEscolhida"), "yyyyMMdd"), Session("strSigEmpre"), Now, "PDPColMaUG", UsuarID)
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
                    Server.Transfer("frmManutencaoUG.aspx")
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

    Private Sub cboContinuidade_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboContinuidade.SelectedIndexChanged
        If cboContinuidade.SelectedIndex = 2 Then
            cboVoltaInicial.Enabled = True
            cboVoltaFinal.Enabled = True
        Else
            cboVoltaInicial.SelectedIndex = 0
            cboVoltaInicial.Enabled = False
            cboVoltaFinal.SelectedIndex = 0
            cboVoltaInicial.Enabled = False
        End If
    End Sub
End Class
