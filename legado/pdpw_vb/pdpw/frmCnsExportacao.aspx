<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" Codebehind="frmCnsExportacao.aspx.vb" Inherits="pdpw.frmCnsExportacao"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <title>ONS - Operador Nacional do Sistema Elétrico</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio.NET 7.0">
		<meta name="CODE_LANGUAGE" content="Visual Basic 7.0">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<LINK href="../pdpw/images/style.css" rel="stylesheet">
		<script language="javascript" src="Lib.js"></script>
        <style type="text/css">
            .auto-style1 {
                left: 76px;
                width: 634px;
                position: absolute;
                top: 312px;
                height: 36px;
            }
        </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
            <TABLE style="WIDTH: 788px; HEIGHT: 232px" height="232" cellSpacing="0" cellPadding="0"
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
															<DIV align="left"><IMG height="23" src="../pdpw/images/tit_ColExportacao.gif" width="120" style="WIDTH: 120px; HEIGHT: 23px"></DIV>
														</TD>
													</TR>
												</TBODY>
											</TABLE>
										</TD>
									</TR>
								</TBODY>
							</TABLE>
							<BR>
							<form id="frmCnsExportacao" name="frmCnsExportacao" runat="server">
								<div id="divtabela" style="LEFT: 80px; WIDTH: 580px; POSITION: absolute; TOP: 120px; HEIGHT: 127px">
									<table class="modulo" style="WIDTH: 614px; HEIGHT: 108px" cellSpacing="0" cellPadding="0"
										border="0">
										<tr vAlign="top">
											<TD style="WIDTH: 123px; HEIGHT: 1px"><%if request.querystring("strAcesso") <> "PDOC" then 
																						response.write("Dados:") 
																				    end if%></TD>
											<td style="WIDTH: 306px; HEIGHT: 1px">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
												Selecionar:</td>
											<td style="HEIGHT: 1px"></td>
											<td style="WIDTH: 92px"></td>
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
												<table class="formulario_texto" style="WIDTH: 311px; HEIGHT: 52px" cellSpacing="0" cellPadding="0"
													border="0">
													<tr>
														<td style="WIDTH: 93px; HEIGHT: 21px" align="right">
															<asp:Label style="Z-INDEX: 0" id="lblData" runat="server">Data do PDP</asp:Label>&nbsp;</td>
														<td style="WIDTH: 209px; HEIGHT: 21px"><asp:dropdownlist id="cboDataInicial" runat="server" Width="90px" AutoPostBack="True"></asp:dropdownlist>&nbsp;</td>
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
											<td align="center" style="WIDTH: 92px"><IMG id="imgPlanilha" onmouseover="this.style.cursor='hand'" onclick="javascript:botao();"
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
			ms_positioning="FlowLayout" align="left" class="auto-style1">
			<asp:table id="tblConsulta" runat="server" Font-Size="X-Small" Width="607px" CellSpacing="0"></asp:table></DIV>
</asp:Content>
