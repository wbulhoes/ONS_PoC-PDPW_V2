Partial Class frmCnsValidacao
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
        Dim intI As Integer
        'Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
        'Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
        Dim Conn As System.Data.SqlClient.SqlConnection = New System.Data.SqlClient.SqlConnection
        Conn.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
        Dim Cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand
        Dim strSql As String

        Dim daData As System.Data.SqlClient.SqlDataAdapter
        Dim dsData As System.Data.DataSet

        Dim daEmpresa As System.Data.SqlClient.SqlDataAdapter
        Dim dsEmpresa As System.Data.DataSet

        If Session("datEscolhida") = Nothing Then
            'Inicializa a variável com data do próximo dia
            Session("datEscolhida") = Now.AddDays(1)
        End If
        Cmd.Connection = Conn
        Try
            If Not Page.IsPostBack Then
                Conn.Open()

                'Preenche combo data
                cboData.Items.Clear()
                strSql = "SELECT datpdp " & _
                         "FROM pdp " & _
                         "ORDER BY datpdp DESC"
                daData = New System.Data.SqlClient.SqlDataAdapter(strSql, Conn)
                dsData = New System.Data.DataSet
                daData.Fill(dsData, "Data")

                cboData.DataTextField = "datpdp"
                cboData.DataValueField = "datpdp"
                cboData.DataSource = dsData.Tables("Data").DefaultView
                cboData.DataBind()
                cboData.Items.Insert(0, "")

                'Preenche Combo Empresa
                cboEmpresa.Items.Clear()
                strSql = "SELECT codempre, sigempre " & _
                         "FROM empre " & _
                         "ORDER BY sigempre"
                daEmpresa = New System.Data.SqlClient.SqlDataAdapter(strSql, Conn)
                dsEmpresa = New System.Data.DataSet
                daEmpresa.Fill(dsEmpresa, "Empresa")

                cboEmpresa.DataTextField = "sigempre"
                cboEmpresa.DataValueField = "codempre"
                cboEmpresa.DataSource = dsEmpresa.Tables("Empresa").DefaultView
                cboEmpresa.DataBind()
                cboEmpresa.Items.Insert(0, "")
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

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        If SessaoAtiva(Page.Session) Then
            MyBase.Render(writer)
            'objMenu.RenderControl(writer)
        End If
    End Sub

    Public Sub dtgValidacao_Paged(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs)
        dtgValidacao.CurrentPageIndex = e.NewPageIndex
        DataGridBind()
    End Sub

    Private Sub cboEmpresa_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboEmpresa.SelectedIndexChanged
        'Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
        Dim Conn As System.Data.SqlClient.SqlConnection = New System.Data.SqlClient.SqlConnection
        Conn.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
        Dim Cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand

        Dim daUsina As System.Data.SqlClient.SqlDataAdapter
        Dim dsUsina As System.Data.DataSet
        Dim strSql As String
        If cboEmpresa.SelectedIndex <> 0 Then
            cboUsina.Items.Clear()
            Conn.Open()
            Try
                strSql = "SELECT codusina " & _
                         "FROM usina " & _
                         "WHERE codempre = '" & cboEmpresa.SelectedValue & "' " & _
                         "AND tipusina = 'H' " & _
                         "ORDER BY codusina"
                daUsina = New System.Data.SqlClient.SqlDataAdapter(strSql, Conn)
                dsUsina = New System.Data.DataSet
                daUsina.Fill(dsUsina, "Usina")
                cboUsina.DataTextField = "codusina"
                cboUsina.DataValueField = "codusina"
                cboUsina.DataSource = dsUsina.Tables("Usina").DefaultView
                cboUsina.DataBind()
                cboUsina.Items.Insert(0, "")
                'Conn.Close()
            Catch ex As Exception
                Session("strMensagem") = ex.Message
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
            cboUsina.Items.Clear()
        End If
        dtgValidacao.CurrentPageIndex = 0
        DataGridBind()

    End Sub

    Private Sub DataGridBind()
        'Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
        'Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
        Dim Conn As System.Data.SqlClient.SqlConnection = New System.Data.SqlClient.SqlConnection
        Conn.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
        Dim Cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand

        Dim strSql As String
        Dim daValidacao As System.Data.SqlClient.SqlDataAdapter
        Dim dsValidacao As System.Data.DataSet
        Cmd.Connection = Conn
        Try
            Conn.Open()
            Try
                Cmd.CommandText = "DROP TABLE tmpValidHidro"
                Cmd.ExecuteNonQuery()
            Catch ex As Exception
                'Se ocorrer erro é porque a usina não existe
            End Try

            If chkTodos.Checked Then
                strSql = "SELECT e.nomempre, u.codusina, v.datvalid, CASE WHEN v.status = 'S' THEN 'SIM' ELSE 'NÃO' END AS status, v.obs " &
                         "FROM empre e, usina u, validhidraulica v " &
                         "WHERE "
                If cboEmpresa.SelectedIndex > 0 Then
                    strSql &= " e.codempre = '" & cboEmpresa.SelectedValue.Trim & "' AND "
                End If
                strSql &= "e.codempre = u.codempre AND u.codusina = v.codusina " &
                          "AND u.flg_recebepdpage = 'S' "
                If cboUsina.SelectedIndex > 0 Then
                    strSql &= "AND v.codusina = '" & cboUsina.SelectedValue.Trim & "' "
                End If
                strSql &= "AND v.datpdp = '" & cboData.SelectedValue & "' " & _
                          "ORDER BY 1, 2, 3 DESC"
            Else
                strSql = "SELECT codusina, MAX(datvalid) AS datvalid " & _
                         "FROM validhidraulica " & _
                         "WHERE datpdp = '" & cboData.SelectedValue & "' " & _
                         "GROUP BY 1 " & _
                         "INTO TEMP tmpValidHidro " & _
                         "WITH NO LOG"
                Cmd.CommandText = strSql
                Cmd.ExecuteNonQuery()

                strSql = "SELECT e.nomempre, u.codusina, v.datvalid, CASE WHEN v.status = 'S' THEN 'SIM' ELSE 'NÃO' END AS status, v.obs " &
                         "FROM empre e, usina u, validhidraulica v, tmpValidHidro t " &
                         "WHERE v.codusina = t.codusina " &
                         "AND v.datvalid = t.datvalid " &
                         "AND u.flg_recebepdpage = 'S' "
                If cboEmpresa.SelectedIndex > 0 Then
                    strSql &= "AND e.codempre = '" & cboEmpresa.SelectedValue.Trim & "' "
                End If
                strSql &= "AND e.codempre = u.codempre " & _
                          "AND u.codusina = v.codusina "
                If cboUsina.SelectedIndex > 0 Then
                    strSql &= "AND v.codusina = '" & cboUsina.SelectedValue.Trim & "' "
                End If
                strSql &= "AND v.datpdp = '" & cboData.SelectedValue & "' " & _
                          "ORDER BY 1, 2, 3 DESC"
            End If
            Cmd.CommandText = strSql
            daValidacao = New System.Data.SqlClient.SqlDataAdapter(Cmd.CommandText, Conn)
            dsValidacao = New System.Data.DataSet
            daValidacao.Fill(dsValidacao, "Validacao")

            dtgValidacao.DataSource = dsValidacao.Tables("Validacao").DefaultView
            dtgValidacao.DataBind()

            'Conn.Close()
        Catch ex As Exception
            If Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If
            Session("strMensagem") = "Não foi possível visualizar os dados"
            Response.Redirect("frmMensagem.aspx")
        Finally
            If Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If
        End Try
    End Sub

    Private Sub btnPesquisar_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnPesquisar.Click
        dtgValidacao.CurrentPageIndex = 0
        DataGridBind()
    End Sub

    Private Sub cboData_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboData.SelectedIndexChanged
        dtgValidacao.CurrentPageIndex = 0
        DataGridBind()
    End Sub

    Private Sub cboUsina_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboUsina.SelectedIndexChanged
        dtgValidacao.CurrentPageIndex = 0
        DataGridBind()
    End Sub
End Class
