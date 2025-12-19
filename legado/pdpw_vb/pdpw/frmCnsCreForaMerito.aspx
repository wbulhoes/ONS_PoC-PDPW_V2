<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" Codebehind="frmCnsCreForaMerito.aspx.vb" Inherits="pdpw.frmCnsCreForaMerito"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
		<title>ONS - Operador Nacional do Sistema Elétrico</title>
		<META http-equiv="Content-Type" content="text/html; charset=windows-1252">
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../pdpw/images/style.css" rel="stylesheet">
		<script language="javascript" src="Lib.js"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<table style="WIDTH: 788px; HEIGHT: 256px" height="256" cellSpacing="0" cellPadding="0"
			width="788" border="0">
			<tbody>
				<tr>
					<td style="WIDTH: 19px" vAlign="top" width="19"><BR>
					</td>
					<td vAlign="top">
						<div align="center">
							<table cellSpacing="0" cellPadding="0" width="100%" border="0">
								<tbody>
									<tr>
										<td width="20%" height="2">&nbsp;</td>
									</tr>
									<tr>
										<td style="HEIGHT: 17px" height="17">
											<table cellSpacing="0" cellPadding="0" width="765" background="../pdpw/images/back_tit_sistema.gif"
												border="0">
												<tbody>
													<tr>
														<td style="HEIGHT: 12px"><script>MontaCabecalho();</script></td>
													</tr>
												</tbody>
											</table>
										</td>
									</tr>
									<tr>
										<td style="HEIGHT: 10px" height="10">
											<table cellSpacing="0" cellPadding="0" width="765" background="../pdpw/images/back_titulo.gif"
												border="0">
												<tbody>
													<tr>
														<td style="HEIGHT: 8px">
															<div align="left"><IMG height="23" src="../pdpw/images/tit_CreForaMerito.gif" width="216" style="WIDTH: 216px; HEIGHT: 23px"></div>
														</td>
													</tr>
												</tbody>
											</table>
										</td>
									</tr>
								</tbody>
							</table>
							<br>
							<form id="frmCreForaMerito" name="frmCreForaMerito" runat="server">
								<div id="divtabela" style="LEFT: 80px; WIDTH: 584px; POSITION: absolute; TOP: 120px; HEIGHT: 146px">
									<table class="modulo" style="WIDTH: 607px; HEIGHT: 102px" cellSpacing="0" cellPadding="0"
										border="0">
										<tr vAlign="top">
											<TD style="WIDTH: 123px; HEIGHT: 11px"><%if request.querystring("strAcesso") <> "PDOC" then 
																						response.write("Dados:") 
																				end if%></TD>
											<td style="WIDTH: 306px; HEIGHT: 11px">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
												Selecionar:</td>
											<td style="HEIGHT: 11px"></td>
											<td style="WIDTH: 87px"></td>
										</tr>
										<tr>
											<TD style="WIDTH: 123px">
												<asp:radiobuttonlist id="optDados" runat="server" Font-Size="XX-Small" Width="118px" AutoPostBack="True">
													<asp:ListItem Value="0" Selected="True">&#193;rea Transfer&#234;ncia</asp:ListItem>
													<asp:ListItem Value="1">Enviados</asp:ListItem>
													<asp:ListItem Enabled="false" Value="2">Consolidados</asp:ListItem>
													<asp:ListItem Value="3">Recebidos DESSEM</asp:ListItem>
													<asp:ListItem Value="4">Consistidos DESSEM</asp:ListItem>
												</asp:radiobuttonlist></TD>
											<td style="WIDTH: 306px">
												<table class="formulario_texto" style="WIDTH: 311px; HEIGHT: 40px" cellSpacing="0" cellPadding="0"
													border="0">
													<tr>
														<td style="WIDTH: 93px; HEIGHT: 27px" align="right">
															<asp:Label style="Z-INDEX: 0" id="lblData" runat="server">Data do PDP</asp:Label>&nbsp;</td>
														<td style="WIDTH: 209px; HEIGHT: 27px"><asp:dropdownlist id="cboDataInicial" runat="server" Width="90px" AutoPostBack="True"></asp:dropdownlist>&nbsp;</td>
													</tr>
													<tr>
														<td style="WIDTH: 93px; HEIGHT: 27px" align="right">Empresa&nbsp;</td>
														<td style="WIDTH: 209px; HEIGHT: 27px"><asp:dropdownlist id="cboEmpresa" runat="server" Width="212px" AutoPostBack="True"></asp:dropdownlist></td>
													</tr>
												</table>
												<P>&nbsp;</P>
											</td>
											<td align="center">
												<asp:ImageButton id="btnVisualizar" runat="server" ImageUrl="images/bt_visualizar.gif"></asp:ImageButton></td>
											<td align="center" style="WIDTH: 87px"><IMG id="imgPlanilha" onmouseover="this.style.cursor='hand'" onclick="javascript:botao();"
													alt="" src="images/bt_planilha.gif" runat="server"></td>
										</tr>
									</table>
									<table style="WIDTH: 368px; HEIGHT: 25px" align="center">
										<tr>
											<td align="center">
												<asp:Label id="lblMensagem" runat="server" Visible="False" Font-Size="X-Small" Font-Bold="True"></asp:Label>
											</td>
										</tr>
									</table>
								</div>
							</form>
						</div>
					</td>
				</tr>
			</tbody>
		</table>
		<DIV style="DISPLAY: inline; LEFT: 68px; WIDTH: 641px; POSITION: absolute; TOP: 260px; HEIGHT: 43px"
			align="left" ms_positioning="FlowLayout">
			<asp:table id="tblConsulta" runat="server" Font-Size="X-Small" Width="607px" CellSpacing="0"
				Height="24px"></asp:table></DIV>
</asp:Content>
