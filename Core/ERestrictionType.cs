namespace SS.Restriction.Core
{
	public enum ERestrictionType
	{
        None,
        BlackList,
		WhiteList
	}

	public static class ERestrictionTypeUtils
	{
		public static string GetValue(ERestrictionType type)
		{
		    switch (type)
            {
                case ERestrictionType.BlackList:
                    return "BlackList";
                case ERestrictionType.WhiteList:
                    return "WhiteList";
                default:
                    return "None";
            }
        }

		public static ERestrictionType GetEnumType(string typeStr)
		{
            var retVal = ERestrictionType.None;

            if (Equals(ERestrictionType.BlackList, typeStr))
			{
                retVal = ERestrictionType.BlackList;
            }
            else if (Equals(ERestrictionType.WhiteList, typeStr))
            {
                retVal = ERestrictionType.WhiteList;
            }

			return retVal;
		}

        private static bool Equals(ERestrictionType type, string typeStr)
		{
			if (string.IsNullOrEmpty(typeStr)) return false;
			if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
			{
				return true;
			}
			return false;
		}
	}
}
