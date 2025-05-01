using Blueprints;
using GameElements;
using UI.Views;

namespace UI.Controllers
{
    /// <summary>
    /// Controls the UI information menu shown when a game element is selected.
    /// Manages interactions like displaying stats, producing units, or destroying elements.
    /// </summary>
    public class UIInformationMenuController : UIControllerBase
    {
        /// <summary>
        /// Casts the base view to a <see cref="UIInformationMenuView"/>.
        /// </summary>
        private UIInformationMenuView InformationMenuView => view as UIInformationMenuView;

        /// <summary>
        /// The currently selected and displayed game element.
        /// </summary>
        private GameElement _inspectingElement;

        private void OnEnable()
        {
            if (SelectionManager.Instance == null) return;
            SelectionManager.Instance.OnSelectableSelected += OnSelectableSelected;
            SelectionManager.Instance.OnNothingSelected += OnNothingSelected;
        }

        private void OnDisable()
        {
            if (SelectionManager.Instance == null) return;
            SelectionManager.Instance.OnSelectableSelected -= OnSelectableSelected;
            SelectionManager.Instance.OnNothingSelected -= OnNothingSelected;
        }

        /// <summary>
        /// Destroys the specified game element.
        /// </summary>
        /// <param name="element">The element to destroy.</param>
        public void DestroyElement(GameElement element) => element.DestroyElement();

        /// <summary>
        /// Commands a building to spawn a unit and refreshes the UI to reflect any changes.
        /// </summary>
        /// <param name="unitSpawnerBuilding">The building that will spawn the unit.</param>
        /// <param name="unit">The blueprint of the unit to produce.</param>
        public void ProduceUnit(UnitSpawnerBuilding unitSpawnerBuilding, UnitBlueprint unit)
        {
            unitSpawnerBuilding.SpawnUnit(unit);
            InformationMenuView.DisplayElementInformation(_inspectingElement);
        }

        /// <summary>
        /// Called when the inspected element is destroyed. Hides the information panel.
        /// </summary>
        private void OnInspectingElementDestroyed(GameElement inspectingElement) =>
            InformationMenuView.HideInformation();

        /// <summary>
        /// Called when the inspected element takes damage. Updates the health display.
        /// </summary>
        private void OnInspectingElementHealthChanged() =>
            InformationMenuView.SetHealthText(_inspectingElement.Health);

        /// <summary>
        /// Called when a selectable object is selected.
        /// Displays its information if it's a game element.
        /// </summary>
        /// <param name="selectable">The selected object.</param>
        private void OnSelectableSelected(ISelectable selectable)
        {
            if (selectable is not GameElement element) return;

            RemoveEventListenersFromInspectingElement();
            _inspectingElement = element;
            AddEventListenersToInspectingElement();

            InformationMenuView.DisplayElementInformation(element);
        }

        /// <summary>
        /// Called when no object is selected. Clears the information panel.
        /// </summary>
        private void OnNothingSelected()
        {
            RemoveEventListenersFromInspectingElement();
            _inspectingElement = null;
            InformationMenuView.HideInformation();
        }

        /// <summary>
        /// Subscribes to events (OnDamaged, OnDestroyed) for the currently displayed element.
        /// </summary>
        private void AddEventListenersToInspectingElement()
        {
            if (_inspectingElement == null) return;
            _inspectingElement.OnHealthChanged += OnInspectingElementHealthChanged;
            _inspectingElement.OnElementDestroyed += OnInspectingElementDestroyed;
        }

        /// <summary>
        /// Unsubscribes from events for the currently displayed element.
        /// </summary>
        private void RemoveEventListenersFromInspectingElement()
        {
            if (_inspectingElement == null) return;
            _inspectingElement.OnHealthChanged -= OnInspectingElementHealthChanged;
            _inspectingElement.OnElementDestroyed -= OnInspectingElementDestroyed;
        }
    }
}
