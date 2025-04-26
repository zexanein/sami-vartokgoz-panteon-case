using UnityEngine;
using Extensions;

public class InputController : MonoBehaviour
{
    public static Vector3 MouseWorldPosition => Camera.main == null ? Vector3.zero : Camera.main.ScreenToWorldPoint(Input.mousePosition).WithZ(0);
}
