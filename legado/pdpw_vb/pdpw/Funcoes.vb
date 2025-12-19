'Classes base para manipulação de texto
Imports System.Collections.Generic
Imports System.Data.SqlClient
'Classes base para manipulação do ambiente corrente
Imports System.Environment
Imports System.Globalization
Imports System.IO
Imports System.Linq
Imports System.Net
Imports System.Text
Imports System.Web.SessionState
Imports DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing
Imports Newtonsoft.Json

Module Funcoes

    Private _util As New Util
    'Private logger As log4net.ILog = log4net.LogManager.GetLogger("PDPW")
    Public Const strMsgValidado As String = "A Programação está encerrada."
    Public Const strMsgNaoValidado As String = "A Programação está aberta."
    Public Const strMsgInicioLimiteEnvioDados As String = "Prazo para alteração de dados ainda não iniciado."
    Public Const strMsgLimiteParaUploadArquivo As String = "Fora do prazo para upload de arquivo."
    Public strMsgLimiteEnvioDados As String = "Prazo esgotado para alteração de dados."

    Public Const strMsgInicioLimiteEnvioDadosAux As String = "Prazo para envio de dados ainda não iniciado."
    Public Const strMsgLimiteEnvioDadosAux As String = "Prazo esgotado para envio de dados."

    Public blnIncluir As Boolean = False

    Public Const K_Const_strIdMarco_AberturaDia As String = "1"
    Public Const K_Const_strIdMarco_ReberturaDia As String = "2"
    Public Const K_Const_strIdMarco_EnvioDados As String = "3"
    Public Const K_Const_strIdMarco_ValidacaoEletrica As String = "4"
    Public Const K_Const_strIdMarco_EnvioPRE As String = "5"

    Private provider As NumberFormatInfo = New NumberFormatInfo

    Public Function VerificaEventos(ByVal strData As String, ByVal strCodEmpresa As String) As Boolean
        Dim Conn As SqlConnection = New SqlConnection
        Dim Cmd As SqlCommand = New SqlCommand
        Cmd.Connection = Conn
        Conn.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
        Conn.Open()
        'Verifica se exitem os eventos 7, 8 e 9 para que seja gerado o arquivo texto
        Cmd.CommandText = "SELECT codstatu " &
                          "FROM eventpdp " &
                          "WHERE datpdp = '" & strData & "' " &
                          "AND cmpevent = '" & strCodEmpresa & "' " &
                          "AND (codstatu = '7' " &
                          "OR codstatu = '8' " &
                          "OR codstatu = '9') " &
                          "GROUP BY codstatu"
        Dim rsEvento As SqlDataReader = Cmd.ExecuteReader
        Dim intQtdStatus As Integer = 0
        Do While rsEvento.Read
            intQtdStatus = intQtdStatus + 1
        Loop
        rsEvento.Close()
        Cmd.Connection.Close()
        Conn.Close()
        If intQtdStatus = 3 Then
            VerificaEventos = True
        Else
            VerificaEventos = False
        End If

    End Function

    Public Function Get_DataPDP_Format(ByVal data As DateTime) As String
        Return data.ToString("yyyyMMdd")
    End Function

    ''' <summary>
    ''' Obtém a conversão para DateTime da Data PDP. Aceita entrada nos formatos: "yyyyMMdd" e "yyyy-MM-dd" e "yyyy/MM/dd"
    ''' </summary>
    ''' <param name="dataPDP">Data PDP</param>
    ''' <returns>Data PDP no formato DateTime</returns>
    Public Function Get_DataPDP_DateTime(dataPDP As String) As DateTime

        dataPDP = dataPDP.Replace("-", "").Replace("/", "")

        Dim ano As String = dataPDP.Substring(0, 4)
        Dim mes As String = dataPDP.Substring(4, 2)
        Dim dia As String = dataPDP.Substring(6, 2)

        Return DateTime.Parse(dia + "/" + mes + "/" + ano + " 00:00:00")
    End Function


    Public Function GetSemanaPMO(data As DateTime?, ano As Integer, mes As Integer?) As List(Of SemanaPMO)

        Try
            Dim data_semana As String = data.Value.ToString("yyyy-MM-dd")

            Dim ret As List(Of SemanaPMO) =
                New SemanaPMOBusiness().ListarTodas().
                    Where(Function(s) data_semana >= s.DataInicio And data_semana <= s.DataFim).
                    Select(Of SemanaPMO)(Function(s) New SemanaPMO(s.Ano, s.Mes, s.Semana, s.DataInicio, s.DataFim, s.IdSemanapmo, s.IdAnomes)).
                    ToList()

            Dim semanaPMO As SemanaPMO
            For Each semanaPMO In ret

                Dim datasSemanaPmo As List(Of DateTime) = New List(Of Date)

                Dim dataAtual As DateTime = semanaPMO.DataInicio

                While (dataAtual <= semanaPMO.DataFim)

                    datasSemanaPmo.Add(dataAtual)
                    dataAtual = dataAtual.AddDays(1)

                End While

                ret(ret.IndexOf(semanaPMO)).Datas_SemanaPmo = datasSemanaPmo

            Next

            Return ret

        Catch ex As Exception
            Throw ex
        End Try

        Return Nothing
    End Function

    Public Function GetProximaSemanaPMO(data As DateTime?, ano As Integer, mes As Integer?) As List(Of SemanaPMO)
        Dim semanaCorrente As List(Of SemanaPMO) = GetSemanaPMO(data, ano, mes)
        Dim ultimoDia As DateTime = semanaCorrente(0).Datas_SemanaPmo(semanaCorrente(0).Datas_SemanaPmo.Count - 1)
        Dim semanaPosterior As List(Of SemanaPMO) = GetSemanaPMO(ultimoDia.AddDays(1), Nothing, Nothing)
        Return semanaPosterior
    End Function

    Public Function ListarDiasAbertos(diaInicial As DateTime, diaFinal As DateTime) As List(Of DateTime)
        Dim retorno As List(Of DateTime) = New List(Of Date)

        Dim Conn As SqlConnection = New SqlConnection
        Dim Cmd As SqlCommand = New SqlCommand

        Conn.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
        Cmd.Connection = Conn
        Conn.Open()

        Cmd.CommandText = "SELECT datpdp " &
                          "  FROM pdp " &
                          " WHERE datpdp between '" & diaInicial.ToString("yyyyMMdd") & "' and '" & diaFinal.ToString("yyyyMMdd") & "' " &
                          " ORDER BY datpdp"

        Dim rsPdp As SqlDataReader = Cmd.ExecuteReader

        Do While rsPdp.Read
            retorno.Add(New DateTime(Integer.Parse(Mid(rsPdp("datpdp"), 1, 4)), Integer.Parse(Mid(rsPdp("datpdp"), 5, 2)), Integer.Parse(Mid(rsPdp("datpdp"), 7, 2)), 0, 0, 0, 0))
        Loop

        rsPdp.Close()
        Cmd.Connection.Close()
        Conn.Close()

        Return retorno
    End Function

    Public Function DiaAberto(dia As DateTime) As Boolean
        Dim retorno As Boolean = False

        Dim Conn As SqlConnection = New SqlConnection
        Dim Cmd As SqlCommand = New SqlCommand
        Conn.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
        Cmd.Connection = Conn
        Conn.Open()

        Cmd.CommandText = "SELECT datpdp " &
                          "  FROM pdp " &
                          " WHERE datpdp = '" & dia.ToString("yyyyMMdd") & "'"

        Dim rsPdp As SqlDataReader = Cmd.ExecuteReader

        retorno = rsPdp.Read()

        rsPdp.Close()
        Cmd.Connection.Close()
        Conn.Close()

        Return retorno
    End Function

    Public Function AndUsina(codUsina As String, Optional aliasUsina As String = "u") As String
        Dim retorno As String = ""

        If (codUsina <> Nothing) And (Trim(codUsina) <> "") Then
            retorno = " and " + aliasUsina + ".codusina = '" + codUsina + "' "
        End If

        Return retorno
    End Function

    Public Function HoraParaInt(ByVal intervalo As String) As Int32
        Select Case intervalo
            Case "00:00 - 00:30"
                Return 1
            Case "00:30 - 01:00"
                Return 2
            Case "01:00 - 01:30"
                Return 3
            Case "01:30 - 02:00"
                Return 4
            Case "02:00 - 02:30"
                Return 5
            Case "02:30 - 03:00"
                Return 6
            Case "03:00 - 03:30"
                Return 7
            Case "03:30 - 04:00"
                Return 8
            Case "04:00 - 04:30"
                Return 9
            Case "04:30 - 05:00"
                Return 10
            Case "05:00 - 05:30"
                Return 11
            Case "05:30 - 06:00"
                Return 12
            Case "06:00 - 06:30"
                Return 13
            Case "06:30 - 07:00"
                Return 14
            Case "07:00 - 07:30"
                Return 15
            Case "07:30 - 08:00"
                Return 16
            Case "08:00 - 08:30"
                Return 17
            Case "08:30 - 09:00"
                Return 18
            Case "09:00 - 09:30"
                Return 19
            Case "09:30 - 10:00"
                Return 20
            Case "10:00 - 10:30"
                Return 21
            Case "10:30 - 11:00"
                Return 22
            Case "11:00 - 11:30"
                Return 23
            Case "11:30 - 12:00"
                Return 24
            Case "12:00 - 12:30"
                Return 25
            Case "12:30 - 13:00"
                Return 26
            Case "13:00 - 13:30"
                Return 27
            Case "13:30 - 14:00"
                Return 28
            Case "14:00 - 14:30"
                Return 29
            Case "14:30 - 15:00"
                Return 30
            Case "15:00 - 15:30"
                Return 31
            Case "15:30 - 16:00"
                Return 32
            Case "16:00 - 16:30"
                Return 33
            Case "16:30 - 17:00"
                Return 34
            Case "17:00 - 17:30"
                Return 35
            Case "17:30 - 18:00"
                Return 36
            Case "18:00 - 18:30"
                Return 37
            Case "18:30 - 19:00"
                Return 38
            Case "19:00 - 19:30"
                Return 39
            Case "19:30 - 20:00"
                Return 40
            Case "20:00 - 20:30"
                Return 41
            Case "20:30 - 21:00"
                Return 42
            Case "21:00 - 21:30"
                Return 43
            Case "21:30 - 22:00"
                Return 44
            Case "22:00 - 22:30"
                Return 45
            Case "22:30 - 23:00"
                Return 46
            Case "23:00 - 23:30"
                Return 47
            Case "23:30 - 23:59"
                Return 48
            Case Else
                Return "Intervalo Inválido"
        End Select
    End Function

    Public Function IntParaHora(ByVal intervalo As Integer) As String
        Select Case intervalo
            Case 1
                Return "00:00 - 00:30"
            Case 2
                Return "00:30 - 01:00"
            Case 3
                Return "01:00 - 01:30"
            Case 4
                Return "01:30 - 02:00"
            Case 5
                Return "02:00 - 02:30"
            Case 6
                Return "02:30 - 03:00"
            Case 7
                Return "03:00 - 03:30"
            Case 8
                Return "03:30 - 04:00"
            Case 9
                Return "04:00 - 04:30"
            Case 10
                Return "04:30 - 05:00"
            Case 11
                Return "05:00 - 05:30"
            Case 12
                Return "05:30 - 06:00"
            Case 13
                Return "06:00 - 06:30"
            Case 14
                Return "06:30 - 07:00"
            Case 15
                Return "07:00 - 07:30"
            Case 16
                Return "07:30 - 08:00"
            Case 17
                Return "08:00 - 08:30"
            Case 18
                Return "08:30 - 09:00"
            Case 19
                Return "09:00 - 09:30"
            Case 20
                Return "09:30 - 10:00"
            Case 21
                Return "10:00 - 10:30"
            Case 22
                Return "10:30 - 11:00"
            Case 23
                Return "11:00 - 11:30"
            Case 24
                Return "11:30 - 12:00"
            Case 25
                Return "12:00 - 12:30"
            Case 26
                Return "12:30 - 13:00"
            Case 27
                Return "13:00 - 13:30"
            Case 28
                Return "13:30 - 14:00"
            Case 29
                Return "14:00 - 14:30"
            Case 30
                Return "14:30 - 15:00"
            Case 31
                Return "15:00 - 15:30"
            Case 32
                Return "15:30 - 16:00"
            Case 33
                Return "16:00 - 16:30"
            Case 34
                Return "16:30 - 17:00"
            Case 35
                Return "17:00 - 17:30"
            Case 36
                Return "17:30 - 18:00"
            Case 37
                Return "18:00 - 18:30"
            Case 38
                Return "18:30 - 19:00"
            Case 39
                Return "19:00 - 19:30"
            Case 40
                Return "19:30 - 20:00"
            Case 41
                Return "20:00 - 20:30"
            Case 42
                Return "20:30 - 21:00"
            Case 43
                Return "21:00 - 21:30"
            Case 44
                Return "21:30 - 22:00"
            Case 45
                Return "22:00 - 22:30"
            Case 46
                Return "22:30 - 23:00"
            Case 47
                Return "23:00 - 23:30"
            Case 48
                Return "23:30 - 23:59"
            Case Else
                Return "Intervalo Inválido"
        End Select
    End Function

    Public Function GravaArquivoTexto(dadodGeracao As GeracaoArquivo, ByRef bRetorno As Boolean) As Boolean

        Dim strCodEmpresa As String = dadodGeracao.strCodEmpresa
        Dim strData As String = dadodGeracao.strData
        Dim strArq As String = dadodGeracao.strArq
        Dim blnTipoEnvio As String = dadodGeracao.blnTipoEnvio
        Dim bFtp As Boolean = dadodGeracao.bFtp
        Dim strOpcao As String = dadodGeracao.strOpcao
        Dim strDataHora As String = dadodGeracao.strDataHora
        Dim strArqDestino As String = dadodGeracao.strArqDestino
        Dim Cmd As SqlCommand = dadodGeracao.Cmd
        Dim strPerfil As String = dadodGeracao.strPerfil
        Dim codUsina As String = dadodGeracao.codUsina

        ' blnTipoEnvio = 0 Área de Transferência
        ' blnTipoEnvio = 1 Dados Enviados
        ' blnTipoEnvio = 2 Dados Consolidados

        ' bFtp = Se o arquivo está sendo criado pela pela rotina de envio (true) ou pela rotina de gerar arquivo (false)
        '        de acordo com esse flag as empresas origem e destino deverão ser trocadas e deve
        '        ser feito um ftp para a área de entrada do pdp.

        ' strOpcao = String contendo 0/1 para saber se gera ou não com o tipo do dado correspondente a posicao do flag
        ' Ex: Geração, Carga, Intercâmbio
        ' strOpcao = '101' (Gera arq. com Geração e Intercâmbio)
        ' strOpcao = '110' (Gera arq. com Geração e Carga)

        Dim strTexto As Char
        Dim intArq, intI, intPedaco As Integer
        Dim strFile, strCampo, strArquivo, strLinha, strTemp, strHorario, strDataAtual As String
        Dim strCodUsina, strCodInter As String
        'Dim Conn As SqlConnection = New SqlConnection
        'Dim Cmd As SqlCommand = New SqlCommand
        Dim strComp As String
        Dim util As Util = New Util()

        'Cmd.Connection = Conn
        'Conn.Open("pdp")

        'abrir o arquivo

        strArq = util.DiretorioEnviodDeDados("Download")

        Dim strArqDESSEM As String = ""
        Dim strCabecalhoDESSEM As String = ""

        If Trim(blnTipoEnvio) = "4" Then 'Não altera o nome nem o cabeçalho para tipo = 5
            strArqDESSEM = "-dessem"
            strCabecalhoDESSEM = " D"
        End If

        If Trim(blnTipoEnvio) = "4" Or Trim(blnTipoEnvio) = "5" Then
            strArq = util.DiretorioEnviodDeDados("Download\\DESSEM")
        End If

        If strPerfil.Trim = "ESTUDO_PDP" Then
            strArq = util.DiretorioEnviodDeDados("Estudo")
            strArq &= "\" & strCodEmpresa.Trim

        Else
            If bFtp Then
                strArq &= "\" & strCodEmpresa.Trim
            Else
                If strArqDestino = "" Then
                    strArq &= "\ON-" & strCodEmpresa.Trim
                Else
                    strArq &= "\" & strCodEmpresa.Trim & "-" & strArqDestino
                End If
            End If

            strFile = Dir(strArq & "*")

            Do While strFile <> ""
                If CDate(strFile.Substring(6, 4) & "-" & strFile.Substring(10, 2) & "-" & strFile.Substring(12, 2)) < Now.AddDays(-7) Then
                    If bFtp Then
                        Kill(strArq.Substring(0, strArq.Length - 2) & strFile)
                    Else
                        Kill(strArq.Substring(0, strArq.Length - 5) & strFile)
                    End If
                End If
                strFile = Dir()
            Loop
        End If

        strHorario = IIf(Mid(strDataHora, 9, 6) = "", DateTime.Now.ToString("HHmmss"), Mid(strDataHora, 9, 6))
        strDataAtual = Mid(strDataHora, 1, 8)


        If bFtp Then
            strArq &= "-ON " & strDataAtual & "-" & strHorario & strArqDESSEM & ".PDP"
        Else
            strArq &= " " & strDataAtual & "-" & strHorario & strArqDESSEM & ".PDP"
        End If

        intArq = FreeFile()
        FileOpen(intArq, strArq, OpenMode.Output, OpenShare.Default)

        GravaArquivoTexto = True

        Dim isDESSEM As Boolean = False

        Try
            'Cabeçalho do arquivo
            If bFtp Then
                strLinha = strDataAtual & strHorario & " " & strData & " " &
                        Trim(strCodEmpresa) & " ON " & "30" & " " & "00:30"
            Else
                If strArqDestino = "" Then
                    strLinha = strDataAtual & strHorario & " " & strData & " " &
                               "ON " & Trim(strCodEmpresa) & " 30" & " " & "00:30"
                Else
                    strLinha = strDataAtual & strHorario & " " & strData & " " &
                               Trim(strCodEmpresa) & " " & strArqDestino & " 30" & " " & "00:30"
                End If
            End If

            strLinha += strCabecalhoDESSEM 'Caso DESSEM Modelo terá um "D" ao final - Não será feito para DESSEM Consolidado

            PrintLine(intArq, strLinha)

            If Trim(blnTipoEnvio) = "0" Then
                strCampo = "tran"
                strArquivo = "emp"
            ElseIf Trim(blnTipoEnvio) = "1" Then
                strCampo = "emp"
                strArquivo = "temp"
            ElseIf Trim(blnTipoEnvio) = "2" Then
                strCampo = "pro"
                strArquivo = ""
            ElseIf Trim(blnTipoEnvio) = "4" Then 'DESSEM Modelo
                strCampo = "pre"
                strArquivo = ""
                isDESSEM = True
            ElseIf Trim(blnTipoEnvio) = "5" Then 'DESSEM Consolidado
                strCampo = "sup"
                strArquivo = ""
                isDESSEM = True
            Else
                'Envio para Estudo GMC1
                'Envio de Carga, Intercâmbio e Geração
                strCampo = ""
                strArquivo = ""
            End If

            'GERAÇÃO
            RegistrarLog("GERAÇÃO")
            If strOpcao.Substring(0, 1) = "1" Then

                If blnTipoEnvio = "3" Then
                    Cmd.CommandText = "SELECT g.codusina, g.valdespa, g.intdespa " &
                                     "FROM usina u, tb_despaestudo g " &
                                     "WHERE u.codempre = '" & strCodEmpresa & "' " &
                                     "AND u.codusina = g.codusina " & AndUsina(codUsina) &
                                     "AND g.datpdp = '" & strData & "' " &
                                     "ORDER BY g.codusina, g.intdespa"
                    RegistrarLog(Cmd.CommandText)
                Else
                    Cmd.CommandText = "SELECT g.codusina, g.valdespa" & strCampo & ", g.intdespa " &
                                     "FROM usina u, despa g " &
                                     "WHERE u.codempre = '" & strCodEmpresa & "' " &
                                     "AND u.codusina = g.codusina " & AndUsina(codUsina) &
                                     "AND g.datpdp = '" & strData & "' " &
                                     "ORDER BY g.codusina, g.intdespa"
                    RegistrarLog(Cmd.CommandText)
                End If

                Dim rsGeracao As SqlDataReader = Cmd.ExecuteReader

                strCodUsina = ""
                Do While rsGeracao.Read
                    If strCodUsina <> Trim(rsGeracao("codusina")) Then
                        If strCodUsina <> "" Then
                            PrintLine(intArq, strLinha)
                        End If
                        strCodUsina = Trim(rsGeracao("codusina"))
                        strLinha = "GER " & Left(strCodUsina & Space(12), 12)
                    End If
                    'Foi alterado por conta de Geração Negativa
                    'strLinha = strLinha & " " & Right("00000" & Trim(Str(IIf(Not IsDBNull(rsGeracao("valdespa" & strCampo)), rsGeracao("valdespa" & strCampo), 0))), 5)
                    strTemp = Trim(Str(IIf(Not IsDBNull(rsGeracao("valdespa" & strCampo)), rsGeracao("valdespa" & strCampo), 0)))

                    If Left(strTemp, 1) = "-" Then
                        strLinha = strLinha & " -" & Right("0000" & Mid(strTemp, 2), 4)
                    Else
                        strLinha = strLinha & " " & Right("00000" & strTemp, 5)
                    End If
                Loop
                If Left(strLinha, 3) = "GER" Then
                    PrintLine(intArq, strLinha)
                End If
                rsGeracao.Close()
            End If
            RegistrarLog(strLinha)

            'CARGA
            RegistrarLog("CARGA")
            If strOpcao.Substring(1, 1) = "1" Then
                If blnTipoEnvio = "3" Then
                    Cmd.CommandText = "SELECT valcarga, intcarga " &
                                     "FROM tb_cargaestudo " &
                                     "WHERE codempre = '" & strCodEmpresa & "' " &
                                     "AND datpdp = '" & strData & "' " &
                                     "ORDER BY intcarga"
                    RegistrarLog(Cmd.CommandText)
                Else
                    Cmd.CommandText = "SELECT valcarga" & strCampo & ", intcarga " &
                                   "FROM carga " &
                                   "WHERE codempre = '" & strCodEmpresa & "' " &
                                   "AND datpdp = '" & strData & "' " &
                                   "ORDER BY intcarga"
                    RegistrarLog(Cmd.CommandText)
                End If

                Dim rsCarga As SqlDataReader = Cmd.ExecuteReader
                strLinha = "CAR"
                Do While rsCarga.Read
                    strLinha = strLinha & " " & Right("00000" & Trim(Str(IIf(Not IsDBNull(rsCarga("valcarga" & strCampo)), rsCarga("valcarga" & strCampo), 0))), 5)
                Loop
                If Len(strLinha) > 3 Then
                    strLinha = strLinha & " " & strCodEmpresa
                    PrintLine(intArq, strLinha)
                End If
                rsCarga.Close()
            End If
            RegistrarLog(strLinha)

            'INTERCÂMBIO
            RegistrarLog("INTERCÂMBIO")
            If strOpcao.Substring(2, 1) = "1" Then
                If blnTipoEnvio = "3" Then
                    Cmd.CommandText = "SELECT codemprepara, codcontamodal, intinter, valinter " &
                                   "FROM tb_interestudo " &
                                   "WHERE datpdp = '" & strData & "' " &
                                   "AND codemprede = '" & strCodEmpresa & "' " &
                                   "ORDER BY codemprepara, codcontamodal, intinter"
                    RegistrarLog(Cmd.CommandText)

                Else
                    Cmd.CommandText = "SELECT codemprepara, codcontade, codcontapara, codcontamodal, intinter, valinter" & strCampo & " " &
                                   "FROM inter " &
                                   "WHERE datpdp = '" & strData & "' " &
                                   "AND codemprede = '" & strCodEmpresa & "' " &
                                   "ORDER BY codemprepara, codcontade, codcontapara, codcontamodal, intinter"
                    RegistrarLog(Cmd.CommandText)
                End If

                Dim rsIntercambio As SqlDataReader = Cmd.ExecuteReader
                strCodInter = ""
                Do While rsIntercambio.Read
                    If blnTipoEnvio = "3" Then
                        If strCodInter <> Trim(rsIntercambio("codemprepara")) &
                                        Trim(rsIntercambio("codcontamodal")) Then
                            If strCodInter <> "" Then
                                PrintLine(intArq, strLinha)
                            End If
                            strCodInter = Trim(rsIntercambio("codemprepara")) &
                                        Trim(rsIntercambio("codcontamodal"))
                            strLinha = "INT " & Trim(strCodEmpresa) & "   " &
                                                Trim(rsIntercambio("codemprepara")) & "   " &
                                                Trim(rsIntercambio("codcontamodal"))
                            RegistrarLog(Cmd.CommandText)
                        End If

                    Else
                        If strCodInter <> Trim(rsIntercambio("codemprepara")) &
                                            Trim(rsIntercambio("codcontade")) &
                                            Trim(rsIntercambio("codcontapara")) &
                                            Trim(rsIntercambio("codcontamodal")) Then
                            If strCodInter <> "" Then
                                PrintLine(intArq, strLinha)
                            End If
                            strCodInter = Trim(rsIntercambio("codemprepara")) &
                                        Trim(rsIntercambio("codcontade")) &
                                        Trim(rsIntercambio("codcontapara")) &
                                        Trim(rsIntercambio("codcontamodal"))
                            strLinha = "INT " & Trim(strCodEmpresa) & "   " &
                                                Trim(rsIntercambio("codemprepara")) & "   " &
                                                Trim(rsIntercambio("codcontade")) & "   " &
                                                Trim(rsIntercambio("codcontapara")) & "   " &
                                                Trim(rsIntercambio("codcontamodal"))
                            RegistrarLog(Cmd.CommandText)
                        End If
                    End If

                    strTemp = Trim(Str(IIf(Not IsDBNull(rsIntercambio("valinter" & strCampo)), rsIntercambio("valinter" & strCampo), 0)))

                    If Left(strTemp, 1) = "-" Then
                        strLinha = strLinha & " -" & Right("00000" & Mid(strTemp, 2), 5)
                    Else
                        strLinha = strLinha & " " & Right("000000" & strTemp, 6)
                    End If
                Loop
                If Left(strLinha, 3) = "INT" Then
                    PrintLine(intArq, strLinha)
                End If
                rsIntercambio.Close()
            End If
            RegistrarLog(strLinha)

            'VAZÃO
            RegistrarLog("VAZÃO")
            If strOpcao.Substring(3, 1) = "1" Then
                If strCampo = "tran" Then
                    strComp = strCampo
                Else
                    strComp = ""
                End If
                '-- CRQ2345 (15/08/2012)

                Cmd.CommandText = "SELECT v.codusina, v.valturb" & strComp & " AS valturb, v.valvert" & strComp & " AS valvert, v.valaflu" & strComp & " AS valaflu, v.valtransf" & strComp & " as valtransf, c.cotaini" & strComp & " AS cotaini, c.cotafim" & strComp & " AS cotafim, c.outrasestruturas" & strComp & " AS outrasestr, c.comentariopdf" & strComp & " AS comentpdf " &
                                  "              FROM vazao v  " &
                                  "        INNER JOIN usina u on  u.codusina = v.codusina " &
                                  "   FULL OUTER JOIN cota c  ON  u.codusina = c.codusina  " &
                                  "  WHERE u.codempre = '" & strCodEmpresa & "' " &
                                  "    AND u.codusina = v.codusina " &
                                  "    AND u.codusina = c.codusina " & AndUsina(codUsina) &
                                  "    AND v.datpdp = '" & strData & "' " &
                                 "     AND c.datpdp = '" & strData & "' " &
                                 "ORDER BY v.codusina"


                RegistrarLog(Cmd.CommandText)
                Dim rsVazao As SqlDataReader = Cmd.ExecuteReader

                Do While rsVazao.Read

                    strLinha = "VAZ " & Left(Trim(rsVazao("codusina")) & Space(13), 13) &
                                Right("0000000" & Trim(Str(IIf(IsDBNull(rsVazao("valturb")), 0, rsVazao("valturb")))), 7) & " " &
                                Right("0000000" & Trim(Str(IIf(IsDBNull(rsVazao("valvert")), 0, rsVazao("valvert")))), 7) & " " &
                                Right("0000000" & Trim(Str(IIf(IsDBNull(rsVazao("valaflu")), 0, rsVazao("valaflu")))), 7) & " " &
                                Right("0000000" & Trim(Str(IIf(IsDBNull(rsVazao("cotaini")), 0, rsVazao("cotaini"))).Replace(",", "").Replace(".", "")), 7) & " " &
                                Right("0000000" & Trim(Str(IIf(IsDBNull(rsVazao("cotafim")), 0, rsVazao("cotafim"))).Replace(",", "").Replace(".", "")), 7) & " " &
                                Right("0000000" & Trim(Str(IIf(IsDBNull(rsVazao("outrasestr")), 0, rsVazao("outrasestr"))).Replace(",", "").Replace(".", "")), 7) & " " &
                                Right("0000000" & Trim(Str(IIf(IsDBNull(rsVazao("valtransf")), 0, rsVazao("valtransf")))), 7) & " " &
                                Left(Trim(IIf(IsDBNull(rsVazao("comentpdf")), "", rsVazao("comentpdf"))) & Space(256), 256)
                    PrintLine(intArq, strLinha)

                Loop
                rsVazao.Close()
            End If
            RegistrarLog(strLinha)

            'INFLEXIBILIDADE
            RegistrarLog("INFLEXIBILIDADE")
            If strOpcao.Substring(4, 1) = "1" Then
                Cmd.CommandText = "SELECT g.codusina, g.valflexi" & strCampo & ", g.intflexi " &
                                 "FROM usina u, inflexibilidade g " &
                                 "WHERE u.codempre = '" & strCodEmpresa & "' " &
                                 "AND u.codusina = g.codusina " & AndUsina(codUsina) &
                                 "AND g.datpdp = '" & strData & "' " &
                                 "ORDER BY g.codusina, g.intflexi"
                RegistrarLog(Cmd.CommandText)
                Dim rsInflexibilidade As SqlDataReader = Cmd.ExecuteReader
                strCodUsina = ""
                Do While rsInflexibilidade.Read
                    If strCodUsina <> Trim(rsInflexibilidade("codusina")) Then
                        If strCodUsina <> "" Then
                            PrintLine(intArq, strLinha)
                        End If
                        strCodUsina = Trim(rsInflexibilidade("codusina"))
                        strLinha = "IFX " & Left(strCodUsina & Space(12), 12)
                    End If
                    strLinha = strLinha & " " & Right("00000" & Trim(Str(IIf(Not IsDBNull(rsInflexibilidade("valflexi" & strCampo)), rsInflexibilidade("valflexi" & strCampo), 0))), 5)
                Loop
                If Left(strLinha, 3) = "IFX" Then
                    PrintLine(intArq, strLinha)
                End If
                rsInflexibilidade.Close()
            End If
            RegistrarLog(strLinha)

            'RAZÃO ENERGÉTICA
            RegistrarLog("RAZÃO ENERGÉTICA")
            If strOpcao.Substring(5, 1) = "1" Then
                Cmd.CommandText = "SELECT g.codusina, g.valrazener" & strCampo & ", g.intrazener " &
                                 "FROM usina u, razaoener g " &
                                 "WHERE u.codempre = '" & strCodEmpresa & "' " &
                                 "AND u.codusina =  g.codusina " & AndUsina(codUsina) &
                                 "AND g.datpdp = '" & strData & "' " &
                                 "ORDER BY g.codusina, g.intrazener"
                RegistrarLog(Cmd.CommandText)
                Dim rsEnergetica As SqlDataReader = Cmd.ExecuteReader
                strCodUsina = ""
                Do While rsEnergetica.Read
                    If strCodUsina <> Trim(rsEnergetica("codusina")) Then
                        If strCodUsina <> "" Then
                            PrintLine(intArq, strLinha)
                        End If
                        strCodUsina = Trim(rsEnergetica("codusina"))
                        strLinha = "ZEN " & Left(strCodUsina & Space(12), 12)
                    End If
                    strLinha = strLinha & " " & Right("00000" & Trim(Str(IIf(Not IsDBNull(rsEnergetica("valrazener" & strCampo)), rsEnergetica("valrazener" & strCampo), 0))), 5)
                Loop
                If Left(strLinha, 3) = "ZEN" Then
                    PrintLine(intArq, strLinha)
                End If
                rsEnergetica.Close()
            End If
            RegistrarLog(strLinha)

            'RAZÃO ELÉTRICA
            RegistrarLog("RAZÃO ELÉTRICA")
            If strOpcao.Substring(6, 1) = "1" Then
                Cmd.CommandText = "SELECT g.codusina, g.valrazelet" & strCampo & ", g.intrazelet " &
                                 "FROM usina u, razaoelet g " &
                                 "WHERE u.codempre = '" & strCodEmpresa & "' " &
                                 "AND u.codusina =  g.codusina " & AndUsina(codUsina) &
                                 "AND g.datpdp = '" & strData & "' " &
                                 "ORDER BY g.codusina, g.intrazelet"
                RegistrarLog(Cmd.CommandText)
                Dim rsEletrica As SqlDataReader = Cmd.ExecuteReader
                strCodUsina = ""
                Do While rsEletrica.Read
                    If strCodUsina <> Trim(rsEletrica("codusina")) Then
                        If strCodUsina <> "" Then
                            PrintLine(intArq, strLinha)
                        End If
                        strCodUsina = Trim(rsEletrica("codusina"))
                        strLinha = "ZEL " & Left(strCodUsina & Space(12), 12)
                    End If
                    strLinha = strLinha & " " & Right("00000" & Trim(Str(IIf(Not IsDBNull(rsEletrica("valrazelet" & strCampo)), rsEletrica("valrazelet" & strCampo), 0))), 5)
                Loop
                If Left(strLinha, 3) = "ZEL" Then
                    PrintLine(intArq, strLinha)
                End If
                rsEletrica.Close()
            End If
            RegistrarLog(strLinha)

            'RESTRIÇÃO DE USINAS
            RegistrarLog("RESTRIÇÃO DE USINAS")
            '-- CRQ6867 - 23/09/2013 - Tb_MotivoRestricao
            If strOpcao.Substring(7, 1) = "1" Then
                '## Em 21/06/2006 passou a selecionar pela data do pdp 
                '## e não mais pelo início e término da restrição na área
                '## da empresa e temporária
                Cmd.CommandText = "SELECT r.codrestr, r.codusina, r.datinirestr, r.intinirestr, r.datfimrestr, " &
                                 "r.intfimrestr, r.valrestr, r.id_motivorestricao, isNull(r.obsrestr,'') as obsrestr " &
                                 "FROM usina u," &
                                 IIf(strArquivo = "temp", strArquivo & "restrusina", "restrusina" & strArquivo) & " r " &
                                 "WHERE u.codempre = '" & strCodEmpresa & "' " &
                                 "AND u.codusina = r.codusina " & AndUsina(codUsina)
                If strArquivo <> "" Then
                    Cmd.CommandText &= "AND r.datpdp = '" & strData & "' "
                Else
                    Cmd.CommandText &= "AND r.datinirestr <= '" & strData & "' " &
                                       "AND r.datfimrestr >= '" & strData & "' "

                End If
                Cmd.CommandText &= "ORDER BY r.codusina"
                RegistrarLog(Cmd.CommandText)
                Dim rsRestricaoUS As SqlDataReader = Cmd.ExecuteReader
                Do While rsRestricaoUS.Read

                    ' Se não tem Observação gera só "RES", se tem gera "RES" e "REO"
                    strLinha = "RES " & Left(Trim(rsRestricaoUS("codusina")) & Space(26), 26) &
                                                        rsRestricaoUS("datinirestr") & " " & Right("00" & rsRestricaoUS("intinirestr"), 2) & " " &
                                                        rsRestricaoUS("datfimrestr") & " " & Right("00" & rsRestricaoUS("intfimrestr"), 2) & " " &
                                                        Right("00000" & Trim(Str(IIf(Not IsDBNull(rsRestricaoUS("valrestr")), rsRestricaoUS("valrestr"), 0))), 5) & " " &
                                                        Right("000" & Trim(Str(IIf(Not IsDBNull(rsRestricaoUS("id_motivorestricao")), rsRestricaoUS("id_motivorestricao"), 0))), 3) & " " &
                                                        Right("000000" & Trim(Str(IIf(Not IsDBNull(rsRestricaoUS("codrestr")), rsRestricaoUS("codrestr"), 0))), 6)
                    PrintLine(intArq, strLinha)

                    If Trim(rsRestricaoUS("obsrestr")) <> "" Then
                        strLinha = "REO " & Left(Trim(rsRestricaoUS("codusina")) & Space(26), 26) &
                                                                                rsRestricaoUS("datinirestr") & " " & Right("00" & rsRestricaoUS("intinirestr"), 2) & " " &
                                                                                rsRestricaoUS("datfimrestr") & " " & Right("00" & rsRestricaoUS("intfimrestr"), 2) & " " &
                                                                                Right("00000" & Trim(Str(IIf(Not IsDBNull(rsRestricaoUS("valrestr")), rsRestricaoUS("valrestr"), 0))), 5) & " " &
                                                                                Right("000" & Trim(Str(IIf(Not IsDBNull(rsRestricaoUS("id_motivorestricao")), rsRestricaoUS("id_motivorestricao"), 0))), 3) & " " &
                                                                                Right("000000" & Trim(Str(IIf(Not IsDBNull(rsRestricaoUS("codrestr")), rsRestricaoUS("codrestr"), 0))), 6) & " " &
                                                                                Left(Trim(rsRestricaoUS("obsrestr")) & Space(255), 255)
                        PrintLine(intArq, strLinha)
                    End If
                Loop
                rsRestricaoUS.Close()
            End If
            RegistrarLog(strLinha)

            'RESTRIÇÃO DE UNIDADES GERADORAS
            RegistrarLog("RESTRIÇÃO DE UNIDADES GERADORAS")
            '-- CRQ6867 - 23/09/2013 - Tb_MotivoRestricao
            If strOpcao.Substring(7, 1) = "1" Then
                '## Em 21/06/2006 passou a selecionar pela data do pdp 
                '## e não mais pelo início e término da restrição na área
                '## da empresa e temporária
                Cmd.CommandText = "SELECT r.codrestr, u.codusina, r.codgerad, r.datinirestr, r.intinirestr, r.datfimrestr, " &
                                 "r.intfimrestr, r.valrestr, r.id_motivorestricao, isNull(r.obsrestr,'') as obsrestr  " &
                                 "FROM usina u, gerad g, " &
                                 IIf(strArquivo = "temp", strArquivo & "restrgerad", "restrgerad" & strArquivo) & " r " &
                                 "WHERE u.codempre = '" & strCodEmpresa & "' " &
                                 "AND u.codusina =  g.codusina " & AndUsina(codUsina) &
                                 "AND g.codgerad = r.codgerad "
                If strArquivo <> "" Then
                    Cmd.CommandText &= "AND r.datpdp = '" & strData & "' "
                Else
                    Cmd.CommandText &= "AND r.datinirestr <= '" & strData & "' " &
                                       "AND r.datfimrestr >= '" & strData & "' "
                End If
                Cmd.CommandText &= "ORDER BY u.codusina, r.codgerad"
                RegistrarLog(Cmd.CommandText)
                Dim rsRestricaoUG As SqlDataReader = Cmd.ExecuteReader
                Do While rsRestricaoUG.Read

                    ' Se não tem Observação gera só "RES", se tem gera "RES" e "REO"
                    strLinha = "RES " & Left(Trim(rsRestricaoUG("codusina")) & Space(13), 13) &
                                Left(Trim(rsRestricaoUG("codgerad")) & Space(13), 13) &
                                rsRestricaoUG("datinirestr") & " " & Right("00" & rsRestricaoUG("intinirestr"), 2) & " " &
                                rsRestricaoUG("datfimrestr") & " " & Right("00" & rsRestricaoUG("intfimrestr"), 2) & " " &
                                Right("00000" & Trim(Str(IIf(Not IsDBNull(rsRestricaoUG("valrestr")), rsRestricaoUG("valrestr"), 0))), 5) & " " &
                                Right("000" & Trim(Str(IIf(Not IsDBNull(rsRestricaoUG("id_motivorestricao")), rsRestricaoUG("id_motivorestricao"), 0))), 3) & " " &
                                Right("000000" & Trim(Str(IIf(Not IsDBNull(rsRestricaoUG("codrestr")), rsRestricaoUG("codrestr"), 0))), 6)
                    PrintLine(intArq, strLinha)

                    If Trim(rsRestricaoUG("obsrestr")) <> "" Then
                        strLinha = "REO " & Left(Trim(rsRestricaoUG("codusina")) & Space(13), 13) &
                                    Left(Trim(rsRestricaoUG("codgerad")) & Space(13), 13) &
                                    rsRestricaoUG("datinirestr") & " " & Right("00" & rsRestricaoUG("intinirestr"), 2) & " " &
                                    rsRestricaoUG("datfimrestr") & " " & Right("00" & rsRestricaoUG("intfimrestr"), 2) & " " &
                                    Right("00000" & Trim(Str(IIf(Not IsDBNull(rsRestricaoUG("valrestr")), rsRestricaoUG("valrestr"), 0))), 5) & " " &
                                    Right("000" & Trim(Str(IIf(Not IsDBNull(rsRestricaoUG("id_motivorestricao")), rsRestricaoUG("id_motivorestricao"), 0))), 3) & " " &
                                    Right("000000" & Trim(Str(IIf(Not IsDBNull(rsRestricaoUG("codrestr")), rsRestricaoUG("codrestr"), 0))), 6) & " " &
                                    Left(Trim(rsRestricaoUG("obsrestr")) & Space(255), 255)
                        PrintLine(intArq, strLinha)
                    End If
                Loop
                rsRestricaoUG.Close()
            End If
            RegistrarLog(strLinha)

            'MANUTENÇÃO DE UNIDADES GERADORAS
            RegistrarLog("MANUTENÇÃO DE UNIDADES GERADORAS")
            If strOpcao.Substring(8, 1) = "1" Then
                '## Em 21/06/2006 passou a selecionar pela data do pdp 
                '## e não mais pelo início e término da PARALIZAÇÃO na área
                '## da empresa e temporária
                Cmd.CommandText = "SELECT u.codusina, p.codparal, p.codequip, p.datiniparal, p.intiniparal, " &
                                 "p.datfimparal, p.intfimparal, p.codnivel, p.indcont " &
                                 "FROM usina u, gerad g, " &
                                 IIf(strArquivo = "temp", strArquivo & "paral", "paral" & strArquivo) & " p " &
                                 "WHERE u.codempre = '" & strCodEmpresa & "' " &
                                 "AND u.codusina =  g.codusina " & AndUsina(codUsina) &
                                 "AND g.codgerad = p.codequip "
                If strArquivo <> "" Then
                    Cmd.CommandText &= "AND p.datpdp = '" & strData & "' "
                Else
                    Cmd.CommandText &= "AND p.datiniparal <= '" & strData & "' " &
                                       "AND p.datfimparal >= '" & strData & "' "
                End If
                Cmd.CommandText &= "ORDER BY u.codusina, p.codequip"
                RegistrarLog(Cmd.CommandText)
                Dim rsManutencaoUG As SqlDataReader = Cmd.ExecuteReader
                Do While rsManutencaoUG.Read
                    strLinha = "MAN " & Left(Trim(rsManutencaoUG("codusina")) & Space(13), 13) &
                                Left(Trim(rsManutencaoUG("codequip")) & Space(13), 13) &
                                rsManutencaoUG("datiniparal") & " " & Right("00" & rsManutencaoUG("intiniparal"), 2) & " " &
                                rsManutencaoUG("datfimparal") & " " & Right("00" & rsManutencaoUG("intfimparal"), 2) & " " &
                                Left(Trim(rsManutencaoUG("codnivel")) & Space(10), 10) &
                                Trim(rsManutencaoUG("indcont")) & " " &
                                Right("00000" & Trim(Str(IIf(Not IsDBNull(rsManutencaoUG("codparal")), rsManutencaoUG("codparal"), 0))), 5)
                    PrintLine(intArq, strLinha)
                Loop
                rsManutencaoUG.Close()
            End If

            RegistrarLog(strLinha)
            'PARADA DE UNIDADES GERADORAS POR CONVENIÊNCIA OPERATIVA
            RegistrarLog("PARADA DE UNIDADES GERADORAS POR CONVENIÊNCIA OPERATIVA")
            If strOpcao.Substring(9, 1) = "1" Then
                Cmd.CommandText = "SELECT u.codusina, p.codparal, p.codequip, p.datiniparal, p.intiniparal, p.datfimparal, p.intfimparal " &
                                 "FROM usina u, gerad g, " & IIf(strArquivo = "temp", strArquivo & "paral_co", "paral" & strArquivo & "_co") & " p " &
                                 "WHERE u.codempre = '" & strCodEmpresa & "' " &
                                 "AND u.codusina =  g.codusina " & AndUsina(codUsina) &
                                 "AND g.codgerad = p.codequip " &
                                 "AND p.datiniparal <= '" & strData & "' " &
                                 "AND p.datfimparal >= '" & strData & "' " &
                                 "ORDER BY u.codusina, p.codequip"
                RegistrarLog(Cmd.CommandText)
                Dim rsParadaUG As SqlDataReader = Cmd.ExecuteReader
                Do While rsParadaUG.Read
                    strLinha = "PCO " & Left(Trim(rsParadaUG("codusina")) & Space(13), 13) &
                                Left(Trim(rsParadaUG("codequip")) & Space(13), 13) &
                                rsParadaUG("datiniparal") & " " & Right("00" & rsParadaUG("intiniparal"), 2) & " " &
                                rsParadaUG("datfimparal") & " " & Right("00" & rsParadaUG("intfimparal"), 2)
                    PrintLine(intArq, strLinha)
                Loop
                rsParadaUG.Close()
            End If
            RegistrarLog(strLinha)
            'EXPORTAÇÃO
            RegistrarLog("EXPORTAÇÃO")
            If strOpcao.Substring(10, 1) = "1" Then
                Cmd.CommandText = "SELECT g.codusina, g.valexporta" & strCampo & ", g.intexporta " &
                                 "FROM usina u, exporta g " &
                                 "WHERE u.codempre = '" & strCodEmpresa & "' " &
                                 "AND u.codusina =  g.codusina " & AndUsina(codUsina) &
                                 "AND g.datpdp = '" & strData & "' " &
                                 "ORDER BY g.codusina, g.intexporta"
                RegistrarLog(Cmd.CommandText)
                Dim rsExporta As SqlDataReader = Cmd.ExecuteReader
                strCodUsina = ""
                Do While rsExporta.Read
                    If strCodUsina <> Trim(rsExporta("codusina")) Then
                        If strCodUsina <> "" Then
                            PrintLine(intArq, strLinha)
                        End If
                        strCodUsina = Trim(rsExporta("codusina"))
                        strLinha = "EXP " & Left(strCodUsina & Space(12), 12)
                    End If
                    'strLinha = strLinha & " " & Right("00000" & Trim(Str(IIf(Not IsDBNull(rsExporta("valexporta" & strCampo)), rsExporta("valexporta" & strCampo), 0))), 5)

                    '
                    'Foi alterado por conta de Geração Negativa
                    'strLinha = strLinha & " " & Right("00000" & Trim(Str(IIf(Not IsDBNull(rsGeracao("valdespa" & strCampo)), rsGeracao("valdespa" & strCampo), 0))), 5)
                    strTemp = Trim(Str(IIf(Not IsDBNull(rsExporta("valexporta" & strCampo)), rsExporta("valexporta" & strCampo), 0)))

                    If Left(strTemp, 1) = "-" Then
                        strLinha = strLinha & " -" & Right("0000" & Mid(strTemp, 2), 4)
                    Else
                        strLinha = strLinha & " " & Right("00000" & strTemp, 5)
                    End If

                    '
                Loop
                If Left(strLinha, 3) = "EXP" Then
                    PrintLine(intArq, strLinha)
                End If
                rsExporta.Close()
            End If

            'IMPORTAÇÃO
            RegistrarLog("IMPORTAÇÃO")
            If strOpcao.Substring(11, 1) = "1" Then
                Cmd.CommandText = "SELECT g.codusina, g.valimporta" & strCampo & ", g.intimporta " &
                                 "FROM usina u, importa g " &
                                 "WHERE u.codempre = '" & strCodEmpresa & "' " &
                                 "AND u.codusina =  g.codusina " & AndUsina(codUsina) &
                                 "AND g.datpdp = '" & strData & "' " &
                                 "ORDER BY g.codusina, g.intimporta"
                RegistrarLog(Cmd.CommandText)
                Dim rsImporta As SqlDataReader = Cmd.ExecuteReader
                strCodUsina = ""
                Do While rsImporta.Read
                    If strCodUsina <> Trim(rsImporta("codusina")) Then
                        If strCodUsina <> "" Then
                            PrintLine(intArq, strLinha)
                        End If
                        strCodUsina = Trim(rsImporta("codusina"))
                        strLinha = "IMP " & Left(strCodUsina & Space(12), 12)
                    End If
                    strLinha = strLinha & " " & Right("00000" & Trim(Str(IIf(Not IsDBNull(rsImporta("valimporta" & strCampo)), rsImporta("valimporta" & strCampo), 0))), 5)
                Loop
                If Left(strLinha, 3) = "IMP" Then
                    PrintLine(intArq, strLinha)
                End If
                rsImporta.Close()
            End If
            RegistrarLog(strLinha)

            'MOTIVO DE DESPACHO RAZÃO ELÉTRICA (MRE)
            RegistrarLog("MOTIVO DE DESPACHO RAZÃO ELÉTRICA (MRE)")
            If strOpcao.Substring(12, 1) = "1" Then
                Cmd.CommandText = "SELECT g.codusina, g.valmre" & strCampo & ", g.intmre " &
                                 "FROM usina u, motivorel g " &
                                 "WHERE u.codempre = '" & strCodEmpresa & "' " &
                                 "AND u.codusina =  g.codusina " & AndUsina(codUsina) &
                                 "AND g.datpdp = '" & strData & "' " &
                                 "ORDER BY g.codusina, g.intmre"
                RegistrarLog(Cmd.CommandText)
                Dim rsDespRE As SqlDataReader = Cmd.ExecuteReader
                strCodUsina = ""
                Do While rsDespRE.Read
                    If strCodUsina <> Trim(rsDespRE("codusina")) Then
                        If strCodUsina <> "" Then
                            PrintLine(intArq, strLinha)
                        End If
                        strCodUsina = Trim(rsDespRE("codusina"))
                        strLinha = "MRE " & Left(strCodUsina & Space(12), 12)
                    End If
                    strLinha = strLinha & " " & Right("00000" & Trim(Str(IIf(Not IsDBNull(rsDespRE("valmre" & strCampo)), rsDespRE("valmre" & strCampo), 0))), 5)
                Loop
                If Left(strLinha, 3) = "MRE" Then
                    PrintLine(intArq, strLinha)
                End If
                rsDespRE.Close()
            End If
            RegistrarLog(strLinha)

            'MOTIVO DE DESPACHO POR INFLEXIBILIDADE (MIF)
            RegistrarLog("MOTIVO DE DESPACHO POR INFLEXIBILIDADE (MIF)")
            If strOpcao.Substring(13, 1) = "1" Then
                Cmd.CommandText = "SELECT g.codusina, g.valmif" & strCampo & ", g.intmif " &
                                 "FROM usina u, motivoinfl g " &
                                 "WHERE u.codempre = '" & strCodEmpresa & "' " &
                                 "AND u.codusina =  g.codusina " & AndUsina(codUsina) &
                                 "AND g.datpdp = '" & strData & "' " &
                                 "ORDER BY g.codusina, g.intmif"
                RegistrarLog(Cmd.CommandText)
                Dim rsDespI As SqlDataReader = Cmd.ExecuteReader
                strCodUsina = ""
                Do While rsDespI.Read
                    If strCodUsina <> Trim(rsDespI("codusina")) Then
                        If strCodUsina <> "" Then
                            PrintLine(intArq, strLinha)
                        End If
                        strCodUsina = Trim(rsDespI("codusina"))
                        strLinha = "MIF " & Left(strCodUsina & Space(12), 12)
                    End If
                    strLinha = strLinha & " " & Right("00000" & Trim(Str(IIf(Not IsDBNull(rsDespI("valmif" & strCampo)), rsDespI("valmif" & strCampo), 0))), 5)
                Loop
                If Left(strLinha, 3) = "MIF" Then
                    PrintLine(intArq, strLinha)
                End If
                rsDespI.Close()
            End If
            RegistrarLog(strLinha)

            'PERDAS CONSUMO INTERNO E COMPENSAÇÃO (PCC)
            RegistrarLog("PERDAS CONSUMO INTERNO E COMPENSAÇÃO (PCC)")
            If strOpcao.Substring(14, 1) = "1" Then
                Cmd.CommandText = "SELECT g.codusina, g.valpcc" & strCampo & ", g.intpcc " &
                                 "FROM usina u, perdascic g " &
                                 "WHERE u.codempre = '" & strCodEmpresa & "' " &
                                 "AND u.codusina =  g.codusina " & AndUsina(codUsina) &
                                 "AND g.datpdp = '" & strData & "' " &
                                 "ORDER BY g.codusina, g.intpcc"
                RegistrarLog(Cmd.CommandText)
                Dim rsConsumo As SqlDataReader = Cmd.ExecuteReader
                strCodUsina = ""
                Do While rsConsumo.Read
                    If strCodUsina <> Trim(rsConsumo("codusina")) Then
                        If strCodUsina <> "" Then
                            PrintLine(intArq, strLinha)
                        End If
                        strCodUsina = Trim(rsConsumo("codusina"))
                        strLinha = "PCC " & Left(strCodUsina & Space(12), 12)
                    End If
                    strLinha = strLinha & " " & Right("00000" & Trim(Str(IIf(Not IsDBNull(rsConsumo("valpcc" & strCampo)), rsConsumo("valpcc" & strCampo), 0))), 5)
                Loop
                If Left(strLinha, 3) = "PCC" Then
                    PrintLine(intArq, strLinha)
                End If
                rsConsumo.Close()
            End If
            RegistrarLog(strLinha)

            'NÚMERO MÁQUINAS PARADAS POR CONVENIÊNCIA OPERATIVA (MCO)
            RegistrarLog("NÚMERO MÁQUINAS PARADAS POR CONVENIÊNCIA OPERATIVA (MCO)")
            If strOpcao.Substring(15, 1) = "1" Then
                Cmd.CommandText = "SELECT g.codusina, g.valmco" & strCampo & ", g.intmco " &
                                 "FROM usina u, conveniencia_oper g " &
                                 "WHERE u.codempre = '" & strCodEmpresa & "' " &
                                 "AND u.codusina =  g.codusina " & AndUsina(codUsina) &
                                 "AND g.datpdp = '" & strData & "' " &
                                 "ORDER BY g.codusina, g.intmco"
                RegistrarLog(Cmd.CommandText)
                Dim rsMaqParada As SqlDataReader = Cmd.ExecuteReader
                strCodUsina = ""
                Do While rsMaqParada.Read
                    If strCodUsina <> Trim(rsMaqParada("codusina")) Then
                        If strCodUsina <> "" Then
                            PrintLine(intArq, strLinha)
                        End If
                        strCodUsina = Trim(rsMaqParada("codusina"))
                        strLinha = "MCO " & Left(strCodUsina & Space(12), 12)
                    End If
                    strLinha = strLinha & " " & Right("00000" & Trim(Str(IIf(Not IsDBNull(rsMaqParada("valmco" & strCampo)), rsMaqParada("valmco" & strCampo), 0))), 5)
                Loop
                If Left(strLinha, 3) = "MCO" Then
                    PrintLine(intArq, strLinha)
                End If
                rsMaqParada.Close()
            End If
            RegistrarLog(strLinha)

            'NÚMERO MÁQUINAS OPERANDO COMO SÍNCRONO (MOS)
            RegistrarLog("NÚMERO MÁQUINAS OPERANDO COMO SÍNCRONO (MOS)")
            If strOpcao.Substring(16, 1) = "1" Then
                Cmd.CommandText = "SELECT g.codusina, g.valmos" & strCampo & ", g.intmos " &
                                 "FROM usina u, oper_sincrono g " &
                                 "WHERE u.codempre = '" & strCodEmpresa & "' " &
                                 "AND u.codusina =  g.codusina " & AndUsina(codUsina) &
                                 "AND g.datpdp = '" & strData & "' " &
                                 "ORDER BY g.codusina, g.intmos"
                RegistrarLog(Cmd.CommandText)
                Dim rsMaqOperando As SqlDataReader = Cmd.ExecuteReader
                strCodUsina = ""
                Do While rsMaqOperando.Read
                    If strCodUsina <> Trim(rsMaqOperando("codusina")) Then
                        If strCodUsina <> "" Then
                            PrintLine(intArq, strLinha)
                        End If
                        strCodUsina = Trim(rsMaqOperando("codusina"))
                        strLinha = "MOS " & Left(strCodUsina & Space(12), 12)
                    End If
                    strLinha = strLinha & " " & Right("00000" & Trim(Str(IIf(Not IsDBNull(rsMaqOperando("valmos" & strCampo)), rsMaqOperando("valmos" & strCampo), 0))), 5)
                Loop
                If Left(strLinha, 3) = "MOS" Then
                    PrintLine(intArq, strLinha)
                End If
                rsMaqOperando.Close()
            End If
            RegistrarLog(strLinha)

            'NÚMERO MÁQUINAS GERANDO
            RegistrarLog("NÚMERO MÁQUINAS GERANDO")
            If strOpcao.Substring(17, 1) = "1" Then
                Cmd.CommandText = "SELECT g.codusina, g.valmeg" & strCampo & ", g.intmeg " &
                                 "FROM usina u, maq_gerando g " &
                                 "WHERE u.codempre = '" & strCodEmpresa & "' " &
                                 "AND u.codusina =  g.codusina " & AndUsina(codUsina) &
                                 "AND g.datpdp = '" & strData & "' " &
                                 "ORDER BY g.codusina, g.intmeg"
                RegistrarLog(Cmd.CommandText)
                Dim rsMaqGerando As SqlDataReader = Cmd.ExecuteReader
                strCodUsina = ""
                Do While rsMaqGerando.Read
                    If strCodUsina <> Trim(rsMaqGerando("codusina")) Then
                        If strCodUsina <> "" Then
                            PrintLine(intArq, strLinha)
                        End If
                        strCodUsina = Trim(rsMaqGerando("codusina"))
                        strLinha = "MEG " & Left(strCodUsina & Space(12), 12)
                    End If
                    strLinha = strLinha & " " & Right("00000" & Trim(Str(IIf(Not IsDBNull(rsMaqGerando("valmeg" & strCampo)), rsMaqGerando("valmeg" & strCampo), 0))), 5)
                Loop
                If Left(strLinha, 3) = "MEG" Then
                    PrintLine(intArq, strLinha)
                End If
                rsMaqGerando.Close()
            End If
            RegistrarLog(strLinha)

            'Energia de Reposição e Perdas (ERP)
            RegistrarLog("Energia de Reposição e Perdas (ERP)")
            If strOpcao.Substring(18, 1) = "1" Then
                Cmd.CommandText = "SELECT g.codusina, g.valerp" & strCampo & ", g.interp " &
                                 "FROM usina u, energia_reposicao g " &
                                 "WHERE u.codempre = '" & strCodEmpresa & "' " &
                                 "AND u.codusina =  g.codusina " & AndUsina(codUsina) &
                                 "AND g.datpdp = '" & strData & "' " &
                                 "ORDER BY g.codusina, g.interp"
                RegistrarLog(Cmd.CommandText)
                Dim rsErp As SqlDataReader = Cmd.ExecuteReader
                strCodUsina = ""
                Do While rsErp.Read
                    If strCodUsina <> Trim(rsErp("codusina")) Then
                        If strCodUsina <> "" Then
                            PrintLine(intArq, strLinha)
                        End If
                        strCodUsina = Trim(rsErp("codusina"))
                        strLinha = "ERP " & Left(strCodUsina & Space(12), 12)
                    End If
                    strLinha = strLinha & " " & Right("00000" & Trim(Str(IIf(Not IsDBNull(rsErp("valerp" & strCampo)), rsErp("valerp" & strCampo), 0))), 5)
                Loop
                If Left(strLinha, 3) = "ERP" Then
                    PrintLine(intArq, strLinha)
                End If
                rsErp.Close()
            End If
            RegistrarLog(strLinha)

            'Disponibilidade (DSP)
            RegistrarLog("Disponibilidade (DSP)")
            If strOpcao.Substring(19, 1) = "1" Then
                Cmd.CommandText = "SELECT g.codusina, g.valdsp" & strCampo & ", g.intdsp " &
                                 "FROM usina u, disponibilidade g " &
                                 "WHERE u.codempre = '" & strCodEmpresa & "' " &
                                 "AND u.codusina =  g.codusina " & AndUsina(codUsina) &
                                 "AND g.datpdp = '" & strData & "' " &
                                 "ORDER BY g.codusina, g.intdsp"
                RegistrarLog(Cmd.CommandText)
                Dim rsDsp As SqlDataReader = Cmd.ExecuteReader
                strCodUsina = ""
                Do While rsDsp.Read
                    If strCodUsina <> Trim(rsDsp("codusina")) Then
                        If strCodUsina <> "" Then
                            PrintLine(intArq, strLinha)
                        End If
                        strCodUsina = Trim(rsDsp("codusina"))
                        strLinha = "DSP " & Left(strCodUsina & Space(12), 12)
                    End If
                    strLinha = strLinha & " " & Right("00000" & Trim(Str(IIf(Not IsDBNull(rsDsp("valdsp" & strCampo)), rsDsp("valdsp" & strCampo), 0))), 5)
                Loop
                If Left(strLinha, 3) = "DSP" Then
                    PrintLine(intArq, strLinha)
                End If
                rsDsp.Close()
            End If

            RegistrarLog(strLinha)

            'Compensação de Lastro Físico (CLF)
            RegistrarLog("Compensação de Lastro Físico (CLF)")
            If strOpcao.Substring(20, 1) = "1" Then
                Cmd.CommandText = "SELECT g.codusina, g.valclf" & strCampo & ", g.intclf " &
                                 "FROM usina u, complastro_fisico g " &
                                 "WHERE u.codempre = '" & strCodEmpresa & "' " &
                                 "AND u.codusina =  g.codusina " & AndUsina(codUsina) &
                                 "AND g.datpdp = '" & strData & "' " &
                                 "ORDER BY g.codusina, g.intclf"
                RegistrarLog(Cmd.CommandText)
                Dim rsClf As SqlDataReader = Cmd.ExecuteReader
                strCodUsina = ""
                Do While rsClf.Read
                    If strCodUsina <> Trim(rsClf("codusina")) Then
                        If strCodUsina <> "" Then
                            PrintLine(intArq, strLinha)
                        End If
                        strCodUsina = Trim(rsClf("codusina"))
                        strLinha = "CLF " & Left(strCodUsina & Space(12), 12)
                    End If
                    strLinha = strLinha & " " & Right("00000" & Trim(Str(IIf(Not IsDBNull(rsClf("valclf" & strCampo)), rsClf("valclf" & strCampo), 0))), 5)
                Loop
                If Left(strLinha, 3) = "CLF" Then
                    PrintLine(intArq, strLinha)
                End If
                rsClf.Close()
            End If
            RegistrarLog(strLinha)

            'Restrição por Falta de Combustível (RFC)
            RegistrarLog("Restrição por Falta de Combustível (RFC)")
            If strOpcao.Substring(21, 1) = "1" Then
                Cmd.CommandText = "SELECT r.codusina, r.valrfc" & strCampo & ", r.intrfc " &
                                 "FROM usina u, rest_falta_comb r " &
                                 "WHERE u.codempre = '" & strCodEmpresa & "' " &
                                 "AND u.codusina = r.codusina " & AndUsina(codUsina) &
                                 "AND r.datpdp = '" & strData & "' " &
                                 "ORDER BY r.codusina, r.intrfc"
                RegistrarLog(Cmd.CommandText)
                Dim rsRFC As SqlDataReader = Cmd.ExecuteReader
                strCodUsina = ""
                Do While rsRFC.Read
                    If strCodUsina <> Trim(rsRFC("codusina")) Then
                        If strCodUsina <> "" Then
                            PrintLine(intArq, strLinha)
                        End If
                        strCodUsina = Trim(rsRFC("codusina"))
                        strLinha = "RFC " & Left(strCodUsina & Space(12), 12)
                    End If
                    strLinha = strLinha & " " & Right("00000" & Trim(Str(IIf(Not IsDBNull(rsRFC("valrfc" & strCampo)), rsRFC("valrfc" & strCampo), 0))), 5)
                Loop
                If Left(strLinha, 3) = "RFC" Then
                    PrintLine(intArq, strLinha)
                End If
                rsRFC.Close()
            End If
            RegistrarLog(strLinha)

            'Garantia Energética (RMP)
            RegistrarLog("Garantia Energética (RMP)")
            If strOpcao.Substring(22, 1) = "1" Then
                Cmd.CommandText = "SELECT r.codusina, r.valrmp" & strCampo & ", r.intrmp " &
                                 "FROM usina u, tb_rmp r " &
                                 "WHERE u.codempre = '" & strCodEmpresa & "' " &
                                 "AND u.codusina = r.codusina " & AndUsina(codUsina) &
                                 "AND r.datpdp = '" & strData & "' " &
                                 "ORDER BY r.codusina, r.intrmp"
                RegistrarLog(Cmd.CommandText)
                Dim rsRMP As SqlDataReader = Cmd.ExecuteReader
                strCodUsina = ""
                Do While rsRMP.Read
                    If strCodUsina <> Trim(rsRMP("codusina")) Then
                        If strCodUsina <> "" Then
                            PrintLine(intArq, strLinha)
                        End If
                        strCodUsina = Trim(rsRMP("codusina"))
                        strLinha = "RMP " & Left(strCodUsina & Space(12), 12)
                    End If
                    strLinha = strLinha & " " & Right("00000" & Trim(Str(IIf(Not IsDBNull(rsRMP("valrmp" & strCampo)), rsRMP("valrmp" & strCampo), 0))), 5)
                Loop
                If Left(strLinha, 3) = "RMP" Then
                    PrintLine(intArq, strLinha)
                End If
                rsRMP.Close()
            End If
            RegistrarLog(strLinha)

            'Geração Fora de Mérito (GFM)
            RegistrarLog("Geração Fora de Mérito (GFM)")
            If strOpcao.Substring(23, 1) = "1" Then
                Cmd.CommandText = "SELECT r.codusina, r.valgfm" & strCampo & ", r.intgfm " &
                                 "FROM usina u, tb_gfm r " &
                                 "WHERE u.codempre = '" & strCodEmpresa & "' " &
                                 "AND u.codusina = r.codusina " & AndUsina(codUsina) &
                                 "AND r.datpdp = '" & strData & "' " &
                                 "ORDER BY r.codusina, r.intgfm"
                RegistrarLog(Cmd.CommandText)
                Dim rsGFM As SqlDataReader = Cmd.ExecuteReader
                strCodUsina = ""
                Do While rsGFM.Read
                    If strCodUsina <> Trim(rsGFM("codusina")) Then
                        If strCodUsina <> "" Then
                            PrintLine(intArq, strLinha)
                        End If
                        strCodUsina = Trim(rsGFM("codusina"))
                        strLinha = "GFM " & Left(strCodUsina & Space(12), 12)
                    End If
                    strLinha = strLinha & " " & Right("00000" & Trim(Str(IIf(Not IsDBNull(rsGFM("valgfm" & strCampo)), rsGFM("valgfm" & strCampo), 0))), 5)
                Loop
                If Left(strLinha, 3) = "GFM" Then
                    PrintLine(intArq, strLinha)
                End If
                rsGFM.Close()
            End If
            RegistrarLog(strLinha)

            'Crédito por Substituição (CFM)
            RegistrarLog("Crédito por Substituição (CFM)")
            If strOpcao.Substring(24, 1) = "1" Then
                Cmd.CommandText = "SELECT r.codusina, r.valcfm" & strCampo & ", r.intcfm " &
                                 "FROM usina u, tb_cfm r " &
                                 "WHERE u.codempre = '" & strCodEmpresa & "' " &
                                 "AND u.codusina = r.codusina " & AndUsina(codUsina) &
                                 "AND r.datpdp = '" & strData & "' " &
                                 "ORDER BY r.codusina, r.intcfm"
                RegistrarLog(Cmd.CommandText)
                Dim rsCFM As SqlDataReader = Cmd.ExecuteReader
                strCodUsina = ""
                Do While rsCFM.Read
                    If strCodUsina <> Trim(rsCFM("codusina")) Then
                        If strCodUsina <> "" Then
                            PrintLine(intArq, strLinha)
                        End If
                        strCodUsina = Trim(rsCFM("codusina"))
                        strLinha = "CFM " & Left(strCodUsina & Space(12), 12)
                    End If
                    strLinha = strLinha & " " & Right("00000" & Trim(Str(IIf(Not IsDBNull(rsCFM("valcfm" & strCampo)), rsCFM("valcfm" & strCampo), 0))), 5)
                Loop
                If Left(strLinha, 3) = "CFM" Then
                    PrintLine(intArq, strLinha)
                End If
                rsCFM.Close()
            End If
            RegistrarLog(strLinha)

            'Geração Substituta (SOM)
            RegistrarLog("Geração Substituta (SOM)")
            If strOpcao.Substring(25, 1) = "1" Then
                Cmd.CommandText = "SELECT r.codusina, r.valsom" & strCampo & ", r.intsom " &
                                 "FROM usina u, tb_som r " &
                                 "WHERE u.codempre = '" & strCodEmpresa & "' " &
                                 "AND u.codusina = r.codusina " & AndUsina(codUsina) &
                                 "AND r.datpdp = '" & strData & "' " &
                                 "ORDER BY r.codusina, r.intsom"
                RegistrarLog(Cmd.CommandText)
                Dim rsSOM As SqlDataReader = Cmd.ExecuteReader
                strCodUsina = ""
                Do While rsSOM.Read
                    If strCodUsina <> Trim(rsSOM("codusina")) Then
                        If strCodUsina <> "" Then
                            PrintLine(intArq, strLinha)
                        End If
                        strCodUsina = Trim(rsSOM("codusina"))
                        strLinha = "SOM " & Left(strCodUsina & Space(12), 12)
                    End If
                    strLinha = strLinha & " " & Right("00000" & Trim(Str(IIf(Not IsDBNull(rsSOM("valsom" & strCampo)), rsSOM("valsom" & strCampo), 0))), 5)
                Loop
                If Left(strLinha, 3) = "SOM" Then
                    PrintLine(intArq, strLinha)
                End If
                rsSOM.Close()
            End If
            RegistrarLog(strLinha)

            'GE Substituição (GES)
            If strOpcao.Substring(26, 1) = "1" Then
                Cmd.CommandText = "SELECT r.codusina, r.valGES" & strCampo & ", r.intGES " &
                                  "FROM usina u, tb_GES r " &
                                  "WHERE u.codempre = '" & strCodEmpresa & "' " &
                                  "AND u.codusina = r.codusina " & AndUsina(codUsina) &
                                  "AND r.datpdp = '" & strData & "' " &
                                  "ORDER BY r.codusina, r.intGES"
                Dim rsGES As SqlDataReader = Cmd.ExecuteReader
                strCodUsina = ""
                Do While rsGES.Read
                    If strCodUsina <> Trim(rsGES("codusina")) Then
                        If strCodUsina <> "" Then
                            PrintLine(intArq, strLinha)
                        End If
                        strCodUsina = Trim(rsGES("codusina"))
                        strLinha = "GES " & Left(strCodUsina & Space(12), 12)
                    End If
                    strLinha = strLinha & " " & Right("00000" & Trim(Str(IIf(Not IsDBNull(rsGES("valGES" & strCampo)), rsGES("valGES" & strCampo), 0))), 5)
                Loop
                If Left(strLinha, 3) = "GES" Then
                    PrintLine(intArq, strLinha)
                End If
                rsGES.Close()
            End If

            'GE Crédito (GEC)
            If strOpcao.Substring(27, 1) = "1" Then
                Cmd.CommandText = "SELECT r.codusina, r.valGEC" & strCampo & ", r.intGEC " &
                                  "FROM usina u, tb_GEC r " &
                                  "WHERE u.codempre = '" & strCodEmpresa & "' " &
                                  "AND u.codusina = r.codusina " & AndUsina(codUsina) &
                                  "AND r.datpdp = '" & strData & "' " &
                                  "ORDER BY r.codusina, r.intGEC"
                Dim rsGEC As SqlDataReader = Cmd.ExecuteReader
                strCodUsina = ""
                Do While rsGEC.Read
                    If strCodUsina <> Trim(rsGEC("codusina")) Then
                        If strCodUsina <> "" Then
                            PrintLine(intArq, strLinha)
                        End If
                        strCodUsina = Trim(rsGEC("codusina"))
                        strLinha = "GEC " & Left(strCodUsina & Space(12), 12)
                    End If
                    strLinha = strLinha & " " & Right("00000" & Trim(Str(IIf(Not IsDBNull(rsGEC("valGEC" & strCampo)), rsGEC("valGEC" & strCampo), 0))), 5)
                Loop
                If Left(strLinha, 3) = "GEC" Then
                    PrintLine(intArq, strLinha)
                End If
                rsGEC.Close()
            End If

            'Despacho Ciclo Aberto (DCA)
            If strOpcao.Substring(28, 1) = "1" Then
                Cmd.CommandText = "SELECT r.codusina, r.valDCA" & strCampo & ", r.intDCA " &
                                  "FROM usina u, tb_DCA r " &
                                  "WHERE u.codempre = '" & strCodEmpresa & "' " &
                                  "AND u.codusina = r.codusina " & AndUsina(codUsina) &
                                  "AND r.datpdp = '" & strData & "' " &
                                  "ORDER BY r.codusina, r.intDCA"
                Dim rsDCA As SqlDataReader = Cmd.ExecuteReader
                strCodUsina = ""
                Do While rsDCA.Read
                    If strCodUsina <> Trim(rsDCA("codusina")) Then
                        If strCodUsina <> "" Then
                            PrintLine(intArq, strLinha)
                        End If
                        strCodUsina = Trim(rsDCA("codusina"))
                        strLinha = "DCA " & Left(strCodUsina & Space(12), 12)
                    End If
                    strLinha = strLinha & " " & Right("00000" & Trim(Str(IIf(Not IsDBNull(rsDCA("valDCA" & strCampo)), rsDCA("valDCA" & strCampo), 0))), 5)
                Loop
                If Left(strLinha, 3) = "DCA" Then
                    PrintLine(intArq, strLinha)
                End If
                rsDCA.Close()
            End If

            'Despacho Ciclo Reduzido (DCR)
            If strOpcao.Substring(29, 1) = "1" Then
                Cmd.CommandText = "SELECT r.codusina, r.valDCR" & strCampo & ", r.intDCR " &
                                  "FROM usina u, tb_DCR r " &
                                  "WHERE u.codempre = '" & strCodEmpresa & "' " &
                                  "AND u.codusina = r.codusina " & AndUsina(codUsina) &
                                  "AND r.datpdp = '" & strData & "' " &
                                  "ORDER BY r.codusina, r.intDCR"
                Dim rsDCR As SqlDataReader = Cmd.ExecuteReader
                strCodUsina = ""
                Do While rsDCR.Read
                    If strCodUsina <> Trim(rsDCR("codusina")) Then
                        If strCodUsina <> "" Then
                            PrintLine(intArq, strLinha)
                        End If
                        strCodUsina = Trim(rsDCR("codusina"))
                        strLinha = "DCR " & Left(strCodUsina & Space(12), 12)
                    End If
                    strLinha = strLinha & " " & Right("00000" & Trim(Str(IIf(Not IsDBNull(rsDCR("valDCR" & strCampo)), rsDCR("valDCR" & strCampo), 0))), 5)
                Loop
                If Left(strLinha, 3) = "DCR" Then
                    PrintLine(intArq, strLinha)
                End If
                rsDCR.Close()
            End If

            'Insumo Reserva 1 (IR1 Nível de Partida)
            If strOpcao.Substring(30, 1) = "1" Then
                Cmd.CommandText = " SELECT r.codusina, r.valIR1" & strCampo &
                                      " FROM usina u, tb_IR1 r " &
                                      " WHERE u.codempre = '" & strCodEmpresa & "' " &
                                      " AND u.codusina = r.codusina " & AndUsina(codUsina) &
                                      " AND r.datpdp = '" & strData & "' " &
                                      " ORDER BY r.codusina "
                Dim rsIR1 As SqlDataReader = Cmd.ExecuteReader
                strCodUsina = ""
                Do While rsIR1.Read
                    If strCodUsina <> Trim(rsIR1("codusina")) Then
                        If strCodUsina <> "" Then
                            PrintLine(intArq, strLinha)
                        End If
                        strCodUsina = Trim(rsIR1("codusina"))
                        strLinha = "NPA " & Left(strCodUsina & Space(12), 12)
                    End If
                    strLinha = strLinha & " " & Right("00000" & Trim(Str(IIf(Not IsDBNull(rsIR1("valIR1" & strCampo)), rsIR1("valIR1" & strCampo), 0))), 5)
                Loop
                If Left(strLinha, 3) = "NPA" Then
                    PrintLine(intArq, strLinha)
                End If
                rsIR1.Close()
            End If

            'Insumo Reserva 2 (IR2)
            If strOpcao.Substring(31, 1) = "1" Then
                Cmd.CommandText = " SELECT r.codusina, r.valIR2" & strCampo & ", r.intIR2 " &
                                      " FROM usina u, tb_IR2 r " &
                                      " WHERE u.codempre = '" & strCodEmpresa & "' " &
                                      " AND u.codusina = r.codusina " & AndUsina(codUsina) &
                                      " AND r.datpdp = '" & strData & "' " &
                                      " ORDER BY r.codusina, r.intIR2"
                Dim rsIR2 As SqlDataReader = Cmd.ExecuteReader
                strCodUsina = ""
                Do While rsIR2.Read
                    If strCodUsina <> Trim(rsIR2("codusina")) Then
                        If strCodUsina <> "" Then
                            PrintLine(intArq, strLinha)
                        End If
                        strCodUsina = Trim(rsIR2("codusina"))
                        strLinha = "R11 " & Left(strCodUsina & Space(12), 12)
                    End If
                    strLinha = strLinha & " " & Right("00000" & Trim(Str(IIf(Not IsDBNull(rsIR2("valIR2" & strCampo)), rsIR2("valIR2" & strCampo), 0))), 5)
                Loop
                If Left(strLinha, 3) = "R11" Then
                    PrintLine(intArq, strLinha)
                End If
                rsIR2.Close()
            End If

            'Insumo Reserva 3 (IR3)
            If strOpcao.Substring(32, 1) = "1" Then
                Cmd.CommandText = "SELECT r.codusina, r.valIR3" & strCampo & ", r.intIR3 " &
                                      "FROM usina u, tb_IR3 r " &
                                      "WHERE u.codempre = '" & strCodEmpresa & "' " &
                                      "AND u.codusina = r.codusina " & AndUsina(codUsina) &
                                      "AND r.datpdp = '" & strData & "' " &
                                      "ORDER BY r.codusina, r.intIR3"
                Dim rsIR3 As SqlDataReader = Cmd.ExecuteReader
                strCodUsina = ""
                Do While rsIR3.Read
                    If strCodUsina <> Trim(rsIR3("codusina")) Then
                        If strCodUsina <> "" Then
                            PrintLine(intArq, strLinha)
                        End If
                        strCodUsina = Trim(rsIR3("codusina"))
                        strLinha = "R12 " & Left(strCodUsina & Space(12), 12)
                    End If
                    strLinha = strLinha & " " & Right("00000" & Trim(Str(IIf(Not IsDBNull(rsIR3("valIR3" & strCampo)), rsIR3("valIR3" & strCampo), 0))), 5)
                Loop
                If Left(strLinha, 3) = "R12" Then
                    PrintLine(intArq, strLinha)
                End If
                rsIR3.Close()
            End If

            'Insumo Reserva 4 (IR4) 'estava strOpcao.Substring(26, 1)
            If strOpcao.Substring(33, 1) = "1" Then
                Cmd.CommandText = "SELECT r.codusina, r.valIR4" & strCampo & ", r.intIR4 " &
                                      "FROM usina u, tb_IR4 r " &
                                      "WHERE u.codempre = '" & strCodEmpresa & "' " &
                                      "AND u.codusina = r.codusina " & AndUsina(codUsina) &
                                      "AND r.datpdp = '" & strData & "' " &
                                      "ORDER BY r.codusina, r.intIR4"
                Dim rsIR4 As SqlDataReader = Cmd.ExecuteReader
                strCodUsina = ""
                Do While rsIR4.Read
                    If strCodUsina <> Trim(rsIR4("codusina")) Then
                        If strCodUsina <> "" Then
                            PrintLine(intArq, strLinha)
                        End If
                        strCodUsina = Trim(rsIR4("codusina"))
                        strLinha = "CAN " & Left(strCodUsina & Space(12), 12)
                    End If
                    strLinha = strLinha & " " & Right("00000" & Trim(Str(IIf(Not IsDBNull(rsIR4("valIR4" & strCampo)), rsIR4("valIR4" & strCampo), 0))), 5)
                Loop
                If Left(strLinha, 3) = "CAN" Then
                    PrintLine(intArq, strLinha)
                End If
                rsIR4.Close()
            End If

            'Insumo CVU da Programação de Oferata Semanal
            RegistrarLog("Insumo CVU da Programação de Oferata Semanal")
            If strOpcao.Substring(34, 1) = "1" Then

                strLinha = ""

                Cmd.CommandText = " SELECT r.cod_usinaprog, rs.val_cvuprog " &
                                    " FROM tb_respdemanda r " &
                                    " join tb_respdemandasemanal rs on rs.id_respdemandasemanal = r.id_respdemandasemanal  WHERE r.datpdp = '" & strData & "' and " &
                                    "  EXISTS (SELECT 1 " &
                                    "          FROM usina u " &
                                    "          WHERE u.codusina = r.cod_usinaprog " & AndUsina(codUsina) &
                                    "               and u.codempre = '" & strCodEmpresa & "'); "

                Dim rsRdemanda As SqlDataReader = Cmd.ExecuteReader

                strCodUsina = ""

                Dim valorCVU As String = ""

                provider.NumberDecimalSeparator = "."

                Do While rsRdemanda.Read
                    If strCodUsina <> Trim(rsRdemanda("cod_usinaprog")) Then
                        If strCodUsina <> "" Then
                            PrintLine(intArq, strLinha)
                        End If
                        strCodUsina = Trim(rsRdemanda("cod_usinaprog"))
                        strLinha = "CVU " & Left(strCodUsina & Space(12), 12)
                    End If

                    valorCVU = String.Format(provider, "{0:####0.00}", IIf(Not IsDBNull(rsRdemanda("val_cvuprog")), rsRdemanda("val_cvuprog"), 0))

                    strLinha = strLinha & " " & Right("00000000" & valorCVU, 8)
                Loop

                If Left(strLinha, 3) = "CVU" Then
                    PrintLine(intArq, strLinha)
                End If
                rsRdemanda.Close()

            End If
            RegistrarLog(strLinha)

            'RRO
            RegistrarLog("RRO")
            If strOpcao.Substring(35, 1) = "1" Then

                Cmd.CommandText = "SELECT g.codusina, g.valrro" & strCampo & ", g.intrro " &
                                     "FROM usina u, tb_rro g " &
                                     "WHERE u.codempre = '" & strCodEmpresa & "' " &
                                     "AND u.codusina = g.codusina " & AndUsina(codUsina) &
                                     "AND g.datpdp = '" & strData & "' " &
                                     "ORDER BY g.codusina, g.intrro"
                RegistrarLog(Cmd.CommandText)

                Dim rsRRO As SqlDataReader = Cmd.ExecuteReader

                strCodUsina = ""
                Do While rsRRO.Read
                    If strCodUsina <> Trim(rsRRO("codusina")) Then
                        If strCodUsina <> "" Then
                            PrintLine(intArq, strLinha)
                        End If
                        strCodUsina = Trim(rsRRO("codusina"))
                        strLinha = "RRO " & Left(strCodUsina & Space(12), 12)
                    End If

                    strTemp = Trim(Str(IIf(Not IsDBNull(rsRRO("valrro" & strCampo)), rsRRO("valrro" & strCampo), 0)))

                    If Left(strTemp, 1) = "-" Then
                        strLinha = strLinha & " -" & Right("0000" & Mid(strTemp, 2), 4)
                    Else
                        strLinha = strLinha & " " & Right("00000" & strTemp, 5)
                    End If
                Loop
                If Left(strLinha, 3) = "RRO" Then
                    PrintLine(intArq, strLinha)
                End If
                rsRRO.Close()
            End If
            RegistrarLog(strLinha)
            '----------------------------------------------

            'Conn.Close()
            FileClose(intArq)
        Catch ex As Exception
            RegistrarLogErro(ex)
            FileClose(intArq)
            Kill(strArq)
            GravaArquivoTexto = False
            Throw New Exception(ex.Message)
            Exit Function
        End Try

        ' Conforme solicitação da Marta, quando a chamada
        ' for através do envio de dados, será necessário gravar
        ' na área de FTP

        bRetorno = True

        'Se estiver debugando não enviar arquivo para o FTP
#If (DEBUG) Then
        bFtp = False
#End If

        If bFtp = True And Not isDESSEM Then
            Dim oReader As SqlDataReader
            Dim oFtpConn As FtpConnection = New FtpConnection

            Cmd.CommandText = "SELECT cod_enderecoip, lgn_usuario, cod_senha, cod_diretorio " &
                             "FROM tb_ftpareapdp " &
                             "WHERE id_perfil = '" & strPerfil & "'"
            RegistrarLog(Cmd.CommandText)
            oReader = Cmd.ExecuteReader
            If oReader.Read() Then
                oFtpConn.Hostname = Trim(oReader.GetString(0))
                oFtpConn.Username = Trim(oReader.GetString(1))
                oFtpConn.Password = Trim(oReader.GetString(2))
                RegistrarLog("FTP Hostname: " + oFtpConn.Hostname)
                RegistrarLog("FTP Username: " + oFtpConn.Username)
                'oFtpConn.Port = 21
                RegistrarLog("Conectando FTP...")
                oFtpConn.Connect()
                RegistrarLog("FTP IsConnected: " + oFtpConn.IsConnected.ToString())
                If oFtpConn.IsConnected Then
                    If Not oFtpConn.UploadFile(strArq, oReader.GetString(3) & Trim(strCodEmpresa) & "-ON " & strDataAtual & "-" & strHorario & ".PDP") Then
                        bRetorno = False
                    End If
                Else
                    bRetorno = False
                End If
                RegistrarLog("Desconectando FTP...")
                oFtpConn.Disconnect()
                RegistrarLog("FTP Desconectado.")
            End If

            oReader.Close()
            'após o envio para a área de FTP o arquivo deve ser eliminado
            'Para o perfil ESUTDO_PDP o arquivo não será eliminado
            If strPerfil.Trim <> "ESTUDO_PDP" Then
                Kill(strArq)
            End If

        End If
    End Function

    Private Sub RegistrarLog(ByVal mensagem As String)
        'log4net.Config.XmlConfigurator.Configure()
        _util.RegistrarLog(mensagem)
    End Sub
    Private Sub RegistrarLogErro(ByVal ex As Exception)
        'log4net.Config.XmlConfigurator.Configure()
        _util.RegistrarLogErro(ex)
    End Sub

    Private Function ObterParametrosInsumo(ByRef idInsumo As String, ByVal mnemonico As String) As String
        Dim Conn As SqlConnection = New SqlConnection
        Dim Cmd As SqlCommand = New SqlCommand

        If Not Conn.State = ConnectionState.Open Then
            Conn.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
            Conn.Open()
        End If

        Cmd.Connection = Conn
        Cmd.CommandText = "select id_insumo from tb_insumo where dsc_mnemonico = '" + mnemonico.ToUpper() + "'"

        idInsumo = Cmd.ExecuteScalar()

        Return Convert.ToString(idInsumo)
    End Function

    ' Grava os dados na base 
    Public Function UpLoadArquivoTexto(ByRef strArq As String,
                                       ByVal strUsuario As String,
                                       ByVal strParamCodEmpresa As String,
                                       ByVal strParamDataPdp As String,
                                       ByVal strSigEmpresa As String,
                                       ByRef strRetorno As String,
                                       Optional ByVal IsDessem As Boolean = False) As Boolean
        ' blnTipoEnvio = 0 Área de Transferência
        ' blnTipoEnvio = 1 Dados Enviados
        ' blnTipoEnvio = 2 Dados Consolidados
        ' blnTipoEnvio = 4 Programacao Semanal / diaria

        Dim bPrimeiro As Boolean
        Dim strDataPdp As String = ""
        Dim intArq, intI, intII, intPedaco As Integer
        Dim strCampo, strArquivo, strLinha, strTemp As String
        Dim strCodUsina, strTipInter, strCodEmpresa, strCodGeradora, strComentario As String
        'Dim Conn As SqlConnection = New SqlConnection
        Dim Conn As System.Data.SqlClient.SqlConnection = New System.Data.SqlClient.SqlConnection
        'Dim Cmd As SqlCommand = New SqlCommand
        Dim Cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand

        '#################################################
        'Dim IfxConn As IBM.Data.Informix.IfxConnection = New IBM.Data.Informix.IfxConnection
        'Dim IfxCmd As IBM.Data.Informix.IfxCommand = New IBM.Data.Informix.IfxCommand
        'IfxConn.ConnectionString = "database=pdpdes;server=rio_desenvsun_tcp;host=10.203.6.156;uid=ccardoso;password=base123;protocol=onsoctcp;service=2600"
        'IfxConn.Open()
        'IfxCmd.Connection = IfxConn
        'Dim IfxTrans As IBM.Data.Informix.IfxTransaction
        '#################################################


        'Dim oReader As SqlDataReader = New SqlDataReader
        Dim oReader As System.Data.SqlClient.SqlDataReader '= New System.Data.SqlClient.SqlDataReader

        Dim strTexto As System.String
        Dim bFlag As Boolean
        Dim iCnt As Integer
        Dim bFlag_GER As Boolean = False
        Dim bFlag_RRO As Boolean = False
        Dim bFlag_CAR As Boolean = False
        Dim bFlag_INT As Boolean = False
        Dim bFlag_VAZ As Boolean = False
        Dim bFlag_IFX As Boolean = False
        Dim bFlag_ZEN As Boolean = False
        Dim bFlag_ZEL As Boolean = False
        Dim bFlag_EXP As Boolean = False
        Dim bFlag_IMP As Boolean = False
        Dim bFlag_RES As Boolean = False
        Dim bFlag_MAN As Boolean = False
        Dim bFlag_MRE As Boolean = False
        Dim bFlag_MIF As Boolean = False
        Dim bFlag_PCC As Boolean = False
        Dim bFlag_MCO As Boolean = False
        Dim bFlag_MOS As Boolean = False
        Dim bFlag_MEG As Boolean = False
        Dim bFlag_ERP As Boolean = False
        Dim bFlag_DSP As Boolean = False
        Dim bFlag_CLF As Boolean = False
        Dim bFlag_PCO As Boolean = False
        Dim bFlag_RFC As Boolean = False
        Dim bFlag_RMP As Boolean = False
        Dim bFlag_GFM As Boolean = False
        Dim bFlag_CFM As Boolean = False
        Dim bFlag_SOM As Boolean = False
        Dim bFlag_GES As Boolean = False
        Dim bFlag_GEC As Boolean = False
        Dim bFlag_DCA As Boolean = False
        Dim bFlag_DCR As Boolean = False
        Dim bFlag_IR1 As Boolean = False
        Dim bFlag_IR2 As Boolean = False
        Dim bFlag_IR3 As Boolean = False
        Dim bFlag_IR4 As Boolean = False


        Dim sql As String = ""
        Cmd.Connection = Conn
        Conn.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
        'Conn.Servico = "PDPUpLoad"
        'Conn.Usuario = strUsuario
        Conn.Open()
        Cmd.CommandTimeout = 180

        'abrir o arquivo
        intArq = FreeFile()
        FileOpen(intArq, strArq, OpenMode.Input, OpenAccess.Read)

        ' Ler todas as linhas do arquivo
        Try
            bPrimeiro = True
            While Not EOF(intArq)
                strLinha = LineInput(intArq)
                If bPrimeiro Then
                    strDataPdp = Mid(strLinha, 16, 8)
                    strCodEmpresa = Mid(strLinha, 25, 2)

                    ' Verificar se a empresa e data escolhida confere com o arquivo
                    If (Trim(strDataPdp) <> Trim(strParamDataPdp)) Or (Trim(strCodEmpresa) <> Trim(strParamCodEmpresa)) Then
                        strRetorno = "A Data do PDP ou a Empresa selecionada não confere com os valores do arquivo."
                        FileClose(intArq)
                        Cmd.Dispose()
                        Conn.Close()
                        Conn.Dispose()
                        UpLoadArquivoTexto = False
                        Exit Function

                    End If

                    ' verificar se a data do pdp foi inicializada
                    Cmd.CommandText = "SELECT datpdp " &
                                      "FROM pdp " &
                                      "WHERE datpdp = '" & strDataPdp & "'"
                    oReader = Cmd.ExecuteReader
                    If Not (oReader.Read()) Then
                        oReader.Close()
                        oReader.Dispose()
                        strRetorno = "A data do PDP que está no arquivo não foi inicializada."
                        FileClose(intArq)
                        Cmd.Dispose()
                        Conn.Close()
                        Conn.Dispose()
                        UpLoadArquivoTexto = False
                        Exit Function
                    End If
                    oReader.Close()
                    ' 08/11/2023 - Conforme combinado com Wallace e Adriano, retirei a verificação de usuário x empresa e deixei apenas a empresa"
                    Cmd.CommandText = "SELECT codempre FROM empre where" &
                                        " codempre = '" & strCodEmpresa & "'"

                    oReader = Cmd.ExecuteReader
                    If Not (oReader.Read()) Then
                        oReader.Close()
                        oReader.Dispose()
                        strRetorno = "A empresa informada no arquivo não existe. (Empresa " & strCodEmpresa & ")"
                        FileClose(intArq)
                        Cmd.Dispose()
                        Conn.Close()
                        Conn.Dispose()
                        UpLoadArquivoTexto = False
                        Exit Function
                    End If
                    oReader.Close()


                    ' Apagar os registros de Restrição e Manutenção para executar a carga novamente
                    sql += "DELETE FROM restrusinaemp " &
                                      "WHERE datpdp = '" & strDataPdp & "' " &
                                      "AND codusina IN " &
                                      "(SELECT codusina " &
                                      " FROM usina " &
                                      " WHERE codempre = '" & strCodEmpresa & "');"

                    sql += "DELETE FROM restrgerademp " &
                                      "WHERE datpdp = '" & strDataPdp & "' " &
                                      "AND codgerad In " &
                                      "(SELECT codgerad " &
                                      " FROM gerad g, usina u " &
                                      " WHERE u.codempre = '" & strCodEmpresa & "' " &
                                      " AND u.codusina = g.codusina);"

                    sql += "DELETE FROM paralemp " &
                                      "WHERE datpdp = '" & strDataPdp & "' " &
                                      "AND codequip IN " &
                                      "(SELECT codgerad " &
                                      " FROM gerad g, usina u " &
                                      " WHERE g.codusina = u.codusina " &
                                      " AND u.codempre = '" & strCodEmpresa & "');"

                    'Parada de Máquinas por Conveniência Operativa
                    sql += "DELETE FROM paralemp_co " &
                                      "WHERE datpdp = '" & strDataPdp & "' " &
                                      "AND codequip IN " &
                                      "(SELECT codgerad " &
                                      "FROM gerad g, usina u " &
                                      "WHERE g.codusina = u.codusina " &
                                      "AND u.codempre = '" & strCodEmpresa & "');"

                    Cmd.CommandText = sql
                    Cmd.ExecuteNonQuery()

                    bPrimeiro = False
                Else
                    If strLinha.Length > 10 Then

                        Dim mnemonicoLinha As String = strLinha.Substring(0, 3).Trim

                        If IsDessem = True And Not mnemonicoLinha = "GER" Then 'DESSEM só faz GER
                            Continue While
                        End If

                        If IsDessem = False And ValidaInsumoBloqueado(mnemonicoLinha) Then
                            Continue While
                        End If

                        Select Case mnemonicoLinha
                            Case Is = "GER"
                                'Geração (ler os 48 valores da linha)
                                strCodUsina = Trim(Mid(strLinha, 5, 6))
                                ' os valores iniciam na coluna 18 com tamanho de 5
                                intPedaco = 18

                                If IsDessem Then

                                    Dim idInsumo As String
                                    ObterParametrosInsumo(idInsumo, "GER")

                                    strCodUsina = Trim(Mid(strLinha, 5, 6))
                                    strCodEmpresa = Mid(strLinha, 25, 2)

                                    If strLinha.Length > 304 Then
                                        Dim defalutString As Byte() = Encoding.Default.GetBytes(Trim(Mid(strLinha, 305, strLinha.Length)))
                                        strComentario = Encoding.UTF8.GetString(defalutString)
                                    Else
                                        'strComentario = "Valores sugeridos pelo Agente. Comentário não informado." - Não haverá comentário padrão
                                        Continue While
                                    End If


                                    Cmd.Parameters.Clear()
                                    Cmd.CommandText = "Select COUNT(id_comentario) as Qtd from tb_comentario_dessem 
                                                        where flg_aprovado is not null 
                                                       and id_insumo = " + idInsumo + "
                                                       and dat_pdp = '" + strDataPdp + "'
                                                       and cod_usina = '" + strCodUsina + "'"

                                    Dim quantidade As Integer = Cmd.ExecuteScalar()
                                    If quantidade > 0 Then 'Não importar dado caso existir registro de comentário com alguma decisão do ONS
                                        Continue While
                                    End If

                                    Cmd.Transaction = Conn.BeginTransaction()

                                    Cmd = RemoverComentariosDESSEM(strDataPdp, strCodUsina, idInsumo, Cmd)

                                    Cmd.Parameters.Clear()
                                    Cmd.CommandText = "insert into tb_comentario_dessem ( id_insumo, dat_pdp, dsc_comentario, cod_Usina, flg_sugestaoAgente) values (" +
                                                                    "" + idInsumo +
                                                                    ", '" + strDataPdp +
                                                                    "', @descricao , '" +
                                                                    strCodUsina + "', 3)"

                                    Dim param As Common.DbParameter = Cmd.CreateParameter()
                                    param.ParameterName = "@descricao"
                                    param.Value = strComentario
                                    param.DbType = DbType.String
                                    param.Direction = ParameterDirection.Input
                                    Cmd.Parameters.Add(param)

                                    Try
                                        Cmd.ExecuteNonQuery()
                                    Catch ex As Exception
                                        Throw ex 'para DEBUG
                                    End Try

                                    Cmd.Parameters.Clear()
                                    Cmd.CommandText = $"Select Max(id_comentario) as id From tb_comentario_dessem c 
                                                        Where c.Dat_Pdp = '{strDataPdp}' 
                                                        and c.Id_Insumo = '{idInsumo}'
                                                        and c.Cod_Usina = '{strCodUsina}'"

                                    Dim idComentario As String
                                    idComentario = Cmd.ExecuteScalar()

                                    For intII = 1 To 48

                                        Dim pat As String = intII.ToString()

                                        Cmd.CommandText = $"select isNull(ValDespaPRE, 0) as Val_DESSEM from Despa Where
                                                        CodUsina = '{strCodUsina}'
                                                        and datpdp = '{strDataPdp}'
                                                        and intdespa = '{pat}'"

                                        Dim vlDessem As String
                                        vlDessem = Cmd.ExecuteScalar()

                                        If IsNothing(vlDessem) Then
                                            vlDessem = "0"
                                        End If

                                        Dim valorSugerido As String = Mid(strLinha, intPedaco, 5)
                                        If String.IsNullOrEmpty(valorSugerido.Trim()) Then
                                            valorSugerido = "0"
                                        End If

                                        Dim valorDessem As String = vlDessem
                                        If String.IsNullOrEmpty(valorDessem) Then
                                            valorDessem = "0"
                                        End If

                                        Cmd.CommandText =
                                                $"/*Usina: {strCodUsina} */ insert into tb_comentario_dessem_patamar 
                                                    ( id_comentario, num_patamar, val_sugerido, val_dessem) 
                                                    values( {idComentario }, { pat }, {valorSugerido} , {valorDessem} )"

                                        Try
                                            Cmd.ExecuteNonQuery()
                                        Catch ex As Exception
                                            Throw ex 'para DEBUG
                                        End Try

                                        intPedaco += 6
                                    Next

                                    Try
                                        Cmd.Transaction.Commit()
                                    Catch ex As Exception
                                        Cmd.Transaction.Rollback()
                                        Throw ex
                                    End Try

                                Else 'Não é DESSEM
                                    For intI = 1 To 48
                                        sql += "UPDATE despa " &
                                                          "SET valdespatran = " & Mid(strLinha, intPedaco, 5) & " " &
                                                          "WHERE datpdp = '" & strDataPdp & "' " &
                                                          "AND codusina = '" & strCodUsina & "' " &
                                                          "AND intdespa = " & intI & ";"

                                        intPedaco += 6
                                    Next

                                    'Grava evento registrando o recebimento de Geração
                                    If bFlag_GER = False Then
                                        sql += GerarSqlGravaEventoPDP("7", strParamDataPdp, strSigEmpresa, Now, "PDPUpload", strUsuario)
                                        bFlag_GER = True
                                    End If
                                End If

                            Case Is = "RRO"
                                'RRO (ler os 48 valores da linha)
                                strCodUsina = Trim(Mid(strLinha, 5, 6))
                                ' os valores iniciam na coluna 18 com tamanho de 5
                                intPedaco = 18

                                For intI = 1 To 48
                                    'tb_rro
                                    sql += "UPDATE tb_rro " &
                                                          "SET valrrotran = " & Mid(strLinha, intPedaco, 5) & " " &
                                                          "WHERE datpdp = '" & strDataPdp & "' " &
                                                          "AND codusina = '" & strCodUsina & "' " &
                                                          "AND intrro = " & intI & ";"
                                    intPedaco += 6
                                Next

                                'Grava evento registrando o recebimento de RRO
                                If bFlag_RRO = False Then
                                    sql += GerarSqlGravaEventoPDP("64", strParamDataPdp, strSigEmpresa, Now, "PDPUpload", strUsuario)
                                    bFlag_RRO = True
                                End If

                            Case Is = "CAR"
                                'CARGA 
                                ' os valores iniciam na coluna 18 com tamanho de 5
                                intPedaco = 5

                                For intI = 1 To 48
                                    sql += "UPDATE carga " &
                                                          "SET valcargatran = " & Mid(strLinha, intPedaco, 5) & " " &
                                                          "WHERE datpdp = '" & strDataPdp & "' " &
                                                          "AND codempre = '" & strCodEmpresa & "' " &
                                                          "AND intcarga = " & intI & ";"

                                    intPedaco += 6
                                Next

                                'Grava evento registrando o recebimento da carga
                                If bFlag_CAR = False Then
                                    sql += GerarSqlGravaEventoPDP("8", strParamDataPdp, strSigEmpresa, Now, "PDPUpload", strUsuario)
                                    bFlag_CAR = True
                                End If
                            Case Is = "INT"
                                'Intercambio (ler os 48 valores da linha)
                                ' os valores iniciam na coluna 18 com tamanho de 5
                                intPedaco = 28

                                For intI = 1 To 48
                                    strTipInter = Mid(strLinha, 10, 4) & ": " &
                                                        Mid(strLinha, 15, 4) & "-" &
                                                        Mid(strLinha, 20, 4) & "/" &
                                                        Mid(strLinha, 25, 2)
                                    sql += "UPDATE inter " &
                                                          "SET valintertran = " & Mid(strLinha, intPedaco, 6) & " " &
                                                          "WHERE datpdp = '" & strDataPdp & "' " &
                                                          "AND codemprede = '" & Mid(strLinha, 5, 4) & "' " &
                                                          "AND codemprepara = '" & Mid(strLinha, 10, 4) & "' " &
                                                          "AND codcontade = '" & Mid(strLinha, 15, 4) & "' " &
                                                          "AND codcontapara = '" & Mid(strLinha, 20, 4) & "' " &
                                                          "AND codcontamodal = '" & Mid(strLinha, 25, 2) & "' " &
                                                          "AND intinter = " & intI & ";"

                                    intPedaco += 7
                                Next

                                If bFlag_INT = False Then
                                    sql += GerarSqlGravaEventoPDP("9", strParamDataPdp, strSigEmpresa, Now, "PDPUpload", strUsuario)
                                    bFlag_INT = True
                                End If
                            Case Is = "VAZ"
                                'VAZÃO (ler os 48 valores da linha)
                                strCodUsina = Trim(Mid(strLinha, 5, 6))

                                'VAZÃO
                                sql += "UPDATE vazao " &
                                                      "SET valturbtran = " & Mid(strLinha, 18, 7) & ", " &
                                                      "valverttran = " & Mid(strLinha, 26, 7) & " "

                                If Trim(Mid(strLinha, 34, 7)) <> "" Then
                                    sql += ", valaflutran = " & Mid(strLinha, 34, 7) & " "
                                End If

                                If Trim(Mid(strLinha, 66, 7)) <> "" Then
                                    sql += ", valtransftran = " & Mid(strLinha, 66, 7) & " "
                                End If

                                sql += "WHERE datpdp = '" & strDataPdp & "' " &
                                                    "AND codusina = '" & strCodUsina & "';"

                                'COTA
                                '-- CRQ2345 (15/08/2012)
                                If Trim(Mid(strLinha, 42, 7)) <> "" Then
                                    Dim strSQLAux = "UPDATE cota " &
                                                          "SET cotainitran = " & Trim(Mid(strLinha, 42, 5)) & "." & Trim(Mid(strLinha, 47, 2))

                                    'cotafimtran = " & Trim(Mid(strLinha, 50, 5)) & "." & Trim(Mid(strLinha, 55, 2)) & ", " & _
                                    If (Trim(Mid(strLinha, 50, 5)) <> "" And Trim(Mid(strLinha, 55, 2)) <> "") Then
                                        strSQLAux = strSQLAux & ", cotafimtran = " & Trim(Mid(strLinha, 50, 5)) & "." & Trim(Mid(strLinha, 55, 2))
                                    End If

                                    'outrasestruturastran = " & Trim(Mid(strLinha, 58, 5)) & "." & Trim(Mid(strLinha, 63, 2)) & ", " & _
                                    If (Trim(Mid(strLinha, 58, 5)) <> "" And Trim(Mid(strLinha, 63, 2)) <> "") Then
                                        strSQLAux = strSQLAux & ", outrasestruturastran = " & Trim(Mid(strLinha, 58, 5)) & "." & Trim(Mid(strLinha, 63, 2))
                                    End If

                                    'comentariopdftran = '" & Trim(Mid(strLinha, 66, 256)) & "' " & _
                                    If (Trim(Mid(strLinha, 66, 256)) <> "") Then
                                        strSQLAux = strSQLAux & ", comentariopdftran = '" & Trim(Mid(strLinha, 66, 256)) & "' "
                                    End If

                                    strSQLAux = strSQLAux & " WHERE datpdp = '" & strDataPdp & "' " &
                                                                 "AND codusina = '" & strCodUsina & "';"

                                    sql += strSQLAux

                                    'Cmd.CommandText = "UPDATE cota " & _
                                    '                  "SET cotainitran = " & Trim(Mid(strLinha, 42, 5)) & "." & Trim(Mid(strLinha, 47, 2)) & ", " & _
                                    '                  "cotafimtran = " & Trim(Mid(strLinha, 50, 5)) & "." & Trim(Mid(strLinha, 55, 2)) & ", " & _
                                    '                  "outrasestruturastran = " & Trim(Mid(strLinha, 58, 5)) & "." & Trim(Mid(strLinha, 63, 2)) & ", " & _
                                    '                  "comentariopdftran = '" & Trim(Mid(strLinha, 66, 256)) & "' " & _
                                    '                  "WHERE datpdp = '" & strDataPdp & "' " & _
                                    '                  "AND codusina = '" & strCodUsina & "'"

                                End If

                                If bFlag_VAZ = False Then
                                    sql += GerarSqlGravaEventoPDP("6", strParamDataPdp, strSigEmpresa, Now, "PDPUpload", strUsuario)
                                    bFlag_VAZ = True
                                End If
                            Case Is = "IFX"
                                'Geração (ler os 48 valores da linha)
                                strCodUsina = Trim(Mid(strLinha, 5, 6))
                                ' os valores iniciam na coluna 18 com tamanho de 5
                                intPedaco = 18

                                For intI = 1 To 48
                                    sql += "UPDATE inflexibilidade " &
                                                          "SET valflexitran = " & Mid(strLinha, intPedaco, 5) & " " &
                                                          "WHERE datpdp = '" & strDataPdp & "' " &
                                                          "AND codusina = '" & strCodUsina & "' " &
                                                          "AND intflexi = " & intI & ";"

                                    intPedaco += 6
                                Next


                                If bFlag_IFX = False Then
                                    sql += GerarSqlGravaEventoPDP("2", strParamDataPdp, strSigEmpresa, Now, "PDPUpload", strUsuario)
                                    bFlag_IFX = True
                                End If
                            Case Is = "ZEN"
                                'Geração (ler os 48 valores da linha)
                                strCodUsina = Trim(Mid(strLinha, 5, 6))
                                ' os valores iniciam na coluna 18 com tamanho de 5
                                intPedaco = 18

                                For intI = 1 To 48
                                    sql += "UPDATE razaoener " &
                                                          "SET valrazenertran = " & Mid(strLinha, intPedaco, 5) & " " &
                                                          "WHERE datpdp = '" & strDataPdp & "' " &
                                                          "AND codusina = '" & strCodUsina & "' " &
                                                          "AND intrazener = " & intI & ";"

                                    intPedaco += 6
                                Next

                                If bFlag_ZEN = False Then
                                    sql += GerarSqlGravaEventoPDP("17", strParamDataPdp, strSigEmpresa, Now, "PDPUpload", strUsuario)
                                    bFlag_ZEN = True
                                End If
                            Case Is = "ZEL"
                                'Geração (ler os 48 valores da linha)
                                strCodUsina = Trim(Mid(strLinha, 5, 6))
                                ' os valores iniciam na coluna 18 com tamanho de 5
                                intPedaco = 18

                                For intI = 1 To 48
                                    sql += "UPDATE razaoelet " &
                                                          "SET valrazelettran = " & Mid(strLinha, intPedaco, 5) & " " &
                                                          "WHERE datpdp = '" & strDataPdp & "' " &
                                                          "AND codusina = '" & strCodUsina & "' " &
                                                          "AND intrazelet = " & intI & ";"

                                    intPedaco += 6
                                Next

                                If bFlag_ZEL = False Then
                                    sql += GerarSqlGravaEventoPDP("18", strParamDataPdp, strSigEmpresa, Now, "PDPUpload", strUsuario)
                                    bFlag_ZEL = True
                                End If
                            Case Is = "EXP"
                                'Geração (ler os 48 valores da linha)
                                strCodUsina = Trim(Mid(strLinha, 5, 6))
                                ' os valores iniciam na coluna 18 com tamanho de 5
                                intPedaco = 18

                                For intI = 1 To 48
                                    sql += "UPDATE exporta " &
                                                          "SET valexportatran = " & Mid(strLinha, intPedaco, 5) & " " &
                                                          "WHERE datpdp = '" & strDataPdp & "' " &
                                                          "AND codusina = '" & strCodUsina & "' " &
                                                          "AND intexporta = " & intI & ";"

                                    intPedaco += 6
                                Next

                                If bFlag_EXP = False Then
                                    sql += GerarSqlGravaEventoPDP("33", strParamDataPdp, strSigEmpresa, Now, "PDPUpload", strUsuario)
                                    bFlag_EXP = True
                                End If
                            Case Is = "ERP"
                                'Geração (ler os 48 valores da linha)
                                strCodUsina = Trim(Mid(strLinha, 5, 6))
                                ' os valores iniciam na coluna 18 com tamanho de 5
                                intPedaco = 18

                                For intI = 1 To 48
                                    sql += "UPDATE energia_reposicao " &
                                                          "SET valerptran = " & Mid(strLinha, intPedaco, 5) & " " &
                                                          "WHERE datpdp = '" & strDataPdp & "' " &
                                                          "AND codusina = '" & strCodUsina & "' " &
                                                          "AND interp = " & intI & ";"

                                    intPedaco += 6
                                Next

                                If bFlag_ERP = False Then
                                    sql += GerarSqlGravaEventoPDP("39", strParamDataPdp, strSigEmpresa, Now, "PDPUpload", strUsuario)
                                    bFlag_ERP = True
                                End If
                            Case Is = "IMP"
                                'Geração (ler os 48 valores da linha)
                                strCodUsina = Trim(Mid(strLinha, 5, 6))
                                ' os valores iniciam na coluna 18 com tamanho de 5
                                intPedaco = 18

                                For intI = 1 To 48
                                    sql += "UPDATE importa " &
                                                          "SET valimportatran = " & Mid(strLinha, intPedaco, 5) & " " &
                                                          "WHERE datpdp = '" & strDataPdp & "' " &
                                                          "AND codusina = '" & strCodUsina & "' " &
                                                          "AND intimporta = " & intI & ";"

                                    intPedaco += 6
                                Next

                                If bFlag_IMP = False Then
                                    sql += GerarSqlGravaEventoPDP("32", strParamDataPdp, strSigEmpresa, Now, "PDPUpload", strUsuario)
                                    bFlag_IMP = True
                                End If
                                '-- CRQ6867 - 23/09/2013
                            Case Is = "RES", "REO"
                                'Restrição (ler os 48 valores da linha)
                                strCodUsina = Trim(Mid(strLinha, 5, 6))
                                strCodGeradora = Trim(Mid(strLinha, 18, 12))

                                If Trim(strCodGeradora) <> "" Then
                                    sql += "INSERT INTO restrgerademp (" &
                                                        "codgerad, " &
                                                        "datinirestr, " &
                                                        "datfimrestr, " &
                                                        "intinirestr, " &
                                                        "intfimrestr, " &
                                                        "valrestr, " &
                                                        "id_motivorestricao, obsrestr, " &
                                                        "datpdp " &
                                                        ") VALUES (" &
                                                        "'" & strCodGeradora & "', " &
                                                        "'" & Mid(strLinha, 31, 8) & "', " &
                                                        "'" & Mid(strLinha, 43, 8) & "', " &
                                                        "'" & Mid(strLinha, 40, 2) & "', " &
                                                        "'" & Mid(strLinha, 52, 2) & "', " &
                                                        CInt(Mid(strLinha, 55, 5)) & ", " &
                                                        CInt(Mid(strLinha, 61, 3)) & ", '" & Mid(strLinha, 72, 255).Replace("'", "") & "', " &
                                                        "'" & strDataPdp & "');"
                                Else
                                    sql += "INSERT INTO restrusinaemp (" &
                                                        "codusina, " &
                                                        "datinirestr, " &
                                                        "datfimrestr, " &
                                                        "intinirestr, " &
                                                        "intfimrestr, " &
                                                        "valrestr, " &
                                                        "id_motivorestricao, obsrestr, " &
                                                        "datpdp " &
                                                        ") VALUES (" &
                                                        "'" & strCodUsina & "', " &
                                                        "'" & Mid(strLinha, 31, 8) & "', " &
                                                        "'" & Mid(strLinha, 43, 8) & "', " &
                                                        "'" & Mid(strLinha, 40, 2) & "', " &
                                                        "'" & Mid(strLinha, 52, 2) & "', " &
                                                        CInt(Mid(strLinha, 55, 5)) & ", " &
                                                        CInt(Mid(strLinha, 61, 3)) & ", '" & Mid(strLinha, 72, 255).Replace("'", "") & "', " &
                                                        "'" & strDataPdp & "');"
                                End If

                                If bFlag_RES = False Then
                                    sql += GerarSqlGravaEventoPDP("4", strParamDataPdp, strSigEmpresa, Now, "PDPUpload", strUsuario)
                                    bFlag_RES = True
                                End If

                            Case Is = "MAN"   ', "MAO"
                                'Restirção (ler os 48 valores da linha)
                                strCodUsina = Trim(Mid(strLinha, 5, 6))
                                strCodGeradora = Trim(Mid(strLinha, 18, 12))


                                sql += "INSERT INTO paralemp (" &
                                                    "codequip, " &
                                                    "tipequip, " &
                                                    "datiniparal, " &
                                                    "datfimparal, " &
                                                    "intiniparal, " &
                                                    "intfimparal, " &
                                                    "codnivel, " &
                                                    "indcont, " &
                                                    "intinivoltaparal, " &
                                                    "intfimvoltaparal, " &
                                                    "datpdp " &
                                                    ") VALUES (" &
                                                    "'" & strCodGeradora & "', " &
                                                    "'GD', " &
                                                    "'" & Mid(strLinha, 31, 8) & "', " &
                                                    "'" & Mid(strLinha, 43, 8) & "', " &
                                                    "'" & Mid(strLinha, 40, 2) & "', " &
                                                    "'" & Mid(strLinha, 52, 2) & "', " &
                                                    "'" & Trim(Mid(strLinha, 55, 3)) & "', " &
                                                    "'" & Trim(Mid(strLinha, 65, 1)) & "', " &
                                                    CInt(Val(Trim(Mid(strLinha, 59, 2)))) & ", " &
                                                    CInt(Val(Trim(Mid(strLinha, 62, 2)))) & ", " &
                                                    "'" & strDataPdp & "');"

                                If bFlag_MAN = False Then
                                    sql += GerarSqlGravaEventoPDP("5", strParamDataPdp, strSigEmpresa, Now, "PDPUpload", strUsuario)
                                    bFlag_MAN = True
                                End If
                            Case Is = "PCO" 'Parada por Conveniência Operativa
                                strCodUsina = Trim(Mid(strLinha, 5, 6))
                                strCodGeradora = Trim(Mid(strLinha, 18, 12))

                                sql += "INSERT INTO paralemp_co (" &
                                                    "codequip, tipequip, datiniparal, datfimparal, " &
                                                    "intiniparal, intfimparal, datpdp " &
                                                    ") VALUES (" &
                                                    "'" & strCodGeradora & "', " &
                                                    "'GD', " &
                                                    "'" & Mid(strLinha, 31, 8) & "', " &
                                                    "'" & Mid(strLinha, 43, 8) & "', " &
                                                    "'" & Mid(strLinha, 40, 2) & "', " &
                                                    "'" & Mid(strLinha, 52, 2) & "', " &
                                                    "'" & strDataPdp & "');"

                                If bFlag_PCO = False Then
                                    'Grava evento registrando o recebimento de Parada de Maq. por Conveniência Operativa
                                    sql += GerarSqlGravaEventoPDP("49", strParamDataPdp, strSigEmpresa, Now, "PDPUpload", strUsuario)
                                    bFlag_PCO = True
                                End If
                            Case Is = "MRE"
                                ' ##### MOTIVO DE DESPACHO RAZÃO ELÉTRICA #####
                                strCodUsina = Trim(Mid(strLinha, 5, 6))
                                ' os valores iniciam na coluna 18 com tamanho de 5
                                intPedaco = 18

                                For intI = 1 To 48
                                    sql += "UPDATE motivorel " &
                                                          "SET valmretran = " & Mid(strLinha, intPedaco, 5) & " " &
                                                          "WHERE datpdp = '" & strDataPdp & "' " &
                                                          "AND codusina = '" & strCodUsina & "' " &
                                                          "AND intmre = " & intI & ";"

                                    intPedaco += 6
                                Next

                                If bFlag_MRE = False Then
                                    sql += GerarSqlGravaEventoPDP("34", strParamDataPdp, strSigEmpresa, Now, "PDPUpload", strUsuario)
                                    bFlag_MRE = True
                                End If
                            Case Is = "MIF"
                                ' ##### MOTIVO DE DESPACHO INFLEXIBILIDADE #####
                                strCodUsina = Trim(Mid(strLinha, 5, 6))
                                ' os valores iniciam na coluna 18 com tamanho de 5
                                intPedaco = 18

                                For intI = 1 To 48
                                    sql += "UPDATE motivoinfl " &
                                                          "SET valmiftran = " & Mid(strLinha, intPedaco, 5) & " " &
                                                          "WHERE datpdp = '" & strDataPdp & "' " &
                                                          "AND codusina = '" & strCodUsina & "' " &
                                                          "AND intmif = " & intI & ";"

                                    intPedaco += 6
                                Next

                                If bFlag_MIF = False Then
                                    sql += GerarSqlGravaEventoPDP("48", strParamDataPdp, strSigEmpresa, Now, "PDPUpload", strUsuario)
                                    bFlag_MIF = True
                                End If
                            Case Is = "PCC"
                                ' ##### PERDAS CONSUMO INTERNO E COMPENSAÇÃO #####
                                strCodUsina = Trim(Mid(strLinha, 5, 6))
                                ' os valores iniciam na coluna 18 com tamanho de 5
                                intPedaco = 18

                                For intI = 1 To 48
                                    sql += "UPDATE perdascic " &
                                                          "SET valpcctran = " & Mid(strLinha, intPedaco, 5) & " " &
                                                          "WHERE datpdp = '" & strDataPdp & "' " &
                                                          "AND codusina = '" & strCodUsina & "' " &
                                                          "AND intpcc = " & intI & ";"

                                    intPedaco += 6
                                Next

                                If bFlag_PCC = False Then
                                    sql += GerarSqlGravaEventoPDP("35", strParamDataPdp, strSigEmpresa, Now, "PDPUpload", strUsuario)
                                    bFlag_PCC = True
                                End If
                            Case Is = "MCO"
                                ' ##### NÚMERO MÁQUINAS PARADA POR CONVENIÊNCIA OPERATIVA #####
                                strCodUsina = Trim(Mid(strLinha, 5, 6))
                                ' os valores iniciam na coluna 18 com tamanho de 5
                                intPedaco = 18

                                For intI = 1 To 48
                                    sql += "UPDATE conveniencia_oper " &
                                                          "SET valmcotran = " & Mid(strLinha, intPedaco, 5) & " " &
                                                          "WHERE datpdp = '" & strDataPdp & "' " &
                                                          "AND codusina = '" & strCodUsina & "' " &
                                                          "AND intmco = " & intI & ";"

                                    intPedaco += 6
                                Next

                                If bFlag_MCO = False Then
                                    sql += GerarSqlGravaEventoPDP("36", strParamDataPdp, strSigEmpresa, Now, "PDPUpload", strUsuario)
                                    bFlag_MCO = True
                                End If
                            Case Is = "MOS"
                                ' ##### NÚMERO MÁQUINAS OPERANDO COMO SÍNCRONO #####
                                strCodUsina = Trim(Mid(strLinha, 5, 6))
                                ' os valores iniciam na coluna 18 com tamanho de 5
                                intPedaco = 18

                                For intI = 1 To 48
                                    sql += "UPDATE oper_sincrono " &
                                                          "SET valmostran = " & Mid(strLinha, intPedaco, 5) & " " &
                                                          "WHERE datpdp = '" & strDataPdp & "' " &
                                                          "AND codusina = '" & strCodUsina & "' " &
                                                          "AND intmos = " & intI & ";"

                                    intPedaco += 6
                                Next

                                If bFlag_MOS = False Then
                                    sql += GerarSqlGravaEventoPDP("37", strParamDataPdp, strSigEmpresa, Now, "PDPUpload", strUsuario)
                                    bFlag_MOS = True
                                End If
                            Case Is = "MEG"
                                ' ##### NÚMERO MÁQUINAS GERANDO #####
                                strCodUsina = Trim(Mid(strLinha, 5, 6))
                                ' os valores iniciam na coluna 18 com tamanho de 5
                                intPedaco = 18

                                For intI = 1 To 48
                                    sql += "UPDATE maq_gerando " &
                                                          "SET valmegtran = " & Mid(strLinha, intPedaco, 5) & " " &
                                                          "WHERE datpdp = '" & strDataPdp & "' " &
                                                          "AND codusina = '" & strCodUsina & "' " &
                                                          "AND intmeg = " & intI & ";"

                                    intPedaco += 6
                                Next

                                If bFlag_MEG = False Then
                                    sql += GerarSqlGravaEventoPDP("38", strParamDataPdp, strSigEmpresa, Now, "PDPUpload", strUsuario)
                                    bFlag_MEG = True
                                End If
                            Case Is = "DSP"
                                'Disponibilidade (ler os 48 valores da linha)
                                strCodUsina = Trim(Mid(strLinha, 5, 6))
                                ' os valores iniciam na coluna 18 com tamanho de 6
                                intPedaco = 18

                                For intI = 1 To 48
                                    sql += "UPDATE disponibilidade " &
                                                          "SET valdsptran = " & Mid(strLinha, intPedaco, 5) & " " &
                                                          "WHERE datpdp = '" & strDataPdp & "' " &
                                                          "AND codusina = '" & strCodUsina & "' " &
                                                          "AND intdsp = " & intI & ";"

                                    intPedaco += 6
                                Next

                                If bFlag_DSP = False Then
                                    sql += GerarSqlGravaEventoPDP("46", strParamDataPdp, strSigEmpresa, Now, "PDPUpload", strUsuario)
                                    bFlag_DSP = True
                                End If
                            Case Is = "CLF"
                                'Compensacao de Lastro Físico (ler os 48 valores da linha)
                                strCodUsina = Trim(Mid(strLinha, 5, 6))
                                ' os valores iniciam na coluna 18 com tamanho de 6
                                intPedaco = 18

                                For intI = 1 To 48
                                    sql += "UPDATE complastro_fisico " &
                                                          "SET valclftran = " & Mid(strLinha, intPedaco, 5) & " " &
                                                          "WHERE datpdp = '" & strDataPdp & "' " &
                                                          "AND codusina = '" & strCodUsina & "' " &
                                                          "AND intclf = " & intI & ";"

                                    intPedaco += 6
                                Next

                                If bFlag_CLF = False Then
                                    sql += GerarSqlGravaEventoPDP("47", strParamDataPdp, strSigEmpresa, Now, "PDPUpload", strUsuario)
                                    bFlag_CLF = True
                                End If

                            Case Is = "RFC"
                                ' ##### RESTRIÇÃO POR FALTA DE COMBUSTÍVEL (RFC) #####
                                strCodUsina = Trim(Mid(strLinha, 5, 6))
                                ' os valores iniciam na coluna 18 com tamanho de 5
                                intPedaco = 18

                                For intI = 1 To 48
                                    sql += "UPDATE rest_falta_comb " &
                                                          "SET valrfctran = " & Mid(strLinha, intPedaco, 5) & " " &
                                                          "WHERE datpdp = '" & strDataPdp & "' " &
                                                          "AND codusina = '" & strCodUsina & "' " &
                                                          "AND intrfc = " & intI & ";"

                                    intPedaco += 6
                                Next

                                If bFlag_RFC = False Then
                                    sql += GerarSqlGravaEventoPDP("51", strParamDataPdp, strSigEmpresa, Now, "PDPUpload", strUsuario)
                                    bFlag_RFC = True
                                End If

                            Case Is = "RMP"
                                ' ##### GARANTIA ENERGÉTICA (RMP) #####
                                strCodUsina = Trim(Mid(strLinha, 5, 6))
                                ' os valores iniciam na coluna 18 com tamanho de 5
                                intPedaco = 18

                                For intI = 1 To 48
                                    sql += "UPDATE tb_rmp " &
                                                          "SET valrmptran = " & Mid(strLinha, intPedaco, 5) & " " &
                                                          "WHERE datpdp = '" & strDataPdp & "' " &
                                                          "AND codusina = '" & strCodUsina & "' " &
                                                          "AND intrmp = " & intI & ";"

                                    intPedaco += 6
                                Next

                                If bFlag_RMP = False Then
                                    sql += GerarSqlGravaEventoPDP("52", strParamDataPdp, strSigEmpresa, Now, "PDPUpload", strUsuario)
                                    bFlag_RMP = True
                                End If

                            Case Is = "GFM"
                                ' ##### GERAÇÃO FORA DE MÉRITO (GFM) #####
                                strCodUsina = Trim(Mid(strLinha, 5, 6))
                                ' os valores iniciam na coluna 18 com tamanho de 5
                                intPedaco = 18

                                For intI = 1 To 48
                                    sql += "UPDATE tb_gfm " &
                                                          "SET valgfmtran = " & Mid(strLinha, intPedaco, 5) & " " &
                                                          "WHERE datpdp = '" & strDataPdp & "' " &
                                                          "AND codusina = '" & strCodUsina & "' " &
                                                          "AND intgfm = " & intI & ";"

                                    intPedaco += 6
                                Next

                                If bFlag_GFM = False Then
                                    sql += GerarSqlGravaEventoPDP("53", strParamDataPdp, strSigEmpresa, Now, "PDPUpload", strUsuario)
                                    bFlag_GFM = True
                                End If

                            Case Is = "CFM"
                                ' ##### Crédito por Substituição (CFM) #####
                                strCodUsina = Trim(Mid(strLinha, 5, 6))
                                ' os valores iniciam na coluna 18 com tamanho de 5
                                intPedaco = 18

                                For intI = 1 To 48
                                    sql += "UPDATE tb_cfm " &
                                                          "SET valcfmtran = " & Mid(strLinha, intPedaco, 5) & " " &
                                                          "WHERE datpdp = '" & strDataPdp & "' " &
                                                          "AND codusina = '" & strCodUsina & "' " &
                                                          "AND intcfm = " & intI & ";"

                                    intPedaco += 6
                                Next

                                If bFlag_CFM = False Then
                                    sql += GerarSqlGravaEventoPDP("54", strParamDataPdp, strSigEmpresa, Now, "PDPUpload", strUsuario)
                                    bFlag_CFM = True
                                End If

                            Case Is = "SOM"
                                ' ##### Geração Substituta (SOM) #####
                                strCodUsina = Trim(Mid(strLinha, 5, 6))
                                ' os valores iniciam na coluna 18 com tamanho de 5
                                intPedaco = 18

                                For intI = 1 To 48
                                    sql += "UPDATE tb_som " &
                                                          "SET valsomtran = " & Mid(strLinha, intPedaco, 5) & " " &
                                                          "WHERE datpdp = '" & strDataPdp & "' " &
                                                          "AND codusina = '" & strCodUsina & "' " &
                                                          "AND intsom = " & intI & ";"
                                    intPedaco += 6
                                Next

                                If bFlag_SOM = False Then
                                    sql += GerarSqlGravaEventoPDP("55", strParamDataPdp, strSigEmpresa, Now, "PDPUpload", strUsuario)
                                    bFlag_SOM = True
                                End If

                            Case Is = "GES"
                                ' ##### GE SUBSTITUIÇÃO (GES) #####
                                strCodUsina = Trim(Mid(strLinha, 5, 6))
                                ' os valores iniciam na coluna 18 com tamanho de 5
                                intPedaco = 18

                                For intI = 1 To 48
                                    sql += "UPDATE tb_GES " &
                                                          "SET valGEStran = " & Mid(strLinha, intPedaco, 5) & " " &
                                                          "WHERE datpdp = '" & strDataPdp & "' " &
                                                          "AND codusina = '" & strCodUsina & "' " &
                                                          "AND intGES = " & intI & ";"

                                    intPedaco += 6
                                Next

                                If bFlag_GES = False Then
                                    sql += GerarSqlGravaEventoPDP("56", strParamDataPdp, strSigEmpresa, Now, "PDPUpload", strUsuario)
                                    bFlag_GES = True
                                End If

                            Case Is = "GEC"
                                ' ##### GE CRÉDITO (GEC) #####
                                strCodUsina = Trim(Mid(strLinha, 5, 6))
                                ' os valores iniciam na coluna 18 com tamanho de 5
                                intPedaco = 18

                                For intI = 1 To 48
                                    sql += "UPDATE tb_GEC " &
                                                          "SET valGECtran = " & Mid(strLinha, intPedaco, 5) & " " &
                                                          "WHERE datpdp = '" & strDataPdp & "' " &
                                                          "AND codusina = '" & strCodUsina & "' " &
                                                          "AND intGEC = " & intI & ";"

                                    intPedaco += 6
                                Next

                                If bFlag_GEC = False Then
                                    sql += GerarSqlGravaEventoPDP("57", strParamDataPdp, strSigEmpresa, Now, "PDPUpload", strUsuario)
                                    bFlag_GEC = True
                                End If

                            Case Is = "DCA"
                                ' ##### DESPACHO CICLO ABERTO (DCA) #####
                                strCodUsina = Trim(Mid(strLinha, 5, 6))
                                ' os valores iniciam na coluna 18 com tamanho de 5
                                intPedaco = 18

                                For intI = 1 To 48
                                    sql += "UPDATE tb_DCA " &
                                                          "SET valDCAtran = " & Mid(strLinha, intPedaco, 5) & " " &
                                                          "WHERE datpdp = '" & strDataPdp & "' " &
                                                          "AND codusina = '" & strCodUsina & "' " &
                                                          "AND intDCA = " & intI & ";"
                                    intPedaco += 6
                                Next

                                If bFlag_DCA = False Then
                                    sql += GerarSqlGravaEventoPDP("58", strParamDataPdp, strSigEmpresa, Now, "PDPUpload", strUsuario)
                                    bFlag_DCA = True
                                End If

                            Case Is = "DCR"
                                ' ##### DESPACHO CICLO REDUZIDO (DCR) #####
                                strCodUsina = Trim(Mid(strLinha, 5, 6))
                                ' os valores iniciam na coluna 18 com tamanho de 5
                                intPedaco = 18

                                For intI = 1 To 48
                                    sql += "UPDATE tb_DCR " &
                                                          "SET valDCRtran = " & Mid(strLinha, intPedaco, 5) & " " &
                                                          "WHERE datpdp = '" & strDataPdp & "' " &
                                                          "AND codusina = '" & strCodUsina & "' " &
                                                          "AND intDCR = " & intI & ";"

                                    intPedaco += 6
                                Next

                                If bFlag_DCR = False Then
                                    sql += GerarSqlGravaEventoPDP("59", strParamDataPdp, strSigEmpresa, Now, "PDPUpload", strUsuario)
                                    bFlag_DCR = True
                                End If

                            Case Is = "NPA"
                                ' ##### INSUMO RESERVA 1 (IR1) #####
                                strCodUsina = Trim(Mid(strLinha, 5, 6))
                                ' os valores iniciam na coluna 18 com tamanho de 5
                                intPedaco = 18

                                sql += "UPDATE tb_IR1 " &
                                                          "SET valIR1tran = " & Mid(strLinha, intPedaco, 5) & " " &
                                                          "WHERE datpdp = '" & strDataPdp & "' " &
                                                          "AND codusina = '" & strCodUsina & "'; "

                                intPedaco += 6

                                If bFlag_IR1 = False Then
                                    sql += GerarSqlGravaEventoPDP("60", strParamDataPdp, strSigEmpresa, Now, "PDPUpload", strUsuario)
                                    bFlag_IR1 = True
                                End If

                            Case Is = "R11"
                                ' ##### INSUMO RESERVA 2 (IR2) #####
                                strCodUsina = Trim(Mid(strLinha, 5, 6))
                                ' os valores iniciam na coluna 18 com tamanho de 5
                                intPedaco = 18

                                For intI = 1 To 48
                                    sql += "UPDATE tb_IR2 " &
                                                          "SET valIR2tran = " & Mid(strLinha, intPedaco, 5) & " " &
                                                          "WHERE datpdp = '" & strDataPdp & "' " &
                                                          "AND codusina = '" & strCodUsina & "' " &
                                                          "AND intIR2 = " & intI & ";"

                                    intPedaco += 6
                                Next

                                If bFlag_IR2 = False Then
                                    sql += GerarSqlGravaEventoPDP("61", strParamDataPdp, strSigEmpresa, Now, "PDPUpload", strUsuario)
                                    bFlag_IR2 = True
                                End If

                            Case Is = "R12"
                                ' ##### INSUMO RESERVA 3 (IR3) #####
                                strCodUsina = Trim(Mid(strLinha, 5, 6))
                                ' os valores iniciam na coluna 18 com tamanho de 5
                                intPedaco = 18

                                For intI = 1 To 48
                                    sql += "UPDATE tb_IR3 " &
                                                          "SET valIR3tran = " & Mid(strLinha, intPedaco, 5) & " " &
                                                          "WHERE datpdp = '" & strDataPdp & "' " &
                                                          "AND codusina = '" & strCodUsina & "' " &
                                                          "AND intIR3 = " & intI & ";"

                                    intPedaco += 6
                                Next

                                If bFlag_IR3 = False Then
                                    sql += GerarSqlGravaEventoPDP("62", strParamDataPdp, strSigEmpresa, Now, "PDPUpload", strUsuario)
                                    bFlag_IR3 = True
                                End If

                            Case Is = "CAN"
                                ' ##### INSUMO RESERVA 4 (IR4) #####
                                strCodUsina = Trim(Mid(strLinha, 5, 6))
                                ' os valores iniciam na coluna 18 com tamanho de 5
                                intPedaco = 18

                                For intI = 1 To 48
                                    sql += "UPDATE tb_IR4 " &
                                                          "SET valIR4tran = " & Mid(strLinha, intPedaco, 5) & " " &
                                                          "WHERE datpdp = '" & strDataPdp & "' " &
                                                          "AND codusina = '" & strCodUsina & "' " &
                                                          "AND intIR4 = " & intI & ";"

                                    intPedaco += 6
                                Next

                                If bFlag_IR4 = False Then
                                    sql += GerarSqlGravaEventoPDP("63", strParamDataPdp, strSigEmpresa, Now, "PDPUpload", strUsuario)
                                    bFlag_IR4 = True
                                End If

                        End Select
                    End If
                End If
            End While

            If Not IsDessem Then
                Cmd.CommandText = sql
                Cmd.ExecuteNonQuery()
            End If

            FileClose(intArq)
            'Elimina o arquivo após a gravação na base
            Kill(strArq)
            oReader.Dispose()
            Cmd.Dispose()
            Conn.Close()
            Conn.Dispose()

            UpLoadArquivoTexto = True
        Catch ex As Exception
            strRetorno = "Não foi possível fazer o upload. Mensagem: " & ex.Message & " Comando: " & Cmd.CommandText
            FileClose(intArq)

            Cmd.Dispose()
            Conn.Close()
            Conn.Dispose()
            UpLoadArquivoTexto = False
        End Try
    End Function

    'Public Function RemoverComentariosDESSEM(strDataPdp As String, strCodUsina As String, idInsumo As String, ByRef Cmd As SqlCommand) As SqlCommand

    '    If Not IsNothing(Cmd) Then

    '        Cmd.Parameters.Clear()
    '        Cmd.CommandText =
    '                    " delete from tb_comentario_dessem_patamar 
    '                                            Where id_comentario In (Select id_comentario from tb_comentario_dessem where flg_aprovado is null 
    '                                                   and id_insumo = " + idInsumo + "
    '                                                   and dat_pdp = '" + strDataPdp + "'
    '                                                   and cod_usina = '" + strCodUsina + "')"

    '        Cmd.ExecuteNonQuery()

    '        Cmd.CommandText =
    '                    " delete from tb_comentario_dessem 
    '                                            where flg_aprovado is null 
    '                                                   and id_insumo = " + idInsumo + "
    '                                                   and dat_pdp = '" + strDataPdp + "'
    '                                                   and cod_usina = '" + strCodUsina + "'"

    '        Cmd.ExecuteNonQuery()

    '        Try
    '            Cmd.ExecuteNonQuery()
    '        Catch ex As Exception
    '            Throw ex 'para DEBUG
    '        End Try

    '    Else
    '        Throw New Exception("RemoverComentariosDESSEM - Command está nulo.")
    '    End If

    '    Return Cmd

    'End Function

    ' Tive que fazer essa sobrecarga pois a referência do método original tinham muitas mudanças a serem feitas, em cascata
    ' O ideal é futuramente unificar os 2 métodos utilizando o parâmetro do sql server
    Public Function RemoverComentariosDESSEM(strDataPdp As String, strCodUsina As String, idInsumo As String, ByRef Cmd As System.Data.SqlClient.SqlCommand) As System.Data.SqlClient.SqlCommand

        If Not IsNothing(Cmd) Then

            Cmd.Parameters.Clear()

            Cmd.CommandText =
                        " delete from tb_comentario_dessem_patamar 
                                                Where id_comentario In (Select id_comentario from tb_comentario_dessem where flg_aprovado is null 
                                                      " & IIf(idInsumo <> String.Empty, " and id_insumo = " + idInsumo, String.Empty) & " 
                                                       and dat_pdp = '" + strDataPdp + "'
                                                       and cod_usina = '" + strCodUsina + "')"

            Cmd.ExecuteNonQuery()

            Cmd.CommandText =
                        " delete from tb_comentario_dessem 
                                                where flg_aprovado is null 
                                                       " & IIf(idInsumo <> String.Empty, " and id_insumo = " + idInsumo, String.Empty) & "
                                                       and dat_pdp = '" + strDataPdp + "'
                                                       and cod_usina = '" + strCodUsina + "'"

            Cmd.ExecuteNonQuery()

            Try
                Cmd.ExecuteNonQuery()
            Catch ex As Exception
                Throw ex 'para DEBUG
            End Try

        Else
            Throw New Exception("RemoverComentariosDESSEM - Command está nulo.")
        End If

        Return Cmd

    End Function

    ' Grava os dados na base 
    Public Function UpLoadArquivoTextoRelatorio(ByRef strArq As String,
                                                ByVal strParamDataPdp As String,
                                                ByRef strRetorno As String) As Boolean
        Dim Conn As SqlConnection = New SqlConnection
        Dim Cmd As SqlCommand = New SqlCommand

        Dim oReader As SqlDataReader
        Dim objTrans As SqlTransaction
        Dim strTexto As System.String
        Dim strLinha As String
        Dim strTipoArq As String
        Dim strDataPdp As String
        Dim strCodArea As String
        Dim strValReq As String
        Dim strHorReq As String
        Dim strValRes As String
        Dim strHorRes As String
        Dim intArq As Integer
        Dim intI As Integer
        Dim intPedaco As Integer
        Dim blnPrimeiro As Boolean
        Dim blnPotSincronizada As Boolean
        Cmd.Connection = Conn
        Cmd.CommandTimeout = 180

        'abrir o arquivo
        intArq = FreeFile()
        FileOpen(intArq, strArq, OpenMode.Input, OpenAccess.Read)

        ' Ler todas as linhas do arquivo
        Try
            blnPrimeiro = True
            While Not EOF(intArq)
                strLinha = LineInput(intArq)
                If blnPrimeiro Then
                    strDataPdp = Mid(strLinha, 16, 8)
                    strTipoArq = Mid(strLinha, 25, 6)
                    strCodArea = Mid(strLinha, 41, 2).Trim

                    ' Verificar se a data escolhida confere com o arquivo
                    If (Trim(strDataPdp) <> Trim(strParamDataPdp)) Then
                        strRetorno = "A Data do PDP selecionada não confere com a data do arquivo."
                        FileClose(intArq)
                        Cmd.Dispose()
                        Conn.Close()
                        Conn.Dispose()
                        UpLoadArquivoTextoRelatorio = False
                        Exit Function
                    End If

                    ' verificar se a Área colocada no arquivo existe
                    Cmd.CommandText = "SELECT codarea " &
                                      "FROM area " &
                                      "WHERE codarea = '" & strCodArea & "'"
                    oReader = Cmd.ExecuteReader
                    If Not (oReader.Read()) Then
                        oReader.Close()
                        oReader.Dispose()
                        strRetorno = "Código da Área no arquivo inexistente."
                        FileClose(intArq)
                        Cmd.Dispose()
                        Conn.Close()
                        Conn.Dispose()
                        UpLoadArquivoTextoRelatorio = False
                        Exit Function
                    End If
                    oReader.Close()

                    ' verificar se a data do pdp foi inicializada
                    Cmd.CommandText = "SELECT datpdp " &
                                      "FROM pdp " &
                                      "WHERE datpdp = '" & strDataPdp & "'"
                    oReader = Cmd.ExecuteReader
                    If Not (oReader.Read()) Then
                        oReader.Close()
                        oReader.Dispose()
                        strRetorno = "A data do PDP que está no arquivo não foi inicializada."
                        FileClose(intArq)
                        Cmd.Dispose()
                        Conn.Close()
                        Conn.Dispose()
                        UpLoadArquivoTextoRelatorio = False
                        Exit Function
                    End If
                    oReader.Close()

                    blnPrimeiro = False

                    objTrans = Conn.BeginTransaction
                    Cmd.Transaction = objTrans

                    ' Apagar os registros de Requisito, Reserva e Potencia para executar a carga novamente
                    Cmd.CommandText = "DELETE FROM requisitos_area " &
                                      "WHERE datpdp = '" & strDataPdp & "' " &
                                      "AND codarea = '" & strCodArea & "'"
                    Cmd.ExecuteNonQuery()

                    Cmd.CommandText = "DELETE FROM potsincronizada " &
                                      "WHERE datpdp = '" & strDataPdp & "' " &
                                      "AND codarea = '" & strCodArea & "'"
                    Cmd.ExecuteNonQuery()

                Else

                    Select Case strLinha.Substring(0, 3).Trim
                        Case Is = "REQ"
                            strValReq = Mid(strLinha, 5, 6).Trim
                            strHorReq = Mid(strLinha, 12, 5)

                            If strValReq = String.Empty Then
                                strRetorno = "Valor do Requisito inexistente no arquivo."
                                FileClose(intArq)
                                objTrans.Rollback()
                                Cmd.Dispose()
                                Conn.Close()
                                Conn.Dispose()
                                UpLoadArquivoTextoRelatorio = False
                                Exit Function
                            End If

                            If strHorReq = String.Empty Then
                                strRetorno = "Horário do Requisito inexistente no arquivo."
                                FileClose(intArq)
                                objTrans.Rollback()
                                Cmd.Dispose()
                                Conn.Close()
                                Conn.Dispose()
                                UpLoadArquivoTextoRelatorio = False
                                Exit Function
                            Else
                                If Mid(strHorReq, 1, 2) > 23 Or Mid(strHorReq, 4, 2) > 59 Then
                                    strRetorno = "Horário do Requisito inválido no arquivo."
                                    FileClose(intArq)
                                    objTrans.Rollback()
                                    Cmd.Dispose()
                                    Conn.Close()
                                    Conn.Dispose()
                                    UpLoadArquivoTextoRelatorio = False
                                    Exit Function
                                End If
                            End If

                        Case Is = "RES"
                            strValRes = Mid(strLinha, 5, 6).Trim
                            strHorRes = Mid(strLinha, 12, 5)

                            If strValRes = String.Empty Then
                                strRetorno = "Valor da Reserva inexistente no arquivo."
                                FileClose(intArq)
                                objTrans.Rollback()
                                Cmd.Dispose()
                                Conn.Close()
                                Conn.Dispose()
                                UpLoadArquivoTextoRelatorio = False
                                Exit Function
                            End If

                            If strHorRes = String.Empty Then
                                strRetorno = "Horário da Reserva inexistente no arquivo."
                                FileClose(intArq)
                                objTrans.Rollback()
                                Cmd.Dispose()
                                Conn.Close()
                                Conn.Dispose()
                                UpLoadArquivoTextoRelatorio = False
                                Exit Function
                            Else
                                If Mid(strHorRes, 1, 2) > 23 Or Mid(strHorRes, 4, 2) > 59 Then
                                    strRetorno = "Horário da Reserva inválido no arquivo."
                                    FileClose(intArq)
                                    objTrans.Rollback()
                                    Cmd.Dispose()
                                    Conn.Close()
                                    Conn.Dispose()
                                    UpLoadArquivoTextoRelatorio = False
                                    Exit Function
                                End If
                            End If

                        Case Is = "PTS"
                            'Potencia Sincronizada
                            blnPotSincronizada = True
                            intPedaco = 5
                            For intI = 1 To 48
                                If Mid(strLinha, intPedaco, 6).Trim <> String.Empty Then
                                    Cmd.CommandText = "INSERT INTO potsincronizada (datpdp, codarea, intpotsincronizada, valpotsincronizadasup) " &
                                                      "VALUES (" &
                                                      "'" & strDataPdp & "', " &
                                                      "'" & strCodArea & "', " &
                                                      "" & intI & ", " &
                                                      "" & Mid(strLinha, intPedaco, 6) & ")"
                                    Cmd.ExecuteNonQuery()
                                    intPedaco += 6
                                Else
                                    strRetorno = "Valor da Potência Sincronizada inválida no arquivo."
                                    FileClose(intArq)
                                    objTrans.Rollback()
                                    Cmd.Dispose()
                                    Conn.Close()
                                    Conn.Dispose()
                                    UpLoadArquivoTextoRelatorio = False
                                    Exit Function
                                End If
                            Next
                    End Select
                End If
            End While

            If blnPotSincronizada Then
                Cmd.CommandText = "INSERT INTO requisitos_area (datpdp, codarea, valreqmax, hreqmax, valresreqmax, hresreqmax) " &
                                  "VALUES (" &
                                  "'" & strDataPdp & "', " &
                                  "'" & strCodArea & "', " &
                                  "" & strValReq & ", " &
                                  "'" & strHorReq & "', " &
                                  "" & strValRes & ", " &
                                  "'" & strHorRes & "')"
                Cmd.ExecuteNonQuery()
                objTrans.Commit()
            Else
                strRetorno = "Valor da Potência Sincronizada inexistente no arquivo."
                FileClose(intArq)
                objTrans.Rollback()
                Cmd.Dispose()
                Conn.Close()
                Conn.Dispose()
                UpLoadArquivoTextoRelatorio = False
                Exit Function
            End If

            FileClose(intArq)
            'Elimina o arquivo após a gravação na base
            Kill(strArq)
            oReader.Dispose()
            Cmd.Dispose()
            Conn.Close()
            Conn.Dispose()

            UpLoadArquivoTextoRelatorio = True
        Catch ex As Exception
            strRetorno = "Não foi possível fazer o upload." & ex.Message & Cmd.CommandText
            FileClose(intArq)
            Try
                'Se a transação não estiver aberta vai ocorrer erro
                objTrans.Rollback()
            Catch
            End Try
            Cmd.Dispose()
            Conn.Close()
            Conn.Dispose()
            UpLoadArquivoTextoRelatorio = False
        End Try
    End Function

    Public Sub GravaEventoPDP(ByVal strCodStatus As String,
                              ByVal strData As String,
                              ByVal strSigEmpresa As String,
                              ByVal datAtual As Date,
                              ByVal strServico As String,
                              ByVal strUsuario As String)
        'Parâmetros
        'strCodStatus -> Código do evento a ser gravado: Carga, Geração, etc...
        'strData -> data do pdp
        'strCodEmpresa -> Código da empresa
        'datAtual -> Data atual 
        'strServico -> Servico em que a função está sendo chamada (usado na auditoria)
        'strUsuario -> Usuário que está usando o aplicativo (usado na auditoria)
        Dim Conn As SqlConnection = New SqlConnection
        Dim Cmd As SqlCommand = New SqlCommand
        Conn.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
        Cmd.Connection = Conn
        Conn.Open()
        Try

            Cmd.CommandText = "INSERT INTO eventpdp (" &
                              "datpdp, " &
                              "dthevent, " &
                              "codstatu, " &
                              "cmpevent" &
                              ") VALUES (" &
                              "'" & strData & "', " &
                              "'" & Format(datAtual, "yyyy-MM-dd hh:mm:ss") & "', " &
                              "'" & strCodStatus & "', " &
                              "'" & strSigEmpresa & "')"
            Cmd.ExecuteNonQuery()
            'Cmd.Connection.Close()
            'Conn.Close()
        Catch ex As Exception
            If Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If
            Throw New Exception(ex.Message)
        Finally
            If Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If
        End Try
    End Sub

    Public Function GerarSqlGravaEventoPDP(ByVal strCodStatus As String,
                              ByVal strData As String,
                              ByVal strSigEmpresa As String,
                              ByVal datAtual As Date,
                              ByVal strServico As String,
                              ByVal strUsuario As String) As String
        'Parâmetros
        'strCodStatus -> Código do evento a ser gravado: Carga, Geração, etc...
        'strData -> data do pdp
        'strCodEmpresa -> Código da empresa
        'datAtual -> Data atual 

        Dim sql As String = ""

        sql = "INSERT INTO eventpdp (" &
                              "datpdp, " &
                              "dthevent, " &
                              "codstatu, " &
                              "cmpevent" &
                              ") VALUES (" &
                              "'" & strData & "', " &
                              "'" & Format(datAtual, "yyyy-MM-dd hh:mm:ss.fff") & "', " &
                              "'" & strCodStatus & "', " &
                              "'" & strSigEmpresa & "');"

        GerarSqlGravaEventoPDP = sql

    End Function

    'Public Function PreencheComboEmpresa(ByVal strUsuar As String,
    '                                ByVal cboEmpresa As WebControls.DropDownList,
    '                                ByVal strCodEmpre As String,
    '                                Optional ByVal strEstudo As String = "N") As Boolean
    '    'Preenche as combos com as empresas de acordo com o usuário
    '    Dim Conn As SqlConnection = New SqlConnection
    '    Dim daEmpresa As SqlDataAdapter
    '    Dim dsEmpresa As DataSet
    '    Dim strSql As String
    '    Conn.Open("pdp")


    '    Try
    '        strSql = "SELECT TRIM(e.codempre) AS codempre, e.sigempre, u.usuar_id " &
    '                 "FROM empre e, usuarempre u " &
    '                 "WHERE TRIM(u.codempre) = TRIM(e.codarea) " &
    '                 "AND e.flg_estudo = '" & strEstudo & "' and u.usuar_id = '" & strUsuar & "' " &
    '                 "UNION " &
    '                 "SELECT TRIM(e.codempre) AS codempre, e.sigempre, u.usuar_id " &
    '                 "FROM empre e, usuarempre u " &
    '                 "WHERE  TRIM(u.codempre) = TRIM(e.codempre) " &
    '                 "AND e.flg_estudo = '" & strEstudo & "' and u.usuar_id = '" & strUsuar & "' " &
    '                 "ORDER BY 2"

    '        daEmpresa = New SqlDataAdapter(strSql, Conn)
    '        dsEmpresa = New DataSet
    '        daEmpresa.Fill(dsEmpresa, "Empresa")
    '        cboEmpresa.DataTextField = "sigempre"
    '        cboEmpresa.DataValueField = "codempre"
    '        cboEmpresa.DataSource = dsEmpresa.Tables("Empresa").DefaultView
    '        cboEmpresa.DataBind()
    '        cboEmpresa.Items.Insert(0, "")
    '        If cboEmpresa.Items.Count = 2 Then
    '            cboEmpresa.SelectedIndex = 1
    '        ElseIf cboEmpresa.Items.Count > 2 And cboEmpresa.Items.FindByValue(Trim(strCodEmpre)) IsNot Nothing Then
    '            cboEmpresa.Items.FindByValue(Trim(strCodEmpre)).Selected = True
    '        End If
    '        'Conn.Close()
    '    Catch ex As Exception
    '        If Conn.State = ConnectionState.Open Then
    '            Conn.Close()
    '        End If
    '        Throw New Exception(ex.Message)
    '    Finally
    '        If Conn.State = ConnectionState.Open Then
    '            Conn.Close()
    '        End If
    '    End Try
    'End Function

    Public Function PreencheComboEmpresaPOP(ByVal strAgentesRepresentados As String,
                                    ByVal cboEmpresa As WebControls.DropDownList,
                                    ByVal strCodEmpre As String,
                                    Optional ByVal strEstudo As String = "N") As Boolean
        'Preenche as combos com as empresas de acordo com o usuário

        Dim Conn As SqlConnection = New SqlConnection
        Dim daEmpresa As SqlDataAdapter
        Dim dsEmpresa As DataSet
        Dim strSql As String
        Conn.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()

        'Dim Conn As SqlConnection = New SqlConnection
        ''  Dim Conn As System.Data.SqlClient.SqlConnection = New System.Data.SqlClient.SqlConnection
        ''Conn.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
        ''  Dim daEmpresa As SqlDataAdapter 'System.Data.IDataAdapter
        'SqlDataAdapter
        ''  Dim dsEmpresa As DataSet
        '' Dim strSql As String
        Conn.Open()

        Dim clausulaSelecao As String = String.Empty

        If Not strAgentesRepresentados.Equals("") Then
            clausulaSelecao = " and empre_bdt_id IN (" & strAgentesRepresentados & ") "
        End If
        Try
            strSql = "SELECT TRIM(codempre) as codempre, sigempre " &
                     "FROM empre  " &
                     "WHERE flg_estudo = '" & strEstudo & "' " & clausulaSelecao &
                     "ORDER BY 2"

            daEmpresa = New SqlDataAdapter(strSql, Conn)
            dsEmpresa = New DataSet
            daEmpresa.Fill(dsEmpresa, "Empresa")
            cboEmpresa.DataTextField = "sigempre"
            cboEmpresa.DataValueField = "codempre"
            cboEmpresa.DataSource = dsEmpresa.Tables("Empresa").DefaultView
            cboEmpresa.DataBind()
            cboEmpresa.Items.Insert(0, "")
            If cboEmpresa.Items.Count = 2 Then
                cboEmpresa.SelectedIndex = 1
            ElseIf cboEmpresa.Items.Count > 2 And cboEmpresa.Items.FindByValue(Trim(strCodEmpre)) IsNot Nothing Then
                cboEmpresa.Items.FindByValue(Trim(strCodEmpre)).Selected = True
            End If
            'Conn.Close()
        Catch ex As Exception
            If Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If
            Throw New Exception(ex.Message)
        Finally
            If Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If
        End Try
    End Function

    Public Function PreencheComboEmpresa(ByVal cboEmpresa As WebControls.DropDownList,
                                         Optional ByVal strEstudo As String = "N") As Boolean
        'Preenche as combos com as empresas de acordo com o usuário
        'Dim Conn As SqlConnection = New SqlConnection
        Dim Conn As System.Data.SqlClient.SqlConnection = New System.Data.SqlClient.SqlConnection
        Conn.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
        Dim daEmpresa As SqlDataAdapter 'System.Data.IDataAdapter
        'Dim daEmpresa As SqlDataAdapter
        Dim dsEmpresa As DataSet
        Dim strSql As String
        Conn.Open()

        Try
            strSql = "SELECT TRIM(e.codempre) AS codempre, e.sigempre " &
                     "FROM empre e " &
                     "WHERE e.flg_estudo = '" & strEstudo & "' " &
                     "ORDER BY 2"

            'daEmpresa = New SqlDataAdapter(strSql, Conn)
            daEmpresa = New SqlDataAdapter(strSql, Conn)
            dsEmpresa = New DataSet
            daEmpresa.Fill(dsEmpresa, "Empresa")
            cboEmpresa.DataTextField = "sigempre"
            cboEmpresa.DataValueField = "codempre"
            cboEmpresa.DataSource = dsEmpresa.Tables("Empresa").DefaultView
            cboEmpresa.DataBind()

            Dim objItem As New WebControls.ListItem
            objItem.Text = ""
            objItem.Value = "0"
            cboEmpresa.Items.Insert(0, objItem)
            'Conn.Close()
        Catch ex As Exception
            If Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If
            Throw New Exception(ex.Message)
        Finally
            If Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If
        End Try
    End Function

    Public Function PreencheListBoxEmpresa(ByVal lstBoxEmpresa As WebControls.ListBox,
                                         Optional ByVal strEstudo As String = "N") As Boolean
        'Preenche as combos com as empresas de acordo com o usuário
        Dim Conn As SqlConnection = New SqlConnection
        Dim daEmpresa As SqlDataAdapter
        Dim dsEmpresa As DataSet
        Dim strSql As String

        Conn.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
        Conn.Open()

        Try
            strSql = "SELECT TRIM(e.codempre) AS codempre, e.sigempre " &
                     "FROM empre e " &
                     "WHERE e.flg_estudo = '" & strEstudo & "' " &
                     "ORDER BY 2"

            daEmpresa = New SqlDataAdapter(strSql, Conn)
            dsEmpresa = New DataSet
            daEmpresa.Fill(dsEmpresa, "Empresa")
            lstBoxEmpresa.DataTextField = "sigempre"
            lstBoxEmpresa.DataValueField = "codempre"
            lstBoxEmpresa.DataSource = dsEmpresa.Tables("Empresa").DefaultView
            lstBoxEmpresa.DataBind()

            'Dim objItem As New WebControls.ListItem
            'objItem.Text = ""
            'objItem.Value = "0"
            'lstBoxEmpresa.Items.Insert(0, objItem)
        Catch ex As Exception
            If Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If
            Throw New Exception(ex.Message)
        Finally
            If Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If
        End Try
    End Function


    Public Function PreencheComboData(ByVal cboDataPdp As WebControls.DropDownList,
                                  ByVal strDataEscolhida As String,
                                  Optional ByVal dataFormatoPDP As Boolean = False) As Boolean
        Try

            Dim cachedData As New List(Of ListItem)

            If dataFormatoPDP Then
                cachedData = CacheDataPDP.GetPdpData(True)
            Else
                cachedData = CacheDataPDP.GetPdpData(False)
            End If


            Dim intI As Integer = 1
            Dim objItem As New WebControls.ListItem
            objItem.Text = ""
            objItem.Value = "0"
            cboDataPdp.Items.Add(objItem)

            For Each item As WebControls.ListItem In cachedData
                cboDataPdp.Items.Add(item)

                ' Corrigindo a comparação: comparando as strings diretamente ou convertendo para datas explicitamente
                If Trim(cboDataPdp.Items(intI).Value) = strDataEscolhida Then
                    cboDataPdp.SelectedIndex = intI
                End If

                intI += 1
            Next

            Return True
        Catch ex As Exception
            Throw New Exception("Erro ao preencher combo data: " & ex.Message)
        End Try
    End Function


    Public Function ValidaLimiteEntradaDados(ByVal pCodEmpresa As String, ByVal pDataPdp As String, ByRef pRetorno As Integer, Optional ByVal mnemonicoInsumo As String = "") As Boolean
        Dim retorno As Boolean = True
        'Dim Conn As SqlConnection = New SqlConnection
        'Dim Cmd As SqlCommand = New SqlCommand

        Dim Conn As System.Data.SqlClient.SqlConnection = New System.Data.SqlClient.SqlConnection
        Conn.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
        Dim Cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand
        Cmd.Connection = Conn
        Conn.Open()

        pRetorno = 0

        If pCodEmpresa <> "" And pDataPdp <> "0" Then
            Dim dataPdpAux As String = Format(CDate(pDataPdp), "yyyyMMdd")

            Try
                Cmd.CommandText = "SELECT  (select c.id_controleagentepdp from tb_controleagentepdp c " &
                                    " where c.datpdp = pdp.datpdp and c.codempre = '" + pCodEmpresa.Trim() + "') as id_controleagentepdp, " &
                                    " pdp.datpdp, " &
                                    " (select c.codempre from tb_controleagentepdp c " &
                                    " where c.datpdp = pdp.datpdp and c.codempre = '" + pCodEmpresa.Trim() + "') as codempre, " &
                                    " pdp.dthinipdp, " &
                                    " pdp.dthfimpdp, " &
                                    " (select c.din_iniciopdp from tb_controleagentepdp c " &
                                    " where c.datpdp = pdp.datpdp and c.codempre = '" + pCodEmpresa.Trim() + "') as din_iniciopdp, " &
                                    " (select c.din_fimpdp from tb_controleagentepdp c " &
                                    " where c.datpdp = pdp.datpdp and c.codempre = '" + pCodEmpresa.Trim() + "') as din_fimpdp " &
                                    " From pdp " &
                                    " WHERE pdp.datpdp = '" + dataPdpAux + "';"

                '"From pdp, outer tb_controleagentepdp " &
                '"Where pdp.datpdp = " + dataPdpAux + " and " &
                '"tb_controleagentepdp.codempre = '" + pCodEmpresa.Trim() + "' and " &
                '"tb_controleagentepdp.datpdp = pdp.datpdp"

                'Dim reader As SqlDataReader = Cmd.ExecuteReader
                Dim reader As SqlDataReader = Cmd.ExecuteReader

                Do While reader.Read

                    '
                    Dim id As Int32 = 0
                    If Not reader.IsDBNull(0) Then
                        id = reader(0)
                    End If

                    '
                    Dim dataIniPDP As DateTime = New DateTime
                    If Not reader.IsDBNull(3) Then
                        dataIniPDP = reader.GetDateTime(3)
                    End If

                    '
                    Dim dataFimPDP As DateTime = New DateTime
                    If Not reader.IsDBNull(4) Then
                        dataFimPDP = reader.GetDateTime(4)
                    End If

                    Dim dataIniControleAgente As DateTime = New DateTime
                    If Not reader.IsDBNull(5) Then
                        dataIniControleAgente = reader.GetDateTime(5)
                    End If

                    Dim dataFimControleAgente As DateTime = New DateTime
                    If Not reader.IsDBNull(6) Then
                        dataFimControleAgente = reader.GetDateTime(6)
                    End If

                    'If (id <> "") Then
                    If (id <> 0) Then
                        'If (DateTime.Now < dataIniControleAgente Or DateTime.Now > dataFimControleAgente) Then
                        If (DateTime.Now < dataIniControleAgente) Then
                            retorno = False
                            pRetorno = 1
                        ElseIf (DateTime.Now > dataFimControleAgente) Then
                            retorno = False
                            pRetorno = 2
                        End If
                    Else
                        'If (DateTime.Now < dataIniPDP Or DateTime.Now > dataFimPDP) Then
                        If (DateTime.Now < dataIniPDP) Then
                            retorno = False
                            pRetorno = 1
                        Else
                            If (dataFimPDP.ToString("dd/MM/yyyy") <> "01/01/0001") Then
                                If (DateTime.Now > dataFimPDP) Then
                                    retorno = False
                                    pRetorno = 2
                                End If
                            End If
                        End If
                    End If
                Loop

                strMsgLimiteEnvioDados = "Prazo esgotado para alteração de dados."

                If Not String.IsNullOrEmpty(mnemonicoInsumo) Then
                    If ValidaInsumoBloqueado(mnemonicoInsumo) Then
                        strMsgLimiteEnvioDados = "Esse insumo foi descontinuado e não pode mais ser enviado para o ONS."
                        retorno = False
                        pRetorno = 3
                    End If
                End If

                reader.Close()
                reader = Nothing
            Catch ex As Exception
                If Conn.State = ConnectionState.Open Then
                    Conn.Close()
                End If
                Throw New Exception(ex.Message)
            Finally
                If Conn.State = ConnectionState.Open Then
                    Conn.Close()
                End If
            End Try
        End If

        ValidaLimiteEntradaDados = retorno
    End Function


    Private Sub Temporizador()
        'O Temporizador aguarda o próximo segundo para retornar
        'Estava ocorrendo erro de chave duplicada na tabela mensa
        'A gravação de um evento estava ocorrendo no mesmo instante do anterior
        Dim iNow As Integer
        Dim iCnt As Integer
        iNow = Second(Now)
        While iNow = Second(Now)
            iCnt = 1
        End While
    End Sub

    Public Function DiaSemana(ByVal intDia As Integer) As String
        Dim arrDia(6) As String
        arrDia(0) = "DOMINGO"
        arrDia(1) = "SEGUNDA"
        arrDia(2) = "TERÇA"
        arrDia(3) = "QUARTA"
        arrDia(4) = "QUINTA"
        arrDia(5) = "SEXTA"
        arrDia(6) = "SÁBADO"
        If intDia > 6 Then
            DiaSemana = ""
        Else
            DiaSemana = arrDia(intDia)
        End If
    End Function

    Public Sub Redireciona(ByVal pURL As String, ByVal pWebForm As Object, ByVal pTarget As String, Optional ByVal pFeatures As String = "")
        Dim objScript As New StringBuilder
        objScript.Append("<Script Language=""JavaScript"">" & NewLine)
        If pTarget <> "" Then
            If pFeatures <> "" Then
                objScript.Append("window.open('" & pURL & "','" & pTarget & "','" & pFeatures & "').focus();")
            Else
                objScript.Append("window.open('" & pURL & "','" & pTarget & "').focus();")
            End If
        Else
            objScript.Append("document.location.replace('" & pURL & "');")
            'objScript.Append("document.location.href('" & pURL & "');")
        End If
        objScript.Append("</Script>")
        pWebForm.Page.RegisterClientScriptBlock("1", objScript.ToString)
    End Sub

    Public Function SessaoAtiva(ByVal pSession As HttpSessionState) As Boolean
        SessaoAtiva = True
        'If pSession("usuarID") = Nothing Then
        'SessaoAtiva = False
        'HttpContext.Current.Response.Write("<script language=javascript> parent.window.location.href ='../menu.aspx' </script> ")
        'End If
    End Function

    'o intervalo de datas deve ser: de hoje a 45 dias para trás, no passado
    Public Function GeraClausulaWHERE_DataPDP_PDO(ByVal pAcesso As String) As String

        Dim dtAux As DateTime = DateTime.Now.AddDays(-45)

        Dim strClausulaWhere As String = ""

        If pAcesso = "PDOC" Then
            strClausulaWhere = " WHERE datpdp >= '" & dtAux.ToString("yyyyMMdd") & "' and datpdp <= '" & DateTime.Now.ToString("yyyyMMdd") & "' "
            strClausulaWhere = strClausulaWhere & " Or exists(SELECT 1 "
            strClausulaWhere = strClausulaWhere & "             FROM tb_logprocessorepdpreger pdpgr "
            strClausulaWhere = strClausulaWhere & "             WHERE nom_statusprocesso = 'FINALIZADO COM SUCESSO' "
            strClausulaWhere = strClausulaWhere & "                     And pdpgr.dat_processo > '" & DateTime.Now.ToString("yyyy-MM-dd") & "' and pdpgr.dat_processo <= '" & DateTime.Now.AddDays(7).ToString("yyyy-MM-dd") & "' "
            strClausulaWhere = strClausulaWhere & "                     and FORMAT(pdpgr.dat_processo,'yyyyMMdd') = datpdp ) "

        Else
            strClausulaWhere = ""
        End If

        Return strClausulaWhere

    End Function

    Public Sub InsereMarcoPDP(ByVal p_IdMarco As String, ByVal pDataPdp As String,
                               ByVal p_Id_UsuarioSolicitante As String, ByVal p_Id_UsuarioResponsavel As String,
                               ByRef pOnsCommand As SqlCommand)

        Try
            pOnsCommand.CommandText = "INSERT INTO tb_marcopdp " &
             "(id_marcoprogpdp, datpdp, id_usuarsolicitante, id_usuarresponsavel, din_marcopdp) VALUES " &
             "('" & p_IdMarco & "','" & pDataPdp & "', '" & p_Id_UsuarioSolicitante & "', '" &
             p_Id_UsuarioResponsavel & "', '" & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") & "')"

            pOnsCommand.ExecuteNonQuery()

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Public Sub InsereMarcoPDP(ByVal p_IdMarco As String, ByVal pDataPdp As String,
                               ByVal p_Id_UsuarioSolicitante As String, ByVal p_Id_UsuarioResponsavel As String,
                               ByVal p_Data As String,
                               ByRef pOnsCommand As SqlCommand)

        Try
            pOnsCommand.CommandText = "INSERT INTO tb_marcopdp " &
             "(id_marcoprogpdp, datpdp, id_usuarsolicitante, id_usuarresponsavel, din_marcopdp) VALUES " &
             "('" & p_IdMarco & "','" & pDataPdp & "', '" & p_Id_UsuarioSolicitante & "', '" &
             p_Id_UsuarioResponsavel & "', '" & p_Data & "')"

            pOnsCommand.ExecuteNonQuery()

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Public Function ValidaLimiteUpload(ByVal codEmpresa As String, ByVal dtPdp As String) As Boolean

        Dim dataPdp As String = Nothing
        Dim horaLimite As Date
        Dim horaDefault As Date = Date.Parse(Date.Now.ToString("dd/MM/yyyy") + " " + "17:00:00")
        Dim fazerUpload As Boolean = True

        Dim dataPdpAux As String = Format(CDate(dtPdp), "yyyyMMdd")
        Dim diaSeguinte As String = DateTime.Now.Date.AddDays(1).ToString("yyyyMMdd")

        Dim Conn As SqlConnection = New SqlConnection
        Dim Cmd As SqlCommand = New SqlCommand

        If Not Conn.State = ConnectionState.Open Then
            Conn.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
            Conn.Open()
        End If

        Cmd.Connection = Conn
        Cmd.CommandText = $"select datpdp, hor_limite from tb_limiteenviocomentariodessemage where codempre = '{codEmpresa}' and datpdp = '{dataPdpAux}'"

        Dim reader As SqlDataReader = Cmd.ExecuteReader

        Do While reader.Read

            If Not reader.IsDBNull(0) Then
                dataPdp = reader.GetString(0)
            End If

            If Not reader.IsDBNull(1) Then
                horaLimite = Date.Parse(Date.Now.ToString("dd/MM/yyyy") + " " + reader("hor_limite").ToString())
            End If
        Loop

        If diaSeguinte.Equals(dataPdpAux) Then
            If IsNothing(dataPdp) Then 'Não existe registro cadastrado no banco
                If DateTime.Now > horaDefault Then 'Caso horário corrente ultrapassou horário padrão
                    fazerUpload = False
                End If
            Else
                If DateTime.Now > horaLimite Then 'Caso horário corrente ultrapassou horário definido no banco de dados
                    fazerUpload = False
                End If
            End If
        Else
            fazerUpload = False
        End If

        '#If DEBUG Then
        '        fazerUpload = True
        '#End If

        reader.Close()
        Cmd.Connection.Close()
        Conn.Close()

        ValidaLimiteUpload = fazerUpload

    End Function

    Public Function ObterComentarioDESSEMOns(ByVal id_comentario As String) As String

        Dim comentario As String = ""

        If Not String.IsNullOrEmpty(id_comentario) Then

            Dim Conn As SqlConnection = New SqlConnection
            Dim Cmd As SqlCommand = New SqlCommand

            If Not Conn.State = ConnectionState.Open Then
                Conn.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
                Conn.Open()
            End If

            Cmd.Connection = Conn
            Cmd.CommandText = $"select cda.dsc_analiseons 
                            from tb_comentario_dessem cd, tb_comentario_dessem_analiseons cda
                            where cd.id_coment_analiseons = cda.id_coment_analiseons
                            and cd.id_comentario = '{id_comentario}'"

            Dim reader As SqlDataReader = Cmd.ExecuteReader

            Do While reader.Read
                If Not reader.IsDBNull(0) Then
                    comentario = reader.GetString(0)
                End If
            Loop

            reader.Close()
            Cmd.Connection.Close()
            Conn.Close()

        End If

        Return comentario

    End Function

    Public Function ValidaInsumoBloqueado(ByVal mnemonicoInsumo As String) As Boolean
        Dim retorno As Boolean = False
        Dim Conn As SqlConnection = New SqlConnection
        Dim Cmd As SqlCommand = New SqlCommand

        Try
            If (Not String.IsNullOrEmpty(mnemonicoInsumo)) Then

                Conn.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
                Cmd.Connection = Conn
                Conn.Open()

                Cmd.CommandText = "select distinct Trim(isNull(I.dsc_mnemonico,'')) as Insumo
                                from tb_bloqueioenvio L
                                join tb_insumo I on (I.id_insumo = L.id_insumo)"

                Dim reader As SqlDataReader = Cmd.ExecuteReader

                Do While reader.Read
                    Dim insumo As String = reader("Insumo")



                    If insumo.Trim() = mnemonicoInsumo.Trim() Then
                        retorno = True
                    End If
                Loop

                reader.Close()
                reader = Nothing
            End If

        Catch ex As Exception
            If Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If

            Throw New Exception("Erro ValidaInsumoBloqueado - " + ex.Message)
        Finally

            If Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If

        End Try



        ValidaInsumoBloqueado = retorno
    End Function

    Public Sub RetirarItensSelecionadosComDuplicidade(ByRef dropDownList As DropDownList)
        If (From li In dropDownList.Items.Cast(Of ListItem) Where li.Selected = True Select li).Count() > 1 Then
            Dim indiceSelecionado As Integer = dropDownList.SelectedIndex
            dropDownList.ClearSelection()
            dropDownList.SelectedIndex = indiceSelecionado
        End If
    End Sub

End Module
