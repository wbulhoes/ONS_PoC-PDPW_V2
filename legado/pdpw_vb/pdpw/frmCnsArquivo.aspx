<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" Codebehind="frmCnsArquivo.aspx.vb" Inherits="pdpw.frmCnsArquivo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
		<title>ONS - Operador Nacional do Sistema Elétrico</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<meta http-equiv="Content-Type" content="text/html; charset=windows-1252">
		<LINK href="../pdpw/images/style.css" rel="stylesheet">
        <script language="javascript" type="text/javascript" src="js/MSGAguarde.js"></script>
        <link href="css/MSGAguarde.css" rel="stylesheet" />
	    <style type="text/css">
            .auto-style1 {
                height: 38px;
            }
        </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
			<TABLE style="WIDTH: 788px; HEIGHT: 418px" height="418" cellSpacing="0" cellPadding="0"
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
														<TD style="HEIGHT: 12px"><IMG height="25" src="../pdpw/images/tit_sis_guideline.gif" width="179"></TD>
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
															<DIV align="left"><IMG height="23" src="../pdpw/images/tit_CnsArquivo.gif" width="192" style="WIDTH: 192px; HEIGHT: 23px"></DIV>
														</TD>
													</TR>
												</TBODY>
											</TABLE>
										</TD>
									</TR>
								</TBODY>
							</TABLE>
							<form id="frmArquivo" name="frmArquivo" runat="server">
								<table class="modulo" style="WIDTH: 295px; HEIGHT: 60px" cellSpacing="0" cellPadding="0"
									border="0">
									<tr>
										<td align="center" class="auto-style1" colspan="2">

                                    <asp:Label ID="lblMsg" Visible="False" ForeColor="Green" runat="server" Font-Size="X-Small" Font-Bold="True">Desculpe, este dado não pôde ser carregado no momento.</asp:Label>
                                    
							                <br />
										</td>
										<td rowspan="3">
											<asp:imagebutton id="btnPesquisar" runat="server" ImageAlign="Top" ImageUrl="images/bt_pesquisar.gif"></asp:imagebutton>
										</td>
									</tr>
									<tr>
										<td align="right">Data PDP:
										</td>
										<td style="WIDTH: 190px">&nbsp;
											<asp:dropdownlist id="cboDataPdp" runat="server" Width="148px"></asp:dropdownlist></td>
									</tr>
									<tr>
										<td align="right">Empresa:
										</td>
										<td style="WIDTH: 190px">&nbsp;
											<asp:dropdownlist id="cboEmpresa" runat="server" Width="148px"></asp:dropdownlist></td>
									</tr>
								</table>
								<table style="WIDTH: 505px; HEIGHT: 53px" cellSpacing="0" cellPadding="0" border="0">
									<tr vAlign="top" align="center">
										<td style="WIDTH: 539px; HEIGHT: 8px"><br>
											<asp:table id="tblArquivo" runat="server" Width="511px" Font-Size="X-Small" CellSpacing="0"></asp:table></td>
									</tr>
								</table>
							</form>
						</DIV>
					</TD>
				</TR>
			</TBODY>
		</TABLE>
</asp:Content>
