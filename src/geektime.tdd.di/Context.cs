namespace geektime.tdd.di;

public class Context
{
    private readonly Dictionary<Type, Func<object>> _providers = new();

    public void Bind<TInterface>(TInterface instance) where TInterface : class
    {
        _providers[typeof(TInterface)] = () => instance;
    }

    public void Bind<TInterface, TImplementation>()
        where TInterface : class
        where TImplementation : TInterface
    {
        _providers[typeof(TInterface)] = () => CreateInstance(typeof(TImplementation));
    }

    public TInterface Get<TInterface>() where TInterface : class
    {
        return (TInterface) _providers[typeof(TInterface)].Invoke();
    }

    private object CreateInstance(Type concreteType)
    {
        var constructors = concreteType.GetConstructors();
        if (constructors.Length > 1)
        {
            throw new IllegalComponentException();
        }

        var defaultConstructor = constructors.Single();
        var paramTypes = defaultConstructor.GetParameters().Select(p => p.ParameterType);
        var dependencies = paramTypes.Select(paramType =>
        {
            if (!_providers.ContainsKey(paramType))
            {
                throw new DependencyNotFoundException();
            }

            return _providers[paramType].Invoke();
        }).ToArray();
        return defaultConstructor.Invoke(dependencies);
    }
}