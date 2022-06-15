namespace geektime.tdd.di;

public interface IDependency
{

}

public class Dependency : IDependency
{

}

public class DependencyWithInjectConstructor : IDependency
{
    private string _dependency;

    public DependencyWithInjectConstructor(string dependency)
    {
        _dependency = dependency;
    }

    public string GetDependency() => _dependency;
}

public class DependencyDependedOnComponent : IDependency
{
    
}