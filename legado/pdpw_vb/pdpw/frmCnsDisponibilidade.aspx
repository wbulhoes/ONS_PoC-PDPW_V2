<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" Codebehind="frmCnsDisponibilidade.aspx.vb" Inherits="pdpw.frmCnsDisponibilidade"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<title>ONS - Operador Nacional do Sistema Elétrico</title>
		<META http-equiv="Content-Type" content="text/html; charset=windows-1252">
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<LINK href="../pdpw/images/style.css" rel="stylesheet">
		<script language="javascript" src="Lib.js"></script>
	    <style type="text/css">
            .auto-style1 {
                width: 108px;
                height: 80px;
            }
            .auto-style2 {
                width: 306px;
                height: 80px;
            }
            .auto-style3 {
                height: 80px;
            }
            .auto-style4 {
                width: 85px;
                height: 80px;
            }
            .auto-style5 {
                text-align: left;
            }
            .auto-style6 {
                height: 7px;
                width: 306px;
            }
            .auto-style7 {
                width: 306px;
            }
            .auto-style8 {
                width: 108px;
                height: 26px;
            }
            .auto-style9 {
                width: 306px;
                height: 26px;
            }
            .auto-style10 {
                height: 26px;
            }
            .auto-style11 {
                width: 85px;
                height: 26px;
            }
            .auto-style12 {
                FONT-WEIGHT: normal;
                FONT-SIZE: 10px;
                COLOR: #000000;
                FONT-STYLE: normal;
                FONT-FAMILY: Arial, Helvetica, sans-serif;
                width: 311px;
                height: 59px;
                margin-bottom: 0px;
            }
            .auto-style13 {
                width: 93px;
                height: 21px;
            }
            .auto-style14 {
                width: 209px;
                height: 21px;
            }
            .auto-style15 {
                left: 80px;
                width: 635px;
                position: absolute;
                top: 402px;
                height: 36px;
            }
        </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table style="WIDTH: 792px; HEIGHT: 249px" height="249" cellSpacing="0" cellPadding="0"
			width="792" border="0">
			<tr>
				<td style="WIDTH: 17px" width="17"><br>
				</td>
				<td style="HEIGHT: 248px" vAlign="top">
					<div align="center">
						<table cellSpacing="0" cellPadding="0" width="100%" border="0">
							<tr>
								<TD width="20%" height="2">&nbsp;</TD>
							</tr>
							<tr>
								<td style="HEIGHT: 17px" height="17">
									<table style="WIDTH: 772px; HEIGHT: 25px" cellSpacing="0" cellPadding="0" width="772" background="../pdpw/images/back_tit_sistema.gif"
										border="0">
										<tr>
											<td style="HEIGHT: 12px"><script>MontaCabecalho();</script></td>
										</tr>
									</table>
								</td>
							</tr>
							<tr>
								<td style="HEIGHT: 10px" height="10">
									<table style="WIDTH: 771px; HEIGHT: 27px" cellSpacing="0" cellPadding="0" width="771" background="images/back_titulo.gif"
										border="0">
										<tr>
											<td style="HEIGHT: 8px">
												<div align="left"><IMG style="WIDTH: 152px; HEIGHT: 23px" height="23" src="images/tit_ColDSP.gif" width="152">
												</div>
											</td>
										</tr>
									</table>
								</td>
							</tr>
						</table>
						<form id="frmCnsDisponibilidade" name="frmCnsDisponibilidade" runat="server">

                                    <asp:Label ID="lblMsg" Visible="False" ForeColor="Green" runat="server" Font-Bold="True" Font-Size="X-Small">Desculpe, este dado não pôde ser carregado no momento.</asp:Label>
						<BR>
							<div id="divtabela" style="LEFT: 80px; WIDTH: 599px; POSITION: absolute; TOP: 120px; HEIGHT: 134px">
								<table class="modulo" style="WIDTH: 606px; HEIGHT: 115px" cellSpacing="0" cellPadding="0"
									border="0">
									<tr vAlign="top">
										<td style="WIDTH: 108px; HEIGHT: 7px"><%if Request.QueryString("strAcesso") <> "PDOC" Then
                                                                                      Response.Write("Dados:")
                                                                                  End If%></td>
										<td class="auto-style6">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Selecionar:</td>
										<td style="HEIGHT: 7px"></td>
										<td style="WIDTH: 85px"></td>
									</tr>
									<tr vAlign="top">
										<td class="auto-style8"></td>
										<td class="auto-style9">
                                            <asp:Label ID="lblUsinaSelecionada" runat="server" Font-Bold="True" Font-Size="Medium" ForeColor="#FF3300" Visible="False" Width="533px"	></asp:Label>
                                        </td>
										<td class="auto-style10"></td>
										<td class="auto-style11"></td>
									</tr>
									<tr vAlign="top">
										<td class="auto-style1">
                                            &nbsp;</td>
										<td class="auto-style2">
                                            <asp:Panel ID="Panel2" runat="server" BorderColor="#BFDEB6" BorderStyle="Solid" BorderWidth="2px" Height="74px" Width="404px">
                                                <div class="auto-style5">
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:Label ID="Label2" runat="server" Font-Bold="True" Text="Selecione o Tipo de Usina para consultar os dados"></asp:Label>
                                                    <br />
                                                    <br />
													&nbsp;&nbsp;
                                                   <asp:RadioButton ID="OptTermica" runat="server" GroupName="1" Text="Usinas Térmicas" AutoPostBack="True" OnCheckedChanged="OptTermica_CheckedChanged" />
													<asp:RadioButton ID="OptHidraulica" runat="server" GroupName="1" Text="Usinas Hidráulicas" AutoPostBack="True" OnCheckedChanged="OptHidraulica_CheckedChanged" />
                                                    &nbsp;&nbsp;&nbsp; &nbsp;<asp:ImageButton ID="btnSelecionarUsina" runat="server" ImageUrl="~/images/bt_confirmar.gif" />
                                                </div>
                                            </asp:Panel>
                                        </td>
										<td class="auto-style3"></td>
										<td class="auto-style4"></td>
									</tr>
									<tr>
										<td style="WIDTH: 108px"><asp:radiobuttonlist id="optDados" runat="server" Width="118px" Font-Size="XX-Small" AutoPostBack="True">
													<asp:ListItem Value="0" Selected="True">&#193;rea Transfer&#234;ncia</asp:ListItem>
													<asp:ListItem Value="1">Enviados</asp:ListItem>
													<asp:ListItem Enabled="false" Value="2">Consolidados</asp:ListItem>
													<asp:ListItem Value="3">Recebidos DESSEM</asp:ListItem>
													<asp:ListItem Value="4">Consistidos DESSEM</asp:ListItem>
											</asp:radiobuttonlist></td>
										<td class="auto-style7">
											<table class="auto-style12" cellSpacing="0" cellPadding="0"
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
												<tr>
													<td style="WIDTH: 93px" align="right" class="auto-style13">Usinas&nbsp; </td>
													<td class="auto-style14"><asp:DropDownList ID="cboUsina" runat="server" AutoPostBack="True" Enabled="False" Width="199px">
                                                        </asp:DropDownList>
                                                    </td>
												</tr>
											</table>
											<br>
											<br>
											<asp:label id="lblMensagem" runat="server" Visible="False"></asp:label></td>
										<td align="center"><asp:imagebutton id="btnVisualizar" runat="server" ImageUrl="images/bt_visualizar.gif"></asp:imagebutton></td>
										<td align="center" style="WIDTH: 85px"><IMG id="imgPlanilha" onmouseover="this.style.cursor='hand'" onclick="javascript:botao();"
												alt="" src="images/bt_planilha.gif" runat="server"></td>
									</tr>
								</table>
							</div>
						</form>
					</div>
				</td>
			</tr>
		</table>
		<div style="DISPLAY: inline; "
			align="left" ms_positioning="FlowLayout" class="auto-style15"><asp:table id="tblConsulta" runat="server" Width="609px" Font-Size="X-Small" CellSpacing="0"></asp:table></div>
</asp:Content>
