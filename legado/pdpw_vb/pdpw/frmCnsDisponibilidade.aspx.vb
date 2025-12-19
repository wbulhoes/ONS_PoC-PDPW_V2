Imports System.Data.SqlClient

Partial Class frmCnsDisponibilidade

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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here





        'objMenu = CType(Session("onsmenu"), OnsWebControls.OnsMenu)
        imgPlanilha.Visible = False
        lblMensagem.Visible = False
        If Page.Request.QueryString("strAcesso") = "PDOC" Then
            optDados.Visible = False
            optDados.SelectedIndex = 2
            lblData.Text = "Data do PDP"
        Else
            lblData.Text = "Data do PDP"
        End If

        If Not Page.IsPostBack Then

            OptTermica.Checked = True
            siglaTipoUsina = "T"
            lblUsinaSelecionada.Text = "Consulta de dados de Disponibilidade - USINAS TÉRMICAS"
            lblUsinaSelecionada.Visible = True

            ' Filtra as usinas de acordo com o login do usuário
            ConfigurarAcessoPorUsuario()

            ' Define OptTermica como selecionado por padrão, se aplicável
            If strtipousina1 = "T" OrElse (strtipousina1 = "H" And strtipousina2 = "T") Then
                OptTermica.Checked = True
                siglaTipoUsina = "T"
                lblUsinaSelecionada.Text = "Consulta de dados de Disponibilidade - USINAS TÉRMICAS"
            ElseIf strtipousina1 = "H" Then
                OptHidraulica.Checked = True
                siglaTipoUsina = "H"
                lblUsinaSelecionada.Text = "Consulta de dados de Disponibilidade - USINAS HIDRAULICAS"
            End If
            lblUsinaSelecionada.Visible = True

            ' Configurar dropdown de empresas com base no tipo de usina
            ConfigurarEmpresasPorTipo()

            ' Carregar datas iniciais no dropdown
            CarregarDatasIniciais()
        End If
    End Sub

    Private Sub ConfigurarAcessoPorUsuario()
        Dim Conn As New SqlConnection(ConfigurationManager.AppSettings("pdpSQL").ToString())
        Dim Cmd As New SqlCommand
        Cmd.Connection = Conn
        Conn.Open()

        ' Filtragem de usinas com base no usuário logado
        Cmd.CommandText = "SELECT DISTINCT s.tipusina FROM empre e, usuarempre u , usina s WHERE Trim(u.codempre) = Trim(e.codarea) And e.flg_estudo = '" & strEstudo & "'  and u.usuar_id = '" & UsuarID & "' And Trim(u.codempre) = Trim(s.codempre) UNION SELECT DISTINCT s.tipusina FROM empre e, usuarempre u, usina s WHERE Trim(u.codempre) = Trim(e.codempre) And e.flg_estudo = '" & strEstudo & "'  and u.usuar_id = '" & UsuarID & "' And Trim(u.codempre) = Trim(s.codempre) ORDER BY s.tipusina"
        Dim rsTipo As SqlDataReader = Cmd.ExecuteReader()

        Dim intaux As Integer = 1
        Do While rsTipo.Read()
            If intaux = 1 Then
                strtipousina1 = rsTipo("tipusina").ToString()
            ElseIf intaux = 2 Then
                strtipousina2 = rsTipo("tipusina").ToString()
            End If
            intaux += 1
        Loop
        rsTipo.Close()
        Conn.Close()
    End Sub

    Private Sub CarregarDatasIniciais()
        Dim Conn As New SqlConnection(ConfigurationManager.AppSettings.Get("pdpSQL").ToString())
        Dim Cmd As New SqlCommand
        Cmd.Connection = Conn

        Try
            Conn.Open()
            Cmd.CommandText = "SELECT datpdp FROM pdp " & GeraClausulaWHERE_DataPDP_PDO(Page.Request.QueryString("strAcesso")) & " ORDER BY datpdp DESC"
            Dim rsData As SqlDataReader = Cmd.ExecuteReader
            Dim intI As Integer = 1
            Dim objItem As New System.Web.UI.WebControls.ListItem With {
            .Text = "",
            .Value = "0"
        }
            cboDataInicial.Items.Add(objItem)

            Do While rsData.Read
                objItem = New System.Web.UI.WebControls.ListItem With {
                .Text = Mid(rsData("datpdp"), 7, 2) & "/" & Mid(rsData("datpdp"), 5, 2) & "/" & Mid(rsData("datpdp"), 1, 4),
                .Value = Mid(rsData("datpdp"), 7, 2) & "/" & Mid(rsData("datpdp"), 5, 2) & "/" & Mid(rsData("datpdp"), 1, 4)
            }
                cboDataInicial.Items.Add(objItem)
                If Trim(cboDataInicial.Items(intI).Value) = Format(Session("datEscolhida"), "dd/MM/yyyy") Then
                    cboDataInicial.SelectedIndex = intI
                End If
                intI += 1
            Loop
            rsData.Close()
            Conn.Close()
        Catch
            lblMsg.Visible = True
            If Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If
        End Try
    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        If SessaoAtiva(Page.Session) Then
            MyBase.Render(writer)
            'objMenu.RenderControl(writer)
        End If
    End Sub

    Private Sub ConsultaIntervalo(ByVal strcampo As String, siglaTIpoUsina As String)
        Dim Conn As SqlConnection = New SqlConnection
        Dim Cmd As SqlCommand = New SqlCommand
        Cmd.Connection = Conn
        Dim objRow As TableRow
        Dim objCell As TableCell
        Dim strCodUsina As String
        Dim data As String

        ' Verifica se há um item selecionado em cboUsina e cboDataInicial
        If cboUsina.SelectedItem IsNot Nothing Then
            strCodUsina = cboUsina.SelectedItem.Value
        Else
            ' Defina um valor padrão ou lança uma mensagem de erro
            strCodUsina = ""
            lblMensagem.Text = "Por favor, selecione uma usina antes de continuar."
            lblMensagem.Visible = True
            Exit Sub
        End If

        If cboDataInicial.SelectedItem IsNot Nothing Then
            data = Convert.ToDateTime(cboDataInicial.SelectedItem.Value).ToString("yyyyMMdd")
        Else
            ' Define um valor padrão ou lança uma mensagem de erro
            data = ""
            lblMensagem.Text = "Por favor, selecione uma data inicial antes de continuar."
            lblMensagem.Visible = True
            Exit Sub
        End If

        Dim intI, intCol, intTotal, intHora, intTamanho, intJ As Integer
        Try
            'data pdp

            If siglaTIpoUsina <> "" Then
                If strCodUsina <> "Todas as Usinas" Then
                    Cmd.CommandText = "Select d.codusina, d.intdsp, d.valdsp" & strcampo & " As valor, u.ordem, u.nomusina " &
                              "FROM disponibilidade d, usina u " &
                              "WHERE u.codempre = '" & cboEmpresa.SelectedItem.Value & "' " &
                              "AND u.codusina = '" & cboUsina.SelectedItem.Value & "'" &
                              "AND d.codusina = '" & cboUsina.SelectedItem.Value & "'" &
                              "AND d.datpdp = '" & data & "' " &
                              "AND u.tipusina = '" & siglaTIpoUsina & "' " &
                              "AND u.flg_recebepdpage = 'S' " &
                              "ORDER BY u.ordem, d.codusina, d.intdsp"
                Else
                    Cmd.CommandText = "SELECT d.codusina, d.intdsp, d.valdsp" & strcampo & " AS valor, u.ordem, u.nomusina " &
                      "FROM disponibilidade d, usina u " &
                      "WHERE u.codempre = '" & cboEmpresa.SelectedItem.Value & "' " &
                      "AND u.codusina = d.codusina " &
                      "AND d.datpdp = '" & data & "' " &
                      "AND u.tipusina = '" & siglaTIpoUsina & "' " &
                      "AND u.flg_recebepdpage = 'S' " &
                      "ORDER BY u.ordem, d.codusina, d.intdsp"
                End If
            End If
            Conn.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
            Conn.Open()
            Dim rsConsulta As SqlDataReader = Cmd.ExecuteReader

            Dim objTamanho As System.Web.UI.WebControls.Unit
            objTamanho = New Unit

            'Colocar cor na tabela (RGB(233, 244, 207) = cor padrão ONS)
            Dim Color As System.Drawing.Color
            Color = New System.Drawing.Color
            Color = System.Drawing.ColorTranslator.FromWin32(RGB(233, 244, 207))

            'nova Celula
            tblConsulta.Width = objTamanho.Pixel(100)
            intTamanho = 100
            objRow = New TableRow
            objRow.BackColor = System.Drawing.Color.YellowGreen
            objCell = New TableCell
            objCell.Font.Bold = True
            objCell.Width = objTamanho.Pixel(100)
            objCell.Text = "Intervalo"
            objRow.Controls.Add(objCell)

            objCell = New TableCell
            objCell.Font.Bold = True
            objCell.Width = objTamanho.Pixel(100)
            objCell.Text = "Total"
            objRow.Controls.Add(objCell)

            tblConsulta.Controls.Add(objRow)

            intHora = 0
            For intI = 1 To 48
                objRow = New TableRow
                If intI Mod 2 = 0 Then
                    'quando linha = par troca cor
                    objRow.BackColor = Color
                End If

                objCell = New TableCell
                objCell.Text = IntParaHora(intI)
                'If intI Mod 2 <> 0 Then
                '    objCell.Text = Format(intHora - 1, "00") & ":00" & "-" & Format(intHora, "00") & ":30"
                '    intHora = intHora + 1
                'Else
                '    objCell.Text = Format(intHora - 1, "00") & ":30" & "-" & Format(intHora, "00") & ":00"
                'End If
                objRow.Controls.Add(objCell)
                objCell = New TableCell
                objRow.Controls.Add(objCell)
                tblConsulta.Controls.Add(objRow)
            Next
            'Total
            objRow = New TableRow
            If intI Mod 2 = 0 Then
                'quando linha = par troca cor
                objRow.BackColor = Color
            End If
            objCell = New TableCell
            objCell.Text = "Total"
            objCell.Font.Bold = True
            objRow.Controls.Add(objCell)
            objCell = New TableCell
            objCell.Font.Bold = True
            objRow.Controls.Add(objCell)
            tblConsulta.Controls.Add(objRow)
            intI = intI + 1
            'Média
            objRow = New TableRow
            If intI Mod 2 = 0 Then
                'quando linha = par troca cor
                objRow.BackColor = Color
            End If
            objCell = New TableCell
            objCell.Text = "Média"
            objCell.Font.Bold = True
            objRow.Controls.Add(objCell)
            objCell = New TableCell
            objCell.Font.Bold = True
            objRow.Controls.Add(objCell)
            tblConsulta.Controls.Add(objRow)
            intI = 0
            strCodUsina = ""

            Dim strUsina As String = ""
            Dim intTamanhoColunaUsina As Integer = 0

            Do While rsConsulta.Read

                If Page.Request.QueryString("strAcesso") <> "PDOC" Then
                    strUsina = rsConsulta("codusina")
                    intTamanhoColunaUsina = 70
                Else
                    strUsina = rsConsulta("nomusina")
                    intTamanhoColunaUsina = 100
                End If

                If strCodUsina <> rsConsulta("codusina") Then
                    If intI <> 0 Then
                        objCell = New TableCell
                        objCell.Font.Bold = True
                        objCell.Text = intTotal / 2
                        tblConsulta.Rows(49).Controls.Add(objCell)

                        objCell = New TableCell
                        objCell.Font.Bold = True
                        objCell.Text = Int(IIf(intTotal = 0, intTotal, intTotal / 48))
                        tblConsulta.Rows(50).Controls.Add(objCell)
                        intTotal = 0
                    End If
                    intTamanho = intTamanho + intTamanhoColunaUsina
                    tblConsulta.Width = objTamanho.Pixel(intTamanho)
                    objCell = New TableCell
                    objCell.Font.Bold = True
                    objCell.Width = objTamanho.Pixel(intTamanhoColunaUsina)
                    'objCell.Text = rsConsulta("codusina")
                    objCell.Text = strUsina
                    tblConsulta.Rows(0).Controls.Add(objCell)
                    strCodUsina = rsConsulta("codusina")
                    intI = 1
                End If

                'Nova coluna da tabela
                objCell = New TableCell
                objCell.Wrap = False
                objCell.Text = IIf(Not IsDBNull(rsConsulta("valor")), rsConsulta("valor"), 0)
                intTotal = intTotal + objCell.Text
                tblConsulta.Rows(intI).Controls.Add(objCell)
                intI = intI + 1
            Loop

            If intI > 0 Then
                objCell = New TableCell
                objCell.Font.Bold = True
                objCell.Text = intTotal / 2
                tblConsulta.Rows(49).Controls.Add(objCell)

                objCell = New TableCell
                objCell.Font.Bold = True
                objCell.Text = Int(IIf(intTotal = 0, intTotal, intTotal / 48))
                tblConsulta.Rows(50).Controls.Add(objCell)
            End If
            rsConsulta.Close()

            If Page.Request.QueryString("strAcesso") <> "PDOC" Then
                If Trim(optDados.SelectedItem.Value) = "2" Then
                    rsConsulta.Close()
                    Cmd.CommandText = "SELECT CASE codstatu WHEN 99 THEN '" & strMsgValidado & "' ELSE '" & strMsgNaoValidado & "' END AS msg " &
                                      "FROM pdp " &
                                      "WHERE datpdp = '" & cboDataInicial.SelectedItem.Value & "'"

                    rsConsulta = Cmd.ExecuteReader
                    Do While rsConsulta.Read
                        lblMensagem.Text = rsConsulta("msg")
                        lblMensagem.Visible = True
                    Loop
                    rsConsulta.Close()
                End If
            End If

            Conn.Close()

            intTotal = 0
            Dim intTotalT As Integer = 0
            For intI = 1 To 48
                For intJ = 2 To tblConsulta.Rows(0).Cells.Count - 1
                    intTotal = intTotal + tblConsulta.Rows(intI).Cells(intJ).Text
                Next
                tblConsulta.Rows(intI).Cells(1).Text = intTotal ' / 2
                intTotalT = intTotalT + intTotal
                intTotal = 0
            Next
            tblConsulta.Rows(49).Cells(1).Text = intTotalT / 2
            tblConsulta.Rows(50).Cells(1).Text = Int(intTotalT / 48)
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

    Private Overloads Sub btnVisualizar_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnVisualizar.Click
        Dim strcampo As String
        lblMensagem.Text = ""
        If cboDataInicial.SelectedIndex <> 0 Then
            Session("datEscolhida") = CDate(cboDataInicial.SelectedItem.Text)
        End If
        If cboEmpresa.SelectedIndex <> 0 Then
            Session("strCodEmpre") = cboEmpresa.SelectedItem.Value
        End If
        If cboUsina.SelectedIndex > 0 AndAlso cboUsina.SelectedItem IsNot Nothing Then
            Session("strCodUsina") = cboUsina.SelectedItem.Value
        Else
            ' Trate o caso em que nenhum item está selecionado
            Session("strCodUsina") = String.Empty ' ou um valor padrão, se necessário
        End If
        If OptTermica.Checked Then
            siglaTipoUsina = "T"
        ElseIf OptHidraulica.Checked Then
            siglaTipoUsina = "H"
        End If

        ' Verifica qual dados deverá ser consultado
        If Trim(optDados.SelectedItem.Value) = "0" Then
            strcampo = "tran"
        ElseIf Trim(optDados.SelectedItem.Value) = "1" Then
            strcampo = "emp"
        ElseIf Trim(optDados.SelectedItem.Value) = "2" Then
            strcampo = "pro"
        ElseIf Trim(optDados.SelectedItem.Value) = "3" Then
            strcampo = "pre"
        ElseIf Trim(optDados.SelectedItem.Value) = "4" Then
            strcampo = "sup"
        End If

        'intervalo
        ConsultaIntervalo(strcampo, siglaTipoUsina)

        'Chama uma nova janela para exibir a planilha e passa os parâmetros por query string
        imgPlanilha.Attributes.Add("onclick",
                                   "window.open('frmPlanilha.aspx?" &
                                   "strDataPDP=" & cboDataInicial.SelectedValue & "&" &
                                   "strCampo=" & strcampo & "&" &
                                   "strEmpresa=" & cboEmpresa.SelectedValue.Trim & "|" & cboEmpresa.SelectedItem.Text.Trim & "&" &
                                   "strTabela=disponibilidade&" &
                                   "strBase=PDP&" &
                                   "strAcesso=" & Page.Request.QueryString("strAcesso") & "'" &
                                   ",'Planilha','height = 600, width = 850, toolbar=yes,location=no,status=no,menubar=yes,scrollbars=yes,scrolling=yes,resizebled=yes');")
        imgPlanilha.Visible = True
    End Sub

    Private Sub btnSelecionarUsina_Click(sender As Object, e As ImageClickEventArgs) Handles btnSelecionarUsina.Click
        If OptTermica.Checked Then
            cboUsina.Items.Clear()
            cboEmpresa.ClearSelection()
            cboDataInicial.Enabled = True
            siglaTipoUsina = "T"
            cboEmpresa.Enabled = True
            cboUsina.Enabled = True
            lblUsinaSelecionada.Text = "Consulta de dados de Disponibilidade - USINAS TÉRMICAS"
            lblUsinaSelecionada.Visible = True

        ElseIf OptHidraulica.Checked Then
            cboUsina.Items.Clear()
            cboEmpresa.ClearSelection()
            cboDataInicial.Enabled = True
            siglaTipoUsina = "H"
            cboEmpresa.Enabled = True
            cboUsina.Enabled = True
            lblUsinaSelecionada.Text = "Consulta de dados de Disponibilidade - USINAS HIDRÁULICAS"
            lblUsinaSelecionada.Visible = True
        Else
            cboDataInicial.Enabled = False
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

    Private Sub cboEmpresa_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboEmpresa.SelectedIndexChanged
        Dim intI As Integer
        Dim Conn As SqlConnection = New SqlConnection
        Dim Cmd As SqlCommand = New SqlCommand

        Conn.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
        Cmd.Connection = Conn

        Dim strCodUsina As String
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
                              "u.tipusina = '" & siglaTipoUsina & "' " &
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
                              "      d.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' And " &
                          "      And u.flg_recebepdpage = 'S' " &
                          "Group By u.ordem, " &
                          "         d.codusina " &
                          "Order By u.ordem, " &
                          "         d.codusina"

            End If

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
            cboUsina.Enabled = True
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


    Private Sub cboDataInicial_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboDataInicial.SelectedIndexChanged
        Try
            If cboDataInicial.SelectedIndex <> 0 Then
                Session("datEscolhida") = CDate(cboDataInicial.SelectedItem.Value)
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

    Protected Sub OptTermica_CheckedChanged(sender As Object, e As EventArgs) Handles OptTermica.CheckedChanged
        If OptTermica.Checked Then
            ' Seta o tipo Usina para termica
            siglaTipoUsina = "T"

            ' Limpa o Combo de Usina 
            cboUsina.Items.Clear()


            lblUsinaSelecionada.Text = "Consulta de dados de Disponibilidade - USINAS TÉRMICAS"
            lblUsinaSelecionada.Visible = True

            ' Atualiza o dropdown de empresa 
            ConfigurarEmpresasPorTipo()
        End If
    End Sub

    Protected Sub OptHidraulica_CheckedChanged(sender As Object, e As EventArgs) Handles OptHidraulica.CheckedChanged
        If OptHidraulica.Checked Then
            ' Seta o tipo Usina para Hidraulica
            siglaTipoUsina = "H"

            ' Limpa o Combo de Usina
            cboUsina.Items.Clear()

            lblUsinaSelecionada.Text = "Consulta de dados de Disponibilidade - USINAS HIDRÁULICAS"
            lblUsinaSelecionada.Visible = True

            '  Atualiza o dropdown de empresa 
            ConfigurarEmpresasPorTipo()
        End If
    End Sub
    Private Sub ConfigurarEmpresasPorTipo()
        cboEmpresa.Items.Clear()

        ' Configura a conexão e o comando SQL
        Dim Conn As SqlConnection = New SqlConnection(ConfigurationManager.AppSettings("pdpSQL").ToString())
        Dim Cmd As SqlCommand = New SqlCommand
        Cmd.Connection = Conn

        ' Query para buscar as empresas de acordo com o tipo de usina selecionado
        Dim query As String = "SELECT DISTINCT e.codempre, e.nomempre " &
                              "FROM empre e " &
                              "JOIN usina u ON e.codempre = u.codempre " &
                              "WHERE u.tipusina = @TipoUsina " &
                              " ORDER BY e.nomempre"

        Cmd.CommandText = query
        Cmd.Parameters.AddWithValue("@TipoUsina", siglaTipoUsina)

        Try
            Conn.Open()
            Dim rsEmpresa As SqlDataReader = Cmd.ExecuteReader
            Dim objItem As ListItem = New ListItem("Selecione uma Empresa", "0")
            cboEmpresa.Items.Add(objItem)

            ' Carrega as empresas no dropdown
            While rsEmpresa.Read()
                objItem = New ListItem(rsEmpresa("nomempre").ToString(), rsEmpresa("codempre").ToString())
                cboEmpresa.Items.Add(objItem)
            End While

            rsEmpresa.Close()
            cboEmpresa.Enabled = True
        Catch ex As Exception
            lblMsg.Visible = True
            lblMsg.Text = "Não foi possível acessar a Base de Dados."
        Finally
            If Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If
        End Try
    End Sub
End Class

