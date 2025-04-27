using Buildings;
using Extensions;
using TMPro;
using UI.Controllers;
using UI.Decorators;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.Views
{
    public class UIInformationMenuView : MonoBehaviour
    {
        private UIInformationMenuController _controller;
    
        [Header("General Info")]
        [SerializeField] private GameObject menuContent;
        [SerializeField] private TMP_Text labelText;
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private Image iconImage;
    
        [Header("Destroy Button")]
        [SerializeField] private TMP_Text destroyButtonText;
        [SerializeField] private Button destroyButton;
    
        [Header("Unit Production")]
        [SerializeField] private GameObject unitProductionPanel;
        [SerializeField] private GameObject unitButtonContainer;
        [SerializeField] private TMP_Text unitSpawnPointBlockedWarningText;
        [SerializeField] private UIButtonDecorator unitButtonDecorator;

        public void SetController(UIInformationMenuController controller) => _controller = controller;
        
        public void DisplayInformation(Building building)
        {
            menuContent.SetActive(true);
            
            // General information
            labelText.text = building.Blueprint.buildingName;
            descriptionText.text = building.Blueprint.buildingDescription;
            iconImage.sprite = building.Blueprint.displaySprite;
            
            // Unit production
            if (building is UnitSpawnerBuilding unitSpawnerBuilding)
            {
                unitProductionPanel.SetActive(true);
                UpdateUnitProductionVisuals(unitSpawnerBuilding);
            }
            
            else unitProductionPanel.SetActive(false);
            
            // Destroy button
            destroyButtonText.text = "Demolish";
            UpdateDemolishKillEvent(() => _controller.DemolishBuilding(building));
        }
        
        public void HideInformation()
        {
            labelText.text = string.Empty;
            descriptionText.text = string.Empty;
            iconImage.sprite = null;
            menuContent.SetActive(false);
            unitProductionPanel.SetActive(false);
        } 

        private void UpdateDemolishKillEvent(UnityAction onClickAction)
        {
            destroyButton.onClick.RemoveAllListeners();
            destroyButton.onClick.AddListener(onClickAction);
        }

        private void UpdateUnitProductionVisuals(UnitSpawnerBuilding unitSpawnerBuilding)
        {
            // Clear previous buttons
            unitButtonContainer.transform.ClearChildren();
        
            if (unitSpawnerBuilding.Blueprint.productionData == null) return;

            var isSpawnPointValid = unitSpawnerBuilding.IsSpawnPointValid();
            unitSpawnPointBlockedWarningText.gameObject.SetActive(!isSpawnPointValid);
            
            // Create new buttons
            foreach (var unit in unitSpawnerBuilding.Blueprint.productionData.unitBlueprints)
            {
                var spawnedDecorator = Instantiate(unitButtonDecorator, unitButtonContainer.transform);
                spawnedDecorator.UpdateVisuals(unit.unitName, unit.uiIcon);
                spawnedDecorator.UpdateOnClickEvent(() => _controller.OnProduceUnitButtonClicked(unit));
                spawnedDecorator.SetInteractable(isSpawnPointValid);
            }
        }
    }
}
