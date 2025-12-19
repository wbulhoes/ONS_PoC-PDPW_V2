'Classes base para manipulação de texto
Imports System.Text
'Classes base para manipulação do ambiente corrente
Imports System.Environment
Imports System.Data.SqlClient

Partial Class frmPlanilha
    Inherits System.Web.UI.Page
    Public dtsRelatorio As DataSet
    Public strTitulo As String
    Public strSubTitulo As String
    Public strTabela As String
    Private strBase As String

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
        MontaDataSet()
        MostraArquivoTexto()
    End Sub

    Private Sub MontaDataSet()
        Dim objConn As New SqlConnection
        Dim strSql, strDataPDP, strDataPDPFim, strCampo, strEmpresa, strNomecurto As String
        'Atribui os parâmetros as variáveis
        strDataPDP = Request.QueryString("strDataPDP")
        strDataPDPFim = Request.QueryString("strDataPDPFim")
        If strDataPDP.Contains("/") Then
            strDataPDP = strDataPDP.Substring(6, 4) + strDataPDP.Substring(3, 2) + strDataPDP.Substring(0, 2)
        End If
        If Not IsNothing(strDataPDPFim) Then
            If (strDataPDPFim.Contains("/")) Then
                strDataPDPFim = strDataPDPFim.Substring(6, 4) + strDataPDPFim.Substring(3, 2) + strDataPDPFim.Substring(0, 2)
            End If
        End If
        strCampo = Request.QueryString("strCampo")
        strEmpresa = Request.QueryString("strEmpresa").Split("|").GetValue(0)
        strNomecurto = Request.QueryString("strEmpresa").Split("|").GetValue(1)
        strTabela = Request.QueryString("strTabela")

        objConn.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
        objConn.Open()

        Select Case strTabela
            Case Is = "carga"
                strTitulo = "Carga "
                strSubTitulo = ""
                strSql = "SELECT intcarga AS intervalo, '" & strNomecurto & "' AS nomecurto, ISNULL(valcarga" & strCampo & ",0) AS valor, '' AS msg " &
                         "FROM carga " &
                         "WHERE codempre = '" & strEmpresa & "' " &
                         "AND datpdp = '" & strDataPDP & "' " &
                         "ORDER BY intcarga"
            Case Is = "despa"
                strTitulo = "Geração "
                strSubTitulo = "Empresa: " & strNomecurto
                strSql = "SELECT d.codusina AS nomecurto, d.intdespa AS intervalo, ISNULL(d.valdespa" & strCampo & ",0) AS valor, u.ordem, u.nomusina " &
                         "FROM despa d, usina u " &
                         "WHERE u.codempre = '" & strEmpresa & "' " &
                         "AND u.codusina = d.codusina " &
                         "AND d.datpdp = '" & strDataPDP & "' " &
                         "ORDER BY d.intdespa, u.ordem, d.codusina"
            Case Is = "balanco"
                strTitulo = "Balanço "
                strSubTitulo = "Empresa: " & strNomecurto
                'Somando a geração em uma tabela temporária
                strSql = "SELECT 'Geração' AS nomecurto, SUM(ISNULL(g.valdespa" & strCampo & ",0)) AS valor, g.intdespa AS intervalo " &
                         "FROM usina u, despa g " &
                         "WHERE u.codempre = '" & strEmpresa & "' " &
                         "AND u.codusina = g.codusina " &
                         "AND g.datpdp = '" & strDataPDP & "' " &
                         "GROUP BY intdespa " &
                         "UNION " &
                         "SELECT 'Carga' AS nomecurto, ISNULL(valcarga" & strCampo & ",0) AS valor, intcarga AS intervalo " &
                         "FROM carga " &
                         "WHERE codempre = '" & strEmpresa & "' " &
                         "AND datpdp = '" & strDataPDP & "' " &
                         "UNION " &
                         "SELECT 'Intercâmbio' AS nomecurto, SUM(ISNULL(valinter" & strCampo & ",0)) AS valor, intinter AS intervalo " &
                         "FROM inter " &
                         "WHERE codemprede = '" & strEmpresa & "' " &
                         "AND datpdp = '" & strDataPDP & "' " &
                         "GROUP BY intinter " &
                         "ORDER BY 3, 1"
            Case Is = "compensacao"
                Dim strNomeTabela As String
                If strBase = "pdp" Then
                    strNomeTabela = "complastro_fisico"
                Else
                    strNomeTabela = "Compensacaolastro_fisico"
                End If
                strTitulo = "Compensação de Lastro Físico"
                strSubTitulo = "Empresa: " & strNomecurto
                strSql = "SELECT d.codusina AS nomecurto, d.intclf AS intervalo, ISNULL(d.valclf" & strCampo & ",0) AS valor, u.ordem, u.nomusina " &
                         "FROM " & strNomeTabela & " d, usina u " &
                         "WHERE u.codempre = '" & strEmpresa & "' " &
                         "AND u.codusina = d.codusina " &
                         "AND d.datpdp = '" & strDataPDP & "' " &
                         "ORDER BY d.intclf, u.ordem, d.codusina"
            Case Is = "perdas"
                strTitulo = "Perdas de Consumo Interno e Compensação"
                strSubTitulo = "Empresa: " & strNomecurto
                strSql = "SELECT d.codusina AS nomecurto, d.intpcc AS intervalo, ISNULL(d.valpcc" & strCampo & ",0) AS valor, u.ordem, u.nomusina " &
                         "FROM perdascic d, usina u " &
                         "WHERE u.codempre = '" & strEmpresa & "' " &
                         "AND u.codusina = d.codusina " &
                         "AND d.datpdp = '" & strDataPDP & "' " &
                         "ORDER BY d.intpcc, u.ordem, d.codusina"
            Case Is = "desp_inflex"
                strTitulo = "Motivo de Despacho por Inflexibilidade"
                strSubTitulo = "Empresa: " & strNomecurto
                strSql = "SELECT d.codusina AS nomecurto, d.intmif AS intervalo, ISNULL(d.valmif" & strCampo & ",0) AS valor, u.ordem, u.nomusina " &
                         "FROM motivoinfl d, usina u " &
                         "WHERE u.codempre = '" & strEmpresa & "' " &
                         "AND u.codusina = d.codusina " &
                         "AND d.datpdp = '" & strDataPDP & "' " &
                         "ORDER BY d.intmif, u.ordem, d.codusina"
            Case Is = "rest_falta_comb"
                strTitulo = "Restrição por Falta de Combustível"
                strSubTitulo = "Empresa: " & strNomecurto
                strSql = "SELECT d.codusina AS nomecurto, d.intrfc AS intervalo, ISNULL(d.valrfc" & strCampo & ",0) AS valor, u.ordem, u.nomusina " &
                         "FROM rest_falta_comb d, usina u " &
                         "WHERE u.codempre = '" & strEmpresa & "' " &
                         "AND u.codusina = d.codusina " &
                         "AND d.datpdp = '" & strDataPDP & "' " &
                         "ORDER BY d.intrfc, u.ordem, d.codusina"
            Case Is = "desp_razele"
                strTitulo = "Motivo de Despacho por Razão Elétrica"
                strSubTitulo = "Empresa: " & strNomecurto
                strSql = "SELECT d.codusina AS nomecurto, d.intmre AS intervalo, ISNULL(d.valmre" & strCampo & ",0) AS valor, u.ordem, u.nomusina " &
                         "FROM motivorel d, usina u " &
                         "WHERE u.codempre = '" & strEmpresa & "' " &
                         "AND u.codusina = d.codusina " &
                         "AND d.datpdp = '" & strDataPDP & "' " &
                         "ORDER BY d.intmre, u.ordem, d.codusina"
            Case Is = "disponibilidade"
                strTitulo = "Disponibilidade"
                strSubTitulo = "Empresa: " & strNomecurto
                strSql = "SELECT d.codusina AS nomecurto, d.intdsp AS intervalo, ISNULL(d.valdsp" & strCampo & ",0) AS valor, u.ordem, u.nomusina " &
                         "FROM disponibilidade d, usina u " &
                         "WHERE u.codempre = '" & strEmpresa & "' " &
                         "AND u.codusina = d.codusina " &
                         "AND d.datpdp = '" & strDataPDP & "' " &
                         "ORDER BY d.intdsp, u.ordem, d.codusina"
            Case Is = "razele"
                strTitulo = "Razão Elétrica"
                strSubTitulo = "Empresa: " & strNomecurto
                strSql = "SELECT d.codusina AS nomecurto, d.intrazelet AS intervalo, ISNULL(d.valrazelet" & strCampo & ",0) AS valor, u.ordem, u.nomusina " &
                         "FROM razaoelet d, usina u " &
                         "WHERE u.codempre = '" & strEmpresa & "' " &
                         "AND u.codusina = d.codusina " &
                         "AND d.datpdp = '" & strDataPDP & "' " &
                         "ORDER BY d.intrazelet, u.ordem, d.codusina"
            Case Is = "razene"
                strTitulo = "Razão Energética"
                strSubTitulo = "Empresa: " & strNomecurto
                strSql = "SELECT d.codusina AS nomecurto, d.intrazener AS intervalo, ISNULL(d.valrazener" & strCampo & ",0) AS valor, u.ordem, u.nomusina " &
                         "FROM razaoener d, usina u " &
                         "WHERE u.codempre = '" & strEmpresa & "' " &
                         "AND u.codusina = d.codusina " &
                         "AND d.datpdp = '" & strDataPDP & "' " &
                         "ORDER BY d.intrazener, u.ordem, d.codusina"
            Case Is = "ERP"
                strTitulo = "Energia de Reposição e Perdas"
                strSubTitulo = "Empresa: " & strNomecurto
                strSql = "SELECT d.codusina AS nomecurto, d.interp AS intervalo, ISNULL(d.valerp" & strCampo & ",0) AS valor, u.ordem, u.nomusina " &
                         "FROM energia_reposicao d, usina u " &
                         "WHERE u.codempre = '" & strEmpresa & "' " &
                         "AND u.codusina = d.codusina " &
                         "AND d.datpdp = '" & strDataPDP & "' " &
                         "ORDER BY d.interp, u.ordem, d.codusina"
            Case Is = "EXP"
                strTitulo = "Exportação"
                strSubTitulo = "Empresa: " & strNomecurto
                strSql = "SELECT d.codusina AS nomecurto, d.intexporta AS intervalo, ISNULL(d.valexporta" & strCampo & ",0) AS valor, u.ordem, u.nomusina " &
                         "FROM exporta d, usina u " &
                         "WHERE u.codempre = '" & strEmpresa & "' " &
                         "AND u.codusina = d.codusina " &
                         "AND d.datpdp = '" & strDataPDP & "' " &
                         "ORDER BY d.intexporta, u.ordem, d.codusina"
            Case Is = "IMP"
                strTitulo = "Importação"
                strSubTitulo = "Empresa: " & strNomecurto
                strSql = "SELECT d.codusina AS nomecurto, d.intimporta AS intervalo, ISNULL(d.valimporta" & strCampo & ",0) AS valor, u.ordem, u.nomusina " &
                         "FROM importa d, usina u " &
                         "WHERE u.codempre = '" & strEmpresa & "' " &
                         "AND u.codusina = d.codusina " &
                         "AND d.datpdp = '" & strDataPDP & "' " &
                         "ORDER BY d.intimporta, u.ordem, d.codusina "
            Case Is = "IFX"
                strTitulo = "Inflexibilidade"
                strSubTitulo = "Empresa: " & strNomecurto
                strSql = "SELECT d.codusina AS nomecurto, d.intflexi AS intervalo, ISNULL(d.valflexi" & strCampo & ",0) AS valor, u.ordem, u.nomusina " &
                         "FROM Inflexibilidade d, usina u " &
                         "WHERE u.codempre = '" & strEmpresa & "' " &
                         "AND u.codusina = d.codusina " &
                         "AND d.datpdp = '" & strDataPDP & "' " &
                         "ORDER BY d.intflexi, u.ordem, d.codusina "
            Case Is = "INT"
                strTitulo = "Intercâmbio"
                strSubTitulo = "Empresa: " & strNomecurto
                strSql = "SELECT codemprede + '-' + codemprepara + '/' + codcontamodal AS nomecurto, intinter AS intervalo, SUM(ISNULL(valinter" & strCampo & ",0)) AS valor " &
                         "FROM inter " &
                         "WHERE codemprede = '" & strEmpresa & "' " &
                         "AND datpdp = " & strDataPDP & " " &
                         "GROUP BY 2, 1 " &
                         "ORDER BY 2, 1"
            Case Is = "MEG"
                strTitulo = "Número de Máquinas Gerando"
                strSubTitulo = "Empresa: " & strNomecurto
                strSql = "SELECT d.codusina AS nomecurto, d.intmeg AS intervalo, ISNULL(d.valmeg" & strCampo & ",0) AS valor, u.ordem, u.nomusina " &
                         "FROM maq_gerando d, usina u " &
                         "WHERE u.codempre = '" & strEmpresa & "' " &
                         "AND u.codusina = d.codusina " &
                         "AND d.datpdp = '" & strDataPDP & "' " &
                         "ORDER BY d.intmeg, u.ordem, d.codusina"
            Case Is = "MOS"
                strTitulo = "Número de Máquinas Operando como Síncrono"
                strSubTitulo = "Empresa: " & strNomecurto
                strSql = "SELECT d.codusina AS nomecurto, d.intmos AS intervalo, ISNULL(d.valmos" & strCampo & ",0) AS valor, u.ordem, u.nomusina " &
                         "FROM oper_sincrono d, usina u " &
                         "WHERE u.codempre = '" & strEmpresa & "' " &
                         "AND u.codusina = d.codusina " &
                         "AND d.datpdp = '" & strDataPDP & "' " &
                         "ORDER BY d.intmos, u.ordem, d.codusina"
            Case Is = "MCO"
                strTitulo = "Número de Máquinas Paradas por Conveniência Operativa"
                strSubTitulo = "Empresa: " & strNomecurto
                strSql = "SELECT d.codusina AS nomecurto, d.intmco AS intervalo, ISNULL(d.valmco" & strCampo & ",0) AS valor, u.ordem, u.nomusina " &
                         "FROM conveniencia_oper d, usina u " &
                         "WHERE u.codempre = '" & strEmpresa & "' " &
                         "AND u.codusina = d.codusina " &
                         "AND d.datpdp = '" & strDataPDP & "' " &
                         "ORDER BY d.intmco, u.ordem, d.codusina"
            Case Is = "MAN"
                strTitulo = "Manutenções"
                strSubTitulo = "Empresa: " & strNomecurto
                strSql = "SELECT p.codparal, u.codempre, u.codusina, g.siggerad, p.datiniparal, p.intiniparal, " &
                         "p.datfimparal, p.intfimparal, p.intinivoltaparal, p.intfimvoltaparal, p.codnivel, " &
                         "p.indcont, u.nomusina " &
                         "FROM " & strCampo & " p, gerad g, usina u " &
                         "WHERE u.codusina = g.codusina " &
                         "AND g.codgerad = p.codequip " &
                         "AND u.codempre = '" & strEmpresa & "' "
                If strDataPDP <> "0" Then
                    strSql &= "AND '" & strDataPDP & "' BETWEEN datiniparal " &
                              "AND datfimparal "
                End If
                strSql &= "ORDER BY u.codempre, p.datiniparal, p.intiniparal, u.codusina"
            Case Is = "RES"
                'ISNULL(r.obsrestr,'') as obsrestr " & _
                strTitulo = "Restrições"
                strSubTitulo = "Empresa: " & strNomecurto
                strSql = "SELECT r.codrestr, u.codempre, u.codusina, g.siggerad, r.datinirestr, " &
                         "r.intinirestr, r.datfimrestr, r.intfimrestr, r.valrestr, " &
                         "ISNULL(m.dsc_motivorestricao,'') as dsc_motivorestricao, u.nomusina " &
                         "FROM " & strCampo & " r, gerad g, usina u, outer tb_motivorestricao m " &
                         "WHERE u.codusina = g.codusina " &
                         "AND g.codgerad = r.codgerad  AND m.id_motivorestricao = r.id_motivorestricao " &
                         "AND u.codempre = '" & strEmpresa & "' "
                If strDataPDP <> 0 Then
                    strSql &= "AND '" & strDataPDP & "' BETWEEN datinirestr " &
                              "AND datfimrestr "
                End If
                ', ISNULL(r.obsrestr,'') as obsrestr " & _
                strSql &= "UNION " &
                          "SELECT r.codrestr, u.codempre, u.codusina, '', r.datinirestr, " &
                          "r.intinirestr, r.datfimrestr, r.intfimrestr, r.valrestr, " &
                          "ISNULL(m.dsc_motivorestricao,'') as dsc_motivorestricao, u.nomusina " &
                          "FROM "
                Select Case strCampo
                    Case Is = "restrgerademp"
                        strSql &= "restrusinaemp"
                    Case Is = "temprestrgerad"
                        strSql &= "temprestrusina"
                    Case Is = "restrgerad"
                        strSql &= "restrusina"
                End Select
                strSql &= " r, usina u, outer tb_motivorestricao m " &
                          "WHERE u.codusina = r.codusina  AND m.id_motivorestricao = r.id_motivorestricao " &
                          "AND u.codempre = '" & strEmpresa & "' "
                If strDataPDP <> 0 Then
                    strSql &= "AND '" & strDataPDP & "' BETWEEN datinirestr " &
                              "AND datfimrestr "
                End If
                strSql &= "ORDER BY 3, 5, 6"
            Case Is = "PCO"
                strTitulo = "Parada de Unidade Geradora por Conveniência Operativa"
                strSubTitulo = "Empresa: " & strNomecurto
                strSql = "SELECT p.codparal, u.codempre, u.codusina, g.siggerad, p.datiniparal, " &
                         "p.intiniparal, p.datfimparal, p.intfimparal, u.nomusina " &
                         "FROM " & strCampo & " p, gerad g, usina u " &
                         "WHERE u.codusina = g.codusina " &
                         "AND g.codgerad = p.codequip " &
                         "AND u.codempre = '" & strEmpresa & "' "
                If strDataPDP <> 0 Then
                    strSql &= "AND '" & strDataPDP & "' BETWEEN datiniparal " &
                              "AND datfimparal "
                End If
                strSql &= "ORDER BY u.codempre, p.datiniparal, p.intiniparal, u.codusina"
            Case Is = "PGA"
                strTitulo = "Proposta de Geração"
                strSubTitulo = "Empresa: " & strNomecurto
                strSql = "SELECT n.ipat, n.patini, n.patfim, n.nivcarga, d.codusina, ISNULL(d.valppgpro,0) AS valor, u.ordem, u.nomusina " &
                         "FROM usina u, despa_ppg d, pdp p, nivcarga_ppg n " &
                         "WHERE u.codempre = '" & strEmpresa & "' " &
                         "AND u.codusina = d.codusina " &
                         "AND d.datpdp = '" & strDataPDP & "' " &
                         "AND d.datpdp = p.datpdp " &
                         "AND p.codtipdia = n.codtipdia " &
                         "AND n.ipat = d.ipat " &
                         "ORDER BY u.ordem, d.codusina, n.ipat"
                '-- CRQ2345 (15/08/2012)
            Case Is = "VAZ"
                strTitulo = "Vazão"
                If strDataPDPFim <> 0 Then
                    strSubTitulo = "Empresa: " & strNomecurto
                    '
                    ' WI 146628 - A query comentada não estava funcionando e foi corrigida
                    '
                    strSql = "SELECT v.datpdp, v.codusina, ISNULL(v.valturb, 0) AS valturb, ISNULL(v.valvert, 0) AS valvert, ISNULL(v.valaflu, 0) AS valaflu, ISNULL(c.cotaini, 0) AS valcota, ISNULL(c.cotafim, 0) AS valcotaf, ISNULL(c.outrasestruturas, 0) AS outrasestr, c.comentariopdf AS comentpdf, u.ordem, u.nomusina " &
                             " FROM  vazao v " &
                             " JOIN usina u ON u.codusina = v.codusina AND u.codempre = '" & strEmpresa & "' " &
                             " LEFT JOIN cota c ON c.codusina = v.codusina AND c.datpdp BETWEEN '" & strDataPDP & "' AND '" & strDataPDPFim & "' " &
                             " WHERE  v.datpdp BETWEEN '" & strDataPDP & "' AND '" & strDataPDPFim & "' " &
                             " ORDER BY v.datpdp, u.ordem, v.codusina;"


                    'strSql = "SELECT v.datpdp, v.codusina, ISNULL(v.valturb" & strCampo & ",0) valturb, ISNULL(v.valvert" & strCampo & ",0) valvert, " &
                    '         "ISNULL(v.valaflu" & strCampo & ",0) valaflu, ISNULL(c.cotaini" & strCampo & ",0) valcota, ISNULL(c.cotafim" & strCampo & ",0) valcotaf, ISNULL(c.outrasestruturas" & strCampo & ",0) outrasestr, c.comentariopdf" & strCampo & " comentpdf, u.ordem, u.nomusina " &
                    '         "FROM vazao v, usina u, outer cota c " &
                    '         "WHERE u.codempre = '" & strEmpresa & "' " &
                    '         "AND u.codusina = v.codusina " &
                    '         "AND u.codusina = c.codusina " &
                    '         "AND v.datpdp BETWEEN '" & strDataPDP & "' " &
                    '         "AND '" & strDataPDPFim & "' " &
                    '         "AND c.datpdp BETWEEN '" & strDataPDP & "' " &
                    '         "AND '" & strDataPDPFim & "' " &
                    '         "ORDER BY v.datpdp, u.ordem, v.codusina"
                Else
                    strSubTitulo = ""
                    '
                    ' WI 146628 - A query comentada não estava funcionando e foi corrigida
                    '
                    strSql = "SELECT e.sigempre, SUM(ISNULL(v.valturbtran, 0)) AS valturb, SUM(ISNULL(v.valverttran, 0)) AS valvert, SUM(ISNULL(v.valaflutran, 0)) AS valaflu, SUM(ISNULL(c.cotainitran, 0)) AS valcota, ISNULL(c.cotafimtran, 0) AS valcotaf, ISNULL(c.outrasestruturastran, 0) AS outrasestr, c.comentariopdftran AS comentpdf " &
                             "FROM vazao v " &
                             "JOIN usina u ON v.codusina = u.codusina " &
                             "JOIN empre e ON u.codempre = e.codempre  " &
                             "LEFT JOIN cota c ON u.codusina = c.codusina " &
                             "AND c.datpdp = '" & strDataPDP & "' " &
                             "WHERE v.datpdp = '" & strDataPDP & "' AND u.tipusina = 'H' " &
                             "GROUP BY e.sigempre, c.cotafimtran, c.outrasestruturastran, c.comentariopdftran " &
                             "ORDER BY e.sigempre;"
                    'strSql = "SELECT e.sigempre, SUM(ISNULL(v.valturb" & strCampo & ",0)) AS valturb, " &
                    '         "SUM(ISNULL(v.valvert" & strCampo & ",0)) AS valvert, SUM(ISNULL(v.valaflu" & strCampo & ",0)) AS valaflu, " &
                    '         "SUM(ISNULL(c.cotaini" & strCampo & ",0)) AS valcota, ISNULL(c.cotafim" & strCampo & ",0) valcotaf, ISNULL(c.outrasestruturas" & strCampo & ",0) outrasestr, c.comentariopdf" & strCampo & " comentpdf " &
                    '         "FROM vazao v, empre e, usina u, OUTER cota c " &
                    '         "WHERE v.datpdp = '" & strDataPDP & "' " &
                    '         "AND v.codusina = u.codusina " &
                    '         "AND u.codempre = e.codempre " &
                    '         "AND u.tipusina = 'H' " &
                    '         "AND u.codusina = c.codusina " &
                    '         "AND c.datpdp = '" & strDataPDP & "' " &
                    '         "GROUP BY e.sigempre " &
                    '         "ORDER BY e.sigempre"
                End If
            Case Is = "RMP"
                strTitulo = "Garantia Energética"
                strSubTitulo = "Empresa: " & strNomecurto
                strSql = "SELECT r.codusina AS nomecurto, r.intrmp AS intervalo, ISNULL(r.valrmp" & strCampo & ",0) AS valor, u.ordem, u.nomusina " &
                         "FROM tb_rmp r, usina u " &
                         "WHERE u.codempre = '" & strEmpresa & "' " &
                         "AND u.codusina = r.codusina " &
                         "AND r.datpdp = '" & strDataPDP & "' " &
                         "ORDER BY r.intrmp, u.ordem, r.codusina"
            Case Is = "GFM"
                strTitulo = "Geração Fora de Mérito"
                strSubTitulo = "Empresa: " & strNomecurto
                strSql = "SELECT r.codusina AS nomecurto, r.intgfm AS intervalo, ISNULL(r.valgfm" & strCampo & ",0) AS valor, u.ordem, u.nomusina " &
                         "FROM tb_gfm r, usina u " &
                         "WHERE u.codempre = '" & strEmpresa & "' " &
                         "AND u.codusina = r.codusina " &
                         "AND r.datpdp = '" & strDataPDP & "' " &
                         "ORDER BY r.intgfm, u.ordem, r.codusina"
            Case Is = "CFM"
                strTitulo = "Crédito por Substituição"
                strSubTitulo = "Empresa: " & strNomecurto
                strSql = "SELECT r.codusina AS nomecurto, r.intcfm AS intervalo, ISNULL(r.valcfm" & strCampo & ",0) AS valor, u.ordem, u.nomusina " &
                         "FROM tb_cfm r, usina u " &
                         "WHERE u.codempre = '" & strEmpresa & "' " &
                         "AND u.codusina = r.codusina " &
                         "AND r.datpdp = '" & strDataPDP & "' " &
                         "ORDER BY r.intcfm, u.ordem, r.codusina"
            Case Is = "SOM"
                strTitulo = "Geração Substituta"
                strSubTitulo = "Empresa: " & strNomecurto
                strSql = "SELECT r.codusina AS nomecurto, r.intsom AS intervalo, ISNULL(r.valsom" & strCampo & ",0) AS valor, u.ordem, u.nomusina " &
                         "FROM tb_som r, usina u " &
                         "WHERE u.codempre = '" & strEmpresa & "' " &
                         "AND u.codusina = r.codusina " &
                         "AND r.datpdp = '" & strDataPDP & "' " &
                         "ORDER BY r.intsom, u.ordem, r.codusina"

            Case Is = "GES"
                strTitulo = "GE Substituição"
                strSubTitulo = "Empresa: " & strNomecurto
                strSql = "SELECT r.codusina AS nomecurto, r.intGES AS intervalo, ISNULL(r.valGES" & strCampo & ",0) AS valor, u.ordem, u.nomusina " &
                         "FROM tb_GES r, usina u " &
                         "WHERE u.codempre = '" & strEmpresa & "' " &
                         "AND u.codusina = r.codusina " &
                         "AND r.datpdp = '" & strDataPDP & "' " &
                         "ORDER BY r.intGES, u.ordem, r.codusina"

            Case Is = "GEC"
                strTitulo = "GE Crédito"
                strSubTitulo = "Empresa: " & strNomecurto
                strSql = "SELECT r.codusina AS nomecurto, r.intGEC AS intervalo, ISNULL(r.valGEC" & strCampo & ",0) AS valor, u.ordem, u.nomusina " &
                         "FROM tb_GEC r, usina u " &
                         "WHERE u.codempre = '" & strEmpresa & "' " &
                         "AND u.codusina = r.codusina " &
                         "AND r.datpdp = '" & strDataPDP & "' " &
                         "ORDER BY r.intGEC, u.ordem, r.codusina"

            Case Is = "DCA"
                strTitulo = "Despacho Ciclo Aberto"
                strSubTitulo = "Empresa: " & strNomecurto
                strSql = "SELECT r.codusina AS nomecurto, r.intDCA AS intervalo, ISNULL(r.valDCA" & strCampo & ",0) AS valor, u.ordem, u.nomusina " &
                         "FROM tb_DCA r, usina u " &
                         "WHERE u.codempre = '" & strEmpresa & "' " &
                         "AND u.codusina = r.codusina " &
                         "AND r.datpdp = '" & strDataPDP & "' " &
                         "ORDER BY r.intDCA, u.ordem, r.codusina"

            Case Is = "DCR"
                strTitulo = "Despacho Ciclo Reduzido"
                strSubTitulo = "Empresa: " & strNomecurto
                strSql = "SELECT r.codusina AS nomecurto, r.intDCR AS intervalo, ISNULL(r.valDCR" & strCampo & ",0) AS valor, u.ordem, u.nomusina " &
                         "FROM tb_DCR r, usina u " &
                         "WHERE u.codempre = '" & strEmpresa & "' " &
                         "AND u.codusina = r.codusina " &
                         "AND r.datpdp = '" & strDataPDP & "' " &
                         "ORDER BY r.intDCR, u.ordem, r.codusina"

            Case Is = "IR1" '?
                strTitulo = "Nível de Partida"
                strSubTitulo = "Empresa: " & strNomecurto
                strSql = "SELECT r.codusina AS nomecurto, ISNULL(r.valIR1" & strCampo & ",0) AS valor, u.ordem, u.nomusina, r.datpdp " &
                         "FROM tb_IR1 r, usina u " &
                         "WHERE u.codempre = '" & strEmpresa & "' " &
                         "AND u.codusina = r.codusina " &
                         "AND r.datpdp = '" & strDataPDP & "' " &
                         "ORDER BY u.ordem, r.codusina"

            Case Is = "IR2" '?
                strTitulo = "Dia -1"
                strSubTitulo = "Empresa: " & strNomecurto
                strSql = "SELECT r.codusina AS nomecurto, r.intIR2 AS intervalo, ISNULL(r.valIR2" & strCampo & ",0) AS valor, u.ordem, u.nomusina " &
                         "FROM tb_IR2 r, usina u " &
                         "WHERE u.codempre = '" & strEmpresa & "' " &
                         "AND u.codusina = r.codusina " &
                         "AND r.datpdp = '" & strDataPDP & "' " &
                         "ORDER BY r.intIR2, u.ordem, r.codusina"

            Case Is = "IR3" '?
                strTitulo = "Dia - 2"
                strSubTitulo = "Empresa: " & strNomecurto
                strSql = "SELECT r.codusina AS nomecurto, r.intIR3 AS intervalo, ISNULL(r.valIR3" & strCampo & ",0) AS valor, u.ordem, u.nomusina " &
                         "FROM tb_IR3 r, usina u " &
                         "WHERE u.codempre = '" & strEmpresa & "' " &
                         "AND u.codusina = r.codusina " &
                         "AND r.datpdp = '" & strDataPDP & "' " &
                         "ORDER BY r.intIR3, u.ordem, r.codusina"

            Case Is = "IR4" '?
                strTitulo = "Carga da Ande"
                strSubTitulo = "Empresa: " & strNomecurto
                strSql = "SELECT r.codusina AS nomecurto, r.intIR4 AS intervalo, ISNULL(r.valIR4" & strCampo & ",0) AS valor, u.ordem, u.nomusina " &
                         "FROM tb_IR4 r, usina u " &
                         "WHERE u.codempre = '" & strEmpresa & "' " &
                         "AND u.codusina = r.codusina " &
                         "AND r.datpdp = '" & strDataPDP & "' " &
                         "ORDER BY r.intIR4, u.ordem, r.codusina"
            Case Is = "tb_rro" '?
                strTitulo = "RRO"
                strSubTitulo = "Empresa: " & strNomecurto
                strSql = "SELECT r.codusina AS nomecurto, r.intrro AS intervalo, ISNULL(r.valrro" & strCampo & ",0) AS valor, u.ordem, u.nomusina " &
                         "FROM tb_rro r, usina u " &
                         "WHERE u.codempre = '" & strEmpresa & "' " &
                         "AND u.codusina = r.codusina " &
                         "AND r.datpdp = '" & strDataPDP & "' " &
                         "ORDER BY r.intrro, u.ordem, r.codusina"
        End Select

        If strDataPDP <> "0" Then
            If strTabela = "VAZ" And strDataPDPFim <> 0 Then
                strTitulo = strTitulo & " ref. a Programação dos dias " & strDataPDP.Substring(6, 2) & "/" & strDataPDP.Substring(4, 2) & "/" & strDataPDP.Substring(0, 4) & " a " & strDataPDPFim.Substring(6, 2) & "/" & strDataPDPFim.Substring(4, 2) & "/" & strDataPDPFim.Substring(0, 4)
            Else
                strTitulo = strTitulo & " ref. a Programação do dia " & strDataPDP.Substring(6, 2) & "/" & strDataPDP.Substring(4, 2) & "/" & strDataPDP.Substring(0, 4)
            End If
        End If
        Dim objDA As New SqlDataAdapter(strSql, objConn)
        dtsRelatorio = New DataSet
        objDA.Fill(dtsRelatorio, "Relatorio")
        objConn.Close()
    End Sub

    Private Sub MostraArquivoTexto()
        Dim intI As Integer
        Dim intArq As Integer
        Dim strUrlPath As String = Request.ApplicationPath
        Dim strPath As String = Request.PhysicalApplicationPath()
        Dim strFileName, strFullPathFileName As String
        Dim strWindow As String
        Dim strWinParam As String
        Dim strLinha As String
        Dim intIntervalo, intHora As Integer
        Dim strTab As String
        Dim dblCarga, dblGeracao, dblIntercambio As Double

        'monta o caminho onde sera gravado
        strPath &= "pdpw\Temp\" & Session.SessionID + "\"
        System.IO.Directory.CreateDirectory(strPath)

        strFileName = "Relatorio.xls"
        strTab = Chr(9) & Space(1)
        strFullPathFileName = strPath & strFileName

        'Se já existir um arquivo excluo e crio outro novo.
        If System.IO.File.Exists(strFullPathFileName) Then
            System.IO.File.Delete(strFullPathFileName)
        End If

        'Abre o arquivo texto
        intArq = FreeFile()
        FileOpen(intArq, strFullPathFileName, OpenMode.Output, OpenShare.Default)


        'Gravação do arquivo texto
        PrintLine(intArq, strTitulo)
        PrintLine(intArq, strSubTitulo)

        If strTabela <> "MAN" And strTabela <> "PCO" And strTabela <> "PGA" And strTabela <> "RES" And strTabela <> "VAZ" And strTabela <> "IR1" Then
            strLinha = "Intervalo"
            intIntervalo = dtsRelatorio.Tables("Relatorio").Rows(0).Item("intervalo")
            For intI = 0 To dtsRelatorio.Tables("Relatorio").Rows.Count - 1
                If intIntervalo <> dtsRelatorio.Tables("Relatorio").Rows(intI).Item("intervalo") Then
                    Exit For
                Else

                    If Page.Request.QueryString("strAcesso") <> "PDOC" Then
                        strLinha &= (strTab & Trim(dtsRelatorio.Tables("Relatorio").Rows(intI).Item("nomecurto")))
                    Else
                        Try
                            strLinha &= (strTab & Trim(dtsRelatorio.Tables("Relatorio").Rows(intI).Item("nomusina")))
                        Catch
                            strLinha &= (strTab & Trim(dtsRelatorio.Tables("Relatorio").Rows(intI).Item("nomecurto")))
                        End Try
                    End If

                End If
            Next
            If strTabela = "balanco" Then
                strLinha &= (strTab & "Fechamento")
            End If
            PrintLine(intArq, strLinha)
            strLinha = ""
            intIntervalo = 0
            intHora = 0
            For intI = 0 To dtsRelatorio.Tables("Relatorio").Rows.Count - 1
                With dtsRelatorio.Tables("Relatorio").Rows(intI)
                    If intIntervalo <> .Item("intervalo") Then
                        If strLinha <> "" Then
                            If strTabela = "balanco" Then
                                'Somente para a opção balanço
                                strLinha &= strTab & Format((dblCarga + dblIntercambio) - dblGeracao, "######0.00")
                            End If
                            PrintLine(intArq, strLinha)
                        End If
                        If .Item("intervalo") Mod 2 <> 0 Then
                            strLinha = Format(intHora, "00") & ":30"
                            intHora = intHora + 1
                        Else
                            strLinha = Format(intHora, "00") & ":00"
                        End If
                        intIntervalo = .Item("intervalo")
                    End If
                    strLinha &= strTab & Format(.Item("valor"), "######0.00")
                    If strTabela = "balanco" Then
                        Select Case .Item("nomecurto").ToString.Trim
                            Case Is = "Geração"
                                dblGeracao = .Item("valor")
                            Case Is = "Carga"
                                dblCarga = .Item("valor")
                            Case Is = "Intercâmbio"
                                dblIntercambio = .Item("valor")
                        End Select
                    End If
                End With
            Next
            If strTabela = "balanco" Then
                'Somente para a opção balanço
                strLinha &= strTab & Format((dblCarga + dblIntercambio) - dblGeracao, "######0.00")
            End If
            PrintLine(intArq, strLinha)
        ElseIf strTabela = "PGA" Then
            'Manutenções e restrições
            strLinha = "Patamar" & strTab & "Início" & strTab & "Fim" & strTab & "Nivel" & strTab &
                       "Usina" & strTab & "valor"
            PrintLine(intArq, strLinha)
            For intI = 0 To dtsRelatorio.Tables("Relatorio").Rows.Count - 1
                With dtsRelatorio.Tables("Relatorio").Rows(intI)
                    strLinha = .Item("ipat")
                    strLinha &= strTab & .Item("patini")
                    strLinha &= strTab & .Item("patfim")
                    strLinha &= strTab & .Item("nivcarga")
                    If Page.Request.QueryString("strAcesso") <> "PDOC" Then
                        strLinha &= strTab & .Item("codusina")
                    Else
                        strLinha &= strTab & .Item("nomusina")
                    End If
                    strLinha &= strTab & .Item("valor")
                    PrintLine(intArq, strLinha)
                End With
            Next
        ElseIf strTabela = "VAZ" Then
            If Request.QueryString("strDataPDPFim") <> 0 Then
                strLinha = "Data" & strTab & "Usina"
            Else
                strLinha = "Empresa"
            End If
            strLinha &= strTab & "Vlr Turbinada" & strTab & "Vlr Vertida" & strTab & "Vlr Afluente" & strTab & "Vlr Cota Ini" & strTab & "Vlr Cota Fim" & strTab & "Outras Estr" & strTab & "Comentario PDF"
            PrintLine(intArq, strLinha)
            For intI = 0 To dtsRelatorio.Tables("Relatorio").Rows.Count - 1
                With dtsRelatorio.Tables("Relatorio").Rows(intI)
                    If Request.QueryString("strDataPDPFim") <> 0 Then
                        strLinha = .Item("datpdp").ToString.Substring(6, 2) & "/" & .Item("datpdp").ToString.Substring(4, 2) & "/" & .Item("datpdp").ToString.Substring(0, 4)

                        If Page.Request.QueryString("strAcesso") <> "PDOC" Then
                            strLinha &= strTab & .Item("codusina")
                        Else
                            strLinha &= strTab & .Item("nomusina")
                        End If
                    Else
                        strLinha = .Item("sigempre")
                    End If
                    strLinha &= strTab & .Item("valturb")
                    strLinha &= strTab & .Item("valvert")
                    strLinha &= strTab & .Item("valaflu")
                    strLinha &= strTab & .Item("valcota")
                    strLinha &= strTab & .Item("valcotaf")
                    strLinha &= strTab & .Item("outrasestr")
                    strLinha &= strTab & .Item("comentpdf")
                    PrintLine(intArq, strLinha)
                End With
            Next
        Else
            'Manutenções e restrições
            strLinha = "Codigo" & strTab & "Empresa" & strTab & "Usina" & strTab & "UG" & strTab & _
                       "Início" & strTab & "Interv. Inicial" & strTab & "Fim" & strTab & _
                       "Interv. Final"
            If strTabela = "MAN" Then
                strLinha &= strTab & "Retorno Inícial" & strTab & "Retorno Final" & strTab & "Nível" & strTab & "Regine"
            ElseIf strTabela = "RES" Then
                strLinha &= strTab & "Valor" & strTab & "Motivo Restrição" '& strTab & "Observação"
            End If
            PrintLine(intArq, strLinha)
            For intI = 0 To dtsRelatorio.Tables("Relatorio").Rows.Count - 1
                With dtsRelatorio.Tables("Relatorio").Rows(intI)
                    If strTabela = "RES" Then
                        strLinha = .Item("codrestr")
                        strLinha &= strTab & .Item("codempre")
                        If Page.Request.QueryString("strAcesso") <> "PDOC" Then
                            strLinha &= strTab & .Item("codusina")
                        Else
                            strLinha &= strTab & .Item("nomusina")
                        End If
                        strLinha &= strTab & .Item("siggerad")
                        strLinha &= strTab & .Item("datinirestr").ToString.Substring(6, 2) & "/" & .Item("datinirestr").ToString.Substring(4, 2) & "/" & .Item("datinirestr").ToString.Substring(0, 4)
                        strLinha &= strTab & .Item("intinirestr")
                        strLinha &= strTab & .Item("datfimrestr").ToString.Substring(6, 2) & "/" & .Item("datfimrestr").ToString.Substring(4, 2) & "/" & .Item("datfimrestr").ToString.Substring(0, 4)
                        strLinha &= strTab & .Item("intfimrestr")
                        strLinha &= strTab & .Item("valrestr")
                        strLinha &= strTab & .Item("dsc_motivorestricao")
                        'strLinha &= strTab & .Item("obsrestr")
                    ElseIf strTabela = "IR1" Then
                        strLinha = .Item("nomusina")
                        strLinha &= strTab & .Item("nomusina")
                        strLinha &= strTab & .Item("datpdp").ToString.Substring(6, 2) & "/" & .Item("datpdp").ToString.Substring(4, 2) & "/" & .Item("datpdp").ToString.Substring(0, 4)
                        strLinha &= strTab & .Item("valor")


                    Else
                        strLinha = .Item("codparal")
                        strLinha &= strTab & .Item("codempre")
                        If Page.Request.QueryString("strAcesso") <> "PDOC" Then
                            strLinha &= strTab & .Item("codusina")
                        Else
                            strLinha &= strTab & .Item("nomusina")
                        End If
                        strLinha &= strTab & .Item("siggerad")
                        strLinha &= strTab & .Item("datiniparal").ToString.Substring(6, 2) & "/" & .Item("datiniparal").ToString.Substring(4, 2) & "/" & .Item("datiniparal").ToString.Substring(0, 4)
                        strLinha &= strTab & .Item("intiniparal")
                        strLinha &= strTab & .Item("datfimparal").ToString.Substring(6, 2) & "/" & .Item("datfimparal").ToString.Substring(4, 2) & "/" & .Item("datfimparal").ToString.Substring(0, 4)
                        strLinha &= strTab & .Item("intfimparal")
                        If strTabela = "MAN" Then
                            strLinha &= strTab & .Item("intinivoltaparal")
                            strLinha &= strTab & .Item("intfimvoltaparal")
                            strLinha &= strTab & .Item("codnivel")
                            strLinha &= strTab & .Item("indcont")
                        End If
                    End If
                    PrintLine(intArq, strLinha)
                End With
            Next
        End If

        'Salva e Fecha o arquivo texto
        FileClose(intArq)

        'Exibir o relatório em uma nova janela
        strWinParam = "height=600,width=800,toolbar=yes,location=no,status=no,menubar=yes,scrollbars=yes,resizable=yes"
        'strWindow = "viewframe"  -> Coloca o arquivo no frame dentro da mesma janela
        'strWindow = "NewWindow"  -> Abre nova janela
        strWindow = ""
        strUrlPath += "/pdpw/Temp/" & Session.SessionID.ToString() + "/" & strFileName
        Redireciona(strUrlPath, Me, strWindow, strWinParam)

    End Sub

End Class
