Imports System.Text

Public Class frmConsultaMarcoProgramacao
    Inherits System.Web.UI.Page

    Dim indice_inicial As Integer = 0         ' Índice inicial para paginação.
    Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
    Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand

    Private Property Datagrid1 As Object

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Put user code to initialize the page here
        'objMenu = CType(Session("onsmenu"), OnsWebControls.OnsMenu)
        Try
            If Not Page.IsPostBack Then
                CarregaComboData()

                'Pontera na data atual
                cboData.SelectedIndex = cboData.Items.IndexOf(cboData.Items.FindByValue(DateTime.Now.ToString("yyyyMMdd")))
                'cboData.SelectedIndex = cboData.Items.IndexOf(cboData.Items.FindByValue(DateTime.Now.ToString("20151010")))

                indice_inicial = 0
                dtgMarcosProgramacao.CurrentPageIndex = 0

                DataGridBind()
            End If
        Catch ex As Exception
            Session("strMensagem") = "Erro ao consultar. (" + ex.Message + ")"
            Response.Redirect("frmMensagem.aspx")
        End Try

    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        If SessaoAtiva(Page.Session) Then
            MyBase.Render(writer)
            'objMenu.RenderControl(writer)
        End If
    End Sub

    Private Sub DataGridBind()
        Dim daMarcos As OnsClasses.OnsData.OnsDataAdapter
        Dim dsMarcos As DataSet

        Dim daUsuario As OnsClasses.OnsData.OnsDataAdapter

        Cmd.Connection = Conn
        Try
            Conn.Open("pdp")
            
            Cmd.CommandText = "SELECT distinct tb_marcopdp.id_marcopdp, tb_marcoprogpdp.id_marcoprogpdp, tb_marcopdp.datpdp, " & _
                              "tb_marcopdp.id_usuarsolicitante, tb_marcopdp.id_usuarresponsavel, " & _
                              "tb_marcopdp.din_marcopdp, tb_marcoprogpdp.cod_marcoprogpdp, tb_marcoprogpdp.dsc_marcoprogpdp, tb_marcoprogpdp.flg_publico, " & _
                              "tb_marcoprogpdp.hor_progpdp, tb_marcoprogpdp.flg_obrigatorio, tb_marcoprogpdp.nom_arquivoimagem, tb_marcoprogpdp.nom_arquivoimagemdesab, tb_marcoprogpdp.num_ordem " & _
                              "FROM outer tb_marcopdp as tb_marcopdp, tb_marcoprogpdp, tb_marcopdp as tb_marcopdpAux " & _
                              "WHERE tb_marcopdp.datpdp = '" & cboData.SelectedItem.Value & "' " & _
                              "and tb_marcopdp.id_marcoprogpdp = tb_marcoprogpdp.id_marcoprogpdp " & _
                              "and tb_marcopdpAux.id_marcoprogpdp = tb_marcoprogpdp.id_marcoprogpdp " & _
                              "ORDER BY tb_marcoprogpdp.num_ordem"

            Dim strSQLUsuarioSolicitante As String = "Select usuar_id, usuar_nome from usuar where usuar_id in (Select id_usuarsolicitante from tb_marcopdp where datpdp = '" & cboData.SelectedItem.Value & "')"
            daUsuario = New OnsClasses.OnsData.OnsDataAdapter(strSQLUsuarioSolicitante, Conn)
            Dim tbUsuarioSolicitante As New DataTable()
            daUsuario.Fill(tbUsuarioSolicitante)

            Dim strSQLUsuarioResponsavel As String = "Select usuar_id, usuar_nome from usuar where usuar_id in (Select id_usuarresponsavel from tb_marcopdp where datpdp = '" & cboData.SelectedItem.Value & "')"
            daUsuario = New OnsClasses.OnsData.OnsDataAdapter(strSQLUsuarioResponsavel, Conn)
            Dim tbUsuarioResponsavel As New DataTable()
            daUsuario.Fill(tbUsuarioResponsavel)


            daMarcos = New OnsClasses.OnsData.OnsDataAdapter(Cmd.CommandText, Conn)
            dsMarcos = New DataSet
            daMarcos.Fill(dsMarcos, "Marcos")
            dtgMarcosProgramacao.DataSource = dsMarcos.Tables("Marcos").DefaultView
            dtgMarcosProgramacao.DataBind()

            Dim ultimoMarcoExecutado As Integer = 0

            Dim corAzul As String = "#B0C4DE"
            Dim corBranca As String = "#FFFFFF"
            Dim corCinza As String = "#CCCCCC"

            Dim strHTMLGeral As String = ""
            Dim strHTMLMarcos As String = ""

            Dim strHTMLCabecalhoGeral As String = ""
            Dim strHTMLCabecalhoMarcos As String = ""

            Dim ehPrimeiroVazio As Boolean = True

            Dim numRegistro As Integer = 0

            Dim visulizaTodosMarcos As Boolean = (PerfilID <> Nothing And PerfilID = "ADM_PDPW")

            For Each linha As DataRow In dsMarcos.Tables("Marcos").Rows

                numRegistro = numRegistro + 1

                Dim exibeMarco As Boolean = True
                If (Not visulizaTodosMarcos) Then
                    exibeMarco = (linha("flg_publico") = "S")
                End If

                If (exibeMarco) Then

                    strHTMLGeral = geraMarco()

                    'Recupera nome dos Usuários
                    Dim dtr_Aux As DataRow()
                    Dim usuarioSolic As String = ""
                    dtr_Aux = tbUsuarioSolicitante.Select("usuar_id = '" + linha("id_usuarsolicitante") + "'")
                    If (dtr_Aux.Length > 0) Then
                        usuarioSolic = dtr_Aux(0)("usuar_nome")
                    End If


                    Dim usuarioResp As String = ""
                    dtr_Aux = tbUsuarioResponsavel.Select("usuar_id = '" + linha("id_usuarresponsavel") + "'")
                    If (dtr_Aux.Length > 0) Then
                        usuarioResp = dtr_Aux(0)("usuar_nome")
                    End If


                    Dim horaPrevisao As DateTime
                    Dim horaPrevisaoStr As String = ""

                    Dim dataHoraOcorrencia As DateTime
                    Dim dataHoraOcorrenciaStr As String = ""

                    Dim nomeArquivoSeta As String = "seta_cinza.png"
                    Dim nomeArquivoImagem As String = linha("nom_arquivoimagemdesab")
                    Dim corPainelMarco As String = corCinza
                    Dim imagemStatusMarco As String = "statusOk_cinza.png"
                    If (Not DBNull.Value.Equals(linha("id_marcopdp"))) Then
                        corPainelMarco = corAzul
                        imagemStatusMarco = "statusOk.png"
                        nomeArquivoImagem = linha("nom_arquivoimagem")
                        nomeArquivoSeta = "seta_azul.png"

                        horaPrevisao = Convert.ToDateTime(linha("hor_progpdp"))
                        horaPrevisaoStr = horaPrevisao.ToString("HH:mm")

                        dataHoraOcorrencia = Convert.ToDateTime(linha("din_marcopdp"))
                        dataHoraOcorrenciaStr = dataHoraOcorrencia.ToString("dd/MM/yyyy HH:mm")
                    Else
                        'Primeiro marco obrigatorio vazio fica aguardando
                        If (ehPrimeiroVazio) And (linha("flg_obrigatorio") = "S") Then
                            'If (ehPrimeiroVazio) Then
                            If (numRegistro = 1) Then
                                ehPrimeiroVazio = False
                            Else
                                corPainelMarco = corBranca
                                imagemStatusMarco = "aguardando.png"
                                nomeArquivoImagem = linha("nom_arquivoimagem")
                                ehPrimeiroVazio = False
                            End If
                        End If

                    End If


                    strHTMLMarcos = strHTMLMarcos & String.Format(strHTMLGeral, corPainelMarco, nomeArquivoImagem, horaPrevisaoStr, imagemStatusMarco, dataHoraOcorrenciaStr, usuarioSolic, usuarioResp, linha("dsc_marcoprogpdp"))


                    If (linha("flg_obrigatorio") = "S") Then
                        strHTMLCabecalhoGeral = geraImagemCabecalho(numRegistro < dsMarcos.Tables("Marcos").Rows.Count)
                        strHTMLCabecalhoMarcos = strHTMLCabecalhoMarcos & String.Format(strHTMLCabecalhoGeral, nomeArquivoImagem, nomeArquivoSeta)
                    End If

                End If

            Next

            litCabecalho.Text = strHTMLCabecalhoMarcos

            litMarcos.Text = strHTMLMarcos

        Catch ex As Exception
            If Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If
            Session("strMensagem") = "Erro ao consultar. (" + ex.Message + ")"
            Response.Redirect("frmMensagem.aspx")
        Finally
            If Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If
        End Try
    End Sub


    Private Sub CarregaComboData()
        Dim intI As Integer
        Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
        Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand

        Cmd.Connection = Conn

        Try
            cboData.Items.Clear()
            Conn.Open("pdp")

            Cmd.CommandText = "Select datpdp From pdp Order By datpdp Desc"

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
                objItem.Value = rsData("datpdp").ToString()
                cboData.Items.Add(objItem)
            Loop

            rsData.Close()
            rsData = Nothing
            Cmd.Connection.Close()
            Conn.Close()
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


    Protected Sub cboData_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboData.SelectedIndexChanged
        DataGridBind()

        'If cboData.SelectedIndex <> 0 Then
        '    Session("datEscolhida") = CDate(cboData.SelectedItem.Value)
        'End If
    End Sub

    Protected Sub dtgResponsaveis_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        dtgMarcosProgramacao.CurrentPageIndex = e.NewPageIndex
        DataGridBind()
    End Sub

    Private Function geraMarco() As String

        Dim sb As StringBuilder = New StringBuilder()

        ' {0} --> Painel background-color
        ' {1} --> ImgMarco arquivo.png
        ' {2} --> lblPrevisao
        ' {3} --> imgStatusMarco
        ' {4} --> lblDataOcorrencia
        ' {5} --> lblUsuSolic
        ' {6} --> lblUsuResp
        sb.AppendLine("<table cellSpacing=0 cellPadding=0 border=0>")
        sb.AppendLine("<tr>")
        sb.AppendLine("<td>")
        sb.AppendLine("<div id=pnlPrincipal style=background-color:{0};border-color:LightSteelBlue;border-width:1px;border-style:solid;height:55px;width:800px;>")
        sb.AppendLine("<table cellSpacing=0 cellPadding=0 border=0 align=center width=800px>")
        sb.AppendLine("<tr>")
        sb.AppendLine("<td rowspan=4 width=100>&nbsp;&nbsp;&nbsp;")
        sb.AppendLine("<img id=imgMarco src=images\{1} border=0 style=height:45px;width:45px />")
        sb.AppendLine("</td>")
        sb.AppendLine("<td rowspan=4 width=300 class=modulo>")
        sb.AppendLine("<span id=lblNomeMarco>{7}</span>")
        sb.AppendLine("</td>")
        sb.AppendLine("<td width=300 class=modulo>Previsão:")
        sb.AppendLine("<span id=lblPrevisao class=formulario_texto>{2}</span>")
        sb.AppendLine("</td>")
        sb.AppendLine("<td rowspan=4 align=center width=100>")
        sb.AppendLine("<img id=imgStatusMarco src=images\{3} border=0 style=height:35px;width:35px; />")
        sb.AppendLine("</td>")
        sb.AppendLine("</tr>")
        sb.AppendLine("<tr><td width=400 class=modulo>Data:")
        sb.AppendLine("<span id=lblDataOcorrencia class=formulario_texto>{4}</span>")
        sb.AppendLine("</td></tr>")
        sb.AppendLine("<tr><td width=400 class=modulo>Usuário Solicitante:")
        sb.AppendLine("<span id=lblUsuSolic class=formulario_texto>{5}</span>")
        sb.AppendLine("</td></tr>")
        sb.AppendLine("<tr><td width=400 class=modulo>Usuário Responsável:")
        sb.AppendLine("<span id=lblUsuResp class=formulario_texto>{6}</span>")
        sb.AppendLine("</td></tr>")
        sb.AppendLine("</table>")
        sb.AppendLine("</div>")
        sb.AppendLine("</td>")
        sb.AppendLine("</tr>")
        sb.AppendLine("<tr>")
        sb.AppendLine("<td height=7 colSpan=2></td>")
        sb.AppendLine("</tr>")
        sb.AppendLine("</table> ")

        geraMarco = sb.ToString()

    End Function

    Private Function geraImagemCabecalho(ByVal p_criaSeta As Boolean) As String

        Dim sb As StringBuilder = New StringBuilder()

        ' {0} --> imgMarco
        ' {1} --> imgSeta
        sb.AppendLine("<img id=imgMarcoCabecalho src=images\{0} border=0 height=40px width=40px />&nbsp;&nbsp;&nbsp;&nbsp;")
        If (p_criaSeta) Then
            sb.AppendLine("<img id=imgSeta src=images\{1} border=0 height=25px width=55px />&nbsp;&nbsp;&nbsp;&nbsp; ")
        End If

        geraImagemCabecalho = sb.ToString()
    End Function
    




End Class