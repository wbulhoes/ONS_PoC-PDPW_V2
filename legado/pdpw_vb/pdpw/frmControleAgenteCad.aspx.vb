Public Class frmControleAgenteCad
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then

            txtDataPDP.Text = Request.QueryString("DataPDP")

            'Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
            'Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
            'Cmd.Connection = Conn

            Try
                PreencheComboEmpresa(cboEmpresa)

                cboEmpresa.Items.RemoveAt(0)

                Dim objItemEmpresa As WebControls.ListItem
                objItemEmpresa = New System.Web.UI.WebControls.ListItem
                objItemEmpresa.Text = "(Selecione)"
                objItemEmpresa.Value = "0"
                cboEmpresa.Items.Insert(0, objItemEmpresa)

                objItemEmpresa = New System.Web.UI.WebControls.ListItem
                objItemEmpresa.Text = "(Todas)"
                objItemEmpresa.Value = "Todas"
                cboEmpresa.Items.Insert(1, objItemEmpresa)

                If Request.QueryString("codigoEmpresa") <> Nothing Then
                    cboEmpresa.SelectedIndex = cboEmpresa.Items.IndexOf(cboEmpresa.Items.FindByValue(Request.QueryString("codigoEmpresa")))
                    'cboEmpresa.Items.FindByValue(Trim(Request.QueryString("codigoEmpresa"))).Selected = True
                End If

                If cboEmpresa.SelectedIndex > 0 Then
                    cboEmpresa_SelectedIndexChanged(sender, e)
                End If

                'Verifica se é inclusão ou alteração
                If Request.QueryString("Acao") = "A" Then
                    txtDataInicial.Text = Request.QueryString("dtHoraIni").Substring(0, 10)
                    txtHoraInicial.Text = Request.QueryString("dtHoraIni").Substring(11, 5)
                    txtDataFinal.Text = Request.QueryString("dtHoraFim").Substring(0, 10)
                    txtHoraFinal.Text = Request.QueryString("dtHoraFim").Substring(11, 5)

                    cboEmpresa.Enabled = False
                End If

            Catch ex As Exception
                Session("strMensagem") = "Erro ao recuperar Empresa. (" + ex.Message + ")"
                'If Conn.State = ConnectionState.Open Then
                '    Conn.Close()
                'End If
                Response.Redirect("frmMensagem.aspx")
            Finally
                'If Conn.State = ConnectionState.Open Then
                '    Conn.Close()
                'End If
            End Try

            btnSalvar.Attributes.Add("onClick", "return confirmaOperacao();")

        End If
    End Sub

    Protected Sub btnSalvar_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSalvar.Click
        
        ' Validação dos campos
        Dim strDataHoraInicial As String = txtDataInicial.Text & " " & txtHoraInicial.Text
        Dim strDataHoraFinal As String = txtDataFinal.Text & " " & txtHoraFinal.Text

        If txtDataPDP.Text = "" Then
            Response.Write("<SCRIPT>alert('Informe a  Data PDP.')</SCRIPT>")
            Exit Sub
        End If

        If cboEmpresa.SelectedItem.Value = "0" Then
            Response.Write("<SCRIPT>alert('Selecione a Empresa.')</SCRIPT>")
            Exit Sub
        End If

        If (txtDataInicial.Text = "" Xor txtHoraInicial.Text = "") Then
            Response.Write("<SCRIPT>alert('Data Hora Inicial deve ser Informada.')</SCRIPT>")
            Exit Sub
        End If

        If (txtDataFinal.Text = "" Xor txtHoraFinal.Text = "") Then
            Response.Write("<SCRIPT>alert('Data Hora Final deve ser Informada.')</SCRIPT>")
            Exit Sub
        End If

        If Not IsDate(strDataHoraInicial) Then
            Response.Write("<SCRIPT>alert('Data Hora Inicial Inválida.')</SCRIPT>")
            Exit Sub
        End If

        If Not IsDate(strDataHoraFinal) Then
            Response.Write("<SCRIPT>alert('Data Hora Final Inválida.')</SCRIPT>")
            Exit Sub
        End If

        Dim dataHoraInicial As DateTime = Convert.ToDateTime(strDataHoraInicial)
        Dim dataHoraFinal As DateTime = Convert.ToDateTime(strDataHoraFinal)

        If dataHoraInicial >= dataHoraFinal Then
            Response.Write("<SCRIPT>alert('Data Hora Final deve ser maior que Data Hora Inicial.')</SCRIPT>")
            Exit Sub
        End If


        Dim Conn As System.Data.SqlClient.SqlConnection = New System.Data.SqlClient.SqlConnection
        Conn.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
        Dim Cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand
        Cmd.Connection = Conn
        Try
            Dim strData As String = txtDataPDP.Text.Substring(6, 4) + txtDataPDP.Text.Substring(3, 2) + txtDataPDP.Text.Substring(0, 2)

            If (cboEmpresa.SelectedItem.Value = "Todas") Then

                Conn.Open()

                Cmd.CommandText = "Delete tb_controleagentepdp where datpdp = " & strData
                Cmd.ExecuteNonQuery()

                Cmd.CommandText = "Insert into tb_controleagentepdp ( codempre, datpdp, din_iniciopdp, din_fimpdp ) " & _
                                     "Select trim(e.codempre), " & _
                                     "'" & strData & "', " & _
                                     "'" & dataHoraInicial.ToString("yyyy-MM-dd HH:mm:ss") & "', " & _
                                     "'" & dataHoraFinal.ToString("yyyy-MM-dd HH:mm:ss") & "' " & _
                                     "From empre e " & _
                                     "Where e.flg_estudo = 'N'"
                Cmd.ExecuteNonQuery()


                Conn.Close()

            Else
                If Request.QueryString("Acao") = "A" Then

                    Cmd.CommandText = "Update tb_controleagentepdp " & _
                                        "Set din_iniciopdp = '" & dataHoraInicial.ToString("yyyy-MM-dd HH:mm:ss") & "',  " & _
                                        "    din_fimpdp = '" & dataHoraFinal.ToString("yyyy-MM-dd HH:mm:ss") & "' " & _
                                        "Where id_controleagentepdp = " & Request.QueryString("Id")
                Else


                    Cmd.CommandText = "Insert Into tb_controleagentepdp (" & _
                                        "datpdp, " & _
                                        "codempre, " & _
                                        "din_iniciopdp, " & _
                                        "din_fimpdp " & _
                                        ") Values (" & _
                                        "'" & strData & "', " & _
                                        "'" & cboEmpresa.SelectedItem.Value & "', " & _
                                        "'" & dataHoraInicial.ToString("yyyy-MM-dd HH:mm:ss") & "', " & _
                                        "'" & dataHoraFinal.ToString("yyyy-MM-dd HH:mm:ss") & "') "

                End If

                Conn.Open()
                Cmd.ExecuteNonQuery()
                Conn.Close()
            End If
        Catch ex As Exception
            Conn.Close()

            If ex.Message.ToUpper.IndexOf("UNIQUE CONSTRAINT") > 0 Then
                Session("strMensagem") = "Empresa já cadastrada para o dia " + txtDataPDP.Text
            Else
                Session("strMensagem") = "Erro ao salvar. (" + ex.Message + ")"
            End If

            Response.Redirect("frmMensagem.aspx")
        End Try

        Server.Transfer("frmControleAgente.aspx")

        'Else
        '    Session("strMensagem") = "Data inicial deve ser menor que a data final !"
        '    Response.Redirect("frmMensagem.aspx")
        'End If
        '    Else
        'Session("strMensagem") = "Data Inválida !"
        'Response.Redirect("frmMensagem.aspx")
        '    End If
    End Sub

    Protected Sub btnVoltar_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnVoltar.Click
        Response.Redirect("frmControleAgente.aspx?DataPDP=" + txtDataPDP.Text + "&codigoEmpresa=" + cboEmpresa.SelectedItem.Value)
    End Sub

    Protected Sub cboEmpresa_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboEmpresa.SelectedIndexChanged
        If cboEmpresa.SelectedIndex > 0 Then
            txtDataInicial.Text = DateTime.Now.ToString("dd/MM/yyyy")
            txtHoraInicial.Text = DateTime.Now.ToString("HH:mm")

            Dim dataHoraFinalAux As DateTime = Convert.ToDateTime(txtDataPDP.Text)
            dataHoraFinalAux = dataHoraFinalAux.AddDays(-1)
            txtDataFinal.Text = dataHoraFinalAux.ToString("dd/MM/yyyy")
            txtHoraFinal.Text = "11:00"

            'txtDataInicial.Text = txtDataPDP.Text
            'txtHoraInicial.Text = "00:00"
            'txtDataFinal.Text = txtDataPDP.Text
            'txtHoraFinal.Text = "14:00"
        Else
            txtDataInicial.Text = ""
            txtHoraInicial.Text = ""
            txtDataFinal.Text = ""
            txtHoraFinal.Text = ""
        End If
    End Sub
End Class