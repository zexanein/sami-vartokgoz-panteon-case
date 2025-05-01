using Extensions;
using GameElements;
using PlacementSystem;
using UnityEngine;

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
    
    public delegate void OnSelectableSelectedHandler(ISelectable selectable);
    public event OnSelectableSelectedHandler OnSelectableSelected;
    
    public delegate void OnNothingSelectedHandler();
    public event OnNothingSelectedHandler OnNothingSelected;

    private ISelectable SelectedSelectable { get; set; }
    private ISelectable LastSelectedSelectable { get; set; }

    private void OnEnable()
    {
        if (PlacementManager.Instance == null) return;
        PlacementManager.Instance.OnPlaceModeChanged += OnPlaceModeChanged;
        PlacementManager.Instance.OnElementBuilt += OnElementBuilt;
    }

    private void OnDisable()
    {
        if (PlacementManager.Instance == null) return;
        PlacementManager.Instance.OnPlaceModeChanged -= OnPlaceModeChanged;
        PlacementManager.Instance.OnElementBuilt -= OnElementBuilt;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (PlacementManager.Instance.InPlacementMode) return;
            if (InputManager.PointerOverUI) return;
            var hit = Physics2D.Raycast(InputManager.MouseWorldPosition, Vector2.zero);
            
            if (hit.collider == null) NothingSelected();
            else if (hit.collider.TryGetComponent(out ISelectable selectable)) SelectableSelected(selectable);
        }

        if (Input.GetMouseButtonDown(1) && !Utils.IsUnityObjectNull(SelectedSelectable))
        {
            if (PlacementManager.Instance.InPlacementMode) return;
            if (InputManager.PointerOverUI) return;
            var hit = Physics2D.Raycast(InputManager.MouseWorldPosition, Vector2.zero);
            
            ISelectable otherSelectable = null;
            if (hit.collider != null) hit.collider.TryGetComponent(out otherSelectable);
            SelectedSelectable.InteractWithOther(InputManager.MouseWorldPosition, otherSelectable);
        }
    }

    private void OnElementBuilt(GameElement element) => SelectableSelected(element);

    private void OnPlaceModeChanged(bool state)
    {
        if (!state) NothingSelected();
    }
    
    private void SelectableSelected(ISelectable selectable)
    {
        LastSelectedSelectable = SelectedSelectable;
        SelectedSelectable = selectable;

        if (!Utils.IsUnityObjectNull(LastSelectedSelectable) && LastSelectedSelectable != SelectedSelectable)
        {
            LastSelectedSelectable.Deselect();
        }

        SelectedSelectable.Select();
        OnSelectableSelected?.Invoke(selectable);
    }

    private void NothingSelected()
    {
        LastSelectedSelectable = SelectedSelectable;
        
        if (!Utils.IsUnityObjectNull(SelectedSelectable))
        {
            SelectedSelectable.Deselect();                          
        }                                                        
                                                                 
        SelectedSelectable = null;                               
        OnNothingSelected?.Invoke();
    }
}
