using BuildingPlacementSystem;
using Buildings;
using UI.Controllers;
using Units;
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
    
    public delegate void OnBuildingSelectedHandler(Building building);
    public OnBuildingSelectedHandler OnBuildingSelected;
    
    public delegate void OnUnitSelectedHandler(Unit unit);
    public OnUnitSelectedHandler OnUnitSelected;
    
    public delegate void OnNothingSelectedHandler();
    public OnNothingSelectedHandler OnNothingSelected;
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (BuildingPlacementManager.Instance.InBuildMode) return;
            if (InputManager.PointerOverUI) return;
            var hit = Physics2D.Raycast(InputManager.MouseWorldPosition, Vector2.zero);
            
            if (hit.collider == null) OnNothingSelected?.Invoke();
            else if (hit.collider.TryGetComponent(out Building building)) OnBuildingSelected?.Invoke(building);
            else if (hit.collider.TryGetComponent(out Unit unit)) OnUnitSelected?.Invoke(unit); 
        }
    }
}
