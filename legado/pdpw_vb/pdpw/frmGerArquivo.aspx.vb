Imports System.Collections.Generic
Imports System.Data.SqlClient

Partial Class frmGerArquivo
    Inherits System.Web.UI.Page
    Protected WithEvents Imagebutton1 As System.Web.UI.WebControls.ImageButton
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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        'objMenu = CType(Session("onsmenu"), OnsWebControls.OnsMenu)

        If Not Page.IsPostBack Then
            Dim intI As Integer

            Dim pdpData As List(Of ListItem) = CacheDataPDP.GetPdpData(False)

            intI = 1
            Dim objItem As New System.Web.UI.WebControls.ListItem
            objItem.Text = ""
            objItem.Value = "0"
            cboDataPdp.Items.Add(objItem)

            For Each item As WebControls.ListItem In pdpData
                cboDataPdp.Items.Add(item)
                If Trim(cboDataPdp.Items(intI).Value) = Format(Session("datEscolhida"), "yyyyMMdd") Then
                    cboDataPdp.SelectedIndex = intI
                End If
                intI += 1
            Next

            PreencheEmpresa()

            lblMsg.Visible = False
        End If

        Dim Dessem As String = Request.QueryString("dessem")
        PreencherRadios(Dessem, Page.IsPostBack)
    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        If SessaoAtiva(Page.Session) Then
            Funcoes.RetirarItensSelecionadosComDuplicidade(cboDataPdp)
            Funcoes.RetirarItensSelecionadosComDuplicidade(cboEmpresa)

            MyBase.Render(writer)
            'objMenu.RenderControl(writer)
        End If
    End Sub
    Private Function ObterCaminhoArquivos() As String
        Return ConfigurationManager.AppSettings.Get("PathArquivosGerados") & "\Download\"
    End Function

    Private Sub btnGravar_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnGravar.Click
        Dim bRetorno As Boolean = True
        Dim intUsu As Int16
        Dim strEmpresaDestino As String
        Dim strDataHora As String
        Dim arrEmpre() As String
        Dim Conn As SqlConnection = New SqlConnection
        Dim Cmd As SqlCommand = New SqlCommand
        Cmd.Connection = Conn
        Conn.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
        Conn.Open()
        If cboDataPdp.SelectedIndex <> 0 And cboEmpresa.SelectedIndex <> 0 Then
            If cboDataPdp.SelectedIndex <> 0 Then
                Session("datEscolhida") = CDate(cboDataPdp.SelectedItem.Text)
            End If
            If cboEmpresa.SelectedIndex <> 0 Then
                Session("strCodEmpre") = cboEmpresa.SelectedItem.Value
            End If

            Session("strMensagem") = Nothing

            Try
                'Dim strArq As String = Server.MapPath("")
                Dim strArq As String = Me.ObterCaminhoArquivos()

                'Grava arquivo para a empresa selecionada na data selecionada
                ' Conforme definição da Marta, será necessário gerar
                ' o arquivo para dados da área de transferência também,
                ' foi por isso que isolei o if abaixo e estou passando o valor
                ' da opção selecionada para a rotina gravaarquivotexto

                If optSelecao.SelectedValue = "E" Then
                    'Somente uma empresa selecionada
                    strDataHora = Format(Now, "yyyyMMddHHmmss")

                    Dim strOpcao As String = New String("1", 36)

                    Dim dadosarquivo As New GeracaoArquivo
                    dadosarquivo.strCodEmpresa = cboEmpresa.SelectedItem.Value
                    dadosarquivo.strData = cboDataPdp.SelectedItem.Value
                    dadosarquivo.strArq = strArq
                    dadosarquivo.blnTipoEnvio = optEnvia.SelectedItem.Value()
                    dadosarquivo.bFtp = False
                    dadosarquivo.bRetorno = bRetorno
                    dadosarquivo.strOpcao = strOpcao
                    dadosarquivo.strDataHora = strDataHora
                    dadosarquivo.strArqDestino = ""
                    dadosarquivo.Cmd = Cmd
                    dadosarquivo.strPerfil = PerfilID
                    dadosarquivo.codUsina = ""

                    If Not GravaArquivoTexto(dadosarquivo, bRetorno) Then
                        Session("strMensagem") = "Não foi possível gravar o arquivo texto, os dados podem estar errados."

                    Else
                        cboEmpresa.SelectedIndex = 0
                        Session("strMensagem") = "Arquivo gerado com sucesso."

                    End If
                Else
                    'Todas as empresas de uma determinada Área
                    If cboEmpresa.SelectedValue.Trim = "RS" Then
                        Cmd.CommandText = "Select codempre " &
                                          "From empre " &
                                          "Where codarea = '" & cboEmpresa.SelectedValue.Trim & "' Or " &
                                          "      codempre = 'CO' Or " &
                                          "      codempre = 'CE' " &
                                          "Group By codempre " &
                                          "Order By codempre"
                    Else
                        Cmd.CommandText = "Select codempre " &
                                          "From empre " &
                                          "Where codarea = '" & cboEmpresa.SelectedValue.Trim & "' " &
                                          "Order By codempre"
                    End If
                    'Conn.Open()
                    Dim rsEmpre As SqlDataReader = Cmd.ExecuteReader
                    intUsu = 0
                    Do While rsEmpre.Read
                        ReDim Preserve arrEmpre(intUsu)
                        arrEmpre(intUsu) = rsEmpre("codempre").ToString.Trim
                        intUsu += 1
                    Loop
                    rsEmpre.Close()

                    strDataHora = Format(Now, "yyyyMMddHHmmss")

                    Dim strOpcao As String = New String("1", 36)

                    For intUsu = 0 To UBound(arrEmpre)

                        Dim dadosarquivo As New GeracaoArquivo
                        dadosarquivo.strCodEmpresa = arrEmpre(intUsu)
                        dadosarquivo.strData = cboDataPdp.SelectedItem.Value
                        dadosarquivo.strArq = strArq
                        dadosarquivo.blnTipoEnvio = optEnvia.SelectedItem.Value()
                        dadosarquivo.bFtp = False
                        dadosarquivo.bRetorno = bRetorno
                        dadosarquivo.strOpcao = strOpcao
                        dadosarquivo.strDataHora = strDataHora
                        dadosarquivo.strArqDestino = cboEmpresa.SelectedValue.Trim
                        dadosarquivo.Cmd = Cmd
                        dadosarquivo.strPerfil = PerfilID

                        If Not GravaArquivoTexto(dadosarquivo, bRetorno) Then
                            Session("strMensagem") = "Não foi possível gravar o arquivo texto, os dados podem estar errados."
                            Response.Redirect("frmMensagem.aspx")
                        End If

                    Next
                    Conn.Close()
                    cboEmpresa.SelectedIndex = 0
                End If
            Catch ex As Exception
                Session("strMensagem") = "Não foi possível gravar o arquivo texto. " & Chr(10) & ex.Message

                If Conn.State = ConnectionState.Open Then
                    Conn.Close()
                End If
                logger.Error(ex.Message, ex)
            Finally

                If Conn.State = ConnectionState.Open Then
                    Conn.Close()
                End If

            End Try

            If Not IsNothing(Session("strMensagem")) Then
                Response.Redirect("frmMensagem.aspx")
            End If

        End If
    End Sub

    Private Sub PreencherRadios(Optional ByVal dessem As String = Nothing, Optional ByVal isPostBack As Boolean = True)

        If IsNothing(dessem) Then
            For Each x As ListItem In optEnvia.Items
                If x.Value = 4 Or x.Value = 5 Then 'Padrão
                    x.Attributes.CssStyle.Add("display", "none")
                End If

                If x.Value = 0 And Not isPostBack Then
                    x.Selected = True
                End If

            Next x
        Else
            For Each x As ListItem In optEnvia.Items
                If x.Value < 4 Or x.Value > 5 Then 'DESSEM
                    x.Attributes.CssStyle.Add("display", "none")
                End If

                If x.Value = 4 And Not isPostBack Then
                    x.Selected = True
                End If

            Next x
        End If

    End Sub

    Private Sub PreencheEmpresa()
        Dim Conn As SqlConnection = New SqlConnection
        Dim Cmd As SqlCommand = New SqlCommand
        Dim intI As Integer
        Cmd.Connection = Conn
        Try
            If optSelecao.SelectedValue = "E" Then
                Cmd.CommandText = "Select codempre, sigempre " &
                                  "From empre " &
                                  "Order By sigempre"
            Else
                Cmd.CommandText = "Select codarea As codempre, nomarea As sigempre " &
                                  "From area " &
                                  "Order By nomarea"
            End If
            Conn.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
            Conn.Open()
            Dim rsEmpresa As SqlDataReader = Cmd.ExecuteReader
            intI = 1
            Dim objItem As System.Web.UI.WebControls.ListItem
            objItem = New System.Web.UI.WebControls.ListItem
            objItem.Text = ""
            objItem.Value = "0"
            cboEmpresa.Items.Clear()
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
            'Conn.Close()
            lblMsg.Visible = False
        Catch ex As Exception
            lblMsg.Visible = True
            'Session("strMensagem") = "Não foi possível acessar a Base de Dados. Ocorreu o seguinte erro: " & Chr(10) & Err.Description
            If Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If
            logger.Error(ex.Message, ex)
            'Response.Redirect("frmMensagem.aspx")
        Finally
            If Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If
        End Try
    End Sub

    Private Sub optSelecao_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optSelecao.SelectedIndexChanged
        PreencheEmpresa()
    End Sub

End Class
