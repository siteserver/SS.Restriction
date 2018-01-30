using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SiteServer.Plugin;
using SS.Restriction.Model;

namespace SS.Restriction.Pages
{
    public class PageList : Page
    {
        public Literal LtlTitle;
        public Repeater RptContents;
        public TextBox TbRestriction;
        public Button BtnAdd;
        public Literal LtlModalTitle;
        public Literal LtlModalMessage;
        public Literal LtlScript;

        private string _type;
        private Config _config;

        public void Page_Load(object sender, EventArgs e)
        {
            if (!Main.Instance.AdminApi.IsPluginAuthorized)
            {
                HttpContext.Current.Response.Write("<h1>未授权访问</h1>");
                HttpContext.Current.Response.End();
                return;
            }

            _type = Request.QueryString["type"];
            _config = Main.GetConfig();

            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["delete"]))
                {
                    var restriction = Request.QueryString["restriction"];

                    if (_type == "Black")
                    {
                        var list = new List<string>(_config.IpBlackList.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries));
                        list.Remove(restriction);
                        _config.IpBlackList = string.Join(",", list.ToArray());
                    }
                    else
                    {
                        var list = new List<string>(_config.IpWhiteList.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries));
                        list.Remove(restriction);
                        _config.IpWhiteList = string.Join(",", list.ToArray());
                    }
                    Main.Instance.SetConfig(_config);
                }
                else if (!string.IsNullOrEmpty(Request.QueryString["edit"]))
                {
                    var restriction = Request.QueryString["restriction"];

                    LtlModalTitle.Text = "编 辑";
                    TbRestriction.Text = restriction;

                    LtlScript.Text = @"<script>
setTimeout(function() {
    $('#modal').modal();
}, 100);
</script>";
                }

                LtlTitle.Text = _type == "Black" ? "访问限制IP地址黑名单" : "访问限制IP地址白名单";

                var theList = _type == "Black" ? _config.IpBlackList : _config.IpWhiteList;
                RptContents.DataSource = new List<string>(theList.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries));
                RptContents.ItemDataBound += RptContents_ItemDataBound;
                RptContents.DataBind();

                LtlModalTitle.Text = "添 加";
                BtnAdd.Attributes.Add("onclick", "$('#modal').modal();return false;");
                //AddButton.Attributes.Add("onclick", ModalRestrictionAdd.GetOpenWindowStringToAdd(0, _type));
            }
        }

        private void RptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;

            var restriction = e.Item.DataItem as string;
            var ltlItem = (Literal)e.Item.FindControl("ltlItem");
            var ltlEdit = (Literal)e.Item.FindControl("ltlEdit");
            var ltlDelete = (Literal) e.Item.FindControl("ltlDelete");

            ltlItem.Text = restriction;
            ltlEdit.Text = $@"<a href=""PageList.aspx?v={new Random().Next(1000)}&edit={true}&type={_type}&restriction={restriction}"" class=""btn btn-success waves-effect waves-light btn-sm"">编 辑</a>";
            ltlDelete.Text =
                $@"<a href=""PageList.aspx?v={new Random().Next(1000)}&delete={true}&type={_type}&restriction={restriction}"" onClick=""javascript:return confirm('此操作将删除IP访问规则“{restriction}”，确认吗？');"" class=""btn btn-danger waves-effect waves-light btn-sm"">删 除</a>";
        }

        //private static void MyDataGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
        //{
        //    if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;

        //    var restriction = e.Item.DataItem as string;
        //    var listItem = (Literal)e.Item.FindControl("ListItem");
        //    //var editUrl = e.Item.FindControl("EditUrl") as Literal;
        //    //var deleteUrl = e.Item.FindControl("DeleteUrl") as Literal;
        //    listItem.Text = restriction;

        //    //var showPopWinString = ModalRestrictionAdd.GetOpenWindowStringToEdit(0, _type, restriction);
        //    //editUrl.Text = $"<a href=\"javascript:;\" onClick=\"{showPopWinString}\">修改</a>";

        //    //var urlDelete = PageUtils.GetPluginsUrl(nameof(PageRestrictionList), new NameValueCollection
        //    //{
        //    //    {"Delete", "True"},
        //    //    {"Type", _type},
        //    //    {"Restriction", restriction}
        //    //});

        //    //deleteUrl.Text =
        //    //    $"<a href=\"{urlDelete}\" onClick=\"javascript:return confirm('此操作将删除IP访问规则“{restriction}”，确认吗？');\">删除</a>";
        //}

        public void Submit_OnClick(object sender, EventArgs e)
        {
            var restriction = _type == "Black" ? _config.IpBlackList : _config.IpWhiteList;

            var list = new List<string>(restriction.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries));

            if (!string.IsNullOrEmpty(Request.QueryString["edit"]))
            {
                var origin = Request.QueryString["restriction"];
                for (var i = 0; i < list.Count; i++)
                {
                    var res = list[i];
                    if (res == origin)
                    {
                        list[i] = TbRestriction.Text;
                    }
                }

                if (_type == "Black")
                {
                    _config.IpBlackList = string.Join(",", list.ToArray());
                }
                else
                {
                    _config.IpWhiteList = string.Join(",", list.ToArray());
                }

                Main.Instance.SetConfig(_config);

                LtlScript.Text =
                    $"<script>location.href='PageList.aspx?v={new Random().Next(1000)}&type={_type}'</script>";
            }
            else
            {
                if (list.Contains(TbRestriction.Text))
                {
                    LtlModalMessage.Text =
                        @"<div class=""alert alert-danger"" role=""alert"">IP访问规则添加失败，IP访问规则已存在！</div>";
                    LtlScript.Text = @"<script>
setTimeout(function() {
    $('#modal').modal();
}, 100);
</script>";
                }
                else
                {
                    list.Add(TbRestriction.Text);
                    if (_type == "Black")
                    {
                        _config.IpBlackList = string.Join(",", list.ToArray());
                    }
                    else
                    {
                        _config.IpWhiteList = string.Join(",", list.ToArray());
                    }

                    Main.Instance.SetConfig(_config);

                    LtlScript.Text =
                        $"<script>location.href='PageList.aspx?v={new Random().Next(1000)}&type={_type}'</script>";
                }
            }
        }
    }
}