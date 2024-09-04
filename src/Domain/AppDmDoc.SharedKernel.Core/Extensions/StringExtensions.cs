namespace AppDmDoc.SharedKernel.Core.Extensions;

public static class StringExtensions
{
    public static Guid ToGuidFromBase64(this string encodedString)
    {
        if (string.IsNullOrEmpty(encodedString))
            return Guid.Empty;

        var bytes = Convert.FromBase64String(encodedString);

        var returnValue = new Guid(bytes);

        return returnValue;
    }
}
