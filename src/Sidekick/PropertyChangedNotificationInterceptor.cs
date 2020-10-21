using System;

namespace Sidekick
{
    public static class PropertyChangedNotificationInterceptor
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        public static void Intercept(object target, Action onPropertyChangedAction, string propertyName)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(onPropertyChangedAction);
        }
    }
}
