Imports System.Data.OleDb

Partial Class frmColRestricaoUS

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
        If Not Page.IsPostBack Then
            Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
            Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
            If Session("datEscolhida") = Nothing Then
                'Inicializa a variável com data do próximo
                Session("datEscolhida") = Now.AddDays(1)
            End If
            Cmd.Connection = Conn

            Try
                Cmd.CommandText = "Select codusina, potinstalada " & _
                                "From usina " & _
                                "Where codempre = '" & Session("strCodEmpre") & "'"
                Conn.Open("pdp")
                Dim rsUsina As OnsClasses.OnsData.OnsDataReader = Cmd.ExecuteReader
                Dim inti As Integer = 1
                Do While rsUsina.Read
                    If inti = 1 Then
                        txtValGer.Text = rsUsina("potinstalada")
                        inti = inti + 1
                    End If
                    cboUsina.Items.Add(rsUsina("codusina"))
                Loop
                rsUsina.Close()
                rsUsina = Nothing
                Conn.Close()

                '-- CRQ6867 - 23/09/2013
                Cmd.CommandText = "SELECT dsc_motivorestricao, id_motivorestricao " & _
                                "FROM tb_motivorestricao " & _
                                "ORDER BY dsc_motivorestricao"
                Conn.Open("pdp")
                Dim rsMotivo As OnsClasses.OnsData.OnsDataReader = Cmd.ExecuteReader
                Dim objItem As WebControls.ListItem
                objItem = New System.Web.UI.WebControls.ListItem
                objItem.Text = ""
                objItem.Value = "0"
                cboMotivo.Items.Add(objItem)
                Do While rsMotivo.Read
                    objItem = New System.Web.UI.WebControls.ListItem
                    objItem.Text = rsMotivo("dsc_motivorestricao")
                    objItem.Value = rsMotivo("id_motivorestricao")
                    cboMotivo.Items.Add(objItem)
                Loop
                rsMotivo.Close()
                rsMotivo = Nothing
                '--

                If Session("strAcao") = "Alterar" Then
                    InicializaControles()
                Else
                    txtDataInicial.Text = Format(Session("datEscolhida"), "dd/MM/yyyy")
                    txtDataFinal.Text = Format(Session("datEscolhida"), "dd/MM/yyyy")
                End If
            Catch
                Session("strMensagem") = "Não foi possível acessar a Base de Dados."
                Conn.Close()
                Response.Redirect("frmMensagem.aspx")
            End Try
        End If
    End Sub

    Private Sub btnVoltar_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnVoltar.Click
        Server.Transfer("frmRestricaoUS.aspx")
    End Sub

    Private Sub InicializaControles()
        Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
        Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
        Cmd.Connection = Conn
        Try
            Cmd.CommandText = "Select t.codrestr, " &
                                "       t.codusina, " &
                                "       t.datinirestr,  " &
                                "       t.datfimrestr, " &
                                "       t.intinirestr, " &
                                "       t.intfimrestr, " &
                                "       u.potinstalada, " &
                                "       t.valrestr, " &
                                "       t.id_motivorestricao, " &
                                "       isnull(t.obsrestr,'') obsrestr, " &
                                "       t.datpdp " &
                                "From   restrusinaemp t, " &
                                "       usina u " &
                                "Where  t.codrestr = '" & Session("strCodRestr") & "' And " &
                                "       t.codusina = u.codusina" &
                                "      And u.flg_recebepdpage = 'S' "

            'Dim erro As System.IO.StreamWriter
            'erro = New System.IO.StreamWriter("C:\temp\erro.txt", True)
            'erro.WriteLine(Cmd.CommandText)
            'erro.Close()

            Conn.Open("pdp")
            Dim rsTempRestr As OnsClasses.OnsData.OnsDataReader = Cmd.ExecuteReader
            Dim intI, intM As Integer
            Do While rsTempRestr.Read
                txtCod.Text = rsTempRestr("codrestr")
                For intI = 0 To cboUsina.Items.Count - 1
                    If Trim(cboUsina.Items.Item(intI).Text) = Trim(rsTempRestr("codusina")) Then
                        cboUsina.SelectedIndex = intI
                    End If
                Next
                '-- CRQ6867
                For intM = 0 To cboMotivo.Items.Count - 1
                    If Trim(cboMotivo.Items.Item(intM).Value) = Trim(rsTempRestr("id_motivorestricao")) Then
                        cboMotivo.SelectedIndex = intM
                    End If
                Next
                '--
                txtValGer.Text = IIf(Not IsDBNull(rsTempRestr("potinstalada")), rsTempRestr("potinstalada"), 0)
                txtValRestr.Text = IIf(Not IsDBNull(rsTempRestr("valrestr")), rsTempRestr("valrestr"), 0)
                txtDataInicial.Text = IIf(Not IsDBNull(rsTempRestr("datinirestr")), (Mid(rsTempRestr("datinirestr"), 7, 2) & "/" & Mid(rsTempRestr("datinirestr"), 5, 2) & "/" & Mid(rsTempRestr("datinirestr"), 1, 4)), "")
                cboHoraInicial.SelectedIndex = IIf(Not IsDBNull(rsTempRestr("intinirestr")), rsTempRestr("intinirestr"), 0)
                If Not IsDBNull(rsTempRestr("datfimrestr")) Then
                    txtDataFinal.Text = Mid(rsTempRestr("datfimrestr"), 7, 2) & "/" & Mid(rsTempRestr("datfimrestr"), 5, 2) & "/" & Mid(rsTempRestr("datfimrestr"), 1, 4)
                End If
                cboHoraFinal.SelectedIndex = IIf(Not IsDBNull(rsTempRestr("intfimrestr")), rsTempRestr("intfimrestr"), 0)
                txtObsRestr.Text = rsTempRestr("obsrestr") 'IIf(Not IsDBNull(rsTempRestr("obsrestr")), rsTempRestr("obsrestr"), "")
            Loop
            rsTempRestr.Close()
            rsTempRestr = Nothing
            Conn.Close()
        Catch
            Session("strMensagem") = "Não foi possível acessar a Base de Dados."
            Conn.Close()
            Response.Redirect("frmMensagem.aspx")
        End Try
    End Sub

    Private Sub btnSalvar_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSalvar.Click

        'Dim objPermissao As OnsClasses.OnsInterfaceUnica.OnsPermissao
        If Session("strAcao") = "Alterar" Then
            'objPermissao = New OnsClasses.OnsInterfaceUnica.OnsPermissao("UPD", "PDPColReUS", UsuarID)
        Else
            'objPermissao = New OnsClasses.OnsInterfaceUnica.OnsPermissao("INS", "PDPColReUS", UsuarID)
        End If
        'Verifica se o usuário tem permissão para salvar os registros
        If EstaAutorizado Then
            Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
            Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
            Cmd.Connection = Conn
            Dim intI As Integer
            Dim strSql As String
            If IsDate(txtDataInicial.Text) And _
                IsDate(txtDataFinal.Text) And _
                cboHoraInicial.SelectedItem.Text <> "" And _
                cboHoraFinal.SelectedItem.Text <> "" Then
                If (CDate(txtDataInicial.Text) < CDate(txtDataFinal.Text)) Or _
                ((CDate(txtDataInicial.Text) = CDate(txtDataFinal.Text)) And _
                (cboHoraInicial.SelectedIndex <= cboHoraFinal.SelectedIndex)) Then
                    Try
                        If Session("strAcao") = "Alterar" Then
                            '-- CRQ6867
                            If Len(Trim(txtObsRestr.Text)) > 0 Then
                                strSql = "obsrestr = '" & Trim(txtObsRestr.Text.Replace("'", "")) & "' "
                            Else
                                strSql = "obsrestr = '' "
                            End If

                            Cmd.CommandText = "Update restrusinaemp " & _
                                                "Set codusina = '" & cboUsina.SelectedItem.Value & "', " & _
                                                "    datinirestr = '" & Format(CDate(txtDataInicial.Text), "yyyyMMdd") & "',  " & _
                                                "    datfimrestr = '" & Format(CDate(txtDataFinal.Text), "yyyyMMdd") & "', " & _
                                                "    intinirestr = " & cboHoraInicial.SelectedIndex & ", " & _
                                                "    intfimrestr = " & cboHoraFinal.SelectedIndex & ", " & _
                                                "    datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "', " & _
                                                "    valrestr = " & txtValRestr.Text & ", " & _
                                                "    id_motivorestricao = " & cboMotivo.Items(cboMotivo.SelectedIndex).Value & ", " & _
                                                strSql & _
                                                "Where codrestr = '" & Session("strCodrestr") & "'"
                        Else
                            'Inclusão
                            '-- CRQ6867
                            If Len(Trim(txtObsRestr.Text)) > 0 Then
                                strSql = "'" & Trim(txtObsRestr.Text.Replace("'", "")) & "') "
                            Else
                                strSql = "'') "
                            End If
                            Cmd.CommandText = "Insert Into restrusinaemp (" & _
                                                "codusina, " & _
                                                "datinirestr, " & _
                                                "datfimrestr, " & _
                                                "intinirestr, " & _
                                                "intfimrestr, " & _
                                                "datpdp, " & _
                                                "valrestr, " & _
                                                "id_motivorestricao, " & _
                                                "obsrestr " & _
                                                ") Values (" & _
                                                "'" & cboUsina.SelectedItem.Value & "', " & _
                                                "'" & Format(CDate(txtDataInicial.Text), "yyyyMMdd") & "',  " & _
                                                "'" & Format(CDate(txtDataFinal.Text), "yyyyMMdd") & "', " & _
                                                "" & cboHoraInicial.SelectedIndex & ", " & _
                                                "" & cboHoraFinal.SelectedIndex & ", " & _
                                                "'" & Format(Session("datEscolhida"), "yyyyMMdd") & "', " & _
                                                "" & IIf(Trim(txtValRestr.Text) = "", 0, txtValRestr.Text) & ", " & _
                                                "" & cboMotivo.Items(cboMotivo.SelectedIndex).Value & ", " & _
                                                strSql
                        End If
                        Conn.Open("pdp")
                        Conn.Servico = "PDPColReUS"
                        Conn.Usuario = UsuarID
                        Cmd.ExecuteNonQuery()
                        Conn.Close()
                        'Grava evento registrando o recebimento de Restrição
                        GravaEventoPDP("4", Format(Session("datEscolhida"), "yyyyMMdd"), Session("strSigEmpre"), Now, "PDPColReUS", UsuarID)
                    Catch
                        Session("strMensagem") = "Não foi possível acessar a Base de Dados."
                        Conn.Close()
                        Response.Redirect("frmMensagem.aspx")
                    End Try
                    Server.Transfer("frmRestricaoUS.aspx")
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

    Private Sub cboUsina_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
        Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
        Cmd.Connection = Conn
        Try
            Cmd.CommandText = "Select potinstalada " & _
                                "From usina " & _
                                "Where codusina = '" & cboUsina.SelectedItem.Text & "'"
            Conn.Open("pdp")
            Dim rsUsina As OnsClasses.OnsData.OnsDataReader = Cmd.ExecuteReader
            rsUsina.Read()
            txtValGer.Text = rsUsina("potinstalada")
            rsUsina.Close()
            rsUsina = Nothing
            Conn.Close()
        Catch
            Session("strMensagem") = "Não foi possível acessar a Base de Dados."
            Conn.Close()
            Response.Redirect("frmMensagem.aspx")
        End Try
    End Sub

End Class
