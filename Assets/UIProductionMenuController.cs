using BuildingPlacementSystem;
using BuildingPlacementSystem.Models;
using UnityEngine;

/// <summary>
/// This controller handles UI functions of Production Menu
/// </summary>
public class UIProductionMenuController : MonoBehaviour
{
    public void OnBuildingButtonClicked(BuildingBlueprint building)
    {
        if (building == null) BuildingPlacementManager.Instance.ExitBuildMode();
        else BuildingPlacementManager.Instance.EnterBuildMode();
            
        BuildingPlacementManager.Instance.SelectBuildingData(building);
    }
}
