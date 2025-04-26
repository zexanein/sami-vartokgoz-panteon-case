using BuildingSystem;
using BuildingSystem.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Decorators
{
    public class UIBuildingSelectButtonDecorator : MonoBehaviour
    {
        public Button button;
        public TMP_Text labelText;
        public Image iconImage;
        public void InitializeVisuals(string buildingName, Sprite displaySprite)
        {
            labelText.text = buildingName;
            iconImage.sprite = displaySprite;
        }

        public void InitializeEvents(BuildingData building)
        {
            button.onClick.AddListener(() => BuildingSystemController.Instance.SelectBuildingData(building));
        }
    }
}
