Partial Class frmColBalanco

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
                Cmd.Connection.Close()
                Conn.Close()
                PreencheComboEmpresaPOP(AgentesRepresentados, cboEmpresa, Session("strCodEmpre"))
                If cboData.SelectedIndex > 0 Then
                    cboData_SelectedIndexChanged(sender, e)
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
    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        If SessaoAtiva(Page.Session) Then
            MyBase.Render(writer)
            'objMenu.RenderControl(writer)
        End If
    End Sub

    Private Sub cboEmpresa_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboEmpresa.SelectedIndexChanged
        If cboEmpresa.SelectedIndex > 0 Then
            Session("strCodEmpre") = cboEmpresa.SelectedItem.Value
            PreencheTable()

            'Valida Limite de Envio
            Dim lRetorno As Integer = 0
            If Not ValidaLimiteEntradaDados(cboEmpresa.SelectedItem.Value, cboData.SelectedItem.Value, lRetorno) Then
                'btnSalvar.Visible = False
                If lRetorno = 1 Then
                    Response.Write("<SCRIPT>alert('" + strMsgInicioLimiteEnvioDados + "')</SCRIPT>")
                Else
                    Response.Write("<SCRIPT>alert('" + strMsgLimiteEnvioDados + "')</SCRIPT>")
                End If
                Exit Sub
            End If
        End If
    End Sub

    Private Sub PreencheTable()
        Dim intI, intJ As Integer
        If cboEmpresa.SelectedIndex = 0 Then
            For intI = 1 To 50
                For intJ = 1 To 4
                    tblBalanco.Rows(intI).Cells(intJ).Text = ""
                Next
            Next
        Else
            Dim intMedia As Double
            Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
            Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
            Cmd.Connection = Conn

            Try
                'GERAÇÃO
                Cmd.CommandText = "Select Sum(g.valdespatran) As valdespatran, " &
                                "       g.intdespa " &
                                "From usina u, " &
                                "     despa g " &
                                "Where u.codempre = '" & Session("strCodEmpre") & "' And " &
                                "      u.codusina = g.codusina And " &
                                "      g.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' " &
                                "      And u.flg_recebepdpage = 'S' " &
                                "Group By intdespa " &
                                "Order By intdespa"
                Conn.Open("pdp")
                Dim rsGeracao As OnsClasses.OnsData.OnsDataReader = Cmd.ExecuteReader
                intMedia = 0
                Do While rsGeracao.Read
                    If Not IsDBNull(rsGeracao("valdespatran")) Then
                        tblBalanco.Rows(rsGeracao("intdespa")).Cells(1).Text = rsGeracao("valdespatran")
                        intMedia = intMedia + rsGeracao("valdespatran")
                    Else
                        tblBalanco.Rows(rsGeracao("intdespa")).Cells(1).Text = 0
                    End If
                Loop
                If intMedia <> 0 Then
                    tblBalanco.Rows(49).Cells(1).Text = intMedia / 2
                    tblBalanco.Rows(50).Cells(1).Text = Int(intMedia / 48)
                Else
                    tblBalanco.Rows(49).Cells(1).Text = 0
                    tblBalanco.Rows(50).Cells(1).Text = 0
                End If
                btnSalvar.Visible = True
                rsGeracao.Close()
                rsGeracao = Nothing

                'CARGA
                Cmd.CommandText = "Select valcargatran, " & _
                                "       intcarga " & _
                                "From carga " & _
                                "Where codempre = '" & Session("strCodEmpre") & "' And " & _
                                "      datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' " & _
                                "Order By intcarga"
                Dim rsCarga As OnsClasses.OnsData.OnsDataReader = Cmd.ExecuteReader
                intMedia = 0
                Do While rsCarga.Read
                    If Not IsDBNull(rsCarga("valcargatran")) Then
                        tblBalanco.Rows(rsCarga("intcarga")).Cells(2).Text = rsCarga("valcargatran")
                        intMedia = intMedia + rsCarga("valcargatran")
                    Else
                        tblBalanco.Rows(rsCarga("intcarga")).Cells(2).Text = 0
                    End If
                Loop
                If intMedia <> 0 Then
                    tblBalanco.Rows(49).Cells(2).Text = intMedia / 2
                    tblBalanco.Rows(50).Cells(2).Text = Int(intMedia / 48)
                Else
                    tblBalanco.Rows(49).Cells(2).Text = 0
                    tblBalanco.Rows(50).Cells(2).Text = 0
                End If
                btnSalvar.Visible = True
                rsCarga.Close()
                rsCarga = Nothing

                'INTERCÂMBIO
                Cmd.CommandText = "Select Sum(valintertran) As valintertran, " & _
                                "       intinter " & _
                                "From inter " & _
                                "Where codemprede = '" & Session("strCodEmpre") & "' And " & _
                                "      datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' " & _
                                "Group By intinter " & _
                                "Order By intinter"
                Dim rsIntercambio As OnsClasses.OnsData.OnsDataReader = Cmd.ExecuteReader
                intMedia = 0
                Do While rsIntercambio.Read
                    If Not IsDBNull(rsIntercambio("valintertran")) Then
                        tblBalanco.Rows(rsIntercambio("intinter")).Cells(3).Text = rsIntercambio("valintertran")
                        intMedia = intMedia + rsIntercambio("valintertran")
                    Else
                        tblBalanco.Rows(rsIntercambio("intinter")).Cells(3).Text = 0
                    End If
                Loop
                If intMedia <> 0 Then
                    tblBalanco.Rows(49).Cells(3).Text = intMedia / 2
                    tblBalanco.Rows(50).Cells(3).Text = Int(intMedia / 48)
                Else
                    tblBalanco.Rows(49).Cells(3).Text = 0
                    tblBalanco.Rows(50).Cells(3).Text = 0
                End If
                btnSalvar.Visible = True
                rsIntercambio.Close()
                rsIntercambio = Nothing

                'Verficando Balanço
                intMedia = 0
                For intI = 1 To 48
                    tblBalanco.Rows(intI).Cells(4).Text = ((Val(tblBalanco.Rows(intI).Cells(2).Text) + _
                                                            Val(tblBalanco.Rows(intI).Cells(3).Text)) - _
                                                            Val(tblBalanco.Rows(intI).Cells(1).Text))
                    intMedia = intMedia + Val(tblBalanco.Rows(intI).Cells(4).Text)
                Next
                If intMedia <> 0 Then
                    tblBalanco.Rows(49).Cells(4).Text = intMedia / 2
                    tblBalanco.Rows(50).Cells(4).Text = Int(intMedia / 48)
                Else
                    tblBalanco.Rows(49).Cells(4).Text = 0
                    tblBalanco.Rows(50).Cells(4).Text = 0
                End If
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

    Private Sub cboData_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboData.SelectedIndexChanged
        Try
            If cboData.SelectedIndex > 0 Then
                Session("datEscolhida") = CDate(cboData.SelectedItem.Value)
            End If
            If cboEmpresa.SelectedIndex > 0 Then
                cboEmpresa_SelectedIndexChanged(sender, e)
            End If
        Catch
            Session("strMensagem") = "Não foi possível acessar a Base de Dados."
            Response.Redirect("frmMensagem.aspx")
        End Try
    End Sub

    Private Sub btnSalvar_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSalvar.Click
        PreencheTable()
    End Sub
End Class
