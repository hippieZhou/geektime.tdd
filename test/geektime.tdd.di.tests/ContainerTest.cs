using FluentAssertions;

namespace geektime.tdd.di;

public partial class ContainerTest
{
    public sealed partial class ComponentConstruction
    {
        //TODO:instance
        [Fact]
        public void should_bind_type_to_a_specific_instance()
        {
            var instance = new ComponentWithSpecificInstance();
            var context = new Context();
            context.Bind<IComponent>(instance);
            context.Get<IComponent>().Should().BeSameAs(instance);
        }
        
        //TODO:abstract class
        //TODO:interface
        public sealed partial class ConstructorInjection
        {
            private readonly Context _context;

            public ConstructorInjection()
            {
                _context = new Context();
            }

            //TODO: No args constructor
            [Fact]
            public void should_bind_type_to_a_class_with_default_constructor()
            {
                _context.Bind<IComponent, ComponentWithDefaultConstructor>();
                var instance = _context.Get<IComponent>();
                instance.Should().NotBeNull();
                instance.Should().BeOfType<ComponentWithDefaultConstructor>();
            }

            //TODO: with dependencies
            [Fact]
            public void should_bind_type_to_a_class_with_dependencies()
            {
                _context.Bind<IComponent, ComponentWithInjectConstructor>();
                _context.Bind<IDependency>(new Dependency());

                var instance = _context.Get<IComponent>();
                instance.Should().NotBeNull();
                instance.Should().BeOfType<ComponentWithInjectConstructor>();

                var dependency = ((ComponentWithInjectConstructor) instance).Dependency;
                dependency.Should().NotBeNull();
                dependency.Should().BeOfType<Dependency>();
            }
            
            //TODO: A->B->C
            [Fact]
            public void should_bind_type_to_a_class_with_transitive_dependencies()
            {
                _context.Bind<IComponent,ComponentWithInjectConstructor>();
                _context.Bind<IDependency>(new DependencyWithInjectConstructor("indirect dependency"));
                var instance = _context.Get<IComponent>();
                instance.Should().NotBeNull();
                instance.Should().BeOfType<ComponentWithInjectConstructor>();

                var dependency = ((ComponentWithInjectConstructor) instance).Dependency;
                dependency.Should().NotBeNull();
                dependency.Should().BeOfType<DependencyWithInjectConstructor>();

                var indirectDependency = (DependencyWithInjectConstructor) dependency;
                indirectDependency.GetDependency().Should().BeEquivalentTo("indirect dependency");
            }
            
            //TODO:multi inject constructors
            [Fact]
            public void should_throw_exception_if_multi_inject_constructors_provided()
            {
                var act = () =>
                {
                    _context.Bind<IComponent, ComponentWithMultiConstructors>();
                    _context.Get<IComponent>();
                };
                act.Should().Throw<IllegalComponentException>();
            }
            [Fact]
            public void should_throw_exception_if_no_inject_nor_default_constructor_provided()
            {
                var act = () =>
                {
                    _context.Bind<IComponent, ComponentWithNoInjectConstructorNorDefaultConstructor>();
                    _context.Get<IComponent>();
                };
                act.Should().Throw<IllegalComponentException>();
            }

            //TODO:no default constructor and inject constructor
            
            //TODO:dependencies not exist
            
        }
        
        public sealed class FieldInjection
        {
            
        }
        
        public sealed class MethodInjection
        {
            
        }
    }
    
    public sealed class DependenciesSelection
    {
        
    }
    
    public sealed class LifecycleManagement
    {
        
    }
}