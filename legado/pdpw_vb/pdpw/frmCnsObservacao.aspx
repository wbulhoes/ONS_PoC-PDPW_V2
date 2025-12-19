<%@ Page Language="vb" AutoEventWireup="false" Codebehind="frmCnsObservacao.aspx.vb" Inherits="pdpw.frmCnsObservacao"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>ONS - Operador Nacional do Sistema Elétrico</title>
		<META http-equiv="Content-Type" content="text/html; charset=windows-1252">
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../pdpw/images/style.css" rel="stylesheet">
	</HEAD>
	<body bgColor="#ffffff" leftMargin="0" topMargin="0">
		<table style="WIDTH: 788px; HEIGHT: 418px" height="418" cellSpacing="0" cellPadding="0"
			width="788" border="0">
			<tbody>
				<tr>
					<td style="WIDTH: 19px" vAlign="top" width="19"><br>
					</td>
					<td vAlign="top">
						<div align="center">
							<table cellSpacing="0" cellPadding="0" width="100%" border="0">
								<tbody>
									<tr>
										<td width="20%" height="2">&nbsp;</td>
									</tr>
									<tr>
										<td style="HEIGHT: 17px" height="17">
											<table cellSpacing="0" cellPadding="0" width="765" background="../pdpw/images/back_tit_sistema.gif"
												border="0">
												<tbody>
													<tr>
														<td style="HEIGHT: 12px"><img height="25" src="../pdpw/images/tit_sis_guideline.gif" width="179"></td>
													</tr>
												</tbody>
											</table>
										</td>
									</tr>
									<tr>
										<td style="HEIGHT: 10px" height="10">
											<table cellSpacing="0" cellPadding="0" width="765" background="../pdpw/images/back_titulo.gif"
												border="0">
												<tbody>
													<tr>
														<td style="HEIGHT: 8px">
															<div align="left"><img height="23" src="../pdpw/images/tit_Observacao.gif" width="136" style="WIDTH: 136px; HEIGHT: 23px"></div>
														</td>
													</tr>
												</tbody>
											</table>
										</td>
									</tr>
								</tbody>
							</table>
							<form id="frmArquivo" name="frmArquivo" runat="server">
								<table class="modulo" style="WIDTH: 295px; HEIGHT: 60px" cellSpacing="0" cellPadding="0"
									border="0">
									<tr>
										<td align="right" style="WIDTH: 111px">Data PDP:
										</td>
										<td style="WIDTH: 147px">&nbsp;
											<asp:dropdownlist id="cboDataPdp" runat="server" Width="97px"></asp:dropdownlist></td>
										<td rowspan="2">
											<asp:imagebutton id="btnPesquisar" runat="server" ImageUrl="images/bt_visualizar.gif" ImageAlign="Top"></asp:imagebutton>
										</td>
									</tr>
								</table>
								<table style="WIDTH: 505px; HEIGHT: 280px" cellSpacing="0" cellPadding="0" border="0">
									<tr vAlign="top" align="center">
										<td style="WIDTH: 539px; HEIGHT: 141px">
											<br>
											<asp:TextBox id="txtObs" runat="server" Width="667px" Height="264px" Columns="1" TextMode="MultiLine"
												ReadOnly="True" BorderStyle="Groove"></asp:TextBox><br>
										</td>
									</tr>
								</table>
							</form>
						</div>
					</td>
				</tr>
			</tbody>
		</table>
	</body>
</HTML>
