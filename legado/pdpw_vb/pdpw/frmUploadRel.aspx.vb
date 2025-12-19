Imports System.IO
Imports System.IO.Path

Partial Class frmUploadRel
    Inherits System.Web.UI.Page
    Protected WithEvents frmArquivo As System.Web.UI.HtmlControls.HtmlForm
    Protected WithEvents txt_filename As System.Web.UI.HtmlControls.HtmlInputHidden
    Private fl_File As File
    Private fl_directory As Directory
    Private i_l_o_col As Collection
    Protected WithEvents lblEmpresa As System.Web.UI.WebControls.Label
    Protected WithEvents drpDownEmpresa As System.Web.UI.WebControls.DropDownList
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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        'objMenu = CType(Session("onsmenu"), OnsWebControls.OnsMenu)

        If Not Page.IsPostBack Then
            Dim intI As Integer
            Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
            Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
            'If Session("datEscolhida") = Nothing Then
            '    'Inicializa a variável com data do próximo
            '    Session("datEscolhida") = Now.AddDays(1)
            'End If
            Cmd.Connection = Conn
            Try
                Conn.Open("rpdp")
                Cmd.CommandText = "SELECT datpdp " & _
                                  "FROM pdp " & _
                                  "ORDER BY datpdp DESC"
                Dim rsData As OnsClasses.OnsData.OnsDataReader = Cmd.ExecuteReader
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
                    'If Trim(drpDownDataPDP.Items(intI).Value) = Format(Session("datEscolhida"), "dd/MM/yyyy") Then
                    '    drpDownDataPDP.SelectedIndex = intI
                    'End If
                    intI = intI + 1
                Loop

                rsData.Close()
                rsData = Nothing
                Cmd.Connection.Close()
                Conn.Close()
                'drpDownDataPDP_SelectedIndexChanged(sender, e)
            Catch
                Session("strMensagem") = "Não foi possível acessar a Base de Dados."
                Conn.Close()
                Response.Redirect("frmMensagem.aspx")
            End Try
        End If
    End Sub

    Private Function ObterCaminhoArquivos() As String
        Return ConfigurationManager.AppSettings.Get("PathArquivosGerados")
    End Function

    Private Sub btnConfirmar_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnConfirmar.Click
        Dim strRetorno As String
        'Dim strArq As String = Server.MapPath("/IntUnica")

        'fl_Arquivo.Value = Request.Form("fl_Arquivo")
        Session("strChamada") = "frmUploadRel"

        Dim I_l_s_attachedfilename As String
        If Not fl_Arquivo.PostedFile Is Nothing Then
            'obtem o nome do arquivo
            I_l_s_attachedfilename = Path.GetFileName(fl_Arquivo.PostedFile.FileName)
            'caso o arquivo já exista excluirá
            fl_File.Delete(Me.ObterCaminhoArquivos() & "\Upload\" & I_l_s_attachedfilename)
            'copia o arquivo para o servidor
            fl_Arquivo.PostedFile.SaveAs(Me.ObterCaminhoArquivos() & "\Upload\" & I_l_s_attachedfilename)
        End If

        strRetorno = ""
        'strArq = strArq & "\pdpw\Download\" & I_l_s_attachedfilename
        'faz a carga do arquivo na base
        If UpLoadArquivoTextoRelatorio(Me.ObterCaminhoArquivos() & "\Upload\" & I_l_s_attachedfilename, Format(CDate(drpDownDataPDP.SelectedValue), "yyyyMMdd"), strRetorno) = False Then
            I_l_s_attachedfilename = ""
            'fl_Arquivo.Value = Nothing
            Session("strMensagem") = strRetorno
            Server.Transfer("frmMsgUpload.aspx")      'Response.Redirect("frmMensagem.aspx")
        Else
            I_l_s_attachedfilename = ""
            'fl_Arquivo.Value = Nothing
            Session("strMensagem") = "Upload executado com sucesso!" & strRetorno
            Server.Transfer("frmMsgUpload.aspx")      'Response.Redirect("frmMensagem.aspx")
        End If

    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        MyBase.Render(writer)
        'objMenu.RenderControl(writer)
    End Sub

    'Private Sub drpDownDataPDP_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles drpDownDataPDP.SelectedIndexChanged
    '    Try
    '        If drpDownDataPDP.SelectedIndex <> 0 Then
    '            Session("datEscolhida") = drpDownDataPDP.SelectedItem.Value
    '        End If
    '    Catch
    '        Session("strMensagem") = "Não foi possível acessar a Base de Dados."
    '        Response.Redirect("frmMensagem.aspx")
    '    End Try
    'End Sub
End Class
