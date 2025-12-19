Imports System.Data.SqlClient

Public Class frmAberturaDia
    Inherits BaseWebUi
    Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
    Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand

    Protected tbResponsaveis As DataTable = New DataTable("Resposaveis")
    Protected tbMarcos As DataTable = New DataTable()

    Protected coluna As DataColumn
    Protected linha As DataRow

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'objMenu = CType(Session("onsmenu"), OnsWebControls.OnsMenu)
        If Not Page.IsPostBack Then
            CarregaComboData()
            PreencheListBoxEmpresa(lstBoxEmpresas)
            'cboData.Attributes.Add("onchange", "preencheDataFinalSugestao();")

            CarregaComboEquipe()

            IniciaTable_Grid_Responsaveis()
        End If
    End Sub

    Protected Sub btnExecutar_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnExecutar.Click

        btnExecutar.Enabled = False
        btnIncluir.Enabled = False
        btnAguarde.Visible = True

        Dim _EmpresaOk As Boolean = True
        If optOperacao.SelectedItem.Value = "E" Then
            _EmpresaOk = ValidaSelecaoEmpresa()
        End If

        If Not _EmpresaOk Then
            Response.Write("<SCRIPT>alert('Selecione uma ou mais Empresas.')</SCRIPT>")
        Else

            If (txtDataFinal.Text = "" Xor txtHoraFinal.Text = "") Then
                Response.Write("<SCRIPT>alert('Data Hora Final deve ser Informada.')</SCRIPT>")
                Exit Sub
            End If

            Dim strDataHoraFinal As String = txtDataFinal.Text & " " & txtHoraFinal.Text
            If Not IsDate(strDataHoraFinal) Then
                Response.Write("<SCRIPT>alert('Data Hora Final Inválida.')</SCRIPT>")
                Exit Sub
            End If


            Dim dataHoraFinal As DateTime = Convert.ToDateTime(strDataHoraFinal)

            Dim dataHoraIniAux As DateTime = DateTime.Now

            'Dim dataHoraIniAux As DateTime
            'If optOperacao.SelectedItem.Value = "A" Or optOperacao.SelectedItem.Value = "R" Then
            '    dataHoraIniAux = Convert.ToDateTime(cboData.SelectedItem.Text & " 00:00")
            'Else
            '    dataHoraIniAux = DateTime.Now
            'End If

            If dataHoraIniAux >= dataHoraFinal Then
                Response.Write("<SCRIPT>alert('Data Hora Final (" & dataHoraFinal.ToString("dd/MM/yyyy HH:mm") & ") deve ser maior que Data Hora Inicial (" & dataHoraIniAux.ToString("dd/MM/yyyy HH:mm") & ") .')</SCRIPT>")
                Exit Sub
            End If


            tbResponsaveis = CType(Session("tbResponsaveis"), DataTable)
            If optOperacao.SelectedItem.Value <> "E" Then
                If (tbResponsaveis.Rows.Count = 0) Then
                    Response.Write("<SCRIPT>alert('Inclua ao menos um Responsável.')</SCRIPT>")
                    Exit Sub
                End If
            End If


            If cboData.SelectedIndex > 0 Then

                Dim objConn As SqlConnection = New SqlConnection
                Dim objCmd As SqlCommand = New SqlCommand
                Dim objCmdAux As SqlCommand = New SqlCommand
                Dim objTrans As SqlTransaction

                objCmd.Connection = objConn
                objCmd.CommandTimeout = 1000000

                objCmdAux.Connection = objConn
                objCmdAux.CommandTimeout = 1000000
                objConn.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
                objConn.Open()
                objTrans = objConn.BeginTransaction()
                Try
                    objCmd.Transaction = objTrans
                    objCmdAux.Transaction = objTrans

                    Dim strDataFinal As String = dataHoraFinal.ToString("yyyy-MM-dd HH:mm:ss")

                    'Abrir Dia e Reabrir dia
                    If optOperacao.SelectedItem.Value = "A" Or optOperacao.SelectedItem.Value = "R" Then

                        objCmd.CommandText = "exec pc_inicializa '" & cboData.SelectedItem.Value & "' , 48, '" & strDataFinal & "' "
                        objCmd.ExecuteNonQuery()

                        'Cadastra o Marco ref. a Abertura do Dia e Reabertura
                        If optOperacao.SelectedItem.Value = "A" Then
                            InsereMarcoPDP(K_Const_strIdMarco_AberturaDia, cboData.SelectedItem.Value, UsuarID, UsuarID, objCmd)
                        Else
                            GravaMarcosExistentes(objCmd)

                            'Insere Marcos ref. a reabertura
                            InsereMarcoPDP(K_Const_strIdMarco_ReberturaDia, cboData.SelectedItem.Value, UsuarID, UsuarID, objCmd)
                        End If

                        'Cadastra os Responsaveis
                        InsereResponsaveis(cboData.SelectedItem.Value, objCmd)

                    Else 'Reabrir por Empresa

                        For i As Integer = 0 To lstBoxEmpresas.Items.Count - 1
                            If lstBoxEmpresas.Items(i).Selected Then

                                'objCmd.CommandText = "execute procedure pc_inicializaunica_tst('" & cboData.SelectedItem.Value & "', 48," & "'" & lstBoxEmpresas.Items(i).Value & "', 'E', '" & strDataFinal & "')"
                                'objCmd.ExecuteNonQuery()
                                ReabrirDiaPorEmpresa(cboData.SelectedItem.Value, lstBoxEmpresas.Items(i).Value, strDataFinal, objCmd, objCmdAux)

                            End If
                        Next

                    End If

                    objTrans.Commit()

                    CarregaComboData()
                    cboData_SelectedIndexChanged(sender, e)

                    tbResponsaveis = New DataTable("Resposaveis")
                    IniciaTable_Grid_Responsaveis()


                    Response.Write("<SCRIPT>alert('Operação efetuada com sucesso!')</SCRIPT>")

                    btnExecutar.Enabled = True
                    btnIncluir.Enabled = True
                    btnAguarde.Visible = False
                    'Me.bt_ok.Attributes.Add("onclick", "javascript:DesabilitaCampos;")

                Catch ex As Exception

                    objTrans.Rollback()

                    btnExecutar.Enabled = True
                    btnIncluir.Enabled = True
                    btnAguarde.Visible = False

                    Session("strMensagem") = "Erro " & ex.Message
                    Response.Redirect("frmMensagem.aspx")

                Finally
                    objConn.Close()
                    objConn = Nothing
                End Try
            Else
                Response.Write("<SCRIPT>alert('Selecione a Data')</SCRIPT>")
            End If

        End If
    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        If SessaoAtiva(Page.Session) Then
            MyBase.Render(writer)
            'objMenu.RenderControl(writer)
        End If
    End Sub

    Protected Sub cboData_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboData.SelectedIndexChanged
        If cboData.SelectedIndex > 0 Then
            Dim dataHoraFinalAux As DateTime = Convert.ToDateTime(cboData.SelectedItem.Text)
            dataHoraFinalAux = dataHoraFinalAux.AddDays(-1)
            txtDataFinal.Text = dataHoraFinalAux.ToString("dd/MM/yyyy")
            txtHoraFinal.Text = "11:00"
        Else
            txtDataFinal.Text = ""
            txtHoraFinal.Text = ""
        End If

        dtgResponsaveis.CurrentPageIndex = 0
        tbResponsaveis = CType(Session("tbResponsaveis"), DataTable)
        dtgResponsaveis.DataSource = tbResponsaveis
        dtgResponsaveis.DataBind()


        If optOperacao.SelectedItem.Value = "R" Or optOperacao.SelectedItem.Value = "E" Then
            DataGridBind_Responsaveis()
        End If

        GuardaMarcosEmMemoria()
    End Sub

    Private Sub CarregaComboData()
        Dim intI As Integer
        Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
        Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand

        Cmd.Connection = Conn

        Try
            cboData.Items.Clear()
            Conn.Open("pdp")

            'Se Abrir
            If optOperacao.SelectedItem.Value = "A" Then
                Cmd.CommandText = "Select max(datpdp) as datpdp From pdp"
            Else
                Cmd.CommandText = "Select datpdp From pdp Order By datpdp Desc"
            End If

            Dim rsData As OnsClasses.OnsData.OnsDataReader = Cmd.ExecuteReader

            intI = 1
            Dim objItem As WebControls.ListItem
            objItem = New WebControls.ListItem
            objItem.Text = ""
            objItem.Value = "0"
            cboData.Items.Add(objItem)

            If optOperacao.SelectedItem.Value = "A" Then
                rsData.Read()
                Dim tempDate As DateTime = New DateTime(Integer.Parse(Mid(rsData("datpdp"), 1, 4)), Integer.Parse(Mid(rsData("datpdp"), 5, 2)), Integer.Parse(Mid(rsData("datpdp"), 7, 2)), 0, 0, 0)

                Do While intI < 60
                    tempDate = tempDate.AddDays(1)

                    objItem = New System.Web.UI.WebControls.ListItem
                    objItem.Text = tempDate.ToString("dd/MM/yyyy")
                    objItem.Value = tempDate.ToString("yyyyMMdd")
                    cboData.Items.Add(objItem)

                    intI = intI + 1
                Loop
            Else
                Do While rsData.Read
                    objItem = New System.Web.UI.WebControls.ListItem
                    objItem.Text = Mid(rsData("datpdp"), 7, 2) & "/" & Mid(rsData("datpdp"), 5, 2) & "/" & Mid(rsData("datpdp"), 1, 4)
                    objItem.Value = rsData("datpdp").ToString()
                    cboData.Items.Add(objItem)
                Loop
            End If

            rsData.Close()
            rsData = Nothing
            Cmd.Connection.Close()
            Conn.Close()
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

    Protected Sub optOperacao_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles optOperacao.SelectedIndexChanged
        If optOperacao.SelectedItem.Value = "E" Then
            lstBoxEmpresas.Enabled = True
            lblEmpresa.Enabled = True

            HabilitaCamposResponsaveis(False)
        Else
            lstBoxEmpresas.Enabled = False
            lblEmpresa.Enabled = False

            HabilitaCamposResponsaveis(True)
        End If
        CarregaComboData()
        txtDataFinal.Text = ""
        txtHoraFinal.Text = ""

        IniciaTable_Grid_Responsaveis()
    End Sub

    Function ValidaSelecaoEmpresa() As Boolean

        Dim _achou As Boolean = False
        For i As Integer = 0 To lstBoxEmpresas.Items.Count - 1
            If lstBoxEmpresas.Items(i).Selected Then
                _achou = True
                Exit For
            End If
        Next

        ValidaSelecaoEmpresa = _achou

    End Function

    Private Sub ReabrirDiaPorEmpresa(ByVal pDataPdp As String, ByVal pCodEmpresa As String, ByVal pStrDataHoraFinal As String, ByRef pOnsCommand As SqlCommand, ByRef pOnsCommandAux As SqlCommand)

        Try
            '--------------------------------------------------
            'Deleta os dados para determinada data e empresa
            '--------------------------------------------------

            ' CARGA *
            Dim strCARGA As String = "DELETE FROM carga WHERE (datpdp = " & pDataPdp & ") AND codempre = '" & pCodEmpresa & "'"
            pOnsCommand.CommandText = strCARGA
            pOnsCommand.ExecuteNonQuery()

            ' INTER *
            Dim strSqlINTER As String = "DELETE FROM inter WHERE (datpdp = " & pDataPdp & ")  AND (codemprede = '" & pCodEmpresa & "' OR codemprepara = '" & pCodEmpresa & "')"
            pOnsCommand.CommandText = strSqlINTER
            pOnsCommand.ExecuteNonQuery()

            ' DESPA *
            Dim strSqlDESPA As String = "DELETE FROM despa WHERE (datpdp = " & pDataPdp & ")  AND EXISTS (SELECT u.codusina FROM usina u WHERE (u.codempre = '" & pCodEmpresa & "') AND u.codusina = despa.codusina)"
            pOnsCommand.CommandText = strSqlDESPA
            pOnsCommand.ExecuteNonQuery()

            ' TB_RRO *
            Dim strSqlTB_RRO As String = "DELETE FROM tb_rro WHERE (datpdp = " & pDataPdp & ")  AND EXISTS (SELECT u.codusina FROM usina u WHERE (u.codempre = '" & pCodEmpresa & "') AND u.codusina = tb_rro.codusina)"
            pOnsCommand.CommandText = strSqlTB_RRO
            pOnsCommand.ExecuteNonQuery()

            ' VAZÃO *
            Dim strSqlVAZAO As String = "DELETE FROM vazao WHERE (datpdp = " & pDataPdp & ")  AND EXISTS (SELECT u.codusina FROM usina u WHERE (u.codempre = '" & pCodEmpresa & "') AND u.codusina = vazao.codusina)"
            pOnsCommand.CommandText = strSqlVAZAO
            pOnsCommand.ExecuteNonQuery()

            ' INFLEXIBILIDADE *
            Dim strSqlINFLEX As String = "DELETE FROM inflexibilidade WHERE (datpdp = " & pDataPdp & ")  AND EXISTS (SELECT u.codusina FROM usina u WHERE (u.codempre = '" & pCodEmpresa & "') AND u.codusina = inflexibilidade.codusina)"
            pOnsCommand.CommandText = strSqlINFLEX
            pOnsCommand.ExecuteNonQuery()

            ' RAZAOELET *
            Dim strSqlRAZAOELET As String = "DELETE FROM razaoelet WHERE (datpdp = " & pDataPdp & ")  AND EXISTS (SELECT u.codusina FROM usina u WHERE (u.codempre = '" & pCodEmpresa & "') AND u.codusina = razaoelet.codusina)"
            pOnsCommand.CommandText = strSqlRAZAOELET
            pOnsCommand.ExecuteNonQuery()

            ' RAZAOENER *
            Dim strSqlRAZAOENER As String = "DELETE FROM razaoener WHERE( datpdp = " & pDataPdp & ")  AND EXISTS (SELECT u.codusina FROM usina u WHERE (u.codempre = '" & pCodEmpresa & "') AND u.codusina = razaoener.codusina)"
            pOnsCommand.CommandText = strSqlRAZAOENER
            pOnsCommand.ExecuteNonQuery()

            ' EXPORTA *
            Dim strSqlEXPORTA As String = "DELETE FROM exporta WHERE (datpdp = " & pDataPdp & ")  AND EXISTS (SELECT u.codusina FROM usina u WHERE (u.codempre = '" & pCodEmpresa & "') AND u.codusina = exporta.codusina)"
            pOnsCommand.CommandText = strSqlEXPORTA
            pOnsCommand.ExecuteNonQuery()

            ' IMPORTA *
            Dim strSqlIMPORTA As String = "DELETE FROM importa WHERE (datpdp = " & pDataPdp & ")  AND EXISTS (SELECT u.codusina FROM usina u WHERE (u.codempre = '" & pCodEmpresa & "') AND u.codusina = importa.codusina)"
            pOnsCommand.CommandText = strSqlIMPORTA
            pOnsCommand.ExecuteNonQuery()

            ' PERDASCIC *
            Dim strSqlPERDASCIC As String = "DELETE FROM perdascic WHERE (datpdp = " & pDataPdp & ")  AND EXISTS (SELECT u.codusina FROM usina u WHERE (u.codempre = '" & pCodEmpresa & "') AND u.codusina = perdascic.codusina)"
            pOnsCommand.CommandText = strSqlPERDASCIC
            pOnsCommand.ExecuteNonQuery()

            ' MOTIVOREL *
            Dim strSqlMOTIVOREL As String = "DELETE FROM motivorel WHERE (datpdp = " & pDataPdp & ")  AND EXISTS (SELECT u.codusina FROM usina u WHERE (u.codempre = '" & pCodEmpresa & "') AND u.codusina = motivorel.codusina)"
            pOnsCommand.CommandText = strSqlMOTIVOREL
            pOnsCommand.ExecuteNonQuery()

            ' CONVENIENCIA_OPER *
            Dim strSqlCONVOPER As String = "DELETE FROM conveniencia_oper WHERE (datpdp = " & pDataPdp & ")  AND EXISTS (SELECT u.codusina FROM usina u WHERE (u.codempre = '" & pCodEmpresa & "') AND u.codusina = conveniencia_oper.codusina)"
            pOnsCommand.CommandText = strSqlCONVOPER
            pOnsCommand.ExecuteNonQuery()

            ' OPER_SINCRONO *
            Dim strSqlOPERSINC As String = "DELETE FROM oper_sincrono WHERE (datpdp = " & pDataPdp & ")  AND EXISTS (SELECT u.codusina FROM usina u WHERE (u.codempre = '" & pCodEmpresa & "') AND u.codusina = oper_sincrono.codusina)"
            pOnsCommand.CommandText = strSqlOPERSINC
            pOnsCommand.ExecuteNonQuery()

            ' MAQ_GERANDO *
            Dim strSqlMAQGERANDO As String = "DELETE FROM maq_gerando WHERE (datpdp = " & pDataPdp & ")  AND EXISTS (SELECT u.codusina FROM usina u WHERE (u.codempre = '" & pCodEmpresa & "') AND u.codusina = maq_gerando.codusina)"
            pOnsCommand.CommandText = strSqlMAQGERANDO
            pOnsCommand.ExecuteNonQuery()

            ' ENERGIA_REPOSICAO *
            Dim strSqlENERGIAREP As String = "DELETE FROM energia_reposicao WHERE (datpdp = " & pDataPdp & ")  AND EXISTS (SELECT u.codusina FROM usina u WHERE (u.codempre = '" & pCodEmpresa & "') AND u.codusina = energia_reposicao.codusina)"
            pOnsCommand.CommandText = strSqlENERGIAREP
            pOnsCommand.ExecuteNonQuery()

            ' DISPONIBILIDADE *
            Dim strSqlDISPONIBILIDADE As String = "DELETE FROM disponibilidade WHERE (datpdp = " & pDataPdp & ")  AND EXISTS (SELECT u.codusina FROM usina u WHERE (u.codempre = '" & pCodEmpresa & "') AND u.codusina = disponibilidade.codusina)"
            pOnsCommand.CommandText = strSqlDISPONIBILIDADE
            pOnsCommand.ExecuteNonQuery()

            ' COMPLASTRO_FISICO *
            Dim strSqlCOMPLASTRO As String = "DELETE FROM complastro_fisico WHERE (datpdp = " & pDataPdp & ")  AND EXISTS (SELECT u.codusina FROM usina u WHERE (u.codempre = '" & pCodEmpresa & "') AND u.codusina = complastro_fisico.codusina);"
            pOnsCommand.CommandText = strSqlCOMPLASTRO
            pOnsCommand.ExecuteNonQuery()

            ' DESPA_PPG *
            Dim strSqlDESPA_PPG As String = "DELETE FROM despa_ppg WHERE (datpdp = " & pDataPdp & ")  AND EXISTS (SELECT u.codusina FROM usina u WHERE (u.codempre = '" & pCodEmpresa & "') AND u.codusina = despa_ppg.codusina)"
            pOnsCommand.CommandText = strSqlDESPA_PPG
            pOnsCommand.ExecuteNonQuery()

            ' MOTIVOINFL *
            Dim strSqlMOTIVONFL As String = "DELETE FROM motivoinfl WHERE(datpdp = " & pDataPdp & ")  AND EXISTS (SELECT u.codusina FROM usina u WHERE (u.codempre = '" & pCodEmpresa & "') AND u.codusina = motivoinfl.codusina)"
            pOnsCommand.CommandText = strSqlMOTIVONFL
            pOnsCommand.ExecuteNonQuery()

            ' COTA *
            Dim strSqlCOTA As String = "DELETE FROM cota WHERE(datpdp = " & pDataPdp & ")  AND EXISTS (SELECT u.codusina FROM usina u WHERE (u.codempre = '" & pCodEmpresa & "') AND u.codusina = cota.codusina)"
            pOnsCommand.CommandText = strSqlCOTA
            pOnsCommand.ExecuteNonQuery()

            ' REST_FALTA_COMB *
            Dim strSqlRES_FALTA_COMB As String = "DELETE FROM rest_falta_comb WHERE(datpdp = " & pDataPdp & ")  AND EXISTS (SELECT u.codusina FROM usina u WHERE (u.codempre = '" & pCodEmpresa & "') AND u.codusina = rest_falta_comb.codusina)"
            pOnsCommand.CommandText = strSqlRES_FALTA_COMB
            pOnsCommand.ExecuteNonQuery()

            ' TB_RMP *
            Dim strSqlRMP As String = "DELETE FROM tb_rmp WHERE(datpdp = " & pDataPdp & ")  AND EXISTS (SELECT u.codusina FROM usina u WHERE (u.codempre = '" & pCodEmpresa & "') AND u.codusina = tb_rmp.codusina)"
            pOnsCommand.CommandText = strSqlRMP
            pOnsCommand.ExecuteNonQuery()

            ' TB_GFM *
            Dim strSqlGFM As String = "DELETE FROM tb_gfm  WHERE(datpdp = " & pDataPdp & ")  AND EXISTS (SELECT u.codusina FROM usina u WHERE (u.codempre = '" & pCodEmpresa & "') AND u.codusina = tb_gfm.codusina)"
            pOnsCommand.CommandText = strSqlGFM
            pOnsCommand.ExecuteNonQuery()

            ' TB_CFM *
            Dim strSqlCFM As String = "DELETE FROM tb_cfm WHERE(datpdp = " & pDataPdp & ")  AND EXISTS (SELECT u.codusina FROM usina u WHERE (u.codempre = '" & pCodEmpresa & "') AND u.codusina = tb_cfm.codusina)"
            pOnsCommand.CommandText = strSqlCFM
            pOnsCommand.ExecuteNonQuery()

            ' TB_SOM *
            Dim strSqlSOM As String = "DELETE FROM tb_som WHERE(datpdp = " & pDataPdp & ")  AND EXISTS (SELECT u.codusina FROM usina u WHERE (u.codempre = '" & pCodEmpresa & "') AND u.codusina = tb_som.codusina)"
            pOnsCommand.CommandText = strSqlSOM
            pOnsCommand.ExecuteNonQuery()

            ' TB_GES *
            Dim strSqlGES As String = "DELETE FROM tb_GES WHERE(datpdp = " & pDataPdp & ")  AND EXISTS (SELECT u.codusina FROM usina u WHERE (u.codempre = '" & pCodEmpresa & "') AND u.codusina = tb_GES.codusina)"
            pOnsCommand.CommandText = strSqlGES
            pOnsCommand.ExecuteNonQuery()

            ' TB_GEC *
            Dim strSqlGEC As String = "DELETE FROM tb_GEC WHERE(datpdp = " & pDataPdp & ")  AND EXISTS (SELECT u.codusina FROM usina u WHERE (u.codempre = '" & pCodEmpresa & "') AND u.codusina = tb_GEC.codusina)"
            pOnsCommand.CommandText = strSqlGEC
            pOnsCommand.ExecuteNonQuery()

            ' TB_DCA *
            Dim strSqlDCA As String = "DELETE FROM tb_DCA WHERE(datpdp = " & pDataPdp & ")  AND EXISTS (SELECT u.codusina FROM usina u WHERE (u.codempre = '" & pCodEmpresa & "') AND u.codusina = tb_DCA.codusina)"
            pOnsCommand.CommandText = strSqlDCA
            pOnsCommand.ExecuteNonQuery()

            ' TB_DCR *
            Dim strSqlDCR As String = "DELETE FROM tb_DCR WHERE(datpdp = " & pDataPdp & ")  AND EXISTS (SELECT u.codusina FROM usina u WHERE (u.codempre = '" & pCodEmpresa & "') AND u.codusina = tb_DCR.codusina)"
            pOnsCommand.CommandText = strSqlDCR
            pOnsCommand.ExecuteNonQuery()

            ' TB_IR1 *
            Dim strSqlIR1 As String = "DELETE FROM tb_IR1 WHERE(datpdp = " & pDataPdp & ")  AND EXISTS (SELECT u.codusina FROM usina u WHERE (u.codempre = '" & pCodEmpresa & "') AND u.codusina = tb_IR1.codusina)"
            pOnsCommand.CommandText = strSqlIR1
            pOnsCommand.ExecuteNonQuery()

            ' TB_IR2 *
            Dim strSqlIR2 As String = "DELETE FROM tb_IR2 WHERE(datpdp = " & pDataPdp & ")  AND EXISTS (SELECT u.codusina FROM usina u WHERE (u.codempre = '" & pCodEmpresa & "') AND u.codusina = tb_IR2.codusina)"
            pOnsCommand.CommandText = strSqlIR2
            pOnsCommand.ExecuteNonQuery()

            ' TB_IR3 *
            Dim strSqlIR3 As String = "DELETE FROM tb_IR3 WHERE(datpdp = " & pDataPdp & ")  AND EXISTS (SELECT u.codusina FROM usina u WHERE (u.codempre = '" & pCodEmpresa & "') AND u.codusina = tb_IR3.codusina)"
            pOnsCommand.CommandText = strSqlIR3
            pOnsCommand.ExecuteNonQuery()

            ' TB_IR4 *
            Dim strSqlIR4 As String = "DELETE FROM tb_IR4 WHERE(datpdp = " & pDataPdp & ")  AND EXISTS (SELECT u.codusina FROM usina u WHERE (u.codempre = '" & pCodEmpresa & "') AND u.codusina = tb_IR4.codusina)"
            pOnsCommand.CommandText = strSqlIR4
            pOnsCommand.ExecuteNonQuery()

            '--------------------------------------------------
            'Reabri o dia para determinda data e empresa
            '--------------------------------------------------

            'Recupera USINAS
            pOnsCommandAux.CommandText = "SELECT u.codusina, u.tipusina, e.flg_estudo " &
                " FROM usina u, empre e " &
                " WHERE u.codempre = e.codempre " &
                " AND e.flg_estudo <> 'S' " &
                " AND e.codempre = '" & pCodEmpresa & "' " &
                " ORDER BY u.codusina "

            Dim rsUsinas As SqlDataReader = pOnsCommandAux.ExecuteReader

            Dim index As Integer
            Dim l_CodUsina As String = ""
            Dim l_TipoUsina As String = ""
            Do While rsUsinas.Read
                l_CodUsina = rsUsinas.Item("codusina")
                l_TipoUsina = rsUsinas.Item("tipusina")

                For index = 1 To 48

                    If l_TipoUsina = "T" Then

                        strSqlINFLEX = "INSERT INTO Inflexibilidade (datpdp, codusina, intflexi) values (" & pDataPdp & ", '" & l_CodUsina & "', " & index.ToString() & ")"
                        pOnsCommand.CommandText = strSqlINFLEX
                        pOnsCommand.ExecuteNonQuery()

                        strSqlRES_FALTA_COMB = "INSERT INTO rest_falta_comb (datpdp, codusina, intrfc) values (" & pDataPdp & ", '" & l_CodUsina & "', " & index.ToString() & ")"
                        pOnsCommand.CommandText = strSqlRES_FALTA_COMB
                        pOnsCommand.ExecuteNonQuery()

                        strSqlSOM = "INSERT INTO TB_SOM (datpdp, codusina, intsom) values (" & pDataPdp & ", '" & l_CodUsina & "', " & index.ToString() & ")"
                        pOnsCommand.CommandText = strSqlSOM
                        pOnsCommand.ExecuteNonQuery()

                        strSqlGES = "INSERT INTO TB_GES (datpdp, codusina, intGES) values (" & pDataPdp & ", '" & l_CodUsina & "', " & index.ToString() & ")"
                        pOnsCommand.CommandText = strSqlGES
                        pOnsCommand.ExecuteNonQuery()

                        strSqlGEC = "INSERT INTO TB_GEC (datpdp, codusina, intGEC) values (" & pDataPdp & ", '" & l_CodUsina & "', " & index.ToString() & ")"
                        pOnsCommand.CommandText = strSqlGEC
                        pOnsCommand.ExecuteNonQuery()

                        strSqlDCA = "INSERT INTO TB_DCA (datpdp, codusina, intDCA) values (" & pDataPdp & ", '" & l_CodUsina & "', " & index.ToString() & ")"
                        pOnsCommand.CommandText = strSqlDCA
                        pOnsCommand.ExecuteNonQuery()

                        strSqlDCR = "INSERT INTO TB_DCR (datpdp, codusina, intDCR) values (" & pDataPdp & ", '" & l_CodUsina & "', " & index.ToString() & ")"
                        pOnsCommand.CommandText = strSqlDCR
                        pOnsCommand.ExecuteNonQuery()

                        strSqlIR1 = "INSERT INTO TB_IR1 (datpdp, codusina, intIR1)  values (" & pDataPdp & ", '" & l_CodUsina & "', " & index.ToString() & ")"
                        pOnsCommand.CommandText = strSqlIR1
                        pOnsCommand.ExecuteNonQuery()

                        strSqlIR2 = "INSERT INTO TB_IR2 (datpdp, codusina, intIR2) values (" & pDataPdp & ", '" & l_CodUsina & "', " & index.ToString() & ")"
                        pOnsCommand.CommandText = strSqlIR2
                        pOnsCommand.ExecuteNonQuery()

                        strSqlIR3 = "INSERT INTO TB_IR3 (datpdp, codusina, intIR3) values (" & pDataPdp & ", '" & l_CodUsina & "', " & index.ToString() & ")"
                        pOnsCommand.CommandText = strSqlIR3
                        pOnsCommand.ExecuteNonQuery()

                        strSqlIR4 = "INSERT INTO TB_IR4 (datpdp, codusina, intIR4) values (" & pDataPdp & ", '" & l_CodUsina & "', " & index.ToString() & ")"
                        pOnsCommand.CommandText = strSqlIR4
                        pOnsCommand.ExecuteNonQuery()

                        strSqlRMP = "INSERT INTO TB_RMP (datpdp, codusina, intrmp) values (" & pDataPdp & ", '" & l_CodUsina & "', " & index.ToString() & ")"
                        pOnsCommand.CommandText = strSqlRMP
                        pOnsCommand.ExecuteNonQuery()

                        strSqlCFM = "INSERT INTO TB_CFM (datpdp, codusina, intcfm) values (" & pDataPdp & ", '" & l_CodUsina & "', " & index.ToString() & ")"
                        pOnsCommand.CommandText = strSqlCFM
                        pOnsCommand.ExecuteNonQuery()

                        strSqlGFM = "INSERT INTO TB_GFM (datpdp, codusina, intgfm) values (" & pDataPdp & ", '" & l_CodUsina & "', " & index.ToString() & ")"
                        pOnsCommand.CommandText = strSqlGFM
                        pOnsCommand.ExecuteNonQuery()

                        strSqlDISPONIBILIDADE = "INSERT INTO Disponibilidade (datpdp, codusina, intdsp) values (" & pDataPdp & ", '" & l_CodUsina & "', " & index.ToString() & ")"
                        pOnsCommand.CommandText = strSqlDISPONIBILIDADE
                        pOnsCommand.ExecuteNonQuery()

                        strSqlCOMPLASTRO = "INSERT INTO complastro_fisico (datpdp, codusina, intclf) values (" & pDataPdp & ", '" & l_CodUsina & "', " & index.ToString() & ")"
                        pOnsCommand.CommandText = strSqlCOMPLASTRO
                        pOnsCommand.ExecuteNonQuery()

                        strSqlENERGIAREP = "INSERT INTO Energia_Reposicao (datpdp, codusina, intERP) values (" & pDataPdp & ", '" & l_CodUsina & "', " & index.ToString() & ")"
                        pOnsCommand.CommandText = strSqlENERGIAREP
                        pOnsCommand.ExecuteNonQuery()

                        strSqlPERDASCIC = "INSERT INTO PerdasCIC (datpdp, codusina, intpcc) values (" & pDataPdp & ", '" & l_CodUsina & "', " & index.ToString() & ")"
                        pOnsCommand.CommandText = strSqlPERDASCIC
                        pOnsCommand.ExecuteNonQuery()

                        strSqlMOTIVOREL = "INSERT INTO MotivoREL (datpdp, codusina, intmre) values (" & pDataPdp & ", '" & l_CodUsina & "', " & index.ToString() & ")"
                        pOnsCommand.CommandText = strSqlMOTIVOREL
                        pOnsCommand.ExecuteNonQuery()

                        strSqlMOTIVONFL = "INSERT INTO MotivoInfl (datpdp, codusina, intmif) values (" & pDataPdp & ", '" & l_CodUsina & "', " & index.ToString() & ")"
                        pOnsCommand.CommandText = strSqlMOTIVONFL
                        pOnsCommand.ExecuteNonQuery()

                        strSqlRAZAOELET = "INSERT INTO RazaoElet (datpdp, codusina, intrazelet) values (" & pDataPdp & ", '" & l_CodUsina & "', " & index.ToString() & ")"
                        pOnsCommand.CommandText = strSqlRAZAOELET
                        pOnsCommand.ExecuteNonQuery()

                        strSqlRAZAOENER = "INSERT INTO RazaoEner (datpdp, codusina, intrazener) values (" & pDataPdp & ", '" & l_CodUsina & "', " & index.ToString() & ")"
                        pOnsCommand.CommandText = strSqlRAZAOENER
                        pOnsCommand.ExecuteNonQuery()

                        strSqlEXPORTA = "INSERT INTO Exporta (datpdp, codusina, intexporta) values (" & pDataPdp & ", '" & l_CodUsina & "', " & index.ToString() & ")"
                        pOnsCommand.CommandText = strSqlEXPORTA
                        pOnsCommand.ExecuteNonQuery()

                        strSqlIMPORTA = "INSERT INTO Importa (datpdp, codusina, intimporta) values (" & pDataPdp & ", '" & l_CodUsina & "', " & index.ToString() & ")"
                        pOnsCommand.CommandText = strSqlIMPORTA
                        pOnsCommand.ExecuteNonQuery()
                    End If


                    If l_TipoUsina = "T" Or l_TipoUsina = "H" Then

                        strSqlDESPA = "INSERT INTO Despa (datpdp, codusina, intdespa) VALUES (" & pDataPdp & ", '" & l_CodUsina & "', " & index.ToString() & ")"
                        pOnsCommand.CommandText = strSqlDESPA
                        pOnsCommand.ExecuteNonQuery()

                        strSqlTB_RRO = "INSERT INTO tb_rro (datpdp, codusina, intrro) VALUES (" & pDataPdp & ", '" & l_CodUsina & "', " & index.ToString() & ")"
                        pOnsCommand.CommandText = strSqlTB_RRO
                        pOnsCommand.ExecuteNonQuery()

                        strSqlCONVOPER = "INSERT INTO Conveniencia_Oper (datpdp, codusina, intmco) values (" & pDataPdp & ", '" & l_CodUsina & "', " & index.ToString() & ")"
                        pOnsCommand.CommandText = strSqlCONVOPER
                        pOnsCommand.ExecuteNonQuery()

                        strSqlMAQGERANDO = "INSERT INTO Maq_Gerando (datpdp, codusina, intmeg) values (" & pDataPdp & ", '" & l_CodUsina & "', " & index.ToString() & ")"
                        pOnsCommand.CommandText = strSqlMAQGERANDO
                        pOnsCommand.ExecuteNonQuery()

                    End If


                    If l_TipoUsina = "H" Then
                        strSqlOPERSINC = "INSERT INTO Oper_Sincrono (datpdp, codusina, intmos) values (" & pDataPdp & ", '" & l_CodUsina & "', " & index.ToString() & ")"
                        pOnsCommand.CommandText = strSqlOPERSINC
                        pOnsCommand.ExecuteNonQuery()
                    End If


                Next

                If l_TipoUsina = "H" Then
                    strSqlVAZAO = "INSERT INTO Vazao (datpdp, codusina) VALUES (" & pDataPdp & ", '" & l_CodUsina & "')"
                    pOnsCommand.CommandText = strSqlVAZAO
                    pOnsCommand.ExecuteNonQuery()

                    strSqlCOTA = "INSERT INTO Cota  (datpdp, codusina) VALUES (" & pDataPdp & ", '" & l_CodUsina & "')"
                    pOnsCommand.CommandText = strSqlCOTA
                    pOnsCommand.ExecuteNonQuery()
                End If


                Dim j As Integer
                For j = 1 To 48
                    strSqlDESPA_PPG = "INSERT INTO Despa_PPG (datpdp, codusina, Ipat) values (" & pDataPdp & ", '" & l_CodUsina & "', " & j.ToString() & ")"
                    pOnsCommand.CommandText = strSqlDESPA_PPG
                    pOnsCommand.ExecuteNonQuery()
                Next

            Loop

            rsUsinas.Close()
            rsUsinas = Nothing


            'Recupera interempre
            pOnsCommandAux.CommandText = "SELECT codemprepara, codcontade, codcontapara, codcontamodal " &
                                      "FROM interempre " &
                                      "WHERE codemprede = '" & pCodEmpresa & "' "
            Dim rsInterEmpre As SqlDataReader = pOnsCommandAux.ExecuteReader

            Dim indexAux As Integer
            For indexAux = 1 To 48

                Do While rsInterEmpre.Read
                    Dim l_tipinter As String = rsInterEmpre.Item("codemprepara").ToString() & ": " &
                        rsInterEmpre.Item("codcontade").ToString() & "-" &
                        rsInterEmpre.Item("codcontapara").ToString() & "/" & rsInterEmpre.Item("codcontamodal").ToString()

                    ' INTER
                    strSqlINTER = "INSERT INTO inter (datpdp,codemprede,codemprepara,codcontade,codcontapara,codcontamodal,intinter," &
                        "valinteremp,valinterpro,valinterpre,valintersup,tipinter)" &
                        "values (" & pDataPdp & ", '" & pCodEmpresa & "', '" &
                        rsInterEmpre.Item("codemprepara").ToString() & "', '" &
                        rsInterEmpre.Item("codcontade").ToString() & "', '" &
                        rsInterEmpre.Item("codcontade").ToString() & "', '" &
                        rsInterEmpre.Item("codcontamodal").ToString() & "', " & indexAux.ToString() & ", " &
                        "0, 0, 0, 0, '" & l_tipinter & "')"
                    pOnsCommand.CommandText = strSqlINTER
                    pOnsCommand.ExecuteNonQuery()
                Loop

                ' CARGA
                strCARGA = "INSERT INTO carga (datpdp, codempre, intcarga) values (" & pDataPdp & ", '" & pCodEmpresa & "', " & indexAux.ToString() & ")"
                pOnsCommand.CommandText = strCARGA
                pOnsCommand.ExecuteNonQuery()
            Next

            rsInterEmpre.Close()
            rsInterEmpre = Nothing


            ' Deleta e insere em tb_controleagentepdp
            pOnsCommand.CommandText = "DELETE tb_controleagentepdp WHERE codempre = '" & pCodEmpresa & "' and datpdp = " & pDataPdp
            pOnsCommand.ExecuteNonQuery()

            'Dim strDataHoraInicial As String = cboData.SelectedItem.Text & " 00:00"
            'Dim dataHoraInicial As DateTime = Convert.ToDateTime(strDataHoraInicial)
            Dim dataHoraInicial As String = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            pOnsCommand.CommandText = "INSERT into tb_controleagentepdp ( codempre, datpdp, din_iniciopdp, din_fimpdp ) values ('" & pCodEmpresa & "', " & pDataPdp & ", '" &
                                    dataHoraInicial & "', " &
                                     "'" & pStrDataHoraFinal & "')"

            pOnsCommand.ExecuteNonQuery()


        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Protected Sub dtgResponsaveis_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgResponsaveis.PageIndexChanged
        dtgResponsaveis.CurrentPageIndex = e.NewPageIndex
        tbResponsaveis = CType(Session("tbResponsaveis"), DataTable)
        dtgResponsaveis.DataSource = tbResponsaveis
        dtgResponsaveis.DataBind()
    End Sub

    Private Sub CarregaComboEquipe()
        Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
        Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand

        Cmd.Connection = Conn
        Try
            cboEquipe.Items.Clear()
            Conn.Open("pdp")

            Cmd.CommandText = "SELECT id_equipepdp, UPPER(nom_equipepdp) as nom_equipepdp " &
                        "FROM tb_equipepdp " &
                        "ORDER BY id_equipepdp"

            Dim rsData As OnsClasses.OnsData.OnsDataReader = Cmd.ExecuteReader

            Dim objItem As WebControls.ListItem
            objItem = New WebControls.ListItem
            objItem.Text = ""
            objItem.Value = "0"
            cboEquipe.Items.Add(objItem)

            Do While rsData.Read
                objItem = New System.Web.UI.WebControls.ListItem
                objItem.Text = rsData("nom_equipepdp").ToString()
                objItem.Value = rsData("id_equipepdp").ToString()
                cboEquipe.Items.Add(objItem)
            Loop

            rsData.Close()
            rsData = Nothing
            Cmd.Connection.Close()
            Conn.Close()
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

    Private Sub CarregaComboUsuarioPDP()
        Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
        Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand

        Cmd.Connection = Conn

        Try
            cboUsuario.Items.Clear()
            Conn.Open("pdp")

            Cmd.CommandText = "SELECT tb_usuarequipepdp.id_usuarequipepdp, UPPER(usuar.usuar_nome) as usuar_nome " &
                        "FROM tb_equipepdp, tb_usuarequipepdp, usuar " &
                        "WHERE tb_equipepdp.id_equipepdp = tb_usuarequipepdp.id_equipepdp " &
                        "and usuar.usuar_id = tb_usuarequipepdp.usuar_id " &
                        "and tb_equipepdp.id_equipepdp = " & cboEquipe.SelectedItem.Value & " " &
                        "ORDER BY usuar.usuar_nome"

            Dim rsData As OnsClasses.OnsData.OnsDataReader = Cmd.ExecuteReader

            Dim objItem As WebControls.ListItem
            objItem = New WebControls.ListItem
            objItem.Text = ""
            objItem.Value = "0"
            cboUsuario.Items.Add(objItem)

            Do While rsData.Read
                objItem = New System.Web.UI.WebControls.ListItem
                objItem.Text = rsData("usuar_nome").ToString()
                objItem.Value = rsData("id_usuarequipepdp").ToString()
                cboUsuario.Items.Add(objItem)
            Loop

            rsData.Close()
            rsData = Nothing
            Cmd.Connection.Close()
            Conn.Close()
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

    Protected Sub cboEquipe_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboEquipe.SelectedIndexChanged
        CarregaComboUsuarioPDP()
        Page.ClientScript.RegisterStartupScript(Me.GetType(), "Foco", "document.forms[0]['cboUsuario'].focus()", True)
    End Sub

    'Protected Sub btnExcluir_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnExcluir.Click
    '    Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
    '    Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
    '    Cmd.Connection = Conn

    '    Dim lstExclui As String = ""

    '    Dim item As DataGridItem
    '    For Each item In dtgResponsaveis.Items

    '        If (CType(item.FindControl("chkMarca"), CheckBox)).Checked Then
    '            If lstExclui.Length > 0 Then
    '                lstExclui += ","
    '            End If
    '            lstExclui += (CType(item.FindControl("lblObjId"), Label)).Text
    '        End If
    '    Next

    '    '
    '    Try
    '        Conn.Open("pdp")

    '        If lstExclui = "" Then
    '            Response.Write("<SCRIPT>alert('Selecione um item.')</SCRIPT>")
    '            Exit Sub
    '        Else
    '            Cmd.CommandText = "Delete From tb_responsavelprogpdp Where id_responsavelprogpdp in (" & lstExclui & ")"
    '            Cmd.ExecuteNonQuery()
    '        End If

    '        dtgResponsaveis.CurrentPageIndex = 0

    '    Catch ex As Exception
    '        Session("strMensagem") = "Erro ao excluir. (" + ex.Message + ")"
    '        If Conn.State = ConnectionState.Open Then
    '            Conn.Close()
    '        End If
    '        Response.Redirect("frmMensagem.aspx")
    '    Finally
    '        If Conn.State = ConnectionState.Open Then
    '            Conn.Close()
    '        End If
    '    End Try

    '    tbResponsaveis = CType(Session("tbResponsaveis"), DataTable)
    '    dtgResponsaveis.DataSource = tbResponsaveis
    '    dtgResponsaveis.DataBind()
    'End Sub


    'Protected Sub btnIncluir_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnIncluir.Click

    '    If cboData.SelectedIndex = 0 Then
    '        Page.ClientScript.RegisterStartupScript(Me.GetType(), "Foco", "document.forms[0]['cboData'].focus();alert('Selecione a Data PDP')", True)
    '        Exit Sub
    '    End If

    '    If cboEquipe.SelectedIndex = 0 Then
    '        Page.ClientScript.RegisterStartupScript(Me.GetType(), "Foco", "document.forms[0]['cboEquipe'].focus();alert('Selecione a Equipe')", True)
    '        Exit Sub
    '    End If

    '    If cboUsuario.SelectedIndex = 0 Then
    '        Page.ClientScript.RegisterStartupScript(Me.GetType(), "Foco", "document.forms[0]['cboUsuario'].focus();alert('Selecione o Usuário Responsável')", True)
    '        Exit Sub
    '    End If

    '    If optTipoOperacao.SelectedIndex = -1 Then
    '        Page.ClientScript.RegisterStartupScript(Me.GetType(), "Foco", "document.forms[0]['optTipoOperacao_0'].focus();alert('Selecione o Tipo de Programação')", True)
    '        Exit Sub
    '    End If

    '    If cboEquipe.SelectedValue <> "0" And cboUsuario.SelectedValue <> "0" Then

    '        Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
    '        Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
    '        Dim objTrans As OnsClasses.OnsData.OnsTransaction

    '        Cmd.Connection = Conn
    '        Conn.Open("pdp")

    '        ' Inicializa uma transação.
    '        objTrans = Conn.BeginTransaction()
    '        Cmd.Transaction = objTrans
    '        Try
    '            ' Compor a declaração de inclusão.
    '            Cmd.CommandText = "INSERT INTO tb_responsavelprogpdp " & _
    '                              "(datpdp, id_usuarequipepdp, tip_programacao, din_inicioprogpdp) VALUES " & _
    '                              "('" & cboData.SelectedValue & "', '" & cboUsuario.SelectedValue & "', '" & _
    '                              optTipoOperacao.SelectedValue & "', '" & DateTime.Now.ToString("yyyy-MM-dd HH:mm") & "')"
    '            Cmd.ExecuteNonQuery()


    '            ' Fecha a transação.
    '            objTrans.Commit()
    '            ' Liberar recursos.
    '            'Cmd.Connection.Close()
    '            'Conn.Close()

    '            cboEquipe.SelectedIndex = 0
    '            cboUsuario.SelectedIndex = 0
    '        Catch ex As Exception
    '            'Cmd.Connection.Close()
    '            objTrans.Rollback()
    '            If Conn.State = ConnectionState.Open Then
    '                Conn.Close()
    '            End If
    '            'Response.Write("<script lang='javascript'>")
    '            'Response.Write("  window.alert('Não foi possível incluir a associação!')")
    '            'Response.Write("</script>")
    '            Session("strMensagem") = "Erro ao incluir. (" + ex.Message + ")"
    '            Response.Redirect("frmMensagem.aspx")
    '        Finally
    '            If Conn.State = ConnectionState.Open Then
    '                Conn.Close()
    '            End If
    '        End Try

    '        DataGridBind_Responsaveis()
    '        Response.Write("<SCRIPT>document.forms[0]['cboEquipe'].focus()</SCRIPT>")
    '    Else
    '        'Response.Write("<script lang='javascript'>")
    '        'Response.Write("  window.alert('Não foi possível incluir a associação!.');")
    '        'Response.Write("</script>")
    '        Session("strMensagem") = " Responsável já cadastrado"
    '        Response.Redirect("frmMensagem.aspx")
    '    End If
    '    '
    'End Sub

    Protected Sub btnIncluir_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnIncluir.Click

        If cboData.SelectedIndex = 0 Then
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "Foco", "document.forms[0]['cboData'].focus();alert('Selecione a Data PDP')", True)
            Exit Sub
        End If

        If cboEquipe.SelectedIndex = 0 Then
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "Foco", "document.forms[0]['cboEquipe'].focus();alert('Selecione a Equipe')", True)
            Exit Sub
        End If

        If cboUsuario.SelectedIndex = 0 Then
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "Foco", "document.forms[0]['cboUsuario'].focus();alert('Selecione o Usuário Responsável')", True)
            Exit Sub
        End If

        If optTipoOperacao.SelectedIndex = -1 Then
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "Foco", "document.forms[0]['optTipoOperacao_0'].focus();alert('Selecione o Tipo de Programação')", True)
            Exit Sub
        End If

        tbResponsaveis = CType(Session("tbResponsaveis"), DataTable)

        Dim dtr_Aux As DataRow() = tbResponsaveis.Select("id_usuarequipepdp = " + cboUsuario.SelectedValue + " and tip_programacao = '" + optTipoOperacao.SelectedValue + "'")
        If (dtr_Aux.Length = 0) Then

            Try
                Dim row As DataRow = tbResponsaveis.NewRow()
                row("id_responsavelprogpdp") = 111
                row("id_usuarequipepdp") = cboUsuario.SelectedValue
                row("usuar_nome") = cboUsuario.SelectedItem.Text
                row("nom_equipepdp") = cboEquipe.SelectedItem.Text
                row("tip_programacao") = optTipoOperacao.SelectedValue
                row("tip_programacaoDescricao") = IIf(optTipoOperacao.SelectedValue = "L", "Elétrica", "Energética")
                tbResponsaveis.Rows.Add(row)

                cboEquipe.SelectedIndex = 0
                cboUsuario.SelectedIndex = 0
            Catch ex As Exception
                Session("strMensagem") = "Erro ao incluir. (" + ex.Message + ")"
                Response.Redirect("frmMensagem.aspx")
            Finally
                If Conn.State = ConnectionState.Open Then
                    Conn.Close()
                End If
            End Try

            dtgResponsaveis.DataSource = tbResponsaveis
            dtgResponsaveis.DataBind()

            Session("tbResponsaveis") = tbResponsaveis

            Page.ClientScript.RegisterStartupScript(Me.GetType(), "Foco", "document.forms[0]['cboEquipe'].focus()", True)
        Else
            Session("strMensagem") = " Responsável já cadastrado"
            Response.Redirect("frmMensagem.aspx")
        End If

    End Sub


    Protected Sub InicializarDataTableResponsaveis()
        '
        coluna = New DataColumn()
        coluna.DataType = System.Type.GetType("System.Int32")
        coluna.ColumnName = "id_responsavelprogpdp"
        tbResponsaveis.Columns.Add(coluna)

        coluna = New DataColumn()
        coluna.DataType = System.Type.GetType("System.String")
        coluna.ColumnName = "id_usuarequipepdp"
        tbResponsaveis.Columns.Add(coluna)

        ' cria coluna area.    
        coluna = New DataColumn()
        coluna.DataType = System.Type.GetType("System.String")
        coluna.ColumnName = "usuar_nome"
        tbResponsaveis.Columns.Add(coluna)

        ' cria coluna area.    
        coluna = New DataColumn()
        coluna.DataType = System.Type.GetType("System.String")
        coluna.ColumnName = "nom_equipepdp"
        tbResponsaveis.Columns.Add(coluna)

        ' cria coluna area.    
        coluna = New DataColumn()
        coluna.DataType = System.Type.GetType("System.String")
        coluna.ColumnName = "tip_programacao"
        tbResponsaveis.Columns.Add(coluna)

        coluna = New DataColumn()
        coluna.DataType = System.Type.GetType("System.String")
        coluna.ColumnName = "tip_programacaoDescricao"
        tbResponsaveis.Columns.Add(coluna)
    End Sub

    Private Sub IniciaTable_Grid_Responsaveis()
        InicializarDataTableResponsaveis()
        dtgResponsaveis.DataSource = tbResponsaveis
        dtgResponsaveis.DataBind()
        Session("tbResponsaveis") = tbResponsaveis
        dtgResponsaveis.CurrentPageIndex = 0
    End Sub

    Protected Sub dtgResponsaveis_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dtgResponsaveis.ItemCommand

        If e.CommandName = "Delete" Then
            tbResponsaveis = CType(Session("tbResponsaveis"), DataTable)
            tbResponsaveis.Rows.RemoveAt(e.Item.ItemIndex)
            Session("tbResponsaveis") = tbResponsaveis
            dtgResponsaveis.DataSource = tbResponsaveis
            dtgResponsaveis.DataBind()
        End If

    End Sub


    Private Sub InsereResponsaveis(ByVal pDataPdp As String, ByRef pOnsCommand As SqlCommand)

        Try
            tbResponsaveis = CType(Session("tbResponsaveis"), DataTable)
            For Each linha As DataRow In tbResponsaveis.Rows

                pOnsCommand.CommandText = "INSERT INTO tb_responsavelprogpdp " &
                 "(datpdp, id_usuarequipepdp, tip_programacao, din_inicioprogpdp) VALUES " &
                 "('" & pDataPdp & "', '" & linha("id_usuarequipepdp") & "', '" &
                 linha("tip_programacao") & "', '" & DateTime.Now.ToString("yyyy-MM-dd HH:mm") & "')"

                pOnsCommand.ExecuteNonQuery()
            Next

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    'Private Sub DeleteResponsaveis(ByVal pDataPdp As String, ByRef pOnsCommand As OnsClasses.OnsData.OnsCommand)

    '    Try
    '        pOnsCommand.CommandText = "DELETE tb_responsavelprogpdp WHERE datpdp = '" & pDataPdp & "'"

    '        pOnsCommand.ExecuteNonQuery()
    '    Catch ex As Exception
    '        Throw New Exception(ex.Message)
    '    End Try
    'End Sub

    Private Sub DataGridBind_Responsaveis()
        Dim daResponsavel As OnsClasses.OnsData.OnsDataAdapter
        Dim dsResponsavel As DataSet
        Cmd.Connection = Conn
        Try
            Conn.Open("pdp")
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

            daResponsavel = New OnsClasses.OnsData.OnsDataAdapter(Cmd.CommandText, Conn)
            dsResponsavel = New DataSet
            daResponsavel.Fill(dsResponsavel, "Responsavel")

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
            Response.Redirect("frmMensagem.aspx")
        Finally
            If Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If
        End Try
    End Sub

    Private Sub GuardaMarcosEmMemoria()
        Dim daMarco As OnsClasses.OnsData.OnsDataAdapter
        Dim dsMarco As DataSet
        Cmd.Connection = Conn
        Try
            Conn.Open("pdp")
            Cmd.CommandText = "SELECT id_marcopdp, id_marcoprogpdp, datpdp, id_usuarsolicitante, id_usuarresponsavel, din_marcopdp " &
                              "FROM tb_marcopdp " &
                              "WHERE datpdp = '" & cboData.SelectedItem.Value & "' " &
                              "ORDER BY id_marcopdp"

            daMarco = New OnsClasses.OnsData.OnsDataAdapter(Cmd.CommandText, Conn)
            dsMarco = New DataSet
            daMarco.Fill(dsMarco, "Marcos")

            tbMarcos = New DataTable()
            tbMarcos = dsMarco.Tables("Marcos")

            Session("tbMarcos") = tbMarcos

        Catch ex As Exception
            If Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If
            Session("strMensagem") = "Erro ao consultar. (" + ex.Message + ")"
            Response.Redirect("frmMensagem.aspx")
        Finally
            If Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If
        End Try
    End Sub

    Private Sub GravaMarcosExistentes(ByRef pOnsCommand As SqlCommand)

        tbMarcos = CType(Session("tbMarcos"), DataTable)

        Dim dataHora As DateTime

        For Each linha As DataRow In tbMarcos.Rows
            dataHora = Convert.ToDateTime(linha("din_marcopdp"))
            InsereMarcoPDP(linha("id_marcoprogpdp"), linha("datpdp"), linha("id_usuarsolicitante"), linha("id_usuarresponsavel"), dataHora.ToString("yyyy-MM-dd HH:mm:ss"), pOnsCommand)
        Next

    End Sub

    Private Sub HabilitaCamposResponsaveis(ByVal p_enable As Boolean)

        cboEquipe.Enabled = p_enable
        cboUsuario.Enabled = p_enable
        optTipoOperacao.Enabled = p_enable
        btnIncluir.Visible = p_enable
        DirectCast(dtgResponsaveis.Columns(4), ButtonColumn).Visible = p_enable

    End Sub


End Class