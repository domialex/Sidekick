using System;
using System.Resources;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Xaml;
using Sidekick.Presentation.Localization;

namespace Sidekick.Presentation.Wpf.Views
{
    public class Translation : DependencyObject
    {
        public static readonly DependencyProperty ResourceManagerProperty =
            DependencyProperty.RegisterAttached("ResourceManager", typeof(ResourceManager), typeof(Translation));

        public static ResourceManager GetResourceManager(DependencyObject dependencyObject)
        {
            return (ResourceManager)dependencyObject.GetValue(ResourceManagerProperty);
        }

        public static void SetResourceManager(DependencyObject dependencyObject, ResourceManager value)
        {
            dependencyObject.SetValue(ResourceManagerProperty, value);
        }
    }

    public class LocExtension : MarkupExtension
    {
        public string StringName { get; }

        public LocExtension(string stringName)
        {
            StringName = stringName;
        }

        private ResourceManager GetResourceManager(object control)
        {
            if (control is DependencyObject dependencyObject)
            {
                var localValue = dependencyObject.ReadLocalValue(Translation.ResourceManagerProperty);

                // does this control have a "Translation.ResourceManager" attached property with a set value?
                if (localValue != DependencyProperty.UnsetValue && localValue is ResourceManager resourceManager)
                {
                    TranslationSource.Instance.AddResourceManager(resourceManager);

                    return resourceManager;
                }
            }

            return null;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            // targetObject is the control that is using the LocExtension
            var targetObject = (serviceProvider as IProvideValueTarget)?.TargetObject;

            if (targetObject?.GetType().Name == "SharedDp") // is extension used in a control template?
                return targetObject; // required for template re-binding

            var baseName = GetResourceManager(targetObject)?.BaseName ?? string.Empty;

            if (string.IsNullOrEmpty(baseName))
            {
                var parentObject = (targetObject as ItemsControl)?.Parent;
                baseName = GetResourceManager(parentObject)?.BaseName ?? string.Empty;
            }

            if (string.IsNullOrEmpty(baseName))
            {
                // rootObject is the root control of the visual tree (the top parent of targetObject)
                var rootObject = (serviceProvider as IRootObjectProvider)?.RootObject;
                baseName = GetResourceManager(rootObject)?.BaseName ?? string.Empty;
            }

            // template re-binding
            if (string.IsNullOrEmpty(baseName) && targetObject is FrameworkElement frameworkElement)
            {
                baseName = GetResourceManager(frameworkElement.TemplatedParent)?.BaseName ?? string.Empty;
            }

            var binding = new Binding
            {
                Mode = BindingMode.OneWay,
                Path = new PropertyPath(path: $"[{baseName}.{StringName}]"),
                Source = TranslationSource.Instance,
                FallbackValue = StringName
            };

            return binding.ProvideValue(serviceProvider);
        }
    }
}

