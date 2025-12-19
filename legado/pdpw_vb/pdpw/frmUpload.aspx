<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" Codebehind="frmUpload.aspx.vb" Inherits="pdpw.frmUpload"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<title>ONS - Operador Nacional do Sistema Elétrico</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
        <script language="javascript" type="text/javascript" src="js/MSGAguarde.js"></script>
        <link href="css/MSGAguarde.css" rel="stylesheet" />
	    <style type="text/css">
            .auto-style1 {
                width: 91px;
                height: 19px;
            }
            .auto-style2 {
                height: 19px;
            }
        </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<%--	<TABLE style="Z-INDEX: 101; LEFT: 0px; WIDTH: 803px; POSITION: absolute; TOP: 0px; HEIGHT: 304px"
			height="304" cellSpacing="0" cellPadding="0" width="803" border="0">--%>
	    <table style="width: 803px; height: 304px" height="304" cellspacing="0" cellpadding="0"
        width="784" border="0">
			<TBODY>
				<TR>
					<TD style="WIDTH: 15px" vAlign="top" width="15"><BR>
					</TD>
					<TD vAlign="top">
						<DIV align="center">
							<TABLE cellSpacing="0" cellPadding="0" width="100%" border="0">
								<TBODY>
									<TR>
										<TD width="20%" height="4" style="HEIGHT: 4px">&nbsp;</TD>
									</TR>
									<TR>
										<TD style="HEIGHT: 17px" height="17">
											<TABLE cellSpacing="0" cellPadding="0" width="789" background="../pdpw/images/back_tit_sistema.gif"
												border="0" style="WIDTH: 789px; HEIGHT: 25px">
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
											<TABLE cellSpacing="0" cellPadding="0" width="789" background="../pdpw/images/back_titulo.gif"
												border="0" style="WIDTH: 789px; HEIGHT: 23px">
												<TBODY>
													<TR>
														<TD style="HEIGHT: 8px">
															<DIV align="left"><IMG height="23" src="../pdpw/images/tit_Upload.gif" width="88" style="WIDTH: 88px; HEIGHT: 23px"></DIV>
														</TD>
													</TR>
												</TBODY>
											</TABLE>
										</TD>
									</TR>
								</TBODY>
							</TABLE>
							<form id="frmArquivo" name="frmArquivo" method="post" enctype="multipart/form-data" runat="server">
								<TABLE id="Table1" style="WIDTH: 535px; HEIGHT: 153px" cellSpacing="1" cellPadding="1"
									width="535" border="0">
									<TR>
										<TD align="center" class="auto-style2" colspan="2">

                                    <asp:Label ID="lblMsg" Visible="False" ForeColor="Green" runat="server" Font-Bold="True" Font-Size="X-Small">Desculpe, este dado não pôde ser carregado no momento.</asp:Label>
                                    
								        </TD>
									</TR>
									<TR>
										<TD align="right" class="auto-style1">
											<asp:label id="lblDataPdp" runat="server" Font-Size="X-Small" Font-Bold="True" Font-Name="Arial"
												Font-Names="Arial">Data do PDP:</asp:label></TD>
										<TD class="auto-style2">
											<asp:dropdownlist id="drpDownDataPDP" runat="server" Font-Size="X-Small" Font-Names="Arial" AutoPostBack="True"
												Width="140px"></asp:dropdownlist></TD>
									</TR>
									<TR>
										<TD style="WIDTH: 91px; HEIGHT: 16px" align="right">
											<asp:label id="lblEmpresa" runat="server" Font-Size="X-Small" Font-Bold="True" Font-Name="Arial">Empresa:</asp:label></TD>
										<TD style="HEIGHT: 16px">
											<asp:dropdownlist id="drpDownEmpresa" runat="server" Font-Size="X-Small" Font-Names="Arial" AutoPostBack="True"
												Width="140px"></asp:dropdownlist></TD>
									</TR>
									<TR>
										<TD align="right" style="WIDTH: 91px"><asp:label id="lblArquivo" runat="server" Font-Name="Arial" Font-Bold="True" Font-Size="X-Small">Arquivo:</asp:label></TD>
										<TD><INPUT class="formulario_texto" onkeypress="return false;" id="fl_Arquivo" style="WIDTH: 434px; HEIGHT: 22px"
												type="file" onchange="return false;" size="53" name="fl_Arquivo" runat="server"></TD>
									</TR>
									<TR>
										<TD align="right" style="WIDTH: 91px"></TD>
										<TD align="center"><asp:imagebutton id="btnConfirmar" runat="server" ImageUrl="images/bt_confirmar.gif"></asp:imagebutton>
											<P>
                                                <asp:Label runat="server" ID="lblInformacao" Text="" ForeColor="Red"></asp:Label>
											</P>
										</TD>
									</TR>
								</TABLE>
							</form>
						</DIV>
					</TD>
				</TR>
			</TBODY>
		</TABLE>
</asp:Content>

