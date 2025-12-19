<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" Codebehind="OldfrmAssocUsuarEquipe.aspx.vb" Inherits="pdpw.frmAssocUsuarEquipe"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
		<title>ONS - Operador Nacional do Sistema Elétrico</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="../pdpw/images/style.css" rel="stylesheet">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<table style="WIDTH: 1098px; HEIGHT: 536px" height="536" cellSpacing="0" cellPadding="0"
			width="1098" border="0">
			<tr>
				<td style="WIDTH: 19px; HEIGHT: 250px" vAlign="top" width="19"><BR>
				</td>
				<td style="WIDTH: 1064px; HEIGHT: 250px" vAlign="top">
					<div align="center">
						<table style="WIDTH: 1066px; HEIGHT: 64px" cellSpacing="0" cellPadding="0" width="1066"
							border="0">
							<tr>
								<td style="HEIGHT: 4px" width="20%" height="4">&nbsp;</td>
							</tr>
							<tr>
								<td style="HEIGHT: 23px" height="23">
									<table style="WIDTH: 939px; HEIGHT: 25px" cellSpacing="0" cellPadding="0" width="939" background="../pdpw/images/back_tit_sistema.gif"
										border="0">
										<tr>
											<td style="HEIGHT: 12px"><IMG height="25" src="../pdpw/images/tit_sis_guideline.gif" width="179"></td>
										</tr>
									</table>
								</td>
							</tr>
							<tr>
								<td style="HEIGHT: 10px" height="10">
									<table style="WIDTH: 940px; HEIGHT: 23px" cellSpacing="0" cellPadding="0" width="940" background="../pdpw/images/back_titulo.gif"
										border="0">
										<tr>
											<td style="HEIGHT: 8px">
												<div align="left"><IMG style="WIDTH: 341px; HEIGHT: 23px" height="23" 
                                                        src="../pdpw/images/tit_AssociarUsuEquipePdp.GIF"></div>
											</td>
										</tr>
									</table>
								</td>
							</tr>
						</table>
						<BR>
						<form id="frmAssocUsuarEquipe" name="frmAssocUsuarEquipe" runat="server">
							<DIV style="DISPLAY: inline; LEFT: 56px; WIDTH: 526px; POSITION: absolute; TOP: 200px; HEIGHT: 208px"
								ms_positioning="FlowLayout">
								<TABLE id="Table2" style="LEFT: 0px; WIDTH: 520px; POSITION: absolute; TOP: 0px; HEIGHT: 192px">
									<TR>
										<TD style="HEIGHT: 168px" vAlign="top" align="left" colSpan="3"><asp:datagrid id="dtgAssociar" runat="server" Width="500px" PageSize="5" AllowPaging="True" AutoGenerateColumns="False"
												Font-Names="Arial" OnPageIndexChanged="dtgAssociar_Paged" Font-Size="XX-Small">
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
														    <asp:Label ID="lblObjId" runat="server" 
                                                                Text='<%# DataBinder.Eval(Container, "DataItem.id_usuarequipepdp") %>' 
                                                                Visible="False"></asp:Label>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:BoundColumn Visible="False" DataField="id_equipepdp" 
                                                        HeaderText="id_equipepdp"></asp:BoundColumn>
													<asp:BoundColumn DataField="nom_equipepdp" HeaderText="Equipe">
														<HeaderStyle Width="150px"></HeaderStyle>
													</asp:BoundColumn>
													<asp:BoundColumn Visible="False" DataField="usuar_id" HeaderText="usuar_id"></asp:BoundColumn>
													<asp:BoundColumn DataField="usuar_nome" HeaderText="Usu&#225;rio">
														<HeaderStyle Width="330px"></HeaderStyle>
													</asp:BoundColumn>
												</Columns>
												<PagerStyle NextPageText="Pr&#243;xima&amp;gt;" Font-Size="XX-Small" Font-Names="Arial" Font-Bold="True"
													PrevPageText="&amp;lt;Anterior"></PagerStyle>
											</asp:datagrid></TD>
									</TR>
									<TR>
										<TD align="right"><asp:imagebutton id="imgIncluir" runat="server" ImageUrl="images\bt_incluir.gif"></asp:imagebutton></TD>
										<TD align="center"></TD>
										<TD align="left"><asp:imagebutton id="btnExcluir" runat="server" ImageUrl="images\bt_excluir.gif"></asp:imagebutton></TD>
									</TR>
								</TABLE>
							</DIV>
							<DIV style="DISPLAY: inline; LEFT: 80px; WIDTH: 347px; POSITION: absolute; TOP: 120px; HEIGHT: 64px">
								<TABLE style="LEFT: 0px; WIDTH: 448px; POSITION: absolute; TOP: 0px; HEIGHT: 60px" cellSpacing="0"
									cellPadding="0" border="0">
									<TR height="30">
										<TD style="WIDTH: 69px" align="right"><FONT face="Arial" size="2">Equipe:</FONT></TD>
										<TD>&nbsp;
											<asp:dropdownlist id="cboEquipe" runat="server" AutoPostBack="True" 
                                                Width="219px"></asp:dropdownlist></TD>
									</TR>
									<TR height="30">
										<TD style="WIDTH: 69px" align="right"><FONT face="Arial" size="2">Usuário:</FONT></TD>
										<TD>&nbsp;
											<asp:dropdownlist id="cboUsuario" runat="server" AutoPostBack="True" Width="372px"></asp:dropdownlist></TD>
									</TR>
								</TABLE>
							</DIV>
						</form>
					</div>
				</td>
			</tr>
		</table>
</asp:Content>
