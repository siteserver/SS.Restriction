<%@ Page Language="C#" Inherits="SS.Restriction.Pages.PageList" %>
<!DOCTYPE html>
<html>

<head>
  <meta charset="utf-8">
  <link href="assets/plugin-utils/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
  <link href="assets/plugin-utils/css/plugin-utils.css" rel="stylesheet" type="text/css" />
  <link href="assets/plugin-utils/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
</head>

<body>

  <div class="container">

    <div class="row">
      <div class="col-sm-12">
        <div class="card-box">

          <form class="form-inline" runat="server">

            <h4 class="m-t-0 header-title">
              <b>
            <asp:Literal id="LtlTitle" runat="server" />
          </b>
            </h4>
            <p class="text-muted m-b-30 font-13">
              设置访问限制后请重新登录后台查看效果
            </p>

            <table class="table">
              <thead>
                <tr>
                  <th style="width:70%;">IP地址</th>
                  <th>操作</th>
                </tr>
              </thead>
              <tbody>
                <asp:Repeater ID="RptContents" runat="server">
                  <itemtemplate>
                    <tr>
                      <td class="middle-align">
                        <asp:Literal id="ltlItem" runat="server"></asp:Literal>
                      </td>
                      <td>
                        <asp:Literal id="ltlEdit" runat="server"></asp:Literal>
                        <asp:Literal id="ltlDelete" runat="server"></asp:Literal>
                      </td>
                    </tr>
                  </itemtemplate>
                </asp:Repeater>
              </tbody>
            </table>

            <asp:Button id="BtnAdd" runat="server" class="btn btn-primary waves-effect waves-light m-t-10" Text="添 加"></asp:Button>

            <div id="modal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true"
              style="display: none;">
              <div class="modal-dialog" style="width:55%;">
                <div class="modal-content">
                  <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title" id="modalLabel">
                      <asp:Literal id="LtlModalTitle" runat="server"></asp:Literal>
                    </h4>
                  </div>
                  <div class="modal-body">
                    <asp:Literal id="LtlModalMessage" runat="server"></asp:Literal>
                    <p>
                      xxx.xxx.xxx.xxx = 精确匹配
                      <br>
                      xxx.xxx.xxx.xxx-xxx.xxx.xxx.xxx = 范围
                      <br>
                      xxx.xxx.xxx.* = 任何匹配
                    </p>
                    <hr>

                    <div class="form-horizontal">
                    <div class="form-group">
                        <label class="col-md-3 control-label">IP访问规则</label>
                        <div class="col-md-6">
                            <asp:TextBox class="form-control" Columns="50" MaxLength="50" id="TbRestriction" runat="server" />
                            <asp:RequiredFieldValidator ControlToValidate="TbRestriction" errorMessage=" *" foreColor="red" display="Dynamic" runat="server" />
                            <asp:RegularExpressionValidator runat="server" ControlToValidate="TbRestriction" ValidationExpression="[\d{0,3}\.\*-]+" errorMessage=" *" foreColor="red" display="Dynamic" />
                        </div>
                    </div>
                    </div>

                  </div>
                  <div class="modal-footer">
                    <button type="button" class="btn btn-default waves-effect" data-dismiss="modal">取 消</button>
                    <asp:Button class="btn btn-primary waves-effect waves-light" onclick="Submit_OnClick" runat="server" Text="保 存"></asp:Button>
                  </div>
                </div>
                <!-- /.modal-content -->
              </div>
              <!-- /.modal-dialog -->
            </div>
            <!-- /.modal -->
            
            <asp:Literal id="LtlScript" runat="server"></asp:Literal>

          </form>

        </div>

      </div>
    </div>
    <!-- end row -->


  </div>


</body>

</html>
<script src="assets/plugin-utils/js/jquery.min.js"></script>
<script src="assets/plugin-utils/js/bootstrap.min.js"></script>