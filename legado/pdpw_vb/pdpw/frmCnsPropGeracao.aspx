<%@ Page Language="vb" AutoEventWireup="false" Codebehind="frmCnsPropGeracao.aspx.vb" Inherits="pdpw.frmCnsPropGeracao"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>ONS - Operador Nacional do Sistema Elétrico</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<meta http-equiv="Content-Type" content="text/html; charset=windows-1252">
		<link href="../pdpw/images/style.css" rel="stylesheet">
	</HEAD>
	<body bgColor="#ffffff" leftMargin="0" topMargin="0">
		<table style="WIDTH: 788px; HEIGHT: 232px" height="232" cellSpacing="0" cellPadding="0"
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
														<td style="HEIGHT: 12px"><img height="25" src="../pdpw/images/tit_sis_guideline.gif" width="179"></td>
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
															<div align="left"><img height="23" src="../pdpw/images/tit_PropGeracao.gif" width="176" style="WIDTH: 176px; HEIGHT: 23px"></div>
														</td>
													</tr>
												</tbody>
											</table>
										</td>
									</tr>
								</tbody>
							</table>
							<br>
							<form id="frmCnsPropGeracao" name="frmCnsPropGeracao" runat="server">
								<div id="divtabela" style="LEFT: 80px; WIDTH: 617px; POSITION: absolute; TOP: 120px; HEIGHT: 48px">
									<table class="modulo" style="WIDTH: 616px; HEIGHT: 144px" cellSpacing="0" cellPadding="0"
										border="0">
										<tr vAlign="top">
											<TD style="WIDTH: 42px; HEIGHT: 1px"><%if request.querystring("strAcesso") <> "PDOC" then 
																						response.write("Dados:") 
																				end if%></TD>
											<td style="WIDTH: 306px; HEIGHT: 1px">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
												Selecionar:</td>
											<td style="HEIGHT: 1px"></td>
											<td style="WIDTH: 81px"></td>
										</tr>
										<tr>
											<TD style="WIDTH: 42px; HEIGHT: 76px" valign="top">
												<asp:radiobuttonlist id="optDados" runat="server" Font-Size="XX-Small" Width="118px">
													<asp:ListItem Value="2" Selected="True">Consolidados</asp:ListItem>
												</asp:radiobuttonlist></TD>
											<td style="WIDTH: 306px; HEIGHT: 76px">
												<table class="formulario_texto" style="WIDTH: 311px; HEIGHT: 58px" cellSpacing="0" cellPadding="0"
													border="0">
													<tr>
														<td style="WIDTH: 93px; HEIGHT: 27px" align="right">Data do PDP&nbsp;</td>
														<td style="WIDTH: 213px; HEIGHT: 27px"><asp:dropdownlist id="cboDataInicial" runat="server" Width="90px" AutoPostBack="True"></asp:dropdownlist></td>
													</tr>
													<tr>
														<td style="WIDTH: 93px" align="right">Empresa&nbsp;</td>
														<td style="WIDTH: 213px"><asp:dropdownlist id="cboEmpresa" runat="server" Width="199px" AutoPostBack="True"></asp:dropdownlist></td>
													</tr>
												</table>
												<P>&nbsp;</P>
											</td>
											<td align="center">
												<asp:ImageButton id="btnVisualizar" runat="server" ImageUrl="images/bt_visualizar.gif"></asp:ImageButton></td>
											<td align="center" style="WIDTH: 81px"><IMG id="imgPlanilha" onmouseover="this.style.cursor='hand'" onclick="javascript:botao();"
													alt="" src="images/bt_planilha.gif" runat="server"></td>
										</tr>
										<tr>
											<td colspan="4" align="center" style="WIDTH: 607px">
												<asp:Label id="lblMensagem" runat="server" Visible="False"></asp:Label>
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
		<DIV style="DISPLAY: inline; LEFT: 20px; WIDTH: 768px; POSITION: absolute; TOP: 248px; HEIGHT: 24px"
			align="left" ms_positioning="FlowLayout"><br>
			<asp:table id="tblConsulta" runat="server" Font-Size="X-Small" Width="70px" CellSpacing="0"></asp:table></DIV>
	</body>
</HTML>
