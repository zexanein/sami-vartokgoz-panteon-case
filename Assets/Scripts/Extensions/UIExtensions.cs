using UnityEngine.Events;
using UnityEngine.UI;

namespace Extensions
{
    /// <summary>
    /// Provides extension methods for Unity's UI components.
    /// </summary>
    public partial class ExtensionMethods
    {
        /// <summary>
        /// Replaces all existing listeners on the button's <c>onClick</c> event with the specified callback.
        /// This ensures that only the provided action is triggered when the button is clicked.
        /// </summary>
        /// <param name="button">The button whose click event listeners will be replaced.</param>
        /// <param name="onClick">The action to be executed when the button is clicked.</param>
        public static void ReplaceButtonClickEvent(this Button button, UnityAction onClick)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(onClick);
        }
    }
}
