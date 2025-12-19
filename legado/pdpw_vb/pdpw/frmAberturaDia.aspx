<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frmAberturaDia.aspx.vb" Inherits="pdpw.frmAberturaDia" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<title>ONS - Operador Nacional do Sistema Elétrico</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<meta http-equiv="Content-Type" content="text/html; charset=windows-1252">
		<LINK href="../pdpw/images/style.css" rel="stylesheet">
        <script language="javascript">

            //function Mascara_Data(CampoData) 
            //{
            //    var data = CampoData.value;
            //    var tecla = event.keyCode;
            //
            //    if ((data.length == 2) && (tecla != 8)) 
            //    {
            //        data = data + '/';
            //        CampoData.value = data;                    
            //    }
            //    if ((data.length == 5) && (tecla != 8)) 
            //    {
            //        data = data + '/';
            //        CampoData.value = data;                    
            //   }
            //}

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

            function preencheDataFinalSugestao() {
                if (frmAberturaDia.cboData.selectedIndex > 0) {
                    var comboDia = document.getElementById("cboData");
                    frmAberturaDia.txtDataFinal.value = comboDia.options[comboDia.selectedIndex].text;
                    frmAberturaDia.txtHoraFinal.value = "14:00";
                }
                else {
                    frmAberturaDia.txtDataFinal.value = "";
                    frmAberturaDia.txtHoraFinal.value = "";
                }
            }
        </script>

        <script language="javascript" type="text/javascript" src="js/MSGAguarde.js"></script>
        <link href="css/MSGAguarde.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <TABLE style="WIDTH: 784px; HEIGHT: 256px" height="256" cellSpacing="0" cellPadding="0"
			width="784" border="0">
			<TBODY>
				<TR>
					<TD vAlign="top" width="55" style="WIDTH: 55px"><BR>
					</TD>
					<TD style="WIDTH: 781px" vAlign="top">
						<DIV align="center">
							<TABLE cellSpacing="0" cellPadding="0" width="100%" border="0">
								<TBODY>
									<TR>
										<TD width="20%" height="5" style="HEIGHT: 5px">&nbsp;</TD>
									</TR>
									<TR>
										<TD height="33" style="HEIGHT: 33px">
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
															<DIV align="left" style="height: 27px"><IMG height="23" 
                                                                    src="../pdpw/images/tit_Abrir_Dia.gif" style="WIDTH: 676px; HEIGHT: 23px"></DIV>
														</TD>
													</TR>
												</TBODY>
											</TABLE>
										</TD>
									</TR>
								</TBODY>
							</TABLE>
							<BR>
							<form id="frmAberturaDia" name="frmAberturaDia" runat="server">
                        	
                                <table style="WIDTH: 726px; HEIGHT: 45px" cellSpacing="0" cellPadding="0" border="0" bordercolor="#e9f4cf" >
									<tr height="30">
										<td style="WIDTH: 150px; HEIGHT: 42px" align="right"><b>Operação:</b></td>
										<td style="WIDTH: 565px; HEIGHT: 42px">
                                        <asp:radiobuttonlist id="optOperacao" runat="server" Width="563px" 
                                                AutoPostBack="True" Height="22px" RepeatDirection="Horizontal">
													<asp:ListItem Value="A" Selected="True">Abrir dia</asp:ListItem>
													<asp:ListItem Value="R">Reabrir dia</asp:ListItem>
                                                    <asp:ListItem Value="E">Reabrir dia por Empresa</asp:ListItem>
										</asp:radiobuttonlist>                                                
										</td>
									</tr>	
                                    <tr height="30">
                                    <td style="WIDTH: 150px; HEIGHT: 42px" align="right">
                                        <b><asp:Label ID="lblEmpresa" runat="server" Text="Empresa:"></asp:Label></b>
                                        </td>
                                    <td style="WIDTH: 565px; HEIGHT: 42px">
                                        <asp:listbox id="lstBoxEmpresas" runat="server" Height="158px" Width="201px" 
                                            CssClass="formulario_texto" SelectionMode="Multiple" Enabled="False"></asp:listbox>
                                    </td>                                        
                                    </tr>	
                                    <tr height="30">
										<td style="WIDTH: 150px; HEIGHT: 42px" align="right"><b>Data PDP:</b></td>
										<td style="WIDTH: 565px; HEIGHT: 42px">&nbsp;
                                                <asp:DropDownList id="cboData" runat="server" Width="97px" 
                                                AutoPostBack="True"></asp:DropDownList>&nbsp;&nbsp;												
										</td>
									</tr>	
                                    <tr height="30">
										<td style="WIDTH: 150px; HEIGHT: 42px" align="right"><b>Data Hora Final:</b></td>
										<td style="WIDTH: 565px; HEIGHT: 42px">&nbsp;
                                                		<asp:textbox id="txtDataFinal" onkeyup="Mascara_Data(this)" runat="server" Height="21px" Width="91px" MaxLength="10"></asp:textbox>&nbsp;&nbsp; 
														<asp:textbox id="txtHoraFinal" onkeyup="Mascara_Hora(this)" runat="server" Height="21px" Width="60px" 
                                                            MaxLength="5"></asp:textbox>												
										</td>
									</tr>

                              
                                    <tr height="30">
                                        <td style="WIDTH: 150px; HEIGHT: 42px" align="right"><b>Responsáveis:</b></td>
                                        <td style="WIDTH: 565px; HEIGHT: 42px">

                                        <!-- -->
                                        <table class="modulo" cellspacing="0" cellpadding="0" border="1" bordercolor="#e9f4cf">
                                	        <tr>
										       <td colspan="2">
                                        <table class="modulo" cellspacing="0" cellpadding="0" border="0">
                                	        <tr>
										       <td colspan="2">
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
                                        </table>
                                        <table class="modulo" cellSpacing="0" cellPadding="0">                                    
                                        <tr>
										    <td style="HEIGHT: 20px" colSpan="2"></td>
									    </tr>
                                        <tr>
										    <td style="WIDTH: 130px" align="right">Equipe:&nbsp;</td>
										    <td style="HEIGHT: 25px"><asp:dropdownlist id="cboEquipe" runat="server" AutoPostBack="True" 
                                                Width="219px" Font-Size="Smaller"></asp:dropdownlist></td>
									    </tr>
									    <tr>
										    <td style="WIDTH: 130px" align="right">Usuário Responsável:&nbsp;</td>
										    <td style="HEIGHT: 25px">
                                                <asp:dropdownlist id="cboUsuario" runat="server" 
                                                Width="372px" Font-Size="Smaller"></asp:dropdownlist></td>
									    </tr>
                                        <tr>
										    <td style="WIDTH: 130px" align="right">Tipo de Programação:&nbsp;</td>
										    <td style="HEIGHT: 25px"><asp:radiobuttonlist id="optTipoOperacao" runat="server" Width="163px" 
                                                AutoPostBack="False" Height="22px" RepeatDirection="Horizontal" 
                                                Font-Size="Smaller">													
													<asp:ListItem Value="L">Elétrica</asp:ListItem>
                                                    <asp:ListItem Value="N">Energética</asp:ListItem>
										    </asp:radiobuttonlist></td>
									    </tr>
                                        <tr>
										    <td style="HEIGHT: 20px" colSpan="2"></td>
									    </tr>																		
								    </table>
                                    <table class="modulo" cellSpacing="0" cellPadding="0" border="0" align="center">
                                        <TR>
											<TD style="WIDTH: 71px"><asp:imagebutton id="btnIncluir" runat="server" Width="71px" Height="25px" ImageUrl="../pdpw/images/bt_incluir.gif"></asp:imagebutton></TD>											    
										</TR>
                                        <tr>
										    <td style="HEIGHT: 5px" colSpan="2"></td>
									    </tr>
                                    </table>
                                    <!-- -->

                                    </td>
									</tr>   
                                    </table>
                                                                                                         
                                    </td>
									</tr>   
                                                                                                         
                                    <tr height="30">
										<td style="WIDTH: 150px; HEIGHT: 42px" align="right">&nbsp;</td>
										<td style="WIDTH: 565px; HEIGHT: 42px"><asp:ImageButton id="btnExecutar" runat="server" ImageUrl="images/bt_executar.gif"></asp:ImageButton>&nbsp;&nbsp;
                                            <asp:Label ID="btnAguarde" runat="server" Text="Por favor, aguarde..." Visible="False"></asp:Label>
                                        </td>
									</tr>									
								</table>
							</form>
						</DIV>
					</TD>
				</TR>
			</TBODY>
		</TABLE>
</asp:Content>