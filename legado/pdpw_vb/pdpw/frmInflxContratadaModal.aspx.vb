
Imports System.Text

Partial Class frmInflxContratadaModal
    Inherits System.Web.UI.Page



    Public tipo As String
    Public CodUsina As String
    Dim codusinaOrigi, nomeusinaOrigi, datainicioOrigi, datafimOrigi, valorOrigi, contratohabilitadoOrigi, modelocontratoOrigi As String


    Protected Sub habilitado_CheckedChanged(sender As Object, e As EventArgs) Handles habilitado.CheckedChanged

    End Sub



#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()



    End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles frmInflxContratadaModal.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub



#End Region



    Protected Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim strUsuar As String = UsuarID

        Dim messtre = MasterPageFile
        Dim selecionado, codusina, nomeusina, datainicio, datafim, valor, contratohabilitado, modelocontrato As String
        selecionado = Request.QueryString("selecionado")
        Dim valores
        LabelTipo.Text = Request.QueryString("tipo")
        If (Not selecionado = "") Then
            valores = selecionado.Split("@")
            codusina = valores(0)
            nomeusina = valores(1)
            datainicio = valores(2)
            datafim = valores(3)
            valor = valores(4)
            contratohabilitado = valores(5)
            modelocontrato = valores(6)
            codusinaOrigi = valores(0)
            nomeusinaOrigi = valores(1)
            datainicioOrigi = valores(2)
            datafimOrigi = valores(3)
            valorOrigi = valores(4)
            LabelUsina.Text = nomeusinaOrigi
            contratohabilitadoOrigi = valores(5)
            modelocontratoOrigi = valores(6)
        End If

        If Not Page.IsPostBack Then


            divCal.Visible = False
            divCal2.Visible = False
            Dim strSql As String
            Dim objItem As ListItem
            Dim Conn As System.Data.SqlClient.SqlConnection = New System.Data.SqlClient.SqlConnection
            Conn.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
            Dim daEquipe As System.Data.SqlClient.SqlDataAdapter
            Dim dsEquipe As DataSet
            Conn.Open()

            Try
                If Not LabelTipo.Text = "Alterar" And selecionado = "" Or LabelTipo.Text = "Alterar" And Not selecionado = "" Then
                    objItem = New ListItem
                    objItem.Text = ""
                    objItem.Value = "0"


                    strSql = "select unique u.codusina, u.nomusina " &
                             "from usina u, usuarempre usr " &
                             "where usr.usuar_id =  '" & strUsuar & "' and usr.codempre = u.codempre " &
                             "ORDER BY u.nomusina"

                    daEquipe = New System.Data.SqlClient.SqlDataAdapter(strSql, Conn)
                    dsEquipe = New DataSet
                    daEquipe.Fill(dsEquipe, "Usina")
                    cboUsina.DataTextField = "nomusina"
                    cboUsina.DataValueField = "codusina"
                    cboUsina.DataSource = dsEquipe.Tables("Usina").DefaultView
                    cboUsina.DataBind()
                    cboUsina.Items.Add(objItem)
                    cboUsina.Items.FindByValue("0")

                    'cboUsina.SelectedIndex = cboUsina.Items.Count - 1

                Else
                    Session("strMensagem") = "Não foi selecionado nenhum registro"
                    Response.Redirect("frmMensagem.aspx")
                End If


            Catch ex As Exception

            End Try
            If LabelTipo.Text = "Alterar" Then
                If Not selecionado = "" Then
                    valores = selecionado.Split("@")
                    codusina = valores(0)
                    nomeusina = valores(1)
                    datainicio = valores(2)
                    datafim = valores(3)
                    valor = valores(4)
                    contratohabilitado = valores(5)
                    modelocontrato = valores(6)
                    codusinaOrigi = valores(0)
                    nomeusinaOrigi = valores(1)
                    datainicioOrigi = valores(2)
                    datafimOrigi = valores(3)
                    valorOrigi = valores(4)
                    LabelUsina.Text = nomeusinaOrigi
                    contratohabilitadoOrigi = valores(5)
                    modelocontratoOrigi = valores(6)

                    objItem = New ListItem
                    objItem.Text = ""
                    objItem.Value = "0"



                    Dim auxtam = nomeusina.Length
                    Dim i As Integer
                    For i = 1 To (30 - auxtam)
                        nomeusina += " "
                    Next
                    'Dim dataini As System.Web.UI.HtmlControls.HtmlInputText = CType(datainicio, Object)
                    'Dim datafin As System.Web.UI.HtmlControls.HtmlInputText = CType(datafim, Object)
                    Dim a = cboUsina.Items.FindByText(nomeusina)
                    cboUsina.SelectedIndex = cboUsina.Items.IndexOf(a)
                    data_inicial.Text = datainicio
                    data_final.Text = datafim
                    valorcontratado.Text = valor
                    contrato.SelectedIndex = contratohabilitado


                    If (modelocontrato = "1") Then
                        habilitado.Checked = True
                    Else
                        habilitado.Checked = False
                    End If



                End If


            End If

        End If




    End Sub
    Public Function FormataDatas(dataPDP As String) As String

        dataPDP = dataPDP.Replace("-", "").Replace("/", "").Trim()
        Dim tam As Integer = dataPDP.Length()
        Dim ano As String = dataPDP.Substring(4, 4)
        Dim mes As String = dataPDP.Substring(2, 2)
        Dim dia As String = dataPDP.Substring(0, 2)

        Dim hora As String
        If tam > 8 Then
            hora = dataPDP.Substring(8, 9)
        Else
            hora = "00:00:00"
        End If

        Return ano + "-" + mes + "-" + dia + " " + hora
    End Function
    Protected Sub btnSalvar_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        Dim objTrans As System.Data.SqlClient.SqlTransaction

        Dim Conn As System.Data.SqlClient.SqlConnection = New System.Data.SqlClient.SqlConnection
        Conn.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
        Dim Cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand


        Dim objRow As DataRow
        Dim objRowAux As DataRow
        Dim objRows As DataRow()
        Dim intI As Integer
        Dim selecionado As String
        Dim dataatualizacao As String
        dataatualizacao = Now
        Dim dataaux As String
        dataatualizacao = FormataDatas(dataatualizacao)
        Dim aux As Boolean
        Dim controleHabil As Boolean = habilitado.Checked
        Dim controleContra As String = contrato.SelectedIndex
        Dim datafim As String = FormataDatas(data_final.Text.ToString())
        Dim datainicio As String = FormataDatas(data_inicial.Text.ToString())
        Dim usina As String = cboUsina.SelectedValue
        Dim cstext2 As StringBuilder = New StringBuilder()
        ' Inicializa uma transação.
        Cmd.Connection = Conn
        Conn.Open()
        objTrans = Conn.BeginTransaction()
        Cmd.Transaction = objTrans
        Try
            If LabelTipo.Text = "Incluir" Then

                If habilitado.Checked = True Then
                    selecionado = 1
                Else
                    selecionado = 0

                End If
                Cmd.CommandText = "insert into tb_inflexbilidadecontratada " &
                                      " (codusina, din_iniciovigencia, din_fimvigencia, flg_registroativo, din_atualizacaoregistro, val_inflexcontratada, dsc_descricaocontrato, flg_modalidadecontrato) " &
                                      "VALUES(" &
                                      "'" & cboUsina.SelectedValue & "', " &
                                      "'" & datainicio & "', " &
                                      "'" & datafim & "', " &
                                      "'" & selecionado & "', " &
                                      "'" & dataatualizacao & "', " &
                                      "" & valorcontratado.Text & ", " &
                                      " NULL," &
                                      " '" & controleContra & "' )"
                Cmd.ExecuteNonQuery()


                objTrans.Commit()


                'atualiza o grid na tela principal
                Response.Write("<script lang='javascript'> ")
                Response.Write("var url = window.opener.location.href; window.opener.location.href = url; ")
                Response.Write("</script>")


            Else
                If habilitado.Checked = True Then
                    selecionado = 1
                Else
                    selecionado = 0

                End If

                Cmd.CommandText = "UPDATE tb_inflexbilidadecontratada " &
                                   " SET din_iniciovigencia = '" & datainicio & "', " &
                                   " codusina =  '" & cboUsina.SelectedValue & "', " &
                                   " din_fimvigencia = '" & datafim & "', " &
                                   " flg_registroativo = '" & selecionado & "', " &
                                   " din_atualizacaoregistro = '" & dataatualizacao & "', " &
                                   " val_inflexcontratada = " & valorcontratado.Text & ", " &
                                   " flg_modalidadecontrato = '" & controleContra & "' " &
                                   "where codusina = '" & codusinaOrigi & "' and din_iniciovigencia = '" & FormataDatas(datainicioOrigi) & "' and din_fimvigencia = '" & FormataDatas(datafimOrigi) & "' and val_inflexcontratada= " & valorOrigi & ";"
                '' Executa a operação.
                Cmd.ExecuteNonQuery()

                objTrans.Commit()
                'atualiza o grid na tela principal
                Response.Write("<script lang='javascript'> ")
                Response.Write("var url = window.opener.location.href; window.opener.location.href = url; ")
                Response.Write("</script>")

            End If
        Catch ex As Exception
            'Cmd.Connection.Close()
            objTrans.Rollback()
            If Conn.State = ConnectionState.Open Then
                Cmd.Connection.Close()
                Conn.Close()
            End If

        Finally
            If Conn.State = ConnectionState.Open Then
                Cmd.Connection.Close()
                Conn.Close()


                aux = True


            End If
        End Try


        cstext2.Clear()
            cstext2.Append("<script lang='javascript'> ")
            cstext2.AppendLine("window.close(); ")
            cstext2.AppendLine("</script>")


            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "ButtonClickScript", cstext2.ToString(), False)


    End Sub

    Protected Sub btnCancelar_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles Button1.Click




    End Sub


    Private Sub btnCalendario_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCalendario.Click
        If divCal.Visible = False Then
            If LabelTipo.Text = "Alterar " Then
                calData.SelectedDate = CDate(datainicioOrigi)
                divCal.Visible = True
                Exit Sub
            Else
                calData.SelectedDate = CDate("01/01/1900")
                divCal.Visible = True
                Exit Sub
            End If
        Else
            divCal.Visible = False
        End If
    End Sub

    Private Sub calData_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calData.SelectionChanged
        data_inicial.Text = Format(calData.SelectedDate.Date, "dd/MM/yyyy")
        divCal.Visible = False
    End Sub

    Private Sub btnCalendario2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCalendario2.Click
        If divCal2.Visible = False Then
            calData2.SelectedDate = CDate("01/01/1900")
            divCal2.Visible = True
        Else
            divCal2.Visible = False
        End If
    End Sub

    Private Sub calData2_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calData2.SelectionChanged
        data_final.Text = Format(calData2.SelectedDate.Date, "dd/MM/yyyy")
        divCal2.Visible = False
    End Sub

End Class
