using Blueprints;
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
            PlacementManager.Instance.OnElementBuilt += OnElementBuilt;
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
                PlacementManager.Instance.OnElementBuilt -= OnElementBuilt;
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
            InformationMenuView.DisplayElementInformation(_currentlyDisplayingElement);
        }

        private void OnElementBuilt(GameElement element)
        {
            InformationMenuView.DisplayElementInformation(element);
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
