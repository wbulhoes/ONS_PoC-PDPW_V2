Partial Class frmCadRequisito
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
        'Put user code to initialize the page here
        If SessaoAtiva(Page.Session) Then
            lblMensagem.Text = ""
            If Not Page.IsPostBack Then
                Dim intI As Integer
                Dim intSelecionado As Integer
                Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
                Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
                Cmd.Connection = Conn
                Try
                    Conn.Open("rpdp")
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
                        objItem.Value = rsData("datpdp")
                        cboData.Items.Add(objItem)
                        If rsData("datpdp") = Request.QueryString("datPDP") Then
                            intSelecionado = intI
                        End If
                        intI = intI + 1
                    Loop
                    If intSelecionado <> 0 Then
                        cboData.SelectedIndex = intSelecionado
                    End If
                    rsData.Close()
                    rsData = Nothing
                    Cmd.Connection.Close()
                    Conn.Close()
                    Call cboData_SelectedIndexChanged(sender, e)
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

    Private Sub cboData_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboData.SelectedIndexChanged
        Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
        Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
        Cmd.Connection = Conn
        Try
            If cboData.SelectedValue <> 0 Then
                Conn.Open("rpdp")
                'Por enquanto somente o relatório do 
                Cmd.CommandText = "SELECT valreqmax, hreqmax, valresreqmax, hresreqmax " & _
                                  "FROM requisitos_area " & _
                                  "WHERE datpdp = '" & cboData.SelectedValue & "' " & _
                                  "AND codarea = 'NE'"
                Dim rsRequisito As OnsClasses.OnsData.OnsDataReader = Cmd.ExecuteReader
                txtVlrRequisito.Text = ""
                txtHorRequisito.Text = ""
                txtVlrReserva.Text = ""
                txtHorReserva.Text = ""
                If rsRequisito.Read Then
                    txtVlrRequisito.Text = rsRequisito("valreqmax")
                    txtHorRequisito.Text = rsRequisito("hreqmax")
                    txtVlrReserva.Text = rsRequisito("valresreqmax")
                    txtHorReserva.Text = rsRequisito("hresreqmax")
                End If
                rsRequisito.Close()
                rsRequisito = Nothing
                'Cmd.Connection.Close()
                'Conn.Close()
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

    Private Sub btnSalvar_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSalvar.Click
        Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
        Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
        Dim intQtd As Int16
        Cmd.Connection = Conn
        Try
            If txtHorRequisito.Text.Trim.Length = 5 And txtHorReserva.Text.Trim.Length = 5 Then
                If IsDate("01/01/2000 " & txtHorRequisito.Text.Trim) And _
                   IsDate("01/01/2000 " & txtHorReserva.Text.Trim) Then
                    Conn.Open("rpdp")
                    'Por enquanto somente o relatório do 
                    Cmd.CommandText = "UPDATE requisitos_area " & _
                                    "SET valreqmax = " & txtVlrRequisito.Text & ", " & _
                                    "hreqmax = '" & txtHorRequisito.Text & "', " & _
                                    "valresreqmax = " & txtVlrReserva.Text & ", " & _
                                    "hresreqmax = '" & txtHorReserva.Text & "' " & _
                                    "WHERE datpdp = '" & cboData.SelectedValue & "' " & _
                                    "AND codarea = 'NE'"
                    intQtd = Cmd.ExecuteNonQuery()
                    If intQtd = 0 Then
                        Cmd.CommandText = "INSERT INTO requisitos_area " & _
                                        "(datpdp, codarea, valreqmax, hreqmax, valresreqmax, hresreqmax) " & _
                                        "Values " & _
                                        "('" & cboData.SelectedValue & "', " & _
                                        "'NE', " & _
                                        txtVlrRequisito.Text & ", " & _
                                        "'" & txtHorRequisito.Text & "', " & _
                                        txtVlrReserva.Text & ", " & _
                                        "'" & txtHorReserva.Text & "')"
                        Cmd.ExecuteNonQuery()
                    End If
                    cboData.SelectedIndex = 0
                    txtVlrRequisito.Text = ""
                    txtHorRequisito.Text = ""
                    txtVlrReserva.Text = ""
                    txtHorReserva.Text = ""
                    'Cmd.Connection.Close()
                    'Conn.Close()
                Else
                    lblMensagem.Text = "Horários Inválidos."
                End If
            Else
                lblMensagem.Text = "Horários Inválidos."
            End If
        Catch
            Session("strMensagem") = "Não foi possível gravar o registro."
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
