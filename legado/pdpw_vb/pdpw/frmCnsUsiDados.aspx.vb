Partial Class frmCnsUsiDados
    Inherits System.Web.UI.Page
    Private indice_inicial As Integer = 0
    Private oCommand As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
    Private oConnection As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
    Private oAdapter As OnsClasses.OnsData.OnsDataAdapter ' OleDb.OleDbDataAdapter
    Private oDataSet As DataSet
    Private strCodUsina As String
    Private strCodEmpresa As String

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
        'If Not Page.IsPostBack Then
        'Else
        strCodUsina = Request.QueryString("strCodUsina")
        strCodEmpresa = Request.QueryString("strCodEmpresa")
        indice_inicial = 0
        dtgUsiDados.CurrentPageIndex = 0
        Carregar_TextBox()
        Carregar_Grid()
        '    End If
    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        If SessaoAtiva(Page.Session) Then
            MyBase.Render(writer)
            'objMenu.RenderControl(writer)
        End If
    End Sub

    Private Function Carregar_Dados() As DataSet
        'Dim oCommand As OleDb.OleDbCommand = New OleDb.OleDbCommand()
        'Dim oConnection As OleDb.OleDbConnection = New OleDb.OleDbConnection()
        'Dim oAdapter As OleDb.OleDbDataAdapter
        'Dim oDataSet As DataSet
        Dim objRow As TableRow
        Dim objCell As TableCell
        Dim intI, intCol, intTotal As Integer

        oConnection.Servico = "PDPCnsUsiDados"
        oConnection.Usuario = UsuarID
        oConnection.Open("pdp")
        oCommand.Connection = oConnection
        Try
            oCommand.CommandText = "select codgerad, siggerad, capgerad " & _
                                    "from gerad " & _
                                    "where codusina = '" & strCodUsina & "' "
            'oCommand.ExecuteNonQuery()

            oAdapter = New OnsClasses.OnsData.OnsDataAdapter(oCommand.CommandText, oConnection)
            oDataSet = New DataSet
            oAdapter.Fill(oDataSet)
            'oConnection.Close()
        Catch ex As Exception
            Session("strMensagem") = ex.Message
            If oConnection.State = ConnectionState.Open Then
                oConnection.Close()
            End If
            Response.Redirect("frmMensagem.aspx")
        Finally
            If oConnection.State = ConnectionState.Open Then
                oConnection.Close()
            End If
        End Try

        Return oDataSet

    End Function

    Private Function UsiDadosComSelecao_Paginacao(ByVal bCargaDados As Boolean) As DataTable
        ' Carrega as linhas levando em conta o esquema de paginação associado.
        'Dim oDataSet As DataSet                     ' Data Set carregado quando o usuário
        ' informar novo critério.
        Dim oDataTable As DataTable                 ' DataTable com linhas do grid.
        Dim oRow As DataRow                         ' Linha auxiliar.
        Dim i As Integer                            ' Contador.

        ' Criar a tabela com os dados.
        oDataTable = New DataTable
        ' Deve carregar o dataset?
        If bCargaDados = True Then
            ' Aplicar os critérios e carrega o dataset. 
            CnsUsiDados.oDataSet = Carregar_Dados()
            For i = 0 To CnsUsiDados.oDataSet.Tables(0).Columns.Count - 1
                oDataTable.Columns.Add(CnsUsiDados.oDataSet.Tables(0).Columns(i).ColumnName, _
                                    System.Type.GetType("System.String"))
            Next
            ' Contar o número de linhas desta tabela.
            dtgUsiDados.VirtualItemCount = oDataSet.Tables(0).Rows.Count
        Else
            ' Adicionar as colunas correspondentes. 
            oDataTable.Columns.Add("codgerad", System.Type.GetType("System.String"))
            oDataTable.Columns.Add("siggerad", System.Type.GetType("System.String"))
            oDataTable.Columns.Add("capgerad", System.Type.GetType("System.String"))
        End If
        ' Copia o número de linhas que cabe em uma página para a tabela que será usada.
        For i = indice_inicial To (indice_inicial + dtgUsiDados.PageSize) - 1
            ' Verificar se não atingimos a última linha da tabela.
            If (i <= CnsUsiDados.oDataSet.Tables(0).Rows.Count - 1) Then
                oRow = oDataTable.NewRow

                oRow(0) = CnsUsiDados.oDataSet.Tables(0).Rows(i)(0)
                oRow(1) = CnsUsiDados.oDataSet.Tables(0).Rows(i)(1)
                oRow(2) = CnsUsiDados.oDataSet.Tables(0).Rows(i)(2)
                oDataTable.Rows.Add(oRow)
            End If
        Next
        Return oDataTable
    End Function


    Private Sub Carregar_Grid()
        dtgUsiDados.DataSource = UsiDadosComSelecao_Paginacao(True)
        dtgUsiDados.DataBind()
        ' Coloca a form em estado Idle.
    End Sub

    Private Sub Carregar_TextBox()
        Dim oConn As OnsClasses.OnsData.OnsConnection
        Dim oComm As OnsClasses.OnsData.OnsCommand
        Dim oReader As OnsClasses.OnsData.OnsDataReader

        oConn = New OnsClasses.OnsData.OnsConnection
        oConn.Open("pdp")
        Try
            oComm = New OnsClasses.OnsData.OnsCommand
            oComm.Connection = oConn
            oComm.CommandText = "select usina.sigusina, usina.nomusina, usina.tipusina, " & _
                                    "bacia.nombacia, usina.codjusante, usina.codinsta, " & _
                                    "usina.idgtpousi, usina.indcag, usina.indpeq, sum(gerad.capgerad) as capgerad " & _
                                    "from usina, outer gerad, outer bacia " & _
                                    "where usina.codusina = '" & strCodUsina & "' " & _
                                    "and usina.codbacia = bacia.codbacia " & _
                                    "and usina.codusina = gerad.codusina " & _
                                "group by usina.sigusina, usina.nomusina, usina.tipusina, " & _
                                    "bacia.nombacia, usina.codjusante, usina.codinsta, " & _
                                    "usina.idgtpousi, usina.indcag, usina.indpeq"
            oReader = oComm.ExecuteReader
            oReader.Read()
            txtCodigo.Text = strCodUsina
            If Not IsDBNull(oReader.GetValue(0)) Then
                txtSigla.Text = oReader.GetString(0)
            End If
            If Not IsDBNull(oReader.GetValue(1)) Then
                txtNome.Text = oReader.GetString(1)
            End If
            If Not IsDBNull(oReader.GetValue(2)) Then
                If Trim(oReader.GetString(2)) = "H" Then
                    chkHidraulica.Checked = True
                    chkTermica.Checked = False
                Else
                    chkHidraulica.Checked = False
                    chkTermica.Checked = True
                End If
            End If
            If Not IsDBNull(oReader.GetValue(3)) Then
                txtBacia.Text = oReader.GetString(3)
            End If
            If Not IsDBNull(oReader.GetValue(4)) Then
                txtUsiJusante.Text = oReader.GetString(4)
            End If
            If Not IsDBNull(oReader.GetValue(5)) Then
                txtIns.Text = oReader.GetString(5)
            End If
            If Not IsDBNull(oReader.GetValue(6)) Then
                txtGtpo.Text = oReader.GetValue(6)
            End If
            If Not IsDBNull(oReader.GetValue(7)) Then
                If oReader.GetValue(7) = 1 Then
                    chkCag.Checked = True
                Else
                    chkCag.Checked = False
                End If
            End If

            If Not IsDBNull(oReader.GetValue(8)) Then
                If oReader.GetValue(8) = 1 Then
                    chkPqu.Checked = True
                Else
                    chkPqu.Checked = False
                End If
            End If

            If Not IsDBNull(oReader.GetValue(9)) Then
                txtPotInstalada.Text = oReader.GetValue(9)
            End If
            oReader.Close()
            oComm.Dispose()

        Catch ex As Exception
            Session("strMensagem") = ex.Message
            If oConn.State = ConnectionState.Open Then
                oConn.Close()
                oConn.Dispose()
            End If
            Response.Redirect("frmMensagem.aspx")
        Finally
            If oConn.State = ConnectionState.Open Then
                oConn.Close()
                oConn.Dispose()
            End If
        End Try


    End Sub


    'Private Sub dtgusidados_PageIndexChanged(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgUsiDados.PageIndexChanged
    '  ' Evento que controla alteração de página no grid.
    '  'Session("strMensagem") = "TESTE"
    '  ' Atualizar a página em que o grid estará posicionado.
    '  dtgUsiDados.CurrentPageIndex = e.NewPageIndex
    '  ' Calcular a posição inicial de onde se iniciará a cópia dos elementos.
    '  indice_inicial = dtgUsiDados.CurrentPageIndex * dtgUsiDados.PageSize
    '  ' Carga dos itens.
    '  dtgUsiDados.DataSource = UsiDadosComSelecao_Paginacao(False)
    '  ' Realiza o bind do grid. 
    '  dtgUsiDados.DataBind()
    '  dtgUsiDados.Visible = True

    'End Sub

    Private Sub imgBtnVoltar_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnVoltar.Click
        Server.Transfer("frmCnsUsina.aspx?strOper=" & _
        "V" & "&strCodEmpresa=" & Session("strCodEmpresa"))
    End Sub

    Private Sub dtgUsiDados_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtgUsiDados.SelectedIndexChanged

    End Sub

    Private Sub dtgUsiDados_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgUsiDados.PageIndexChanged
        dtgUsiDados.CurrentPageIndex = e.NewPageIndex
        ' Calcular a posição inicial de onde se iniciará a cópia dos elementos.
        indice_inicial = dtgUsiDados.CurrentPageIndex * dtgUsiDados.PageSize
        ' Carga dos itens.
        dtgUsiDados.DataSource = UsiDadosComSelecao_Paginacao(False)
        ' Realiza o bind do grid. 
        dtgUsiDados.DataBind()
        dtgUsiDados.Visible = True
    End Sub
End Class
Module CnsUsiDados
    Public oDataSet As New DataSet
End Module
