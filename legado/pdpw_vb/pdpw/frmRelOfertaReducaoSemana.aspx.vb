Imports System.Collections.Generic
Imports System.IO
Imports System.Linq
Imports System.Net
Imports Newtonsoft.Json

Public Class frmRelOfertaReducaoSemana
    Inherits BaseWebUi

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        cmbAno.Items.Add(DateTime.Now.Year)
        cmbAno.Items.Add(DateTime.Now.AddYears(-1).Year)

        cmbMes.Items.AddRange(GerarMeses().ToArray())

    End Sub

    Private Function GerarMeses() As List(Of ListItem)

        Dim mes As List(Of ListItem) = New List(Of ListItem)

        mes.Add(New ListItem("Janeiro", 1))
        mes.Add(New ListItem("Fevereiro", 2))
        mes.Add(New ListItem("Março", 3))
        mes.Add(New ListItem("Abril", 4))
        mes.Add(New ListItem("Maio", 5))
        mes.Add(New ListItem("Junho", 6))
        mes.Add(New ListItem("Julho", 7))
        mes.Add(New ListItem("Agosto", 8))
        mes.Add(New ListItem("Setembro", 9))
        mes.Add(New ListItem("Outubro", 10))
        mes.Add(New ListItem("Novembro", 11))
        mes.Add(New ListItem("Dezembro", 12))

        Return mes

    End Function

    Protected Sub btnGerar_Click(sender As Object, e As ImageClickEventArgs) Handles btnGerar.Click

        'pegar insumo vindo da tela (parametros)
        Dim ano As ListItem = cmbAno.SelectedItem
        Dim mes As ListItem = cmbMes.SelectedItem
        Dim nomeArquivo As String
        Dim nomeConsumidorArquivo As String

        'pega a semana pmo
        Dim pmo As List(Of SemanaPMO) = GetSemanaPMO(Integer.Parse(ano.Value), Integer.Parse(mes.Value))
        Dim idsSemanaPMO As String = ""
        Dim itemPmo As SemanaPMO

        'Pega as informações da tabela tb_respdemandasemanal
        Dim respDemandaSemanalDesordenada As New List(Of OfertaSemanal)
        Dim respDemandaSemanal As New List(Of OfertaSemanal)

        For Each itemPmo In pmo
            respDemandaSemanalDesordenada.AddRange(GetRespDemandaSemanal(itemPmo.IdSemanapmo))
        Next

        respDemandaSemanal = respDemandaSemanalDesordenada.OrderBy(Function(x) x.CodEmpresa).ThenBy(Function(x) x.Id_SemanaPMO).ToList()

        'Montar objeto
        Dim listObj As New List(Of OfertaReducaoSemana)

        'controle
        Dim flagEmpresaControle As String = ""
        Dim flagEmpresa As String = ""

        Dim itemOfertaSemanal As OfertaSemanal
        For Each itemOfertaSemanal In respDemandaSemanal

            'controle  
            If (flagEmpresaControle = "") Then
                flagEmpresa = itemOfertaSemanal.CodEmpresa
                flagEmpresaControle = flagEmpresa
            Else
                flagEmpresa = itemOfertaSemanal.CodEmpresa
            End If

            nomeConsumidorArquivo = flagEmpresaControle.Trim()

            If (flagEmpresaControle <> flagEmpresa) Then

                nomeArquivo = "Oferta_de_Reducao_Semanal_" & nomeConsumidorArquivo & "_" & mes.Value.ToString.PadLeft(2, "0"c) + ano.Value

                'Gera o CSV
                GerarCSV(listObj.OrderBy(Function(x) x.DiaInicio).ThenBy(Function(x) x.DiaFim).ThenBy(Function(x) x.Produto).ThenBy(Function(x) x.AvisoPrevio).ToList(), nomeArquivo)
                listObj.Clear()
                flagEmpresaControle = flagEmpresa

            End If

            'PROGRAMACAO
            Dim obj As New OfertaReducaoSemana

            obj.AvisoPrevio = "D-1"

            Try
                obj.DiaFim = Convert.ToDateTime(pmo.FirstOrDefault(Function(x) x.IdSemanapmo = itemOfertaSemanal.Id_SemanaPMO).DataFim).ToShortDateString().ToString()
            Catch
                obj.DiaFim = ""
            End Try

            Try
                obj.DiaInicio = Convert.ToDateTime(pmo.FirstOrDefault(Function(x) x.IdSemanapmo = itemOfertaSemanal.Id_SemanaPMO).DataInicio).ToShortDateString().ToString()
            Catch
                obj.DiaInicio = ""
            End Try

            obj.MW = itemOfertaSemanal.VolumeProgramacao
            obj.MWh = itemOfertaSemanal.PrecoProgramacao

            obj.Produto = itemOfertaSemanal.CodUsinaProgramacao.Trim().Last().ToString()
            obj.CodigoDoProduto = itemOfertaSemanal.CodUsinaProgramacao.Trim()

            SetDadosSAGIC(obj)

            listObj.Add(obj)

            'TEMPO REAL
            Dim objTR As New OfertaReducaoSemana

            objTR.AvisoPrevio = "D-0"

            Try
                objTR.DiaFim = Convert.ToDateTime(pmo.FirstOrDefault(Function(x) x.IdSemanapmo = itemOfertaSemanal.Id_SemanaPMO).DataFim).ToShortDateString().ToString()
            Catch ex As Exception
                objTR.DiaFim = ""
            End Try

            Try
                objTR.DiaInicio = Convert.ToDateTime(pmo.FirstOrDefault(Function(x) x.IdSemanapmo = itemOfertaSemanal.Id_SemanaPMO).DataInicio).ToShortDateString().ToString()
            Catch ex As Exception
                objTR.DiaInicio = ""
            End Try

            objTR.MW = itemOfertaSemanal.VolumeTempoReal
            objTR.MWh = itemOfertaSemanal.PrecoTempoReal

            objTR.Produto = itemOfertaSemanal.CodUsinaTempoReal.Trim().Last().ToString()
            objTR.CodigoDoProduto = itemOfertaSemanal.CodUsinaTempoReal.Trim()

            SetDadosSAGIC(objTR)

            listObj.Add(objTR)
        Next


        nomeConsumidorArquivo = listObj.Last.Consumidor
        If nomeConsumidorArquivo = "" Then
            nomeConsumidorArquivo = flagEmpresaControle.Trim()
        End If

        'gera o ultimo bloco do arquivo .csv
        'nomeArquivo = "Oferta_de_Reducao_Semanal_" & flagEmpresaControle.Trim() & "_" & mes.Value.ToString.PadLeft(2, "0"c) + ano.Value
        nomeArquivo = "Oferta_de_Reducao_Semanal_" & nomeConsumidorArquivo & "_" & mes.Value.ToString.PadLeft(2, "0"c) + ano.Value


        'Gera o CSV
        GerarCSV(listObj.OrderBy(Function(x) x.DiaInicio).ThenBy(Function(x) x.DiaFim).ThenBy(Function(x) x.Produto).ThenBy(Function(x) x.AvisoPrevio).ToList(), nomeArquivo)
        listObj.Clear()
        flagEmpresaControle = flagEmpresa


        Response.Write("<SCRIPT>alert('Arquivo gerado com sucesso!')</SCRIPT>")

    End Sub

    Private Sub SetDadosSAGIC(oferta As OfertaReducaoSemana)
        Dim Codigos As List(Of String) = New List(Of String)
        Dim DadoSagic As List(Of SAGIC) = Nothing

        Codigos.Clear()
        Codigos.Add(oferta.CodigoDoProduto)

        DadoSagic = PegarInfoSAGIC(Codigos)

        Try
            oferta.PontoMedicao = DadoSagic(0).NomePtomedicao
            oferta.Consumidor = DadoSagic(0).NomeAgente
        Catch
            oferta.PontoMedicao = ""
            oferta.Consumidor = ""
        End Try
    End Sub

    Private Function GetRespDemandaSemanal(idsSemanaPMO As String) As List(Of OfertaSemanal)

        Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
        Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
        Cmd.Connection = Conn

        Try
            Cmd.CommandText = " Select u.codusina, r.id_respdemandasemanal, r.id_semanapmo, r.cod_usinaprog, round(r.val_volumeprog)::int val_volumeprog, r.val_cvuprog, r.cod_usinatr, round(r.val_volumetr)::int val_volumetr, r.val_cvutr, r.flg_dependente , u.codempre" &
                              "  From usina u" &
                              "  Left Join tb_respdemandasemanal r on u.codusina = r.cod_usinaprog and r.id_semanapmo = " & idsSemanaPMO &
                              " where tpusina_id = 'UTD'" &
                              " order by u.codempre, u.codusina"

            Conn.Open("pdp")
            Dim er As OnsClasses.OnsData.OnsDataReader = Cmd.ExecuteReader

            Dim itens As List(Of OfertaSemanal) = New List(Of OfertaSemanal)

            Do While er.Read
                Dim item As New OfertaSemanal


                If (String.IsNullOrEmpty(er.Item("id_semanapmo").ToString())) Then
                    item.Id_SemanaPMO = idsSemanaPMO
                Else
                    item.Id_SemanaPMO = er.Item("id_semanapmo")
                End If


                If (String.IsNullOrEmpty(er.Item("cod_usinatr").ToString())) Then
                    item.CodUsinaTempoReal = er.Item("codusina")
                Else
                    If (item.Id_SemanaPMO = idsSemanaPMO) Then
                        item.CodUsinaTempoReal = er.Item("cod_usinatr")
                    Else
                        item.CodUsinaTempoReal = er.Item("codusina")
                    End If
                End If


                If (String.IsNullOrEmpty(er.Item("cod_usinaprog").ToString())) Then
                    item.CodUsinaProgramacao = er.Item("codusina")
                Else
                    If (item.Id_SemanaPMO = idsSemanaPMO) Then
                        item.CodUsinaProgramacao = er.Item("cod_usinaprog")
                    Else
                        item.CodUsinaProgramacao = er.Item("codusina")
                    End If
                End If


                If (String.IsNullOrEmpty(er.Item("val_cvutr").ToString())) Then
                    item.PrecoTempoReal = 0
                Else
                    If (item.Id_SemanaPMO = idsSemanaPMO) Then
                        item.PrecoTempoReal = er.Item("val_cvutr")
                    Else
                        item.PrecoTempoReal = 0
                    End If
                End If


                If (String.IsNullOrEmpty(er.Item("val_volumetr").ToString())) Then
                    item.VolumeTempoReal = 0
                Else
                    If (item.Id_SemanaPMO = idsSemanaPMO) Then
                        item.VolumeTempoReal = er.Item("val_volumetr")
                    Else
                        item.VolumeTempoReal = 0
                    End If
                End If


                If (String.IsNullOrEmpty(er.Item("val_volumeprog").ToString())) Then
                    item.VolumeProgramacao = 0
                Else
                    If (item.Id_SemanaPMO = idsSemanaPMO) Then
                        item.VolumeProgramacao = er.Item("val_volumeprog")
                    Else
                        item.VolumeProgramacao = 0
                    End If
                End If


                If (String.IsNullOrEmpty(er.Item("val_cvuprog").ToString())) Then
                    item.PrecoProgramacao = 0
                Else
                    If (item.Id_SemanaPMO = idsSemanaPMO) Then
                        item.PrecoProgramacao = er.Item("val_cvuprog")
                    Else
                        item.PrecoProgramacao = 0
                    End If
                End If

                If (String.IsNullOrEmpty(er.Item("codempre").ToString())) Then
                    item.CodEmpresa = 0
                Else
                    item.CodEmpresa = er.Item("codempre").ToString()
                End If

                itens.Add(item)

            Loop

            er.Close()
            er = Nothing
            Conn.Close()
            Return itens

        Catch ex As Exception

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

    End Function


    Private Sub GerarCSV(obj As List(Of OfertaReducaoSemana), nomeArquivo As String)

        Try

            Dim csv As New ExportarCSV()
            Dim dt = csv.ConverterListParaDataTable(obj)
            Dim textCsv As String = csv.CsvDoDataTable(dt)

            Dim pathDestino As String = Server.MapPath("") & "\Temp\" & nomeArquivo & ".csv"
            Using CsvWriter As New StreamWriter(pathDestino)
                Dim bytes As Byte() = System.Text.Encoding.GetEncoding("iso-8859-8").GetBytes(textCsv)
                Dim texto As String = System.Text.Encoding.UTF8.GetString(bytes)

                'obtem o datatable e gera o arquivo csv
                CsvWriter.Write(texto)
            End Using

            'faz o download do arquivo para maquina client
            DownloadArquivo(pathDestino)

        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Private Sub DownloadArquivo(nomeArquivo As String)

        Dim file As FileInfo = New FileInfo(nomeArquivo)

        Try
            Response.ClearContent()
            Response.Clear()
            Response.ContentType = "application/octet-stream"
            Response.AddHeader("Content-Disposition",
                               "attachment; filename=" + file.Name + ";")

            'faz o download   
            Response.TransmitFile(file.FullName)
            Response.Flush()
            Response.End()
        Catch ex As Exception
            Throw ex
        Finally
            'exclui o arquivo
            Kill(file.FullName)
        End Try
    End Sub

    Private Function PegarInfoSAGIC(usinas As List(Of String)) As List(Of SAGIC)
        Try

            Dim ret As New List(Of SAGIC)

            Using proxy As New DadosSAGICServiceReference.DadosSAGICSoapClient("DadosSAGICSoap")

                Dim objList As DadosSAGICServiceReference.RespostaDemanda() = proxy.DadosCadastroRespostaDemanda(usinas.ToArray())

                Dim item As DadosSAGICServiceReference.RespostaDemanda
                For Each item In objList

                    Dim obj As New SAGIC
                    obj.DataFimVigencia = item.DataFimVigencia
                    obj.DataInicioVigencia = item.DataInicioVigencia
                    obj.IdAgente = item.IdAgente
                    obj.IdEmpreendimento = item.IdEmpreendimento
                    obj.IdNivelTensao = item.IdNivelTensao
                    obj.IdPtomedicao = item.IdPtomedicao
                    obj.NomeAgente = item.NomeAgente
                    obj.NomePtomedicao = item.NomePtomedicao
                    obj.UsinaPDP = item.UsinaPDP

                    ret.Add(obj)

                Next

            End Using

            Return ret

        Catch ex As Exception
            Throw ex
        End Try

        Return Nothing
    End Function

    Private Function GetSemanaPMO(ano As Integer, mes As Integer) As List(Of SemanaPMO)

        Try

            Dim ret As List(Of SemanaPMO) =
                 New SemanaPMOBusiness().ListarTodas().
                    Where(Function(s) s.Ano = ano And s.Mes = mes).
                    Select(Of SemanaPMO)(Function(s) New SemanaPMO(s.Ano, s.Mes, s.Semana, s.DataInicio, s.DataFim, s.IdSemanapmo, s.IdAnomes)).
                    ToList()

            Return ret

        Catch ex As Exception
            Throw ex
        End Try

        Return Nothing
    End Function
End Class