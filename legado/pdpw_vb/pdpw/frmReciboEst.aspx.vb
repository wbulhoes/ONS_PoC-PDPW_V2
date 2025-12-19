Partial Class frmReciboEst
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
        If SessaoAtiva(Page.Session) Then
            If Trim(Request.QueryString("strNomeArquivo")) = "" Then
                Exit Sub
            End If

            Dim strNomeArquivo As String = Request.QueryString("strNomeArquivo")
            Dim oConn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
            Dim oComm As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
            Dim oReader As OnsClasses.OnsData.OnsDataReader = New OnsClasses.OnsData.OnsDataReader

            oConn.Open("pdp")
            oConn.Servico = "frmRecibo"
            oConn.Usuario = UsuarID
            oComm.Connection = oConn

            oComm.CommandText = "SELECT e.nomempre, m.datpdp, m.dthmensa, m.sitmensa, m.usuar_id, m.totcarga, m.totinter, m.totdespa " & _
                                "FROM mensa m, empre e " & _
                                "WHERE m.nomarq = '" & Trim(strNomeArquivo) & "' " & _
                                "AND m.codempre = e.codempre"
            oReader = oComm.ExecuteReader
            If oReader.Read() Then
                lblEmpresaValor.Text = oReader.GetString(0)
                lblDataPdpValor.Text = Mid(oReader.GetString(1), 7, 2) & "/" & Mid(oReader.GetString(1), 5, 2) & "/" & Mid(oReader.GetString(1), 1, 4)
                lblUsuarioValor.Text = oReader.GetString(4)
                lblDataEnvioValor.Text = Mid(oReader.GetString(2), 7, 2) & "/" & Mid(oReader.GetString(2), 5, 2) & "/" & Mid(oReader.GetString(2), 1, 4)
                lblHoraEnvioValor.Text = Mid(oReader.GetString(2), 9, 2) & ":" & Mid(oReader.GetString(2), 11, 2) & ":" & Mid(oReader.GetString(2), 13, 2)
                lblArquivoValor.Text = strNomeArquivo
                lblSituacaoValor.Text = oReader.GetString(3)
                lblCargaValor.Text = oReader.GetValue(5)
                lblInterValor.Text = oReader.GetValue(6)
                lblDespaValor.Text = oReader.GetValue(7)
            End If
            oReader.Close()
            oComm.Dispose()
            oConn.Dispose()
            oConn.Close()
        End If
    End Sub

    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnImprimir.Click
        Response.Write("<script language=JavaScript>")
        Response.Write("window.print()")
        Response.Write("</script>")
    End Sub
End Class
