Imports System.Collections.Generic
Imports System.Data.SqlClient

Partial Class frmColDisponibilidade
    Inherits System.Web.UI.Page
    Dim siglaTipoUsina As String
    Dim tipousina As ListItem
    Dim strEstudo As String = "N"
    Dim strtipousina1 As String
    Dim strtipousina2 As String

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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load, Me.Load
        'Put user code to initialize the page here
        'objMenu = CType(Session("onsmenu"), OnsWebControls.OnsMenu)

        If Not Page.IsPostBack Then
            Dim intI As Integer
            Dim Conn As SqlConnection = New SqlConnection
            Dim Cmd As SqlCommand = New SqlCommand
            Cmd.Connection = Conn
            If Session("datEscolhida") = Nothing Then
                'Inicializa a variável com data do próximo
                Session("datEscolhida") = Now.AddDays(1)
            End If

            Try
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

                ' verifica qual tipo de geração das usinas com o usuário logado. Demanda 26219 Mauricio Magalhae
                Dim strUsuar As String = UsuarID
                Cmd.Connection = Conn
                Conn.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
                Conn.Open()
                If PerfilID.Equals("ADM_PDPW") Then
                    Cmd.CommandText = "SELECT DISTINCT s.tipusina " &
                                  "From empre e, usina s " &
                                  "Where Trim(s.codempre) = Trim(e.codempre) " &
                                  "And e.flg_estudo = '" & strEstudo &
                                  "' And s.tipusina <> 'E' " &
                                  "ORDER BY s.tipusina"
                Else
                    Cmd.CommandText = "SELECT DISTINCT s.tipusina " &
                                  "From empre e, usina s " &
                                  "Where Trim(s.codempre) = Trim(e.codempre) " &
                                  "And e.flg_estudo = '" & strEstudo &
                                  "' And  e.empre_bdt_id in (" & AgentesRepresentados & ") " &
                                  "ORDER BY s.tipusina"
                End If


                Dim rsTipo As SqlDataReader = Cmd.ExecuteReader

                Dim objItem2 As System.Web.UI.WebControls.ListItem
                objItem2 = New System.Web.UI.WebControls.ListItem
                objItem2.Text = ""
                objItem2.Value = "0"


                Dim intaux As Integer = 1

                'verifica os valores que voltaram da query e reserva Demanda 26219 Mauricio Magalhae
                Do While rsTipo.Read()
                    objItem2 = New System.Web.UI.WebControls.ListItem
                    objItem2.Text = rsTipo("tipusina")

                    If intaux = 1 Then
                        strtipousina1 = objItem2.ToString

                    End If
                    If intaux = 2 Then
                        strtipousina2 = objItem2.ToString
                    End If
                    intaux = intaux + 1

                Loop
                rsTipo.Close()
                rsTipo = Nothing
                Cmd.Connection.Close()
                Conn.Close()

                ' verifica quais usinas o agente possui acesso, se termica, heidraulica ou ambas. Demanda 26219 Mauricio Magalhae
                If strtipousina1 = "H" And strtipousina2 = "T" Then

                    OptTermica.Visible = True
                    OptHidraulica.Visible = True
                    OptTermica.Checked = False
                    OptHidraulica.Checked = False
                    cboEmpresa.Enabled = False
                    cboData.Enabled = False
                    OptHidraulica.Enabled = True
                    OptTermica.Enabled = True


                Else
                    If strtipousina1 = "T" Then
                        btnSelecionarUsina.Visible = True
                        OptTermica.Visible = True
                        OptHidraulica.Enabled = False
                        OptTermica.Checked = True
                        cboData.Enabled = True
                        siglaTipoUsina = "T"
                        cboEmpresa.Enabled = True
                        cboUsina.Enabled = True
                        lblUsinaSelecionada.Text = "Cadastro de dados de Disponibilidade - USINAS TÉRMICAS"
                        lblUsinaSelecionada.Visible = True


                    Else
                        OptTermica.Visible = True
                        OptTermica.Enabled = False
                        btnSelecionarUsina.Visible = True
                        OptHidraulica.Visible = True
                        OptHidraulica.Checked = True
                        cboData.Enabled = True
                        siglaTipoUsina = "H"
                        cboEmpresa.Enabled = True
                        cboUsina.Enabled = True
                        lblUsinaSelecionada.Text = "Cadastro de dados de Disponibilidade - USINAS HIDRAULICAS"
                        lblUsinaSelecionada.Visible = True


                    End If


                End If


                Cmd.Connection.Close()

                'If siglaTipoUsina = "" Then
                PreencheComboEmpresaPOP(AgentesRepresentados, cboEmpresa, Session("strCodEmpre"))
                If cboData.SelectedIndex > 0 Then
                    cboData_SelectedIndexChanged(sender, e)
                End If

                'Else
                'PreencheComboEmpresaPorTipo(Session("usuarID"), cboEmpresa, Session("strCodEmpre"), siglaTipoUsina)
                '    If cboData.SelectedIndex > 0 Then
                '        cboData_SelectedIndexChanged(sender, e)
                '    End If

                'End If


                Conn.Close()

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
        divValor.Visible = False
        btnSalvar.Visible = False
        If cboEmpresa.SelectedIndex > 0 Then
            Session("strCodEmpre") = cboEmpresa.SelectedItem.Value
        End If

        Try
            If OptTermica.Checked Then
                siglaTipoUsina = "T"
            ElseIf OptHidraulica.Checked Then
                siglaTipoUsina = "H"
            Else
                siglaTipoUsina = ""
            End If
            If siglaTipoUsina <> "" Then
                Cmd.CommandText = "Select d.codusina, " &
                                  "       u.ordem " &
                                  "From disponibilidade d, " &
                                  "     usina u " &
                                  "Where u.codempre = '" & Session("strCodEmpre") & "' And " &
                                  "      u.codusina = d.codusina And " &
                                  "      d.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' And " &
                                  "      u.tipusina = '" & siglaTipoUsina & "' " &
                                  "      And u.flg_recebepdpage = 'S' " &
                              "Group By u.ordem, " &
                              "         d.codusina " &
                              "Order By u.ordem, " &
                              "         d.codusina"
            Else
                Cmd.CommandText = "Select d.codusina, " &
                              "       u.ordem " &
                              "From disponibilidade d, " &
                              "     usina u " &
                              "Where u.codempre = '" & Session("strCodEmpre") & "' And " &
                              "      u.codusina = d.codusina And " &
                              "      d.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' " &
                          "      And u.flg_recebepdpage = 'S' " &
                          "Group By u.ordem, " &
                          "         d.codusina " &
                          "Order By u.ordem, " &
                          "         d.codusina"

            End If
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


            'Valida Limite de Envio
            Dim lRetorno As Integer = 0
            If Not ValidaLimiteEntradaDados(cboEmpresa.SelectedItem.Value, cboData.SelectedItem.Value, lRetorno, "DSP") Then
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
        Cmd.Connection = Conn
        Try 'Demanda 26219 Mauricio Magalhae alterado query para filtrar usinas por tipo, se usina termica ou hidraulica
            If OptTermica.Checked Then
                siglaTipoUsina = "T"
            ElseIf OptHidraulica.Checked Then
                siglaTipoUsina = "H"
            Else
                siglaTipoUsina = ""
            End If
            If siglaTipoUsina <> "" Then


                If cboUsina.SelectedItem.Text = "Todas as Usinas" Then
                    Cmd.CommandText = "Select d.valdsptran, " &
                                "       d.intdsp, " &
                                "       d.codusina, " &
                                "       u.ordem " &
                                "From disponibilidade d, " &
                                "     usina u " &
                                "Where u.codempre = '" & cboEmpresa.SelectedItem.Value & "' And " &
                                "      u.codusina = d.codusina And " &
                               "      d.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' " &
                                "      And u.flg_recebepdpage = 'S' " &
                                "and u.tipusina = '" & siglaTipoUsina & "' " &
                                "Order By d.intdsp, " &
                                "         u.ordem, " &
                                "         d.codusina"
                Else
                    Cmd.CommandText = "Select d.valdsptran, " &
                                    "       d.intdsp " &
                                    "From disponibilidade d, " &
                                    "     usina u " &
                                    "Where u.codempre = '" & cboEmpresa.SelectedItem.Value & "' And " &
                                    "      u.codusina = d.codusina And " &
                                    "      d.codusina = '" & cboUsina.SelectedItem.Value & "' And " &
                                    "      d.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' " &
                                    "      And u.flg_recebepdpage = 'S' " &
                                    "Order By d.intdsp"
                End If
            End If
            Conn.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
            Conn.Open()
            Dim rsUsina As SqlDataReader = Cmd.ExecuteReader

            'Colocando os valores de carga no text para alteração
            Dim objTextArea As HtmlControls.HtmlTextArea
            objTextArea = New HtmlTextArea
            objTextArea.Rows = 48
            objTextArea.ID = "txtValor"
            objTextArea.Attributes.Item("onkeyup") = "RetiraEnter(event)"
            objTextArea.Attributes.Item("runat") = "server"
            If cboUsina.SelectedItem.Text <> "Todas as Usinas" Then
                objTextArea.Attributes.Item("style") = "font-size:XSmall;height:1001px;width:81px;line-height:20px"
            Else
                objTextArea.Attributes.Item("style") = "font-size:XSmall;height:1001px;width:" & (((cboUsina.Items.Count - 2) * 65) + 16) - Int(cboUsina.Items.Count / 2) & "px;line-height:20px"
            End If

            Dim intInstante As Integer = 1
            Dim blnPassou As Boolean = False
            Do While rsUsina.Read
                If blnPassou Then
                    If rsUsina("intdsp") <> intInstante Then
                        intInstante = rsUsina("intdsp")
                        objTextArea.Value = objTextArea.Value & Chr(13)
                    Else
                        objTextArea.Value = objTextArea.Value & Chr(9)
                    End If
                End If
                If Not IsDBNull(rsUsina.Item("valdsptran")) Then
                    objTextArea.Value = objTextArea.Value & rsUsina.Item("valdsptran")
                Else
                    objTextArea.Value = objTextArea.Value & 0
                End If
                'na primeira passagem não escreve TAB nem ENTER
                blnPassou = True
            Loop
            divValor.Controls.Add(objTextArea)
            divValor.Style.Item("TOP") = "22px"
            If cboUsina.SelectedItem.Text <> "Todas as Usinas" Then
                divValor.Style.Item("LEFT") = Trim(Str(72 + (cboUsina.SelectedIndex * 64))) & "px"
            Else
                divValor.Style.Item("LEFT") = "136px"
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
        Try    'Demanda 26219 Mauricio Magalhaes adicionada siglaTipoUsina para filtrar o tipo de usina termica ou hidraulica
            If OptTermica.Checked Then
                siglaTipoUsina = "T"
            ElseIf OptHidraulica.Checked Then
                siglaTipoUsina = "H"
            Else
                siglaTipoUsina = ""
            End If
            If siglaTipoUsina <> "" Then
                Cmd.CommandText = "Select d.codusina, " &
                              "       d.intdsp, " &
                              "       d.valdsptran, " &
                              "       u.ordem " &
                              "From disponibilidade d, " &
                              "     usina u " &
                              "Where u.codempre = '" & Session("strCodEmpre") & "' And " &
                              "      u.codusina = d.codusina And " &
                              "      d.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' AND " &
                              "      u.tipusina = '" & siglaTipoUsina & "' " &
                              "      And u.flg_recebepdpage = 'S' " &
                              "Order By u.ordem, " &
                              "         d.codusina, " &
                              "         d.intdsp "
            Else
                Cmd.CommandText = "Select d.codusina, " &
              "       d.intdsp, " &
              "       d.valdsptran, " &
              "       u.ordem " &
              "From disponibilidade d, " &
              "     usina u " &
              "Where u.codempre = '" & Session("strCodEmpre") & "' And " &
              "      u.codusina = d.codusina And " &
              "      d.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' AND " &
              "      And u.flg_recebepdpage = 'S' " &
              "Order By u.ordem, " &
              "         d.codusina, " &
              "         d.intdsp "
            End If
            Conn.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
            Conn.Open()
            Dim rsUsina As SqlDataReader = Cmd.ExecuteReader
            tblDSP.Rows.Clear()
            objTamanho = New WebControls.Unit

            objRow = New TableRow
            objRow.BackColor = System.Drawing.Color.Beige
            objRow.Width = objTamanho.Pixel(100)
            objCel = New TableCell
            objCel.Wrap = False
            objCel.Text = "Intervalo"
            objCel.Font.Bold = True
            objCel.HorizontalAlign = HorizontalAlign.Center
            objCel.Width = objTamanho.Pixel(100)
            objRow.Controls.Add(objCel)

            objCel = New TableCell
            objCel.Text = "Total"
            objCel.Wrap = False
            objCel.Font.Bold = True
            objCel.Width = objTamanho.Pixel(62)
            objCel.HorizontalAlign = HorizontalAlign.Center
            objRow.Width = objTamanho.Pixel(132)
            objRow.Controls.Add(objCel)
            tblDSP.Width = objTamanho.Pixel(132)
            tblDSP.Controls.Add(objRow)

            Dim intHora As Integer = 0
            For intI = 1 To 48
                objRow = New TableRow
                objRow.Width = objTamanho.Pixel(132)
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
                objCel.Width = objTamanho.Pixel(62)
                objRow.Controls.Add(objCel)
                tblDSP.Controls.Add(objRow)
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
            tblDSP.Controls.Add(objRow)

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
            tblDSP.Controls.Add(objRow)

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
                    tblDSP.Rows(0).Width = objTamanho.Pixel(tblDSP.Rows(0).Width.Value + 64)
                    tblDSP.Width = objTamanho.Pixel(tblDSP.Width.Value + 64)

                    If Trim(rsUsina("codusina")) = Trim(cboUsina.SelectedItem.Text) Then
                        objCel.ForeColor = System.Drawing.Color.Red
                    End If
                    objCel.Text = rsUsina("codusina")
                    tblDSP.Rows(0).Controls.Add(objCel)
                    strCodUsina = rsUsina("codusina")
                    intI = intI + 1
                    intLin = 1
                End If
                'Inseri as celulas com os valores das usinas
                objCel = New TableCell
                objCel.Wrap = False
                objCel.Width = objTamanho.Pixel(64)
                If Not IsDBNull(rsUsina.Item("valdsptran")) Then
                    objCel.Text = rsUsina.Item("valdsptran")
                    dblMedia = dblMedia + rsUsina.Item("valdsptran")
                Else
                    objCel.Text = 0
                End If
                tblDSP.Rows(intLin).Width = objTamanho.Pixel(tblDSP.Rows(intLin).Width.Value + 63)
                tblDSP.Rows(intLin).Controls.Add(objCel)
                intLin = intLin + 1
                If intLin = 49 Then
                    objCel = New TableCell
                    objCel.Wrap = False
                    objCel.Width = objTamanho.Pixel(64)
                    objCel.Text = Trim(Str(dblMedia / 2))
                    tblDSP.Rows(intLin).Width = objTamanho.Pixel(tblDSP.Rows(intLin).Width.Value + 64)
                    tblDSP.Rows(intLin).Controls.Add(objCel)
                    tblDSP.Rows(intLin).Cells(1).Text = Trim(Str(Val(tblDSP.Rows(intLin).Cells(1).Text) + dblMedia)) '(dblMedia / 2)))

                    objCel = New TableCell
                    objCel.Wrap = False
                    objCel.Width = objTamanho.Pixel(64)
                    objCel.Text = Trim(Str(Int(dblMedia / 48)))
                    tblDSP.Rows(intLin + 1).Width = objTamanho.Pixel(tblDSP.Rows(intLin + 1).Width.Value + 64)
                    tblDSP.Rows(intLin + 1).Controls.Add(objCel)
                    tblDSP.Rows(intLin + 1).Cells(1).Text = Trim(Str(Val(tblDSP.Rows(intLin + 1).Cells(1).Text) + Int(dblMedia / 48)))

                    dblMedia = 0
                End If
            Loop

            For intI = 1 To 50
                dblMedia = 0
                For intJ = 2 To tblDSP.Rows(0).Cells.Count - 1
                    dblMedia = dblMedia + Val(tblDSP.Rows(intI).Cells(intJ).Text)
                Next
                tblDSP.Rows(intI).Cells(1).Text = dblMedia '/ 2
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

    Private Sub btnSalvar_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSalvar.Click
        Dim intI As Integer
        Dim intJ As Integer
        Dim intColAtual As Integer
        Dim intColAnterior As Integer
        Dim intQtdReg As Integer
        Dim intCol As Integer
        Dim intValor As Integer
        Dim strValor As String

        'Dim objPermissao As OnsClasses.OnsInterfaceUnica.OnsPermissao
        'objPermissao = New OnsClasses.OnsInterfaceUnica.OnsPermissao("UPD", "PDPColDsp", UsuarID)
        'Verifica se o usuário tem permissão para salvar os registros
        If EstaAutorizado Then
            Dim Conn As SqlConnection = New SqlConnection
            Dim Cmd As SqlCommand = New SqlCommand
            Cmd.Connection = Conn
            Dim objTrans As SqlTransaction
            Conn.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
            Conn.Open()

            'Alterando os valores de carga na BDT
            objTrans = Conn.BeginTransaction()
            Cmd.Transaction = objTrans
            Dim datAtual As Date = Now
            strValor = Page.Request.Form("_ctl0:ContentPlaceHolder1:txtValor")

            Try
                'Atualiza a grid
                If strValor = "" Then
                    'Quando o txtValor estiver em branco, branqueia tabela e a grid
                    If cboUsina.SelectedItem.Text <> "Todas as Usinas" Then
                        For intI = 1 To 48
                            'Atualiza na BDT a Coluna Alterada
                            Cmd.CommandText = "Update disponibilidade " &
                                              "Set valdsptran = 0 " &
                                              "Where datPdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' And " &
                                              "      codusina = '" & Trim(cboUsina.SelectedItem.Value) & "' And " &
                                              "      intdsp = " & intI
                            Cmd.ExecuteNonQuery()
                        Next
                    Else
                        For intI = 1 To 48
                            For intJ = 1 To cboUsina.Items.Count - 2
                                'Atualiza na BDT TODAS as Colunas Alteradas
                                Cmd.CommandText = "Update disponibilidade " &
                                                  "Set valdsptran = 0 " &
                                                  "Where datPdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' And " &
                                                  "      codusina = '" & Trim(cboUsina.Items(intJ).Value) & "' And " &
                                                  "      intdsp = " & intI
                                Cmd.ExecuteNonQuery()
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
                            Cmd.CommandText = "Update disponibilidade " &
                                              "Set valdsptran = " & intValor & " " &
                                              "Where datPdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' And " &
                                              "      codusina = '" & cboUsina.SelectedItem.Value & "' And " &
                                              "      intdsp = " & intI
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

                                Cmd.CommandText = "Update disponibilidade " &
                                                  "Set valdsptran = " & intValor & " " &
                                                  "Where datPdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' And " &
                                                  "      codusina = '" & cboUsina.Items(intUsina).Value & "' And " &
                                                  "      intdsp = " & intI
                                Cmd.ExecuteNonQuery()
                                intUsina = intUsina + 1
                                intColAnterior = intFim + 1
                                intCelula = intCelula + 1
                            Loop

                            If Trim(Mid(strLinha, intColAnterior)) <> "" Then
                                intValor = Val(Mid(strLinha, intColAnterior))
                            Else
                                intValor = 0
                            End If
                            Cmd.CommandText = "Update disponibilidade " &
                                                "Set valdsptran = " & intValor & " " &
                                                "Where datPdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' And " &
                                                "      codusina = '" & cboUsina.Items(intUsina).Value & "' And " &
                                                "      intdsp = " & intI
                            Cmd.ExecuteNonQuery()
                        Next
                    End If
                    strValor = ""
                End If

                'Grava evento registrando o recebimento de Disponibilidade (DSP)
                GravaEventoPDP("46", Format(Session("datEscolhida"), "yyyyMMdd"), cboEmpresa.SelectedItem.Text, datAtual, "PDPColDsp", UsuarID)

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
                cboUsina.SelectedIndex = 0
                PreencheTable()
                lblMsg.Visible = False
            Catch
                lblMsg.Visible = True
                'Session("strMensagem") = "Não foi possível acessar a Base de Dados."
                If Conn.State = ConnectionState.Open Then
                    Conn.Close()
                End If
                ' Response.Redirect("frmMensagem.aspx")
            Finally
                If Conn.State = ConnectionState.Open Then
                    Conn.Close()
                End If
            End Try
        Else
            Session("strMensagem") = "Usuário não tem permissão para alterar os valores."
            Response.Redirect("frmMensagem.aspx")
        End If
    End Sub

    Protected Sub btnbtnSelecionarUsina_Click(sender As Object, e As ImageClickEventArgs) Handles btnSelecionarUsina.Click
        'Demanda 26219 Mauricio Magalhaes botao para selecionar tipo de usina.
        If OptTermica.Checked Then
            cboUsina.Items.Clear()
            cboEmpresa.ClearSelection()
            cboData.Enabled = True
            siglaTipoUsina = "T"
            cboEmpresa.Enabled = True
            cboUsina.Enabled = True
            lblUsinaSelecionada.Text = "Coleta de dados de Disponibilidade - USINAS TÉRMICAS"
            lblUsinaSelecionada.Visible = True

        ElseIf OptHidraulica.Checked Then
            cboUsina.Items.Clear()
            cboEmpresa.ClearSelection()
            cboData.Enabled = True
            siglaTipoUsina = "H"
            cboEmpresa.Enabled = True
            cboUsina.Enabled = True
            lblUsinaSelecionada.Text = "Coleta de dados de Disponibilidade - USINAS HIDRÁULICAS"
            lblUsinaSelecionada.Visible = True
        Else
            cboData.Enabled = False
            siglaTipoUsina = ""
            cboEmpresa.Enabled = False
            cboUsina.Enabled = False
            lblUsinaSelecionada.Visible = False

            Response.Write("<script lang='javascript'>")
            Response.Write("  window.alert('Selecione um Tipo de Usina para continuar')")
            Response.Write("</script>")
            Exit Sub

        End If
    End Sub


End Class
