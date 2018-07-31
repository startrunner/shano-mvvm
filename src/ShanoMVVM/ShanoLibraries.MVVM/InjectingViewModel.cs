using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ShanoLibraries.MVVM
{
    /// <summary>
    /// A base ViewModel intended to make use of an IOC container easier.
    /// Inherits from <see cref="ViewModel"/>
    /// </summary>
    public abstract class InjectingViewModel : ViewModel
    {

        protected InjectingViewModel(IDependencyManager dependencies)
        {
            Dependencies = dependencies;

            Injection[] injections = InjectionsByType.GetOrAdd(this.GetType(), GenerateInjectionsForType);

            foreach (Injection x in injections)
            {
                if (x.HasIdentifier) x.SetterAction(this, dependencies.InjectOnConstruct(x.ServiceType, x.Identifier));
                else x.SetterAction(this, dependencies.InjectOnConstruct(x.ServiceType));
            }
        }

        static readonly ConcurrentDictionary<Type, Injection[]> InjectionsByType = new ConcurrentDictionary<Type, Injection[]>();
        protected IDependencyManager Dependencies { get; }

        static Injection[] GenerateInjectionsForType(Type viewModelType)
        {
            return Enumerable.Concat(
                viewModelType
                .GetRuntimeProperties()
                .Select(x => Tuple.Create(x, x.GetCustomAttribute<InjectAttribute>()))
                .Where(x => x.Item2 != null)
                .Select(x => GenerateInjection(viewModelType, x.Item1.PropertyType, x.Item2, x.Item1.Name))
                ,
                viewModelType
                .GetRuntimeFields()
                .Select(x => Tuple.Create(x, x.GetCustomAttribute<InjectAttribute>()))
                .Where(x => x.Item2 != null)
                .Select(x => GenerateInjection(viewModelType, x.Item1.FieldType, x.Item2, x.Item1.Name))
            )
            .ToArray();
        }

        static Injection GenerateInjection(Type viewModelType, Type propertyType, InjectAttribute attribute, string propertyName)
        {
            ParameterExpression parameterViewModel = Expression.Parameter(typeof(object), "viewModel");
            ParameterExpression parameterService = Expression.Parameter(typeof(object), "service");
            UnaryExpression expressionViewModel = Expression.ConvertChecked(parameterViewModel, viewModelType);
            UnaryExpression expressionService = Expression.ConvertChecked(parameterService, propertyType);

            Expression<Injection.Setter> setterExpression = Expression.Lambda<Injection.Setter>(
                parameters: new[] { parameterViewModel, parameterService },
                body: Expression.Assign(
                    left: Expression.PropertyOrField(expressionViewModel, propertyName),
                    right: expressionService
                )
            );

            ;

            return new Injection(
                serviceType: propertyType,
                setterAction: setterExpression.Compile(),
                hasIdentifier: attribute.HasIdentifier(out object injectionIdentifier),
                identifier: injectionIdentifier
            );
        }
    }
}
