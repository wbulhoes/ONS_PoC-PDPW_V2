<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" Codebehind="OldfrmCnsIR4.aspx.vb" Inherits="pdpw.frmCnsIR4"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <title>ONS - Operador Nacional do Sistema Elétrico</title>
		<META http-equiv="Content-Type" content="text/html; charset=windows-1252">
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<LINK href="../pdpw/images/style.css" rel="stylesheet">
		<script language="javascript" src="Lib.js"></script>
        <style type="text/css">
            .auto-style1 {
                left: 76px;
                width: 635px;
                position: absolute;
                top: 310px;
                height: 36px;
            }
        </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
            <table style="WIDTH: 792px; HEIGHT: 249px" height="249" cellSpacing="0" cellPadding="0"
			width="792" border="0">
			<tr>
				<td style="WIDTH: 17px" width="17"><br>
				</td>
				<td style="HEIGHT: 248px" vAlign="top">
					<div align="center">
						<table cellSpacing="0" cellPadding="0" width="100%" border="0">
							<tr>
								<TD width="20%" height="2">&nbsp;</TD>
							</tr>
							<tr>
								<td style="HEIGHT: 17px" height="17">
									<table style="WIDTH: 772px; HEIGHT: 25px" cellSpacing="0" cellPadding="0" width="772" background="../pdpw/images/back_tit_sistema.gif"
										border="0">
										<tr>
											<td style="HEIGHT: 12px"><script>MontaCabecalho();</script></td>
										</tr>
									</table>
								</td>
							</tr>
							<tr>
								<td style="HEIGHT: 10px" height="10">
									<table style="WIDTH: 771px; HEIGHT: 27px" cellSpacing="0" cellPadding="0" width="771"
										border="0">
										<tr>
											<td style="HEIGHT: 8px">
												<div align="left"><p class="titulo">Carga da Ande</p>
												</div>
											</td>
										</tr>
									</table>
								</td>
							</tr>
						</table>
						<BR>
						<form id="frmCnsIR4" name="frmCnsIR4" runat="server">
							<div id="divtabela" style="LEFT: 80px; WIDTH: 599px; POSITION: absolute; TOP: 120px; HEIGHT: 134px">
								<table class="modulo" style="WIDTH: 606px; HEIGHT: 115px" cellSpacing="0" cellPadding="0"
									border="0">
									<tr vAlign="top">
										<td style="WIDTH: 108px; HEIGHT: 7px"><%if Request.QueryString("strAcesso") <> "PDOC" Then
                                                                                      Response.Write("Dados:")
                                                                                  End If%></td>
										<td style="WIDTH: 306px; HEIGHT: 7px">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Selecionar:</td>
										<td style="HEIGHT: 7px"></td>
										<td style="WIDTH: 85px"></td>
									</tr>
									<tr>
										<td style="WIDTH: 108px"><asp:radiobuttonlist id="optDados" runat="server" Width="118px" Font-Size="XX-Small" AutoPostBack="True">
													<asp:ListItem Value="0" Selected="True">&#193;rea Transfer&#234;ncia</asp:ListItem>
													<asp:ListItem Value="1">Enviados</asp:ListItem>
													<asp:ListItem Enabled="false" Value="2">Consolidados</asp:ListItem>
													<asp:ListItem Value="3">Recebidos DESSEM</asp:ListItem>
													<asp:ListItem Value="4">Consistidos DESSEM</asp:ListItem>
											</asp:radiobuttonlist></td>
										<td style="WIDTH: 306px">
											<table class="formulario_texto" style="WIDTH: 311px; HEIGHT: 59px" cellSpacing="0" cellPadding="0"
												border="0">
												<tr>
													<td style="WIDTH: 93px; HEIGHT: 27px" align="right">
														<asp:Label style="Z-INDEX: 0" id="lblData" runat="server">Data do PDP</asp:Label>&nbsp;</td>
													<td style="WIDTH: 209px; HEIGHT: 27px"><asp:dropdownlist id="cboDataInicial" runat="server" Width="90px" AutoPostBack="True"></asp:dropdownlist>&nbsp;</td>
												</tr>
												<tr>
													<td style="WIDTH: 93px" align="right">Empresa&nbsp;</td>
													<td style="WIDTH: 209px"><asp:dropdownlist id="cboEmpresa" runat="server" Width="199px" AutoPostBack="True"></asp:dropdownlist></td>
												</tr>
											</table>
											<br>
											<br>
											<asp:label id="lblMensagem" runat="server" Visible="False"></asp:label></td>
										<td align="center"><asp:imagebutton id="btnVisualizar" runat="server" ImageUrl="images/bt_visualizar.gif"></asp:imagebutton></td>
										<td align="center" style="WIDTH: 85px"><IMG id="imgPlanilha" onmouseover="this.style.cursor='hand'" onclick="javascript:botao();"
												alt="" src="images/bt_planilha.gif" runat="server"></td>
									</tr>
								</table>
							</div>
						</form>
					</div>
				</td>
			</tr>
		</table>
		<div style="DISPLAY: inline; "
			align="left" ms_positioning="FlowLayout" class="auto-style1"><asp:table id="tblConsulta" runat="server" Width="609px" Font-Size="X-Small" CellSpacing="0"></asp:table></div>
</asp:Content>