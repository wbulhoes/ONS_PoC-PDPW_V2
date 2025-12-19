<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" Codebehind="frmCnsInflexibilidade.aspx.vb" Inherits="pdpw.frmCnsInflexibilidade"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <title>ONS - Operador Nacional do Sistema Elétrico</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<meta http-equiv="Content-Type" content="text/html; charset=windows-1252">
		<LINK href="../pdpw/images/style.css" rel="stylesheet">
		<script language="javascript" src="Lib.js"></script>
        <style type="text/css">
            .auto-style1 {
                left: 81px;
                width: 634px;
                position: absolute;
                top: 312px;
                height: 36px;
            }
        </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
            <TABLE style="WIDTH: 788px; HEIGHT: 234px" height="234" cellSpacing="0" cellPadding="0"
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
															<DIV align="left"><IMG height="23" src="../pdpw/images/tit_ColInflexibilidade.gif" width="160" style="WIDTH: 160px; HEIGHT: 23px"></DIV>
														</TD>
													</TR>
												</TBODY>
											</TABLE>
										</TD>
									</TR>
								</TBODY>
							</TABLE>
							<BR>
							<form id="frmCnsInflexibilidade" name="frmCnsInflexibilidade" runat="server">

                                    <asp:Label ID="lblMsg" Visible="False" ForeColor="Green" runat="server" Font-Bold="True" Font-Size="X-Small">Desculpe, este dado não pôde ser carregado no momento.</asp:Label>
                                    
								<div id="divtabela" style="LEFT: 80px; WIDTH: 597px; POSITION: absolute; TOP: 120px; HEIGHT: 133px">
									<table class="modulo" style="WIDTH: 612px; HEIGHT: 114px" cellSpacing="0" cellPadding="0"
										border="0">
										<tr vAlign="top">
											<TD style="WIDTH: 123px; HEIGHT: 1px"><%if request.querystring("strAcesso") <> "PDOC" then 
																						response.write("Dados:") 
																				    end if%></TD>
											<td style="WIDTH: 306px; HEIGHT: 1px">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
												Selecionar:</td>
											<td style="HEIGHT: 1px"></td>
											<td style="WIDTH: 89px"></td>
										</tr>
										<tr>
											<TD style="WIDTH: 123px">
												<asp:radiobuttonlist id="optDados" runat="server" Font-Size="XX-Small" Width="118px" AutoPostBack="True">
													<asp:ListItem Value="0" Selected="True">&#193;rea Transfer&#234;ncia</asp:ListItem>
													<asp:ListItem Value="1">Enviados</asp:ListItem>
													<asp:ListItem Enabled="false" Value="2">Consolidados</asp:ListItem>
													<asp:ListItem Value="3">Recebidos DESSEM</asp:ListItem>
													<asp:ListItem Value="4">Consistidos DESSEM</asp:ListItem>
												</asp:radiobuttonlist></TD>
											<td style="WIDTH: 306px">
												<table class="formulario_texto" style="WIDTH: 311px; HEIGHT: 58px" cellSpacing="0" cellPadding="0"
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
												<asp:Label id="lblMensagem" runat="server" Visible="False"></asp:Label>
											</td>
											<td align="center">
												<asp:ImageButton id="btnVisualizar" runat="server" ImageUrl="images/bt_visualizar.gif"></asp:ImageButton></td>
											<td align="center" style="WIDTH: 89px"><IMG id="imgPlanilha" onmouseover="this.style.cursor='hand'" onclick="javascript:botao();"
													alt="" src="images/bt_planilha.gif" runat="server"></td>
										</tr>
									</table>
								</div>
							</form>
						</DIV>
					</TD>
				</TR>
			</TBODY>
		</TABLE>
		<DIV style="DISPLAY: inline; "
			align="left" ms_positioning="FlowLayout" class="auto-style1">
			<asp:table id="tblConsulta" runat="server" Font-Size="X-Small" Width="605px" CellSpacing="0"></asp:table></DIV>
</asp:Content>