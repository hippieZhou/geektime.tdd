namespace geektime.tdd.args;

[AttributeUsage(AttributeTargets.Parameter)]
public class OptionAttribute : Attribute
{
    public OptionAttribute(string value)
    {
        Value = value;
    }

    public string Value { get; }
}