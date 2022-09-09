namespace FormulaTokenizer;

public enum CharType
{
    Unknown,
    WhiteSpace,
    EOL,
    Operator,
    NonZeroNumber,
    Zero,
}
public static class CharsUtil
{
    public static char SafeGetAt(this string thisText, int index) =>
        (0 <= index && index < thisText.Length) ? thisText[index] : '\0';
    public static IEnumerable<char> GetChars(this string thisText, bool includeNull = true)
    {
        var stopIndex = includeNull ? thisText.Length : thisText.Length - 1;
        for (var i = 0; i <= stopIndex; i++)
        {
            yield return thisText.SafeGetAt(i);
        }
    }
    public static CharType GetCharType(this char thisChar)
    {
        var charType = thisChar switch
        {
            char c when c.IsWhiteSpace() => CharType.WhiteSpace,
            char c when c.IsEol() => CharType.EOL,
            char c when c.IsOperator() => CharType.Operator,
            char c when c.IsNonZeroNumber() => CharType.NonZeroNumber,
            char c when c.IsZero() => CharType.Zero,
            _ => CharType.Unknown
        };

        return charType;
    }

    #region private methods
    private static bool IsWhiteSpace(this char thisChar)
        => ' ' == thisChar;
    private static bool IsEol(this char thisChar)
        => '\0' == thisChar;
    private static bool IsOperator(this char thisChar)
        => '+' == thisChar || '-' == thisChar || '*' == thisChar || '/' == thisChar;

    private static bool IsNonZeroNumber(this char thisChar)
        => '1' <= thisChar && thisChar <= '9';

    private static bool IsZero(this char thisChar)
        => '0' == thisChar;
    #endregion

}