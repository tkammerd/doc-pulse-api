namespace AppDmDoc.SharedKernel.Core.Extensions;

public static class EnumExtensions
{
    public static EnumType TryParseEnumOrDefault<EnumType>(this string value, EnumType defaultVlue, bool ignoreCase = false) where EnumType : struct, Enum
    {
        if (Enum.TryParse(value, ignoreCase, out EnumType enumeration))
        {
            return enumeration;
        }
        else
        {
            return defaultVlue;
        }
    }
}
