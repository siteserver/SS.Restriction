<%@ Page Language="C#" Inherits="SS.Restriction.Pages.PageOptions" %>
	<!DOCTYPE html>
	<html>

	<head>
		<meta charset="utf-8">
		<link href="assets/plugin-utils/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
		<link href="assets/plugin-utils/css/plugin-utils.css" rel="stylesheet" type="text/css" />
		<link href="assets/plugin-utils/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
	</head>

	<body>
		<div style="padding: 20px 0;">

			<div class="container">
				<form id="form" runat="server" class="form-horizontal">

					<div class="row">
						<div class="card-box">
							<div class="row">
								<div class="col-lg-10">
									<h4 class="m-t-0 header-title">
										<b>后台访问限制配置</b>
									</h4>
									<p class="text-muted font-13 m-b-30">
										在此配置管理后台的访问限制选项
									</p>
								</div>
							</div>

							<asp:Literal id="LtlMessage" runat="server" />

							<div class="row">

								<div class="form-group">
									<label class="col-sm-3 control-label">访问Ip地址限制</label>
									<div class="col-sm-9">
										<asp:RadioButtonList ID="RblIpRestrictionType" class="radio radio-primary" RepeatDirection="Vertical" runat="server"></asp:RadioButtonList>
									</div>
								</div>

								<div class="form-group">
									<label class="col-sm-3 control-label">管理后台域名限制</label>
									<div class="col-sm-6">
										<asp:DropDownList ID="DdlIsHostRestriction" AutoPostBack="true" OnSelectedIndexChanged="DdlIsHostRestriction_SelectedIndexChanged"
										  class="form-control" runat="server"></asp:DropDownList>
									</div>
									<div class="col-sm-3"></div>
								</div>

								<asp:PlaceHolder id="PhHost" runat="server">
									<div class="form-group">
										<label class="col-sm-3 control-label">管理后台访问域名</label>
										<div class="col-sm-6">
											<asp:TextBox ID="TbHost" class="form-control" runat="server"></asp:TextBox>
											<span class="help-block">如果非80端口，后台访问域名需要包含。</span>
										</div>
										<div class="col-sm-3">
											
										</div>
									</div>
								</asp:PlaceHolder>

								<div class="form-group m-b-0">
									<div class="col-sm-offset-3 col-sm-9">
										<asp:Button class="btn btn-primary" id="Submit" text="确 定" onclick="Submit_OnClick" runat="server" />
									</div>
								</div>

							</div>
						</div>
					</div>

				</form>
			</div>
		</div>
	</body>

	</html>