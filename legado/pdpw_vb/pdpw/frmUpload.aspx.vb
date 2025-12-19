Imports System.Collections.Generic
Imports System.IO
Imports System.IO.Path

Partial Class frmUpload
    Inherits BaseWebUi

    Protected WithEvents txt_filename As System.Web.UI.HtmlControls.HtmlInputHidden
    Private fl_File As File
    Private fl_directory As Directory
    Private i_l_o_col As Collection
    Private strFile As String

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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load, Me.Load
        'Put user code to initialize the page here
        'objMenu = CType(Session("onsmenu"), OnsWebControls.OnsMenu)
        Dim Conn As System.Data.SqlClient.SqlConnection = New System.Data.SqlClient.SqlConnection
        Conn.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()

        Session("Dessem") = Request.QueryString("dessem")

        If Not Page.IsPostBack Then

            If (drpDownDataPDP.Items.Count > 0) Then
                Exit Sub
            End If

            Dim intI As Integer

            Dim Cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand
            'Cmd.Connection = Conn
            'Cmd.CommandTimeout = 1200 ' Define o timeout do comando para 1200 segundos

            Dim filtroDataDessem As String = ""

            If Session("datEscolhida") = Nothing Then
                'Inicializa a variável com data do próximo
                Session("datEscolhida") = Now.AddDays(1)
            End If

            Cmd.Connection = Conn

            Try
                Conn.Open()

                Dim rsData As System.Data.SqlClient.SqlDataReader = Nothing

                If Not IsNothing(Session("Dessem")) Then
                    filtroDataDessem = " INNER JOIN tb_statusimportacaodessem ON pdp.datpdp = REPLACE(CONVERT(varchar(8), tb_statusimportacaodessem.dat_processo, 112), '-', '') "


                    Cmd.CommandText = "SELECT distinct datpdp " &
                                  "FROM pdp " &
                                  filtroDataDessem &
                                  "ORDER BY datpdp DESC"

                    rsData = Cmd.ExecuteReader

                    intI = 1
                    Dim objItem As WebControls.ListItem
                    objItem = New WebControls.ListItem
                    objItem.Text = ""
                    objItem.Value = "0"

                    drpDownDataPDP.Items.Add(objItem)
                    Do While rsData.Read
                        objItem = New System.Web.UI.WebControls.ListItem
                        objItem.Text = Mid(rsData("datpdp"), 7, 2) & "/" & Mid(rsData("datpdp"), 5, 2) & "/" & Mid(rsData("datpdp"), 1, 4)
                        objItem.Value = Mid(rsData("datpdp"), 7, 2) & "/" & Mid(rsData("datpdp"), 5, 2) & "/" & Mid(rsData("datpdp"), 1, 4)
                        drpDownDataPDP.Items.Add(objItem)
                        If Trim(drpDownDataPDP.Items(intI).Value) = Format(Session("datEscolhida"), "dd/MM/yyyy") Then
                            drpDownDataPDP.SelectedIndex = intI
                        End If
                        intI = intI + 1
                    Loop

                    rsData.Close()
                    rsData = Nothing

                Else
                    Dim cachedData As List(Of ListItem) = CacheDataPDP.GetPdpData(True)

                    For Each item As WebControls.ListItem In cachedData
                        drpDownDataPDP.Items.Add(item)
                    Next

                    intI = 0
                    For Each item As WebControls.ListItem In drpDownDataPDP.Items
                        If Trim(item.Value) = Format(Session("datEscolhida"), "dd/MM/yyyy") Then
                            drpDownDataPDP.SelectedIndex = intI
                            Exit For
                        End If
                        intI += 1
                    Next

                End If

                drpDownDataPDP_SelectedIndexChanged(sender, e)

                lblInformacao.Text = ""

                If Not IsNothing(Session("Dessem")) Then
                    lblInformacao.Text = "Atenção! Só serão considerados os dados de geração (GER) das usinas com comentário."
                End If

                If IsNothing(Session("Dessem")) Then
                    Cmd.CommandText = "select distinct Trim(isnull(I.dsc_mnemonico,'')) as Insumo
                                from tb_bloqueioenvio L
                                join tb_insumo I on (I.id_insumo = L.id_insumo)"

                    rsData = Cmd.ExecuteReader

                    Dim listaMnemonicosBloqueados As String = ""
                    Dim separador As String = ""
                    Do While rsData.Read
                        listaMnemonicosBloqueados = listaMnemonicosBloqueados + separador + rsData("Insumo")
                        separador = " - "
                    Loop

                    If Not String.IsNullOrEmpty(listaMnemonicosBloqueados) Then
                        lblInformacao.Text = lblInformacao.Text + vbCrLf +
                            "Os insumos (" + listaMnemonicosBloqueados +
                            ") foram descontinuados e não serão capturados pelo ONS."
                    End If
                End If

                Cmd.Connection.Close()
                Conn.Close()

                lblMsg.Visible = False
            Catch
                lblMsg.Visible = True
                'If Conn.State = ConnectionState.Open Then
                Conn.Close()
                'End If
                'Session("strMensagem") = "Não foi possível acessar a Base de Dados."

                'Response.Redirect("frmMensagem.aspx")
            End Try
        End If
    End Sub

    Private Function ObterCaminhoArquivos() As String
        Return ConfigurationManager.AppSettings.Get("PathArquivosGerados")
    End Function

    Private Sub btnConfirmar_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnConfirmar.Click
        Dim strRetorno As String = ""
        Dim IsDessem As Boolean = Not IsNothing(Session("Dessem"))

        ' Verifica se o diretório existe e cria se necessário
        Dim uploadDir As String = ObterCaminhoArquivos() & "\Upload"
        If Not System.IO.Directory.Exists(uploadDir) Then
            System.IO.Directory.CreateDirectory(uploadDir)
        End If

        ' Verifica se foi enviado algum arquivo
        If fl_Arquivo.PostedFile IsNot Nothing AndAlso fl_Arquivo.PostedFile.ContentLength > 0 Then
            ' Obtém o nome do arquivo
            Dim I_l_s_attachedfilename As String = Path.GetFileName(fl_Arquivo.PostedFile.FileName)
            Dim filePath As String = Path.Combine(uploadDir, I_l_s_attachedfilename)

            ' Protege contra exclusão/acesso simultâneo
            Try
                ' Exclui o arquivo se já existir
                If System.IO.File.Exists(filePath) Then
                    System.IO.File.Delete(filePath)
                End If

                ' Salva o arquivo no servidor usando streams para garantir o fechamento adequado
                Using fileStream As New FileStream(filePath, FileMode.Create, FileAccess.Write)
                    fl_Arquivo.PostedFile.InputStream.CopyTo(fileStream)
                End Using

                ' Faz a carga do arquivo na base
                Dim strAgentesRepresentados As String = AgentesRepresentados
                If Not UpLoadArquivoTexto(filePath, UsuarID, drpDownEmpresa.SelectedItem.Value(), Format(Session("datEscolhida"), "yyyyMMdd"), drpDownEmpresa.SelectedItem.Text, strRetorno, IsDessem) Then
                    ' Em caso de erro
                    Session("strMensagem") = strRetorno
                    Server.Transfer("frmMsgUpload.aspx")
                Else
                    ' Em caso de sucesso
                    Session("strMensagem") = "Upload executado com sucesso!" & strRetorno
                    Server.Transfer("frmMsgUpload.aspx")
                End If
            Catch ex As IOException
                ' Tratamento em caso de erro de acesso ao arquivo
                Session("strMensagem") = "Erro ao manipular o arquivo: " & ex.Message
                Server.Transfer("frmMsgUpload.aspx")
            End Try
        Else
            ' Caso nenhum arquivo tenha sido selecionado
            Session("strMensagem") = "Selecione um arquivo para Upload!"
            Server.Transfer("frmMsgUpload.aspx")
        End If
    End Sub


    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        MyBase.Render(writer)
        'objMenu.RenderControl(writer)
    End Sub

    Private Sub drpDownEmpresa_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles drpDownEmpresa.SelectedIndexChanged
        If drpDownEmpresa.SelectedIndex > 0 Then
            Session("strCodEmpre") = drpDownEmpresa.SelectedItem.Value
        End If

        If IsNothing(Session("Dessem")) Then
            'Valida Limite de Envio
            Dim lRetorno As Integer = 0
            If Not ValidaLimiteEntradaDados(drpDownEmpresa.SelectedItem.Value, drpDownDataPDP.SelectedItem.Value, lRetorno) Then
                btnConfirmar.Visible = False
                fl_Arquivo.Visible = False
                lblArquivo.Visible = False
                If lRetorno = 1 Then
                    Response.Write("<SCRIPT>alert('" + strMsgInicioLimiteEnvioDados + "')</SCRIPT>")
                Else
                    Response.Write("<SCRIPT>alert('" + strMsgLimiteEnvioDados + "')</SCRIPT>")
                End If
                Exit Sub
            Else
                btnConfirmar.Visible = True
                fl_Arquivo.Visible = True
                lblArquivo.Visible = True
            End If
        Else

            If drpDownEmpresa.SelectedItem.Value <> "" And drpDownDataPDP.SelectedItem.Value <> "0" Then

                If Not Me.FactoryBusiness.ComentarioDESSEMBusiness.ValidaLimiteEnvioComentarioDESSEM(drpDownEmpresa.SelectedItem.Value, drpDownDataPDP.SelectedItem.Value) Then
                    btnConfirmar.Visible = False
                    fl_Arquivo.Visible = False
                    lblArquivo.Visible = False
                    Response.Write("<SCRIPT>alert('" + strMsgLimiteParaUploadArquivo + "')</SCRIPT>")
                    Exit Sub
                Else
                    btnConfirmar.Visible = True
                    fl_Arquivo.Visible = True
                    lblArquivo.Visible = True
                End If
            Else
                btnConfirmar.Visible = True
                fl_Arquivo.Visible = True
                lblArquivo.Visible = True
            End If

        End If

    End Sub

    Private Sub drpDownDataPDP_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles drpDownDataPDP.SelectedIndexChanged
        Try
            If drpDownDataPDP.SelectedIndex <> 0 Then
                Session("datEscolhida") = CDate(drpDownDataPDP.SelectedItem.Value)
            End If
            'preenche a combo de empresas de acordo com o usuário logado
            PreencheComboEmpresaPOP(AgentesRepresentados, drpDownEmpresa, Session("strCodEmpre"))
            drpDownEmpresa_SelectedIndexChanged(sender, e)
            lblMsg.Visible = False
        Catch ex As Exception
            ' Captura a mensagem de erro
            Dim errorMessage As String = ex.Message
            lblMsg.Visible = True

        End Try
    End Sub
End Class
