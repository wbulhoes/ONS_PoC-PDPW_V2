Public Class frmControleAgente
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Put user code to initialize the page here
        'objMenu = CType(Session("onsmenu"), OnsWebControls.OnsMenu)
        If Not Page.IsPostBack Then
            Dim Conn As System.Data.SqlClient.SqlConnection = New System.Data.SqlClient.SqlConnection
            Conn.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
            Dim Cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand
            'If Session("datEscolhida") = Nothing Then
            '    'Inicializa a variável com data do próximo
            '    Session("datEscolhida") = Now.AddDays(1)
            'End If
            Cmd.Connection = Conn
            Try
                Conn.Open()
                Cmd.CommandText = "Select datpdp " & _
                                "From pdp " & _
                                "Order By datpdp Desc"
                Dim rsData As System.Data.SqlClient.SqlDataReader = Cmd.ExecuteReader
                Dim objItem As WebControls.ListItem
                objItem = New WebControls.ListItem
                objItem.Text = ""
                objItem.Value = "0"
                cboData.Items.Add(objItem)
                Do While rsData.Read
                    objItem = New System.Web.UI.WebControls.ListItem
                    objItem.Text = Mid(rsData("datpdp"), 7, 2) & "/" & Mid(rsData("datpdp"), 5, 2) & "/" & Mid(rsData("datpdp"), 1, 4)
                    objItem.Value = rsData("datpdp").ToString()
                    cboData.Items.Add(objItem)
                Loop

                rsData.Close()
                rsData = Nothing

                If Request.QueryString("DataPDP") <> Nothing Then
                    cboData.Items.FindByText(Trim(Request.QueryString("DataPDP"))).Selected = True
                End If

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
                    cboEmpresa.Items.FindByValue(Trim(Request.QueryString("codigoEmpresa"))).Selected = True
                End If
                'cboEmpresa_SelectedIndexChanged(sender, e)

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

        PreencheTable()

    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        If SessaoAtiva(Page.Session) Then
            MyBase.Render(writer)
            'objMenu.RenderControl(writer)
        End If
    End Sub
    Protected Sub btnExcluir_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnExcluir.Click
        Dim intI As Integer
        Dim objCheckBox As System.Web.UI.WebControls.CheckBox
        Dim objHiddenAux As System.Web.UI.WebControls.HiddenField
        Dim Conn As System.Data.SqlClient.SqlConnection = New System.Data.SqlClient.SqlConnection
        Conn.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
        Dim Cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand
        Cmd.Connection = Conn

        Dim isChecked As Boolean = False
        Try
            Conn.Open()
            objCheckBox = New System.Web.UI.WebControls.CheckBox

            Dim listStrId As String = ""

            For intI = 1 To tblDados.Rows.Count - 1
                objCheckBox = tblDados.Rows(intI).Cells(0).Controls(0)
                If objCheckBox.Checked = True Then

                    isChecked = True

                    objHiddenAux = tblDados.Rows(intI).Cells(1).Controls(0)
                    Dim Dados() As String = objHiddenAux.Value.Split("|"c)

                    listStrId = listStrId + Dados(0) + ","
                End If
            Next

            If Not isChecked Then
                Response.Write("<SCRIPT>alert('Selecione um item.')</SCRIPT>")
                Exit Sub
            Else
                Dim listStrIdAux As String = listStrId.Remove(listStrId.Length - 1, 1)
                listStrIdAux = "(" + listStrIdAux + ")"
                Cmd.CommandText = "Delete From tb_controleagentepdp Where id_controleagentepdp in " & listStrIdAux
                Cmd.ExecuteNonQuery()
            End If

            Conn.Close()
            PreencheTable()
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

    Protected Sub btnAlterar_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnAlterar.Click
        Dim objCheckBox As System.Web.UI.WebControls.CheckBox
        Dim objHiddenAux As System.Web.UI.WebControls.HiddenField
        Dim dtIni As String = ""
        Dim dtFim As String = ""
        Dim intI As Integer
        objCheckBox = New System.Web.UI.WebControls.CheckBox
        Dim isChecked As Integer  = False

        If (cboEmpresa.SelectedItem.Value <> "Todas") Then
            For intI = 1 To tblDados.Rows.Count - 1
                objCheckBox = tblDados.Rows(intI).Cells(0).Controls(0)
                If objCheckBox.Checked = True Then

                    isChecked = True

                    objHiddenAux = tblDados.Rows(intI).Cells(1).Controls(0)
                    Dim Dados() As String = objHiddenAux.Value.Split("|"c)

                    Response.Redirect("frmControleAgenteCad.aspx?Acao=A&DataPDP=" + cboData.SelectedItem.Text + "&Id=" + Dados(0) + "&codigoEmpresa=" + Dados(1).Trim() + "&dtHoraIni=" + Dados(2) + "&dtHoraFim=" + Dados(3))
                End If
            Next

            If Not isChecked Then
                Response.Write("<SCRIPT>alert('Selecione um item.')</SCRIPT>")
                Exit Sub
            End If
        Else
            Dim achou As Boolean = False
            Dim DadosAux() As String
            For intI = 1 To tblDados.Rows.Count - 1
                objCheckBox = tblDados.Rows(intI).Cells(0).Controls(0)

                objHiddenAux = tblDados.Rows(intI).Cells(1).Controls(0)
                DadosAux = objHiddenAux.Value.Split("|"c)

                If objCheckBox.Checked = True Then
                    Response.Redirect("frmControleAgenteCad.aspx?Acao=A&DataPDP=" + cboData.SelectedItem.Text + "&Id=" + DadosAux(0) + "&codigoEmpresa=" + DadosAux(1).Trim() + "&dtHoraIni=" + DadosAux(2) + "&dtHoraFim=" + DadosAux(3))
                End If

                achou = True

            Next

            If achou Then
                Response.Redirect("frmControleAgenteCad.aspx?Acao=A&DataPDP=" + cboData.SelectedItem.Text + "&Id=-1&codigoEmpresa=Todas&dtHoraIni=" + DadosAux(2) + "&dtHoraFim=" + DadosAux(3))
            Else
                Response.Write("<SCRIPT>alert('Nenhum item encontrado.')</SCRIPT>")
                Exit Sub
            End If

        End If
    End Sub

    Protected Sub btnIncluir_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnIncluir.Click

        If cboData.SelectedItem.Value = "0" Then
            Response.Write("<SCRIPT>alert('Selecione a Data PDP.')</SCRIPT>")
            Exit Sub
        End If

        Response.Redirect("frmControleAgenteCad.aspx?Acao=I&DataPDP=" + cboData.SelectedItem.Text + "&codigoEmpresa=" + cboEmpresa.SelectedItem.Value)
    End Sub

    Protected Sub cboData_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboData.SelectedIndexChanged
        'PreencheTable()
    End Sub

    Protected Sub cboEmpresa_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboEmpresa.SelectedIndexChanged
        PreencheTable()
    End Sub

    Private Sub PreencheTable()
        If cboData.SelectedItem.Value <> "0" Then
            Dim intRow, intCol As Integer
            Dim objCheck As CheckBox
            Dim objRow As TableRow
            Dim objCell As TableCell

            Dim objHidden As HiddenField

            'limpa a tabela
            tblDados.Rows.Clear()

            objRow = New TableRow
            objRow.BackColor = System.Drawing.Color.YellowGreen
            For intCol = 1 To 3
                'nova Celula
                objCell = New TableCell
                Select Case intCol
                    'Case Is = 1
                    'objCell.Controls.Add(New LiteralControl("Código"))
                    'Case Is = 2
                    'objCell.Controls.Add(New LiteralControl("Data PDP"))
                    Case Is = 1
                        objCell.Controls.Add(New LiteralControl("Empresa"))
                    Case Is = 2
                        objCell.Controls.Add(New LiteralControl("Data Hora Início"))
                    Case Is = 3
                        objCell.Controls.Add(New LiteralControl("Data Hora Fim"))
                End Select
                objRow.Controls.Add(objCell)
            Next

            tblDados.Rows.Add(objRow)

            If (cboEmpresa.SelectedItem.Value <> "0") Then

                Dim Conn As System.Data.SqlClient.SqlConnection = New System.Data.SqlClient.SqlConnection
                Conn.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
                Dim Cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand
                Cmd.Connection = Conn
                Try
                    Dim clausulaAgente As String = ""
                    If (cboEmpresa.SelectedItem.Value <> "Todas") Then
                        clausulaAgente = " and tb_controleagentepdp.codempre = '" & Trim(cboEmpresa.SelectedItem.Value) & "' "
                    End If

                    Cmd.CommandText = "Select tb_controleagentepdp.id_controleagentepdp, " &
                                        "      tb_controleagentepdp.datpdp, " &
                                        "      tb_controleagentepdp.codempre, " &
                                        "      empre.nomempre, " &
                                        "      tb_controleagentepdp.din_iniciopdp, " &
                                        "      tb_controleagentepdp.din_fimpdp " &
                                        "From tb_controleagentepdp, " &
                                        "     empre " &
                                        "Where tb_controleagentepdp.codempre = empre.codempre and " &
                                        "      tb_controleagentepdp.datpdp = '" & cboData.SelectedItem.Value & "' " & clausulaAgente &
                                        "Order By empre.nomempre"

                    Conn.Open()
                    Dim _reader As System.Data.SqlClient.SqlDataReader = Cmd.ExecuteReader

                    'Colocar cor na tabela (RGB(233, 244, 207) = cor padrão ONS)
                    Dim Color As System.Drawing.Color
                    Color = New System.Drawing.Color
                    Color = System.Drawing.ColorTranslator.FromWin32(RGB(233, 244, 207))
                    intRow = 1
                    Do While _reader.Read
                        'Cria um novo check box
                        objCheck = New CheckBox
                        objCheck.ID = _reader("id_controleagentepdp")

                        Dim id As String = _reader.GetInt32(0).ToString()
                        Dim codEmpresa As String = _reader.GetString(2).ToString().Trim()
                        Dim empresa As String = _reader.GetString(3).ToString().Trim()
                        Dim dataHoraIni As DateTime = _reader.GetDateTime(4)
                        Dim dataHoraFim As DateTime = _reader.GetDateTime(5)

                        objHidden = New HiddenField
                        'objHidden.ID = id
                        objHidden.Value = id + "|" + codEmpresa + "|" + dataHoraIni.ToString("dd/MM/yyyy HH:mm") + "|" + dataHoraFim.ToString("dd/MM/yyyy HH:mm")
                        ' Id+ codEmpresa + DataHoraIni + DataHoraFim

                        'Nova linha da tabela
                        objRow = New TableRow
                        If intRow Mod 2 = 0 Then
                            'quanto linha = par troca cor
                            objRow.BackColor = Color
                        End If


                        For intCol = 1 To 3
                            'nova Celula
                            objCell = New TableCell

                            Select Case intCol
                                'Case Is = 1
                                'objCell.Controls.Add(objCheck)
                                'objCell.Controls.Add(New LiteralControl(id))
                                Case Is = 1
                                    objCell.Controls.Add(objCheck)
                                    objCell.Controls.Add(New LiteralControl(empresa))
                                Case Is = 2
                                    objCell.Controls.Add(objHidden)
                                    objCell.Controls.Add(New LiteralControl(dataHoraIni.ToString("dd/MM/yyyy HH:mm")))
                                Case Is = 3
                                    objCell.Controls.Add(New LiteralControl(dataHoraFim.ToString("dd/MM/yyyy HH:mm")))
                                    'Case Is = 5
                                    'objCell.Controls.Add(New LiteralControl(arrHorario(Val(_reader("intinirestr")))))
                            End Select
                            objRow.Controls.Add(objCell)
                        Next
                        'Adiciona a linha a tabela
                        tblDados.Rows.Add(objRow)
                        intRow = intRow + 1

                        'objCheck.Enabled = cboEmpresa.SelectedItem.Value <> "Todas"
                        'objCheck.Checked = cboEmpresa.SelectedItem.Value = "Todas"
                    Loop
                    _reader.Close()
                    _reader = Nothing
                    'Cmd.Connection.Close()
                    'Conn.Close()
                Catch ex As Exception
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

        End If

    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button1.Click

    End Sub
End Class