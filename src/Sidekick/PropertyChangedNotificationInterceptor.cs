using System;

namespace Sidekick
{
    public static class PropertyChangedNotificationInterceptor
    {
        public static void Intercept(object target, Action onPropertyChangedAction, string propertyName)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(onPropertyChangedAction);
        }
    }
}
