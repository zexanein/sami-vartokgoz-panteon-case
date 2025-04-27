using BuildingPlacementSystem;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    public UIInformationMenuController informationMenuController;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (BuildingPlacementManager.Instance.InBuildMode) return;
            if (InputManager.PointerOverUI) return;
            var hit = Physics2D.Raycast(InputManager.MouseWorldPosition, Vector2.zero);
            if (hit.collider == null)
            {
                informationMenuController.OnOtherClicked();
                return;
            }
            
            if (!hit.collider.TryGetComponent(out Building selectableBuilding)) return;
            informationMenuController.OnSpawnedBuildingClicked(selectableBuilding);
        }
    }
}
