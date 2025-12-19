<%@ Page Language="vb" AutoEventWireup="false" Codebehind="frmColMaqOperando.aspx.vb" Inherits="pdpw.frmColMaqOperando"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>ONS - Operador Nacional do Sistema Elétrico</title>
		<META http-equiv="Content-Type" content="text/html; charset=windows-1252">
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../pdpw/images/style.css" rel="stylesheet">
		<script language="JavaScript">
			function RetiraEnter(teclapres) {
				var tecla = teclapres.keyCode;
				if( tecla == 13 ){
					// Retira Caracteres especiais (ENTER, TAB, etc...) quando existirem 2 seguidos em qualquer lugar do texto
					vr = escape(document.frmColMaqOperando.txtValor.value);
					//TAB + ENTER
					vr = vr.replace('%09%0D%0A','%09')
					//ENTER + TAB
					vr = vr.replace('%0D%0A%09','%09')
					//ENTER + ENTER
					vr = unescape(vr.replace('%0D%0A%0D%0A','%0D%0A'));
					document.frmColMaqOperando.txtValor.value = vr;
				}
			}
		</script>
	</HEAD>
	<body bottomMargin="0" bgColor="#ffffff" leftMargin="0" topMargin="0" rightMargin="0">
		<P>
			<table style="WIDTH: 1100px; HEIGHT: 266px" height="266" cellSpacing="0" cellPadding="0"
				width="1100" border="0">
				<tbody>
					<tr>
						<td style="WIDTH: 28px; HEIGHT: 250px" vAlign="top" width="28"><BR>
						</td>
						<td style="WIDTH: 1064px; HEIGHT: 250px" vAlign="top">
							<div align="center">
								<table cellSpacing="0" cellPadding="0" width="100%" border="0">
									<tbody>
										<TR>
											<TD width="20%" height="2">&nbsp;</TD>
										</TR>
										<TR>
											<TD height="2">
												<TABLE cellSpacing="0" cellPadding="0" width="765" background="../pdpw/images/back_tit_sistema.gif"
													border="0">
													<TBODY>
														<TR>
															<TD><IMG height="25" src="../pdpw/images/tit_sis_guideline.gif" width="179"></TD>
														</TR>
													</TBODY>
												</TABLE>
											</TD>
										</TR>
										<TR>
											<TD height="2">
												<TABLE cellSpacing="0" cellPadding="0" width="765" background="../pdpw/images/back_titulo.gif"
													border="0">
													<TBODY>
														<TR>
															<TD>
																<DIV align="left"><IMG style="WIDTH: 368px; HEIGHT: 23px" height="23" src="../pdpw/images/tit_ColMaqOperando.gif"
																		width="368"></DIV>
															</TD>
														</TR>
													</TBODY>
												</TABLE>
											</TD>
										</TR>
									</tbody>
								</table>
								<BR>
								<form id="frmColMaqOperando" name="frmColMaqOperando" runat="server">
									<table style="WIDTH: 1078px; HEIGHT: 124px" height="124" cellSpacing="0" cellPadding="0"
										width="1078" border="0">
										<tr height="30">
											<td style="WIDTH: 170px; HEIGHT: 22px" align="right"><b>Data PDP:</b></td>
											<td style="WIDTH: 257px; HEIGHT: 22px">&nbsp;<asp:dropdownlist id="cboData" runat="server" Width="98px" AutoPostBack="True"></asp:dropdownlist></td>
										</tr>
										<tr height="30">
											<td style="WIDTH: 170px; HEIGHT: 39px" align="right"><b>Empresa:</b></td>
											<td style="WIDTH: 750px; HEIGHT: 39px">
												<P>&nbsp;<asp:dropdownlist id="cboEmpresa" runat="server" Width="219px" AutoPostBack="True"></asp:dropdownlist>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</P>
											</td>
										</tr>
										<tr height="30">
											<td style="WIDTH: 170px; HEIGHT: 28px" align="right"><b>Usinas:</b></td>
											<td style="WIDTH: 881px; HEIGHT: 28px">
												<P>&nbsp;<asp:dropdownlist id="cboUsina" runat="server" Width="219px" AutoPostBack="True"></asp:dropdownlist>&nbsp;&nbsp;&nbsp;
													<asp:ImageButton id="btnSalvar" runat="server" ImageUrl="images/bt_salvar.gif"></asp:ImageButton>
												</P>
											</td>
										</tr>
										<tr>
											<td style="WIDTH: 10px" vAlign="top" align="right">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
											</td>
											<td style="WIDTH: 750px" vAlign="top">
												<DIV id="divValor" style="DISPLAY: inline; Z-INDEX: 200; LEFT: 180px; WIDTH: 82px; POSITION: absolute; TOP: 270px; HEIGHT: 21px"
													runat="server" ms_positioning="FlowLayout"></DIV>
											</td>
										</tr>
										<tr>
											<td colSpan="2">
												<DIV style="DISPLAY: inline; Z-INDEX: 1; LEFT: 20px; WIDTH: 141px; POSITION: absolute; TOP: 230px; HEIGHT: 45px"
													ms_positioning="FlowLayout">
													<asp:table id="tblMaqOperando" runat="server" Width="47px" BorderStyle="Ridge" BorderWidth="1px"
														CellPadding="2" CellSpacing="0" GridLines="Both" Height="26px" Font-Size="Smaller"></asp:table></DIV>
											</td>
										</tr>
									</table>
								</form>
							</div>
						</td>
					</tr>
				</tbody>
			</table>
		<P></P>
	</body>
</HTML>
