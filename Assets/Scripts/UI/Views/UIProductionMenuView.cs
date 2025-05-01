using PlacementSystem;
using UI.Controllers;
using UI.Decorators;
using UnityEngine;

namespace UI.Views
{
    /// <summary>
    /// Handles the visual representation of the Production Menu UI.
    /// Dynamically spawns buttons for available buildings using blueprints from the <see cref="PlacementManager"/>.
    /// </summary>
    public class UIProductionMenuView : UIViewBase
    {

        /// <summary>
        /// The prefab used to instantiate each building button.
        /// </summary>
        [Header("References")]
        public UIButtonDecorator buildingButtonDecorator;

        /// <summary>
        /// The parent transform where building buttons will be instantiated.
        /// </summary>
        public Transform buildingButtonsContainer;

        /// <summary>
        /// Casts the base controller to a <see cref="UIProductionMenuController"/>.
        /// </summary>
        private UIProductionMenuController ProductionMenuController => Controller as UIProductionMenuController;

        /// <summary>
        /// Called on Start. Automatically populates the UI with available building buttons.
        /// </summary>
        private void Start() => SpawnBuildingButtons();

        /// <summary>
        /// Instantiates buttons for all building blueprints listed in the <see cref="PlacementManager"/>.
        /// Each button is configured with visuals and an associated click event.
        /// </summary>
        private void SpawnBuildingButtons()
        {
            foreach (var building in PlacementManager.Instance.blueprints)
            {
                var spawnedDecorator = Instantiate(buildingButtonDecorator, buildingButtonsContainer);
                spawnedDecorator.UpdateVisuals(building.elementName, building.uiIcon);
                spawnedDecorator.SetOnClickEvent(() => ProductionMenuController.OnBuildingButtonClicked(building));
            }
        }
    }
}