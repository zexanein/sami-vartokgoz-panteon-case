using UnityEngine;
using Extensions;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    public static Vector2 MouseWorldPosition => Camera.main == null ? Vector3.zero : Camera.main.ScreenToWorldPoint(Input.mousePosition).WithZ(0);
    public static bool PointerOverUI => EventSystem.current.IsPointerOverGameObject();
}
