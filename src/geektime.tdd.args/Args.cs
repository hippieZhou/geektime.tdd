using System.Reflection;
using BindingFlags = System.Reflection.BindingFlags;

namespace geektime.tdd.args;

public static class Args
{
    private static readonly IDictionary<Type, IOptionParser> PARSERS =
        new Dictionary<Type, IOptionParser>
        {
            {typeof(bool), new BooleanOptionParser()},
            {typeof(int), new SingleValueOptionParser<int>(Convert.ToInt32)},
            {typeof(string), new SingleValueOptionParser<string>(Convert.ToString)}
        };

    public static T Parse<T>(params string[] args)
    {
        var constructor = typeof(T)
            .GetConstructors(BindingFlags.Instance | BindingFlags.Public)[0];

        var values = constructor
            .GetParameters()
            .Select(x => ParseOption(args, x));

        return (T) constructor.Invoke(values.ToArray());
    }

    private static object ParseOption(string[] arguments, ParameterInfo parameter)
    {
        return GetOptionParser(parameter.ParameterType)
            .Parse(arguments, parameter.GetCustomAttribute<OptionAttribute>());
    }

    private static IOptionParser GetOptionParser(Type type)
    {
        return PARSERS[type];
    }
}