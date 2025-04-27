using UnityEngine;

namespace Extensions
{
    public partial class ExtensionMethods
    {
        public static void ClearChildren(this Transform parent)
        {
            foreach (Transform child in parent) Object.Destroy(child.gameObject);
        }
    }
}
