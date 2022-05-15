using System.Text.RegularExpressions;

namespace geektime.tdd.args;

public partial class Args
{
    public static IDictionary<string, IEnumerable<string>> ToMap(params string[] args)
    {
        var result = new Dictionary<string, IEnumerable<string>>();
        var option = string.Empty;
        var values = new List<string>();
        foreach (var arg in args)
        {
            if (new Regex("^-[a-zA-Z]+$").IsMatch(arg))
            {
                if (!string.IsNullOrEmpty(option))
                {
                    result.Add(option[1..], values);
                }
                option = arg;
                values = new List<string>();
            }
            else
            {
                values.Add(arg);
            }
        }

        result.Add(option[1..], values);
        return result;
    }
}