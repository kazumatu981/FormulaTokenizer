namespace FormulaTokenizer;

public class OperatorToken : Token
{
    public const string Plus = "+";
    public const string Minus = "-";
    public const string Product = "*";
    public const string Devide = "/";
    public Token? LeftHand { get; set; }
    public Token? RightHand { get; set; }
    public OperatorToken(string text) : base(TokenType.Operator, text) { }

    public override int GetValue()
        => (LeftHand, RightHand, Text) switch
        {
            (null, null, _) => throw new NotImplementedException(),
            (_, null, _) => throw new NotImplementedException(),
            (null, _, Plus) => RightHand.GetValue(),
            (null, _, Minus) => (-1) * RightHand.GetValue(),
            (null, _, _) => throw new NotImplementedException(),
            (_, _, Plus) => LeftHand.GetValue() + RightHand.GetValue(),
            (_, _, Minus) => LeftHand.GetValue() - RightHand.GetValue(),
            (_, _, Product) => LeftHand.GetValue() * RightHand.GetValue(),
            (_, _, Devide) => LeftHand.GetValue() / RightHand.GetValue(),
            _ => throw new NotImplementedException()
        };


}