Partial Class frmColPotSinc
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents btnFechar As System.Web.UI.WebControls.ImageButton

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
            'Put user code to initialize the page here
            If Not Page.IsPostBack Then
                Dim intI As Integer
                Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
                Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
                Cmd.Connection = Conn

                Try
                    Conn.Open("rpdp")
                    Cmd.CommandText = "Select datpdp " & _
                                      "From pdp " & _
                                      "Order By datpdp Desc"
                    Dim rsData As OnsClasses.OnsData.OnsDataReader = Cmd.ExecuteReader
                    intI = 1
                    Dim objItem As WebControls.ListItem
                    objItem = New WebControls.ListItem
                    objItem.Text = ""
                    objItem.Value = "0"
                    cboData.Items.Add(objItem)
                    Do While rsData.Read
                        objItem = New System.Web.UI.WebControls.ListItem
                        objItem.Text = Mid(rsData("datpdp"), 7, 2) & "/" & Mid(rsData("datpdp"), 5, 2) & "/" & Mid(rsData("datpdp"), 1, 4)
                        objItem.Value = rsData("datpdp")
                        cboData.Items.Add(objItem)
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
        End If
    End Sub

    Private Sub PreencheTable()
        Dim intI, intJ As Integer
        Dim intLin As Integer
        Dim dblMedia As Double
        Dim strSql As String
        Dim objCel As TableCell
        Dim objRow As TableRow
        Dim objTamanho As WebControls.Unit
        Dim objConn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
        Dim objCmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
        objCmd.Connection = objConn
        Dim strCodPonto As String
        Dim objColor As System.Drawing.Color
        objColor = New System.Drawing.Color
        objColor = System.Drawing.ColorTranslator.FromWin32(RGB(233, 244, 207))

        Try
            'Seleciona os 48 valores passando para 24 valores
            objCmd.CommandText = CompactaSelect()

            objConn.Open("rpdp")
            Dim rsPotSinc As OnsClasses.OnsData.OnsDataReader = objCmd.ExecuteReader
            tblPotSinc.Rows.Clear()
            objTamanho = New WebControls.Unit

            objRow = New TableRow
            objRow.Width = objTamanho.Pixel(85)
            objRow.BackColor = Drawing.Color.YellowGreen
            objCel = New TableCell
            objCel.Wrap = False
            objCel.Text = "Instante"
            objCel.Font.Bold = True
            objCel.HorizontalAlign = HorizontalAlign.Center
            objCel.Width = objTamanho.Pixel(85)
            objRow.Controls.Add(objCel)

            tblPotSinc.Width = objTamanho.Pixel(85)
            tblPotSinc.Controls.Add(objRow)
            For intI = 1 To 24
                objRow = New TableRow
                objRow.Width = objTamanho.Pixel(85)
                If intI Mod 2 = 0 Then
                    objRow.BackColor = objColor
                End If
                objCel = New TableCell
                objCel.Font.Bold = True
                objCel.Width = objTamanho.Pixel(85)
                objCel.HorizontalAlign = HorizontalAlign.Center
                objCel.Text = intI.ToString.PadLeft(2, "0") & ":00"
                objRow.Controls.Add(objCel)
                tblPotSinc.Controls.Add(objRow)
            Next
            intI = 0

            objCel = New TableCell
            objCel.Wrap = False
            objCel.Width = objTamanho.Pixel(65)
            objCel.Font.Bold = True
            tblPotSinc.Rows(0).Width = objTamanho.Pixel(tblPotSinc.Rows(0).Width.Value + 65)
            tblPotSinc.Width = objTamanho.Pixel(tblPotSinc.Width.Value + 65)
            'objCel.Font.Size = FontUnit.XXSmall
            objCel.Text = "Pot Sinc"
            tblPotSinc.Rows(0).Controls.Add(objCel)
            intLin = 1
            For intJ = 1 To 24
                objCel = New TableCell
                objCel.Wrap = False
                objCel.Width = objTamanho.Pixel(65)
                objCel.HorizontalAlign = HorizontalAlign.Right
                objCel.Text = ""
                tblPotSinc.Rows(intLin).Width = objTamanho.Pixel(tblPotSinc.Rows(intLin).Width.Value + 65)
                tblPotSinc.Rows(intLin).Controls.Add(objCel)
                intLin += 1
            Next
            intI += 1
            intLin = 1
            Do While rsPotSinc.Read
                'Inseri as celulas com os valores dos pontos de conexão
                If Not IsDBNull(rsPotSinc("valpotsincronizadasup")) Then
                    tblPotSinc.Rows(intLin).Cells(1).Text = Format(rsPotSinc("valpotsincronizadasup"), "#####0")
                Else
                    tblPotSinc.Rows(intLin).Cells(1).Text = Format(0, "#####0")
                End If
                intLin += 1
            Loop
            rsPotSinc.Close()
            rsPotSinc = Nothing
            'objCmd.Connection.Close()
            'objConn.Close()
        Catch
            Session("strMensagem") = "Não foi possível acessar a Base de Dados, tente mais tarde."
            If objConn.State = ConnectionState.Open Then
                objConn.Close()
            End If
            Response.Redirect("frmMensagem.aspx")
        Finally
            If objConn.State = ConnectionState.Open Then
                objConn.Close()
            End If
        End Try
    End Sub

    Private Sub btnSalvar_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSalvar.Click
        Dim intI As Integer
        Dim intJ As Integer
        Dim intColAtual As Integer
        Dim intColAnterior As Integer
        Dim intQtdReg As Integer
        Dim intCol As Integer
        Dim intValor As Integer
        Dim strValor As String
        Dim intRetorno As Integer

        'Dim objPermissao As OnsClasses.OnsInterfaceUnica.OnsPermissao
        'objPermissao = New OnsClasses.OnsInterfaceUnica.OnsPermissao("UPD", "RPDPColPotSinc", UsuarID)
        'Verifica se o usuário tem permissão para salvar os registros
        If EstaAutorizado Then
            Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
            Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
            Cmd.Connection = Conn
            Dim objTrans As OnsClasses.OnsData.OnsTransaction
            Conn.Open("rpdp")
            Conn.Servico = "RPDPColPotSinc"
            Conn.Usuario = UsuarID

            objTrans = Conn.BeginTransaction()
            Cmd.Transaction = objTrans
            Dim datAtual As Date = Now
            strValor = Page.Request.Form.Item("txtValor")

            Try
                'Atualiza a grid
                If strValor = "" Then
                    'Quando o txtValor estiver em branco, branqueia tabela e a grid
                    For intI = 1 To 48
                        'Atualiza na BDT a Coluna Alterada
                        Cmd.CommandText = "Update potsincronizada " & _
                                          "Set valpotsincronizadasup = 0 " & _
                                          "Where codarea = 'NE' And " & _
                                          "      datPdp = '" & cboData.SelectedItem.Value.Trim & "' And " & _
                                          "      intpotsincronizada = " & intI
                        intRetorno = Cmd.ExecuteNonQuery()
                        If intRetorno = 0 Then
                            Cmd.CommandText = "Insert Into potsincronizada (" & _
                                              "datpdp, " & _
                                              "codarea, " & _
                                              "intpotsincronizada, " & _
                                              "valpotsincronizadasup " & _
                                              ") Values (" & _
                                              "'" & cboData.SelectedItem.Value.Trim & "', " & _
                                              "'NE', " & _
                                              "" & intI & ", " & _
                                              "" & "0" & ")"
                            Cmd.ExecuteNonQuery()
                        End If
                    Next
                Else
                    intI = 1
                    intColAnterior = 1
                    intColAtual = InStr(intColAnterior, strValor, Chr(13), CompareMethod.Binary)
                    For intI = 1 To 24
                        If intColAtual <> 0 Then
                            If Mid(strValor, intColAnterior, (intColAtual - intColAnterior) + 1) <> "" Then
                                'Atualiza na BDT a Coluna Alterada
                                intValor = Val(Mid(strValor, intColAnterior, (intColAtual - intColAnterior) + 1))
                            End If
                            intColAnterior = intColAtual
                        ElseIf intColAtual = 0 And Mid(strValor, intColAnterior) <> "" Then
                            'Não tem ENTER (chr(13)) no final do texto
                            intValor = Val(Mid(strValor, intColAnterior))
                            intColAnterior = intColAnterior + Trim(Mid(strValor, intColAnterior)).Length
                        Else
                            intValor = 0
                        End If

                        'Gravação do Intervalo (inti X 2) - 1, onde inti (1 a 24)
                        Cmd.CommandText = "Update potsincronizada " & _
                                          "Set valpotsincronizadasup = " & intValor & " " & _
                                          "Where codarea = 'NE' And " & _
                                          "      datPdp = '" & cboData.SelectedItem.Value.Trim & "' And " & _
                                          "      intpotsincronizada = " & ((intI * 2) - 1)
                        intRetorno = Cmd.ExecuteNonQuery()
                        If intRetorno = 0 Then
                            Cmd.CommandText = "Insert Into potsincronizada (" & _
                                              "datpdp, " & _
                                              "codarea, " & _
                                              "intpotsincronizada, " & _
                                              "valpotsincronizadasup " & _
                                              ") Values (" & _
                                              "'" & cboData.SelectedItem.Value.Trim & "', " & _
                                              "'NE', " & _
                                              "" & ((intI * 2) - 1) & ", " & _
                                              "" & intValor & ")"
                            Cmd.ExecuteNonQuery()
                        End If

                        'Gravação do Intervalo (inti X 2), onde inti (1 a 24)
                        Cmd.CommandText = "Update potsincronizada " & _
                                          "Set valpotsincronizadasup = " & intValor & " " & _
                                          "Where codarea = 'NE' And " & _
                                          "      datPdp = '" & cboData.SelectedItem.Value.Trim & "' And " & _
                                          "      intpotsincronizada = " & (intI * 2)
                        intRetorno = Cmd.ExecuteNonQuery()
                        If intRetorno = 0 Then
                            Cmd.CommandText = "Insert Into potsincronizada (" & _
                                              "datpdp, " & _
                                              "codarea, " & _
                                              "intpotsincronizada, " & _
                                              "valpotsincronizadasup " & _
                                              ") Values (" & _
                                              "'" & cboData.SelectedItem.Value.Trim & "', " & _
                                              "'NE', " & _
                                              "" & (intI * 2) & ", " & _
                                              "" & intValor & ")"
                            Cmd.ExecuteNonQuery()
                        End If
                        intColAtual = InStr(intColAnterior + 1, strValor, Chr(13), CompareMethod.Binary)
                    Next
                End If
                'Grava toda a transação
                objTrans.Commit()
            Catch
                'houve erro, aborta a transação e fecha a conexão
                Session("strMensagem") = "Não foi possível gravar os dados."
                objTrans.Rollback()
                If Conn.State = ConnectionState.Open Then
                    Conn.Close()
                End If
                'Chama a tela de mensagem
                Response.Redirect("frmMensagem.aspx")
            Finally
                If Conn.State = ConnectionState.Open Then
                    Conn.Close()
                End If
            End Try

            Try
                'fecha a conexão com o banco
                'Cmd.Connection.Close()
                'Conn.Close()
                'tblTexto.Visible = False
                divValor.Visible = False
                btnSalvar.Visible = False
                PreencheTable()
            Catch
                Session("strMensagem") = "Não foi possível acessar a Base de Dados."
                'Conn.Close()
                Response.Redirect("frmMensagem.aspx")
            End Try
        Else
            Session("strMensagem") = "Usuário não tem permissão para alterar os valores."
            Response.Redirect("frmMensagem.aspx")
        End If
    End Sub

    Private Sub cboData_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboData.SelectedIndexChanged
        If cboData.SelectedIndex <> 0 Then
            PreencheTexto()
            PreencheTable()
        End If
    End Sub

    Private Sub PreencheTexto()
        If cboData.SelectedIndex > 0 Then
            Dim intI As Integer
            Dim strLista As String
            Dim strSql As String
            Dim datTemp As DateTime
            Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
            Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
            Cmd.Connection = Conn
            Try
                Conn.Open("rpdp")

                'Seleciona os 48 valores compactando para 24
                strSql = CompactaSelect()

                Cmd.CommandText = strSql
                Dim rsPotSinc As OnsClasses.OnsData.OnsDataReader = Cmd.ExecuteReader

                'Colocando os valores de carga no text para alteração
                Dim objTextArea As HtmlControls.HtmlTextArea
                objTextArea = New HtmlTextArea
                objTextArea.Rows = 27
                objTextArea.ID = "txtValor"
                objTextArea.Attributes.Item("onkeyup") = "RetiraEnter(event)"
                objTextArea.Attributes.Item("runat") = "server"
                objTextArea.Attributes.Item("style") = "font-size:x-small;line-height:20px;height:500px;width:85px;"

                Dim blnPassou As Boolean = False
                Do While rsPotSinc.Read
                    If blnPassou Then
                        objTextArea.Value &= Chr(13)
                    End If

                    If Not IsDBNull(rsPotSinc("valpotsincronizadasup")) Then
                        objTextArea.Value &= Format(rsPotSinc("valpotsincronizadasup"), "#####0")
                    Else
                        objTextArea.Value &= 0
                    End If

                    'na primeira passagem não escreve TAB nem ENTER
                    blnPassou = True
                Loop
                divValor.Controls.Add(objTextArea)
                divValor.Style.Item("TOP") = "193px"
                divValor.Style.Item("LEFT") = "108px"

                divValor.Visible = True
                rsPotSinc.Close()
                rsPotSinc = Nothing
                'Cmd.Connection.Close()
                'Conn.Close()
                btnSalvar.Visible = True
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

    Private Function CompactaSelect()
        Dim inti As Int16
        Dim strsql As String
        For inti = 1 To 24
            strsql &= "Select " & inti & " As intpotsincronizada, Avg(valpotsincronizadasup) As valpotsincronizadasup " & _
                      "From potsincronizada " & _
                      "Where datpdp = '" & cboData.SelectedItem.Value.Trim & "' And codarea = 'NE' And intpotsincronizada In (" & ((inti * 2) - 1) & "," & (inti * 2) & ") " & _
                      "Union "
        Next
        'retira o último Union e coloca o Order By
        CompactaSelect = (strsql.Substring(0, strsql.Length() - 6) & " Order By 1")
    End Function
End Class


