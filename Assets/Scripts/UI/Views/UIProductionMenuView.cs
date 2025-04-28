using PlacementSystem;
using UI.Controllers;
using UI.Decorators;
using UnityEngine;

namespace UI.Views
{
    /// <summary>
    /// This class handles visuals of Production Menu
    /// </summary>
    public class UIProductionMenuView : UIViewBase
    {
        [Header("References")]
        public UIButtonDecorator buildingButtonDecorator;
        public Transform buildingButtonsContainer;

        private UIProductionMenuController ProductionMenuController => Controller as UIProductionMenuController;

        private void Start() => SpawnBuildingButtons();

        private void SpawnBuildingButtons()
        {
            foreach (var building in PlacementManager.Instance.blueprints)
            {
                var spawnedDecorator = Instantiate(buildingButtonDecorator, buildingButtonsContainer);
                spawnedDecorator.UpdateVisuals(building.elementName, building.uiIcon);
                spawnedDecorator.UpdateOnClickEvent(() => ProductionMenuController.OnBuildingButtonClicked(building));
            }
        }
    }
}
