<%@ Page Language="vb" AutoEventWireup="false" Codebehind="frmCnsCadInter.aspx.vb" Inherits="pdpw.frmCnsCadInter"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>frmCnsCadInter</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio.NET 7.0">
		<meta name="CODE_LANGUAGE" content="Visual Basic 7.0">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../pdpw/images/style.css" rel="stylesheet">
	</HEAD>
	<BODY bgColor="#ffffff" leftMargin="0" topMargin="0">
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
															<DIV align="left"><IMG height="23" src="../pdpw/images/tit_CnsInter.gif" width="152" style="WIDTH: 152px; HEIGHT: 23px"></DIV>
														</TD>
													</TR>
												</TBODY>
											</TABLE>
										</TD>
									</TR>
								</TBODY>
							</TABLE>
							<form id="frmArquivo" name="frmArquivo" runat="server">
								<TABLE id="Table1" style="WIDTH: 363px; HEIGHT: 16px" cellSpacing="1" cellPadding="1" width="363"
									border="0">
									<TR>
										<TD style="HEIGHT: 16px" align="right"><asp:label id="lblEmpresa" runat="server" Font-Size="X-Small" Font-Bold="True" Font-Name="Arial">Empresa:</asp:label></TD>
										<TD style="WIDTH: 188px; HEIGHT: 16px"><asp:dropdownlist id="cboEmpresa" runat="server" Font-Names="Arial" Font-Size="X-Small" Width="140px"
												Height="28px"></asp:dropdownlist></TD>
										<TD style="HEIGHT: 16px"><asp:ImageButton id="ImageButton1" runat="server" ImageUrl="images/bt_visualizar.gif" ImageAlign="Top"></asp:ImageButton></TD>
									</TR>
								</TABLE>
								<DIV class="modulo" id="divGrid" style="DISPLAY: inline; LEFT: 155px; WIDTH: 479px; POSITION: absolute; TOP: 155px; HEIGHT: 228px"
									ms_positioning="FlowLayout">
									<asp:DataGrid id="dtgInter" runat="server" AutoGenerateColumns="False" AllowCustomPaging="True"
										AllowPaging="True" Width="510px">
										<SelectedItemStyle Wrap="False" BackColor="Lavender"></SelectedItemStyle>
										<EditItemStyle Wrap="False"></EditItemStyle>
										<AlternatingItemStyle Font-Size="9pt" Font-Names="Arial" Wrap="False" BackColor="#F7F7F7"></AlternatingItemStyle>
										<ItemStyle Font-Size="9pt" Font-Names="Arial" Wrap="False" BackColor="#E9F4CF"></ItemStyle>
										<HeaderStyle Font-Size="X-Small" Font-Names="Arial" Font-Bold="True" ForeColor="Black" BackColor="#99CC00"></HeaderStyle>
										<Columns>
											<asp:BoundColumn DataField="sigempre" ReadOnly="True" HeaderText="Com a Empresa">
												<HeaderStyle Width="160px"></HeaderStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="contabil" ReadOnly="True" HeaderText="Intercambio Cont&#225;bil">
												<HeaderStyle Width="190px"></HeaderStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="desmodal" ReadOnly="True" HeaderText="Modalidade">
												<HeaderStyle Width="160px"></HeaderStyle>
											</asp:BoundColumn>
										</Columns>
										<PagerStyle NextPageText="Pr&#243;xima&amp;gt;" Font-Size="7pt" Font-Names="Arial" Font-Bold="True"
											PrevPageText="&amp;lt;Anterior"></PagerStyle>
									</asp:DataGrid></DIV>
							</form>
						</DIV>
					</TD>
				</TR>
			</TBODY>
		</TABLE>
	</BODY>
</HTML>
