Imports System.Data.SqlClient

Partial Class frmEnviaDadosEst
    Inherits System.Web.UI.Page
    Protected strOpcao As String

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
        'Put user code to initialize the page here
        'objMenu = CType(Session("onsmenu"), OnsWebControls.OnsMenu)
        If Not Page.IsPostBack Then
            Try
                divCal.Visible = False
                PreencheComboEmpresaPOP(AgentesRepresentados, cboEmpresa, Session("strCodEmpre"), "S")
                If cboEmpresa.SelectedIndex > 0 Then
                    cboEmpresa_SelectedIndexChanged(sender, e)
                End If
            Catch
                Session("strMensagem") = "Não foi possível acessar os dados, tente novamente."
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

    Private Sub cboEmpresa_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboEmpresa.SelectedIndexChanged

        If IsDate(txtData.Text) And cboEmpresa.SelectedIndex > 0 Then
            PreencheCabecalho()
            PreencheTable()
            VerificaDigitacao()
        End If
    End Sub

    Private Sub PreencheCabecalho()
        Dim objRow As TableRow
        Dim objCell As TableCell

        'limpa a tabela
        tblMensa.Rows.Clear()
        Dim objTamanho As System.Web.UI.WebControls.Unit
        objTamanho = New Unit

        objRow = New TableRow
        objRow.BackColor = System.Drawing.Color.YellowGreen
        objRow.Font.Bold = True

        'Descrição
        objCell = New TableCell
        objCell.Width = objTamanho.Pixel(260)
        objCell.Text = "Descrição"
        objRow.Controls.Add(objCell)

        'Situação
        objCell = New TableCell
        objCell.Width = objTamanho.Pixel(80)
        objCell.Text = "Situação"
        objRow.Controls.Add(objCell)

        'Carga
        objCell = New TableCell
        objCell.Width = objTamanho.Pixel(80)
        objCell.Text = "Carga"
        objRow.Controls.Add(objCell)

        'Intercâmbio
        objCell = New TableCell
        objCell.Width = objTamanho.Pixel(80)
        objCell.Text = "Intercâmbio"
        objRow.Controls.Add(objCell)

        'Geração
        objCell = New TableCell
        objCell.Width = objTamanho.Pixel(80)
        objCell.Text = "Geração"
        objRow.Controls.Add(objCell)

        tblMensa.Rows.Add(objRow)
    End Sub

    Private Sub VerificaDigitacao()
        Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
        Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
        Cmd.Connection = Conn
        Conn.Open("pdp")
        Try

            chkCarga.Checked = False
            chkIntercambio.Checked = False

            'Verifica se exitem os eventos 
            Cmd.CommandText = "SELECT codstatu " &
                            "FROM eventpdp " &
                            "WHERE datpdp = '" & Format(CDate(txtData.Text), "yyyyMMdd") & "' " &
                            "AND cmpevent = '" & cboEmpresa.SelectedItem.Text & "' " &
                            "AND (codstatu = '7' " &
                            "OR codstatu = '8' " &
                            "OR codstatu = '9') " &
                            "GROUP BY codstatu"
            Dim rsEvento As OnsClasses.OnsData.OnsDataReader = Cmd.ExecuteReader

            Do While rsEvento.Read
                Select Case rsEvento("codstatu")
                    Case Is = 7
                        chkGeracao.Checked = True
                    Case Is = 8
                        chkCarga.Checked = True
                    Case Is = 9
                        chkIntercambio.Checked = True
                End Select
            Loop

            If Not chkCarga.Checked And Not chkIntercambio.Checked And Not chkGeracao.Checked Then
                btnEnviar.Enabled = False
            Else
                btnEnviar.Enabled = True
            End If
            rsEvento.Close()
            'Cmd.Connection.Close()
            'Conn.Close()
        Catch ex As Exception
            Session("strMensagem") = "Erro ao acessar banco de dados, por favor tente novamente ou comunique a ocorrência ao ONS."
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

    Private Sub EnviarDados(ByVal strDataHora As String)
        Dim strData As String
        'Dim objPermissao As OnsClasses.OnsInterfaceUnica.OnsPermissao
        Dim strArq As String = Server.MapPath("")
        Dim bRetorno As Boolean = False
        Dim strNomeArquivo As String

        'objPermissao = New OnsClasses.OnsInterfaceUnica.OnsPermissao("INS", "PDPEnvDadoEst", UsuarID)
        'Verifica se o usuário tem permissão para salvar os registros
        If EstaAutorizado Then
            'objPermissao = New OnsClasses.OnsInterfaceUnica.OnsPermissao("DEL", "PDPEnvDadoEst", UsuarID)
            'Verifica se o usuário tem permissão para salvar os registros
            If EstaAutorizado Then
                Dim dblCarga, dblIntercambio, dblGeracao As Double
                Dim objTrans As SqlTransaction
                Dim Conn As SqlConnection = New SqlConnection
                Dim Cmd As SqlCommand = New SqlCommand
                Conn.Open()
                Cmd.Connection = Conn

                Dim intI As Integer
                Dim ConnIns As SqlConnection = New SqlConnection
                Dim CmdIns As SqlCommand = New SqlCommand
                ConnIns.Open()
                CmdIns.Connection = ConnIns

                dblCarga = 0
                dblIntercambio = 0
                dblGeracao = 0

                strOpcao = ""
                objTrans = Conn.BeginTransaction
                Cmd.Transaction = objTrans

                Try

                    strOpcao = ""

                    'GERAÇÃO
                    If chkGeracao.Checked Then
                        strOpcao &= "1" '3
                        Cmd.CommandText = "SELECT SUM(d.valdespa) AS valGeracao " &
                                          "FROM tb_despaestudo d, usina u " &
                                          "WHERE u.codempre = '" & cboEmpresa.SelectedValue & "' " &
                                          "AND u.codusina = d.codusina " &
                                          "AND d.datpdp = '" & Format(CDate(txtData.Text), "yyyyMMdd") & "'"
                        Dim rsGeracao As SqlDataReader = Cmd.ExecuteReader

                        If rsGeracao.Read Then
                            dblGeracao = IIf(Not IsDBNull(rsGeracao("valGeracao")), rsGeracao("valGeracao"), 0)
                        End If
                        rsGeracao.Close()
                        rsGeracao = Nothing
                    Else
                        strOpcao &= "0"
                    End If

                    'CARGA
                    If chkCarga.Checked Then
                        strOpcao &= "1" '1
                        Cmd.CommandText = "SELECT SUM(valcarga) AS valCarga " &
                                          "FROM tb_cargaestudo " &
                                          "WHERE codempre = '" & cboEmpresa.SelectedItem.Value & "' " &
                                          "AND datpdp = '" & Format(CDate(txtData.Text), "yyyyMMdd") & "'"
                        Dim rsCarga As SqlDataReader = Cmd.ExecuteReader
                        If rsCarga.Read Then
                            dblCarga = IIf(Not IsDBNull(rsCarga("valCarga")), rsCarga("valCarga"), 0)
                        End If
                        rsCarga.Close()
                        rsCarga = Nothing
                    Else
                        strOpcao &= "0"
                    End If

                    'INTERCÂMBIO
                    If chkIntercambio.Checked Then
                        strOpcao &= "1" '2
                        Cmd.CommandText = "SELECT SUM(valinter) AS valIntercambio " &
                                          "FROM tb_interestudo " &
                                          "WHERE datpdp = '" & Format(CDate(txtData.Text), "yyyyMMdd") & "' " &
                                          "AND codemprede = '" & cboEmpresa.SelectedItem.Value & "'"
                        Dim rsIntercambio As SqlDataReader = Cmd.ExecuteReader
                        If rsIntercambio.Read Then
                            dblIntercambio = IIf(Not IsDBNull(rsIntercambio("valIntercambio")), rsIntercambio("valIntercambio"), 0)
                        End If
                        rsIntercambio.Close()
                        rsIntercambio = Nothing
                    Else
                        strOpcao &= "0"
                    End If

                    strOpcao &= "00000000000000000000000"

                    strNomeArquivo = Trim(cboEmpresa.SelectedItem.Value) & "-ON " & Mid(strDataHora, 1, 8) & "-" & Mid(strDataHora, 9) & " WEB"
                    Cmd.CommandText = "INSERT INTO Mensa (datpdp, dthmensa, codempre, nomarq, indrecenv, sitmensa, totcarga, totinter, totdespa, usuar_id " &
                                      ") VALUES (" &
                                      "'" & Format(CDate(txtData.Text), "yyyyMMdd") & "', " &
                                      "'" & Format(Now, "yyyyMMddHHmmss") & "', " &
                                      "'" & Trim(cboEmpresa.SelectedItem.Value) & "', " &
                                      "'" & Trim(cboEmpresa.SelectedItem.Value) & "-ON " & Mid(strDataHora, 1, 8) & "-" & Mid(strDataHora, 9) & " WEB" & "', " &
                                      "'" & "R" & "', " &
                                      "'" & "Processado" & "', " &
                                      "" & CType((dblCarga / 2), Integer) & ", " &
                                      "" & CType((dblIntercambio / 2), Integer) & ", " &
                                      "" & CType((dblGeracao / 2), Integer) & ", " &
                                      "'" & UsuarID & "')"
                    Cmd.ExecuteNonQuery()

                    GravaEventoPDP("11", Format(CDate(txtData.Text), "yyyyMMdd"), cboEmpresa.SelectedItem.Text, DateAdd(DateInterval.Second, 1, Now), "PDPEnvDadoEst", UsuarID)

                    'Grava arquivo para a empresa selecionada na data selecionada
                    strData = Mid(txtData.Text, 7, 4) & Mid(txtData.Text, 4, 2) & Mid(txtData.Text, 1, 2)

                    Dim dadosarquivo As New GeracaoArquivo
                    dadosarquivo.strCodEmpresa = cboEmpresa.SelectedItem.Value
                    dadosarquivo.strData = strData
                    dadosarquivo.strArq = strArq
                    dadosarquivo.blnTipoEnvio = "3"
                    dadosarquivo.bFtp = True
                    dadosarquivo.bRetorno = bRetorno
                    dadosarquivo.strOpcao = strOpcao
                    dadosarquivo.strDataHora = strDataHora
                    dadosarquivo.strArqDestino = ""
                    dadosarquivo.Cmd = Cmd
                    dadosarquivo.strPerfil = PerfilID

                    If Not GravaArquivoTexto(dadosarquivo, bRetorno) Then
                        'Session("strMensagem") = "Não foi possível gravar o arquivo texto, os dados podem estar incorretos. "
                        Session("strMensagem") = "Não foi possivel enviar os dados, por favor tente novamente ou comunique a ocorrência ao ONS."
                        Response.Redirect("frmMensagem.aspx")
                    Else
                        ' Conforme definição da Marta, após enviar o arquivo para área de FTP
                        ' apagá-lo do diretório Download

                        If bRetorno = True Then
                            Response.Write("<script language=JavaScript>")
                            Response.Write("window.open('frmReciboEst.aspx?strNomeArquivo=" & Trim(strNomeArquivo) & "' , '', 'width=700,height=270,status=no,scrollbars=no,titlebar=yes,menubar=no')")
                            Response.Write("</script>")
                        Else
                            Session("strMensagem") = "Não foi possivel enviar os dados, por favor tente novamente ou comunique a ocorrência ao ONS."
                            Response.Redirect("frmMensagem.aspx")
                        End If
                    End If

                    'Grava Transação
                    objTrans.Commit()

                    Cmd.Dispose()
                    CmdIns.Dispose()
                    Conn.Close()
                    Conn.Dispose()
                    ConnIns.Close()
                    ConnIns.Dispose()

                    PreencheCabecalho()
                    PreencheTable()
                Catch ex As Exception
                    Try
                        'se a transação estiver fechada vai ocorrer erro
                        objTrans.Rollback()
                    Catch
                    End Try
                    Cmd.Dispose()
                    CmdIns.Dispose()
                    Conn.Close()
                    Conn.Dispose()
                    ConnIns.Close()
                    ConnIns.Dispose()
                    Session("strMensagem") = "Não foi possivel enviar os dados, por favor tente novamente ou comunique a ocorrência ao ONS."
                    Response.Redirect("frmMensagem.aspx")
                End Try
            Else
                Session("strMensagem") = "Usuário não tem permissão para Transferir os valores."
                Response.Redirect("frmMensagem.aspx")
            End If
        Else
            Session("strMensagem") = "Usuário não tem permissão para Transferir os valores."
            Response.Redirect("frmMensagem.aspx")
        End If
    End Sub

    Private Sub PreencheTable()
        Dim intRow, intCol, intArq As Integer
        Dim objRow As TableRow
        Dim objCell As TableCell

        Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
        Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
        Cmd.Connection = Conn

        Cmd.CommandText = "SELECT nomarq, dthmensa, sitmensa, totcarga, totinter, totdespa " &
                          "FROM mensa " &
                          "WHERE codempre = '" & cboEmpresa.SelectedValue & "' " &
                          "AND datpdp = '" & Format(CDate(txtData.Text), "yyyyMMdd") & "' " &
                          "ORDER BY dthmensa DESC"
        Conn.Open("pdp")
        Dim rsMensa As OnsClasses.OnsData.OnsDataReader = Cmd.ExecuteReader

        'Colocar cor na tabela (RGB(233, 244, 207) = cor padrão ONS)
        Dim Color As System.Drawing.Color
        Color = New System.Drawing.Color
        Color = System.Drawing.ColorTranslator.FromWin32(RGB(233, 244, 207))
        intRow = 1

        Do While rsMensa.Read
            'Nova linha da tabela
            objRow = New TableRow
            If intRow Mod 2 = 0 Then
                'quando linha = par troca cor
                objRow.BackColor = Color
            End If

            'Descrição
            objCell = New TableCell
            objCell.Wrap = False
            objCell.Text = rsMensa("nomarq")
            objRow.Controls.Add(objCell)

            'Situação
            objCell = New TableCell
            objCell.Wrap = False
            objCell.Text = rsMensa("sitmensa")
            objRow.Controls.Add(objCell)

            'Carga
            objCell = New TableCell
            objCell.Wrap = False
            objCell.Text = IIf(Not IsDBNull(rsMensa("totcarga")), rsMensa("totcarga"), 0)
            objRow.Controls.Add(objCell)

            'Intercâmbio
            objCell = New TableCell
            objCell.Wrap = False
            objCell.Text = IIf(Not IsDBNull(rsMensa("totinter")), rsMensa("totinter"), 0)
            objRow.Controls.Add(objCell)

            'Geração
            objCell = New TableCell
            objCell.Wrap = False
            objCell.Text = IIf(Not IsDBNull(rsMensa("totdespa")), rsMensa("totdespa"), 0)
            objRow.Controls.Add(objCell)

            'Adiciona a linha a tabela
            tblMensa.Rows.Add(objRow)
            intRow = intRow + 1
        Loop
        rsMensa.Close()
        Conn.Close()
    End Sub

    Private Sub btnEnviar_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnEnviar.Click
        Dim strDataHora As String

        'O Horário deve ser o mesmo na hora de criar o registro na mensa e gerar o arquivo texto
        strDataHora = Format(Now, "yyyyMMddHHmmss")
        EnviarDados(strDataHora)

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
        VerificaDigitacao()
        PreencheCabecalho()
        PreencheTable()
    End Sub

End Class
