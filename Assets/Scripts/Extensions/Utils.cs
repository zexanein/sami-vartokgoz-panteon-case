using UnityEngine;

namespace Extensions
{
    /// <summary>
    /// Utility class containing static methods for various operations.
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Safely checks whether a Unity object is null.
        /// This accounts for Unity's custom null handling for destroyed objects that still appear non-null in C#.
        /// </summary>
        /// /// <typeparam name="T">The object type to check.</typeparam>
        /// <param name="selectable">The object reference to evaluate.</param>
        /// <returns>
        /// <c>true</c> if the object is either null or has been destroyed by Unity;
        /// otherwise, <c>false</c>.
        /// </returns>
        public static bool IsUnityObjectNull<T>(T selectable) => selectable == null || (selectable is Object obj && obj == null);
    }
}
