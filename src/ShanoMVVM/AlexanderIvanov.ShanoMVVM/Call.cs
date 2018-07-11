using System;
using System.Linq;
using System.Windows;
using System.Windows.Markup;

namespace AlexanderIvanov.ShanoMVVM
{
    public class Call : MarkupExtension
    {
        public Call(string methodName)
        {
            Command = new CallCommand {
                MethodName = methodName
            };
        }

        CallCommand Command { get; set; } 

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var provideValueTarget = (IProvideValueTarget)serviceProvider.GetService(typeof(IProvideValueTarget));
            FrameworkElement t = (FrameworkElement)provideValueTarget.TargetObject; ;
            t.AddHandler(FrameworkElement.LoadedEvent, (RoutedEventHandler)((s, e) => HandleLoaded((FrameworkElement)s)) );
            return Command;
        }

        void HandleLoaded(FrameworkElement e)
        {
            throw new Exception(
                e.DataContext.GetType().ToString()+"\n"+
                ((DependencyObjectType)e.DataContext.GetType().GetMethod("get_DependencyObjectType").Invoke(e.DataContext, null)).SystemType.ToString()
            );
            Command.DataContext = e.DataContext;
        }
    }
}
