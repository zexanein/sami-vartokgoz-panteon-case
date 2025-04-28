using Buildings;
using GameElements;
using PlacementSystem;
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
    
    public delegate void OnElementSelectedHandler(GameElement element);
    public OnElementSelectedHandler OnElementSelected;
    
    public delegate void OnNothingSelectedHandler();
    public OnNothingSelectedHandler OnNothingSelected;
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (PlacementManager.Instance.InPlacementMode) return;
            if (InputManager.PointerOverUI) return;
            var hit = Physics2D.Raycast(InputManager.MouseWorldPosition, Vector2.zero);
            
            if (hit.collider == null) OnNothingSelected?.Invoke();
            else if (hit.collider.TryGetComponent(out GameElement element)) OnElementSelected?.Invoke(element);
        }
    }
}
