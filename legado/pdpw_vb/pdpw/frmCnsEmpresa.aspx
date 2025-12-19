<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" Codebehind="frmCnsEmpresa.aspx.vb" Inherits="pdpw.frmCnsEmpresa"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <TITLE>ONS - Operador Nacional do Sistema Elétrico</TITLE>
		<META http-equiv="Content-Type" content="text/html; charset=windows-1252">
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="../pdpw/images/style.css" rel="stylesheet">
        <style type="text/css">
            .auto-style1 {
                margin-top: 38px;
            }
        </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <TABLE style="WIDTH: 966px; HEIGHT: 354px" height="354" cellSpacing="0" cellPadding="0"
			width="966" border="0">
			<TBODY>
				<TR>
					<TD style="WIDTH: 75px; HEIGHT: 100px" vAlign="top" width="75"><BR>
					</TD>
					<TD style="WIDTH: 1231px" vAlign="top">
						<DIV align="center">
							<TABLE cellSpacing="0" cellPadding="0" width="100%" border="0">
								<TBODY>
									<TR>
										<TD width="20%" height="2">&nbsp;</TD>
									</TR>
									<TR>
										<TD style="HEIGHT: 17px" height="17">
											<TABLE style="WIDTH: 867px; HEIGHT: 26px" cellSpacing="0" cellPadding="0" width="867" background="../pdpw/images/back_tit_sistema.gif"
												border="0">
												<TBODY>
													<TR>
														<TD style="HEIGHT: 26px"><IMG height="25" src="../pdpw/images/tit_sis_guideline.gif" width="179"></TD>
													</TR>
												</TBODY>
											</TABLE>
										</TD>
									</TR>
									<TR>
										<TD style="HEIGHT: 10px" height="10">
											<TABLE style="WIDTH: 868px; HEIGHT: 23px" cellSpacing="0" cellPadding="0" width="868" background="../pdpw/images/back_titulo.gif"
												border="0">
												<TBODY>
													<TR>
														<TD style="HEIGHT: 8px">
															<DIV align="left"><IMG style="WIDTH: 120px; HEIGHT: 23px" height="23" src="../pdpw/images/tit_Empresas.gif"
																	width="120"></DIV>
														</TD>
													</TR>
												</TBODY>
											</TABLE>
										</TD>
									</TR>
								</TBODY>
							</TABLE>
							<form id="frmCnsEmpresa" name="frmCnsEmpresa" runat="server">
								<DIV class="modulo" style="DISPLAY: inline; LEFT: 30px; WIDTH: 852px; POSITION: absolute; TOP: 90px; HEIGHT: 240px"
									ms_positioning="FlowLayout"></DIV>
							    <asp:datagrid id="dtgEmpresa" runat="server" AllowCustomPaging="True" PageSize="8" AllowPaging="True"
										AutoGenerateColumns="False" Width="850px" CssClass="auto-style1" Height="249px">
										<SelectedItemStyle BackColor="Lavender"></SelectedItemStyle>
										<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
										<ItemStyle Font-Size="8pt" Font-Names="Arial" BackColor="#E9F4CF"></ItemStyle>
										<HeaderStyle Font-Size="9pt" Font-Names="Arial" Font-Bold="True" Wrap="False" HorizontalAlign="Center"
											ForeColor="Black" BackColor="#99CC00"></HeaderStyle>
										<Columns>
											<asp:BoundColumn DataField="codempre" ReadOnly="True" HeaderText="Empresa">
												<HeaderStyle Width="60px"></HeaderStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="nomempre" ReadOnly="True" HeaderText="Nome">
												<HeaderStyle Width="170px"></HeaderStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="sigempre" ReadOnly="True" HeaderText="Sigla">
												<HeaderStyle Width="90px"></HeaderStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="idgtpoempre" ReadOnly="True" HeaderText="GTPO">
												<HeaderStyle Width="40px"></HeaderStyle>
											</asp:BoundColumn>
											<asp:TemplateColumn SortExpression="contr" HeaderText="Controladora de &#193;rea">
												<HeaderStyle Width="80px"></HeaderStyle>
												<ItemStyle HorizontalAlign="Center"></ItemStyle>
												<ItemTemplate>
													<asp:CheckBox runat="server" id="Checkbox1" Enabled=False Checked='<%# DataBinder.Eval(Container.DataItem, "contr") %>'/>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:BoundColumn DataField="regiao" ReadOnly="True" HeaderText="Regi&#227;o">
												<HeaderStyle Width="60px"></HeaderStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="sistema" ReadOnly="True" HeaderText="Sistema">
												<HeaderStyle Width="60px"></HeaderStyle>
											</asp:BoundColumn>
											<asp:TemplateColumn SortExpression="area_contr" HeaderText="Controlada por outra Empresa">
												<HeaderStyle Width="100px"></HeaderStyle>
												<ItemStyle HorizontalAlign="Center"></ItemStyle>
												<ItemTemplate>
													<asp:CheckBox runat="server" id="Checkbox4" Enabled=False Checked='<%# DataBinder.Eval(Container.DataItem, "area_contr") %>'/>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:BoundColumn DataField="area_nao_contr" ReadOnly="True" HeaderText="&#193;rea">
												<HeaderStyle Width="60px"></HeaderStyle>
											</asp:BoundColumn>
											<asp:TemplateColumn SortExpression="infpdp" HeaderText="PDP Informado">
												<HeaderStyle Width="70px"></HeaderStyle>
												<ItemStyle HorizontalAlign="Center"></ItemStyle>
												<ItemTemplate>
													<asp:CheckBox runat="server" id="Checkbox2" Enabled =False Checked='<%# DataBinder.Eval(Container.DataItem, "infpdp") %>'/>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:BoundColumn DataField="empresa_nao_contr" ReadOnly="True" HeaderText="Empresa">
												<HeaderStyle Width="60px"></HeaderStyle>
											</asp:BoundColumn>
										</Columns>
										<PagerStyle NextPageText="Pr&#243;xima&amp;gt;" Font-Size="XX-Small" Font-Names="Arial" Font-Bold="True"
											PrevPageText="&amp;lt;Anterior"></PagerStyle>
									</asp:datagrid>
							</form>
						</DIV>
					</TD>
				</TR>
			</TBODY>
		</TABLE>
</asp:Content>
