using System;
using BuildingPlacementSystem;
using Buildings;
using UI.Views;
using UnityEngine;

namespace UI.Controllers
{
    /// <summary>
    /// This controller handles UI functions of Production Menu
    /// </summary>
    public class UIProductionMenuController : MonoBehaviour
    {
        public UIProductionMenuView view;
        private void Awake() => view.SetController(this);

        public void OnBuildingButtonClicked(BuildingBlueprint building)
        {
            if (building == null) BuildingPlacementManager.Instance.ExitBuildMode();
            else BuildingPlacementManager.Instance.EnterBuildMode();
            
            BuildingPlacementManager.Instance.SelectBuildingData(building);
        }
    }
}
