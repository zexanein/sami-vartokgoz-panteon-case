using Blueprints;
using PlacementSystem;
using UI.Views;
using UnityEngine;

namespace UI.Controllers
{
    /// <summary>
    /// This controller handles UI functions of Production Menu
    /// </summary>
    public class UIProductionMenuController : UIControllerBase
    {
        public void OnBuildingButtonClicked(GameElementBlueprint building)
        {
            if (building == null) PlacementManager.Instance.ExitPlacementMode();
            else PlacementManager.Instance.EnterPlacementMode();
            
            PlacementManager.Instance.SelectElementData(building);
        }
    }
}
