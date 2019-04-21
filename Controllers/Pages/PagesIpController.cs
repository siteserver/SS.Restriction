using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;
using SiteServer.Plugin;
using SS.Restriction.Core;

namespace SS.Restriction.Controllers.Pages
{
    [RoutePrefix("pages/ip")]
    public class PagesIpController : ApiController
    {
        private const string Route = "";

        [HttpGet, Route(Route)]
        public IHttpActionResult GetIpAddress()
        {
            try
            {
                var request = Context.AuthenticatedRequest;
                if (!request.IsAdminLoggin || !request.AdminPermissions.HasSystemPermissions(Main.PluginId)) return Unauthorized();

                return Ok(new
                {
                    Value = RestrictionManager.GetIpAddress()
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost, Route(Route)]
        public IHttpActionResult AddRestriction()
        {
            try
            {
                var request = Context.AuthenticatedRequest;
                if (!request.IsAdminLoggin || !request.AdminPermissions.HasSystemPermissions(Main.PluginId)) return Unauthorized();

                var type = request.GetPostString("type");
                var restriction = request.GetPostString("restriction");

                var config = Main.GetConfig();

                var list = new List<string>(
                    (type == "Black" ? config.IpBlackList : config.IpWhiteList).Split(new[] {","},
                        StringSplitOptions.RemoveEmptyEntries));

                if (!list.Contains(restriction))
                {
                    list.Add(restriction);
                }

                if (type == "Black")
                {
                    config.IpBlackList = string.Join(",", list.ToArray());
                }
                else
                {
                    config.IpWhiteList = string.Join(",", list.ToArray());
                }

                Main.SetConfig(config);

                return Ok(new { });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPut, Route(Route)]
        public IHttpActionResult EditRestriction()
        {
            try
            {
                var request = Context.AuthenticatedRequest;
                if (!request.IsAdminLoggin || !request.AdminPermissions.HasSystemPermissions(Main.PluginId)) return Unauthorized();

                var type = request.GetPostString("type");
                var before = request.GetPostString("before");
                var now = request.GetPostString("now");

                var config = Main.GetConfig();

                var list = new List<string>((type == "Black" ? config.IpBlackList : config.IpWhiteList).Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries));

                for (var i = 0; i < list.Count; i++)
                {
                    var res = list[i];
                    if (res == before)
                    {
                        list[i] = now;
                    }
                }

                if (type == "Black")
                {
                    config.IpBlackList = string.Join(",", list.ToArray());
                }
                else
                {
                    config.IpWhiteList = string.Join(",", list.ToArray());
                }

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
