Public Class frmInflxContratada

    Inherits System.Web.UI.Page
    Public objTable As New System.Data.DataTable
    Dim indicegrid As Integer = 0
    Dim indice_inicial As Integer = 0         ' Índice inicial para paginação.
    Dim Conn As System.Data.SqlClient.SqlConnection = New System.Data.SqlClient.SqlConnection
    Dim Cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand
    Dim daMot As System.Data.SqlClient.SqlDataAdapter
    Public dsMot As DataSet
    Dim objRow As DataRow



    Dim codusina, usina, datainicio, datafim, valor, habilitado, contrato As String

    Public tipo As String

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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load, Me.Load
        'Put user code to initialize the page here
        'objMenu = CType(Session("onsmenu"), OnsWebControls.OnsMenu)
        Dim intI As Integer
        Dim strSql As String
        Dim objItem As DataGridItem
        Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection

        Try
            If Not Page.IsPostBack Then
                indice_inicial = 0
                dtgMotivo.CurrentPageIndex = 0
                DataGridBind()
            Else
                Dim objCol As DataColumn
                Dim objCheckBox As CheckBox
                ' Antes de prosseguir, eliminar quaisquer linhas existentes na tabela
                ' auxiliar.
                For Each objRow In objTable.Rows
                    objRow.Delete()
                Next


                For Each objItem In dtgMotivo.Items
                    ' Procuramos o controle denominado marca. Esta estratégia funciona,
                    ' associada a um ItemTemplate. Este contém um CheckBox, que conseguimos
                    ' associar um ID.
                    objCheckBox = objItem.FindControl("chkMarca")
                    indicegrid = objItem.ItemIndex
                    ' Salva os IDs marcados para deleção.
                    If (objCheckBox.Checked) Then
                        objRow = objTable.NewRow


                        codusina = objItem.Cells(1).Text
                        usina = objItem.Cells(2).Text
                        datainicio = objItem.Cells(3).Text
                        datafim = objItem.Cells(4).Text
                        contrato = If(objItem.Cells(7).Text = "Posterior a 2011", "1", "2")
                        valor = objItem.Cells(5).Text
                        habilitado = If(objItem.Cells(6).Text = "Sim", "1", "0")
                        ' prepara os dados para serem enseridos na string e passados por querystring para o novo modal.
                        HiddenFieldSelecionado.Value = codusina & "@" & usina & "@" & datainicio & "@" & datafim & "@" & valor & "@" & habilitado & "@" & contrato




                    Else
                        Session("strMensagem") = "nenhum item selecionado"
                    End If
                Next

            End If

        Catch ex As Exception
            Session("strMensagem") = "Não foi possível visualizar os dados"
            Response.Redirect("frmMensagem.aspx")
        End Try


    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        If SessaoAtiva(Page.Session) Then
            MyBase.Render(writer)
            'objMenu.RenderControl(writer)
        End If
    End Sub

    Public Sub dtgMotivo_Paged(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs)
        dtgMotivo.CurrentPageIndex = e.NewPageIndex
        DataGridBind()
    End Sub

    Public Sub DataGridBind()

        Dim strUsuar As String = UsuarID

        Cmd.Connection = Conn

        Try
            Conn.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
            Conn.Open()
            Cmd.CommandText = "SELECT " &
                                " inf.codusina, inf.din_iniciovigencia, " &
                                " inf.din_fimvigencia, inf.flg_registroativo, " &
                                " inf.val_inflexcontratada, inf.flg_modalidadecontrato, " &
                                " u.nomusina" &
                                " FROM usina u, tb_inflexbilidadecontratada inf, usuarempre usr " &
                                " where usr.usuar_id = '" & strUsuar & "' and usr.codempre = u.codempre and inf.codusina = u.codusina" &
                                " ORDER BY inf.codusina "

            daMot = New System.Data.SqlClient.SqlDataAdapter(Cmd.CommandText, Conn)
            dsMot = New DataSet
            daMot.Fill(dsMot, "codusina")
            Dim listar = dsMot.Tables("codusina").DefaultView
            For Each item As Object In listar
                Dim diaini() = item.Item(1).ToString().Split(" ")
                item.Item(1) = diaini(0)
                Dim diafim() = item.Item(2).ToString().Split(" ")
                item.Item(2) = diafim(0)
                Dim a = item.Item(3)
                item.item(3) = If(a = "1", "Sim", "Nao")
                Dim b = item.Item(5)
                item.Item(5) = If(b = "1", "Anterior a 2011", "Posterior a 2011")

            Next

            dtgMotivo.DataSource = listar
            dtgMotivo.DataBind()

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




    Protected Sub btnIncluir_Click(ByVal sender As Object, e As EventArgs) Handles Button1.Click




    End Sub


    Protected Sub btnAlterar_Click(ByVal sender As Object, e As EventArgs)




    End Sub

    Protected Sub chkMarca_CheckedChanged(sender As Object, e As EventArgs)


    End Sub





    Protected Sub btnExcluir_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles Button3.Click
        ' Este botão executa a exclusão das linhas marcadas.
        ' Exclui as linhas marcadas e depois disso transfere para a mesma página.
        ' Com isso os dados da tabela base serão recarregados.

        Dim objTrans As System.Data.SqlClient.SqlTransaction

        Dim Conn As System.Data.SqlClient.SqlConnection = New System.Data.SqlClient.SqlConnection
        Conn.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
        Dim Cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand

        Dim objItem As DataGridItem

        Dim objRow As DataRow
        Dim objRowAux As DataRow
        Dim objRows As DataRow()
        Dim intI As Integer

        Dim objCheckBox As CheckBox
        Dim aux
        Dim resposta


        For Each objItem In dtgMotivo.Items
            objCheckBox = objItem.FindControl("chkMarca")


            ' Inicializa uma transação.
            If (objCheckBox.Checked) Then
                Response.Write("<script lang='javascript'>")
                Response.Write("  window.confirm('Deseja Excluir (s)!')")
                Response.Write("</script>")
                objRow = objTable.NewRow
                aux = True

                Cmd.Connection = Conn
                Conn.Open()
                objTrans = Conn.BeginTransaction()
                Cmd.Transaction = objTrans
                Try
                    ' Percorre a tabela, eliminando todas as linhas marcadas.
                    ' Compor a declaração de exclusão.
                    Cmd.CommandText = "DELETE FROM tb_inflexbilidadecontratada " &
                                          "WHERE codusina = '" & codusina & "' " &
                                          "AND  din_iniciovigencia = '" & FormataDatas(datainicio) & "' " &
                                          "AND din_fimvigencia = '" & FormataDatas(datafim) & "' " &
                                          "AND val_inflexcontratada = " & valor & ""
                    ' Executa a operação.
                    Cmd.ExecuteNonQuery()
                    ' Eliminar as linhas marcadas.
                    For intI = 0 To objTable.Rows.Count - 1
                        ' Excluir a linha. Sempre o item 0 porque ao eliminar a primeira linha, 
                        ' as outras serão renumeradas.
                        objTable.Rows(0).Delete()
                    Next
                    ' Fecha a transação.
                    objTrans.Commit()



                Catch ex As Exception
                    'Cmd.Connection.Close()
                    objTrans.Rollback()
                    If Conn.State = ConnectionState.Open Then
                        Conn.Close()
                    End If
                    Response.Write("<script lang='javascript'>")
                    Response.Write("  window.alert('Não foi possível excluir o(s) registro(s)!')")
                    Response.Write("</script>")
                Finally
                    If Conn.State = ConnectionState.Open Then
                        Conn.Close()

                    End If
                End Try


            End If


        Next
        'artificio para evitar que apareca somente um popup informando que nao teve nenhuma selecao
        If Not aux Then
            Response.Write("<script lang='javascript'>")
            Response.Write("  window.alert('Nenhuma registro selecionado.')")
            Response.Write("</script>")
        End If

        DataGridBind()



    End Sub

    Public Function FormataDatas(dataPDP As String) As String

        dataPDP = dataPDP.Replace("-", "").Replace("/", "").Trim()
        Dim tam As Integer = dataPDP.Length()
        Dim ano As String = dataPDP.Substring(4, 4)
        Dim mes As String = dataPDP.Substring(2, 2)
        Dim dia As String = dataPDP.Substring(0, 2)
        Dim hora As String = dataPDP.Substring(8, 9)
        If hora = "" Or Nothing Then
            hora = "00:00:00"
        End If

        Return ano + "-" + mes + "-" + dia + " " + hora
    End Function

    Public Sub dtgMotivo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles dtgMotivo.SelectedIndexChanged


    End Sub
End Class

