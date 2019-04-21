namespace SS.Restriction.Core
{
    public class Config
    {
        public string IpRestrictionType { get; set; }
        public string IpWhiteList { get; set; }
        public string IpBlackList { get; set; }
        public bool IsHostRestriction { get; set; }
        public string Host { get; set; }
    }
}