namespace geektime.tdd.args.tests.Fixture;

public interface IMockValueParser
{
    bool ConvertToBool();
    int ConvertToInt(string input);
    string ConvertToString(string input);
}