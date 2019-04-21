using SiteServer.Plugin;
using SS.Restriction.Core;

namespace SS.Restriction
{
    public class Main : PluginBase
    {
        public const string PluginId = "SS.Restriction";

        public override void Startup(IService service)
        {
            service.AddSystemMenu(() =>
            {
                var config = GetConfig();
                var isAllowed = RestrictionManager.IsVisitAllowed(config);
                var text = "后台访问限制";
                if (!isAllowed)
                {
                    var errorUrl = Context.UtilsApi.GetAdminUrl("pageError.html");
                    text =
                        $@"<img onerror=""location.href='{errorUrl}?message=' + encodeURIComponent('{$"IP地址 {RestrictionManager.GetIpAddress()} 禁止访问后台，请与网站管理员联系开通相关权限。"}');"" src=""{Context.PluginApi.GetPluginUrl(PluginId, "assets/load.gif")}"">";
                }
                return new Menu
                {
                    Text = text,
                    Href = "pages/settings.html"
                };
            });
        }

        public static void SetConfig(Config config)
        {
            Context.ConfigApi.SetConfig(PluginId, 0, config);
        }

        public static Config GetConfig()
        {
            var config = Context.ConfigApi.GetConfig<Config>(PluginId, 0) ?? new Config
            {
                IpRestrictionType = ERestrictionTypeUtils.GetValue(ERestrictionType.None),
                IpBlackList = string.Empty,
                IpWhiteList = string.Empty
            };
            if (config.IpBlackList == null)
            {
                config.IpBlackList = string.Empty;
            }
            if (config.IpWhiteList == null)
            {
                config.IpWhiteList = string.Empty;
            }

            return config;
        }
    }
}