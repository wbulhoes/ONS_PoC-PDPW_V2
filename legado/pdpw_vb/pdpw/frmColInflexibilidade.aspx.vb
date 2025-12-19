Imports System.Collections.Generic
Imports System.Data.SqlClient

Partial Class frmColInflexibilidade

    Inherits System.Web.UI.Page

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

        If Not Page.IsPostBack Then
            Dim intI As Integer
            If Session("datEscolhida") = Nothing Then
                'Inicializa a variável com data do próximo
                Session("datEscolhida") = Now.AddDays(1)
            End If

            Dim pdpData As List(Of ListItem) = CacheDataPDP.GetPdpData(True)

            cboData.Items.Clear()

            Dim objItem As New WebControls.ListItem
            objItem.Text = ""
            objItem.Value = "0"
            cboData.Items.Add(objItem)

            intI = 0
            For Each item As WebControls.ListItem In pdpData
                cboData.Items.Add(item)
            Next

            Dim selectedValue As String = Format(Session("datEscolhida"), "dd/MM/yyyy")
            Dim itemIndex As Integer = 0
            Dim found As Boolean = False

            For Each item As WebControls.ListItem In cboData.Items
                If Trim(item.Value) = selectedValue Then
                    cboData.SelectedIndex = itemIndex
                    found = True
                    Exit For
                End If
                itemIndex += 1
            Next

            PreencheComboEmpresaPOP(AgentesRepresentados, cboEmpresa, Session("strCodEmpre"))

            If cboData.SelectedIndex > 0 Then
                cboData_SelectedIndexChanged(sender, e)
            End If

            lblMsg.Visible = False
        End If
    End Sub


    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        If SessaoAtiva(Page.Session) Then
            MyBase.Render(writer)
            'objMenu.RenderControl(writer)
        End If
    End Sub

    Private Sub cboEmpresa_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboEmpresa.SelectedIndexChanged
        Dim intI As Integer
        Dim Conn As SqlConnection = New SqlConnection
        Dim Cmd As SqlCommand = New SqlCommand
        Cmd.Connection = Conn
        Dim strCodUsina As String
        'tblTexto.Visible = False
        divValor.Visible = False
        btnSalvar.Visible = False
        If cboEmpresa.SelectedIndex > 0 Then
            Session("strCodEmpre") = cboEmpresa.SelectedItem.Value
        End If

        Try
            Cmd.CommandText = "Select d.codusina, " &
                                "       u.ordem " &
                                "From inflexibilidade d, " &
                                "     usina u " &
                                "Where u.codempre = '" & Session("strCodEmpre") & "' And " &
                                "      u.codusina = d.codusina And " &
                                "      d.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' " &
                                "      And u.flg_recebepdpage = 'S' " &
                                "Group By u.ordem, " &
                                "         d.codusina " &
                                "Order By u.ordem, " &
                                "         d.codusina"
            Conn.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
            Conn.Open()
            Dim rsUsina As SqlDataReader = Cmd.ExecuteReader
            Dim objItem As System.Web.UI.WebControls.ListItem
            cboUsina.Items.Clear()
            intI = 1
            Do While rsUsina.Read
                If intI = 1 Then
                    cboUsina.Items.Add("Selecione uma Usina")
                End If
                cboUsina.Items.Add(rsUsina.Item("codusina"))
                intI = intI + 1
            Loop
            If intI > 1 Then
                cboUsina.Items.Add("Todas as Usinas")
            End If
            rsUsina.Close()
            rsUsina = Nothing
            Conn.Close()
            PreencheTable()

            'Valida Limite de Envio
            Dim lRetorno As Integer = 0
            If Not ValidaLimiteEntradaDados(cboEmpresa.SelectedItem.Value, cboData.SelectedItem.Value, lRetorno, "IFX") Then
                cboUsina.Enabled = False
                If lRetorno = 1 Then
                    Response.Write("<SCRIPT>alert('" + strMsgInicioLimiteEnvioDados + "')</SCRIPT>")
                Else
                    Response.Write("<SCRIPT>alert('" + strMsgLimiteEnvioDados + "')</SCRIPT>")
                End If
                Exit Sub
            Else
                cboUsina.Enabled = True
            End If
            lblMsg.Visible = False
        Catch
            lblMsg.Visible = True
            'Session("strMensagem") = "Não foi possível acessar a Base de Dados."
            If Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If
            'Response.Redirect("frmMensagem.aspx")
        Finally
            If Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If
        End Try
    End Sub

    Private Sub cboUsina_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboUsina.SelectedIndexChanged
        Dim intI As Integer
        Dim Conn As SqlConnection = New SqlConnection
        Dim Cmd As SqlCommand = New SqlCommand
        Dim Cmd1 As SqlCommand = New SqlCommand

        Cmd.Connection = Conn
        Cmd1.Connection = Conn

        Try
            If cboUsina.SelectedItem.Text = "Todas as Usinas" Then
                Cmd.CommandText = "Select d.valflexitran, " &
                                "       d.intflexi, " &
                                "       d.codusina, " &
                                "       u.ordem " &
                                "From inflexibilidade d, " &
                                "     usina u " &
                                "Where u.codempre = '" & cboEmpresa.SelectedItem.Value & "' And " &
                                "      u.codusina = d.codusina And " &
                                "      d.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' " &
                                "      And u.flg_recebepdpage = 'S' " &
                                "Order By d.intflexi, " &
                                "         u.ordem, " &
                                "         d.codusina"

            Else
                Cmd.CommandText = "Select d.valflexitran, " &
                                "       d.intflexi " &
                                "From inflexibilidade d, " &
                                "     usina u " &
                                "Where u.codempre = '" & cboEmpresa.SelectedItem.Value & "' And " &
                                "      u.codusina = d.codusina And " &
                                "      d.codusina = '" & cboUsina.SelectedItem.Value & "' And " &
                                "      d.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' " &
                                "      And u.flg_recebepdpage = 'S' " &
                                "Order By d.intflexi"
            End If
            Conn.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
            Conn.Open()
            Dim rsUsina As SqlDataReader = Cmd.ExecuteReader
            'Dim rsSaldoInflex As SqlDataReader

            'Colocando os valores de carga no text para alteração
            Dim objTextArea As HtmlControls.HtmlTextArea
            objTextArea = New HtmlTextArea
            objTextArea.Rows = 48
            objTextArea.ID = "txtValor"
            objTextArea.Attributes.Item("onkeyup") = "RetiraEnter(event)"
            objTextArea.Attributes.Item("runat") = "server"
            If cboUsina.SelectedItem.Text <> "Todas as Usinas" Then
                objTextArea.Attributes.Item("style") = "font-size:XSmall;height:1001px;width:50px;line-height:20px"
            Else
                objTextArea.Attributes.Item("style") = "font-size:XSmall;height:1001px;width:" & (((cboUsina.Items.Count - 2) * 56)) - Int(cboUsina.Items.Count / 2) & "px;line-height:20px"
            End If

            Dim intInstante As Integer = 1
            Dim blnPassou As Boolean = False
            Dim ZerouSaldoInflex As Boolean = False
            'Dim dtatual As String
            'Dim ValSaldoInflex As Integer

            Do While rsUsina.Read

                'Verificar o Saldo de Inflexibilidade para cada Usina Selecionada
                'If cboUsina.SelectedItem.Text <> "Todas as Usinas" And ZerouSaldoInflex = False Then
                '    'Retirada a data atual para usar a data da programação selecionada (WI 26948)
                '    'dtatual = Format(CDate(Date.Today()), "yyyyMMdd")

                '    Cmd1.CommandText = "select codusina, val_saldo from tb_saldoinflexibilidadepmo where datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' and trim(codusina) = '" & Trim(cboUsina.SelectedItem.Value) & "'"
                '    'rsSaldoInflex = Cmd1.ExecuteReader

                '    'Do While rsSaldoInflex.Read
                '    '    ValSaldoInflex = rsSaldoInflex("val_saldo")
                '    '    If ValSaldoInflex <= 0 Then
                '    '        rsSaldoInflex.Close()
                '    '        Cmd1.Connection.Close()
                '    '        If Conn.State = ConnectionState.Open Then
                '    '            Conn.Close()
                '    '        End If
                '    '        Response.Write("<script lang='javascript'>")
                '    '        Response.Write("  window.alert('Não existe saldo de inflexibilidade para a Usina: " & Trim(cboUsina.SelectedItem.Value) & ".    Favor deixar para declarar a geração da Usina durante os comentários do DESSEM.')")
                '    '        Response.Write("</script>")

                '    '        Exit Sub
                '    '    Else
                '    '        ZerouSaldoInflex = True
                '    '    End If
                '    '    rsSaldoInflex.Close()
                '    '    Exit Do
                '    'Loop
                '    'rsSaldoInflex.Close()
                'End If
                '''Fim Verificação do saldo de Inflexibilidade

                If blnPassou Then
                    If rsUsina("intflexi") <> intInstante Then
                        intInstante = rsUsina("intflexi")
                        objTextArea.Value = objTextArea.Value & Chr(13)
                    Else
                        objTextArea.Value = objTextArea.Value & Chr(9)
                    End If
                End If
                If Not IsDBNull(rsUsina.Item("valflexitran")) Then
                    objTextArea.Value = objTextArea.Value & rsUsina.Item("valflexitran")
                Else
                    objTextArea.Value = objTextArea.Value & 0
                End If
                'na primeira passagem não escreve TAB nem ENTER
                blnPassou = True
            Loop
            divValor.Controls.Add(objTextArea)
            divValor.Style.Item("TOP") = "331px"
            If cboUsina.SelectedItem.Text <> "Todas as Usinas" Then
                divValor.Style.Item("LEFT") = Trim(Str(125 + (cboUsina.SelectedIndex * 56))) & "px"
            Else
                divValor.Style.Item("LEFT") = "182px"
            End If

            If Trim(cboUsina.SelectedItem.Text) <> "Selecione uma Usina" Then
                divValor.Visible = True
            Else
                divValor.Visible = False
            End If
            btnSalvar.Visible = True
            rsUsina.Close()
            rsUsina = Nothing
            Cmd.Connection.Close()
            Conn.Close()
            PreencheTable()
            lblMsg.Visible = False
        Catch ex As Exception
            lblMsg.Visible = True
            'Session("strMensagem") = "Não foi possível acessar a Base de Dados."
            If Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If
            'Response.Redirect("frmMensagem.aspx")
        Finally
            If Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If
        End Try
    End Sub

    Private Sub PreencheTable()

        'Usinas
        Dim intI, intJ As Integer
        Dim intLin As Integer
        Dim dblMedia As Double
        Dim objCel As TableCell
        Dim objRow As TableRow
        Dim objTamanho As WebControls.Unit
        Dim Conn As SqlConnection = New SqlConnection
        Dim Cmd As SqlCommand = New SqlCommand
        Cmd.Connection = Conn
        Dim strCodUsina As String
        Try
            Cmd.CommandText = "Select d.codusina, " &
                                "       d.intflexi, " &
                                "       d.valflexitran, " &
                                "       u.ordem " &
                                "From inflexibilidade d, " &
                                "     usina u " &
                                "Where u.codempre = '" & Session("strCodEmpre") & "' And " &
                                "      u.codusina = d.codusina And " &
                                "      d.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' " &
                                "      And u.flg_recebepdpage = 'S' " &
                                "Order By u.ordem, " &
                                "         d.codusina, " &
                                "         d.intflexi"
            Conn.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
            Conn.Open()
            Dim rsUsina As SqlDataReader = Cmd.ExecuteReader
            tblInflexibilidade.Rows.Clear()
            objTamanho = New WebControls.Unit

            objRow = New TableRow
            objRow.BackColor = System.Drawing.Color.Beige
            objRow.Width = objTamanho.Pixel(100)
            objCel = New TableCell
            objCel.Wrap = False
            objCel.Text = "Intervalo"
            objCel.Font.Bold = True
            objCel.HorizontalAlign = HorizontalAlign.Center
            'objCel.Width = objTamanho.Pixel(100)
            objCel.Attributes.Add("style", "min-width: 80px; width: 80px;")
            objRow.Controls.Add(objCel)

            objCel = New TableCell
            objCel.Text = "Total"
            objCel.Wrap = False
            objCel.Font.Bold = True
            objCel.Width = objTamanho.Pixel(62)
            objCel.HorizontalAlign = HorizontalAlign.Center
            'objRow.Width = objTamanho.Pixel(132)
            objCel.Attributes.Add("style", "min-width: 50px; width: 50px; max-width:50px;")
            objRow.Controls.Add(objCel)
            tblInflexibilidade.Width = objTamanho.Pixel(132)
            tblInflexibilidade.Controls.Add(objRow)

            Dim intHora As Integer = 0
            For intI = 1 To 48
                objRow = New TableRow
                objRow.Width = objTamanho.Pixel(100)
                objCel = New TableCell
                objCel.BackColor = System.Drawing.Color.Beige
                objCel.Font.Bold = True
                objCel.Width = objTamanho.Pixel(100)
                objCel.HorizontalAlign = HorizontalAlign.Center

                objCel.Text = IntParaHora(intI)

                'If intI Mod 2 <> 0 Then
                '    objCel.Text = Format(intHora - 1, "00") & ":00" & "-" & Format(intHora, "00") & ":30"
                '    intHora = intHora + 1
                'Else
                '    objCel.Text = Format(intHora - 1, "00") & ":30" & "-" & Format(intHora, "00") & ":00"
                'End If
                objRow.Controls.Add(objCel)
                objCel = New TableCell
                objCel.Text = 0
                objCel.BackColor = System.Drawing.Color.Beige
                objCel.Font.Bold = True
                objCel.Width = objTamanho.Pixel(500)
                objRow.Controls.Add(objCel)
                tblInflexibilidade.Controls.Add(objRow)
            Next

            objRow = New TableRow
            objRow.Width = objTamanho.Pixel(132)
            objCel = New TableCell
            objCel.Text = "Total"
            objCel.BackColor = System.Drawing.Color.Beige
            objCel.Font.Bold = True
            objCel.Width = objTamanho.Pixel(70)
            objCel.HorizontalAlign = HorizontalAlign.Center
            objRow.Controls.Add(objCel)
            objCel = New TableCell
            objCel.BackColor = System.Drawing.Color.Beige
            objCel.Font.Bold = True
            objCel.Width = objTamanho.Pixel(62)
            objRow.Controls.Add(objCel)
            tblInflexibilidade.Controls.Add(objRow)

            objRow = New TableRow
            objRow.Width = objTamanho.Pixel(132)
            objCel = New TableCell
            objCel.Text = "Média"
            objCel.BackColor = System.Drawing.Color.Beige
            objCel.Font.Bold = True
            objCel.Width = objTamanho.Pixel(70)
            objCel.HorizontalAlign = HorizontalAlign.Center
            objRow.Controls.Add(objCel)
            objCel = New TableCell
            objCel.BackColor = System.Drawing.Color.Beige
            objCel.Font.Bold = True
            objCel.Width = objTamanho.Pixel(62)
            objRow.Controls.Add(objCel)
            tblInflexibilidade.Controls.Add(objRow)

            intI = 1
            strCodUsina = ""
            dblMedia = 0

            Do While rsUsina.Read
                If strCodUsina <> rsUsina.Item("codusina") Then
                    'Cria uma nova coluna na tabela
                    objCel = New TableCell
                    objCel.Wrap = False
                    objCel.Width = objTamanho.Pixel(64)
                    objCel.Font.Bold = True
                    tblInflexibilidade.Rows(0).Width = objTamanho.Pixel(tblInflexibilidade.Rows(0).Width.Value + 64)
                    tblInflexibilidade.Width = objTamanho.Pixel(tblInflexibilidade.Width.Value + 64)

                    If Trim(rsUsina("codusina")) = Trim(cboUsina.SelectedItem.Text) Then
                        objCel.ForeColor = System.Drawing.Color.Red
                    End If
                    objCel.Text = rsUsina("codusina")
                    tblInflexibilidade.Rows(0).Controls.Add(objCel)
                    strCodUsina = rsUsina("codusina")
                    intI = intI + 1
                    intLin = 1
                End If
                'Inseri as celulas com os valores das usinas
                objCel = New TableCell
                objCel.Wrap = False
                objCel.Width = objTamanho.Pixel(64)
                If Not IsDBNull(rsUsina.Item("valflexitran")) Then
                    objCel.Text = rsUsina.Item("valflexitran")
                    dblMedia = dblMedia + rsUsina.Item("valflexitran")
                Else
                    objCel.Text = 0
                End If
                tblInflexibilidade.Rows(intLin).Width = objTamanho.Pixel(tblInflexibilidade.Rows(intLin).Width.Value + 63)
                tblInflexibilidade.Rows(intLin).Controls.Add(objCel)
                intLin = intLin + 1
                If intLin = 49 Then
                    objCel = New TableCell
                    objCel.Wrap = False
                    objCel.Width = objTamanho.Pixel(64)
                    objCel.Text = Trim(Str(dblMedia / 2))
                    tblInflexibilidade.Rows(intLin).Width = objTamanho.Pixel(tblInflexibilidade.Rows(intLin).Width.Value + 64)
                    tblInflexibilidade.Rows(intLin).Controls.Add(objCel)
                    tblInflexibilidade.Rows(intLin).Cells(1).Text = Trim(Str(Val(tblInflexibilidade.Rows(intLin).Cells(1).Text) + dblMedia))  ' (dblMedia / 2)))

                    objCel = New TableCell
                    objCel.Wrap = False
                    objCel.Width = objTamanho.Pixel(64)
                    objCel.Text = Trim(Str(Int(dblMedia / 48)))
                    tblInflexibilidade.Rows(intLin + 1).Width = objTamanho.Pixel(tblInflexibilidade.Rows(intLin + 1).Width.Value + 64)
                    tblInflexibilidade.Rows(intLin + 1).Controls.Add(objCel)
                    tblInflexibilidade.Rows(intLin + 1).Cells(1).Text = Trim(Str(Val(tblInflexibilidade.Rows(intLin + 1).Cells(1).Text) + Int(dblMedia / 48)))

                    dblMedia = 0
                End If
            Loop

            For intI = 1 To 50
                dblMedia = 0
                For intJ = 2 To tblInflexibilidade.Rows(0).Cells.Count - 1
                    dblMedia = dblMedia + Val(tblInflexibilidade.Rows(intI).Cells(intJ).Text)
                Next
                tblInflexibilidade.Rows(intI).Cells(1).Text = dblMedia '/ 2
            Next

            rsUsina.Close()
            rsUsina = Nothing
            'Cmd.Connection.Close()
            'Conn.Close()
            lblMsg.Visible = False
        Catch
            lblMsg.Visible = True
            'Session("strMensagem") = "Não foi possível acessar a Base de Dados."
            If Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If
            'Response.Redirect("frmMensagem.aspx")
        Finally
            If Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If
        End Try
    End Sub

    Private Sub cboData_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboData.SelectedIndexChanged
        Try
            If cboData.SelectedIndex <> 0 Then
                Session("datEscolhida") = CDate(cboData.SelectedItem.Value)
            End If
            If cboEmpresa.SelectedIndex > 0 Then
                cboEmpresa_SelectedIndexChanged(sender, e)
            End If
            lblMsg.Visible = False
        Catch
            lblMsg.Visible = True
            'Session("strMensagem") = "Não foi possível acessar a Base de Dados."
            'Response.Redirect("frmMensagem.aspx")
        End Try
    End Sub

    Private Overloads Sub btnSalvar_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSalvar.Click
        Dim intI As Integer
        Dim intJ As Integer
        Dim intk As Integer
        Dim intColAtual As Integer
        Dim intColAnterior As Integer
        Dim intQtdReg As Integer
        Dim intCol As Integer
        Dim intValor As Integer
        Dim strValor As String

        'Dim objPermissao As OnsClasses.OnsInterfaceUnica.OnsPermissao
        'objPermissao = New OnsClasses.OnsInterfaceUnica.OnsPermissao("UPD", "PDPColInfl", UsuarID)
        'Verifica se o usuário tem permissão para salvar os registros
        If EstaAutorizado Then
            Dim Conn As SqlConnection = New SqlConnection
            Dim Conn1 As SqlConnection = New SqlConnection
            Dim Cmd As SqlCommand = New SqlCommand
            Dim Cmd1 As SqlCommand = New SqlCommand
            'Dim rsSaldoInflex As SqlDataReader

            Cmd.Connection = Conn

            Dim objTrans As SqlTransaction
            Conn.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
            Conn.Open()
            Conn1.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
            Conn1.Open()


            'Alterando os valores de carga na BDT
            objTrans = Conn.BeginTransaction()
            Cmd.Transaction = objTrans
            Dim datAtual As Date = Now
            'strValor = Page.Request.Form.Item("txtValor")
            strValor = Page.Request.Form("_ctl0:ContentPlaceHolder1:txtValor")

            Try

                'Dim dtatual As String
                'Dim ValSaldoInflex As Integer

                'Dim LstUsinaInflexZero As String = ""
                Cmd1.Connection = Conn1

                'Verificar o Saldo de Inflexibilidade para cada Usina Selecionada
                'If cboUsina.SelectedItem.Text = "Todas as Usinas" Then
                '    'Retirada a data atual para usar a data da programação selecionada (WI 26948)
                '    'dtatual = Format(CDate(Date.Today()), "yyyyMMdd")

                '    For intk = 1 To cboUsina.Items.Count - 2 'Percorre a lista de Usinas
                '        Cmd1.CommandText = "select codusina, val_saldo from tb_saldoinflexibilidadepmo where datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' and trim(codusina) = '" & Trim(cboUsina.Items(intk).Value) & "'"
                '        rsSaldoInflex = Cmd1.ExecuteReader

                '        'Do While rsSaldoInflex.Read
                '        '    ValSaldoInflex = rsSaldoInflex("val_saldo")

                '        '    'If ValSaldoInflex <= 0 Then
                '        '    '    rsSaldoInflex.Close()
                '        '    '    LstUsinaInflexZero = LstUsinaInflexZero & Trim(cboUsina.Items(intk).Value) & ";"
                '        '    'End If
                '        '    rsSaldoInflex.Close()
                '        '    Exit Do
                '        'Loop
                '        rsSaldoInflex.Close()
                '    Next
                'End If
                'Conn1.Close()

                'Fim Verificação do saldo de Inflexibilidade


                'Atualiza a grid
                If strValor = "" Then
                    'Quando o txtValor estiver em branco, branqueia tabela e a grid
                    If cboUsina.SelectedItem.Text <> "Todas as Usinas" Then
                        For intI = 1 To 48
                            'Atualiza na BDT a Coluna Alterada
                            Cmd.CommandText = "Update inflexibilidade " &
                                                "Set valflexitran = 0 " &
                                                "Where datPdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' And " &
                                                "      codusina = '" & Trim(cboUsina.SelectedItem.Value) & "' And " &
                                                "      intflexi = " & intI
                            Cmd.ExecuteNonQuery()
                        Next
                    Else
                        For intI = 1 To 48
                            For intJ = 1 To cboUsina.Items.Count - 2
                                'If InStr(LstUsinaInflexZero, Trim(cboUsina.Items(intJ).Value)) = 0 Then 'Verifica se está na lista de Usinas com Inflex=0. Se estiver não grava
                                'Atualiza na BDT TODAS as Colunas Alteradas
                                Cmd.CommandText = "Update inflexibilidade " &
                                                "Set valflexitran = 0 " &
                                                "Where datPdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' And " &
                                                "      codusina = '" & Trim(cboUsina.Items(intJ).Value) & "' And " &
                                                "      intflexi = " & intI
                                Cmd.ExecuteNonQuery()
                                'End If
                            Next
                        Next
                    End If
                Else
                    If cboUsina.SelectedItem.Text <> "Todas as Usinas" Then
                        'Somente uma USINA
                        intI = 1
                        intColAnterior = 1
                        intColAtual = InStr(intColAnterior, strValor, Chr(13), CompareMethod.Binary)
                        For intI = 1 To 48
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
                            Cmd.CommandText = "Update inflexibilidade " &
                                                "Set valflexitran = " & intValor & " " &
                                                "Where datPdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' And " &
                                                "      codusina = '" & cboUsina.SelectedItem.Value & "' And " &
                                                "      intflexi = " & intI
                            Cmd.ExecuteNonQuery()
                            intColAtual = InStr(intColAnterior + 1, strValor, Chr(13), CompareMethod.Binary)
                        Next
                    Else

                        Dim strLinha As String
                        Dim intCelula, intFim, intUsina As Integer

                        strLinha = ""
                        intColAtual = 1
                        For intI = 1 To 48

                            'Todas as USINA
                            intUsina = 1
                            If InStr(intColAtual, strValor, Chr(13), CompareMethod.Binary) <> 0 Then
                                strLinha = Mid(strValor, intColAtual, ((InStr(intColAtual, strValor, Chr(13), CompareMethod.Binary)) - intColAtual) + 1)
                            Else
                                strLinha = Mid(strValor, intColAtual)
                            End If

                            If InStr(strLinha, Chr(13)) <> 0 Then
                                strLinha = Left(strLinha, InStr(strLinha, Chr(13)) - 1)
                            End If
                            intColAtual = intColAtual + Len(Trim(strLinha)) + 2
                            intColAnterior = 1
                            intCelula = 2
                            Do While InStr(intColAnterior, strLinha, Chr(9), CompareMethod.Binary) <> 0
                                intFim = InStr(intColAnterior, strLinha, Chr(9), CompareMethod.Binary)

                                If Trim(Mid(strLinha, intColAnterior, intFim - intColAnterior)) <> "" Then
                                    intValor = Val(Mid(strLinha, intColAnterior, intFim - intColAnterior))
                                Else
                                    intValor = 0
                                End If

                                'If InStr(LstUsinaInflexZero, Trim(cboUsina.Items(intUsina).Value)) = 0 Then 'Verifica se está na lista de Usinas com Inflex=0. Se estiver não grava
                                Cmd.CommandText = "Update inflexibilidade " &
                                            "Set valflexitran = " & intValor & " " &
                                            "Where datPdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' And " &
                                            "      codusina = '" & cboUsina.Items(intUsina).Value & "' And " &
                                            "      intflexi = " & intI
                                Cmd.ExecuteNonQuery()
                                'End If

                                intUsina = intUsina + 1
                                intColAnterior = intFim + 1
                                intCelula = intCelula + 1
                            Loop

                            If Trim(Mid(strLinha, intColAnterior)) <> "" Then
                                intValor = Val(Mid(strLinha, intColAnterior))
                            Else
                                intValor = 0
                            End If
                            'If InStr(LstUsinaInflexZero, Trim(cboUsina.Items(intUsina).Value)) = 0 Then 'Verifica se está na lista de Usinas com Inflex=0. Se estiver não grava
                            Cmd.CommandText = "Update inflexibilidade " &
                                            "Set valflexitran = " & intValor & " " &
                                            "Where datPdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' And " &
                                            "      codusina = '" & cboUsina.Items(intUsina).Value & "' And " &
                                            "      intflexi = " & intI
                            Cmd.ExecuteNonQuery()
                            'End If
                        Next
                    End If
                    strValor = ""
                End If

                'Grava evento registrando o recebimento de Inflexibilidade
                GravaEventoPDP("2", Format(Session("datEscolhida"), "yyyyMMdd"), cboEmpresa.SelectedItem.Text, datAtual, "PDPColInfl", UsuarID)

                'Grava toda a transação
                objTrans.Commit()
                lblMsg.Visible = False

                'If cboUsina.SelectedItem.Text = "Todas as Usinas" And LstUsinaInflexZero <> "" Then
                '    Dim LUsi As String = Mid(LstUsinaInflexZero, 1, (Len(LstUsinaInflexZero) - 1))

                '    Response.Write("<script lang='javascript'>")
                '    Response.Write("  window.alert('As usinas: " & LUsi & " não foram salvas, pois não existe saldo de inflexibilidade para elas. Favor deixar para declarar a geração da Usina durante os comentários do DESSEM.')")
                '    Response.Write("</script>")
                'End If
            Catch ex As Exception
                lblMsg.Visible = True
                'houve erro, aborta a transação e fecha a conexão
                'Session("strMensagem") = "Não foi possível gravar os dados."
                objTrans.Rollback()
                If Conn.State = ConnectionState.Open Then
                    Conn.Close()
                End If
                'Chama a tela de mensagem
                'Response.Redirect("frmMensagem.aspx")
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
                cboUsina.SelectedIndex = 0
                PreencheTable()
                lblMsg.Visible = False
            Catch
                lblMsg.Visible = True
                'Session("strMensagem") = "Não foi possível acessar a Base de Dados."
                'Response.Redirect("frmMensagem.aspx")
            End Try
        Else
            Session("strMensagem") = "Usuário não tem permissão para alterar os valores."
            Response.Redirect("frmMensagem.aspx")
        End If
    End Sub

End Class
