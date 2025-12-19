'Classes base para manipulação de texto
Imports System.Text
'Classes base para manipulação do ambiente corrente
Imports System.Environment

'Classes base do Crystal Report
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.CrystalReports
Imports CrystalDecisions.ReportSource
Imports CrystalDecisions.Shared

Imports System.Net.WebClient
Imports System.IO.Directory

Partial Class frmExporta
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
    Private strNomeRel As String
    Private objReport As ReportDocument
    Public intRequisito As Integer
    Public strRequisito As String
    Public intReserva As Integer
    Public strReserva As String
    Public strDataReser As String

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        If Request.QueryString("strTipoGer") = "TodosSE" Or Request.QueryString("strTipoGer") = "TodosNEtxt" Then
            cboTipo.Enabled = False
            btnExportar.Enabled = False
        End If

        'Foi feita uma alteração na estrutura da tabela reser
        'por isso para as data anteriores a 26/09/2008 é necessário realizar este teste abaixo
        'e utilizar esta variável ao invés do page.request.querystring("strData")
        If Page.Request.QueryString("strData") <= "20080630" Then
            strDataReser = "20080630"
        ElseIf Page.Request.QueryString("strData") >= "20080701" And Page.Request.QueryString("strData") <= "20080731" Then
            strDataReser = "20080731"
        ElseIf Page.Request.QueryString("strData") >= "20080801" And Page.Request.QueryString("strData") <= "20080831" Then
            strDataReser = "20080831"
        ElseIf Page.Request.QueryString("strData") >= "20080901" And Page.Request.QueryString("strData") <= "20080930" Then
            strDataReser = "20080930"
        Else
            strDataReser = Page.Request.QueryString("strData")
        End If

        If Not Page.IsPostBack Then
            If Request.QueryString("strTipoGer") = "TodosSE" Or Request.QueryString("strTipoGer") = "TodosNEtxt" Then
                'Todos os Relatórios do SE ou do NE em Arquivo Texto
                MontaTipos("T")
                MostraArquivoTexto()
            ElseIf Request.QueryString("strTipoGer") = "TodosS" Then
                'Todos os Relatórios do Sul em PDF
                MontaTipos("E")
                MostraRelatorioSubS()
            ElseIf Request.QueryString("strTipoGer") = "TodosNE" Then
                'Todos os Relatórios do NE em PDF
                MontaTipos("E")
                MostraRelatorioSubNE()
            ElseIf Request.QueryString("strTipoGer") = "TodosNCO" Then
                'Todos os Relatórios do N/CO em PDF
                MontaTipos("E")
                MostraRelatorioSubNCO()
            ElseIf Request.QueryString("strTipoGer") = "TodosDeflux" Then
                'Prog Diaria Defluxo SIN em PDF
                '-- CRQ5000 - 17/09/2013
                'MontaTipos("E") alterado para ("S")
                MontaTipos("S")
                MostraRelatorioSubEC()
                Session("report_count") = 0
            Else
                'Outros Relatórios em PDF
                MontaTipos("E")
                BindReport(Request.QueryString("strTipoGer"))
                MostraRelatorio()
            End If
        End If
    End Sub

    Private Sub MontaTipos(ByVal strTipo As String)
        If strTipo = "E" Then
            cboTipo.Items.Add("Excel")
            cboTipo.Items.Add("Word")
            '-- CRQ5000 - 17/09/2013
        ElseIf strTipo = "S" Then
            cboTipo.Items.Add("CSV")
            cboTipo.Items.Add("Word")
        Else
            cboTipo.Items.Add("Arquivo Texto")
        End If
    End Sub

    Sub BindReport(ByVal strTipoGer As String)
        Dim objConnection As New OnsClasses.OnsData.OnsConnection
        Dim objCommand As New OnsClasses.OnsData.OnsCommand
        Dim strSql As String
        Dim strTable As String

        '-- CRQ3713 - 14/01/2013
        'Dim varRegistry As Microsoft.Win32.RegistryKey
        'varRegistry = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\ONS\IU\rpdp", False)
        Session("DataSourceBDT") = ConfigurationManager.AppSettings.Get("DataSourceBDT") 'varRegistry.GetValue("DataSourceBDT")

        Try
            objConnection.Open("rpdp")
            objCommand.Connection = objConnection
            Select Case strTipoGer

                Case Is = "PDDefluxObs" '-- CRQ3713 (14/01/13)

                    strSql = "SELECT b.nombacia AS bacia, u.nomusina AS usina, c.comentariopdf AS observacao " &
                             "  FROM usina u, bacia b, cota c " &
                             " WHERE u.tipusina = 'H' " &
                             "   AND u.codusina = c.codusina " &
                             "   AND c.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                             "   AND u.codbacia = b.codbacia " &
                             "   AND c.comentariopdf is not null " &
                             "   ORDER BY 1, 2"

                    strTable = "PDDefluxObs"


                Case Is = "PDDefluxSIN" '-- CRQ3713 (14/01/13)

                    '--Temporária de Seleção de Usinas por Bacia
                    Try
                        objCommand.CommandText = "DROP TABLE tmpPDDefluxSIN1"
                        objCommand.ExecuteNonQuery()
                    Catch ex As Exception
                        'Se ocorrer o erro, ignora
                    End Try

                    objCommand.CommandText = "SELECT b.nombacia AS bacia, u.codusina AS codusina, u.nomusina AS usina, u.sitoperacao AS sitoperacao, " &
                                             "v.valaflu AS afluente, v.valturb AS turbinada, v.valvert AS vertida, " &
                                             "c.outrasestruturas AS outrasestr, c.cotaini AS cotaini, c.cotafim AS cotafim, " &
                                             "c.comentariopdf AS observacao, u.usi_bdt_id AS usi_bdt_id, " &
                                             "substr(v.datpdp, 1, 4) || '-' || substr(v.datpdp, 5, 2) || '-' || substr(v.datpdp, 7, 2) as datpdp " &
                                             "  FROM usina u, bacia b, vazao v, outer cota c " &
                                             " WHERE u.tipusina = 'H' " &
                                             "   AND u.codusina = v.codusina " &
                                             "   AND u.codusina = c.codusina " &
                                             "   AND v.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                                             "   AND c.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                                             "   AND u.codbacia = b.codbacia " &
                                             "  INTO TEMP tmpPDDefluxSIN1 " &
                                             "WITH NO LOG"
                    objCommand.ExecuteNonQuery()

                    '--Temporária de Seleção Percentual e Volume
                    Try
                        objCommand.CommandText = "DROP TABLE tmpPDDefluxSIN2"
                        objCommand.ExecuteNonQuery()
                    Catch ex As Exception
                        'Se ocorrer o erro, ignora
                    End Try

                    objCommand.CommandText = "SELECT d1.bacia AS bacia, d1.codusina AS codusina, d1.usina AS usina, " &
                                             " d1.usi_bdt_id AS usi_bdt_id, r.res_id AS res_id,  r.volume AS volume, d1.cotaini AS cotaini, " &
                                             " res.res_volmin AS volmin, res.res_volutil AS volutil, g.valor AS volumeg " &
                                             "  FROM tmpPDDefluxSIN1 d1, " & Session("DataSourceBDT") & ":ins i, " & Session("DataSourceBDT") & ":aprov a, " &
                                             "       OUTER " & Session("DataSourceBDT") & ":rescotvol r, " & Session("DataSourceBDT") & ":res res, OUTER " & Session("DataSourceBDT") & ":gr_hidr_res g " &
                                             " WHERE i.ido_ins = d1.usi_bdt_id " &
                                             "   AND a.usi_id =  i.ins_id " &
                                             "   AND a.res_id = r.res_id " &
                                             "   AND a.res_id = res.res_id " &
                                             "   AND r.cota = d1.cotaini " &
                                             "   AND g.tpgrand_id = 'VUR' " &
                                             "   AND g.res_id = a.res_id " &
                                             "   AND EXTEND(g.instante, YEAR TO DAY) = '" &
                                             Mid(Page.Request.QueryString("strData"), 1, 4) & "-" & Mid(Page.Request.QueryString("strData"), 5, 2) & "-" & Mid(Page.Request.QueryString("strData"), 7, 2) & "' " &
                                             "  INTO TEMP tmpPDDefluxSIN2 " &
                                             "WITH NO LOG"
                    objCommand.ExecuteNonQuery()

                    '--Temporária de Seleção Volume para Cota Inferior e Superior a pesquisada, se não encontrada antes
                    Try
                        objCommand.CommandText = "DROP TABLE tmpPDDefluxSIN3"
                        objCommand.ExecuteNonQuery()
                    Catch ex As Exception
                        'Se ocorrer o erro, ignora
                    End Try

                    objCommand.CommandText = "SELECT d2.bacia AS bacia,  d2.codusina AS codusina, d2.usina AS usina, d2.usi_bdt_id AS usi_bdt_id, d2.res_id AS res_id, " &
                                             "(select r1.volume from " & Session("DataSourceBDT") & ":rescotvol r1 where r1.res_id = d2.res_id and r1.cota = (select max(r2.cota) from " & Session("DataSourceBDT") & ":rescotvol r2 where r2.res_id = d2.res_id and r2.cota < d2.cotaini)) AS volinf, " &
                                             "(select r1.volume from " & Session("DataSourceBDT") & ":rescotvol r1 where r1.res_id = d2.res_id and r1.cota = (select min(r2.cota) from " & Session("DataSourceBDT") & ":rescotvol r2 where r2.res_id = d2.res_id and r2.cota > d2.cotaini)) AS volsup " &
                                             "  FROM tmpPDDefluxSIN2 d2 " &
                                             "  INTO TEMP tmpPDDefluxSIN3 " &
                                             "WITH NO LOG"
                    objCommand.ExecuteNonQuery()

                    '--- Fim

                    '--Seleção para relatório
                    strSql = "SELECT d1.bacia AS bacia, d1.usina AS usina, d1.sitoperacao AS sitoperacao, " &
                             "d1.afluente AS afluente, d1.turbinada AS turbinada, d1.vertida AS vertida, " &
                             "d1.outrasestr AS outrasestr, d1.cotaini AS cotaini, d1.cotafim AS cotafinal, " &
                             "((coalesce(isnull(d2.volume,0), 0, isnull(d3.volinf,0) + (isnull(d3.volsup,0) - isnull(d3.volinf, 0)), d2.volume) - d2.volmin) * 100) / coalesce(isnull(d2.volutil,0),0,1) AS percentual, " &
                             "d2.volumeg AS volume, d1.observacao AS observacao " &
                             "FROM tmpPDDefluxSIN1 d1, tmpPDDefluxSIN2 d2, tmpPDDefluxSIN3 d3 " &
                             "WHERE d1.codusina = d2.codusina " &
                             "  AND d1.codusina = d3.codusina " &
                             "ORDER BY d1.bacia, d1.usina"

                    strTable = "PDDefluxSIN"

                Case Is = "UsiPorEmpre"
                    '-- IM44972 - Novo Relatório de Usina por Empresa
                    'Usina por Empresa 
                    strSql = "SELECT e.codempre, e.nomempre, u.sigusina, u.nomusina " &
                             "FROM usina u, empre e " &
                             "WHERE u.codempre = e.codempre " &
                             "ORDER BY e.codempre, u.sigusina "
                    strTable = "UsiPorEmpre"
                Case Is = "Hidro"
                    'Geração Hidráulica da Regional SUL
                    '-- MarcosA 2009-09-11 IM50174 - Retirada da seleção abaixo a restrição u.codusina <> 'ERPTER'
                    strSql = "SELECT d.codusina[1,2] AS codempre, d.codusina[3,10] AS codusina, d.intdespa, NVL(d.valdespasup,0) AS valdespaemp, NVL(u.ordem,1) AS ordem " &
                             "FROM despa d, usina u, empre e " &
                             "WHERE d.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                             "AND d.codusina = u.codusina " &
                             "AND u.tipusina = 'H' " &
                             "AND u.codusina <> 'TSPHID' " &
                             "AND u.codempre = e.codempre " &
                             "AND e.codarea = 'RS' " &
                             "AND e.codempre <> 'CE' " &
                             "AND e.codempre <> 'CO' " &
                             "UNION " &
                             "SELECT 'HIDRO' AS codempre, 'TOTAL' AS codusina, d.intdespa, SUM(NVL(d.valdespasup,0)) AS valdespaemp, 97 AS ordem " &
                             "FROM despa d, usina u, empre e " &
                             "WHERE d.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                             "AND d.codusina = u.codusina " &
                             "AND u.tipusina = 'H' " &
                             "AND u.codusina <> 'TSPHID' " &
                             "AND u.codempre = e.codempre " &
                             "AND e.codarea = 'RS' " &
                             "AND e.codempre <> 'CE' " &
                             "AND e.codempre <> 'CO' " &
                             "GROUP BY intdespa " &
                             "UNION " &
                             "SELECT d.codusina[1,2] AS codempre, d.codusina[3,10] AS codusina, d.intdespa, NVL(d.valdespasup,0) AS valdespaemp, 98 AS ordem " &
                             "FROM despa d, usina u, empre e " &
                             "WHERE d.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                             "AND d.codusina = u.codusina " &
                             "AND (u.codusina = 'EFEVSO' " &
                             "OR u.codusina = 'EFEVSS' " &
                             "OR u.codusina = 'EFEVSI') " &
                             "AND u.codempre = e.codempre " &
                             "AND e.codarea = 'RS' " &
                             "UNION " &
                             "SELECT 'GERAL' AS codempre, 'TOTAL' AS codusina, d.intdespa, SUM(NVL(d.valdespasup,0)) AS valdespaemp, 99 AS ordem " &
                             "FROM despa d, usina u, empre e " &
                             "WHERE datpdp = '" & Page.Request.QueryString("strData") & "' " &
                             "AND d.codusina = u.codusina " &
                             "AND (u.tipusina = 'H' Or u.tipusina = 'T') " &
                             "AND u.codusina <> 'TSPTER' " &
                             "AND u.codusina <> 'TSUTWE' " &
                             "AND u.codusina <> 'SCPTER' " &
                             "AND u.codusina <> 'TSPHID' " &
                             "AND u.codempre = e.codempre " &
                             "AND e.codarea = 'RS' " &
                             "AND e.codempre <> 'CE' " &
                             "AND e.codempre <> 'CO' " &
                             "GROUP BY intdespa " &
                             "ORDER BY 3, 5"
                    strTable = "despa"
                Case Is = "Termo"
                    'Geração Térmica da Regional SUL
                    '-- IM44972
                    'Elimina a tabela temporária com Geração da BSUTCA
                    objCommand.CommandText = "DROP TABLE tmpGerBSUTCA"
                    Try
                        objCommand.ExecuteNonQuery()
                    Catch ex As Exception
                        'se a temporária não existe ignora o erro
                    End Try
                    'Seleciona os valores de Geração da BSUTCA
                    objCommand.CommandText = "SELECT SUM(valdespasup) AS valdespasup, intdespa " &
                                        "FROM despa " &
                                        "WHERE datpdp = '" & Page.Request.QueryString("strData") & "' " &
                                        "AND codusina in ('BSTCCA','BSTCAB','BSTCAT') " &
                                        "GROUP BY intdespa " &
                                        "INTO TEMP tmpGerBSUTCA " &
                                        "WITH NO LOG"
                    objCommand.ExecuteNonQuery()
                    'Elimina a tabela temporária com Geração da BSARC
                    objCommand.CommandText = "DROP TABLE tmpGerBSARC"
                    Try
                        objCommand.ExecuteNonQuery()
                    Catch ex As Exception
                        'se a temporária não existe ignora o erro
                    End Try
                    'Seleciona os valores de Geração da BSARC
                    objCommand.CommandText = "SELECT SUM(valdespasup) AS valdespasup, intdespa " &
                                        "FROM despa " &
                                        "WHERE datpdp = '" & Page.Request.QueryString("strData") & "' " &
                                        "AND codusina in ('BSARCA','BSARCB','BSARCT') " &
                                        "GROUP BY intdespa " &
                                        "INTO TEMP tmpGerBSARC " &
                                        "WITH NO LOG"
                    objCommand.ExecuteNonQuery()
                    'Elimina a tabela temporária com Geração Total do Sul
                    objCommand.CommandText = "DROP TABLE tmpGerTotSul"
                    Try
                        objCommand.ExecuteNonQuery()
                    Catch ex As Exception
                        'se a temporária não existe ignora o erro
                    End Try
                    'Seleciona os valores de Geração Total do Sul
                    '-- MarcosA 2009-09-11 IM50174 - Retirada da seleção abaixo a restrição u.codusina <> 'ERPTER'
                    objCommand.CommandText = "SELECT 'TERMO' AS codempre, 'TOTAL' AS codusina, d.intdespa, SUM(NVL(d.valdespasup,0)) AS valdespasup, 98 AS ordem " &
                             "FROM despa d, usina u, empre e " &
                             "WHERE d.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                             "AND d.codusina = u.codusina " &
                             "AND u.tipusina = 'T' " &
                             "AND u.codusina <> 'TSPTER' " &
                             "AND u.codusina <> 'TSUTWE' " &
                             "AND u.codusina <> 'SCPTER' " &
                             "AND u.codusina <> 'CEPTER' " &
                             "AND u.codusina <> 'EFEVSO' " &
                             "AND u.codusina <> 'EFEVSS' " &
                             "AND u.codusina <> 'EFEVSI' " &
                             "AND u.codusina <> 'BSUTCA' " &
                             "AND u.codusina <> 'BSARC' " &
                             "AND u.codempre = e.codempre " &
                             "AND e.codarea = 'RS' " &
                             "AND e.codempre <> 'CE' " &
                             "AND e.codempre <> 'CO' " &
                             "GROUP BY intdespa " &
                             "INTO TEMP tmpGerTotSul " &
                             "WITH NO LOG"
                    objCommand.ExecuteNonQuery()
                    '--
                    '-- MarcosA 2009-09-11 IM50174 - Retirada da seleção abaixo a restrição u.codusina <> 'ERPTER'
                    strSql = "SELECT d.codusina[1,2] AS codempre, d.codusina[3,10] AS codusina, d.intdespa, NVL(d.valdespasup,0) AS valdespaemp, NVL(u.ordem,1) AS ordem " &
                             "FROM despa d, usina u, empre e " &
                             "WHERE d.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                             "AND d.codusina = u.codusina " &
                             "AND u.tipusina = 'T' " &
                             "AND u.codusina <> 'TSPTER' " &
                             "AND u.codusina <> 'TSUTWE' " &
                             "AND u.codusina <> 'SCPTER' " &
                             "AND u.codusina <> 'CEPTER' " &
                             "AND u.codusina <> 'EFEVSO' " &
                             "AND u.codusina <> 'EFEVSS' " &
                             "AND u.codusina <> 'EFEVSI' " &
                             "AND u.codusina <> 'BSUTCA' " &
                             "AND u.codusina <> 'BSARC' " &
                             "AND u.codempre = e.codempre " &
                             "AND e.codarea = 'RS' " &
                             "AND e.codempre <> 'CE' " &
                             "AND e.codempre <> 'CO' " &
                             "UNION " &
                             "SELECT u.codusina[1,2] AS codempre, d.codusina[3,10] AS codusina, d.intdespa, NVL(t.valdespasup,0) AS valdespaemp, NVL(u.ordem,1) AS ordem " &
                             "FROM despa d, usina u, tmpGerBSUTCA t " &
                             "WHERE d.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                             "AND d.codusina = u.codusina " &
                             "AND u.codusina = 'BSUTCA' " &
                             "AND d.intdespa = t.intdespa " &
                             "UNION " &
                             "SELECT u.codusina[1,2] AS codempre, d.codusina[3,10] AS codusina, d.intdespa, NVL(t.valdespasup,0) AS valdespaemp, NVL(u.ordem,1) AS ordem " &
                             "FROM despa d, usina u, tmpGerBSARC t " &
                             "WHERE d.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                             "AND d.codusina = u.codusina " &
                             "AND u.codusina = 'BSARC' " &
                             "AND d.intdespa = t.intdespa " &
                             "UNION " &
                             "SELECT ts.codempre, ts.codusina, ts.intdespa, (ts.valdespasup + t1.valdespasup + t2.valdespasup) AS valdespaemp,ts.ordem " &
                             "FROM tmpGerTotSul ts, tmpGerBSUTCA t1, tmpGerBSARC t2 " &
                             "WHERE ts.intdespa = t1.intdespa " &
                             "AND t1.intdespa = t2.intdespa " &
                             "ORDER BY 3, 5"
                    '-- IM44972
                    'strSql = "SELECT d.codusina[1,2] AS codempre, d.codusina[3,10] AS codusina, d.intdespa, NVL(d.valdespasup,0) AS valdespaemp, NVL(u.ordem,1) AS ordem " & _
                    '         "FROM despa d, usina u, empre e " & _
                    '         "WHERE d.datpdp = '" & Page.Request.QueryString("strData") & "' " & _
                    '         "AND d.codusina = u.codusina " & _
                    '         "AND u.tipusina = 'T' " & _
                    '         "AND u.codusina <> 'TSPTER' " & _
                    '         "AND u.codusina <> 'TSUTWE' " & _
                    '         "AND u.codusina <> 'ERPTER' " & _
                    '         "AND u.codusina <> 'SCPTER' " & _
                    '         "AND u.codusina <> 'CEPTER' " & _
                    '         "AND u.codusina <> 'EFEVSO' " & _
                    '         "AND u.codusina <> 'EFEVSS' " & _
                    '         "AND u.codusina <> 'EFEVSI' " & _
                    '         "AND u.codempre = e.codempre " & _
                    '         "AND e.codarea = 'RS' " & _
                    '         "AND e.codempre <> 'CE' " & _
                    '         "AND e.codempre <> 'CO' " & _
                    '         "UNION " & _
                    '         "SELECT 'TERMO' AS codempre, 'TOTAL' AS codusina, d.intdespa, SUM(NVL(d.valdespasup,0)) AS valdespaemp, 98 AS ordem " & _
                    '         "FROM despa d, usina u, empre e " & _
                    '         "WHERE d.datpdp = '" & Page.Request.QueryString("strData") & "' " & _
                    '         "AND d.codusina = u.codusina " & _
                    '         "AND u.tipusina = 'T' " & _
                    '         "AND u.codusina <> 'TSPTER' " & _
                    '         "AND u.codusina <> 'TSUTWE' " & _
                    '         "AND u.codusina <> 'ERPTER' " & _
                    '         "AND u.codusina <> 'SCPTER' " & _
                    '         "AND u.codusina <> 'CEPTER' " & _
                    '         "AND u.codusina <> 'EFEVSO' " & _
                    '         "AND u.codusina <> 'EFEVSS' " & _
                    '         "AND u.codusina <> 'EFEVSI' " & _
                    '         "AND u.codempre = e.codempre " & _
                    '         "AND e.codarea = 'RS' " & _
                    '         "AND e.codempre <> 'CE' " & _
                    '         "AND e.codempre <> 'CO' " & _
                    '         "GROUP BY intdespa " & _
                    '         "ORDER BY 3, 5"

                    strTable = "despa"
                Case Is = "GeracaoSE"
                    'Geração de Usinas da Regional SUDESTE
                    strSql = "SELECT d.codusina[1,2] AS codempre, d.codusina[3,10] AS codusina, d.intdespa, d.valdespasup AS valdespaemp, 1 AS ordem, TRIM(d.codusina) AS codigo " &
                             "FROM despa d, usina u, empre e " &
                             "WHERE d.codusina IN " &
                             "(SELECT u.codusina " &
                             "FROM usina u, empre e " &
                             "WHERE (e.codarea = 'RE' " &
                             "OR e.codarea = 'IB' " &
                             "OR e.codarea = 'CD' " &
                             "OR e.codarea = 'CM' " &
                             "OR e.codarea = 'SP' " &
                             "OR e.codarea = 'EP' " &
                             "OR e.codarea = 'GT' " &
                             "OR e.codarea = 'CS' " &
                             "OR e.codarea = 'GP') " &
                             "AND e.codempre = u.codempre) " &
                             "AND e.codempre = u.codempre " &
                             "AND u.codusina <> 'FUPTER' " &
                             "AND u.codusina <> 'FUPHID' " &
                             "AND u.codusina <> 'CBPTER' " &
                             "AND u.codusina <> 'CTPTER' " &
                             "AND u.codusina <> 'LGPHID' " &
                             "AND u.codusina <> 'LGPTER' " &
                             "AND u.codusina = d.codusina " &
                             "AND d.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                             "UNION ALL " &
                             "SELECT 'HIDRO' AS codempre, 'TOTAL' AS codusina, d.intdespa, SUM(d.valdespasup) AS valdespaemp, 2 AS ordem, 'TOTAL HIDRO' AS codigo " &
                             "FROM despa d, usina u, empre e " &
                             "WHERE d.codusina IN " &
                             "(SELECT u.codusina " &
                             "FROM usina u, empre e " &
                             "WHERE (e.codarea = 'RE' " &
                             "OR e.codarea = 'IB' " &
                             "OR e.codarea = 'CD' " &
                             "OR e.codarea = 'CM' " &
                             "OR e.codarea = 'SP' " &
                             "OR e.codarea = 'EP' " &
                             "OR e.codarea = 'GT' " &
                             "OR e.codarea = 'CS' " &
                             "OR e.codarea = 'GP') " &
                             "AND e.codempre = u.codempre " &
                             "AND u.tipusina = 'H') " &
                             "AND e.codempre = u.codempre " &
                             "AND u.tipusina = 'H' " &
                             "AND u.codusina <> 'FUPTER' " &
                             "AND u.codusina <> 'FUPHID' " &
                             "AND u.codusina <> 'CBPTER' " &
                             "AND u.codusina <> 'CTPTER' " &
                             "AND u.codusina <> 'LGPHID' " &
                             "AND u.codusina <> 'LGPTER' " &
                             "AND u.codusina = d.codusina " &
                             "AND d.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                             "GROUP BY 1, 2, 3, 5, 6 " &
                             "UNION ALL " &
                             "SELECT 'TERMO' AS codempre, 'TOTAL' AS codusina, d.intdespa, SUM(d.valdespasup) AS valdespaemp, 3 AS ordem, 'TOTAL TERMO' AS codigo " &
                             "FROM despa d, usina u, empre e " &
                             "WHERE d.codusina IN " &
                             "(SELECT u.codusina " &
                             "FROM usina u, empre e " &
                             "WHERE (e.codarea = 'RE' " &
                             "OR e.codarea = 'IB' " &
                             "OR e.codarea = 'CD' " &
                             "OR e.codarea = 'CM' " &
                             "OR e.codarea = 'SP' " &
                             "OR e.codarea = 'EP' " &
                             "OR e.codarea = 'GT' " &
                             "OR e.codarea = 'CS' " &
                             "OR e.codarea = 'GP') " &
                             "AND e.codempre = u.codempre " &
                             "AND u.tipusina = 'T') " &
                             "AND e.codempre = u.codempre " &
                             "AND u.tipusina = 'T' " &
                             "AND u.codusina <> 'FUPTER' " &
                             "AND u.codusina <> 'FUPHID' " &
                             "AND u.codusina <> 'CBPTER' " &
                             "AND u.codusina <> 'CTPTER' " &
                             "AND u.codusina <> 'LGPHID' " &
                             "AND u.codusina <> 'LGPTER' " &
                             "AND u.codusina = d.codusina " &
                             "AND d.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                             "GROUP BY 1, 2, 3, 5, 6 " &
                             "UNION ALL " &
                             "SELECT 'GERAÇÃO' AS codempre, 'TOTAL' AS codusina, d.intdespa, SUM(d.valdespasup) AS valdespaemp, 4 AS ordem, 'GERACAO TOTAL' AS codigo " &
                             "FROM despa d, usina u, empre e " &
                             "WHERE d.codusina IN " &
                             "(SELECT u.codusina " &
                             "FROM usina u, empre e " &
                             "WHERE (e.codarea = 'RE' " &
                             "OR e.codarea = 'IB' " &
                             "OR e.codarea = 'CD' " &
                             "OR e.codarea = 'CM' " &
                             "OR e.codarea = 'SP' " &
                             "OR e.codarea = 'EP' " &
                             "OR e.codarea = 'GT' " &
                             "OR e.codarea = 'CS' " &
                             "OR e.codarea = 'GP') " &
                             "AND e.codempre = u.codempre) " &
                             "AND e.codempre = u.codempre " &
                             "AND u.codusina <> 'FUPTER' " &
                             "AND u.codusina <> 'FUPHID' " &
                             "AND u.codusina <> 'CBPTER' " &
                             "AND u.codusina <> 'CTPTER' " &
                             "AND u.codusina <> 'LGPHID' " &
                             "AND u.codusina <> 'LGPTER' " &
                             "AND u.codusina = d.codusina " &
                             "AND d.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                             "GROUP BY 1, 2, 3, 5, 6 " &
                             "ORDER BY 3, 5, 1, 2"
                    strTable = "despa"
                Case Is = "GerHidroNE"
                    strSql = "SELECT d.codusina[1,2] As codempre, d.codusina[3,10] As codusina, d.intdespa, d.valdespasup As valdespaemp, Nvl(u.ordem,0) As ordem " &
                             "FROM empre e, usina u, despa d " &
                             "WHERE e.codarea = 'NE' " &
                             "AND e.codempre = u.codempre " &
                             "AND u.codusina <> 'CHPHID' " &
                             "AND u.codusina <> 'CHUFL' " &
                             "AND u.codusina <> 'CHUPE' " &
                             "AND u.tipusina = 'H' " &
                             "AND u.codusina = d.codusina " &
                             "AND d.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                             "UNION ALL " &
                             "SELECT 'CH' AS codempre, 'UND' AS codusina, d.intdespa, SUM(d.valdespasup) AS valdespaemp, 999 AS ordem " &
                             "FROM usina u, despa d " &
                             "WHERE (u.codusina = 'CHPHID' " &
                             "OR u.codusina = 'CHUFL' " &
                             "OR u.codusina = 'CHUPE') " &
                             "AND u.codusina = d.codusina " &
                             "AND d.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                             "GROUP BY 3 " &
                             "ORDER BY 3, 5"
                    strTable = "despa"
                Case Is = "GerTermoNE"
                    'Após a eliminação das Usinas Emergenciais da base do RPDP
                    'essa query deverá ser modificada.
                    '-- IM46473 - Novo Reagrupamento de Usinas (Início)
                    '-- IM46473 - Etapa 1: Query original comentada
                    'strSql = "SELECT d.codusina[1,2] As codempre, d.codusina[3,10] As codusina, d.intdespa, d.valdespasup As valdespaemp, u.ordem " & _
                    '         "FROM empre e, usina u, despa d " & _
                    '         "WHERE e.codarea = 'NE' " & _
                    '         "AND e.codempre = u.codempre " & _
                    '         "AND u.tipusina = 'T' " & _
                    '         "AND u.codusina = d.codusina " & _
                    '         "AND d.datpdp = '" & Page.Request.QueryString("strData") & "' " & _
                    '         "AND d.codusina <> 'SYBEJD' " & _
                    '         "AND d.codusina <> 'SYBERL' " & _
                    '         "AND d.codusina <> 'TGGCCB' " & _
                    '         "AND d.codusina <> 'TGGCIP' " & _
                    '         "AND d.codusina <> 'TGGCPT' " & _
                    '         "AND d.codusina <> 'TGGCPZ' " & _
                    '         "AND d.codusina <> 'TGGCRF' " & _
                    '         "AND d.codusina <> 'TGGCSP' " & _
                    '         "AND d.codusina <> 'TETECP' " & _
                    '         "AND d.codusina <> 'TOTMCB' " & _
                    '         "ORDER BY 3, 5, 1, 2"

                    '-- IM46473 - Etapa 2: Query refeita com uso de Tabelas Temporárias
                    '-- BLUTA
                    '-- Elimina a tabela temporária com Geração da BLUTA
                    objCommand.CommandText = "DROP TABLE tmpGerBLUTA"
                    Try
                        objCommand.ExecuteNonQuery()
                    Catch ex As Exception
                        '-- Se a temporária não existe, ignora o erro
                    End Try
                    '-- Seleciona os valores de Geração da BLUTA
                    objCommand.CommandText = "SELECT SUM(valdespasup) AS valdespasup, intdespa " &
                                        "FROM despa " &
                                        "WHERE datpdp = '" & Page.Request.QueryString("strData") & "' " &
                                        "AND codusina in ('BLUTA','BLUTAT','BLUTAB','BLUTAA') " &
                                        "GROUP BY intdespa " &
                                        "INTO TEMP tmpGerBLUTA " &
                                        "WITH NO LOG"
                    objCommand.ExecuteNonQuery()
                    '-- BLUTFA
                    '-- Elimina a tabela temporária com Geração da BLUTFA
                    objCommand.CommandText = "DROP TABLE tmpGerBLUTFA"
                    Try
                        objCommand.ExecuteNonQuery()
                    Catch ex As Exception
                        '-- Se a temporária não existe, ignora o erro
                    End Try
                    '-- Seleciona os valores de Geração da BLUTFA
                    objCommand.CommandText = "SELECT SUM(valdespasup) AS valdespasup, intdespa " &
                                        "FROM despa " &
                                        "WHERE datpdp = '" & Page.Request.QueryString("strData") & "' " &
                                        "AND codusina in ('BLUTFA','BLTFAT','BLTFAB','BLTFAA') " &
                                        "GROUP BY intdespa " &
                                        "INTO TEMP tmpGerBLUTFA " &
                                        "WITH NO LOG"
                    objCommand.ExecuteNonQuery()
                    '-- BLUSCJ
                    '-- Elimina a tabela temporária com Geração da BLUSCJ
                    objCommand.CommandText = "DROP TABLE tmpGerBLUSCJ"
                    Try
                        objCommand.ExecuteNonQuery()
                    Catch ex As Exception
                        '-- Se a temporária não existe, ignora o erro
                    End Try
                    '-- Seleciona os valores de Geração da BLUSCJ
                    objCommand.CommandText = "SELECT SUM(valdespasup) AS valdespasup, intdespa " &
                                        "FROM despa " &
                                        "WHERE datpdp = '" & Page.Request.QueryString("strData") & "' " &
                                        "AND codusina in ('BLUSCJ','BLUCJT','BLUCJB','BLUCJA') " &
                                        "GROUP BY intdespa " &
                                        "INTO TEMP tmpGerBLUSCJ " &
                                        "WITH NO LOG"
                    objCommand.ExecuteNonQuery()
                    '-- BLUTBC
                    '-- Elimina a tabela temporária com Geração da BLUTBC
                    objCommand.CommandText = "DROP TABLE tmpGerBLUTBC"
                    Try
                        objCommand.ExecuteNonQuery()
                    Catch ex As Exception
                        '-- Se a temporária não existe, ignora o erro
                    End Try
                    '-- Seleciona os valores de Geração da BLUTBC
                    objCommand.CommandText = "SELECT SUM(valdespasup) AS valdespasup, intdespa " &
                                        "FROM despa " &
                                        "WHERE datpdp = '" & Page.Request.QueryString("strData") & "' " &
                                        "AND codusina in ('BLUTBC','BLTBCT','BLTBCB','BLTBCA') " &
                                        "GROUP BY intdespa " &
                                        "INTO TEMP tmpGerBLUTBC " &
                                        "WITH NO LOG"
                    objCommand.ExecuteNonQuery()
                    '-- BLTPRD
                    '-- Elimina a tabela temporária com Geração da BLTPRD
                    objCommand.CommandText = "DROP TABLE tmpGerBLTPRD"
                    Try
                        objCommand.ExecuteNonQuery()
                    Catch ex As Exception
                        '-- Se a temporária não existe, ignora o erro
                    End Try
                    '-- Seleciona os valores de Geração da BLTPRD
                    objCommand.CommandText = "SELECT SUM(valdespasup) AS valdespasup, intdespa " &
                                        "FROM despa " &
                                        "WHERE datpdp = '" & Page.Request.QueryString("strData") & "' " &
                                        "AND codusina in ('BLTPRD','BLTRDT','BLTRDB','BLTRDA') " &
                                        "GROUP BY intdespa " &
                                        "INTO TEMP tmpGerBLTPRD " &
                                        "WITH NO LOG"
                    objCommand.ExecuteNonQuery()
                    '-- BLUJSL
                    '-- Elimina a tabela temporária com Geração da BLUJSL
                    objCommand.CommandText = "DROP TABLE tmpGerBLUJSL"
                    Try
                        objCommand.ExecuteNonQuery()
                    Catch ex As Exception
                        '-- Se a temporária não existe, ignora o erro
                    End Try
                    '-- Seleciona os valores de Geração da BLUJSL
                    objCommand.CommandText = "SELECT SUM(valdespasup) AS valdespasup, intdespa " &
                                        "FROM despa " &
                                        "WHERE datpdp = '" & Page.Request.QueryString("strData") & "' " &
                                        "AND codusina in ('BLUJSL','BLUJST','BLUJSB','BLUJSA') " &
                                        "GROUP BY intdespa " &
                                        "INTO TEMP tmpGerBLUJSL " &
                                        "WITH NO LOG"
                    objCommand.ExecuteNonQuery()
                    '-- PTCEPT
                    '-- Elimina a tabela temporária com Geração da PTCEPT
                    objCommand.CommandText = "DROP TABLE tmpGerPTCEPT"
                    Try
                        objCommand.ExecuteNonQuery()
                    Catch ex As Exception
                        '-- Se a temporária não existe, ignora o erro
                    End Try
                    '-- Seleciona os valores de Geração da PTCEPT
                    objCommand.CommandText = "SELECT SUM(valdespasup) AS valdespasup, intdespa " &
                                        "FROM despa " &
                                        "WHERE datpdp = '" & Page.Request.QueryString("strData") & "' " &
                                        "AND codusina in ('PTCEPT','PTCPTT','PTCPTB','PTCPTA') " &
                                        "GROUP BY intdespa " &
                                        "INTO TEMP tmpGerPTCEPT " &
                                        "WITH NO LOG"
                    objCommand.ExecuteNonQuery()
                    '-- Monta Seleção com todas as usinas mais as temporárias acima


                    strSql = "SELECT d.codusina[1,2] As codempre, d.codusina[3,10] As codusina, d.intdespa, d.valdespasup As valdespaemp, NVL(u.ordem,1) AS ordem " &
                             "FROM empre e, usina u, despa d " &
                             "WHERE e.codarea = 'NE' " &
                             "AND e.codempre = u.codempre " &
                             "AND u.tipusina = 'T' " &
                             "AND u.codusina = d.codusina " &
                             "AND d.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                             "AND d.codusina <> 'SYBEJD' " &
                             "AND d.codusina <> 'SYBERL' " &
                             "AND d.codusina <> 'TGGCCB' " &
                             "AND d.codusina <> 'TGGCIP' " &
                             "AND d.codusina <> 'TGGCPT' " &
                             "AND d.codusina <> 'TGGCPZ' " &
                             "AND d.codusina <> 'TGGCRF' " &
                             "AND d.codusina <> 'TGGCSP' " &
                             "AND d.codusina <> 'TETECP' " &
                             "AND d.codusina <> 'TOTMCB' " &
                             "AND d.codusina <> 'BLUTA' " &
                             "AND d.codusina <> 'BLUTAT' " &
                             "AND d.codusina <> 'BLUTAB' " &
                             "AND d.codusina <> 'BLUTAA' " &
                             "AND d.codusina <> 'BLUTFA' " &
                             "AND d.codusina <> 'BLTFAT' " &
                             "AND d.codusina <> 'BLTFAB' " &
                             "AND d.codusina <> 'BLTFAA' " &
                             "AND d.codusina <> 'BLUSCJ' " &
                             "AND d.codusina <> 'BLUCJT' " &
                             "AND d.codusina <> 'BLUCJB' " &
                             "AND d.codusina <> 'BLUCJA' " &
                             "AND d.codusina <> 'BLUTBC' " &
                             "AND d.codusina <> 'BLTBCT' " &
                             "AND d.codusina <> 'BLTBCB' " &
                             "AND d.codusina <> 'BLTBCA' " &
                             "AND d.codusina <> 'BLTPRD' " &
                             "AND d.codusina <> 'BLTRDT' " &
                             "AND d.codusina <> 'BLTRDB' " &
                             "AND d.codusina <> 'BLTRDA' " &
                             "AND d.codusina <> 'BLUJSL' " &
                             "AND d.codusina <> 'BLUJST' " &
                             "AND d.codusina <> 'BLUJSB' " &
                             "AND d.codusina <> 'BLUJSA' " &
                             "AND d.codusina <> 'PTCEPT' " &
                             "AND d.codusina <> 'PTCPTT' " &
                             "AND d.codusina <> 'PTCPTB' " &
                             "AND d.codusina <> 'PTCPTA' " &
                             "UNION " &
                             "SELECT u.codusina[1,2] AS codempre, d.codusina[3,10] AS codusina, d.intdespa, NVL(t.valdespasup,0) AS valdespaemp, NVL(u.ordem,1) AS ordem " &
                             "FROM despa d, usina u, tmpGerBLUTA t " &
                             "WHERE d.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                             "AND d.codusina = u.codusina " &
                             "AND u.codusina = 'BLUTA' " &
                             "AND d.intdespa = t.intdespa " &
                             "UNION " &
                             "SELECT u.codusina[1,2] AS codempre, d.codusina[3,10] AS codusina, d.intdespa, NVL(t.valdespasup,0) AS valdespaemp, NVL(u.ordem,1) AS ordem " &
                             "FROM despa d, usina u, tmpGerBLUTFA t " &
                             "WHERE d.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                             "AND d.codusina = u.codusina " &
                             "AND u.codusina = 'BLUTFA' " &
                             "AND d.intdespa = t.intdespa " &
                             "UNION " &
                             "SELECT u.codusina[1,2] AS codempre, d.codusina[3,10] AS codusina, d.intdespa, NVL(t.valdespasup,0) AS valdespaemp, NVL(u.ordem,1) AS ordem " &
                             "FROM despa d, usina u, tmpGerBLUSCJ t " &
                             "WHERE d.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                             "AND d.codusina = u.codusina " &
                             "AND u.codusina = 'BLUSCJ' " &
                             "AND d.intdespa = t.intdespa " &
                             "UNION " &
                             "SELECT u.codusina[1,2] AS codempre, d.codusina[3,10] AS codusina, d.intdespa, NVL(t.valdespasup,0) AS valdespaemp, NVL(u.ordem,1) AS ordem " &
                             "FROM despa d, usina u, tmpGerBLUTBC t " &
                             "WHERE d.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                             "AND d.codusina = u.codusina " &
                             "AND u.codusina = 'BLUTBC' " &
                             "AND d.intdespa = t.intdespa " &
                             "UNION " &
                             "SELECT u.codusina[1,2] AS codempre, d.codusina[3,10] AS codusina, d.intdespa, NVL(t.valdespasup,0) AS valdespaemp, NVL(u.ordem,1) AS ordem " &
                             "FROM despa d, usina u, tmpGerBLTPRD t " &
                             "WHERE d.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                             "AND d.codusina = u.codusina " &
                             "AND u.codusina = 'BLTPRD' " &
                             "AND d.intdespa = t.intdespa " &
                             "UNION " &
                             "SELECT u.codusina[1,2] AS codempre, d.codusina[3,10] AS codusina, d.intdespa, NVL(t.valdespasup,0) AS valdespaemp, NVL(u.ordem,1) AS ordem " &
                             "FROM despa d, usina u, tmpGerBLUJSL t " &
                             "WHERE d.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                             "AND d.codusina = u.codusina " &
                             "AND u.codusina = 'BLUJSL' " &
                             "AND d.intdespa = t.intdespa " &
                             "UNION " &
                             "SELECT u.codusina[1,2] AS codempre, d.codusina[3,10] AS codusina, d.intdespa, NVL(t.valdespasup,0) AS valdespaemp, NVL(u.ordem,1) AS ordem " &
                             "FROM despa d, usina u, tmpGerPTCEPT t " &
                             "WHERE d.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                             "AND d.codusina = u.codusina " &
                             "AND u.codusina = 'PTCEPT' " &
                             "AND d.intdespa = t.intdespa " &
                             "ORDER BY 3, 5, 1, 2"
                    '-- IM46473 - Novo Reagrupamento de Usinas (Fim)


                    'strSql = "SELECT d.codusina[1,2] As codempre, d.codusina[3,10] As codusina, d.intdespa, d.valdespasup As valdespaemp, 1 As ordem " & _
                    '         "FROM empre e, usina u, despa d " & _
                    '         "WHERE e.codarea = 'NE' " & _
                    '         "AND e.codempre = u.codempre " & _
                    '         "AND u.tipusina = 'T' " & _
                    '         "AND u.codusina = d.codusina " & _
                    '         "AND d.datpdp = '" & Page.Request.QueryString("strData") & "' " & _
                    '         "ORDER BY d.intdespa, d.codusina"
                    strTable = "despa"

                Case Is = "GerHidroNCO"
                    'Coluna separada para as PCH's + CURUA-UNA + JUBA I + JUBA II, 
                    'Coluna Itiquira I e II somadas 
                    'Coluna Cachoeira Dourada 138 e 230 somadas
                    strSql = "SELECT d.codusina[1,2] As codempre, d.codusina[3,10] As codusina, d.intdespa, d.valdespasup As valdespaemp, Nvl(u.ordem,0) As ordem " &
                             "FROM empre e, usina u, despa d " &
                             "WHERE e.codarea = 'RN' " &
                             "AND e.codempre <> 'IQ' " &
                             "AND e.codempre <> 'CD' " &
                             "AND e.codempre = u.codempre " &
                             "AND u.tipusina = 'H' " &
                             "AND u.codusina <> 'ENCRU' " &
                             "AND u.codusina <> 'CTJBD' " &
                             "AND u.codusina <> 'CTJBU' " &
                             "AND u.codusina <> 'CTPCH1' " &
                             "AND u.codusina <> 'CTPCH2' " &
                             "AND u.codusina <> 'CTPCH3' " &
                             "AND u.codusina <> 'CTPCH4' " &
                             "AND u.codusina <> 'CTPCH5' " &
                             "AND u.codusina <> 'CTPCH6' " &
                             "AND u.codusina <> 'CTPCH7' " &
                             "AND u.codusina <> 'CTPCH8' " &
                             "AND u.codusina <> 'CTPCH9' " &
                             "AND u.codusina <> 'CTPH10' " &
                             "AND u.codusina <> 'CBPHID' " &
                             "AND u.codusina <> 'CGPHID' " &
                             "AND u.codusina <> 'CTPHID' " &
                             "AND u.codusina <> 'ENPHID' " &
                             "AND u.codusina = d.codusina " &
                             "AND d.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                             "UNION ALL " &
                             "SELECT 'IQ' As codempre, 'UIQR' As codusina, d.intdespa, SUM(d.valdespasup) As valdespaemp, NVL(u.ordem, 0) As ordem " &
                             "FROM empre e, usina u, despa d " &
                             "WHERE e.codarea = 'RN' " &
                             "AND e.codempre = 'IQ' " &
                             "AND e.codempre = u.codempre " &
                             "AND u.tipusina = 'H' " &
                             "AND u.codusina = d.codusina " &
                             "AND d.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                             "GROUP BY 1, 2, 3, 5 " &
                             "UNION ALL " &
                             "SELECT 'CD' As codempre, 'CDOU' As codusina, d.intdespa, SUM(d.valdespasup) As valdespaemp, NVL(u.ordem, 0) As ordem " &
                             "FROM empre e, usina u, despa d " &
                             "WHERE e.codarea = 'RN' " &
                             "AND e.codempre = 'CD' " &
                             "AND e.codempre = u.codempre " &
                             "AND u.tipusina = 'H' " &
                             "AND u.codusina = d.codusina " &
                             "AND d.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                             "GROUP BY 1, 2, 3, 5 " &
                             "UNION ALL " &
                             "SELECT 'N/CO' As codempre, 'PHID' As codusina, d.intdespa, SUM(d.valdespasup) As valdespaemp, 998 As ordem " &
                             "FROM empre e, usina u, despa d " &
                             "WHERE e.codarea = 'RN' " &
                             "AND e.codempre = u.codempre " &
                             "AND u.tipusina = 'H' " &
                             "AND (u.codusina = 'ENCRU' " &
                             "OR u.codusina = 'CTJBD' " &
                             "OR u.codusina = 'CTJBU' " &
                             "OR u.codusina = 'CTPCH1' " &
                             "OR u.codusina = 'CTPCH2' " &
                             "OR u.codusina = 'CTPCH3' " &
                             "OR u.codusina = 'CTPCH4' " &
                             "OR u.codusina = 'CTPCH5' " &
                             "OR u.codusina = 'CTPCH6' " &
                             "OR u.codusina = 'CTPCH7' " &
                             "OR u.codusina = 'CTPCH8' " &
                             "OR u.codusina = 'CTPCH9' " &
                             "OR u.codusina = 'CTPH10' " &
                             "OR u.codusina = 'CBPHID' " &
                             "OR u.codusina = 'CGPHID' " &
                             "OR u.codusina = 'CTPHID' " &
                             "OR u.codusina = 'ENPHID') " &
                             "AND u.codusina = d.codusina " &
                             "AND d.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                             "GROUP BY 1, 2, 3, 5 " &
                             "UNION ALL " &
                             "SELECT 'HIDRO' As codempre, 'TOTAL' As codusina, d.intdespa, SUM(d.valdespasup) As valdespaemp, 999 As ordem " &
                             "FROM empre e, usina u, despa d " &
                             "WHERE e.codarea = 'RN' " &
                             "AND e.codempre = u.codempre " &
                             "AND u.tipusina = 'H' " &
                             "AND u.codusina = d.codusina " &
                             "AND d.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                             "GROUP BY 1, 2, 3, 5 " &
                             "ORDER BY 3, 5, 1, 2"

                    strTable = "despa"

                Case Is = "GerTermoNCO"

                    strSql = "SELECT d.codusina[1,2] As codempre, d.codusina[3,10] As codusina, d.intdespa, d.valdespasup As valdespaemp, Nvl(u.ordem,0) As ordem " &
                             "FROM empre e, usina u, despa d " &
                             "WHERE e.codarea = 'RN' " &
                             "AND e.codempre = u.codempre " &
                             "AND u.tipusina = 'T' " &
                             "AND u.codusina = d.codusina " &
                             "AND d.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                             "UNION ALL " &
                             "SELECT 'TERMO' As codempre, 'TOTAL' As codusina, d.intdespa, SUM(d.valdespasup) As valdespaemp, 999 As ordem " &
                             "FROM empre e, usina u, despa d " &
                             "WHERE e.codarea = 'RN' " &
                             "AND e.codempre = u.codempre " &
                             "AND u.tipusina = 'T' " &
                             "AND u.codusina = d.codusina " &
                             "AND d.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                             "GROUP BY 1, 2, 3, 5 " &
                             "ORDER BY 3, 5, 1, 2"

                    strTable = "despa"

                Case Is = "GeracaoCargaS"
                    strSql = "SELECT d.codusina[1,2] As codempre, d.codusina[3,10] AS codusina, d.intdespa, d.valdespasup AS valdespaemp, u.ordem " &
                            "FROM despa d, usina u " &
                            "WHERE u.codempre = 'CE' " &
                            "AND u.codusina = d.codusina " &
                            "AND d.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                            "AND u.codusina <> 'CEPTER' " &
                            "UNION ALL " &
                            "SELECT 'RGS' AS codempre, 'CARGA' AS codusina, intcarga AS intdespa, SUM(valcargasup) AS valdespaemp, 0 AS ordem " &
                            "FROM carga " &
                            "WHERE codempre = 'CE' " &
                            "AND datpdp = '" & Page.Request.QueryString("strData") & "' " &
                            "GROUP BY 1, 2, 3, 5 " &
                            "UNION ALL " &
                            "SELECT 'HIDRO' As codempre, 'TOTAL' AS codusina, d.intdespa, SUM(d.valdespasup) AS valdespaemp, 98 AS ordem " &
                            "FROM despa d, usina u " &
                            "WHERE u.codempre = 'CE' " &
                            "AND u.codusina = d.codusina " &
                            "AND u.tipusina = 'H' " &
                            "AND d.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                            "GROUP BY 1, 2, 3, 5 " &
                            "UNION ALL " &
                            "SELECT 'GERAL' As codempre, 'TOTAL' AS codusina, d.intdespa, SUM(d.valdespasup) AS valdespaemp, 99 AS ordem " &
                            "FROM despa d, usina u " &
                            "WHERE u.codempre = 'CE' " &
                            "AND u.codusina = d.codusina " &
                            "AND d.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                            "GROUP BY 1, 2, 3, 5 " &
                            "ORDER BY 3, 5, 2"
                    'strSql = "SELECT codusina[1,2] As codempre, codusina[3,10] AS codusina, intdespa, valdespasup AS valdespaemp, 1 AS ordem " & _
                    '         "FROM despa " & _
                    '         "WHERE codusina IN " & _
                    '         "(SELECT u.codusina " & _
                    '         "FROM despa d, usina u " & _
                    '         "WHERE u.codempre = 'CE' " & _
                    '         "AND u.codusina = d.codusina " & _
                    '         "AND d.datpdp = '" & Page.Request.QueryString("strData") & "' " & _
                    '         "GROUP BY 1 " & _
                    '         "HAVING SUM(ABS(d.valdespasup)) > 0) " & _
                    '         "AND datpdp = '" & Page.Request.QueryString("strData") & "' " & _
                    '         "UNION ALL " & _
                    '         "SELECT 'RGS' AS codempre, 'CARGA' AS codusina, intcarga AS intdespa, SUM(valcargasup) AS valdespaemp, 2 AS ordem " & _
                    '         "FROM carga " & _
                    '         "WHERE codempre = 'CE' " & _
                    '         "AND datpdp = '" & Page.Request.QueryString("strData") & "' " & _
                    '         "GROUP BY 1, 2, 3, 5 " & _
                    '         "ORDER BY 3, 5, 2 DESC"
                    strTable = "despa"

                    '-- MarcosA 2009-09-14 IM50174 - troca da ordem 4 por 64 e da ordem 8 por 68 na seleção abaixo (totais das usinas)
                Case Is = "GeracaoCargaCOPEL"
                    strSql = "SELECT d.codusina[1,2] As codempre, d.codusina[3,10] AS codusina, d.intdespa, d.valdespasup AS valdespaemp, u.ordem  " &
                            "FROM despa d, usina u " &
                            "WHERE u.codempre = 'CO' " &
                            "AND u.codusina = d.codusina " &
                            "AND d.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                            "AND d.codusina <> 'COGBM' " &
                            "AND d.codusina <> 'COPTER' " &
                            "AND d.codusina <> 'COSGD' " &
                            "UNION ALL " &
                            "SELECT 'CO' As codempre, 'TOT GBM' AS codusina, intdespa, SUM(valdespasup) AS valdespaemp, 64 AS ordem  " &
                            "FROM despa " &
                            "WHERE (codusina = 'COGBM1' " &
                            "OR codusina = 'COGBM2' " &
                            "OR codusina = 'COGBM3' " &
                            "OR codusina = 'COGBM4') " &
                            "AND datpdp = '" & Page.Request.QueryString("strData") & "' " &
                            "GROUP BY 1, 2, 3, 5 " &
                            "UNION ALL " &
                            "SELECT 'CO' As codempre, 'TOT SGD' AS codusina, intdespa, SUM(valdespasup) AS valdespaemp, 68 AS ordem  " &
                            "FROM despa " &
                            "WHERE (codusina = 'COSGD1' " &
                            "OR codusina = 'COSGD2' " &
                            "OR codusina = 'COSGD3' " &
                            "OR codusina = 'COSGD4') " &
                            "AND datpdp = '" & Page.Request.QueryString("strData") & "' " &
                            "GROUP BY 1, 2, 3, 5 " &
                            "UNION ALL " &
                            "SELECT 'HIDRO' As codempre, 'TOTAL' AS codusina, d.intdespa, SUM(d.valdespasup) AS valdespaemp, 98 AS ordem  " &
                            "FROM despa d, usina u " &
                            "WHERE u.codempre = 'CO' " &
                            "AND u.tipusina = 'H' " &
                            "AND u.codusina = d.codusina " &
                            "AND d.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                            "GROUP BY 1, 2, 3, 5 " &
                            "UNION ALL " &
                            "SELECT 'GERAL' As codempre, 'TOTAL' AS codusina, d.intdespa, SUM(d.valdespasup) AS valdespaemp, 99 AS ordem  " &
                            "FROM despa d, usina u " &
                            "WHERE u.codempre = 'CO' " &
                            "AND u.codusina = d.codusina " &
                            "AND d.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                            "GROUP BY 1, 2, 3, 5 " &
                            "ORDER BY 3, 5, 2"

                    'strSql = "SELECT 'COPEL' AS codempre, 'CARGA' AS codusina, intcarga AS intdespa, SUM(valcargasup) AS valdespaemp, 0 AS ordem " & _
                    '         "FROM carga " & _
                    '         "WHERE codempre = 'CO' " & _
                    '         "AND datpdp = '" & Page.Request.QueryString("strData") & "' " & _
                    '         "GROUP BY 1, 2, 3, 5 " & _
                    '         "UNION ALL " & _
                    '         "SELECT d.codusina[1,2] As codempre, d.codusina[3,10] AS codusina, d.intdespa, d.valdespasup AS valdespaemp, u.ordem  " & _
                    '         "FROM despa d, usina u " & _
                    '         "WHERE u.codempre = 'CO' " & _
                    '         "AND u.codusina = d.codusina " & _
                    '         "AND d.datpdp = '" & Page.Request.QueryString("strData") & "' " & _
                    '         "AND d.codusina <> 'COGBM' " & _
                    '         "AND d.codusina <> 'COPTER' " & _
                    '         "AND d.codusina <> 'COSGD' " & _
                    '         "UNION ALL " & _
                    '         "SELECT 'CO' As codempre, 'TOT GBM' AS codusina, intdespa, SUM(valdespasup) AS valdespaemp, 4 AS ordem  " & _
                    '         "FROM despa " & _
                    '         "WHERE (codusina = 'COGBM1' " & _
                    '         "OR codusina = 'COGBM2' " & _
                    '         "OR codusina = 'COGBM3' " & _
                    '         "OR codusina = 'COGBM4') " & _
                    '         "AND datpdp = '" & Page.Request.QueryString("strData") & "' " & _
                    '         "GROUP BY 1, 2, 3, 5 " & _
                    '         "UNION ALL " & _
                    '         "SELECT 'CO' As codempre, 'TOT SGD' AS codusina, intdespa, SUM(valdespasup) AS valdespaemp, 8 AS ordem  " & _
                    '         "FROM despa " & _
                    '         "WHERE (codusina = 'COSGD1' " & _
                    '         "OR codusina = 'COSGD2' " & _
                    '         "OR codusina = 'COSGD3' " & _
                    '         "OR codusina = 'COSGD4') " & _
                    '         "AND datpdp = '" & Page.Request.QueryString("strData") & "' " & _
                    '         "GROUP BY 1, 2, 3, 5 " & _
                    '         "UNION ALL " & _
                    '         "SELECT 'HIDRO' As codempre, 'TOTAL' AS codusina, d.intdespa, SUM(d.valdespasup) AS valdespaemp, 98 AS ordem  " & _
                    '         "FROM despa d, usina u " & _
                    '         "WHERE u.codempre = 'CO' " & _
                    '         "AND u.tipusina = 'H' " & _
                    '         "AND u.codusina = d.codusina " & _
                    '         "AND d.datpdp = '" & Page.Request.QueryString("strData") & "' " & _
                    '         "GROUP BY 1, 2, 3, 5 " & _
                    '         "UNION ALL " & _
                    '         "SELECT 'GERAL' As codempre, 'TOTAL' AS codusina, d.intdespa, SUM(d.valdespasup) AS valdespaemp, 99 AS ordem  " & _
                    '         "FROM despa d, usina u " & _
                    '         "WHERE u.codempre = 'CO' " & _
                    '         "AND u.codusina = d.codusina " & _
                    '         "AND d.datpdp = '" & Page.Request.QueryString("strData") & "' " & _
                    '         "GROUP BY 1, 2, 3, 5 " & _
                    '         "ORDER BY 3, 5, 2"
                    'strSql = "SELECT codusina[1,2] As codempre, codusina[3,10] AS codusina, intdespa, valdespasup AS valdespaemp, 1 AS ordem " & _
                    '         "FROM despa " & _
                    '         "WHERE codusina IN " & _
                    '         "(SELECT u.codusina " & _
                    '         "FROM despa d, usina u " & _
                    '         "WHERE u.codempre = 'CO' " & _
                    '         "AND u.codusina = d.codusina " & _
                    '         "AND d.datpdp = '" & Page.Request.QueryString("strData") & "' " & _
                    '         "GROUP BY 1 " & _
                    '         "HAVING SUM(ABS(d.valdespasup)) > 0) " & _
                    '         "AND datpdp = '" & Page.Request.QueryString("strData") & "' " & _
                    '         "UNION ALL " & _
                    '         "SELECT 'COPEL' AS codempre, 'CARGA' AS codusina, intcarga AS intdespa, SUM(valcargasup) AS valdespaemp, 2 AS ordem " & _
                    '         "FROM carga " & _
                    '         "WHERE codempre = 'CO' " & _
                    '         "AND datpdp = '" & Page.Request.QueryString("strData") & "' " & _
                    '         "GROUP BY 1, 2, 3, 5 " & _
                    '         "ORDER BY 3, 5, 2 DESC"
                    strTable = "despa"
                Case Is = "Inter"
                    'Intercâmbio Programado e Reprogramado Reg. SUL
                    CarregaDSInter()
                    Exit Sub
                Case Is = "CargaInter"
                    'Cargas e Intercâmbios Reg. SUL
                    CarregaDSCargaInter()
                    Exit Sub
                Case Is = "InflexTotal"
                    'Cargas e Intercâmbios Reg. SUL
                    strSql = "SELECT ifx.intflexi AS intervalo, SUM(NVL(ifx.valflexisup,0)) AS valIFX, " &
                            "SUM(NVL(zel.valrazeletsup,0)) AS valZEL, SUM(NVL(zen.valrazenersup,0)) AS valZEN, " &
                            "SUM(NVL(exp.valexportasup,0)) AS valEXP, SUM(NVL(imp.valimportasup,0)) AS valIMP, " &
                            "SUM(NVL(pcc.valpccsup,0)) AS valPCC, SUM(NVL(erp.valerpsup,0)) AS valERP, " &
                            "SUM(NVL(clf.valclfsup,0)) AS valCLF " &
                            "FROM empre e, usina u, inflexibilidade ifx, OUTER razaoelet zel, OUTER razaoener zen, " &
                            "OUTER exporta exp, OUTER importa imp, OUTER perdascic pcc, OUTER energia_reposicao erp, " &
                            "OUTER compensacaolastro_fisico clf " &
                            "WHERE e.codarea = 'RS' " &
                            "AND e.codempre = u.codempre " &
                            "AND u.codusina <> 'EFEVSO' " &
                            "AND u.codusina <> 'EFEVSS' " &
                            "AND u.codusina <> 'EFEVSI' " &
                            "AND u.codusina = ifx.codusina " &
                            "AND ifx.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                            "AND u.codusina = zel.codusina " &
                            "AND ifx.datpdp = zel.datpdp " &
                            "AND ifx.intflexi = zel.intrazelet " &
                            "AND u.codusina = zen.codusina " &
                            "AND ifx.datpdp = zen.datpdp " &
                            "AND ifx.intflexi = zen.intrazener " &
                            "AND u.codusina = exp.codusina " &
                            "AND ifx.datpdp = exp.datpdp " &
                            "AND ifx.intflexi = exp.intexporta " &
                            "AND exp.codusina <> 'ESRIV' " &
                            "AND exp.codusina <> 'IEGBI2' " &
                            "AND u.codusina = imp.codusina " &
                            "AND ifx.datpdp = imp.datpdp " &
                            "AND ifx.intflexi = imp.intimporta " &
                            "AND u.codusina = pcc.codusina " &
                            "AND ifx.datpdp = pcc.datpdp " &
                            "AND ifx.intflexi = pcc.intpcc " &
                            "AND u.codusina = erp.codusina " &
                            "AND ifx.datpdp = erp.datpdp " &
                            "AND ifx.intflexi = erp.interp " &
                            "AND u.codusina = clf.codusina " &
                            "AND ifx.datpdp = clf.datpdp " &
                            "AND ifx.intflexi = clf.intclf " &
                            "GROUP BY ifx.intflexi " &
                            "ORDER BY ifx.intflexi"
                    strTable = "InflexTotalS"
                Case Is = "InflexUsina"
                    'Inflexibilidade por Usina Regional SUL
                    '-- MarcosA 2009-09-11 IM50174 - Retirada da seleção abaixo a restrição u.codusina <> 'ERPTER'
                    strSql = "SELECT u.codusina[3,12], 'IFX' AS grandeza, ifx.intflexi AS intervalo, SUM(ifx.valflexisup) AS valor, ordem " &
                            "FROM empre e, usina u, inflexibilidade ifx " &
                            "WHERE e.codarea = 'RS' " &
                            "AND e.codempre = u.codempre " &
                            "AND u.tipusina = 'T' " &
                            "AND u.codusina <> 'TSPTER' " &
                            "AND u.codusina <> 'TSUTWE' " &
                            "AND u.codusina <> 'SCPTER' " &
                            "AND u.codusina <> 'CEPTER' " &
                            "AND u.codusina <> 'EFEVSO' " &
                            "AND u.codusina <> 'EFEVSS' " &
                            "AND u.codusina <> 'EFEVSI' " &
                            "AND u.codusina = ifx.codusina " &
                            "AND ifx.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                            "GROUP BY 3, 1, 5 " &
                            "HAVING(SUM(ifx.valflexisup) <> 0) " &
                            "UNION " &
                            "SELECT u.codusina[3,12], 'ZEL' AS grandeza, zel.intrazelet AS intervalo, SUM(zel.valrazeletsup) AS valor, ordem " &
                            "FROM empre e, usina u, razaoelet zel " &
                            "WHERE e.codarea = 'RS' " &
                            "AND e.codempre = u.codempre " &
                            "AND u.tipusina = 'T' " &
                            "AND u.codusina <> 'TSPTER' " &
                            "AND u.codusina <> 'TSUTWE' " &
                            "AND u.codusina <> 'SCPTER' " &
                            "AND u.codusina <> 'CEPTER' " &
                            "AND u.codusina <> 'EFEVSO' " &
                            "AND u.codusina <> 'EFEVSS' " &
                            "AND u.codusina <> 'EFEVSI' " &
                            "AND u.codusina = zel.codusina " &
                            "AND zel.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                            "GROUP BY 3, 1, 5 " &
                            "HAVING SUM(zel.valrazeletsup) <> 0 " &
                            "UNION " &
                            "SELECT u.codusina[3,12], 'ZEN' AS grandeza, zen.intrazener AS intervalo, SUM(zen.valrazenersup) AS valor, ordem " &
                            "FROM empre e, usina u, razaoener zen " &
                            "WHERE e.codarea = 'RS' " &
                            "AND e.codempre = u.codempre " &
                            "AND u.tipusina = 'T' " &
                            "AND u.codusina <> 'TSPTER' " &
                            "AND u.codusina <> 'TSUTWE' " &
                            "AND u.codusina <> 'SCPTER' " &
                            "AND u.codusina <> 'CEPTER' " &
                            "AND u.codusina <> 'EFEVSO' " &
                            "AND u.codusina <> 'EFEVSS' " &
                            "AND u.codusina <> 'EFEVSI' " &
                            "AND u.codusina = zen.codusina " &
                            "AND zen.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                            "GROUP BY 3, 1, 5 " &
                            "HAVING SUM(zen.valrazenersup) <> 0 " &
                            "UNION " &
                            "SELECT u.codusina[3,12], 'EXP' AS grandeza, exp.intexporta AS intervalo, SUM(exp.valexportasup) AS valor, ordem " &
                            "FROM empre e, usina u, exporta exp " &
                            "WHERE e.codarea = 'RS' " &
                            "AND e.codempre = u.codempre " &
                            "AND u.tipusina = 'T' " &
                            "AND u.codusina <> 'TSPTER' " &
                            "AND u.codusina <> 'TSUTWE' " &
                            "AND u.codusina <> 'SCPTER' " &
                            "AND u.codusina <> 'CEPTER' " &
                            "AND u.codusina <> 'EFEVSO' " &
                            "AND u.codusina <> 'EFEVSS' " &
                            "AND u.codusina <> 'EFEVSI' " &
                            "AND u.codusina = exp.codusina " &
                            "AND exp.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                            "GROUP BY 3, 1, 5 " &
                            "HAVING SUM(exp.valexportasup) <> 0 " &
                            "UNION " &
                            "SELECT u.codusina[3,12], 'GER' AS grandeza, ger.intdespa AS intervalo, SUM(ger.valdespasup) AS valor, ordem " &
                            "FROM empre e, usina u, despa ger " &
                            "WHERE e.codarea = 'RS' " &
                            "AND e.codempre = u.codempre " &
                            "AND u.tipusina = 'T' " &
                            "AND u.codusina <> 'TSPTER' " &
                            "AND u.codusina <> 'TSUTWE' " &
                            "AND u.codusina <> 'SCPTER' " &
                            "AND u.codusina <> 'CEPTER' " &
                            "AND u.codusina <> 'EFEVSO' " &
                            "AND u.codusina <> 'EFEVSS' " &
                            "AND u.codusina <> 'EFEVSI' " &
                            "AND u.codusina = ger.codusina " &
                            "AND ger.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                            "GROUP BY 3, 1, 5 " &
                            "UNION " &
                            "SELECT u.codusina[3,12], 'CLF' AS grandeza, clf.intclf AS intervalo, SUM(clf.valclfsup) AS valor, ordem " &
                            "FROM empre e, usina u, compensacaolastro_fisico clf " &
                            "WHERE e.codarea = 'RS' " &
                            "AND e.codempre = u.codempre " &
                            "AND u.tipusina = 'T' " &
                            "AND u.codusina <> 'TSPTER' " &
                            "AND u.codusina <> 'TSUTWE' " &
                            "AND u.codusina <> 'SCPTER' " &
                            "AND u.codusina <> 'CEPTER' " &
                            "AND u.codusina <> 'EFEVSO' " &
                            "AND u.codusina <> 'EFEVSS' " &
                            "AND u.codusina <> 'EFEVSI' " &
                            "AND u.codusina = clf.codusina " &
                            "AND clf.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                            "GROUP BY 3, 1, 5 " &
                            "HAVING SUM(clf.valclfsup) <> 0 " &
                            "UNION " &
                            "SELECT u.codusina[3,12], 'PCC' AS grandeza, pcc.intpcc AS intervalo, SUM(pcc.valpccsup) AS valor, ordem " &
                            "FROM empre e, usina u, perdascic pcc " &
                            "WHERE e.codarea = 'RS' " &
                            "AND e.codempre = u.codempre " &
                            "AND u.tipusina = 'T' " &
                            "AND u.codusina <> 'TSPTER' " &
                            "AND u.codusina <> 'TSUTWE' " &
                            "AND u.codusina <> 'SCPTER' " &
                            "AND u.codusina <> 'CEPTER' " &
                            "AND u.codusina <> 'EFEVSO' " &
                            "AND u.codusina <> 'EFEVSS' " &
                            "AND u.codusina <> 'EFEVSI' " &
                            "AND u.codusina = pcc.codusina " &
                            "AND pcc.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                            "GROUP BY 3, 1, 5 " &
                            "HAVING SUM(pcc.valpccsup) <> 0 " &
                            "UNION " &
                            "SELECT u.codusina[3,12], 'ERP' AS grandeza, erp.interp AS intervalo, SUM(erp.valerpsup) AS valor, ordem " &
                            "FROM empre e, usina u, energia_reposicao erp " &
                            "WHERE e.codarea = 'RS' " &
                            "AND e.codempre = u.codempre " &
                            "AND u.tipusina = 'T' " &
                            "AND u.codusina <> 'TSPTER' " &
                            "AND u.codusina <> 'TSUTWE' " &
                            "AND u.codusina <> 'SCPTER' " &
                            "AND u.codusina <> 'CEPTER' " &
                            "AND u.codusina <> 'EFEVSO' " &
                            "AND u.codusina <> 'EFEVSS' " &
                            "AND u.codusina <> 'EFEVSI' " &
                            "AND u.codusina = erp.codusina " &
                            "AND erp.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                            "GROUP BY 3, 1, 5 " &
                            "HAVING SUM(erp.valerpsup) <> 0 " &
                            "ORDER BY 3, 5, 2"
                    strTable = "InflexUsinaS"
                Case Is = "CargaSE"
                    strSql = "SELECT Trim(e.sigempre) AS codempre, c.intcarga, c.valcargasup " &
                            "FROM carga c, empre e " &
                            "WHERE e.codempre IN (" &
                            "SELECT c.codempre " &
                            "FROM carga c, empre e " &
                            "WHERE (e.codarea = 'RE' " &
                            "OR e.codarea = 'CM' " &
                            "OR e.codarea = 'SP' " &
                            "OR e.codarea = 'EP' " &
                            "OR e.codarea = 'GT' " &
                            "OR e.codarea = 'CS' " &
                            "OR e.codarea = 'GP') " &
                            "AND e.codempre = c.codempre " &
                            "AND c.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                            "GROUP BY 1 " &
                            "HAVING SUM(ABS(c.valcargasup)) > 0) " &
                            "AND (e.codarea = 'RE' " &
                            "OR e.codarea = 'CM' " &
                            "OR e.codarea = 'SP' " &
                            "OR e.codarea = 'EP' " &
                            "OR e.codarea = 'GT' " &
                            "OR e.codarea = 'CS' " &
                            "OR e.codarea = 'GP') " &
                            "AND e.codempre = c.codempre " &
                            "AND c.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                            "ORDER BY 2, 1"
                    strTable = "Carga"
                Case Is = "VazaoSE"
                    strSql = "SELECT e.sigempre, u.codusina AS sigusina, v.valturb, v.valvert " &
                            "FROM empre e, vazao v, usina u " &
                            "WHERE (e.codarea = 'RE' " &
                            "OR e.codarea = 'CM' " &
                            "OR e.codarea = 'SP' " &
                            "OR e.codarea = 'EP' " &
                            "OR e.codarea = 'GT' " &
                            "OR e.codarea = 'CS' " &
                            "OR e.codarea = 'GP') " &
                            "AND e.codempre = u.codempre " &
                            "AND u.codusina = v.codusina " &
                            "AND v.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                            "AND (v.valturb <> 0 " &
                            "OR v.valvert <> 0) " &
                            "ORDER BY e.codarea, e.sigempre, u.sigusina"

                    'strSql = "SELECT e.sigempre, u.codusina AS sigusina, v.valturb, v.valvert " & _
                    '         "FROM empre e, vazao v, usina u " & _
                    '         "WHERE (v.codusina = 'CMEMBO' " & _
                    '         "OR v.codusina = 'CMFUNI' " & _
                    '         "OR v.codusina = 'CMSSUS' " & _
                    '         "OR v.codusina = 'CMAIMO' " & _
                    '         "OR v.codusina = 'CMCPB1' " & _
                    '         "OR v.codusina IN " & _
                    '         "(SELECT u.codusina " & _
                    '         "FROM usina u, empre e " & _
                    '         "WHERE e.codarea = 'RE' " & _
                    '         "AND e.codempre = u.codempre)) " & _
                    '         "AND v.datpdp = '" & Page.Request.QueryString("strData") & "' " & _
                    '         "AND (v.valturb <> 0 " & _
                    '         "OR v.valvert <> 0) " & _
                    '         "AND v.codusina = u.codusina " & _
                    '         "AND u.codempre = e.codempre " & _
                    '         "ORDER BY e.sigempre, u.sigusina"
                    strTable = "Vazao"
                Case Is = "InterSE"
                    strSql = "SELECT intinter, (Trim(codemprede)||'-'||Trim(codemprepara)||'/'||Trim(codcontamodal)) AS codinter, valintersup, tipinter " &
                            "FROM inter " &
                            "WHERE (codemprede = 'RE' " &
                            "OR (codemprede = 'RS' " &
                            "AND codemprepara = 'ER') " &
                            "OR (codemprede = 'RN' " &
                            "AND codemprepara = 'NE')) " &
                            "AND datpdp = '" & Page.Request.QueryString("strData") & "' " &
                            "ORDER BY 1, 2"
                    strTable = "Intercambio"
                Case Is = "TransfSE"
                    'Temporária de Transferência de Energia entre Regiões
                    Try
                        objCommand.CommandText = "Drop Table tmpEnergiaSE"
                        objCommand.ExecuteNonQuery()
                    Catch ex As Exception
                        'Se ocorrer o erro, ignora
                    End Try
                    objCommand.CommandText = "SELECT intinter AS intervalo, SUM(valintersup) AS valor, 'RS' AS codempre " &
                                            "FROM inter " &
                                            "WHERE codemprede = 'RS' " &
                                            "AND codemprepara = 'RE' " &
                                            "AND (codcontamodal = 'IT' " &
                                            "OR codcontamodal = 'EF') " &
                                            "AND datpdp = '" & Page.Request.QueryString("strData") & "' " &
                                            "GROUP BY intinter " &
                                            "UNION " &
                                            "SELECT intinter AS intervalo, SUM(valintersup) AS valor, 'ER' AS codempre " &
                                            "FROM inter " &
                                            "WHERE codemprede = 'ER' " &
                                            "AND datpdp = '" & Page.Request.QueryString("strData") & "' " &
                                            "GROUP BY intinter " &
                                            "UNION " &
                                            "SELECT intdespa AS intervalo, SUM(valdespasup) AS valor, 'WA' AS codempre " &
                                            "FROM despa " &
                                            "WHERE (codusina = 'TSUTWA' " &
                                            "OR codusina = 'TSUTWE') " &
                                            "AND datpdp = '" & Page.Request.QueryString("strData") & "' " &
                                            "GROUP BY intdespa " &
                                            "ORDER BY 1, 3 " &
                                            "INTO TEMP tmpEnergiaSE " &
                                            "WITH NO LOG"
                    objCommand.ExecuteNonQuery()

                    strSql = "SELECT e1.intervalo, ((e1.valor + (-1 * e2.valor)) - e3.valor) AS vlrSul, (((e1.valor + (-1*e2.valor)) - e3.valor) + d.valdespasup) AS vlrSudeste " &
                            "FROM tmpEnergiaSE e1, tmpEnergiaSE e2, tmpEnergiaSE e3, despa d " &
                            "WHERE e1.codempre = 'RS' " &
                            "AND e1.intervalo = e2.intervalo " &
                            "AND e2.codempre = 'ER' " &
                            "AND e2.intervalo = e3.intervalo " &
                            "AND e3.codempre = 'WA' " &
                            "AND e3.intervalo = d.intdespa " &
                            "AND d.codusina = 'IBIT60' " &
                            "AND datpdp = '" & Page.Request.QueryString("strData") & "' " &
                            "ORDER BY e1.intervalo"
                    strTable = "TransfSE"
                Case Is = "BalancoSE"
                    'Temporária da Geração
                    Try
                        objCommand.CommandText = "DROP TABLE tmpGeracaoSE"
                        objCommand.ExecuteNonQuery()
                    Catch ex As Exception
                        'Se ocorrer o erro, ignora
                    End Try
                    objCommand.CommandText = "SELECT e.sigempre, d.intdespa, SUM(d.valdespasup) AS valdespasup " &
                                            "FROM empre e, usina u, despa d " &
                                            "WHERE (e.codarea = 'RE' " &
                                            "OR e.codarea = 'SP' " &
                                            "OR e.codarea = 'EP' " &
                                            "OR e.codarea = 'GT' " &
                                            "OR e.codarea = 'CS' " &
                                            "OR e.codarea = 'GP') " &
                                            "AND e.codempre = u.codempre " &
                                            "AND u.codusina = d.codusina " &
                                            "AND d.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                                            "GROUP BY e.sigempre, d.intdespa " &
                                            "INTO TEMP tmpGeracaoSE " &
                                            "WITH NO LOG"
                    objCommand.ExecuteNonQuery()

                    'Temporária do Intercâmbio
                    Try
                        objCommand.CommandText = "DROP TABLE tmpIntercambioSE"
                        objCommand.ExecuteNonQuery()
                    Catch ex As Exception
                        'Se ocorrer o erro, ignora
                    End Try
                    objCommand.CommandText = "SELECT e.sigempre, i.intinter, SUM(i.valintersup) AS valintersup " &
                                            "FROM empre e, inter i " &
                                            "WHERE (e.codarea = 'RE' " &
                                            "OR e.codarea = 'SP' " &
                                            "OR e.codarea = 'EP' " &
                                            "OR e.codarea = 'GT' " &
                                            "OR e.codarea = 'CS' " &
                                            "OR e.codarea = 'GP') " &
                                            "AND e.codempre = i.codemprede " &
                                            "AND i.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                                            "GROUP BY e.sigempre, i.intinter " &
                                            "INTO TEMP tmpIntercambioSE " &
                                            "WITH NO LOG"
                    objCommand.ExecuteNonQuery()

                    strSql = "SELECT Trim(e.sigempre) AS sigempre, c.intcarga As intervalo, c.valcargasup AS valor, 'CARGA' AS grandeza " &
                            "FROM empre e, carga c " &
                            "WHERE (e.codarea = 'RE' " &
                            "OR e.codarea = 'SP' " &
                            "OR e.codarea = 'EP' " &
                            "OR e.codarea = 'GT' " &
                            "OR e.codarea = 'CS' " &
                            "OR e.codarea = 'GP') " &
                            "AND e.codempre = c.codempre " &
                            "AND c.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                            "UNION ALL " &
                            "SELECT Trim(sigempre) AS sigempre, intdespa AS intervalo, valdespasup AS valor, 'GERACAO' AS grandeza " &
                            "FROM tmpGeracaoSE " &
                            "UNION ALL " &
                            "SELECT TRIM(sigempre) AS sigempre, intinter AS intervalo, valintersup AS valor, 'INTER' AS grandeza " &
                            "FROM tmpIntercambioSE " &
                            "ORDER BY 2, 1, 4"
                    strTable = "Balanco"
                Case Is = "FolgaSE"
                    ' Manutenção
                    Try
                        objCommand.CommandText = "DROP TABLE tmpManutencaoSE"
                        objCommand.ExecuteNonQuery()
                    Catch ex As Exception
                        'Se ocorrer o erro, ignora
                    End Try
                    objCommand.CommandText = "SELECT u.codusina, p.intparal, SUM(g.capgerad) AS vlrManutencao " &
                                            "FROM empre e, usina u, gerad g, paralpdp p " &
                                            "Where (e.codarea = 'RE' " &
                                            "OR e.codarea = 'SP' " &
                                            "OR e.codarea = 'EP' " &
                                            "OR e.codarea = 'GT' " &
                                            "OR e.codarea = 'CS' " &
                                            "OR e.codarea = 'GP') " &
                                            "AND e.codempre = u.codempre " &
                                            "AND u.tipusina = 'H' " &
                                            "AND u.codusina = g.codusina " &
                                            "AND g.codgerad = p.codequip " &
                                            "AND p.paralsup = 1 " &
                                            "AND p.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                                            "GROUP BY u.codusina, p.intparal " &
                                            "INTO TEMP tmpManutencaoSE " &
                                            "WITH NO LOG"
                    objCommand.ExecuteNonQuery()

                    'Restrição
                    Try
                        objCommand.CommandText = "DROP TABLE tmpRestrSE"
                        objCommand.ExecuteNonQuery()
                    Catch ex As Exception
                        'Se ocorrer o erro, ignora
                    End Try
                    objCommand.CommandText = "SELECT r.intrestr AS intervalo, u.codusina, " &
                                            "CASE WHEN SUM(r.valrestrprpsup) > SUM(r.valrestrusisup) " &
                                            "THEN SUM(r.valrestrprpsup) " &
                                            "ELSE SUM(r.valrestrusisup) END AS valor " &
                                            "FROM empre e, usina u, gerad g, restrpdp r " &
                                            "WHERE (e.codarea = 'RE' " &
                                            "OR e.codarea = 'SP' " &
                                            "OR e.codarea = 'EP' " &
                                            "OR e.codarea = 'GT' " &
                                            "OR e.codarea = 'CS' " &
                                            "OR e.codarea = 'GP') " &
                                            "AND e.codempre = u.codempre " &
                                            "AND u.codusina = g.codusina " &
                                            "AND g.codgerad = r.codgerad " &
                                            "AND r.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                                            "GROUP BY r.intrestr, u.codusina " &
                                            "INTO TEMP tmpRestrSE " &
                                            "WITH NO LOG"
                    objCommand.ExecuteNonQuery()

                    'Folga = Capacidade - Geração - Restrição - Manutenção
                    strSql = "SELECT d.intdespa AS intervalo, TRIM(u.codusina) AS usina, (u.potinstalada - d.valdespasup - NVL(m.vlrmanutencao,0) - NVL(r.valor,0)) AS valor " &
                            "FROM empre e, usina u, despa d, OUTER tmpManutencaoSE m, OUTER tmpRestrSE r " &
                            "WHERE (e.codarea = 'RE' " &
                            "OR e.codarea = 'SP' " &
                            "OR e.codarea = 'EP' " &
                            "OR e.codarea = 'GT' " &
                            "OR e.codarea = 'CS' " &
                            "OR e.codarea = 'GP') " &
                            "AND e.codempre = u.codempre " &
                            "AND u.tipusina = 'H' " &
                            "AND u.codusina = d.codusina " &
                            "AND d.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                            "AND d.codusina = m.codusina " &
                            "AND d.intdespa = m.intparal " &
                            "AND d.codusina = r.codusina " &
                            "AND d.intdespa = r.intervalo " &
                            "ORDER BY 1, 2"
                    strTable = "RestrSE"
                Case Is = "RestrSE"
                    'Restrições Operativas de Usinas (maior valor entre valtrstrpdpsup e valrestrusisup)
                    strSql = "SELECT r.intrestr AS intervalo, TRIM(u.codusina) AS usina, " &
                            "CASE WHEN SUM(isnull(r.valrestrprppre,0)) > SUM(isnull(r.valrestrusipre,0)) " &
                            "THEN SUM(isnull(r.valrestrprppre,0)) " &
                            "ELSE SUM(isnull(r.valrestrusipre,0)) END AS valor " &
                            "FROM empre e, usina u, gerad g, restrpdp r " &
                            "WHERE (e.codarea = 'RE' " &
                            "OR e.codarea = 'SP' " &
                            "OR e.codarea = 'EP' " &
                            "OR e.codarea = 'GT' " &
                            "OR e.codarea = 'CS' " &
                            "OR e.codarea = 'GP') " &
                            "AND e.codempre = u.codempre " &
                            "AND u.codusina = g.codusina " &
                            "AND g.codgerad = r.codgerad " &
                            "AND datpdp = '" & Page.Request.QueryString("strData") & "' " &
                            "GROUP BY 1, 2 " &
                            "ORDER BY 1, 2"
                    strTable = "RestrSE"
                Case Is = "ReservaSE"
                    'Estou usando o data set de Carga para não precisar criar outro para reserva
                    strSql = "SELECT TRIM(e.sigempre) AS codempre, r.intreser AS intcarga, (r.reserpri + r.resersec + r.reserter) AS valcargasup " &
                            "FROM reser r, empre e " &
                            "WHERE e.codempre IN (" &
                            "SELECT e.codempre " &
                            "FROM reser r, empre e " &
                            "WHERE (e.codarea = 'RE' " &
                            "OR e.codarea = 'CM' " &
                            "OR e.codarea = 'SP' " &
                            "OR e.codarea = 'EP' " &
                            "OR e.codarea = 'GT' " &
                            "OR e.codarea = 'CS' " &
                            "OR e.codarea = 'GP') " &
                            "AND e.codempre = r.codempre " &
                            "AND r.datareser = '" & strDataReser & "' " &
                            "GROUP BY 1 " &
                            "HAVING SUM(ABS(r.reserpri + r.resersec + r.reserter)) > 0) " &
                            "AND (e.codarea = 'RE' " &
                            "OR e.codarea = 'CM' " &
                            "OR e.codarea = 'SP' " &
                            "OR e.codarea = 'EP' " &
                            "OR e.codarea = 'GT' " &
                            "OR e.codarea = 'CS' " &
                            "OR e.codarea = 'GP') " &
                            "AND e.codempre = r.codempre " &
                            "AND r.datareser = '" & strDataReser & "' " &
                            "ORDER BY 2, 1"
                    strTable = "Carga"
                Case Is = "RazaoSE"
                    'IFX
                    strSql = "SELECT TRIM(u.sigusina) AS codusina, 'IFX' AS grandeza, ifx.intflexi AS intervalo, SUM(ifx.valflexisup) AS valor " &
                            "FROM empre e, usina u, inflexibilidade ifx " &
                            "WHERE u.codusina IN (" &
                            "SELECT u.codusina " &
                            "FROM empre e, usina u, inflexibilidade ifx " &
                            "WHERE (e.codarea = 'RE' OR e.codarea = 'CM' OR e.codarea = 'SP' OR e.codarea = 'EP' OR e.codarea = 'GT' OR e.codarea = 'CS' OR e.codarea = 'GP') " &
                            "AND e.codempre = u.codempre AND u.tipusina = 'T' AND u.codusina = ifx.codusina AND ifx.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                            "GROUP BY 1 " &
                            "HAVING SUM(ABS(ifx.valflexisup)) > 0) " &
                            "AND (e.codarea = 'RE' OR e.codarea = 'CM' OR e.codarea = 'SP' OR e.codarea = 'EP' OR e.codarea = 'GT' OR e.codarea = 'CS' OR e.codarea = 'GP') " &
                            "AND e.codempre = u.codempre AND u.tipusina = 'T' AND u.codusina = ifx.codusina AND ifx.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                            "GROUP BY 3, 1 "
                    'ZEL
                    strSql &= "UNION " &
                            "SELECT TRIM(u.sigusina) AS codusina, 'ZEL' AS grandeza, zel.intrazelet AS intervalo, SUM(zel.valrazeletsup) AS valor " &
                            "FROM empre e, usina u, razaoelet zel " &
                            "WHERE u.codusina IN (" &
                            "SELECT u.codusina " &
                            "FROM empre e, usina u, razaoelet zel " &
                            "WHERE (e.codarea = 'RE' OR e.codarea = 'CM' OR e.codarea = 'SP' OR e.codarea = 'EP' OR e.codarea = 'GT' OR e.codarea = 'CS' OR e.codarea = 'GP') " &
                            "AND e.codempre = u.codempre AND u.tipusina = 'T' AND u.codusina = zel.codusina AND zel.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                            "GROUP BY 1 " &
                            "HAVING SUM(ABS(zel.valrazeletsup)) > 0) " &
                            "AND (e.codarea = 'RE' OR e.codarea = 'CM' OR e.codarea = 'SP' OR e.codarea = 'EP' OR e.codarea = 'GT' OR e.codarea = 'CS' OR e.codarea = 'GP') " &
                            "AND e.codempre = u.codempre AND u.tipusina = 'T' AND u.codusina = zel.codusina AND zel.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                            "GROUP BY 3, 1 "
                    'EXP
                    strSql &= "UNION " &
                            "SELECT TRIM(u.sigusina) AS codusina, 'EXP' AS grandeza, exp.intexporta AS intervalo, SUM(exp.valexportasup) AS valor " &
                            "FROM empre e, usina u, exporta exp " &
                            "WHERE u.codusina IN (" &
                            "SELECT u.codusina " &
                            "FROM empre e, usina u, exporta exp " &
                            "WHERE (e.codarea = 'RE' OR e.codarea = 'CM' OR e.codarea = 'SP' OR e.codarea = 'EP' OR e.codarea = 'GT' OR e.codarea = 'CS' OR e.codarea = 'GP') " &
                            "AND e.codempre = u.codempre AND u.tipusina = 'T' AND u.codusina = exp.codusina AND exp.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                            "GROUP BY 1 " &
                            "HAVING SUM(Abs(exp.valexportasup)) > 0) " &
                            "AND (e.codarea = 'RE' OR e.codarea = 'CM' OR e.codarea = 'SP' OR e.codarea = 'EP' OR e.codarea = 'GT' OR e.codarea = 'CS' OR e.codarea = 'GP') " &
                            "AND e.codempre = u.codempre AND u.tipusina = 'T' AND u.codusina = exp.codusina AND exp.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                            "GROUP BY 3, 1 "
                    'ZEN
                    strSql &= "UNION " &
                            "SELECT TRIM(u.sigusina) AS codusina, 'ZEN' AS grandeza, zen.intrazener AS intervalo, SUM(zen.valrazenersup) As valor " &
                            "FROM empre e, usina u, razaoener zen " &
                            "WHERE u.codusina IN (" &
                            "SELECT u.codusina " &
                            "FROM empre e, usina u, razaoener zen " &
                            "WHERE (e.codarea = 'RE' OR e.codarea = 'CM' OR e.codarea = 'SP' OR e.codarea = 'EP' OR e.codarea = 'GT' OR e.codarea = 'CS' OR e.codarea = 'GP') " &
                            "AND e.codempre = u.codempre AND u.tipusina = 'T' AND u.codusina = zen.codusina AND zen.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                            "GROUP BY 1 " &
                            "HAVING SUM(ABS(zen.valrazenersup)) > 0) " &
                            "AND (e.codarea = 'RE' OR e.codarea = 'CM' OR e.codarea = 'SP' OR e.codarea = 'EP' OR e.codarea = 'GT' OR e.codarea = 'CS' OR e.codarea = 'GP') " &
                            "AND e.codempre = u.codempre AND u.tipusina = 'T' AND u.codusina = zen.codusina AND zen.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                            "GROUP BY 3, 1 "
                    'CFL
                    strSql &= "UNION " &
                            "SELECT TRIM(u.sigusina) AS codusina, 'CLF' AS grandeza, clf.intclf AS intervalo, SUM(clf.valclfsup) As valor " &
                            "FROM empre e, usina u, compensacaolastro_fisico clf " &
                            "WHERE u.codusina IN (" &
                            "SELECT u.codusina " &
                            "FROM empre e, usina u, compensacaolastro_fisico clf " &
                            "WHERE (e.codarea = 'RE' OR e.codarea = 'CM' OR e.codarea = 'SP' OR e.codarea = 'EP' OR e.codarea = 'GT' OR e.codarea = 'CS' OR e.codarea = 'GP') " &
                            "AND e.codempre = u.codempre AND u.tipusina = 'T' AND u.codusina = clf.codusina AND clf.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                            "GROUP BY 1 " &
                            "HAVING SUM(ABS(clf.valclfsup)) > 0) " &
                            "AND (e.codarea = 'RE' OR e.codarea = 'CM' OR e.codarea = 'SP' OR e.codarea = 'EP' OR e.codarea = 'GT' OR e.codarea = 'CS' OR e.codarea = 'GP') " &
                            "AND e.codempre = u.codempre AND u.tipusina = 'T' AND u.codusina = clf.codusina AND clf.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                            "GROUP BY 3, 1 "
                    'CFM
                    strSql &= "UNION " &
                            "SELECT TRIM(u.sigusina) AS codusina, 'CFO' AS grandeza, cfm.intcfm AS intervalo, SUM(cfm.valcfmsup) As valor " &
                            "FROM empre e, usina u, tb_cfm cfm " &
                            "WHERE u.codusina IN (" &
                            "SELECT u.codusina " &
                            "FROM empre e, usina u, tb_cfm cfm " &
                            "WHERE (e.codarea = 'RE' OR e.codarea = 'CM' OR e.codarea = 'SP' OR e.codarea = 'EP' OR e.codarea = 'GT' OR e.codarea = 'CS' OR e.codarea = 'GP') " &
                            "AND e.codempre = u.codempre AND u.tipusina = 'T' AND u.codusina = cfm.codusina AND cfm.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                            "GROUP BY 1 " &
                            "HAVING SUM(ABS(cfm.valcfmsup)) > 0) " &
                            "AND (e.codarea = 'RE' OR e.codarea = 'CM' OR e.codarea = 'SP' OR e.codarea = 'EP' OR e.codarea = 'GT' OR e.codarea = 'CS' OR e.codarea = 'GP') " &
                            "AND e.codempre = u.codempre AND u.tipusina = 'T' AND u.codusina = cfm.codusina AND cfm.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                            "GROUP BY 3, 1 "
                    'GFM
                    strSql &= "UNION " &
                            "SELECT TRIM(u.sigusina) AS codusina, 'GFO' AS grandeza, gfm.intgfm AS intervalo, SUM(gfm.valgfmsup) As valor " &
                            "FROM empre e, usina u, tb_gfm gfm " &
                            "WHERE u.codusina IN (" &
                            "SELECT u.codusina " &
                            "FROM empre e, usina u, tb_gfm gfm " &
                            "WHERE (e.codarea = 'RE' OR e.codarea = 'CM' OR e.codarea = 'SP' OR e.codarea = 'EP' OR e.codarea = 'GT' OR e.codarea = 'CS' OR e.codarea = 'GP') " &
                            "AND e.codempre = u.codempre AND u.tipusina = 'T' AND u.codusina = gfm.codusina AND gfm.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                            "GROUP BY 1 " &
                            "HAVING SUM(ABS(gfm.valgfmsup)) > 0) " &
                            "AND (e.codarea = 'RE' OR e.codarea = 'CM' OR e.codarea = 'SP' OR e.codarea = 'EP' OR e.codarea = 'GT' OR e.codarea = 'CS' OR e.codarea = 'GP') " &
                            "AND e.codempre = u.codempre AND u.tipusina = 'T' AND u.codusina = gfm.codusina AND gfm.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                            "GROUP BY 3, 1 "
                    'RMP
                    strSql &= "UNION " &
                            "SELECT TRIM(u.sigusina) AS codusina, 'GE' AS grandeza, rmp.intrmp AS intervalo, SUM(rmp.valrmpsup) As valor " &
                            "FROM empre e, usina u, tb_rmp rmp " &
                            "WHERE u.codusina IN (" &
                            "SELECT u.codusina " &
                            "FROM empre e, usina u, tb_rmp rmp " &
                            "WHERE (e.codarea = 'RE' OR e.codarea = 'CM' OR e.codarea = 'SP' OR e.codarea = 'EP' OR e.codarea = 'GT' OR e.codarea = 'CS' OR e.codarea = 'GP') " &
                            "AND e.codempre = u.codempre AND u.tipusina = 'T' AND u.codusina = rmp.codusina AND rmp.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                            "GROUP BY 1 " &
                            "HAVING SUM(ABS(rmp.valrmpsup)) > 0) " &
                            "AND (e.codarea = 'RE' OR e.codarea = 'CM' OR e.codarea = 'SP' OR e.codarea = 'EP' OR e.codarea = 'GT' OR e.codarea = 'CS' OR e.codarea = 'GP') " &
                            "AND e.codempre = u.codempre AND u.tipusina = 'T' AND u.codusina = rmp.codusina AND rmp.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                            "GROUP BY 3, 1 "
                    'SOM
                    strSql &= "UNION " &
                            "SELECT TRIM(u.sigusina) AS codusina, 'GSU' AS grandeza, som.intsom AS intervalo, SUM(som.valsomsup) As valor " &
                            "FROM empre e, usina u, tb_som som " &
                            "WHERE u.codusina IN (" &
                            "SELECT u.codusina " &
                            "FROM empre e, usina u, tb_som som " &
                            "WHERE (e.codarea = 'RE' OR e.codarea = 'CM' OR e.codarea = 'SP' OR e.codarea = 'EP' OR e.codarea = 'GT' OR e.codarea = 'CS' OR e.codarea = 'GP') " &
                            "AND e.codempre = u.codempre AND u.tipusina = 'T' AND u.codusina = som.codusina AND som.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                            "GROUP BY 1 " &
                            "HAVING SUM(ABS(som.valsomsup)) > 0) " &
                            "AND (e.codarea = 'RE' OR e.codarea = 'CM' OR e.codarea = 'SP' OR e.codarea = 'EP' OR e.codarea = 'GT' OR e.codarea = 'CS' OR e.codarea = 'GP') " &
                            "AND e.codempre = u.codempre AND u.tipusina = 'T' AND u.codusina = som.codusina AND som.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                            "GROUP BY 3, 1 "

                    'GES
                    strSql &= "UNION " &
                            "SELECT TRIM(u.sigusina) AS codusina, 'GSU' AS grandeza, GES.intGES AS intervalo, SUM(GES.valGESsup) As valor " &
                            "FROM empre e, usina u, tb_GES GES " &
                            "WHERE u.codusina IN (" &
                            "SELECT u.codusina " &
                            "FROM empre e, usina u, tb_GES GES " &
                            "WHERE (e.codarea = 'RE' OR e.codarea = 'CM' OR e.codarea = 'SP' OR e.codarea = 'EP' OR e.codarea = 'GT' OR e.codarea = 'CS' OR e.codarea = 'GP') " &
                            "AND e.codempre = u.codempre AND u.tipusina = 'T' AND u.codusina = GES.codusina AND GES.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                            "GROUP BY 1 " &
                            "HAVING SUM(ABS(GES.valGESsup)) > 0) " &
                            "AND (e.codarea = 'RE' OR e.codarea = 'CM' OR e.codarea = 'SP' OR e.codarea = 'EP' OR e.codarea = 'GT' OR e.codarea = 'CS' OR e.codarea = 'GP') " &
                            "AND e.codempre = u.codempre AND u.tipusina = 'T' AND u.codusina = GES.codusina AND GES.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                            "GROUP BY 3, 1 "

                    'GEC
                    strSql &= "UNION " &
                            "SELECT TRIM(u.sigusina) AS codusina, 'GSU' AS grandeza, GEC.intGEC AS intervalo, SUM(GEC.valGECsup) As valor " &
                            "FROM empre e, usina u, tb_GEC GEC " &
                            "WHERE u.codusina IN (" &
                            "SELECT u.codusina " &
                            "FROM empre e, usina u, tb_GEC GEC " &
                            "WHERE (e.codarea = 'RE' OR e.codarea = 'CM' OR e.codarea = 'SP' OR e.codarea = 'EP' OR e.codarea = 'GT' OR e.codarea = 'CS' OR e.codarea = 'GP') " &
                            "AND e.codempre = u.codempre AND u.tipusina = 'T' AND u.codusina = GEC.codusina AND GEC.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                            "GROUP BY 1 " &
                            "HAVING SUM(ABS(GEC.valGECsup)) > 0) " &
                            "AND (e.codarea = 'RE' OR e.codarea = 'CM' OR e.codarea = 'SP' OR e.codarea = 'EP' OR e.codarea = 'GT' OR e.codarea = 'CS' OR e.codarea = 'GP') " &
                            "AND e.codempre = u.codempre AND u.tipusina = 'T' AND u.codusina = GEC.codusina AND GEC.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                            "GROUP BY 3, 1 "

                    'DCA
                    strSql &= "UNION " &
                            "SELECT TRIM(u.sigusina) AS codusina, 'GSU' AS grandeza, DCA.intDCA AS intervalo, SUM(DCA.valDCAsup) As valor " &
                            "FROM empre e, usina u, tb_DCA DCA " &
                            "WHERE u.codusina IN (" &
                            "SELECT u.codusina " &
                            "FROM empre e, usina u, tb_DCA DCA " &
                            "WHERE (e.codarea = 'RE' OR e.codarea = 'CM' OR e.codarea = 'SP' OR e.codarea = 'EP' OR e.codarea = 'GT' OR e.codarea = 'CS' OR e.codarea = 'GP') " &
                            "AND e.codempre = u.codempre AND u.tipusina = 'T' AND u.codusina = DCA.codusina AND DCA.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                            "GROUP BY 1 " &
                            "HAVING SUM(ABS(DCA.valDCAsup)) > 0) " &
                            "AND (e.codarea = 'RE' OR e.codarea = 'CM' OR e.codarea = 'SP' OR e.codarea = 'EP' OR e.codarea = 'GT' OR e.codarea = 'CS' OR e.codarea = 'GP') " &
                            "AND e.codempre = u.codempre AND u.tipusina = 'T' AND u.codusina = DCA.codusina AND DCA.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                            "GROUP BY 3, 1 "

                    'DCR
                    strSql &= "UNION " &
                            "SELECT TRIM(u.sigusina) AS codusina, 'GSU' AS grandeza, DCR.intDCR AS intervalo, SUM(DCR.valDCRsup) As valor " &
                            "FROM empre e, usina u, tb_DCR DCR " &
                            "WHERE u.codusina IN (" &
                            "SELECT u.codusina " &
                            "FROM empre e, usina u, tb_DCR DCR " &
                            "WHERE (e.codarea = 'RE' OR e.codarea = 'CM' OR e.codarea = 'SP' OR e.codarea = 'EP' OR e.codarea = 'GT' OR e.codarea = 'CS' OR e.codarea = 'GP') " &
                            "AND e.codempre = u.codempre AND u.tipusina = 'T' AND u.codusina = DCR.codusina AND DCR.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                            "GROUP BY 1 " &
                            "HAVING SUM(ABS(DCR.valDCRsup)) > 0) " &
                            "AND (e.codarea = 'RE' OR e.codarea = 'CM' OR e.codarea = 'SP' OR e.codarea = 'EP' OR e.codarea = 'GT' OR e.codarea = 'CS' OR e.codarea = 'GP') " &
                            "AND e.codempre = u.codempre AND u.tipusina = 'T' AND u.codusina = DCR.codusina AND DCR.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                            "GROUP BY 3, 1 "

                    'IR1
                    strSql &= "UNION " &
                            "SELECT TRIM(u.sigusina) AS codusina, 'GSU' AS grandeza, null AS intervalo, SUM(IR1.valIR1sup) As valor " &
                            "FROM empre e, usina u, tb_IR1 IR1 " &
                            "WHERE u.codusina IN (" &
                            "SELECT u.codusina " &
                            "FROM empre e, usina u, tb_IR1 IR1 " &
                            "WHERE (e.codarea = 'RE' OR e.codarea = 'CM' OR e.codarea = 'SP' OR e.codarea = 'EP' OR e.codarea = 'GT' OR e.codarea = 'CS' OR e.codarea = 'GP') " &
                            "AND e.codempre = u.codempre AND u.tipusina = 'T' AND u.codusina = IR1.codusina AND IR1.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                            "GROUP BY 1 " &
                            "HAVING SUM(ABS(IR1.valIR1sup)) > 0) " &
                            "AND (e.codarea = 'RE' OR e.codarea = 'CM' OR e.codarea = 'SP' OR e.codarea = 'EP' OR e.codarea = 'GT' OR e.codarea = 'CS' OR e.codarea = 'GP') " &
                            "AND e.codempre = u.codempre AND u.tipusina = 'T' AND u.codusina = IR1.codusina AND IR1.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                            "GROUP BY 3, 1 "

                    'IR2
                    strSql &= "UNION " &
                            "SELECT TRIM(u.sigusina) AS codusina, 'GSU' AS grandeza, IR2.intIR2 AS intervalo, SUM(IR2.valIR2sup) As valor " &
                            "FROM empre e, usina u, tb_IR2 IR2 " &
                            "WHERE u.codusina IN (" &
                            "SELECT u.codusina " &
                            "FROM empre e, usina u, tb_IR2 IR2 " &
                            "WHERE (e.codarea = 'RE' OR e.codarea = 'CM' OR e.codarea = 'SP' OR e.codarea = 'EP' OR e.codarea = 'GT' OR e.codarea = 'CS' OR e.codarea = 'GP') " &
                            "AND e.codempre = u.codempre AND u.tipusina = 'T' AND u.codusina = IR2.codusina AND IR2.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                            "GROUP BY 1 " &
                            "HAVING SUM(ABS(IR2.valIR2sup)) > 0) " &
                            "AND (e.codarea = 'RE' OR e.codarea = 'CM' OR e.codarea = 'SP' OR e.codarea = 'EP' OR e.codarea = 'GT' OR e.codarea = 'CS' OR e.codarea = 'GP') " &
                            "AND e.codempre = u.codempre AND u.tipusina = 'T' AND u.codusina = IR2.codusina AND IR2.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                            "GROUP BY 3, 1 "

                    'IR3
                    strSql &= "UNION " &
                            "SELECT TRIM(u.sigusina) AS codusina, 'GSU' AS grandeza, IR3.intIR3 AS intervalo, SUM(IR3.valIR3sup) As valor " &
                            "FROM empre e, usina u, tb_IR3 IR3 " &
                            "WHERE u.codusina IN (" &
                            "SELECT u.codusina " &
                            "FROM empre e, usina u, tb_IR3 IR3 " &
                            "WHERE (e.codarea = 'RE' OR e.codarea = 'CM' OR e.codarea = 'SP' OR e.codarea = 'EP' OR e.codarea = 'GT' OR e.codarea = 'CS' OR e.codarea = 'GP') " &
                            "AND e.codempre = u.codempre AND u.tipusina = 'T' AND u.codusina = IR3.codusina AND IR3.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                            "GROUP BY 1 " &
                            "HAVING SUM(ABS(IR3.valIR3sup)) > 0) " &
                            "AND (e.codarea = 'RE' OR e.codarea = 'CM' OR e.codarea = 'SP' OR e.codarea = 'EP' OR e.codarea = 'GT' OR e.codarea = 'CS' OR e.codarea = 'GP') " &
                            "AND e.codempre = u.codempre AND u.tipusina = 'T' AND u.codusina = IR3.codusina AND IR3.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                            "GROUP BY 3, 1 "

                    'IR4
                    strSql &= "UNION " &
                            "SELECT TRIM(u.sigusina) AS codusina, 'GSU' AS grandeza, IR4.intIR4 AS intervalo, SUM(IR4.valIR4sup) As valor " &
                            "FROM empre e, usina u, tb_IR4 IR4 " &
                            "WHERE u.codusina IN (" &
                            "SELECT u.codusina " &
                            "FROM empre e, usina u, tb_IR4 IR4 " &
                            "WHERE (e.codarea = 'RE' OR e.codarea = 'CM' OR e.codarea = 'SP' OR e.codarea = 'EP' OR e.codarea = 'GT' OR e.codarea = 'CS' OR e.codarea = 'GP') " &
                            "AND e.codempre = u.codempre AND u.tipusina = 'T' AND u.codusina = IR4.codusina AND IR4.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                            "GROUP BY 1 " &
                            "HAVING SUM(ABS(IR4.valIR4sup)) > 0) " &
                            "AND (e.codarea = 'RE' OR e.codarea = 'CM' OR e.codarea = 'SP' OR e.codarea = 'EP' OR e.codarea = 'GT' OR e.codarea = 'CS' OR e.codarea = 'GP') " &
                            "AND e.codempre = u.codempre AND u.tipusina = 'T' AND u.codusina = IR4.codusina AND IR4.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                            "GROUP BY 3, 1 "

                    'PE
                    strSql &= "UNION " &
                            "SELECT TRIM(u.sigusina) AS codusina, 'PE' AS grandeza, pe.intpcc AS intervalo, SUM(pe.valpccsup) As valor " &
                            "FROM empre e, usina u, perdascic pe " &
                            "WHERE u.codusina IN (" &
                            "SELECT u.codusina " &
                            "FROM empre e, usina u, perdascic pe " &
                            "WHERE (e.codarea = 'RE' OR e.codarea = 'CM' OR e.codarea = 'SP' OR e.codarea = 'EP' OR e.codarea = 'GT' OR e.codarea = 'CS' OR e.codarea = 'GP') " &
                            "AND e.codempre = u.codempre AND u.tipusina = 'T' AND u.codusina = pe.codusina AND pe.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                            "GROUP BY 1 " &
                            "HAVING SUM(ABS(pe.valpccsup)) > 0) " &
                            "AND (e.codarea = 'RE' OR e.codarea = 'CM' OR e.codarea = 'SP' OR e.codarea = 'EP' OR e.codarea = 'GT' OR e.codarea = 'CS' OR e.codarea = 'GP') " &
                            "AND e.codempre = u.codempre AND u.tipusina = 'T' AND u.codusina = pe.codusina AND pe.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                            "GROUP BY 3, 1 " &
                            "ORDER BY 3, 1"
                    strTable = "InflexUsinaS"
                Case Is = "ConfigSE"
                    strSql = "SELECT m.intmeg AS intervalo, TRIM(m.codusina) AS usina, m.valmegsup AS valor " &
                            "FROM empre e, usina u, maq_gerando m " &
                            "WHERE m.codusina IN (" &
                            "SELECT u.codusina " &
                            "FROM empre e, usina u, maq_gerando m " &
                            "WHERE (e.codarea = 'RE' " &
                            "OR e.codarea = 'CM' " &
                            "OR e.codarea = 'SP' " &
                            "OR e.codarea = 'EP' " &
                            "OR e.codarea = 'GT' " &
                            "OR e.codarea = 'CS' " &
                            "OR e.codarea = 'GP') " &
                            "AND e.codempre = u.codempre " &
                            "AND u.codusina = m.codusina " &
                            "AND m.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                            "GROUP BY 1 " &
                            "HAVING SUM(ABS(m.valmegsup)) > 0) " &
                            "AND (e.codarea = 'RE' " &
                            "OR e.codarea = 'CM' " &
                            "OR e.codarea = 'SP' " &
                            "OR e.codarea = 'EP' " &
                            "OR e.codarea = 'GT' " &
                            "OR e.codarea = 'CS' " &
                            "OR e.codarea = 'GP') " &
                            "AND e.codempre = u.codempre " &
                            "AND u.codusina = m.codusina " &
                            "AND m.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                            "ORDER BY 1, 2"
                    strTable = "RestrSE"
                Case Is = "ParadaNE_H"
                    Try
                        objCommand.CommandText = "DROP TABLE tmpParadaNE"
                        objCommand.ExecuteNonQuery()
                    Catch ex As Exception
                        'Se ocorrer o erro, ignora
                    End Try
                    'A partir de Fevereiro de 2008 os valores de manutenção passaram a ser somados da coluna PRE e não mais da SUP
                    'Algum procedimento está zerando a coluna SUP e não foi encontrado o problema.
                    objCommand.CommandText = "SELECT u.codusina, c.intmco AS intervalo, SUM(c.valmcosup) AS valor, 'CO' AS tipo, 'DESL.' AS estado, u.ordem " &
                                            "FROM empre e, usina u, conveniencia_oper c " &
                                            "WHERE e.codarea = 'NE' " &
                                            "AND e.codempre = u.codempre " &
                                            "AND (u.tipusina = 'H' " &
                                            "OR u.codusina = 'CHUTC') " &
                                            "AND u.codusina = c.codusina " &
                                            "AND c.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                                            "GROUP BY 1, 2, 6 " &
                                            "UNION ALL " &
                                            "SELECT u.codusina, d.intdespa As intervalo, SUM(NVL(p.paralpre,0)) AS valor, 'PP' AS tipo, 'DESL.' AS estado, u.ordem " &
                                            "FROM empre e, usina u, gerad g, despa d, OUTER paralpdp p " &
                                            "WHERE e.codarea = 'NE' " &
                                            "AND e.codempre = u.codempre " &
                                            "AND (u.tipusina = 'H' " &
                                            "OR u.codusina = 'CHUTC') " &
                                            "AND u.codusina = d.codusina " &
                                            "AND d.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                                            "AND d.codusina = g.codusina " &
                                            "AND g.codgerad = p.codequip " &
                                            "AND p.paralpre = 1 " &
                                            "AND d.datpdp = p.datpdp " &
                                            "AND d.intdespa = p.intparal " &
                                            "GROUP BY 1, 2, 6 " &
                                            "UNION ALL " &
                                            "SELECT u.codusina, m.intmeg AS intervalo, SUM(m.valmegsup) AS valor, 'MG' AS tipo, 'LIG.' AS estado, u.ordem " &
                                            "FROM empre e, usina u, maq_gerando m " &
                                            "WHERE e.codarea = 'NE' " &
                                            "AND e.codempre = u.codempre " &
                                            "AND (u.tipusina = 'H' " &
                                            "OR u.codusina = 'CHUTC') " &
                                            "AND u.codusina = m.codusina " &
                                            "AND m.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                                            "GROUP BY 1, 2, 6 " &
                                            "UNION ALL " &
                                            "SELECT u.codusina, d.intdespa AS intervalo, SUM(NVL(o.valmossup,0)) AS valor, 'OS' AS tipo, 'LIG.' AS estado, u.ordem " &
                                            "FROM empre e, usina u, despa d, OUTER oper_sincrono o " &
                                            "WHERE e.codarea = 'NE' " &
                                            "AND e.codempre = u.codempre " &
                                            "AND (u.tipusina = 'H' " &
                                            "OR u.codusina = 'CHUTC') " &
                                            "AND u.codusina = d.codusina " &
                                            "AND d.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                                            "AND u.codusina = o.codusina " &
                                            "AND o.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                                            "GROUP BY 1, 2, 6 " &
                                            "ORDER BY 1, 2, 4 " &
                                            "INTO TEMP tmpParadaNE " &
                                            "WITH NO LOG"
                    'objCommand.CommandText = "SELECT u.codusina, c.intmco AS intervalo, SUM(c.valmcosup) AS valor, 'CO' AS tipo, 'DESL.' AS estado " & _
                    '                         "FROM empre e, usina u, conveniencia_oper c " & _
                    '                         "WHERE e.codarea = 'NE' " & _
                    '                         "AND e.codempre = u.codempre " & _
                    '                         "AND u.codusina = c.codusina " & _
                    '                         "AND c.datpdp = '" & Page.Request.QueryString("strData") & "' " & _
                    '                         "GROUP BY 1, 2 " & _
                    '                         "UNION ALL " & _
                    '                         "SELECT u.codusina, p.intparal As intervalo, COUNT(p.paralsup) AS valor, 'PP' AS tipo, 'DESL.' AS estado " & _
                    '                         "FROM empre e, usina u, gerad g, paralpdp p " & _
                    '                         "WHERE e.codarea = 'NE' " & _
                    '                         "AND e.codempre = u.codempre " & _
                    '                         "AND u.codusina = g.codusina " & _
                    '                         "AND g.codgerad = p.codequip " & _
                    '                         "AND p.paralsup = 1 " & _
                    '                         "AND p.datpdp = '" & Page.Request.QueryString("strData") & "' " & _
                    '                         "GROUP BY 1, 2 " & _
                    '                         "UNION ALL " & _
                    '                         "SELECT u.codusina, m.intmeg AS intervalo, SUM(m.valmegsup) AS valor, 'MG' AS tipo, 'LIG.' AS estado " & _
                    '                         "FROM empre e, usina u, maq_gerando m " & _
                    '                         "WHERE e.codarea = 'NE' " & _
                    '                         "AND e.codempre = u.codempre " & _
                    '                         "AND u.codusina = m.codusina " & _
                    '                         "AND m.datpdp = '" & Page.Request.QueryString("strData") & "' " & _
                    '                         "GROUP BY 1, 2 " & _
                    '                         "UNION ALL " & _
                    '                         "SELECT u.codusina, o.intmos AS intervalo, SUM(o.valmossup) AS valor, 'OS' AS tipo, 'LIG.' AS estado " & _
                    '                         "FROM empre e, usina u, oper_sincrono o " & _
                    '                         "WHERE e.codarea = 'NE' " & _
                    '                         "AND e.codempre = u.codempre " & _
                    '                         "AND u.codusina = o.codusina " & _
                    '                         "AND o.datpdp = '" & Page.Request.QueryString("strData") & "' " & _
                    '                         "GROUP BY 1, 2 " & _
                    '                         "ORDER BY 1, 2, 4 " & _
                    '                         "INTO TEMP tmpParadaNE " & _
                    '                         "WITH NO LOG"
                    objCommand.ExecuteNonQuery()

                    Try
                        objCommand.CommandText = "DROP TABLE tmpMaquinaNE"
                        objCommand.ExecuteNonQuery()
                    Catch ex As Exception
                        'Se ocorrer o erro, ignora
                    End Try

                    objCommand.CommandText = "SELECT u.codusina, COUNT(g.codgerad) AS maq " &
                                            "FROM empre e, usina u, gerad g " &
                                            "WHERE e.codarea = 'NE' " &
                                            "AND e.codempre = u.codempre " &
                                            "AND (u.tipusina = 'H' " &
                                            "OR u.codusina = 'CHUTC') " &
                                            "AND u.codusina = g.codusina " &
                                            "GROUP BY u.codusina " &
                                            "ORDER BY u.codusina " &
                                            "INTO TEMP tmpMaquinaNE " &
                                            "WITH NO LOG"
                    objCommand.ExecuteNonQuery()

                    strSql = "SELECT m.codusina, m.maq, p.intervalo, p.estado, p.tipo, NVL(p.valor,0) AS valor, p.ordem " &
                            "FROM tmpMaquinaNE m, tmpParadaNE p " &
                            "WHERE m.codusina = p.codusina " &
                            "AND p.codusina NOT IN ('CHPHID','CHUFL','CHUPE')" &
                            "ORDER BY p.intervalo, p.ordem, m.codusina, estado, tipo"

                    'strSql = "SELECT m.codusina, m.maq, p.intervalo, p.estado, p.tipo, NVL(p.valor,0) AS valor, p.ordem " & _
                    '         "FROM tmpMaquinaNE m, tmpParadaNE p " & _
                    '         "WHERE m.codusina = p.codusina " & _
                    '         "AND p.codusina NOT IN ('SYBEJD','SYBERL','TGGCCB','TGGCIP','TGGCPT','TGGCPZ','TGGCRF','TGGCSP','TETECP','TOTMCB')" & _
                    '         "ORDER BY p.intervalo, p.ordem, m.codusina, estado, tipo"

                    'strSql = "SELECT m.codusina, m.maq, p.intervalo, p.estado, p.tipo, NVL(p.valor,0) AS valor " & _
                    '         "FROM tmpMaquinaNE m, tmpParadaNE p " & _
                    '         "WHERE m.codusina = p.codusina " & _
                    '         "ORDER BY p.intervalo, m.codusina"
                    strTable = "ParadaNE"
                    'Case Is = "ParadaNE_T"
                    '    Try
                    '        objCommand.CommandText = "DROP TABLE tmpParadaNE"
                    '        objCommand.ExecuteNonQuery()
                    '    Catch ex As Exception
                    '        'Se ocorrer o erro, ignora
                    '    End Try
                    '    objCommand.CommandText = "SELECT u.codusina, c.intmco AS intervalo, SUM(c.valmcosup) AS valor, 'CO' AS tipo, 'DESL.' AS estado, u.ordem " & _
                    '                            "FROM empre e, usina u, conveniencia_oper c " & _
                    '                            "WHERE e.codarea = 'NE' " & _
                    '                            "AND e.codempre = u.codempre " & _
                    '                            "AND u.tipusina = 'T' " & _
                    '                            "AND u.codusina = c.codusina " & _
                    '                            "AND c.datpdp = '" & Page.Request.QueryString("strData") & "' " & _
                    '                            "GROUP BY 1, 2, 6 " & _
                    '                            "UNION ALL " & _
                    '                            "SELECT u.codusina, d.intdespa As intervalo, SUM(NVL(p.paralsup,0)) AS valor, 'PP' AS tipo, 'DESL.' AS estado, u.ordem " & _
                    '                            "FROM empre e, usina u, gerad g, despa d, OUTER paralpdp p " & _
                    '                            "WHERE e.codarea = 'NE' " & _
                    '                            "AND e.codempre = u.codempre " & _
                    '                            "AND u.tipusina = 'T' " & _
                    '                            "AND u.codusina = d.codusina " & _
                    '                            "AND d.datpdp = '" & Page.Request.QueryString("strData") & "' " & _
                    '                            "AND d.codusina = g.codusina " & _
                    '                            "AND g.codgerad = p.codequip " & _
                    '                            "AND p.paralsup = 1 " & _
                    '                            "AND d.datpdp = p.datpdp " & _
                    '                            "AND d.intdespa = p.intparal " & _
                    '                            "GROUP BY 1, 2, 6 " & _
                    '                            "UNION ALL " & _
                    '                            "SELECT u.codusina, m.intmeg AS intervalo, SUM(m.valmegsup) AS valor, 'MG' AS tipo, 'LIG.' AS estado, u.ordem " & _
                    '                            "FROM empre e, usina u, maq_gerando m " & _
                    '                            "WHERE e.codarea = 'NE' " & _
                    '                            "AND e.codempre = u.codempre " & _
                    '                            "AND u.tipusina = 'T' " & _
                    '                            "AND u.codusina = m.codusina " & _
                    '                            "AND m.datpdp = '" & Page.Request.QueryString("strData") & "' " & _
                    '                            "GROUP BY 1, 2, 6 " & _
                    '                            "UNION ALL " & _
                    '                            "SELECT u.codusina, d.intdespa AS intervalo, SUM(NVL(o.valmossup,0)) AS valor, 'OS' AS tipo, 'LIG.' AS estado, u.ordem " & _
                    '                            "FROM empre e, usina u, despa d, OUTER oper_sincrono o " & _
                    '                            "WHERE e.codarea = 'NE' " & _
                    '                            "AND e.codempre = u.codempre " & _
                    '                            "AND u.tipusina = 'T' " & _
                    '                            "AND u.codusina = d.codusina " & _
                    '                            "AND d.datpdp = '" & Page.Request.QueryString("strData") & "' " & _
                    '                            "AND d.codusina = o.codusina " & _
                    '                            "AND d.intdespa = o.intmos " & _
                    '                            "AND d.datpdp = o.datpdp " & _
                    '                            "GROUP BY 1, 2, 6 " & _
                    '                            "ORDER BY 1, 2, 4 " & _
                    '                            "INTO TEMP tmpParadaNE " & _
                    '                            "WITH NO LOG"
                    '    'objCommand.CommandText = "SELECT u.codusina, c.intmco AS intervalo, SUM(c.valmcosup) AS valor, 'CO' AS tipo, 'DESL.' AS estado " & _
                    '    '                         "FROM empre e, usina u, conveniencia_oper c " & _
                    '    '                         "WHERE e.codarea = 'NE' " & _
                    '    '                         "AND e.codempre = u.codempre " & _
                    '    '                         "AND u.codusina = c.codusina " & _
                    '    '                         "AND c.datpdp = '" & Page.Request.QueryString("strData") & "' " & _
                    '    '                         "GROUP BY 1, 2 " & _
                    '    '                         "UNION ALL " & _
                    '    '                         "SELECT u.codusina, p.intparal As intervalo, COUNT(p.paralsup) AS valor, 'PP' AS tipo, 'DESL.' AS estado " & _
                    '    '                         "FROM empre e, usina u, gerad g, paralpdp p " & _
                    '    '                         "WHERE e.codarea = 'NE' " & _
                    '    '                         "AND e.codempre = u.codempre " & _
                    '    '                         "AND u.codusina = g.codusina " & _
                    '    '                         "AND g.codgerad = p.codequip " & _
                    '    '                         "AND p.paralsup = 1 " & _
                    '    '                         "AND p.datpdp = '" & Page.Request.QueryString("strData") & "' " & _
                    '    '                         "GROUP BY 1, 2 " & _
                    '    '                         "UNION ALL " & _
                    '    '                         "SELECT u.codusina, m.intmeg AS intervalo, SUM(m.valmegsup) AS valor, 'MG' AS tipo, 'LIG.' AS estado " & _
                    '    '                         "FROM empre e, usina u, maq_gerando m " & _
                    '    '                         "WHERE e.codarea = 'NE' " & _
                    '    '                         "AND e.codempre = u.codempre " & _
                    '    '                         "AND u.codusina = m.codusina " & _
                    '    '                         "AND m.datpdp = '" & Page.Request.QueryString("strData") & "' " & _
                    '    '                         "GROUP BY 1, 2 " & _
                    '    '                         "UNION ALL " & _
                    '    '                         "SELECT u.codusina, o.intmos AS intervalo, SUM(o.valmossup) AS valor, 'OS' AS tipo, 'LIG.' AS estado " & _
                    '    '                         "FROM empre e, usina u, oper_sincrono o " & _
                    '    '                         "WHERE e.codarea = 'NE' " & _
                    '    '                         "AND e.codempre = u.codempre " & _
                    '    '                         "AND u.codusina = o.codusina " & _
                    '    '                         "AND o.datpdp = '" & Page.Request.QueryString("strData") & "' " & _
                    '    '                         "GROUP BY 1, 2 " & _
                    '    '                         "ORDER BY 1, 2, 4 " & _
                    '    '                         "INTO TEMP tmpParadaNE " & _
                    '    '                         "WITH NO LOG"
                    '    objCommand.ExecuteNonQuery()

                    '    Try
                    '        objCommand.CommandText = "DROP TABLE tmpMaquinaNE"
                    '        objCommand.ExecuteNonQuery()
                    '    Catch ex As Exception
                    '        'Se ocorrer o erro, ignora
                    '    End Try

                    '    objCommand.CommandText = "SELECT u.codusina, COUNT(g.codgerad) AS maq " & _
                    '                            "FROM empre e, usina u, gerad g " & _
                    '                            "WHERE e.codarea = 'NE' " & _
                    '                            "AND e.codempre = u.codempre " & _
                    '                            "AND u.tipusina = 'T' " & _
                    '                            "AND u.codusina = g.codusina " & _
                    '                            "GROUP BY u.codusina " & _
                    '                            "ORDER BY u.codusina " & _
                    '                            "INTO TEMP tmpMaquinaNE " & _
                    '                            "WITH NO LOG"
                    '    objCommand.ExecuteNonQuery()
                    '    strSql = "SELECT m.codusina, m.maq, p.intervalo, p.estado, p.tipo, NVL(p.valor,0) AS valor, p.ordem " & _
                    '            "FROM tmpMaquinaNE m, tmpParadaNE p " & _
                    '            "WHERE m.codusina = p.codusina " & _
                    '            "AND p.codusina NOT IN ('SYBEJD','SYBERL','TGGCCB','TGGCIP','TGGCPT','TGGCPZ','TGGCRF','TGGCSP','TETECP','TOTMCB') " & _
                    '            "ORDER BY p.intervalo, p.ordem, m.codusina, estado, tipo"

                    '    'strSql = "SELECT m.codusina, m.maq, p.intervalo, p.estado, p.tipo, NVL(p.valor,0) AS valor " & _
                    '    '         "FROM tmpMaquinaNE m, tmpParadaNE p " & _
                    '    '         "WHERE m.codusina = p.codusina " & _
                    '    '         "ORDER BY p.intervalo, m.codusina"
                    '    strTable = "ParadaNE"
                Case Is = "CargaNE"
                    strSql = "SELECT e.nomempre[10,23] AS codempre, intcarga, valcargasup " &
                            "FROM empre e, carga c " &
                            "WHERE (e.codempre = 'HC' " &
                            "OR e.codempre = 'HD' " &
                            "OR e.codempre = 'HL' " &
                            "OR e.codempre = 'HN' " &
                            "OR e.codempre = 'HO' " &
                            "OR e.codempre = 'HS') " &
                            "AND e.codempre = c.codempre " &
                            "AND c.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                            "ORDER BY c.intcarga, e.nomempre"
                    strTable = "Carga"

                    '-- O bloco ReqGerNE, renomeado como ReqGerNE_OLD foi desativado
                    '-- com a criação da view VW_RELPDP_NE_REQUISITO_GERACAO - CRQ4434 em 14/03/2013
                Case Is = "ReqGerNE_Old"

                    '## Temporária ## Requisito NE (Carga)
                    Try
                        objCommand.CommandText = "DROP TABLE tmpCargaNE"
                        objCommand.ExecuteNonQuery()
                    Catch ex As Exception
                        'Se ocorrer o erro, ignora
                    End Try
                    objCommand.CommandText = "SELECT c.intcarga, SUM(NVL(c.valcargasup,0)) AS vlrCarga " &
                                            "FROM empre e, carga c " &
                                            "WHERE (e.codempre = 'HC' " &
                                            "OR e.codempre = 'HD' " &
                                            "OR e.codempre = 'HL' " &
                                            "OR e.codempre = 'HN' " &
                                            "OR e.codempre = 'HO' " &
                                            "OR e.codempre = 'HS') " &
                                            "AND e.codempre = c.codempre " &
                                            "AND c.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                                            "GROUP BY c.intcarga " &
                                            "INTO TEMP tmpCargaNE " &
                                            "WITH NO LOG"
                    objCommand.ExecuteNonQuery()

                    '## Temporária ## Geração Hidráulica NE
                    Try
                        objCommand.CommandText = "DROP TABLE tmpHidroNE"
                        objCommand.ExecuteNonQuery()
                    Catch ex As Exception
                        'Se ocorrer o erro, ignora
                    End Try

                    '-- CRQ2997 (03/10/2012) - Novas Usinas Hidráulicas
                    objCommand.CommandText = "SELECT d.intdespa, SUM(NVL(d.valdespasup,0)) AS vlrGerH " &
                                            "FROM empre e, usina u, despa d " &
                                            "WHERE e.codarea = 'NE' " &
                                            "AND e.codempre = u.codempre " &
                                            "AND u.codusina in ('CHUTB ','CHUPE ','CHPHID','CHUAS ','CHUSB ', " &
                                            "                   'CHUBE ','CHUFL ','CHULG ','CHUTC ','CHUSU ', " &
                                            "                   'CHUSD ','CHUST ','CHUSQ ','CHUXG ','IPUITP', " &
                                            "                   'LAPHLG','VCUCV ', " &
                                            "                   'SGPHSG','PFPHRP','PFPHBS','PFPHLG','PFPHPF') " &
                                            "AND u.codusina = d.codusina " &
                                            "AND d.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                                            "GROUP BY d.intdespa " &
                                            "INTO TEMP tmpHidroNE " &
                                            "WITH NO LOG"

                    objCommand.ExecuteNonQuery()

                    '## Temporária ## Geração Térmica NE
                    Try
                        objCommand.CommandText = "DROP TABLE tmpTermoNE"
                        objCommand.ExecuteNonQuery()
                    Catch ex As Exception
                        'Se ocorrer o erro, ignora
                    End Try

                    '-- CRQnnnn (23/10/2012) - Nova Seleção de Usinas Termicas
                    objCommand.CommandText = "SELECT d.intdespa, SUM(NVL(d.valdespasup,0)) AS vlrGerT " &
                                            "FROM empre e, usina u, despa d " &
                                            "WHERE e.codarea = 'NE' " &
                                            "AND e.codempre = u.codempre " &
                                            "AND u.codusina in ('BEBRFZ', 'BLCAMI', 'BLPOLO', 'BLTPRD', 'BLUJSL', " &
                                            "                   'BLUTBC', 'BOUTCG', 'CHPTER', 'CQUECQ', 'GBEGJG', " &
                                            "                   'GCEGCC', 'GCEGCT', 'GCEGJN', 'GCEGPC', 'GCEGTE', " &
                                            "                   'GCEGTI', 'GCEGTU', 'GEBGCP', 'GEBGLG', 'GEBGMD', " &
                                            "                   'GEBGMT', 'GEBGPP', 'GIEGAT', 'GIEGCM', 'GIEGMB', " &
                                            "                   'GIEGNZ', 'GSGIMR', 'JBDJSR', 'KAUTGD', 'KAUTGU', " &
                                            "                   'MUUTMA', 'NGNGCD', 'PBBHPP', 'PFUTSY', 'PMPEPM', " &
                                            "                   'POTCB3', 'POTMCB', 'PTCEPT', 'TDTPRD', 'TMUPF1', " &
                                            "                   'TMUTM',  'UDCAPI', 'UNUTN',  'UNUTPB', 'CHUTC', " &
                                            "                   'BLUTA',  'BLUTFA', 'TPUTPE', 'BLUSCJ', 'SAUTFO') " &
                                            "AND u.codusina = d.codusina " &
                                            "AND d.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                                            "GROUP BY d.intdespa " &
                                            "INTO TEMP tmpTermoNE " &
                                            "WITH NO LOG"

                    objCommand.ExecuteNonQuery()

                    '## Temporária ## Geração Eólica NE
                    Try
                        objCommand.CommandText = "DROP TABLE tmpEoliNE"
                        objCommand.ExecuteNonQuery()
                    Catch ex As Exception
                        'Se ocorrer o erro, ignora
                    End Try

                    '-- CRQnnnn (23/10/2012) - Nova Seleção de Usinas Eolicas
                    objCommand.CommandText = "SELECT d.intdespa, SUM(NVL(d.valdespasup,0)) AS vlrGerE " &
                                            "FROM empre e, usina u, despa d " &
                                            "WHERE e.codarea = 'NE' " &
                                            "AND e.codempre = u.codempre " &
                                            "AND u.codusina in ('ALUEA1', 'ALUEA2', 'BBUEBB', 'BBUEPS', 'BLUEM1', " &
                                            "                   'BLUEM2', 'BLUEM3', 'BLUEM5', 'DSUEMA', 'DSUENH', " &
                                            "                   'DSUESE', 'FOUEFO', 'ICUEIC', 'MPUEPM', 'PPUEPP', " &
                                            "                   'PUEUPU', 'RCUERC', 'UEUEEN', 'VBUEBV', 'VRUEVR', 'NBUERF') " &
                                            "AND u.codusina = d.codusina " &
                                            "AND d.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                                            "GROUP BY d.intdespa " &
                                            "INTO TEMP tmpEoliNE " &
                                            "WITH NO LOG"

                    objCommand.ExecuteNonQuery()

                    '## Temporária ## Intercâmbio NE->N
                    Try
                        objCommand.CommandText = "Drop Table tmpInterNNE"
                        objCommand.ExecuteNonQuery()
                    Catch ex As Exception
                        'Se ocorrer o erro, ignora
                    End Try

                    objCommand.CommandText = "SELECT intinter, SUM(NVL(valintersup,0)) AS vlrInterNNE " &
                                            "FROM inter " &
                                            "WHERE codemprede = 'NE' " &
                                            "AND codemprepara = 'RN' " &
                                            "AND datpdp = '" & Page.Request.QueryString("strData") & "' " &
                                            "GROUP BY intinter " &
                                            "INTO TEMP tmpInterNNE " &
                                            "WITH NO LOG"
                    objCommand.ExecuteNonQuery()

                    ''## Temporária ## Intercâmbio N->SE
                    'Try
                    '    objCommand.CommandText = "DROP TABLE tmpInterNSE"
                    '    objCommand.ExecuteNonQuery()
                    'Catch ex As Exception
                    '    'Se ocorrer o erro, ignora
                    'End Try
                    'objCommand.CommandText = "SELECT intinter, SUM(NVL(valintersup,0)) AS vlrInterNSE " & _
                    '                         "FROM inter " & _
                    '                         "WHERE codemprede = 'RN' " & _
                    '                         "AND codemprepara = 'RE' " & _
                    '                         "AND datpdp = '" & Page.Request.QueryString("strData") & "' " & _
                    '                         "GROUP BY intinter " & _
                    '                         "INTO TEMP tmpInterNSE " & _
                    '                         "WITH NO LOG"
                    'objCommand.ExecuteNonQuery()

                    ''## Temporária ## Intercâmbio SE->NE
                    'Try
                    '    objCommand.CommandText = "DROP TABLE tmpInterSENE"
                    '    objCommand.ExecuteNonQuery()
                    'Catch ex As Exception
                    '    'Se ocorrer o erro, ignora
                    'End Try
                    'objCommand.CommandText = "SELECT intinter, Sum(Nvl(valintersup,0)) As vlrInterSENE " & _
                    '                         "FROM inter " & _
                    '                         "WHERE codemprede = 'NE' " & _
                    '                         "AND codemprepara = 'RE' " & _
                    '                         "AND datpdp = '" & Page.Request.QueryString("strData") & "' " & _
                    '                         "GROUP BY intinter " & _
                    '                         "INTO TEMP tmpInterSENE " & _
                    '                         "WITH NO LOG"
                    'objCommand.ExecuteNonQuery()

                    '## Temporária ## Reserva R2 NE
                    Try
                        objCommand.CommandText = "DROP TABLE tmpReservaNE"
                        objCommand.ExecuteNonQuery()
                    Catch ex As Exception
                        'Se ocorrer o erro, ignora
                    End Try

                    objCommand.CommandText = "SELECT i.intinter AS intervalo, r1.resersec + r2.resersec + i.valintersup AS valor " &
                                            "FROM reser r1, reser r2, inter i " &
                                            "WHERE i.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                                            "AND i.codemprede = 'RN' " &
                                            "AND i.codemprepara = 'NE' " &
                                            "AND i.codcontamodal = 'R2' " &
                                            "AND i.intinter = r1.intreser " &
                                            "AND r1.codempre = 'CH' " &
                                            "AND r1.datareser = '" & strDataReser & "' " &
                                            "AND r1.intreser = r2.intreser " &
                                            "AND r2.codempre = 'NE' " &
                                            "AND r2.datareser = '" & strDataReser & "' " &
                                            "INTO TEMP tmpReservaNE " &
                                            "WITH NO LOG"
                    objCommand.ExecuteNonQuery()

                    'Selecionando dados para o Dataset das tabelas temporárias
                    strSql = "SELECT c.intcarga AS intervalo, " &
                            "isnull(c.vlrcarga,0) AS vlrcarga, " &
                            "isnull(gh.vlrgerh,0) AS vlrgerh, " &
                            "isnull(gt.vlrgert,0) AS vlrgert, " &
                            "isnull(ge.vlrgere,0) AS vlrgere, " &
                            "isnull(i1.vlrinternne,0) As vlrinternne, " &
                            "isnull(p.valpotsincronizadasup,0) AS vlrreservane, " &
                            "isnull(r.valor,0) AS vlrreservar2 " &
                            "FROM tmpCargaNE c, OUTER tmpHidroNE gh, OUTER tmpTermoNE gt, OUTER tmpEoliNE ge, OUTER tmpInterNNE i1, OUTER tmpReservaNE r, OUTER potsincronizada p " &
                            "WHERE c.intcarga = gh.intdespa " &
                            "AND c.intcarga = gt.intdespa " &
                            "AND c.intcarga = ge.intdespa " &
                            "AND c.intcarga = i1.intinter " &
                            "AND c.intcarga = r.intervalo " &
                            "AND c.intcarga = p.intpotsincronizada " &
                            "AND p.codarea = 'NE' " &
                            "AND p.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                            "ORDER BY c.intcarga"
                    strTable = "ReqGerNE"

                    '-- CRQ4434 (14/03/2013) - seleção dos dados através da view VW_RELPDP_NE_REQUISITO_GERACAO
                Case Is = "ReqGerNE"

                    strSql = "SELECT intervalo, vlrcarga , vlrgerh, " &
                             "       vlrgert, vlrgere, vlrinternne, " &
                             "       vlrreservane, vlrreservar2 " &
                             "  FROM VW_RELPDP_NE_REQUISITO_GERACAO " &
                             " WHERE datpdp = '" & Page.Request.QueryString("strData") & "' " &
                             " ORDER BY 1, 2"
                    strTable = "ReqGerNE"

                Case Is = "CargaNCO"
                    strSql = "SELECT 'ÁREA' AS codempre, 'CARGA' AS codusina, intcarga AS intdespa, SUM(valcargasup) AS valdespaemp, 1 AS ordem " &
                             "FROM empre e, carga c " &
                             "WHERE e.codarea = 'RN' " &
                             "AND e.codempre = c.codempre " &
                             "AND c.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                             "GROUP BY 1, 2, 3, 5 " &
                             "UNION ALL " &
                             "SELECT 'SUB' AS codempre, 'CARGA' AS codusina, intervsme AS intdespa, carga AS valdespaemp, 2 AS ordem " &
                             "FROM totsme " &
                             "WHERE sigsme = 'N' " &
                             "AND datpdp = '" & Page.Request.QueryString("strData") & "' " &
                             "UNION ALL " &
                             "SELECT e.nomempre AS codempre, 'PA' AS codusina, c.intcarga AS intdespa, c.valcargasup AS valdespaemp, 3 AS ordem " &
                             "FROM carga c, empre e " &
                             "WHERE e.codempre = 'PA' " &
                             "AND e.codempre = c.codempre " &
                             "AND c.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                             "UNION ALL " &
                             "SELECT e.nomempre AS codempre, 'MA' AS codusina, c.intcarga AS intdespa, c.valcargasup AS valdespaemp, 4 AS ordem " &
                             "FROM carga c, empre e " &
                             "WHERE e.codempre = 'MA' " &
                             "AND e.codempre = c.codempre " &
                             "AND c.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                             "UNION ALL " &
                             "SELECT e.nomempre AS codempre, 'TO' AS codusina, c.intcarga AS intdespa, c.valcargasup AS valdespaemp, 5 AS ordem " &
                             "FROM carga c, empre e " &
                             "WHERE e.codempre = 'ET' " &
                             "AND e.codempre = c.codempre " &
                             "AND c.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                             "UNION ALL " &
                             "SELECT e.nomempre AS codempre, 'GO' AS codusina, c.intcarga AS intdespa, c.valcargasup AS valdespaemp, 6 AS ordem " &
                             "FROM carga c, empre e " &
                             "WHERE e.codempre = 'CG' " &
                             "AND e.codempre = c.codempre " &
                             "AND c.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                             "UNION ALL " &
                             "SELECT e.nomempre AS codempre, 'DF' AS codusina, c.intcarga AS intdespa, c.valcargasup AS valdespaemp, 7 AS ordem " &
                             "FROM carga c, empre e " &
                             "WHERE e.codempre = 'CB' " &
                             "AND e.codempre = c.codempre " &
                             "AND c.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                             "UNION ALL " &
                             "SELECT e.nomempre AS codempre, 'MT' AS codusina, c.intcarga AS intdespa, c.valcargasup AS valdespaemp, 8 AS ordem " &
                             "FROM carga c, empre e " &
                             "WHERE e.codempre = 'CT' " &
                             "AND e.codempre = c.codempre " &
                             "AND c.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                             "UNION ALL " &
                             "SELECT 'LÍQUIDO' AS codempre, 'INTER' AS codusina, intinter AS intdespa, SUM(valintersup) AS valdespaemp, 9 AS ordem " &
                             "FROM inter " &
                             "WHERE codemprede = 'RN' " &
                             "AND datpdp = '" & Page.Request.QueryString("strData") & "' " &
                             "AND (codemprepara = 'NE' " &
                             "OR codemprepara = 'RE') " &
                             "GROUP BY 1, 2, 3 " &
                             "UNION ALL " &
                             "SELECT 'N/NE' AS codempre, 'INTER' AS codusina, intinter AS intdespa, SUM(valintersup)AS valdespaemp, 10 AS ordem " &
                             "FROM inter " &
                             "WHERE codemprede = 'RN' " &
                             "AND codemprepara = 'NE' " &
                             "AND datpdp = '" & Page.Request.QueryString("strData") & "' " &
                             "GROUP BY 1, 2, 3 " &
                             "UNION ALL " &
                             "SELECT 'N/SE' AS codempre, 'INTER' AS codusina, intinter AS intdespa, SUM(valintersup) AS valdespaemp, 11 AS ordem " &
                             "FROM inter " &
                             "WHERE codemprede = 'RN' " &
                             "AND codemprepara = 'RE' " &
                             "AND datpdp = '" & Page.Request.QueryString("strData") & "' " &
                             "GROUP BY 1, 2, 3 " &
                             "UNION ALL " &
                             "SELECT 'N/S' AS codempre, 'INTER' AS codusina, intinter AS intdespa, SUM(valintersup) AS valdespaemp, 12 AS ordem " &
                             "FROM inter " &
                             "WHERE codemprede = 'RN' " &
                             "AND codemprepara = 'RS' " &
                             "AND datpdp = '" & Page.Request.QueryString("strData") & "' " &
                             "GROUP BY 1, 2, 3 " &
                             "ORDER BY 3, 5"
                    strTable = "despa"
                    '-- CRQ5001 - 18/09/2013
                    '-- Linhas acima, começando por UNION ALL / SELECT 'N/S'... e indo até GROUP BY...

                    'Case Is = "InterNCO"
                    '    strSql = "SELECT 'INT LÍQ' AS codempre, intinter AS intcarga, SUM(valintersup) AS valcargasup, 1 AS ordem " & _
                    '             "FROM inter " & _
                    '             "WHERE codemprede = 'RN' " & _
                    '             "AND datpdp = '" & Page.Request.QueryString("strData") & "' " & _
                    '             "AND (codemprepara = 'NE' " & _
                    '             "OR codemprepara = 'RE') " & _
                    '             "GROUP BY 1, 2 " & _
                    '             "UNION ALL " & _
                    '             "SELECT 'INT N/NE' AS codempre, intinter AS intcarga, SUM(valintersup)AS valcargasup, 2 AS ordem " & _
                    '             "FROM inter " & _
                    '             "WHERE codemprede = 'RN' " & _
                    '             "AND codemprepara = 'NE' " & _
                    '             "AND datpdp = '" & Page.Request.QueryString("strData") & "' " & _
                    '             "GROUP BY 1, 2 " & _
                    '             "UNION ALL " & _
                    '             "SELECT 'INT N/SE' AS codempre, intinter AS intcarga, SUM(valintersup) AS valcargasup, 3 AS ordem " & _
                    '             "FROM inter " & _
                    '             "WHERE codemprede = 'RN' " & _
                    '             "AND codemprepara = 'RE' " & _
                    '             "AND datpdp = '" & Page.Request.QueryString("strData") & "' " & _
                    '             "GROUP BY 1, 2 " & _
                    '             "ORDER BY 2, 4"

                    '    strTable = "carga"

                Case Is = "CargaCNOS"
                    'Carga do CNOS
                    'Verifica o tipo da Agregação
                    Select Case Request.QueryString("strAgrega")
                        Case Is = "age"
                            '-- Previsão de Carga por Agente 
                            strSql = "SELECT e.sigempre AS codigo, c.intcarga AS intervalo, NVL(c.valcargasup,0) AS valor " &
                                    "FROM carga c, empre e " &
                                    "WHERE c.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                                    "AND c.codempre = e.codempre " &
                                    "ORDER BY c.intcarga, e.sigempre "
                        Case Is = "are"
                            '-- Previsão de Carga por Área
                            strSql = "SELECT a.nomarea AS codigo, c.intcarga AS intervalo, SUM(NVL(c.valcargasup,0)) AS valor " &
                                    "FROM carga c, empre e, area a " &
                                    "WHERE c.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                                    "AND c.codempre = e.codempre " &
                                    "And e.codarea = a.codarea " &
                                    "GROUP BY a.nomarea, c.intcarga " &
                                    "ORDER BY c.intcarga, a.nomarea"
                        Case Is = "reg"
                            strSql = "SELECT r.sigregia AS sigla, r.nomregia AS codigo, c.intcarga AS intervalo, SUM(NVL(c.valcargasup,0)) AS valor, 1 AS ordem " &
                                    "FROM carga c, empre e, area a, regia r " &
                                    "WHERE c.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                                    "AND c.codempre = e.codempre " &
                                    "AND e.codarea = a.codarea " &
                                    "AND a.sigregia = r.sigregia " &
                                    "AND (r.sigregia = 'N' " &
                                    "OR r.sigregia = 'NE') " &
                                    "GROUP BY 3, 1, 2 " &
                                    "UNION ALL " &
                                    "SELECT r.sigregia AS sigla, r.nomregia AS codigo, c.intcarga AS intervalo, SUM(NVL(c.valcargasup,0)) AS valor, 3 AS ordem " &
                                    "FROM carga c, empre e, area a, regia r " &
                                    "WHERE c.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                                    "AND c.codempre = e.codempre " &
                                    "AND e.codarea = a.codarea " &
                                    "AND a.sigregia = r.sigregia " &
                                    "AND (r.sigregia = 'S' Or r.sigregia = 'SE') " &
                                    "GROUP BY 3, 1, 2 " &
                                    "UNION ALL " &
                                    "SELECT r.codsiste AS sigla, 'Norte Nordeste' AS codigo, c.intcarga AS intervalo, SUM(NVL(c.valcargasup,0)) AS valor, 2 AS ordem " &
                                    "FROM carga c, empre e, area a, regia r, siste s " &
                                    "WHERE c.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                                    "AND c.codempre = e.codempre " &
                                    "AND e.codarea = a.codarea " &
                                    "AND a.sigregia = r.sigregia " &
                                    "AND r.codsiste = s.codsiste " &
                                    "AND s.codsiste = 'NN' " &
                                    "GROUP BY 3, 1, 2 " &
                                    "UNION ALL " &
                                    "SELECT r.codsiste AS sigla, 'Sul Sudeste' AS codigo, c.intcarga AS intervalo, SUM(NVL(c.valcargasup,0)) AS valor, 4 AS ordem " &
                                    "FROM carga c, empre e, area a, regia r, siste s " &
                                    "WHERE c.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                                    "AND c.codempre = e.codempre " &
                                    "AND e.codarea = a.codarea " &
                                    "AND a.sigregia = r.sigregia " &
                                    "AND r.codsiste = s.codsiste " &
                                    "AND s.codsiste = 'SS' " &
                                    "GROUP BY 3, 1, 2 " &
                                    "UNION ALL " &
                                    "SELECT 'SIN' As sigla, 'SIN' As codigo, c.intcarga As intervalo, SUM(NVL(c.valcargasup,0)) AS valor, 5 AS ordem " &
                                    "FROM carga c " &
                                    "WHERE c.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                                    "GROUP BY 3, 1, 2 " &
                                    "ORDER BY 3, 5, 1"
                    End Select
                    strTable = "CargaCNOS"
                Case Is = "GeracaoCNOS"
                    'Geração do CNOS
                    'Verifica o tipo da Agregação
                    Select Case Request.QueryString("strAgrega")
                        Case Is = "age"
                            strSql = "SELECT e.sigempre As codigo, 'HIDRO' AS tipo, d.codusina AS usina, d.intdespa AS intervalo, SUM(NVL(d.valdespasup,0)) AS valor " &
                                    "FROM despa d, usina u, empre e " &
                                    "WHERE d.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                                    "AND d.codusina = u.codusina " &
                                    "AND u.tipusina = 'H' " &
                                    "AND u.codempre = e.codempre " &
                                    "GROUP BY d.intdespa, e.sigempre, d.codusina " &
                                    "UNION ALL " &
                                    "SELECT e.sigempre AS codigo, 'TERMO' AS tipo, d.codusina AS usina, d.intdespa AS intervalo, SUM(NVL(d.valdespasup,0)) AS valor " &
                                    "FROM despa d, usina u, empre e " &
                                    "WHERE d.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                                    "AND d.codusina = u.codusina " &
                                    "AND u.tipusina = 'T' " &
                                    "AND u.codempre = e.codempre " &
                                    "GROUP BY d.intdespa, e.sigempre, d.codusina " &
                                    "UNION ALL " &
                                    "SELECT e.sigempre AS codigo, 'TOTAL' AS tipo, ' ' AS usina, d.intdespa AS intervalo, SUM(NVL(d.valdespasup,0)) AS valor " &
                                    "FROM despa d, usina u, empre e " &
                                    "WHERE d.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                                    "AND d.codusina = u.codusina " &
                                    "AND u.codempre = e.codempre " &
                                    "GROUP BY 4, 1, 3 " &
                                    "ORDER BY 4, 1, 3"
                        Case Is = "are"
                            '-- Geração por Área
                            strSql = "SELECT a.nomarea AS codigo, 'HIDRO' AS tipo, d.codusina AS usina, d.intdespa AS intervalo, SUM(NVL(d.valdespasup,0)) AS valor " &
                                    "FROM despa d, usina u, empre e, area a " &
                                    "WHERE d.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                                    "AND d.codusina = u.codusina " &
                                    "AND u.tipusina = 'H' " &
                                    "AND u.codempre = e.codempre " &
                                    "AND e.codarea = a.codarea " &
                                    "GROUP BY 4, 1, 3 " &
                                    "UNION ALL " &
                                    "SELECT a.nomarea AS codigo, 'TERMO' AS tipo, d.codusina AS usina, d.intdespa AS intervalo, SUM(NVL(d.valdespasup,0)) AS valor " &
                                    "FROM despa d, usina u, empre e, area a " &
                                    "WHERE d.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                                    "AND d.codusina = u.codusina " &
                                    "AND u.tipusina = 'T' " &
                                    "AND u.codempre = e.codempre " &
                                    "AND e.codarea = a.codarea " &
                                    "GROUP BY 4, 1, 3 " &
                                    "UNION ALL " &
                                    "SELECT a.nomarea AS codigo, 'TOTAL' AS tipo, ' ' AS usina, d.intdespa AS intervalo, SUM(NVL(d.valdespasup,0)) AS valor " &
                                    "FROM despa d, usina u, empre e, area a " &
                                    "WHERE d.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                                    "AND d.codusina = u.codusina " &
                                    "AND u.codempre = e.codempre " &
                                    "AND e.codarea = a.codarea " &
                                    "GROUP BY 4, 1, 3 " &
                                    "ORDER BY 4, 1, 3"
                        Case Is = "reg"
                            '-- Geração por Região
                            strSql = "SELECT r.nomregia AS codigo, 'HIDRO' AS tipo, d.codusina AS usina, d.intdespa AS intervalo, SUM(NVL(d.valdespasup,0)) AS valor " &
                                    "FROM despa d, usina u, empre e, area a, regia r " &
                                    "WHERE d.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                                    "AND d.codusina = u.codusina " &
                                    "AND u.tipusina = 'H' " &
                                    "AND u.codempre = e.codempre " &
                                    "AND e.codarea = a.codarea " &
                                    "AND a.sigregia = r.sigregia " &
                                    "GROUP BY 4, 1, 3 " &
                                    "UNION ALL " &
                                    "SELECT r.nomregia AS codigo, 'TERMO' AS tipo, d.codusina AS usina, d.intdespa AS intervalo, SUM(NVL(d.valdespasup,0)) AS valor " &
                                    "FROM despa d, usina u, empre e, area a, regia r " &
                                    "WHERE d.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                                    "AND d.codusina = u.codusina " &
                                    "AND u.tipusina = 'T' " &
                                    "AND u.codempre = e.codempre " &
                                    "AND e.codarea = a.codarea " &
                                    "AND a.sigregia = r.sigregia " &
                                    "GROUP BY 4, 1, 3 " &
                                    "UNION ALL " &
                                    "SELECT r.nomregia As codigo, 'TOTAL' AS tipo, ' ' AS usina, d.intdespa AS intervalo, SUM(NVL(d.valdespasup,0)) AS valor " &
                                    "FROM despa d, usina u, empre e, area a, regia r " &
                                    "WHERE d.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                                    "AND d.codusina = u.codusina " &
                                    "AND u.codempre = e.codempre " &
                                    "AND e.codarea = a.codarea " &
                                    "AND a.sigregia = r.sigregia " &
                                    "GROUP BY 4, 1, 3 " &
                                    "ORDER BY 4, 1, 3"
                        Case Is = "sis"
                            '-- Geração por Subsistema
                            strSql = "SELECT s.nomsiste AS codigo, 'HIDRO' AS tipo, d.codusina AS usina, d.intdespa AS intervalo, SUM(d.valdespasup) AS valor " &
                                    "FROM despa d, usina u, empre e, area a, regia r, siste s " &
                                    "WHERE d.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                                    "AND d.codusina = u.codusina " &
                                    "AND u.tipusina = 'H' " &
                                    "AND u.codempre = e.codempre " &
                                    "AND e.codarea = a.codarea " &
                                    "AND a.sigregia = r.sigregia " &
                                    "AND r.codsiste = s.codsiste " &
                                    "GROUP BY 4, 1, 3 " &
                                    "UNION ALL " &
                                    "SELECT s.nomsiste AS codigo, 'TERMO' AS tipo, d.codusina AS usina, d.intdespa AS intervalo, SUM(d.valdespasup) AS valor " &
                                    "FROM despa d, usina u, empre e, area a, regia r, siste s " &
                                    "WHERE d.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                                    "AND d.codusina = u.codusina " &
                                    "AND u.tipusina = 'T' " &
                                    "AND u.codempre = e.codempre " &
                                    "AND e.codarea = a.codarea " &
                                    "AND a.sigregia = r.sigregia " &
                                    "AND r.codsiste = s.codsiste " &
                                    "GROUP BY 4, 1, 3 " &
                                    "UNION ALL " &
                                    "SELECT s.nomsiste AS codigo, 'TOTAL' AS tipo, ' ' AS usina, d.intdespa AS intervalo, SUM(d.valdespasup) AS valor " &
                                    "FROM despa d, usina u, empre e, area a, regia r, siste s " &
                                    "WHERE d.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                                    "AND d.codusina = u.codusina " &
                                    "AND u.codempre = e.codempre " &
                                    "AND e.codarea = a.codarea " &
                                    "AND a.sigregia = r.sigregia " &
                                    "AND r.codsiste = s.codsiste " &
                                    "GROUP BY 4, 1, 3 " &
                                    "ORDER BY 4, 1, 3"
                        Case Is = "sbr"
                            '-- Geração por SIN
                            strSql = "SELECT 'SIN' AS codigo, 'HIDRO' AS tipo, d.codusina AS usina, d.intdespa AS intervalo, SUM(d.valdespasup) AS valor " &
                                    "FROM despa d, usina u " &
                                    "WHERE d.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                                    "AND d.codusina = u.codusina " &
                                    "AND u.tipusina = 'H' " &
                                    "GROUP BY 4, 1, 3 " &
                                    "UNION ALL " &
                                    "SELECT 'SIN' AS codigo, 'TERMO' AS tipo, d.codusina AS usina, d.intdespa AS intervalo, SUM(d.valdespasup) AS valor " &
                                    "FROM despa d, usina u " &
                                    "WHERE d.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                                    "AND d.codusina = u.codusina " &
                                    "AND u.tipusina = 'T' " &
                                    "GROUP BY 4, 1, 3 " &
                                    "UNION ALL " &
                                    "SELECT 'SIN' AS codigo, 'TOTAL' AS tipo, ' ' AS usina, d.intdespa AS intervalo, SUM(d.valdespasup) AS valor " &
                                    "FROM despa d " &
                                    "WHERE d.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                                    "GROUP BY 4, 1, 3 " &
                                    "ORDER BY 4, 1, 3"
                    End Select
                    strTable = "GeracaoCNOS"
                Case Is = "VazaoCNOS"
                    Select Case Request.QueryString("strAgrega")
                        Case Is = "age"
                            strSql = "SELECT e.sigempre AS empresa, u.nomusina AS usina, b.nombacia AS bacia, SUM(v.valturb) AS turbinada, SUM(v.valvert) AS vertida " &
                                    "FROM empre e, usina u, bacia b, vazao v " &
                                    "WHERE e.codempre = u.codempre " &
                                    "AND u.tipusina = 'H' " &
                                    "AND u.codusina = v.codusina " &
                                    "AND v.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                                    "AND u.codbacia = b.codbacia " &
                                    "AND (v.valturb <> 0 " &
                                    "OR v.valvert <> 0) " &
                                    "GROUP BY e.sigempre, u.nomusina, b.nombacia " &
                                    "ORDER BY e.sigempre, u.nomusina, b.nombacia"
                        Case Is = "bac"
                            strSql = "SELECT b.nombacia AS bacia, u.nomusina AS usina, SUM(v.valturb) AS turbinada, SUM(v.valvert) AS vertida " &
                                    "FROM usina u, bacia b, vazao v " &
                                    "WHERE u.tipusina = 'H' " &
                                    "AND u.codusina = v.codusina " &
                                    "AND v.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                                    "AND u.codbacia = b.codbacia " &
                                    "AND (v.valturb <> 0 " &
                                    "OR v.valvert <> 0) " &
                                    "GROUP BY b.nombacia, u.nomusina " &
                                    "ORDER BY b.nombacia, u.nomusina"
                    End Select
                    strTable = "VazaoCNOS"
                Case Is = "InterCNOS"
                    'Temporária de Intercâmbio
                    Try
                        objCommand.CommandText = "DROP TABLE tmpInterCNOS"
                        objCommand.ExecuteNonQuery()
                    Catch ex As Exception
                        'Se ocorrer o erro, ignora
                    End Try
                    objCommand.CommandText = "SELECT i.codemprede, i.codemprepara, i.codcontamodal, SUM(i.valintersup) AS valor " &
                                            "FROM area a, empre e, inter i " &
                                            "WHERE a.codarea = e.codempre " &
                                            "AND e.codempre = i.codemprede " &
                                            "AND i.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                                            "GROUP BY i.codemprede, i.codemprepara, i.codcontamodal " &
                                            "INTO TEMP tmpInterCNOS " &
                                            "WITH NO LOG"
                    objCommand.ExecuteNonQuery()

                    strSql = "SELECT e1.nomempre AS empresade, e2.codempre AS empresapara, i.codcontamodal AS modalidade, i.intinter AS intervalo, i.valintersup AS valor " &
                            "FROM area a1, empre e1, empre e2, inter i, tmpInterCNOS t " &
                            "WHERE a1.codarea = e1.codempre " &
                            "AND e1.codempre = i.codemprede " &
                            "AND i.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                            "AND i.codemprepara = e2.codempre " &
                            "AND i.codemprede = t.codemprede " &
                            "AND i.codemprepara = t.codemprepara " &
                            "AND i.codcontamodal = t.codcontamodal " &
                            "AND t.valor <> 0 " &
                            "ORDER BY i.intinter, e1.nomempre"
                    strTable = "InterCNOS"
                Case Is = "TransfCNOS"
                    '--Temporária de Transferência de Energia entre Regiões CNOS
                    Try
                        objCommand.CommandText = "DROP TABLE tmpEnergiaCNOS"
                        objCommand.ExecuteNonQuery()
                    Catch ex As Exception
                        'Se ocorrer o erro, ignora
                    End Try

                    objCommand.CommandText = "SELECT intinter AS intervalo, SUM(valintersup) AS valor, 'RS' AS codempre " &
                                            "FROM inter " &
                                            "WHERE codemprede = 'RS' " &
                                            "AND codemprepara = 'RE' " &
                                            "AND (codcontamodal = 'IT' " &
                                            "OR codcontamodal = 'EF') " &
                                            "AND datpdp = '" & Page.Request.QueryString("strData") & "' " &
                                            "GROUP BY intinter " &
                                            "UNION ALL " &
                                            "SELECT intinter AS intervalo, SUM(valintersup) AS valor, 'ER' AS codempre " &
                                            "FROM inter " &
                                            "WHERE codemprede = 'ER' " &
                                            "AND datpdp = '" & Page.Request.QueryString("strData") & "' " &
                                            "GROUP BY intinter " &
                                            "UNION ALL " &
                                            "SELECT intdespa AS intervalo, SUM(valdespasup) AS valor, 'WA' AS codempre " &
                                            "FROM despa " &
                                            "WHERE (codusina = 'TSUTWA' " &
                                            "OR codusina = 'TSUTWE') " &
                                            "AND datpdp = '" & Page.Request.QueryString("strData") & "' " &
                                            "GROUP BY intdespa " &
                                            "UNION ALL " &
                                            "SELECT intinter AS intervalo, SUM(valintersup) AS valor, 'FNE' AS codempre " &
                                            "FROM inter " &
                                            "WHERE codemprede = 'RN' " &
                                            "AND codemprepara = 'NE' " &
                                            "AND datpdp = '" & Page.Request.QueryString("strData") & "' " &
                                            "GROUP BY 1, 3 " &
                                            "UNION ALL " &
                                            "SELECT intinter AS intervalo, SUM(valintersup) AS valor, 'FSENE' AS codempre " &
                                            "FROM inter " &
                                            "WHERE codemprede = 'RE' " &
                                            "AND codemprepara = 'NE' " &
                                            "AND datpdp = '" & Page.Request.QueryString("strData") & "' " &
                                            "GROUP BY 1, 3 " &
                                            "UNION ALL " &
                                            "SELECT intinter AS intervalo, SUM(valintersup) AS valor, 'FNS' AS codempre " &
                                            "FROM inter " &
                                            "WHERE codemprede = 'RN' " &
                                            "AND codemprepara = 'RE' " &
                                            "AND datpdp = '" & Page.Request.QueryString("strData") & "' " &
                                            "GROUP BY 1, 3 " &
                                            "ORDER BY 1, 3 " &
                                            "INTO TEMP tmpEnergiaCNOS " &
                                            "WITH NO LOG"
                    objCommand.ExecuteNonQuery()

                    strSql = "SELECT e1.intervalo, ((e1.valor + (-1 * e2.valor)) - e3.valor) AS vlrrec_sul, (((e1.valor + (-1 * e2.valor)) - e3.valor) + d.valdespasup) AS vlrrec_se, e4.valor AS vlrfne, e5.valor AS vlrfsene, (e4.valor + e5.valor) AS vlrrec_ne, e6.valor AS vlrfns " &
                            "FROM tmpEnergiaCNOS e1, tmpEnergiaCNOS e2, tmpEnergiaCNOS e3, tmpEnergiaCNOS e4, tmpEnergiaCNOS e5, tmpEnergiaCNOS e6, despa d " &
                            "WHERE e1.codempre = 'RS' " &
                            "AND e1.intervalo = e2.intervalo " &
                            "AND e2.codempre = 'ER' " &
                            "AND e2.intervalo = e3.intervalo " &
                            "AND e3.codempre = 'WA' " &
                            "AND e3.intervalo = e4.intervalo " &
                            "AND e4.codempre = 'FNE' " &
                            "AND e4.intervalo = e5.intervalo " &
                            "AND e5.codempre = 'FSENE' " &
                            "AND e5.intervalo = e6.intervalo " &
                            "AND e6.codempre = 'FNS' " &
                            "AND e6.intervalo = d.intdespa " &
                            "AND d.codusina = 'IBIT60' " &
                            "AND datpdp = '" & Page.Request.QueryString("strData") & "' " &
                            "ORDER BY e1.intervalo"
                    strTable = "TransfCNOS"
                Case Is = "RestrCNOS"
                    'Restrições Operativas de Usinas (maior valor entre valtrstrpdpsup e valrestrusisup)
                    'Temporária de Restrições
                    Try
                        objCommand.CommandText = "DROP TABLE tmpRestrCNOS"
                        objCommand.ExecuteNonQuery()
                    Catch ex As Exception
                        'Se ocorrer o erro, ignora
                    End Try
                    objCommand.CommandText = "SELECT r.intrestr, r.codgerad, u.tipusina, u.codusina, " &
                                            "CASE WHEN r.valrestrprppre > r.valrestrusipre " &
                                            "THEN r.valrestrprppre " &
                                            "ELSE r.valrestrusipre END AS valor " &
                                            "FROM restrpdp r, gerad g, usina u " &
                                            "WHERE r.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                                            "AND r.codgerad = g.codgerad " &
                                            "AND g.codusina = u.codusina " &
                                            "INTO TEMP tmpRestrCNOS " &
                                            "WITH NO LOG"
                    objCommand.ExecuteNonQuery()

                    Select Case Request.QueryString("strAgrega")
                        Case Is = "age"
                            '-- Restrições por Agente
                            strSql = "Select r.intrestr As intervalo, " &
                                    "       e.sigempre As codigo, " &
                                    "       r.tipusina As tipo, " &
                                    "       r.codusina As usina, " &
                                    "       Sum(r.valor) As valor " &
                                    "From empre e, usina u, gerad g, tmpRestrCNOS r " &
                                    "Where r.codgerad = g.codgerad And " &
                                    "      g.codusina = u.codusina And " &
                                    "      u.codempre = e.codempre " &
                                    "Group By r.intrestr, e.sigempre, r.tipusina, r.codusina " &
                                    "Order By r.intrestr, e.sigempre, r.tipusina, r.codusina"
                        Case Is = "are"
                            '-- Restrições por Área
                            strSql = "Select r.intrestr As intervalo, " &
                                    "       a.nomarea As codigo, " &
                                    "       r.tipusina As tipo, " &
                                    "       r.codusina As usina, " &
                                    "       Sum(r.valor) As valor " &
                                    "From area a, empre e, usina u, gerad g, tmpRestrCNOS r " &
                                    "Where r.codgerad = g.codgerad And " &
                                    "      g.codusina = u.codusina And " &
                                    "      u.codempre = e.codempre And " &
                                    "      e.codarea = a.codarea " &
                                    "Group By r.intrestr, a.nomarea, r.tipusina, r.codusina " &
                                    "Order By r.intrestr, a.nomarea, r.tipusina, r.codusina"
                        Case Is = "reg"
                            '-- Restrições por Região
                            strSql = "Select r.intrestr As intervalo, " &
                                    "       re.nomregia As codigo, " &
                                    "       r.tipusina As tipo, " &
                                    "       r.codusina As usina, " &
                                    "       Sum(r.valor) As valor " &
                                    "From regia re, area a, empre e, usina u, gerad g, tmpRestrCNOS r " &
                                    "Where r.codgerad = g.codgerad And " &
                                    "      g.codusina = u.codusina And " &
                                    "      u.codempre = e.codempre And " &
                                    "      e.codarea = a.codarea And " &
                                    "      a.sigregia = re.sigregia " &
                                    "Group By r.intrestr, re.nomregia, r.tipusina, r.codusina " &
                                    "Order By r.intrestr, re.nomregia, r.tipusina, r.codusina"
                        Case Is = "sis"
                            '-- Restrições por Sistema
                            strSql = "Select r.intrestr As intervalo, " &
                                    "       s.nomsiste As codigo, " &
                                    "       r.tipusina As tipo, " &
                                    "       r.codusina As usina, " &
                                    "       Sum(r.valor) As valor " &
                                    "From siste s, regia re, area a, empre e, usina u, gerad g, tmpRestrCNOS r " &
                                    "Where r.codgerad = g.codgerad And " &
                                    "      g.codusina = u.codusina And " &
                                    "      u.codempre = e.codempre And " &
                                    "      e.codarea = a.codarea And " &
                                    "      a.sigregia = re.sigregia And " &
                                    "      re.codsiste = s.codsiste " &
                                    "Group By r.intrestr, s.nomsiste, r.tipusina, r.codusina " &
                                    "Order By r.intrestr, s.nomsiste, r.tipusina, r.codusina"
                        Case Is = "sbr"
                            '-- Restrições por SIN
                            strSql = "Select r.intrestr As intervalo, " &
                                    "       'SIN' As codigo, " &
                                    "       r.tipusina As tipo, " &
                                    "       r.codusina As usina, " &
                                    "       Sum(r.valor) As valor " &
                                    "From tmpRestrCNOS r " &
                                    "Group By 1, 2, 3, 4 " &
                                    "Order By 1, 2, 3, 4"
                    End Select
                    'Restrição do CNOS usa o mesmo dataset de Carga CNOS
                    strTable = "GeracaoCNOS"
                Case Is = "ReservaCNOS"

                    'Elimina todas as tabelas temporárias
                    '--Manutenção
                    Try
                        objCommand.CommandText = "Drop Table tmpManutencaoCNOS"
                        objCommand.ExecuteNonQuery()
                    Catch ex As Exception
                        'Se ocorrer o erro, ignora
                    End Try

                    '--Restrição
                    Try
                        objCommand.CommandText = "Drop Table tmpRestrCNOS"
                        objCommand.ExecuteNonQuery()
                    Catch ex As Exception
                        'Se ocorrer o erro, ignora
                    End Try

                    '-- Geração
                    Try
                        objCommand.CommandText = "Drop Table tmpGeracaoCNOS"
                        objCommand.ExecuteNonQuery()
                    Catch ex As Exception
                        'Se ocorrer o erro, ignora
                    End Try

                    '-- Reserva
                    Try
                        objCommand.CommandText = "Drop Table tmpReservaCNOS"
                        objCommand.ExecuteNonQuery()
                    Catch ex As Exception
                        'Se ocorrer o erro, ignora
                    End Try

                    '-- Potência Instalada
                    Try
                        objCommand.CommandText = "Drop Table tmpPotenciaCNOS"
                        objCommand.ExecuteNonQuery()
                    Catch ex As Exception
                        'Se ocorrer o erro, ignora
                    End Try

                    Select Case Request.QueryString("strAgrega")
                        Case Is = "age"
                            '--Manutenção
                            objCommand.CommandText = "Select e.sigempre As codigo, p.intparal As intervalo, Sum(g.capgerad) as vlrManutencao " &
                                                    "From empre e, usina u, gerad g, paralpdp p " &
                                                    "Where e.codempre = u.codempre And " &
                                                    "      u.tipusina = 'H' And " &
                                                    "      u.codusina = g.codusina And " &
                                                    "      g.codgerad = p.codequip And " &
                                                    "      p.datpdp = '" & Page.Request.QueryString("strData") & "' And " &
                                                    "      p.paralsup = 1 " &
                                                    "Group By 1, 2 " &
                                                    "Into Temp tmpManutencaoCNOS " &
                                                    "With No Log"
                            objCommand.ExecuteNonQuery()

                            '--Restrição
                            objCommand.CommandText = "Select r.intrestr As intervalo, e.sigempre As codigo, Sum(Case When r.valrestrprppre > r.valrestrusipre Then r.valrestrprppre Else r.valrestrusipre End) As vlrRestricao " &
                                                    "From empre e, usina u, gerad g, restrpdp r " &
                                                    "Where e.codempre = u.codempre And " &
                                                    "      u.tipusina = 'H' And " &
                                                    "      u.codusina = g.codusina And " &
                                                    "      g.codgerad = r.codgerad And " &
                                                    "      r.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                                                    "Group By 1, 2 " &
                                                    "Into Temp tmpRestrCNOS " &
                                                    "With No Log"
                            objCommand.ExecuteNonQuery()

                            '-- Geração
                            objCommand.CommandText = "Select e.sigempre As codigo, d.intdespa As intervalo, Sum(valdespapre) As vlrGeracao " &
                                                    "From empre e, usina u, despa d " &
                                                    "Where e.codempre = u.codempre And " &
                                                    "      u.tipusina = 'H' And " &
                                                    "      u.codusina = d.codusina And " &
                                                    "      d.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                                                    "Group By 1, 2 " &
                                                    "Into Temp tmpGeracaoCNOS " &
                                                    "With No Log"
                            objCommand.ExecuteNonQuery()

                            '-- Reserva
                            objCommand.CommandText = "Select e.sigempre As codigo, r.intreser As intervalo, Sum((Nvl(r.reserpri,0) + Nvl(r.resersec,0) + Nvl(r.reserter,0))) As vlrReserva " &
                                                    "From empre e, reser r " &
                                                    "Where r.datareser = '" & strDataReser & "' And " &
                                                    "      r.codempre = e.codempre " &
                                                    "Group By r.intreser, e.sigempre " &
                                                    "Into Temp tmpReservaCNOS " &
                                                    "With No Log"
                            objCommand.ExecuteNonQuery()

                            '-- Potência Instalada
                            objCommand.CommandText = "Select e.sigempre As codigo, r.intreser As intervalo, Sum(u.potinstalada) As vlrPotencia " &
                                                    "From empre e, reser r, usina u " &
                                                    "Where e.codempre = u.codempre And " &
                                                    "      u.tipusina = 'H' And " &
                                                    "      e.codempre = r.codempre And " &
                                                    "      r.datareser = '" & strDataReser & "' " &
                                                    "Group By 1, 2 " &
                                                    "Into Temp tmpPotenciaCNOS " &
                                                    "With No Log"
                            objCommand.ExecuteNonQuery()
                        Case Is = "are"
                            '--Manutenção
                            objCommand.CommandText = "Select a.nomarea As codigo, p.intparal As intervalo, Sum(g.capgerad) as vlrManutencao " &
                                                    "From area a, empre e, usina u, gerad g, paralpdp p " &
                                                    "Where a.codarea = e.codarea And " &
                                                    "      e.codempre = u.codempre And " &
                                                    "      u.tipusina = 'H' And " &
                                                    "      u.codusina = g.codusina And " &
                                                    "      g.codgerad = p.codequip And " &
                                                    "      p.datpdp = '" & Page.Request.QueryString("strData") & "' And " &
                                                    "      p.paralsup = 1 " &
                                                    "Group By 1, 2 " &
                                                    "Into Temp tmpManutencaoCNOS " &
                                                    "With No Log"
                            objCommand.ExecuteNonQuery()

                            '--Restrição
                            objCommand.CommandText = "Select a.nomarea As codigo, r.intrestr As intervalo, Sum(Case When r.valrestrprppre > r.valrestrusipre Then r.valrestrprppre Else r.valrestrusipre End) As vlrRestricao " &
                                                    "From area a, empre e, usina u, gerad g, restrpdp r " &
                                                    "Where a.codarea = e.codarea And " &
                                                    "      e.codempre = u.codempre And " &
                                                    "      u.tipusina = 'H' And " &
                                                    "      u.codusina = g.codusina And " &
                                                    "      g.codgerad = r.codgerad And " &
                                                    "      r.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                                                    "Group By 1, 2 " &
                                                    "Into Temp tmpRestrCNOS " &
                                                    "With No Log"
                            objCommand.ExecuteNonQuery()

                            '-- Geração
                            objCommand.CommandText = "Select a.nomarea As codigo, d.intdespa As intervalo, Sum(valdespapre) As vlrGeracao " &
                                                    "From area a, empre e, usina u, despa d " &
                                                    "Where a.codarea = e.codarea And " &
                                                    "      e.codempre = u.codempre And " &
                                                    "      u.tipusina = 'H' And " &
                                                    "      u.codusina = d.codusina And " &
                                                    "      d.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                                                    "Group By 1, 2 " &
                                                    "Into Temp tmpGeracaoCNOS " &
                                                    "With No Log"
                            objCommand.ExecuteNonQuery()

                            '-- Reserva
                            objCommand.CommandText = "Select a.nomarea As codigo, r.intreser As intervalo, Sum((Nvl(r.reserpri,0) + Nvl(r.resersec,0) + Nvl(r.reserter,0))) As vlrReserva " &
                                                    "From area a, empre e, reser r " &
                                                    "Where a.codarea = e.codarea And " &
                                                    "      e.codempre = r.codempre And " &
                                                    "      r.datareser = '" & strDataReser & "' " &
                                                    "Group By 1, 2 " &
                                                    "Into Temp tmpReservaCNOS " &
                                                    "With No Log"
                            objCommand.ExecuteNonQuery()

                            '-- Potência Instalada
                            objCommand.CommandText = "Select a.nomarea As codigo, r.intreser As intervalo, Sum(u.potinstalada) As vlrPotencia " &
                                                    "From area a, empre e, reser r, usina u " &
                                                    "Where a.codarea = e.codarea And " &
                                                    "      e.codempre = u.codempre And " &
                                                    "      u.tipusina = 'H' And " &
                                                    "      e.codempre = r.codempre And " &
                                                    "      r.datareser = '" & strDataReser & "' " &
                                                    "Group By 1, 2 " &
                                                    "Into Temp tmpPotenciaCNOS " &
                                                    "With No Log"
                            objCommand.ExecuteNonQuery()
                        Case Is = "reg"
                            '--Manutenção
                            objCommand.CommandText = "Select r.nomregia As codigo, p.intparal As intervalo, Sum(g.capgerad) as vlrManutencao " &
                                                    "From regia r, area a, empre e, usina u, gerad g, paralpdp p " &
                                                    "Where r.sigregia = a.sigregia And " &
                                                    "      a.codarea = e.codarea And " &
                                                    "      e.codempre = u.codempre And " &
                                                    "      u.tipusina = 'H' And " &
                                                    "      u.codusina = g.codusina And " &
                                                    "      g.codgerad = p.codequip And " &
                                                    "      p.datpdp = '" & Page.Request.QueryString("strData") & "' And " &
                                                    "      p.paralsup = 1 " &
                                                    "Group By 1, 2 " &
                                                    "Into Temp tmpManutencaoCNOS " &
                                                    "With No Log"
                            objCommand.ExecuteNonQuery()

                            '--Restrição
                            objCommand.CommandText = "Select re.nomregia As codigo, r.intrestr As intervalo, Sum(Case When r.valrestrprppre > r.valrestrusipre Then r.valrestrprppre Else r.valrestrusipre End) As vlrRestricao " &
                                                    "From regia re, area a, empre e, usina u, gerad g, restrpdp r " &
                                                    "Where re.sigregia = a.sigregia And " &
                                                    "      a.codarea = e.codarea And " &
                                                    "      e.codempre = u.codempre And " &
                                                    "      u.tipusina = 'H' And " &
                                                    "      u.codusina = g.codusina And " &
                                                    "      g.codgerad = r.codgerad And " &
                                                    "      r.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                                                    "Group By 1, 2 " &
                                                    "Into Temp tmpRestrCNOS " &
                                                    "With No Log"
                            objCommand.ExecuteNonQuery()

                            '-- Geração
                            objCommand.CommandText = "Select r.nomregia As codigo, d.intdespa As intervalo, Sum(valdespapre) As vlrGeracao " &
                                                    "From regia r, area a, empre e, usina u, despa d " &
                                                    "Where r.sigregia = a.sigregia And " &
                                                    "      a.codarea = e.codarea And " &
                                                    "      e.codempre = u.codempre And " &
                                                    "      u.tipusina = 'H' And " &
                                                    "      u.codusina = d.codusina And " &
                                                    "      d.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                                                    "Group By 1, 2 " &
                                                    "Into Temp tmpGeracaoCNOS " &
                                                    "With No Log"
                            objCommand.ExecuteNonQuery()

                            '-- Reserva
                            objCommand.CommandText = "Select re.nomregia As codigo, r.intreser As intervalo, Sum((Nvl(r.reserpri,0) + Nvl(r.resersec,0) + Nvl(r.reserter,0))) As vlrReserva " &
                                                    "From regia re, area a, empre e, reser r " &
                                                    "Where re.sigregia = a.sigregia And " &
                                                    "      a.codarea = e.codarea And " &
                                                    "      e.codempre = r.codempre And " &
                                                    "      r.datareser = '" & strDataReser & "' " &
                                                    "Group By 1, 2 " &
                                                    "Into Temp tmpReservaCNOS " &
                                                    "With No Log"
                            objCommand.ExecuteNonQuery()

                            '-- Potência Instalada
                            objCommand.CommandText = "Select re.nomregia As codigo, r.intreser As intervalo, Sum(u.potinstalada) As vlrPotencia " &
                                                    "From regia re, area a, empre e, reser r, usina u " &
                                                    "Where re.sigregia = a.sigregia And " &
                                                    "      a.codarea = e.codarea And " &
                                                    "      e.codempre = u.codempre And " &
                                                    "      u.tipusina = 'H' And " &
                                                    "      e.codempre = r.codempre And " &
                                                    "      r.datareser = '" & strDataReser & "' " &
                                                    "Group By 1, 2 " &
                                                    "Into Temp tmpPotenciaCNOS " &
                                                    "With No Log"
                            objCommand.ExecuteNonQuery()

                        Case Is = "sis"
                            '--Manutenção
                            objCommand.CommandText = "Select s.nomsiste As codigo, p.intparal As intervalo, Sum(g.capgerad) as vlrManutencao " &
                                                    "From siste s, regia r, area a, empre e, usina u, gerad g, paralpdp p " &
                                                    "Where s.codsiste = r.codsiste And " &
                                                    "      r.sigregia = a.sigregia And " &
                                                    "      a.codarea = e.codarea And " &
                                                    "      e.codempre = u.codempre And " &
                                                    "      u.tipusina = 'H' And " &
                                                    "      u.codusina = g.codusina And " &
                                                    "      g.codgerad = p.codequip And " &
                                                    "      p.datpdp = '" & Page.Request.QueryString("strData") & "' And " &
                                                    "      p.paralsup = 1 " &
                                                    "Group By 1, 2 " &
                                                    "Into Temp tmpManutencaoCNOS " &
                                                    "With No Log"
                            objCommand.ExecuteNonQuery()

                            '--Restrição
                            objCommand.CommandText = "Select s.nomsiste As codigo, r.intrestr As intervalo, Sum(Case When r.valrestrprppre > r.valrestrusipre Then r.valrestrprppre Else r.valrestrusipre End) As vlrRestricao " &
                                                    "From siste s, regia re, area a, empre e, usina u, gerad g, restrpdp r " &
                                                    "Where s.codsiste = re.codsiste And " &
                                                    "      re.sigregia = a.sigregia And " &
                                                    "      a.codarea = e.codarea And " &
                                                    "      e.codempre = u.codempre And " &
                                                    "      u.tipusina = 'H' And " &
                                                    "      u.codusina = g.codusina And " &
                                                    "      g.codgerad = r.codgerad And " &
                                                    "      r.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                                                    "Group By 1, 2 " &
                                                    "Into Temp tmpRestrCNOS " &
                                                    "With No Log"
                            objCommand.ExecuteNonQuery()

                            '-- Geração
                            objCommand.CommandText = "Select s.nomsiste As codigo, d.intdespa As intervalo, Sum(valdespapre) As vlrGeracao " &
                                                    "From siste s, regia r, area a, empre e, usina u, despa d " &
                                                    "Where s.codsiste = r.codsiste And " &
                                                    "      r.sigregia = a.sigregia And " &
                                                    "      a.codarea = e.codarea And " &
                                                    "      e.codempre = u.codempre And " &
                                                    "      u.tipusina = 'H' And " &
                                                    "      u.codusina = d.codusina And " &
                                                    "      d.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                                                    "Group By 1, 2 " &
                                                    "Into Temp tmpGeracaoCNOS " &
                                                    "With No Log"
                            objCommand.ExecuteNonQuery()

                            '-- Reserva
                            objCommand.CommandText = "Select s.nomsiste As codigo, r.intreser As intervalo, Sum((Nvl(r.reserpri,0) + Nvl(r.resersec,0) + Nvl(r.reserter,0))) As vlrReserva " &
                                                    "From siste s, regia re, area a, empre e, reser r " &
                                                    "Where s.codsiste = re.codsiste And " &
                                                    "      re.sigregia = a.sigregia And " &
                                                    "      a.codarea = e.codarea And " &
                                                    "      e.codempre = r.codempre And " &
                                                    "      r.datareser = '" & strDataReser & "' " &
                                                    "Group By 1, 2 " &
                                                    "Into Temp tmpReservaCNOS " &
                                                    "With No Log"
                            objCommand.ExecuteNonQuery()

                            '-- Potência Instalada
                            objCommand.CommandText = "Select s.nomsiste As codigo, r.intreser As intervalo, Sum(u.potinstalada) As vlrPotencia " &
                                                    "From siste s, regia re, area a, empre e, reser r, usina u " &
                                                    "Where s.codsiste = re.codsiste And " &
                                                    "      re.sigregia = a.sigregia And " &
                                                    "      a.codarea = e.codarea And " &
                                                    "      e.codempre = u.codempre And " &
                                                    "      u.tipusina = 'H' And " &
                                                    "      e.codempre = r.codempre And " &
                                                    "      r.datareser = '" & strDataReser & "' " &
                                                    "Group By 1, 2 " &
                                                    "Into Temp tmpPotenciaCNOS " &
                                                    "With No Log"
                            objCommand.ExecuteNonQuery()
                        Case Is = "sbr"
                            '--Manutenção
                            objCommand.CommandText = "Select 'SIN' As codigo, p.intparal As intervalo, Sum(g.capgerad) as vlrManutencao " &
                                                    "From usina u, gerad g, paralpdp p " &
                                                    "Where u.tipusina = 'H' And " &
                                                    "      u.codusina = g.codusina And " &
                                                    "      g.codgerad = p.codequip And " &
                                                    "      p.datpdp = '" & Page.Request.QueryString("strData") & "' And " &
                                                    "      p.paralsup = 1 " &
                                                    "Group By 1, 2 " &
                                                    "Into Temp tmpManutencaoCNOS " &
                                                    "With No Log"
                            objCommand.ExecuteNonQuery()

                            '--Restrição
                            objCommand.CommandText = "Select 'SIN' As codigo, r.intrestr As intervalo, Sum(Case When r.valrestrprppre > r.valrestrusipre Then r.valrestrprppre Else r.valrestrusipre End) As vlrRestricao " &
                                                    "From usina u, gerad g, restrpdp r " &
                                                    "Where u.tipusina = 'H' And " &
                                                    "      u.codusina = g.codusina And " &
                                                    "      g.codgerad = r.codgerad And " &
                                                    "      r.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                                                    "Group By 1, 2 " &
                                                    "Into Temp tmpRestrCNOS " &
                                                    "With No Log"
                            objCommand.ExecuteNonQuery()

                            '-- Geração
                            objCommand.CommandText = "Select 'SIN' As codigo, d.intdespa As intervalo, Sum(valdespapre) As vlrGeracao " &
                                                    "From usina u, despa d " &
                                                    "Where u.tipusina = 'H' And " &
                                                    "      u.codusina = d.codusina And " &
                                                    "      d.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                                                    "Group By 1, 2 " &
                                                    "Into Temp tmpGeracaoCNOS " &
                                                    "With No Log"
                            objCommand.ExecuteNonQuery()

                            '-- Reserva
                            objCommand.CommandText = "Select 'SIN' As codigo, r.intreser As intervalo, Sum((Nvl(r.reserpri,0) + Nvl(r.resersec,0) + Nvl(r.reserter,0))) As vlrReserva " &
                                                    "From reser r " &
                                                    "Where r.datareser = '" & strDataReser & "' " &
                                                    "Group By 1, 2 " &
                                                    "Into Temp tmpReservaCNOS " &
                                                    "With No Log"
                            objCommand.ExecuteNonQuery()

                            '-- Potência Instalada
                            objCommand.CommandText = "Select 'SIN' As codigo, r.intreser As intervalo, Sum(u.potinstalada) As vlrPotencia " &
                                                    "From empre e, reser r, usina u " &
                                                    "Where e.codempre = u.codempre And " &
                                                    "      u.tipusina = 'H' And " &
                                                    "      e.codempre = r.codempre And " &
                                                    "      r.datareser = '" & strDataReser & "' " &
                                                    "Group By 1, 2 " &
                                                    "Into Temp tmpPotenciaCNOS " &
                                                    "With No Log"
                            objCommand.ExecuteNonQuery()
                    End Select
                    'Estou usando o data set de Carga para não precisar criar outro para reserva

                    '-- Reserva de Potência por Agente
                    strSql = "Select intervalo, codigo, vlrpotencia As valor, 'CAPACID' As sigla, 1 As ordem " &
                            "From tmpPotenciaCNOS " &
                            "Union All " &
                            "Select r.intervalo, r.codigo, r.vlrreserva As valor, 'RESERVA' As sigla, 2 As ordem " &
                            "From tmpReservaCNOS r, tmpPotenciaCNOS p " &
                            "Where r.codigo = p.codigo And " &
                            "      r.intervalo = p.intervalo " &
                            "Union All " &
                            "Select intervalo, codigo, vlrgeracao As valor, 'GERAÇÃO' As sigla, 3 As ordem " &
                            "From tmpGeracaoCNOS " &
                            "Union All " &
                            "Select intervalo, codigo, vlrrestricao As valor, 'RESTR' As sigla, 4 As ordem " &
                            "From tmpRestrCNOS " &
                            "Union All " &
                            "Select intervalo, codigo, vlrmanutencao As valor, 'MANUT' As sigla, 5 As ordem " &
                            "From tmpManutencaoCNOS " &
                            "Union All " &
                            "Select p.intervalo, p.codigo, (nvl(p.vlrpotencia, 0) - Nvl(rs.vlrreserva, 0) - Nvl(g.vlrgeracao, 0) - Nvl(rt.vlrrestricao, 0) - Nvl(m.vlrmanutencao, 0)) As valor, 'SALDO' As sigla, 6 As ordem " &
                            "From tmpPotenciaCNOS p, Outer tmpReservaCNOS rs, Outer tmpGeracaoCNOS g, Outer tmpRestrCNOS rt, Outer tmpManutencaoCNOS m " &
                            "Where p.codigo = rs.codigo And " &
                            "      p.intervalo = rs.intervalo And " &
                            "      p.codigo = g.codigo And " &
                            "      p.intervalo = g.intervalo And " &
                            "      p.codigo = rt.codigo And " &
                            "      p.intervalo = rt.intervalo And " &
                            "      p.codigo = m.codigo And " &
                            "      p.intervalo = m.intervalo " &
                            "Order By 1, 2, 5"
                    strTable = "CargaCNOS"
                Case Is = "RazaoCNOS"
                    Select Case Request.QueryString("strAgrega")
                        Case Is = "age"
                            '-- Razões do Despacho das Usinas Térmicas por Agente
                            strSql = "Select e.sigempre As codigo, 'IFX' As tipo, ifx.intflexi As intervalo, Sum(ifx.valflexisup) As valor " &
                                    "From inflexibilidade ifx, usina u, empre e " &
                                    "Where  ifx.datpdp = '" & Page.Request.QueryString("strData") & "' And " &
                                    "       ifx.codusina = u.codusina And " &
                                    "       u.tipusina = 'T' And " &
                                    "       u.codempre = e.codempre " &
                                    "Group By 3, 1 " &
                                    "Union " &
                                    "Select e.sigempre As codigo, 'ZEL' As tipo, zel.intrazelet As intervalo, Sum(zel.valrazeletsup) As valor " &
                                    "From razaoelet zel, usina u, empre e " &
                                    "Where zel.datpdp = '" & Page.Request.QueryString("strData") & "' And " &
                                    "      zel.codusina = u.codusina And " &
                                    "      u.tipusina = 'T' And " &
                                    "      u.codempre = e.codempre " &
                                    "Group By 3, 1 " &
                                    "Union " &
                                    "Select e.sigempre As codigo, 'EXP' As tipo, exp.intexporta As intervalo, Sum(exp.valexportasup) As valor " &
                                    "From exporta exp, usina u, empre e " &
                                    "Where exp.datpdp = '" & Page.Request.QueryString("strData") & "' And " &
                                    "      exp.codusina = u.codusina And " &
                                    "      u.tipusina = 'T' And " &
                                    "      u.codempre = e.codempre " &
                                    "Group By 3, 1 " &
                                    "Union " &
                                    "Select e.sigempre As codigo, 'ZEN' As tipo, zen.intrazener As intervalo, Sum(zen.valrazenersup) As valor " &
                                    "From razaoener zen, usina u, empre e " &
                                    "Where zen.datpdp = '" & Page.Request.QueryString("strData") & "' And " &
                                    "      zen.codusina = u.codusina And " &
                                    "      u.tipusina = 'T' And " &
                                    "      u.codempre = e.codempre " &
                                    "Group By 3, 1 " &
                                    "Order By 3, 1"
                        Case Is = "are"
                            '-- Razões do Despacho das Usinas Térmicas por Área
                            strSql = "Select a.nomarea As codigo, 'IFX' As tipo, ifx.intflexi As intervalo, Sum(ifx.valflexisup) As valor " &
                                    "From inflexibilidade ifx, usina u, empre e, area a " &
                                    "Where ifx.datpdp = '" & Page.Request.QueryString("strData") & "' And " &
                                    "      ifx.codusina = u.codusina And " &
                                    "      u.tipusina = 'T' And " &
                                    "      u.codempre = e.codempre And " &
                                    "      e.codarea = a.codarea " &
                                    "Group By 3, 1 " &
                                    "Union " &
                                    "Select a.nomarea As codigo, 'ZEL' As tipo, zel.intrazelet As intervalo, Sum(zel.valrazeletsup) As valor " &
                                    "From razaoelet zel, usina u, empre e, area a " &
                                    "Where zel.datpdp = '" & Page.Request.QueryString("strData") & "' And " &
                                    "      zel.codusina = u.codusina And " &
                                    "      u.tipusina = 'T' And " &
                                    "      u.codempre = e.codempre And " &
                                    "      e.codarea = a.codarea " &
                                    "Group By 3, 1 " &
                                    "Union " &
                                    "Select a.nomarea As codigo, 'EXP' As tipo, exp.intexporta As intervalo, Sum(exp.valexportasup) As valor " &
                                    "From exporta exp, usina u, empre e, area a " &
                                    "Where exp.datpdp = '" & Page.Request.QueryString("strData") & "' And " &
                                    "      exp.codusina = u.codusina And " &
                                    "      u.tipusina = 'T' And " &
                                    "      u.codempre = e.codempre And " &
                                    "      e.codarea = a.codarea " &
                                    "Group By 3, 1 " &
                                    "Union " &
                                    "Select a.nomarea As codigo, 'ZEN' As tipo, zen.intrazener As intervalo, Sum(zen.valrazenersup) As valor " &
                                    "From razaoener zen, usina u, empre e, area a " &
                                    "Where zen.datpdp = '" & Page.Request.QueryString("strData") & "' And " &
                                    "      zen.codusina = u.codusina And " &
                                    "      u.tipusina = 'T' And " &
                                    "      u.codempre = e.codempre And " &
                                    "      e.codarea = a.codarea " &
                                    "Group By 3, 1 " &
                                    "Order By 3, 1"
                        Case Is = "reg"
                            '-- Razões do Despacho das Usinas Térmicas por Região
                            strSql = "Select r.nomregia As codigo, 'IFX' As tipo, ifx.intflexi As intervalo, Sum(ifx.valflexisup) As valor " &
                                    "From inflexibilidade ifx, usina u, empre e, area a, regia r " &
                                    "Where ifx.datpdp = '" & Page.Request.QueryString("strData") & "' And " &
                                    "      ifx.codusina = u.codusina And " &
                                    "      u.tipusina = 'T' And " &
                                    "      u.codempre = e.codempre And " &
                                    "      e.codarea = a.codarea And " &
                                    "      a.sigregia = r.sigregia " &
                                    "Group By 3, 1 " &
                                    "Union " &
                                    "Select r.nomregia As codigo, 'ZEL' As tipo, zel.intrazelet As intervalo, Sum(zel.valrazeletsup) As valor " &
                                    "From razaoelet zel, usina u, empre e, area a, regia r " &
                                    "Where zel.datpdp = '" & Page.Request.QueryString("strData") & "' And " &
                                    "      zel.codusina = u.codusina And " &
                                    "      u.tipusina = 'T' And " &
                                    "      u.codempre = e.codempre And " &
                                    "      e.codarea = a.codarea And " &
                                    "      a.sigregia = r.sigregia " &
                                    "Group By 3, 1 " &
                                    "Union " &
                                    "Select r.nomregia As codigo, 'EXP' As tipo, exp.intexporta As intervalo, Sum(exp.valexportasup) As valor " &
                                    "From exporta exp, usina u, empre e, area a, regia r " &
                                    "Where exp.datpdp = '" & Page.Request.QueryString("strData") & "' And " &
                                    "      exp.codusina = u.codusina And " &
                                    "      u.tipusina = 'T' And " &
                                    "      u.codempre = e.codempre And " &
                                    "      e.codarea = a.codarea And " &
                                    "      a.sigregia = r.sigregia " &
                                    "Group By 3, 1 " &
                                    "Union " &
                                    "Select r.nomregia As codigo, 'ZEN' As tipo, zen.intrazener As intervalo, Sum(zen.valrazenersup) As valor " &
                                    "From razaoener zen, usina u, empre e, area a, regia r " &
                                    "Where zen.datpdp = '" & Page.Request.QueryString("strData") & "' And " &
                                    "      zen.codusina = u.codusina And " &
                                    "      u.tipusina = 'T' And " &
                                    "      u.codempre = e.codempre And " &
                                    "      e.codarea = a.codarea And " &
                                    "      a.sigregia = r.sigregia " &
                                    "Group By 3, 1 " &
                                    "Order By 3, 1"
                        Case Is = "sis"
                            '-- Razões do Despacho das Usinas Térmicas por Subsistema
                            strSql = "Select s.nomsiste As codigo, 'IFX' As tipo, ifx.intflexi As intervalo, Sum(ifx.valflexisup) As valor " &
                                    "From inflexibilidade ifx, usina u, empre e, area a, regia r, siste s " &
                                    "Where ifx.datpdp = '" & Page.Request.QueryString("strData") & "' And " &
                                    "      ifx.codusina = u.codusina And " &
                                    "      u.tipusina = 'T' And " &
                                    "      u.codempre = e.codempre And " &
                                    "      e.codarea = a.codarea And " &
                                    "      a.sigregia = r.sigregia And " &
                                    "      r.codsiste = s.codsiste " &
                                    "Group By 3, 1 " &
                                    "Union " &
                                    "Select s.nomsiste As codigo, 'ZEL' As tipo, zel.intrazelet As intervalo, Sum(zel.valrazeletsup) As valor " &
                                    "From razaoelet zel, usina u, empre e, area a, regia r, siste s " &
                                    "Where zel.datpdp = '" & Page.Request.QueryString("strData") & "' And " &
                                    "      zel.codusina = u.codusina And " &
                                    "      u.tipusina = 'T' And " &
                                    "      u.codempre = e.codempre And " &
                                    "      e.codarea = a.codarea And " &
                                    "      a.sigregia = r.sigregia And " &
                                    "      r.codsiste = s.codsiste " &
                                    "Group By 3, 1 " &
                                    "Union " &
                                    "Select s.nomsiste As codigo, 'EXP' As tipo, exp.intexporta As intervalo, Sum(exp.valexportasup) As valor " &
                                    "From exporta exp, usina u, empre e, area a, regia r, siste s " &
                                    "Where exp.datpdp = '" & Page.Request.QueryString("strData") & "' And " &
                                    "      exp.codusina = u.codusina And " &
                                    "      u.tipusina = 'T' And " &
                                    "      u.codempre = e.codempre And " &
                                    "      e.codarea = a.codarea And " &
                                    "      a.sigregia = r.sigregia And " &
                                    "      r.codsiste = s.codsiste " &
                                    "Group By 3, 1 " &
                                    "Union " &
                                    "Select s.nomsiste As codigo, 'ZEN' As tipo, zen.intrazener As intervalo, Sum(zen.valrazenersup) As valor " &
                                    "From razaoener zen, usina u, empre e, area a, regia r, siste s " &
                                    "Where zen.datpdp = '" & Page.Request.QueryString("strData") & "' And " &
                                    "      zen.codusina = u.codusina And " &
                                    "      u.tipusina = 'T' And " &
                                    "      u.codempre = e.codempre And " &
                                    "      e.codarea = a.codarea And " &
                                    "      a.sigregia = r.sigregia And " &
                                    "      r.codsiste = s.codsiste " &
                                    "Group By 3, 1 " &
                                    "Order By 3, 1"
                        Case Is = "sbr"
                            '-- Razões do Despacho das Usinas Térmicas por SIN
                            strSql = "Select 'SIN' As codigo, 'IFX' As tipo, ifx.intflexi As intervalo, Sum(ifx.valflexisup) As valor " &
                                    "From inflexibilidade ifx, usina u " &
                                    "Where  ifx.datpdp = '" & Page.Request.QueryString("strData") & "' And " &
                                    "       ifx.codusina = u.codusina And " &
                                    "       u.tipusina = 'T' " &
                                    "Group By 3, 1 " &
                                    "Union " &
                                    "Select 'SIN' As codigo, 'ZEL' As tipo, zel.intrazelet As intervalo, Sum(zel.valrazeletsup) As valor " &
                                    "From razaoelet zel, usina u " &
                                    "Where zel.datpdp = '" & Page.Request.QueryString("strData") & "' And " &
                                    "      zel.codusina = u.codusina And " &
                                    "      u.tipusina = 'T' " &
                                    "Group By 3, 1 " &
                                    "Union " &
                                    "Select 'SIN' As codigo, 'EXP' As tipo, exp.intexporta As intervalo, Sum(exp.valexportasup) As valor " &
                                    "From exporta exp, usina u " &
                                    "Where exp.datpdp = '" & Page.Request.QueryString("strData") & "' And " &
                                    "      exp.codusina = u.codusina And " &
                                    "      u.tipusina = 'T' " &
                                    "Group By 3, 1 " &
                                    "Union " &
                                    "Select 'SIN' As codigo, 'ZEN' As tipo, zen.intrazener As intervalo, Sum(zen.valrazenersup) As valor " &
                                    "From razaoener zen, usina u " &
                                    "Where zen.datpdp = '" & Page.Request.QueryString("strData") & "' And " &
                                    "      zen.codusina = u.codusina And " &
                                    "      u.tipusina = 'T' " &
                                    "Group By 3, 1 " &
                                    "Order By 3, 1"
                    End Select
                    strTable = "GeracaoCNOS"
                Case Is = "BalancoCNOS"
                    Try
                        objCommand.CommandText = "Drop Table tmpBalancoCNOS"
                        objCommand.ExecuteNonQuery()
                    Catch ex As Exception
                        'Se ocorrer o erro, ignora
                    End Try

                    Select Case Request.QueryString("strAgrega")
                        Case Is = "age"
                            '-- Agente 
                            strSql = "Select e.sigempre As codigo, c.intcarga As intervalo, c.valcargasup As valor, 'CARGA' As tipo " &
                                    "From empre e, Outer carga c " &
                                    "Where c.datpdp = '" & Page.Request.QueryString("strData") & "' And " &
                                    "      c.codempre = e.codempre And " &
                                    "      e.codempre <> 'I5' And " &
                                    "      e.codempre <> 'I6' " &
                                    "Union All " &
                                    "Select e.sigempre As codigo, d.intdespa As intervalo, Sum(d.valdespasup) As valor, 'GERACAO' As tipo " &
                                    "From empre e, usina u, Outer despa d " &
                                    "Where d.datpdp = '" & Page.Request.QueryString("strData") & "' And " &
                                    "      d.codusina = u.codusina  And " &
                                    "      u.codempre = e.codempre " &
                                    "Group By 1, 2 " &
                                    "Union All " &
                                    "Select e.sigempre As codigo, i.intinter As intervalo, Sum(i.valintersup) As valintersup , 'INTER' As tipo " &
                                    "From empre e, Outer inter i " &
                                    "Where i.datpdp = '" & Page.Request.QueryString("strData") & "' And " &
                                    "      i.codemprede = e.codempre And " &
                                    "      e.codempre <> 'I5' And " &
                                    "      e.codempre <> 'I6' " &
                                    "Group By 1, 2 " &
                                    "Into Temp tmpBalancoCNOS " &
                                    "With No Log"
                            objCommand.CommandText = strSql
                            objCommand.ExecuteNonQuery()

                            strSql = "Select e.sigempre As codigo, c.intcarga As intervalo, c.valcargasup As valor, 'CARGA' As tipo, 'A' As usina " &
                                    "From empre e, carga c " &
                                    "Where c.datpdp = '" & Page.Request.QueryString("strData") & "' And " &
                                    "      c.codempre = e.codempre And " &
                                    "      e.codempre <> 'I5' And " &
                                    "      e.codempre <> 'I6' " &
                                    "Union All " &
                                    "Select e.sigempre As codigo, d.intdespa As intervalo, Sum(d.valdespasup) As valor, 'GERACAO' As tipo, 'B' As usina " &
                                    "From empre e, usina u, despa d " &
                                    "Where d.datpdp = '" & Page.Request.QueryString("strData") & "' And " &
                                    "      d.codusina = u.codusina  And " &
                                    "      u.codempre = e.codempre " &
                                    "Group By 1, 2 " &
                                    "Union All " &
                                    "Select e.sigempre As codigo, i.intinter As intervalo, Sum(i.valintersup) As valintersup , 'INTER' As tipo, 'C' As usina " &
                                    "From empre e, inter i " &
                                    "Where i.datpdp = '" & Page.Request.QueryString("strData") & "' And " &
                                    "      i.codemprede = e.codempre And " &
                                    "      e.codempre <> 'I5' And " &
                                    "      e.codempre <> 'I6' " &
                                    "Group By 1, 2 " &
                                    "Union All " &
                                    "Select t1.codigo, t1.intervalo, (Nvl(t1.valor,0) + (Nvl(t2.valor,0) - Nvl(t3.valor,0))) As valor, 'FECHAMENTO' As tipo, 'Z' As usina " &
                                    "From tmpBalancoCNOS t1, tmpBalancoCNOS t2, Outer tmpBalancoCNOS t3 " &
                                    "Where t1.tipo = 'CARGA' And " &
                                    "      t1.intervalo = t2.intervalo And " &
                                    "      t1.codigo = t2.codigo And " &
                                    "      t2.tipo = 'INTER' And " &
                                    "      t2.intervalo = t3.intervalo And " &
                                    "      t2.codigo = t3.codigo And " &
                                    "      t3.tipo = 'GERACAO' " &
                                    "Order By 2, 1, 4"
                        Case Is = "are"
                            '-- Área
                            strSql = "Select a.nomarea As codigo, c.intcarga As intervalo, Sum(c.valcargasup) As valor, 'CARGA' As tipo " &
                                    "From carga c, empre e, area a " &
                                    "Where c.datpdp = '" & Page.Request.QueryString("strData") & "' And " &
                                    "      c.codempre = e.codempre And " &
                                    "      e.codarea = a.codarea And " &
                                    "      a.codarea <> 'I5' And " &
                                    "      a.codarea <> 'I6' " &
                                    "Group By 1, 2 " &
                                    "Union All " &
                                    "Select a.nomarea As codigo, d.intdespa As intervalo, Sum(d.valdespasup) As valor, 'GERACAO' As tipo " &
                                    "From despa d, usina u, empre e, area a " &
                                    "Where d.datpdp = '" & Page.Request.QueryString("strData") & "' And " &
                                    "      d.codusina = u.codusina And " &
                                    "      u.codempre = e.codempre And " &
                                    "      e.codarea = a.codarea And " &
                                    "      a.codarea <> 'I5' And " &
                                    "      a.codarea <> 'I6' " &
                                    "Group By 1, 2 " &
                                    "Union All " &
                                    "Select a.nomarea As codigo, i.intinter As intervalo, Sum(i.valintersup) As valor, 'INTER' As tipo " &
                                    "From inter i, empre e, area a " &
                                    "Where i.datpdp = '" & Page.Request.QueryString("strData") & "' And " &
                                    "      i.codemprede = e.codempre And " &
                                    "      e.codarea = a.codarea And " &
                                    "      a.codarea <> 'I5' And " &
                                    "      a.codarea <> 'I6' " &
                                    "Group By 1, 2 " &
                                    "Into Temp tmpBalancoCNOS " &
                                    "With No Log"
                            objCommand.CommandText = strSql
                            objCommand.ExecuteNonQuery()

                            strSql = "Select a.nomarea As codigo, c.intcarga As intervalo, Sum(c.valcargasup) As valor, 'CARGA' As tipo, 'A' As usina " &
                                    "From carga c, empre e, area a " &
                                    "Where c.datpdp = '" & Page.Request.QueryString("strData") & "' And " &
                                    "      c.codempre = e.codempre And " &
                                    "      e.codarea = a.codarea And " &
                                    "      a.codarea <> 'I5' And " &
                                    "      a.codarea <> 'I6' " &
                                    "Group By 1, 2 " &
                                    "Union All " &
                                    "Select a.nomarea As codigo, d.intdespa As intervalo, Sum(d.valdespasup) As valor, 'GERACAO' As tipo, 'B' As usina " &
                                    "From despa d, usina u, empre e, area a " &
                                    "Where d.datpdp = '" & Page.Request.QueryString("strData") & "' And " &
                                    "      d.codusina = u.codusina And " &
                                    "      u.codempre = e.codempre And " &
                                    "      e.codarea = a.codarea And " &
                                    "      a.codarea <> 'I5' And " &
                                    "      a.codarea <> 'I6' " &
                                    "Group By 1, 2 " &
                                    "Union All " &
                                    "Select a.nomarea As codigo, i.intinter As intervalo, Sum(i.valintersup) As valor, 'INTER' As tipo, 'C' As usina " &
                                    "From inter i, empre e, area a " &
                                    "Where i.datpdp = '" & Page.Request.QueryString("strData") & "' And " &
                                    "      i.codemprede = e.codempre And " &
                                    "      e.codarea = a.codarea And " &
                                    "      a.codarea <> 'I5' And " &
                                    "      a.codarea <> 'I6' " &
                                    "Group By 1, 2 " &
                                    "Union All " &
                                    "Select t1.codigo, t1.intervalo, (Nvl(t1.valor,0) + (Nvl(t2.valor,0) - Nvl(t3.valor,0))) As valor, 'FECHAMENTO' As tipo, 'Z' As usina " &
                                    "From tmpBalancoCNOS t1, tmpBalancoCNOS t2, Outer tmpBalancoCNOS t3 " &
                                    "Where t1.tipo = 'CARGA' And " &
                                    "      t1.intervalo = t2.intervalo And " &
                                    "      t1.codigo = t2.codigo And " &
                                    "      t2.tipo = 'INTER' And " &
                                    "      t2.intervalo = t3.intervalo And " &
                                    "      t2.codigo = t3.codigo And " &
                                    "      t3.tipo = 'GERACAO' " &
                                    "Order By 2, 1, 4"


                        Case Is = "reg"
                            '-- Região
                            strSql = "Select r.nomregia As codigo, c.intcarga As intervalo, Sum(c.valcargasup) As valor, 'CARGA' As tipo " &
                                    "From carga c, empre e, area a, regia r " &
                                    "Where c.datpdp = '" & Page.Request.QueryString("strData") & "' And " &
                                    "      c.codempre = e.codempre And " &
                                    "      e.codarea = a.codarea And a.sigregia = r.sigregia " &
                                    "Group By 1, 2 " &
                                    "Union All " &
                                    "Select r.nomregia As codigo, d.intdespa As intervalo, Sum(d.valdespasup) As valor, 'GERACAO' As tipo " &
                                    "From despa d, usina u, empre e, area a, regia r " &
                                    "Where d.datpdp = '" & Page.Request.QueryString("strData") & "' And " &
                                    "      d.codusina = u.codusina And " &
                                    "      u.codempre = e.codempre And " &
                                    "      e.codarea = a.codarea And " &
                                    "      a.sigregia = r.sigregia " &
                                    "Group By 1, 2 " &
                                    "Union All " &
                                    "Select r.nomregia As codigo, i.intinter As intervalo, Sum(i.valintersup) As valor, 'INTER' As tipo " &
                                    "From inter i, empre e, area a, regia r " &
                                    "Where i.datpdp = '" & Page.Request.QueryString("strData") & "' And " &
                                    "      i.codemprede = e.codempre And " &
                                    "      e.codarea = a.codarea And " &
                                    "      a.sigregia = r.sigregia " &
                                    "Group By 1, 2 " &
                                    "Into Temp tmpBalancoCNOS " &
                                    "With No Log"
                            objCommand.CommandText = strSql
                            objCommand.ExecuteNonQuery()

                            strSql = "Select r.nomregia As codigo, c.intcarga As intervalo, Sum(c.valcargasup) As valor, 'CARGA' As tipo, 'A' As usina " &
                                    "From carga c, empre e, area a, regia r " &
                                    "Where c.datpdp = '" & Page.Request.QueryString("strData") & "' And " &
                                    "      c.codempre = e.codempre And " &
                                    "      e.codarea = a.codarea And a.sigregia = r.sigregia " &
                                    "Group By 1, 2 " &
                                    "Union All " &
                                    "Select r.nomregia As codigo, d.intdespa As intervalo, Sum(d.valdespasup) As valor, 'GERACAO' As tipo, 'B' As usina " &
                                    "From despa d, usina u, empre e, area a, regia r " &
                                    "Where d.datpdp = '" & Page.Request.QueryString("strData") & "' And " &
                                    "      d.codusina = u.codusina And " &
                                    "      u.codempre = e.codempre And " &
                                    "      e.codarea = a.codarea And " &
                                    "      a.sigregia = r.sigregia " &
                                    "Group By 1, 2 " &
                                    "Union All " &
                                    "Select r.nomregia As codigo, i.intinter As intervalo, Sum(i.valintersup) As valor, 'INTER' As tipo, 'C' As usina " &
                                    "From inter i, empre e, area a, regia r " &
                                    "Where i.datpdp = '" & Page.Request.QueryString("strData") & "' And " &
                                    "      i.codemprede = e.codempre And " &
                                    "      e.codarea = a.codarea And " &
                                    "      a.sigregia = r.sigregia " &
                                    "Group By 1, 2 " &
                                    "Union All " &
                                    "Select t1.codigo, t1.intervalo, (Nvl(t1.valor,0) + (Nvl(t2.valor,0) - Nvl(t3.valor,0))) As valor, 'FECHAMENTO' As tipo, 'Z' As usina " &
                                    "From tmpBalancoCNOS t1, tmpBalancoCNOS t2, Outer tmpBalancoCNOS t3 " &
                                    "Where t1.tipo = 'CARGA' And " &
                                    "      t1.intervalo = t2.intervalo And " &
                                    "      t1.codigo = t2.codigo And " &
                                    "      t2.tipo = 'INTER' And " &
                                    "      t2.intervalo = t3.intervalo And " &
                                    "      t2.codigo = t3.codigo And " &
                                    "      t3.tipo = 'GERACAO' " &
                                    "Order By 2, 1, 4"
                        Case Is = "sis"
                            '-- Subsistema
                            strSql = "Select s.nomsiste As codigo, c.intcarga As intervalo, Sum(c.valcargasup) As valor, 'CARGA' As tipo " &
                                    "From carga c, empre e, area a, regia r, siste s " &
                                    "Where c.datpdp = '" & Page.Request.QueryString("strData") & "' And " &
                                    "      c.codempre = e.codempre And " &
                                    "      e.codarea = a.codarea And " &
                                    "      a.sigregia = r.sigregia And " &
                                    "      r.codsiste = s.codsiste " &
                                    "Group By 1, 2 " &
                                    "Union All " &
                                    "Select s.nomsiste As codigo, d.intdespa As intervalo, Sum(d.valdespasup) As valor, 'GERACAO' As tipo " &
                                    "From despa d, usina u, empre e, area a, regia r, siste s " &
                                    "Where d.datpdp = '" & Page.Request.QueryString("strData") & "' And " &
                                    "      d.codusina = u.codusina And " &
                                    "      u.codempre = e.codempre And " &
                                    "      e.codarea = a.codarea And " &
                                    "      a.sigregia = r.sigregia And " &
                                    "      r.codsiste = s.codsiste " &
                                    "Group By 1, 2 " &
                                    "Union All " &
                                    "Select s.nomsiste As codigo, i.intinter As intervalo, Sum(i.valintersup) As valor, 'INTER' As tipo " &
                                    "From inter i, empre e, area a, regia r, siste s " &
                                    "Where i.datpdp = '" & Page.Request.QueryString("strData") & "' And " &
                                    "      i.codemprede = e.codempre And " &
                                    "      e.codarea = a.codarea And " &
                                    "      a.sigregia = r.sigregia And " &
                                    "      r.codsiste = s.codsiste " &
                                    "Group By 1, 2 " &
                                    "Into Temp tmpBalancoCNOS " &
                                    "With No Log"
                            objCommand.CommandText = strSql
                            objCommand.ExecuteNonQuery()

                            strSql = "Select s.nomsiste As codigo, c.intcarga As intervalo, Sum(c.valcargasup) As valor, 'CARGA' As tipo, 'A' As usina " &
                                    "From carga c, empre e, area a, regia r, siste s " &
                                    "Where c.datpdp = '" & Page.Request.QueryString("strData") & "' And " &
                                    "      c.codempre = e.codempre And " &
                                    "      e.codarea = a.codarea And " &
                                    "      a.sigregia = r.sigregia And " &
                                    "      r.codsiste = s.codsiste " &
                                    "Group By 1, 2 " &
                                    "Union All " &
                                    "Select s.nomsiste As codigo, d.intdespa As intervalo, Sum(d.valdespasup) As valor, 'GERACAO' As tipo, 'B' As usina " &
                                    "From despa d, usina u, empre e, area a, regia r, siste s " &
                                    "Where d.datpdp = '" & Page.Request.QueryString("strData") & "' And " &
                                    "      d.codusina = u.codusina And " &
                                    "      u.codempre = e.codempre And " &
                                    "      e.codarea = a.codarea And " &
                                    "      a.sigregia = r.sigregia And " &
                                    "      r.codsiste = s.codsiste " &
                                    "Group By 1, 2 " &
                                    "Union All " &
                                    "Select s.nomsiste As codigo, i.intinter As intervalo, Sum(i.valintersup) As valor, 'INTER' As tipo, 'C' As usina " &
                                    "From inter i, empre e, area a, regia r, siste s " &
                                    "Where i.datpdp = '" & Page.Request.QueryString("strData") & "' And " &
                                    "      i.codemprede = e.codempre And " &
                                    "      e.codarea = a.codarea And " &
                                    "      a.sigregia = r.sigregia And " &
                                    "      r.codsiste = s.codsiste " &
                                    "Group By 1, 2 " &
                                    "Union All " &
                                    "Select t1.codigo, t1.intervalo, (Nvl(t1.valor,0) + (Nvl(t2.valor,0) - Nvl(t3.valor,0))) As valor, 'FECHAMENTO' As tipo, 'Z' As usina " &
                                    "From tmpBalancoCNOS t1, tmpBalancoCNOS t2, Outer tmpBalancoCNOS t3 " &
                                    "Where t1.tipo = 'CARGA' And " &
                                    "      t1.intervalo = t2.intervalo And " &
                                    "      t1.codigo = t2.codigo And " &
                                    "      t2.tipo = 'INTER' And " &
                                    "      t2.intervalo = t3.intervalo And " &
                                    "      t2.codigo = t3.codigo And " &
                                    "      t3.tipo = 'GERACAO' " &
                                    "Order By 2, 1, 4"
                        Case Is = "sbr"
                            '-- Sistema Interligado Nacional
                            strSql = "Select 'SIN' As codigo, c.intcarga As intervalo, Sum(c.valcargasup) As valor, 'CARGA' As tipo " &
                                    "From carga c " &
                                    "Where c.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                                    "Group By 1, 2 " &
                                    "Union All " &
                                    "Select 'SIN' As codigo, d.intdespa As intervalo, Sum(d.valdespasup) As valor, 'GERACAO' As tipo " &
                                    "From despa d " &
                                    "Where d.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                                    "Group By 1, 2 " &
                                    "Union All " &
                                    "Select 'SIN' As codigo, i.intinter As intervalo, Sum(i.valintersup) As valor, 'INTER' As tipo " &
                                    "From inter i " &
                                    "Where i.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                                    "Group By 1, 2 " &
                                    "Into Temp tmpBalancoCNOS " &
                                    "With No Log"
                            objCommand.CommandText = strSql
                            objCommand.ExecuteNonQuery()

                            strSql = "Select 'SIN' As codigo, c.intcarga As intervalo, Sum(c.valcargasup) As valor, 'CARGA' As tipo, 'A' As usina " &
                                    "From carga c " &
                                    "Where c.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                                    "Group By 1, 2 " &
                                    "Union All " &
                                    "Select 'SIN' As codigo, d.intdespa As intervalo, Sum(d.valdespasup) As valor, 'GERACAO' As tipo, 'B' As usina " &
                                    "From despa d " &
                                    "Where d.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                                    "Group By 1, 2 " &
                                    "Union All " &
                                    "Select 'SIN' As codigo, i.intinter As intervalo, Sum(i.valintersup) As valor, 'INTER' As tipo, 'C' As usina " &
                                    "From inter i " &
                                    "Where i.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                                    "Group By 1, 2 " &
                                    "Union All " &
                                    "Select t1.codigo, t1.intervalo, (Nvl(t1.valor,0) + (Nvl(t2.valor,0) - Nvl(t3.valor,0))) As valor, 'FECHAMENTO' As tipo, 'Z' As usina " &
                                    "From tmpBalancoCNOS t1, tmpBalancoCNOS t2, Outer tmpBalancoCNOS t3 " &
                                    "Where t1.tipo = 'CARGA' And " &
                                    "      t1.intervalo = t2.intervalo And " &
                                    "      t1.codigo = t2.codigo And " &
                                    "      t2.tipo = 'INTER' And " &
                                    "      t2.intervalo = t3.intervalo And " &
                                    "      t2.codigo = t3.codigo And " &
                                    "      t3.tipo = 'GERACAO' " &
                                    "Order By 2, 1, 4"
                    End Select
                    strTable = "GeracaoCNOS"
                Case Is = "ConfigCNOS"
                    Try
                        objCommand.CommandText = "Drop Table tmpConfigCNOS"
                        objCommand.ExecuteNonQuery()
                    Catch ex As Exception
                        'Se ocorrer o erro, ignora
                    End Try
                    objCommand.CommandText = "Select u.codempre, c.intmco As intervalo, Sum(c.valmcosup) As valor, 'CO' As tipo, 'DESL.' As estado " &
                                            "From conveniencia_oper c, usina u " &
                                            "Where c.datpdp = '" & Page.Request.QueryString("strData") & "' And " &
                                            "      c.codusina = u.codusina " &
                                            "Group By 1, 2 " &
                                            "Union All " &
                                            "Select u.codempre, p.intparal As intervalo, Count(p.paralsup) As valor, 'PP' As tipo, 'DESL.' As estado " &
                                            "From paralpdp p, gerad g, usina u " &
                                            "Where p.datpdp = '" & Page.Request.QueryString("strData") & "' And " &
                                            "      p.paralsup = 1 And " &
                                            "      p.codequip = g.codgerad And " &
                                            "      g.codusina = u.codusina " &
                                            "Group By 1, 2 " &
                                            "Union All " &
                                            "Select u.codempre, m.intmeg As intervalo, Sum(m.valmegsup) As valor, 'MG' As tipo, 'LIG.' As estado " &
                                            "From maq_gerando m, usina u " &
                                            "Where m.datpdp = '" & Page.Request.QueryString("strData") & "' And " &
                                            "      m.codusina = u.codusina " &
                                            "Group By 1, 2 " &
                                            "Union All " &
                                            "Select u.codempre, o.intmos As intervalo, Sum(o.valmossup) As valor, 'OS' As tipo, 'LIG.' As estado " &
                                            "From oper_sincrono o, usina u " &
                                            "Where o.datpdp = '" & Page.Request.QueryString("strData") & "' And " &
                                            "      o.codusina = u.codusina " &
                                            "Group By 1, 2 " &
                                            "Order By 1, 2, 4 " &
                                            "Into Temp tmpConfigCNOS " &
                                            "With No Log"
                    objCommand.ExecuteNonQuery()

                    Try
                        objCommand.CommandText = "Drop Table tmpMaquinaCNOS"
                        objCommand.ExecuteNonQuery()
                    Catch ex As Exception
                        'Se ocorrer o erro, ignora
                    End Try

                    Select Case Request.QueryString("strAgrega")
                        Case Is = "age"
                            '--Agente
                            objCommand.CommandText = "Select u.codempre, Count(g.codgerad) As maq " &
                                                    "From gerad g, usina u " &
                                                    "Where g.codusina = u.codusina " &
                                                    "Group By 1 " &
                                                    "Into Temp tmpMaquinaCNOS " &
                                                    "With No Log"
                            objCommand.ExecuteNonQuery()

                            strSql = "Select e.sigempre As codigo, c.intervalo, c.estado, c.tipo, m.maq, Nvl(c.valor,0) As valor " &
                                    "From tmpMaquinaCNOS m, tmpConfigCNOS c, empre e " &
                                    "Where m.codempre = c.codempre And " &
                                    "      c.codempre = e.codempre " &
                                    "Order By c.intervalo, m.codempre"
                        Case Is = "are"
                            '--Área
                            objCommand.CommandText = "Select e.codarea, Count(g.codgerad) As maq " &
                                                    "From gerad g, usina u, empre e " &
                                                    "Where g.codusina = u.codusina And " &
                                                    "      u.codempre = e.codempre " &
                                                    "Group By 1 " &
                                                    "Into Temp tmpMaquinaCNOS " &
                                                    "With No Log"
                            objCommand.ExecuteNonQuery()

                            strSql = "Select a.nomarea As codigo, c.intervalo, c.estado, c.tipo, m.maq, Sum(Nvl(c.valor,0)) As valor " &
                                    "From tmpMaquinaCNOS m, tmpConfigCNOS c, empre e, area a " &
                                    "Where c.codempre = e.codempre And " &
                                    "      e.codarea = a.codarea And " &
                                    "      a.codarea = m.codarea " &
                                    "Group By c.intervalo, a.nomarea, c.estado, c.tipo, m.maq " &
                                    "Order By c.intervalo, a.nomarea"
                        Case Is = "reg"
                            '--Região
                            objCommand.CommandText = "Select a.sigregia, Count(g.codgerad) As maq " &
                                                    "From gerad g, usina u, empre e, area a " &
                                                    "Where g.codusina = u.codusina And " &
                                                    "      u.codempre = e.codempre And " &
                                                    "      e.codarea = a.codarea " &
                                                    "Group By 1 " &
                                                    "Into Temp tmpMaquinaCNOS " &
                                                    "With No Log"
                            objCommand.ExecuteNonQuery()

                            strSql = "Select r.nomregia As codigo, c.intervalo, c.estado, c.tipo, m.maq, Sum(Nvl(c.valor,0)) As valor " &
                                    "From tmpMaquinaCNOS m, tmpConfigCNOS c, empre e, area a, regia r " &
                                    "Where c.codempre = e.codempre And " &
                                    "      e.codarea = a.codarea And " &
                                    "      a.sigregia = r.sigregia And " &
                                    "      r.sigregia = m.sigregia " &
                                    "Group By c.intervalo, r.nomregia, c.estado, c.tipo, m.maq " &
                                    "Order By c.intervalo, r.nomregia"
                        Case Is = "sis"
                            '--Subsistema
                            objCommand.CommandText = "Select r.codsiste, Count(g.codgerad) As maq " &
                                                    "From gerad g, usina u, empre e, area a, regia r " &
                                                    "Where g.codusina = u.codusina And " &
                                                    "      u.codempre = e.codempre And " &
                                                    "      e.codarea = a.codarea And " &
                                                    "      a.sigregia = r.sigregia " &
                                                    "Group By 1 " &
                                                    "Into Temp tmpMaquinaCNOS " &
                                                    "With No Log"
                            objCommand.ExecuteNonQuery()

                            strSql = "Select s.nomsiste As codigo, c.intervalo, c.estado, c.tipo, m.maq, Sum(Nvl(c.valor,0)) As valor " &
                                    "From tmpMaquinaCNOS m, tmpConfigCNOS c, empre e, area a, regia r, siste s " &
                                    "Where c.codempre = e.codempre And " &
                                    "      e.codarea = a.codarea And " &
                                    "      a.sigregia = r.sigregia And " &
                                    "      r.codsiste = s.codsiste And " &
                                    "      s.codsiste = m.codsiste " &
                                    "Group By c.intervalo, s.nomsiste, c.estado, c.tipo, m.maq " &
                                    "Order By c.intervalo, s.nomsiste"
                        Case Is = "sbr"
                            '--Sistema Interligado Brasileiro
                            objCommand.CommandText = "Select 'SIN' As codigo, Count(g.codgerad) As maq " &
                                                    "From gerad g, usina u, empre e, area a, regia r " &
                                                    "Where g.codusina = u.codusina And " &
                                                    "      u.codempre = e.codempre And " &
                                                    "      e.codarea = a.codarea And " &
                                                    "      a.sigregia = r.sigregia " &
                                                    "Group By 1 " &
                                                    "Into Temp tmpMaquinaCNOS " &
                                                    "With No Log"
                            objCommand.ExecuteNonQuery()

                            strSql = "Select 'SIN' As codigo, c.intervalo, c.estado, c.tipo, m.maq, Sum(Nvl(c.valor,0)) As valor " &
                                    "From tmpMaquinaCNOS m, tmpConfigCNOS c " &
                                    "Group By 2, 1, 3, 4, 5 " &
                                    "Order By 2, 1"
                    End Select
                    strTable = "ParadaCNOS"
                Case Is = "FolgaCNOS"

                    '### Folga = Capacidade - Geração - Restrição - Manutenção ###

                    '--Manutenção
                    Try
                        objCommand.CommandText = "Drop Table tmpManutencaoSE"
                        objCommand.ExecuteNonQuery()
                    Catch ex As Exception
                        'Se ocorrer o erro, ignora
                    End Try

                    objCommand.CommandText = "Select u.codusina, p.intparal, Sum(g.capgerad) as vlrManutencao " &
                                            "From usina u, gerad g, paralpdp p " &
                                            "Where u.tipusina = 'H' And " &
                                            "      u.codusina = g.codusina And " &
                                            "      g.codgerad = p.codequip And " &
                                            "      p.datpdp = '" & Page.Request.QueryString("strData") & "' And " &
                                            "      p.paralsup = 1 " &
                                            "Group By u.codusina, p.intparal " &
                                            "Into Temp tmpManutencaoSE " &
                                            "With No Log"
                    objCommand.ExecuteNonQuery()

                    '--Restrição
                    Try
                        objCommand.CommandText = "Drop Table tmpRestrSE"
                        objCommand.ExecuteNonQuery()
                    Catch ex As Exception
                        'Se ocorrer o erro, ignora
                    End Try

                    objCommand.CommandText = "Select r.intrestr As intervalo, " &
                                            "       u.codusina, " &
                                            "       Sum(Case When r.valrestrprpsup > r.valrestrusisup Then r.valrestrprpsup Else r.valrestrusisup End) As valor " &
                                            "From usina u, gerad g, restrpdp r " &
                                            "Where u.tipusina = 'H' And " &
                                            "      u.codusina = g.codusina And " &
                                            "      g.codgerad = r.codgerad And " &
                                            "      r.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                                            "Group By r.intrestr, u.codusina " &
                                            "Into Temp tmpRestrSE " &
                                            "With No Log"
                    objCommand.ExecuteNonQuery()

                    Select Case Request.QueryString("strAgrega")
                        Case Is = "age"
                            '-- Agentes
                            strSql = "Select e.sigempre As codigo, u.codusina As sigla, d.intdespa As intervalo, Sum(Nvl(u.potinstalada,0) - Nvl(d.valdespasup,0) - Nvl(m.vlrmanutencao,0) - Nvl(r.valor,0)) As valor, 1 As ordem " &
                                    "From empre e, usina u, despa d, Outer tmpManutencaoSE m, Outer tmpRestrSE r " &
                                    "Where e.codempre = u.codempre And " &
                                    "      u.tipusina = 'H' And " &
                                    "      u.codusina = d.codusina And " &
                                    "      d.datpdp = '" & Page.Request.QueryString("strData") & "' And " &
                                    "      d.codusina = m.codusina And " &
                                    "      d.intdespa = m.intparal And " &
                                    "      d.codusina = r.codusina And " &
                                    "      d.intdespa = r.intervalo " &
                                    "Group By 3, 1, 2 " &
                                    "Union All " &
                                    "Select e.sigempre As codigo, 'TOTAL' As sigla, d.intdespa As intervalo, Sum(Nvl(u.potinstalada,0) - Nvl(d.valdespasup,0) - Nvl(m.vlrmanutencao,0) - Nvl(r.valor,0)) As valor, 99 As ordem " &
                                    "From empre e, usina u, despa d, Outer tmpManutencaoSE m, Outer tmpRestrSE r " &
                                    "Where e.codempre = u.codempre And " &
                                    "      u.tipusina = 'H' And " &
                                    "      u.codusina = d.codusina And " &
                                    "      d.datpdp = '" & Page.Request.QueryString("strData") & "' And " &
                                    "      d.codusina = m.codusina And " &
                                    "      d.intdespa = m.intparal And " &
                                    "      d.codusina = r.codusina And " &
                                    "      d.intdespa = r.intervalo " &
                                    "Group By 3, 1, 2 " &
                                    "Order By 3, 1, 2"
                        Case Is = "are"
                            '-- Área 
                            strSql = "Select d.intdespa As intervalo, u.codusina As sigla, a.nomarea As codigo, Sum(Nvl(u.potinstalada,0) - Nvl(d.valdespasup,0) - Nvl(m.vlrmanutencao,0) - Nvl(r.valor,0)) As valor, 1 As ordem " &
                                    "From area a, empre e, usina u, despa d, Outer tmpManutencaoSE m, Outer tmpRestrSE r " &
                                    "Where a.codarea = e.codarea And " &
                                    "      e.codempre = u.codempre And " &
                                    "      u.tipusina = 'H' And " &
                                    "      u.codusina = d.codusina And " &
                                    "      d.datpdp = '" & Page.Request.QueryString("strData") & "' And " &
                                    "      d.codusina = m.codusina And " &
                                    "      d.intdespa = m.intparal And " &
                                    "      d.codusina = r.codusina And " &
                                    "      d.intdespa = r.intervalo " &
                                    "Group By 1, 3, 2 " &
                                    "Union All " &
                                    "Select d.intdespa As intervalo, 'TOTAL' As sigla, a.nomarea As codigo, Sum(Nvl(u.potinstalada,0) - Nvl(d.valdespasup,0) - Nvl(m.vlrmanutencao,0) - Nvl(r.valor,0)) As valor, 99 As ordem " &
                                    "From area a, empre e, usina u, despa d, Outer tmpManutencaoSE m, Outer tmpRestrSE r " &
                                    "Where a.codarea = e.codarea And " &
                                    "      e.codempre = u.codempre And " &
                                    "      u.tipusina = 'H' And " &
                                    "      u.codusina = d.codusina And " &
                                    "      d.datpdp = '" & Page.Request.QueryString("strData") & "' And " &
                                    "      d.codusina = m.codusina And " &
                                    "      d.intdespa = m.intparal And " &
                                    "      d.codusina = r.codusina And " &
                                    "      d.intdespa = r.intervalo " &
                                    "Group By 1, 3, 2 " &
                                    "Order By 1, 3, 2"
                        Case Is = "reg"
                            '-- Região
                            strSql = "Select d.intdespa As intervalo, re.sigregia As codigo, u.codusina As sigla, Sum(Nvl(u.potinstalada,0) - Nvl(d.valdespasup,0) - Nvl(m.vlrmanutencao,0) - Nvl(r.valor,0)) As valor, 1 As ordem " &
                                    "From regia re, area a, empre e, usina u, despa d, Outer tmpManutencaoSE m, Outer tmpRestrSE r " &
                                    "Where re.sigregia = a.sigregia And " &
                                    "      a.codarea = e.codarea And " &
                                    "      e.codempre = u.codempre And " &
                                    "      u.tipusina = 'H' And " &
                                    "      u.codusina = d.codusina And " &
                                    "      d.datpdp = '" & Page.Request.QueryString("strData") & "' And " &
                                    "      d.codusina = m.codusina And " &
                                    "      d.intdespa = m.intparal And " &
                                    "      d.codusina = r.codusina And " &
                                    "      d.intdespa = r.intervalo " &
                                    "Group By 1, 2, 3 " &
                                    "Union All " &
                                    "Select d.intdespa As intervalo, re.sigregia As codigo, 'TOTAL' As sigla, Sum(Nvl(u.potinstalada,0) - Nvl(d.valdespasup,0) - Nvl(m.vlrmanutencao,0) - Nvl(r.valor,0)) As valor, 99 As ordem " &
                                    "From regia re, area a, empre e, usina u, despa d, Outer tmpManutencaoSE m, Outer tmpRestrSE r " &
                                    "Where re.sigregia = a.sigregia And " &
                                    "      a.codarea = e.codarea And " &
                                    "      e.codempre = u.codempre And " &
                                    "      u.tipusina = 'H' And " &
                                    "      u.codusina = d.codusina And " &
                                    "      d.datpdp = '" & Page.Request.QueryString("strData") & "' And " &
                                    "      d.codusina = m.codusina And " &
                                    "      d.intdespa = m.intparal And " &
                                    "      d.codusina = r.codusina And " &
                                    "      d.intdespa = r.intervalo " &
                                    "Group By 1, 2, 3 " &
                                    "Order By 1, 2, 3"
                        Case Is = "sis"
                            '-- Subsistema
                            strSql = "Select d.intdespa As intervalo, s.nomsiste As codigo, u.codusina As sigla, Sum(Nvl(u.potinstalada,0) - Nvl(d.valdespasup,0) - Nvl(m.vlrmanutencao,0) - Nvl(r.valor,0)) As valor, 1 As ordem " &
                                    "From siste s, regia re, area a, empre e, usina u, despa d, Outer tmpManutencaoSE m, Outer tmpRestrSE r " &
                                    "Where s.codsiste = re.codsiste And " &
                                    "      re.sigregia = a.sigregia And " &
                                    "      a.codarea = e.codarea And " &
                                    "      e.codempre = u.codempre And " &
                                    "      u.tipusina = 'H' And " &
                                    "      u.codusina = d.codusina And " &
                                    "      d.datpdp = '" & Page.Request.QueryString("strData") & "' And " &
                                    "      d.codusina = m.codusina And " &
                                    "      d.intdespa = m.intparal And " &
                                    "      d.codusina = r.codusina And " &
                                    "      d.intdespa = r.intervalo " &
                                    "Group By 1, 2, 3 " &
                                    "Union All " &
                                    "Select d.intdespa As intervalo, s.nomsiste As codigo, 'TOTAL' As sigla, Sum(Nvl(u.potinstalada,0) - Nvl(d.valdespasup,0) - Nvl(m.vlrmanutencao,0) - Nvl(r.valor,0)) As valor, 99 As ordem " &
                                    "From siste s, regia re, area a, empre e, usina u, despa d, Outer tmpManutencaoSE m, Outer tmpRestrSE r " &
                                    "Where s.codsiste = re.codsiste And " &
                                    "      re.sigregia = a.sigregia And " &
                                    "      a.codarea = e.codarea And " &
                                    "      e.codempre = u.codempre And " &
                                    "      u.tipusina = 'H' And " &
                                    "      u.codusina = d.codusina And " &
                                    "      d.datpdp = '" & Page.Request.QueryString("strData") & "' And " &
                                    "      d.codusina = m.codusina And " &
                                    "      d.intdespa = m.intparal And " &
                                    "      d.codusina = r.codusina And " &
                                    "      d.intdespa = r.intervalo " &
                                    "Group By 1, 2, 3 " &
                                    "Order By 1, 2, 3"
                        Case Is = "sbr"
                            '-- Sistema Interligado Nacional
                            strSql = "Select d.intdespa As intervalo, 'SIN' As codigo, u.codusina As sigla, Sum(Nvl(u.potinstalada,0) - Nvl(d.valdespasup,0) - Nvl(m.vlrmanutencao,0) - Nvl(r.valor,0)) As valor, 1 As ordem " &
                                    "From usina u, despa d, Outer tmpManutencaoSE m, Outer tmpRestrSE r " &
                                    "Where u.tipusina = 'H' And " &
                                    "      u.codusina = d.codusina And " &
                                    "      d.datpdp = '" & Page.Request.QueryString("strData") & "' And " &
                                    "      d.codusina = m.codusina And " &
                                    "      d.intdespa = m.intparal And " &
                                    "      d.codusina = r.codusina And " &
                                    "      d.intdespa = r.intervalo " &
                                    "Group By 1, 2, 3 " &
                                    "Union All " &
                                    "Select d.intdespa As intervalo, 'SIN' As codigo, 'TOTAL' As sigla, Sum(Nvl(u.potinstalada,0) - Nvl(d.valdespasup,0) - Nvl(m.vlrmanutencao,0) - Nvl(r.valor,0)) As valor, 99 As ordem " &
                                    "From usina u, despa d, Outer tmpManutencaoSE m, Outer tmpRestrSE r " &
                                    "Where u.tipusina = 'H' And " &
                                    "      u.codusina = d.codusina And " &
                                    "      d.datpdp = '" & Page.Request.QueryString("strData") & "' And " &
                                    "      d.codusina = m.codusina And " &
                                    "      d.intdespa = m.intparal And " &
                                    "      d.codusina = r.codusina And " &
                                    "      d.intdespa = r.intervalo " &
                                    "Group By 1, 2, 3 " &
                                    "Order By 1, 2, 3"
                    End Select
                    strTable = "CargaCNOS"

            End Select

            objCommand.CommandText = strSql
            objCommand.CommandType = CommandType.Text
            Dim objDA As New OnsClasses.OnsData.OnsDataAdapter
            objDA.SelectCommand = objCommand
            Dim objDS As New dtsRelatorio
            objDA.Fill(objDS, strTable)
            Session("report_ds") = objDS

            '-- CRQ3713
            Session("report_count") = 0
            If strTable = "PDDefluxObs" Then Session("report_count") = objDS.PDDefluxObs.Count()

            objConnection.Close()
        Catch ex As Exception
            Response.Write("<script>alert('Não foi possível gerar o relatório, ocorreu o seguinte erro:" & Err.Description & "')</script>")
            'Server.Transfer("frmMensagem.aspx")
        End Try
    End Sub

    Private Sub CarregaDSInter()
        '-- CRQ4903 (10/04/2013) - Seleção dos dados através da view VW_RELPDP_NE_REQUISITO_GERACAO
        Dim objCon As New OnsClasses.OnsData.OnsConnection
        'Dim objCmd As New OnsClasses.OnsData.OnsCommand
        Dim strSQL As String

        objCon.Open("rpdp")
        'objCmd.Connection = objCon

        'Seleciona todos os dados da view VW_RELPDP_S_INTERCAMBIO_PROG_REPROG

        'objCmd.CommandText = "SELECT intervalo, valcarga, valgeracao, valinterliq, " & _
        '                    " valprogsuse, valFluxoRG, valreserva " & _
        '                    "FROM VW_RELPDP_S_INTERCAMBIO_PROG_REPROG " & _
        '                    "WHERE datpdp = '" & Page.Request.QueryString("strData") & "' " & _
        '                    "ORDER BY intervalo"

        strSQL = "SELECT intervalo, valcarga, valgeracao, valinterliq, " &
                            " valprogsuse, valFluxoRG, valreserva " &
                            "FROM VW_RELPDP_S_INTERCAMBIO_PROG_REPROG " &
                            "WHERE datpdp = '" & Page.Request.QueryString("strData") & "' " &
                            "ORDER BY intervalo"


        'objCmd.CommandType = CommandType.Text
        Dim objDA As New OnsClasses.OnsData.OnsDataAdapter(strSQL, objCon)
        'objDA.SelectCommand = objCmd
        Dim objDS As New dtsRelatorio
        Session("report_ds") = objDS
        objDA.Fill(objDS, "InterS")
        Session("report_ds") = objDS
        objCon.Close()
    End Sub

    Private Sub CarregaDSInterOld()
        '-- CRQ4903 (10/04/2013) - Antiga Sub CarregaDSInter desativada
        '-- Seleção dos dados será através da view VW_RELPDP_NE_REQUISITO_GERACAO na Sub CarregaDSInter 
        Dim objCon As New OnsClasses.OnsData.OnsConnection
        Dim objCmd As New OnsClasses.OnsData.OnsCommand

        objCon.Open("rpdp")
        objCmd.Connection = objCon

        'Elimina a tabela temporária para somar a Carga.
        objCmd.CommandText = "DROP TABLE tmpCargaS"
        Try
            objCmd.ExecuteNonQuery()
        Catch ex As Exception
            'se a temporária não existe ignora o erro
        End Try

        'Carga S
        objCmd.CommandText = "SELECT c.intcarga AS intervalo, SUM(c.valcargasup) AS valcarga " &
                            "FROM empre e, carga c " &
                            "WHERE e.codarea = 'RS' " &
                            "AND e.codempre = c.codempre " &
                            "AND c.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                            "GROUP BY 1 " &
                            "INTO TEMP tmpCargaS " &
                            "WITH NO LOG"
        objCmd.ExecuteNonQuery()

        'Elimina a tabela temporária para somar a Geração.
        objCmd.CommandText = "DROP TABLE tmpGeraLiq"
        Try
            objCmd.ExecuteNonQuery()
        Catch ex As Exception
            'se a temporária não existe ignora o erro
        End Try

        'Geracao Hidro e Termo
        '-- MarcosA 2009-09-11 IM50174 - Retirada da seleção abaixo a restrição u.codusina <> 'ERPTER'
        objCmd.CommandText = "SELECT d.intdespa AS intervalo, SUM(NVL(d.valdespasup,0)) AS valgeracao " &
                            "FROM despa d, usina u, empre e " &
                            "WHERE datpdp = '" & Page.Request.QueryString("strData") & "' " &
                            "AND d.codusina = u.codusina " &
                            "AND (u.tipusina = 'H' " &
                            "OR u.tipusina = 'T') " &
                            "AND u.codusina <> 'TSPTER' " &
                            "AND u.codusina <> 'TSUTWE' " &
                            "AND u.codusina <> 'SCPTER' " &
                            "AND u.codusina <> 'TSPHID' " &
                            "AND u.codempre = e.codempre " &
                            "AND e.codarea = 'RS' " &
                            "GROUP BY 1 " &
                            "INTO TEMP tmpGeraLiq " &
                            "WITH NO LOG"
        'objCmd.CommandText = "SELECT d.intdespa AS intervalo, SUM(NVL(d.valdespasup,0)) AS valgeracao " & _
        '                     "FROM despa d, usina u, empre e " & _
        '                     "WHERE datpdp = '" & Page.Request.QueryString("strData") & "' " & _
        '                     "AND d.codusina = u.codusina " & _
        '                     "AND (u.tipusina = 'H' " & _
        '                     "OR u.tipusina = 'T') " & _
        '                     "AND u.codusina <> 'TSPTER' " & _
        '                     "AND u.codusina <> 'TSUTWE' " & _
        '                     "AND u.codusina <> 'ERPTER' " & _
        '                     "AND u.codusina <> 'SCPTER' " & _
        '                     "AND u.codusina <> 'TSPHID' " & _
        '                     "AND u.codempre = e.codempre " & _
        '                     "AND e.codarea = 'RS' " & _
        '                     "AND e.codempre <> 'CE' " & _
        '                     "GROUP BY 1 " & _
        '                     "INTO TEMP tmpGeraLiq " & _
        '                     "WITH NO LOG"
        objCmd.ExecuteNonQuery()

        'Elimina a tabela temporária para somar os intercâmbios.
        objCmd.CommandText = "DROP TABLE tmpIntercambio"
        Try
            objCmd.ExecuteNonQuery()
        Catch ex As Exception
            'se a temporária não existe ignora o erro
        End Try

        'Grava Intercâmbio na tabela temporária
        objCmd.CommandText = "SELECT intinter As intervalo, SUM(NVL(valintersup,0)) AS valinterliq " &
                            "FROM inter " &
                            "WHERE datpdp = '" & Page.Request.QueryString("strData") & "' " &
                            "AND codemprede = 'RS' " &
                            "AND codemprepara = 'RE' " &
                            "GROUP BY 1 " &
                            "INTO TEMP tmpIntercambio " &
                            "WITH NO LOG"
        'objCmd.CommandText = "SELECT intinter As intervalo, SUM(NVL(valintersup,0)) AS valinterliq " & _
        '                     "FROM inter " & _
        '                     "WHERE datpdp = '" & Page.Request.QueryString("strData") & "' " & _
        '                     "AND ((codemprede = 'RS' " & _
        '                     "AND codemprepara = 'CO') " & _
        '                     "OR (codemprede = 'RS' " & _
        '                     "AND codemprepara = 'RE')) " & _
        '                     "GROUP BY 1 " & _
        '                     "INTO TEMP tmpIntercambio " & _
        '                     "WITH NO LOG"
        objCmd.ExecuteNonQuery()

        'Elimina a tabela temporária com a Geração da Usina UTAL
        objCmd.CommandText = "DROP TABLE tmpGeracao"
        Try
            objCmd.ExecuteNonQuery()
        Catch ex As Exception
            'se a temporária não existe ignora o erro
        End Try
        'Grava Geração na tabela temporária
        objCmd.CommandText = "SELECT intdespa AS intervalo, NVL(valdespasup,0) AS valgeracao " &
                            "FROM despa " &
                            "WHERE codusina = 'TSUTAL' " &
                            "AND datpdp= '" & Page.Request.QueryString("strData") & "' " &
                            "INTO TEMP tmpGeracao " &
                            "WITH NO LOG"
        objCmd.ExecuteNonQuery()

        'Elimina a tabela temporária com o Intercâmbio de FURNAS
        objCmd.CommandText = "DROP TABLE tmpInterFU"
        Try
            objCmd.ExecuteNonQuery()
        Catch ex As Exception
            'se a temporária não existe ignora o erro
        End Try
        'Seleciona os valores de Intercâmbio de FURNAS
        objCmd.CommandText = "SELECT SUM(valintersup) AS valintersup, intinter AS intervalo " &
                            "FROM inter " &
                            "WHERE datpdp = '" & Page.Request.QueryString("strData") & "' " &
                            "AND codemprede = 'RS' " &
                            "AND codemprepara = 'RE' " &
                            "GROUP BY intinter " &
                            "INTO TEMP tmpInterFU " &
                            "WITH NO LOG"
        objCmd.ExecuteNonQuery()

        'Seleciona os valores de Geracao das usinas: TSUTWA, TSUTWE, ERMIM e ERPHID
        objCmd.CommandText = "DROP TABLE tmpGerUsina"
        Try
            objCmd.ExecuteNonQuery()
        Catch ex As Exception
            'se a temporária não existe ignora o erro
        End Try
        'Seleciona os valores de Geracao das usinas: TSUTWA, TSUTWE, ERMIM e ERPHID

        '-- Alterado conforme IM44972 - Início
        'objCmd.CommandText = "SELECT SUM(valdespasup) AS valdespasup, intdespa AS intervalo " & _
        '                    "FROM despa " & _
        '                    "WHERE datpdp = '" & Page.Request.QueryString("strData") & "' " & _
        '                    "AND (codusina = 'ERMIM' " & _
        '                    "OR codusina = 'TSUTWA' " & _
        '                    "OR codusina = 'TSUTWE' " & _
        '                    "OR codusina = 'ERPHID') " & _
        '                    "GROUP BY intdespa " & _
        '                    "INTO TEMP tmpGerUsina " & _
        '                    "WITH NO LOG"
        '-- Alterado conforme IM44972 - Fim

        '-- MarcosA 2009-09-11 IM50174 - Retirada da seleção abaixo a restrição u.codusina <> 'ERPTER'
        '-- CRQ2615 (29/08/2012) - inclui restrição de AAUTAG e KCCHAP
        '-- CRQnnnn (17/10/2012) - seleciona ERMIN, ERPCH1, ERPCH2, OGPCAS, AEPCBU, AEPCPP, ERPTER, AAUTAG e KCCHAP; não seleciona TSUTWA e ERPHID
        objCmd.CommandText = "SELECT SUM(valdespasup) AS valdespasup, intdespa AS intervalo " &
                            "FROM despa " &
                            "WHERE datpdp = '" & Page.Request.QueryString("strData") & "' " &
                            "AND (codusina = 'ERMIM' " &
                            "OR codusina = 'ERPCH1' " &
                            "OR codusina = 'ERPCH2' " &
                            "OR codusina = 'OGPCAS' " &
                            "OR codusina = 'AEPCBU' " &
                            "OR codusina = 'AEPCPP' " &
                            "OR codusina = 'ERPTER' " &
                            "OR codusina = 'AAUTAG' " &
                            "OR codusina = 'KCCHAP') " &
                            "GROUP BY intdespa " &
                            "INTO TEMP tmpGerUsina " &
                            "WITH NO LOG"

        objCmd.ExecuteNonQuery()

        ''Elimina a tabela temporária com o Intercâmbio de CO
        'objCmd.CommandText = "DROP TABLE tmpInterCO"
        'Try
        '    objCmd.ExecuteNonQuery()
        'Catch ex As Exception
        '    'se a temporária não existe ignora o erro
        'End Try
        ''Seleciona os valores de Intercâmbio de FURNAS
        'objCmd.CommandText = "SELECT SUM(valintersup) AS valintersup, intinter AS intervalo " & _
        '                     "FROM inter " & _
        '                     "WHERE datpdp = '" & Page.Request.QueryString("strData") & "' " & _
        '                     "AND codemprede = 'CO' " & _
        '                     "AND codemprepara = 'RS' " & _
        '                     "GROUP BY intinter " & _
        '                     "INTO TEMP tmpInterCO " & _
        '                     "WITH NO LOG"
        'objCmd.ExecuteNonQuery()



        'Elimina a tabela temporária com a Geração da Usina UTAL
        objCmd.CommandText = "DROP TABLE tmpInterCE"
        Try
            objCmd.ExecuteNonQuery()
        Catch ex As Exception
            'se a temporária não existe ignora o erro
        End Try
        'Seleciona os valores de Intercâmbio de CEEE
        objCmd.CommandText = "SELECT SUM(valintersup) AS valintersup, intinter AS intervalo " &
                            "FROM inter " &
                            "WHERE datpdp = '" & Page.Request.QueryString("strData") & "' " &
                            "AND codemprede = 'CE' " &
                            "AND codemprepara = 'RS' " &
                            "GROUP BY intinter " &
                            "INTO TEMP tmpInterCE " &
                            "WITH NO LOG"
        objCmd.ExecuteNonQuery()

        'Seleciona os valores de Geracao das usinas: TSUHPF, TSUTCH, TSUTAL, BSUTCA, EUUTUR, NCUHMC e ESRIV
        objCmd.CommandText = "DROP TABLE tmpGerUsina2"
        Try
            objCmd.ExecuteNonQuery()
        Catch ex As Exception
            'se a temporária não existe ignora o erro
        End Try

        'Seleciona os valores de Geracao das usinas que serão debitadas de Carga Sul, gerando coluna FRS do relatório
        '-- CRQnnnn (13/09/2012) - Novas Usinas na seleção
        objCmd.CommandText = "SELECT SUM(valdespasup) AS valdespasup, intdespa AS intervalo " &
                            "FROM despa " &
                            "WHERE datpdp = '" & Page.Request.QueryString("strData") & "' " &
                            "AND (codusina = 'CEUPRE' " &
                            "OR codusina = 'CEUJAC' " &
                            "OR codusina = 'CEUITA' " &
                            "OR codusina = 'CEUDFR' " &
                            "OR codusina = 'CEUCAN' " &
                            "OR codusina = 'CEPHID' " &
                            "OR codusina = 'CEPCH1' " &
                            "OR codusina = 'CEPCH2' " &
                            "OR codusina = 'TSUHPF' " &
                            "OR codusina = 'MOUHMO' " &
                            "OR codusina = 'NCUHCA' " &
                            "OR codusina = 'NCUHMC' " &
                            "OR codusina = 'NCUHQJ' " &
                            "OR codusina = 'HIPCDI' " &
                            "OR codusina = 'HIPCJA' " &
                            "OR codusina = 'BCPCCA' " &
                            "OR codusina = 'BCPCLE' " &
                            "OR codusina = 'BCPCCO' " &
                            "OR codusina = 'MOPCEM' " &
                            "OR codusina = 'EFEVSI' " &
                            "OR codusina = 'EFEVSO' " &
                            "OR codusina = 'EFEVSS' " &
                            "OR codusina = 'IJUSJO' " &
                            "OR codusina = 'ESECC1' " &
                            "OR codusina = 'ESECC2' " &
                            "OR codusina = 'ESECC3' " &
                            "OR codusina = 'ELUECI' " &
                            "OR codusina = 'BCPCPM' " &
                            "OR codusina = 'ESHPSJ' " &
                            "OR codusina = 'HIPCSM' " &
                            "OR codusina = 'BMPHED' " &
                            "OR codusina = 'BMPHJA' " &
                            "OR codusina = 'TSUTCH' " &
                            "OR codusina = 'TSUTAL' " &
                            "OR codusina = 'EEUPMA' " &
                            "OR codusina = 'EEUPMB' " &
                            "OR codusina = 'EEUSJE' " &
                            "OR codusina = 'EEUPAL' " &
                            "OR codusina = 'BSUTCA' " &
                            "OR codusina = 'BSTCAA' " &
                            "OR codusina = 'BSTCAB' " &
                            "OR codusina = 'BSTCAT' " &
                            "OR codusina = 'BSRFAP' " &
                            "OR codusina = 'ESRIV' " &
                            "OR codusina = 'ESCUR' " &
                            "OR codusina = 'EUUTUR' " &
                            "OR codusina = 'EEUTC3') " &
                            "GROUP BY intdespa " &
                            "INTO TEMP tmpGerUsina2 " &
                            "WITH NO LOG"
        objCmd.ExecuteNonQuery()

        '-- Alterado conforme IM44972 - Início
        'objCmd.CommandText = "SELECT SUM(valdespasup) AS valdespasup, intdespa AS intervalo " & _
        '                    "FROM despa " & _
        '                    "WHERE datpdp = '" & Page.Request.QueryString("strData") & "' " & _
        '                    "AND (codusina = 'TSUHPF' " & _
        '                    "OR codusina = 'TSUTCH' " & _
        '                    "OR codusina = 'TSUTAL' " & _
        '                    "OR codusina = 'BSUTCA' " & _
        '                    "OR codusina = 'EUUTUR' " & _
        '                    "OR codusina = 'NCUHMC' " & _
        '                    "OR codusina = 'NCUHCA' " & _
        '                    "OR codusina = 'ESRIV' " & _
        '                    "OR codusina = 'EEUPMA' " & _
        '                    "OR codusina = 'EEUPMB' " & _
        '                    "OR codusina = 'EEUSJE' " & _
        '                    "OR codusina = 'EFEVSO' " & _
        '                    "OR codusina = 'EFEVSS' " & _
        '                    "OR codusina = 'EFEVSI') " & _
        '                    "GROUP BY intdespa " & _
        '                    "INTO TEMP tmpGerUsina2 " & _
        '                    "WITH NO LOG"
        '-- Alterado conforme IM44972 - Fim

        ''Elimina a tabela temporária com o Intercâmbio da CE modalidade R2
        'objCmd.CommandText = "DROP TABLE tmpR2CE"
        'Try
        '    objCmd.ExecuteNonQuery()
        'Catch ex As Exception
        '    'se a temporária não existe ignora o erro
        'End Try
        ''Seleciona os valores de Intercâmbio de CEEE na Modalidade R2
        'objCmd.CommandText = "SELECT Sum(valintersup) AS valintersup, intinter AS intervalo " & _
        '                     "FROM inter " & _
        '                     "WHERE datpdp = '" & Page.Request.QueryString("strData") & "' " & _
        '                     "AND codemprede = 'RS' " & _
        '                     "AND codemprepara = 'CE' " & _
        '                     "AND codcontamodal = 'R2' " & _
        '                     "GROUP BY intinter " & _
        '                     "INTO TEMP tmpR2CE " & _
        '                     "WITH NO LOG"
        'objCmd.ExecuteNonQuery()

        ''Elimina a tabela temporária com o Intercâmbio da CO modalidade R2
        'objCmd.CommandText = "DROP TABLE tmpR2CO"
        'Try
        '    objCmd.ExecuteNonQuery()
        'Catch ex As Exception
        '    'se a temporária não existe ignora o erro
        'End Try
        ''Seleciona os valores de Intercâmbio de COPEL na Modalidade R2
        'objCmd.CommandText = "SELECT SUM(valintersup) AS valintersup, intinter AS intervalo " & _
        '                     "FROM inter " & _
        '                     "WHERE datpdp = '" & Page.Request.QueryString("strData") & "' " & _
        '                     "AND codemprede = 'CO' " & _
        '                     "AND codemprepara = 'RS' " & _
        '                     "AND codcontamodal = 'R2' " & _
        '                     "GROUP BY intinter " & _
        '                     "INTO TEMP tmpR2CO " & _
        '                     "WITH NO LOG"
        'objCmd.ExecuteNonQuery()

        'Elimina a tabela temporária com a Reserva de Potencia do Sul
        objCmd.CommandText = "DROP TABLE tmpReservaS"
        Try
            objCmd.ExecuteNonQuery()
        Catch ex As Exception
            'se a temporária não existe ignora o erro
        End Try
        'Seleciona os valores de Reserva de Potência do COSR-S (SUL)
        objCmd.CommandText = "SELECT intreser AS intervalo, (NVL(reserpri, 0) + NVL(resersec, 0)) AS valReserva " &
                            "FROM reser " &
                            "WHERE codempre = 'RS' " &
                            "AND datareser = '" & strDataReser & "' " &
                            "INTO TEMP tmpReservaS " &
                            "WITH NO LOG"
        objCmd.ExecuteNonQuery()

        'Seleciona todos os dados das tabelas temporárias
        'A partir de fevereiro de 2008 a geração de UTAL deixou de ser subtraida do intercâmbio líquido

        '-- Alterado conforme IM44972 (Este bloco é novo) - Início
        objCmd.CommandText = "SELECT gl.intervalo, ct.valcarga, gl.valgeracao, i.valinterliq, (ifu.valintersup + (c.valcargasup - gu.valdespasup)) AS valprogsuse, ice.valintersup AS valliqce, (c2.valcargasup - gu2.valdespasup) AS valFluxoRG, NVL(r.valreserva,0) AS valreserva " &
                            "FROM tmpGeraLiq AS gl, tmpCargaS AS ct, tmpIntercambio AS i, tmpInterFU AS ifu, tmpGerUsina AS gu, tmpInterCE AS ice, tmpGerUsina2 AS gu2, carga AS c, carga AS c2, OUTER tmpReservaS AS r " &
                            "WHERE gl.intervalo = ct.intervalo " &
                            "AND ct.intervalo = i.intervalo " &
                            "AND i.intervalo = ifu.intervalo " &
                            "AND ifu.intervalo = gu.intervalo " &
                            "AND gu.intervalo = ice.intervalo " &
                            "AND ice.intervalo = gu2.intervalo " &
                            "AND gu2.intervalo = c.intcarga " &
                            "AND c.codempre = 'ER' " &
                            "AND c.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                            "AND c.intcarga = r.intervalo " &
                            "AND c.datpdp = c2.datpdp " &
                            "AND c.intcarga = c2.intcarga " &
                            "AND c2.codempre = 'CE' " &
                            "ORDER BY gl.intervalo"

        '-- Alterado conforme IM44972 (Antigo)
        'objCmd.CommandText = "SELECT gl.intervalo, ct.valcarga, gl.valgeracao, i.valinterliq, (ifu.valintersup + (c.valcargasup - gu.valdespasup)) AS valprogsuse, ice.valintersup AS valliqce, ((ice.valintersup * -1) - gu2.valdespasup + (d.valdespasup * -1)) AS valFluxoRG, NVL(r.valreserva,0) AS valreserva " & _
        '                    "FROM tmpGeraLiq AS gl, tmpCargaS AS ct, tmpIntercambio AS i, tmpInterFU AS ifu, tmpGerUsina AS gu, tmpInterCE AS ice, tmpGerUsina2 AS gu2, carga AS c, despa AS d, OUTER tmpReservaS AS r " & _
        '                    "WHERE gl.intervalo = ct.intervalo " & _
        '                    "AND ct.intervalo = i.intervalo " & _
        '                    "AND i.intervalo = ifu.intervalo " & _
        '                    "AND ifu.intervalo = gu.intervalo " & _
        '                    "AND gu.intervalo = ice.intervalo " & _
        '                    "AND ice.intervalo = gu2.intervalo " & _
        '                    "AND gu2.intervalo = c.intcarga " & _
        '                    "AND c.codempre = 'ER' " & _
        '                    "AND c.datpdp = '" & Page.Request.QueryString("strData") & "' " & _
        '                    "AND c.datpdp = d.datpdp " & _
        '                    "AND c.intcarga = d.intdespa " & _
        '                    "AND d.codusina = 'ESCUR' " & _
        '                    "AND d.intdespa = r.intervalo " & _
        '                    "ORDER BY gl.intervalo"
        'objCmd.CommandText = "SELECT gl.intervalo, ct.valcarga, gl.valgeracao, (i.valinterliq - g.valgeracao) AS valinterliq, (ifu.valintersup + (c.valcargasup - gu.valdespasup)) AS valprogsuse, ice.valintersup AS valliqce, ((ice.valintersup * -1) - gu2.valdespasup + (d.valdespasup * -1)) AS valFluxoRG, r.valreserva " & _
        '                     "FROM tmpGeraLiq AS gl, tmpCargaS AS ct, tmpIntercambio AS i, tmpGeracao AS g, tmpInterFU AS ifu, tmpGerUsina AS gu, tmpInterCE AS ice, tmpGerUsina2 AS gu2, carga AS c, tmpReservaS AS r, despa d " & _
        '                     "WHERE gl.intervalo = ct.intervalo " & _
        '                     "AND ct.intervalo = i.intervalo " & _
        '                     "AND i.intervalo = g.intervalo " & _
        '                     "AND g.intervalo = ifu.intervalo " & _
        '                     "AND ifu.intervalo = gu.intervalo " & _
        '                     "AND gu.intervalo = ice.intervalo " & _
        '                     "AND ice.intervalo = gu2.intervalo " & _
        '                     "AND gu2.intervalo = r.intervalo " & _
        '                     "AND r.intervalo = c.intcarga " & _
        '                     "AND c.codempre = 'ER' " & _
        '                     "AND c.datpdp = '" & Page.Request.QueryString("strData") & "' " & _
        '                     "AND c.datpdp = d.datpdp " & _
        '                     "AND c.intcarga = d.intdespa " & _
        '                     "AND d.codusina = 'ESCUR' " & _
        '                     "ORDER BY gl.intervalo"
        'objCmd.CommandText = "SELECT gl.intervalo, ct.valcarga, gl.valgeracao, (i.valinterliq - g.valgeracao) AS valinterliq, (ifu.valintersup + (c.valcargasup - gu.valdespasup)) AS valprogsuse, ice.valintersup AS valliqce, ico.valintersup AS valliqco, ((ice.valintersup * -1) - gu2.valdespasup) AS valFluxoRG, r2ce.valintersup AS valr2ce, r2co.valintersup AS valr2co, r.valreserva " & _
        '                     "FROM tmpGeraLiq AS gl, tmpCargaS AS ct, tmpIntercambio AS i, tmpGeracao AS g, tmpInterFU AS ifu, tmpGerUsina AS gu, tmpInterCE AS ice, tmpGerUsina2 AS gu2, tmpInterCO AS ico, carga AS c, tmpR2CE AS r2ce, tmpR2CO AS r2co, tmpReservaS AS r " & _
        '                     "WHERE gl.intervalo = ct.intervalo " & _
        '                     "AND ct.intervalo = i.intervalo " & _
        '                     "AND i.intervalo = g.intervalo " & _
        '                     "AND g.intervalo = ifu.intervalo " & _
        '                     "AND ifu.intervalo = gu.intervalo " & _
        '                     "AND gu.intervalo = ice.intervalo " & _
        '                     "AND ice.intervalo = gu2.intervalo " & _
        '                     "AND gu2.intervalo = ico.intervalo " & _
        '                     "AND ico.intervalo = r2ce.intervalo " & _
        '                     "AND r2ce.intervalo = r2co.intervalo " & _
        '                     "AND r2co.intervalo = r.intervalo " & _
        '                     "AND r.intervalo = c.intcarga " & _
        '                     "AND c.codempre = 'ER' " & _
        '                     "AND c.datpdp = '" & Page.Request.QueryString("strData") & "' " & _
        '                     "ORDER BY gl.intervalo"
        objCmd.CommandType = CommandType.Text
        Dim objDA As New OnsClasses.OnsData.OnsDataAdapter
        objDA.SelectCommand = objCmd
        Dim objDS As New dtsRelatorio
        objDA.Fill(objDS, "InterS")
        Session("report_ds") = objDS
        objCon.Close()
    End Sub

    Private Sub CarregaDSCargaInter()
        Dim objCon As New OnsClasses.OnsData.OnsConnection
        Dim objCmd As New OnsClasses.OnsData.OnsCommand

        objCon.Open("rpdp")
        objCmd.Connection = objCon

        'Elimina a tabela temporária com o Intercâmbio de FURNAS
        objCmd.CommandText = "Drop Table tmpInterFU"
        Try
            objCmd.ExecuteNonQuery()
        Catch ex As Exception
            'se a temporária não existe ignora o erro
        End Try
        'Seleciona os valores de Intercâmbio de FURNAS
        objCmd.CommandText = "Select Sum(valintersup) As valintersup, " &
                            "       intinter As intervalo " &
                            "From inter " &
                            "Where datpdp = '" & Page.Request.QueryString("strData") & "' And " &
                            "      codemprede = 'RS' And " &
                            "      codemprepara = 'RE' " &
                            "Group By intinter " &
                            "Into Temp tmpInterRE " &
                            "With No Log"
        objCmd.ExecuteNonQuery()

        ''Elimina a tabela temporária com o Intercâmbio de CO
        'objCmd.CommandText = "Drop Table tmpInterCO"
        'Try
        '    objCmd.ExecuteNonQuery()
        'Catch ex As Exception
        '    'se a temporária não existe ignora o erro
        'End Try
        ''Seleciona os valores de Intercâmbio de COPEL
        'objCmd.CommandText = "Select Sum(valintersup) As valintersup, " & _
        '                     "       intinter As intervalo " & _
        '                     "From inter " & _
        '                     "Where datpdp = '" & Page.Request.QueryString("strData") & "' And " & _
        '                     "      codemprede = 'RS' And " & _
        '                     "      codemprepara = 'CO' " & _
        '                     "Group By intinter " & _
        '                     "Into Temp tmpInterCO " & _
        '                     "With No Log"
        'objCmd.ExecuteNonQuery()

        '''Retira após descontratação do COS-RG
        ''''Elimina a tabela temporária com a Geração da Usina UTAL
        '''objCmd.CommandText = "Drop Table tmpInterCE"
        '''Try
        '''    objCmd.ExecuteNonQuery()
        '''Catch ex As Exception
        '''    'se a temporária não existe ignora o erro
        '''End Try
        ''''Seleciona os valores de Intercâmbio de CEEE
        '''objCmd.CommandText = "Select Sum(valintersup) As valintersup, " & _
        '''                     "       intinter As intervalo " & _
        '''                     "From inter " & _
        '''                     "Where datpdp = '" & Page.Request.QueryString("strData") & "' And " & _
        '''                     "      codemprede = 'RS' And " & _
        '''                     "      codemprepara = 'CE' " & _
        '''                     "Group By intinter " & _
        '''                     "Into Temp tmpInterCE " & _
        '''                     "With No Log"
        '''objCmd.ExecuteNonQuery()

        ''Elimina a tabela temporária da Geração Total do Regional Sul
        'objCmd.CommandText = "Drop Table tmpGerTotalS"
        'Try
        '    objCmd.ExecuteNonQuery()
        'Catch ex As Exception
        '    'se a temporária não existe ignora o erro
        'End Try

        'objCmd.CommandText = "SELECT d.intdespa AS intervalo, SUM(d.valdespasup) AS valgertotal " & _
        '                     "FROM despa d, usina u, empre e " & _
        '                     "WHERE datpdp = '" & Page.Request.QueryString("strData") & "' " & _
        '                     "AND d.codusina = u.codusina " & _
        '                     "AND (u.tipusina = 'H' " & _
        '                     "OR u.tipusina = 'T') " & _
        '                     "AND u.codusina <> 'TSPTER' " & _
        '                     "AND u.codusina <> 'TSUTWE' " & _
        '                     "AND u.codusina <> 'ERPTER' " & _
        '                     "AND u.codusina <> 'SCPTER' " & _
        '                     "AND u.codusina <> 'TSPHID' " & _
        '                     "AND u.codempre = e.codempre " & _
        '                     "AND e.codarea = 'RS' " & _
        '                     "GROUP BY 1 " & _
        '                     "INTO TEMP tmpGerTotalS " & _
        '                     "WITH NO LOG"
        'objCmd.ExecuteNonQuery()


        'Seleciona todos os dados das tabelas temporárias
        objCmd.CommandText = "SELECT cer.intcarga As intervalo, cer.valcargasup AS valCargaER, " &
                            "crs.valcargasup AS valCargaRS, csc.valcargasup AS valCargaSC, " &
                            "cts.valcargasup AS valCargaTS, cco.valcargasup AS valCargaCO, " &
                            "cce.valcargasup AS valCargaCE, ire.valintersup AS valInterFU " &
                            "FROM carga AS cer, carga AS crs, carga AS csc, carga AS cts, " &
                            "carga AS cce, carga AS cco, tmpInterRE AS ire " &
                            "WHERE cer.datpdp = '" & Page.Request.QueryString("strData") & "' " &
                            "AND cer.codempre = 'ER' " &
                            "AND cer.datpdp = crs.datpdp " &
                            "AND cer.intcarga = crs.intcarga " &
                            "AND crs.codempre = 'RS' " &
                            "AND crs.datpdp = csc.datpdp " &
                            "AND crs.intcarga = csc.intcarga " &
                            "AND csc.codempre = 'SC' " &
                            "AND csc.datpdp = cts.datpdp " &
                            "AND csc.intcarga = cts.intcarga " &
                            "AND cts.codempre = 'TS' " &
                            "AND cts.intcarga = cce.intcarga " &
                            "AND cts.datpdp = cce.datpdp " &
                            "AND cce.codempre = 'CE' " &
                            "AND cce.intcarga = cco.intcarga " &
                            "AND cce.datpdp = cco.datpdp " &
                            "AND cco.codempre = 'CO' " &
                            "AND cco.intcarga = ire.intervalo " &
                            "ORDER BY cer.intcarga"
        'objCmd.CommandText = "SELECT cer.intcarga As intervalo, cer.valcargasup AS valCargaER, " & _
        '                     "crs.valcargasup AS valCargaRS, csc.valcargasup AS valCargaSC, " & _
        '                     "cts.valcargasup AS valCargaTS, ico.valintersup AS valInterCO, " & _
        '                     "ire.valintersup AS valInterFU, gts.valgertotal AS valGerTotal " & _
        '                     "FROM carga AS cer, carga AS crs, carga AS csc, carga AS cts, " & _
        '                     "tmpInterRE AS ire, tmpInterCO AS ico, tmpGerTotalS AS gts " & _
        '                     "WHERE cer.datpdp = '" & Page.Request.QueryString("strData") & "' " & _
        '                     "AND cer.codempre = 'ER' " & _
        '                     "AND cer.datpdp = crs.datpdp " & _
        '                     "AND cer.intcarga = crs.intcarga " & _
        '                     "AND crs.codempre = 'RS' " & _
        '                     "AND crs.datpdp = csc.datpdp " & _
        '                     "AND crs.intcarga = csc.intcarga " & _
        '                     "AND csc.codempre = 'SC' " & _
        '                     "AND csc.datpdp = cts.datpdp " & _
        '                     "AND csc.intcarga = cts.intcarga " & _
        '                     "AND cts.codempre = 'TS' " & _
        '                     "AND cts.intcarga = ire.intervalo " & _
        '                     "AND ire.intervalo = ico.intervalo " & _
        '                     "AND ico.intervalo = gts.intervalo " & _
        '                     "ORDER BY cer.intcarga"
        objCmd.CommandType = CommandType.Text
        Dim objDA As New OnsClasses.OnsData.OnsDataAdapter
        objDA.SelectCommand = objCmd
        Dim objDS As New dtsRelatorio
        objDA.Fill(objDS, "CargaInterS")
        Session("report_ds") = objDS
        objCon.Close()
    End Sub

    Private Sub MostraRelatorio()
        If Request.QueryString("strRelatorio").Trim = "relVazaoCNOS" And Request.QueryString("strAgrega") = "bac" Then
            strNomeRel = "relVazaoCNOS2"
        Else
            strNomeRel = Request.QueryString("strRelatorio")
        End If
        ViewState("nomeRel") = strNomeRel & "_" & Format(Now, "yyMMdd_HHmmss")

        objReport = New ReportDocument
        objReport.Load(Server.MapPath("Relatorios\" + strNomeRel + ".rpt"), OpenReportMethod.OpenReportByDefault)
        'objReport.Refresh()

        'dataset
        Dim objDataSource As DataSet = CType(Session("report_ds"), DataSet)
        Dim datPDP As Date = CType(Request.QueryString("strData").Substring(6, 2) & "/" &
                                Request.QueryString("strData").Substring(4, 2) & "/" &
                                Request.QueryString("strData").Substring(0, 4), Date)
        objReport.DataDefinition.FormulaFields.Reset()
        objReport.DataDefinition.FormulaFields("fmlDiaSemana").Text = "'" & Format(datPDP, "dd/MM/yyyy") & " " & DiaSemana(datPDP.DayOfWeek()) & "'"

        Select Case strNomeRel
            Case Is = "relGeracao"
                Select Case Request.QueryString("strTipoGer")
                    Case Is = "Hidro"
                        objReport.DataDefinition.FormulaFields("fmlTitulo").Text = "'PROG. DIÁRIA - GERAÇÃO HIDRO (MW)'"
                        objReport.DataDefinition.FormulaFields("fmlSubTitulo").Text = "'CENTRO REGIONAL SUL'"
                    Case Is = "Termo"
                        objReport.DataDefinition.FormulaFields("fmlTitulo").Text = "'PROG. DIÁRIA - GERAÇÃO TERMO (MW)'"
                        objReport.DataDefinition.FormulaFields("fmlSubTitulo").Text = "'CENTRO REGIONAL SUL'"
                    Case Is = "GeracaoSE"
                        objReport.DataDefinition.FormulaFields("fmlTitulo").Text = "'PROG. DIÁRIA - GERAÇÃO POR USINA'"
                        objReport.DataDefinition.FormulaFields("fmlSubTitulo").Text = "'CENTRO REGIONAL SUDESTE'"
                    Case Is = "GerHidroNE"
                        objReport.DataDefinition.FormulaFields("fmlTitulo").Text = "'GERAÇÃO HORÁRIA DAS USINAS HIDRÁULICAS'"
                        objReport.DataDefinition.FormulaFields("fmlSubTitulo").Text = "'CENTRO REGIONAL NORDESTE'"
                    Case Is = "GerTermoNE"
                        objReport.DataDefinition.FormulaFields("fmlTitulo").Text = "'GERAÇÃO HORÁRIA DAS USINAS TÉRMICAS E EÓLICAS'"
                        objReport.DataDefinition.FormulaFields("fmlSubTitulo").Text = "'CENTRO REGIONAL NORDESTE'"
                    Case Is = "GeracaoCargaS"
                        objReport.DataDefinition.FormulaFields("fmlTitulo").Text = "'PROG. DIÁRIA - GERAÇÃO DA CEEE E CARGA RGS (MW)'"
                        objReport.DataDefinition.FormulaFields("fmlSubTitulo").Text = "'CENTRO REGIONAL SUL'"
                    Case Is = "GeracaoCargaCOPEL"
                        objReport.DataDefinition.FormulaFields("fmlTitulo").Text = "'PROG. DIÁRIA - CARGA E GERAÇÃO DA COPEL (MW)'"
                        objReport.DataDefinition.FormulaFields("fmlSubTitulo").Text = "'CENTRO REGIONAL SUL'"
                End Select
            Case Is = "relGeracaoNCO"
                Select Case Request.QueryString("strTipoGer")

                    Case Is = "GerHidroNCO"
                        objReport.DataDefinition.FormulaFields("fmlTitulo").Text = "'GERAÇÃO HORÁRIA DAS USINAS HIDRÁULICAS'"
                        objReport.DataDefinition.FormulaFields("fmlSubTitulo").Text = "'CENTRO REGIONAL NORTE/CENTRO-OESTE'"
                    Case Is = "GerTermoNCO"
                        objReport.DataDefinition.FormulaFields("fmlTitulo").Text = "'GERAÇÃO HORÁRIA DAS USINAS TÉRMICAS'"
                        objReport.DataDefinition.FormulaFields("fmlSubTitulo").Text = "'CENTRO REGIONAL NORTE/CENTRO-OESTE'"
                    Case Is = "CargaNCO"
                        objReport.DataDefinition.FormulaFields("fmlTitulo").Text = "'PREVISÃO DE CARGA E INTERCÂMBIOS'"
                        objReport.DataDefinition.FormulaFields("fmlSubTitulo").Text = "'CENTRO REGIONAL NORTE/CENTRO-OESTE'"
                End Select

            Case Is = "relInterS"
                objReport.DataDefinition.FormulaFields("fmlTitulo").Text = "'PROG. DIÁRIA - INTERCÂMBIOS PROG/REPROG (MW)'"
                objReport.DataDefinition.FormulaFields("fmlSubTitulo").Text = "'CENTRO REGIONAL SUL'"

            Case Is = "relUsiPorEmpre"
                '-- IM44972 - Novo Relatório de Usina por Empresa
                objReport.DataDefinition.FormulaFields("fmlTitulo").Text = "'RELAÇÃO DE USINAS POR EMPRESA'"
                objReport.DataDefinition.FormulaFields("fmlSubTitulo").Text = "'CENTRO REGIONAL SUL'"

            Case Is = "relCargaInterS"
                objReport.DataDefinition.FormulaFields("fmlTitulo").Text = "'PROG. DIÁRIA - CARGAS E INTERCÂMBIOS (MW)'"
                objReport.DataDefinition.FormulaFields("fmlSubTitulo").Text = "'CENTRO REGIONAL SUL'"
            Case Is = "relInflexUsinaS"
                objReport.DataDefinition.FormulaFields("fmlTitulo").Text = "'PROG. DIÁRIA - INFLEXIBILIDADES POR USINA (MW)'"
                objReport.DataDefinition.FormulaFields("fmlSubTitulo").Text = "'CENTRO REGIONAL SUL'"
            Case Is = "relInflexTotalS"
                objReport.DataDefinition.FormulaFields("fmlTitulo").Text = "'PROG. DIÁRIA - INFLEXIBILIDADES TOTAIS (MW)'"
                objReport.DataDefinition.FormulaFields("fmlSubTitulo").Text = "'CENTRO REGIONAL SUL'"
            Case Is = "relCarga"
                Select Case Request.QueryString("strTipoGer")
                    Case Is = "CargaSE"
                        objReport.DataDefinition.FormulaFields("fmlTitulo").Text = "'PREVISÃO DE CARGA'"
                        objReport.DataDefinition.FormulaFields("fmlSubTitulo").Text = "'CENTRO REGIONAL SUDESTE'"
                    Case Is = "CargaNE"
                        objReport.DataDefinition.FormulaFields("fmlTitulo").Text = "'REQUISITOS POR ÁREAS'"
                        objReport.DataDefinition.FormulaFields("fmlSubTitulo").Text = "'CENTRO REGIONAL NORDESTE'"
                End Select
                'Case Is = "relCargaNCO"
                '    objReport.DataDefinition.FormulaFields("fmlTitulo").Text = "'INTERCÂMBIOS'"
                '    objReport.DataDefinition.FormulaFields("fmlSubTitulo").Text = "'CENTRO REGIONAL NORTE/CENTRO-OESTE'"
            Case Is = "relVazao"
                objReport.DataDefinition.FormulaFields("fmlTitulo").Text = "'PROGRAMA DE DEFLUÊNCIA POR USINA HIDRELÉTRICA'"
                objReport.DataDefinition.FormulaFields("fmlSubTitulo").Text = "'CENTRO REGIONAL SUDESTE'"
            Case Is = "relInter"
                objReport.DataDefinition.FormulaFields("fmlTitulo").Text = "'INTERCÂMBIO LÍQUIDO'"
                objReport.DataDefinition.FormulaFields("fmlSubTitulo").Text = "'CENTRO REGIONAL SUDESTE'"
            Case Is = "relTransf"
                objReport.DataDefinition.FormulaFields("fmlTitulo").Text = "'TRANSFERÊNCIA DE ENERGIA ENTRE REGIÕES'"
                objReport.DataDefinition.FormulaFields("fmlSubTitulo").Text = "'CENTRO REGIONAL SUDESTE'"
            Case Is = "relRestr"
                objReport.DataDefinition.FormulaFields("fmlTitulo").Text = "'RESTRIÇÃO OPERATIVA DE USINAS'"
                objReport.DataDefinition.FormulaFields("fmlSubTitulo").Text = "'CENTRO REGIONAL SUDESTE'"
            Case Is = "relFolga"
                objReport.DataDefinition.FormulaFields("fmlTitulo").Text = "'FOLGA DE POTÊNCIA POR USINA HIDRELÉTRICA'"
                objReport.DataDefinition.FormulaFields("fmlSubTitulo").Text = "'CENTRO REGIONAL SUDESTE'"
            Case Is = "relBalanco"
                objReport.DataDefinition.FormulaFields("fmlTitulo").Text = "'BALANÇO DE ENERGIA POR AGENTE'"
                objReport.DataDefinition.FormulaFields("fmlSubTitulo").Text = "'CENTRO REGIONAL SUDESTE'"
            Case Is = "relReserva"
                objReport.DataDefinition.FormulaFields("fmlTitulo").Text = "'RESERVA DE POTÊNCIA POR AGENTE'"
                objReport.DataDefinition.FormulaFields("fmlSubTitulo").Text = "'CENTRO REGIONAL SUDESTE'"
            Case Is = "relRazao"
                objReport.DataDefinition.FormulaFields("fmlTitulo").Text = "'RAZÃO DO DESPACHO DAS USINAS TÉRMICAS'"
                objReport.DataDefinition.FormulaFields("fmlSubTitulo").Text = "'CENTRO REGIONAL SUDESTE'"
            Case Is = "relConfig"
                objReport.DataDefinition.FormulaFields("fmlTitulo").Text = "'CONFIG UNIDADES GERADORAS USINAS HIDRÁULICAS'"
                objReport.DataDefinition.FormulaFields("fmlSubTitulo").Text = "'CENTRO REGIONAL SUDESTE'"
            Case Is = "relParada"
                If Request.QueryString("strTipoGer") = "ParadaNE_H" Then
                    objReport.DataDefinition.FormulaFields("fmlTitulo").Text = "'CRONOG PARADA DE MÁQUINAS HIDRÁULICAS E TÉRMICAS'"
                ElseIf Request.QueryString("strTipoGer") = "ParadaNE_T" Then
                    objReport.DataDefinition.FormulaFields("fmlTitulo").Text = "'CRONOGRAMA DE PARADA DE MÁQUINAS TÉRMICAS E EÓLICAS'"
                Else
                    objReport.DataDefinition.FormulaFields("fmlTitulo").Text = "'CRONOGRAMA DE PARADA DE MÁQUINAS'"
                End If
                objReport.DataDefinition.FormulaFields("fmlSubTitulo").Text = "'CENTRO REGIONAL NORDESTE'"
            Case Is = "relReqGer"
                'Seleciona os valores de Requisito e Reserva
                Dim objCon As New OnsClasses.OnsData.OnsConnection
                Dim objCmd As New OnsClasses.OnsData.OnsCommand

                objCon.Open("rpdp")
                objCmd.Connection = objCon
                '-- CRQ4434 (14/03/2013) - seleção de requisitos pela view VW_RELPDP_NE_REQUISITO_GERACAO
                'objCmd.CommandText = "SELECT valreqmax, hreqmax, valresreqmax, hresreqmax " & _
                '                    "FROM requisitos_area " & _
                '                    "WHERE datpdp = '" & Page.Request.QueryString("strData") & "' " & _
                '                    "AND codarea = 'NE'"
                objCmd.CommandText = "SELECT valreqmax, hreqmax, valresreqmax, hresreqmax " &
                                    "FROM VW_RELPDP_NE_REQUISITO_GERACAO " &
                                    "WHERE datpdp = '" & Page.Request.QueryString("strData") & "' "
                Dim dtrRequisito As OnsClasses.OnsData.OnsDataReader = objCmd.ExecuteReader
                intRequisito = 0
                strRequisito = ""
                intReserva = 0
                strReserva = ""
                If dtrRequisito.Read Then
                    intRequisito = dtrRequisito("valreqmax")
                    strRequisito = dtrRequisito("hreqmax")
                    intReserva = dtrRequisito("valresreqmax")
                    strReserva = dtrRequisito("hresreqmax")
                End If
                dtrRequisito.Close()
                dtrRequisito = Nothing
                objCon.Close()

                objReport.DataDefinition.FormulaFields("fmlVlrRequisito").Text = "'" & intRequisito.ToString.Trim & "'"
                objReport.DataDefinition.FormulaFields("fmlHorRequisito").Text = "'" & strRequisito.Trim & "'"
                objReport.DataDefinition.FormulaFields("fmlVlrReserva").Text = "'" & intReserva.ToString.Trim & "'"
                objReport.DataDefinition.FormulaFields("fmlHorReserva").Text = "'" & strReserva.Trim & "'"
                objReport.DataDefinition.FormulaFields("fmlTitulo").Text = "'REQUISITO / GERAÇÃO'"
                objReport.DataDefinition.FormulaFields("fmlSubTitulo").Text = "'CENTRO REGIONAL NORDESTE'"
            Case Is = "relCargaCNOS"
                Select Case Request.QueryString("strAgrega")
                    Case Is = "age"
                        objReport.DataDefinition.FormulaFields("fmlTitulo").Text = "'PREVISÃO DE CARGA POR AGENTE'"
                    Case Is = "are"
                        objReport.DataDefinition.FormulaFields("fmlTitulo").Text = "'PREVISÃO DE CARGA POR ÁREA'"
                    Case Is = "reg"
                        objReport.DataDefinition.FormulaFields("fmlTitulo").Text = "'PREVISÃO DE CARGA POR REGIÃO'"
                    Case Is = "sis"
                        objReport.DataDefinition.FormulaFields("fmlTitulo").Text = "'PREVISÃO DE CARGA POR SUBSISTEMA'"
                    Case Is = "sbr"
                        objReport.DataDefinition.FormulaFields("fmlTitulo").Text = "'PREVISÃO DE CARGA POR SIN'"
                End Select
                objReport.DataDefinition.FormulaFields("fmlSubTitulo").Text = "'CENTRO NACIONAL DE OPERAÇÃO DO SISTEMA'"
            Case Is = "relGeracaoCNOS"
                Select Case Request.QueryString("strAgrega")
                    Case Is = "age"
                        objReport.DataDefinition.FormulaFields("fmlTitulo").Text = "'PROGRAMA DE GERAÇÃO DAS USINAS POR AGENTE'"
                    Case Is = "are"
                        objReport.DataDefinition.FormulaFields("fmlTitulo").Text = "'PROGRAMA DE GERAÇÃO DAS USINAS POR ÁREA'"
                    Case Is = "reg"
                        objReport.DataDefinition.FormulaFields("fmlTitulo").Text = "'PROGRAMA DE GERAÇÃO DAS USINAS POR REGIÃO'"
                    Case Is = "sis"
                        objReport.DataDefinition.FormulaFields("fmlTitulo").Text = "'PROGRAMA DE GERAÇÃO DAS USINAS POR SUBSISTEMA'"
                    Case Is = "sbr"
                        objReport.DataDefinition.FormulaFields("fmlTitulo").Text = "'PROGRAMA DE GERAÇÃO DAS USINAS POR SIN'"
                End Select
                objReport.DataDefinition.FormulaFields("fmlSubTitulo").Text = "'CENTRO NACIONAL DE OPERAÇÃO DO SISTEMA'"
            Case Is = "relVazaoCNOS"
                objReport.DataDefinition.FormulaFields("fmlTitulo").Text = "'PROGRAMA DE DEFLUÊNCIA NAS USINAS POR AGENTE'"
                objReport.DataDefinition.FormulaFields("fmlSubTitulo").Text = "'CENTRO NACIONAL DE OPERAÇÃO DO SISTEMA'"
            Case Is = "relVazaoCNOS2"
                objReport.DataDefinition.FormulaFields("fmlTitulo").Text = "'PROGRAMA DE DEFLUÊNCIA NAS USINAS POR BACIA'"
                objReport.DataDefinition.FormulaFields("fmlSubTitulo").Text = "'CENTRO NACIONAL DE OPERAÇÃO DO SISTEMA'"
            Case Is = "relInterCNOS"
                objReport.DataDefinition.FormulaFields("fmlTitulo").Text = "'INTERCÂMBIO LÍQUIDO POR CENTRO'"
                objReport.DataDefinition.FormulaFields("fmlSubTitulo").Text = "'CENTRO NACIONAL DE OPERAÇÃO DO SISTEMA'"
            Case Is = "relTransfCNOS"
                objReport.DataDefinition.FormulaFields("fmlTitulo").Text = "'TRANSFERÊNCIAS DE ENERGIA ENTRE REGIÕES'"
                objReport.DataDefinition.FormulaFields("fmlSubTitulo").Text = "'CENTRO NACIONAL DE OPERAÇÃO DO SISTEMA'"
            Case Is = "relRestrCNOS"
                Select Case Request.QueryString("strAgrega")
                    Case Is = "age"
                        objReport.DataDefinition.FormulaFields("fmlTitulo").Text = "'RESTRIÇÕES OPERATIVAS DAS USINAS POR AGENTE'"
                    Case Is = "are"
                        objReport.DataDefinition.FormulaFields("fmlTitulo").Text = "'RESTRIÇÕES OPERATIVAS DAS USINAS POR ÁREA'"
                    Case Is = "reg"
                        objReport.DataDefinition.FormulaFields("fmlTitulo").Text = "'RESTRIÇÕES OPERATIVAS DAS USINAS POR REGIÃO'"
                    Case Is = "sis"
                        objReport.DataDefinition.FormulaFields("fmlTitulo").Text = "'RESTRIÇÕES OPERATIVAS DAS USINAS POR SUBSISTEMA'"
                    Case Is = "sbr"
                        objReport.DataDefinition.FormulaFields("fmlTitulo").Text = "'RESTRIÇÕES OPERATIVAS DAS USINAS POR SIN'"
                End Select
                objReport.DataDefinition.FormulaFields("fmlSubTitulo").Text = "'CENTRO NACIONAL DE OPERAÇÃO DO SISTEMA'"
            Case Is = "relReservaCNOS"
                Select Case Request.QueryString("strAgrega")
                    Case Is = "age"
                        objReport.DataDefinition.FormulaFields("fmlTitulo").Text = "'RESERVA DE POTÊNCIA POR AGENTE'"
                    Case Is = "are"
                        objReport.DataDefinition.FormulaFields("fmlTitulo").Text = "'RESERVA DE POTÊNCIA POR ÁREA'"
                    Case Is = "reg"
                        objReport.DataDefinition.FormulaFields("fmlTitulo").Text = "'RESERVA DE POTÊNCIA POR REGIÃO'"
                    Case Is = "sis"
                        objReport.DataDefinition.FormulaFields("fmlTitulo").Text = "'RESERVA DE POTÊNCIA POR SUBSISTEMA'"
                    Case Is = "sbr"
                        objReport.DataDefinition.FormulaFields("fmlTitulo").Text = "'RESERVA DE POTÊNCIA POR SIN'"
                End Select
                objReport.DataDefinition.FormulaFields("fmlSubTitulo").Text = "'CENTRO NACIONAL DE OPERAÇÃO DO SISTEMA'"
            Case Is = "relRazaoCNOS"
                Select Case Request.QueryString("strAgrega")
                    Case Is = "age"
                        objReport.DataDefinition.FormulaFields("fmlTitulo").Text = "'RAZÕES DO DESPACHO POR AGENTE'"
                    Case Is = "are"
                        objReport.DataDefinition.FormulaFields("fmlTitulo").Text = "'RAZÕES DO DESPACHO POR ÁREA'"
                    Case Is = "reg"
                        objReport.DataDefinition.FormulaFields("fmlTitulo").Text = "'RAZÕES DO DESPACHO POR REGIÃO'"
                    Case Is = "sis"
                        objReport.DataDefinition.FormulaFields("fmlTitulo").Text = "'RAZÕES DO DESPACHO POR SUBSISTEMA'"
                    Case Is = "sbr"
                        objReport.DataDefinition.FormulaFields("fmlTitulo").Text = "'RAZÕES DO DESPACHO POR SIN'"
                End Select
                objReport.DataDefinition.FormulaFields("fmlSubTitulo").Text = "'CENTRO NACIONAL DE OPERAÇÃO DO SISTEMA'"
            Case Is = "relBalancoCNOS"
                Select Case Request.QueryString("strAgrega")
                    Case Is = "age"
                        objReport.DataDefinition.FormulaFields("fmlTitulo").Text = "'BALANÇO DE ENERGIA POR AGENTE'"
                    Case Is = "are"
                        objReport.DataDefinition.FormulaFields("fmlTitulo").Text = "'BALANÇO DE ENERGIA POR ÁREA'"
                    Case Is = "reg"
                        objReport.DataDefinition.FormulaFields("fmlTitulo").Text = "'BALANÇO DE ENERGIA POR REGIÃO'"
                    Case Is = "sis"
                        objReport.DataDefinition.FormulaFields("fmlTitulo").Text = "'BALANÇO DE ENERGIA POR SUBSISTEMA'"
                    Case Is = "sbr"
                        objReport.DataDefinition.FormulaFields("fmlTitulo").Text = "'BALANÇO DE ENERGIA POR SIN'"
                End Select
                objReport.DataDefinition.FormulaFields("fmlSubTitulo").Text = "'CENTRO NACIONAL DE OPERAÇÃO DO SISTEMA'"
            Case Is = "relConfigCNOS"
                Select Case Request.QueryString("strAgrega")
                    Case Is = "age"
                        objReport.DataDefinition.FormulaFields("fmlTitulo").Text = "'CONFIG UNID GERADORAS HIDRÁULICAS POR AGENTE'"
                    Case Is = "are"
                        objReport.DataDefinition.FormulaFields("fmlTitulo").Text = "'CONFIG UNID GERADORAS HIDRÁULICAS POR ÁREA'"
                    Case Is = "reg"
                        objReport.DataDefinition.FormulaFields("fmlTitulo").Text = "'CONFIG UNID GERADORAS HIDRÁULICAS POR REGIÃO'"
                    Case Is = "sis"
                        objReport.DataDefinition.FormulaFields("fmlTitulo").Text = "'CONFIG UNID GERADORAS HIDRÁULICAS POR SUBSISTEMA'"
                    Case Is = "sbr"
                        objReport.DataDefinition.FormulaFields("fmlTitulo").Text = "'CONFIG UNID GERADORAS HIDRÁULICAS POR SIN'"
                End Select
                objReport.DataDefinition.FormulaFields("fmlSubTitulo").Text = "'CENTRO NACIONAL DE OPERAÇÃO DO SISTEMA'"
            Case Is = "relFolgaCNOS"
                Select Case Request.QueryString("strAgrega")
                    Case Is = "age"
                        objReport.DataDefinition.FormulaFields("fmlTitulo").Text = "'FOLGA POTÊNCIA USINAS HIDRÁULICAS POR AGENTE'"
                    Case Is = "are"
                        objReport.DataDefinition.FormulaFields("fmlTitulo").Text = "'FOLGA POTÊNCIA USINAS HIDRÁULICAS POR ÁREA'"
                    Case Is = "reg"
                        objReport.DataDefinition.FormulaFields("fmlTitulo").Text = "'FOLGA POTÊNCIA USINAS HIDRÁULICAS POR REGIÃO'"
                    Case Is = "sis"
                        objReport.DataDefinition.FormulaFields("fmlTitulo").Text = "'FOLGA POTÊNCIA USINAS HIDRÁULICAS POR SUBSISTEMA'"
                    Case Is = "sbr"
                        objReport.DataDefinition.FormulaFields("fmlTitulo").Text = "'FOLGA POTÊNCIA USINAS HIDRÁULICAS POR SIN'"
                End Select
                objReport.DataDefinition.FormulaFields("fmlSubTitulo").Text = "'CENTRO NACIONAL DE OPERAÇÃO DO SISTEMA'"
            Case Is = "relPDDefluxSIN" '-- CRQ3713 (14/01/13)
                objReport.DataDefinition.FormulaFields("fmlTitulo").Text = "'PROGRAMA DIÁRIO DE DEFLUÊNCIAS DAS BACIAS DO SIN - PDF'"
                objReport.DataDefinition.FormulaFields("fmlSubTitulo").Text = "'ESCRITÓRIO CENTRAL'"
            Case Is = "relPDDefluxObs" '-- CRQ3713 (14/01/13)
                If Session("33") > 0 Then
                    objReport.DataDefinition.FormulaFields("fmlTitulo").Text = "'PROGRAMA DIÁRIO DE DEFLUÊNCIAS DAS BACIAS DO SIN - PDF'"
                    objReport.DataDefinition.FormulaFields("fmlSubTitulo").Text = "'ESCRITÓRIO CENTRAL'"
                Else
                    objReport.DataDefinition.FormulaFields("fmlTitulo").Text = ""
                    objReport.DataDefinition.FormulaFields("fmlSubTitulo").Text = ""
                End If
        End Select
        objReport.SetDataSource(objDataSource)
        Exporta(objReport, True, cboTipo.SelectedItem.Text)
        objReport.Close()
    End Sub

    Private Sub MostraRelatorioSubS()
        Dim objSubRepInter As ReportDocument
        Dim objSubRepHidro As ReportDocument
        Dim objSubRepTermo As ReportDocument
        Dim objSubRepCarInt As ReportDocument
        Dim objSubRepGerCop As ReportDocument
        Dim objSubRepGerCee As ReportDocument
        Dim objSubRepInflex As ReportDocument
        Dim objSubRepUsiEmpre As ReportDocument '-- IM44972

        strNomeRel = Request.QueryString("strRelatorio")
        ViewState("nomeRel") = strNomeRel & "_" & Format(Now, "yyMMdd_HHmmss")

        objReport = New ReportDocument
        objSubRepInter = New ReportDocument
        objSubRepHidro = New ReportDocument
        objSubRepTermo = New ReportDocument
        objSubRepCarInt = New ReportDocument
        objSubRepGerCop = New ReportDocument
        objSubRepGerCee = New ReportDocument
        objSubRepInflex = New ReportDocument
        objSubRepUsiEmpre = New ReportDocument '-- IM44972

        objReport.Load(Server.MapPath("Relatorios\" + ".rpt"), OpenReportMethod.OpenReportByDefault)

        Dim datPDP As Date = CType(Request.QueryString("strData").Substring(6, 2) & "/" &
                                Request.QueryString("strData").Substring(4, 2) & "/" &
                                Request.QueryString("strData").Substring(0, 4), Date)

        'Intercâmbio Prog/Reprog - SUL
        objSubRepInter = objReport.OpenSubreport("INTER")
        objSubRepInter.DataDefinition.FormulaFields.Reset()
        objSubRepInter.DataDefinition.FormulaFields("fmlTitulo").Text = "'PROG. DIÁRIA - INTERCÂMBIOS PROG/REPROG (MW)'"
        objSubRepInter.DataDefinition.FormulaFields("fmlSubTitulo").Text = "'CENTRO REGIONAL SUL'"
        objSubRepInter.DataDefinition.FormulaFields("fmlDiaSemana").Text = "'" & Format(datPDP, "dd/MM/yyyy") & " " & DiaSemana(datPDP.DayOfWeek()) & "'"
        BindReport("Inter")
        Dim objDSInter As DataSet = CType(Session("report_ds"), DataSet)
        objSubRepInter.SetDataSource(objDSInter)

        'Geração Hidro - SUL
        objSubRepHidro = objReport.OpenSubreport("HIDRO")
        objSubRepHidro.DataDefinition.FormulaFields.Reset()
        objSubRepHidro.DataDefinition.FormulaFields("fmlTitulo").Text = "'PROG. DIÁRIA - GERAÇÃO HIDRO (MW)'"
        objSubRepHidro.DataDefinition.FormulaFields("fmlSubTitulo").Text = "'CENTRO REGIONAL SUL'"
        objSubRepHidro.DataDefinition.FormulaFields("fmlDiaSemana").Text = "'" & Format(datPDP, "dd/MM/yyyy") & " " & DiaSemana(datPDP.DayOfWeek()) & "'"
        BindReport("Hidro")
        Dim objDSHidro As DataSet = CType(Session("report_ds"), DataSet)
        objSubRepHidro.SetDataSource(objDSHidro)

        'Geração Termo - SUL
        objSubRepTermo = objReport.OpenSubreport("TERMO")
        objSubRepTermo.DataDefinition.FormulaFields.Reset()
        objSubRepTermo.DataDefinition.FormulaFields("fmlTitulo").Text = "'PROG. DIÁRIA - GERAÇÃO TERMO (MW)'"
        objSubRepTermo.DataDefinition.FormulaFields("fmlSubTitulo").Text = "'CENTRO REGIONAL SUL'"
        objSubRepTermo.DataDefinition.FormulaFields("fmlDiaSemana").Text = "'" & Format(datPDP, "dd/MM/yyyy") & " " & DiaSemana(datPDP.DayOfWeek()) & "'"
        BindReport("Termo")
        Dim objDSTermo As DataSet = CType(Session("report_ds"), DataSet)
        objSubRepTermo.SetDataSource(objDSTermo)

        'Carga e Intercâmbio - SUL
        objSubRepCarInt = objReport.OpenSubreport("CARGAINTER")
        objSubRepCarInt.DataDefinition.FormulaFields.Reset()
        objSubRepCarInt.DataDefinition.FormulaFields("fmlTitulo").Text = "'PROG. DIÁRIA - CARGAS E INTERCÂMBIOS (MW)'"
        objSubRepCarInt.DataDefinition.FormulaFields("fmlSubTitulo").Text = "'CENTRO REGIONAL SUL'"
        objSubRepCarInt.DataDefinition.FormulaFields("fmlDiaSemana").Text = "'" & Format(datPDP, "dd/MM/yyyy") & " " & DiaSemana(datPDP.DayOfWeek()) & "'"
        BindReport("CargaInter")
        Dim objDSCarInt As DataSet = CType(Session("report_ds"), DataSet)
        objSubRepCarInt.SetDataSource(objDSCarInt)

        'Geração COPEL - SUL
        objSubRepGerCop = objReport.OpenSubreport("GERCOPEL")
        objSubRepGerCop.DataDefinition.FormulaFields.Reset()
        objSubRepGerCop.DataDefinition.FormulaFields("fmlTitulo").Text = "'PROG. DIÁRIA - CARGA E GERAÇÃO DA COPEL (MW)'"
        objSubRepGerCop.DataDefinition.FormulaFields("fmlSubTitulo").Text = "'CENTRO REGIONAL SUL'"
        objSubRepGerCop.DataDefinition.FormulaFields("fmlDiaSemana").Text = "'" & Format(datPDP, "dd/MM/yyyy") & " " & DiaSemana(datPDP.DayOfWeek()) & "'"
        BindReport("GeracaoCargaCOPEL")
        Dim objDSGerCop As DataSet = CType(Session("report_ds"), DataSet)
        objSubRepGerCop.SetDataSource(objDSGerCop)

        'Geração CEEE - SUL
        objSubRepGerCee = objReport.OpenSubreport("GERCEEE")
        objSubRepGerCee.DataDefinition.FormulaFields.Reset()
        objSubRepGerCee.DataDefinition.FormulaFields("fmlTitulo").Text = "'PROG. DIÁRIA - GERAÇÃO DA CEEE E CARGA RGS (MW)'"
        objSubRepGerCee.DataDefinition.FormulaFields("fmlSubTitulo").Text = "'CENTRO REGIONAL SUL'"
        objSubRepGerCee.DataDefinition.FormulaFields("fmlDiaSemana").Text = "'" & Format(datPDP, "dd/MM/yyyy") & " " & DiaSemana(datPDP.DayOfWeek()) & "'"
        BindReport("GeracaoCargaS")
        Dim objDSGerCee As DataSet = CType(Session("report_ds"), DataSet)
        objSubRepGerCee.SetDataSource(objDSGerCee)

        'Inflexibilidade - SUL
        objSubRepInflex = objReport.OpenSubreport("INFLEX")
        objSubRepInflex.DataDefinition.FormulaFields.Reset()
        objSubRepInflex.DataDefinition.FormulaFields("fmlTitulo").Text = "'PROG. DIÁRIA - INFLEXIBILIDADES POR USINA (MW)'"
        objSubRepInflex.DataDefinition.FormulaFields("fmlSubTitulo").Text = "'CENTRO REGIONAL SUL'"
        objSubRepInflex.DataDefinition.FormulaFields("fmlDiaSemana").Text = "'" & Format(datPDP, "dd/MM/yyyy") & " " & DiaSemana(datPDP.DayOfWeek()) & "'"
        BindReport("InflexUsina")
        Dim objDSInflex As DataSet = CType(Session("report_ds"), DataSet)
        objSubRepInflex.SetDataSource(objDSInflex)

        'Usina por Empresa - SUL
        objSubRepUsiEmpre = objReport.OpenSubreport("USIPOREMPRE")
        objSubRepUsiEmpre.DataDefinition.FormulaFields.Reset()
        objSubRepUsiEmpre.DataDefinition.FormulaFields("fmlTitulo").Text = "'RELAÇÃO DE USINAS POR EMPRESA'"
        objSubRepUsiEmpre.DataDefinition.FormulaFields("fmlSubTitulo").Text = "'CENTRO REGIONAL SUL'"
        objSubRepUsiEmpre.DataDefinition.FormulaFields("fmlDiaSemana").Text = "'" & Format(datPDP, "dd/MM/yyyy") & " " & DiaSemana(datPDP.DayOfWeek()) & "'"
        BindReport("UsiPorEmpre")
        Dim objDSUsiPorEmpre As DataSet = CType(Session("report_ds"), DataSet)
        objSubRepUsiEmpre.SetDataSource(objDSUsiPorEmpre)


        Exporta(objReport, True, cboTipo.SelectedItem.Text)
        objReport.Close()
    End Sub

    Private Sub MostraRelatorioSubNE()
        Dim objSubRepHidro As ReportDocument
        Dim objSubRepTermo As ReportDocument
        Dim objSubRepParHid As ReportDocument
        Dim objSubRepParTer As ReportDocument
        Dim objSubRepReqArea As ReportDocument
        Dim objSubRepReqGer As ReportDocument

        strNomeRel = Request.QueryString("strRelatorio")
        ViewState("nomeRel") = strNomeRel & "_" & Format(Now, "yyMMdd_HHmmss")

        objReport = New ReportDocument
        objSubRepHidro = New ReportDocument
        objSubRepTermo = New ReportDocument
        objSubRepParHid = New ReportDocument
        'objSubRepParTer = New ReportDocument
        objSubRepReqArea = New ReportDocument
        objSubRepReqGer = New ReportDocument

        objReport.Load(Server.MapPath("Relatorios\" + strNomeRel + ".rpt"), OpenReportMethod.OpenReportByDefault)

        Dim datPDP As Date = CType(Request.QueryString("strData").Substring(6, 2) & "/" &
                                Request.QueryString("strData").Substring(4, 2) & "/" &
                                Request.QueryString("strData").Substring(0, 4), Date)

        'Requisitos Geração - Nordeste
        'Seleciona os valores de Requisito e Reserva
        Dim objCon As New OnsClasses.OnsData.OnsConnection
        Dim objCmd As New OnsClasses.OnsData.OnsCommand
        objCon.Open("rpdp")
        objCmd.Connection = objCon
        objCmd.CommandText = "SELECT valreqmax, hreqmax, valresreqmax, hresreqmax " &
                            "FROM requisitos_area " &
                            "WHERE datpdp = '" & Page.Request.QueryString("strData") & "' " &
                            "AND codarea = 'NE'"
        Dim dtrRequisito As OnsClasses.OnsData.OnsDataReader = objCmd.ExecuteReader
        intRequisito = 0
        strRequisito = ""
        intReserva = 0
        strReserva = ""
        If dtrRequisito.Read Then
            intRequisito = dtrRequisito("valreqmax")
            strRequisito = dtrRequisito("hreqmax")
            intReserva = dtrRequisito("valresreqmax")
            strReserva = dtrRequisito("hresreqmax")
        End If
        dtrRequisito.Close()
        dtrRequisito = Nothing
        objCon.Close()

        objSubRepReqGer = objReport.OpenSubreport("REQGER")
        objSubRepReqGer.DataDefinition.FormulaFields.Reset()
        objSubRepReqGer.DataDefinition.FormulaFields("fmlVlrRequisito").Text = "'" & intRequisito.ToString.Trim & "'"
        objSubRepReqGer.DataDefinition.FormulaFields("fmlHorRequisito").Text = "'" & strRequisito.Trim & "'"
        objSubRepReqGer.DataDefinition.FormulaFields("fmlVlrReserva").Text = "'" & intReserva.ToString.Trim & "'"
        objSubRepReqGer.DataDefinition.FormulaFields("fmlHorReserva").Text = "'" & strReserva.Trim & "'"
        objSubRepReqGer.DataDefinition.FormulaFields("fmlTitulo").Text = "'REQUISITO / GERAÇÃO'"
        objSubRepReqGer.DataDefinition.FormulaFields("fmlSubTitulo").Text = "'CENTRO REGIONAL NORDESTE'"
        objSubRepReqGer.DataDefinition.FormulaFields("fmlDiaSemana").Text = "'" & Format(datPDP, "dd/MM/yyyy") & " " & DiaSemana(datPDP.DayOfWeek()) & "'"
        BindReport("ReqGerNE")
        Dim objDSReqGer As DataSet = CType(Session("report_ds"), DataSet)
        objSubRepReqGer.SetDataSource(objDSReqGer)

        'Geração Hidráulica - Nordeste
        objSubRepHidro = objReport.OpenSubreport("HIDRO")
        objSubRepHidro.DataDefinition.FormulaFields.Reset()
        objSubRepHidro.DataDefinition.FormulaFields("fmlTitulo").Text = "'GERAÇÃO HORÁRIA DAS USINAS HIDRÁULICAS'"
        objSubRepHidro.DataDefinition.FormulaFields("fmlSubTitulo").Text = "'CENTRO REGIONAL NORDESTE'"
        objSubRepHidro.DataDefinition.FormulaFields("fmlDiaSemana").Text = "'" & Format(datPDP, "dd/MM/yyyy") & " " & DiaSemana(datPDP.DayOfWeek()) & "'"
        BindReport("GerHidroNE")
        Dim objDSHidro As DataSet = CType(Session("report_ds"), DataSet)
        objSubRepHidro.SetDataSource(objDSHidro)
        'objSubRepHidro.Refresh()

        'Geração Térmica - Nordeste
        objSubRepTermo = objReport.OpenSubreport("TERMO")
        objSubRepTermo.DataDefinition.FormulaFields.Reset()
        objSubRepTermo.DataDefinition.FormulaFields("fmlTitulo").Text = "'GERAÇÃO HORÁRIA DAS USINAS TÉRMICAS E EÓLICAS'"
        objSubRepTermo.DataDefinition.FormulaFields("fmlSubTitulo").Text = "'CENTRO REGIONAL NORDESTE'"
        objSubRepTermo.DataDefinition.FormulaFields("fmlDiaSemana").Text = "'" & Format(datPDP, "dd/MM/yyyy") & " " & DiaSemana(datPDP.DayOfWeek()) & "'"
        BindReport("GerTermoNE")
        Dim objDSTermo As DataSet = CType(Session("report_ds"), DataSet)
        objSubRepTermo.SetDataSource(objDSTermo)

        'Parada de Máquinas Hidráulicas - Nordeste
        objSubRepParHid = objReport.OpenSubreport("PARHIDRO")
        objSubRepParHid.DataDefinition.FormulaFields.Reset()
        objSubRepParHid.DataDefinition.FormulaFields("fmlTitulo").Text = "'CRONOG PARADA DE MÁQUINAS HIDRÁULICAS E TÉRMICAS'"
        objSubRepParHid.DataDefinition.FormulaFields("fmlSubTitulo").Text = "'CENTRO REGIONAL NORDESTE'"
        objSubRepParHid.DataDefinition.FormulaFields("fmlDiaSemana").Text = "'" & Format(datPDP, "dd/MM/yyyy") & " " & DiaSemana(datPDP.DayOfWeek()) & "'"
        BindReport("ParadaNE_H")
        Dim objDSParHid As DataSet = CType(Session("report_ds"), DataSet)
        objSubRepParHid.SetDataSource(objDSParHid)

        ''Parada de Máquinas Térmicas e Eólicas - Nordeste
        'objSubRepParTer = objReport.OpenSubreport("PARTERMO")
        'objSubRepParTer.DataDefinition.FormulaFields.Reset()
        'objSubRepParTer.DataDefinition.FormulaFields("fmlTitulo").Text = "'CRONOGRAMA DE PARADA DE MÁQUINAS TÉRMICAS E EÓLICAS'"
        'objSubRepParTer.DataDefinition.FormulaFields("fmlSubTitulo").Text = "'CENTRO REGIONAL NORDESTE'"
        'objSubRepParTer.DataDefinition.FormulaFields("fmlDiaSemana").Text = "'" & Format(datPDP, "dd/MM/yyyy") & " " & DiaSemana(datPDP.DayOfWeek()) & "'"
        'BindReport("ParadaNE_T")
        'Dim objDSParTer As DataSet = CType(Session("report_ds"), DataSet)
        'objSubRepParTer.SetDataSource(objDSParTer)

        'Requisitos por Áreas - Nordeste
        objSubRepReqArea = objReport.OpenSubreport("REQAREA")
        objSubRepReqArea.DataDefinition.FormulaFields.Reset()
        objSubRepReqArea.DataDefinition.FormulaFields("fmlTitulo").Text = "'REQUISITOS POR ÁREAS'"
        objSubRepReqArea.DataDefinition.FormulaFields("fmlSubTitulo").Text = "'CENTRO REGIONAL NORDESTE'"
        objSubRepReqArea.DataDefinition.FormulaFields("fmlDiaSemana").Text = "'" & Format(datPDP, "dd/MM/yyyy") & " " & DiaSemana(datPDP.DayOfWeek()) & "'"
        BindReport("CargaNE")
        Dim objDSReqArea As DataSet = CType(Session("report_ds"), DataSet)
        objSubRepReqArea.SetDataSource(objDSReqArea)

        Exporta(objReport, True, cboTipo.SelectedItem.Text)
        objReport.Close()
    End Sub

    Private Sub MostraRelatorioSubNCO()
        Dim objSubRepHidro As ReportDocument
        Dim objSubRepTermo As ReportDocument
        Dim objSubRepCarga As ReportDocument
        Dim objSubRepInter As ReportDocument

        strNomeRel = Request.QueryString("strRelatorio")
        ViewState("nomeRel") = strNomeRel & "_" & Format(Now, "yyMMdd_HHmmss")

        objReport = New ReportDocument
        objSubRepHidro = New ReportDocument
        objSubRepTermo = New ReportDocument
        objSubRepCarga = New ReportDocument
        objSubRepInter = New ReportDocument

        objReport.Load(Server.MapPath("Relatorios\" + strNomeRel + ".rpt"), OpenReportMethod.OpenReportByDefault)

        Dim datPDP As Date = CType(Request.QueryString("strData").Substring(6, 2) & "/" &
                                Request.QueryString("strData").Substring(4, 2) & "/" &
                                Request.QueryString("strData").Substring(0, 4), Date)

        'Requisitos Geração - Nordeste
        'Seleciona os valores de Requisito e Reserva

        'Geração Hidráulica - Norte/Centro-Oeste
        objSubRepHidro = objReport.OpenSubreport("HIDRO")
        objSubRepHidro.DataDefinition.FormulaFields.Reset()
        objSubRepHidro.DataDefinition.FormulaFields("fmlTitulo").Text = "'GERAÇÃO HORÁRIA DAS USINAS HIDRÁULICAS'"
        objSubRepHidro.DataDefinition.FormulaFields("fmlSubTitulo").Text = "'CENTRO REGIONAL NORTE/CENTRO-OESTE'"
        objSubRepHidro.DataDefinition.FormulaFields("fmlDiaSemana").Text = "'" & Format(datPDP, "dd/MM/yyyy") & " " & DiaSemana(datPDP.DayOfWeek()) & "'"
        BindReport("GerHidroNCO")
        Dim objDSHidro As DataSet = CType(Session("report_ds"), DataSet)
        objSubRepHidro.SetDataSource(objDSHidro)
        'objSubRepHidro.Refresh()

        'Geração Térmica - Norte/Centro-Oeste
        objSubRepTermo = objReport.OpenSubreport("TERMO")
        objSubRepTermo.DataDefinition.FormulaFields.Reset()
        objSubRepTermo.DataDefinition.FormulaFields("fmlTitulo").Text = "'GERAÇÃO HORÁRIA DAS USINAS TÉRMICAS'"
        objSubRepTermo.DataDefinition.FormulaFields("fmlSubTitulo").Text = "'CENTRO REGIONAL NORTE/CENTRO-OESTE'"
        objSubRepTermo.DataDefinition.FormulaFields("fmlDiaSemana").Text = "'" & Format(datPDP, "dd/MM/yyyy") & " " & DiaSemana(datPDP.DayOfWeek()) & "'"
        BindReport("GerTermoNCO")
        Dim objDSTermo As DataSet = CType(Session("report_ds"), DataSet)
        objSubRepTermo.SetDataSource(objDSTermo)

        'Previsão de Carga - Norte/Centro-Oeste
        objSubRepCarga = objReport.OpenSubreport("CARGA")
        objSubRepCarga.DataDefinition.FormulaFields.Reset()
        objSubRepCarga.DataDefinition.FormulaFields("fmlTitulo").Text = "'PREVISÃO DE CARGA E INTERCÂMBIOS'"
        objSubRepCarga.DataDefinition.FormulaFields("fmlSubTitulo").Text = "'CENTRO REGIONAL NORTE/CENTRO-OESTE'"
        objSubRepCarga.DataDefinition.FormulaFields("fmlDiaSemana").Text = "'" & Format(datPDP, "dd/MM/yyyy") & " " & DiaSemana(datPDP.DayOfWeek()) & "'"
        BindReport("CargaNCO")
        Dim objDSCarga As DataSet = CType(Session("report_ds"), DataSet)
        objSubRepCarga.SetDataSource(objDSCarga)

        'Intercâmbio - Norte/Centro-Oeste
        '###### ATENÇÃO ###### - NÃO ESTÁ SENDO IMPRESSO (SECTION ESTA COM SUPRESS)
        'objSubRepInter = objReport.OpenSubreport("INTER")
        'objSubRepInter.DataDefinition.FormulaFields.Reset()
        'objSubRepInter.DataDefinition.FormulaFields("fmlTitulo").Text = "'INTERCÂMBIOS'"
        'objSubRepInter.DataDefinition.FormulaFields("fmlSubTitulo").Text = "'CENTRO REGIONAL NORTE/CENTRO-OESTE'"
        'objSubRepInter.DataDefinition.FormulaFields("fmlDiaSemana").Text = "'" & Format(datPDP, "dd/MM/yyyy") & " " & DiaSemana(datPDP.DayOfWeek()) & "'"
        'BindReport("InterNCO")
        'Dim objDSInter As DataSet = CType(Session("report_ds"), DataSet)
        'objSubRepInter.SetDataSource(objDSInter)

        Exporta(objReport, True, cboTipo.SelectedItem.Text)
        objReport.Close()

    End Sub

    Private Sub MostraRelatorioSubEC()
        Dim ObjSubRepSIN As ReportDocument
        Dim ObjSubRepObs As ReportDocument

        strNomeRel = "relTodosDeflux" '--Request.QueryString("strRelatorio")
        ViewState("nomeRel") = strNomeRel & "_" & Format(Now, "yyMMdd_HHmmss")

        objReport = New ReportDocument
        ObjSubRepSIN = New ReportDocument
        ObjSubRepObs = New ReportDocument

        objReport.Load(Server.MapPath("Relatorios\" + strNomeRel + ".rpt"), OpenReportMethod.OpenReportByDefault)


        Dim datPDP As Date = CType(Request.QueryString("strData").Substring(6, 2) & "/" &
                                Request.QueryString("strData").Substring(4, 2) & "/" &
                                Request.QueryString("strData").Substring(0, 4), Date)


        'Programação Diaria Defluxo Observacao
        ObjSubRepObs = objReport.OpenSubreport("relPDDefluxObs.rpt")
        ObjSubRepObs.DataDefinition.FormulaFields.Reset()
        ObjSubRepObs.DataDefinition.FormulaFields("fmlTitulo").Text = "'PROGRAMA DIÁRIO DE DEFLUÊNCIAS DAS BACIAS DO SIN - PDF'"
        ObjSubRepObs.DataDefinition.FormulaFields("fmlSubTitulo").Text = "'ESCRITÓRIO CENTRAL'"
        ObjSubRepObs.DataDefinition.FormulaFields("fmlDiaSemana").Text = "'" & Format(datPDP, "dd/MM/yyyy") & " " & DiaSemana(datPDP.DayOfWeek()) & "'"
        BindReport("PDDefluxObs")
        Dim objDSDefluxObs As DataSet = CType(Session("report_ds"), DataSet)
        ObjSubRepObs.SetDataSource(objDSDefluxObs)

        If Session("report_count") > 0 Then
            objReport.DataDefinition.FormulaFields("fmlObs").Text = "'PROGRAMA DIÁRIO DE DEFLUÊNCIAS DAS BACIAS DO SIN - PDF'"
        Else
            objReport.DataDefinition.FormulaFields("fmlObs").Text = ""
        End If

        'Programação Diaria Defluxo
        ObjSubRepSIN = objReport.OpenSubreport("relPDDefluxSIN.rpt")
        ObjSubRepSIN.DataDefinition.FormulaFields.Reset()
        ObjSubRepSIN.DataDefinition.FormulaFields("fmlTitulo").Text = "'PROGRAMA DIÁRIO DE DEFLUÊNCIAS DAS BACIAS DO SIN - PDF'"
        ObjSubRepSIN.DataDefinition.FormulaFields("fmlSubTitulo").Text = "'ESCRITÓRIO CENTRAL'"
        ObjSubRepSIN.DataDefinition.FormulaFields("fmlDiaSemana").Text = "'" & Format(datPDP, "dd/MM/yyyy") & " " & DiaSemana(datPDP.DayOfWeek()) & "'"
        BindReport("PDDefluxSIN")
        Dim objDSDefluxSIN As DataSet = CType(Session("report_ds"), DataSet)
        ObjSubRepSIN.SetDataSource(objDSDefluxSIN)

        Exporta(objReport, True, cboTipo.SelectedItem.Text)
        objReport.Close()

    End Sub

    Public Overloads Sub Exporta(ByVal pReport As ReportDocument, ByVal pView As Boolean, ByVal pTipo As String)
        Dim strFileName As String
        Dim objExportOptions As ExportOptions
        Dim objDestinationOptions As DiskFileDestinationOptions
        objDestinationOptions = New DiskFileDestinationOptions
        Dim strExt As String
        Dim strWindow As String
        Dim strWinParam As String

        'Troca a extensão do arquivo para o escolhido pelo usuário
        If Not IsPostBack Then
            pTipo = "PDF"
            strExt = ".PDF"
            strWindow = "viewframe"
            strWinParam = ""
        Else
            If pTipo = "Word" Then
                strExt = ".DOC"
                strWindow = "newWindow"
            ElseIf pTipo = "RTF" Then
                strExt = ".RTF"
                strWindow = "newWindow"
            ElseIf pTipo = "Excel" Then
                strExt = ".XLS"
                strWindow = "newWindow"
            ElseIf pTipo <> "CSV" Then
                Msg(Me, "Formato de arquivo inválido.")
                Exit Sub
            End If
            strWinParam = "height=530,width=780,toolbar=yes,location=yes,directories=yes,status=yes,menubar=yes,scrollbars=yes,resizable=yes"

            '''strWindow = "viewframe"
            '''strWinParam = ""

        End If

        '''If Session("intContador") = Nothing Then
        '''    If pTipo <> "PDF" Then
        '''        Session("intContador") = 1
        '''    End If
        '''Else
        '''    Session("intContador") += 1
        '''End If

        Dim urlPath As String = Request.ApplicationPath
        Dim path As String = Request.PhysicalApplicationPath()

        '''Session("path") = path & "pdpw\Temp\" & Session.SessionID.ToString() & IIf(Session("intContador") = Nothing, "", Session("intContador")) & "\"
        Session("path") = path & "Temp\" & Session.SessionID.ToString() & "\"

        If pTipo = "CSV" Then

            '-- CRQ5000 - 17/09/2013
            'Exporta(objReport, True, cboTipo.SelectedItem.Text)
            strFileName = Session("path") & ViewState("nomeRel") & ".CSV"
            urlPath += "/Temp/" & Session.SessionID.ToString() & "/" & ViewState("nomeRel") + ".CSV"
            'ExportaCSV(viewstate("nomeRel") & ".CSV")
            'ExportaCSV(strFileName)

            Dim dataSetCSV As dtsRelatorio = Session("report_ds")

            DataTable2CSV(dataSetCSV.PDDefluxSIN, dataSetCSV.PDDefluxObs, strFileName, ";")

            Redireciona(urlPath, Me, strWindow, strWinParam)

        Else

            CreateDirectory(Session("path"))
            strFileName = Session("path") & ViewState("nomeRel") & strExt

            'Propriedades relacionadas a gravação do arquivo em disco
            objDestinationOptions.DiskFileName = strFileName

            'Objeto que armazena as configurações de exportação
            objExportOptions = pReport.ExportOptions

            With objExportOptions
                .DestinationOptions = objDestinationOptions
                .ExportDestinationType = ExportDestinationType.DiskFile
                If strExt = ".DOC" Then
                    .ExportFormatType = ExportFormatType.WordForWindows
                ElseIf strExt = ".PDF" Then
                    .ExportFormatType = ExportFormatType.PortableDocFormat
                ElseIf strExt = ".RTF" Then
                    .ExportFormatType = ExportFormatType.RichText
                ElseIf strExt = ".XLS" Then
                    .ExportFormatType = ExportFormatType.Excel
                ElseIf strExt = ".TXT" Then
                    .ExportFormatType = ExportFormatType.NoFormat
                End If
            End With

            Try
                pReport.Export()
                'pReport.SaveAs(strFileName, ReportFileFormat.VSNetFileFormat)

            Catch ex As Exception
                Throw ex
            End Try


            urlPath += "/Temp/" & Session.SessionID.ToString() & "/" & ViewState("nomeRel") + strExt
            If pView Then
                Redireciona(urlPath, Me, strWindow, strWinParam)
            End If

        End If

    End Sub

    Private Function ConverteAcentuacao(ByVal p_string As String) As String
        Dim strRetorno As String

        strRetorno = p_string
        strRetorno = strRetorno.Replace("Ç", "C")
        strRetorno = strRetorno.Replace("ç", "c")
        strRetorno = strRetorno.Replace("Á", "A")
        strRetorno = strRetorno.Replace("á", "a")
        strRetorno = strRetorno.Replace("À", "A")
        strRetorno = strRetorno.Replace("à", "a")
        strRetorno = strRetorno.Replace("Ã", "A")
        strRetorno = strRetorno.Replace("ã", "a")
        strRetorno = strRetorno.Replace("Â", "A")
        strRetorno = strRetorno.Replace("â", "a")
        strRetorno = strRetorno.Replace("É", "E")
        strRetorno = strRetorno.Replace("é", "e")
        strRetorno = strRetorno.Replace("È", "E")
        strRetorno = strRetorno.Replace("è", "e")
        strRetorno = strRetorno.Replace("Ê", "E")
        strRetorno = strRetorno.Replace("ê", "e")
        strRetorno = strRetorno.Replace("Í", "I")
        strRetorno = strRetorno.Replace("í", "i")
        strRetorno = strRetorno.Replace("Ì", "I")
        strRetorno = strRetorno.Replace("ì", "i")
        strRetorno = strRetorno.Replace("Î", "I")
        strRetorno = strRetorno.Replace("î", "i")
        strRetorno = strRetorno.Replace("Ó", "O")
        strRetorno = strRetorno.Replace("ó", "o")
        strRetorno = strRetorno.Replace("Ò", "O")
        strRetorno = strRetorno.Replace("ò", "o")
        strRetorno = strRetorno.Replace("Ô", "O")
        strRetorno = strRetorno.Replace("ô", "o")
        strRetorno = strRetorno.Replace("ú", "u")
        strRetorno = strRetorno.Replace("Ú", "U")
        strRetorno = strRetorno.Replace("ù", "u")
        strRetorno = strRetorno.Replace("Û", "U")
        strRetorno = strRetorno.Replace("û", "u")
        strRetorno = strRetorno.Replace("?", "")

        Return strRetorno.Trim()


    End Function
    Private Sub DataTable2CSV(ByVal table As DataTable, ByVal tableAux As DataTable, ByVal filename As String)
        DataTable2CSV(table, tableAux, filename, vbTab)
    End Sub

    Private Sub DataTable2CSV(ByVal table As DataTable, ByVal tableAux As DataTable, ByVal filename As String,
        ByVal sepChar As String)
        Dim writer As System.IO.StreamWriter
        Try
            writer = New System.IO.StreamWriter(filename)

            ' first write a line with the columns name
            Dim sep As String = ""
            Dim builder As New System.Text.StringBuilder
            'For Each col As DataColumn In table.Columns
            '    builder.Append(sep).Append(col.ColumnName)
            '    sep = sepChar
            'Next
            'writer.WriteLine(builder.ToString())

            ' Nome colunas
            writer.WriteLine("Bacia;Usina;Situacao de Operacao;Previsao de Afluencia;Vazao Turbinada;Vazao Vertida;Outras Estrut.;Total;Nivel Inicial (0h00);Nivel Final(23h59);Percentual;Volume de Espera;Observacao")

            ' then write all the rows

            Dim total As Decimal = 0

            For Each row As DataRow In table.Rows
                sep = ""
                builder = New System.Text.StringBuilder

                For Each col As DataColumn In table.Columns

                    If (col.DataType.FullName = "System.String") Then

                        'If (col.ColumnName = "observacao") Then
                        '    Dim dtr_Aux As DataRow()
                        '    Dim observacaoAux As String = ""

                        '    dtr_Aux = tableAux.Select("usina = '" + row("usina") + "'")
                        '    For y As Integer = 0 To dtr_Aux.Length - 1
                        '        observacaoAux = dtr_Aux(y)("observacao")
                        '        Exit For
                        '    Next
                        '    builder.Append(sep).Append(ConverteAcentuacao(observacaoAux))
                        'Else
                        If (row(col.ColumnName).ToString() = "") Then
                            builder.Append(sep).Append(row(col.ColumnName))
                        Else
                            builder.Append(sep).Append(ConverteAcentuacao(row(col.ColumnName)))
                        End If
                        'End If

                    Else
                        If (col.ColumnName = "turbinada" Or col.ColumnName = "vertida" Or col.ColumnName = "outrasestr") Then

                            Try
                                If (col.ColumnName = "turbinada") Then
                                    total = Decimal.Parse(row(col.ColumnName))
                                ElseIf (col.ColumnName = "vertida") Then
                                    total = total + Decimal.Parse(row(col.ColumnName))
                                ElseIf (col.ColumnName = "outrasestr") Then
                                    total = total + Decimal.Parse(row(col.ColumnName))
                                End If

                            Catch
                                total = -1
                            End Try

                        End If

                        builder.Append(sep).Append(row(col.ColumnName))

                        ' quando foi coluna outrasestr, escreve tb o Total
                        If (col.ColumnName = "outrasestr") Then

                            If (total = -1) Then
                                builder.Append(sep).Append("")
                            Else
                                builder.Append(sep).Append(total)

                            End If

                        End If

                    End If

                    sep = sepChar


                Next
                writer.WriteLine(builder.ToString())
            Next
        Finally
            If Not writer Is Nothing Then writer.Close()
        End Try
    End Sub

    Private Sub btnVoltar_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnVoltar.Click
        Session("report_ds") = Nothing
        Try
            'Elimina os arquivos da pasta
            Kill(Session("path") & "*.*")
            'Elimina a pasta temporária
            Delete(Session("path"))
        Catch ex As Exception
            Session("path") = Nothing
        End Try
        Redireciona("frmRelatorio.aspx?strRegional=" & Request.QueryString("strRegional"), Me, "_parent")
    End Sub

    Public Sub Msg(ByVal pWebform As Page, ByVal pStr As String)
        Dim strMsg As New StringBuilder

        strMsg.Append("<Script Language=""JavaScript"">" & NewLine)
        strMsg.Append("alert('" & pStr & "');")
        strMsg.Append("</Script>")

        pWebform.RegisterClientScriptBlock("foco", strMsg.ToString)
    End Sub

    Public Overloads Sub Redireciona(ByVal pURL As String, ByVal pWebForm As Object, ByVal pTarget As String, Optional ByVal pFeatures As String = "")
        Dim objScript As New StringBuilder
        objScript.Append("<Script Language=""JavaScript"">" & NewLine)
        If pTarget <> "" Then
            If pFeatures <> "" Then
                objScript.Append("window.open('" & pURL & "','" & pTarget & "','" & pFeatures & "').focus();")
            Else
                objScript.Append("window.open('" & pURL & "','" & pTarget & "').focus();")
            End If
        Else
            objScript.Append("document.location.href('" & pURL & "');")
        End If
        objScript.Append("</Script>")
        pWebForm.Page.RegisterClientScriptBlock("1", objScript.ToString)
    End Sub

    Private Sub btnExportar_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnExportar.Click
        If Request.QueryString("strTipoGer") = "TodosS" Then
            Call MostraRelatorioSubS()
        ElseIf Request.QueryString("strTipoGer") = "TodosNE" Then
            Call MostraRelatorioSubNE()
        ElseIf Request.QueryString("strTipoGer") = "TodosNCO" Then
            Call MostraRelatorioSubNCO()
            '-- 2013-08-01
        ElseIf Request.QueryString("strTipoGer") = "TodosDeflux" Then
            Call MostraRelatorioSubEC()
        Else
            Call MostraRelatorio()
        End If
        ViewState("Tipo") = cboTipo.SelectedItem.Text
    End Sub

    Private Sub MostraArquivoTexto()
        Dim intI, intJ, intArq As Integer
        Dim dtsDataset As DataSet
        Dim strUrlPath As String = Request.ApplicationPath
        Dim strPath As String = Request.PhysicalApplicationPath()
        Dim strFileName, strFullPathFileName As String
        Dim strWindow As String
        Dim strWinParam As String
        Dim strLinha, strLinha2, strLinha3, strCodUsina As String
        Dim strTab As String = Chr(9) & Space(1)

        'monta o caminho onde sera gravado
        strPath &= "Temp\" & Session.SessionID + "\"
        System.IO.Directory.CreateDirectory(strPath)

        If Request.QueryString("strTipoGer") = "TodosSE" Then
            strFileName = "RelatorioSE_" & Format(Now, "yyMMdd_HHmmss") & ".txt"
        ElseIf Request.QueryString("strTipoGer") = "TodosNEtxt" Then
            strFileName = "RelatorioNE_" & Format(Now, "yyMMdd_HHmmss") & ".txt"
        End If

        strFullPathFileName = strPath & strFileName

        'Se já existir um arquivo excluo e crio outro novo.
        If System.IO.File.Exists(strFullPathFileName) Then
            System.IO.File.Delete(strFullPathFileName)
        End If

        'Abre o arquivo texto
        intArq = FreeFile()
        FileOpen(intArq, strFullPathFileName, OpenMode.Output, OpenShare.Default)
        Dim intCarga As Int16 = 0

        If Request.QueryString("strTipoGer") = "TodosSE" Then

            'PREVISÃO DE CARGA - SE
            'Tabela Carga
            BindReport("CargaSE")
            dtsDataset = Session("report_ds")
            PrintLine(intArq, "PREVISAO DE CARGA - SE (" & Request.QueryString("strdata").Substring(6, 2) & "/" & Request.QueryString("strdata").Substring(4, 2) & "/" & Request.QueryString("strdata").Substring(0, 4) & ")")
            strLinha = "Intervalo"
            For intI = 0 To dtsDataset.Tables("carga").Rows.Count - 1
                If dtsDataset.Tables("carga").Rows(intI).Item("intcarga") <> 1 Then
                    Exit For
                End If
                strLinha &= Chr(9) & dtsDataset.Tables("carga").Rows(intI).Item("codempre")
            Next
            PrintLine(intArq, strLinha)
            strLinha = ""
            For intI = 0 To dtsDataset.Tables("carga").Rows.Count - 1
                With dtsDataset.Tables("carga").Rows(intI)
                    If intCarga <> .Item("intcarga") Then
                        PrintLine(intArq, strLinha)
                        strLinha = Intervalo(.Item("intcarga"))
                        intCarga = .Item("intcarga")
                    End If
                    strLinha &= strTab & .Item("valcargasup").ToString
                End With
            Next
            PrintLine(intArq, strLinha)
            PrintLine(intArq, "")

            'GERAÇÃO POR USINA - SE
            'Tabela despa
            BindReport("GeracaoSE")
            dtsDataset = Session("report_ds")
            PrintLine(intArq, "GERACAO - SE (" & Request.QueryString("strdata").Substring(6, 2) & "/" & Request.QueryString("strdata").Substring(4, 2) & "/" & Request.QueryString("strdata").Substring(0, 4) & ")")
            strLinha = "Intervalo"
            For intI = 0 To dtsDataset.Tables("despa").Rows.Count - 1
                If dtsDataset.Tables("despa").Rows(intI).Item("intdespa") <> 1 Then
                    Exit For
                End If
                strLinha &= Chr(9) & dtsDataset.Tables("despa").Rows(intI).Item("codigo")
            Next
            PrintLine(intArq, strLinha)
            strLinha = ""
            For intI = 0 To dtsDataset.Tables("despa").Rows.Count - 1
                With dtsDataset.Tables("despa").Rows(intI)
                    If intCarga <> .Item("intdespa") Then
                        PrintLine(intArq, strLinha)
                        strLinha = Intervalo(.Item("intdespa"))
                        intCarga = .Item("intdespa")
                    End If
                    strLinha &= strTab & .Item("valdespaemp").ToString
                End With
            Next
            PrintLine(intArq, strLinha)
            PrintLine(intArq, "")

            'VAZÃO - SE
            'Tabela vazao
            BindReport("VazaoSE")
            dtsDataset = Session("report_ds")
            PrintLine(intArq, "VAZAO - SE (" & Request.QueryString("strdata").Substring(6, 2) & "/" & Request.QueryString("strdata").Substring(4, 2) & "/" & Request.QueryString("strdata").Substring(0, 4) & ")")
            PrintLine(intArq, "Empresa" & Chr(9) & "Usina" & Chr(9) & "Turb" & Chr(9) & "Vert")
            PrintLine(intArq, "")
            For intI = 0 To dtsDataset.Tables("vazao").Rows.Count - 1
                With dtsDataset.Tables("vazao").Rows(intI)
                    If .Item("valturb") <> 0 Or .Item("valvert") <> 0 Then
                        PrintLine(intArq, .Item("sigempre").ToString.Trim & Chr(9) & .Item("sigusina").ToString.Trim & strTab & .Item("valturb") & strTab & .Item("valvert"))
                    End If
                End With
            Next
            PrintLine(intArq, "")

            'INTERCÂMBIO LÍQUIDO - SE
            'Tabela despa
            BindReport("InterSE")
            dtsDataset = Session("report_ds")
            PrintLine(intArq, "INTERCAMBIO LIQUIDO - SE (" & Request.QueryString("strdata").Substring(6, 2) & "/" & Request.QueryString("strdata").Substring(4, 2) & "/" & Request.QueryString("strdata").Substring(0, 4) & ")")
            strLinha = "Intervalo"
            For intI = 0 To dtsDataset.Tables("intercambio").Rows.Count - 1
                If dtsDataset.Tables("intercambio").Rows(intI).Item("intinter") <> 1 Then
                    Exit For
                End If
                strLinha &= Chr(9) & dtsDataset.Tables("intercambio").Rows(intI).Item("codinter")
            Next
            PrintLine(intArq, strLinha)
            strLinha = ""
            For intI = 0 To dtsDataset.Tables("intercambio").Rows.Count - 1
                With dtsDataset.Tables("intercambio").Rows(intI)
                    If intCarga <> .Item("intinter") Then
                        PrintLine(intArq, strLinha)
                        strLinha = Intervalo(.Item("intinter"))
                        intCarga = .Item("intinter")
                    End If
                    strLinha &= strTab & .Item("valintersup").ToString
                End With
            Next
            PrintLine(intArq, strLinha)
            PrintLine(intArq, "")

            'TRANSFERÊNCIA DE ENERGIA - SE
            'Tabela despa
            BindReport("TransfSE")
            dtsDataset = Session("report_ds")
            PrintLine(intArq, "TRANSFERENCIA DE ENERGIA - SE (" & Request.QueryString("strdata").Substring(6, 2) & "/" & Request.QueryString("strdata").Substring(4, 2) & "/" & Request.QueryString("strdata").Substring(0, 4) & ")")
            strLinha = "Intervalo" & Chr(9) & "SUL" & Chr(9) & "SUDESTE"
            PrintLine(intArq, strLinha)
            PrintLine(intArq, "")
            For intI = 0 To dtsDataset.Tables("transfse").Rows.Count - 1
                With dtsDataset.Tables("transfse").Rows(intI)
                    PrintLine(intArq, Intervalo(.Item("intervalo")) & strTab & .Item("vlrsul") & strTab & .Item("vlrsudeste"))
                End With
            Next
            PrintLine(intArq, "")

            'RESTRIÇÕES OPERATIVAS - SE
            'Tabela restrse
            BindReport("RestrSE")
            dtsDataset = Session("report_ds")
            PrintLine(intArq, "RESTRICOES OPERATIVAS - SE (" & Request.QueryString("strdata").Substring(6, 2) & "/" & Request.QueryString("strdata").Substring(4, 2) & "/" & Request.QueryString("strdata").Substring(0, 4) & ")")
            strLinha = "Intervalo"
            For intI = 0 To dtsDataset.Tables("restrse").Rows.Count - 1
                If dtsDataset.Tables("restrse").Rows(intI).Item("intervalo") <> 1 Then
                    Exit For
                End If
                strLinha &= Chr(9) & dtsDataset.Tables("restrse").Rows(intI).Item("usina")
            Next
            PrintLine(intArq, strLinha)
            strLinha = ""
            For intI = 0 To dtsDataset.Tables("restrse").Rows.Count - 1
                With dtsDataset.Tables("restrse").Rows(intI)
                    If intCarga <> .Item("intervalo") Then
                        PrintLine(intArq, strLinha)
                        strLinha = Intervalo(.Item("intervalo"))
                        intCarga = .Item("intervalo")
                    End If
                    strLinha &= strTab & .Item("valor").ToString
                End With
            Next
            PrintLine(intArq, strLinha)
            PrintLine(intArq, "")

            'FOLGA - SE
            'Tabela restrse
            BindReport("FolgaSE")
            dtsDataset = Session("report_ds")
            PrintLine(intArq, "FOLGA DE POTENCIA - SE (" & Request.QueryString("strdata").Substring(6, 2) & "/" & Request.QueryString("strdata").Substring(4, 2) & "/" & Request.QueryString("strdata").Substring(0, 4) & ")")
            strLinha = "Intervalo"
            For intI = 0 To dtsDataset.Tables("restrse").Rows.Count - 1
                If dtsDataset.Tables("restrse").Rows(intI).Item("intervalo") <> 1 Then
                    Exit For
                End If
                strLinha &= Chr(9) & dtsDataset.Tables("restrse").Rows(intI).Item("usina")
            Next
            PrintLine(intArq, strLinha)
            strLinha = ""
            For intI = 0 To dtsDataset.Tables("restrse").Rows.Count - 1
                With dtsDataset.Tables("restrse").Rows(intI)
                    If intCarga <> .Item("intervalo") Then
                        PrintLine(intArq, strLinha)
                        strLinha = Intervalo(.Item("intervalo"))
                        intCarga = .Item("intervalo")
                    End If
                    strLinha &= strTab & .Item("valor").ToString
                End With
            Next
            PrintLine(intArq, strLinha)
            PrintLine(intArq, "")

            'BALANCO - SE
            'Tabela balanco
            BindReport("BalancoSE")
            dtsDataset = Session("report_ds")
            PrintLine(intArq, "BALANCO DE ENERGIA - SE (" & Request.QueryString("strdata").Substring(6, 2) & "/" & Request.QueryString("strdata").Substring(4, 2) & "/" & Request.QueryString("strdata").Substring(0, 4) & ")")
            strLinha = Space(9)
            strLinha2 = "Intervalo"
            For intI = 0 To dtsDataset.Tables("balanco").Rows.Count - 1
                If dtsDataset.Tables("balanco").Rows(intI).Item("intervalo") <> 1 Then
                    Exit For
                End If
                strLinha &= Chr(9) & dtsDataset.Tables("balanco").Rows(intI).Item("sigempre").ToString.Trim
                strLinha2 &= Chr(9) & dtsDataset.Tables("balanco").Rows(intI).Item("grandeza").ToString.Trim
            Next
            PrintLine(intArq, strLinha)
            PrintLine(intArq, strLinha2)
            strLinha = ""
            For intI = 0 To dtsDataset.Tables("balanco").Rows.Count - 1
                With dtsDataset.Tables("balanco").Rows(intI)
                    If intCarga <> .Item("intervalo") Then
                        PrintLine(intArq, strLinha)
                        strLinha = Intervalo(.Item("intervalo"))
                        intCarga = .Item("intervalo")
                    End If
                    strLinha &= strTab & .Item("valor").ToString
                End With
            Next
            PrintLine(intArq, strLinha)
            PrintLine(intArq, "")

            'RESERVA DE POTENCIA - SE
            'Tabela Carga
            BindReport("ReservaSE")
            dtsDataset = Session("report_ds")
            PrintLine(intArq, "RESERVA DE POTENCIA - SE (" & Request.QueryString("strdata").Substring(6, 2) & "/" & Request.QueryString("strdata").Substring(4, 2) & "/" & Request.QueryString("strdata").Substring(0, 4) & ")")
            strLinha = "Intervalo"
            For intI = 0 To dtsDataset.Tables("carga").Rows.Count - 1
                If dtsDataset.Tables("carga").Rows(intI).Item("intcarga") <> 1 Then
                    Exit For
                End If
                strLinha &= Chr(9) & dtsDataset.Tables("carga").Rows(intI).Item("codempre")
            Next
            PrintLine(intArq, strLinha)
            strLinha = ""
            For intI = 0 To dtsDataset.Tables("carga").Rows.Count - 1
                With dtsDataset.Tables("carga").Rows(intI)
                    If intCarga <> .Item("intcarga") Then
                        PrintLine(intArq, strLinha)
                        strLinha = Intervalo(.Item("intcarga"))
                        intCarga = .Item("intcarga")
                    End If
                    strLinha &= strTab & .Item("valcargasup").ToString
                End With
            Next
            PrintLine(intArq, strLinha)
            PrintLine(intArq, "")

            'RAZÃO DO DESPACHO - SE
            'Tabela inflexusinas
            BindReport("RazaoSE")
            dtsDataset = Session("report_ds")
            PrintLine(intArq, "RAZAO DO DESPACHO - SE (" & Request.QueryString("strdata").Substring(6, 2) & "/" & Request.QueryString("strdata").Substring(4, 2) & "/" & Request.QueryString("strdata").Substring(0, 4) & ")")
            strLinha = Space(9)
            strLinha2 = "Intervalo"
            For intI = 0 To dtsDataset.Tables("inflexusinas").Rows.Count - 1
                If dtsDataset.Tables("inflexusinas").Rows(intI).Item("intervalo") <> 1 Then
                    Exit For
                End If
                strLinha &= Chr(9) & dtsDataset.Tables("inflexusinas").Rows(intI).Item("codusina").ToString.Trim
                strLinha2 &= Chr(9) & dtsDataset.Tables("inflexusinas").Rows(intI).Item("grandeza").ToString.Trim
            Next
            PrintLine(intArq, strLinha)
            PrintLine(intArq, strLinha2)
            strLinha = ""
            For intI = 0 To dtsDataset.Tables("inflexusinas").Rows.Count - 1
                With dtsDataset.Tables("inflexusinas").Rows(intI)
                    If intCarga <> .Item("intervalo") Then
                        PrintLine(intArq, strLinha)
                        strLinha = Intervalo(.Item("intervalo"))
                        intCarga = .Item("intervalo")
                    End If
                    strLinha &= strTab & .Item("valor").ToString
                End With
            Next
            PrintLine(intArq, strLinha)
            PrintLine(intArq, "")

            'CONFIGURAÇÃO DE UG - SE
            'Tabela restrse
            BindReport("ConfigSE")
            dtsDataset = Session("report_ds")
            PrintLine(intArq, "CONFIGURACAO DE UG - SE (" & Request.QueryString("strdata").Substring(6, 2) & "/" & Request.QueryString("strdata").Substring(4, 2) & "/" & Request.QueryString("strdata").Substring(0, 4) & ")")
            strLinha = "Intervalo"
            For intI = 0 To dtsDataset.Tables("restrse").Rows.Count - 1
                If dtsDataset.Tables("restrse").Rows(intI).Item("intervalo") <> 1 Then
                    Exit For
                End If
                strLinha &= Chr(9) & dtsDataset.Tables("restrse").Rows(intI).Item("usina")
            Next
            PrintLine(intArq, strLinha)
            strLinha = ""
            For intI = 0 To dtsDataset.Tables("restrse").Rows.Count - 1
                With dtsDataset.Tables("restrse").Rows(intI)
                    If intCarga <> .Item("intervalo") Then
                        PrintLine(intArq, strLinha)
                        strLinha = Intervalo(.Item("intervalo"))
                        intCarga = .Item("intervalo")
                    End If
                    strLinha &= strTab & .Item("valor").ToString
                End With
            Next
            PrintLine(intArq, strLinha)

        ElseIf Request.QueryString("strTipoGer") = "TodosNEtxt" Then

            'REQUISITOS / GERAÇÃO - NE
            'Tabela reqgerne

            'Seleciona os valores de Requisito e Reserva
            Dim objCon As New OnsClasses.OnsData.OnsConnection
            Dim objCmd As New OnsClasses.OnsData.OnsCommand

            objCon.Open("rpdp")
            objCmd.Connection = objCon
            objCmd.CommandText = "SELECT valreqmax, hreqmax, valresreqmax, hresreqmax " &
                                "FROM requisitos_area " &
                                "WHERE datpdp = '" & Page.Request.QueryString("strData") & "' " &
                                "AND codarea = 'NE'"
            Dim dtrRequisito As OnsClasses.OnsData.OnsDataReader = objCmd.ExecuteReader
            intRequisito = 0
            strRequisito = ""
            intReserva = 0
            strReserva = ""
            If dtrRequisito.Read Then
                intRequisito = dtrRequisito("valreqmax")
                strRequisito = dtrRequisito("hreqmax")
                intReserva = dtrRequisito("valresreqmax")
                strReserva = dtrRequisito("hresreqmax")
            End If
            dtrRequisito.Close()
            dtrRequisito = Nothing
            objCon.Close()

            BindReport("ReqGerNE")
            dtsDataset = Session("report_ds")
            PrintLine(intArq, "CENTRO REGIONAL NORDESTE - COSR-NE")
            PrintLine(intArq, "")

            PrintLine(intArq, "1-REQUISITO / GERAÇÃO (" & Request.QueryString("strdata").Substring(6, 2) & "/" & Request.QueryString("strdata").Substring(4, 2) & "/" & Request.QueryString("strdata").Substring(0, 4) & ")")
            strLinha = "  REQUISITO MÁXIMO: " & intRequisito.ToString.Trim & " MW / " & strRequisito
            PrintLine(intArq, strLinha)
            strLinha = "  RESERVA NO REQUISITO MÁXIMO: " & intReserva.ToString.Trim & " MW / " & strReserva
            PrintLine(intArq, strLinha)
            strLinha = Space(46) & "GERAÇÃO"
            strLinha &= Space(26) & "INTERCÂMBIO"
            strLinha &= Space(20) & "RESERVA"
            strLinha &= Space(16) & "POT SINCR"
            PrintLine(intArq, strLinha)

            strLinha = "Intervalo"
            strLinha &= Space(11) & "REQ"
            strLinha &= Space(13) & "HIDRO"
            strLinha &= Space(6) & "TERM"
            strLinha &= Space(6) & "EÓLICA"
            strLinha &= Space(12) & "NE=>N-CO"
            strLinha &= Space(4) & "LIQ NE"
            strLinha &= Space(13) & "R2 NE"
            strLinha &= Space(5) & "TOTAL"
            strLinha &= Space(16) & "NE"

            PrintLine(intArq, strLinha)
            strLinha = ""
            For intI = 0 To dtsDataset.Tables("reqgerne").Rows.Count - 1
                With dtsDataset.Tables("reqgerne").Rows(intI)
                    'If intCarga <> .Item("intcarga") Then
                    PrintLine(intArq, strLinha)
                    strLinha = Intervalo(.Item("intervalo"))
                    'intCarga = .Item("intcarga")
                    'End If
                    strLinha &= Right(Space(12) & .Item("vlrcarga").ToString, 12)
                    strLinha &= Right(Space(18) & .Item("vlrgerh").ToString, 18)
                    strLinha &= Right(Space(10) & .Item("vlrgert").ToString, 10)
                    strLinha &= Right(Space(10) & .Item("vlrgere").ToString, 10)
                    strLinha &= Right(Space(20) & .Item("vlrinternne").ToString, 20)
                    strLinha &= Right(Space(10) & .Item("vlrinternne").ToString, 10)
                    strLinha &= Right(Space(20) & .Item("vlrreservar2").ToString, 20)
                    strLinha &= Right(Space(10) & (.Item("vlrreservane") - .Item("vlrgerh")).ToString, 10)
                    strLinha &= Right(Space(15) & .Item("vlrreservane").ToString, 20)
                End With
            Next
            PrintLine(intArq, strLinha)
            PrintLine(intArq, "")
            PrintLine(intArq, "")

            'GERAÇÃO HORÁRIA DAS USINAS HIDRÁULICAS - NE
            'Tabela despa
            BindReport("GerHidroNE")
            dtsDataset = Session("report_ds")
            PrintLine(intArq, "2-GERAÇÃO HORÁRIA DAS USINAS HIDRÁULICAS (" & Request.QueryString("strdata").Substring(6, 2) & "/" & Request.QueryString("strdata").Substring(4, 2) & "/" & Request.QueryString("strdata").Substring(0, 4) & ")")
            strLinha = "Intervalo  "
            For intI = 0 To dtsDataset.Tables("despa").Rows.Count - 1
                If dtsDataset.Tables("despa").Rows(intI).Item("intdespa") <> 1 Then
                    Exit For
                End If
                strLinha &= Right(Space(15) & Trim(dtsDataset.Tables("despa").Rows(intI).Item("codempre") &
                                            dtsDataset.Tables("despa").Rows(intI).Item("codusina")), 15)
            Next
            PrintLine(intArq, strLinha)
            strLinha = ""
            For intI = 0 To dtsDataset.Tables("despa").Rows.Count - 1
                With dtsDataset.Tables("despa").Rows(intI)
                    If intCarga <> .Item("intdespa") Then
                        PrintLine(intArq, strLinha)
                        strLinha = Intervalo(.Item("intdespa"))
                        intCarga = .Item("intdespa")
                    End If
                    strLinha &= Right(Space(15) & .Item("valdespaemp").ToString, 15)
                End With
            Next
            PrintLine(intArq, strLinha)
            PrintLine(intArq, "")
            PrintLine(intArq, "")

            'GERAÇÃO HORÁRIA DAS USINAS TÉRMICAS E EÓLICAS - NE
            'Tabela despa
            BindReport("GerTermoNE")
            dtsDataset = Session("report_ds")
            PrintLine(intArq, "3-GERAÇÃO HORÁRIA DAS USINAS TÉRMICAS E EÓLICAS (" & Request.QueryString("strdata").Substring(6, 2) & "/" & Request.QueryString("strdata").Substring(4, 2) & "/" & Request.QueryString("strdata").Substring(0, 4) & ")")
            strLinha = "Intervalo  "
            For intI = 0 To dtsDataset.Tables("despa").Rows.Count - 1
                If dtsDataset.Tables("despa").Rows(intI).Item("intdespa") <> 1 Then
                    Exit For
                End If
                strLinha &= Right(Space(15) & Trim(dtsDataset.Tables("despa").Rows(intI).Item("codempre") &
                                            dtsDataset.Tables("despa").Rows(intI).Item("codusina")), 15)
            Next
            PrintLine(intArq, strLinha)
            strLinha = ""
            For intI = 0 To dtsDataset.Tables("despa").Rows.Count - 1
                With dtsDataset.Tables("despa").Rows(intI)
                    If intCarga <> .Item("intdespa") Then
                        PrintLine(intArq, strLinha)
                        strLinha = Intervalo(.Item("intdespa"))
                        intCarga = .Item("intdespa")
                    End If
                    strLinha &= Right(Space(15) & .Item("valdespaemp").ToString, 15)
                End With
            Next
            PrintLine(intArq, strLinha)
            PrintLine(intArq, "")
            PrintLine(intArq, "")

            'CRONOGRAMA DE PARADAS DE MÁQUINAS HIDRÁULICAS - NE
            'Tabela ParadaNE
            BindReport("ParadaNE_H")
            dtsDataset = Session("report_ds")
            PrintLine(intArq, "4-CRONOGRAMA DE PARADAS DE MÁQUINAS HIDRÁULICAS E TÉRMICAS (" & Request.QueryString("strdata").Substring(6, 2) & "/" & Request.QueryString("strdata").Substring(4, 2) & "/" & Request.QueryString("strdata").Substring(0, 4) & ")")
            strLinha = Space(15)
            strLinha2 = Space(18)
            strLinha3 = "Intervalo  "
            strCodUsina = ""
            For intI = 0 To dtsDataset.Tables("paradane").Rows.Count - 1
                If dtsDataset.Tables("paradane").Rows(intI).Item("intervalo") <> 1 Then
                    Exit For
                End If
                If strCodUsina <> Trim(dtsDataset.Tables("paradane").Rows(intI).Item("codusina")) Then
                    strCodUsina = Trim(dtsDataset.Tables("paradane").Rows(intI).Item("codusina"))
                    strLinha &= Right(Space(17) & strCodUsina & Space(8), 17) & Space(3)
                    strLinha2 &= Right(Space(17) & ("Nº MAQ. " & Trim(dtsDataset.Tables("paradane").Rows(intI).Item("maq").ToString)) & Space(10), 17) & Space(3)
                    strLinha3 &= "   C/O  M   G   S   "
                End If
            Next

            PrintLine(intArq, strLinha)
            PrintLine(intArq, strLinha2)
            PrintLine(intArq, strLinha3)

            strLinha = ""
            strCodUsina = ""
            For intI = 0 To dtsDataset.Tables("paradane").Rows.Count - 1
                With dtsDataset.Tables("paradane").Rows(intI)
                    If intCarga <> .Item("intervalo") Then
                        PrintLine(intArq, strLinha)
                        strLinha = Intervalo(.Item("intervalo")) & Space(1)
                        intCarga = .Item("intervalo")
                    End If

                    If strCodUsina <> Trim(dtsDataset.Tables("paradane").Rows(intI).Item("codusina")) Then
                        If Len(strLinha) > 12 Then
                            strLinha &= Space(4)
                        End If
                        strCodUsina = Trim(dtsDataset.Tables("paradane").Rows(intI).Item("codusina"))
                    End If
                    strLinha &= Right(Space(4) & .Item("valor").ToString, 4)
                End With
            Next
            PrintLine(intArq, strLinha)
            PrintLine(intArq, "")
            PrintLine(intArq, "")

            ''CRONOGRAMA DE PARADAS DE MÁQUINAS TÉRMICAS E EÓLICAS - NE
            ''Tabela ParadaNE
            'BindReport("ParadaNE_T")
            'dtsDataset = Session("report_ds")
            'PrintLine(intArq, "5-CRONOGRAMA DE PARADAS DE MÁQUINAS TÉRMICAS E EÓLICAS (" & Request.QueryString("strdata").Substring(6, 2) & "/" & Request.QueryString("strdata").Substring(4, 2) & "/" & Request.QueryString("strdata").Substring(0, 4) & ")")
            'strLinha = Space(15)
            'strLinha2 = Space(18)
            'strLinha3 = "Intervalo  "
            'strCodUsina = ""
            'For intI = 0 To dtsDataset.Tables("paradane").Rows.Count - 1
            '    If dtsDataset.Tables("paradane").Rows(intI).Item("intervalo") <> 1 Then
            '        Exit For
            '    End If
            '    If strCodUsina <> Trim(dtsDataset.Tables("paradane").Rows(intI).Item("codusina")) Then
            '        strCodUsina = Trim(dtsDataset.Tables("paradane").Rows(intI).Item("codusina"))
            '        strLinha &= Right(Space(17) & strCodUsina & Space(8), 17) & Space(3)
            '        strLinha2 &= Right(Space(17) & ("Nº MAQ. " & Trim(dtsDataset.Tables("paradane").Rows(intI).Item("maq").ToString)) & Space(10), 17) & Space(3)
            '        strLinha3 &= "   C/O  M   G   S   "
            '    End If
            'Next

            'PrintLine(intArq, strLinha)
            'PrintLine(intArq, strLinha2)
            'PrintLine(intArq, strLinha3)

            'strLinha = ""
            'strCodUsina = ""
            'For intI = 0 To dtsDataset.Tables("paradane").Rows.Count - 1
            '    With dtsDataset.Tables("paradane").Rows(intI)
            '        If intCarga <> .Item("intervalo") Then
            '            PrintLine(intArq, strLinha)
            '            strLinha = Intervalo(.Item("intervalo")) & Space(1)
            '            intCarga = .Item("intervalo")
            '        End If

            '        If strCodUsina <> Trim(dtsDataset.Tables("paradane").Rows(intI).Item("codusina")) Then
            '            If Len(strLinha) > 12 Then
            '                strLinha &= Space(4)
            '            End If
            '            strCodUsina = Trim(dtsDataset.Tables("paradane").Rows(intI).Item("codusina"))
            '        End If
            '        strLinha &= Right(Space(4) & .Item("valor").ToString, 4)
            '    End With
            'Next
            'PrintLine(intArq, strLinha)
            'PrintLine(intArq, "")
            'PrintLine(intArq, "")


            'REQUISITOS POR ÁREAS - NE
            'Tabela carga
            BindReport("CargaNE")
            dtsDataset = Session("report_ds")
            PrintLine(intArq, "6-REQUISITOS POR ÁREAS (" & Request.QueryString("strdata").Substring(6, 2) & "/" & Request.QueryString("strdata").Substring(4, 2) & "/" & Request.QueryString("strdata").Substring(0, 4) & ")")
            strLinha = "Intervalo  "
            For intI = 0 To dtsDataset.Tables("carga").Rows.Count - 1
                If dtsDataset.Tables("carga").Rows(intI).Item("intcarga") <> 1 Then
                    Exit For
                End If
                strLinha &= Right(Space(15) & Trim(dtsDataset.Tables("carga").Rows(intI).Item("codempre")), 15)
            Next
            PrintLine(intArq, strLinha)
            strLinha = ""
            For intI = 0 To dtsDataset.Tables("carga").Rows.Count - 1
                With dtsDataset.Tables("carga").Rows(intI)
                    If intCarga <> .Item("intcarga") Then
                        PrintLine(intArq, strLinha)
                        strLinha = Intervalo(.Item("intcarga"))
                        intCarga = .Item("intcarga")
                    End If
                    strLinha &= Right(Space(15) & .Item("valcargasup").ToString, 15)
                End With
            Next
            PrintLine(intArq, strLinha)
            PrintLine(intArq, "")
            PrintLine(intArq, "")

        End If

        'Salva e Fecha o arquivo texto
        FileClose(intArq)

        'Exibir o relatório em uma nova janela
        strWinParam = "height=600,width=800,toolbar=yes,location=no,directories=no,status=no,menubar=yes,scrollbars=yes,resizable=yes"
        strWindow = "newWindow"
        strUrlPath += "/Temp/" & Session.SessionID.ToString() + "/" & strFileName
        Redireciona(strUrlPath, Me, strWindow, strWinParam)
    End Sub

    Private Function Intervalo(ByVal intIntervalo As Integer) As String
        Dim intHora As Int16
        Dim strHora As String
        intHora = ((intIntervalo + 1) / 2) - 1
        If intIntervalo Mod 2 = 0 Then
            strHora = ((intIntervalo / 2) - 1).ToString.PadLeft(2, "0") & ":30 " & (intIntervalo / 2).ToString.PadLeft(2, "0") & ":00"
        Else
            strHora = intHora.ToString.PadLeft(2, "0") & ":00 " & intHora.ToString.PadLeft(2, "0") & ":30"
        End If
        Intervalo = strHora
    End Function

    '-- CRQ5000 - 17/09/2013
    '-- Nova Rotina

    'Private Function ExportaCSV(ByVal filename As String) As String
    '    '-- Define variaveis
    '    Dim tabelaEntrada As dtsRelatorio = Session("report_ds")
    '    Dim TextoDelimitador As Char
    '    Dim TextoQualificadores As Char
    '    '--
    '    Dim strPath As String = Request.PhysicalApplicationPath()
    '    Dim strFullPathFileName As String
    '    strPath &= "pdpw\Temp\" & Session.SessionID + "\"
    '    System.IO.Directory.CreateDirectory(strPath)
    '    strFullPathFileName = strPath & filename
    '    'Se já existir um arquivo excluo e crio outro novo.
    '    If System.IO.File.Exists(strFullPathFileName) Then
    '        System.IO.File.Delete(strFullPathFileName)
    '    End If
    '    '--
    '    Dim arquivo As System.IO.StreamWriter
    '    Dim linha As New System.Text.StringBuilder

    '    '-- Atribui valores
    '    TextoDelimitador = ";"c
    '    TextoQualificadores = """"c
    '    arquivo = New System.IO.StreamWriter(filename)

    '    '-- Monta Header 
    '    Try
    '        For Each ExportaColuna As DataColumn In tabelaEntrada.PDDefluxSIN.Columns
    '            Dim ColunaTexto As String = ExportaColuna.ColumnName.ToString()
    '            ColunaTexto = ColunaTexto.Replace(TextoQualificadores.ToString(), TextoQualificadores.ToString() + TextoQualificadores.ToString())
    '            linha.Append(TextoQualificadores + ExportaColuna.ColumnName + TextoQualificadores)
    '            linha.Append(TextoDelimitador)
    '        Next
    '        arquivo.WriteLine(linha.ToString())
    '    Catch ex As Exception
    '        Throw ex
    '    End Try

    '    '-- Monta Linhas
    '    Try
    '        For Each ExportarRow As DataRow In tabelaEntrada.PDDefluxSIN.Rows
    '            For Each ExportaColuna As DataColumn In tabelaEntrada.PDDefluxSIN.Columns
    '                Dim ColunaTexto As String = ExportarRow(ExportaColuna.ColumnName).ToString()
    '                ColunaTexto = ColunaTexto.Replace(TextoQualificadores.ToString(), TextoQualificadores.ToString() + TextoQualificadores.ToString())
    '                linha.Append(TextoQualificadores + ColunaTexto + TextoQualificadores)
    '                linha.Append(TextoDelimitador)
    '            Next
    '            arquivo.WriteLine(linha.ToString())
    '        Next
    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        If Not arquivo Is Nothing Then arquivo.Close()
    '    End Try

    '    '-- Retorna
    '    Return "OK" '--arquivo.ToString()

    'End Function

End Class
