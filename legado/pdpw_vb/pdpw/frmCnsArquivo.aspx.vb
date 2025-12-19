Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports System.Globalization
Imports System.IO
Imports System.Net
Imports System.Net.Http
Imports System.Net.Http.Headers
Imports DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing


Partial Class frmCnsArquivo
    Inherits BaseWebUi
    Private logger As log4net.ILog = log4net.LogManager.GetLogger(Me.GetType())
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

    Private Sub DownloadArquivo()
        If Trim(Request.QueryString("strNomeArquivo")) <> "" Then
            Dim strNomeArquivo As String = Request.QueryString("strNomeArquivo")
            Dim arq = New System.IO.FileInfo(Path.Combine(Me.ObterCaminhoArquivos(), strNomeArquivo))

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

            Me.DownloadArquivo()

            Dim intI As Integer
            Dim Conn As SqlConnection = New SqlConnection
            Dim Cmd As SqlCommand = New SqlCommand
            Cmd.Connection = Conn
            Try
                'Elimina arquivos na pasta download gerados com mais de uma semana
                EliminaArquivos()

                Cmd.CommandText = "Select codempre, " &
                                "       sigempre " &
                                "From empre " &
                                "Order By sigempre"
                Conn.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
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

                ' Cache para Datas PDP
                Dim pdpData As List(Of ListItem) = CacheDataPDP.GetPdpData(False)

                intI = 1
                objItem = New WebControls.ListItem
                objItem.Text = ""
                objItem.Value = "0"
                cboDataPdp.Items.Add(objItem)

                For Each item As WebControls.ListItem In pdpData
                    cboDataPdp.Items.Add(item)
                    If Trim(item.Value) = Format(Session("datEscolhida"), "yyyyMMdd") Then
                        cboDataPdp.SelectedIndex = intI
                    End If
                    intI += 1
                Next

                lblMsg.Visible = False
            Catch ex As Exception

                lblMsg.Visible = True
                If Conn.State = ConnectionState.Open Then
                    Conn.Close()
                End If

                logger.Error(ex.Message, ex)
                'Session("strMensagem") = "Não foi possível acessar a Página. ocorreu o seguinte erro: " & Chr(10) & Err.Description
                'If Conn.State = ConnectionState.Open Then
                '    Conn.Close()
                'End If
                'Response.Redirect("frmMensagem.aspx")
            Finally
                If Conn.State = ConnectionState.Open Then
                    Conn.Close()
                End If
            End Try
        End If

        Session("Dessem") = Request.QueryString("dessem")
    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        If SessaoAtiva(Page.Session) Then
            Funcoes.RetirarItensSelecionadosComDuplicidade(cboDataPdp)
            Funcoes.RetirarItensSelecionadosComDuplicidade(cboEmpresa)

            MyBase.Render(writer)
            'objMenu.RenderControl(writer)
        End If
    End Sub

    Private Sub EliminaArquivos()
        Dim strPath, strArquivo, strDataLimite As String
        strDataLimite = Format(System.DateTime.Now(), "yyyyMMdd")
        strPath = Me.ObterCaminhoArquivos()
        strArquivo = Dir(strPath)
        Do While strArquivo <> ""
            If strArquivo.ToLower().Contains(".pdp") Then
                If Int32.Parse(strArquivo.Substring(6, 8)) < Int32.Parse(strDataLimite) Then
                    Kill(strPath & strArquivo)
                End If
            End If
            strArquivo = Dir()
        Loop
    End Sub

    Private Function ObterCaminhoArquivos() As String

        Dim pathDESSEM As String = ""
        If Not IsNothing(Session("Dessem")) Then
            pathDESSEM = "DESSEM\"
        End If

        Dim fullPath As String = ConfigurationManager.AppSettings.Get("PathArquivosGerados") & "\Download\" & pathDESSEM

        If (Not Directory.Exists(fullPath)) Then
            Directory.CreateDirectory(fullPath)
        End If

        Return fullPath

    End Function


    Private Sub btnPesquisar_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnPesquisar.Click
        Dim intRow, intCol, intArq As Integer
        Dim objRow As TableRow
        Dim objCell As TableCell
        Dim objLink As System.Web.UI.WebControls.HyperLink
        Dim strPath, strArquivo, strLinha, strData As String

        Try
            If cboDataPdp.SelectedIndex <> 0 Then
                Session("datEscolhida") = CDate(cboDataPdp.SelectedItem.Text)
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

            strPath = Me.ObterCaminhoArquivos()
            strArquivo = Dir(strPath)

            Do While strArquivo <> ""

                'Se não selecionar empresa ou selecionar e o arquivo conter o código da empresa
                'Mostra o arquivo na grid
                If cboEmpresa.SelectedIndex = 0 Or (cboEmpresa.SelectedIndex <> 0 And
              (cboEmpresa.SelectedItem.Value.Trim = strArquivo.Substring(3, 2) Or
               cboEmpresa.SelectedItem.Value.Trim = strArquivo.Substring(0, 2))) Then
                    intArq = FreeFile()
                    FileOpen(intArq, strPath & strArquivo, OpenMode.Input, OpenAccess.Read, OpenShare.Shared)
                    strLinha = LineInput(intArq)
                    strData = Mid(strLinha, 16, 8)
                    FileClose(intArq)
                    If cboDataPdp.SelectedIndex <> 0 And
                    cboDataPdp.SelectedItem.Value <> strData Then
                        'Arquivo não entra na lista
                    Else
                        'Cria um novo check box
                        objLink = New HyperLink
                        objLink.Text = strArquivo
                        objLink.Target = "NovoFormulario"

                        objLink.NavigateUrl = "frmCnsArquivo.aspx?strNomeArquivo=" & strArquivo

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
                                    objCell.Text = Mid(strData, 7, 2) & "/" &
                                            Mid(strData, 5, 2) & "/" &
                                            Mid(strData, 1, 4)
                                Case Is = 3
                                    objCell.Text = Mid(strArquivo, 13, 2) & "/" &
                                            Mid(strArquivo, 11, 2) & "/" &
                                            Mid(strArquivo, 7, 4) & " - " &
                                            Mid(strArquivo, 16, 2) & ":" &
                                            Mid(strArquivo, 18, 2) & ":" &
                                            Mid(strArquivo, 20, 2)
                            End Select
                            objRow.Controls.Add(objCell)
                        Next
                        'Adiciona a linha a tabela
                        tblArquivo.Rows.Add(objRow)
                        intRow = intRow + 1
                    End If
                End If
                strArquivo = Dir()
            Loop
        Catch ex As Exception
            logger.Error(ex.Message, ex)
            Throw ex
        End Try
    End Sub

    Private Sub PopularTabela()

    End Sub

End Class
