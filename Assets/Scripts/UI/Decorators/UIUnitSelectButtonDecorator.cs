using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.Decorators
{
    public class UIUnitSelectButtonDecorator : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private TMP_Text labelText;
        [SerializeField] private Image iconImage;
        
        public void UpdateVisuals(string unitName, Sprite displaySprite)
        {
            labelText.text = unitName;
            iconImage.sprite = displaySprite;
        }

        public void UpdateOnClickEvent(UnityAction onClickAction)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(onClickAction);
        }
    }
}
