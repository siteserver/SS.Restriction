using System;
using System.Collections.Generic;
using System.Web;
using SiteServer.Plugin;
using SiteServer.Plugin.Features;
using SiteServer.Plugin.Models;
using SS.Restriction.Core;
using SS.Restriction.Model;

namespace SS.Restriction
{
    public class Main : PluginBase, IMenu
    {
        private static Config _config;

        public const string Permission = "";

        public static IPluginContext Context { get; set; }

        public static void SetConfig(Config config)
        {
            _config = config;
            Context.ConfigApi.SetConfig(0, config);
        }

        public static Config GetConfig()
        {
            return _config;
        }

        public override Action<IPluginContext> PluginActive => context =>
        {
            Context = context;
            _config = Context.ConfigApi.GetConfig<Config>(0) ?? new Config
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
        };

        public override PluginMenu PluginMenu
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
                return new PluginMenu
                {
                    Text = "后台访问限制",
                    Menus = new List<PluginMenu>
                    {
                        new PluginMenu
                        {
                            Text = "黑名单",
                            Href = "PageList.aspx?type=Black"
                        },
                        new PluginMenu
                        {
                            Text = "白名单",
                            Href = "PageList.aspx?type=White"
                        },
                        new PluginMenu
                        {
                            Text = "访问限制选项",
                            Href = "PageOptions.aspx"
                        }
                    }
                };
            }
        }
    }
}