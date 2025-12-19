Imports System.Collections.Generic

Partial Class frmColVazao

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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not Page.IsPostBack Then
            If Session("datEscolhida") Is Nothing Then
                ' Inicializa a variável com a data do próximo dia
                Session("datEscolhida") = Now.AddDays(1)
            End If

            ' Obtém os dados do cache ou realiza a consulta ao banco de dados se o cache estiver vazio
            Dim pdpData As List(Of ListItem) = CacheDataPDP.GetPdpData(True)

            Dim intI As Integer = 1
            Dim objItem As New WebControls.ListItem
            objItem.Text = ""
            objItem.Value = "0"
            cboData.Items.Add(objItem)

            ' Preenche o combo box com os dados recuperados
            For Each item As WebControls.ListItem In pdpData
                cboData.Items.Add(item)

                ' Define o item selecionado com base na data escolhida armazenada na sessão
                If Trim(cboData.Items(intI).Value) = Format(Session("datEscolhida"), "dd/MM/yyyy") Then
                    cboData.SelectedIndex = intI
                End If

                intI += 1
            Next

            PreencheComboEmpresaPOP(AgentesRepresentados, cboEmpresa, Session("strCodEmpre"))

            If cboData.SelectedIndex > 0 Then
                cboData_SelectedIndexChanged(sender, e)
            End If
        End If
    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        If SessaoAtiva(Page.Session) Then
            Funcoes.RetirarItensSelecionadosComDuplicidade(cboData)
            Funcoes.RetirarItensSelecionadosComDuplicidade(cboEmpresa)

            MyBase.Render(writer)
            'objMenu.RenderControl(writer)
        End If
    End Sub

    Private Sub cboEmpresa_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboEmpresa.SelectedIndexChanged
        If cboEmpresa.SelectedIndex > 0 Then
            Session("strCodEmpre") = cboEmpresa.SelectedItem.Value
        End If

        PreencheTable()

        'Valida Limite de Envio
        Dim lRetorno As Integer = 0
        If Not ValidaLimiteEntradaDados(cboEmpresa.SelectedItem.Value, cboData.SelectedItem.Value, lRetorno) Then
            btnSalvar.Visible = False
            If lRetorno = 1 Then
                Response.Write("<SCRIPT>alert('" + strMsgInicioLimiteEnvioDados + "')</SCRIPT>")
            Else
                Response.Write("<SCRIPT>alert('" + strMsgLimiteEnvioDados + "')</SCRIPT>")
            End If
            Exit Sub
        Else
            btnSalvar.Visible = True
        End If
    End Sub

    Private Sub PreencheTable()
        Dim intI As Integer
        Dim intLin As Integer
        Dim dblMediaT, dblMediaV, dblMediaA, dblMediaC, dblMediaCF, dblMediaVT, dblMediaOE As Double
        Dim strCel As System.Web.UI.WebControls.TableCell
        Dim strRow As System.Web.UI.WebControls.TableRow
        Dim txtValor As System.Web.UI.WebControls.TextBox
        'Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
        'Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
        Dim Conn As System.Data.SqlClient.SqlConnection = New System.Data.SqlClient.SqlConnection
        Conn.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
        Dim Cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand
        Cmd.Connection = Conn
        '-- CRQ2345 (15/08/2012)
        Try
            Cmd.CommandText = "SELECT v.codusina, v.valturbtran, v.valverttran, v.valaflutran, v.valtransftran, c.cotainitran, c.cotafimtran, c.outrasestruturastran, c.comentariopdftran, u.ordem " &
                              "FROM vazao v inner join usina u on (u.codusina = v.codusina) " &
                              " left join cota c on (u.codusina = c.codusina) " &
                              "WHERE u.codempre = '" & Session("strCodEmpre") & "' " &
                              "AND u.tipusina = 'H' " &
                              "AND v.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' " &
                              "AND c.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' " &
                              "AND u.flg_recebepdpage = 'S' " &
                              "ORDER BY u.ordem, v.codusina"
            Conn.Open()
            Dim rsUsina As System.Data.SqlClient.SqlDataReader = Cmd.ExecuteReader

            Dim Color As System.Drawing.Color
            Color = New System.Drawing.Color
            Color = System.Drawing.ColorTranslator.FromWin32(RGB(233, 244, 207))

            Dim Tamanho As System.Web.UI.WebControls.Unit
            Tamanho = New System.Web.UI.WebControls.Unit

            tblVazao.Rows.Clear()
            strRow = New TableRow
            strCel = New TableCell

            'Usina
            strCel.BackColor = System.Drawing.Color.Beige
            strCel.Font.Bold = True
            strCel.Font.Size.Unit.Point(10)
            strCel.Text = "Usina"
            strRow.Controls.Add(strCel)
            'Turbinada
            strCel = New TableCell
            strCel.BackColor = System.Drawing.Color.Beige
            strCel.Font.Bold = True
            strCel.Text = "Turbinada"
            strRow.Controls.Add(strCel)
            'Vertida
            strCel = New TableCell
            strCel.BackColor = System.Drawing.Color.Beige
            strCel.Font.Bold = True
            strCel.Text = "Vertida"
            strRow.Controls.Add(strCel)
            'Afluente
            strCel = New TableCell
            strCel.BackColor = System.Drawing.Color.Beige
            strCel.Font.Bold = True
            strCel.Text = "Afluente"
            strRow.Controls.Add(strCel)
            'Cota Inicial
            strCel = New TableCell
            strCel.BackColor = System.Drawing.Color.Beige
            strCel.Font.Bold = True
            strCel.Text = "Cota Inicial"
            strRow.Controls.Add(strCel)
            '-- CRQ2345 (15/08/2012)
            'Cota Final
            strCel = New TableCell
            strCel.BackColor = System.Drawing.Color.Beige
            strCel.Font.Bold = True
            strCel.Text = "Cota Final"
            strRow.Controls.Add(strCel)
            'Outras Estruturas
            strCel = New TableCell
            strCel.BackColor = System.Drawing.Color.Beige
            strCel.Font.Bold = True
            strCel.Text = "Outras Estr"
            strRow.Controls.Add(strCel)
            'valtransftran
            strCel = New TableCell
            strCel.BackColor = System.Drawing.Color.Beige
            strCel.Font.Bold = True
            strCel.Text = "Vazão Transferida"
            strRow.Controls.Add(strCel)
            'Comentario PDF
            strCel = New TableCell
            strCel.BackColor = System.Drawing.Color.Beige
            strCel.Font.Bold = True
            strCel.Text = "Comentário PDF"
            strRow.Controls.Add(strCel)


            tblVazao.Controls.Add(strRow)

            intLin = 0
            dblMediaT = 0
            dblMediaV = 0
            Do While rsUsina.Read
                intLin = intLin + 1
                'Nova linha
                strRow = New TableRow
                'Coluna da Usina
                strCel = New TableCell
                strCel.BackColor = System.Drawing.Color.Beige
                txtValor = New System.Web.UI.WebControls.TextBox
                txtValor.BorderStyle = BorderStyle.None
                txtValor.Width = Tamanho.Pixel(70)
                txtValor.BackColor = System.Drawing.Color.Beige
                txtValor.Font.Bold = True
                txtValor.ID = "UsinaID_" & intLin
                txtValor.Text = rsUsina("codusina")
                txtValor.ReadOnly = True
                strCel.Controls.Add(txtValor)
                strRow.Controls.Add(strCel)

                'Coluna do Valor da Turbinada
                txtValor = New System.Web.UI.WebControls.TextBox
                txtValor.BorderStyle = BorderStyle.None
                txtValor.Width = Tamanho.Pixel(70)
                txtValor.ID = "Turbinada_" & intLin
                txtValor.Text = IIf(Not IsDBNull(rsUsina.Item("valturbtran")), rsUsina.Item("valturbtran"), 0)
                strCel = New TableCell
                strCel.Controls.Add(txtValor)
                strRow.Controls.Add(strCel)
                dblMediaT += txtValor.Text

                'Coluna do Valor da Vertida
                txtValor = New System.Web.UI.WebControls.TextBox
                txtValor.BorderStyle = BorderStyle.None
                txtValor.Width = Tamanho.Pixel(70)
                txtValor.ID = "Vertida_" & intLin
                txtValor.Text = IIf(Not IsDBNull(rsUsina.Item("valverttran")), rsUsina.Item("valverttran"), 0)
                strCel = New TableCell
                strCel.Controls.Add(txtValor)
                strRow.Controls.Add(strCel)
                dblMediaV += txtValor.Text

                'Coluna da Vazão Afluente
                txtValor = New System.Web.UI.WebControls.TextBox
                txtValor.BorderStyle = BorderStyle.None
                txtValor.Width = Tamanho.Pixel(70)
                txtValor.ID = "Afluente_" & intLin
                txtValor.Text = IIf(Not IsDBNull(rsUsina.Item("valaflutran")), rsUsina.Item("valaflutran"), 0)
                strCel = New TableCell
                strCel.Controls.Add(txtValor)
                strRow.Controls.Add(strCel)
                dblMediaA += txtValor.Text

                'Coluna da Cota Inicial
                txtValor = New System.Web.UI.WebControls.TextBox
                txtValor.BorderStyle = BorderStyle.None
                txtValor.Width = Tamanho.Pixel(70)
                txtValor.ID = "Cota_" & intLin
                txtValor.Text = IIf(Not IsDBNull(rsUsina.Item("cotainitran")), rsUsina.Item("cotainitran"), 0)
                strCel = New TableCell
                strCel.Controls.Add(txtValor)
                strRow.Controls.Add(strCel)
                dblMediaC += txtValor.Text

                '-- CRQ2345 (15/08/2012)
                'Coluna da Cota Final
                txtValor = New System.Web.UI.WebControls.TextBox
                txtValor.BorderStyle = BorderStyle.None
                txtValor.Width = Tamanho.Pixel(70)
                txtValor.ID = "Cotaf_" & intLin
                txtValor.Text = IIf(Not IsDBNull(rsUsina.Item("Cotafimtran")), rsUsina.Item("Cotafimtran"), 0)
                strCel = New TableCell
                strCel.Controls.Add(txtValor)
                strRow.Controls.Add(strCel)
                dblMediaCF += txtValor.Text

                'Coluna de Outras Estruturas
                txtValor = New System.Web.UI.WebControls.TextBox
                txtValor.BorderStyle = BorderStyle.None
                txtValor.Width = Tamanho.Pixel(70)
                txtValor.ID = "OutrasEstr_" & intLin
                txtValor.Text = IIf(Not IsDBNull(rsUsina.Item("outrasestruturastran")), rsUsina.Item("outrasestruturastran"), 0)
                strCel = New TableCell
                strCel.Controls.Add(txtValor)
                strRow.Controls.Add(strCel)
                dblMediaOE += txtValor.Text

                'Coluna de valtransftran
                txtValor = New System.Web.UI.WebControls.TextBox
                txtValor.BorderStyle = BorderStyle.None
                txtValor.Width = Tamanho.Pixel(70)
                txtValor.ID = "Valtransftran_" & intLin
                txtValor.Text = IIf(Not IsDBNull(rsUsina.Item("valtransftran")), rsUsina.Item("valtransftran"), 0)
                strCel = New TableCell
                strCel.Controls.Add(txtValor)
                strRow.Controls.Add(strCel)
                dblMediaVT += txtValor.Text

                'Coluna de Comentário PDF
                txtValor = New System.Web.UI.WebControls.TextBox
                txtValor.BorderStyle = BorderStyle.None
                txtValor.Width = Tamanho.Pixel(200)
                txtValor.MaxLength = 256
                txtValor.ToolTip = IIf(Not IsDBNull(rsUsina.Item("comentariopdftran")), rsUsina.Item("comentariopdftran"), "")
                txtValor.ID = "ComentPdf_" & intLin
                txtValor.Text = IIf(Not IsDBNull(rsUsina.Item("comentariopdftran")), rsUsina.Item("comentariopdftran"), "")
                strCel = New TableCell
                strCel.Controls.Add(txtValor)
                strRow.Controls.Add(strCel)


                'Incluindo a linha na tabela
                tblVazao.Controls.Add(strRow)

            Loop
            Session("intQtdLinha") = intLin
            'Total Turbinada, Vertida, Afluente, Cota Inicial, Cota Final e Outras Estruturas e valtransftran
            strRow = New TableRow
            strCel = New TableCell
            strCel.BackColor = System.Drawing.Color.Beige
            strCel.Font.Bold = True
            strCel.Text = "Total"
            strRow.Controls.Add(strCel)
            'Total Turbinada
            strCel = New TableCell
            strCel.BackColor = System.Drawing.Color.Beige
            strCel.Controls.Add(New LiteralControl(dblMediaT))
            strRow.Controls.Add(strCel)
            'Total Vertida
            strCel = New TableCell
            strCel.BackColor = System.Drawing.Color.Beige
            strCel.Controls.Add(New LiteralControl(dblMediaV))
            strRow.Controls.Add(strCel)
            tblVazao.Controls.Add(strRow)
            'Total Afluente
            strCel = New TableCell
            strCel.BackColor = System.Drawing.Color.Beige
            strCel.Controls.Add(New LiteralControl(dblMediaA))
            strRow.Controls.Add(strCel)
            tblVazao.Controls.Add(strRow)
            'Total Cota Inicial
            strCel = New TableCell
            strCel.BackColor = System.Drawing.Color.Beige
            strCel.Controls.Add(New LiteralControl(Format(dblMediaC, "######0.00")))
            strRow.Controls.Add(strCel)
            tblVazao.Controls.Add(strRow)
            '-- CRQ2345 (15/08/2012)
            'Total Cota Final
            strCel = New TableCell
            strCel.BackColor = System.Drawing.Color.Beige
            strCel.Controls.Add(New LiteralControl(Format(dblMediaCF, "######0.00")))
            strRow.Controls.Add(strCel)
            tblVazao.Controls.Add(strRow)
            'Total Outras Estruturas
            strCel = New TableCell
            strCel.BackColor = System.Drawing.Color.Beige
            strCel.Controls.Add(New LiteralControl(Format(dblMediaOE, "######0.00")))
            strRow.Controls.Add(strCel)
            tblVazao.Controls.Add(strRow)
            'Total valtransftran
            strCel = New TableCell
            strCel.BackColor = System.Drawing.Color.Beige
            strCel.Controls.Add(New LiteralControl(Format(dblMediaVT, "######0.00")))
            strRow.Controls.Add(strCel)
            tblVazao.Controls.Add(strRow)

            'Médias Turbinada, Vertida, Afluente, Cota Inicial, Cota Final e Outras Estruturas e valtransftran
            strRow = New TableRow
            strCel = New TableCell
            strCel.BackColor = System.Drawing.Color.Beige
            strCel.Font.Bold = True
            strCel.Text = "Média"
            strRow.Controls.Add(strCel)
            'Média Turbinada
            strCel = New TableCell
            strCel.BackColor = System.Drawing.Color.Beige
            If intLin = 0 Then
                strCel.Text = 0
            Else
                strCel.Controls.Add(New LiteralControl(Int(dblMediaT / intLin)))
            End If
            strRow.Controls.Add(strCel)
            'Média Vertida
            strCel = New TableCell
            strCel.BackColor = System.Drawing.Color.Beige
            If intLin = 0 Then
                strCel.Text = 0
            Else
                strCel.Controls.Add(New LiteralControl(Int(dblMediaV / intLin)))
            End If
            strRow.Controls.Add(strCel)
            'Média Afluente
            strCel = New TableCell
            strCel.BackColor = System.Drawing.Color.Beige
            If intLin = 0 Then
                strCel.Text = 0
            Else
                strCel.Controls.Add(New LiteralControl(Int(dblMediaA / intLin)))
            End If
            strRow.Controls.Add(strCel)
            'Média Cota Inicial
            strCel = New TableCell
            strCel.BackColor = System.Drawing.Color.Beige
            If intLin = 0 Then
                strCel.Text = 0
            Else
                strCel.Controls.Add(New LiteralControl(Format((dblMediaC / intLin), "######0.00")))
            End If
            strRow.Controls.Add(strCel)
            '-- CRQ2345 (15/08/2012)
            'Média Cota Final
            strCel = New TableCell
            strCel.BackColor = System.Drawing.Color.Beige
            If intLin = 0 Then
                strCel.Text = 0
            Else
                strCel.Controls.Add(New LiteralControl(Format((dblMediaCF / intLin), "######0.00")))
            End If
            strRow.Controls.Add(strCel)
            'Média Outras Estruturas
            strCel = New TableCell
            strCel.BackColor = System.Drawing.Color.Beige
            If intLin = 0 Then
                strCel.Text = 0
            Else
                strCel.Controls.Add(New LiteralControl(Format((dblMediaOE / intLin), "######0.00")))
            End If
            strRow.Controls.Add(strCel)
            'Média valtransftran
            strCel = New TableCell
            strCel.BackColor = System.Drawing.Color.Beige
            If intLin = 0 Then
                strCel.Text = 0
            Else
                strCel.Controls.Add(New LiteralControl(Format((dblMediaVT / intLin), "######0.00")))
            End If
            strRow.Controls.Add(strCel)

            tblVazao.Controls.Add(strRow)

            rsUsina.Close()
            rsUsina = Nothing
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
    End Sub

    Private Sub cboData_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboData.SelectedIndexChanged
        Try
            If cboData.SelectedIndex > 0 Then
                Session("datEscolhida") = CDate(cboData.SelectedItem.Value)
            End If
            If cboEmpresa.SelectedIndex > 0 Then
                cboEmpresa_SelectedIndexChanged(sender, e)
            End If
        Catch
            Session("strMensagem") = "Não foi possível acessar a Base de Dados."
            Response.Redirect("frmMensagem.aspx")
        End Try
    End Sub

    Private Sub btnSalvar_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSalvar.Click
        Dim intI As Integer
        Dim strCodUsina, strComentPdf As String
        Dim dblValTurbTran, dblValVertTran, dblValAfluTran, dblValCotaTran, dblValCotafTran, dblValOutrasEstrTran, dblValtransftran, dblMediaT, dblMediaV, dblMediaA, dblMediaC, dblMediaCF, dblMediaVT, dblMediaOE As Double
        'Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
        'Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
        Dim Conn As System.Data.SqlClient.SqlConnection = New System.Data.SqlClient.SqlConnection
        Conn.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
        Dim Cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand
        Cmd.Connection = Conn
        Dim objTrans As System.Data.SqlClient.SqlTransaction
        Dim objText As System.Web.UI.WebControls.TextBox
        Try
            Conn.Open()
            'Conn.Servico = "PDPColVaza"
            'Conn.Usuario = UsuarID

            'Alterando os valores de carga na BDT
            objTrans = Conn.BeginTransaction()
            Cmd.Transaction = objTrans

            objText = New System.Web.UI.WebControls.TextBox
            For intI = 1 To Session("intQtdLinha")

                'Código da Usina
                strCodUsina = Page.Request.Form.Item("_ctl0:ContentPlaceHolder1:UsinaID_" & intI)
                'valor Turbinada
                dblValTurbTran = Page.Request.Form.Item("_ctl0:ContentPlaceHolder1:Turbinada_" & intI)
                dblMediaT = dblMediaT + dblValTurbTran
                'valor Vertida
                dblValVertTran = Page.Request.Form.Item("_ctl0:ContentPlaceHolder1:Vertida_" & intI)
                dblMediaV = dblMediaV + dblValVertTran
                'valor Afluente
                dblValAfluTran = Page.Request.Form.Item("_ctl0:ContentPlaceHolder1:Afluente_" & intI)
                dblMediaA = dblMediaA + dblValAfluTran
                'valor Cota Inicial
                dblValCotaTran = Page.Request.Form.Item("_ctl0:ContentPlaceHolder1:Cota_" & intI)
                dblMediaC = dblMediaC + dblValCotaTran
                '-- CRQ2345 (15/08/2012)
                'valor Cota Final
                dblValCotafTran = Page.Request.Form.Item("_ctl0:ContentPlaceHolder1:Cotaf_" & intI)
                dblMediaCF = dblMediaCF + dblValCotafTran
                'valor Outras Estruturas
                dblValOutrasEstrTran = Page.Request.Form.Item("_ctl0:ContentPlaceHolder1:OutrasEstr_" & intI)
                dblMediaOE = dblMediaOE + dblValOutrasEstrTran
                'valor valtransftran
                dblValtransftran = Page.Request.Form.Item("_ctl0:ContentPlaceHolder1:Valtransftran_" & intI)
                dblMediaVT = dblMediaVT + dblValtransftran
                'valor Outras Estruturas
                strComentPdf = Page.Request.Form.Item("_ctl0:ContentPlaceHolder1:ComentPdf_" & intI)

                'Atualiza a Base
                Cmd.CommandText = "UPDATE vazao " &
                                  "SET valturbtran = " & dblValTurbTran & ", valtransftran = " & dblValtransftran & ", valverttran = " & dblValVertTran & ", valaflutran = " & dblValAfluTran & " " &
                                  "WHERE datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' " &
                                  "AND codusina = '" & strCodUsina & "'"
                Cmd.ExecuteNonQuery()

                'Atualiza a Base
                Cmd.CommandText = "UPDATE cota " & _
                                  "SET cotainitran = " & dblValCotaTran.ToString.Replace(",", ".") & ", " & _
                                      "cotafimtran = " & dblValCotafTran.ToString.Replace(",", ".") & ", " & _
                                      "outrasestruturastran = " & dblValOutrasEstrTran.ToString.Replace(",", ".") & ", " & _
                                      "comentariopdftran = '" & strComentPdf & "' " & _
                                  "WHERE datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' " & _
                                  "AND codusina = '" & strCodUsina & "'"
                Cmd.ExecuteNonQuery()

            Next

            'Grava eventos informando entrega da Vazão
            GravaEventoPDP("6", Format(Session("datEscolhida"), "yyyyMMdd"), cboEmpresa.SelectedItem.Text, Now, "PDPColVaza", UsuarID)

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
            PreencheTable()
        Catch
            Session("strMensagem") = "Não foi possível acessar a Base de Dados."
            'Conn.Close()
            Response.Redirect("frmMensagem.aspx")
        End Try
    End Sub
End Class
