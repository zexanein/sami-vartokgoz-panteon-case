using UnityEngine;

namespace Extensions
{
    /// <summary>
    /// Provides extension methods for Unity's Transform class.
    /// </summary>
    public partial class ExtensionMethods
    {
        /// <summary>
        /// Destroys all child objects of the specified parent transform.
        /// </summary>
        /// <param name="parent">The parent transform whose children will be destroyed.</param>
        public static void ClearChildren(this Transform parent)
        {
            foreach (Transform child in parent) Object.Destroy(child.gameObject);
        }
    }
}
