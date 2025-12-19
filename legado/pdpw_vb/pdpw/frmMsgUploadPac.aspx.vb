Public Class frmMsgUploadPac
    Inherits System.Web.UI.Page
    Protected WithEvents lblMensagem As System.Web.UI.WebControls.Label
    Protected WithEvents btnVoltar As System.Web.UI.WebControls.ImageButton

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

    '-- IM112378 - MarcosA 2011-07-22 - Tela que mostra resultados após carregar parcelas
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        If Not Page.IsPostBack Then

            Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
            Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
            Dim strGravaParcelaInsumo As String

            '-- Variáveis do RecordSet
            Dim strInsumo As String = ""
            Dim strUsina As String = ""
            Dim intPatamar As Integer = 0
            Dim intValor As Integer = 0

            '-- Variáveis da Query Dinâmica
            Dim strPatamar As String = ""
            Dim strTabela As String = ""
            Dim strValor As String = ""

            lblMensagem.Text = "Aguarde, atualizando parcelas..."

            Cmd.Connection = Conn

            Try
                Conn.Open("pdp")

                '-- Obtem Parcelas para determinada data / usina / insumo
                Cmd.CommandText = "SELECT INSUMO, CODUSINA, INTPAC, SUM(isnull(VALPACTRAN,0)) AS VALOR  " &
                                  "FROM TB_PAC " &
                                  "WHERE DATPDP =  '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' " &
                                  "GROUP BY INSUMO, CODUSINA, INTPAC " &
                                  "ORDER BY INSUMO, CODUSINA, INTPAC "

                Dim rsParcela As OnsClasses.OnsData.OnsDataReader = Cmd.ExecuteReader

                '-- Faz leitura da Tb_Pac
                Do While rsParcela.Read

                    '-- Recupera valores do Recordset
                    strInsumo = rsParcela("INSUMO")
                    strUsina = rsParcela("CODUSINA")
                    intPatamar = rsParcela("INTPAC")
                    intValor = rsParcela("VALOR")

                    '-- Trata Insumo lido

                    '-- Geração
                    If strInsumo = "GER" Then
                        strTabela = "despa"
                        strValor = "ValDespatran"
                        strPatamar = "IntDespa"
                    End If

                    '-- Inflexibilidade
                    If strInsumo = "IFX" Then
                        strTabela = "Inflexibilidade"
                        strValor = "ValFlexitran"
                        strPatamar = "IntFlexi"
                    End If

                    '-- Restrição por Falta de Combustivel
                    If strInsumo = "RFC" Then
                        strTabela = "rest_falta_comb"
                        strValor = "valrfctran"
                        strPatamar = "intrfc"
                    End If

                    '-- Garantia Energética
                    If strInsumo = "RMP" Then
                        strTabela = "TB_RMP"
                        strValor = "ValRMPtran"
                        strPatamar = "IntRMP"
                    End If

                    '-- Crédito por Substituição
                    If strInsumo = "CFM" Then
                        strTabela = "TB_CFM"
                        strValor = "ValCFMtran"
                        strPatamar = "IntCFM"
                    End If

                    '-- Geração fora de Mérito
                    If strInsumo = "GFM" Then
                        strTabela = "TB_GFM"
                        strValor = "ValGFMtran"
                        strPatamar = "IntGFM"
                    End If

                    '-- Geração Substituta
                    If strInsumo = "SOM" Then
                        strTabela = "TB_SOM"
                        strValor = "ValSOMtran"
                        strPatamar = "IntSOM"
                    End If

                    '-- GE Substituição
                    If strInsumo = "GES" Then
                        strTabela = "TB_GES"
                        strValor = "ValGEStran"
                        strPatamar = "IntGES"
                    End If

                    '-- GE Crédito
                    If strInsumo = "GEC" Then
                        strTabela = "TB_GEC"
                        strValor = "ValGECtran"
                        strPatamar = "IntGEC"
                    End If

                    '-- Despacho Ciclo Aberto
                    If strInsumo = "DCA" Then
                        strTabela = "TB_DCA"
                        strValor = "ValDCAtran"
                        strPatamar = "IntDCA"
                    End If

                    '-- Despacho Ciclo Reduzido
                    If strInsumo = "DCR" Then
                        strTabela = "TB_DCR"
                        strValor = "ValDCRtran"
                        strPatamar = "IntDCR"
                    End If

                    '-- Insumo Reserva 1
                    If strInsumo = "IR1" Then
                        strTabela = "TB_IR1"
                        strValor = "ValIR1tran"
                    End If

                    '-- Insumo Reserva 2
                    If strInsumo = "IR2" Then
                        strTabela = "TB_IR2"
                        strValor = "ValIR2tran"
                        strPatamar = "IntIR2"
                    End If

                    '-- Insumo Reserva 3
                    If strInsumo = "IR3" Then
                        strTabela = "TB_IR3"
                        strValor = "ValIR3tran"
                        strPatamar = "IntIR3"
                    End If

                    '-- Insumo Reserva 4
                    If strInsumo = "IR4" Then
                        strTabela = "TB_IR4"
                        strValor = "ValIR4tran"
                        strPatamar = "IntIR4"
                    End If

                    '-- Disponibilidade
                    If strInsumo = "DSP" Then
                        strTabela = "Disponibilidade"
                        strValor = "ValDSPtran"
                        strPatamar = "IntDSP"
                    End If

                    '-- Compl. de Lastro Fisico
                    If strInsumo = "CLF" Then
                        strTabela = "CompLastro_Fisico"
                        strValor = "ValCLFtran"
                        strPatamar = "IntCLF"
                    End If

                    '-- Energia de Reposição
                    If strInsumo = "ERP" Then
                        strTabela = "Energia_Reposicao"
                        strValor = "ValERPtran"
                        strPatamar = "IntERP"
                    End If

                    '-- Razao Energetica
                    If strInsumo = "ZEN" Then
                        strTabela = "RazaoEner"
                        strValor = "ValRazEnertran"
                        strPatamar = "IntRazEner"
                    End If

                    '-- Razao Eletrica
                    If strInsumo = "ZEL" Then
                        strTabela = "RazaoElet"
                        strValor = "ValRazElettran"
                        strPatamar = "IntRazElet"
                    End If

                    '-- Exportação
                    If strInsumo = "EXP" Then
                        strTabela = "Exporta"
                        strValor = "ValExportatran"
                        strPatamar = "IntExporta"
                    End If

                    '-- Importação
                    If strInsumo = "IMP" Then
                        strTabela = "Importa"
                        strValor = "ValImportatran"
                        strPatamar = "IntImporta"
                    End If

                    '-- Perdas Cl. e Comp.
                    If strInsumo = "PCC" Then
                        strTabela = "PerdasCIC"
                        strValor = "ValPCCtran"
                        strPatamar = "IntPCC"
                    End If

                    '-- Motivo Razão Elétrica
                    If strInsumo = "MRE" Then
                        strTabela = "MotivoREL"
                        strValor = "ValMREtran"
                        strPatamar = "IntMRE"
                    End If

                    '-- Motivo de Inflexibilidade
                    If strInsumo = "MIF" Then
                        strTabela = "MotivoInfl"
                        strValor = "ValMIFtran"
                        strPatamar = "IntMIF"
                    End If

                    '-- Conviencia Operativa
                    If strInsumo = "MCO" Then
                        strTabela = "Conveniencia_Oper"
                        strValor = "ValMCOtran"
                        strPatamar = "IntMCO"
                    End If

                    '-- Sincrono
                    If strInsumo = "MOS" Then
                        strTabela = "Oper_sincrono"
                        strValor = "ValMOStran"
                        strPatamar = "IntMOS"
                    End If

                    '-- Maquina Gerando
                    If strInsumo = "MEG" Then
                        strTabela = "Maq_Gerando"
                        strValor = "ValMEGtran"
                        strPatamar = "IntMEG"
                    End If

                    '-- Inicia uma transação
                    'objTrans = Conn.BeginTransaction
                    'Try
                    '    'Se a transação não estiver aberta vai ocorrer erro
                    '    Cmd.Transaction = objTrans
                    'Catch
                    'End Try

                    strGravaParcelaInsumo = ""
                    strGravaParcelaInsumo = GravaParcelaInsumo(strTabela, strValor, intValor, strUsina, strPatamar, intPatamar)
                    If strGravaParcelaInsumo <> "" Then
                        Session("strMensagem") = strGravaParcelaInsumo
                        Exit Do
                    Else
                        Session("strMensagem") = "Atualização de Parcela de Usina executado com sucesso!"
                    End If

                Loop

                '-- Finaliza Loop
                rsParcela.Close()
                rsParcela = Nothing
                Cmd.Connection.Close()
                Conn.Close()

            Catch
                Session("strMensagem") = "Não foi possível acessar a Base de Dados."
                Conn.Close()
                Response.Redirect("frmMensagem.aspx")
            End Try
        End If

        lblMensagem.Text = Session("strMensagem")

    End Sub

    Private Sub btnVoltar_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnVoltar.Click
        Server.Transfer("frmUpload.aspx")
    End Sub


    '-- IM112378 - MarcosA 2011-07-22 - Rotina para totalizar insumos após carregar parcelas
    Public Function GravaParcelaInsumo(ByVal strTabela As String, ByVal strvalor As String, ByVal intValor As Integer, ByVal strUsina As String, ByVal strPatamar As String, ByVal intpatamar As Integer) As String

        Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
        Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
        Dim objTrans As OnsClasses.OnsData.OnsTransaction

        Dim iNow As Integer
        Dim iCnt As Integer

        Cmd.Connection = Conn
        Conn.Open("pdp")

        Dim strResult As String = ""

        Try

            Cmd.CommandText = "UPDATE " & strTabela & " SET " & strvalor & " = " & intValor & _
                              " WHERE DATPDP = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' " & _
                              " AND CODUSINA = '" & strUsina & "' " & _
                              " AND " & strPatamar & " = '" & intpatamar & "'"
            Cmd.ExecuteNonQuery()

            ''-- Temporizador -- Só descomentar se for necessário utilizar
            'iNow = Second(Now)
            'While iNow = Second(Now)
            '    iCnt = 1
            'End While

            'Grava Transação
            Try
                'Se a transação não estiver aberta vai ocorrer erro
                objTrans.Commit()
            Catch
            End Try


        Catch ex As Exception
            'houve erro, aborta a transação e fecha a conexão
            objTrans.Rollback()
            Conn.Close()
            strResult = "Não foi possível gravar os dados. " & ex.Message
            Return strResult
        End Try
        Try
            'fecha a conexão com o banco
            strResult = ""
            Cmd.Connection.Close()
            Conn.Close()
        Catch ex As Exception
            strResult = "Não foi possível acessar a Base de Dados. " & ex.Message
            Conn.Close()
        End Try

        Return strResult

    End Function

End Class
