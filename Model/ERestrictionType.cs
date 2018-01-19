namespace SS.Restriction.Model
{
	public enum ERestrictionType
	{
        None,
        BlackList,
		WhiteList
	}

	public class ERestrictionTypeUtils
	{
		public static string GetValue(ERestrictionType type)
		{
		    if (type == ERestrictionType.BlackList)
		    {
		        return "BlackList";
		    }
		    if (type == ERestrictionType.WhiteList)
		    {
		        return "WhiteList";
		    }
            return "None";
        }

		public static string GetText(ERestrictionType type)
		{
		    if (type == ERestrictionType.BlackList)
		    {
		        return "启用黑名单，禁止黑名单中的IP进行访问，其余允许访问";
		    }
		    if (type == ERestrictionType.WhiteList)
		    {
		        return "启用白名单，允许白名单中的IP进行访问，其余禁止访问";
		    }
            return "无访问限制";
        }

		public static ERestrictionType GetEnumType(string typeStr)
		{
            var retval = ERestrictionType.None;

            if (Equals(ERestrictionType.BlackList, typeStr))
			{
                retval = ERestrictionType.BlackList;
            }
            else if (Equals(ERestrictionType.WhiteList, typeStr))
            {
                retval = ERestrictionType.WhiteList;
            }

			return retval;
		}

		public static bool Equals(ERestrictionType type, string typeStr)
		{
			if (string.IsNullOrEmpty(typeStr)) return false;
			if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
			{
				return true;
			}
			return false;
		}

        public static bool Equals(string typeStr, ERestrictionType type)
        {
            return Equals(type, typeStr);
        }
	}
}
