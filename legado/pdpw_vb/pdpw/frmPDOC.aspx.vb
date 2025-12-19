Imports OnsWebControls
Imports FtpClientCtrl
Imports System.Net.Sockets
Imports OnsClasses.OnsData

Public Class frmPDOC
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents dtPDO As System.Web.UI.WebControls.Calendar
    Protected WithEvents lblDtPDO As System.Web.UI.WebControls.Label
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents lblCargFreq1 As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblDefluConsol1 As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblRecomend1 As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblInfMet1 As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblLimObser1 As System.Web.UI.WebControls.LinkButton
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    Protected WithEvents lnkPDPW As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lbIntervencoes1 As System.Web.UI.WebControls.LinkButton
    Protected WithEvents Rblist As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents cmdOk As System.Web.UI.WebControls.Button
    Protected WithEvents Rblist1 As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents cmdOk1 As System.Web.UI.WebControls.Button
    Protected WithEvents RbListValElet As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents CmdVeOk As System.Web.UI.WebControls.Button
    Protected WithEvents lblValElet As System.Web.UI.WebControls.LinkButton
    Protected WithEvents CmdVoltaVe As System.Web.UI.WebControls.Button
    Protected WithEvents cmdVolta1 As System.Web.UI.WebControls.Button
    Protected WithEvents cmdVolta As System.Web.UI.WebControls.Button

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    Public NomeArq(20) As String
    Public TipoArq(2) As String
    Public nomeEmp As String
    Public ArqExiste As Boolean
    Public NomeAge As String
    Public dirData As String

    Private Sub HabilitaCampos()
        cmdOk.Visible = True
        cmdOk1.Visible = True
    End Sub
    Private Sub DesabilitaCampos()
        cmdOk.Visible = False
        cmdOk1.Visible = False
        CmdVeOk.Visible = False
        Rblist.Visible = False
        Rblist1.Visible = False
        RbListValElet.Visible = False
    End Sub
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
#If DEBUG Then
        'Session("cosID") = "N"
        'Session("ageID") = "ENT"
#End If

        ' Data: 07/2012
        ' Empresa: Stefanini
        ' Descrição: Criação e armazenamento da estrutura de ano, mês e dia.
        'CRQ1343 - EXIBICAO DOS RELATORIOS EM UM PERIODO DE 45 DIASC

        If Not Page.IsPostBack Then
            lblError.Visible = False

            'Dim dir As New System.IO.DirectoryInfo(Request.ApplicationPath & "\PDOC")

            Dim Arg As String
            Dim con As New OnsConnection()
            Dim conPDP As New OnsConnection
            Dim cmd As New OnsCommand
            Dim dr As OnsDataReader
            'Dim oLocalMachine As Microsoft.Win32.RegistryKey
            'Dim strUsrBanco As String
            'Dim strPwdBanco As String
            'Dim strHost As String
            'Dim strServico As String
            'Dim strCLocale As String
            'Dim strDBLocale As String
            'Dim strDataSource As String
            'Dim strServer As String

            '' Abre conexão com a base de dados técnica
            '' ----------------------------------------
            'oLocalMachine = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Ons\IU\BDT")

            'strUsrBanco = oLocalMachine.GetValue("Usuario")
            'strPwdBanco = oLocalMachine.GetValue("Senha")

            'strDataSource = oLocalMachine.GetValue("DataSource")

            'strServer = oLocalMachine.GetValue("Server")
            'strHost = oLocalMachine.GetValue("Host")
            'strServico = oLocalMachine.GetValue("Serviço")


            con.Open("BDT")

            If Trim(CosID) = "CN" Then
                Session("ageID") = "FUR"
            ElseIf Trim(CosID) = "SE" Then
                Session("ageID") = "FUR"
            ElseIf Trim(CosID) = "S" Then
                Session("ageID") = "AES"
            ElseIf Trim(CosID) = "NE" Then
                Session("ageID") = "CHF"
            ElseIf Trim(CosID) = "N" Then
                Session("ageID") = "ENT"
            End If

            cmd.Connection = con
            cmd.CommandText = "SELECT nomecurto " &
                         "FROM age " &
                        "where age_id = '" & AgeID & "'"
            dr = cmd.ExecuteReader()
            dr.Read()
            NomeAge = Trim(dr("nomecurto"))
            dr.Close()
            con.Close()

            '' Abre conexão com a base de dados do PDP
            '' ----------------------------------------
            'oLocalMachine = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Ons\IU\PDP")

            'strUsrBanco = oLocalMachine.GetValue("Usuario")
            'strPwdBanco = oLocalMachine.GetValue("Senha")

            'strDataSource = oLocalMachine.GetValue("DataSource")

            'strServer = oLocalMachine.GetValue("Server")
            'strHost = oLocalMachine.GetValue("Host")
            'strServico = oLocalMachine.GetValue("Serviço")

            'conPDP.ConnectionString = "database=" & strDataSource.Trim() & " ;server=" & strServer.Trim() & ";host=" & strHost.Trim() & _
            '                          ";uid=" & strUsrBanco.Trim() & ";password=" & strPwdBanco.Trim() & ";protocol=onsoctcp" & _
            '                          ";service=" & strServico.Trim()
            conPDP.Open("PDP")
            cmd.Connection = conPDP
            Session("nomeEmp") = Trim(NomeAge)
            conPDP.Close()
            Arg = Request.QueryString("tpArq")
            ' Monta o nome do arquivo
            dtPDO.SelectedDate = Today
            lblDtPDO.Text = "Data do PDO: " & Format(dtPDO.SelectedDate, "dd/MM/yyyy")
            Session("dataID") = Format(dtPDO.SelectedDate, "dd/MM/yyyy")
        End If
    End Sub

    Public Sub lnkPDPW_Click(ByVal s As System.Object, ByVal e As System.EventArgs) Handles lnkPDPW.Click
        Dim oLocalMachine As Microsoft.Win32.RegistryKey
        oLocalMachine = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Wow6432Node\Ons\IU\PDOC")
        Response.Redirect(oLocalMachine.GetValue("EndPDP"))
    End Sub
    Public Sub VerificaData(ByVal s As System.Object, ByVal e As System.EventArgs) Handles lblCargFreq1.Click, lblDefluConsol1.Click, lblRecomend1.Click, lblInfMet1.Click, lblLimObser1.Click
        Dim lnkChamou As New System.Web.UI.WebControls.LinkButton
        Dim Arq As String
        Dim lnk As New LinkButton
        Dim ArqCompletData As String

        'CRQ1343
        ArqCompletData = "-" & Format(dtPDO.SelectedDate, "dd-MM-yyyy") & ".doc"

        lnk = s

        Arq = ""
        NomeArq(1) = "PDIC_Completo" & ArqCompletData
        NomeArq(2) = "PDCF" & Mid(Session("DataID"), 1, 2) & Mid(Session("DataID"), 4, 2) & ArqCompletData
        NomeArq(3) = "PDFC" & Mid(Session("DataID"), 1, 2) & Mid(Session("DataID"), 4, 2) & ArqCompletData
        NomeArq(4) = "Reco" & Mid(Session("DataID"), 1, 2) & Mid(Session("DataID"), 4, 2) & ArqCompletData
        NomeArq(5) = "InfMet" & Mid(Session("DataID"), 1, 2) & Mid(Session("DataID"), 4, 2) & ArqCompletData
        NomeArq(6) = "LimObs" & Mid(Session("DataID"), 1, 2) & Mid(Session("DataID"), 4, 2) & ArqCompletData

        lnkChamou = s
        lblDtPDO.Text = "Data do PDO: " & Format(dtPDO.SelectedDate, "dd/MM/yyyy")
        Session("dataID") = Format(dtPDO.SelectedDate, "dd/MM/yyyy")

        If lnkChamou.ID = "lblCargFreq1" Then
            Arq = NomeArq(2)
            ArqExiste = VerificaArquivo(Arq)
        ElseIf lnkChamou.ID = "lblDefluConsol1" Then
            Arq = NomeArq(3)
            ArqExiste = VerificaArquivo(Arq)
        ElseIf lnkChamou.ID = "lblRecomend1" Then
            Arq = NomeArq(4)
            ArqExiste = VerificaArquivo(Arq)
        ElseIf lnkChamou.ID = "lblInfMet1" Then
            Arq = NomeArq(5)
            ArqExiste = VerificaArquivo(Arq)
        ElseIf lnkChamou.ID = "lblLimObser1" Then
            Arq = NomeArq(6)
            ArqExiste = VerificaArquivo(Arq)
        ElseIf lnkChamou.ID = "lblPrevCarga1" Then
            Arq = NomeArq(7)
            ArqExiste = VerificaArquivo(Arq)
        End If
        DesabilitaCampos()

        If ArqExiste Then
            MontaArqFTP(ArqExiste, Arq, lnk.Text)
        Else
            MensagemOff()
        End If
    End Sub
    Private Sub MontaArqFTP(ByVal ArqExiste As Boolean, ByVal Arq As String, ByVal Titulo As String)
        Dim dirData As String

        'CRQ1343
        dirData = Format(dtPDO.SelectedDate, "dd-MM-yyyy")

        Dim oLocalMachine As Microsoft.Win32.RegistryKey
        oLocalMachine = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Wow6432Node\Ons\IU\PDOC")
        Dim ftp As New FtpConnection
        Dim vdirectory As System.IO.Directory

        'recuperando os parametros do registro para estabelecer a conexão com o servidor FTP
        ftp.Username = oLocalMachine.GetValue("FTP_UserName")
        ftp.Password = oLocalMachine.GetValue("FTP_Password")
        ftp.Hostname = oLocalMachine.GetValue("FTP_HOST")
        ftp.Port = oLocalMachine.GetValue("FTP_Port")
        ftp.Connect()

        'verifica se a conexão foi estabelecida com sucesso
        If Not ftp.IsConnected Then
            Response.Write("<SCRIPT>alert('Não foi possível estabelecer a conexão com o servidor FTP.')</SCRIPT>")
            Exit Sub
        End If

        If ArqExiste Then
            lblError.Visible = False
            While InStr(Arq, " ") <> 0
                Arq = Arq.Replace(" ", "%20")
            End While
            Arq = Arq.Replace("Ê", "%CA")
            Arq = Arq.Replace("Á", "%C1")
            Arq = Arq.Replace("Ç", "%C7")
            Arq = Arq.Replace("Ã", "%C3")

            If System.IO.File.Exists(Request.PhysicalApplicationPath & "PDOC\temp\" & Arq) Then
                System.IO.File.Delete(Request.PhysicalApplicationPath & "PDOC\temp\" & Arq)
            End If

            'realiza o downloado do arquivo
            ftp.DownloadFile("\" & oLocalMachine.GetValue("FTP_Pasta") & "\" & dirData & "\" & Arq, Request.PhysicalApplicationPath & "PDOC\temp\" & Arq)
            ftp.DownloadFile(Arq, Request.PhysicalApplicationPath & "PDOC\temp\" & Arq)

            Dim vFileSize As New System.IO.FileInfo(Request.PhysicalApplicationPath & "PDOC\temp\" & Arq)

            If vFileSize.Length = 0 Then
                lblError.Text = "Não há relatório para o dia selecionado. " & Format(dtPDO.SelectedDate, "dd/MM/yyyy")
                lblError.Visible = True
                MensagemOff()
                Exit Sub
            Else
                Response.Write("<SCRIPT language='JavaScript'>")
                Response.Write("window.open('pdoc/temp/" & Arq & "');")
                Response.Write("</SCRIPT>")

               
            End If
        Else
            lblError.Text = "Não há relatório para o dia selecionado. " & Format(dtPDO.SelectedDate, "dd/MM/yyyy")
            lblError.Visible = True
            MensagemOff()
            Exit Sub
        End If

    End Sub
    Function VerificaArquivo(ByVal Arq As String) As Boolean
        Dim conPDP As New OnsConnection
        Dim cmd As New OnsCommand
        Dim dr As OnsDataReader
        'Dim oLocalMachine As Microsoft.Win32.RegistryKey
        'Dim strUsrBanco As String
        'Dim strPwdBanco As String
        'Dim strHost As String
        'Dim strServico As String
        'Dim strCLocale As String
        'Dim strDBLocale As String
        'Dim strDataSource As String
        'Dim strServer As String
        'oLocalMachine = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Ons\IU\PDP")
        'strUsrBanco = oLocalMachine.GetValue("Usuario")
        'strPwdBanco = oLocalMachine.GetValue("Senha")
        'strDataSource = oLocalMachine.GetValue("DataSource")
        'strServer = oLocalMachine.GetValue("Server")
        'strHost = oLocalMachine.GetValue("Host")
        'strServico = oLocalMachine.GetValue("Serviço")

        'conPDP.ConnectionString = "database=" & strDataSource.Trim() & " ;server=" & strServer.Trim() & ";host=" & strHost.Trim() & _
        '                                  ";uid=" & strUsrBanco.Trim() & ";password=" & strPwdBanco.Trim() & ";protocol=onsoctcp" & _
        '                                  ";service=" & strServico.Trim()
        conPDP.Open("rpdp")
        cmd.Connection = conPDP
        cmd.CommandText = "SELECT codstatu " & _
                          "FROM eventpdp " & _
                          "where datpdp = '" & Format(dtPDO.SelectedDate, "yyyyMMdd") & "' and codstatu = 65"
        dr = cmd.ExecuteReader()
        lblError.Text = ""
        If dr.Read = False Then
            lblError.Text = " Não há relatório para o dia selecionado. " & Format(dtPDO.SelectedDate, "dd/MM/yyyy")
            lblError.Visible = True
            MensagemOff()
            Return False
        End If
        dr.Close()
        conPDP.Close()
        Return True
    End Function
    Public Sub MensagemOn(ByVal Texto As String)
        Dim strScript As String
        Dim i As Integer
        strScript = "<script type=""text/javascript"">alert('" & Texto & "')</script>"
        If Not Page.IsStartupScriptRegistered("MyScript") Then
            Page.RegisterStartupScript("MyScript", strScript)
        End If
    End Sub
    Public Sub MensagemOff()
        Dim strScript As String
        Dim i As Integer
        strScript = "<script type=""text/javascript"">\n</script>"
        If Not Page.IsStartupScriptRegistered("MyScript") Then
            Page.RegisterStartupScript("MyScript", strScript)
        End If
    End Sub

    Private Sub cmdOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOk.Click
        Dim Arq As String
        Dim lnk As LinkButton
        Dim ArqCompletData As String

        'CRQ1343
        ArqCompletData = "-" & Format(dtPDO.SelectedDate, "dd-MM-yyyy") & ".doc"

        NomeArq(8) = "PDIC_" & Replace(Session("nomeEmp"), " ", "-") & ArqCompletData
        If Rblist.SelectedItem.Value = "ON" Then
            Arq = "PDIC_Completo" & ArqCompletData
            ArqExiste = True
        ElseIf Rblist.SelectedItem.Value = "N" Then
            Arq = "PDIC_COSR-NCO" & ArqCompletData
            ArqExiste = True
        ElseIf Rblist.SelectedItem.Value = "NE" Then
            Arq = "PDIC_COSR-NE" & ArqCompletData
            ArqExiste = True
        ElseIf Rblist.SelectedItem.Value = "S" Then
            Arq = "PDIC_COSR-S" & ArqCompletData
            ArqExiste = True
        ElseIf Rblist.SelectedItem.Value = "SE" Then
            Arq = "PDIC_COSR-SE" & ArqCompletData
            ArqExiste = True
        ElseIf Rblist.SelectedItem.Value = "AG" Then
            Arq = NomeArq(8)
            ArqExiste = True
        End If
        If ArqExiste Then
            MontaArqFTP(VerificaArquivo(Arq), Arq, "PROGRAMA DIÁRIO DE INTERVENÇÕES CONSOLIDADO - PDIc")
        End If
    End Sub
    Private Sub cmdOk1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOk1.Click
        Dim Arq As String
        Dim lnk As LinkButton
        Dim ArqCompletData As String

        'CRQ1343
        ArqCompletData = "-" & Format(dtPDO.SelectedDate, "dd-MM-yyyy") & ".doc"

        NomeArq(8) = "PDIC_" & Session("nomeEmp") & ArqCompletData
        If Rblist1.SelectedItem.Value = "ON" Then
            Arq = "PDIC_Completo" & ArqCompletData
            ArqExiste = True
        ElseIf Rblist1.SelectedItem.Value = "N" Then
            Arq = "PDIC_COSR-NCO" & ArqCompletData
            ArqExiste = True
        ElseIf Rblist1.SelectedItem.Value = "NE" Then
            Arq = "PDIC_COSR-NE" & ArqCompletData
            ArqExiste = True
        ElseIf Rblist1.SelectedItem.Value = "S" Then
            Arq = "PDIC_COSR-S" & ArqCompletData
            ArqExiste = True
        ElseIf Rblist1.SelectedItem.Value = "SE" Then
            Arq = "PDIC_COSR-SE" & ArqCompletData
            ArqExiste = True
        End If
        If ArqExiste Then
            MontaArqFTP(VerificaArquivo(Arq), Arq, "PROGRAMA DIÁRIO DE INTERVENÇÕES CONSOLIDADO - PDIc")
        End If
    End Sub
    Private Sub CmdVeOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdVeOk.Click
        Dim Arq As String
        Dim lnk As LinkButton

        'CRQ1343
        If RbListValElet.SelectedItem.Value = "VE" Then
            Arq = "ValidacaoEletrica" & "-" & Format(dtPDO.SelectedDate, "dd-MM-yyyy") & ".doc"
            ArqExiste = True
        ElseIf RbListValElet.SelectedItem.Value = "VEV" Then
            Arq = "ComentariosValidacaoEletrica" & "-" & Format(dtPDO.SelectedDate, "dd-MM-yyyy") & ".doc"
            ArqExiste = True
        End If
        If ArqExiste Then
            MontaArqFTP(VerificaArquivo(Arq), Arq, "PROGRAMA DIÁRIO DE INTERVENÇÕES CONSOLIDADO - PDIc")
        End If
    End Sub

    Private Sub dtPDO_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtPDO.SelectionChanged
        lblDtPDO.Text = "Data do PDO: " & Format(dtPDO.SelectedDate, "dd/MM/yyyy")
        lblError.Visible = False
        Session("dataID") = Format(dtPDO.SelectedDate, "dd/MM/yyyy")
    End Sub

    Private Sub lbIntervencoes1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbIntervencoes1.Click

        ArqExiste = VerificaArquivo("PDIC_Completo" & "-" & Format(dtPDO.SelectedDate, "dd-MM-yyyy") & ".doc")

        If ArqExiste Then
            If CosID <> "" Then
                Rblist1.Visible = True
                cmdOk1.Visible = True
                lblError.Text = ""
            Else
                Rblist.Visible = True
                cmdOk.Visible = True
                lblError.Text = ""
            End If
        End If
        CmdVeOk.Visible = False
        RbListValElet.Visible = False
    End Sub

    Private Sub lblValElet_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblValElet.Click
        ArqExiste = VerificaArquivo("PDIC_Completo" & "-" & Format(dtPDO.SelectedDate, "dd-MM-yyyy") & ".doc")

        If ArqExiste Then
            RbListValElet.Visible = True
            CmdVeOk.Visible = True
            lblError.Text = ""
        Else
            CmdVeOk.Visible = False
            RbListValElet.Visible = False
        End If

        If CosID <> "" Then
            Rblist1.Visible = False
            cmdOk1.Visible = False
        Else
            Rblist.Visible = False
            cmdOk.Visible = False
        End If
    End Sub

    Private Sub Rblist_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Rblist.SelectedIndexChanged
        If CosID <> "" Then
            Rblist.Items.Remove("AG")
        End If
    End Sub
End Class
