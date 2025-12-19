<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" Codebehind="frmCadUsuario.aspx.vb" Inherits="pdpw.frmCadUsuario"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
		<title>ONS - Operador Nacional do Sistema Elétrico</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<meta http-equiv="Content-Type" content="text/html; charset=windows-1252">
		<LINK href="../pdpw/images/style.css" rel="stylesheet">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<TABLE style="WIDTH: 831px; HEIGHT: 584px" cellSpacing="0" cellPadding="0" border="0">
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
										<TD style="HEIGHT: 24px" height="24">
											<TABLE style="WIDTH: 764px; HEIGHT: 23px" cellSpacing="0" cellPadding="0" width="764" background="../pdpw/images/back_titulo.gif"
												border="0">
												<TBODY>
													<TR>
														<TD style="WIDTH: 762px; HEIGHT: 8px">
															<DIV align="left"><IMG style="WIDTH: 184px; HEIGHT: 23px" height="23" src="../pdpw/images/tit_CadUsuar.gif"
																	width="184"></DIV>
														</TD>
													</TR>
												</TBODY>
											</TABLE>
										</TD>
									</TR>
								</TBODY>
							</TABLE>
							<BR>
							<form id="frmCadUsuario" name="frmCadUsuario" runat="server">
								<table class="modulo" cellSpacing="0" cellPadding="0" border="0">
									<tr>
										<td style="WIDTH: 71px" align="right">Login:&nbsp;</td>
										<td><asp:textbox id="txtLogin" runat="server" MaxLength="8" Width="89px" Height="22px"></asp:textbox></td>
									</tr>
									<tr>
										<td style="WIDTH: 71px" align="right">Nome:&nbsp;</td>
										<td><asp:textbox id="txtNome" runat="server" MaxLength="40" Width="438px" Height="22px"></asp:textbox></td>
									</tr>
									<tr>
										<td style="WIDTH: 71px" align="right">E-mail:&nbsp;</td>
										<td><asp:textbox id="txtEmail" runat="server" MaxLength="40" Width="438px" Height="22px"></asp:textbox></td>
									</tr>
									<tr>
										<td style="WIDTH: 71px" align="right">Telefone:&nbsp;</td>
										<td><asp:textbox id="txtTelefone" runat="server" MaxLength="20" Width="121px" Height="22px"></asp:textbox></td>
									</tr>
									<tr>
										<td style="HEIGHT: 15px" colSpan="2"></td>
									</tr>
									<tr>
										<td colSpan="2"><asp:datagrid id="dtgUsuario" runat="server" Width="620px" Font-Size="XX-Small" OnPageIndexChanged="dtgUsuario_Paged"
												Font-Names="Arial" AutoGenerateColumns="False" AllowPaging="True" PageSize="4">
												<SelectedItemStyle BackColor="Lavender"></SelectedItemStyle>
												<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
												<ItemStyle BackColor="#E9F4CF"></ItemStyle>
												<HeaderStyle Font-Bold="True" BackColor="YellowGreen"></HeaderStyle>
												<Columns>
													<asp:TemplateColumn>
														<HeaderStyle Width="20px"></HeaderStyle>
														<ItemStyle Width="10px"></ItemStyle>
														<ItemTemplate>
															<asp:CheckBox id="chkMarca" Runat="server" />
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:BoundColumn DataField="usuar_id" HeaderText="Login">
														<HeaderStyle Width="100px"></HeaderStyle>
													</asp:BoundColumn>
													<asp:BoundColumn DataField="usuar_nome" HeaderText="Nome">
														<HeaderStyle Width="200px"></HeaderStyle>
													</asp:BoundColumn>
													<asp:BoundColumn DataField="usuar_email" HeaderText="E-mail">
														<HeaderStyle Width="200px"></HeaderStyle>
													</asp:BoundColumn>
													<asp:BoundColumn DataField="usuar_telefone" HeaderText="Telefone">
														<HeaderStyle Width="100px"></HeaderStyle>
													</asp:BoundColumn>
												</Columns>
												<PagerStyle NextPageText="Pr&#243;xima&amp;gt;" Font-Size="XX-Small" Font-Names="Arial" Font-Bold="True"
													PrevPageText="&amp;lt;Anterior"></PagerStyle>
											</asp:datagrid></td>
									</tr>
								</table>
								<DIV id="divPesquisa" style="DISPLAY: inline; LEFT: 164px; WIDTH: 456px; POSITION: absolute; TOP: 380px; HEIGHT: 50px"
									runat="server" ms_positioning="FlowLayout">
									<table align="center">
										<TR>
											<TD style="WIDTH: 71px"><asp:imagebutton id="btnPesquisar" runat="server" Width="71px" Height="25px" ImageUrl="../pdpw/images/bt_pesquisar.gif"></asp:imagebutton></TD>
											<TD style="WIDTH: 71px"><asp:imagebutton id="btnAlterar" runat="server" Width="71px" Height="25px" ImageUrl="../pdpw/images/bt_alterar.gif"
													EnableViewState="False"></asp:imagebutton></TD>
											<TD style="WIDTH: 71px"><asp:imagebutton id="btnSalvar" runat="server" Width="71px" Height="25px" ImageUrl="../pdpw/images/bt_salvar.gif"></asp:imagebutton></TD>
											<TD style="WIDTH: 71px"><asp:imagebutton id="btnExcluir" runat="server" Width="71px" Height="25px" ImageUrl="../pdpw/images/bt_excluir.gif"></asp:imagebutton></TD>
											<TD style="WIDTH: 71px"><asp:imagebutton id="btnCancelar" runat="server" Width="71px" Height="25px" ImageUrl="../pdpw/images/bt_cancelar.gif"></asp:imagebutton></TD>
										</TR>
									</table>
								</DIV>
							</form>
						</DIV>
					</TD>
				</TR>
			</TBODY>
		</TABLE>
</asp:Content>
