namespace geektime.tdd.args;

public class TooManyArgumentsException : Exception
{
    public string Option { get; }

    public TooManyArgumentsException(string option)
    {
        Option = option;
    }
}

public class InsufficientArgumentsException : Exception
{
    public string Option { get; }

    public InsufficientArgumentsException(string option)
    {
        Option = option;
    }
}

public class IllegalOptionException : Exception
{
    public string Parameter { get; }

    public IllegalOptionException(string parameter)
    {
        Parameter = parameter;
    }
}

public class IllegalValueException : Exception
{
    public string Option { get; }
    public string Value { get;  }
    public IllegalValueException(string option, string value)
    {
        Option = option;
        Value = value;
    }
}

public class UnsupportedOptionTypeException : Exception
{
    public string Option { get; }

    public UnsupportedOptionTypeException(string option)
    {
        Option = option;
    }
}
