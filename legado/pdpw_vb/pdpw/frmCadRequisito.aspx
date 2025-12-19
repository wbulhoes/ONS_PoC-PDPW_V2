<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" Codebehind="frmCadRequisito.aspx.vb" Inherits="pdpw.frmCadRequisito"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
		<title>ONS - Operador Nacional do Sistema Elétrico</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="images/style.css" rel="stylesheet">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<table style="WIDTH: 528px; HEIGHT: 320px" height="320" cellSpacing="0" cellPadding="0"
			width="528" border="0">
			<tbody>
				<tr>
					<td style="WIDTH: 865px; HEIGHT: 116px" vAlign="top" width="865"><BR>
					</td>
					<td style="WIDTH: 513px; HEIGHT: 116px" vAlign="top">
						<div align="center">
							<table style="WIDTH: 503px; HEIGHT: 105px" cellSpacing="0" cellPadding="0" width="503"
								border="0">
								<tbody>
									<tr>
										<td style="WIDTH: 1063px; HEIGHT: 18px" width="1063" height="18">&nbsp;</td>
									</tr>
									<tr>
										<td style="WIDTH: 1063px" height="2">
											<table style="WIDTH: 510px; HEIGHT: 25px" cellSpacing="0" cellPadding="0" width="510" background="images/back_tit_sistema.gif"
												border="0">
												<tbody>
													<tr>
														<td><IMG style="WIDTH: 179px; HEIGHT: 25px" height="25" src="images/tit_sis_guideline.gif"
																width="179"></td>
													</tr>
												</tbody>
											</table>
										</td>
									</tr>
									<tr>
										<td style="WIDTH: 1063px; HEIGHT: 26px" height="26">
											<table style="WIDTH: 510px; HEIGHT: 23px" cellSpacing="0" cellPadding="0" width="510" background="images/back_titulo.gif"
												border="0">
												<tbody>
													<tr>
														<td>
															<div align="left"><IMG style="WIDTH: 192px; HEIGHT: 23px" height="23" src="images/tit_CadRequisito.gif"
																	width="192"></div>
														</td>
													</tr>
												</tbody>
											</table>
										</td>
									</tr>
								</tbody>
							</table>
						</div>
					</td>
				</tr>
				<tr>
					<td style="WIDTH: 865px" vAlign="top" width="865"><BR>
					</td>
					<td style="WIDTH: 513px">
						<form id="frmCadRequisito" name="frmCadRequisito" method="post" encType="multipart/form-data"
							runat="server">
							<table class="modulo" style="WIDTH: 99.82%; HEIGHT: 125px" cellSpacing="0" cellPadding="0"
								border="0">
								<tr>
									<td align="right" style="WIDTH: 270px; HEIGHT: 29px">
										Data do PDP:&nbsp;
									</td>
									<td style="WIDTH: 307px; HEIGHT: 29px">
										<asp:DropDownList id="cboData" runat="server" Width="96px" AutoPostBack="True"></asp:DropDownList>
									</td>
								</tr>
								<tr>
									<td align="right" style="WIDTH: 270px; HEIGHT: 25px">
										Requisito Máximo:&nbsp;
									</td>
									<td align="left" style="WIDTH: 307px; HEIGHT: 25px">
										<asp:textbox id="txtVlrRequisito" runat="server" Width="47px"></asp:textbox>&nbsp;MW
										<asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" ErrorMessage="Requisito Máximo Requerido"
											ControlToValidate="txtVlrRequisito">*</asp:RequiredFieldValidator>
										&nbsp;&nbsp;/&nbsp;&nbsp;&nbsp;
										<asp:textbox id="txtHorRequisito" runat="server" Width="42px"></asp:textbox>&nbsp;HS
										<asp:RequiredFieldValidator id="RequiredFieldValidator3" runat="server" ControlToValidate="txtHorRequisito"
											ErrorMessage="Hora do Requisito Requerida">*</asp:RequiredFieldValidator>
									</td>
								</tr>
								<tr>
									<td align="right" style="WIDTH: 270px; HEIGHT: 30px">
										Reserva Mín. No Req. Máx.:&nbsp;
									</td>
									<td align="left" style="WIDTH: 307px; HEIGHT: 30px">
										<asp:textbox id="txtVlrReserva" runat="server" Width="47px"></asp:textbox>&nbsp;MW
										<asp:RequiredFieldValidator id="RequiredFieldValidator2" runat="server" ErrorMessage="Valor da Reserva Requerida"
											ControlToValidate="txtVlrReserva">*</asp:RequiredFieldValidator>
										&nbsp;&nbsp;/&nbsp;&nbsp;&nbsp;
										<asp:textbox id="txtHorReserva" runat="server" Width="42px"></asp:textbox>&nbsp;HS
										<asp:RequiredFieldValidator id="RequiredFieldValidator4" runat="server" ControlToValidate="txtHorReserva" ErrorMessage="Hora da Reserva Requerida">*</asp:RequiredFieldValidator>
									</td>
								</tr>
								<tr align="center">
									<td align="center" valign="middle" colspan="2" style="WIDTH: 581px">
										<asp:imagebutton id="btnSalvar" runat="server" ImageUrl="images/bt_salvar.gif"></asp:imagebutton>&nbsp;&nbsp;&nbsp;
										<IMG onmouseover="this.style.cursor='hand'" onclick="javascript:window.close();" alt=""
											src="images\bt_fechar.gif">
									</td>
								</tr>
								<tr>
									<td colspan="2" align="center" style="WIDTH: 581px">
										<br>
										<asp:Label id="lblMensagem" runat="server" Width="504px" Height="18px" Font-Size="Small" ForeColor="Red"></asp:Label>
									</td>
								</tr>
								<asp:ValidationSummary id="ValidationSummary1" runat="server" Width="480px" ShowMessageBox="True" ShowSummary="False"
									Height="41px"></asp:ValidationSummary>
							</table>
						</form>
					</td>
				</tr>
			</tbody>
		</table>
</asp:Content>