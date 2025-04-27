using BuildingPlacementSystem;
using UI.Controllers;
using UI.Decorators;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI.Views
{
    /// <summary>
    /// This class handles visuals of Production Menu
    /// </summary>
    public class UIProductionMenuView : MonoBehaviour
    {
        private UIProductionMenuController _controller;
        
        [Header("References")]
        public UIButtonDecorator buildingButtonDecorator;
        public Transform buildingButtonsContainer;

        public void SetController(UIProductionMenuController controller) => _controller = controller;

        private void Start() => SpawnBuildingButtons();

        private void SpawnBuildingButtons()
        {
            foreach (var building in BuildingPlacementManager.Instance.buildingBlueprints)
            {
                var spawnedDecorator = Instantiate(buildingButtonDecorator, buildingButtonsContainer);
                spawnedDecorator.UpdateVisuals(building.buildingName, building.uiIcon);
                spawnedDecorator.UpdateOnClickEvent(() => _controller.OnBuildingButtonClicked(building));
            }
        }
    }
}
