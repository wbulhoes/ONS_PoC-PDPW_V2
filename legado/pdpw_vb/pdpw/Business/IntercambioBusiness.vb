Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports pdpw

Public Class IntercambioBusiness
    Inherits BaseBusiness
    Implements IIntercambioBusiness

    Public Function AtualizaIntercambio(dataPDP As String) As String Implements IIntercambioBusiness.AtualizaIntercambio
        Dim sql As New StringBuilder
        Try
            Dim listaEmpresas As List(Of OfertaExportacaoDTO) =
                Me.FactoryDao.OfertaExportacaoDAO.Listar(dataPDP)

            Dim listaCodEmpre As List(Of String) = listaEmpresas.Select(Function(o) o.CodUsina.Substring(0, 2)).ToList()
            listaCodEmpre.AddRange(listaEmpresas.Select(Function(o) o.CodUsiConversora.Substring(0, 2)).ToList())

            listaCodEmpre = listaCodEmpre.Distinct().ToList()


            sql.AppendLine("merge into inter as t
                        using (")

            Dim _union As String = ""

            For Each codEmpre As String In listaCodEmpre
                sql.AppendLine(_union + Me.AtualizaIntercambio(dataPDP, codEmpre))
                _union = " UNION ALL "
            Next

            sql.AppendLine($")  as s 
                         on t.datpdp = s.DatPDP
                         and t.CodEmpreDe = s.CodEmpreDe
                         and t.CodEmprepara = s.CodEmprePara
                         and t.CodContaDe = s.CodContaDe
                         and t.CodContaPara = s.CodContaPara
                         and t.CodContaModal = s.CodContaModal
                         and t.IntInter = s.Patamar
                         when matched then
	                        update set t.ValInterSUP = s.ValorSUP; ")

            'Me.FactoryDao.InterDAO.Salvar()
            'Me.FactoryDao.InterDAO.ExecutarSQL(sql.ToString())

        Catch ex As Exception
            Throw TratarErro(ex)
        End Try
        Return sql.ToString()
    End Function

    Private Function AtualizaIntercambio(dataPDP As String, codEmpre As String) As String

        Dim sucesso As Boolean = False
        Dim sql As New StringBuilder

        Dim listaCarga As List(Of CargaDTO) =
            Me.FactoryDao.CargaDAO.
            Listar(dataPDP).
            Where(Function(carga) carga.CodEmpre = codEmpre).
            ToList()

        Dim listaDespa As List(Of DespaDTO) =
            Me.FactoryDao.DespaDAO.
            Listar(dataPDP).
            Where(Function(geracao) geracao.CodUsina.Substring(0, 2) = codEmpre).
            ToList()

        Dim listaIntercambio As List(Of InterDTO) =
            Me.FactoryDao.InterDAO.
            Listar(dataPDP).
            Where(Function(intercambio) intercambio.CodEmpreDe = codEmpre Or
                                        intercambio.CodEmprePara = codEmpre).
            ToList()

        Dim _union As String = ""

        For i As Integer = 1 To 48
            Dim NumPatamar As Integer = i

            'Buscar o total da Carga pela empresa naquela data PDP naquele Patamar
            Dim ValorCarga As Integer =
                listaCarga.Where(Function(c) c.Patamar = NumPatamar).Select(Function(c) c.ValCargaSup).Sum()

            'Buscar o total da Geração pela empresa naquela data PDP naquele Patamar
            Dim ValorGeracao As Integer =
                listaDespa.Where(Function(d) d.Patamar = NumPatamar).Select(Function(d) d.ValDespaSup).Sum()

            'Calcular o Valor DE do Intercambio
            Dim valorDe As Integer = ValorGeracao - ValorCarga

            'Calcular o Valor Para do Intercambio
            Dim valorPara As Integer = ValorCarga - ValorGeracao

            Dim intercambios As List(Of InterDTO) =
                listaIntercambio.Where(Function(inter) inter.Patamar = NumPatamar).ToList()

            For Each inter As InterDTO In intercambios
                If inter.CodEmpreDe = codEmpre Then
                    inter.ValInterSup = valorDe
                End If

                If inter.CodEmprePara = codEmpre Then
                    inter.ValInterSup = valorPara
                End If

                'Me.FactoryDao.InterDAO.Atualizar(inter)
                sql.AppendLine($"{_union} 
                                Select '{inter.DataPDP}' as DatPDP, 
                                '{inter.CodEmpreDe}' as CodEmpreDe, 
                                '{inter.CodEmprePara}' as CodEmprePara, 
                                '{inter.CodContaDe}' as CodContaDe, 
                                '{inter.CodContaPara}' as CodContaPara, 
                                '{inter.CodContaModal}' as CodContaModal, 
                                '{inter.Patamar}' as Patamar, 
                                '{inter.ValInterSup}' as ValorSUP  
                                From SYSMASTER:SYSDUAL")

                _union = " UNION ALL "
            Next
        Next



        Return sql.ToString()
    End Function

    Public Function AtualizaIntercambioTodasOfertas(dataPDP As String) As String Implements IIntercambioBusiness.AtualizaIntercambioTodasOfertas
        Dim sql As New StringBuilder
        Try
            Dim listaEmpresas As List(Of OfertaExportacaoDTO) =
                Me.FactoryDao.OfertaExportacaoDAO.Listar(dataPDP)

            Dim listaCodEmpre As List(Of String) = listaEmpresas.Select(Function(o) o.CodUsina.Substring(0, 2)).Distinct().ToList()
            listaCodEmpre.AddRange(listaEmpresas.Select(Function(o) o.CodUsiConversora.Substring(0, 2)).Distinct().ToList())

            If (listaCodEmpre.Count() > 0) Then
                Dim strCodEmpre As String = ""
                For Each codEmpre As String In listaCodEmpre
                    If strCodEmpre.Equals("") Then
                        strCodEmpre += "'" + codEmpre + "'"
                    Else
                        strCodEmpre += ",'" + codEmpre + "'"
                    End If
                Next

                sql.AppendLine($"MERGE INTO inter AS t
                                USING (
                                    SELECT
                                        inter.datpdp
                                        ,inter.intinter
                                        ,inter.codemprede
                                        ,inter.codemprepara
                                        ,(ISNULL(geracao.geracaoPRE, 0) - ISNULL(carga.cargaPRE, 0))  AS VALINTERPRE
                                        ,(ISNULL(geracao.geracaoSUP, 0) - ISNULL(carga.cargaSUP, 0))  AS VALINTERSUP
                                        FROM INTER
                                        left join(
                                           select ISNULL(sum(valcargaPRE),0) as cargaPRE
                                           ,ISNULL(sum(valcargaSUP),0) as cargaSUP
                                           , intcarga
                                           ,codempre
                                           from carga
                                           where datpdp = '{ dataPDP }'
                                           group by intcarga
                                           ,codempre
                                        ) carga
                                        on carga.intcarga = inter.intinter
                                        and carga.codempre = inter.codemprede
                                        left join(
                                           select ISNULL(sum(valdespaPRE),0) as geracaoPRE
                                           ,ISNULL(sum(valdespaSUP),0) as geracaoSUP
                                           , intdespa
                                           ,codempre
                                           from usina
                                           inner join despa
                                              on usina.codusina = despa.codusina
                                           where despa.datpdp = '{ dataPDP }'
                                           group by intdespa, codempre
                                        ) geracao
                                        on geracao.intdespa = inter.intinter
                                        and geracao.codempre = inter.codemprede
                                        WHERE DATPDP = '{ dataPDP }' and 
                    	                      codemprede in ({ strCodEmpre }) and
                    	                      codemprepara in ('NE','RE','RN','RS')
                                ) as S
                                    ON 
                                        T.datpdp         = s.datpdp and 
                                        T.codemprede     = S.codemprede and 
                                        T.codemprepara   = S.codemprepara and
                                        T.intinter       = S.intinter
                                    WHEN MATCHED THEN UPDATE
                                         SET T.ValInterSUP = S.VALINTERSUP,
                                             T.ValInterPRE = S.VALINTERPRE; ")

                sql.AppendLine($"MERGE INTO inter AS t
                                USING (
                                    SELECT
                                        inter.datpdp
                                        ,inter.intinter
                                        ,inter.codemprede
                                        ,inter.codemprepara
                                        ,(inter.valinterpre * -1) AS VALINTERPRE
                                        ,(inter.valintersup * -1) AS VALINTERSUP
                                    FROM INTER
                                    WHERE DATPDP = '{ dataPDP }' and 
                                          codemprede in ({ strCodEmpre }) and
                    	  	              codemprepara in ('NE','RE','RN','RS')
                                ) as S
                                    ON 
                                        T.datpdp         = S.datpdp and 
                                        T.codemprede     = S.codemprepara and 
                                        T.codemprepara   = S.codemprede and
                                        T.intinter       = S.intinter
                                    WHEN MATCHED THEN UPDATE
                                         SET T.ValInterSUP = S.VALINTERSUP,
                                             T.ValInterPRE = S.VALINTERPRE; ")

                'Me.FactoryDao.InterDAO.Salvar()
                'Me.FactoryDao.InterDAO.ExecutarSQL(sql.ToString())
            End If

        Catch ex As Exception
            Throw TratarErro(ex)
        End Try
        Return sql.ToString()
    End Function

End Class
