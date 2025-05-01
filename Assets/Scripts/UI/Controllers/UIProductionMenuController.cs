using Blueprints;
using PlacementSystem;

namespace UI.Controllers
{
    /// <summary>
    /// Controls the UI behavior of the Production Menu.
    /// Handles building button clicks and triggers placement mode via the <see cref="PlacementManager"/>
    /// </summary>
    public class UIProductionMenuController : UIControllerBase
    {
        /// <summary>
        /// Called when a building button is clicked in the UI.
        /// Activates or deactivates placement mode depending on the selected blueprint.
        /// </summary>
        /// <param name="building">The blueprint of the building to place.</param>
        public void OnBuildingButtonClicked(GameElementBlueprint building)
        {
            if (building == null) return;
            PlacementManager.Instance.EnterPlacementMode();
            PlacementManager.Instance.SelectElementData(building);
        }
    }
}