using UnityEngine;

namespace Extensions
{
    public static class Utils
    {
        public static bool IsUnityObjectNull<T>(T selectable) => selectable == null || (selectable is Object obj && obj == null);
    }
}
