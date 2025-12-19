<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" Codebehind="frmCnsEnvioEmp.aspx.vb" Inherits="pdpw.frmCnsEnvioEmp"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
		<title>ONS - Operador Nacional do Sistema Elétrico</title>
		<meta http-equiv="Content-Type" content="text/html; charset=windows-1252">
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="images/style.css" rel="stylesheet">
        <script language="javascript" type="text/javascript" src="js/MSGAguarde.js"></script>
        <link href="css/MSGAguarde.css" rel="stylesheet" />

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
							<form id="frmCnsEnvioEmp" name="frmCnsEnvioEmp" runat="server">
								<DIV style="DISPLAY: inline; LEFT: 200px; WIDTH: 189px; POSITION: absolute; TOP: 110px; HEIGHT: 27px"
									ms_positioning="FlowLayout">
									<TABLE class="modulo" id="Table1" style="WIDTH: 295px; HEIGHT: 36px" cellSpacing="0" cellPadding="0"
										border="0">
										<TR>
											<TD style="WIDTH: 111px" align="right">Data PDP:
											</TD>
											<TD style="WIDTH: 147px">&nbsp;
												<asp:dropdownlist id="cboData" runat="server" Width="97px"></asp:dropdownlist></TD>
											<TD rowSpan="2">
												<asp:imagebutton id="btnPesquisar" runat="server" ImageUrl="images/bt_visualizar.gif" ImageAlign="Top"></asp:imagebutton></TD>
										</TR>
									</TABLE>
								</DIV>
                                    
								<DIV class="modulo" style="DISPLAY: inline; LEFT: 30px; WIDTH: 649px; POSITION: absolute; TOP: 160px; HEIGHT: 240px"
									ms_positioning="FlowLayout">

                                    <asp:Label ID="lblMsg" Visible="false" ForeColor="Green" runat="server">Desculpe, este dado não pôde ser carregado no momento.</asp:Label>
                                    
									<asp:DataGrid id="dtgEnvio" runat="server" OnPageIndexChanged="dtgEnvio_Paged" 
                                        Width="640px" AutoGenerateColumns="False"
										AllowPaging="True" PageSize="15">
										<SelectedItemStyle BackColor="Lavender"></SelectedItemStyle>
										<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
										<ItemStyle Font-Size="8pt" Font-Names="Arial" BackColor="#E9F4CF"></ItemStyle>
										<HeaderStyle Font-Size="9pt" Font-Names="Arial" Font-Bold="True" HorizontalAlign="Center" ForeColor="Black"
											BackColor="#99CC00"></HeaderStyle>
										<Columns>
											<asp:BoundColumn DataField="codempre" HeaderText="C&#243;digo">
												<HeaderStyle Width="70px"></HeaderStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="nomempre" HeaderText="Nome">
												<HeaderStyle Width="140px"></HeaderStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="qtdenvio" HeaderText="N&#250;mero de Envio">
												<HeaderStyle Width="100px"></HeaderStyle>
											</asp:BoundColumn>										    
										    <asp:TemplateColumn HeaderText="Data Início">
                                            <HeaderStyle Width="110px"></HeaderStyle>
								                <ItemTemplate>
									                <asp:Label id="lblDataHoraIni" runat="server" 
                                                        Text='<%# DataBinder.Eval(Container.DataItem,"dthinipdp") %>' 
                                                        Visible='<%# DataBinder.Eval(Container.DataItem,"id_controleagentepdp") = "0" %>'></asp:Label>
                                                    <asp:Label id="lblDataHoraIniControleAgente" runat="server" 
                                                        Text='<%# DataBinder.Eval(Container.DataItem,"dthinipdpcontroleage") %>'
                                                        Visible='<%# DataBinder.Eval(Container.DataItem,"id_controleagentepdp") <> "0" %>'></asp:Label>                                                        
								                </ItemTemplate>                                            
                                            </asp:TemplateColumn>

                                            <asp:TemplateColumn HeaderText="Data Fim">
                                            <HeaderStyle Width="120px"></HeaderStyle>
								                <ItemTemplate>
									                <asp:Label id="lblDataHoraFim" runat="server" 
                                                        Text='<%# DataBinder.Eval(Container.DataItem,"dthfimpdp") %>'
                                                        Visible='<%# DataBinder.Eval(Container.DataItem,"id_controleagentepdp") = "0" %>'></asp:Label>
                                                    <asp:Label id="lblDataHoraFimControleAgente" runat="server" 
                                                        Text='<%# DataBinder.Eval(Container.DataItem,"dthfimpdpcontroleage") %>'
                                                         Visible='<%# DataBinder.Eval(Container.DataItem,"id_controleagentepdp") <> "0" %>'></asp:Label>         
								                </ItemTemplate>
                                                <ItemStyle Font-Bold="True" Font-Italic="True" Font-Overline="False" 
                                                    Font-Strikeout="False" Font-Underline="False" ForeColor="Teal" />
                                            </asp:TemplateColumn>

										    <asp:BoundColumn DataField="flg_alterado_ons" HeaderText="Alterado ONS">
                                                <HeaderStyle Width="100px" />
                                            </asp:BoundColumn>

										</Columns>
										<PagerStyle NextPageText="Pr&#243;xima&amp;gt;" Font-Size="XX-Small" Font-Names="Arial" Font-Bold="True"
											PrevPageText="&amp;lt;Anterior"></PagerStyle>
									</asp:DataGrid></DIV>
                                    <div class="modulo" style="DISPLAY: inline; LEFT: 30px; WIDTH: 649px; POSITION: absolute; TOP: 458px; HEIGHT: 240px"
									ms_positioning="FlowLayout">
                                        <asp:datagrid id="dtgResponsaveis" runat="server" Width="560px" 
                                            Font-Size="XX-Small"
											Font-Names="Arial" AutoGenerateColumns="False" AllowPaging="True" PageSize="4">
											<SelectedItemStyle BackColor="Lavender"></SelectedItemStyle>
											<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
											<ItemStyle BackColor="#E9F4CF"></ItemStyle>
											<HeaderStyle Font-Bold="True" BackColor="YellowGreen"></HeaderStyle>
											<Columns>
												<asp:TemplateColumn Visible="False">
													<HeaderStyle Width="20px"></HeaderStyle>
													<ItemStyle Width="10px"></ItemStyle>
													<ItemTemplate>															
														<asp:Label ID="lblObjId" runat="server" 
                                                            Text='<%# DataBinder.Eval(Container, "DataItem.id_responsavelprogpdp") %>' 
                                                            Visible="False"></asp:Label>
													</ItemTemplate>
												</asp:TemplateColumn>
												<asp:BoundColumn DataField="usuar_nome" HeaderText="Usuário">
													<HeaderStyle Width="280px"></HeaderStyle>
												</asp:BoundColumn>
												<asp:BoundColumn DataField="nom_equipepdp" HeaderText="Equipe">
													<HeaderStyle Width="140px"></HeaderStyle>
												</asp:BoundColumn>
												<asp:BoundColumn DataField="tip_programacaoDescricao" HeaderText="Tipo Programação">
                                                    <HeaderStyle Width="100px" />
                                                </asp:BoundColumn>

											</Columns>
											<PagerStyle NextPageText="Pr&#243;xima&amp;gt;" Font-Size="XX-Small" Font-Names="Arial" Font-Bold="True"
												PrevPageText="&amp;lt;Anterior"></PagerStyle>
										</asp:datagrid>
                                    </div>
							</form>
						</DIV>
					</TD>
				</TR>
			</TBODY>
		</TABLE>

</asp:Content>

