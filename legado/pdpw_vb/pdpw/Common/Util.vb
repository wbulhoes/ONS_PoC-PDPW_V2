Imports System.Data.SqlClient
Imports System.IO
Imports System.Text

Public Class Util
    Inherits System.Web.UI.Page
    Private logger As log4net.ILog = log4net.LogManager.GetLogger(Me.GetType())

    Private strNomeArquivo As String 'Variável que será transportada para o recibo
    Protected strOpcao As String
    Protected strListErro As String = ""
    Protected tbMensagem As DataTable = New DataTable()

    Public Sub New()
        'IniciaLog()
    End Sub

    Public Sub MensagemFormulario(mensagem As String)
        Session("strMensagem") = mensagem
        RegistrarLog(Session("strMensagem"))
        System.Web.HttpContext.Current.Response.Redirect("frmMensagem.aspx")
    End Sub

    Public Function DiretorioEnviodDeDados(Optional dir As String = "") As String

        'Dim fullPath As String = Path.Combine(Server.MapPath(""), dir)
        Dim fullPath As String = Path.Combine(ConfigurationManager.AppSettings.Get("PathArquivosGerados"), dir)

        If (Not Directory.Exists(fullPath)) Then
            Directory.CreateDirectory(fullPath)
        End If

        Return fullPath

    End Function

    Public Function ExisteOfertaDeDemanda(datPdp As String, cmd As SqlCommand) As Boolean

        Dim sql As StringBuilder = New StringBuilder()

        sql.AppendLine("select TOP 1 1")
        sql.AppendLine("  from disponibilidade d")
        sql.AppendLine(" inner join usina u on u.codusina = d.codusina")
        sql.AppendLine(" where d.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'")
        sql.AppendLine("   and u.codempre = '" & Session("strCodEmpre") & "'")
        sql.AppendLine("   and u.tpusina_id = 'UTD'")

        cmd.CommandText = sql.ToString()

        Dim resultado As SqlDataReader = cmd.ExecuteReader
        Dim existe As Boolean = resultado.Read()

        resultado.Close()

        Return existe

    End Function

    ''' <summary>
    ''' Método refatorado para enviar os dados, pretende ter a mesma função do EnviarDados
    ''' </summary>
    ''' <param name="dtPDP"></param>
    ''' <param name="envioDeDados"></param>
    ''' <returns></returns>
    Public Function EnviarDadosOtimizado(ByVal dtPDP As String, envioDeDados As EnvioDeDados) As Boolean

        'Dim objPermissao As OnsClasses.OnsInterfaceUnica.OnsPermissao
        Dim bRetorno As Boolean = False
        Dim strArq As String = DiretorioEnviodDeDados()
        Dim retorno As Boolean = False
        Dim strData As String
        Dim cmbDtValue As String = envioDeDados.CboDataValue

        'IniciaLog()

        RegistrarLog("Inicio do processo de envio de dados otimizado.")

        RegistrarLog("Verificando permissão do usuário. INS, PDPColMaUG usuarID: " + UsuarID)

        'objPermissao = New OnsClasses.OnsInterfaceUnica.OnsPermissao("INS", "PDPColMaUG", UsuarID)
        'Verifica se o usuário tem permissão para salvar os registros
        RegistrarLog("INS - EstaAutorizado: " + EstaAutorizado.ToString())

        If EstaAutorizado Then

            RegistrarLog("Verificando permissão do usuário. DEL, PDPColMaUG usuarID: " + UsuarID)
            'objPermissao = New OnsClasses.OnsInterfaceUnica.OnsPermissao("DEL", "PDPColMaUG", UsuarID)
            'Verifica se o usuário tem permissão para salvar os registros
            RegistrarLog("DEL - EstaAutorizado: " + EstaAutorizado.ToString())

            If EstaAutorizado Then


                Dim Conn As SqlConnection = New SqlConnection
                Conn.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
                Dim Cmd As SqlCommand = New SqlCommand

                Try

                    Conn.Open()
                    Cmd.Connection = Conn

                    RegistrarLog("Criando a OnsConnection(conn) - pdp - PDPEnvDado " + UsuarID)

                    Dim existeOferta As Boolean = False

                    strOpcao = ""

                    existeOferta = ExisteOfertaDeDemanda(dtPDP, Cmd)

                    Dim sql As String = ""

                    'GERAÇÃO
                    RegistrarLog("Verificando se Geração e Geração, Carga, Intercâmbio estão selecionados")
                    If (existeOferta) Or (envioDeDados.ChkGeracao And envioDeDados.ChkEnvia1) Then

                        strOpcao &= "1" '0
                        RegistrarLog("Variavel Opcao recebe o valor 1")

                        RegistrarLog("Geração - Update:")
                        sql += "UPDATE despa " &
                                      "SET valdespaemp = valdespatran " &
                                      "WHERE codusina IN (" &
                                      "SELECT codusina " &
                                      "FROM usina u " &
                                      "WHERE codempre = '" & Session("strCodEmpre") & "'" & AndUsina(envioDeDados.CboUsinaText) & ") " &
                                      "AND datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "';"
                    Else
                        strOpcao &= "0"
                        RegistrarLog("Variavel Opcao recebe o valor 0")
                    End If

                    'CARGA
                    RegistrarLog("Verificando se Carga e Geração, Carga, Intercâmbio estão selecionados")
                    If envioDeDados.ChkCarga And envioDeDados.ChkEnvia1 Then
                        'Conforme definição da Marta em 17/07/2003,
                        'gravar o conteúdo do que está na área de transferência
                        'para os valores da empresa
                        strOpcao &= "1" '1
                        RegistrarLog("Variavel Opcao recebe o valor 1")
                        RegistrarLog("Carga - Update:")
                        sql += "UPDATE carga " &
                                      "SET valcargaemp = valcargatran " &
                                      "WHERE codempre = '" & Session("strCodEmpre") & "' " &
                                      "AND datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "';"

                    Else
                        strOpcao &= "0"
                        RegistrarLog("Variavel Opcao recebe o valor 0")
                    End If

                    'INTERCÂMBIO
                    RegistrarLog("Verificando se Intercambio e eração, Carga, Intercâmbio estão selecionados")
                    If (existeOferta) Or (envioDeDados.ChkIntercambio And envioDeDados.ChkEnvia1) Then
                        'Conforme definição da Marta em 17/07/2003,
                        'gravar o conteúdo do que está na área de transferência
                        'para os valores da empresa
                        strOpcao &= "1" '2
                        RegistrarLog("Variavel Opcao recebe o valor 1")
                        RegistrarLog("Intercambio - Update:")
                        sql += "UPDATE inter " &
                                      "SET valinteremp = valintertran " &
                                      "WHERE codemprede = '" & Session("strCodEmpre") & "' " &
                                      "AND datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "';"
                    Else
                        strOpcao &= "0"
                        RegistrarLog("Variavel Opcao recebe o valor 0")
                    End If

                    '-- CRQ2345 (15/08/2012)
                    RegistrarLog("Verifica se Vazão(Envio) esta selecionado")
                    If envioDeDados.ChkVazE Then
                        'Conforme definição da Marta em 17/07/2003,
                        'gravar o conteúdo do que está na área de transferência
                        'para os valores da empresa
                        'VAZÃO
                        strOpcao &= "1" '3
                        RegistrarLog("Variavel Opcao recebe o valor 1")
                        'VAZÃO
                        RegistrarLog("Vazão - Update:")
                        sql += "UPDATE vazao " &
                                      "SET valturb = valturbtran, valvert = valverttran, valaflu = valaflutran, valtransf = valtransftran " &
                                      "WHERE codusina IN (SELECT codusina FROM usina u WHERE codempre = '" & Session("strCodEmpre") & "'" & AndUsina(envioDeDados.CboUsinaText) & ") " &
                                      "AND datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "';"

                        'COTA INICIAL
                        RegistrarLog("Cota Inicial - Update:")
                        sql += "UPDATE cota " &
                                      "SET cotaini = cotainitran, cotafim = cotafimtran, outrasestruturas = outrasestruturastran, comentariopdf = comentariopdftran " &
                                      "WHERE codusina IN (SELECT codusina FROM usina u WHERE codempre = '" & Session("strCodEmpre") & "'" & AndUsina(envioDeDados.CboUsinaText) & ") " &
                                      "AND datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "';"
                    Else
                        strOpcao &= "0"
                        RegistrarLog("Variavel Opcao recebe o valor 0")
                    End If

                    RegistrarLog("Verificando se Inflexibilidade(envio) está selecionado")
                    If envioDeDados.ChkIfxE Then
                        'Conforme definição da Marta em 17/07/2003,
                        'gravar o conteúdo do que está na área de transferência
                        'para os valores da empresa
                        'INFLEXIBILIDADE
                        strOpcao &= "1" '4
                        RegistrarLog("Variavel Opcao recebe o valor 1")
                        RegistrarLog("Inflexibilidade - Update:")
                        sql += "UPDATE inflexibilidade " &
                                      "SET valflexiemp = valflexitran " &
                                      "WHERE codusina IN (" &
                                      "SELECT codusina " &
                                      "FROM usina u " &
                                      "WHERE codempre = '" & Session("strCodEmpre") & "'" & AndUsina(envioDeDados.CboUsinaText) & ") " &
                                      "AND datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "';"
                    Else
                        strOpcao &= "0"
                        RegistrarLog("Variavel Opcao recebe o valor 0")
                    End If

                    RegistrarLog("Verificando se Razão Energética(envio) está selecionado ")
                    If envioDeDados.ChkZenE Then
                        'Conforme definição da Marta em 17/07/2003,
                        'gravar o conteúdo do que está na área de transferência
                        'para os valores da empresa
                        'RAZÃO ENERGÉTICA
                        strOpcao &= "1" '5
                        RegistrarLog("Variavel Opcao recebe o valor 1")
                        RegistrarLog("Razão Energética - Update:")
                        sql += "UPDATE razaoener " &
                                      "SET valrazeneremp = valrazenertran " &
                                      "WHERE codusina IN (" &
                                      "SELECT codusina " &
                                      "FROM usina u " &
                                      "WHERE codempre = '" & Session("strCodEmpre") & "'" & AndUsina(envioDeDados.CboUsinaText) & ") " &
                                      "AND datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "';"
                    Else
                        strOpcao &= "0"
                        RegistrarLog("Variavel Opcao recebe o valor 0")
                    End If

                    RegistrarLog("Verificando se Razão Elétrica(envio) está selecionado")
                    If envioDeDados.ChkZelE Then
                        'Conforme definição da Marta em 17/07/2003,
                        'gravar o conteúdo do que está na área de transferência
                        'para os valores da empresa
                        'RAZÃO ELÉTRICA
                        strOpcao &= "1" '6
                        RegistrarLog("Variavel Opcao recebe o valor 1")
                        RegistrarLog("Razão Elétrica - Update:")
                        sql += "UPDATE razaoelet " &
                                      "SET valrazeletemp = valrazelettran " &
                                      "WHERE codusina IN (" &
                                      "SELECT codusina " &
                                      "FROM usina u " &
                                      "WHERE codempre = '" & Session("strCodEmpre") & "'" & AndUsina(envioDeDados.CboUsinaText) & ") " &
                                      "AND datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "';"
                    Else
                        strOpcao &= "0"
                        RegistrarLog("Variavel Opcao recebe o valor 0")
                    End If

                    RegistrarLog("Verificando se Restrições de usinas e Manutenções e Restrições estão selecionados")
                    If envioDeDados.ChkRestricao And envioDeDados.ChkEnvia2 Then
                        'RESTRIÇÃO DE USINAS

                        strOpcao &= "1" '7
                        RegistrarLog("Variavel Opcao recebe o valor 1")

                        RegistrarLog("Restrições de Usinas - Delete:")
                        sql += "DELETE FROM temprestrusina " &
                                      "WHERE datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' " &
                                      "AND codusina IN (" &
                                      "SELECT codusina " &
                                      "FROM usina u " &
                                      "WHERE codempre = '" & Session("strCodEmpre") & "'" & AndUsina(envioDeDados.CboUsinaText) & ");"

                        RegistrarLog("Restrições de Usinas - INSERT:")
                        sql += "INSERT INTO temprestrusina (" &
                                          "codusina, " &
                                          "datinirestr, " &
                                          "datfimrestr, " &
                                          "intinirestr, " &
                                          "intfimrestr, " &
                                          "valrestr, " &
                                          "refoutrosis, " &
                                          "id_motivorestricao, " &
                                          "obsrestr, " &
                                          "datpdp " &
                                          ") " &
                                          "SELECT r.codusina, r.datinirestr, r.datfimrestr, r.intinirestr, " &
                                         "r.intfimrestr, r.valrestr, r.refoutrosis, r.id_motivorestricao, r.obsrestr, r.datpdp " &
                                         "FROM restrusinaemp r, usina u " &
                                         "WHERE r.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' " &
                                         "AND r.codusina = u.codusina " & AndUsina(envioDeDados.CboUsinaText) &
                                         "AND u.codempre = '" & Session("strCodEmpre") & "';"

                        RegistrarLog("Restrições de Usinas Geradoras - DELETE:")
                        sql += "DELETE FROM temprestrgerad " &
                                      "WHERE datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' " &
                                      "AND codgerad IN (" &
                                      "SELECT codgerad " &
                                      "FROM gerad " &
                                      "WHERE codusina IN (" &
                                      "SELECT codusina " &
                                      "FROM usina u " &
                                      "WHERE codempre = '" & Session("strCodEmpre") & "'" & AndUsina(envioDeDados.CboUsinaText) & "));"

                        RegistrarLog("Restrições de Usinas Geradoras - Insert:")
                        sql += "INSERT INTO temprestrgerad (" &
                                          "codgerad, " &
                                          "datinirestr, " &
                                          "datfimrestr, " &
                                          "intinirestr, " &
                                          "intfimrestr, " &
                                          "valrestr, " &
                                          "refoutrosis, " &
                                          "id_motivorestricao, " &
                                          "obsrestr, " &
                                          "datpdp " &
                                          ") " &
                                          "SELECT r.codgerad, r.datinirestr, r.datfimrestr, r.intinirestr, " &
                                         "r.intfimrestr, r.valrestr, r.refoutrosis, r.id_motivorestricao, r.obsrestr, r.datpdp " &
                                         "FROM restrgerademp r, gerad g, usina u " &
                                         "WHERE r.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' " &
                                         "AND r.codgerad = g.codgerad " &
                                         "AND g.codusina = u.codusina " & AndUsina(envioDeDados.CboUsinaText) &
                                         "AND u.codempre = '" & Session("strCodEmpre") & "';"


                    Else
                        strOpcao &= "0"
                        RegistrarLog("Variavel Opcao recebe o valor 0")
                    End If

                    RegistrarLog("Verificando se Manutenção e Manutenções e Restrições estão selecionados")
                    If envioDeDados.ChkManutencao And envioDeDados.ChkEnvia2 Then
                        'MANUTENÇÃO DE UNIDADES GERADORAS

                        strOpcao &= "1" '8
                        RegistrarLog("Variavel Opcao recebe o valor 1")
                        RegistrarLog("Manutenção - Delete:")
                        sql += "DELETE FROM tempparal " &
                                      "WHERE datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' " &
                                      "AND codequip IN (" &
                                      "SELECT codgerad AS codequip " &
                                      "FROM gerad " &
                                      "WHERE codusina IN (" &
                                      "SELECT codusina " &
                                      "FROM usina u " &
                                      "WHERE codempre = '" & Session("strCodEmpre") & "'" & AndUsina(envioDeDados.CboUsinaText) & "));"

                        RegistrarLog("Manutenção - Insert:")
                        sql += "INSERT INTO paralemp (" &
                                          "codequip, " &
                                          "tipequip, " &
                                          "datiniparal, " &
                                          "datfimparal, " &
                                          "intiniparal, " &
                                          "intfimparal, " &
                                          "codnivel, " &
                                          "refoutrosis, " &
                                          "indcont, " &
                                          "intinivoltaparal, " &
                                          "intfimvoltaparal, " &
                                          "datpdp " &
                                          ") " &
                                         "SELECT p.codequip, p.tipequip, p.datiniparal, p.datfimparal, " &
                                         "p.intiniparal, p.intfimparal, p.codnivel, p.refoutrosis, p.indcont, " &
                                         "p.intinivoltaparal, p.intfimvoltaparal, p.datpdp " &
                                         "FROM tempparal p, gerad g, usina u " &
                                         "WHERE p.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' " &
                                         "AND p.codequip = g.codgerad " &
                                         "AND g.codusina = u.codusina " & AndUsina(envioDeDados.CboUsinaText) &
                                         "AND u.codempre = '" & Session("strCodEmpre") & "';"

                    Else
                        strOpcao &= "0"
                        RegistrarLog("Variavel Opcao recebe o valor 0")
                    End If

                    RegistrarLog("Verificando se Parada de Máquinas por Conveniência Operativa(envio) está selecionado ")
                    If envioDeDados.ChkPcoE Then
                        'PARADA DE MÁQUINAS POR CONVENIÊNCIA OPERATIVA
                        'Transferindo da Tabela de Empresa para a de área de Transferência
                        strOpcao &= "1" '9
                        RegistrarLog("Variavel Opcao recebe o valor 1")

                        RegistrarLog("Parada de Máquinas por Conveniência Operativa - Delete:")
                        sql += "DELETE FROM tempparal_co " &
                                      "WHERE datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' " &
                                      "AND codequip IN " &
                                      "(SELECT codgerad AS codequip " &
                                      "FROM gerad " &
                                      "WHERE codusina IN " &
                                      "(SELECT codusina " &
                                      "FROM usina u " &
                                      "WHERE codempre = '" & Session("strCodEmpre") & "'" & AndUsina(envioDeDados.CboUsinaText) & "));"

                        RegistrarLog("Parada de Máquinas por Conveniência Operativa - Insert:")
                        sql += "INSERT INTO tempparal_co (" &
                                          "codequip, " &
                                          "tipequip, " &
                                          "datiniparal, " &
                                          "datfimparal, " &
                                          "intiniparal, " &
                                          "intfimparal, " &
                                          "datpdp " &
                                          ") " &
                             "SELECT p.codequip, p.tipequip, p.datiniparal, p.datfimparal, p.intiniparal, p.intfimparal, p.datpdp " &
                                         "FROM paralemp_co p, gerad g, usina u " &
                                         "WHERE p.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' " &
                                         "AND p.codequip = g.codgerad " &
                                         "AND g.codusina = u.codusina " & AndUsina(envioDeDados.CboUsinaText) &
                                         "AND u.codempre = '" & Session("strCodEmpre") & "';"

                    Else
                        strOpcao &= "0"
                        RegistrarLog("Variavel Opcao recebe o valor 0")
                    End If

                    RegistrarLog("Verificando se Exportação(envio) está selecionado")
                    If envioDeDados.ChkExpE Then
                        'Conforme definição da Marta em 17/07/2003,
                        'gravar o conteúdo do que está na área de transferência
                        'para os valores da empresa
                        'EXPORTAÇÃO
                        strOpcao &= "1" '10
                        RegistrarLog("Variavel Opcao recebe o valor 1")
                        RegistrarLog("Exportação - Update:")
                        sql += "UPDATE exporta " &
                                      "SET valexportaemp = valexportatran " &
                                      "WHERE codusina IN (" &
                                      "SELECT codusina " &
                                      "FROM usina u " &
                                      "WHERE codempre = '" & Session("strCodEmpre") & "'" & AndUsina(envioDeDados.CboUsinaText) & ") " &
                                      "AND datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "';"
                    Else
                        strOpcao &= "0"
                        RegistrarLog("Variavel Opcao recebe o valor 0")
                    End If

                    RegistrarLog("Verificando se Importação(envio) está selecionado")
                    If envioDeDados.ChkImpE Then
                        'Conforme definição da Marta em 17/07/2003,
                        'gravar o conteúdo do que está na área de transferência
                        'para os valores da empresa
                        'IMPORTAÇÃO
                        strOpcao &= "1" '11
                        RegistrarLog("Variavel Opcao recebe o valor 1")
                        RegistrarLog("Importação - Update:")
                        sql += "UPDATE importa " &
                                      "SET valimportaemp = valimportatran " &
                                      "WHERE codusina IN (" &
                                      "SELECT codusina " &
                                      "FROM usina u " &
                                      "WHERE codempre = '" & Session("strCodEmpre") & "'" & AndUsina(envioDeDados.CboUsinaText) & ") " &
                                      "AND datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "';"
                    Else
                        strOpcao &= "0"
                        RegistrarLog("Variavel Opcao recebe o valor 0")

                    End If

                    RegistrarLog("Verificando se Motivo de Despacho Razão Elétrica(envio) está selecionado")
                    If envioDeDados.ChkMreE Then
                        'Motivo de Despacho Razão Elétrica
                        strOpcao &= "1" '12
                        RegistrarLog("Variavel Opcao recebe o valor 1")

                        RegistrarLog("Motivo de Despacho Razão Elétrica - Update:")
                        sql += "UPDATE motivorel " &
                                      "SET valmreemp = valmretran " &
                                      "WHERE codusina IN (" &
                                      "SELECT codusina " &
                                      "FROM usina u " &
                                      "WHERE codempre = '" & Session("strCodEmpre") & "'" & AndUsina(envioDeDados.CboUsinaText) & ") " &
                                      "AND datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "';"
                    Else
                        strOpcao &= "0"
                        RegistrarLog("Variavel Opcao recebe o valor 0")
                    End If

                    RegistrarLog("Verificando se Motivo de Despacho por Inflexibilidade(envio) está selecionado")
                    If envioDeDados.ChkMifE Then
                        'Motivo de Despacho por Inflexibilidade
                        strOpcao &= "1" '13
                        RegistrarLog("Variavel Opcao recebe o valor 1")

                        RegistrarLog("Motivo de Despacho por Inflexibilidade - Update:")
                        sql += "UPDATE motivoinfl " &
                                      "SET valmifemp = valmiftran " &
                                      "WHERE codusina IN (" &
                                      "SELECT codusina " &
                                      "FROM usina u " &
                                      "WHERE codempre = '" & Session("strCodEmpre") & "'" & AndUsina(envioDeDados.CboUsinaText) & ") " &
                                      "AND datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "';"
                    Else
                        strOpcao &= "0"
                        RegistrarLog("Variavel Opcao recebe o valor 0")

                    End If

                    RegistrarLog("Verificando se Perdas Consumo Interno e Compensação(envio) está selecionado")
                    If envioDeDados.ChkPccE Then
                        'Perdas Consumo Interno e Compensação
                        strOpcao &= "1" '14
                        RegistrarLog("Variavel Opcao recebe o valor 1")

                        RegistrarLog("Perdas Consumo Interno e Compensação - Update:")
                        sql += "UPDATE perdascic " &
                                      "SET valpccemp = valpcctran " &
                                      "WHERE codusina IN (" &
                                      "SELECT codusina " &
                                      "FROM usina u " &
                                      "WHERE codempre = '" & Session("strCodEmpre") & "'" & AndUsina(envioDeDados.CboUsinaText) & ") " &
                                      "AND datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "';"
                    Else
                        strOpcao &= "0"
                        RegistrarLog("Variavel Opcao recebe o valor 0")
                    End If

                    RegistrarLog("Verificando se Número Máquinas Paradas por Conveniência Operativa(envio) está selecionado")
                    If envioDeDados.ChkMcoE Then
                        'Número Máquinas Paradas por Conveniência Operativa
                        strOpcao &= "1" '15
                        RegistrarLog("Variavel Opcao recebe o valor 1")

                        RegistrarLog("Número Máquinas Paradas por Conveniência Operativa - Update:")
                        sql += "UPDATE conveniencia_oper " &
                                      "SET valmcoemp = valmcotran " &
                                      "WHERE codusina IN (" &
                                      "SELECT codusina " &
                                      "FROM usina u " &
                                      "WHERE codempre = '" & Session("strCodEmpre") & "'" & AndUsina(envioDeDados.CboUsinaText) & ") " &
                                      "AND datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "';"
                    Else
                        strOpcao &= "0"
                        RegistrarLog("Variavel Opcao recebe o valor 0")
                    End If

                    RegistrarLog("Verificando se Número Máquinas Operando como Síncrono(envio) está selecionado")
                    If envioDeDados.ChkMosE Then
                        'Número Máquinas Operando como Síncrono
                        strOpcao &= "1" '16
                        RegistrarLog("Variavel Opcao recebe o valor 1")
                        RegistrarLog("Número Máquinas Operando como Síncrono - Update:")
                        sql += "UPDATE oper_sincrono " &
                                      "SET valmosemp = valmostran " &
                                      "WHERE codusina IN (" &
                                      "SELECT codusina " &
                                      "FROM usina u " &
                                      "WHERE codempre = '" & Session("strCodEmpre") & "'" & AndUsina(envioDeDados.CboUsinaText) & ") " &
                                      "AND datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "';"
                    Else
                        strOpcao &= "0"
                        RegistrarLog("Variavel Opcao recebe o valor 0")
                    End If

                    RegistrarLog("Verificando se Número Máquinas Gerando(envio) está selecionado")
                    If envioDeDados.ChkMegE Then
                        'Número Máquinas Gerando
                        strOpcao &= "1" '17
                        RegistrarLog("Variavel Opcao recebe o valor 1")

                        RegistrarLog("Número Máquinas Gerando - Update:")
                        sql += "UPDATE maq_gerando " &
                                      "SET valmegemp = valmegtran " &
                                      "WHERE codusina IN (" &
                                      "SELECT codusina " &
                                      "FROM usina u " &
                                      "WHERE codempre = '" & Session("strCodEmpre") & "'" & AndUsina(envioDeDados.CboUsinaText) & ") " &
                                      "AND datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "';"

                    Else
                        strOpcao &= "0"
                        RegistrarLog("Variavel Opcao recebe o valor 0")
                    End If

                    RegistrarLog("Verificando se Energia de Reposição e Perdas(envio) está selecionado")
                    If envioDeDados.ChkErpE Then
                        'Energia de Reposição e Perdas
                        strOpcao &= "1" '18
                        RegistrarLog("Variavel Opcao recebe o valor 1")

                        RegistrarLog("Energia de Reposição e Perdas - Update:")
                        sql += "UPDATE energia_reposicao " &
                                      "SET valerpemp = valerptran " &
                                      "WHERE codusina IN (" &
                                      "SELECT codusina " &
                                      "FROM usina u " &
                                      "WHERE codempre = '" & Session("strCodEmpre") & "'" & AndUsina(envioDeDados.CboUsinaText) & ") " &
                                      "AND datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "';"
                    Else
                        strOpcao &= "0"
                        RegistrarLog("Variavel Opcao recebe o valor 0")

                    End If

                    RegistrarLog("Verificando se Disponibilidade está selecionado")
                    If envioDeDados.ChkDspE Then
                        'Disponibilidade
                        strOpcao &= "1" '19
                        RegistrarLog("Variavel Opcao recebe o valor 1")

                        RegistrarLog("Disponibilidade - UPDATE:")
                        sql += "UPDATE disponibilidade " &
                                      "SET valdspemp = valdsptran " &
                                      "WHERE codusina IN (" &
                                      "SELECT codusina " &
                                      "FROM usina u " &
                                      "WHERE codempre = '" & Session("strCodEmpre") & "'" & AndUsina(envioDeDados.CboUsinaText) & ") " &
                                      "AND datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "';"
                    Else
                        strOpcao &= "0"
                        RegistrarLog("Variavel Opcao recebe o valor 0")
                    End If

                    RegistrarLog("Verificando se Compensação de Lastro Físico está selecionado")
                    If envioDeDados.ChkClfE Then
                        'Compensação de Lastro Físico
                        strOpcao &= "1" '20
                        RegistrarLog("Variavel Opcao recebe o valor 1")

                        RegistrarLog("Compensação de Lastro Físico - UPDATE:")
                        sql += "UPDATE complastro_fisico " &
                                      "SET valclfemp = valclftran " &
                                      "WHERE codusina IN (" &
                                      "SELECT codusina " &
                                      "FROM usina u " &
                                      "WHERE codempre = '" & Session("strCodEmpre") & "'" & AndUsina(envioDeDados.CboUsinaText) & ") " &
                                      "AND datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "';"
                    Else
                        strOpcao &= "0"
                        RegistrarLog("Variavel Opcao recebe o valor 0")
                    End If

                    RegistrarLog("Verificando se Restrição por Falta de Combustível está selecionado")
                    If envioDeDados.ChkRfcE Then
                        'Restrição por Falta de Combustível
                        strOpcao &= "1" '21
                        RegistrarLog("Variavel Opcao recebe o valor 1")

                        RegistrarLog("Restrição por Falta de Combustível - Update:")
                        sql += "UPDATE rest_falta_comb " &
                                      "SET valrfcemp = valrfctran " &
                                      "WHERE codusina IN " &
                                      "(SELECT codusina " &
                                      "FROM usina u " &
                                      "WHERE codempre = '" & Session("strCodEmpre") & "'" & AndUsina(envioDeDados.CboUsinaText) & ") " &
                                      "AND datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "';"
                    Else
                        strOpcao &= "0"
                        RegistrarLog("Variavel Opcao recebe o valor 0")
                    End If

                    RegistrarLog("Verificando se Crédito por Substituição está selecionado")
                    If envioDeDados.ChkRmpE Then
                        'Garantia Energética
                        strOpcao &= "1" '22
                        RegistrarLog("Variavel Opcao recebe o valor 1")

                        RegistrarLog("Garantia Energética - Update:")
                        sql += "UPDATE tb_rmp " &
                                      "SET valrmpemp = valrmptran " &
                                      "WHERE codusina IN " &
                                      "(SELECT codusina " &
                                      "FROM usina u " &
                                      "WHERE codempre = '" & Session("strCodEmpre") & "'" & AndUsina(envioDeDados.CboUsinaText) & ") " &
                                      "AND datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "';"
                    Else
                        strOpcao &= "0"
                        RegistrarLog("Variavel Opcao recebe o valor 0")
                    End If

                    RegistrarLog("Verificando se Crédito por Substituição está selecionado")
                    If envioDeDados.ChkGfmE Then
                        'Geração Fora de Mérito
                        strOpcao &= "1" '23
                        RegistrarLog("Variavel Opcao recebe o valor 1")
                        RegistrarLog("Geração Fora de Mérito - Update:")
                        sql += "UPDATE tb_gfm " &
                                      "SET valgfmemp = valgfmtran " &
                                      "WHERE codusina IN " &
                                      "(SELECT codusina " &
                                      "FROM usina u " &
                                      "WHERE codempre = '" & Session("strCodEmpre") & "'" & AndUsina(envioDeDados.CboUsinaText) & ") " &
                                      "AND datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "';"
                    Else
                        strOpcao &= "0"
                        RegistrarLog("Variavel Opcao recebe o valor 0")
                    End If

                    RegistrarLog("Verificando se Crédito por Substituição está selecionado")
                    If envioDeDados.ChkCfmE Then
                        'Crédito por Substituição
                        strOpcao &= "1" '24
                        RegistrarLog("Variavel Opcao recebe o valor 1")

                        RegistrarLog("Crédito por Substituição - Update:")
                        sql += "UPDATE tb_cfm " &
                                      "SET valcfmemp = valcfmtran " &
                                      "WHERE codusina IN " &
                                      "(SELECT codusina " &
                                      "FROM usina u " &
                                      "WHERE codempre = '" & Session("strCodEmpre") & "'" & AndUsina(envioDeDados.CboUsinaText) & ") " &
                                      "AND datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "';"
                    Else
                        strOpcao &= "0"
                        RegistrarLog("Variavel Opcao recebe o valor 0")
                    End If

                    RegistrarLog("Verificando se Geração Substituta está selecionado")
                    If envioDeDados.ChkSomE Then
                        'Geração Substituta
                        strOpcao &= "1" '25
                        RegistrarLog("Variavel Opcao recebe o valor 1")

                        RegistrarLog("Geração Substituta - Update:")
                        sql += "UPDATE tb_som " &
                                      "SET valsomemp = valsomtran " &
                                      "WHERE codusina IN " &
                                      "(SELECT codusina " &
                                      "FROM usina u " &
                                      "WHERE codempre = '" & Session("strCodEmpre") & "'" & AndUsina(envioDeDados.CboUsinaText) & ") " &
                                      "AND datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "';"
                    Else
                        strOpcao &= "0"
                        RegistrarLog("Variavel Opcao recebe o valor 0")
                    End If

                    If envioDeDados.ChkGesE Then
                        'GE Substituição
                        strOpcao &= "1" '26
                        sql += "UPDATE tb_GES " &
                                      "SET valGESemp = valGEStran " &
                                      "WHERE codusina IN " &
                                      "(SELECT codusina " &
                                      "FROM usina u " &
                                      "WHERE codempre = '" & Session("strCodEmpre") & "'" & AndUsina(envioDeDados.CboUsinaText) & ") " &
                                      "AND datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "';"
                    Else
                        strOpcao &= "0"
                    End If

                    If envioDeDados.ChkGecE Then
                        'GE Crédito
                        strOpcao &= "1" '27
                        sql += "UPDATE tb_GEC " &
                                      "SET valGECemp = valGECtran " &
                                      "WHERE codusina IN " &
                                      "(SELECT codusina " &
                                      "FROM usina u " &
                                      "WHERE codempre = '" & Session("strCodEmpre") & "'" & AndUsina(envioDeDados.CboUsinaText) & ") " &
                                      "AND datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "';"
                    Else
                        strOpcao &= "0"
                    End If

                    If envioDeDados.ChkDcaE Then
                        'Despacho Ciclo Aberto
                        strOpcao &= "1" '28
                        sql += "UPDATE tb_DCA " &
                                      "SET valDCAemp = valDCAtran " &
                                      "WHERE codusina IN " &
                                      "(SELECT codusina " &
                                      "FROM usina u " &
                                      "WHERE codempre = '" & Session("strCodEmpre") & "'" & AndUsina(envioDeDados.CboUsinaText) & ") " &
                                      "AND datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "';"
                    Else
                        strOpcao &= "0"
                    End If

                    If envioDeDados.ChkDcrE Then
                        'Despacho Ciclo Reduzido
                        strOpcao &= "1" '29
                        sql += "UPDATE tb_DCR " &
                                      "SET valDCRemp = valDCRtran " &
                                      "WHERE codusina IN " &
                                      "(SELECT codusina " &
                                      "FROM usina u " &
                                      "WHERE codempre = '" & Session("strCodEmpre") & "'" & AndUsina(envioDeDados.CboUsinaText) & ") " &
                                      "AND datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "';"

                    Else
                        strOpcao &= "0"
                    End If

                    If envioDeDados.ChkIr1E Then
                        'Insumo Reserva 1
                        strOpcao &= "1" '30
                        sql += "UPDATE tb_IR1 " &
                                      "SET valIR1emp = valIR1tran " &
                                      "WHERE codusina IN " &
                                      "(SELECT codusina " &
                                      "FROM usina u " &
                                      "WHERE codempre = '" & Session("strCodEmpre") & "'" & AndUsina(envioDeDados.CboUsinaText) & ") " &
                                      "AND datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "';"
                    Else
                        strOpcao &= "0"
                    End If

                    If envioDeDados.ChkIr2E Then
                        'Insumo Reserva 2
                        strOpcao &= "1" '31
                        sql += "UPDATE tb_IR2 " &
                                      "SET valIR2emp = valIR2tran " &
                                      "WHERE codusina IN " &
                                      "(SELECT codusina " &
                                      "FROM usina u " &
                                      "WHERE codempre = '" & Session("strCodEmpre") & "'" & AndUsina(envioDeDados.CboUsinaText) & ") " &
                                      "AND datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "';"
                    Else
                        strOpcao &= "0"
                    End If

                    If envioDeDados.ChkIr3E Then
                        'Insumo Reserva 3
                        strOpcao &= "1" '32
                        sql += "UPDATE tb_IR3 " &
                                      "SET valIR3emp = valIR3tran " &
                                      "WHERE codusina IN " &
                                      "(SELECT codusina " &
                                      "FROM usina u " &
                                      "WHERE codempre = '" & Session("strCodEmpre") & "'" & AndUsina(envioDeDados.CboUsinaText) & ") " &
                                      "AND datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "';"
                    Else
                        strOpcao &= "0"
                    End If

                    If envioDeDados.ChkIr4E Then
                        'Insumo Reserva 4
                        strOpcao &= "1" '33
                        sql += "UPDATE tb_IR4 " &
                                      "SET valIR4emp = valIR4tran " &
                                      "WHERE codusina IN " &
                                      "(SELECT codusina " &
                                      "FROM usina u " &
                                      "WHERE codempre = '" & Session("strCodEmpre") & "'" & AndUsina(envioDeDados.CboUsinaText) & ") " &
                                      "AND datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "';"
                    Else
                        strOpcao &= "0"
                    End If

                    If envioDeDados.ChkCVU Then
                        'Insumo CVU
                        strOpcao &= "1" '34
                        'A consulta está diretamente na inclusão da mensa
                    Else
                        strOpcao &= "0"
                    End If

                    'RRO
                    RegistrarLog("Verificando se RRO está selecionado")
                    If (existeOferta) Or (envioDeDados.ChkRROE) Then

                        strOpcao &= "1" '35
                        RegistrarLog("Variavel Opcao recebe o valor 1")

                        RegistrarLog("RRO - Update:")
                        sql += "UPDATE tb_rro " &
                                      "SET valrroemp = valrrotran " &
                                      "WHERE codusina IN (" &
                                      "SELECT codusina " &
                                      "FROM usina u " &
                                      "WHERE codempre = '" & Session("strCodEmpre") & "'" & AndUsina(envioDeDados.CboUsinaText) & ") " &
                                      "AND datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "';"
                    Else
                        strOpcao &= "0"
                        RegistrarLog("Variavel Opcao recebe o valor 0")
                    End If

                    strNomeArquivo = Trim(Session("strCodEmpre")) & "-ON " & Mid(dtPDP, 1, 8) & "-" & IIf(envioDeDados.ChkCVU, DateTime.Now.ToString("HHmmss"), Mid(dtPDP, 9)) & " WEB"

                    RegistrarLog("Nome do Arquivo: " + strNomeArquivo)

                    sql += "INSERT INTO Mensa (" &
                                          "datpdp, " &
                                          "dthmensa, " &
                                          "codempre, " &
                                          "nomarq, " &
                                          "indrecenv, " &
                                          "sitmensa, " &
                                          "totdespa, " &
                                          "totcarga, " &
                                          "totinter, " &
                                          "totvazao, " &
                                          "qtdrestr, " &
                                          "qtdparal, " &
                                          "totinflexi, " &
                                          "totrazener, " &
                                          "totrazelet, " &
                                          "totexporta, " &
                                          "totimporta," &
                                          "totmre, " &
                                          "totpcc, " &
                                          "totmco, " &
                                          "totmos, " &
                                          "totmeg, " &
                                          "toterp, " &
                                          "totdsp, " &
                                          "totclf, " &
                                          "totmif, " &
                                          "qtdparalco, " &
                                          "totcota, " &
                                          "totrfc, " &
                                          "totrmp, " &
                                          "totgfm, " &
                                          "totcfm, " &
                                          "totsom, " &
                                          "totges, " &
                                          "totgec, " &
                                          "totdca, " &
                                          "totdcr, " &
                                          "totir1, " &
                                          "totir2, " &
                                          "totir3, " &
                                          "totir4, " &
                                          "totrro, " &
                                          "totcvu, " &
                                          "usuar_id " &
                                          ") VALUES (" &
                                          "'" & IIf(Format(Session("datEscolhida"), "yyyyMMdd").Equals("yyyyMMdd"), Session("datEscolhida"), Format(Session("datEscolhida"), "yyyyMMdd")) & "', " &'ref
                                          "'" & Format(Now, "yyyyMMddHHmmss") & "', " &'ok
                                          "'" & Trim(Session("strCodEmpre")) & "', " &'ok
                                          "'" & strNomeArquivo & "', " &
                                          "'" & "R" & "', " &
                                          "'" & "Processado" & "', " &
                                          "(" & 'Geração
                                            "SELECT SUM(g.valdespaemp)/2 AS valGeracao FROM usina u, despa g WHERE u.codempre = '" & Session("strCodEmpre") & "' AND u.codusina = g.codusina " & AndUsina(envioDeDados.CboUsinaText) & " AND g.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") &
                                          "')," &
                                          "(" & 'Carga
                                               "SELECT SUM(valcargaemp)/2 AS valCarga FROM carga WHERE codempre = '" & Session("strCodEmpre") & "' AND datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'" &
                                          "), " &
                                          "(" & 'intercambio
                                                    "SELECT SUM(valinteremp)/2 AS valIntercambio " &
                                                    "FROM inter " &
                                                    "WHERE datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' " &
                                                    "AND codemprede = '" & Session("strCodEmpre") & "'" &
                                          "), " &
                                          "(" & 'vazao
                                          "SELECT SUM(v.valturb) AS valTurbinada 
                                  From vazao v, usina u
                                  Where u.codempre = '" & Session("strCodEmpre") & "' 
                                  And u.codusina = v.codusina
                                  And u.tipusina = 'H' 
                                  And v.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'" & "), " &
                                          "(" & 'restrição
                                         "SELECT count(*) " &
                                         "FROM restrgerademp r, gerad g, usina u " &
                                         "WHERE r.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' " &
                                         "AND r.codgerad = g.codgerad " &
                                         "AND g.codusina = u.codusina " & AndUsina(envioDeDados.CboUsinaText) &
                                         "AND u.codempre = '" & Session("strCodEmpre") & "'" &
                                          "), " &
                                          "(" & 'manutenção
                                         "SELECT count(*) " &
                                         "FROM tempparal p, gerad g, usina u " &
                                         "WHERE p.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' " &
                                         "AND p.codequip = g.codgerad " &
                                         "AND g.codusina = u.codusina " & AndUsina(envioDeDados.CboUsinaText) &
                                         "AND u.codempre = '" & Session("strCodEmpre") & "'" &
                                          "), " &
                                          "(" & 'Inflexibilidade
                                      "SELECT SUM(i.valflexiemp)/2 AS valInflexibilidade " &
                                      "FROM usina u, inflexibilidade i " &
                                      "WHERE u.codempre = '" & Session("strCodEmpre") & "' " &
                                      "AND u.codusina = i.codusina " & AndUsina(envioDeDados.CboUsinaText) &
                                      "AND i.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'" &
                                        "), " &
                                        "(" & 'rz energética
                                        "SELECT SUM(r.valrazeneremp)/2 AS valEnergetica " &
                                      "FROM usina u, razaoener  r " &
                                      "WHERE u.codempre = '" & Session("strCodEmpre") & "' " &
                                      "AND u.codusina = r.codusina " & AndUsina(envioDeDados.CboUsinaText) &
                                      "AND r.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'" &
                                        ")," &
                                          "(" & 'rz elétrica
                                        "SELECT SUM(r.valrazeletemp)/2 AS valEletrica " &
                                      "FROM usina u, razaoelet  r " &
                                      "WHERE u.codempre = '" & Session("strCodEmpre") & "' " &
                                      "AND u.codusina = r.codusina " & AndUsina(envioDeDados.CboUsinaText) &
                                      "AND r.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'" &
                                          "), " &
                                          "(" & 'exportacao 
                                      "SELECT SUM(i.valexportaemp)/2 AS valexporta " &
                                      "FROM usina u, exporta i " &
                                      "WHERE u.codempre = '" & Session("strCodEmpre") & "' " &
                                      "AND u.codusina = i.codusina " & AndUsina(envioDeDados.CboUsinaText) &
                                      "AND i.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'" &
                                         "), " &
                                          "(" & 'importação 
                                        "SELECT SUM(i.valimportaemp)/2 AS valimporta " &
                                      "FROM usina u, importa i " &
                                      "WHERE u.codempre = '" & Session("strCodEmpre") & "' " &
                                      "AND u.codusina = i.codusina " & AndUsina(envioDeDados.CboUsinaText) &
                                      "AND i.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'" &
                                      "), " &
                                          "(" & 'MRE
                                          "SELECT SUM(i.valmreemp) AS valmre " &
                                      "FROM usina u, motivorel i " &
                                      "WHERE u.codempre = '" & Session("strCodEmpre") & "' " &
                                      "AND u.codusina = i.codusina " & AndUsina(envioDeDados.CboUsinaText) &
                                      "AND i.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'" &
                                      "), " &
                                          "(" & 'PCC
                                        "SELECT SUM(i.valpccemp)/2 AS valpcc " &
                                      "FROM usina u, perdascic i " &
                                      "WHERE u.codempre = '" & Session("strCodEmpre") & "' " &
                                      "AND u.codusina = i.codusina " & AndUsina(envioDeDados.CboUsinaText) &
                                      "AND i.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'" &
                                       "), " &
                                       "(" & 'MCO
                                       "SELECT SUM(i.valmcoemp) AS valmco " &
                                      "FROM usina u, conveniencia_oper i " &
                                      "WHERE u.codempre = '" & Session("strCodEmpre") & "' " &
                                      "AND u.codusina = i.codusina " & AndUsina(envioDeDados.CboUsinaText) &
                                      "AND i.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'" &
                                        "), " &
                                         "(" & 'MOS
                                        "SELECT SUM(i.valmosemp) AS valmos " &
                                      "FROM usina u, oper_sincrono i " &
                                      "WHERE u.codempre = '" & Session("strCodEmpre") & "' " &
                                      "AND u.codusina = i.codusina " & AndUsina(envioDeDados.CboUsinaText) &
                                      "AND i.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'" &
                                      "), " &
                                          "(" & 'MEG 
                                       "SELECT SUM(i.valmegemp) AS valmeg " &
                                      "FROM usina u, maq_gerando i " &
                                      "WHERE u.codempre = '" & Session("strCodEmpre") & "' " &
                                      "AND u.codusina = i.codusina " & AndUsina(envioDeDados.CboUsinaText) &
                                      "AND i.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'" &
                                        "), " &
                                          "(" & 'ERP
                                       "SELECT SUM(i.valerpemp)/2 AS valerp " &
                                      "FROM usina u, energia_reposicao i " &
                                      "WHERE u.codempre = '" & Session("strCodEmpre") & "' " &
                                      "AND u.codusina = i.codusina " & AndUsina(envioDeDados.CboUsinaText) &
                                      "AND i.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'" &
                                      "), " &
                                          "(" & 'Disponibilidade
                                        "SELECT SUM(i.valdspemp)/2 AS valdsp " &
                                      "FROM usina u, disponibilidade i " &
                                      "WHERE u.codempre = '" & Session("strCodEmpre") & "' " &
                                      "AND u.codusina = i.codusina " & AndUsina(envioDeDados.CboUsinaText) &
                                      "AND i.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'" &
                                        "), " &
                                          "(" & 'CLF
                                        "SELECT SUM(i.valclfemp)/2 AS valclf " &
                                      "FROM usina u, complastro_fisico i " &
                                      "WHERE u.codempre = '" & Session("strCodEmpre") & "' " &
                                      "AND u.codusina = i.codusina " & AndUsina(envioDeDados.CboUsinaText) &
                                      "AND i.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'" &
                                        "), " &
                                        "(" & 'MIF
                                            "SELECT SUM(i.valmifemp) As valmif " &
                                      "FROM usina u, motivoinfl i " &
                                      "WHERE u.codempre = '" & Session("strCodEmpre") & "' " &
                                      "AND u.codusina = i.codusina " & AndUsina(envioDeDados.CboUsinaText) &
                                      "AND i.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'" &
                                        "), " &
                                          "(" & 'Paral CO 
                                            "SELECT Count(*) " &
                                             "FROM paralemp_co p, gerad g, usina u " &
                                             "WHERE p.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' " &
                                             "AND p.codequip = g.codgerad " &
                                             "AND g.codusina = u.codusina " & AndUsina(envioDeDados.CboUsinaText) &
                                             "AND u.codempre = '" & Session("strCodEmpre") & "'" &
                                            "), " &
                                          "0, " & 'cota
                                          "(" & 'RFC 
                                            "SELECT SUM(r.valrfcemp)/2 As valrfc " &
                                          "FROM usina u, rest_falta_comb r " &
                                          "WHERE u.codempre = '" & Session("strCodEmpre") & "' " &
                                          "AND u.codusina = r.codusina " & AndUsina(envioDeDados.CboUsinaText) &
                                          "AND r.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'" &
                                            "), " &
                                          "(" & 'RMP
                                        "SELECT SUM(r.valrmpemp) As valrmp " &
                                      "FROM usina u, tb_rmp r " &
                                      "WHERE u.codempre = '" & Session("strCodEmpre") & "' " &
                                      "AND u.codusina = r.codusina " & AndUsina(envioDeDados.CboUsinaText) &
                                      "AND r.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'" &
                                            "), " &
                                          "(" & 'GFM
                                        "SELECT SUM(r.valgfmemp) As valgfm " &
                                      "FROM usina u, tb_gfm r " &
                                      "WHERE u.codempre = '" & Session("strCodEmpre") & "' " &
                                      "AND u.codusina = r.codusina " & AndUsina(envioDeDados.CboUsinaText) &
                                      "AND r.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'" &
                                            "), " &
                                          "(" & 'CFM
                                        "SELECT SUM(r.valcfmemp) As valcfm " &
                                      "FROM usina u, tb_cfm r " &
                                      "WHERE u.codempre = '" & Session("strCodEmpre") & "' " &
                                      "AND u.codusina = r.codusina " & AndUsina(envioDeDados.CboUsinaText) &
                                      "AND r.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'" &
                                            "), " &
                                          "(" & 'SOM
                                        "SELECT SUM(r.valsomemp) As valsom " &
                                      "FROM usina u, tb_som r " &
                                      "WHERE u.codempre = '" & Session("strCodEmpre") & "' " &
                                      "AND u.codusina = r.codusina " & AndUsina(envioDeDados.CboUsinaText) &
                                      "AND r.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'" &
                                            "), " &
                                          "(" & 'GES
                                        "SELECT SUM(r.valGESemp) As valGES " &
                                      "FROM usina u, tb_GES r " &
                                      "WHERE u.codempre = '" & Session("strCodEmpre") & "' " &
                                      "AND u.codusina = r.codusina " & AndUsina(envioDeDados.CboUsinaText) &
                                      "AND r.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'" &
                                            "), " &
                                          "(" & 'GEC
                                        "SELECT SUM(r.valGECemp) As valGEC " &
                                      "FROM usina u, tb_GEC r " &
                                      "WHERE u.codempre = '" & Session("strCodEmpre") & "' " &
                                      "AND u.codusina = r.codusina " & AndUsina(envioDeDados.CboUsinaText) &
                                      "AND r.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'" &
                                            "), " &
                                          "(" & 'DCA
                                        "SELECT SUM(r.valDCAemp) As valDCA " &
                                      "FROM usina u, tb_DCA r " &
                                      "WHERE u.codempre = '" & Session("strCodEmpre") & "' " &
                                      "AND u.codusina = r.codusina " & AndUsina(envioDeDados.CboUsinaText) &
                                      "AND r.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'" &
                                            "), " &
                                          "(" & 'DCR
                                        "SELECT SUM(r.valDCRemp) As valDCR " &
                                      "FROM usina u, tb_DCR r " &
                                      "WHERE u.codempre = '" & Session("strCodEmpre") & "' " &
                                      "AND u.codusina = r.codusina " & AndUsina(envioDeDados.CboUsinaText) &
                                      "AND r.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'" &
                                            "), " &
                                          "(" & 'IR1
                                        "SELECT SUM(r.valIR1emp) As valIR1 " &
                                      "FROM usina u, tb_IR1 r " &
                                      "WHERE u.codempre = '" & Session("strCodEmpre") & "' " &
                                      "AND u.codusina = r.codusina " & AndUsina(envioDeDados.CboUsinaText) &
                                      "AND r.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'" &
                                            "), " &
                                          "(" & 'IR2
                                        "SELECT SUM(r.valIR2emp) As valIR2 " &
                                      "FROM usina u, tb_IR2 r " &
                                      "WHERE u.codempre = '" & Session("strCodEmpre") & "' " &
                                      "AND u.codusina = r.codusina " & AndUsina(envioDeDados.CboUsinaText) &
                                      "AND r.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'" &
                                            "), " &
                                          "(" & 'IR3
                                        "SELECT SUM(r.valIR3emp) As valIR3 " &
                                      "FROM usina u, tb_IR3 r " &
                                      "WHERE u.codempre = '" & Session("strCodEmpre") & "' " &
                                      "AND u.codusina = r.codusina " & AndUsina(envioDeDados.CboUsinaText) &
                                      "AND r.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'" &
                                            "), " &
                                          "(" & 'IR4
                                        "SELECT SUM(r.valIR4emp) As valIR4 " &
                                      "FROM usina u, tb_IR4 r " &
                                      "WHERE u.codempre = '" & Session("strCodEmpre") & "' " &
                                      "AND u.codusina = r.codusina " & AndUsina(envioDeDados.CboUsinaText) &
                                      "AND r.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'" &
                                            "), " &
                                          "(" & 'RRO
                                        "SELECT SUM(g.valrroemp) AS valRRO FROM usina u, tb_rro g WHERE u.codempre = '" & Session("strCodEmpre") & "' AND u.codusina = g.codusina " & AndUsina(envioDeDados.CboUsinaText) & " AND g.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'" &
                                        "), " &
                                          "(" & 'CVU
                                            "select round(rs.val_cvuprog,2)as totCvu from tb_respdemanda r join tb_respdemandasemanal rs on rs.id_respdemandasemanal = r.id_respdemandasemanal where r.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'                         and exists (select 1
                                            from usina u where u.codusina = r.cod_usinaprog " & AndUsina(envioDeDados.CboUsinaText) & " and u.codempre = '" & Session("strCodEmpre") & "')" &
                                            "), " &
                                          "'" & UsuarID & "')"

                    Cmd.CommandText = sql
                    Cmd.ExecuteNonQuery()

                    'Verifica se a Carga, Geração e Intercâmbio foram gravados
                    RegistrarLog("Verifica se a Carga, Geração e Intercâmbio foram gravados. Data Escolhida: " + Format(Session("datEscolhida"), "yyyyMMdd"))

                    If VerificaEventos(Format(Session("datEscolhida"), "yyyyMMdd"), envioDeDados.CboEmpresaText) Then
                        RegistrarLog("Evento verificado.")

                        GravaEventoPDP("11", Format(Session("datEscolhida"), "yyyyMMdd"), envioDeDados.CboEmpresaText, DateAdd(DateInterval.Second, 1, Now), "PDPEnvDado", UsuarID)
                        RegistrarLog("Evento PDP Gravado.")
                    End If

                    'Grava arquivo para a empresa selecionada na data selecionada
                    ' Conforme definição da Marta, será necessário gerar
                    ' o arquivo para dados da área de transferência também,
                    ' foi por isso que isolei o if abaixo e estou passando o valor
                    ' da opção selecionada para a rotina gravaarquivotexto

                    strData = IIf(envioDeDados.CboDataValue = "", dtPDP, IIf(cmbDtValue <> "", Mid(envioDeDados.CboDataValue, 7, 4) & Mid(envioDeDados.CboDataValue, 4, 2) & Mid(envioDeDados.CboDataValue, 1, 2), dtPDP))

                    RegistrarLog("Gravando Arquivo Texto: strCodEmpresa: " + envioDeDados.CboEmpresaValue + ", strData: " + strData + ", strArq: " + strArq + ", blnTipoEnvio: 1, bFtp: True, strOpcao: " + strOpcao + ", strDataHora: " + dtPDP + ", strArqDestino:'' ")

                    Dim dadosarquivo As New GeracaoArquivo
                    dadosarquivo.strCodEmpresa = envioDeDados.CboEmpresaValue
                    dadosarquivo.strData = strData
                    dadosarquivo.strArq = strArq
                    dadosarquivo.blnTipoEnvio = "1"
                    dadosarquivo.bFtp = True
                    dadosarquivo.bRetorno = bRetorno
                    dadosarquivo.strOpcao = strOpcao
                    dadosarquivo.strDataHora = dtPDP
                    dadosarquivo.strArqDestino = ""
                    dadosarquivo.Cmd = Cmd
                    dadosarquivo.strPerfil = PerfilID
                    dadosarquivo.codUsina = envioDeDados.CboUsinaText

                    If Not GravaArquivoTexto(dadosarquivo, bRetorno) Then
                        'Session("strMensagem") = "Não foi possível gravar o arquivo texto, os dados podem estar incorretos. "

                        Session("strMensagem") = "Não foi possivel enviar os dados (4), por favor tente novamente ou comunique a ocorrência ao ONS."
                        RegistrarLog(Session("strMensagem"))
                        System.Web.HttpContext.Current.Response.Redirect("frmMensagem.aspx")
                    Else
                        ' Conforme definição da Marta, após enviar o arquivo para área de FTP
                        ' apagá-lo do diretório Download

                        If bRetorno = True Then
                            System.Web.HttpContext.Current.Response.Write("<script language=JavaScript>")
                            System.Web.HttpContext.Current.Response.Write("window.open('frmRecibo.aspx?strNomeArquivo=" & Trim(strNomeArquivo) & "' , '', 'width=515,height=610,status=no,scrollbars=no,titlebar=yes,menubar=no')")
                            System.Web.HttpContext.Current.Response.Write("</script>")
                        Else
                            'Session("strMensagem") = "Não foi possível gravar o arquivo na área de FTP!"
                            Session("strMensagem") = "Não foi possivel enviar os dados (5), por favor tente novamente ou comunique a ocorrência ao ONS."
                            RegistrarLog(Session("strMensagem"))
                            System.Web.HttpContext.Current.Response.Redirect("frmMensagem.aspx")
                        End If
                    End If

                    RegistrarLog("Realizando commit.")

                    'Cadastra o Marco ref. ao Envio de Dados
                    InsereMarcoPDP(K_Const_strIdMarco_EnvioDados, Format(Session("datEscolhida"), "yyyyMMdd"), UsuarID, UsuarID, Cmd)


                    Cmd.Dispose()
                    Conn.Close()
                    Conn.Dispose()
                    RegistrarLog("Dispose realizado.")

                    'PreencheCabecalho() preenche a tabela da tela de envio de dados
                    'PreencheTable() preenche a tabela da tela de envio de dados

                    retorno = True

                Catch ex As Exception
                    RegistrarLogErro(ex)

                    Cmd.Dispose()

                    If Conn.State = ConnectionState.Open Then
                        Conn.Close()
                        Conn.Dispose()
                    End If

                    retorno = False

                    'Session("strMensagem") = "Não foi possivel enviar os dados." & ex.Message & Cmd.CommandText
                    Session("strMensagem") = "Não foi possivel enviar os dados (6), por favor tente novamente ou comunique a ocorrência ao ONS."
                    RegistrarLog(Session("strMensagem"))
                    System.Web.HttpContext.Current.Response.Redirect("frmMensagem.aspx")

                Finally
                    If Conn.State = ConnectionState.Open Then
                        Conn.Close()
                        Conn.Dispose()
                    End If

                End Try
            Else
                Session("strMensagem") = "Usuário não tem permissão para Transferir os valores."
                RegistrarLog(Session("strMensagem"))
                System.Web.HttpContext.Current.Response.Redirect("frmMensagem.aspx")
            End If
        Else
            Session("strMensagem") = "Usuário não tem Verificando se Geração permissão para Transferir os valores."
            RegistrarLog(Session("strMensagem"))
            System.Web.HttpContext.Current.Response.Redirect("frmMensagem.aspx")
        End If

        Return retorno

    End Function

    Public Function EnviarDados(ByVal dtPDP As String, envioDeDados As EnvioDeDados) As Boolean

        Dim strData As String
        'Dim objPermissao As OnsClasses.OnsInterfaceUnica.OnsPermissao

        Dim bRetorno As Boolean = False

        Dim strArq As String = DiretorioEnviodDeDados()

        Dim retorno As Boolean = False

        Dim cmbDtValue As String = envioDeDados.CboDataValue
        'Session("datEscolhida") = IIf(Session("datEscolhida") <> "", Session("datEscolhida"), dtPDP)


        RegistrarLog("Inicio do processo de envio de dados.")

        RegistrarLog("Verificando permissão do usuário. INS, PDPColMaUG usuarID: " + UsuarID)

        'objPermissao = New OnsClasses.OnsInterfaceUnica.OnsPermissao("INS", "PDPColMaUG", UsuarID)
        'Verifica se o usuário tem permissão para salvar os registros
        RegistrarLog("INS - EstaAutorizado: " + EstaAutorizado.ToString())

        If EstaAutorizado Then

            RegistrarLog("Verificando permissão do usuário. DEL, PDPColMaUG usuarID: " + UsuarID)
            'objPermissao = New OnsClasses.OnsInterfaceUnica.OnsPermissao("DEL", "PDPColMaUG", UsuarID)
            'Verifica se o usuário tem permissão para salvar os registros
            RegistrarLog("DEL - EstaAutorizado: " + EstaAutorizado.ToString())

            If EstaAutorizado Then

                Dim dblGeracao, dblCarga, dblIntercambio, dblVazao, dblCota, dblRestricao,
                    dblManutencao, dblInflexibilidade, dblEnergetica, dblEletrica,
                    dblExporta, dblImporta, dblMRE, dblMIF, dblPCC, dblMCO, dblMOS, dblMEG,
                    dblERP, dblDSP, dblCLF, dblPCO, dblRFC, dblRMP, dblGFM, dblCFM, dblSOM,
                    dblGEC, dblGES, dblDCA, dblDCR, dblIR1, dblIR2, dblIR3, dblIR4, dblRRO, dblCVU As Double

                Dim objTrans As SqlTransaction
                Dim Conn As SqlConnection = New SqlConnection
                Dim Cmd As SqlCommand = New SqlCommand

                Conn.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
                Conn.Open()
                Cmd.Connection = Conn
                RegistrarLog("Criando a OnsConnection(conn) - pdp - PDPEnvDado " + UsuarID)

                Dim intI As Integer
                Dim ConnIns As SqlConnection = New SqlConnection
                Dim CmdIns As SqlCommand = New SqlCommand

                Conn.ConnectionString = ConfigurationManager.AppSettings.Get("pdpSQL").ToString()
                ConnIns.Open()
                CmdIns.Connection = ConnIns
                RegistrarLog("Criando a OnsConnection(ConnIns) - pdp - PDPEnvDado " + UsuarID)

                Dim existeOferta As Boolean = False

                dblGeracao = 0
                dblCarga = 0
                dblIntercambio = 0
                dblVazao = 0
                dblCota = 0
                dblRestricao = 0
                dblManutencao = 0
                dblInflexibilidade = 0
                dblEnergetica = 0
                dblEletrica = 0
                dblExporta = 0
                dblImporta = 0
                dblMRE = 0
                dblMIF = 0
                dblPCC = 0
                dblMCO = 0
                dblMOS = 0
                dblMEG = 0
                dblERP = 0
                dblDSP = 0
                dblCLF = 0
                dblPCO = 0
                dblRFC = 0
                dblRMP = 0
                dblGFM = 0
                dblCFM = 0
                dblSOM = 0
                dblGES = 0
                dblGEC = 0
                dblDCA = 0
                dblDCR = 0
                dblIR1 = 0
                dblIR2 = 0
                dblIR3 = 0
                dblIR4 = 0
                dblRRO = 0
                dblCVU = 0

                strOpcao = ""
                objTrans = Conn.BeginTransaction
                Cmd.Transaction = objTrans

                existeOferta = ExisteOfertaDeDemanda(dtPDP, Cmd)

                Try
                    'GERAÇÃO
                    RegistrarLog("Verificando se Geração e Geração, Carga, Intercâmbio estão selecionados")
                    If (existeOferta) Or (envioDeDados.ChkGeracao And envioDeDados.ChkEnvia1) Then

                        strOpcao &= "1" '0
                        RegistrarLog("Variavel Opcao recebe o valor 1")
                        'Conforme definição da Marta em 17/07/2003,
                        'gravar o conteúdo do que está na área de transferência
                        'para os valores da empresa

                        'objTrans = Conn.BeginTransaction
                        'Cmd.Transaction = objTrans
                        RegistrarLog("Geração - Update:")
                        Cmd.CommandText = "UPDATE despa " &
                                      "SET valdespaemp = valdespatran " &
                                      "WHERE codusina IN (" &
                                      "SELECT codusina " &
                                      "FROM usina u " &
                                      "WHERE codempre = '" & Session("strCodEmpre") & "'" & AndUsina(envioDeDados.CboUsinaText) & ") " &
                                      "AND datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'"
                        RegistrarLog(Cmd.CommandText)
                        Cmd.ExecuteNonQuery()
                        'objTrans.Commit()
                        RegistrarLog("Geração - Select:")
                        Cmd.CommandText = "SELECT SUM(g.valdespaemp) AS valGeracao " &
                                      "FROM usina u, despa g " &
                                      "WHERE u.codempre = '" & Session("strCodEmpre") & "' " &
                                      "AND u.codusina = g.codusina " & AndUsina(envioDeDados.CboUsinaText) &
                                      "AND g.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'"
                        RegistrarLog(Cmd.CommandText)

                        Dim rsGeracao As SqlDataReader = Cmd.ExecuteReader
                        'RegistrarLog("Resultado Geracao: " + rsGeracao.Read.ToString())
                        If rsGeracao.Read Then

                            dblGeracao = IIf(Not IsDBNull(rsGeracao("valGeracao")), rsGeracao("valGeracao"), 0)
                            RegistrarLog("Informações de Geracao: " + dblGeracao.ToString())
                        End If
                        rsGeracao.Close()
                        rsGeracao = Nothing
                    Else
                        strOpcao &= "0"
                        RegistrarLog("Variavel Opcao recebe o valor 0")
                    End If

                    'RRO
                    RegistrarLog("Verificando se RRO está selecionado")
                    If (existeOferta) Or (envioDeDados.ChkRROE) Then

                        strOpcao &= "1" '0
                        RegistrarLog("Variavel Opcao recebe o valor 1")

                        RegistrarLog("RRO - Update:")
                        Cmd.CommandText = "UPDATE tb_rro " &
                                      "SET valrroemp = valrrotran " &
                                      "WHERE codusina IN (" &
                                      "SELECT codusina " &
                                      "FROM usina u " &
                                      "WHERE codempre = '" & Session("strCodEmpre") & "'" & AndUsina(envioDeDados.CboUsinaText) & ") " &
                                      "AND datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'"
                        RegistrarLog(Cmd.CommandText)
                        Cmd.ExecuteNonQuery()

                        RegistrarLog("RRO - Select:")
                        Cmd.CommandText = "SELECT SUM(g.valrroemp) AS valRRO " &
                                      "FROM usina u, tb_rro g " &
                                      "WHERE u.codempre = '" & Session("strCodEmpre") & "' " &
                                      "AND u.codusina = g.codusina " & AndUsina(envioDeDados.CboUsinaText) &
                                      "AND g.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'"
                        RegistrarLog(Cmd.CommandText)

                        Dim rsRRO As SqlDataReader = Cmd.ExecuteReader
                        If rsRRO.Read Then

                            dblRRO = IIf(Not IsDBNull(rsRRO("valRRO")), rsRRO("valRRO"), 0)
                            RegistrarLog("Informações de RRO: " + dblRRO.ToString())
                        End If
                        rsRRO.Close()
                        rsRRO = Nothing
                    Else
                        strOpcao &= "0"
                        RegistrarLog("Variavel Opcao recebe o valor 0")
                    End If

                    'CARGA
                    RegistrarLog("Verificando se Carga e Geração, Carga, Intercâmbio estão selecionados")
                    If envioDeDados.ChkCarga And envioDeDados.ChkEnvia1 Then
                        'Conforme definição da Marta em 17/07/2003,
                        'gravar o conteúdo do que está na área de transferência
                        'para os valores da empresa
                        strOpcao &= "1" '1
                        RegistrarLog("Variavel Opcao recebe o valor 1")
                        'objTrans = Conn.BeginTransaction
                        'Cmd.Transaction = objTrans
                        RegistrarLog("Carga - Update:")
                        Cmd.CommandText = "UPDATE carga " &
                                      "SET valcargaemp = valcargatran " &
                                      "WHERE codempre = '" & Session("strCodEmpre") & "' " &
                                      "AND datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'"
                        RegistrarLog(Cmd.CommandText)
                        Cmd.ExecuteNonQuery()
                        'objTrans.Commit()

                        RegistrarLog("Carga - Select:")
                        Cmd.CommandText = "SELECT SUM(valcargaemp) AS valCarga " &
                                      "FROM carga " &
                                      "WHERE codempre = '" & Session("strCodEmpre") & "' " &
                                      "AND datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'"
                        RegistrarLog(Cmd.CommandText)
                        'Cmd.ExecuteNonQuery()
                        Dim rsCarga As SqlDataReader = Cmd.ExecuteReader

                        'RegistrarLog("Resultado Geracao: " + rsCarga.Read.ToString())
                        If rsCarga.Read Then

                            dblCarga = IIf(Not IsDBNull(rsCarga("valCarga")), rsCarga("valCarga"), 0)
                            RegistrarLog("Informações da Carga: " + dblCarga.ToString())
                        End If
                        rsCarga.Close()
                        rsCarga = Nothing
                    Else
                        strOpcao &= "0"
                        RegistrarLog("Variavel Opcao recebe o valor 0")
                    End If

                    'INTERCÂMBIO
                    RegistrarLog("Verificando se Intercambio e eração, Carga, Intercâmbio estão selecionados")
                    If (existeOferta) Or (envioDeDados.ChkIntercambio And envioDeDados.ChkEnvia1) Then
                        'Conforme definição da Marta em 17/07/2003,
                        'gravar o conteúdo do que está na área de transferência
                        'para os valores da empresa
                        strOpcao &= "1" '2
                        RegistrarLog("Variavel Opcao recebe o valor 1")
                        'objTrans = Conn.BeginTransaction
                        'Cmd.Transaction = objTrans
                        RegistrarLog("Intercambio - Update:")
                        Cmd.CommandText = "UPDATE inter " &
                                      "SET valinteremp = valintertran " &
                                      "WHERE codemprede = '" & Session("strCodEmpre") & "' " &
                                      "AND datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'"
                        RegistrarLog(Cmd.CommandText)
                        Cmd.ExecuteNonQuery()
                        'objTrans.Commit()

                        RegistrarLog("Intercambio - Select:")
                        Cmd.CommandText = "SELECT SUM(valinteremp) AS valIntercambio " &
                                      "FROM inter " &
                                      "WHERE datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' " &
                                      "AND codemprede = '" & Session("strCodEmpre") & "'"
                        RegistrarLog(Cmd.CommandText)
                        Dim rsIntercambio As SqlDataReader = Cmd.ExecuteReader
                        'RegistrarLog("Resultado Intercambio: " + rsIntercambio.Read.ToString())
                        If rsIntercambio.Read Then

                            dblIntercambio = IIf(Not IsDBNull(rsIntercambio("valIntercambio")), rsIntercambio("valIntercambio"), 0)
                            RegistrarLog("Informações da Intercambio: " + dblIntercambio.ToString())
                        End If
                        rsIntercambio.Close()
                        rsIntercambio = Nothing
                    Else
                        strOpcao &= "0"
                        RegistrarLog("Variavel Opcao recebe o valor 0")
                    End If

                    '-- CRQ2345 (15/08/2012)
                    RegistrarLog("Verifica se Vazão(Envio) esta selecionado")
                    If envioDeDados.ChkVazE Then
                        'Conforme definição da Marta em 17/07/2003,
                        'gravar o conteúdo do que está na área de transferência
                        'para os valores da empresa
                        'VAZÃO
                        strOpcao &= "1" '3
                        RegistrarLog("Variavel Opcao recebe o valor 1")
                        'objTrans = Conn.BeginTransaction
                        'Cmd.Transaction = objTrans
                        'VAZÃO
                        RegistrarLog("Vazão - Update:")
                        Cmd.CommandText = "UPDATE vazao " &
                                      "SET valturb = valturbtran, valvert = valverttran, valaflu = valaflutran, valtransf = valtransftran " &
                                      "WHERE codusina IN (SELECT codusina FROM usina u WHERE codempre = '" & Session("strCodEmpre") & "'" & AndUsina(envioDeDados.CboUsinaText) & ") " &
                                      "AND datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'"
                        RegistrarLog(Cmd.CommandText)
                        Cmd.ExecuteNonQuery()
                        'COTA INICIAL
                        RegistrarLog("Cota Inicial - Update:")
                        Cmd.CommandText = "UPDATE cota " &
                                      "SET cotaini = cotainitran, cotafim = cotafimtran, outrasestruturas = outrasestruturastran, comentariopdf = comentariopdftran " &
                                      "WHERE codusina IN (SELECT codusina FROM usina u WHERE codempre = '" & Session("strCodEmpre") & "'" & AndUsina(envioDeDados.CboUsinaText) & ") " &
                                      "AND datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'"
                        RegistrarLog(Cmd.CommandText)
                        Cmd.ExecuteNonQuery()

                        'objTrans.Commit()
                        RegistrarLog("Cota Vazao - Select:")
                        Cmd.CommandText = "SELECT SUM(v.valturb) AS valTurbinada, SUM(v.valvert) AS valVertida, SUM(v.valaflu) AS valAfluente, SUM(c.cotaini) AS valCotaInicial, SUM(c.cotafim) AS valCotaFinal, SUM(c.outrasestruturas) AS valOutrasEstr, SUM(v.valtransf) as ValVazaoTransferida " &
                                      "FROM vazao v, usina u, OUTER cota c " &
                                      "WHERE u.codempre = '" & Session("strCodEmpre") & "' " &
                                      "AND u.codusina = v.codusina " &
                                      "AND u.codusina = c.codusina " & AndUsina(envioDeDados.CboUsinaText) &
                                      "AND u.tipusina = 'H' " &
                                      "AND v.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' " &
                                      "AND c.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'"
                        RegistrarLog(Cmd.CommandText)

                        Dim rsVazao As SqlDataReader = Cmd.ExecuteReader
                        'RegistrarLog("Resultado Vazão: " + rsVazao.Read.ToString())
                        If rsVazao.Read Then

                            'VAZÃO
                            dblVazao = IIf(Not IsDBNull(rsVazao("valTurbinada")), rsVazao("valTurbinada"), 0)
                            dblVazao += IIf(Not IsDBNull(rsVazao("valVertida")), rsVazao("valVertida"), 0)
                            dblVazao += IIf(Not IsDBNull(rsVazao("valAfluente")), rsVazao("valAfluente"), 0)
                            dblVazao += IIf(Not IsDBNull(rsVazao("ValVazaoTransferida")), rsVazao("ValVazaoTransferida"), 0)

                            RegistrarLog("Informações da Vazao: Turbinada: " + dblVazao.ToString())

                            'COTA INICIAL
                            dblCota = IIf(Not IsDBNull(rsVazao("valCotaInicial")), rsVazao("valCotaInicial"), 0)
                            dblCota += IIf(Not IsDBNull(rsVazao("valCotaFinal")), rsVazao("valCotaFinal"), 0)
                            dblCota += IIf(Not IsDBNull(rsVazao("valOutrasEstr")), rsVazao("valOutrasEstr"), 0)

                            RegistrarLog("Informações da Vazao: Cota Inicial: " + dblCota.ToString())

                        End If
                        rsVazao.Close()
                        rsVazao = Nothing
                    Else
                        strOpcao &= "0"
                        RegistrarLog("Variavel Opcao recebe o valor 0")
                    End If

                    RegistrarLog("Verificando se Inflexibilidade(envio) está selecionado")
                    If envioDeDados.ChkIfxE Then
                        'Conforme definição da Marta em 17/07/2003,
                        'gravar o conteúdo do que está na área de transferência
                        'para os valores da empresa
                        'INFLEXIBILIDADE
                        strOpcao &= "1" '4
                        RegistrarLog("Variavel Opcao recebe o valor 1")
                        'objTrans = Conn.BeginTransaction
                        'Cmd.Transaction = objTrans
                        RegistrarLog("Inflexibilidade - Update:")
                        Cmd.CommandText = "UPDATE inflexibilidade " &
                                      "SET valflexiemp = valflexitran " &
                                      "WHERE codusina IN (" &
                                      "SELECT codusina " &
                                      "FROM usina u " &
                                      "WHERE codempre = '" & Session("strCodEmpre") & "'" & AndUsina(envioDeDados.CboUsinaText) & ") " &
                                      "AND datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'"
                        RegistrarLog(Cmd.CommandText)
                        Cmd.ExecuteNonQuery()
                        'objTrans.Commit()

                        RegistrarLog("Inflexibilidade - Select:")
                        Cmd.CommandText = "SELECT SUM(i.valflexiemp) AS valInflexibilidade " &
                                      "FROM usina u, inflexibilidade i " &
                                      "WHERE u.codempre = '" & Session("strCodEmpre") & "' " &
                                      "AND u.codusina = i.codusina " & AndUsina(envioDeDados.CboUsinaText) &
                                      "AND i.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'"
                        RegistrarLog(Cmd.CommandText)

                        Dim rsInflexibilidade As SqlDataReader = Cmd.ExecuteReader
                        'RegistrarLog("Resultado Inflexibilidade: " + rsInflexibilidade.Read.ToString())
                        If rsInflexibilidade.Read Then
                            dblInflexibilidade = IIf(Not IsDBNull(rsInflexibilidade("valInflexibilidade")), rsInflexibilidade("valInflexibilidade"), 0)
                            RegistrarLog("Informações da Inflexibilidade: " + dblInflexibilidade.ToString())
                        End If
                        rsInflexibilidade.Close()
                        rsInflexibilidade = Nothing
                    Else
                        strOpcao &= "0"
                        RegistrarLog("Variavel Opcao recebe o valor 0")
                    End If

                    RegistrarLog("Verificando se Razão Energética(envio) está selecionado ")
                    If envioDeDados.ChkZenE Then
                        'Conforme definição da Marta em 17/07/2003,
                        'gravar o conteúdo do que está na área de transferência
                        'para os valores da empresa
                        'RAZÃO ENERGÉTICA
                        strOpcao &= "1" '5
                        RegistrarLog("Variavel Opcao recebe o valor 1")
                        'objTrans = Conn.BeginTransaction
                        'Cmd.Transaction = objTrans
                        RegistrarLog("Razão Energética - Update:")
                        Cmd.CommandText = "UPDATE razaoener " &
                                      "SET valrazeneremp = valrazenertran " &
                                      "WHERE codusina IN (" &
                                      "SELECT codusina " &
                                      "FROM usina u " &
                                      "WHERE codempre = '" & Session("strCodEmpre") & "'" & AndUsina(envioDeDados.CboUsinaText) & ") " &
                                      "AND datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'"
                        RegistrarLog(Cmd.CommandText)
                        Cmd.ExecuteNonQuery()
                        'objTrans.Commit()
                        RegistrarLog("Razão Energética - Select:")
                        Cmd.CommandText = "SELECT SUM(r.valrazeneremp) AS valEnergetica " &
                                      "FROM usina u, razaoener  r " &
                                      "WHERE u.codempre = '" & Session("strCodEmpre") & "' " &
                                      "AND u.codusina = r.codusina " & AndUsina(envioDeDados.CboUsinaText) &
                                      "AND r.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'"
                        RegistrarLog(Cmd.CommandText)
                        Dim rsEnergetica As SqlDataReader = Cmd.ExecuteReader
                        'RegistrarLog("Resultado Razão Energética: " + rsEnergetica.Read.ToString())
                        If rsEnergetica.Read Then

                            dblEnergetica = IIf(Not IsDBNull(rsEnergetica("valEnergetica")), rsEnergetica("valEnergetica"), 0)
                            RegistrarLog("Informações da Razão Energética: " + dblEnergetica.ToString())
                        End If
                        rsEnergetica.Close()
                        rsEnergetica = Nothing
                    Else
                        strOpcao &= "0"
                        RegistrarLog("Variavel Opcao recebe o valor 0")
                    End If

                    RegistrarLog("Verificando se Razão Elétrica(envio) está selecionado")
                    If envioDeDados.ChkZelE Then
                        'Conforme definição da Marta em 17/07/2003,
                        'gravar o conteúdo do que está na área de transferência
                        'para os valores da empresa
                        'RAZÃO ELÉTRICA
                        strOpcao &= "1" '6
                        RegistrarLog("Variavel Opcao recebe o valor 1")
                        'objTrans = Conn.BeginTransaction
                        'Cmd.Transaction = objTrans
                        RegistrarLog("Razão Elétrica - Update:")
                        Cmd.CommandText = "UPDATE razaoelet " &
                                      "SET valrazeletemp = valrazelettran " &
                                      "WHERE codusina IN (" &
                                      "SELECT codusina " &
                                      "FROM usina u " &
                                      "WHERE codempre = '" & Session("strCodEmpre") & "'" & AndUsina(envioDeDados.CboUsinaText) & ") " &
                                      "AND datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'"
                        RegistrarLog(Cmd.CommandText)
                        Cmd.ExecuteNonQuery()
                        'objTrans.Commit()
                        RegistrarLog("Razão Elétrica - Select:")
                        Cmd.CommandText = "SELECT SUM(r.valrazeletemp) AS valEletrica " &
                                      "FROM usina u, razaoelet  r " &
                                      "WHERE u.codempre = '" & Session("strCodEmpre") & "' " &
                                      "AND u.codusina = r.codusina " & AndUsina(envioDeDados.CboUsinaText) &
                                      "AND r.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'"
                        RegistrarLog(Cmd.CommandText)
                        Dim rsEletrica As SqlDataReader = Cmd.ExecuteReader
                        'RegistrarLog("Resultado Razão Elétrica: " + rsEletrica.Read.ToString())
                        If rsEletrica.Read Then

                            dblEletrica = IIf(Not IsDBNull(rsEletrica("valEletrica")), rsEletrica("valEletrica"), 0)
                            RegistrarLog("Informações da Razão Elétrica: " + dblEletrica.ToString())
                        End If
                        rsEletrica.Close()
                        rsEletrica = Nothing
                    Else
                        strOpcao &= "0"
                        RegistrarLog("Variavel Opcao recebe o valor 0")
                    End If

                    RegistrarLog("Verificando se Restrições de usinas e Manutenções e Restrições estão selecionados")
                    If envioDeDados.ChkRestricao And envioDeDados.ChkEnvia2 Then
                        'RESTRIÇÃO DE USINAS
                        'Transferindo dados para a área de transferência
                        'Eliminando os registros que estão na área de transferência
                        'Cmd.CommandText = "select * from temprestrusina " & _
                        '                "Where datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' And " & _
                        '                    "codusina In " & _
                        '                    " (Select codusina " & _
                        '                    "  From usina " & _
                        '                    "  where codempre = '" & Session("strCodEmpre") & "')"
                        'Dim rsExisteRestrUsinaEmp As SqlDataReader = Cmd.ExecuteReader
                        'If rsExisteRestrUsinaEmp.Read() Then
                        '    rsExisteRestrUsinaEmp.Close()
                        strOpcao &= "1" '7
                        RegistrarLog("Variavel Opcao recebe o valor 1")
                        'objTrans = Conn.BeginTransaction
                        'Cmd.Transaction = objTrans
                        RegistrarLog("Restrições de Usinas - Delete:")
                        Cmd.CommandText = "DELETE FROM temprestrusina " &
                                      "WHERE datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' " &
                                      "AND codusina IN (" &
                                      "SELECT codusina " &
                                      "FROM usina u " &
                                      "WHERE codempre = '" & Session("strCodEmpre") & "'" & AndUsina(envioDeDados.CboUsinaText) & ")"
                        RegistrarLog(Cmd.CommandText)

                        Cmd.ExecuteNonQuery()
                        'Else
                        '    rsExisteRestrUsinaEmp.Close()

                        'End If
                        'rsExisteRestrUsinaEmp = Nothing

                        'Transferindo os dados da empresa para a área de transferência
                        RegistrarLog("Restrições de Usinas - Select:")
                        CmdIns.CommandText = "SELECT r.codusina, r.datinirestr, r.datfimrestr, r.intinirestr, " &
                                         "r.intfimrestr, r.valrestr, r.refoutrosis, r.id_motivorestricao, r.obsrestr, r.datpdp " &
                                         "FROM restrusinaemp r, usina u " &
                                         "WHERE r.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' " &
                                         "AND r.codusina = u.codusina " & AndUsina(envioDeDados.CboUsinaText) &
                                         "AND u.codempre = '" & Session("strCodEmpre") & "'"
                        RegistrarLog(Cmd.CommandText)
                        Dim rsRestrUsinaEmp As SqlDataReader = CmdIns.ExecuteReader
                        dblRestricao = 0
                        'RegistrarLog("Resultado Restrições de Usinas : " + rsRestrUsinaEmp.Read.ToString())
                        Do While rsRestrUsinaEmp.Read
                            RegistrarLog("Restrições de Usinas - INSERT:")
                            Cmd.CommandText = "INSERT INTO temprestrusina (" &
                                          "codusina, " &
                                          "datinirestr, " &
                                          "datfimrestr, " &
                                          "intinirestr, " &
                                          "intfimrestr, " &
                                          "valrestr, " &
                                          "refoutrosis, " &
                                          "id_motivorestricao, " &
                                          "obsrestr, " &
                                          "datpdp " &
                                          ") VALUES (" &
                                          "'" & rsRestrUsinaEmp("codusina") & "', " &
                                          "'" & rsRestrUsinaEmp("datinirestr") & "', " &
                                          "'" & rsRestrUsinaEmp("datfimrestr") & "', " &
                                          "" & rsRestrUsinaEmp("intinirestr") & ", " &
                                          "" & rsRestrUsinaEmp("intfimrestr") & ", " &
                                          "'" & rsRestrUsinaEmp("valrestr") & "', " &
                                          "" & IIf(IsDBNull(rsRestrUsinaEmp("refoutrosis")), "Null", rsRestrUsinaEmp("refoutrosis")) & ", " &
                                          "" & IIf(IsDBNull(rsRestrUsinaEmp("id_motivorestricao")), "Null", rsRestrUsinaEmp("id_motivorestricao")) & ", " &
                                          "" & IIf(IsDBNull(rsRestrUsinaEmp("obsrestr")), "Null", "'" & rsRestrUsinaEmp("obsrestr") & "'") & ", " &
                                          "'" & rsRestrUsinaEmp("datpdp") & "')"

                            RegistrarLog(Cmd.CommandText)
                            ''cmdins
                            'Cmd.Parameters.Clear()
                            'If Not IsDBNull(rsRestrUsinaEmp("obsrestr")) Then
                            '    'cmdins 
                            '    Cmd.CommandText = Cmd.CommandText & _
                            '                        "?, "
                            '    Dim arrByte(Len(Trim(rsRestrUsinaEmp("obsrestr"))) - 1) As Byte
                            '    For intI = 0 To Len(Trim(rsRestrUsinaEmp("obsrestr"))) - 1
                            '        arrByte(intI) = CByte(Asc(Mid(rsRestrUsinaEmp("obsrestr"), intI + 1, 1)))
                            '    Next
                            '    Dim objParam As New OleDb.OleDbParameter("@obsrestr", OleDb.OleDbType.LongVarBinary, arrByte.Length, ParameterDirection.Input, False, 0, 0, Nothing, DataRowVersion.Current, arrByte)
                            '    'cmdins
                            '    Cmd.Parameters.Add(objParam)
                            'Else
                            '    'cmdins
                            '    Cmd.CommandText = Cmd.CommandText & _
                            '                        "Null, "
                            'End If
                            'cmdins

                            Cmd.ExecuteNonQuery()
                            dblRestricao += 1
                        Loop
                        rsRestrUsinaEmp.Close()
                        rsRestrUsinaEmp = Nothing
                        'objTrans.Commit()

                        'Fim da Transferência de Restrição de Usinas

                        'RESTRIÇÃO DE UNIDADES GERADORAS
                        'Transferindo os dados da Empresa para a área de transferência
                        'Eliminando os registros que estão na área de transferência
                        'Cmd.CommandText = "Select * from temprestrgerad " & _
                        '                    "Where datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' And " & _
                        '                    "codgerad In " & _
                        '                    "(Select codgerad " & _
                        '                    " From gerad " & _
                        '                    " Where codusina In " & _
                        '                    " (Select codusina " & _
                        '                    "  From usina " & _
                        '                    "  where codempre = '" & Session("strCodEmpre") & "'))"
                        'Dim rsExisteRestrGeradEmp As SqlDataReader = Cmd.ExecuteReader
                        'If (rsExisteRestrGeradEmp.Read() = True) Then
                        '    rsExisteRestrGeradEmp.Close()
                        'objTrans = Conn.BeginTransaction
                        'Cmd.Transaction = objTrans
                        RegistrarLog("Restrições de Usinas Geradoras - DELETE:")
                        Cmd.CommandText = "DELETE FROM temprestrgerad " &
                                      "WHERE datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' " &
                                      "AND codgerad IN (" &
                                      "SELECT codgerad " &
                                      "FROM gerad " &
                                      "WHERE codusina IN (" &
                                      "SELECT codusina " &
                                      "FROM usina u " &
                                      "WHERE codempre = '" & Session("strCodEmpre") & "'" & AndUsina(envioDeDados.CboUsinaText) & "))"

                        RegistrarLog(Cmd.CommandText)
                        Cmd.ExecuteNonQuery()
                        'Else
                        '    rsExisteRestrGeradEmp.Close()

                        'End If
                        'rsExisteRestrGeradEmp = Nothing


                        'Transferindo os dados da empresa para a área de transferência
                        RegistrarLog("Restrições de Usinas Geradoras - Select:")
                        CmdIns.CommandText = "SELECT r.codgerad, r.datinirestr, r.datfimrestr, r.intinirestr, " &
                                         "r.intfimrestr, r.valrestr, r.refoutrosis, r.id_motivorestricao, r.obsrestr, r.datpdp " &
                                         "FROM restrgerademp r, gerad g, usina u " &
                                         "WHERE r.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' " &
                                         "AND r.codgerad = g.codgerad " &
                                         "AND g.codusina = u.codusina " & AndUsina(envioDeDados.CboUsinaText) &
                                         "AND u.codempre = '" & Session("strCodEmpre") & "'"

                        RegistrarLog(Cmd.CommandText)
                        Dim rsRestrGeradEmp As SqlDataReader = CmdIns.ExecuteReader

                        'RegistrarLog("Resultado Restrições de Usinas Geradoras: " + rsRestrGeradEmp.Read.ToString())
                        Do While rsRestrGeradEmp.Read
                            'cmdins
                            RegistrarLog("Restrições de Usinas Geradoras - Insert:")
                            Cmd.CommandText = "INSERT INTO temprestrgerad (" &
                                          "codgerad, " &
                                          "datinirestr, " &
                                          "datfimrestr, " &
                                          "intinirestr, " &
                                          "intfimrestr, " &
                                          "valrestr, " &
                                          "refoutrosis, " &
                                          "id_motivorestricao, " &
                                          "obsrestr, " &
                                          "datpdp " &
                                          ") VALUES (" &
                                          "'" & rsRestrGeradEmp("codgerad") & "', " &
                                          "'" & rsRestrGeradEmp("datinirestr") & "', " &
                                          "'" & rsRestrGeradEmp("datfimrestr") & "', " &
                                          "" & rsRestrGeradEmp("intinirestr") & ", " &
                                          "" & rsRestrGeradEmp("intfimrestr") & ", " &
                                          "'" & rsRestrGeradEmp("valrestr") & "', " &
                                          "" & IIf(IsDBNull(rsRestrGeradEmp("refoutrosis")), "Null", rsRestrGeradEmp("refoutrosis")) & ", " &
                                          "" & IIf(IsDBNull(rsRestrGeradEmp("id_motivorestricao")), "Null", rsRestrGeradEmp("id_motivorestricao")) & ", " &
                                          "" & IIf(IsDBNull(rsRestrGeradEmp("obsrestr")), "Null", "'" & rsRestrGeradEmp("obsrestr") & "'") & ", " &
                                          "'" & rsRestrGeradEmp("datpdp") & "')"
                            RegistrarLog(Cmd.CommandText)
                            ''cmdins
                            'Cmd.Parameters.Clear()
                            'If Not IsDBNull(rsRestrGeradEmp("obsrestr")) Then
                            '    'cmdins
                            '    Cmd.CommandText = Cmd.CommandText & _
                            '                            "?, "
                            '    Dim arrByte(Len(Trim(rsRestrGeradEmp("obsrestr"))) - 1) As Byte
                            '    For intI = 0 To Len(Trim(rsRestrGeradEmp("obsrestr"))) - 1
                            '        arrByte(intI) = CByte(Asc(Mid(rsRestrGeradEmp("obsrestr"), intI + 1, 1)))
                            '    Next
                            '    Dim objParam As New OleDb.OleDbParameter("@obsrestr", OleDb.OleDbType.LongVarBinary, arrByte.Length, ParameterDirection.Input, False, 0, 0, Nothing, DataRowVersion.Current, arrByte)
                            '    'cmdins
                            '    Cmd.Parameters.Add(objParam)
                            'Else
                            '    'cmdins
                            '    Cmd.CommandText = Cmd.CommandText & _
                            '                        "Null, "
                            'End If
                            ''cmdins

                            Cmd.ExecuteNonQuery()
                            dblRestricao += 1
                        Loop
                        rsRestrGeradEmp.Close()
                        rsRestrGeradEmp = Nothing
                        'objTrans.Commit()
                    Else
                        strOpcao &= "0"
                        RegistrarLog("Variavel Opcao recebe o valor 0")
                    End If

                    RegistrarLog("Verificando se Manutenção e Manutenções e Restrições estão selecionados")
                    If envioDeDados.ChkManutencao And envioDeDados.ChkEnvia2 Then
                        'MANUTENÇÃO DE UNIDADES GERADORAS
                        'Transferindo da Tabela de Empresa para a de área de Transferência
                        'Eliminando os registros que que estão na área de transferência
                        'Cmd.CommandText = "select * from tempparal " & _
                        '                "Where datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' And " & _
                        '                "codequip In " & _
                        '                "(Select codgerad As codequip " & _
                        '                " From gerad " & _
                        '                " Where codusina In " & _
                        '                " (Select codusina " & _
                        '                "  From usina " & _
                        '                "  where codempre = '" & Session("strCodEmpre") & "'))"
                        'Dim rsExisteParalEmp As SqlDataReader = Cmd.ExecuteReader
                        'If (rsExisteParalEmp.Read() = True) Then
                        '    rsExisteParalEmp.Close()
                        strOpcao &= "1" '8
                        RegistrarLog("Variavel Opcao recebe o valor 1")
                        'objTrans = Conn.BeginTransaction
                        'Cmd.Transaction = objTrans

                        RegistrarLog("Manutenção - Delete:")
                        Cmd.CommandText = "DELETE FROM tempparal " &
                                      "WHERE datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' " &
                                      "AND codequip IN (" &
                                      "SELECT codgerad AS codequip " &
                                      "FROM gerad " &
                                      "WHERE codusina IN (" &
                                      "SELECT codusina " &
                                      "FROM usina u " &
                                      "WHERE codempre = '" & Session("strCodEmpre") & "'" & AndUsina(envioDeDados.CboUsinaText) & "))"

                        RegistrarLog(Cmd.CommandText)
                        Cmd.ExecuteNonQuery()

                        'Else
                        '    rsExisteParalEmp.Close()
                        'End If
                        'rsExisteParalEmp = Nothing

                        'Transferindo os dados da empresa para a área de transferência
                        RegistrarLog("Manutenção - Select:")
                        CmdIns.CommandText = "SELECT p.codequip, p.tipequip, p.datiniparal, p.datfimparal, " &
                                         "p.intiniparal, p.intfimparal, p.codnivel, p.refoutrosis, p.indcont, " &
                                         "p.intinivoltaparal, p.intfimvoltaparal, p.datpdp " &
                                         "FROM paralemp p, gerad g, usina u " &
                                         "WHERE p.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' " &
                                         "AND p.codequip = g.codgerad " &
                                         "AND g.codusina = u.codusina " & AndUsina(envioDeDados.CboUsinaText) &
                                         "AND u.codempre = '" & Session("strCodEmpre") & "'"

                        RegistrarLog(Cmd.CommandText)
                        Dim rsParalEmp As SqlDataReader = CmdIns.ExecuteReader
                        dblManutencao = 0
                        'RegistrarLog("Resultado Manutenção: " + rsParalEmp.Read.ToString())
                        Do While rsParalEmp.Read
                            'cmdins
                            RegistrarLog("Manutenção - Insert:")
                            Cmd.CommandText = "INSERT INTO tempparal (" &
                                          "codequip, " &
                                          "tipequip, " &
                                          "datiniparal, " &
                                          "datfimparal, " &
                                          "intiniparal, " &
                                          "intfimparal, " &
                                          "codnivel, " &
                                          "refoutrosis, " &
                                          "indcont, " &
                                          "intinivoltaparal, " &
                                          "intfimvoltaparal, " &
                                          "datpdp " &
                                          ") VALUES (" &
                                          "'" & rsParalEmp("codequip") & "', " &
                                          "'" & rsParalEmp("tipequip") & "', " &
                                          "'" & rsParalEmp("datiniparal") & "', " &
                                          "'" & rsParalEmp("datfimparal") & "', " &
                                          "" & rsParalEmp("intiniparal") & ", " &
                                          "" & rsParalEmp("intfimparal") & ", " &
                                          "'" & rsParalEmp("codnivel") & "', " &
                                          "" & IIf(IsDBNull(rsParalEmp("refoutrosis")), "Null", rsParalEmp("refoutrosis")) & ", " &
                                          "'" & rsParalEmp("indcont") & "', " &
                                          "" & IIf(IsDBNull(rsParalEmp("intinivoltaparal")), "Null", rsParalEmp("intinivoltaparal")) & ", " &
                                          "" & IIf(IsDBNull(rsParalEmp("intfimvoltaparal")), "Null", rsParalEmp("intfimvoltaparal")) & ", " &
                                          "'" & rsParalEmp("datpdp") & "')"
                            RegistrarLog(Cmd.CommandText)
                            ''cmdins
                            'Cmd.Parameters.Clear()
                            'If Not IsDBNull(rsParalEmp("obsparal")) Then
                            '    'cmdins
                            '    Cmd.CommandText = Cmd.CommandText & _
                            '                        "?, "
                            '    Dim arrByte(Len(Trim(rsParalEmp("obsparal"))) - 1) As Byte
                            '    For intI = 0 To Len(Trim(rsParalEmp("obsparal"))) - 1
                            '        arrByte(intI) = CByte(Asc(Mid(rsParalEmp("obsparal"), intI + 1, 1)))
                            '    Next
                            '    Dim objParam As New OleDb.OleDbParameter("@obsparal", OleDb.OleDbType.LongVarBinary, arrByte.Length, ParameterDirection.Input, False, 0, 0, Nothing, DataRowVersion.Current, arrByte)
                            '    'cmdins
                            '    Cmd.Parameters.Add(objParam)
                            'Else
                            '    'cmdins
                            '    Cmd.CommandText = Cmd.CommandText & _
                            '                        "Null, "
                            'End If
                            ''cmdins
                            Cmd.ExecuteNonQuery()
                            dblManutencao += 1
                        Loop
                        rsParalEmp.Close()
                        rsParalEmp = Nothing
                        'objTrans.Commit()
                        'Fim da Transferência
                    Else
                        strOpcao &= "0"
                        RegistrarLog("Variavel Opcao recebe o valor 0")
                    End If

                    RegistrarLog("Verificando se Parada de Máquinas por Conveniência Operativa(envio) está selecionado ")
                    If envioDeDados.ChkPcoE Then
                        'PARADA DE MÁQUINAS POR CONVENIÊNCIA OPERATIVA
                        'Transferindo da Tabela de Empresa para a de área de Transferência
                        strOpcao &= "1" '9
                        RegistrarLog("Variavel Opcao recebe o valor 1")
                        'objTrans = Conn.BeginTransaction
                        'Cmd.Transaction = objTrans

                        RegistrarLog("Parada de Máquinas por Conveniência Operativa - Delete:")
                        Cmd.CommandText = "DELETE FROM tempparal_co " &
                                      "WHERE datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' " &
                                      "AND codequip IN " &
                                      "(SELECT codgerad AS codequip " &
                                      "FROM gerad " &
                                      "WHERE codusina IN " &
                                      "(SELECT codusina " &
                                      "FROM usina u " &
                                      "WHERE codempre = '" & Session("strCodEmpre") & "'" & AndUsina(envioDeDados.CboUsinaText) & "))"
                        RegistrarLog(Cmd.CommandText)
                        Cmd.ExecuteNonQuery()

                        'Transferindo os dados da empresa para a área de transferência
                        RegistrarLog("Parada de Máquinas por Conveniência Operativa - Select:")
                        CmdIns.CommandText = "SELECT p.codequip, p.tipequip, p.datiniparal, p.datfimparal, p.intiniparal, p.intfimparal, p.datpdp " &
                                         "FROM paralemp_co p, gerad g, usina u " &
                                         "WHERE p.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "' " &
                                         "AND p.codequip = g.codgerad " &
                                         "AND g.codusina = u.codusina " & AndUsina(envioDeDados.CboUsinaText) &
                                         "AND u.codempre = '" & Session("strCodEmpre") & "'"
                        RegistrarLog(Cmd.CommandText)
                        Dim rsParalEmp As SqlDataReader = CmdIns.ExecuteReader
                        dblPCO = 0
                        'RegistrarLog("Resultado Parada de Máquinas por Conveniência Operativa: " + rsParalEmp.Read.ToString())
                        Do While rsParalEmp.Read
                            'cmdins
                            RegistrarLog("Parada de Máquinas por Conveniência Operativa - Insert:")
                            Cmd.CommandText = "INSERT INTO tempparal_co (" &
                                          "codequip, " &
                                          "tipequip, " &
                                          "datiniparal, " &
                                          "datfimparal, " &
                                          "intiniparal, " &
                                          "intfimparal, " &
                                          "datpdp " &
                                          ") Values (" &
                                          "'" & rsParalEmp("codequip") & "', " &
                                          "'" & rsParalEmp("tipequip") & "', " &
                                          "'" & rsParalEmp("datiniparal") & "', " &
                                          "'" & rsParalEmp("datfimparal") & "', " &
                                          "" & rsParalEmp("intiniparal") & ", " &
                                          "" & rsParalEmp("intfimparal") & ", " &
                                          "'" & rsParalEmp("datpdp") & "')"
                            RegistrarLog(Cmd.CommandText)
                            Cmd.ExecuteNonQuery()
                            dblPCO += 1
                        Loop
                        rsParalEmp.Close()
                        rsParalEmp = Nothing
                        'objTrans.Commit()
                        'Fim da Transferência
                    Else
                        strOpcao &= "0"
                        RegistrarLog("Variavel Opcao recebe o valor 0")
                    End If

                    RegistrarLog("Verificando se Exportação(envio) está selecionado")
                    If envioDeDados.ChkExpE Then
                        'Conforme definição da Marta em 17/07/2003,
                        'gravar o conteúdo do que está na área de transferência
                        'para os valores da empresa
                        'EXPORTAÇÃO
                        strOpcao &= "1" '10
                        RegistrarLog("Variavel Opcao recebe o valor 1")
                        'objTrans = Conn.BeginTransaction
                        'Cmd.Transaction = objTrans
                        RegistrarLog("Exportação - Update:")
                        Cmd.CommandText = "UPDATE exporta " &
                                      "SET valexportaemp = valexportatran " &
                                      "WHERE codusina IN (" &
                                      "SELECT codusina " &
                                      "FROM usina u " &
                                      "WHERE codempre = '" & Session("strCodEmpre") & "'" & AndUsina(envioDeDados.CboUsinaText) & ") " &
                                      "AND datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'"
                        RegistrarLog(Cmd.CommandText)
                        Cmd.ExecuteNonQuery()
                        'objTrans.Commit()
                        RegistrarLog("Exportação - Select:")
                        Cmd.CommandText = "SELECT SUM(i.valexportaemp) AS valexporta " &
                                      "FROM usina u, exporta i " &
                                      "WHERE u.codempre = '" & Session("strCodEmpre") & "' " &
                                      "AND u.codusina = i.codusina " & AndUsina(envioDeDados.CboUsinaText) &
                                      "AND i.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'"
                        RegistrarLog(Cmd.CommandText)
                        Dim rsExporta As SqlDataReader = Cmd.ExecuteReader
                        'RegistrarLog("Resultado Exportação: " + rsExporta.Read.ToString())
                        If rsExporta.Read Then

                            dblExporta = IIf(Not IsDBNull(rsExporta("valexporta")), rsExporta("valexporta"), 0)
                            RegistrarLog("Informações da Exportação: " + dblExporta.ToString())
                        End If
                        rsExporta.Close()
                        rsExporta = Nothing
                    Else
                        strOpcao &= "0"
                        RegistrarLog("Variavel Opcao recebe o valor 0")
                    End If

                    RegistrarLog("Verificando se Importação(envio) está selecionado")
                    If envioDeDados.ChkImpE Then
                        'Conforme definição da Marta em 17/07/2003,
                        'gravar o conteúdo do que está na área de transferência
                        'para os valores da empresa
                        'IMPORTAÇÃO
                        strOpcao &= "1" '11
                        RegistrarLog("Variavel Opcao recebe o valor 1")
                        'objTrans = Conn.BeginTransaction
                        'Cmd.Transaction = objTrans
                        RegistrarLog("Importação - Update:")
                        Cmd.CommandText = "UPDATE importa " &
                                      "SET valimportaemp = valimportatran " &
                                      "WHERE codusina IN (" &
                                      "SELECT codusina " &
                                      "FROM usina u " &
                                      "WHERE codempre = '" & Session("strCodEmpre") & "'" & AndUsina(envioDeDados.CboUsinaText) & ") " &
                                      "AND datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'"
                        RegistrarLog(Cmd.CommandText)
                        Cmd.ExecuteNonQuery()
                        'objTrans.Commit()
                        RegistrarLog("Importação - Select:")
                        Cmd.CommandText = "SELECT SUM(i.valimportaemp) AS valimporta " &
                                      "FROM usina u, importa i " &
                                      "WHERE u.codempre = '" & Session("strCodEmpre") & "' " &
                                      "AND u.codusina = i.codusina " & AndUsina(envioDeDados.CboUsinaText) &
                                      "AND i.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'"
                        RegistrarLog(Cmd.CommandText)
                        Dim rsImporta As SqlDataReader = Cmd.ExecuteReader

                        'RegistrarLog("Resultado Importação: " + rsImporta.Read.ToString())
                        If rsImporta.Read Then

                            dblImporta = IIf(Not IsDBNull(rsImporta("valimporta")), rsImporta("valimporta"), 0)
                            RegistrarLog("Informações da Importação: " + dblImporta.ToString())
                        End If
                        rsImporta.Close()
                        rsImporta = Nothing
                    Else
                        strOpcao &= "0"
                        RegistrarLog("Variavel Opcao recebe o valor 0")

                    End If

                    RegistrarLog("Verificando se Motivo de Despacho Razão Elétrica(envio) está selecionado")
                    If envioDeDados.ChkMreE Then
                        'Motivo de Despacho Razão Elétrica
                        strOpcao &= "1" '12
                        RegistrarLog("Variavel Opcao recebe o valor 1")
                        'objTrans = Conn.BeginTransaction
                        'Cmd.Transaction = objTrans
                        RegistrarLog("Motivo de Despacho Razão Elétrica - Update:")
                        Cmd.CommandText = "UPDATE motivorel " &
                                      "SET valmreemp = valmretran " &
                                      "WHERE codusina IN (" &
                                      "SELECT codusina " &
                                      "FROM usina u " &
                                      "WHERE codempre = '" & Session("strCodEmpre") & "'" & AndUsina(envioDeDados.CboUsinaText) & ") " &
                                      "AND datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'"
                        RegistrarLog(Cmd.CommandText)
                        Cmd.ExecuteNonQuery()
                        'objTrans.Commit()
                        RegistrarLog("Motivo de Despacho Razão Elétrica - Select:")
                        Cmd.CommandText = "SELECT SUM(i.valmreemp) AS valmre " &
                                      "FROM usina u, motivorel i " &
                                      "WHERE u.codempre = '" & Session("strCodEmpre") & "' " &
                                      "AND u.codusina = i.codusina " & AndUsina(envioDeDados.CboUsinaText) &
                                      "AND i.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'"
                        RegistrarLog(Cmd.CommandText)
                        Dim rsMRE As SqlDataReader = Cmd.ExecuteReader

                        'RegistrarLog("Resultado Motivo de Despacho Razão Elétrica: " + rsMRE.Read.ToString())
                        If rsMRE.Read Then

                            dblMRE = IIf(Not IsDBNull(rsMRE("valmre")), rsMRE("valmre"), 0)
                            RegistrarLog("Informações da Motivo de Despacho Razão Elétrica: " + dblMRE.ToString())
                        End If
                        rsMRE.Close()
                        rsMRE = Nothing
                    Else
                        strOpcao &= "0"
                        RegistrarLog("Variavel Opcao recebe o valor 0")
                    End If

                    RegistrarLog("Verificando se Motivo de Despacho por Inflexibilidade(envio) está selecionado")
                    If envioDeDados.ChkMifE Then
                        'Motivo de Despacho por Inflexibilidade
                        strOpcao &= "1" '13
                        RegistrarLog("Variavel Opcao recebe o valor 1")
                        'objTrans = Conn.BeginTransaction
                        'Cmd.Transaction = objTrans

                        RegistrarLog("Motivo de Despacho por Inflexibilidade - Update:")
                        Cmd.CommandText = "UPDATE motivoinfl " &
                                      "SET valmifemp = valmiftran " &
                                      "WHERE codusina IN (" &
                                      "SELECT codusina " &
                                      "FROM usina u " &
                                      "WHERE codempre = '" & Session("strCodEmpre") & "'" & AndUsina(envioDeDados.CboUsinaText) & ") " &
                                      "AND datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'"
                        RegistrarLog(Cmd.CommandText)
                        Cmd.ExecuteNonQuery()
                        'objTrans.Commit()
                        RegistrarLog("Motivo de Despacho por Inflexibilidade - select:")
                        Cmd.CommandText = "SELECT SUM(i.valmifemp) As valmif " &
                                      "FROM usina u, motivoinfl i " &
                                      "WHERE u.codempre = '" & Session("strCodEmpre") & "' " &
                                      "AND u.codusina = i.codusina " & AndUsina(envioDeDados.CboUsinaText) &
                                      "AND i.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'"
                        RegistrarLog(Cmd.CommandText)
                        Dim rsMIF As SqlDataReader = Cmd.ExecuteReader
                        'RegistrarLog("Resultado Motivo de Despacho por Inflexibilidade: " + rsMIF.Read.ToString())
                        If rsMIF.Read Then


                            dblMIF = IIf(Not IsDBNull(rsMIF("valmif")), rsMIF("valmif"), 0)
                            RegistrarLog("Informações da Motivo de Despacho por Inflexibilidade: " + dblMIF.ToString())
                        End If
                        rsMIF.Close()
                        rsMIF = Nothing
                    Else
                        strOpcao &= "0"
                        RegistrarLog("Variavel Opcao recebe o valor 0")

                    End If

                    RegistrarLog("Verificando se Perdas Consumo Interno e Compensação(envio) está selecionado")
                    If envioDeDados.ChkPccE Then
                        'Perdas Consumo Interno e Compensação
                        strOpcao &= "1" '14
                        RegistrarLog("Variavel Opcao recebe o valor 1")
                        'objTrans = Conn.BeginTransaction
                        'Cmd.Transaction = objTrans

                        RegistrarLog("Perdas Consumo Interno e Compensação - Update:")
                        Cmd.CommandText = "UPDATE perdascic " &
                                      "SET valpccemp = valpcctran " &
                                      "WHERE codusina IN (" &
                                      "SELECT codusina " &
                                      "FROM usina u " &
                                      "WHERE codempre = '" & Session("strCodEmpre") & "'" & AndUsina(envioDeDados.CboUsinaText) & ") " &
                                      "AND datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'"
                        RegistrarLog(Cmd.CommandText)
                        Cmd.ExecuteNonQuery()
                        'objTrans.Commit()
                        RegistrarLog("Perdas Consumo Interno e Compensação - Select:")
                        Cmd.CommandText = "SELECT SUM(i.valpccemp) AS valpcc " &
                                      "FROM usina u, perdascic i " &
                                      "WHERE u.codempre = '" & Session("strCodEmpre") & "' " &
                                      "AND u.codusina = i.codusina " & AndUsina(envioDeDados.CboUsinaText) &
                                      "AND i.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'"
                        RegistrarLog(Cmd.CommandText)
                        Dim rsPCC As SqlDataReader = Cmd.ExecuteReader

                        'RegistrarLog("Resultado Perdas Consumo Interno e Compensação: " + rsPCC.Read.ToString())
                        If rsPCC.Read Then
                            dblPCC = IIf(Not IsDBNull(rsPCC("valpcc")), rsPCC("valpcc"), 0)
                            RegistrarLog("Informações da Perdas Consumo Interno e Compensação: " + dblPCC.ToString())
                        End If
                        rsPCC.Close()
                        rsPCC = Nothing
                    Else
                        strOpcao &= "0"
                        RegistrarLog("Variavel Opcao recebe o valor 0")
                    End If

                    RegistrarLog("Verificando se Número Máquinas Paradas por Conveniência Operativa(envio) está selecionado")
                    If envioDeDados.ChkMcoE Then
                        'Número Máquinas Paradas por Conveniência Operativa
                        strOpcao &= "1" '15
                        RegistrarLog("Variavel Opcao recebe o valor 1")
                        'objTrans = Conn.BeginTransaction
                        'Cmd.Transaction = objTrans
                        RegistrarLog("Número Máquinas Paradas por Conveniência Operativa - Update:")
                        Cmd.CommandText = "UPDATE conveniencia_oper " &
                                      "SET valmcoemp = valmcotran " &
                                      "WHERE codusina IN (" &
                                      "SELECT codusina " &
                                      "FROM usina u " &
                                      "WHERE codempre = '" & Session("strCodEmpre") & "'" & AndUsina(envioDeDados.CboUsinaText) & ") " &
                                      "AND datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'"
                        RegistrarLog(Cmd.CommandText)
                        Cmd.ExecuteNonQuery()
                        'objTrans.Commit()
                        RegistrarLog("Número Máquinas Paradas por Conveniência Operativa - Select:")
                        Cmd.CommandText = "SELECT SUM(i.valmcoemp) AS valmco " &
                                      "FROM usina u, conveniencia_oper i " &
                                      "WHERE u.codempre = '" & Session("strCodEmpre") & "' " &
                                      "AND u.codusina = i.codusina " & AndUsina(envioDeDados.CboUsinaText) &
                                      "AND i.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'"
                        RegistrarLog(Cmd.CommandText)
                        Dim rsMCO As SqlDataReader = Cmd.ExecuteReader
                        'RegistrarLog("Resultado Número Máquinas Paradas por Conveniência Operativa: " + rsMCO.Read.ToString())
                        If rsMCO.Read Then
                            dblMCO = IIf(Not IsDBNull(rsMCO("valmco")), rsMCO("valmco"), 0)
                            RegistrarLog("Informações de Número Máquinas Paradas por Conveniência Operativa: " + dblMCO.ToString())
                        End If
                        rsMCO.Close()
                        rsMCO = Nothing
                    Else
                        strOpcao &= "0"
                        RegistrarLog("Variavel Opcao recebe o valor 0")
                    End If

                    RegistrarLog("Verificando se Número Máquinas Operando como Síncrono(envio) está selecionado")
                    If envioDeDados.ChkMosE Then
                        'Número Máquinas Operando como Síncrono
                        strOpcao &= "1" '16
                        RegistrarLog("Variavel Opcao recebe o valor 1")
                        'objTrans = Conn.BeginTransaction
                        'Cmd.Transaction = objTrans
                        RegistrarLog("Número Máquinas Operando como Síncrono - Update:")
                        Cmd.CommandText = "UPDATE oper_sincrono " &
                                      "SET valmosemp = valmostran " &
                                      "WHERE codusina IN (" &
                                      "SELECT codusina " &
                                      "FROM usina u " &
                                      "WHERE codempre = '" & Session("strCodEmpre") & "'" & AndUsina(envioDeDados.CboUsinaText) & ") " &
                                      "AND datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'"
                        RegistrarLog(Cmd.CommandText)
                        Cmd.ExecuteNonQuery()
                        'objTrans.Commit()
                        RegistrarLog("Número Máquinas Operando como Síncrono - Select:")
                        Cmd.CommandText = "SELECT SUM(i.valmosemp) AS valmos " &
                                      "FROM usina u, oper_sincrono i " &
                                      "WHERE u.codempre = '" & Session("strCodEmpre") & "' " &
                                      "AND u.codusina = i.codusina " & AndUsina(envioDeDados.CboUsinaText) &
                                      "AND i.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'"
                        RegistrarLog(Cmd.CommandText)
                        Dim rsMOS As SqlDataReader = Cmd.ExecuteReader
                        'RegistrarLog("Resultado Intercambio: " + rsMOS.Read.ToString())
                        If rsMOS.Read Then
                            dblMOS = IIf(Not IsDBNull(rsMOS("valmos")), rsMOS("valmos"), 0)
                            RegistrarLog("Informações de Geracao: " + dblMOS.ToString())
                        End If
                        rsMOS.Close()
                        rsMOS = Nothing
                    Else
                        strOpcao &= "0"
                        RegistrarLog("Variavel Opcao recebe o valor 0")
                    End If

                    RegistrarLog("Verificando se Número Máquinas Gerando(envio) está selecionado")
                    If envioDeDados.ChkMegE Then
                        'Número Máquinas Gerando
                        strOpcao &= "1" '17
                        RegistrarLog("Variavel Opcao recebe o valor 1")
                        'objTrans = Conn.BeginTransaction
                        'Cmd.Transaction = objTrans

                        RegistrarLog("Número Máquinas Gerando - Update:")
                        Cmd.CommandText = "UPDATE maq_gerando " &
                                      "SET valmegemp = valmegtran " &
                                      "WHERE codusina IN (" &
                                      "SELECT codusina " &
                                      "FROM usina u " &
                                      "WHERE codempre = '" & Session("strCodEmpre") & "'" & AndUsina(envioDeDados.CboUsinaText) & ") " &
                                      "AND datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'"

                        RegistrarLog(Cmd.CommandText)
                        Cmd.ExecuteNonQuery()
                        'objTrans.Commit()
                        RegistrarLog("Número Máquinas Gerando - Select:")
                        Cmd.CommandText = "SELECT SUM(i.valmegemp) AS valmeg " &
                                      "FROM usina u, maq_gerando i " &
                                      "WHERE u.codempre = '" & Session("strCodEmpre") & "' " &
                                      "AND u.codusina = i.codusina " & AndUsina(envioDeDados.CboUsinaText) &
                                      "AND i.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'"

                        RegistrarLog(Cmd.CommandText)
                        Dim rsMEG As SqlDataReader = Cmd.ExecuteReader

                        'RegistrarLog("Resultado Número Máquinas Gerando: " + rsMEG.Read.ToString())
                        If rsMEG.Read Then
                            dblMEG = IIf(Not IsDBNull(rsMEG("valmeg")), rsMEG("valmeg"), 0)

                            RegistrarLog("Informações de Número Máquinas Gerando: " + dblMEG.ToString())
                        End If
                        rsMEG.Close()
                        rsMEG = Nothing
                    Else
                        strOpcao &= "0"
                        RegistrarLog("Variavel Opcao recebe o valor 0")
                    End If

                    RegistrarLog("Verificando se Energia de Reposição e Perdas(envio) está selecionado")
                    If envioDeDados.ChkErpE Then
                        'Energia de Reposição e Perdas
                        strOpcao &= "1" '18
                        RegistrarLog("Variavel Opcao recebe o valor 1")
                        'objTrans = Conn.BeginTransaction
                        'Cmd.Transaction = objTrans
                        RegistrarLog("Energia de Reposição e Perdas - Update:")
                        Cmd.CommandText = "UPDATE energia_reposicao " &
                                      "SET valerpemp = valerptran " &
                                      "WHERE codusina IN (" &
                                      "SELECT codusina " &
                                      "FROM usina u " &
                                      "WHERE codempre = '" & Session("strCodEmpre") & "'" & AndUsina(envioDeDados.CboUsinaText) & ") " &
                                      "AND datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'"

                        RegistrarLog(Cmd.CommandText)
                        Cmd.ExecuteNonQuery()
                        'objTrans.Commit()
                        RegistrarLog("Energia de Reposição e Perdas - Select:")
                        Cmd.CommandText = "SELECT SUM(i.valerpemp) AS valerp " &
                                      "FROM usina u, energia_reposicao i " &
                                      "WHERE u.codempre = '" & Session("strCodEmpre") & "' " &
                                      "AND u.codusina = i.codusina " & AndUsina(envioDeDados.CboUsinaText) &
                                      "AND i.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'"
                        RegistrarLog(Cmd.CommandText)
                        Dim rsErp As SqlDataReader = Cmd.ExecuteReader

                        'RegistrarLog("Resultado Energia de Reposição e Perdas : " + rsErp.Read.ToString())
                        If rsErp.Read Then
                            dblERP = IIf(Not IsDBNull(rsErp("valerp")), rsErp("valerp"), 0)
                            RegistrarLog("Informações de Energia de Reposição e Perdas : " + dblERP.ToString())
                        End If
                        rsErp.Close()
                        rsErp = Nothing
                    Else
                        strOpcao &= "0"
                        RegistrarLog("Variavel Opcao recebe o valor 0")

                    End If

                    RegistrarLog("Verificando se Disponibilidade está selecionado")
                    If envioDeDados.ChkDspE Then
                        'Disponibilidade
                        strOpcao &= "1" '19
                        RegistrarLog("Variavel Opcao recebe o valor 1")
                        'objTrans = Conn.BeginTransaction
                        'Cmd.Transaction = objTrans
                        RegistrarLog("Disponibilidade - UPDATE:")
                        Cmd.CommandText = "UPDATE disponibilidade " &
                                      "SET valdspemp = valdsptran " &
                                      "WHERE codusina IN (" &
                                      "SELECT codusina " &
                                      "FROM usina u " &
                                      "WHERE codempre = '" & Session("strCodEmpre") & "'" & AndUsina(envioDeDados.CboUsinaText) & ") " &
                                      "AND datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'"
                        RegistrarLog(Cmd.CommandText)
                        Cmd.ExecuteNonQuery()
                        'objTrans.Commit()

                        RegistrarLog("Disponibilidade - Select:")
                        Cmd.CommandText = "SELECT SUM(i.valdspemp) AS valdsp " &
                                      "FROM usina u, disponibilidade i " &
                                      "WHERE u.codempre = '" & Session("strCodEmpre") & "' " &
                                      "AND u.codusina = i.codusina " & AndUsina(envioDeDados.CboUsinaText) &
                                      "AND i.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'"
                        RegistrarLog(Cmd.CommandText)
                        Dim rsDsp As SqlDataReader = Cmd.ExecuteReader
                        'RegistrarLog("Resultado Disponibilidade: " + rsDsp.Read.ToString())
                        If rsDsp.Read Then
                            dblDSP = IIf(Not IsDBNull(rsDsp("valdsp")), rsDsp("valdsp"), 0)
                            RegistrarLog("Informações de Disponibilidade: " + dblDSP.ToString())
                        End If
                        rsDsp.Close()
                        rsDsp = Nothing
                    Else
                        strOpcao &= "0"
                        RegistrarLog("Variavel Opcao recebe o valor 0")
                    End If

                    RegistrarLog("Verificando se Compensação de Lastro Físico está selecionado")
                    If envioDeDados.ChkClfE Then
                        'Compensação de Lastro Físico
                        strOpcao &= "1" '20
                        RegistrarLog("Variavel Opcao recebe o valor 1")
                        'objTrans = Conn.BeginTransaction
                        'Cmd.Transaction = objTrans
                        RegistrarLog("Compensação de Lastro Físico - UPDATE:")
                        Cmd.CommandText = "UPDATE complastro_fisico " &
                                      "SET valclfemp = valclftran " &
                                      "WHERE codusina IN (" &
                                      "SELECT codusina " &
                                      "FROM usina u " &
                                      "WHERE codempre = '" & Session("strCodEmpre") & "'" & AndUsina(envioDeDados.CboUsinaText) & ") " &
                                      "AND datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'"
                        RegistrarLog(Cmd.CommandText)
                        Cmd.ExecuteNonQuery()
                        'objTrans.Commit()

                        RegistrarLog("Compensação de Lastro Físico - Select:")
                        Cmd.CommandText = "SELECT SUM(i.valclfemp) AS valclf " &
                                      "FROM usina u, complastro_fisico i " &
                                      "WHERE u.codempre = '" & Session("strCodEmpre") & "' " &
                                      "AND u.codusina = i.codusina " & AndUsina(envioDeDados.CboUsinaText) &
                                      "AND i.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'"
                        RegistrarLog(Cmd.CommandText)
                        Dim rsClf As SqlDataReader = Cmd.ExecuteReader
                        'RegistrarLog("Resultado Compensação de Lastro Físico: " + rsClf.Read.ToString())
                        If rsClf.Read Then
                            dblCLF = IIf(Not IsDBNull(rsClf("valclf")), rsClf("valclf"), 0)
                            RegistrarLog("Informações de Compensação de Lastro Físico: " + dblCLF.ToString())
                        End If
                        rsClf.Close()
                        rsClf = Nothing
                    Else
                        strOpcao &= "0"
                        RegistrarLog("Variavel Opcao recebe o valor 0")
                    End If

                    RegistrarLog("Verificando se Restrição por Falta de Combustível está selecionado")
                    If envioDeDados.ChkRfcE Then
                        'Restrição por Falta de Combustível
                        strOpcao &= "1" '21
                        RegistrarLog("Variavel Opcao recebe o valor 1")
                        'objTrans = Conn.BeginTransaction
                        'Cmd.Transaction = objTrans
                        RegistrarLog("Restrição por Falta de Combustível - Update:")
                        Cmd.CommandText = "UPDATE rest_falta_comb " &
                                      "SET valrfcemp = valrfctran " &
                                      "WHERE codusina IN " &
                                      "(SELECT codusina " &
                                      "FROM usina u " &
                                      "WHERE codempre = '" & Session("strCodEmpre") & "'" & AndUsina(envioDeDados.CboUsinaText) & ") " &
                                      "AND datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'"
                        RegistrarLog(Cmd.CommandText)
                        Cmd.ExecuteNonQuery()
                        'objTrans.Commit()
                        RegistrarLog("Restrição por Falta de Combustível - Select:")
                        Cmd.CommandText = "SELECT SUM(r.valrfcemp) As valrfc " &
                                      "FROM usina u, rest_falta_comb r " &
                                      "WHERE u.codempre = '" & Session("strCodEmpre") & "' " &
                                      "AND u.codusina = r.codusina " & AndUsina(envioDeDados.CboUsinaText) &
                                      "AND r.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'"
                        RegistrarLog(Cmd.CommandText)
                        Dim rsRFC As SqlDataReader = Cmd.ExecuteReader
                        'RegistrarLog("Resultado Restrição por Falta de Combustível: " + rsRFC.Read.ToString())
                        If rsRFC.Read Then
                            dblRFC = IIf(Not IsDBNull(rsRFC("valrfc")), rsRFC("valrfc"), 0)
                            RegistrarLog("Informações de Restrição por Falta de Combustível: " + dblRFC.ToString())
                        End If
                        rsRFC.Close()
                        rsRFC = Nothing
                    Else
                        strOpcao &= "0"
                        RegistrarLog("Variavel Opcao recebe o valor 0")
                    End If

                    RegistrarLog("Verificando se Crédito por Substituição está selecionado")
                    If envioDeDados.ChkRmpE Then
                        'Garantia Energética
                        strOpcao &= "1" '22
                        RegistrarLog("Variavel Opcao recebe o valor 1")
                        'objTrans = Conn.BeginTransaction
                        'Cmd.Transaction = objTrans
                        RegistrarLog("Garantia Energética - Update:")
                        Cmd.CommandText = "UPDATE tb_rmp " &
                                      "SET valrmpemp = valrmptran " &
                                      "WHERE codusina IN " &
                                      "(SELECT codusina " &
                                      "FROM usina u " &
                                      "WHERE codempre = '" & Session("strCodEmpre") & "'" & AndUsina(envioDeDados.CboUsinaText) & ") " &
                                      "AND datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'"
                        RegistrarLog(Cmd.CommandText)
                        Cmd.ExecuteNonQuery()
                        'objTrans.Commit()
                        RegistrarLog("Garantia Energética - Select:")
                        Cmd.CommandText = "SELECT SUM(r.valrmpemp) As valrmp " &
                                      "FROM usina u, tb_rmp r " &
                                      "WHERE u.codempre = '" & Session("strCodEmpre") & "' " &
                                      "AND u.codusina = r.codusina " & AndUsina(envioDeDados.CboUsinaText) &
                                      "AND r.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'"
                        RegistrarLog(Cmd.CommandText)
                        Dim rsRMP As SqlDataReader = Cmd.ExecuteReader
                        'RegistrarLog("Resultado Garantia Energética: " + rsRMP.Read.ToString())
                        If rsRMP.Read Then
                            dblRMP = IIf(Not IsDBNull(rsRMP("valrmp")), rsRMP("valrmp"), 0)
                        End If
                        rsRMP.Close()
                        rsRMP = Nothing
                    Else
                        strOpcao &= "0"
                        RegistrarLog("Variavel Opcao recebe o valor 0")
                    End If

                    RegistrarLog("Verificando se Crédito por Substituição está selecionado")
                    If envioDeDados.ChkGfmE Then
                        'Geração Fora de Mérito
                        strOpcao &= "1" '23
                        RegistrarLog("Variavel Opcao recebe o valor 1")
                        'objTrans = Conn.BeginTransaction
                        'Cmd.Transaction = objTrans
                        RegistrarLog("Geração Fora de Mérito - Update:")
                        Cmd.CommandText = "UPDATE tb_gfm " &
                                      "SET valgfmemp = valgfmtran " &
                                      "WHERE codusina IN " &
                                      "(SELECT codusina " &
                                      "FROM usina u " &
                                      "WHERE codempre = '" & Session("strCodEmpre") & "'" & AndUsina(envioDeDados.CboUsinaText) & ") " &
                                      "AND datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'"
                        Cmd.ExecuteNonQuery()
                        RegistrarLog(Cmd.CommandText)
                        'objTrans.Commit()
                        RegistrarLog("Geração Fora de Mérito - Select:")
                        Cmd.CommandText = "SELECT SUM(r.valgfmemp) As valgfm " &
                                      "FROM usina u, tb_gfm r " &
                                      "WHERE u.codempre = '" & Session("strCodEmpre") & "' " &
                                      "AND u.codusina = r.codusina " & AndUsina(envioDeDados.CboUsinaText) &
                                      "AND r.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'"
                        RegistrarLog(Cmd.CommandText)
                        Dim rsGFM As SqlDataReader = Cmd.ExecuteReader
                        'RegistrarLog("Resultado Geração Fora de Mérito: " + rsGFM.Read.ToString())
                        If rsGFM.Read Then
                            dblGFM = IIf(Not IsDBNull(rsGFM("valgfm")), rsGFM("valgfm"), 0)
                            RegistrarLog("Informações de Geração Fora de Mérito: " + dblGFM.ToString())
                        End If
                        rsGFM.Close()
                        rsGFM = Nothing
                    Else
                        strOpcao &= "0"
                        RegistrarLog("Variavel Opcao recebe o valor 0")
                    End If

                    RegistrarLog("Verificando se Crédito por Substituição está selecionado")
                    If envioDeDados.ChkCfmE Then
                        'Crédito por Substituição
                        strOpcao &= "1" '24
                        RegistrarLog("Variavel Opcao recebe o valor 1")
                        'objTrans = Conn.BeginTransaction
                        'Cmd.Transaction = objTrans
                        RegistrarLog("Crédito por Substituição - Update:")
                        Cmd.CommandText = "UPDATE tb_cfm " &
                                      "SET valcfmemp = valcfmtran " &
                                      "WHERE codusina IN " &
                                      "(SELECT codusina " &
                                      "FROM usina u " &
                                      "WHERE codempre = '" & Session("strCodEmpre") & "'" & AndUsina(envioDeDados.CboUsinaText) & ") " &
                                      "AND datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'"
                        RegistrarLog(Cmd.CommandText)
                        Cmd.ExecuteNonQuery()
                        'objTrans.Commit()						

                        RegistrarLog("Crédito por Substituição - Select:")
                        Cmd.CommandText = "SELECT SUM(r.valcfmemp) As valcfm " &
                                      "FROM usina u, tb_cfm r " &
                                      "WHERE u.codempre = '" & Session("strCodEmpre") & "' " &
                                      "AND u.codusina = r.codusina " & AndUsina(envioDeDados.CboUsinaText) &
                                      "AND r.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'"
                        RegistrarLog(Cmd.CommandText)
                        Dim rsCFM As SqlDataReader = Cmd.ExecuteReader
                        'RegistrarLog("Resultado Crédito por Substituição: " + rsCFM.Read.ToString())
                        If rsCFM.Read Then
                            dblCFM = IIf(Not IsDBNull(rsCFM("valcfm")), rsCFM("valcfm"), 0)
                            RegistrarLog("Informações do Crédito por Substituição: " + dblCFM.ToString())
                        End If
                        rsCFM.Close()
                        rsCFM = Nothing
                    Else
                        strOpcao &= "0"
                        RegistrarLog("Variavel Opcao recebe o valor 0")
                    End If

                    RegistrarLog("Verificando se Geração Substituta está selecionado")
                    If envioDeDados.ChkSomE Then
                        'Geração Substituta
                        strOpcao &= "1" '25
                        RegistrarLog("Variavel Opcao recebe o valor 1")
                        'objTrans = Conn.BeginTransaction
                        'Cmd.Transaction = objTrans
                        RegistrarLog("Geração Substituta - Update:")
                        Cmd.CommandText = "UPDATE tb_som " &
                                      "SET valsomemp = valsomtran " &
                                      "WHERE codusina IN " &
                                      "(SELECT codusina " &
                                      "FROM usina u " &
                                      "WHERE codempre = '" & Session("strCodEmpre") & "'" & AndUsina(envioDeDados.CboUsinaText) & ") " &
                                      "AND datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'"
                        RegistrarLog(Cmd.CommandText)
                        Cmd.ExecuteNonQuery()
                        'objTrans.Commit()
                        RegistrarLog("Geração Substituta - Select:")
                        Cmd.CommandText = "SELECT SUM(r.valsomemp) As valsom " &
                                      "FROM usina u, tb_som r " &
                                      "WHERE u.codempre = '" & Session("strCodEmpre") & "' " &
                                      "AND u.codusina = r.codusina " & AndUsina(envioDeDados.CboUsinaText) &
                                      "AND r.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'"
                        RegistrarLog(Cmd.CommandText)
                        Dim rsSOM As SqlDataReader = Cmd.ExecuteReader
                        'RegistrarLog("Resultado Geração Substituta: " + rsSOM.Read.ToString())
                        If rsSOM.Read Then
                            dblSOM = IIf(Not IsDBNull(rsSOM("valsom")), rsSOM("valsom"), 0)
                            RegistrarLog("Informações do Geração Substituta: " + dblSOM.ToString())
                        End If
                        rsSOM.Close()
                        rsSOM = Nothing
                    Else
                        strOpcao &= "0"
                        RegistrarLog("Variavel Opcao recebe o valor 0")
                    End If

                    If envioDeDados.ChkGesE Then
                        'GE Substituição
                        strOpcao &= "1"
                        Cmd.CommandText = "UPDATE tb_GES " &
                                      "SET valGESemp = valGEStran " &
                                      "WHERE codusina IN " &
                                      "(SELECT codusina " &
                                      "FROM usina u " &
                                      "WHERE codempre = '" & Session("strCodEmpre") & "'" & AndUsina(envioDeDados.CboUsinaText) & ") " &
                                      "AND datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'"
                        Cmd.ExecuteNonQuery()

                        Cmd.CommandText = "SELECT SUM(r.valGESemp) As valGES " &
                                      "FROM usina u, tb_GES r " &
                                      "WHERE u.codempre = '" & Session("strCodEmpre") & "' " &
                                      "AND u.codusina = r.codusina " & AndUsina(envioDeDados.CboUsinaText) &
                                      "AND r.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'"
                        Dim rsGES As SqlDataReader = Cmd.ExecuteReader
                        If rsGES.Read Then
                            dblGES = IIf(Not IsDBNull(rsGES("valGES")), rsGES("valGES"), 0)
                        End If
                        rsGES.Close()
                        rsGES = Nothing
                    Else
                        strOpcao &= "0"
                    End If

                    If envioDeDados.ChkGecE Then
                        'GE Crédito
                        strOpcao &= "1"
                        Cmd.CommandText = "UPDATE tb_GEC " &
                                      "SET valGECemp = valGECtran " &
                                      "WHERE codusina IN " &
                                      "(SELECT codusina " &
                                      "FROM usina u " &
                                      "WHERE codempre = '" & Session("strCodEmpre") & "'" & AndUsina(envioDeDados.CboUsinaText) & ") " &
                                      "AND datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'"
                        Cmd.ExecuteNonQuery()

                        Cmd.CommandText = "SELECT SUM(r.valGECemp) As valGEC " &
                                      "FROM usina u, tb_GEC r " &
                                      "WHERE u.codempre = '" & Session("strCodEmpre") & "' " &
                                      "AND u.codusina = r.codusina " & AndUsina(envioDeDados.CboUsinaText) &
                                      "AND r.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'"
                        Dim rsGEC As SqlDataReader = Cmd.ExecuteReader
                        If rsGEC.Read Then
                            dblGEC = IIf(Not IsDBNull(rsGEC("valGEC")), rsGEC("valGEC"), 0)
                        End If
                        rsGEC.Close()
                        rsGEC = Nothing
                    Else
                        strOpcao &= "0"
                    End If

                    If envioDeDados.ChkDcaE Then
                        'Despacho Ciclo Aberto
                        strOpcao &= "1"
                        Cmd.CommandText = "UPDATE tb_DCA " &
                                      "SET valDCAemp = valDCAtran " &
                                      "WHERE codusina IN " &
                                      "(SELECT codusina " &
                                      "FROM usina u " &
                                      "WHERE codempre = '" & Session("strCodEmpre") & "'" & AndUsina(envioDeDados.CboUsinaText) & ") " &
                                      "AND datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'"
                        Cmd.ExecuteNonQuery()

                        Cmd.CommandText = "SELECT SUM(r.valDCAemp) As valDCA " &
                                      "FROM usina u, tb_DCA r " &
                                      "WHERE u.codempre = '" & Session("strCodEmpre") & "' " &
                                      "AND u.codusina = r.codusina " & AndUsina(envioDeDados.CboUsinaText) &
                                      "AND r.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'"
                        Dim rsDCA As SqlDataReader = Cmd.ExecuteReader
                        If rsDCA.Read Then
                            dblDCA = IIf(Not IsDBNull(rsDCA("valDCA")), rsDCA("valDCA"), 0)
                        End If
                        rsDCA.Close()
                        rsDCA = Nothing
                    Else
                        strOpcao &= "0"
                    End If

                    If envioDeDados.ChkDcrE Then
                        'Despacho Ciclo Reduzido
                        strOpcao &= "1"
                        Cmd.CommandText = "UPDATE tb_DCR " &
                                      "SET valDCRemp = valDCRtran " &
                                      "WHERE codusina IN " &
                                      "(SELECT codusina " &
                                      "FROM usina u " &
                                      "WHERE codempre = '" & Session("strCodEmpre") & "'" & AndUsina(envioDeDados.CboUsinaText) & ") " &
                                      "AND datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'"
                        Cmd.ExecuteNonQuery()

                        Cmd.CommandText = "SELECT SUM(r.valDCRemp) As valDCR " &
                                      "FROM usina u, tb_DCR r " &
                                      "WHERE u.codempre = '" & Session("strCodEmpre") & "' " &
                                      "AND u.codusina = r.codusina " & AndUsina(envioDeDados.CboUsinaText) &
                                      "AND r.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'"
                        Dim rsDCR As SqlDataReader = Cmd.ExecuteReader
                        If rsDCR.Read Then
                            dblDCR = IIf(Not IsDBNull(rsDCR("valDCR")), rsDCR("valDCR"), 0)
                        End If
                        rsDCR.Close()
                        rsDCR = Nothing
                    Else
                        strOpcao &= "0"
                    End If

                    If envioDeDados.ChkIr1E Then
                        'Insumo Reserva 1
                        strOpcao &= "1"
                        Cmd.CommandText = "UPDATE tb_IR1 " &
                                      "SET valIR1emp = valIR1tran " &
                                      "WHERE codusina IN " &
                                      "(SELECT codusina " &
                                      "FROM usina u " &
                                      "WHERE codempre = '" & Session("strCodEmpre") & "'" & AndUsina(envioDeDados.CboUsinaText) & ") " &
                                      "AND datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'"
                        Cmd.ExecuteNonQuery()

                        Cmd.CommandText = "SELECT SUM(r.valIR1emp) As valIR1 " &
                                      "FROM usina u, tb_IR1 r " &
                                      "WHERE u.codempre = '" & Session("strCodEmpre") & "' " &
                                      "AND u.codusina = r.codusina " & AndUsina(envioDeDados.CboUsinaText) &
                                      "AND r.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'"
                        Dim rsIR1 As SqlDataReader = Cmd.ExecuteReader
                        If rsIR1.Read Then
                            dblIR1 = IIf(Not IsDBNull(rsIR1("valIR1")), rsIR1("valIR1"), 0)
                        End If
                        rsIR1.Close()
                        rsIR1 = Nothing
                    Else
                        strOpcao &= "0"
                    End If

                    If envioDeDados.ChkIr2E Then
                        'Insumo Reserva 2
                        strOpcao &= "1"
                        Cmd.CommandText = "UPDATE tb_IR2 " &
                                      "SET valIR2emp = valIR2tran " &
                                      "WHERE codusina IN " &
                                      "(SELECT codusina " &
                                      "FROM usina u " &
                                      "WHERE codempre = '" & Session("strCodEmpre") & "'" & AndUsina(envioDeDados.CboUsinaText) & ") " &
                                      "AND datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'"
                        Cmd.ExecuteNonQuery()

                        Cmd.CommandText = "SELECT SUM(r.valIR2emp) As valIR2 " &
                                      "FROM usina u, tb_IR2 r " &
                                      "WHERE u.codempre = '" & Session("strCodEmpre") & "' " &
                                      "AND u.codusina = r.codusina " & AndUsina(envioDeDados.CboUsinaText) &
                                      "AND r.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'"
                        Dim rsIR2 As SqlDataReader = Cmd.ExecuteReader
                        If rsIR2.Read Then
                            dblIR2 = IIf(Not IsDBNull(rsIR2("valIR2")), rsIR2("valIR2"), 0)
                        End If
                        rsIR2.Close()
                        rsIR2 = Nothing
                    Else
                        strOpcao &= "0"
                    End If

                    If envioDeDados.ChkIr3E Then
                        'Insumo Reserva 3
                        strOpcao &= "1"
                        Cmd.CommandText = "UPDATE tb_IR3 " &
                                      "SET valIR3emp = valIR3tran " &
                                      "WHERE codusina IN " &
                                      "(SELECT codusina " &
                                      "FROM usina u " &
                                      "WHERE codempre = '" & Session("strCodEmpre") & "'" & AndUsina(envioDeDados.CboUsinaText) & ") " &
                                      "AND datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'"
                        Cmd.ExecuteNonQuery()

                        Cmd.CommandText = "SELECT SUM(r.valIR3emp) As valIR3 " &
                                      "FROM usina u, tb_IR3 r " &
                                      "WHERE u.codempre = '" & Session("strCodEmpre") & "' " &
                                      "AND u.codusina = r.codusina " & AndUsina(envioDeDados.CboUsinaText) &
                                      "AND r.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'"
                        Dim rsIR3 As SqlDataReader = Cmd.ExecuteReader
                        If rsIR3.Read Then
                            dblIR3 = IIf(Not IsDBNull(rsIR3("valIR3")), rsIR3("valIR3"), 0)
                        End If
                        rsIR3.Close()
                        rsIR3 = Nothing
                    Else
                        strOpcao &= "0"
                    End If

                    If envioDeDados.ChkIr4E Then
                        'Insumo Reserva 4
                        strOpcao &= "1"
                        Cmd.CommandText = "UPDATE tb_IR4 " &
                                      "SET valIR4emp = valIR4tran " &
                                      "WHERE codusina IN " &
                                      "(SELECT codusina " &
                                      "FROM usina u " &
                                      "WHERE codempre = '" & Session("strCodEmpre") & "'" & AndUsina(envioDeDados.CboUsinaText) & ") " &
                                      "AND datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'"
                        Cmd.ExecuteNonQuery()

                        Cmd.CommandText = "SELECT SUM(r.valIR4emp) As valIR4 " &
                                      "FROM usina u, tb_IR4 r " &
                                      "WHERE u.codempre = '" & Session("strCodEmpre") & "' " &
                                      "AND u.codusina = r.codusina " & AndUsina(envioDeDados.CboUsinaText) &
                                      "AND r.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'"
                        Dim rsIR4 As SqlDataReader = Cmd.ExecuteReader
                        If rsIR4.Read Then
                            dblIR4 = IIf(Not IsDBNull(rsIR4("valIR4")), rsIR4("valIR4"), 0)
                        End If
                        rsIR4.Close()
                        rsIR4 = Nothing
                    Else
                        strOpcao &= "0"
                    End If

                    If envioDeDados.ChkCVU Then
                        'Insumo CVU
                        strOpcao &= "1"
                        '------------------------------------------------------------------------------------

                        Cmd.CommandText = ("select round(rs.val_cvuprog,2)as totCvu from tb_respdemanda r
                                            join tb_respdemandasemanal rs on rs.id_respdemandasemanal = r.id_respdemandasemanal where r.datpdp = '" & Format(Session("datEscolhida"), "yyyyMMdd") & "'
                                            and exists (select 1
		                                            from usina u
        	                                            where u.codusina = r.cod_usinaprog
                                                           " & AndUsina(envioDeDados.CboUsinaText) & "
        	                                and u.codempre = '" & Session("strCodEmpre") & "');")

                        Dim rsCvu As SqlDataReader = Cmd.ExecuteReader

                        If rsCvu.Read Then
                            'total preco
                            dblCVU = IIf(Not IsDBNull(rsCvu("totCvu")), rsCvu("totCvu"), 0)
                            RegistrarLog("Informações de Programação Semanal - CVU: " + dblCVU.ToString())

                        End If

                        rsCvu.Close()
                        rsCvu = Nothing
                    Else
                        strOpcao &= "0"
                    End If

                    strNomeArquivo = Trim(Session("strCodEmpre")) & "-ON " & Mid(dtPDP, 1, 8) & "-" & IIf(envioDeDados.ChkCVU, DateTime.Now.ToString("HHmmss"), Mid(dtPDP, 9)) & " WEB"

                    RegistrarLog("Nome do Arquivo: " + strNomeArquivo)

                    Cmd.CommandText = "INSERT INTO Mensa (" &
                                      "datpdp, " &
                                      "dthmensa, " &
                                      "codempre, " &
                                      "nomarq, " &
                                      "indrecenv, " &
                                      "sitmensa, " &
                                      "totdespa, " &
                                      "totcarga, " &
                                      "totinter, " &
                                      "totvazao, " &
                                      "qtdrestr, " &
                                      "qtdparal, " &
                                      "totinflexi, " &
                                      "totrazener, " &
                                      "totrazelet, " &
                                      "totexporta, " &
                                      "totimporta," &
                                      "totmre, " &
                                      "totpcc, " &
                                      "totmco, " &
                                      "totmos, " &
                                      "totmeg, " &
                                      "toterp, " &
                                      "totdsp, " &
                                      "totclf, " &
                                      "totmif, " &
                                      "qtdparalco, " &
                                      "totcota, " &
                                      "totrfc, " &
                                      "totrmp, " &
                                      "totgfm, " &
                                      "totcfm, " &
                                      "totsom, " &
                                      "totges, " &
                                      "totgec, " &
                                      "totdca, " &
                                      "totdcr, " &
                                      "totir1, " &
                                      "totir2, " &
                                      "totir3, " &
                                      "totir4, " &
                                      "totrro, " &
                                      "totcvu, " &
                                      "usuar_id " &
                                      ") VALUES (" &
                                      "'" & IIf(Format(Session("datEscolhida"), "yyyyMMdd").Equals("yyyyMMdd"), Session("datEscolhida"), Format(Session("datEscolhida"), "yyyyMMdd")) & "', " &'ref
                                      "'" & Format(Now, "yyyyMMddHHmmss") & "', " &'ok
                                      "'" & Trim(Session("strCodEmpre")) & "', " &'ok
                                      "'" & strNomeArquivo & "', " &
                                      "'" & "R" & "', " &
                                      "'" & "Processado" & "', " &
                                      "" & CType((dblGeracao / 2), Integer) & ", " &
                                      "" & CType((dblCarga / 2), Integer) & ", " &
                                      "" & CType((dblIntercambio / 2), Integer) & ", " &
                                      "" & dblVazao & ", " &
                                      "" & dblRestricao & ", " &
                                      "" & dblManutencao & ", " &
                                      "" & CType((dblInflexibilidade / 2), Integer) & ", " &
                                      "" & CType((dblEnergetica / 2), Integer) & ", " &
                                      "" & CType((dblEletrica / 2), Integer) & ", " &
                                      "" & CType((dblExporta / 2), Integer) & ", " &
                                      "" & CType((dblImporta / 2), Integer) & ", " &
                                      "" & dblMRE & ", " &
                                      "" & CType((dblPCC / 2), Integer) & ", " &
                                      "" & dblMCO & ", " &
                                      "" & dblMOS & ", " &
                                      "" & dblMEG & ", " &
                                      "" & CType((dblERP / 2), Integer) & ", " &
                                      "" & CType((dblDSP / 2), Integer) & ", " &
                                      "" & CType((dblCLF / 2), Integer) & ", " &
                                      "" & dblMIF & ", " &
                                      "" & dblPCO & ", " &
                                      "" & Replace(dblCota, ",", ".") & ", " &
                                      "" & CType((dblRFC / 2), Integer) & ", " &
                                      "" & dblRMP & ", " &
                                      "" & dblGFM & ", " &
                                      "" & dblCFM & ", " &
                                      "" & dblSOM & ", " &
                                      "" & dblGES & ", " &
                                      "" & dblGEC & ", " &
                                      "" & dblDCA & ", " &
                                      "" & dblDCR & ", " &
                                      "" & dblIR1 & ", " &
                                      "" & dblIR2 & ", " &
                                      "" & dblIR3 & ", " &
                                      "" & dblIR4 & ", " &
                                      "" & CType((dblRRO / 2), Integer) & ", " &
                                      "" & CType((dblCVU), Integer) & ", " &
                                      "'" & UsuarID & "')"

                    RegistrarLog(Cmd.CommandText)

                    Cmd.ExecuteNonQuery()

                    'Verifica se a Carga, Geração e Intercâmbio foram gravados
                    RegistrarLog("Verifica se a Carga, Geração e Intercâmbio foram gravados. Data Escolhida: " + Format(Session("datEscolhida"), "yyyyMMdd"))

                    If VerificaEventos(Format(Session("datEscolhida"), "yyyyMMdd"), envioDeDados.CboEmpresaText) Then
                        RegistrarLog("Evento verificado.")

                        GravaEventoPDP("11", Format(Session("datEscolhida"), "yyyyMMdd"), envioDeDados.CboEmpresaText, DateAdd(DateInterval.Second, 1, Now), "PDPEnvDado", UsuarID)
                        RegistrarLog("Evento PDP Gravado.")
                    End If

                    'Grava arquivo para a empresa selecionada na data selecionada
                    ' Conforme definição da Marta, será necessário gerar
                    ' o arquivo para dados da área de transferência também,
                    ' foi por isso que isolei o if abaixo e estou passando o valor
                    ' da opção selecionada para a rotina gravaarquivotexto

                    strData = IIf(envioDeDados.CboDataValue = "", dtPDP, IIf(cmbDtValue <> "", Mid(envioDeDados.CboDataValue, 7, 4) & Mid(envioDeDados.CboDataValue, 4, 2) & Mid(envioDeDados.CboDataValue, 1, 2), dtPDP))

                    RegistrarLog("Gravando Arquivo Texto: strCodEmpresa: " + envioDeDados.CboEmpresaValue + ", strData: " + strData + ", strArq: " + strArq + ", blnTipoEnvio: 1, bFtp: True, strOpcao: " + strOpcao + ", strDataHora: " + dtPDP + ", strArqDestino:'' ")

                    Dim dadosarquivo As New GeracaoArquivo
                    dadosarquivo.strCodEmpresa = envioDeDados.CboEmpresaValue
                    dadosarquivo.strData = strData
                    dadosarquivo.strArq = strArq
                    dadosarquivo.blnTipoEnvio = "1"
                    dadosarquivo.bFtp = True
                    dadosarquivo.bRetorno = bRetorno
                    dadosarquivo.strOpcao = strOpcao
                    dadosarquivo.strDataHora = dtPDP
                    dadosarquivo.strArqDestino = ""
                    dadosarquivo.Cmd = Cmd
                    dadosarquivo.strPerfil = PerfilID
                    dadosarquivo.codUsina = envioDeDados.CboUsinaText

                    If Not GravaArquivoTexto(dadosarquivo, bRetorno) Then
                        'Session("strMensagem") = "Não foi possível gravar o arquivo texto, os dados podem estar incorretos. "

                        Session("strMensagem") = "Não foi possivel enviar os dados (7), por favor tente novamente ou comunique a ocorrência ao ONS."
                        RegistrarLog(Session("strMensagem"))
                        System.Web.HttpContext.Current.Response.Redirect("frmMensagem.aspx")
                    Else
                        ' Conforme definição da Marta, após enviar o arquivo para área de FTP
                        ' apagá-lo do diretório Download

                        If bRetorno = True Then
                            System.Web.HttpContext.Current.Response.Write("<script language=JavaScript>")
                            System.Web.HttpContext.Current.Response.Write("window.open('frmRecibo.aspx?strNomeArquivo=" & Trim(strNomeArquivo) & "' , '', 'width=515,height=610,status=no,scrollbars=no,titlebar=yes,menubar=no')")
                            System.Web.HttpContext.Current.Response.Write("</script>")
                        Else
                            'Session("strMensagem") = "Não foi possível gravar o arquivo na área de FTP!"
                            Session("strMensagem") = "Não foi possivel enviar os dados (8), por favor tente novamente ou comunique a ocorrência ao ONS."
                            RegistrarLog(Session("strMensagem"))
                            System.Web.HttpContext.Current.Response.Redirect("frmMensagem.aspx")
                        End If
                    End If

                    RegistrarLog("Realizando commit.")

                    'Cadastra o Marco ref. ao Envio de Dados
                    InsereMarcoPDP(K_Const_strIdMarco_EnvioDados, Format(Session("datEscolhida"), "yyyyMMdd"), UsuarID, UsuarID, Cmd)

                    'Grava Transação
                    objTrans.Commit()
                    RegistrarLog("Commit realizado.")

                    RegistrarLog("Realizando Dispose.")
                    Cmd.Dispose()
                    CmdIns.Dispose()
                    Conn.Close()
                    Conn.Dispose()
                    ConnIns.Close()
                    ConnIns.Dispose()
                    RegistrarLog("Dispose realizado.")

                    'PreencheCabecalho() preenche a tabela da tela de envio de dados
                    'PreencheTable() preenche a tabela da tela de envio de dados

                    retorno = True

                Catch ex As Exception
                    RegistrarLogErro(ex)
                    Try
                        'se a transação estiver fechada vai ocorrer erro
                        objTrans.Rollback()
                    Catch
                    End Try
                    Cmd.Dispose()
                    CmdIns.Dispose()
                    If Conn.State = ConnectionState.Open Then
                        Conn.Close()
                        Conn.Dispose()
                    End If
                    If ConnIns.State = ConnectionState.Open Then
                        ConnIns.Close()
                        ConnIns.Dispose()
                    End If

                    retorno = False

                    'Session("strMensagem") = "Não foi possivel enviar os dados." & ex.Message & Cmd.CommandText
                    Session("strMensagem") = "Não foi possivel enviar os dados (9), por favor tente novamente ou comunique a ocorrência ao ONS."
                    RegistrarLog(Session("strMensagem"))
                    System.Web.HttpContext.Current.Response.Redirect("frmMensagem.aspx")

                Finally
                    If Conn.State = ConnectionState.Open Then
                        Conn.Close()
                        Conn.Dispose()
                    End If
                    If ConnIns.State = ConnectionState.Open Then
                        ConnIns.Close()
                        ConnIns.Dispose()
                    End If
                End Try
            Else
                Session("strMensagem") = "Usuário não tem permissão para Transferir os valores."
                RegistrarLog(Session("strMensagem"))
                System.Web.HttpContext.Current.Response.Redirect("frmMensagem.aspx")
            End If
        Else
            Session("strMensagem") = "Usuário não tem Verificando se Geração permissão para Transferir os valores."
            RegistrarLog(Session("strMensagem"))
            System.Web.HttpContext.Current.Response.Redirect("frmMensagem.aspx")
        End If

        Return retorno

    End Function
    Private Sub IniciaLog()
        'log4net.Config.XmlConfigurator.Configure()
    End Sub
    Public Sub RegistrarLog(ByVal mensagem As String)
        'log4net.Config.XmlConfigurator.Configure()
        logger.Debug(mensagem)
    End Sub

    Public Sub RegistrarLogErro(ByVal ex As Exception)
        'log4net.Config.XmlConfigurator.Configure()
        Me.RegistrarLogErro(ex.Message, ex)
    End Sub

    Public Sub RegistrarLogErro(msg As String, ByVal ex As Exception)
        'log4net.Config.XmlConfigurator.Configure()
        logger.Error(msg, ex)
    End Sub

    Public Function VerificaFeriado(data As String) As Boolean
        Dim Conn As OnsClasses.OnsData.OnsConnection = New OnsClasses.OnsData.OnsConnection
        Dim Cmd As OnsClasses.OnsData.OnsCommand = New OnsClasses.OnsData.OnsCommand
        Cmd.Connection = Conn
        Conn.Open("pdp")

        Dim sql As StringBuilder = New StringBuilder()
        Dim feriado As Boolean = False

        sql.Append(" select diaferia from feria where diaferia = " & data)

        Cmd.CommandText = sql.ToString()

        Dim drFeriado As OnsClasses.OnsData.OnsDataReader = Cmd.ExecuteReader

        Do While drFeriado.Read()
            feriado = True
        Loop
    End Function

End Class
