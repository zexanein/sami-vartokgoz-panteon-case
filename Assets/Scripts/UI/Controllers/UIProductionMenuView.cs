using BuildingPlacementSystem;
using UI.Decorators;
using UnityEngine;

namespace UI.Controllers
{
    /// <summary>
    /// This class handles visuals of Production Menu
    /// </summary>
    public class UIProductionMenuView : MonoBehaviour
    {
        public UIProductionMenuController controller;
        public UIBuildingSelectButtonDecorator buildingSelectButton;
        public Transform buildingButtonsContainer;
        public Sprite noneBuildingSprite;

        private void Start()
        {
            SpawnBuildingButtons();
        }

        private void SpawnBuildingButtons()
        {
            // var noneButton = Instantiate(buildingSelectButton, buildingButtonsContainer);
            // noneButton.UpdateVisuals("None", noneBuildingSprite);
            // noneButton.UpdateOnClickEvent(() => controller.OnBuildingButtonClicked(null));
        
            foreach (var building in BuildingPlacementManager.Instance.buildingBlueprints)
            {
                var spawnedButton = Instantiate(buildingSelectButton, buildingButtonsContainer);
                spawnedButton.UpdateVisuals(building.buildingName, building.uiIcon);
                spawnedButton.UpdateOnClickEvent(() => controller.OnBuildingButtonClicked(building));
            }
        }
    }
}
