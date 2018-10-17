using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Restriction.Model;

namespace SS.Restriction.Pages
{
    public class PageOptions : Page
    {
        public Literal LtlMessage;
        public RadioButtonList RblIpRestrictionType;
        public DropDownList DdlIsHostRestriction;
        public PlaceHolder PhHost;
        public TextBox TbHost;

        private Config _config;

        public void Page_Load(object sender, EventArgs e)
        {
            if (!SiteServer.Plugin.Context.Request.AdminPermissions.HasSystemPermissions(Main.PluginId))
            {
                HttpContext.Current.Response.Write("<h1>未授权访问</h1>");
                HttpContext.Current.Response.End();
                return;
            }

            _config = Main.GetConfig();

            if (IsPostBack) return;

            RblIpRestrictionType.Items.Add(new ListItem
            {
                Text = ERestrictionTypeUtils.GetText(ERestrictionType.None),
                Value = ERestrictionTypeUtils.GetValue(ERestrictionType.None),
                Selected = ERestrictionTypeUtils.Equals(ERestrictionType.None, _config.IpRestrictionType)
            });
            RblIpRestrictionType.Items.Add(new ListItem
            {
                Text = ERestrictionTypeUtils.GetText(ERestrictionType.BlackList),
                Value = ERestrictionTypeUtils.GetValue(ERestrictionType.BlackList),
                Selected = ERestrictionTypeUtils.Equals(ERestrictionType.BlackList, _config.IpRestrictionType)
            });
            RblIpRestrictionType.Items.Add(new ListItem
            {
                Text = ERestrictionTypeUtils.GetText(ERestrictionType.WhiteList),
                Value = ERestrictionTypeUtils.GetValue(ERestrictionType.WhiteList),
                Selected = ERestrictionTypeUtils.Equals(ERestrictionType.WhiteList, _config.IpRestrictionType)
            });

            DdlIsHostRestriction.Items.Add(new ListItem
            {
                Text = "不设置",
                Value = false.ToString(),
                Selected = !_config.IsHostRestriction
            });
            DdlIsHostRestriction.Items.Add(new ListItem
            {
                Text = "设置",
                Value = true.ToString(),
                Selected = _config.IsHostRestriction
            });

            PhHost.Visible = _config.IsHostRestriction;
            TbHost.Text = _config.Host;
        }

        public void DdlIsHostRestriction_SelectedIndexChanged(object sender, EventArgs e)
        {
            PhHost.Visible = Convert.ToBoolean(DdlIsHostRestriction.SelectedValue);
        }

        public void Submit_OnClick(object sender, EventArgs e)
        {
            if (!Page.IsPostBack || !Page.IsValid) return;

            _config.IpRestrictionType = RblIpRestrictionType.SelectedValue;
            _config.IsHostRestriction = Convert.ToBoolean(DdlIsHostRestriction.SelectedValue);
            _config.Host = TbHost.Text;

            try
            {
                Main.SetConfig(_config);

                LtlMessage.Text = @"<div class=""alert alert-primary"" role=""alert"">访问限制选项修改成功！</div>";
            }
            catch (Exception ex)
            {
                LtlMessage.Text = $@"<div class=""alert alert-danger"" role=""alert"">访问限制选项修改失败：{ex.Message}！</div>";
            }
        }
    }
}