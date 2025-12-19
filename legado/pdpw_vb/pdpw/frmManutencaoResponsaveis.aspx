<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="frmManutencaoResponsaveis.aspx.vb" Inherits="pdpw.frmManutencaoResponsaveis" %>
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
        <script language="javascript">

            function Mascara_Data(CampoData) {
                var data = CampoData.value;

                if (data.length == 2) {
                    data = data + '/';
                    CampoData.value = data;
                }
                if (data.length == 5) {
                    data = data + '/';
                    CampoData.value = data;
                }
            }

            function Mascara_Hora(CampoHora) {
                var hora01 = '';
                hora01 = hora01 + CampoHora.value;
                if (hora01.length == 2) {
                    hora01 = hora01 + ':';
                    CampoHora.value = hora01;
                }
            }
            
        </script>
	</HEAD>
	<BODY bgColor="#ffffff" leftMargin="0" topMargin="0">
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
															<DIV align="left">
                                                                <IMG style="WIDTH: 337px; HEIGHT: 23px" height="23" 
                                                                    src="../pdpw/images/tit_CadastroRespPdp.GIF"></DIV>
														</TD>
													</TR>
												</TBODY>
											</TABLE>
										</TD>
									</TR>
								</TBODY>
							</TABLE>
							<BR>
							<form id="frmManutencaoResponsaveis" name="frmManutencaoResponsaveis" runat="server">
                                <table class="modulo" cellSpacing="0" cellPadding="0" border="0" width="800">                                    
                                    <tr>
										<td style="WIDTH: 81px" align="right">Data PDP:&nbsp;</td>
										<td><asp:DropDownList id="cboData" runat="server" Width="97px" AutoPostBack="True"></asp:DropDownList></td>
									</tr>
									<tr>
										<td style="HEIGHT: 20px" colSpan="2"></td>
									</tr>																		
								</table>
								<table class="modulo" cellSpacing="0" cellPadding="0" border="0">
                                    <tr>
										<td colSpan="2">
                                               <asp:datagrid id="dtgResponsaveis" runat="server" Width="800px" 
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
														    <asp:CheckBox ID="chkMarca" runat="server" />
														    <asp:Label ID="lblObjId" runat="server" 
                                                                Text='<%# DataBinder.Eval(Container, "DataItem.id_responsavelprogpdp") %>' 
                                                                Visible="False"></asp:Label>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:BoundColumn DataField="usuar_nome" HeaderText="Usuário">
														<HeaderStyle Width="280px"></HeaderStyle>
													</asp:BoundColumn>
													<asp:BoundColumn DataField="nom_equipepdp" HeaderText="Equipe">
														<HeaderStyle Width="120px"></HeaderStyle>
													</asp:BoundColumn>
												    <asp:BoundColumn DataField="tip_programacaoDescricao" HeaderText="Tipo Programação">
                                                        <HeaderStyle Width="100px" />
                                                    </asp:BoundColumn>
												    <asp:BoundColumn DataField="din_inicioprogpdp" HeaderText="Início">
                                                        <HeaderStyle Width="120px" />
                                                    </asp:BoundColumn>
                                                    <asp:TemplateColumn HeaderText="Fim">
                                                        <EditItemTemplate>
                                                            <asp:TextBox runat="server" 
                                                                Text='<%# DataBinder.Eval(Container, "DataItem.din_fimprogpdp") %>'></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDataFim" runat="server" 
                                                                Text='<%# DataBinder.Eval(Container, "DataItem.din_fimprogpdp") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
												    <asp:ButtonColumn Text="excluir" CommandName="Delete">
													    <HeaderStyle Width="40px"></HeaderStyle>
													    <ItemStyle ForeColor="Gray"></ItemStyle>
													</asp:ButtonColumn>
												</Columns>
												<PagerStyle NextPageText="Pr&#243;xima&amp;gt;" Font-Size="XX-Small" Font-Names="Arial" Font-Bold="True"
													PrevPageText="&amp;lt;Anterior"></PagerStyle>
											   </asp:datagrid>
                                            </td>
									</tr>
                                    <tr>
										<td style="HEIGHT: 20px" colSpan="2"></td>
									</tr>
                                </table>
                                <table class="modulo" cellSpacing="0" cellPadding="0" border="0" align="center" width="800px">
                                        <TR>
											<TD style="WIDTH: 798px" align="center">
                                                <asp:imagebutton id="btnIncluir" runat="server" Width="71px" Height="25px" ImageUrl="../pdpw/images/bt_incluir.gif"></asp:imagebutton>
                                            </TD>
											<TD style="WIDTH: 1px">
                                                <asp:imagebutton id="btnAlterar" runat="server" Width="71px" Height="25px" ImageUrl="../pdpw/images/bt_alterar.gif" 
                                                    Visible="False"></asp:imagebutton>
                                            </TD>
											<TD style="WIDTH: 1px"><asp:imagebutton id="btnExcluir" runat="server" 
                                                    Width="71px" Height="25px" ImageUrl="../pdpw/images/bt_excluir.gif" 
                                                    Visible="False"></asp:imagebutton></TD>
										</TR>
                                        <tr>
										<td style="HEIGHT: 30px" colSpan="2">
                                            <asp:HiddenField ID="hiddenAcao" runat="server" />
                                            </td>
									    </tr>
                                </table>
                                <table class="modulo" cellSpacing="0" cellPadding="0" border="1" borderColor="#e9f4cf">                                    
                                        <tr>
										    <td style="HEIGHT: 20px" colSpan="2"></td>
									    </tr>
                                        <tr>
										    <td style="WIDTH: 140px" align="right">Equipe:&nbsp;</td>
										    <td style="HEIGHT: 25px"><asp:dropdownlist id="cboEquipe" runat="server" AutoPostBack="True" 
                                                Width="219px" Font-Size="Smaller"></asp:dropdownlist></td>
									    </tr>
									    <tr>
										    <td style="WIDTH: 140px" align="right">Usuário Responsável:&nbsp;</td>
										    <td style="HEIGHT: 25px">
                                                <asp:dropdownlist id="cboUsuario" runat="server" 
                                                Width="372px" Font-Size="Smaller"></asp:dropdownlist></td>
									    </tr>
                                        <tr>
										    <td style="WIDTH: 140px" align="right">Tipo de Programação:&nbsp;</td>
										    <td style="HEIGHT: 25px"><asp:radiobuttonlist id="optTipoOperacao" runat="server" Width="163px" 
                                                AutoPostBack="False" Height="22px" RepeatDirection="Horizontal" 
                                                Font-Size="Smaller">													
													<asp:ListItem Value="L">Elétrica</asp:ListItem>
                                                    <asp:ListItem Value="N">Energética</asp:ListItem>
										    </asp:radiobuttonlist></td>
									    </tr>
                                        <tr>
										    <td style="WIDTH: 140px" align="right">Data Início Programação:&nbsp;</td>
										    <td style="HEIGHT: 25px"><asp:textbox id="txtDataInicio" onkeyup="Mascara_Data(this)" runat="server" Height="21px" Width="91px" MaxLength="10"></asp:textbox>&nbsp;&nbsp; 
														<asp:textbox id="txtHoraInicio" onkeyup="Mascara_Hora(this)" runat="server" Height="21px" Width="60px" 
                                                            MaxLength="5"></asp:textbox>
                                            
                                            </td>
									    </tr>
                                        <tr>
										    <td style="WIDTH: 140px" align="right">&nbsp;</td>
										    <td style="HEIGHT: 25px"><asp:textbox id="txtDataFim" onkeyup="Mascara_Data(this)" 
                                                    runat="server" Height="21px" Width="91px" MaxLength="10" Visible="False"></asp:textbox>&nbsp;&nbsp; 
														<asp:textbox id="txtHoraFim" onkeyup="Mascara_Hora(this)" 
                                                    runat="server" Height="21px" Width="60px" 
                                                            MaxLength="5" Visible="False"></asp:textbox>                                            
                                            </td>
									    </tr>
                                        <tr>
										    <td style="HEIGHT: 20px" colSpan="2"></td>
									    </tr>																		
								    </table>
                                <table class="modulo" cellSpacing="0" cellPadding="0" border="0" align="center">
                                        <TR>
											<TD style="WIDTH: 81px"><asp:imagebutton id="btnSalvar" runat="server" Width="71px" 
                                                    Height="25px" ImageUrl="../pdpw/images/bt_salvar.gif" Visible="False"></asp:imagebutton></TD>
											<TD style="WIDTH: 81px"><asp:imagebutton id="btnCancelar" runat="server" 
                                                    Width="71px" Height="25px" ImageUrl="../pdpw/images/bt_cancelar.gif" 
                                                    Visible="False"></asp:imagebutton></TD>
										</TR>
                                </table>
								
							</form>
						</DIV>
					</TD>
				</TR>
			</TBODY>
		</TABLE>
	</BODY>
</HTML>
