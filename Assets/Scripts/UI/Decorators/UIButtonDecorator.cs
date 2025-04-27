using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.Decorators
{
    public class UIButtonDecorator : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private TMP_Text labelText;
        [SerializeField] private Image iconImage;
        
        public virtual void UpdateVisuals(string displayName, Sprite displaySprite)
        {
            labelText.text = displayName;
            iconImage.sprite = displaySprite;
        }

        public virtual void UpdateOnClickEvent(UnityAction onClickAction)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(onClickAction);
        }

        public virtual void SetInteractable(bool state) => button.interactable = state;
    }
}
