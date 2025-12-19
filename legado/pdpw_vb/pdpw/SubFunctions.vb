Option Strict On
Option Explicit On 

'----------------------------------------------------------------------------------------------------------------
'--
'-- Nome............: SubFunctions Class
'-- Versao..........: 01
'--
'-- Produção........: Probank Software e Consultoria
'-- Cliente.........: ONS - Sistema PDPW
'-- Autor...........: Marcos Alves - Meganume Consultoria (Copyright © 2010)
'-- Data............: 01/10/2010
'-- Objetivo........: Classe genérica de SubRotinas e Funções.
'--            
'-- Escrito em .....: VB.NET
'--
'----------------------------------------------------------------------------------------------------------------

Imports System

Module SubFunctions

    Public Function ObterSubString(ByVal pStringCompleta As String, ByVal pSeparador As String) As String

        Dim sSubString As String
        Dim iPosString As Integer

        iPosString = InStr(pStringCompleta, pSeparador) - 1
        sSubString = Mid(pStringCompleta, 1, iPosString)

        Return sSubString

    End Function

End Module
