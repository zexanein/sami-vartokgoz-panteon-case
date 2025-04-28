using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Extensions
{
    public partial class ExtensionMethods
    {
        public static void ReplaceButtonClickEvent(this Button button, UnityAction onClick)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(onClick);
        }
    }
}
