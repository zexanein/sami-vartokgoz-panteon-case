using Extensions;
using GameElements;
using TMPro;
using UI.Controllers;
using UI.Decorators;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.Views
{
    public class UIInformationMenuView : UIViewBase
    {
        [Header("General Info")]
        [SerializeField] private GameObject menuContent;
        [SerializeField] private TMP_Text labelText;
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private Image iconImage;
        
        [Header("Health and Damage")]
        public GameObject healthObject;
        public GameObject damageObject;
        public TMP_Text healthText;
        public TMP_Text damageText;
    
        [Header("Unit Production")]
        [SerializeField] private GameObject unitProductionPanel;
        [SerializeField] private GameObject unitButtonContainer;
        [SerializeField] private TMP_Text unitSpawnPointBlockedWarningText;
        [SerializeField] private UIButtonDecorator unitButtonDecorator;
    
        [Header("Destroy Button")]
        [SerializeField] private TMP_Text destroyButtonText;
        [SerializeField] private Button destroyButton;

        private UIInformationMenuController InformationMenuController => Controller as UIInformationMenuController;

        private void SetGeneralInformation(GameElement gameElement)
        {
            labelText.text = gameElement.Blueprint.elementName;
            descriptionText.text = gameElement.Blueprint.elementDescription;
            iconImage.sprite = gameElement.Blueprint.uiIcon;
        }
        
        public void DisplayElementInformation(GameElement gameElement)
        {
            ResetInformation();
            menuContent.SetActive(true);
            SetGeneralInformation(gameElement);

            switch (gameElement)
            {
                case Building building: DisplayBuildingInformation(building); break;
                case Unit unit: DisplayUnitInformation(unit); break;
            }
            
            destroyButton.ReplaceButtonClickEvent(() => InformationMenuController.DestroyElement(gameElement));
        }

        private void DisplayBuildingInformation(Building building)
        {
            // Health and Destroy button
            healthObject.SetActive(true);
            healthText.text = building.Health.ToString();
            destroyButtonText.text = "Destruct";
            
            // Production visuals
            var unitSpawnerBuilding = building as UnitSpawnerBuilding;
            var hasProduction = unitSpawnerBuilding != null && unitSpawnerBuilding.BuildingBlueprint.productionData != null;
            unitProductionPanel.SetActive(hasProduction);
            if (hasProduction) UpdateUnitProductionVisuals(unitSpawnerBuilding);
        }

        private void DisplayUnitInformation(Unit unit)
        {
            // Health, Damage and Destroy button
            healthObject.SetActive(true);
            healthText.text = unit.Health.ToString();
            damageObject.SetActive(true);
            damageText.text = unit.Damage.ToString();
            destroyButtonText.text = "Kill";
        }
        
        public void HideInformation()
        {
            ResetInformation();
            menuContent.SetActive(false);
        }

        private void ResetInformation()
        {
            labelText.text = string.Empty;
            descriptionText.text = string.Empty;
            iconImage.sprite = null;   
            healthObject.SetActive(false);
            damageObject.SetActive(false);
            unitProductionPanel.SetActive(false);
        }

        private void UpdateUnitProductionVisuals(UnitSpawnerBuilding unitSpawnerBuilding)
        {
            // Clear previous buttons
            unitButtonContainer.transform.ClearChildren();

            var isSpawnPointEmpty = unitSpawnerBuilding.IsSpawnPointEmpty();
            unitSpawnPointBlockedWarningText.gameObject.SetActive(!isSpawnPointEmpty);
            
            // Create new buttons
            foreach (var unitBlueprint in unitSpawnerBuilding.BuildingBlueprint.productionData.blueprints)
            {
                var spawnedDecorator = Instantiate(unitButtonDecorator, unitButtonContainer.transform);
                spawnedDecorator.UpdateVisuals(unitBlueprint.elementName, unitBlueprint.uiIcon);
                spawnedDecorator.UpdateOnClickEvent(() => InformationMenuController.ProduceUnit(unitSpawnerBuilding, unitBlueprint));
                spawnedDecorator.SetInteractable(isSpawnPointEmpty);
            }
        }
    }
}
