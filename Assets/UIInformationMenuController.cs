using System;
using System.Collections;
using System.Collections.Generic;
using BuildingPlacementSystem;
using UnityEngine;

public class UIInformationMenuController : MonoBehaviour
{
    public UIInformationMenuView view;

    public void DemolishBuilding(Building building)
    {
        BuildingPlacementManager.Instance.buildingSystemLayer.Destroy(building.Coordinates);
        view.ClearInformation();
    }

    public void OnProduceUnitButtonClicked()
    {
        throw new NotImplementedException();
    }

    public void OnSpawnedBuildingClicked(Building building)
    {
        view.DisplayBuilding(building);
    }

    public void OnOtherClicked()
    {
        view.ClearInformation();
    }
}
