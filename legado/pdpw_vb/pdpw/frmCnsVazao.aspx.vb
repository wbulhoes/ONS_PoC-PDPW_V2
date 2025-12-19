Imports System.Data.SqlClient

Partial Class frmCnsVazao
    Inherits System.Web.UI.Page
    Private strBase As String

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
        'objMenu = CType(Session("onsmenu"), OnsWebControls.OnsMenu)
        imgPlanilha.Visible = False
        If Page.Request.QueryString("strAcesso") = "PDOC" Then
            optDados.Visible = False
            optDados.SelectedIndex = 1
            strBase = "rpdp"
            lblData.Text = "Data do PDP"
        Else
            strBase = "pdp"
            lblData.Text = "Data do PDP"
        End If

        If Not Page.IsPostBack Then
            Dim intI As Integer
            Dim Conn As SqlConnection = New SqlConnection
            Conn.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
            Dim Cmd As SqlCommand = New SqlCommand
            Cmd.Connection = Conn
            Try
                Cmd.CommandText = "SELECT codempre, sigempre " &
                                  "FROM empre " &
                                  "ORDER BY sigempre"
                Conn.Open()
                Dim rsEmpresa As SqlDataReader = Cmd.ExecuteReader
                intI = 1
                Dim objItem As System.Web.UI.WebControls.ListItem
                objItem = New System.Web.UI.WebControls.ListItem
                objItem.Text = ""
                objItem.Value = "0"
                cboEmpresa.Items.Add(objItem)
                Do While rsEmpresa.Read
                    objItem = New System.Web.UI.WebControls.ListItem
                    objItem.Text = rsEmpresa.Item("sigempre")
                    objItem.Value = rsEmpresa.Item("codempre")
                    cboEmpresa.Items.Add(objItem)
                    If Trim(cboEmpresa.Items(intI).Value) = Trim(Session("strCodEmpre")) Then
                        cboEmpresa.SelectedIndex = intI
                    End If
                    intI = intI + 1
                Loop
                rsEmpresa.Close()
                rsEmpresa = Nothing
                Cmd.CommandText = "SELECT datpdp " &
                                  "FROM pdp " & GeraClausulaWHERE_DataPDP_PDO(Page.Request.QueryString("strAcesso")) &
                                  "ORDER BY datpdp DESC"
                Dim rsData As SqlDataReader = Cmd.ExecuteReader
                intI = 1
                objItem = New System.Web.UI.WebControls.ListItem
                objItem.Text = ""
                objItem.Value = "0"
                cboDataInicial.Items.Add(objItem)
                cboDataFinal.Items.Add(objItem)
                Do While rsData.Read
                    objItem = New System.Web.UI.WebControls.ListItem
                    objItem.Text = Mid(rsData("datpdp"), 7, 2) & "/" & Mid(rsData("datpdp"), 5, 2) & "/" & Mid(rsData("datpdp"), 1, 4)
                    objItem.Value = rsData("datpdp")
                    cboDataInicial.Items.Add(objItem)
                    cboDataFinal.Items.Add(objItem)
                    If Trim(cboDataInicial.Items(intI).Value) = Format(Session("datEscolhida"), "yyyyMMdd") Then
                        cboDataInicial.SelectedIndex = intI
                        cboDataFinal.SelectedIndex = intI
                    End If
                    intI = intI + 1
                Loop
                rsData.Close()
                rsData = Nothing

                'Cmd.Connection.Close()
                'Conn.Close()
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
        End If
    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        If SessaoAtiva(Page.Session) Then
            MyBase.Render(writer)
            'objMenu.RenderControl(writer)
        End If
    End Sub

    Private Sub optVisualiza_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optVisualiza.SelectedIndexChanged
        Select Case optVisualiza.SelectedItem.Value
            Case Is = 0
                'Data PDP
                lblLetra.Visible = True
                cboDataFinal.Visible = True
                cboEmpresa.Enabled = True
            Case Is = 1
                'Empresa
                cboEmpresa.Enabled = False
                cboDataFinal.Visible = False
                cboDataFinal.SelectedIndex = 0
                lblLetra.Visible = False
        End Select
    End Sub

    Private Sub ConsultaData(ByVal strcampo As String)
        Dim Conn As System.Data.SqlClient.SqlConnection = New System.Data.SqlClient.SqlConnection
        Conn.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
        Dim Cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand
        Cmd.Connection = Conn
        Dim objRow As TableRow
        Dim objCell As TableCell
        Dim intI, intJ, intQtdReg, intTamanho, intTotTurb, intTotVert, intTotAflu, intTotTransf As Integer
        Dim dblTotCota, dblTotCotaf, dblTotOutrasEstr As Double
        Dim strCodUsina As String
        Try
            'data pdp
            Cmd.CommandText = "SELECT datpdp " &
                              "FROM vazao v, usina u " &
                              "WHERE u.codempre = '" & cboEmpresa.SelectedItem.Value & "' " &
                              "AND u.codusina = v.codusina " &
                              "AND v.datpdp >= '" & cboDataInicial.SelectedItem.Value & "' " &
                              "AND v.datpdp <= '" & cboDataFinal.SelectedItem.Value & "' " &
                              "AND u.flg_recebepdpage = 'S' " &
                              "GROUP BY datpdp " &
                              "ORDER BY datpdp"
            Conn.Open()
            Dim rsConsulta As System.Data.SqlClient.SqlDataReader = Cmd.ExecuteReader
            Dim objTamanho As System.Web.UI.WebControls.Unit
            objTamanho = New Unit

            'Colocar cor na tabela (RGB(233, 244, 207) = cor padrão ONS)
            Dim Color As System.Drawing.Color
            Color = New System.Drawing.Color
            Color = System.Drawing.ColorTranslator.FromWin32(RGB(233, 244, 207))
            intI = 0
            tblConsulta.Width = objTamanho.Pixel(300)  'objTamanho.Pixel(240) -- CRQ2345 (15/08/2012)
            Do While rsConsulta.Read
                If intI = 0 Then
                    objRow = New TableRow
                    objRow.Font.Bold = True
                    objRow.BackColor = System.Drawing.Color.YellowGreen
                    objCell = New TableCell
                    objCell.Width = objTamanho.Pixel(100)
                    objCell.RowSpan = 2
                    objCell.Text = "Data PDP"
                    objCell.BorderWidth = 1
                    objRow.Controls.Add(objCell)
                    objCell = New TableCell
                    objCell.Width = objTamanho.Pixel(140)
                    objCell.ColumnSpan = 7
                    objCell.HorizontalAlign = HorizontalAlign.Center
                    objCell.Text = "Total"
                    objCell.BorderWidth = 1
                    objRow.Controls.Add(objCell)
                    tblConsulta.Rows.Add(objRow)

                    objRow = New TableRow
                    objRow.Font.Bold = True
                    objRow.BackColor = System.Drawing.Color.YellowGreen
                    objCell = New TableCell
                    objCell.BorderWidth = 1
                    objCell.Width = objTamanho.Pixel(70)
                    objCell.Text = "Turbinada"
                    objRow.Controls.Add(objCell)
                    objCell = New TableCell
                    objCell.BorderWidth = 1
                    objCell.Width = objTamanho.Pixel(70)
                    objCell.Text = "Vertida"
                    objRow.Controls.Add(objCell)
                    tblConsulta.Rows.Add(objRow)
                    objCell = New TableCell
                    objCell.BorderWidth = 1
                    objCell.Width = objTamanho.Pixel(70)
                    objCell.Text = "Afluente"
                    objRow.Controls.Add(objCell)
                    tblConsulta.Rows.Add(objRow)
                    objCell = New TableCell
                    objCell.BorderWidth = 1
                    objCell.Width = objTamanho.Pixel(70)
                    objCell.Text = "Cota Inicial"
                    objCell.Wrap = False
                    objRow.Controls.Add(objCell)
                    tblConsulta.Rows.Add(objRow)
                    '-- CRQ2345 (15/08/2012)
                    objCell = New TableCell
                    objCell.BorderWidth = 1
                    objCell.Width = objTamanho.Pixel(70)
                    objCell.Text = "Cota Final"
                    objCell.Wrap = False
                    objRow.Controls.Add(objCell)
                    tblConsulta.Rows.Add(objRow)
                    objCell = New TableCell
                    objCell.BorderWidth = 1
                    objCell.Width = objTamanho.Pixel(70)
                    objCell.Text = "Outras Estr"
                    objCell.Wrap = False
                    objRow.Controls.Add(objCell)
                    tblConsulta.Rows.Add(objRow)
                    '--WI5072 (02/10/2018)
                    objCell = New TableCell
                    objCell.BorderWidth = 1
                    objCell.Width = objTamanho.Pixel(70)
                    objCell.Text = "Transferida"
                    objCell.Wrap = False
                    objRow.Controls.Add(objCell)
                    tblConsulta.Rows.Add(objRow)

                    intI = 2
                End If

                objRow = New TableRow
                If intI Mod 2 <> 0 Then
                    'quando linha = par troca cor
                    objRow.BackColor = Color
                End If
                objCell = New TableCell
                objCell.Wrap = False
                objCell.Text = Mid(rsConsulta("datpdp"), 7, 2) & "/" &
                            Mid(rsConsulta("datpdp"), 5, 2) & "/" &
                            Mid(rsConsulta("datpdp"), 1, 4)
                objRow.Controls.Add(objCell)
                objCell = New TableCell
                objRow.Controls.Add(objCell)
                tblConsulta.Rows.Add(objRow)
                objCell = New TableCell
                objRow.Controls.Add(objCell)
                objCell = New TableCell
                objRow.Controls.Add(objCell)
                tblConsulta.Rows.Add(objRow)
                objCell = New TableCell
                objRow.Controls.Add(objCell)
                '-- CRQ2345 (15/08/2012)
                objCell = New TableCell
                objRow.Controls.Add(objCell)
                tblConsulta.Rows.Add(objRow)
                objCell = New TableCell
                objRow.Controls.Add(objCell)
                tblConsulta.Rows.Add(objRow)
                '--WI5072 (02/10/2018)
                objCell = New TableCell
                objRow.Controls.Add(objCell)
                tblConsulta.Rows.Add(objRow)


                intI = intI + 1
            Loop
            intQtdReg = intI - 1
            If intI > 0 Then
                objRow = New TableRow
                If intI Mod 2 <> 0 Then
                    'quando linha = par troca cor
                    objRow.BackColor = Color
                End If
                objCell = New TableCell
                objCell.Text = "Total"
                objCell.Font.Bold = True
                objRow.Controls.Add(objCell)

                objCell = New TableCell
                objCell.Font.Bold = True
                objRow.Controls.Add(objCell)

                objCell = New TableCell
                objCell.Font.Bold = True
                objRow.Controls.Add(objCell)

                objCell = New TableCell
                objCell.Font.Bold = True
                objRow.Controls.Add(objCell)

                objCell = New TableCell
                objCell.Font.Bold = True
                objRow.Controls.Add(objCell)

                '-- CRQ2345 (15/08/2012)
                objCell = New TableCell
                objCell.Font.Bold = True
                objRow.Controls.Add(objCell)

                objCell = New TableCell
                objCell.Font.Bold = True
                objRow.Controls.Add(objCell)

                '--WI5072 (02/10/2018)
                objCell = New TableCell
                objCell.Font.Bold = True
                objRow.Controls.Add(objCell)

                tblConsulta.Controls.Add(objRow)
                intI = intI + 1
                objRow = New TableRow
                If intI Mod 2 <> 0 Then
                    'quando linha = par troca cor
                    objRow.BackColor = Color
                End If
                objCell = New TableCell
                objCell.Text = "Média"
                objCell.Font.Bold = True
                objRow.Controls.Add(objCell)

                objCell = New TableCell
                objCell.Font.Bold = True
                objRow.Controls.Add(objCell)

                objCell = New TableCell
                objCell.Font.Bold = True
                objRow.Controls.Add(objCell)

                objCell = New TableCell
                objCell.Font.Bold = True
                objRow.Controls.Add(objCell)

                objCell = New TableCell
                objCell.Font.Bold = True
                objRow.Controls.Add(objCell)

                '-- CRQ2345 (15/08/2012)				
                objCell = New TableCell
                objCell.Font.Bold = True
                objRow.Controls.Add(objCell)

                objCell = New TableCell
                objCell.Font.Bold = True
                objRow.Controls.Add(objCell)

                '--WI5072 (02/10/2018)
                objCell = New TableCell
                objCell.Font.Bold = True
                objRow.Controls.Add(objCell)

                tblConsulta.Controls.Add(objRow)
            End If
            rsConsulta.Close()

            '-- CRQ2345 (15/08/2012)
            '--WI5072 (02/10/2018)
            Cmd.CommandText = "SELECT v.datpdp, v.codusina, v.valturb" & strcampo & " AS valturb, v.valvert" & strcampo &
                              " AS valvert, v.valaflu" & strcampo & " AS valaflu, " &
                              "v.valtransf" & strcampo & " AS valtransf, c.cotaini" & strcampo &
                              " AS valcota, c.cotafim" & strcampo & " AS valcotaf, c.outrasestruturas" & strcampo &
                              " AS valoutrasestr, u.ordem, u.nomusina " &
                              "FROM vazao v INNER JOIN usina u ON u.codusina = v.codusina " &
                              "LEFT OUTER JOIN cota c ON u.codusina = c.codusina AND v.datpdp = c.datpdp " &
                              "WHERE u.codempre = '" & cboEmpresa.SelectedItem.Value & "' " &
                              "AND v.datpdp >= '" & cboDataInicial.SelectedItem.Value & "' " &
                              "AND v.datpdp <= '" & cboDataFinal.SelectedItem.Value & "' " &
                              "AND u.flg_recebepdpage = 'S' " &
                              "ORDER BY u.ordem, v.codusina, v.datpdp"

            rsConsulta = Cmd.ExecuteReader
            intI = 0
            intTotTurb = 0
            intTotVert = 0
            intTotAflu = 0
            dblTotCota = 0
            '-- CRQ2345 (15/08/2012)
            dblTotCotaf = 0
            dblTotOutrasEstr = 0
            '--WI5072 (02/10/2018)
            intTotTransf = 0
            strCodUsina = ""
            intTamanho = 380

            Dim strUsina As String = ""

            Do While rsConsulta.Read

                If Page.Request.QueryString("strAcesso") <> "PDOC" Then
                    strUsina = rsConsulta("codusina")
                Else
                    strUsina = rsConsulta("nomusina")
                End If

                If strCodUsina <> rsConsulta("codusina") Then
                    If intI > 0 And intI < tblConsulta.Rows.Count Then
                        Do While intI <= intQtdReg
                            objCell = New TableCell
                            tblConsulta.Rows(intI).Controls.Add(objCell)
                            objCell = New TableCell
                            tblConsulta.Rows(intI).Controls.Add(objCell)
                            objCell = New TableCell
                            tblConsulta.Rows(intI).Controls.Add(objCell)
                            objCell = New TableCell
                            tblConsulta.Rows(intI).Controls.Add(objCell)
                            '-- CRQ2345 (15/08/2012)							
                            objCell = New TableCell
                            tblConsulta.Rows(intI).Controls.Add(objCell)
                            objCell = New TableCell
                            tblConsulta.Rows(intI).Controls.Add(objCell)
                            '--WI5072 (02/10/2018)
                            objCell = New TableCell
                            tblConsulta.Rows(intI).Controls.Add(objCell)
                            intI = intI + 1
                        Loop
                        'Total Turbinada
                        objCell = New TableCell
                        objCell.Font.Bold = True
                        objCell.Text = intTotTurb
                        tblConsulta.Rows(intI).Controls.Add(objCell)
                        'Total Vertida
                        objCell = New TableCell
                        objCell.Font.Bold = True
                        objCell.Text = intTotVert
                        tblConsulta.Rows(intI).Controls.Add(objCell)
                        'Total Afluente
                        objCell = New TableCell
                        objCell.Font.Bold = True
                        objCell.Text = intTotAflu
                        tblConsulta.Rows(intI).Controls.Add(objCell)
                        'Total Cota Inicial
                        objCell = New TableCell
                        objCell.Font.Bold = True
                        objCell.Text = Format(dblTotCota, "#####0.00")
                        tblConsulta.Rows(intI).Controls.Add(objCell)
                        '-- CRQ2345 (15/08/2012)
                        'Total Cota Final
                        objCell = New TableCell
                        objCell.Font.Bold = True
                        objCell.Text = Format(dblTotCotaf, "#####0.00")
                        tblConsulta.Rows(intI).Controls.Add(objCell)
                        'Total Outras Estruturas
                        objCell = New TableCell
                        objCell.Font.Bold = True
                        objCell.Text = Format(dblTotOutrasEstr, "#####0.00")
                        tblConsulta.Rows(intI).Controls.Add(objCell)
                        '--WI5072 (02/10/2018)
                        'Total Transferida
                        objCell = New TableCell
                        objCell.Font.Bold = True
                        objCell.Text = intTotTransf
                        tblConsulta.Rows(intI).Controls.Add(objCell)
                        intI = intI + 1
                        'Média Turbinada
                        objCell = New TableCell
                        objCell.Font.Bold = True
                        objCell.Text = Int(IIf(intTotTurb = 0, intTotTurb, intTotTurb / (intI - 3)))
                        tblConsulta.Rows(intI).Controls.Add(objCell)
                        'Média Vertida
                        objCell = New TableCell
                        objCell.Font.Bold = True
                        objCell.Text = Int(IIf(intTotVert = 0, intTotVert, intTotVert / (intI - 3)))
                        tblConsulta.Rows(intI).Controls.Add(objCell)
                        'Média Afluente
                        objCell = New TableCell
                        objCell.Font.Bold = True
                        objCell.Text = Int(IIf(intTotAflu = 0, intTotAflu, intTotAflu / (intI - 3)))
                        tblConsulta.Rows(intI).Controls.Add(objCell)
                        'Média Cota Inicial
                        objCell = New TableCell
                        objCell.Font.Bold = True
                        objCell.Text = Format(IIf(dblTotCota = 0, dblTotCota, dblTotCota / (intI - 3)), "######0.00")
                        tblConsulta.Rows(intI).Controls.Add(objCell)
                        '-- CRQ2345 (15/08/2012)
                        'Média Cota Final
                        objCell = New TableCell
                        objCell.Font.Bold = True
                        objCell.Text = Format(IIf(dblTotCotaf = 0, dblTotCotaf, dblTotCotaf / (intI - 3)), "######0.00")
                        tblConsulta.Rows(intI).Controls.Add(objCell)
                        'Média Outras Estruturas
                        objCell = New TableCell
                        objCell.Font.Bold = True
                        objCell.Text = Format(IIf(dblTotOutrasEstr = 0, dblTotOutrasEstr, dblTotOutrasEstr / (intI - 3)), "######0.00")
                        tblConsulta.Rows(intI).Controls.Add(objCell)
                        '--WI5072 (02/10/2018)
                        'Média Transferida
                        objCell = New TableCell
                        objCell.Font.Bold = True
                        objCell.Text = Int(IIf(intTotTransf = 0, intTotTransf, intTotTransf / (intI - 3)))
                        tblConsulta.Rows(intI).Controls.Add(objCell)

                    End If
                    intTamanho = intTamanho + 140
                    tblConsulta.Width = objTamanho.Pixel(intTamanho)
                    objCell = New TableCell
                    objCell.Width = objTamanho.Pixel(140)
                    objCell.BorderWidth = 1

                    objCell.ColumnSpan = 7
                    objCell.HorizontalAlign = HorizontalAlign.Center
                    'objCell.Text = rsConsulta("codusina")
                    objCell.Text = strUsina
                    tblConsulta.Rows(0).Controls.Add(objCell)

                    'Turbinada
                    objCell = New TableCell
                    objCell.BorderWidth = 1
                    objCell.Width = objTamanho.Pixel(70)
                    objCell.Text = "Turbinada"
                    tblConsulta.Rows(1).Controls.Add(objCell)
                    'Vertida
                    objCell = New TableCell
                    objCell.BorderWidth = 1
                    objCell.Width = objTamanho.Pixel(70)
                    objCell.Text = "Vertida"
                    tblConsulta.Rows(1).Controls.Add(objCell)
                    'Afluente
                    objCell = New TableCell
                    objCell.BorderWidth = 1
                    objCell.Width = objTamanho.Pixel(70)
                    objCell.Text = "Afluente"
                    tblConsulta.Rows(1).Controls.Add(objCell)
                    'Cota Inicial
                    objCell = New TableCell
                    objCell.BorderWidth = 1
                    objCell.Width = objTamanho.Pixel(70)
                    objCell.Text = "Cota Inicial"
                    objCell.Wrap = False
                    tblConsulta.Rows(1).Controls.Add(objCell)
                    '-- CRQ2345 (15/08/2012)
                    'Cota Final
                    objCell = New TableCell
                    objCell.BorderWidth = 1
                    objCell.Width = objTamanho.Pixel(70)
                    objCell.Text = "Cota Final"
                    objCell.Wrap = False
                    tblConsulta.Rows(1).Controls.Add(objCell)
                    'Outras Estruturas
                    objCell = New TableCell
                    objCell.BorderWidth = 1
                    objCell.Width = objTamanho.Pixel(70)
                    objCell.Text = "Outras Estr"
                    objCell.Wrap = False
                    tblConsulta.Rows(1).Controls.Add(objCell)
                    '--WI5072 (02/10/2018)
                    'Transferida
                    objCell = New TableCell
                    objCell.BorderWidth = 1
                    objCell.Width = objTamanho.Pixel(70)
                    objCell.Text = "Transferida"
                    tblConsulta.Rows(1).Controls.Add(objCell)

                    strCodUsina = rsConsulta("codusina")
                    intTotTurb = 0
                    intTotVert = 0
                    intTotAflu = 0
                    dblTotCota = 0
                    '-- CRQ2345 (15/08/2012)
                    dblTotCotaf = 0
                    dblTotOutrasEstr = 0
                    '--WI5072 (02/10/2018)
                    intTotTransf = 0
                    intI = 2
                End If
                If intI < tblConsulta.Rows.Count Then
                    If IsDate(tblConsulta.Rows(intI).Cells(0).Text) Then
                        Do While Format(CDate(Trim(tblConsulta.Rows(intI).Cells(0).Text)), "yyyy/MM/dd") <
                                Mid(rsConsulta("datpdp"), 1, 4) & "/" &
                                Mid(rsConsulta("datpdp"), 5, 2) & "/" &
                                Mid(rsConsulta("datpdp"), 7, 2)
                            objCell = New TableCell
                            tblConsulta.Rows(intI).Controls.Add(objCell)
                            objCell = New TableCell
                            tblConsulta.Rows(intI).Controls.Add(objCell)
                            objCell = New TableCell
                            tblConsulta.Rows(intI).Controls.Add(objCell)
                            objCell = New TableCell
                            tblConsulta.Rows(intI).Controls.Add(objCell)
                            '-- CRQ2345 (15/08/2012)
                            objCell = New TableCell
                            tblConsulta.Rows(intI).Controls.Add(objCell)
                            objCell = New TableCell
                            tblConsulta.Rows(intI).Controls.Add(objCell)
                            '--WI5072 (02/10/2018)
                            objCell = New TableCell
                            tblConsulta.Rows(intI).Controls.Add(objCell)
                            intI = intI + 1
                            If intI >= tblConsulta.Rows.Count Then
                                Exit Do
                            End If
                        Loop
                    End If
                    'Turbinada
                    objCell = New TableCell
                    objCell.Wrap = False
                    objCell.Text = IIf(Not IsDBNull(rsConsulta("valturb")), rsConsulta("valturb"), 0)
                    tblConsulta.Rows(intI).Controls.Add(objCell)
                    intTotTurb = intTotTurb + objCell.Text
                    'Vertida
                    objCell = New TableCell
                    objCell.Wrap = False
                    objCell.Text = IIf(Not IsDBNull(rsConsulta("valvert")), rsConsulta("valvert"), 0)
                    tblConsulta.Rows(intI).Controls.Add(objCell)
                    intTotVert = intTotVert + objCell.Text
                    'Afluente
                    objCell = New TableCell
                    objCell.Wrap = False
                    objCell.Text = IIf(Not IsDBNull(rsConsulta("valaflu")), rsConsulta("valaflu"), 0)
                    tblConsulta.Rows(intI).Controls.Add(objCell)
                    intTotAflu = intTotAflu + objCell.Text
                    'Cota Inicial
                    objCell = New TableCell
                    objCell.Wrap = False
                    objCell.Text = Format(IIf(Not IsDBNull(rsConsulta("valcota")), rsConsulta("valcota"), 0), "######0.00")
                    tblConsulta.Rows(intI).Controls.Add(objCell)
                    dblTotCota = dblTotCota + objCell.Text
                    '-- CRQ2345 (15/08/2012)
                    'Cota Final
                    objCell = New TableCell
                    objCell.Wrap = False
                    objCell.Text = Format(IIf(Not IsDBNull(rsConsulta("valcotaf")), rsConsulta("valcotaf"), 0), "######0.00")
                    tblConsulta.Rows(intI).Controls.Add(objCell)
                    dblTotCotaf = dblTotCotaf + objCell.Text
                    'Outras Estruturas
                    objCell = New TableCell
                    objCell.Wrap = False
                    objCell.Text = Format(IIf(Not IsDBNull(rsConsulta("valoutrasestr")), rsConsulta("valoutrasestr"), 0), "######0.00")
                    tblConsulta.Rows(intI).Controls.Add(objCell)
                    dblTotOutrasEstr = dblTotOutrasEstr + objCell.Text
                    '--WI5072 (02/10/2018)
                    'Transferida
                    objCell = New TableCell
                    objCell.Wrap = False
                    objCell.Text = IIf(Not IsDBNull(rsConsulta("valtransf")), rsConsulta("valtransf"), 0)
                    tblConsulta.Rows(intI).Controls.Add(objCell)
                    intTotTransf = intTotTransf + objCell.Text

                    intI = intI + 1
                End If
            Loop

            Do While intI <= intQtdReg
                objCell = New TableCell
                tblConsulta.Rows(intI).Controls.Add(objCell)
                objCell = New TableCell
                tblConsulta.Rows(intI).Controls.Add(objCell)
                objCell = New TableCell
                tblConsulta.Rows(intI).Controls.Add(objCell)
                objCell = New TableCell
                tblConsulta.Rows(intI).Controls.Add(objCell)
                '-- CRQ2345 (15/08/2012)
                objCell = New TableCell
                tblConsulta.Rows(intI).Controls.Add(objCell)
                objCell = New TableCell
                tblConsulta.Rows(intI).Controls.Add(objCell)
                '--WI5072 (02/10/2018)
                objCell = New TableCell
                tblConsulta.Rows(intI).Controls.Add(objCell)
                intI = intI + 1
            Loop
            If intI > 0 And intI < tblConsulta.Rows.Count Then
                'Total Turbinada
                objCell = New TableCell
                objCell.Font.Bold = True
                objCell.Text = intTotTurb
                tblConsulta.Rows(intI).Controls.Add(objCell)
                'Total Vertida
                objCell = New TableCell
                objCell.Font.Bold = True
                objCell.Text = intTotVert
                tblConsulta.Rows(intI).Controls.Add(objCell)
                'Total Afluente
                objCell = New TableCell
                objCell.Font.Bold = True
                objCell.Text = intTotAflu
                tblConsulta.Rows(intI).Controls.Add(objCell)
                'Total Cota Inicial
                objCell = New TableCell
                objCell.Font.Bold = True
                objCell.Text = Format(dblTotCota, "#####0.00")
                tblConsulta.Rows(intI).Controls.Add(objCell)
                '-- CRQ2345 (15/08/2012)
                'Total Cota Final
                objCell = New TableCell
                objCell.Font.Bold = True
                objCell.Text = Format(dblTotCotaf, "#####0.00")
                tblConsulta.Rows(intI).Controls.Add(objCell)
                'Total Outras Estruturas
                objCell = New TableCell
                objCell.Font.Bold = True
                objCell.Text = Format(dblTotOutrasEstr, "#####0.00")
                tblConsulta.Rows(intI).Controls.Add(objCell)
                '--WI5072 (02/10/2018)
                'Total Transferida
                objCell = New TableCell
                objCell.Font.Bold = True
                objCell.Text = intTotTransf
                tblConsulta.Rows(intI).Controls.Add(objCell)
                intI = intI + 1
                'Média Turbinada
                objCell = New TableCell
                objCell.Font.Bold = True
                objCell.Text = Int(IIf(intTotTurb = 0, intTotTurb, intTotTurb / (intI - 3)))
                tblConsulta.Rows(intI).Controls.Add(objCell)
                'Média Vertida
                objCell = New TableCell
                objCell.Font.Bold = True
                objCell.Text = Int(IIf(intTotVert = 0, intTotVert, intTotVert / (intI - 3)))
                tblConsulta.Rows(intI).Controls.Add(objCell)
                'Média Afluente
                objCell = New TableCell
                objCell.Font.Bold = True
                objCell.Text = Int(IIf(intTotAflu = 0, intTotAflu, intTotAflu / (intI - 3)))
                tblConsulta.Rows(intI).Controls.Add(objCell)
                'Média Cota Inicial
                objCell = New TableCell
                objCell.Font.Bold = True
                objCell.Text = Format(IIf(dblTotCota = 0, dblTotCota, dblTotCota / (intI - 3)), "######0.00")
                tblConsulta.Rows(intI).Controls.Add(objCell)
                '-- CRQ2345 (15/08/2012)
                'Média Cota Final
                objCell = New TableCell
                objCell.Font.Bold = True
                objCell.Text = Format(IIf(dblTotCotaf = 0, dblTotCotaf, dblTotCotaf / (intI - 3)), "######0.00")
                tblConsulta.Rows(intI).Controls.Add(objCell)
                'Média Outras Estruturas
                objCell = New TableCell
                objCell.Font.Bold = True
                objCell.Text = Format(IIf(dblTotOutrasEstr = 0, dblTotOutrasEstr, dblTotOutrasEstr / (intI - 3)), "######0.00")
                tblConsulta.Rows(intI).Controls.Add(objCell)
                '--WI5072 (02/10/2018)
                'Média Transferida
                objCell = New TableCell
                objCell.Font.Bold = True
                objCell.Text = Int(IIf(intTotTransf = 0, intTotTransf, intTotTransf / (intI - 3)))
                tblConsulta.Rows(intI).Controls.Add(objCell)
            End If
            rsConsulta.Close()
            Conn.Close()

            intTotTurb = 0
            intTotVert = 0
            intTotAflu = 0
            dblTotCota = 0
            '-- CRQ2345 (15/08/2012)
            dblTotCotaf = 0
            dblTotOutrasEstr = 0
            '--WI5072 (02/10/2018)
            intTotTransf = 0
            Dim intTotTurbT As Integer = 0
            Dim intTotVertT As Integer = 0
            Dim intTotAfluT As Integer = 0
            Dim dblTotCotaT As Double = 0
            '-- CRQ2345 (15/08/2012)
            Dim dblTotCotafT As Double = 0
            Dim dblTotOutrasEstrT As Double = 0
            '--WI5072 (02/10/2018)
            Dim intTotTransfT As Integer = 0
            For intI = 2 To tblConsulta.Rows.Count - 3
                For intJ = 8 To tblConsulta.Rows(intI).Cells.Count - 1 Step 7
                    intTotTurb = intTotTurb + IIf(tblConsulta.Rows(intI).Cells(intJ).Text <> "", tblConsulta.Rows(intI).Cells(intJ).Text, 0)
                    intTotVert = intTotVert + IIf(tblConsulta.Rows(intI).Cells(intJ + 1).Text <> "", tblConsulta.Rows(intI).Cells(intJ + 1).Text, 0)
                    intTotAflu = intTotAflu + IIf(tblConsulta.Rows(intI).Cells(intJ + 2).Text <> "", tblConsulta.Rows(intI).Cells(intJ + 2).Text, 0)
                    dblTotCota = dblTotCota + IIf(tblConsulta.Rows(intI).Cells(intJ + 3).Text <> "", tblConsulta.Rows(intI).Cells(intJ + 3).Text, 0)
                    '-- CRQ2345 (15/08/2012)
                    dblTotCotaf = dblTotCotaf + IIf(tblConsulta.Rows(intI).Cells(intJ + 4).Text <> "", tblConsulta.Rows(intI).Cells(intJ + 4).Text, 0)
                    dblTotOutrasEstr = dblTotOutrasEstr + IIf(tblConsulta.Rows(intI).Cells(intJ + 5).Text <> "", tblConsulta.Rows(intI).Cells(intJ + 5).Text, 0)
                    '--WI5072 (02/10/2018)
                    intTotTransf = intTotTransf + IIf(tblConsulta.Rows(intI).Cells(intJ + 6).Text <> "", tblConsulta.Rows(intI).Cells(intJ + 6).Text, 0)
                Next
                tblConsulta.Rows(intI).Cells(1).Text = intTotTurb
                tblConsulta.Rows(intI).Cells(2).Text = intTotVert
                tblConsulta.Rows(intI).Cells(3).Text = intTotAflu
                tblConsulta.Rows(intI).Cells(4).Text = Format(dblTotCota, "#####0.00")
                '-- CRQ2345 (15/08/2012)
                tblConsulta.Rows(intI).Cells(5).Text = Format(dblTotCotaf, "#####0.00")
                tblConsulta.Rows(intI).Cells(6).Text = Format(dblTotOutrasEstr, "#####0.00")
                '--WI5072 (02/10/2018)
                tblConsulta.Rows(intI).Cells(7).Text = intTotTransf
                intTotTurbT += intTotTurb
                intTotVertT += intTotVert
                intTotAfluT += intTotAflu
                dblTotCotaT += dblTotCota
                '-- CRQ2345 (15/08/2012)
                dblTotCotafT += dblTotCotaf
                dblTotOutrasEstrT += dblTotOutrasEstr
                '--WI5072 (02/10/2018)
                intTotTransfT += intTotTransf
                intTotTurb = 0
                intTotVert = 0
                intTotAflu = 0
                dblTotCota = 0
                '-- CRQ2345 (15/08/2012)
                dblTotCotaf = 0
                dblTotOutrasEstr = 0
                '--WI5072 (02/10/2018)
                intTotTransf = 0
            Next
            tblConsulta.Rows(intI).Cells(1).Text = intTotTurbT
            tblConsulta.Rows(intI).Cells(2).Text = intTotVertT
            tblConsulta.Rows(intI).Cells(3).Text = intTotAfluT
            tblConsulta.Rows(intI).Cells(4).Text = Format(dblTotCotaT, "######0.00")
            '-- CRQ2345 (15/08/2012)
            tblConsulta.Rows(intI).Cells(5).Text = Format(dblTotCotafT, "######0.00")
            tblConsulta.Rows(intI).Cells(6).Text = Format(dblTotOutrasEstrT, "######0.00")
            '--WI5072 (02/10/2018)
            tblConsulta.Rows(intI).Cells(7).Text = intTotTransfT

            tblConsulta.Rows(intI + 1).Cells(1).Text = Int(intTotTurbT / (intI - 2))
            tblConsulta.Rows(intI + 1).Cells(2).Text = Int(intTotVertT / (intI - 2))
            tblConsulta.Rows(intI + 1).Cells(3).Text = Int(intTotAfluT / (intI - 2))
            tblConsulta.Rows(intI + 1).Cells(4).Text = Format(dblTotCotaT / (intI - 2), "######0.00")
            '-- CRQ2345 (15/08/2012)
            tblConsulta.Rows(intI + 1).Cells(5).Text = Format(dblTotCotafT / (intI - 2), "######0.00")
            tblConsulta.Rows(intI + 1).Cells(6).Text = Format(dblTotOutrasEstrT / (intI - 2), "######0.00")
            '--WI5072 (02/10/2018)
            tblConsulta.Rows(intI + 1).Cells(7).Text = Int(intTotTransfT / (intI - 2))
        Catch ex As Exception
            '
            ' WI 146628 - As críticas não estavam funcionando.
            '
            If cboDataInicial.SelectedItem.Value.Trim() = "0" Or cboDataFinal.SelectedItem.Value.Trim() = "0" Then
                Session("strMensagem") = "As duas datas têm de ser preenchidas, mesmo que sejam iguais."
            ElseIf cboEmpresa.SelectedItem.Value.Trim() = "0" Then
                Session("strMensagem") = "O nome do agente deve ser escolhido."
            Else
                Session("strMensagem") = "Não foi possível acessar a Base de Dados."
            End If

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

    Private Sub ConsultaEmpresa(ByVal strcampo As String)
        Dim Conn As System.Data.SqlClient.SqlConnection = New System.Data.SqlClient.SqlConnection
        Conn.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
        Dim Cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand
        Cmd.Connection = Conn
        Dim objRow As TableRow
        Dim objCell As TableCell
        Dim intI, intCol, intTotTurb, intTotVert, intTotAflu, intTotTransf As Integer
        Dim dblTotCota, dblTotCotaf, dblTotOutrasEstr As Double
        Try
            '-- CRQ2345 (15/08/2012)
            '--WI5072 (02/10/2018)
            Cmd.CommandText = "SELECT e.sigempre, SUM(v.valturb" & strcampo & ") AS valturb, " &
                              "SUM(v.valvert" & strcampo & ") AS valvert, SUM(v.valaflu" & strcampo & ") AS valaflu, " &
                              "SUM(v.valtransf" & strcampo & ") AS valtransf, SUM(c.cotaini" & strcampo & ") AS valcota, " &
                              "SUM(c.cotafim" & strcampo & ") AS valcotaf, " &
                              "SUM(c.outrasestruturas" & strcampo & ") AS valoutrasestr " &
                              "FROM vazao v INNER JOIN usina u ON u.codusina = v.codusina INNER JOIN empre e ON u.codempre = e.codempre " &
                              "LEFT JOIN cota c ON u.codusina = c.codusina " &
                              "WHERE v.datpdp = '" & cboDataInicial.SelectedValue & "' " &
                              "AND u.tipusina = 'H'" &
                              "AND u.flg_recebepdpage = 'S'" &
                              "AND c.datpdp = '" & cboDataInicial.SelectedValue & "' " &
                              "GROUP BY e.sigempre " &
                              "ORDER BY e.sigempre;"

            Conn.Open()
            Dim rsConsulta As System.Data.SqlClient.SqlDataReader = Cmd.ExecuteReader
            intI = 0
            Dim objTamanho As System.Web.UI.WebControls.Unit
            objTamanho = New Unit

            'Colocar cor na tabela (RGB(233, 244, 207) = cor padrão ONS)
            Dim Color As System.Drawing.Color
            Color = New System.Drawing.Color
            Color = System.Drawing.ColorTranslator.FromWin32(RGB(233, 244, 207))

            tblConsulta.Width = objTamanho.Pixel(400)
            Do While rsConsulta.Read
                If intI = 0 Then
                    objRow = New TableRow
                    objRow.BackColor = System.Drawing.Color.YellowGreen
                    For intCol = 1 To 8
                        'nova Celula
                        objCell = New TableCell
                        objCell.Font.Bold = True
                        Select Case intCol
                            Case Is = 1
                                objCell.Width = objTamanho.Pixel(120)
                                objCell.Text = "Empresa"
                            Case Is = 2
                                objCell.Width = objTamanho.Pixel(70)
                                objCell.Text = "Turbinada"
                            Case Is = 3
                                objCell.Width = objTamanho.Pixel(70)
                                objCell.Text = "Vertida"
                            Case Is = 4
                                objCell.Width = objTamanho.Pixel(70)
                                objCell.Text = "Afluente"
                            Case Is = 5
                                objCell.Width = objTamanho.Pixel(70)
                                objCell.Text = "Cota Inicial"
                                objCell.Wrap = False
                            '-- CRQ2345 (15/08/2012)
                            Case Is = 6
                                objCell.Width = objTamanho.Pixel(70)
                                objCell.Text = "Cota Final"
                                objCell.Wrap = False
                            Case Is = 7
                                objCell.Width = objTamanho.Pixel(70)
                                objCell.Text = "Outras Estr"
                                objCell.Wrap = False
                                '--WI5072 (02/10/2018)
                            Case Is = 8
                                objCell.Width = objTamanho.Pixel(70)
                                objCell.Text = "Transferida"
                        End Select
                        objRow.Controls.Add(objCell)
                    Next
                    tblConsulta.Rows.Add(objRow)
                    intI = intI + 1
                End If

                'Nova linha da tabela
                objRow = New TableRow
                If intI Mod 2 = 0 Then
                    'quando linha = par troca cor
                    objRow.BackColor = Color
                End If
                For intCol = 1 To 8
                    'nova Celula
                    objCell = New TableCell
                    objCell.Wrap = False
                    Select Case intCol
                        Case Is = 1
                            objCell.Text = rsConsulta("sigempre")
                        Case Is = 2
                            objCell.Text = IIf(Not IsDBNull(rsConsulta("valturb")), rsConsulta("valturb"), 0)
                            intTotTurb += objCell.Text
                        Case Is = 3
                            objCell.Text = IIf(Not IsDBNull(rsConsulta("valvert")), rsConsulta("valvert"), 0)
                            intTotVert += objCell.Text
                        Case Is = 4
                            objCell.Text = IIf(Not IsDBNull(rsConsulta("valaflu")), rsConsulta("valaflu"), 0)
                            intTotAflu += objCell.Text
                        Case Is = 5
                            objCell.Text = Format(IIf(Not IsDBNull(rsConsulta("valcota")), rsConsulta("valcota"), 0), "######0.00")
                            dblTotCota += objCell.Text
                        '-- CRQ2345 (15/08/2012)
                        Case Is = 6
                            objCell.Text = Format(IIf(Not IsDBNull(rsConsulta("valcotaf")), rsConsulta("valcotaf"), 0), "######0.00")
                            dblTotCotaf += objCell.Text
                        Case Is = 7
                            objCell.Text = Format(IIf(Not IsDBNull(rsConsulta("valoutrasestr")), rsConsulta("valoutrasestr"), 0), "######0.00")
                            dblTotOutrasEstr += objCell.Text
                            '--WI5072 (02/10/2018)
                        Case Is = 8
                            objCell.Text = IIf(Not IsDBNull(rsConsulta("valtransf")), rsConsulta("valtransf"), 0)
                            intTotTransf += objCell.Text
                    End Select
                    objRow.Controls.Add(objCell)
                Next
                'Adiciona a linha a tabela
                tblConsulta.Rows.Add(objRow)
                intI = intI + 1
            Loop

            If intI > 0 Then
                'Total
                objRow = New TableRow
                If intI Mod 2 = 0 Then
                    'quando linha = par troca cor
                    objRow.BackColor = Color
                End If
                objCell = New TableCell
                objCell.Text = "Total"
                objCell.Font.Bold = True
                objRow.Controls.Add(objCell)
                'Turbinada
                objCell = New TableCell
                objCell.Font.Bold = True
                objCell.Text = intTotTurb
                objRow.Controls.Add(objCell)
                'Vertida
                objCell = New TableCell
                objCell.Font.Bold = True
                objCell.Text = intTotVert
                objRow.Controls.Add(objCell)
                tblConsulta.Controls.Add(objRow)
                'Afluente
                objCell = New TableCell
                objCell.Font.Bold = True
                objCell.Text = intTotAflu
                objRow.Controls.Add(objCell)
                tblConsulta.Controls.Add(objRow)
                'Cota Inicial
                objCell = New TableCell
                objCell.Font.Bold = True
                objCell.Text = Format(dblTotCota, "#####0.00")
                objRow.Controls.Add(objCell)
                tblConsulta.Controls.Add(objRow)
                '-- CRQ2345 (15/08/2012)
                'Cota Final
                objCell = New TableCell
                objCell.Font.Bold = True
                objCell.Text = Format(dblTotCotaf, "#####0.00")
                objRow.Controls.Add(objCell)
                tblConsulta.Controls.Add(objRow)
                'Outras Estruturas
                objCell = New TableCell
                objCell.Font.Bold = True
                objCell.Text = Format(dblTotOutrasEstr, "#####0.00")
                objRow.Controls.Add(objCell)
                tblConsulta.Controls.Add(objRow)
                '--WI5072 (02/10/2018)
                'Transferida
                objCell = New TableCell
                objCell.Font.Bold = True
                objCell.Text = intTotTransf
                objRow.Controls.Add(objCell)
                tblConsulta.Controls.Add(objRow)
                intI = intI + 1

                'Média
                objRow = New TableRow
                If intI Mod 2 = 0 Then
                    'quando linha = par troca cor
                    objRow.BackColor = Color
                End If
                objCell = New TableCell
                objCell.Text = "Média"
                objCell.Font.Bold = True
                objRow.Controls.Add(objCell)
                'Turbinada
                objCell = New TableCell
                objCell.Font.Bold = True
                objCell.Text = Int(IIf(intTotTurb = 0, intTotTurb, intTotTurb / (intI - 2)))
                objRow.Controls.Add(objCell)
                'Vertida
                objCell = New TableCell
                objCell.Font.Bold = True
                objCell.Text = Int(IIf(intTotVert = 0, intTotVert, intTotVert / (intI - 2)))
                objRow.Controls.Add(objCell)
                'Afluente
                objCell = New TableCell
                objCell.Font.Bold = True
                objCell.Text = Int(IIf(intTotAflu = 0, intTotAflu, intTotAflu / (intI - 2)))
                objRow.Controls.Add(objCell)
                'Cota Inicial
                objCell = New TableCell
                objCell.Font.Bold = True
                objCell.Text = Format(IIf(dblTotCota = 0, dblTotCota, dblTotCota / (intI - 2)), "######0.00")
                objRow.Controls.Add(objCell)
                '-- CRQ2345 (15/08/2012)
                'Cota Final
                objCell = New TableCell
                objCell.Font.Bold = True
                objCell.Text = Format(IIf(dblTotCotaf = 0, dblTotCotaf, dblTotCotaf / (intI - 2)), "######0.00")
                objRow.Controls.Add(objCell)
                'Outras Estruturas
                objCell = New TableCell
                objCell.Font.Bold = True
                objCell.Text = Format(IIf(dblTotOutrasEstr = 0, dblTotOutrasEstr, dblTotOutrasEstr / (intI - 2)), "######0.00")
                objRow.Controls.Add(objCell)
                tblConsulta.Controls.Add(objRow)
                '--WI5072 (02/10/2018)
                'Transferida
                objCell = New TableCell
                objCell.Font.Bold = True
                objCell.Text = Int(IIf(intTotTransf = 0, intTotTransf, intTotTransf / (intI - 2)))
                objRow.Controls.Add(objCell)
            End If
            rsConsulta.Close()
            'Conn.Close()
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

    Private Overloads Sub btnVisualizar_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnVisualizar.Click
        Dim strCampo As String
        If cboDataInicial.SelectedIndex <> 0 Then
            Session("datEscolhida") = CDate(cboDataInicial.SelectedItem.Text)
        End If
        If cboEmpresa.SelectedIndex <> 0 Then
            Session("strCodEmpre") = cboEmpresa.SelectedItem.Value
        End If

        If Trim(optDados.SelectedItem.Value) = "0" Then
            strCampo = "tran"
        Else
            strCampo = ""
        End If
        Select Case optVisualiza.SelectedIndex
            Case Is = 0
                'data pdp
                ConsultaData(strCampo)
            Case Is = 1
                'empresa
                ConsultaEmpresa(strCampo)
        End Select

        'Chama uma nova janela para exibir a planilha e passa os parâmetros por query string
        imgPlanilha.Attributes.Add("onclick",
                                   "window.open('frmPlanilha.aspx?" &
                                   "strDataPDP=" & cboDataInicial.SelectedValue & "&" &
                                   "strDataPDPFim=" & cboDataFinal.SelectedValue & "&" &
                                   "strCampo=" & strCampo & "&" &
                                   "strEmpresa=" & cboEmpresa.SelectedValue.Trim & "|" & cboEmpresa.SelectedItem.Text.Trim & "&" &
                                   "strTabela=VAZ" & "&" &
                                   "strBase=" & strBase & "&" &
                                   "strAcesso=" & Page.Request.QueryString("strAcesso") & "'" &
                                   ",'Planilha','height = 600, width = 850, toolbar=yes,location=no,status=no,menubar=yes,scrollbars=yes,scrolling=yes,resizebled=yes');")
        imgPlanilha.Visible = True

    End Sub

End Class
