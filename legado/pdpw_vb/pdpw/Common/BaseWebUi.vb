Imports System
Imports System.Web
Imports pdpw

Public Class BaseWebUi
    Inherits System.Web.UI.Page

    Private _factoryBusiness As FactoryBusiness

    Protected ReadOnly Property FactoryBusiness As FactoryBusiness
        Get
            If IsNothing(_factoryBusiness) Then
                _factoryBusiness = New FactoryBusiness()
            End If

            Return _factoryBusiness
        End Get
    End Property

    Protected Overrides Sub OnInit(ByVal e As EventArgs)

        'se o div de Aguarde ainda estiver mostrando ele tira
        Dim src As ScriptManager = ScriptManager.GetCurrent(Page)

        If src IsNot Nothing Then
            ScriptManager.RegisterClientScriptBlock(
                Me,
                GetType(Page),
                "TiraDivAguarde",
                "if(document.getElementById('divProcessando')) document.getElementById('divProcessando').style.display = 'none';",
                True)


            ScriptManager.RegisterOnSubmitStatement(
                Me,
            GetType(Page),
            "zerarfiltro",
            "if(document.getElementById('divProcessando') && document.getElementById('divProcessando').style.display!='none')return false;")

            ScriptManager.RegisterOnSubmitStatement(
                Me,
            GetType(Page),
            "Aguarde",
            "if (typeof(ValidatorOnSubmit) == 'function' && ValidatorOnSubmit() == false) return false; avisoAguarde();")
        Else
            ClientScript.RegisterStartupScript(
                GetType(Page),
                "TiraDivAguarde",
                "if(document.getElementById('divProcessando')) document.getElementById('divProcessando').style.display = 'none';",
                True)

            ClientScript.RegisterOnSubmitStatement(
            GetType(Page),
            "zerarfiltro",
            "if(document.getElementById('divProcessando') && document.getElementById('divProcessando').style.display!='none')return false;")

            ClientScript.RegisterOnSubmitStatement(
            GetType(Page),
            "Aguarde",
            "if (typeof(ValidatorOnSubmit) == 'function' && ValidatorOnSubmit() == false) return false; avisoAguarde();")
        End If


        MyBase.OnInit(e)
    End Sub

End Class