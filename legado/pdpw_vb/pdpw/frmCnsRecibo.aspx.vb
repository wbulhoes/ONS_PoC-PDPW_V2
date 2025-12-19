Imports System.Collections.Generic

Partial Class frmCnsRecibo

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

            Dim pdpData As List(Of ListItem) = CacheDataPDP.GetPdpData(True)

            intI = 1
            Dim objItem As New WebControls.ListItem
            objItem.Text = ""
            objItem.Value = "0"
            drpDownDataPDP.Items.Add(objItem)

            For Each item As WebControls.ListItem In pdpData
                drpDownDataPDP.Items.Add(item)
                If Trim(drpDownDataPDP.Items(intI).Value) = Format(Session("datEscolhida"), "dd/MM/yyyy") Then
                    drpDownDataPDP.SelectedIndex = intI
                End If
                intI += 1
            Next

            objItem = New WebControls.ListItem
            objItem.Text = "-Semana PMO-"
            objItem.Value = "-1"
            drpDownDataPDP.Items.Add(objItem)

            Dim pmo As SemanaPMO = GetSemanaPMO(DateTime.Today, Nothing, Nothing)(0)

            For Each dia As DateTime In pmo.Datas_SemanaPmo
                If Not drpDownDataPDP.Items.Contains(New WebControls.ListItem(dia.ToString("dd/MM/yyyy"))) Then
                    drpDownDataPDP.Items.Add(New WebControls.ListItem(dia.ToString("dd/MM/yyyy")))
                End If
            Next

            pmo = GetProximaSemanaPMO(DateTime.Today, Nothing, Nothing)(0)

            For Each dia As DateTime In pmo.Datas_SemanaPmo
                If Not drpDownDataPDP.Items.Contains(New WebControls.ListItem(dia.ToString("dd/MM/yyyy"))) Then
                    drpDownDataPDP.Items.Add(New WebControls.ListItem(dia.ToString("dd/MM/yyyy")))
                End If
            Next

            drpDownDataPDP_SelectedIndexChanged(sender, e)
            lblMsg.Visible = False
        End If
    End Sub


    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        If SessaoAtiva(Page.Session) Then
            Funcoes.RetirarItensSelecionadosComDuplicidade(drpDownDataPDP)
            Funcoes.RetirarItensSelecionadosComDuplicidade(drpDownEmpresa)

            MyBase.Render(writer)
            'objMenu.RenderControl(writer)
        End If
    End Sub

    Private Sub drpDownDataPDP_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles drpDownDataPDP.SelectedIndexChanged
        Try

            If drpDownDataPDP.SelectedIndex <> 0 Then
                Session("datEscolhida") = CDate(drpDownDataPDP.SelectedItem.Value)
            End If
            'preenche a combo de empresas de acordo com o usuário logado
            PreencheComboEmpresaPOP(AgentesRepresentados, drpDownEmpresa, Session("strCodEmpre"))
            Carregar_Arquivo()
            drpDownEmpresa_SelectedIndexChanged(sender, e)
            lblMsg.Visible = False
        Catch
            lblMsg.Visible = True
            'Session("strMensagem") = "Não foi possível acessar a Base de Dados. Ocorreu o seguinte erro: " & Chr(10) & Err.Description
            'Response.Redirect("frmMensagem.aspx")
        End Try
    End Sub

    Private Sub drpDownEmpresa_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles drpDownEmpresa.SelectedIndexChanged
        If drpDownEmpresa.SelectedIndex > 0 Then
            Session("strCodEmpre") = drpDownEmpresa.SelectedItem.Value
        End If
        Carregar_Arquivo()
    End Sub

    Private Sub Carregar_Arquivo()
        Dim intI As Integer
        'Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
        'Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
        Dim Conn As System.Data.SqlClient.SqlConnection = New System.Data.SqlClient.SqlConnection
        Conn.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
        Dim Cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand

        Dim objItem As System.Web.UI.WebControls.ListItem
        Conn.Open()
        Cmd.Connection = Conn
        Try
            If drpDownEmpresa.SelectedItem.Text.Trim() <> "" And
                drpDownDataPDP.SelectedItem.Text.Trim() <> "" Then
                Cmd.CommandText = "Select nomarq " &
                                "From mensa " &
                                "where datpdp = '" & Trim(Mid(drpDownDataPDP.SelectedItem.Text, 7, 4)) &
                                                    Trim(Mid(drpDownDataPDP.SelectedItem.Text, 4, 2)) &
                                                    Trim(Mid(drpDownDataPDP.SelectedItem.Text, 1, 2)) & "' " &
                                    "and codempre = '" & drpDownEmpresa.SelectedItem.Value & "' " &
                                    "AND SUBSTRING(nomarq, LEN(LTRIM(RTRIM(nomarq))) - 2, 3) = 'WEB' " &
                                "order by 1"
                'Dim rsArquivo As OnsClasses.OnsData.OnsDataReader = Cmd.ExecuteReader
                Dim rsArquivo As System.Data.SqlClient.SqlDataReader = Cmd.ExecuteReader
                intI = 1
                drpDownArquivo.Items.Clear()
                objItem = New System.Web.UI.WebControls.ListItem
                objItem.Text = ""
                objItem.Value = "0"
                drpDownArquivo.Items.Add(objItem)
                Do While rsArquivo.Read
                    objItem = New System.Web.UI.WebControls.ListItem
                    objItem.Text = rsArquivo("nomarq")
                    objItem.Value = rsArquivo("nomarq")
                    drpDownArquivo.Items.Add(objItem)

                    intI = intI + 1
                Loop
                rsArquivo.Close()
                rsArquivo = Nothing
            End If
            'Cmd.Connection.Close()
            'Cmd.Dispose()
            'Conn.Close()
            'Conn.Dispose()
            lblMsg.Visible = False
        Catch e As Exception
            lblMsg.Visible = False
            'Session("strMensagem") = e.Message
            If Conn.State = ConnectionState.Open Then
                Cmd.Dispose()
                Conn.Close()
                Conn.Dispose()
            End If
            ' Response.Redirect("frmMensagem.aspx")
        Finally
            If Conn.State = ConnectionState.Open Then
                Cmd.Dispose()
                Conn.Close()
                Conn.Dispose()
            End If
        End Try
    End Sub

    Private Sub btnConfirmar_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnConfirmar.Click
        Try
            'Server.Transfer("frmRecibo.aspx?&strNomeArquivo=" & Trim(drpDownArquivo.SelectedItem.Text()))
            Response.Write("<script language=JavaScript>")
            Response.Write("window.open('frmRecibo.aspx?strNomeArquivo=" & Trim(drpDownArquivo.SelectedItem.Text()) & "' , '', 'width=515,height=610,status=no,scrollbars=no,titlebar=yes,menubar=no')")
            Response.Write("</script>")
            lblMsg.Visible = False
        Catch
            lblMsg.Visible = True
        End Try

    End Sub
End Class
