using Doc.Pulse.Core.Enums;

namespace Doc.Pulse.Core.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class FixedWidthFieldAttribute : Attribute
{
    public FixedWidthFieldAttribute(int start, int length, TrimConfig trim = TrimConfig.FullTrim, char padding = ' ')
    {
        Start = start;
        Length = length;
        TrimMode = trim;
        PadCharacter = padding;
    }

    public int Start { get; }
    public int Length { get; }
    public char PadCharacter { get; }
    public TrimConfig TrimMode { get; }


    public string Parse(string fullLine)
    {
        if (Start > fullLine.Length - 1) return "";

        string value = fullLine.Substring(Start, Math.Min(Length, fullLine.Length - Start));

        switch (TrimMode)
        {
            case TrimConfig.FullTrim:
                value = value.Trim();
                break;
            case TrimConfig.Left:
                value = value.TrimStart();
                break;
            case TrimConfig.Right:
                value = value.TrimEnd();
                break;
        }

        //property.SetValue(this, tmp, null);
        //break;

        return value;
    }
}
