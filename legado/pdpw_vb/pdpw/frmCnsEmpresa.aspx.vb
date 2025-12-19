Imports System.Data.SqlClient
Partial Class frmCnsEmpresa
    Inherits System.Web.UI.Page
    Dim indice_inicial As Integer = 0
    Private oCommand As SqlCommand = New SqlCommand()
    Private oConnection As SqlConnection = New SqlConnection
    Private oAdapter As SqlDataAdapter
    Private oDataSet As DataSet

#Region " Web Form Designer Generated Code "
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        InitializeComponent()
    End Sub
#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        oOnsMenu = Session("onsmenu")
        If Not Page.IsPostBack Then
            oConnection.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
            Carregar_Grid()
            dtgEmpresa.Visible = True
        End If
    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        If SessaoAtiva(Page.Session) Then
            MyBase.Render(writer)
        End If
    End Sub

    Private Function Carregar_Dados() As DataSet

        oConnection.Open()
        oCommand.Connection = oConnection
        Try

            oCommand.CommandText = "CREATE TABLE #EMPRESA (codempre varchar(200) , codarea varchar(200) , nomempre varchar(200),    
                                                 sigempre varchar(200)  , idgtpoempre varchar(200),
									             infpdp varchar(200)    , nomarea_1  varchar(200),    
                                                 sigregia  varchar(200) , codsiste varchar(200) , 
									             nomarea_2  varchar(200), nomregia varchar(200) ,    
                                                 controladora varchar(2), regiao varchar(200) null, 
								                 sistema varchar(200) null , area_nao_contr varchar(200) null, 
                                                 empresa_nao_contr  varchar(200) null) "
            oCommand.ExecuteNonQuery()

            oCommand.CommandText = "INSERT INTO #EMPRESA
                                            SELECT  empre.codempre, empre.codarea, empre.nomempre,    
                                                    empre.sigempre,  empre.idgtpoempre, empre.infpdp, 
									                area_1.nomarea as nomarea_1, area_1.sigregia,
									                reg.codsiste,  area_2.nomarea as nomarea_2, reg.nomregia,    
                                                    CASE WHEN empre.codempre = empre.codarea then 'S'    
                                                    ELSE 'N'    
                                                    END as controladora,    
								                    'XXXXXXXXXX' as regiao, 
								                    'XXXXXX'     as sistema, 
								                    'XXXXXXXXXXXX' as area_nao_contr,   
								                    'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX' as empresa_nao_contr  							  
                                            FROM empre 
								            LEFT JOIN area  as area_1 ON area_1.codarea = empre.codarea
								            LEFT JOIN area  as area_2 ON area_2.codarea = empre.infpdp
								            LEFT JOIN regia as reg    ON area_2.sigregia = reg.sigregia"

            oCommand.ExecuteNonQuery()

            oCommand.CommandText = "UPDATE #EMPRESA set regiao = nomregia, 
                                           sistema = codsiste,  
                                           area_nao_contr = null, 
                                           empresa_nao_contr = null 
                                     WHERE controladora = 'S'"
            oCommand.ExecuteNonQuery()

            oCommand.CommandText = "UPDATE #EMPRESA set
                                           area_nao_contr = nomarea_1, 
                                           empresa_nao_contr = nomarea_2, 
                                           sistema = null, regiao = null 
                                     WHERE controladora = 'N'"
            oCommand.ExecuteNonQuery()

            oCommand.CommandText = "SELECT codempre, nomempre, sigempre, 
                                           idgtpoempre, case when controladora = 'S' then 1 else 0 end as contr, 
                                           regiao, sistema, case when controladora = 'N' THEN 1 
							                                     when controladora = null then 0
							                                     else 0 END as area_contr, 
                                           case when  empresa_nao_contr = null THEN 0 ELSE 1 END as infpdp,
                                           area_nao_contr,  empresa_nao_contr 
                                     FROM #EMPRESA 
                                    ORDER BY codempre"

            oAdapter = New SqlDataAdapter(oCommand.CommandText, oConnection)
            oDataSet = New DataSet
            oAdapter.Fill(oDataSet)

            oCommand.CommandText = "DROP TABLE #EMPRESA"
            oCommand.ExecuteNonQuery()

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

    Private Function EmpresaComSelecao_Paginacao(ByVal bCargaDados As Boolean) As DataTable
        ' Carrega as linhas levando em conta o esquema de paginação associado.
        Dim oDataTable As DataTable                 ' DataTable com linhas do grid.
        Dim oRow As DataRow                         ' Linha auxiliar.
        Dim i As Integer                            ' Contador.
        oDataTable = New DataTable

        If bCargaDados = True Then
            ' Aplicar os critérios e carrega o dataset. 
            Empresa.oDataSet = Carregar_Dados()
            For i = 0 To Empresa.oDataSet.Tables(0).Columns.Count - 1
                oDataTable.Columns.Add(Empresa.oDataSet.Tables(0).Columns(i).ColumnName,
                                    System.Type.GetType("System.String"))
            Next
            ' Contar o número de linhas desta tabela.
            dtgEmpresa.VirtualItemCount = oDataSet.Tables(0).Rows.Count
        Else
            ' Adicionar as colunas correspondentes. 
            oDataTable.Columns.Add("codempre", System.Type.GetType("System.String"))
            oDataTable.Columns.Add("nomempre", System.Type.GetType("System.String"))
            oDataTable.Columns.Add("sigempre", System.Type.GetType("System.String"))
            oDataTable.Columns.Add("idgtpoempre", System.Type.GetType("System.String"))
            oDataTable.Columns.Add("contr", System.Type.GetType("System.String"))
            oDataTable.Columns.Add("regiao", System.Type.GetType("System.String"))
            oDataTable.Columns.Add("sistema", System.Type.GetType("System.String"))
            oDataTable.Columns.Add("area_contr", System.Type.GetType("System.String"))
            oDataTable.Columns.Add("infpdp", System.Type.GetType("System.String"))
            oDataTable.Columns.Add("area_nao_contr", System.Type.GetType("System.String"))
            oDataTable.Columns.Add("empresa_nao_contr", System.Type.GetType("System.String"))
        End If
        ' Copia o número de linhas que cabe em uma página para a tabela que será usada.
        For i = indice_inicial To (indice_inicial + dtgEmpresa.PageSize) - 1
            ' Verificar se não atingimos a última linha da tabela.
            If (i <= Empresa.oDataSet.Tables(0).Rows.Count - 1) Then
                oRow = oDataTable.NewRow

                oRow(0) = Empresa.oDataSet.Tables(0).Rows(i)(0)
                oRow(1) = Empresa.oDataSet.Tables(0).Rows(i)(1)
                oRow(2) = Empresa.oDataSet.Tables(0).Rows(i)(2)
                oRow(3) = Empresa.oDataSet.Tables(0).Rows(i)(3)
                oRow(4) = Empresa.oDataSet.Tables(0).Rows(i)(4)
                oRow(5) = Empresa.oDataSet.Tables(0).Rows(i)(5)
                oRow(6) = Empresa.oDataSet.Tables(0).Rows(i)(6)
                oRow(7) = Empresa.oDataSet.Tables(0).Rows(i)(7)
                oRow(8) = Empresa.oDataSet.Tables(0).Rows(i)(8)
                oRow(9) = Empresa.oDataSet.Tables(0).Rows(i)(9)
                oRow(10) = Empresa.oDataSet.Tables(0).Rows(i)(10)
                oDataTable.Rows.Add(oRow)
            End If
        Next
        Return oDataTable
    End Function


    Private Sub Carregar_Grid()
        dtgEmpresa.DataSource = EmpresaComSelecao_Paginacao(True)
        dtgEmpresa.DataBind()
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Private Sub dtgEmpresa_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgEmpresa.PageIndexChanged

        dtgEmpresa.CurrentPageIndex = e.NewPageIndex
        indice_inicial = dtgEmpresa.CurrentPageIndex * dtgEmpresa.PageSize
        dtgEmpresa.DataSource = EmpresaComSelecao_Paginacao(False)
        dtgEmpresa.DataBind()
        dtgEmpresa.Visible = True
    End Sub
End Class

Module Empresa
    Public oDataSet As New DataSet
    Public oOnsMenu As OnsWebControls.OnsMenu
End Module

