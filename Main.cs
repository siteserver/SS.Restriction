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
        public static string PluginId { get; private set; }

        private static Config _config;

        public override void Startup(IService service)
        {
            PluginId = Id;

            _config = Context.ConfigApi.GetConfig<Config>(PluginId, 0) ?? new Config
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

            service.AddSystemMenu(() =>
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
            });
        }

        public static void SetConfig(Config config)
        {
            _config = config;
            Context.ConfigApi.SetConfig(PluginId, 0, config);
        }

        public static Config GetConfig()
        {
            return _config;
        }
    }
}