using BuildingSystem;
using UI.Decorators;
using UnityEngine;

namespace UI.Controllers
{
    public class UIProductionMenuController : MonoBehaviour
    {
        public UIBuildingSelectButtonDecorator buildingSelectButton;
        public Transform buildingButtonsContainer;
        public Sprite noneBuildingSprite;

        private void Start()
        {
            SpawnBuildingButtons();
        }

        private void SpawnBuildingButtons()
        {
            var noneButton = Instantiate(buildingSelectButton, buildingButtonsContainer);
            noneButton.InitializeVisuals("None", noneBuildingSprite);
            noneButton.InitializeEvents(null);
        
            foreach (var building in BuildingSystemController.Instance.buildingDataList)
            {
                var spawnedButton = Instantiate(buildingSelectButton, buildingButtonsContainer);
                spawnedButton.InitializeVisuals(building.buildingName, building.displaySprite);
                spawnedButton.InitializeEvents(building);
            }
        }
    }
}
