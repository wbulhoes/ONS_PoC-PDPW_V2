Imports System.Data
Imports System.Data.OleDb
Imports System.Collections.Generic
Imports System.Linq
Imports System.Net.Mail
Imports System.Text

Partial Class frmRampasUsinasTerm
    Inherits System.Web.UI.Page

    Public ConnPDP As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
    Public ConnPDP1 As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
    Public CmdPDP As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
    Public CmdPDP1 As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
    Public ConnBdTecn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
    Public CmdBdTecn As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
    Dim objTrans As OnsClasses.OnsData.OnsTransaction


    Public CmdPdpSql As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand
    Public ConnPDPSql As System.Data.SqlClient.SqlConnection = New System.Data.SqlClient.SqlConnection

    Dim objTransSql As System.Data.SqlClient.SqlTransaction

    Dim rsDataHeaderSql As System.Data.SqlClient.SqlDataReader


    Dim HR As String
    Dim TOT, IntI, I As Integer
    Dim rsDataHeader As OnsClasses.OnsData.OnsDataReader
    Dim ObjListItem As WebControls.ListItem
    Dim CON1, CON2 As String
    Dim LST1 As New List(Of UGEDessem)
    Dim UGED, USI, CG, CUSI As String
    Dim TEM As String
    Dim HH, MM, TEMPO As String
    Dim H1, H2, TRampImp As Integer

    Dim item As DataGridItem
    Dim codusina, cod_conjuntouge, id_tpformarampa, id_tprampausi, prd_tempo, prd_tempo1, val_potencia, prd_ton, forma_rampa, tipo_rampa As String

    Dim objTable As New System.Data.DataTable
    Dim objRow As DataRow

    Dim daGrid2 As OnsClasses.OnsData.OnsDataAdapter
    Dim dsGrid2 As DataSet

    Dim lstGDEssem As List(Of GDessem)
    Dim lstEstrutural As List(Of GDessem)
    Dim lstRampConj As List(Of GDessem)
    Dim lstExclusao As List(Of GDessem)
    Dim lstInclusao As List(Of GDessem)

    Dim objCheckBox As CheckBox
    Dim Achou As Boolean = False

    Dim strUsuar

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

    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'objMenu = CType(Session("onsmenu"), OnsWebControls.OnsMenu)
        strUsuar = UsuarID
        ' Desabilitando campo txtPotencia
        txtPotencia.Enabled = True
        Try
            If Not Page.IsPostBack Then

                Session("lstRampConj") = lstRampConj
                Session("lstExclusao") = lstExclusao
                Session("lstInclusao") = lstInclusao

                CmdPdpSql.Connection = ConnPDPSql

                Dim clausulaSelecao As String = String.Empty
                If Not AgentesRepresentados.Equals("") Then
                    clausulaSelecao = " and empre_bdt_id IN (" & AgentesRepresentados & ") "
                End If
                Try
                    CON1 = "SELECT TRIM(codempre) as codempre, sigempre " &
                             "FROM empre  " &
                             "WHERE flg_estudo = 'N' " & clausulaSelecao &
                             "ORDER BY 2"
                    '***Preenche COMBO USINA

                    cboUsina.Items.Clear()
                    ConnPDPSql.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
                    ConnPDPSql.Open()

                    'CON1 = "Select Trim(e.codempre) As codempre, e.sigempre, u.usuar_id " &
                    ' "FROM empre e, usuarempre u " &
                    ' "WHERE TRIM(u.codempre) = TRIM(e.codarea) " &
                    ' "And e.flg_estudo = 'N' and u.usuar_id = '" & strUsuar & "' " &
                    ' "UNION " &
                    ' "SELECT TRIM(e.codempre) AS codempre, e.sigempre, u.usuar_id " &
                    ' "FROM empre e, usuarempre u " &
                    ' "WHERE  TRIM(u.codempre) = TRIM(e.codempre) " &
                    ' "AND e.flg_estudo = 'N' and u.usuar_id = '" & strUsuar & "' " &
                    ' "ORDER BY 2"

                    CmdPdpSql.CommandText = CON1
                    rsDataHeaderSql = CmdPdpSql.ExecuteReader
                    Dim CON2 As String = ""
                    Do While rsDataHeaderSql.Read
                        CON2 = CON2 & Trim(rsDataHeaderSql("codempre")) & "', '"
                    Loop
                    CON2 = Mid(CON2, 1, (Len(CON2) - 3))

                    rsDataHeaderSql.Close()
                    rsDataHeaderSql = Nothing

                    'CON1 = "Select Distinct Trim(us.codusina) As CodUsina, Trim(us.codusina) || ' - ' || Trim(us.nomusina) as DescricaoUsina, us.nomusina "
                    CON1 = "SELECT DISTINCT LTRIM(RTRIM(us.codusina)) AS CodUsina, LTRIM(RTRIM(us.codusina)) + ' - ' + LTRIM(RTRIM(us.nomusina)) as DescricaoUsina, us.nomusina  "
                    CON1 = CON1 & "From usina us Where us.tpusina_id = 'UTE' and us.flg_dusi ='S' and us.CodEmpre In ('" & CON2 & ") ORDER BY 2"
                    CmdPdpSql.CommandText = CON1

                    IntI = 1
                    rsDataHeaderSql = CmdPdpSql.ExecuteReader
                    ObjListItem = New WebControls.ListItem
                    ObjListItem.Text = ""
                    ObjListItem.Value = "0"
                    cboUsina.Items.Add(ObjListItem)
                    Do While rsDataHeaderSql.Read
                        ObjListItem = New System.Web.UI.WebControls.ListItem
                        ObjListItem.Text = Trim(rsDataHeaderSql("nomusina"))
                        ObjListItem.Value = rsDataHeaderSql("codusina")
                        cboUsina.Items.Add(ObjListItem.Text)
                        IntI = IntI + 1
                    Loop
                    cboUsina.DataBind()
                    rsDataHeaderSql.Close()
                    rsDataHeaderSql = Nothing
                    CmdPdpSql.Connection.Close()
                    ConnPDPSql.Close()

                Catch ex As Exception
                    If ConnPDPSql.State = ConnectionState.Open Then
                        ConnPDPSql.Close()
                        CmdPdpSql.Connection.Close()
                    End If
                End Try

                '***Preenche COMBO TEMPO
                cboTempo.Items.Clear()
                HR = ""
                For IntI = 0 To 80
                    If IntI < 10 Then
                        If IntI > 0 Then
                            HR = "0" & IntI & ":00"
                            cboTempo.Items.Add(HR)
                        End If
                        HR = "0" & IntI & ":30"
                        cboTempo.Items.Add(HR)
                    Else
                        HR = IntI & ":00"
                        cboTempo.Items.Add(HR)
                        HR = IntI & ":30"
                        cboTempo.Items.Add(HR)
                    End If
                    If IntI = 80 Then cboTempo.Items.Remove(HR)
                Next
                cboTempo.DataBind()

                '***Preenche COMBO FORMA
                CmdBdTecn.Connection = ConnBdTecn
                Try

                    ConnBdTecn.Open("bdtecn")
                    CmdBdTecn.CommandText = "select * from tb_tpformarampa"

                    IntI = 1
                    rsDataHeader = CmdBdTecn.ExecuteReader
                    ObjListItem = New WebControls.ListItem
                    ObjListItem.Text = ""
                    ObjListItem.Value = "0"
                    cboFormaRampa.Items.Add(ObjListItem)
                    Do While rsDataHeader.Read
                        ObjListItem = New System.Web.UI.WebControls.ListItem
                        ObjListItem.Text = Trim(rsDataHeader("nom_tpformarampa"))
                        ObjListItem.Value = rsDataHeader("id_tpformarampa")
                        cboFormaRampa.Items.Add(ObjListItem.Text)
                        IntI = IntI + 1
                    Loop
                    cboFormaRampa.DataBind()
                    rsDataHeader.Close()
                    rsDataHeader = Nothing

                Catch
                    If ConnBdTecn.State = ConnectionState.Open Then
                        ConnBdTecn.Close()
                        CmdBdTecn.Connection.Close()
                    End If
                End Try

                '***Preenche COMBO TIPO RAMPA
                Try
                    CmdBdTecn.CommandText = "select * from tb_tprampausi where nom_tprampausi like '%térmica%' or nom_tprampausi like '%descida%'"

                    IntI = 1
                    rsDataHeader = CmdBdTecn.ExecuteReader
                    ObjListItem = New WebControls.ListItem
                    ObjListItem.Text = ""
                    ObjListItem.Value = "0"
                    cboTipoRampa.Items.Add(ObjListItem)
                    Do While rsDataHeader.Read
                        ObjListItem = New System.Web.UI.WebControls.ListItem
                        ObjListItem.Text = Trim(rsDataHeader("nom_tprampausi"))
                        ObjListItem.Value = rsDataHeader("id_tprampausi")
                        cboTipoRampa.Items.Add(ObjListItem.Text)

                        IntI = IntI + 1
                    Loop
                    cboTipoRampa.DataBind()
                    rsDataHeader.Close()
                    rsDataHeader = Nothing
                    CmdBdTecn.Connection.Close()
                    ConnBdTecn.Close()

                    For Each objRow In objTable.Rows
                        objRow.Delete()
                    Next

                    objTable.Columns.Add("UGE")
                    objTable.Columns.Add("FormaRampa")
                    objTable.Columns.Add("TipoRampa")
                    objTable.Columns.Add("Tempo")
                    objTable.Columns.Add("Pot")
                    objTable.Columns.Add("Ton")
                    objTable.Columns.Add("PotMinUGE")
                    objTable.Columns.Add("IDFormaRampa")
                    objTable.Columns.Add("IDTipoRampa")
                    objTable.Columns.Add("Lgn_Usuario")
                    objTable.Columns.Add("id_rampaugeconjuntural")

                    For I = 1 To 15
                        objRow = objTable.NewRow
                        objRow("UGE") = ""
                        objRow("FormaRampa") = ""
                        objRow("TipoRampa") = ""
                        objRow("Tempo") = ""
                        objRow("Pot") = ""
                        objRow("Ton") = ""
                        objRow("PotMinUGE") = ""
                        objRow("IDFormaRampa") = ""
                        objRow("IDTipoRampa") = ""
                        objRow("Lgn_Usuario") = ""
                        objRow("id_rampaugeconjuntural") = ""
                        objTable.Rows.Add(objRow)
                        If I = 6 Then
                            GridDessem.DataSource = objTable
                            GridDessem.DataBind()
                        End If
                    Next I


                    GridEstrutural.DataSource = objTable
                    GridEstrutural.DataBind()

                Catch

                    If ConnBdTecn.State = ConnectionState.Open Then
                        ConnBdTecn.Close()
                        CmdBdTecn.Connection.Close()
                    End If
                End Try

                Valida_Potencia()
            End If
        Catch ex As Exception
            Response.Write("<script lang='javascript'>")
            Response.Write("  window.alert('Ocorreu um erro ao acessar a página!')")
            Response.Write("</script>")
            Exit Sub
        End Try

    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        If SessaoAtiva(Page.Session) Then
            MyBase.Render(writer)
            'objMenu.RenderControl(writer)
        End If
    End Sub

    Protected Sub cboTempo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboTempo.SelectedIndexChanged
        Valida_Potencia()
    End Sub

    Protected Sub cboUsina_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboUsina.SelectedIndexChanged
        Dim aux As String = cboUsina.SelectedValue
        LimparDados()
        cboUsina.SelectedValue = aux

        CON1 = "Select Distinct us.usi_bdt_id, us.codusina "
        CON1 = CON1 & "From usina us Where us.tpusina_id = 'UTE' and us.NomUsina = '" & cboUsina.SelectedValue & "'"
        CmdPDP.CommandText = CON1

        CmdPdpSql.Connection = ConnPDPSql
        ConnPDPSql.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
        ConnPDPSql.Open()

        CmdPdpSql.CommandText = CON1
        rsDataHeaderSql = CmdPdpSql.ExecuteReader
        Dim usiBDTID As String = ""
        Do While rsDataHeaderSql.Read
            usiBDTID = Trim(rsDataHeaderSql("usi_bdt_id"))
            txtCodUsina.Text = Trim(rsDataHeaderSql("codusina"))
        Loop
        rsDataHeaderSql.Close()
        ConnPDPSql.Close()
        CmdPdpSql.Connection.Close()

        ObterUGEDessem(usiBDTID)
        MostrarGridEstruturais(usiBDTID, "", "")
    End Sub

    Private Sub ObterUGEDessem(UsiBDT As String)
        Try

            CON1 = ""
            CON1 = "select CUGE.id_conjuntouge, CUGE.cod_conjuntouge from tb_conjuntouge CUGE "
            CON1 = CON1 & " inner join tb_conjuntougeequivrampa CRE ON CUGE.id_conjuntouge = CRE.id_conjuntouge "
            CON1 = CON1 & " inner join usi ON CRE.usi_id = usi.usi_id where (usi.usi_id = '" & Trim(UsiBDT) & "' or usi.ido_usi = '" & Trim(UsiBDT) & "') "
            CON1 = CON1 & " and CUGE.id_tpconjuntouge = 3 and (CUGE.din_fimvig is null or CUGE.din_fimvig > current)"

            Try
                cboUGDessem.Items.Clear()

                CmdBdTecn.Connection = ConnBdTecn
                ConnBdTecn.Open("bdtecn")

                CmdBdTecn.CommandText = CON1
                rsDataHeader = CmdBdTecn.ExecuteReader

                ObjListItem = New WebControls.ListItem
                ObjListItem.Text = ""
                ObjListItem.Value = "0"
                cboUGDessem.Items.Add(ObjListItem)
                CON2 = ""
                I = 0
                Do While rsDataHeader.Read
                    ObjListItem = New System.Web.UI.WebControls.ListItem
                    ObjListItem.Text = Trim(rsDataHeader("cod_conjuntouge"))
                    ObjListItem.Value = rsDataHeader("id_conjuntouge")
                    cboUGDessem.Items.Add(ObjListItem.Text)
                    CON2 = CON2 & ObjListItem.Text & "', '"
                    IntI = IntI + 1
                Loop
                CON2 = Mid(CON2, 1, (Len(CON2) - 3))
                rsDataHeader.Close()

            Catch
                If ConnBdTecn.State = ConnectionState.Open Then
                    ConnBdTecn.Close()
                    CmdBdTecn.Connection.Close()
                End If
            End Try
        Catch
            LimparDados()
        End Try

        LST1.Clear()



    End Sub

    Public Class GDessem
        Public Property UGE As String
        Public Property FormaRampa As String
        Public Property TipoRampa As String
        Public Property Tempo As String
        Public Property Pot As String
        Public Property Ton As String
        Public Property PotMinUGE As String
        Public Property IDFormaRampa As String
        Public Property IDTipoRampa As String
        Public Property Lgn_Usuario As String
        Public Property id_rampaugeconjuntural As String
        Public Sub New(ByVal UGE As String, ByVal FormaRampa As String, ByVal TipoRampa As String, ByVal Tempo As String, ByVal Pot As String, ByVal Ton As String, ByVal PotMinUGE As String, ByVal IDFormaRampa As String, ByVal IDTipoRampa As String, ByVal Lgn_Usuario As String, ByVal id_rampaugeconjuntural As String)
            Me.UGE = UGE
            Me.FormaRampa = FormaRampa
            Me.TipoRampa = TipoRampa
            Me.Tempo = Tempo
            Me.Pot = Pot
            Me.Ton = Ton
            Me.PotMinUGE = PotMinUGE
            Me.IDFormaRampa = IDFormaRampa
            Me.IDTipoRampa = IDTipoRampa
            Me.Lgn_Usuario = Lgn_Usuario
            Me.id_rampaugeconjuntural = id_rampaugeconjuntural
        End Sub

    End Class


    Public Class UGEDessem

        Public Property CGerad As String
        Public Property UGEBDT As String
        Public Property NomUsin As String
        Public Property CUsin As String

        Public Sub New(ByVal _CGerad As String, ByVal _UGEBDT As String, ByVal _NomUsin As String, ByVal _CUsin As String)
            Me.CGerad = _CGerad
            Me.UGEBDT = _UGEBDT
            Me.NomUsin = _NomUsin
            Me.CUsin = _CUsin
        End Sub

    End Class




    Protected Sub cboFormaRampa_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboFormaRampa.SelectedIndexChanged
        Try
            CON1 = ""
            CON1 = "select id_tpformarampa from tb_tpformarampa where nom_tpformarampa = '" & cboFormaRampa.SelectedValue & "'"

            CmdBdTecn.Connection = ConnBdTecn
            ConnBdTecn.Open("bdtecn")
            CmdBdTecn.CommandText = CON1
            rsDataHeader = CmdBdTecn.ExecuteReader

            Do While rsDataHeader.Read
                txtIDFormaRampa.Text = Trim(rsDataHeader("id_tpformarampa"))
            Loop
            rsDataHeader.Close()
            ConnBdTecn.Close()
            CmdBdTecn.Connection.Close()

            Valida_Potencia()
        Catch
            Response.Write("<script lang='javascript'>")
            Response.Write("  window.alert('Ocorreu um erro ao carregar o campo. Tente novamente!')")
            Response.Write("</script>")
            Exit Sub
        End Try

    End Sub


    Protected Sub cboUGDessem_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboUGDessem.SelectedIndexChanged
        Dim idforma, idtipo As Integer
        lstExclusao = Session("lstExclusao")
        If Not IsNothing(lstExclusao) Then lstExclusao.Clear()

        MostrarGridEstruturais("", cboUGDessem.SelectedItem.Value, "")

        Try
            Dim UGAtual As String = cboUGDessem.SelectedValue
            For Each item In GridDessem.Items
                Dim UGAnt As String = item.Cells(1).Text
                If (UGAtual <> UGAnt) And (UGAnt <> "&nbsp;") Then
                    Session("GrdDessem") = New List(Of GDessem)
                End If
            Next

            If IsNothing(Session("GrdDessem")) Then
                Session("GrdDessem") = New List(Of GDessem)
            End If
            lstGDEssem = Session("GrdDessem")
            Session("lstRampConj") = New List(Of GDessem)
            lstRampConj = Session("lstRampConj")

            CON1 = ""
            CON1 = "select codusina, cod_conjuntouge, id_tpformarampa, id_tprampausi, prd_tempo, val_potencia, prd_ton, Lgn_Usuario, id_rampaugeconjuntural from tb_rampaugeconjuntural where codusina = '" + txtCodUsina.Text + "' and cod_conjuntouge = '" + cboUGDessem.SelectedValue + "' and din_fimvig is null order by id_tpformarampa, id_tprampausi, cast(prd_tempo as integer)"

            'Dim idtipoanti As Integer = ""
            CmdPdpSql.Connection = ConnPDPSql
            ConnPDPSql.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
            ConnPDPSql.Open()
            CmdPdpSql.CommandText = CON1
            rsDataHeaderSql = CmdPdpSql.ExecuteReader
            Do While rsDataHeaderSql.Read
                TEM = CDbl(Trim(rsDataHeaderSql("prd_tempo"))) / 60

                If InStr(TEM, ",") <> 0 Or InStr(TEM, ".") <> 0 Then
                    If InStr(TEM, ",") <> 0 Then
                        H1 = CInt(Mid(TEM, 1, InStr(TEM, ",")))
                    ElseIf InStr(TEM, ".") <> 0 Then
                        H1 = CInt(Mid(TEM, 1, InStr(TEM, ".")))
                    End If
                Else
                    H1 = CInt(TEM)
                End If
                If H1 < 10 Then
                    HH = "0" & H1
                Else
                    HH = H1
                End If

                If Right(TEM, 2) = ",5" Or Right(TEM, 2) = ".5" Then
                    MM = ":30"
                Else
                    MM = ":00"
                End If
                TEMPO = HH + MM

                idforma = Trim(rsDataHeaderSql("id_tpformarampa"))
                idtipo = Trim(rsDataHeaderSql("id_tprampausi"))
                If idforma = 1 Then
                    forma_rampa = "Linear"
                Else
                    forma_rampa = "Tabular"
                End If

                If idtipo = 6 Then
                    tipo_rampa = "Rampa de subida a frio (térmica)"
                ElseIf idtipo = 7 Then
                    tipo_rampa = "Rampa de subida a quente (térmica)"
                ElseIf idtipo = 8 Then
                    tipo_rampa = "Rampa de descida"
                End If

                lstGDEssem.Add(New GDessem(cboUGDessem.SelectedValue, forma_rampa, tipo_rampa,
                                       TEMPO, Trim(rsDataHeaderSql("val_potencia")), Trim(rsDataHeaderSql("prd_ton")), txtPotenciaMin.Text, idforma, idtipo,
                                       Trim(rsDataHeaderSql("Lgn_Usuario")), Trim(rsDataHeaderSql("id_rampaugeconjuntural"))))

                lstRampConj.Add(New GDessem(cboUGDessem.SelectedValue, forma_rampa, tipo_rampa,
                                       TEMPO, Trim(rsDataHeaderSql("val_potencia")), Trim(rsDataHeaderSql("prd_ton")), txtPotenciaMin.Text, idforma, idtipo,
                                       Trim(rsDataHeaderSql("Lgn_Usuario")), Trim(rsDataHeaderSql("id_rampaugeconjuntural"))))

                'If idtipoanti <> idtipo Then
                '    Dim sel As New ButtonColumn
                '    sel.ButtonType = ButtonColumnType.LinkButton
                '    sel.CommandName = "Select"
                '    sel.Text = "Selecionar"
                '    Me.GridEstrutural.Columns.Add(sel)




                'End If
                'idtipoanti = idtipo
            Loop
            Session("lstRampConj") = lstRampConj

            Session("GrdDessem") = lstGDEssem

            GridDessem.DataSource = lstGDEssem
            GridDessem.DataBind()

        Catch
            Response.Write("<script lang='javascript'>")
            Response.Write("  window.alert('Ocorreu um erro ao carregar os dados.')")
            Response.Write("</script>")
        End Try

    End Sub


    Protected Sub btnExcluir_Click(sender As Object, e As ImageClickEventArgs) Handles btnExcluir.Click
        If GridDessem.Items.Count = 0 Then
            Response.Write("<script lang='javascript'>")
            Response.Write("  window.alert('Nenhuma informação para excluir.')")
            Response.Write("</script>")
            Exit Sub
        End If

        Try
            lstGDEssem = Session("GrdDessem")
            lstExclusao = Session("lstExclusao")
            Dim Lstexc As New List(Of Integer)

            For Each item In GridDessem.Items
                objCheckBox = item.FindControl("chkMarca")
                If (objCheckBox.Checked) Then
                    Dim indice As Integer = item.ItemIndex
                    Lstexc.Add(indice)

                    If IsNothing(lstExclusao) Then
                        Session("lstExclusao") = New List(Of GDessem)
                        lstExclusao = Session("lstExclusao")

                    ElseIf lstExclusao.Count = 0 Then
                        Session("lstExclusao") = New List(Of GDessem)
                        lstExclusao = Session("lstExclusao")
                    Else
                        If cboUGDessem.SelectedValue <> lstExclusao.Item(0).UGE Then
                            lstExclusao = New List(Of GDessem)
                        End If
                    End If
                    If item.Cells(11).Text <> "&nbsp;" Then
                        lstExclusao.Add(New GDessem(cboUGDessem.SelectedValue, item.Cells(2).Text, item.Cells(3).Text,
                                        item.Cells(4).Text, item.Cells(5).Text, item.Cells(6).Text, item.Cells(7).Text, item.Cells(8).Text, item.Cells(9).Text,
                                        item.Cells(10).Text, item.Cells(11).Text))
                    End If


                End If
            Next

            For I = (Lstexc.Count - 1) To 0 Step -1
                'GridDessem.Items(0).Cells.RemoveAt(I)
                lstGDEssem.RemoveAt(Lstexc.Item(I))
            Next

            Session("GrdDessem") = lstGDEssem
            lstGDEssem = Session("GrdDessem")

            GridDessem.DataSource = lstGDEssem
            GridDessem.DataBind()



        Catch
            Response.Write("<script lang='javascript'>")
            Response.Write("  window.alert('Erro ao excluir a(s) linha(s) selecionada(s).')")
            Response.Write("</script>")
        End Try
    End Sub


    Protected Sub btnIncluir_Click(sender As Object, e As ImageClickEventArgs) Handles btnIncluir.Click
        'Session("GrdDessem") = lstGDEssem
        'lstGDEssem = Session("GrdDessem")

        'GridDessem.DataSource = lstGDEssem
        'GridDessem.DataBind()

        If cboUsina.SelectedValue = "" Or cboUGDessem.SelectedValue = "" Or cboTipoRampa.SelectedValue = "" Or cboFormaRampa.SelectedValue = "" Or cboTempo.SelectedValue = "" Or txtPotencia.Text = "" Then
            Response.Write("<script lang='javascript'>")
            Response.Write("  window.alert('Um ou mais campos não foram preenchidos. Verifique e tente novamente!')")
            Response.Write("</script>")
            Exit Sub
        End If

        If CInt(txtPotencia.Text) > CInt(txtPotenciaMin.Text) Then
            Response.Write("<script lang='javascript'>")
            Response.Write("  window.alert('O valor da potência é maior do que a potência mínima da UGE!')")
            Response.Write("</script>")
            Exit Sub
        End If

        If CInt(txtPotencia.Text < 0) And cboTipoRampa.Text Like "%descida%" Then
            Response.Write("<script lang='javascript'>")
            Response.Write("  window.alert('A Potência digitada é menor que 0!')")
            Response.Write("</script>")
            Exit Sub
        End If

        'Não deixar incluir mais de 1 TIPO para FORMAS diferentes (SUBIDA)
        Dim FormaAtual, FormaAntiga, TipoAtual, TipoAntigo As String
        Dim FlagF As Boolean = False
        lstGDEssem = Session("GrdDessem")

        FormaAtual = cboFormaRampa.SelectedValue
        TipoAtual = cboTipoRampa.SelectedValue
        For I = 0 To lstGDEssem.Count - 1
            FormaAntiga = lstGDEssem.Item(I).FormaRampa
            TipoAntigo = lstGDEssem.Item(I).TipoRampa
            If cboTipoRampa.Text = "Rampa de subida a frio (térmica)" Or cboTipoRampa.Text = "Rampa de subida a quente (térmica)" Then
                If (FormaAtual <> FormaAntiga) And TipoAntigo <> "Rampa de descida" Then
                    Response.Write("<script lang='javascript'>")
                    Response.Write("window.alert('Não é possível cadastrar FORMAS diferentes. Verifique por favor!')")
                    Response.Write("</script>")
                    Exit Sub
                End If

                If (TipoAtual <> TipoAntigo) And TipoAntigo <> "Rampa de descida" Then
                    Response.Write("<script lang='javascript'>")
                    Response.Write("window.alert('Não é possível cadastrar TIPOS diferentes para Rampas de Subida. Verifique por favor!')")
                    Response.Write("</script>")
                    Exit Sub
                End If
            End If

            If cboTipoRampa.SelectedValue = "Rampa de descida" Then
                If (FormaAtual <> FormaAntiga) And TipoAntigo = "Rampa de descida" Then
                    Response.Write("<script lang='javascript'>")
                    Response.Write("window.alert('Não é possível cadastrar FORMAS diferentes. Verifique por favor!')")
                    Response.Write("</script>")
                    Exit Sub
                End If

            End If
        Next


        'Verificando o TON
        Dim tempoInclusao As Integer = 0
        H1 = 0
        TEMPO = cboTempo.Text
        HH = Mid(TEMPO, 1, InStr(TEMPO, ":") - 1)
        MM = Mid(TEMPO, InStr(TEMPO, ":") + 1, 2)
        tempoInclusao = (CInt(HH) * 60) + CInt(MM)

        For I = 0 To lstGDEssem.Count - 1
            Dim tipoTabela As String = lstGDEssem.Item(I).TipoRampa

            If cboTipoRampa.Text = "Rampa de subida a frio (térmica)" Or cboTipoRampa.Text = "Rampa de subida a quente (térmica)" Then
                If tipoTabela = "Rampa de descida" Then
                    TEMPO = lstGDEssem.Item(I).Tempo
                    HH = Mid(TEMPO, 1, InStr(TEMPO, ":") - 1)
                    MM = Mid(TEMPO, InStr(TEMPO, ":") + 1, 2)
                    H1 = (CInt(HH) * 60) + CInt(MM)
                End If
            Else
                If tipoTabela = "Rampa de subida a frio (térmica)" Or tipoTabela = "Rampa de subida a quente (térmica)" Then
                    TEMPO = lstGDEssem.Item(I).Tempo
                    HH = Mid(TEMPO, 1, InStr(TEMPO, ":") - 1)
                    MM = Mid(TEMPO, InStr(TEMPO, ":") + 1, 2)
                    H1 = (CInt(HH) * 60) + CInt(MM)
                End If
            End If
        Next



        Dim TonMins As Double
        TonMins = CDbl(txtTon.Text) * 60

        Dim Tot As Double = tempoInclusao + H1
        If (Tot > TonMins) Then
            Response.Write("<script lang='javascript'>")
            Response.Write("  window.alert('ATENÇÃO: A soma dos tempos das rampas é superior ao Ton!')")
            Response.Write("</script>")
            'Exit Sub
        End If


        'Verifica se na GRID 1 já existe algum cadastro com o mesmo tempo 
        Dim TmpAtual As String = cboTempo.SelectedValue
        Dim TRampaAtual As String = cboTipoRampa.SelectedValue
        For I = 0 To lstGDEssem.Count - 1
            Dim TRampaGrid As String = lstGDEssem.Item(I).TipoRampa
            Dim TmpGrid As String = lstGDEssem.Item(I).Tempo
            If (TmpAtual = TmpGrid) And (TRampaGrid = TRampaAtual) And (TmpGrid <> "&nbsp;") And (TRampaGrid <> "&nbsp;") Then
                Response.Write("<script lang='javascript'>")
                Response.Write("  window.alert('Já existe um cadastro com o Tempo informado!                                    Tipo de Rampa =" & TRampaAtual & " e Tempo = " & TmpAtual & " Linha da Grid = " & (I + 1) & ". Verifique, por favor!')")
                Response.Write("</script>")
                Exit Sub
            End If
        Next


        'Se for Linear, só deixa inserir 1 registro para cada Tipo de Rampa
        If cboFormaRampa.SelectedValue = "Linear" Then
            For I = 0 To lstGDEssem.Count - 1

                If (cboFormaRampa.SelectedValue = lstGDEssem.Item(I).FormaRampa) And (cboTipoRampa.SelectedValue = lstGDEssem.Item(I).TipoRampa) Then
                    Response.Write("<script lang='javascript'>")
                    Response.Write("  window.alert('Esta Rampa Linear já foi incluída no Grid.')")
                    Response.Write("</script>")
                    Exit Sub
                End If
            Next
        End If


        'Subida: Tempo & Pot > Tempo & Pot da Grid /Descida: Tempo > Tempo da Grid & pot < pot grid
        TEMPO = cboTempo.Text
        HH = Mid(TEMPO, 1, InStr(TEMPO, ":") - 1)
        MM = Mid(TEMPO, InStr(TEMPO, ":") + 1, 2)
        H1 = (CInt(HH) * 60) + CInt(MM)
        Achou = False
        lstGDEssem = Session("GrdDessem")

        If lstGDEssem.Count = 0 Then Achou = True

        If cboTipoRampa.SelectedValue = "Rampa de subida a frio (térmica)" Or cboTipoRampa.SelectedValue = "Rampa de subida a quente (térmica)" Then
            For I = 0 To lstGDEssem.Count - 1
                TEMPO = lstGDEssem.Item(I).Tempo
                HH = Mid(TEMPO, 1, InStr(TEMPO, ":") - 1)
                MM = Mid(TEMPO, InStr(TEMPO, ":") + 1, 2)
                H2 = (CInt(HH) * 60) + CInt(MM)

                If (cboFormaRampa.SelectedValue = lstGDEssem.Item(I).FormaRampa) And (lstGDEssem.Item(I).TipoRampa = "Rampa de subida a frio (térmica)" Or lstGDEssem.Item(I).TipoRampa = "Rampa de subida a quente (térmica)") Then
                    If (H1 > H2) And (CDbl(txtPotencia.Text) > CDbl(lstGDEssem.Item(I).Pot)) Then
                        Achou = True
                    Else
                        Achou = False
                    End If
                Else
                    Achou = True
                End If
            Next
        Else
            For I = 0 To lstGDEssem.Count - 1
                TEMPO = lstGDEssem.Item(I).Tempo
                HH = Mid(TEMPO, 1, InStr(TEMPO, ":") - 1)
                MM = Mid(TEMPO, InStr(TEMPO, ":") + 1, 2)
                H2 = (CInt(HH) * 60) + CInt(MM)

                If (cboFormaRampa.SelectedValue = lstGDEssem.Item(I).FormaRampa) And lstGDEssem.Item(I).TipoRampa = "Rampa de descida" Then
                    If (H1 > H2) And (CDbl(txtPotencia.Text) < CDbl(lstGDEssem.Item(I).Pot)) Then
                        Achou = True
                    Else
                        Achou = False
                    End If
                Else
                    Achou = True
                End If
            Next
        End If

        If Achou = False And (cboTipoRampa.SelectedValue = "Rampa de subida a frio (térmica)" Or cboTipoRampa.SelectedValue = "Rampa de subida a quente (térmica)") Then
            Response.Write("<script lang='javascript'>")
            Response.Write("  window.alert('O Tempo e a Potência para RAMPA DE SUBIDA deve ser crescente!')")
            Response.Write("</script>")
            Exit Sub
        ElseIf Achou = False And cboTipoRampa.SelectedValue = "Rampa de descida" Then
            Response.Write("<script lang='javascript'>")
            Response.Write("  window.alert('O Tempo e a Potência para RAMPA DE DESCIDA deve ser descresente!')")
            Response.Write("</script>")
            Exit Sub
        End If


        'Se existir rampa diferente da que está sendo inserida
        CON1 = ""
        CON1 = "select distinct id_tprampausi from tb_rampaugeconjuntural where codusina = '" + txtCodUsina.Text + "' and cod_conjuntouge = '" & cboUGDessem.SelectedValue & "' and id_tprampausi = " + txtIDTipoRampa.Text & " and din_fimvig is null"
        Try
            CmdPdpSql.Connection = ConnPDPSql
            ConnPDPSql.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
            ConnPDPSql.Open()
            CmdPdpSql.CommandText = CON1

            rsDataHeaderSql = CmdPdpSql.ExecuteReader
            Do While rsDataHeaderSql.Read
                If CInt(rsDataHeaderSql("id_tprampausi")) <> CInt(txtIDTipoRampa.Text) Then
                    Response.Write("<script lang='javascript'>")
                    Response.Write("  window.alert('Já existe uma rampa conjuntural cadastrada para esta UGE diferente da selecionada.')")
                    Response.Write("</script>")
                    rsDataHeaderSql.Close()
                    Exit Sub
                End If
            Loop

            If IsNothing(Session("GrdDessem")) Then
                Session("GrdDessem") = New List(Of GDessem)
            End If
            lstGDEssem = Session("GrdDessem")

            lstGDEssem.Add(New GDessem(cboUGDessem.SelectedValue, cboFormaRampa.SelectedValue, cboTipoRampa.SelectedValue,
                                       cboTempo.SelectedValue, txtPotencia.Text, txtTon.Text, txtPotenciaMin.Text, txtIDFormaRampa.Text, txtIDTipoRampa.Text, strUsuar, ""))


            Session("GrdDessem") = lstGDEssem
            lstGDEssem = Session("GrdDessem")
            GridDessem.DataSource = lstGDEssem
            GridDessem.DataBind()


            Valida_Potencia()

            'ordenação do grid
            objTable.Columns.Add("UGE")
            objTable.Columns.Add("FormaRampa")
            objTable.Columns.Add("TipoRampa")
            objTable.Columns.Add("Tempo")
            objTable.Columns.Add("Pot")
            objTable.Columns.Add("Ton")
            objTable.Columns.Add("PotMinUGE")
            objTable.Columns.Add("IDFormaRampa")
            objTable.Columns.Add("IDTipoRampa")
            objTable.Columns.Add("Lgn_Usuario")
            objTable.Columns.Add("id_rampaugeconjuntural")

            Tot = lstGDEssem.Count
            For I = 0 To Tot - 1
                objRow = objTable.NewRow
                objRow("UGE") = lstGDEssem.Item(I).UGE
                objRow("FormaRampa") = lstGDEssem.Item(I).FormaRampa
                objRow("TipoRampa") = lstGDEssem.Item(I).TipoRampa
                objRow("Tempo") = lstGDEssem.Item(I).Tempo
                objRow("Pot") = lstGDEssem.Item(I).Pot
                objRow("Ton") = lstGDEssem.Item(I).Ton
                objRow("PotMinUGE") = lstGDEssem.Item(I).PotMinUGE
                objRow("IDFormaRampa") = lstGDEssem.Item(I).IDFormaRampa
                objRow("IDTipoRampa") = lstGDEssem.Item(I).IDTipoRampa
                objRow("Lgn_Usuario") = lstGDEssem.Item(I).Lgn_Usuario
                objRow("id_rampaugeconjuntural") = lstGDEssem.Item(I).id_rampaugeconjuntural

                objTable.Rows.Add(objRow)
            Next

            Dim dv As DataView = New DataView(objTable)

            dv.Sort = "IDTipoRampa, Tempo"

            GridDessem.DataSource = dv
            GridDessem.DataBind()
            'fim da ordenação


        Catch
            Response.Write("<script lang='javascript'>")
            Response.Write("  window.alert('Erro no acesso ao Banco de Dados')")
            Response.Write("</script>")
        End Try
    End Sub


    Protected Sub btnSalvar_Click(sender As Object, e As ImageClickEventArgs) Handles btnSalvar.Click
        lstExclusao = Session("lstExclusao")

        If GridDessem.Items.Count = 0 Then
            If IsNothing(lstExclusao) Then
                Response.Write("<script lang='javascript'>")
                Response.Write("  window.alert('Nenhuma informação para salvar.')")
                Response.Write("</script>")
                Exit Sub
            ElseIf lstExclusao.Count = 0 Then
                Response.Write("<script lang='javascript'>")
                Response.Write("  window.alert('Nenhuma informação para salvar.')")
                Response.Write("</script>")
                Exit Sub
            End If
        End If


        Try
            'Verifica se os dados atingiram o valor da potência mínima na subida e de 0 na descida
            Dim FlagTRS As Boolean = False
            Dim FlagTRD As Boolean = False
            Dim TemRS As Boolean = False
            Dim TemRD As Boolean = False
            lstGDEssem = Session("GrdDessem")

            For I = 0 To lstGDEssem.Count - 1
                If lstGDEssem.Item(I).TipoRampa = "Rampa de subida a frio (térmica)" Or lstGDEssem.Item(I).TipoRampa = "Rampa de subida a quente (térmica)" Then
                    TemRS = True
                    If lstGDEssem.Item(I).Pot = txtPotenciaMin.Text Then
                        FlagTRS = True
                    End If
                ElseIf lstGDEssem.Item(I).TipoRampa Like "Rampa de descida" Then
                    TemRD = True
                    If lstGDEssem.Item(I).Pot = "0" Then
                        FlagTRD = True
                    End If
                End If
            Next

            If TemRS = True Then 'Se tiver Rampa de Subida sem a potência min
                If FlagTRS = False Then
                    Response.Write("<script lang='javascript'>")
                    Response.Write("  window.alert('A Rampa de Subida deve atingir a Potência Mínima. Verifique, por favor!')")
                    Response.Write("</script>")
                    Exit Sub
                End If
            End If

            If TemRD = True Then 'Se tiver Rampa de Descida sem 0
                If FlagTRD = False Then
                    Response.Write("<script lang='javascript'>")
                    Response.Write("  window.alert('A Rampa de Descida deve atingir a Potência 0. Verifique, por favor!')")
                    Response.Write("</script>")
                    Exit Sub
                End If
            End If

            ConnPDPSql.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
            CmdPdpSql.Connection = ConnPDPSql
            ConnPDPSql.Open()

            ' Coloquei esta verificação pois aplicação estava quebrando quando a session vinha nula
            If Not IsNothing(Session("lstRampConj")) Then
                lstRampConj = Session("lstRampConj")
            End If

            'lstExclusao = Session("lstExclusao")
            CON2 = ""
            If Not IsNothing(lstExclusao) Then
                If lstExclusao.Count > 0 Then
                    For I = 0 To lstExclusao.Count - 1
                        If lstExclusao.Item(I).id_rampaugeconjuntural.Length > 0 Then
                            CON2 = CON2 & lstExclusao.Item(I).id_rampaugeconjuntural & ", "
                            If I = (lstExclusao.Count - 1) Then CON2 = Mid(CON2, 1, (Len(CON2) - 2))
                        End If
                    Next

                    If (CON2.Length > 0) Then
                        CON1 = "update tb_rampaugeconjuntural set lgn_usuario = '" & strUsuar & "', din_fimvig = getdate() where id_rampaugeconjuntural in (" & CON2 & ")"

                        objTransSql = ConnPDPSql.BeginTransaction()
                        CmdPdpSql.Transaction = objTransSql

                        CmdPdpSql.CommandText = CON1
                        CmdPdpSql.ExecuteNonQuery()
                        objTransSql.Commit()
                    End If
                End If
            End If

            CON1 = ""
            'CON2 = ""
            For IntI = 0 To lstGDEssem.Count - 1
                TEMPO = lstGDEssem.Item(IntI).Tempo
                HH = Mid(TEMPO, 1, InStr(TEMPO, ":") - 1)
                MM = Mid(TEMPO, InStr(TEMPO, ":") + 1, 2)
                H1 = (CInt(HH) * 60) + CInt(MM)
                prd_tempo = CStr(H1)

                codusina = txtCodUsina.Text
                cod_conjuntouge = cboUGDessem.SelectedValue
                id_tpformarampa = lstGDEssem.Item(IntI).IDFormaRampa
                id_tprampausi = lstGDEssem.Item(IntI).IDTipoRampa
                'prd_tempo = H1
                val_potencia = lstGDEssem.Item(IntI).Pot
                prd_ton = lstGDEssem.Item(IntI).Ton
                Achou = False

                If Not IsNothing(lstRampConj) Then
                    If lstRampConj.Count > 0 Then
                        For I = 0 To lstRampConj.Count - 1 'Para cada linha do Grid, eu verifico se já existia ou se é novo
                            HH = Mid(lstRampConj.Item(I).Tempo, 1, InStr(lstRampConj.Item(I).Tempo, ":") - 1)
                            MM = Mid(lstRampConj.Item(I).Tempo, InStr(lstRampConj.Item(I).Tempo, ":") + 1, 2)
                            H1 = (CInt(HH) * 60) + CInt(MM)
                            prd_tempo1 = CStr(H1)

                            If lstRampConj.Item(I).UGE = cod_conjuntouge And lstRampConj.Item(I).IDFormaRampa = id_tpformarampa And lstRampConj.Item(I).IDTipoRampa = id_tprampausi And
                                   prd_tempo1 = prd_tempo And lstRampConj.Item(I).Pot = val_potencia And lstRampConj.Item(I).Ton = prd_ton Then
                                Achou = True
                                Exit For
                            End If
                        Next
                    End If
                End If
                If Achou = False Then
                    CON1 = CON1 & "insert into tb_rampaugeconjuntural (codusina, cod_conjuntouge, id_tpformarampa, id_tprampausi, prd_tempo, val_potencia, prd_ton, din_iniciovig, lgn_usuario) values "
                    CON1 = CON1 & "('" & codusina & "', '" & cod_conjuntouge & "', " & CInt(id_tpformarampa) & ", " & CInt(id_tprampausi) & ", '" & prd_tempo & "', " & CDbl(val_potencia) & ", " & CDbl(prd_ton) & ", GETDATE(), '" & strUsuar & "'); "
                End If
            Next
            If (GridDessem.Items.Count <> 0) And CON1 <> "" Then
                CON1 = Mid(CON1, 1, (Len(CON1) - 2))

                objTransSql = ConnPDPSql.BeginTransaction()
                CmdPdpSql.Transaction = objTransSql

                CmdPdpSql.CommandText = CON1
                CmdPdpSql.ExecuteNonQuery()
                objTransSql.Commit()

                CmdPdpSql.Connection.Close()
                ConnPDPSql.Close()

                ' demanda 26260
                EnviarEmail(txtCodUsina.Text, cboUGDessem.SelectedValue, strUsuar, "ATUALIZAÇÃO")
                ' demanda 26260
                LimparDados()
            End If
            Response.Write("<script lang='javascript'>")
            Response.Write("  window.alert('Dados salvos com sucesso!')")
            Response.Write("</script>")
        Catch ex As Exception
            Dim excp = ex.Message
            objTrans.Rollback()
            CmdPdpSql.Connection.Close()
            ConnPDPSql.Close()

            Response.Write("<script lang='javascript'>")
            Response.Write("  window.alert('Erro ao salvar os dados. Tente novamente.')")
            Response.Write("</script>")
        End Try

    End Sub

    Protected Sub cboTipoRampa_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboTipoRampa.SelectedIndexChanged

        ' MostrarGridEstruturais("", cboUGDessem.SelectedValue, cboTipoRampa.SelectedValue)
        Try
            CON1 = ""
            CON1 = "select id_tprampausi from tb_tprampausi where nom_tprampausi = '" & cboTipoRampa.SelectedValue & "'"

            CmdBdTecn.Connection = ConnBdTecn
            ConnBdTecn.Open("bdtecn")
            CmdBdTecn.CommandText = CON1
            rsDataHeader = CmdBdTecn.ExecuteReader

            Do While rsDataHeader.Read
                txtIDTipoRampa.Text = Trim(rsDataHeader("id_tprampausi"))
            Loop
            rsDataHeader.Close()
            ConnBdTecn.Close()
            CmdBdTecn.Connection.Close()

            Valida_Potencia()
        Catch
            Response.Write("<script lang='javascript'>")
            Response.Write("  window.alert('Ocorreu um erro ao carregar o campo. Tente novamente!')")
            Response.Write("</script>")
            Exit Sub
        End Try
    End Sub


    Private Sub btnCancelar_Click(sender As Object, e As ImageClickEventArgs) Handles btnCancelar.Click
        LimparDados()

    End Sub



    Private Sub MostrarGridEstruturais(UsiIDSel As String, UGESel As String, TPRampa As String)
        CON1 = ""

        CON1 = "Select tb_conjuntouge.cod_conjuntouge UGE, tb_tpformarampa.nom_tpformarampa FormaRampa, tb_tprampausi.nom_tprampausi TipoRampa, "
        CON1 = CON1 & "tb_rampaconjuntouge.prd_rampaugeequiv Tempo, tb_rampaconjuntouge.val_rampaugeequiv Pot, tb_conjuntougeequivrampa.prd_ton TON, "
        CON1 = CON1 & "tb_conjuntougeequivrampa.val_potminugeequiv PotMinUGE, tb_tpformarampa.id_tpformarampa IDFormaRampa, tb_tprampausi.id_tprampausi IDTipoRampa, '' lgn_usuario "
        CON1 = CON1 & "From tb_rampaconjuntouge "
        CON1 = CON1 & "inner Join tb_conjuntougerampatipo on tb_rampaconjuntouge.id_conjuntougerampatipo = tb_conjuntougerampatipo.id_conjuntougerampatipo "
        CON1 = CON1 & "inner Join tb_tprampausi on tb_conjuntougerampatipo.id_tprampausi = tb_tprampausi.id_tprampausi "
        CON1 = CON1 & "inner Join tb_tpformarampa on tb_tpformarampa.id_tpformarampa = tb_conjuntougerampatipo.id_tpformarampa "
        CON1 = CON1 & "inner Join tb_conjuntougeequivrampa on tb_conjuntougeequivrampa.id_conjuntougeequivrampa = tb_conjuntougerampatipo.id_conjuntougeequivrampa "
        CON1 = CON1 & "inner Join tb_conjuntouge on tb_conjuntouge.id_conjuntouge = tb_conjuntougeequivrampa.id_conjuntouge "
        CON1 = CON1 & "where tb_conjuntouge.id_tpconjuntouge = 3 "
        CON1 = CON1 & "And (tb_conjuntouge.din_fimvig Is null Or tb_conjuntouge.din_fimvig > current) "

        If UsiIDSel <> "" Then
            CON1 = CON1 & "And tb_conjuntougeequivrampa.usi_id = '" & UsiIDSel & "' "
        End If

        If UGESel <> "" Then
            CON1 = CON1 & "And tb_conjuntouge.cod_conjuntouge = '" & UGESel & "' "
            'Else
            '    CON1 = CON1 & "And tb_conjuntouge.cod_conjuntouge in ('" & CON2 & ") "
        End If

        'If TPRampa <> "" Then
        '    CON1 = CON1 & "And tb_tprampausi.nom_tprampausi = '" & Trim(TPRampa) & "'"
        'End If

        CmdBdTecn.Connection = ConnBdTecn
        If ConnBdTecn.State <> 1 Then ConnBdTecn.Open("bdtecn")

        Try
            CmdBdTecn.CommandText = CON1
            rsDataHeader = CmdBdTecn.ExecuteReader

            Session("GrdEstrutural") = New List(Of GDessem)
            lstEstrutural = Session("GrdEstrutural")

            ObjListItem = New WebControls.ListItem
            ObjListItem.Text = ""
            ObjListItem.Value = "0"

            I = 0
            Do While rsDataHeader.Read
                TEM = CDbl(Trim(rsDataHeader("Tempo"))) / 60

                If InStr(TEM, ",") <> 0 Or InStr(TEM, ".") <> 0 Then
                    If InStr(TEM, ",") <> 0 Then
                        H1 = CInt(Mid(TEM, 1, InStr(TEM, ",")))
                    ElseIf InStr(TEM, ".") <> 0 Then
                        H1 = CInt(Mid(TEM, 1, InStr(TEM, ".")))
                    End If
                Else
                    H1 = CInt(TEM)
                End If
                If H1 < 10 Then
                    HH = "0" & H1
                Else
                    HH = H1
                End If

                If Right(TEM, 2) = ",5" Or Right(TEM, 2) = ".5" Then
                    MM = ":30"
                Else
                    MM = ":00"
                End If
                TEMPO = HH + MM

                lstEstrutural.Add(New GDessem(Trim(rsDataHeader("UGE")), Trim(rsDataHeader("FormaRampa")), Trim(rsDataHeader("TipoRampa")),
                                 TEMPO, Trim(rsDataHeader("Pot")), Trim(rsDataHeader("TON")), Trim(rsDataHeader("PotMinUGE")), Trim(rsDataHeader("IDFormaRampa")), Trim(rsDataHeader("IDTipoRampa")), Trim(rsDataHeader("Lgn_Usuario")), ""))

                ObjListItem = New System.Web.UI.WebControls.ListItem
                If lstEstrutural.Count = 1 Then
                    txtPotenciaMin.Text = Trim(rsDataHeader("PotMinUGE"))
                    txtTon.Text = Trim(rsDataHeader("TON"))
                End If
                I = I + 1
            Loop

            If lstEstrutural.Count = 0 Then
                For Each objRow In objTable.Rows
                    objRow.Delete()
                Next

                objTable.Columns.Add("UGE")
                objTable.Columns.Add("FormaRampa")
                objTable.Columns.Add("TipoRampa")
                objTable.Columns.Add("Tempo")
                objTable.Columns.Add("Pot")
                objTable.Columns.Add("Ton")
                objTable.Columns.Add("PotMinUGE")
                objTable.Columns.Add("IDFormaRampa")
                objTable.Columns.Add("IDTipoRampa")
                objTable.Columns.Add("Lgn_Usuario")
                objTable.Columns.Add("id_rampaugeconjuntural")

                For I = 1 To 15
                    objRow = objTable.NewRow
                    objRow("UGE") = ""
                    objRow("FormaRampa") = ""
                    objRow("TipoRampa") = ""
                    objRow("Tempo") = ""
                    objRow("Pot") = ""
                    objRow("Ton") = ""
                    objRow("PotMinUGE") = ""
                    objRow("IDFormaRampa") = ""
                    objRow("IDTipoRampa") = ""
                    objRow("Lgn_Usuario") = ""
                    objRow("id_rampaugeconjuntural") = ""
                    objTable.Rows.Add(objRow)

                Next I


                GridEstrutural.DataSource = objTable
                GridEstrutural.DataBind()
            Else

                Session("GrdEstrutural") = lstEstrutural
                lstEstrutural = Session("GrdEstrutural")
                GridEstrutural.DataSource = lstEstrutural
                GridEstrutural.DataBind()
            End If

            If UGESel <> "" Then
                Mostrar.Enabled = True
                btnImportarGERCAD.Enabled = True
            Else
                Mostrar.Enabled = False
                btnImportarGERCAD.Enabled = False
            End If

            rsDataHeader.Close()
            CmdBdTecn.Connection.Close()
            ConnBdTecn.Close()
        Catch

        End Try

    End Sub


    Private Sub GridDessem_PageIndexChanged(source As Object, e As DataGridPageChangedEventArgs) Handles GridDessem.PageIndexChanged
        GridDessem.CurrentPageIndex = e.NewPageIndex
        lstGDEssem = Session("GrdDessem")
        GridDessem.DataSource = lstGDEssem
        GridDessem.DataBind()
    End Sub

    Private Sub GridEstrutural_PageIndexChanged(source As Object, e As DataGridPageChangedEventArgs) Handles GridEstrutural.PageIndexChanged
        GridEstrutural.CurrentPageIndex = e.NewPageIndex
        lstEstrutural = Session("GrdEstrutural")
        GridEstrutural.DataSource = lstEstrutural
        GridEstrutural.DataBind()

    End Sub

    Private Sub Valida_Potencia()
        If (cboFormaRampa.SelectedValue = "Linear") And (cboTipoRampa.SelectedValue = "Rampa de descida") Then
            txtPotencia.Text = "0"
            txtPotencia.Enabled = False
        ElseIf (cboFormaRampa.SelectedValue = "Linear") And (cboTipoRampa.SelectedValue = "Rampa de subida a frio (térmica)" Or cboTipoRampa.SelectedValue = "Rampa de subida a quente (térmica)") Then
            txtPotencia.Text = txtPotenciaMin.Text
            txtPotencia.Enabled = False
        Else
            txtPotencia.Text = ""
            txtPotencia.Enabled = True
        End If
    End Sub

    Private Sub LimparDados()
        Session.Remove("GrdEstrutural")
        Session.Remove("GrdDessem")
        Session.Remove("lstExclusao")
        Session.Remove("lstRampConj")
        Session.Remove("lstInclusao")

        OptDescida.Checked = False
        OptSubFrio.Checked = False
        OptSubQuente.Checked = False
        btnImportarGERCAD.Enabled = False
        Mostrar.Enabled = False

        CON1 = ""
        CON2 = ""

        cboUsina.ClearSelection()
        txtCodUsina.Text = ""
        cboUGDessem.ClearSelection()
        cboUGDessem.Items.Clear()
        cboFormaRampa.ClearSelection()
        txtIDFormaRampa.Text = ""
        cboTipoRampa.ClearSelection()
        txtIDTipoRampa.Text = ""
        cboTempo.ClearSelection()
        txtPotencia.Text = ""
        txtPotenciaMin.Text = ""
        txtTon.Text = ""

        LST1.Clear()
        If Not IsNothing(lstGDEssem) Then lstGDEssem.Clear()
        If Not IsNothing(lstEstrutural) Then lstEstrutural.Clear()
        If Not IsNothing(lstExclusao) Then lstExclusao.Clear()
        If Not IsNothing(lstRampConj) Then lstRampConj.Clear()
        If Not IsNothing(lstInclusao) Then lstInclusao.Clear()

        For Each objRow In objTable.Rows
            objRow.Delete()
        Next

        objTable.Columns.Add("UGE")
        objTable.Columns.Add("FormaRampa")
        objTable.Columns.Add("TipoRampa")
        objTable.Columns.Add("Tempo")
        objTable.Columns.Add("Pot")
        objTable.Columns.Add("Ton")
        objTable.Columns.Add("PotMinUGE")
        objTable.Columns.Add("IDFormaRampa")
        objTable.Columns.Add("IDTipoRampa")
        objTable.Columns.Add("Lgn_Usuario")
        objTable.Columns.Add("id_rampaugeconjuntural")

        For I = 1 To 15
            objRow = objTable.NewRow
            objRow("UGE") = ""
            objRow("FormaRampa") = ""
            objRow("TipoRampa") = ""
            objRow("Tempo") = ""
            objRow("Pot") = ""
            objRow("Ton") = ""
            objRow("PotMinUGE") = ""
            objRow("IDFormaRampa") = ""
            objRow("IDTipoRampa") = ""
            objRow("Lgn_Usuario") = ""
            objRow("id_rampaugeconjuntural") = ""

            objTable.Rows.Add(objRow)
            If I = 6 Then
                GridDessem.DataSource = objTable
                GridDessem.DataBind()
            End If
        Next I

        GridEstrutural.DataSource = objTable
        GridEstrutural.DataBind()

    End Sub

    Private Sub btnImportarGERCAD_Click(sender As Object, e As ImageClickEventArgs) Handles btnImportarGERCAD.Click
        If Not (OptSubFrio.Checked) And Not (OptSubQuente.Checked) And Not (OptDescida.Checked) Then
            Response.Write("<script lang='javascript'>")
            Response.Write("  window.alert('Nenhuma rampa para importar do GERCAD.')")
            Response.Write("</script>")
            btnImportarGERCAD.Enabled = False
            Exit Sub
        End If

        If GridEstrutural.Items.Count = 0 Then
            Response.Write("<script lang='javascript'>")
            Response.Write("  window.alert('Nenhuma rampa para importar do GERCAD.')")
            Response.Write("</script>")
            btnImportarGERCAD.Enabled = False
            Exit Sub
        End If

        Try
            'pego o ops que está marcado
            If OptSubFrio.Checked Then
                TRampImp = 6
            ElseIf OptSubQuente.Checked Then
                TRampImp = 7
            Else
                TRampImp = 8
            End If

            'pego as rampas no GERCAD
            lstEstrutural = Session("GrdEstrutural")
            Dim listaFiltrada2 As List(Of GDessem) 'GRID 2
            Dim listaFiltrada1 As List(Of GDessem) 'GRID 1

            listaFiltrada2 = lstEstrutural.FindAll(Function(p As GDessem) p.IDTipoRampa = TRampImp)
            'Verificando se existe o tipo de rampa estrutural no GERCAD para realizar a importação
            If listaFiltrada2.Count > 0 Then
                'verifico se tem a rampa no grid1
                lstGDEssem = Session("GrdDessem")
                listaFiltrada1 = lstGDEssem.FindAll(Function(p As GDessem) p.IDTipoRampa = TRampImp)
                'Verificando se existe rampa outro tipo de rampa de subida
                If (listaFiltrada1.Count = 0 And TRampImp = 6) Then
                    listaFiltrada1 = lstGDEssem.FindAll(Function(p As GDessem) p.IDTipoRampa = 7) 'Buscando a outra rampa de subida
                ElseIf (listaFiltrada1.Count = 0 And TRampImp = 7) Then
                    listaFiltrada1 = lstGDEssem.FindAll(Function(p As GDessem) p.IDTipoRampa = 6) 'Buscando a outra rampa de subida
                End If

                If listaFiltrada1.Count > 0 Then
                    lstExclusao = Session("lstExclusao")

                    For I = 0 To listaFiltrada1.Count - 1 'incluo toda a rampa na lista de exclusão lógica
                        If IsNothing(lstExclusao) Then
                            Session("lstExclusao") = New List(Of GDessem)
                            lstExclusao = Session("lstExclusao")

                        ElseIf lstExclusao.Count = 0 Then
                            Session("lstExclusao") = New List(Of GDessem)
                            lstExclusao = Session("lstExclusao")
                        Else
                            If cboUGDessem.SelectedValue <> lstExclusao.Item(0).UGE Then
                                lstExclusao = New List(Of GDessem)
                            End If
                        End If

                        lstExclusao.Add(New GDessem(cboUGDessem.SelectedValue, listaFiltrada1.Item(I).FormaRampa, listaFiltrada1.Item(I).TipoRampa,
                        listaFiltrada1.Item(I).Tempo, listaFiltrada1.Item(I).Pot, listaFiltrada1.Item(I).Ton, listaFiltrada1.Item(I).PotMinUGE, listaFiltrada1.Item(I).IDFormaRampa,
                        listaFiltrada1.Item(I).IDTipoRampa, listaFiltrada1.Item(I).Lgn_Usuario, listaFiltrada1.Item(I).id_rampaugeconjuntural))

                        lstGDEssem.RemoveAll(Function(p As GDessem) p.IDTipoRampa = listaFiltrada1.Item(I).IDTipoRampa) 'removo toda a rampa do grid1
                    Next
                End If

                Session("GrdDessem") = lstGDEssem
                lstGDEssem = Session("GrdDessem")

                For I = 0 To listaFiltrada2.Count - 1
                    lstGDEssem.Add(New GDessem(cboUGDessem.SelectedValue, listaFiltrada2.Item(I).FormaRampa, listaFiltrada2.Item(I).TipoRampa,
                    listaFiltrada2.Item(I).Tempo, listaFiltrada2.Item(I).Pot, listaFiltrada2.Item(I).Ton, listaFiltrada2.Item(I).PotMinUGE, listaFiltrada2.Item(I).IDFormaRampa,
                    listaFiltrada2.Item(I).IDTipoRampa, listaFiltrada2.Item(I).Lgn_Usuario, listaFiltrada2.Item(I).id_rampaugeconjuntural))
                Next

                Dim novalista = lstGDEssem.OrderBy(Function(o) o.IDTipoRampa).ThenBy(Function(o) o.Tempo).ToList()

                Session("GrdDessem") = novalista
                lstGDEssem.Clear()
                lstGDEssem = Session("GrdDessem")

                GridDessem.DataSource = lstGDEssem
                GridDessem.DataBind()


                Response.Write("<script lang='javascript'>")
                Response.Write("  window.alert('Rampa importada com sucesso do GERCAD.')")
                Response.Write("</script>")

                OptSubQuente.Checked = False
                OptSubFrio.Checked = False
                OptDescida.Checked = False
            Else
                Response.Write("<script lang='javascript'>")
                Response.Write("  window.alert('O tipo de rampa selecionado não existe no cadastro do GERCAD.')")
                Response.Write("</script>")
            End If

        Catch ex As Exception

        End Try

    End Sub

    Private Sub EnviarEmail(ByVal Usina As String, ByVal UGEquivalente As String, ByVal usuario As String, ByVal tipoTransacao As String)

        Dim mail As New MailMessage()
        Dim Email, Username, HostSMTP, strBody As String
        Dim Port As Integer

        Email = ConfigurationManager.AppSettings("EmailGrupoONS")
        Username = ConfigurationManager.AppSettings("EmailUserName")
        HostSMTP = ConfigurationManager.AppSettings("EmailHost")
        Port = ConfigurationManager.AppSettings("EmailPort")

        ' ----------------------------------------------------------
        strBody = "<html>"
        strBody += "<body>"
        strBody += "<table>"
        strBody += "<tr>"
        strBody += "<td>"
        strBody += "Usuário Login: " + usuario
        strBody += "</td>"
        strBody += "</tr>"

        strBody += "<tr>"
        strBody += "<td>"
        strBody += "Usina: " + Usina
        strBody += "</td>"
        strBody += "</tr>"

        strBody += "<tr>"
        strBody += "<td>"
        strBody += "UG Equivalente: " + UGEquivalente
        strBody += "</td>"
        strBody += "</tr>"

        strBody += "</table>"
        strBody += "</body>"
        strBody += "</html>"

        ' ----------------------------------------------------------

        Dim destinatarios As String() = Email.Split(New Char() {";"c})
        Dim destinatario As String
        For Each destinatario In destinatarios

            'destino
            mail.[To].Add(destinatario)
        Next

        'origem
        mail.From = New MailAddress(Username)
        'assunto
        mail.Subject = "PDP Web: " + tipoTransacao.ToUpper() + " DE RAMPA CONJUNTURAL (" + UGEquivalente + ")"
        'corpo do email (texto)
        mail.Body = strBody
        'é html ?
        mail.IsBodyHtml = True
        'cria instãncia STMP
        Dim smtp As New SmtpClient()
        'define o servidor SMTP
        smtp.Host = HostSMTP
        'Dados do seu servidor SMTP
        'smtp.Credentials = New System.Net.NetworkCredential(Username, SenhaONS)

        Dim CredCache As New System.Net.CredentialCache
        smtp.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials


        'Suas credenciais SMTP
        'habilita o envio via SSL
        smtp.EnableSsl = False
        smtp.Port = Port

        'envia o email
        smtp.Send(mail)


        'Response.Write("<script lang='javascript'>")
        'Response.Write("  window.alert('Notificação enviada para equipe de usuários do ONS')")
        'Response.Write("</script>")
        Exit Sub

    End Sub

End Class