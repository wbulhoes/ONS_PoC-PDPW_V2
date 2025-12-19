Imports System.IO

Partial Class frmCnsArquivoEst
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

    Private Sub DownloadArquivo()
        If Trim(Request.QueryString("strNomeArquivo")) <> "" Then
            Dim strNomeArquivo As String = Request.QueryString("strNomeArquivo")
            Dim arq = New System.IO.FileInfo(Path.Combine(Me.ObterCaminhoArquivos() & "\Download\Estudo\", strNomeArquivo))

            System.Web.HttpContext.Current.Response.Clear()
            System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + strNomeArquivo)
            System.Web.HttpContext.Current.Response.ContentType = "application/octet-stream"
            System.Web.HttpContext.Current.Response.WriteFile(arq.FullName)
            System.Web.HttpContext.Current.Response.End()
        End If
    End Sub

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'objMenu = CType(Session("onsmenu"), OnsWebControls.OnsMenu)
        If Not Page.IsPostBack Then
            Try
                Me.DownloadArquivo()

                divCal.Visible = False
                'Elimina arquivos na pasta download gerados com mais de uma semana
                EliminaArquivos()
                PreencheComboEmpresaPOP(AgentesRepresentados, cboEmpresa, Session("strCodEmpre"), "S")

            Catch
                Session("strMensagem") = "Não foi possível acessar a Página. ocorreu o seguinte erro: " & Chr(10) & Err.Description
                Response.Redirect("frmMensagem.aspx")
            End Try
        End If
    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        If SessaoAtiva(Page.Session) Then
            MyBase.Render(writer)
            'objMenu.RenderControl(writer)
        End If
    End Sub

    Private Function ObterCaminhoArquivos() As String
        Return ConfigurationManager.AppSettings.Get("PathArquivosGerados")
    End Function

    Private Sub EliminaArquivos()
        Dim strPath, strArquivo, strDataLimite As String
        strDataLimite = Format(System.DateTime.Now().AddDays(-30), "yyyyMMdd")
        strPath = Me.ObterCaminhoArquivos() & "\Download\Estudo\"
        strArquivo = Dir(strPath)
        Do While strArquivo <> ""
            'Verifica se o arquivo foi gerado a mais de 30 dias
            If Int32.Parse(strArquivo.Substring(6, 8)) < Int32.Parse(strDataLimite) Then
                'Elimina o arquivo
                Kill(strPath & strArquivo)
            End If
            'Verifica o próximo arquivo
            strArquivo = Dir()
        Loop
    End Sub

    Private Sub btnPesquisar_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnPesquisar.Click
        Dim intRow, intCol, intArq As Integer
        Dim objRow As TableRow
        Dim objCell As TableCell
        Dim objLink As System.Web.UI.WebControls.HyperLink
        Dim strPath, strArquivo, strLinha, strData As String
        Dim blnVale As Boolean
        Dim arrArquivo(3, 0) As String
        Dim arrInstante() As DateTime
        'Dim aTeste As Array
        Dim intI, intJ As Integer

        'teste.Reverse(teste, 4, 10)

        If txtData.Text <> "" Then
            Session("datEscolhida") = CDate(txtData.Text)
        Else
            Session("datEscolhida") = ""
        End If
        If cboEmpresa.SelectedIndex <> 0 Then
            Session("strCodEmpre") = cboEmpresa.SelectedItem.Value
        Else
            Session("strCodEmpre") = ""
        End If
        'limpa a tabela
        tblArquivo.Rows.Clear()
        Dim objTamanho As System.Web.UI.WebControls.Unit
        objTamanho = New Unit

        objRow = New TableRow
        objRow.BackColor = System.Drawing.Color.YellowGreen
        For intCol = 1 To 3
            'nova Celula
            objCell = New TableCell
            objCell.Font.Bold = True
            Select Case intCol
                Case Is = 1
                    objCell.Width = objTamanho.Pixel(250)
                    objCell.Controls.Add(New LiteralControl("Arquivo"))
                Case Is = 2
                    objCell.Width = objTamanho.Pixel(100)
                    objCell.Controls.Add(New LiteralControl("Data PDP"))
                Case Is = 3
                    objCell.Width = objTamanho.Pixel(100)
                    objCell.Controls.Add(New LiteralControl("Gerado em"))
            End Select
            objRow.Controls.Add(objCell)
        Next
        tblArquivo.Rows.Add(objRow)

        'Colocar cor na tabela (RGB(233, 244, 207) = cor padrão ONS)
        Dim Color As System.Drawing.Color
        Color = New System.Drawing.Color
        Color = System.Drawing.ColorTranslator.FromWin32(RGB(233, 244, 207))
        intRow = 1

        strPath = Me.ObterCaminhoArquivos() & "\Download\Estudo\"
        strArquivo = Dir(strPath)

        intI = 0
        Do While strArquivo <> ""
            'Se não selecionar empresa ou selecionar e o arquivo conter o código da empresa
            'Mostra o arquivo na grid
            If cboEmpresa.SelectedIndex = 0 Or (cboEmpresa.SelectedIndex <> 0 And
            (cboEmpresa.SelectedItem.Value.Trim = strArquivo.Substring(3, 2) Or
            cboEmpresa.SelectedItem.Value.Trim = strArquivo.Substring(0, 2))) Then
                intArq = FreeFile()
                FileOpen(intArq, strPath & strArquivo, OpenMode.Input, OpenAccess.Read, OpenShare.Default)
                strLinha = LineInput(intArq)
                strData = Mid(strLinha, 16, 8)
                FileClose(intArq)
                blnVale = True
                If txtData.Text <> "" Then
                    If txtData.Text.Substring(6, 4) & txtData.Text.Substring(3, 2) & txtData.Text.Substring(0, 2) <> strData Then
                        blnVale = False
                    End If
                End If

                If blnVale Then
                    ReDim Preserve arrArquivo(3, intI)
                    ReDim Preserve arrInstante(intI)

                    arrArquivo(0, intI) = strArquivo

                    arrArquivo(1, intI) = "frmCnsArquivoEst.aspx?strNomeArquivo=" & strArquivo

                    arrArquivo(2, intI) = Mid(strData, 7, 2) & "/" &
                                          Mid(strData, 5, 2) & "/" &
                                          Mid(strData, 1, 4)
                    arrArquivo(3, intI) = Mid(strArquivo, 13, 2) & "/" &
                                          Mid(strArquivo, 11, 2) & "/" &
                                          Mid(strArquivo, 7, 4) & " - " &
                                          Mid(strArquivo, 16, 2) & ":" &
                                          Mid(strArquivo, 18, 2) & ":" &
                                          Mid(strArquivo, 20, 2)
                    arrInstante(intI) = CDate(Mid(strArquivo, 13, 2) & "/" &
                                              Mid(strArquivo, 11, 2) & "/" &
                                              Mid(strArquivo, 7, 4) & " " &
                                              Mid(strArquivo, 16, 2) & ":" &
                                              Mid(strArquivo, 18, 2) & ":" &
                                              Mid(strArquivo, 20, 2))
                    intI += 1
                End If
            End If
            strArquivo = Dir()
        Loop
        If intI > 0 Then
            arrInstante.Sort(arrInstante)
            arrInstante.Reverse(arrInstante)
            For intI = 0 To arrInstante.GetUpperBound(0)
                For intJ = 0 To arrArquivo.GetUpperBound(1)
                    If CDate(arrArquivo(3, intJ).Replace("- ", "")) = arrInstante(intI) Then
                        'Cria um novo check box
                        objLink = New HyperLink
                        objLink.Text = arrArquivo(0, intJ)
                        objLink.Target = "NovoFormulario"
                        objLink.NavigateUrl = arrArquivo(1, intJ)
                        'Nova linha da tabela
                        objRow = New TableRow
                        If intRow Mod 2 = 0 Then
                            'quanto linha = par troca cor
                            objRow.BackColor = Color
                        End If
                        For intCol = 1 To 3
                            'nova Celula
                            objCell = New TableCell
                            objCell.Wrap = False
                            Select Case intCol
                                Case Is = 1
                                    objCell.Font.Bold = True
                                    objCell.Controls.Add(objLink)
                                Case Is = 2
                                    objCell.Text = arrArquivo(2, intJ)
                                Case Is = 3
                                    objCell.Text = arrArquivo(3, intJ)
                            End Select
                            objRow.Controls.Add(objCell)
                        Next
                        'Adiciona a linha a tabela
                        tblArquivo.Rows.Add(objRow)

                        intRow = intRow + 1

                        Exit For
                    End If
                Next
            Next
        End If
    End Sub

    Private Sub btnCalendario_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCalendario.Click
        'If txtData.Text <> "" Then
        '    calData.SelectedDate = CType(txtData.Text, Date)
        'Else
        '    calData.SelectedDate = System.DateTime.Today.Date
        'End If
        calData.SelectedDate = CDate("01/01/1900")
        divCal.Visible = True
    End Sub

    Private Sub calData_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calData.SelectionChanged
        txtData.Text = Format(calData.SelectedDate.Date, "dd/MM/yyyy")
        divCal.Visible = False
    End Sub

End Class
