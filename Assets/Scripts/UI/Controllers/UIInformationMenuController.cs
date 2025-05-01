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
            if (SelectionManager.Instance != null)
            {
                SelectionManager.Instance.OnSelectableSelected += OnSelectableSelected;
                SelectionManager.Instance.OnNothingSelected += OnNothingSelected;   
            }
        }

        private void OnDisable()
        {
            if (SelectionManager.Instance != null)
            {
                SelectionManager.Instance.OnSelectableSelected -= OnSelectableSelected;
                SelectionManager.Instance.OnNothingSelected -= OnNothingSelected;   
            }
        }

        public void DestroyElement(GameElement element) => element.DestroyElement();

        public void ProduceUnit(UnitSpawnerBuilding unitSpawnerBuilding, UnitBlueprint unit)
        {
            unitSpawnerBuilding.SpawnUnit(unit);
            InformationMenuView.DisplayElementInformation(_currentlyDisplayingElement);
        }

        private void OnInspectingElementDestroyed(GameElement inspectingElement) => InformationMenuView.HideInformation();

        private void OnInspectingElementDamaged() => InformationMenuView.SetHealthText(_currentlyDisplayingElement.Health);

        private void OnSelectableSelected(ISelectable selectable)
        {
            if (selectable is not GameElement element) return;
            
            RemoveEventListenersFromInspectingElement();
            _currentlyDisplayingElement = element;
            AddEventListenersToInspectingElement();
            
            InformationMenuView.DisplayElementInformation(element);
        }

        private void OnNothingSelected()
        {
            RemoveEventListenersFromInspectingElement();
            _currentlyDisplayingElement = null;
            InformationMenuView.HideInformation();
        }

        private void AddEventListenersToInspectingElement()
        {
            if (_currentlyDisplayingElement == null) return;
            _currentlyDisplayingElement.OnDamaged += OnInspectingElementDamaged;
            _currentlyDisplayingElement.OnElementDestroyed += OnInspectingElementDestroyed;
        }

        private void RemoveEventListenersFromInspectingElement()
        {
            if (_currentlyDisplayingElement == null) return; 
            _currentlyDisplayingElement.OnDamaged -= OnInspectingElementDamaged;
            _currentlyDisplayingElement.OnElementDestroyed -= OnInspectingElementDestroyed;
        }
    }
}
