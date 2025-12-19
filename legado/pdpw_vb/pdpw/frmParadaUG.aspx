<%@ Page Language="vb" AutoEventWireup="false" Codebehind="frmParadaUG.aspx.vb" Inherits="pdpw.frmParadaUG"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>ONS - Operador Nacional do Sistema Elétrico</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<meta http-equiv="Content-Type" content="text/html; charset=windows-1252">
		<LINK href="images/style.css" rel="stylesheet">
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<TABLE style="WIDTH: 788px; HEIGHT: 418px" height="418" cellSpacing="0" cellPadding="0"
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
														<TD style="HEIGHT: 12px"><IMG height="25" src="../pdpw/images/tit_sis_guideline.gif" width="179"></TD>
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
															<DIV align="left"><IMG height="23" src="images/tit_ColParadaUG.gif" width="416" style="WIDTH: 416px; HEIGHT: 23px"></DIV>
														</TD>
													</TR>
												</TBODY>
											</TABLE>
										</TD>
									</TR>
								</TBODY>
							</TABLE>
							<BR>
							<form id="frmParadaUG" name="frmParadaUG" runat="server">
								<table style="WIDTH: 505px; HEIGHT: 256px" height="256" cellSpacing="0" cellPadding="0"
									width="505" border="0">
									<TR>
										<TD style="WIDTH: 218px; HEIGHT: 30px" align="right"><b>Data PDP:</b></TD>
										<TD style="WIDTH: 345px; HEIGHT: 30px">
											&nbsp;<asp:DropDownList id="cboData" runat="server" Width="96px" AutoPostBack="True"></asp:DropDownList></TD>
									</TR>
									<tr height="42">
										<td style="WIDTH: 218px; HEIGHT: 30px" align="right"><b>Empresa:</b></td>
										<td style="WIDTH: 345px; HEIGHT: 30px">
											<P>&nbsp;<asp:dropdownlist id="cboEmpresa" runat="server" AutoPostBack="True" Width="219px"></asp:dropdownlist>
											</P>
										</td>
									</tr>
									<tr height="42">
										<td style="WIDTH: 218px; HEIGHT: 28px" align="right"><b>Usina:</b></td>
										<td style="WIDTH: 345px; HEIGHT: 28px">
											<P>&nbsp;<asp:dropdownlist id="cboUsina" runat="server" AutoPostBack="True" Width="219px"></asp:dropdownlist>
											</P>
										</td>
									</tr>
									<tr vAlign="top" align="center">
										<td style="WIDTH: 539px; HEIGHT: 46px" colSpan="2"><br>
											<asp:table id="tblGerador" runat="server" Width="472px" Font-Size="X-Small" CellSpacing="0">
												<asp:TableRow BackColor="#99CC00">
													<asp:TableCell Text="C&#243;digo"></asp:TableCell>
													<asp:TableCell Text="Equipamento"></asp:TableCell>
													<asp:TableCell Text="Sigla"></asp:TableCell>
													<asp:TableCell Text="Data In&#237;cio"></asp:TableCell>
													<asp:TableCell Text="Hora In&#237;cio"></asp:TableCell>
												</asp:TableRow>
											</asp:table></td>
									</tr>
									<tr>
										<td style="WIDTH: 539px; HEIGHT: 50px" align="center" colSpan="2">
											<table style="WIDTH: 263px; HEIGHT: 31px">
												<tr>
													<td style="WIDTH: 71px"><asp:imagebutton id="btnIncluir" runat="server" Width="71px" Height="25px" ImageUrl="../pdpw/images/bt_incluir.gif"></asp:imagebutton>
													<td style="WIDTH: 71px"><asp:imagebutton id="btnExcluir" runat="server" Width="71px" Height="25px" ImageUrl="../pdpw/images/bt_excluir.gif"></asp:imagebutton></td>
													<td style="WIDTH: 71px"><asp:imagebutton id="btnAlterar" runat="server" Width="71px" Height="25px" ImageUrl="../pdpw/images/bt_alterar.gif"></asp:imagebutton></td>
												</tr>
											</table>
										</td>
									</tr>
								</table>
							</form>
						</DIV>
					</TD>
				</TR>
			</TBODY>
		</TABLE>
	</body>
</HTML>
