Partial Class frmCnsObservacao
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
        objMenu = CType(Session("onsmenu"), OnsWebControls.OnsMenu)
        If Not Page.IsPostBack Then
            Dim intI As Integer
            Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
            Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
            Dim objitem As WebControls.ListItem
            Cmd.Connection = Conn
            Try
                Conn.Open("pdp")
                Cmd.CommandText = "Select datpdp " & _
                                  "From pdp " & _
                                  "Order By datpdp Desc"
                Dim rsData As OnsClasses.OnsData.OnsDataReader = Cmd.ExecuteReader
                intI = 1

                objitem = New System.Web.UI.WebControls.ListItem
                objitem.Text = ""
                objitem.Value = "0"
                cboDataPdp.Items.Add(objitem)
                Do While rsData.Read
                    objitem = New System.Web.UI.WebControls.ListItem
                    objitem.Text = Mid(rsData("datpdp"), 7, 2) & "/" & Mid(rsData("datpdp"), 5, 2) & "/" & Mid(rsData("datpdp"), 1, 4)
                    objitem.Value = rsData("datpdp")
                    cboDataPdp.Items.Add(objitem)

                    If Trim(cboDataPdp.Items(intI).Value) = Format(Session("datEscolhida"), "yyyyMMdd") Then
                        cboDataPdp.SelectedIndex = intI
                    End If
                    intI = intI + 1
                Loop
                rsData.Close()
                rsData = Nothing
                'Cmd.Connection.Close()
                'Conn.Close()
            Catch
                Session("strMensagem") = "Não foi possível acessar a Página. ocorreu o seguinte erro: " & Chr(10) & Err.Description
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
            objMenu.RenderControl(writer)
        End If
    End Sub

    Private Sub btnPesquisar_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnPesquisar.Click
        Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
        Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
        Cmd.Connection = Conn
        Try
            Conn.Open("pdp")
            Cmd.CommandText = "Select '   ' || observacao As obs " & _
                              "From observ_diar " & _
                              "Where datpdp = '" & cboDataPdp.SelectedItem.Value & "'"
            Dim drObs As OnsClasses.OnsData.OnsDataReader = Cmd.ExecuteReader
            txtObs.Text = ""
            Do While drObs.Read
                txtObs.Text &= drObs("obs").ToString.TrimEnd & Chr(10) & Chr(10)
            Loop
            drObs.Close()
            'Conn.Close()
        Catch
            Session("strMensagem") = "Não foi possível acessar a Página. ocorreu o seguinte erro: " & Chr(10) & Err.Description
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
End Class
