using System.Collections.Generic;
using System.Web;
using SiteServer.Plugin;
using SS.Restriction.Core;
using SS.Restriction.Model;
using SS.Restriction.Pages;

namespace SS.Restriction
{
    public class Main : PluginBase
    {
        private static Config _config;

        public const string Permission = "";

        public static Main Instance { get; private set; }

        public override void Startup(IService service)
        {
            _config = ConfigApi.GetConfig<Config>(0) ?? new Config
            {
                IpRestrictionType = ERestrictionTypeUtils.GetValue(ERestrictionType.None),
                IpBlackList = string.Empty,
                IpWhiteList = string.Empty
            };
            if (_config.IpBlackList == null)
            {
                _config.IpBlackList = string.Empty;
            }
            if (_config.IpWhiteList == null)
            {
                _config.IpWhiteList = string.Empty;
            }

            service.AddPluginMenu(PluginMenu);

            Instance = this;
        }

        public void SetConfig(Config config)
        {
            _config = config;
            ConfigApi.SetConfig(0, config);
        }

        public static Config GetConfig()
        {
            return _config;
        }

        public Menu PluginMenu
        {
            get
            {
                var isAllowed = RestrictionManager.IsVisitAllowed(_config);
                if (!isAllowed)
                {
                    HttpContext.Current.Response.Write("<h1>禁止访问</h1>");
                    HttpContext.Current.Response.Write($"<p>IP地址：{RestrictionManager.GetIpAddress()}<br />需要访问后台请与网站管理员联系开通相关权限.</p>");
                    HttpContext.Current.Response.End();
                    return null;
                }
                return new Menu
                {
                    Text = "后台访问限制",
                    Href = $"{nameof(PageOptions)}.aspx",
                    Menus = new List<Menu>
                    {
                        new Menu
                        {
                            Text = "黑名单",
                            Href = $"{nameof(PageList)}.aspx?type=Black"
                        },
                        new Menu
                        {
                            Text = "白名单",
                            Href = $"{nameof(PageList)}.aspx?type=White"
                        },
                        new Menu
                        {
                            Text = "访问限制选项",
                            Href = $"{nameof(PageOptions)}.aspx"
                        }
                    }
                };
            }
        }
    }
}