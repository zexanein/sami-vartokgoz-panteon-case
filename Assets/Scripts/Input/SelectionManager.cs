using System;
using Buildings;
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
    
    public delegate void OnElementSelectedHandler(GameElement element);
    public OnElementSelectedHandler OnElementSelected;
    
    public delegate void OnNothingSelectedHandler();
    public OnNothingSelectedHandler OnNothingSelected;
    
    public GameElement SelectedElement { get; private set; }
    public GameElement LastSelectedElement { get; private set; }

    private void OnEnable()
    {
        PlacementManager.Instance.OnEnterPlacementMode += NothingSelected;
    }

    private void OnDisable()
    {
        PlacementManager.Instance.OnEnterPlacementMode -= NothingSelected;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (PlacementManager.Instance.InPlacementMode) return;
            if (InputManager.PointerOverUI) return;
            var hit = Physics2D.Raycast(InputManager.MouseWorldPosition, Vector2.zero);
            
            if (hit.collider == null) NothingSelected();
            else if (hit.collider.TryGetComponent(out GameElement element)) ElementSelected(element);
        }

        if (Input.GetMouseButtonDown(1) && SelectedElement is Unit selectedUnit)
        {
            selectedUnit.MoveToPosition(InputManager.MouseWorldPosition);
        }
    }
    
    private void ElementSelected(GameElement element)
    {
        LastSelectedElement = SelectedElement;
        SelectedElement = element;

        if (LastSelectedElement != null && LastSelectedElement != SelectedElement)
        {
            LastSelectedElement.OnDeselected();
        }

        SelectedElement.OnSelected();
        OnElementSelected?.Invoke(element);
    }

    private void NothingSelected()
    {
        LastSelectedElement = SelectedElement;
        
        if (SelectedElement != null)
        {
            SelectedElement.OnDeselected();
        }
        
        SelectedElement = null;
        OnNothingSelected?.Invoke();
    }
}
