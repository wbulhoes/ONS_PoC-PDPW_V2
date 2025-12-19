<%@ Page Language="vb" AutoEventWireup="false" Codebehind="frmCnsRestricao.aspx.vb" Inherits="pdpw.frmCnsRestricao"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>ONS - Operador Nacional do Sistema Elétrico</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<meta http-equiv="Content-Type" content="text/html; charset=windows-1252">
		<LINK href="../pdpw/images/style.css" rel="stylesheet">
		<script language="javascript" src="Lib.js"></script>
	</HEAD>
	<BODY bgColor="#ffffff" leftMargin="0" topMargin="0">
		<TABLE style="WIDTH: 788px; HEIGHT: 216px" height="216" cellSpacing="0" cellPadding="0"
			width="788" border="0">
			<TBODY>
				<TR>
					<TD style="WIDTH: 19px" vAlign="top" width="19"><BR>
					</TD>
					<TD vAlign="top">
						<DIV align="center">
							<TABLE cellSpacing="0" cellPadding="0" width="100%" border="0">
								<TBODY>
									<TR>
										<TD width="20%" height="2">&nbsp;</TD>
									</TR>
									<TR>
										<TD style="HEIGHT: 17px" height="17">
											<TABLE cellSpacing="0" cellPadding="0" width="765" background="../pdpw/images/back_tit_sistema.gif"
												border="0">
												<TBODY>
													<TR>
														<TD style="HEIGHT: 12px"><script>MontaCabecalho();</script></TD>
													</TR>
												</TBODY>
											</TABLE>
										</TD>
									</TR>
									<TR>
										<TD style="HEIGHT: 10px" height="10">
											<TABLE cellSpacing="0" cellPadding="0" width="765" background="../pdpw/images/back_titulo.gif"
												border="0">
												<TBODY>
													<TR>
														<TD style="HEIGHT: 8px">
															<DIV align="left"><IMG height="23" src="../pdpw/images/tit_CnsRestricao.gif" width="120" style="WIDTH: 120px; HEIGHT: 23px"></DIV>
														</TD>
													</TR>
												</TBODY>
											</TABLE>
										</TD>
									</TR>
								</TBODY>
							</TABLE>
							<BR>
							<form id="frmCnsRestricao" name="frmCnsRestricao" runat="server">
								<div id="divtabela" style="LEFT: 80px; WIDTH: 560px; POSITION: absolute; TOP: 120px; HEIGHT: 121px">
									<table class="modulo" style="WIDTH: 584px; HEIGHT: 102px" cellSpacing="0" cellPadding="0"
										border="0">
										<tr vAlign="top">
											<td style="WIDTH: 125px; HEIGHT: 10px">
												&nbsp;&nbsp;&nbsp;<%if request.querystring("strAcesso") <> "PDOC" then 
																						response.write("Dados:") 
																					end if%></td>
											<td style="WIDTH: 173px; HEIGHT: 10px">&nbsp;&nbsp;&nbsp;Selecionar:</td>
											<td style="WIDTH: 96px; HEIGHT: 10px"></td>
											<td style="WIDTH: 85px"></td>
										</tr>
										<tr>
											<td style="WIDTH: 125px" vAlign="top" align="right"><asp:radiobuttonlist id="optDados" runat="server" Width="118px" Font-Size="XX-Small" AutoPostBack="True">
													<asp:ListItem Value="restrgerademp" Selected="True">&#193;rea Transfer&#234;ncia</asp:ListItem>
													<asp:ListItem Value="temprestrgerad">Enviados</asp:ListItem>
													<asp:ListItem Value="restrgerad">Consolidados</asp:ListItem>
												</asp:radiobuttonlist></td>
											<td style="WIDTH: 173px" vAlign="top">
												<table class="formulario_texto" style="WIDTH: 277px; HEIGHT: 52px" cellSpacing="0" cellPadding="0"
													border="0">
													<tr>
														<td style="WIDTH: 93px; HEIGHT: 27px" align="right">
															<asp:Label style="Z-INDEX: 0" id="lblData" runat="server">Data do PDP</asp:Label>&nbsp;</td>
														<td style="WIDTH: 211px; HEIGHT: 27px"><asp:dropdownlist id="cboDataInicial" runat="server" Width="90px" AutoPostBack="True"></asp:dropdownlist>&nbsp;</td>
													</tr>
													<tr>
														<td style="WIDTH: 93px" align="right">Empresa&nbsp;</td>
														<td style="WIDTH: 211px"><asp:dropdownlist id="cboEmpresa" runat="server" Width="199px" AutoPostBack="True"></asp:dropdownlist></td>
													</tr>
												</table>
											</td>
											<td align="center" style="WIDTH: 96px">
												<asp:ImageButton id="btnVisualizar" runat="server" ImageUrl="images/bt_visualizar.gif"></asp:ImageButton></td>
											<td align="center" style="WIDTH: 85px"><IMG id="imgPlanilha" onmouseover="this.style.cursor='hand'" onclick="javascript:botao();"
													alt="" src="images/bt_planilha.gif" runat="server"></td>
										</tr>
									</table>
								</div>
							</form>
						</DIV>
					</TD>
				</TR>
			</TBODY>
		</TABLE>
		<DIV style="DISPLAY: inline; LEFT: 76px; WIDTH: 602px; POSITION: absolute; TOP: 240px; HEIGHT: 36px"
			align="left" ms_positioning="FlowLayout">
			<asp:table id="tblConsulta" runat="server" Width="537px" Font-Size="X-Small" CellSpacing="0"></asp:table></DIV>
	</BODY>
</HTML>
