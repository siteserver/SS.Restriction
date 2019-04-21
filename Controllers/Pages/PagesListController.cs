using System;
using System.Collections.Generic;
using System.Web.Http;
using SiteServer.Plugin;

namespace SS.Restriction.Controllers.Pages
{
    [RoutePrefix("pages/list")]
    public class PagesListController : ApiController
    {
        private const string Route = "";

        [HttpGet, Route(Route)]
        public IHttpActionResult GetList()
        {
            try
            {
                var request = Context.AuthenticatedRequest;
                if (!request.IsAdminLoggin || !request.AdminPermissions.HasSystemPermissions(Main.PluginId)) return Unauthorized();

                var type = request.GetQueryString("type");

                var config = Main.GetConfig();
                var list = new List<string>((type == "Black" ? config.IpBlackList : config.IpWhiteList).Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries));

                return Ok(new
                {
                    Value = list
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpDelete, Route(Route)]
        public IHttpActionResult Delete()
        {
            try
            {
                var request = Context.AuthenticatedRequest;
                if (!request.IsAdminLoggin || !request.AdminPermissions.HasSystemPermissions(Main.PluginId)) return Unauthorized();

                var type = request.GetPostString("type");
                var restriction = request.GetPostString("restriction");

                var config = Main.GetConfig();
                var list = new List<string>((type == "Black" ? config.IpBlackList : config.IpWhiteList).Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries));

                list.Remove(restriction);

                if (type == "Black")
                {
                    config.IpBlackList = string.Join(",", list.ToArray());
                }
                else
                {
                    config.IpWhiteList = string.Join(",", list.ToArray());
                }

                Main.SetConfig(config);

                return Ok(new
                {
                    Value = list
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
