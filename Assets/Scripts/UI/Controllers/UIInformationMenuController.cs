using System;
using BuildingPlacementSystem;
using Buildings;
using UI.Views;
using Units;
using UnityEngine;

namespace UI.Controllers
{
    public class UIInformationMenuController : MonoBehaviour
    {
        public UIInformationMenuView view;
        private Building _selectedBuilding;

        private void Awake() => view.SetController(this);

        private void OnEnable()
        {
            SelectionManager.Instance.OnBuildingSelected += OnBuildingSelected;
            SelectionManager.Instance.OnNothingSelected += OnNothingSelected;
            SelectionManager.Instance.OnUnitSelected += OnUnitSelected;
        }

        private void OnDisable()
        {
            SelectionManager.Instance.OnBuildingSelected -= OnBuildingSelected;
            SelectionManager.Instance.OnNothingSelected -= OnNothingSelected;
            SelectionManager.Instance.OnUnitSelected -= OnUnitSelected;
        }

        public void DemolishBuilding(Building building)
        {
            BuildingPlacementManager.Instance.buildingSystemLayer.Destroy((Vector2Int) building.Coordinates);
            view.HideInformation();
        }

        public void OnProduceUnitButtonClicked(UnitBlueprint unit)
        {
            if (_selectedBuilding is not UnitSpawnerBuilding unitSpawnerBuilding) return;
            unitSpawnerBuilding.SpawnUnit(unit);
            view.HideInformation();
        }

        public bool IsUnitSpawnPointValid(Building building)
        {
            return building is not UnitSpawnerBuilding unitSpawnerBuilding || unitSpawnerBuilding.IsSpawnPointValid();
        }

        private void OnBuildingSelected(Building building)
        {
            _selectedBuilding = building;
            view.DisplayInformation(building);
        }

        private void OnUnitSelected(Unit unit)
        {
            
        }

        private void OnNothingSelected()
        {
            _selectedBuilding = null;
            view.HideInformation();
        }
    }
}
