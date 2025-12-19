Imports System.Data.SqlClient

Partial Class frmRecuperarDados
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
        'objMenu = CType(Session("onsmenu"), OnsWebControls.OnsMenu)
        If Not Page.IsPostBack Then
            PreencheComboData(cboDataPDP, Format(Session("datEscolhida"), "yyyyMMdd"), True)
            PreencheComboDataAnt()
            PreencheComboEmpresaPOP(AgentesRepresentados, cboEmpresa, Session("strCodEmpre"))
        End If
    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        If SessaoAtiva(Page.Session) Then
            Funcoes.RetirarItensSelecionadosComDuplicidade(cboDataPDP)
            Funcoes.RetirarItensSelecionadosComDuplicidade(cboDataAnterior)
            Funcoes.RetirarItensSelecionadosComDuplicidade(cboEmpresa)

            MyBase.Render(writer)
            'objMenu.RenderControl(writer)
        End If
    End Sub

    Private Sub cboDataPDP_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboDataPDP.SelectedIndexChanged
        PreencheComboDataAnt()
        If cboDataPDP.SelectedIndex > 0 Then
            Session("datEscolhida") = CDate(cboDataPDP.SelectedItem.Value)
            lblMsg.Text = "ATENÇÃO" & Chr(10) & "ao realizar esta função os dados existentes no dia " & cboDataPDP.SelectedItem.Text & " serão apagados."
        Else
            lblMsg.Text = ""
        End If
    End Sub

    Private Sub btnEnviar_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnEnviar.Click
        'Dim objPermissao As OnsClasses.OnsInterfaceUnica.OnsPermissao
        'objPermissao = New OnsClasses.OnsInterfaceUnica.OnsPermissao("INS", "PDPRecuperar", UsuarID)
        'Verifica se o usuário tem permissão para salvar os registros
        If EstaAutorizado Then

            Dim Conn As System.Data.SqlClient.SqlConnection = New System.Data.SqlClient.SqlConnection
            Conn.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
            Dim Cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand
            'Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
            'Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
            Dim objTrans As System.Data.SqlClient.SqlTransaction 'OnsClasses.OnsData.OnsTransaction
            Dim intRetorno As Integer
            Cmd.Connection = Conn
            Conn.Open()
            Cmd.CommandText = "SELECT datpdp " &
                            "FROM pdp " &
                            "WHERE datpdp = '" & Format(CDate(cboDataPDP.SelectedValue), "yyyyMMdd") & "' " &
                            "AND codstatu = '99'"
            Dim rsData As System.Data.SqlClient.SqlDataReader = Cmd.ExecuteReader
            'Dim rsData As OnsClasses.OnsData.OnsDataReader = Cmd.ExecuteReader
            If Not rsData.Read Then
                rsData.Close()
                '### Início da recuperação dos dados ###

                'Inicia uma transação
                objTrans = Conn.BeginTransaction
                Cmd.Transaction = objTrans

                Try

                    '#### GERAÇÃO ####
                    If chkGER.Checked = True Then
                        'Elimina tabela temporária
                        Cmd.CommandText = "DROP TABLE #tmp_despa"
                        Try
                            Cmd.ExecuteNonQuery()
                        Catch ex As Exception
                            'Se ocorrer erro ignora
                        End Try
                        '-- Coloca os valores na tabela temporária
                        Cmd.CommandText = "Select u.codusina, d.intdespa, d.valdespatran " &
                                            "INTO #tmp_despa " &
                                            "From despa d " &
                                            "Join usina u ON u.codusina = d.codusina " &
                                            "Where u.codempre = '" & cboEmpresa.SelectedValue.Trim & "' AND d.datpdp = '" & cboDataAnterior.SelectedValue & "' "

                        'Cmd.CommandText = "SELECT u.codusina, d.intdespa, d.valdespatran " &
                        '                "FROM despa d, usina u " &
                        '                "WHERE u.codempre = '" & cboEmpresa.SelectedValue.Trim & "' " &
                        '                "AND u.codusina = d.codusina " &
                        '                "AND d.datpdp = '" & cboDataAnterior.SelectedValue & "' " &
                        '                "INTO TEMP tmp_despa " &
                        '                "WITH NO LOG"
                        Cmd.ExecuteNonQuery()

                        '-- Atualiza os valores originais com a tabela temporária
                        Cmd.CommandText = "UPDATE despa " &
                                        "SET valdespatran = " &
                                        "(SELECT t.valdespatran " &
                                        "FROM #tmp_despa t " &
                                        "WHERE t.codusina = despa.codusina " &
                                        "AND t.intdespa = despa.intdespa) " &
                                        "WHERE EXISTS " &
                                        "(SELECT codusina " &
                                        "FROM usina " &
                                        "WHERE despa.codusina = usina.codusina " &
                                        "AND codempre = '" & cboEmpresa.SelectedValue.Trim & "') " &
                                        "AND datpdp = '" & Format(CDate(cboDataPDP.SelectedValue), "yyyyMMdd") & "'"
                        intRetorno = Cmd.ExecuteNonQuery()
                        If intRetorno > 0 Then
                            'Grava evento registrando o recebimento da carga
                            GravaEventoPDP("7", Format(Session("datEscolhida"), "yyyyMMdd"), cboEmpresa.SelectedItem.Text, Now, "PDPRecupera", UsuarID)
                        End If
                    End If

                    '#### CARGA ####
                    If chkCAR.Checked = True Then
                        'Elimina tabela temporária
                        Cmd.CommandText = "DROP TABLE #tmp_carga"

                        Try
                            Cmd.ExecuteNonQuery()
                        Catch ex As Exception
                            'Se ocorrer erro ignora
                        End Try
                        '-- Coloca os valores na tabela temporária

                        Cmd.CommandText = "SELECT intcarga, valcargatran " &
                                        " INTO #tmp_carga " &
                                        " FROM carga " &
                                        " WHERE codempre = '" & cboEmpresa.SelectedValue.Trim & "' " &
                                        " AND datpdp = '" & cboDataAnterior.SelectedValue & "' "

                        'Cmd.CommandText = "SELECT intcarga, valcargatran " &
                        '                "FROM carga " &
                        '                "WHERE codempre = '" & cboEmpresa.SelectedValue.Trim & "' " &
                        '                "AND datpdp = '" & cboDataAnterior.SelectedValue & "' " &
                        '                "INTO TEMP tmp_carga " &
                        '                "WITH NO LOG"
                        Cmd.ExecuteNonQuery()

                        '-- Atualiza os valores originais com a tabela temporária
                        Cmd.CommandText = "UPDATE carga " &
                                        "SET valcargatran = " &
                                        "(SELECT t.valcargatran " &
                                        "FROM #tmp_carga t " &
                                        "WHERE t.intcarga = carga.intcarga) " &
                                        "WHERE codempre = '" & cboEmpresa.SelectedValue.Trim & "' " &
                                        "AND datpdp = '" & Format(CDate(cboDataPDP.SelectedValue), "yyyyMMdd") & "'"
                        intRetorno = Cmd.ExecuteNonQuery()
                        If intRetorno > 0 Then
                            'Grava evento registrando o recebimento da carga
                            GravaEventoPDP("8", Format(Session("datEscolhida"), "yyyyMMdd"), cboEmpresa.SelectedItem.Text, Now.AddSeconds(1), "PDPRecupera", UsuarID)
                        End If
                    End If


                    '#### INTERCÂMBIO ####
                    If chkINT.Checked = True Then
                        'Elimina tabela temporária
                        Cmd.CommandText = "DROP TABLE #tmp_inter"
                        Try
                            Cmd.ExecuteNonQuery()
                        Catch ex As Exception
                            'Se ocorrer erro ignora
                        End Try
                        '-- Coloca os valores na tabela temporária
                        Cmd.CommandText = "SELECT codemprepara, codcontade, codcontapara, codcontamodal, intinter, valintertran " &
                                        "INTO #tmp_inter " &
                                        "FROM inter " &
                                        "WHERE codemprede = '" & cboEmpresa.SelectedValue.Trim & "' " &
                                        "AND datpdp = '" & cboDataAnterior.SelectedValue & "' "

                        Cmd.ExecuteNonQuery()

                        '-- Atualiza os valores originais com a tabela temporária
                        Cmd.CommandText = "UPDATE inter " &
                                        "SET valintertran = " &
                                        "(SELECT t.valintertran " &
                                        "FROM #tmp_inter t " &
                                        "WHERE t.codemprepara = inter.codemprepara " &
                                        "AND t.codcontade = inter.codcontade " &
                                        "AND t.codcontapara = inter.codcontapara " &
                                        "AND t.codcontamodal = inter.codcontamodal " &
                                        "AND t.intinter = inter.intinter) " &
                                        "WHERE codemprede = '" & cboEmpresa.SelectedValue.Trim & "' " &
                                        "AND datpdp = '" & Format(CDate(cboDataPDP.SelectedValue), "yyyyMMdd") & "'"
                        intRetorno = Cmd.ExecuteNonQuery()
                        If intRetorno > 0 Then
                            'Grava evento registrando o recebimento da carga
                            GravaEventoPDP("9", Format(Session("datEscolhida"), "yyyyMMdd"), cboEmpresa.SelectedItem.Text, Now.AddSeconds(2), "PDPRecupera", UsuarID)
                        End If
                    End If

                    '#### VAZÃO ####
                    If chkVAZ.Checked = True Then
                        'Elimina tabela temporária
                        Cmd.CommandText = "DROP TABLE #tmp_vazao"
                        Try
                            Cmd.ExecuteNonQuery()
                        Catch ex As Exception
                            'Se ocorrer erro ignora
                        End Try
                        '-- Coloca os valores na tabela temporária
                        '-- CRQ2345 (15/08/2012)
                        Cmd.CommandText = "SELECT v.codusina, v.valturbtran, v.valverttran, v.valaflutran, c.cotainitran, c.cotafimtran, c.outrasestruturastran, c.comentariopdftran " &
                                        "INTO #tmp_vazao " &
                                        "FROM  vazao v  " &
                                        "Join Usina u ON u.codusina = v.codusina " &
                                        "Left Join  " &
                                        "cota c ON u.codusina = c.codusina And c.datpdp = '" & cboDataAnterior.SelectedValue & "' " &
                                        "WHERE u.codempre = 'EE' AND v.datpdp = '" & cboDataAnterior.SelectedValue & "'"
                        Cmd.ExecuteNonQuery()

                        '-- Atualiza os valores Turbinados da tabela vazao com a tabela temporária
                        Cmd.CommandText = "UPDATE vazao " &
                                        "SET valturbtran = " &
                                        "(SELECT t.valturbtran " &
                                        "FROM #tmp_vazao t " &
                                        "WHERE t.codusina = vazao.codusina) " &
                                        "WHERE EXISTS (SELECT u.codusina FROM usina u WHERE vazao.codusina = u.codusina AND u.codempre = '" & cboEmpresa.SelectedValue.Trim & "') " &
                                        "AND datpdp = '" & Format(CDate(cboDataPDP.SelectedValue), "yyyyMMdd") & "'"
                        Cmd.ExecuteNonQuery()

                        '-- Atualiza os valores Vertidos da tabela vazao com a tabela temporária
                        Cmd.CommandText = "UPDATE vazao " &
                                        "SET valverttran = " &
                                        "(SELECT t.valverttran " &
                                        "FROM #tmp_vazao t " &
                                        "WHERE t.codusina = vazao.codusina) " &
                                        "WHERE EXISTS (SELECT u.codusina FROM usina u WHERE vazao.codusina = u.codusina AND u.codempre = '" & cboEmpresa.SelectedValue.Trim & "') " &
                                        "AND datpdp = '" & Format(CDate(cboDataPDP.SelectedValue), "yyyyMMdd") & "'"
                        intRetorno = Cmd.ExecuteNonQuery()

                        '-- Atualiza os valores Afluentes da tabela vazao com a tabela temporária
                        Cmd.CommandText = "UPDATE vazao " &
                                        "SET valaflutran = " &
                                        "(SELECT t.valaflutran " &
                                        "FROM #tmp_vazao t " &
                                        "WHERE t.codusina = vazao.codusina) " &
                                        "WHERE EXISTS (SELECT u.codusina FROM usina u WHERE vazao.codusina = u.codusina AND u.codempre = '" & cboEmpresa.SelectedValue.Trim & "') " &
                                        "AND datpdp = '" & Format(CDate(cboDataPDP.SelectedValue), "yyyyMMdd") & "'"
                        Cmd.ExecuteNonQuery()

                        '-- Atualiza os valores Cota Inicial da tabela cota com a tabela temporária
                        Cmd.CommandText = "UPDATE cota " &
                                        "SET cotainitran = " &
                                        "(SELECT t.cotainitran " &
                                        "FROM #tmp_vazao t " &
                                        "WHERE t.codusina = cota.codusina) " &
                                        "WHERE EXISTS (SELECT u.codusina FROM usina u WHERE cota.codusina = u.codusina AND u.codempre = '" & cboEmpresa.SelectedValue.Trim & "') " &
                                        "AND datpdp = '" & Format(CDate(cboDataPDP.SelectedValue), "yyyyMMdd") & "'"
                        Cmd.ExecuteNonQuery()

                        '-- CRQ2345 (15/08/2012)
                        '-- Atualiza os valores Cota Final da tabela cota com a tabela temporária
                        Cmd.CommandText = "UPDATE cota " &
                                                          "SET cotafimtran = " &
                                                          "(SELECT t.cotafimtran " &
                                                          "FROM #tmp_vazao t " &
                                                          "WHERE t.codusina = cota.codusina) " &
                                                          "WHERE EXISTS (SELECT u.codusina FROM usina u WHERE cota.codusina = u.codusina AND u.codempre = '" & cboEmpresa.SelectedValue.Trim & "') " &
                                                          "AND datpdp = '" & Format(CDate(cboDataPDP.SelectedValue), "yyyyMMdd") & "'"
                        Cmd.ExecuteNonQuery()

                        '-- Atualiza os valores Outras Estruturas da tabela cota com a tabela temporária
                        Cmd.CommandText = "UPDATE cota " &
                                                          "SET outrasestruturastran = " &
                                                          "(SELECT t.outrasestruturastran " &
                                                          "FROM #tmp_vazao t " &
                                                          "WHERE t.codusina = cota.codusina) " &
                                                          "WHERE EXISTS (SELECT u.codusina FROM usina u WHERE cota.codusina = u.codusina AND u.codempre = '" & cboEmpresa.SelectedValue.Trim & "') " &
                                                          "AND datpdp = '" & Format(CDate(cboDataPDP.SelectedValue), "yyyyMMdd") & "'"
                        Cmd.ExecuteNonQuery()

                        '-- Atualiza os valores Comentario PDF da tabela cota com a tabela temporária
                        Cmd.CommandText = "UPDATE cota " &
                                                          "SET comentariopdftran = " &
                                                          "(SELECT t.comentariopdftran " &
                                                          "FROM #tmp_vazao t " &
                                                          "WHERE t.codusina = cota.codusina) " &
                                                          "WHERE EXISTS (SELECT u.codusina FROM usina u WHERE cota.codusina = u.codusina AND u.codempre = '" & cboEmpresa.SelectedValue.Trim & "') " &
                                                          "AND datpdp = '" & Format(CDate(cboDataPDP.SelectedValue), "yyyyMMdd") & "'"
                        Cmd.ExecuteNonQuery()

                        If intRetorno > 0 Then
                            'Grava evento registrando o recebimento de vazão e cota
                            GravaEventoPDP("6", Format(Session("datEscolhida"), "yyyyMMdd"), cboEmpresa.SelectedItem.Text, Now.AddSeconds(3), "PDPRecupera", UsuarID)
                        End If
                    End If

                    '#### INFLEXIBILIDADE ####
                    If chkINF.Checked = True Then
                        'Elimina tabela temporária
                        Cmd.CommandText = "DROP TABLE #tmp_inflex"
                        Try
                            Cmd.ExecuteNonQuery()
                        Catch ex As Exception
                            'Se ocorrer erro ignora
                        End Try
                        '-- Coloca os valores na tabela temporária
                        Cmd.CommandText = "SELECT i.codusina, i.intflexi, i.valflexitran " &
                                        "INTO #tmp_inflex " &
                                        "FROM inflexibilidade i, usina u " &
                                        "WHERE u.codempre = '" & cboEmpresa.SelectedValue.Trim & "' " &
                                        "AND u.codusina = i.codusina " &
                                        "AND i.datpdp = '" & cboDataAnterior.SelectedValue & "' "
                        Cmd.ExecuteNonQuery()

                        '-- Atualiza os valores originais com a tabela temporária
                        Cmd.CommandText = "UPDATE inflexibilidade " &
                                        "SET valflexitran = " &
                                        "(SELECT t.valflexitran " &
                                        "FROM #tmp_inflex t " &
                                        "WHERE t.intflexi = inflexibilidade.intflexi " &
                                        "AND t.codusina = inflexibilidade.codusina) " &
                                        "WHERE EXISTS (SELECT u.codusina FROM usina u WHERE inflexibilidade.codusina = u.codusina AND u.codempre = '" & cboEmpresa.SelectedValue.Trim & "') " &
                                        "AND datpdp = '" & Format(CDate(cboDataPDP.SelectedValue), "yyyyMMdd") & "'"
                        intRetorno = Cmd.ExecuteNonQuery()
                        If intRetorno > 0 Then
                            'Grava evento registrando o recebimento da carga
                            GravaEventoPDP("2", Format(Session("datEscolhida"), "yyyyMMdd"), cboEmpresa.SelectedItem.Text, Now.AddSeconds(4), "PDPRecupera", UsuarID)
                        End If
                    End If


                    '#### RAZÃO ENERGÉTICA ####
                    If chkENE.Checked = True Then
                        'Elimina tabela temporária
                        Cmd.CommandText = "DROP TABLE #tmp_razaoener"
                        Try
                            Cmd.ExecuteNonQuery()
                        Catch ex As Exception
                            'Se ocorrer erro ignora
                        End Try
                        '-- Coloca os valores na tabela temporária
                        Cmd.CommandText = "SELECT r.codusina, r.intrazener, r.valrazenertran " &
                                        "INTO #tmp_razaoener " &
                                        "FROM razaoener r, usina u " &
                                        "WHERE u.codempre = '" & cboEmpresa.SelectedValue.Trim & "' " &
                                        "AND u.codusina = r.codusina " &
                                        "AND r.datpdp = '" & cboDataAnterior.SelectedValue & "' "
                        Cmd.ExecuteNonQuery()

                        '-- Atualiza os valores da tabela despa com a tabela temporária
                        Cmd.CommandText = "UPDATE razaoener " &
                                        "SET valrazenertran = " &
                                        "(SELECT t.valrazenertran " &
                                        "FROM #tmp_razaoener t " &
                                        "WHERE t.intrazener = razaoener.intrazener " &
                                        "AND t.codusina = razaoener.codusina) " &
                                        "WHERE EXISTS (SELECT u.codusina FROM usina u WHERE razaoener.codusina = u.codusina AND u.codempre = '" & cboEmpresa.SelectedValue.Trim & "') " &
                                        "AND datpdp = '" & Format(CDate(cboDataPDP.SelectedValue), "yyyyMMdd") & "'"
                        intRetorno = Cmd.ExecuteNonQuery()
                        If intRetorno > 0 Then
                            'Grava evento registrando o recebimento da carga
                            GravaEventoPDP("17", Format(Session("datEscolhida"), "yyyyMMdd"), cboEmpresa.SelectedItem.Text, Now.AddSeconds(5), "PDPRecupera", UsuarID)
                        End If
                    End If

                    '#### RAZÃO ELÉTRICA ####
                    If chkELE.Checked = True Then
                        'Elimina tabela temporária
                        Cmd.CommandText = "DROP TABLE #tmp_razaoelet"
                        Try
                            Cmd.ExecuteNonQuery()
                        Catch ex As Exception
                            'Se ocorrer erro ignora
                        End Try
                        '-- Coloca os valores na tabela temporária
                        Cmd.CommandText = "SELECT r.codusina, r.intrazelet, r.valrazelettran " &
                                        "INTO #tmp_razaoelet " &
                                        "FROM razaoelet r, usina u " &
                                        "WHERE u.codempre = '" & cboEmpresa.SelectedValue.Trim & "' " &
                                        "AND u.codusina = r.codusina " &
                                        "AND r.datpdp = '" & cboDataAnterior.SelectedValue & "' "
                        Cmd.ExecuteNonQuery()

                        '-- Atualiza os valores da tabela despa com a tabela temporária
                        Cmd.CommandText = "UPDATE razaoelet " &
                                        "SET valrazelettran = " &
                                        "(SELECT t.valrazelettran " &
                                        "FROM #tmp_razaoelet t " &
                                        "WHERE t.intrazelet = razaoelet.intrazelet " &
                                        "AND t.codusina = razaoelet.codusina) " &
                                        "WHERE EXISTS (SELECT u.codusina FROM usina u WHERE razaoelet.codusina = u.codusina AND u.codempre = '" & cboEmpresa.SelectedValue & "') " &
                                        "AND datpdp = '" & Format(CDate(cboDataPDP.SelectedValue), "yyyyMMdd") & "'"
                        intRetorno = Cmd.ExecuteNonQuery()
                        If intRetorno > 0 Then
                            'Grava evento registrando o recebimento da carga
                            GravaEventoPDP("18", Format(Session("datEscolhida"), "yyyyMMdd"), cboEmpresa.SelectedItem.Text, Now.AddSeconds(6), "PDPRecupera", UsuarID)
                        End If
                    End If

                    '#### EXPORTAÇÃO ####
                    If chkEXP.Checked = True Then
                        'Elimina tabela temporária
                        Cmd.CommandText = "DROP TABLE #tmp_exporta"
                        Try
                            Cmd.ExecuteNonQuery()
                        Catch ex As Exception
                            'Se ocorrer erro ignora
                        End Try
                        '-- Coloca os valores na tabela temporária
                        Cmd.CommandText = "SELECT e.codusina, e.intexporta, e.valexportatran " &
                                        "INTO #tmp_exporta " &
                                        "FROM exporta e, usina u " &
                                        "WHERE u.codempre = '" & cboEmpresa.SelectedValue & "' " &
                                        "AND u.codusina = e.codusina " &
                                        "AND e.datpdp = '" & cboDataAnterior.SelectedValue & "' "
                        Cmd.ExecuteNonQuery()

                        '-- Atualiza os valores da tabela despa com a tabela temporária
                        Cmd.CommandText = "UPDATE exporta " &
                                        "SET valexportatran = " &
                                        "(SELECT t.valexportatran " &
                                        "FROM #tmp_exporta t " &
                                        "WHERE t.intexporta = exporta.intexporta " &
                                        "AND t.codusina = exporta.codusina) " &
                                        "WHERE EXISTS (SELECT u.codusina FROM usina u WHERE exporta.codusina = u.codusina AND u.codempre = '" & cboEmpresa.SelectedValue.Trim & "') " &
                                        "AND datpdp = '" & Format(CDate(cboDataPDP.SelectedValue), "yyyyMMdd") & "'"
                        intRetorno = Cmd.ExecuteNonQuery()
                        If intRetorno > 0 Then
                            'Grava evento registrando o recebimento da carga
                            GravaEventoPDP("33", Format(Session("datEscolhida"), "yyyyMMdd"), cboEmpresa.SelectedItem.Text, Now.AddSeconds(7), "PDPRecupera", UsuarID)
                        End If
                    End If

                    '#### IMPORTAÇÃO ####
                    If chkIMP.Checked = True Then
                        'Elimina tabela temporária
                        Cmd.CommandText = "DROP TABLE #tmp_importa"
                        Try
                            Cmd.ExecuteNonQuery()
                        Catch ex As Exception
                            'Se ocorrer erro ignora
                        End Try
                        '-- Coloca os valores na tabela temporária
                        Cmd.CommandText = "SELECT i.codusina, i.intimporta, i.valimportatran " &
                                        "INTO #tmp_importa " &
                                        "FROM importa i, usina u " &
                                        "WHERE u.codempre = '" & cboEmpresa.SelectedValue.Trim & "' " &
                                        "AND u.codusina = i.codusina " &
                                        "AND i.datpdp = '" & cboDataAnterior.SelectedValue & "' "
                        Cmd.ExecuteNonQuery()

                        '-- Atualiza os valores da tabela despa com a tabela temporária
                        Cmd.CommandText = "UPDATE importa " &
                                        "SET valimportatran = " &
                                        "(SELECT t.valimportatran " &
                                        "FROM #tmp_importa t " &
                                        "WHERE t.intimporta = importa.intimporta " &
                                        "AND t.codusina = importa.codusina) " &
                                        "WHERE EXISTS (SELECT u.codusina FROM usina u WHERE importa.codusina = u.codusina AND u.codempre = '" & cboEmpresa.SelectedValue.Trim & "') " &
                                        "AND datpdp = '" & Format(CDate(cboDataPDP.SelectedValue), "yyyyMMdd") & "'"
                        intRetorno = Cmd.ExecuteNonQuery()
                        If intRetorno > 0 Then
                            'Grava evento registrando o recebimento da carga
                            GravaEventoPDP("32", Format(Session("datEscolhida"), "yyyyMMdd"), cboEmpresa.SelectedItem.Text, Now.AddSeconds(8), "PDPRecupera", UsuarID)
                        End If
                    End If

                    '#### MOTIVO DE DESPACHO RAZÃO ELÉTRICA ####
                    If chkMRE.Checked = True Then
                        'Elimina tabela temporária
                        Cmd.CommandText = "DROP TABLE #tmp_motivorel"
                        Try
                            Cmd.ExecuteNonQuery()
                        Catch ex As Exception
                            'Se ocorrer erro ignora
                        End Try
                        '-- Coloca os valores na tabela temporária
                        Cmd.CommandText = "SELECT m.codusina, m.intmre, m.valmretran " &
                                        "INTO #tmp_motivorel " &
                                        "FROM motivorel m, usina u " &
                                        "WHERE u.codempre = '" & cboEmpresa.SelectedValue.Trim & "' " &
                                        "AND u.codusina = m.codusina " &
                                        "AND m.datpdp = '" & cboDataAnterior.SelectedValue & "' "
                        Cmd.ExecuteNonQuery()

                        '-- Atualiza os valores da tabela despa com a tabela temporária
                        Cmd.CommandText = "UPDATE motivorel " &
                                        "SET valmretran = " &
                                        "(SELECT t.valmretran " &
                                        "FROM #tmp_motivorel t " &
                                        "WHERE t.intmre = motivorel.intmre " &
                                        "AND t.codusina = motivorel.codusina) " &
                                        "WHERE EXISTS (SELECT u.codusina FROM usina u WHERE motivorel.codusina = u.codusina AND u.codempre = '" & cboEmpresa.SelectedValue.Trim & "') " &
                                        "AND datpdp = '" & Format(CDate(cboDataPDP.SelectedValue), "yyyyMMdd") & "'"
                        intRetorno = Cmd.ExecuteNonQuery()
                        If intRetorno > 0 Then
                            'Grava evento registrando o recebimento da carga
                            GravaEventoPDP("34", Format(Session("datEscolhida"), "yyyyMMdd"), cboEmpresa.SelectedItem.Text, Now.AddSeconds(9), "PDPRecupera", UsuarID)
                        End If
                    End If

                    '#### MOTIVO DE DESPACHO INFLEXIBILIDADE ####
                    If chkMIF.Checked = True Then
                        'Elimina tabela temporária
                        Cmd.CommandText = "DROP TABLE #tmp_motivoinfl"
                        Try
                            Cmd.ExecuteNonQuery()
                        Catch ex As Exception
                            'Se ocorrer erro ignora
                        End Try
                        '-- Coloca os valores na tabela temporária
                        Cmd.CommandText = "SELECT m.codusina, m.intmif, m.valmiftran " &
                                           "INTO #tmp_motivoinfl " &
                                          "FROM motivoinfl m, usina u " &
                                          "WHERE u.codempre = '" & cboEmpresa.SelectedValue.Trim & "' " &
                                          "AND u.codusina = m.codusina " &
                                          "AND m.datpdp = '" & cboDataAnterior.SelectedValue & "' "
                        Cmd.ExecuteNonQuery()

                        '-- Atualiza os valores da tabela despa com a tabela temporária
                        Cmd.CommandText = "UPDATE motivoinfl " &
                                          "SET valmiftran = " &
                                          "(SELECT t.valmiftran " &
                                          "FROM #tmp_motivoinfl t " &
                                          "WHERE t.intmif = motivoinfl.intmif " &
                                          "AND t.codusina = motivoinfl.codusina) " &
                                          "WHERE EXISTS (SELECT u.codusina FROM usina u WHERE motivoinfl.codusina = u.codusina AND u.codempre = '" & cboEmpresa.SelectedValue.Trim & "') " &
                                          "AND datpdp = '" & Format(CDate(cboDataPDP.SelectedValue), "yyyyMMdd") & "'"
                        intRetorno = Cmd.ExecuteNonQuery()
                        If intRetorno > 0 Then
                            'Grava evento registrando o recebimento da carga
                            GravaEventoPDP("48", Format(Session("datEscolhida"), "yyyyMMdd"), cboEmpresa.SelectedItem.Text, Now.AddSeconds(10), "PDPRecupera", UsuarID)
                        End If
                    End If

                    '#### PERDAS DO CONSUMO INTERNO E COMPENSAÇÃO ####
                    If chkPCC.Checked = True Then
                        'Elimina tabela temporária
                        Cmd.CommandText = "DROP TABLE #tmp_perdascic"
                        Try
                            Cmd.ExecuteNonQuery()
                        Catch ex As Exception
                            'Se ocorrer erro ignora
                        End Try
                        '-- Coloca os valores na tabela temporária
                        Cmd.CommandText = "SELECT p.codusina, p.intpcc, p.valpcctran " &
                                        "INTO #tmp_perdascic " &
                                        "FROM perdascic p, usina u " &
                                        "WHERE u.codempre = '" & cboEmpresa.SelectedValue.Trim & "' " &
                                        "AND u.codusina = p.codusina " &
                                        "AND p.datpdp = '" & cboDataAnterior.SelectedValue & "' "
                        Cmd.ExecuteNonQuery()

                        '-- Atualiza os valores da tabela despa com a tabela temporária
                        Cmd.CommandText = "UPDATE perdascic " &
                                        "SET valpcctran = " &
                                        "(SELECT t.valpcctran " &
                                        "FROM #tmp_perdascic t " &
                                        "WHERE t.intpcc = perdascic.intpcc " &
                                        "AND t.codusina = perdascic.codusina) " &
                                        "WHERE EXISTS (SELECT u.codusina FROM usina u WHERE perdascic.codusina = u.codusina AND u.codempre = '" & cboEmpresa.SelectedValue.Trim & "') " &
                                        "AND datpdp = '" & Format(CDate(cboDataPDP.SelectedValue), "yyyyMMdd") & "'"
                        intRetorno = Cmd.ExecuteNonQuery()
                        If intRetorno > 0 Then
                            'Grava evento registrando o recebimento da carga
                            GravaEventoPDP("35", Format(Session("datEscolhida"), "yyyyMMdd"), cboEmpresa.SelectedItem.Text, Now.AddSeconds(11), "PDPRecupera", UsuarID)
                        End If
                    End If


                    '#### NÚMERO DE MÁQUINAS PARADAS POR CONVENIÊNCIA OPERATIVA ####
                    If chkMCO.Checked = True Then
                        'Elimina tabela temporária
                        Cmd.CommandText = "DROP TABLE #tmp_conv_oper"
                        Try
                            Cmd.ExecuteNonQuery()
                        Catch ex As Exception
                            'Se ocorrer erro ignora
                        End Try
                        '-- Coloca os valores na tabela temporária
                        Cmd.CommandText = "SELECT c.codusina, c.intmco, c.valmcotran " &
                                        "INTO #tmp_conv_oper " &
                                        "FROM conveniencia_oper c, usina u " &
                                        "WHERE u.codempre = '" & cboEmpresa.SelectedValue.Trim & "' " &
                                        "AND u.codusina = c.codusina " &
                                        "AND c.datpdp = '" & cboDataAnterior.SelectedValue & "' "
                        Cmd.ExecuteNonQuery()

                        '-- Atualiza os valores da tabela despa com a tabela temporária
                        Cmd.CommandText = "UPDATE conveniencia_oper " &
                                        "SET valmcotran = " &
                                        "(SELECT t.valmcotran " &
                                        "FROM #tmp_conv_oper t " &
                                        "WHERE t.intmco = conveniencia_oper.intmco " &
                                        "AND t.codusina = conveniencia_oper.codusina) " &
                                        "WHERE EXISTS (SELECT u.codusina FROM usina u WHERE conveniencia_oper.codusina = u.codusina AND u.codempre = '" & cboEmpresa.SelectedValue.Trim & "') " &
                                        "AND datpdp = '" & Format(CDate(cboDataPDP.SelectedValue), "yyyyMMdd") & "'"
                        intRetorno = Cmd.ExecuteNonQuery()
                        If intRetorno > 0 Then
                            'Grava evento registrando o recebimento da carga
                            GravaEventoPDP("36", Format(Session("datEscolhida"), "yyyyMMdd"), cboEmpresa.SelectedItem.Text, Now.AddSeconds(12), "PDPRecupera", UsuarID)
                        End If
                    End If


                    '#### NÚMERO DE MÁQUINAS OPERANDO COMO SÍNCRONO ####
                    If chkMOS.Checked = True Then
                        'Elimina tabela temporária
                        Cmd.CommandText = "DROP TABLE #tmp_oper_sinc"
                        Try
                            Cmd.ExecuteNonQuery()
                        Catch ex As Exception
                            'Se ocorrer erro ignora
                        End Try
                        '-- Coloca os valores na tabela temporária
                        Cmd.CommandText = "SELECT c.codusina, c.intmos, c.valmostran " &
                                        "INTO #tmp_oper_sinc " &
                                        "FROM oper_sincrono c, usina u " &
                                        "WHERE u.codempre = '" & cboEmpresa.SelectedValue.Trim & "' " &
                                        "AND u.codusina = c.codusina " &
                                        "AND c.datpdp = '" & cboDataAnterior.SelectedValue & "' "
                        Cmd.ExecuteNonQuery()

                        '-- Atualiza os valores da tabela despa com a tabela temporária
                        Cmd.CommandText = "UPDATE oper_sincrono " &
                                        "SET valmostran = " &
                                        "(SELECT t.valmostran " &
                                        "FROM #tmp_oper_sinc t " &
                                        "WHERE t.intmos = oper_sincrono.intmos " &
                                        "AND t.codusina = oper_sincrono.codusina) " &
                                        "WHERE EXISTS (SELECT u.codusina FROM usina u WHERE oper_sincrono.codusina = u.codusina AND u.codempre = '" & cboEmpresa.SelectedValue.Trim.Trim & "') " &
                                        "AND datpdp = '" & Format(CDate(cboDataPDP.SelectedValue), "yyyyMMdd") & "'"
                        intRetorno = Cmd.ExecuteNonQuery()
                        If intRetorno > 0 Then
                            'Grava evento registrando o recebimento da carga
                            GravaEventoPDP("37", Format(Session("datEscolhida"), "yyyyMMdd"), cboEmpresa.SelectedItem.Text, Now.AddSeconds(13), "PDPRecupera", UsuarID)
                        End If
                    End If


                    '#### NÚMERO DE MÁQUINAS GERANDO ####
                    If chkMEG.Checked = True Then
                        'Elimina tabela temporária
                        Cmd.CommandText = "DROP TABLE #tmp_maq_gerando"
                        Try
                            Cmd.ExecuteNonQuery()
                        Catch ex As Exception
                            'Se ocorrer erro ignora
                        End Try
                        '-- Coloca os valores na tabela temporária
                        Cmd.CommandText = "SELECT m.codusina, m.intmeg, m.valmegtran " &
                                        "INTO #tmp_maq_gerando " &
                                        "FROM maq_gerando m, usina u " &
                                        "WHERE u.codempre = '" & cboEmpresa.SelectedValue.Trim & "' " &
                                        "AND u.codusina = m.codusina " &
                                        "AND m.datpdp = '" & cboDataAnterior.SelectedValue & "' "
                        Cmd.ExecuteNonQuery()

                        '-- Atualiza os valores da tabela despa com a tabela temporária
                        Cmd.CommandText = "UPDATE maq_gerando " &
                                        "SET valmegtran = " &
                                        "(SELECT t.valmegtran " &
                                        "FROM #tmp_maq_gerando t " &
                                        "WHERE t.intmeg = maq_gerando.intmeg " &
                                        "AND t.codusina = maq_gerando.codusina) " &
                                        "WHERE EXISTS (SELECT u.codusina FROM usina u WHERE maq_gerando.codusina = u.codusina AND u.codempre = '" & cboEmpresa.SelectedValue.Trim & "') " &
                                        "AND datpdp = '" & Format(CDate(cboDataPDP.SelectedValue), "yyyyMMdd") & "'"
                        intRetorno = Cmd.ExecuteNonQuery()
                        If intRetorno > 0 Then
                            'Grava evento registrando o recebimento da carga
                            GravaEventoPDP("38", Format(Session("datEscolhida"), "yyyyMMdd"), cboEmpresa.SelectedItem.Text, Now.AddSeconds(14), "PDPRecupera", UsuarID)
                        End If
                    End If


                    '#### ENERGIA DE REPOSIÇÃO E PERDAS ####
                    If chkERP.Checked = True Then
                        'Elimina tabela temporária
                        Cmd.CommandText = "DROP TABLE #tmp_ener_rep"
                        Try
                            Cmd.ExecuteNonQuery()
                        Catch ex As Exception
                            'Se ocorrer erro ignora
                        End Try
                        '-- Coloca os valores na tabela temporária
                        Cmd.CommandText = "SELECT e.codusina, e.interp, e.valerptran " &
                                        "INTO #tmp_ener_rep " &
                                        "FROM energia_reposicao e, usina u " &
                                        "WHERE u.codempre = '" & cboEmpresa.SelectedValue.Trim & "' " &
                                        "AND u.codusina = e.codusina " &
                                        "AND e.datpdp = '" & cboDataAnterior.SelectedValue & "' "
                        Cmd.ExecuteNonQuery()

                        '-- Atualiza os valores da tabela despa com a tabela temporária
                        Cmd.CommandText = "UPDATE energia_reposicao " &
                                        "SET valerptran = " &
                                        "(SELECT t.valerptran " &
                                        "FROM #tmp_ener_rep t " &
                                        "WHERE t.interp = energia_reposicao.interp " &
                                        "AND t.codusina = energia_reposicao.codusina) " &
                                        "WHERE EXISTS (SELECT u.codusina FROM usina u WHERE energia_reposicao.codusina = u.codusina AND u.codempre = '" & cboEmpresa.SelectedValue.Trim & "') " &
                                        "AND datpdp = '" & Format(CDate(cboDataPDP.SelectedValue), "yyyyMMdd") & "'"
                        intRetorno = Cmd.ExecuteNonQuery()
                        If intRetorno > 0 Then
                            'Grava evento registrando o recebimento da carga
                            GravaEventoPDP("39", Format(Session("datEscolhida"), "yyyyMMdd"), cboEmpresa.SelectedItem.Text, Now.AddSeconds(15), "PDPRecupera", UsuarID)
                        End If
                    End If


                    '#### DISPONIBILIDADE ####
                    If chkDSP.Checked = True Then
                        'Elimina tabela temporária
                        Cmd.CommandText = "DROP TABLE #tmp_disponib"
                        Try
                            Cmd.ExecuteNonQuery()
                        Catch ex As Exception
                            'Se ocorrer erro ignora
                        End Try
                        '-- Coloca os valores na tabela temporária
                        Cmd.CommandText = "SELECT d.codusina, d.intdsp, d.valdsptran " &
                                        "INTO #tmp_disponib " &
                                        "FROM disponibilidade d, usina u " &
                                        "WHERE u.codempre = '" & cboEmpresa.SelectedValue.Trim & "' " &
                                        "AND u.codusina = d.codusina " &
                                        "AND d.datpdp = '" & cboDataAnterior.SelectedValue & "' "
                        Cmd.ExecuteNonQuery()

                        '-- Atualiza os valores da tabela despa com a tabela temporária
                        Cmd.CommandText = "UPDATE disponibilidade " &
                                        "SET valdsptran = " &
                                        "(SELECT t.valdsptran " &
                                        "FROM #tmp_disponib t " &
                                        "WHERE t.intdsp = disponibilidade.intdsp " &
                                        "AND t.codusina = disponibilidade.codusina) " &
                                        "WHERE EXISTS (SELECT u.codusina FROM usina u WHERE disponibilidade.codusina = u.codusina AND u.codempre = '" & cboEmpresa.SelectedValue.Trim & "') " &
                                        "AND datpdp = '" & Format(CDate(cboDataPDP.SelectedValue), "yyyyMMdd") & "'"
                        intRetorno = Cmd.ExecuteNonQuery()
                        If intRetorno > 0 Then
                            'Grava evento registrando o recebimento da carga
                            GravaEventoPDP("46", Format(Session("datEscolhida"), "yyyyMMdd"), cboEmpresa.SelectedItem.Text, Now.AddSeconds(16), "PDPRecupera", UsuarID)
                        End If
                    End If

                    '#### COMPENSAÇÃO DE LASTRO FÍSICO ####
                    If chkCLF.Checked = True Then
                        'Elimina tabela temporária
                        Cmd.CommandText = "DROP TABLE #tmp_last_fisico"
                        Try
                            Cmd.ExecuteNonQuery()
                        Catch ex As Exception
                            'Se ocorrer erro ignora
                        End Try
                        '-- Coloca os valores na tabela temporária
                        Cmd.CommandText = "SELECT c.codusina, c.intclf, c.valclftran " &
                                        "INTO #tmp_last_fisico " &
                                        "FROM complastro_fisico c, usina u " &
                                        "WHERE u.codempre = '" & cboEmpresa.SelectedValue.Trim & "' " &
                                        "AND u.codusina = c.codusina " &
                                        "AND c.datpdp = '" & cboDataAnterior.SelectedValue & "' "
                        Cmd.ExecuteNonQuery()

                        '-- Atualiza os valores da tabela despa com a tabela temporária
                        Cmd.CommandText = "UPDATE complastro_fisico " &
                                        "SET valclftran = " &
                                        "(SELECT t.valclftran " &
                                        "FROM #tmp_last_fisico t " &
                                        "WHERE t.intclf = complastro_fisico.intclf " &
                                        "AND t.codusina = complastro_fisico.codusina) " &
                                        "WHERE EXISTS (SELECT u.codusina FROM usina u WHERE complastro_fisico.codusina = u.codusina AND u.codempre = '" & cboEmpresa.SelectedValue.Trim & "') " &
                                        "AND datpdp = '" & Format(CDate(cboDataPDP.SelectedValue), "yyyyMMdd") & "'"
                        intRetorno = Cmd.ExecuteNonQuery()
                        If intRetorno > 0 Then
                            'Grava evento registrando o recebimento da carga
                            GravaEventoPDP("47", Format(Session("datEscolhida"), "yyyyMMdd"), cboEmpresa.SelectedItem.Text, Now.AddSeconds(17), "PDPRecupera", UsuarID)
                        End If
                    End If

                    '#### RESTRIÇÃO POR FALTA DE COMBUSTÍVEL ####
                    If chkRFC.Checked = True Then
                        'Elimina tabela temporária
                        Cmd.CommandText = "DROP TABLE #tmp_resfaltacomb"
                        Try
                            Cmd.ExecuteNonQuery()
                        Catch ex As Exception
                            'Se ocorrer erro ignora
                        End Try
                        '-- Coloca os valores na tabela temporária
                        Cmd.CommandText = "SELECT c.codusina, c.intrfc, c.valrfctran " &
                                        "INTO #tmp_resfaltacomb " &
                                        "FROM rest_falta_comb c, usina u " &
                                        "WHERE u.codempre = '" & cboEmpresa.SelectedValue.Trim & "' " &
                                        "AND u.codusina = c.codusina " &
                                        "AND c.datpdp = '" & cboDataAnterior.SelectedValue & "' "
                        Cmd.ExecuteNonQuery()

                        '-- Atualiza os valores da tabela despa com a tabela temporária
                        Cmd.CommandText = "UPDATE rest_falta_comb " &
                                        "SET valrfctran = " &
                                        "(SELECT t.valrfctran " &
                                        "FROM #tmp_resfaltacomb t " &
                                        "WHERE t.intrfc = rest_falta_comb.intrfc " &
                                        "AND t.codusina = rest_falta_comb.codusina) " &
                                        "WHERE EXISTS (SELECT u.codusina FROM usina u WHERE rest_falta_comb.codusina = u.codusina AND u.codempre = '" & cboEmpresa.SelectedValue.Trim & "') " &
                                        "AND datpdp = '" & Format(CDate(cboDataPDP.SelectedValue), "yyyyMMdd") & "'"
                        intRetorno = Cmd.ExecuteNonQuery()
                        If intRetorno > 0 Then
                            'Grava evento registrando o recebimento do insumo
                            GravaEventoPDP("51", Format(Session("datEscolhida"), "yyyyMMdd"), cboEmpresa.SelectedItem.Text, Now.AddSeconds(18), "PDPRecupera", UsuarID)
                        End If
                    End If

                    If chkRES.Checked = True Then
                        '#### RESTRIÇÃO DE UNIDADE GERADORA ####
                        Cmd.CommandText = "DELETE FROM restrgerademp " &
                                          "WHERE codgerad IN " &
                                          "(SELECT g.codgerad " &
                                          "FROM usina u, gerad g " &
                                          "WHERE u.codempre = '" & cboEmpresa.SelectedValue.Trim & "' " &
                                          "AND u.codusina = g.codusina) " &
                                          "AND datpdp = '" & Format(CDate(cboDataPDP.SelectedValue), "yyyyMMdd") & "'"
                        Cmd.ExecuteNonQuery()

                        Cmd.CommandText = "INSERT INTO restrgerademp " &
                                        "(codgerad, codrestr, datinirestr, datfimrestr, intinirestr, intfimrestr, valrestr, obsrestr, refoutrosis, datpdp) " &
                                        "SELECT r.codgerad, 0 AS codrestr, r.datinirestr, r.datfimrestr, r.intinirestr, r.intfimrestr, r.valrestr, r.obsrestr, r.refoutrosis, " & Format(CDate(cboDataPDP.SelectedValue), "yyyyMMdd") & " AS datpdp " &
                                        "FROM restrgerademp r, gerad g, usina u " &
                                        "WHERE u.codempre = '" & cboEmpresa.SelectedValue.Trim & "' " &
                                        "AND u.codusina = g.codusina " &
                                        "AND g.codgerad = r.codgerad " &
                                        "AND r.datpdp = '" & cboDataAnterior.SelectedValue & "' " &
                                        "AND r.datfimrestr >= '" & Format(CDate(cboDataPDP.SelectedValue), "yyyyMMdd") & "'"
                        intRetorno = Cmd.ExecuteNonQuery()
                        If intRetorno > 0 Then
                            'Grava evento registrando o recebimento da carga
                            GravaEventoPDP("4", Format(Session("datEscolhida"), "yyyyMMdd"), cboEmpresa.SelectedItem.Text, Now.AddSeconds(19), "PDPRecupera", UsuarID)
                        End If

                        '#### RESTRIÇÃO DE USINA ####
                        Cmd.CommandText = "DELETE FROM restrusinaemp " &
                                          "WHERE codusina IN " &
                                          "(SELECT codusina " &
                                          "FROM usina " &
                                          "WHERE codempre = '" & cboEmpresa.SelectedValue.Trim & "') " &
                                          "AND datpdp = '" & Format(CDate(cboDataPDP.SelectedValue), "yyyyMMdd") & "'"
                        Cmd.ExecuteNonQuery()

                        Cmd.CommandText = "INSERT INTO restrusinaemp " &
                                        "(codusina, datinirestr, datfimrestr, intinirestr, intfimrestr, valrestr, refoutrosis, obsrestr, datpdp) " &
                                        "SELECT r.codusina, datinirestr, datfimrestr, intinirestr, intfimrestr, valrestr, refoutrosis, obsrestr, " & Format(CDate(cboDataPDP.SelectedValue), "yyyyMMdd") & " AS datpdp " &
                                        "FROM restrusinaemp r, usina u " &
                                        "WHERE u.codempre = '" & cboEmpresa.SelectedValue.Trim & "' " &
                                        "AND u.codusina = r.codusina " &
                                        "AND r.datpdp = '" & cboDataAnterior.SelectedValue & "' " &
                                        "AND r.datfimrestr >= '" & Format(CDate(cboDataPDP.SelectedValue), "yyyyMMdd") & "'"
                        intRetorno = Cmd.ExecuteNonQuery()
                        If intRetorno > 0 Then
                            'Grava evento registrando o recebimento da carga
                            GravaEventoPDP("4", Format(Session("datEscolhida"), "yyyyMMdd"), cboEmpresa.SelectedItem.Text, Now.AddSeconds(20), "PDPRecupera", UsuarID)
                        End If
                    End If

                    If chkMAN.Checked = True Then
                        '#### MANUTENÇÃO DE UNIDADE GERADORA ####
                        Cmd.CommandText = "DELETE FROM paralemp " &
                                          "WHERE codequip IN " &
                                          "(SELECT g.codgerad " &
                                          "FROM usina u, gerad g " &
                                          "WHERE u.codempre = '" & cboEmpresa.SelectedValue.Trim & "' " &
                                          "AND u.codusina = g.codusina) " &
                                          "AND datpdp = '" & Format(CDate(cboDataPDP.SelectedValue), "yyyyMMdd") & "'"
                        Cmd.ExecuteNonQuery()

                        Cmd.CommandText = "INSERT INTO paralemp " &
                                        "(codequip, tipequip, datiniparal, datfimparal, intiniparal, intfimparal, codnivel, refoutrosis, indcont, intinivoltaparal, intfimvoltaparal, obsparal, datpdp) " &
                                        "SELECT codequip, tipequip, datiniparal, datfimparal, intiniparal, intfimparal, codnivel, refoutrosis, indcont, intinivoltaparal, intfimvoltaparal, obsparal, " & Format(CDate(cboDataPDP.SelectedValue), "yyyyMMdd") & " AS datpdp " &
                                        "FROM paralemp p, gerad g, usina u " &
                                        "WHERE u.codempre = '" & cboEmpresa.SelectedValue.Trim & "' " &
                                        "AND u.codusina = g.codusina " &
                                        "AND g.codgerad = p.codequip " &
                                        "AND p.datpdp = '" & cboDataAnterior.SelectedValue & "' " &
                                        "AND datfimparal >= '" & Format(CDate(cboDataPDP.SelectedValue), "yyyyMMdd") & "'"
                        intRetorno = Cmd.ExecuteNonQuery()
                        If intRetorno > 0 Then
                            'Grava evento registrando o recebimento da carga
                            GravaEventoPDP("5", Format(Session("datEscolhida"), "yyyyMMdd"), cboEmpresa.SelectedItem.Text, Now.AddSeconds(21), "PDPRecupera", UsuarID)
                        End If
                    End If

                    If chkPCO.Checked = True Then
                        '#### PARADA DE MÁQUINAS POR CONVENIÊNCIA OPERATIVA ####
                        Cmd.CommandText = "DELETE FROM paralemp_co " &
                                          "WHERE codequip IN " &
                                          "(SELECT g.codgerad " &
                                          "FROM usina u, gerad g " &
                                          "WHERE u.codempre = '" & cboEmpresa.SelectedValue.Trim & "' " &
                                          "AND u.codusina = g.codusina) " &
                                          "AND datpdp = '" & Format(CDate(cboDataPDP.SelectedValue), "yyyyMMdd") & "'"
                        Cmd.ExecuteNonQuery()

                        Cmd.CommandText = "INSERT INTO paralemp_co " &
                                        "(codequip, tipequip, datiniparal, datfimparal, intiniparal, intfimparal, datpdp) " &
                                        "SELECT codequip, tipequip, datiniparal, datfimparal, intiniparal, intfimparal, " & Format(CDate(cboDataPDP.SelectedValue), "yyyyMMdd") & " AS datpdp " &
                                        "FROM paralemp_co p, gerad g, usina u " &
                                        "WHERE u.codempre = '" & cboEmpresa.SelectedValue.Trim & "' " &
                                        "AND u.codusina = g.codusina " &
                                        "AND g.codgerad = p.codequip " &
                                        "AND p.datpdp = '" & cboDataAnterior.SelectedValue & "' " &
                                        "AND datfimparal >= '" & Format(CDate(cboDataPDP.SelectedValue), "yyyyMMdd") & "'"
                        intRetorno = Cmd.ExecuteNonQuery()
                        If intRetorno > 0 Then
                            'Grava evento registrando o recebimento de Parada de Máquinas por Conveniência Operativa
                            GravaEventoPDP("49", Format(Session("datEscolhida"), "yyyyMMdd"), cboEmpresa.SelectedItem.Text, Now.AddSeconds(22), "PDPRecupera", UsuarID)
                        End If
                    End If

                    '#### GARANTIA ENERGÉTICA ####
                    If chkRMP.Checked = True Then
                        'Elimina tabela temporária
                        Cmd.CommandText = "DROP TABLE #tmp_rmp"
                        Try
                            Cmd.ExecuteNonQuery()
                        Catch ex As Exception
                            'Se ocorrer erro ignora
                        End Try
                        '-- Coloca os valores na tabela temporária
                        Cmd.CommandText = "SELECT c.codusina, c.intrmp, c.valrmptran " &
                                        "INTO #tmp_rmp " &
                                        "FROM tb_rmp c, usina u " &
                                        "WHERE u.codempre = '" & cboEmpresa.SelectedValue.Trim & "' " &
                                        "AND u.codusina = c.codusina " &
                                        "AND c.datpdp = '" & cboDataAnterior.SelectedValue & "' "
                        Cmd.ExecuteNonQuery()

                        '-- Atualiza os valores da tabela tb_rmp com a tabela temporária
                        Cmd.CommandText = "UPDATE tb_rmp " &
                                        "SET valrmptran = " &
                                        "(SELECT t.valrmptran " &
                                        "FROM #tmp_rmp t " &
                                        "WHERE t.intrmp = tb_rmp.intrmp " &
                                        "AND t.codusina = tb_rmp.codusina) " &
                                        "WHERE EXISTS (SELECT u.codusina FROM usina u WHERE tb_rmp.codusina = u.codusina AND u.codempre = '" & cboEmpresa.SelectedValue.Trim & "') " &
                                        "AND datpdp = '" & Format(CDate(cboDataPDP.SelectedValue), "yyyyMMdd") & "'"
                        intRetorno = Cmd.ExecuteNonQuery()
                        If intRetorno > 0 Then
                            'Grava evento registrando o recebimento do insumo
                            GravaEventoPDP("52", Format(Session("datEscolhida"), "yyyyMMdd"), cboEmpresa.SelectedItem.Text, Now.AddSeconds(18), "PDPRecupera", UsuarID)
                        End If
                    End If

                    '#### GERAÇÃO FORMA DE MÉRITO ####
                    If chkGFM.Checked = True Then
                        'Elimina tabela temporária
                        Cmd.CommandText = "DROP TABLE #tmp_gfm"
                        Try
                            Cmd.ExecuteNonQuery()
                        Catch ex As Exception
                            'Se ocorrer erro ignora
                        End Try
                        '-- Coloca os valores na tabela temporária
                        Cmd.CommandText = "SELECT c.codusina, c.intgfm, c.valgfmtran " &
                                        "INTO #tmp_gfm " &
                                        "FROM tb_gfm c, usina u " &
                                        "WHERE u.codempre = '" & cboEmpresa.SelectedValue.Trim & "' " &
                                        "AND u.codusina = c.codusina " &
                                        "AND c.datpdp = '" & cboDataAnterior.SelectedValue & "' "
                        Cmd.ExecuteNonQuery()

                        '-- Atualiza os valores da tabela tb_gfm com a tabela temporária
                        Cmd.CommandText = "UPDATE tb_gfm " &
                                        "SET valgfmtran = " &
                                        "(SELECT t.valgfmtran " &
                                        "FROM #tmp_gfm t " &
                                        "WHERE t.intgfm = tb_gfm.intgfm " &
                                        "AND t.codusina = tb_gfm.codusina) " &
                                        "WHERE EXISTS (SELECT u.codusina FROM usina u WHERE tb_gfm.codusina = u.codusina AND u.codempre = '" & cboEmpresa.SelectedValue.Trim & "') " &
                                        "AND datpdp = '" & Format(CDate(cboDataPDP.SelectedValue), "yyyyMMdd") & "'"
                        intRetorno = Cmd.ExecuteNonQuery()
                        If intRetorno > 0 Then
                            'Grava evento registrando o recebimento do insumo
                            GravaEventoPDP("53", Format(Session("datEscolhida"), "yyyyMMdd"), cboEmpresa.SelectedItem.Text, Now.AddSeconds(18), "PDPRecupera", UsuarID)
                        End If
                    End If

                    '#### Crédito por Substituição ####
                    If chkCFM.Checked = True Then
                        'Elimina tabela temporária
                        Cmd.CommandText = "DROP TABLE #tmp_cfm"
                        Try
                            Cmd.ExecuteNonQuery()
                        Catch ex As Exception
                            'Se ocorrer erro ignora
                        End Try
                        '-- Coloca os valores na tabela temporária
                        Cmd.CommandText = "SELECT c.codusina, c.intcfm, c.valcfmtran " &
                                        "INTO #tmp_cfm " &
                                        "FROM tb_cfm c, usina u " &
                                        "WHERE u.codempre = '" & cboEmpresa.SelectedValue.Trim & "' " &
                                        "AND u.codusina = c.codusina " &
                                        "AND c.datpdp = '" & cboDataAnterior.SelectedValue & "' "
                        Cmd.ExecuteNonQuery()

                        '-- Atualiza os valores da tabela tb_cfm com a tabela temporária
                        Cmd.CommandText = "UPDATE tb_cfm " &
                                        "SET valcfmtran = " &
                                        "(SELECT t.valcfmtran " &
                                        "FROM #tmp_cfm t " &
                                        "WHERE t.intcfm = tb_cfm.intcfm " &
                                        "AND t.codusina = tb_cfm.codusina) " &
                                        "WHERE EXISTS (SELECT u.codusina FROM usina u WHERE tb_cfm.codusina = u.codusina AND u.codempre = '" & cboEmpresa.SelectedValue.Trim & "') " &
                                        "AND datpdp = '" & Format(CDate(cboDataPDP.SelectedValue), "yyyyMMdd") & "'"
                        intRetorno = Cmd.ExecuteNonQuery()
                        If intRetorno > 0 Then
                            'Grava evento registrando o recebimento do insumo
                            GravaEventoPDP("54", Format(Session("datEscolhida"), "yyyyMMdd"), cboEmpresa.SelectedItem.Text, Now.AddSeconds(18), "PDPRecupera", UsuarID)
                        End If
                    End If

                    '#### Geração Substituta ####
                    If chkSOM.Checked = True Then
                        'Elimina tabela temporária
                        Cmd.CommandText = "DROP TABLE #tmp_som"
                        Try
                            Cmd.ExecuteNonQuery()
                        Catch ex As Exception
                            'Se ocorrer erro ignora
                        End Try
                        '-- Coloca os valores na tabela temporária
                        Cmd.CommandText = "SELECT c.codusina, c.intsom, c.valsomtran " &
                                        "INTO #tmp_som " &
                                        "FROM tb_som c, usina u " &
                                        "WHERE u.codempre = '" & cboEmpresa.SelectedValue.Trim & "' " &
                                        "AND u.codusina = c.codusina " &
                                        "AND c.datpdp = '" & cboDataAnterior.SelectedValue & "' "
                        Cmd.ExecuteNonQuery()

                        '-- Atualiza os valores da tabela tb_som com a tabela temporária
                        Cmd.CommandText = "UPDATE tb_som " &
                                        "SET valsomtran = " &
                                        "(SELECT t.valsomtran " &
                                        "FROM #tmp_som t " &
                                        "WHERE t.intsom = tb_som.intsom " &
                                        "AND t.codusina = tb_som.codusina) " &
                                        "WHERE EXISTS (SELECT u.codusina FROM usina u WHERE tb_som.codusina = u.codusina AND u.codempre = '" & cboEmpresa.SelectedValue.Trim & "') " &
                                        "AND datpdp = '" & Format(CDate(cboDataPDP.SelectedValue), "yyyyMMdd") & "'"
                        intRetorno = Cmd.ExecuteNonQuery()
                        If intRetorno > 0 Then
                            'Grava evento registrando o recebimento do insumo
                            GravaEventoPDP("55", Format(Session("datEscolhida"), "yyyyMMdd"), cboEmpresa.SelectedItem.Text, Now.AddSeconds(18), "PDPRecupera", UsuarID)
                        End If
                    End If

                    '#### GE SUBSTITUIÇÃO ####
                    If chkGES.Checked = True Then
                        'Elimina tabela temporária
                        Cmd.CommandText = "DROP TABLE tmp_GES"
                        Try
                            Cmd.ExecuteNonQuery()
                        Catch ex As Exception
                            'Se ocorrer erro ignora
                        End Try
                        '-- Coloca os valores na tabela temporária
                        Cmd.CommandText = "SELECT c.codusina, c.intGES, c.valGEStran " &
                                        "INTO #tmp_GES " &
                                        "FROM tb_GES c, usina u " &
                                        "WHERE u.codempre = '" & cboEmpresa.SelectedValue.Trim & "' " &
                                        "AND u.codusina = c.codusina " &
                                        "AND c.datpdp = '" & cboDataAnterior.SelectedValue & "' "
                        Cmd.ExecuteNonQuery()

                        '-- Atualiza os valores da tabela tb_GES com a tabela temporária
                        Cmd.CommandText = "UPDATE tb_GES " &
                                        "SET valGEStran = " &
                                        "(SELECT t.valGEStran " &
                                        "FROM #tmp_GES t " &
                                        "WHERE t.intGES = tb_GES.intGES " &
                                        "AND t.codusina = tb_GES.codusina) " &
                                        "WHERE EXISTS (SELECT u.codusina FROM usina u WHERE tb_GES.codusina = u.codusina AND u.codempre = '" & cboEmpresa.SelectedValue.Trim & "') " &
                                        "AND datpdp = '" & Format(CDate(cboDataPDP.SelectedValue), "yyyyMMdd") & "'"
                        intRetorno = Cmd.ExecuteNonQuery()
                        If intRetorno > 0 Then
                            'Grava evento registrando o recebimento do insumo
                            GravaEventoPDP("56", Format(Session("datEscolhida"), "yyyyMMdd"), cboEmpresa.SelectedItem.Text, Now.AddSeconds(18), "PDPRecupera", UsuarID)
                        End If
                    End If

                    '#### GE CRÉDITO ####
                    If chkGEC.Checked = True Then
                        'Elimina tabela temporária
                        Cmd.CommandText = "DROP TABLE #tmp_GEC"
                        Try
                            Cmd.ExecuteNonQuery()
                        Catch ex As Exception
                            'Se ocorrer erro ignora
                        End Try
                        '-- Coloca os valores na tabela temporária
                        Cmd.CommandText = "SELECT c.codusina, c.intGEC, c.valGECtran " &
                                        "INTO #tmp_GEC " &
                                        "FROM tb_GEC c, usina u " &
                                        "WHERE u.codempre = '" & cboEmpresa.SelectedValue.Trim & "' " &
                                        "AND u.codusina = c.codusina " &
                                        "AND c.datpdp = '" & cboDataAnterior.SelectedValue & "' "
                        Cmd.ExecuteNonQuery()

                        '-- Atualiza os valores da tabela tb_GEC com a tabela temporária
                        Cmd.CommandText = "UPDATE tb_GEC " &
                                        "SET valGECtran = " &
                                        "(SELECT t.valGECtran " &
                                        "FROM #tmp_GEC t " &
                                        "WHERE t.intGEC = tb_GEC.intGEC " &
                                        "AND t.codusina = tb_GEC.codusina) " &
                                        "WHERE EXISTS (SELECT u.codusina FROM usina u WHERE tb_GEC.codusina = u.codusina AND u.codempre = '" & cboEmpresa.SelectedValue.Trim & "') " &
                                        "AND datpdp = '" & Format(CDate(cboDataPDP.SelectedValue), "yyyyMMdd") & "'"
                        intRetorno = Cmd.ExecuteNonQuery()
                        If intRetorno > 0 Then
                            'Grava evento registrando o recebimento do insumo
                            GravaEventoPDP("57", Format(Session("datEscolhida"), "yyyyMMdd"), cboEmpresa.SelectedItem.Text, Now.AddSeconds(18), "PDPRecupera", UsuarID)
                        End If
                    End If

                    '#### DESPACHO CICLO ABERTO ####
                    If chkDCA.Checked = True Then
                        'Elimina tabela temporária
                        Cmd.CommandText = "DROP TABLE #tmp_DCA"
                        Try
                            Cmd.ExecuteNonQuery()
                        Catch ex As Exception
                            'Se ocorrer erro ignora
                        End Try
                        '-- Coloca os valores na tabela temporária
                        Cmd.CommandText = "SELECT c.codusina, c.intDCA, c.valDCAtran " &
                                        "INTO #tmp_DCA " &
                                        "FROM tb_DCA c, usina u " &
                                        "WHERE u.codempre = '" & cboEmpresa.SelectedValue.Trim & "' " &
                                        "AND u.codusina = c.codusina " &
                                        "AND c.datpdp = '" & cboDataAnterior.SelectedValue & "' "
                        Cmd.ExecuteNonQuery()

                        '-- Atualiza os valores da tabela tb_DCA com a tabela temporária
                        Cmd.CommandText = "UPDATE tb_DCA " &
                                        "SET valDCAtran = " &
                                        "(SELECT t.valDCAtran " &
                                        "FROM #tmp_DCA t " &
                                        "WHERE t.intDCA = tb_DCA.intDCA " &
                                        "AND t.codusina = tb_DCA.codusina) " &
                                        "WHERE EXISTS (SELECT u.codusina FROM usina u WHERE tb_DCA.codusina = u.codusina AND u.codempre = '" & cboEmpresa.SelectedValue.Trim & "') " &
                                        "AND datpdp = '" & Format(CDate(cboDataPDP.SelectedValue), "yyyyMMdd") & "'"
                        intRetorno = Cmd.ExecuteNonQuery()
                        If intRetorno > 0 Then
                            'Grava evento registrando o recebimento do insumo
                            GravaEventoPDP("58", Format(Session("datEscolhida"), "yyyyMMdd"), cboEmpresa.SelectedItem.Text, Now.AddSeconds(18), "PDPRecupera", UsuarID)
                        End If
                    End If

                    '#### DESPACHO CICLO REDUZIDO ####
                    If chkDCR.Checked = True Then
                        'Elimina tabela temporária
                        Cmd.CommandText = "DROP TABLE #tmp_DCR"
                        Try
                            Cmd.ExecuteNonQuery()
                        Catch ex As Exception
                            'Se ocorrer erro ignora
                        End Try
                        '-- Coloca os valores na tabela temporária
                        Cmd.CommandText = "SELECT c.codusina, c.intDCR, c.valDCRtran " &
                                        "INTO #tmp_DCR " &
                                        "FROM tb_DCR c, usina u " &
                                        "WHERE u.codempre = '" & cboEmpresa.SelectedValue.Trim & "' " &
                                        "AND u.codusina = c.codusina " &
                                        "AND c.datpdp = '" & cboDataAnterior.SelectedValue & "' "
                        Cmd.ExecuteNonQuery()

                        '-- Atualiza os valores da tabela tb_DCR com a tabela temporária
                        Cmd.CommandText = "UPDATE tb_DCR " &
                                        "SET valDCRtran = " &
                                        "(SELECT t.valDCRtran " &
                                        "FROM #tmp_DCR t " &
                                        "WHERE t.intDCR = tb_DCR.intDCR " &
                                        "AND t.codusina = tb_DCR.codusina) " &
                                        "WHERE EXISTS (SELECT u.codusina FROM usina u WHERE tb_DCR.codusina = u.codusina AND u.codempre = '" & cboEmpresa.SelectedValue.Trim & "') " &
                                        "AND datpdp = '" & Format(CDate(cboDataPDP.SelectedValue), "yyyyMMdd") & "'"
                        intRetorno = Cmd.ExecuteNonQuery()
                        If intRetorno > 0 Then
                            'Grava evento registrando o recebimento do insumo
                            GravaEventoPDP("59", Format(Session("datEscolhida"), "yyyyMMdd"), cboEmpresa.SelectedItem.Text, Now.AddSeconds(18), "PDPRecupera", UsuarID)
                        End If
                    End If

                    '#### INSUMO RESERVA 1  ####
                    If chkIR1.Checked = True Then
                        'Elimina tabela temporária
                        Cmd.CommandText = "DROP TABLE #tmp_IR1"
                        Try
                            Cmd.ExecuteNonQuery()
                        Catch ex As Exception
                            'Se ocorrer erro ignora
                        End Try
                        '-- Coloca os valores na tabela temporária
                        Cmd.CommandText = "SELECT c.datPdp, c.codusina, c.valIR1tran " &
                                        "INTO #tmp_IR1 " &
                                        "FROM tb_IR1 c, usina u " &
                                        "WHERE u.codempre = '" & cboEmpresa.SelectedValue.Trim & "' " &
                                        "AND u.codusina = c.codusina " &
                                        "AND c.datpdp = '" & cboDataAnterior.SelectedValue & "' "
                        Cmd.ExecuteNonQuery()

                        '-- Atualiza os valores da tabela tb_IR1 com a tabela temporária
                        Cmd.CommandText = "UPDATE tb_IR1 " &
                                        "SET valIR1tran = " &
                                        "(SELECT t.valIR1tran " &
                                        "FROM #tmp_IR1 t " &
                                        "WHERE t.datPdp = tb_IR1.datPdp " & '?
                                        "AND t.codusina = tb_IR1.codusina) " &
                                        "WHERE EXISTS (SELECT u.codusina FROM usina u WHERE tb_IR1.codusina = u.codusina AND u.codempre = '" & cboEmpresa.SelectedValue.Trim & "') " &
                                        "AND datpdp = '" & Format(CDate(cboDataPDP.SelectedValue), "yyyyMMdd") & "'"
                        intRetorno = Cmd.ExecuteNonQuery()
                        If intRetorno > 0 Then
                            'Grava evento registrando o recebimento do insumo
                            GravaEventoPDP("60", Format(Session("datEscolhida"), "yyyyMMdd"), cboEmpresa.SelectedItem.Text, Now.AddSeconds(18), "PDPRecupera", UsuarID)
                        End If
                    End If

                    '#### INSUMO RESERVA 2  ####
                    If chkIR2.Checked = True Then
                        'Elimina tabela temporária
                        Cmd.CommandText = "DROP TABLE #tmp_IR2"
                        Try
                            Cmd.ExecuteNonQuery()
                        Catch ex As Exception
                            'Se ocorrer erro ignora
                        End Try
                        '-- Coloca os valores na tabela temporária
                        Cmd.CommandText = "SELECT  c.codusina, c.intIR2, c.valIR2tran " &
                                        "INTO #tmp_IR2 " &
                                        "FROM tb_IR2 c, usina u " &
                                        "WHERE u.codempre = '" & cboEmpresa.SelectedValue.Trim & "' " &
                                        "AND u.codusina = c.codusina " &
                                        "AND c.datpdp = '" & cboDataAnterior.SelectedValue & "' "
                        Cmd.ExecuteNonQuery()

                        '-- Atualiza os valores da tabela tb_IR2 com a tabela temporária
                        Cmd.CommandText = "UPDATE tb_IR2 " &
                                        "SET valIR2tran = " &
                                        "(SELECT t.valIR2tran " &
                                        "FROM #tmp_IR2 t " &
                                        "WHERE t.intIR2 = tb_IR2.intIR2 " &
                                        "AND t.codusina = tb_IR2.codusina) " &
                                        "WHERE EXISTS (SELECT u.codusina FROM usina u WHERE tb_IR2.codusina = u.codusina AND u.codempre = '" & cboEmpresa.SelectedValue.Trim & "') " &
                                        "AND datpdp = '" & Format(CDate(cboDataPDP.SelectedValue), "yyyyMMdd") & "'"
                        intRetorno = Cmd.ExecuteNonQuery()
                        If intRetorno > 0 Then
                            'Grava evento registrando o recebimento do insumo
                            GravaEventoPDP("61", Format(Session("datEscolhida"), "yyyyMMdd"), cboEmpresa.SelectedItem.Text, Now.AddSeconds(18), "PDPRecupera", UsuarID)
                        End If
                    End If

                    '#### INSUMO RESERVA 3  ####
                    If chkIR3.Checked = True Then
                        'Elimina tabela temporária
                        Cmd.CommandText = "DROP TABLE #tmp_IR3"
                        Try
                            Cmd.ExecuteNonQuery()
                        Catch ex As Exception
                            'Se ocorrer erro ignora
                        End Try
                        '-- Coloca os valores na tabela temporária
                        Cmd.CommandText = "SELECT c.codusina, c.intIR3, c.valIR3tran " &
                                        "INTO #tmp_IR3 " &
                                        "FROM tb_IR3 c, usina u " &
                                        "WHERE u.codempre = '" & cboEmpresa.SelectedValue.Trim & "' " &
                                        "AND u.codusina = c.codusina " &
                                        "AND c.datpdp = '" & cboDataAnterior.SelectedValue & "' "
                        Cmd.ExecuteNonQuery()

                        '-- Atualiza os valores da tabela tb_IR3 com a tabela temporária
                        Cmd.CommandText = "UPDATE tb_IR3 " &
                                        "SET valIR3tran = " &
                                        "(SELECT t.valIR3tran " &
                                        "FROM #tmp_IR3 t " &
                                        "WHERE t.intIR3 = tb_IR3.intIR3 " &
                                        "AND t.codusina = tb_IR3.codusina) " &
                                        "WHERE EXISTS (SELECT u.codusina FROM usina u WHERE tb_IR3.codusina = u.codusina AND u.codempre = '" & cboEmpresa.SelectedValue.Trim & "') " &
                                        "AND datpdp = '" & Format(CDate(cboDataPDP.SelectedValue), "yyyyMMdd") & "'"
                        intRetorno = Cmd.ExecuteNonQuery()
                        If intRetorno > 0 Then
                            'Grava evento registrando o recebimento do insumo
                            GravaEventoPDP("62", Format(Session("datEscolhida"), "yyyyMMdd"), cboEmpresa.SelectedItem.Text, Now.AddSeconds(18), "PDPRecupera", UsuarID)
                        End If
                    End If

                    '#### INSUMO RESERVA 4  ####
                    If chkIR4.Checked = True Then
                        'Elimina tabela temporária
                        Cmd.CommandText = "DROP TABLE #tmp_IR4"
                        Try
                            Cmd.ExecuteNonQuery()
                        Catch ex As Exception
                            'Se ocorrer erro ignora
                        End Try
                        '-- Coloca os valores na tabela temporária
                        Cmd.CommandText = "SELECT c.codusina, c.intIR4, c.valIR4tran " &
                                        "INTO #tmp_IR4 " &
                                        "FROM tb_IR4 c, usina u " &
                                        "WHERE u.codempre = '" & cboEmpresa.SelectedValue.Trim & "' " &
                                        "AND u.codusina = c.codusina " &
                                        "AND c.datpdp = '" & cboDataAnterior.SelectedValue & "' "
                        Cmd.ExecuteNonQuery()

                        '-- Atualiza os valores da tabela tb_IR4 com a tabela temporária
                        Cmd.CommandText = "UPDATE tb_IR4 " &
                                        "SET valIR4tran = " &
                                        "(SELECT t.valIR4tran " &
                                        "FROM #tmp_IR4 t " &
                                        "WHERE t.intIR4 = tb_IR4.intIR4 " &
                                        "AND t.codusina = tb_IR4.codusina) " &
                                        "WHERE EXISTS (SELECT u.codusina FROM usina u WHERE tb_IR4.codusina = u.codusina AND u.codempre = '" & cboEmpresa.SelectedValue.Trim & "') " &
                                        "AND datpdp = '" & Format(CDate(cboDataPDP.SelectedValue), "yyyyMMdd") & "'"
                        intRetorno = Cmd.ExecuteNonQuery()
                        If intRetorno > 0 Then
                            'Grava evento registrando o recebimento do insumo
                            GravaEventoPDP("63", Format(Session("datEscolhida"), "yyyyMMdd"), cboEmpresa.SelectedItem.Text, Now.AddSeconds(18), "PDPRecupera", UsuarID)
                        End If
                    End If

                    '### Término da recuperação dos dados ###
                    objTrans.Commit()
                    Conn.Close()
                    lblMsg.Text = "Recuperação realizada."

                Catch ex As Exception
                    objTrans.Rollback()
                    Conn.Close()
                    Session("strMensagem") = "Ocorreu um erro ao efetuar a recuperação."

                    Response.Redirect("frmMensagem.aspx")
                End Try

            Else
                rsData.Close()
                Session("strMensagem") = "A data do PDP já foi encerrada. "
                Response.Redirect("frmMensagem.aspx")
            End If
        Else
            Session("strMensagem") = "Usuário não tem permissão para recuperar os dados."
            Response.Redirect("frmMensagem.aspx")
        End If
    End Sub

    Private Sub PreencheComboDataAnt()
        'Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
        Dim Conn As System.Data.SqlClient.SqlConnection = New System.Data.SqlClient.SqlConnection
        Conn.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
        'Dim daData As OnsClasses.OnsData.OnsDataAdapter
        Dim daData As SqlDataAdapter
        Dim dsData As DataSet
        Dim strSql As String

        If cboDataPDP.SelectedIndex <> 0 Then
            cboDataAnterior.Items.Clear()
            Conn.Open()
            strSql = "SELECT " &
                            " SUBSTRING(datpdp, 7, 2) + '/' + SUBSTRING(datpdp, 5, 2) + '/' + SUBSTRING(datpdp, 1, 4) AS datformat, " &
                            " datpdp FROM pdp  WHERE datpdp < '" & Format(CDate(cboDataPDP.SelectedValue), "yyyyMMdd") & "' " &
                        " ORDER BY  datpdp DESC"

            'strSql = "SELECT datpdp[7,8] || '/' || datpdp[5,6] || '/' || datpdp[1,4] AS datformat, datpdp " &
            '         "FROM pdp " &
            '         "WHERE datpdp < '" & Format(CDate(cboDataPDP.SelectedValue), "yyyyMMdd") & "' " &
            '         "ORDER BY datpdp Desc"

            'daData = New OnsClasses.OnsData.OnsDataAdapter(strSql, Conn)
            daData = New SqlDataAdapter(strSql, Conn)
            dsData = New DataSet
            daData.Fill(dsData, "Data")
            cboDataAnterior.DataTextField = "datformat"
            cboDataAnterior.DataValueField = "datpdp"
            cboDataAnterior.DataSource = dsData.Tables("Data").DefaultView
            cboDataAnterior.Items.Add("")
            cboDataAnterior.DataBind()
            Conn.Close()
        End If
    End Sub

    Private Sub cboEmpresa_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboEmpresa.SelectedIndexChanged
        If cboEmpresa.SelectedIndex > 0 Then
            Session("strCodEmpre") = cboEmpresa.SelectedItem.Value
        End If
    End Sub
End Class

