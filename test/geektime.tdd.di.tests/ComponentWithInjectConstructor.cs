namespace geektime.tdd.di;

public class ComponentWithInjectConstructor : IComponent
{
    public IDependency Dependency { get; }

    public ComponentWithInjectConstructor(IDependency dependency)
    {
        Dependency = dependency;
    }
}