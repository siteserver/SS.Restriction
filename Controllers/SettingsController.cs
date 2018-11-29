using System;
using System.Web.Http;
using SiteServer.Plugin;

namespace SS.Restriction.Controllers
{
    [RoutePrefix("settings")]
    public class SettingsController : ApiController
    {
        private const string Route = "";

        [HttpGet, Route(Route)]
        public IHttpActionResult Get()
        {
            try
            {
                var request = Context.GetCurrentRequest();
                if (!request.IsAdminLoggin || !request.AdminPermissions.HasSystemPermissions(Main.PluginId)) return Unauthorized();

                var config = Main.GetConfig();

                return Ok(new
                {
                    Value = config
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost, Route(Route)]
        public IHttpActionResult Submit()
        {
            try
            {
                var request = Context.GetCurrentRequest();
                if (!request.IsAdminLoggin || !request.AdminPermissions.HasSystemPermissions(Main.PluginId)) return Unauthorized();

                var ipRestrictionType = request.GetPostString("ipRestrictionType");
                var isHostRestriction = request.GetPostBool("isHostRestriction");
                var host = request.GetPostString("host");

                var config = Main.GetConfig();
                config.IpRestrictionType = ipRestrictionType;
                config.IsHostRestriction = isHostRestriction;
                config.Host = host;

                Main.SetConfig(config);

                return Ok(new { });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
