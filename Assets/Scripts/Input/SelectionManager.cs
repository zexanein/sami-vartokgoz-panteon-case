using Extensions;
using GameElements;
using PlacementSystem;
using UnityEngine;

/// <summary>
/// This class manages the selection of selectable objects in the game.
/// </summary>
public class SelectionManager : MonoBehaviour
{
    #region Singleton
    private static SelectionManager _instance;

    public static SelectionManager Instance
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<SelectionManager>();
            return _instance;
        }
    }
    #endregion

    /// <summary>
    /// This event is triggered when a selectable object is selected.
    /// </summary>
    public delegate void OnSelectableSelectedHandler(ISelectable selectable);
    public event OnSelectableSelectedHandler OnSelectableSelected;

    /// <summary>
    /// This event is triggered when no selectable object is selected.
    /// </summary>
    public delegate void OnNothingSelectedHandler();
    public event OnNothingSelectedHandler OnNothingSelected;

    /// <summary>
    /// The currently selected selectable object.
    /// </summary>
    private ISelectable SelectedSelectable { get; set; }
    
    /// <summary>
    /// The last selected selectable object.
    ///  This is used to deselect the previous selection when a new one is made.
    /// </summary>
    private ISelectable LastSelectedSelectable { get; set; }

    /// <summary>
    /// Subscribes to events from the PlacementManager when the script is enabled.
    /// </summary>
    private void OnEnable()
    {
        if (PlacementManager.Instance == null) return;
        PlacementManager.Instance.OnPlaceModeChanged += OnPlaceModeChanged;
        PlacementManager.Instance.OnElementBuilt += OnElementBuilt;
    }

    /// <summary>
    /// Unsubscribes from events from the PlacementManager when the script is disabled.
    /// </summary>
    private void OnDisable()
    {
        if (PlacementManager.Instance == null) return;
        PlacementManager.Instance.OnPlaceModeChanged -= OnPlaceModeChanged;
        PlacementManager.Instance.OnElementBuilt -= OnElementBuilt;
    }

    private void Update()
    {
        // Check for left mouse button click
        if (Input.GetMouseButtonDown(0))
        {
            // Check if the conditions for selection are met
            if (PlacementManager.Instance.InPlacementMode) return;
            if (InputManager.PointerOverUI) return;
            var hit = Physics2D.Raycast(InputManager.MouseWorldPosition, Vector2.zero);
            
            // Check if the hit object is null or if it has a selectable component
            if (hit.collider == null) NothingSelected();
            
            // If the hit object is not null and has a selectable component, select it
            else if (hit.collider.TryGetComponent(out ISelectable selectable)) SelectableSelected(selectable);
        }

        // Check for right mouse button click
        if (Input.GetMouseButtonDown(1) && !Utils.IsUnityObjectNull(SelectedSelectable))
        {
            // Check if the conditions for selection are met
            if (PlacementManager.Instance.InPlacementMode) return;
            if (InputManager.PointerOverUI) return;
            var hit = Physics2D.Raycast(InputManager.MouseWorldPosition, Vector2.zero);
            
            // Check if the hit object is null or if it has a selectable component
            ISelectable otherSelectable = null;
            
            // If the hit object is not null and has a selectable component, select it
            if (hit.collider != null) hit.collider.TryGetComponent(out otherSelectable);
            SelectedSelectable.InteractWithOther(InputManager.MouseWorldPosition, otherSelectable);
        }
    }

    /// <summary>
    /// This method is called when an element is built in the placement system.
    /// </summary>
    /// <param name="element">The built game element.</param>
    private void OnElementBuilt(GameElement element) => SelectableSelected(element);

    /// <summary>
    /// This method is called when the placement mode enters or exits.
    /// </summary>
    /// <param name="state">The state of the placement mode.</param>
    private void OnPlaceModeChanged(bool state)
    {
        if (!state) NothingSelected();
    }
    
    /// <summary>
    /// This method is called when a selectable object is selected.
    /// </summary>
    /// <param name="selectable">The selectable object to be selected.</param>
    private void SelectableSelected(ISelectable selectable)
    {
        LastSelectedSelectable = SelectedSelectable;
        SelectedSelectable = selectable;

        // Check if the last selected selectable is not null and is different from the current selected selectable and deselect it
        if (!Utils.IsUnityObjectNull(LastSelectedSelectable) && LastSelectedSelectable != SelectedSelectable)
            LastSelectedSelectable.Deselect();

        SelectedSelectable.Select();
        OnSelectableSelected?.Invoke(selectable);
    }

    /// <summary>
    /// This method is called when no selectable object is selected.
    /// </summary>
    private void NothingSelected()
    {
        LastSelectedSelectable = SelectedSelectable;
        if (!Utils.IsUnityObjectNull(SelectedSelectable)) SelectedSelectable.Deselect();             
        SelectedSelectable = null;                               
        OnNothingSelected?.Invoke();
    }
}
