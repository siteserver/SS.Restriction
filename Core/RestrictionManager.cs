using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;
using SS.Restriction.Model;

namespace SS.Restriction.Core
{
	public class RestrictionManager
	{
        private RestrictionManager()
		{
		}

        public static bool Contains(string text, string inner)
        {
            return text?.IndexOf(inner, StringComparison.Ordinal) >= 0;
        }

        public static int GetCount(string innerText, string content)
        {
            if (innerText == null || content == null)
            {
                return 0;
            }
            var count = 0;
            for (var index = content.IndexOf(innerText, StringComparison.Ordinal); index != -1; index = content.IndexOf(innerText, index + innerText.Length, StringComparison.Ordinal))
            {
                count++;
            }
            return count;
        }

        public static bool IsIpAddress(string ip)
        {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }

        public static string GetIpAddress()
        {
            var result = string.Empty;

            try
            {
                //取CDN用户真实IP的方法
                //当用户使用代理时，取到的是代理IP
                result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (!string.IsNullOrEmpty(result))
                {
                    //可能有代理
                    if (result.IndexOf(".", StringComparison.Ordinal) == -1)
                        result = null;
                    else
                    {
                        if (result.IndexOf(",", StringComparison.Ordinal) != -1)
                        {
                            result = result.Replace("  ", "").Replace("'", "");
                            var temparyip = result.Split(",;".ToCharArray());
                            foreach (var t in temparyip)
                            {
                                if (IsIpAddress(t) && t.Substring(0, 3) != "10." && t.Substring(0, 7) != "192.168" && t.Substring(0, 7) != "172.16.")
                                {
                                    result = t;
                                }
                            }
                            var str = result.Split(',');
                            if (str.Length > 0)
                                result = str[0].Trim();
                        }
                        else if (IsIpAddress(result))
                            return result;
                    }
                }

                if (string.IsNullOrEmpty(result))
                    result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                if (string.IsNullOrEmpty(result))
                    result = HttpContext.Current.Request.UserHostAddress;
                if (string.IsNullOrEmpty(result))
                    result = "localhost";

                if (result == "::1")
                {
                    result = "localhost";
                }
            }
            catch
            {
                // ignored
            }

            return result;
        }

        public static bool IsVisitAllowed(Config config)
        {
            var restrictionType = ERestrictionTypeUtils.GetEnumType(config.IpRestrictionType);

            var restrictionList = new List<string>();
            if (restrictionType == ERestrictionType.BlackList)
            {
                restrictionList = new List<string>(config.IpBlackList.Split(new[] {","}, StringSplitOptions.RemoveEmptyEntries));
            }
            else if (restrictionType == ERestrictionType.WhiteList)
            {
                restrictionList = new List<string>(config.IpWhiteList.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries));
            }

            var isAllowed = true;
            if (restrictionType != ERestrictionType.None)
            {
                var userIp = GetIpAddress();
                if (restrictionType == ERestrictionType.BlackList)
                {
                    var list = new IpList();
                    foreach (var restriction in restrictionList)
                    {
                        AddRestrictionToIpList(list, restriction);
                    }
                    if (list.CheckNumber(userIp))
                    {
                        isAllowed = false;
                    }
                }
                else if (restrictionType == ERestrictionType.WhiteList)
                {
                    if (restrictionList.Count > 0)
                    {
                        isAllowed = false;
                        var list = new IpList();
                        foreach (var restriction in restrictionList)
                        {
                            AddRestrictionToIpList(list, restriction);
                        }
                        if (list.CheckNumber(userIp))
                        {
                            isAllowed = true;
                        }
                    }
                }
            }
            if (isAllowed)
            {
                if (config.IsHostRestriction && !string.IsNullOrEmpty(config.Host))
                {
                    var currentHost = RemoveProtocolFromUrl(GetHost());
                    if (!StartsWithIgnoreCase(currentHost, RemoveProtocolFromUrl(config.Host)))
                    {
                        isAllowed = false;
                    }
                }
            }
            return isAllowed;
        }

        public static bool StartsWithIgnoreCase(string text, string startString)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(startString)) return false;
            return text.Trim().ToLower().StartsWith(startString.Trim().ToLower()) || string.Equals(text.Trim(), startString.Trim(), StringComparison.CurrentCultureIgnoreCase);
        }

        public static string RemoveProtocolFromUrl(string url)
        {
            if (string.IsNullOrEmpty(url)) return string.Empty;

            url = url.Trim();
            return IsProtocolUrl(url) ? url.Substring(url.IndexOf("://", StringComparison.Ordinal) + 3) : url;
        }

        public static bool IsProtocolUrl(string url)
        {
            if (string.IsNullOrEmpty(url)) return false;

            url = url.Trim();
            return url.IndexOf("://", StringComparison.Ordinal) != -1 || url.StartsWith("javascript:");
        }

        private static string GetHost()
        {
            var host = string.Empty;
            if (HttpContext.Current == null) return string.IsNullOrEmpty(host) ? string.Empty : host.Trim().ToLower();
            host = HttpContext.Current.Request.Headers["HOST"];
            if (string.IsNullOrEmpty(host))
            {
                host = HttpContext.Current.Request.Url.Host;
            }

            return string.IsNullOrEmpty(host) ? string.Empty : host.Trim().ToLower();
        }

        private static void AddRestrictionToIpList(IpList list, string restriction)
        {
            if (string.IsNullOrEmpty(restriction)) return;

            if (Contains(restriction, "-"))
            {
                restriction = restriction.Trim(' ', '-');
                var arr = restriction.Split('-');
                list.AddRange(arr[0].Trim(), arr[1].Trim());
            }
            else if (Contains(restriction, "*"))
            {
                var ipPrefix = restriction.Substring(0, restriction.IndexOf('*'));
                ipPrefix = ipPrefix.Trim(' ', '.');
                var dotNum = GetCount(".", ipPrefix);

                string ipNumber;
                string mask;
                if (dotNum == 0)
                {
                    ipNumber = ipPrefix + ".0.0.0";
                    mask = "255.0.0.0";
                }
                else if (dotNum == 1)
                {
                    ipNumber = ipPrefix + ".0.0";
                    mask = "255.255.0.0";
                }
                else
                {
                    ipNumber = ipPrefix + ".0";
                    mask = "255.255.255.0";
                }
                list.Add(ipNumber, mask);
            }
            else
            {
                list.Add(restriction);
            }
        }
	}
}
