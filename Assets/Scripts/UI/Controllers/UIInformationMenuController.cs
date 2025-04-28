using Blueprints;
using Buildings;
using GameElements;
using PlacementSystem;
using UI.Views;
using UnityEngine;

namespace UI.Controllers
{
    public class UIInformationMenuController : UIControllerBase
    {
        private UIInformationMenuView InformationMenuView => view as UIInformationMenuView;
        private GameElement _currentlyDisplayingElement;

        private void OnEnable()
        {
            PlacementManager.Instance.OnElementPlaced += OnElementPlaced;
            SelectionManager.Instance.OnElementSelected += OnElementSelected;
            SelectionManager.Instance.OnNothingSelected += OnNothingSelected;
        }

        private void OnDisable()
        {
            if (SelectionManager.Instance != null)
            {
                SelectionManager.Instance.OnElementSelected -= OnElementSelected;
                SelectionManager.Instance.OnNothingSelected -= OnNothingSelected;   
            }

            if (PlacementManager.Instance != null)
            {
                PlacementManager.Instance.OnElementPlaced -= OnElementPlaced;
            }
        }

        public void DestroyElement(GameElement element)
        {
            PlacementManager.Instance.DestroyElementFrom(element.Coordinates);
            InformationMenuView.HideInformation();
        }

        public void ProduceUnit(UnitSpawnerBuilding unitSpawnerBuilding, UnitBlueprint unit)
        {
            unitSpawnerBuilding.SpawnUnit(unit);
            InformationMenuView.HideInformation();
        }

        private void OnElementPlaced()
        {
            if (_currentlyDisplayingElement == null) return;
            InformationMenuView.DisplayElementInformation(_currentlyDisplayingElement);
        }

        private void OnElementSelected(GameElement element)
        {
            _currentlyDisplayingElement = element;
            InformationMenuView.DisplayElementInformation(element);
        }

        private void OnNothingSelected()
        {
            _currentlyDisplayingElement = null;
            InformationMenuView.HideInformation();
        }
    }
}
