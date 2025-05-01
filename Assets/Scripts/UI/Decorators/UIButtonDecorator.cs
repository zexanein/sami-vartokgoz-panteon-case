using Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.Decorators
{
    /// <summary>
    /// A utility class for customizing and controlling a UI Button at runtime.
    /// Handles label, icon, click behavior, and interactability.
    /// </summary>
    public class UIButtonDecorator : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private TMP_Text labelText;
        [SerializeField] private Image iconImage;

        /// <summary>
        /// Updates the button's display name and icon.
        /// </summary>
        /// <param name="displayName">The label text to show on the button.</param>
        /// <param name="displaySprite">The icon sprite to show on the button.</param>
        public void UpdateVisuals(string displayName, Sprite displaySprite)
        {
            labelText.text = displayName;
            iconImage.sprite = displaySprite;
        }

        /// <summary>
        /// Replaces all existing onClick listeners with the given action.
        /// </summary>
        /// <param name="onClickAction">The action to invoke when the button is clicked.</param>
        public void SetOnClickEvent(UnityAction onClickAction) => button.ReplaceButtonClickEvent(onClickAction);

        /// <summary>
        /// Enables or disables the button's interactivity.
        /// </summary>
        /// <param name="state">If <c>true</c>, the button is interactable; otherwise, it is disabled.</param>
        public void SetInteractable(bool state) => button.interactable = state;
    }
}