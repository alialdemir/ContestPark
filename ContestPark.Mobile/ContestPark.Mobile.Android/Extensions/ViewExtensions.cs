using System.Reflection;

namespace ContestPark.Mobile.Droid.Extensions
{
    internal static class ViewExtensions
    {
        public static T FindChildOfType<T>(this ViewGroup parent) where T : View
        {
            if (parent == null)
                return null;

            if (parent.ChildCount == 0)
                return null;

            for (var i = 0; i < parent.ChildCount; i++)
            {
                var child = parent.GetChildAt(i);

                var typedChild = child as T;
                if (typedChild != null)
                {
                    return typedChild;
                }

                if (!(child is ViewGroup))
                    continue;

                var result = FindChildOfType<T>(child as ViewGroup);
                if (result != null)
                    return result;
            }

            return null;
        }
        public static T GetInternalField<T>(this Xamarin.Forms.BindableObject element, string propertyKeyName) where T : class
        {
            // reflection stinks, but hey, what can you do?
            var pi = element.GetType().GetField(propertyKeyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            var key = (T)pi?.GetValue(element);

            return key;
        }
    }
}