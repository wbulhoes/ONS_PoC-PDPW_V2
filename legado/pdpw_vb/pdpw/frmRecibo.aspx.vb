Partial Class frmRecibo
    Inherits System.Web.UI.Page
    Protected WithEvents lblHoraEnvio As System.Web.UI.WebControls.Label

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
        If SessaoAtiva(Page.Session) Then
            If Trim(Request.QueryString("strNomeArquivo")) = "" Then
                Exit Sub
            End If

            Dim strNomeArquivo As String = Request.QueryString("strNomeArquivo")
            'Dim oConn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
            'Dim oComm As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
            Dim oConn As System.Data.SqlClient.SqlConnection = New System.Data.SqlClient.SqlConnection
            oConn.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
            Dim oComm As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand
            'Dim oReader As OnsClasses.OnsData.OnsDataReader = New OnsClasses.OnsData.OnsDataReader
            'Dim oReader As System.Data.SqlClient.SqlDataReader = New SqlClient.SqlDataReader
            Dim oReader As System.Data.SqlClient.SqlDataReader = Nothing
            Try
                oConn.Open()
                'oConn.Servico = "frmRecibo"
                'oConn.Usuario = UsuarID
                oComm.Connection = oConn

                oComm.CommandText = "SELECT e.nomempre, m.datpdp, m.dthmensa, m.sitmensa, m.usuar_id, " &
                                "ISNULL(m.totdespa,0) as totdespa , ISNULL(m.totcarga,0) as totcarga, ISNULL(m.totinter,0) as totinter, ISNULL(m.totvazao,0) AS totvazao, ISNULL(m.qtdrestr,0) as qtdrestr, " &
                                "ISNULL(m.totinflexi,0) as totinflexi, ISNULL(m.qtdparal,0) as qtdparal, ISNULL(m.totrazener,0) as totrazener, ISNULL(m.totrazelet,0) as totrazelet, " &
                                "ISNULL(m.totexporta,0) As totexporta, ISNULL(m.totimporta,0) As totimporta, " &
                                "ISNULL(m.totmre,0) As totmre, ISNULL(m.totmif,0) As totmif, " &
                                "ISNULL(m.totpcc,0) As totpcc, ISNULL(m.totmco,0) As totmco, " &
                                "ISNULL(m.totmos,0) As totmos, ISNULL(m.totmeg,0) As totmeg, " &
                                "ISNULL(m.toterp,0) As toterp, ISNULL(m.totdsp,0) AS totdsp, " &
                                "ISNULL(m.totclf,0) AS totclf, ISNULL(m.qtdparalco,0) AS totpco, " &
                                "ISNULL(m.totcota,0) AS totcota, ISNULL(m.totrfc,0) AS totrfc, " &
                                "ISNULL(m.totrmp,0) AS totrmp, ISNULL(m.totgfm,0) AS totgfm, " &
                                "ISNULL(m.totcfm,0) AS totcfm, ISNULL(m.totsom,0) AS totsom, " &
                                "ISNULL(m.totges,0) AS totges, ISNULL(m.totgec,0) AS totgec, " &
                                "ISNULL(m.totdca,0) AS totdca, ISNULL(m.totdcr,0) AS totdcr, " &
                                "ISNULL(m.totir1,0) AS totir1, ISNULL(m.totir2,0) AS totir2, " &
                                "ISNULL(m.totir3,0) AS totir3, ISNULL(m.totir4,0) AS totir4, " &
                                "ISNULL(m.totcvu,0) AS totcvu, ISNULL(m.totrro,0) AS totrro " &
                                "FROM mensa m, empre e " &
                                "WHERE m.nomarq = '" & Trim(strNomeArquivo) & "' " &
                                "AND m.codempre = e.codempre"

                oReader = oComm.ExecuteReader




                If oReader.Read() Then
                    lblempresavalor.Text = oReader.GetString(0)
                    lbldatapdpvalor.Text = Mid(oReader.GetString(1), 7, 2) & "/" & Mid(oReader.GetString(1), 5, 2) & "/" & Mid(oReader.GetString(1), 1, 4)
                    lblusuariovalor.Text = oReader.GetString(4)
                    lbldataenviovalor.Text = Mid(oReader.GetString(2), 7, 2) & "/" & Mid(oReader.GetString(2), 5, 2) & "/" & Mid(oReader.GetString(2), 1, 4)
                    lblhoraenviovalor.Text = Mid(oReader.GetString(2), 9, 2) & ":" & Mid(oReader.GetString(2), 11, 2) & ":" & Mid(oReader.GetString(2), 13, 2)
                    lblarquivovalor.Text = strNomeArquivo
                    lblsituacaovalor.Text = oReader.GetString(3)
                    lblgeracaovalor.Text = oReader.GetValue(5)
                    lblcargavalor.Text = oReader.GetValue(6)
                    lblintervalor.Text = oReader.GetValue(7)
                    lblvazaovalor.Text = oReader.GetValue(8)
                    lblrestrvalor.Text = oReader.GetValue(9)
                    lblinflexivalor.Text = oReader.GetValue(10)
                    lblmanuvalor.Text = oReader.GetValue(11)
                    lblrazenervalor.Text = oReader.GetValue(12)
                    lblrazeletvalor.Text = oReader.GetValue(13)
                    lblexporta.Text = oReader.GetValue(14)
                    lblimporta.Text = oReader.GetValue(15)
                    lblmre.Text = oReader.GetValue(16)
                    lblmif.Text = oReader.GetValue(17)
                    lblpcc.Text = oReader.GetValue(18)
                    lblmco.Text = oReader.GetValue(19)
                    lblmos.Text = oReader.GetValue(20)
                    lblmeg.Text = oReader.GetValue(21)
                    lblerp.Text = oReader.GetValue(22)
                    lbldsp.Text = oReader.GetValue(23)
                    lblclf.Text = oReader.GetValue(24)
                    lblpco.Text = oReader.GetValue(25)
                    lblcota.Text = oReader.GetValue(26)
                    lblrfc.Text = oReader.GetValue(27)
                    lblrmp.Text = oReader.GetValue(28)
                    lblgfm.Text = oReader.GetValue(29)
                    lblcfm.Text = oReader.GetValue(30)
                    lblsom.Text = oReader.GetValue(31)
                    lblges.Text = oReader.GetValue(32)
                    lblgec.Text = oReader.GetValue(33)
                    lbldca.Text = oReader.GetValue(34)
                    lbldcr.Text = oReader.GetValue(35)
                    lblIR1.Text = oReader.GetValue(36)
                    lblIR2.Text = oReader.GetValue(37)
                    lblIR3.Text = oReader.GetValue(38)
                    lblIR4.Text = oReader.GetValue(39)
                    lblCvu.Text = oReader.GetValue(40)
                    lblRRO.Text = oReader.GetValue(41)
                End If
                oReader.Close()
                oComm.Dispose()
                oConn.Dispose()
                oConn.Close()
                lblMsg.Visible = False
            Catch
                lblMsg.Visible = True
            End Try

        End If
    End Sub

    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnimprimir.Click
        Try
            Response.Write("<script language=JavaScript>")
            Response.Write("window.print()")
            Response.Write("</script>")
            lblMsg.Visible = False

        Catch
            lblMsg.Visible = True
        End Try
    End Sub
End Class
