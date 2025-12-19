<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" Codebehind="frmCnsCarga.aspx.vb" Inherits="pdpw.frmCnsCarga"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
		<title>ONS - Operador Nacional do Sistema Elétrico</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<meta http-equiv="Content-Type" content="text/html; charset=windows-1252">
		<script language="javascript" src="Lib.js"></script>
		<LINK href="../pdpw/images/style.css" rel="stylesheet">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<TABLE style="WIDTH: 788px; HEIGHT: 252px" height="252" cellSpacing="0" cellPadding="0"
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
															<DIV align="left"><IMG style="WIDTH: 80px; HEIGHT: 23px" height="23" src="../pdpw/images/tit_ColCarga.gif"
																	width="80"></DIV>
														</TD>
													</TR>
												</TBODY>
											</TABLE>
										</TD>
									</TR>
								</TBODY>
							</TABLE>
							<form id="frmCnsCarga" name="frmCnsCarga" runat="server">

							<BR>
								<div id="divtabela" style="LEFT: 80px; WIDTH: 616px; POSITION: absolute; TOP: 120px; HEIGHT: 120px">
									<table class="modulo" style="WIDTH: 617px; HEIGHT: 120px" cellSpacing="0" cellPadding="0"
										border="0">
										<tr vAlign="top">
											<TD style="WIDTH: 99px; HEIGHT: 10px"><%if request.querystring("strAcesso") <> "PDOC" then 
																						response.write("Dados:") 
																					end if%></TD>
											<td style="WIDTH: 293px; HEIGHT: 10px">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
												Selecionar:</td>
											<td style="WIDTH: 96px; HEIGHT: 10px"></td>
											<td></td>
										</tr>
										<tr>
											<TD style="WIDTH: 99px"><asp:radiobuttonlist id="optDados" runat="server" Font-Size="XX-Small" Width="118px" AutoPostBack="True">
													<asp:ListItem Value="0" Selected="True">&#193;rea Transfer&#234;ncia</asp:ListItem>
													<asp:ListItem Value="1">Enviados</asp:ListItem>
													<asp:ListItem Enabled="false" Value="2">Consolidados</asp:ListItem>
												</asp:radiobuttonlist></TD>
											<td style="WIDTH: 293px">
												<table class="formulario_texto" style="WIDTH: 311px; HEIGHT: 64px" cellSpacing="0" cellPadding="0"
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
											<td style="WIDTH: 96px">&nbsp;&nbsp;&nbsp;
												<asp:ImageButton id="btnVisualizar" runat="server" ImageUrl="images/bt_visualizar.gif"></asp:ImageButton>
											</td>
											<td align="center">&nbsp; <IMG alt="" src="images/bt_planilha.gif" onclick="javascript:botao();" runat="server"
													onmouseover="this.style.cursor='hand'" id="imgPlanilha">
											</td>
										</tr>
									</table>
								</div>

                                    <asp:Label ID="lblMsg" Visible="False" ForeColor="Green" runat="server" Font-Size="X-Small" Font-Bold="True">Desculpe, este dado não pôde ser carregado no momento.</asp:Label>
                                    
							</form>
						</DIV>
					</TD>
				</TR>
			</TBODY>
		</TABLE>
		<DIV style="DISPLAY: inline; LEFT: 72px; WIDTH: 638px; POSITION: absolute; TOP: 250px; HEIGHT: 40px"
			align="left" ms_positioning="FlowLayout">
			<asp:table id="tblConsulta" runat="server" Width="610px" Font-Size="X-Small" CellSpacing="0"></asp:table></DIV>
</asp:Content>
