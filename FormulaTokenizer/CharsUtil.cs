namespace FormulaTokenizer;

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
}