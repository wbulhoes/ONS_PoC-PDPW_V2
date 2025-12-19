Imports System.Collections.Generic
Imports System.Data.SqlClient

Partial Class frmCnsEnvioEmp
    Inherits BaseWebUi
    Private logger As log4net.ILog = log4net.LogManager.GetLogger(Me.GetType())
    Dim indice_inicial As Integer = 0      ' Índice inicial para paginação.
    Dim Conn As SqlConnection = New SqlConnection
    Dim Cmd As SqlCommand = New SqlCommand

    Protected tbResponsaveis As DataTable = New DataTable("Resposaveis")
    Protected coluna As DataColumn
    Protected linha As DataRow
    Protected WithEvents dtgResponsaveis As Global.System.Web.UI.WebControls.DataGrid

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_DataBinding(sender As Object, e As System.EventArgs) Handles Me.DataBinding

    End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load, Me.Load
        'Put user code to initialize the page here
        'objMenu = CType(Session("onsmenu"), OnsWebControls.OnsMenu)
        Try
            If Not Page.IsPostBack Then
                cboData.Items.Clear()

                Dim pdpData As List(Of ListItem) = CacheDataPDP.GetPdpData(False)

                Dim objitem As New WebControls.ListItem
                objitem.Text = ""
                objitem.Value = "0"
                cboData.Items.Add(objitem)

                Dim intI As Integer = 1
                For Each item As WebControls.ListItem In pdpData
                    cboData.Items.Add(item)
                    If Trim(item.Value) = Format(Session("datEscolhida"), "yyyyMMdd") Then
                        cboData.SelectedIndex = intI
                    End If
                    intI += 1
                Next

                If cboData.SelectedIndex = 0 Then
                    cboData.SelectedIndex = 1
                End If

                indice_inicial = 0
                dtgEnvio.CurrentPageIndex = 0
                DataGridBind()

                IniciaTable_Grid_Responsaveis()
                DataGridBind_Responsaveis()
            End If

        Catch ex As Exception
            Session("strMensagem") = "Não foi possível visualizar os dados" & ".  " & ex.Message & "____" & ex.StackTrace
            logger.Error(ex.Message, ex)
            'Response.Redirect("frmMensagem.aspx")
        End Try
    End Sub

    Private Sub btnPesquisar_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnPesquisar.Click
        DataGridBind()
        DataGridBind_Responsaveis()
    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        If SessaoAtiva(Page.Session) Then
            Funcoes.RetirarItensSelecionadosComDuplicidade(cboData)

            MyBase.Render(writer)
            'objMenu.RenderControl(writer)
        End If
    End Sub

    Public Sub dtgEnvio_Paged(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs)
        dtgEnvio.CurrentPageIndex = e.NewPageIndex
        DataGridBind()
    End Sub

    Private Sub DataGridBind()
        Dim strSql As String
        Dim daMot As SqlDataAdapter
        Dim dsMot As DataSet
        Dim dsDadosAux As DataSet
        Cmd.Connection = Conn
        Try
            Conn.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
            Conn.Open()

            'Cmd.CommandText = "SELECT e.codempre, e.nomempre, COUNT(m.codempre) AS qtdEnvio " & _
            '                  "FROM empre e, OUTER mensa m " & _
            '                  "WHERE e.codempre = m.codempre " & _
            '                  "AND m.datpdp = '" & cboData.SelectedValue & "' " & _
            '                  "AND m.sitmensa = 'Processado' " & _
            '                  "GROUP BY e.codempre, e.nomempre " & _
            '                  "ORDER BY 3, 2"

            Dim clausulaFrom_EmpresaxUsuario As String = ""
            Dim clausulaWhere_EmpresaxUsuario As String = ""

            If (PerfilID <> Nothing And PerfilID = "ATUPDP") Then
                clausulaWhere_EmpresaxUsuario = " WHERE e.empre_bdt_id IN (" & AgentesRepresentados & ") "

                'If AgeID <> Nothing And AgeID <> "" Then
                '    clausulaFrom_EmpresaxUsuario = ", usuarempre u "
                '    clausulaWhere_EmpresaxUsuario = " u.usuar_id = '" & UsuarID & "' And e.codempre = u.codempre And "
                'End If
            End If


            Cmd.CommandText = "SELECT e.codempre, e.nomempre, p.dthinipdp, p.dthfimpdp,  " &
                              "isNull(t.id_controleagentepdp,0) as id_controleagentepdp, t.din_iniciopdp as dthinipdpcontroleage, " &
                              "t.din_fimpdp as dthfimpdpcontroleage, COUNT(m.codempre) AS qtdEnvio, '' as flg_alterado_ons " &
                              "FROM empre e left join mensa m on m.datpdp = '" & cboData.SelectedValue & "' And e.codempre = m.codempre " &
                              " And m.sitmensa = 'Processado' left join tb_controleagentepdp t on t.datpdp = m.datpdp " &
                              " And t.codempre = e.codempre left join pdp p on p.datpdp = m.datpdp And p.datpdp = '" & cboData.SelectedValue & "'" &
                              clausulaWhere_EmpresaxUsuario &
                              "GROUP BY e.codempre, e.nomempre,p.dthinipdp, p.dthfimpdp, t.id_controleagentepdp, t.din_iniciopdp, t.din_fimpdp " &
                              "ORDER BY 3, 2"


            daMot = New SqlDataAdapter(Cmd.CommandText, Conn)
            dsMot = New DataSet

            Try

                daMot.Fill(dsMot, "Motivo")
                'Throw New Exception("Teste de problema de INFORMIX")

            Catch ex As Exception

                'Carrega um Select "FALSO" para grid não gerar um erro de tela.
                Cmd.CommandText = "SELECT 
                                        '' as codempre, 
                                        '' as nomempre, 
                                        '' as dthinipdp, 
                                        '' as dthfimpdp,  
                                        '' as id_controleagentepdp, 
                                        '' as dthinipdpcontroleage, 
                                        '' as dthfimpdpcontroleage, 
                                        '' AS qtdEnvio, 
                                        '' as flg_alterado_ons"

                daMot = New SqlDataAdapter(Cmd.CommandText, Conn)
                dsMot = New DataSet
                daMot.Fill(dsMot, "Motivo")

                lblMsg.Visible = True
            End Try


            Cmd.CommandText = "SELECT e.codempre, e.nomempre, m.sitmensa, p.dthinipdp, p.dthfimpdp, max(m.dthmensa) as dthmensa " &
                                "FROM pdp p, empre e, mensa m " &
                                "WHERE " &
                                "p.datpdp = '" & cboData.SelectedValue & "' And " &
                                "e.codempre = m.codempre And " &
                                "m.datpdp = '" & cboData.SelectedValue & "' And " &
                                "m.sitmensa = '*Processado' And " &
                                "m.datpdp = p.datpdp " &
                                "GROUP BY e.codempre, e.nomempre, m.sitmensa, p.dthinipdp, p.dthfimpdp " &
                                "ORDER BY e.codempre"

            daMot = New SqlDataAdapter(Cmd.CommandText, Conn)
            dsDadosAux = New DataSet

            Try
                daMot.Fill(dsDadosAux, "DadosAux")
                'Throw New Exception("Teste de problema de INFORMIX")

            Catch ex As Exception
                Cmd.CommandText = "SELECT 
                                        '' as codempre, 
                                        '' as nomempre, 
                                        '' as sitmensa, 
                                        '' as dthinipdp, 
                                        '' as dthfimpdp, 
                                        '' as dthmensa "

                daMot = New SqlDataAdapter(Cmd.CommandText, Conn)
                dsDadosAux = New DataSet
                daMot.Fill(dsDadosAux, "DadosAux")

                lblMsg.Visible = True
            End Try

            Dim dtDadosAux As DataTable = dsDadosAux.Tables("DadosAux")

            Dim dt As DataTable = dsMot.Tables("Motivo")
            Dim dtAux As DataTable = dsMot.Tables("Motivo").Clone()
            dtAux.Clear()

            Dim dtr_Aux As DataRow()

            For Each linha As DataRow In dt.Rows

                Dim rowAux As DataRow = dtAux.NewRow()

                rowAux("codempre") = linha("codempre")
                rowAux("nomempre") = linha("nomempre")
                rowAux("dthinipdp") = linha("dthinipdp")
                rowAux("dthfimpdp") = linha("dthfimpdp")
                rowAux("id_controleagentepdp") = linha("id_controleagentepdp")
                rowAux("dthinipdpcontroleage") = linha("dthinipdpcontroleage")
                rowAux("dthfimpdpcontroleage") = linha("dthfimpdpcontroleage")
                rowAux("qtdEnvio") = linha("qtdEnvio")

                If Not String.IsNullOrEmpty(linha("codempre")) Then

                    dtr_Aux = dtDadosAux.Select("codempre = '" + linha("codempre") + "'")
                    If (dtr_Aux.Length > 0) Then
                        rowAux("flg_alterado_ons") = "Sim"
                    Else
                        rowAux("flg_alterado_ons") = "Não"
                    End If

                End If

                dtAux.Rows.Add(rowAux)
            Next

            'dtgEnvio.DataSource = dsMot.Tables("Motivo").DefaultView
            dtgEnvio.DataSource = dtAux.DefaultView
            dtgEnvio.DataBind()

            'Conn.Close()
        Catch ex As Exception
            logger.Error(ex.Message, ex)
            Throw (ex)
        Finally
            If Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If
        End Try
    End Sub

    Private Sub DataGridBind_Responsaveis()
        Dim daResponsavel As SqlDataAdapter
        Dim dsResponsavel As DataSet
        Cmd.Connection = Conn
        Try
            Conn.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
            Conn.Open()
            Cmd.CommandText = "SELECT tb_responsavelprogpdp.id_responsavelprogpdp, tb_responsavelprogpdp.datpdp, " &
                              "tb_responsavelprogpdp.id_usuarequipepdp, tb_responsavelprogpdp.tip_programacao, " &
                              "usuar.usuar_nome, tb_equipepdp.nom_equipepdp, " &
                              "tb_responsavelprogpdp.din_inicioprogpdp, tb_responsavelprogpdp.din_fimprogpdp, " &
                              "CASE WHEN tb_responsavelprogpdp.tip_programacao = 'L' THEN" &
                                            "'Elétrica' " &
                                            "ELSE " &
                                            "'Energética' END AS tip_programacaoDescricao " &
                              "FROM tb_responsavelprogpdp, tb_usuarequipepdp, usuar, tb_equipepdp " &
                              "WHERE datpdp = '" & cboData.SelectedItem.Value & "' " &
                              "and tb_responsavelprogpdp.id_usuarequipepdp = tb_usuarequipepdp.id_usuarequipepdp " &
                              "and tb_usuarequipepdp.usuar_id = usuar.usuar_id " &
                              "and tb_usuarequipepdp.id_equipepdp = tb_equipepdp.id_equipepdp " &
                              "ORDER BY id_responsavelprogpdp"

            daResponsavel = New SqlDataAdapter(Cmd.CommandText, Conn)
            dsResponsavel = New DataSet

            Try
                daResponsavel.Fill(dsResponsavel, "Responsavel")
                'Throw New Exception("Teste de problema de INFORMIX")

            Catch ex As Exception
                Cmd.CommandText = "SELECT 
                                        '' as id_responsavelprogpdp, 
                                        '' as datpdp, 
                                        '' as id_usuarequipepdp, 
                                        '' as tip_programacao, 
                                        '' as usuar_nome, 
                                        '' as nom_equipepdp, 
                                        '' as din_inicioprogpdp, 
                                        '' as din_fimprogpdp, 
                                        '' as tip_programacaoDescricao "

                daResponsavel = New SqlDataAdapter(Cmd.CommandText, Conn)
                dsResponsavel = New DataSet
                daResponsavel.Fill(dsResponsavel, "Responsavel")

                lblMsg.Visible = True
            End Try

            tbResponsaveis = CType(Session("tbResponsaveis"), DataTable)
            tbResponsaveis = dsResponsavel.Tables("Responsavel")

            'dtgResponsaveis.DataSource = dsResponsavel.Tables("Responsavel").DefaultView

            dtgResponsaveis.DataSource = tbResponsaveis
            dtgResponsaveis.DataBind()

            Session("tbResponsaveis") = tbResponsaveis
        Catch ex As Exception
            If Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If
            Session("strMensagem") = "Erro ao consultar. (" + ex.Message + ")"
            logger.Error(ex.Message, ex)
            'Response.Redirect("frmMensagem.aspx")
        Finally
            If Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If
        End Try
    End Sub

    Protected Sub InicializarDataTableResponsaveis()
        If Not tbResponsaveis.Columns.Contains("id_responsavelprogpdp") Then
            coluna = New DataColumn()
            coluna.DataType = System.Type.GetType("System.Int32")
            coluna.ColumnName = "id_responsavelprogpdp"
            tbResponsaveis.Columns.Add(coluna)
        End If

        If Not tbResponsaveis.Columns.Contains("id_usuarequipepdp") Then
            coluna = New DataColumn()
            coluna.DataType = System.Type.GetType("System.String")
            coluna.ColumnName = "id_usuarequipepdp"
            tbResponsaveis.Columns.Add(coluna)
        End If

        If Not tbResponsaveis.Columns.Contains("usuar_nome") Then
            ' cria coluna area.    
            coluna = New DataColumn()
            coluna.DataType = System.Type.GetType("System.String")
            coluna.ColumnName = "usuar_nome"
            tbResponsaveis.Columns.Add(coluna)
        End If

        If Not tbResponsaveis.Columns.Contains("nom_equipepdp") Then
            ' cria coluna area.    
            coluna = New DataColumn()
            coluna.DataType = System.Type.GetType("System.String")
            coluna.ColumnName = "nom_equipepdp"
            tbResponsaveis.Columns.Add(coluna)
        End If

        If Not tbResponsaveis.Columns.Contains("tip_programacao") Then
            ' cria coluna area.    
            coluna = New DataColumn()
            coluna.DataType = System.Type.GetType("System.String")
            coluna.ColumnName = "tip_programacao"
            tbResponsaveis.Columns.Add(coluna)
        End If

        If Not tbResponsaveis.Columns.Contains("tip_programacaoDescricao") Then
            coluna = New DataColumn()
            coluna.DataType = System.Type.GetType("System.String")
            coluna.ColumnName = "tip_programacaoDescricao"
            tbResponsaveis.Columns.Add(coluna)
        End If
    End Sub

    Private Sub IniciaTable_Grid_Responsaveis()
        InicializarDataTableResponsaveis()
        dtgResponsaveis.DataSource = tbResponsaveis
        dtgResponsaveis.DataBind()
        Session("tbResponsaveis") = tbResponsaveis
        dtgResponsaveis.CurrentPageIndex = 0
    End Sub

    Protected Sub dtgResponsaveis_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgResponsaveis.PageIndexChanged
        dtgResponsaveis.CurrentPageIndex = e.NewPageIndex
        tbResponsaveis = CType(Session("tbResponsaveis"), DataTable)
        dtgResponsaveis.DataSource = tbResponsaveis
        dtgResponsaveis.DataBind()
    End Sub

    Private Sub btnPesquisar_Command(sender As Object, e As CommandEventArgs) Handles btnPesquisar.Command

    End Sub
End Class

