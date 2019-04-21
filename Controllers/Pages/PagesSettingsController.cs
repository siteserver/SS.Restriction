using System;
using System.Web;
using System.Web.Http;
using SiteServer.Plugin;
using SS.Restriction.Core;

namespace SS.Restriction.Controllers.Pages
{
    [RoutePrefix("pages/settings")]
    public class PagesSettingsController : ApiController
    {
        private const string Route = "";

        [HttpGet, Route(Route)]
        public IHttpActionResult GetConfig()
        {
            try
            {
                var request = Context.AuthenticatedRequest;
                if (!request.IsAdminLoggin || !request.AdminPermissions.HasSystemPermissions(Main.PluginId)) return Unauthorized();

                var config = Main.GetConfig();
                var host = HttpContext.Current.Request.Url.Host;

                return Ok(new
                {
                    Value = config,
                    Host = host
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost, Route(Route)]
        public IHttpActionResult SetConfig()
        {
            try
            {
                var request = Context.AuthenticatedRequest;
                if (!request.IsAdminLoggin || !request.AdminPermissions.HasSystemPermissions(Main.PluginId)) return Unauthorized();

                var config = request.GetPostObject<Config>();

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
