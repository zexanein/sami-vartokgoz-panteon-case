using UnityEngine;
using Extensions;
using UnityEngine.EventSystems;

/// <summary>
/// This class manages input events and provides utility methods for handling mouse input.
/// </summary>
public class InputManager : MonoBehaviour
{
    /// <summary>
    /// Gets the current mouse position in world coordinates.
    /// </summary>
    /// <returns>
    /// A <c>Vector2</c> representing the mouse position in world coordinates if main camera exists
    /// <c>Vector2.zero</c> otherwise.
    /// </returns>
    public static Vector2 MouseWorldPosition => Camera.main == null ? Vector2.zero : Camera.main.ScreenToWorldPoint(Input.mousePosition);
    
    /// <summary>
    /// Checks whether the mouse pointer is currently over any UI element.
    /// Useful for preventing game actions when interacting with UI.
    /// </summary>
    /// <returns>
    /// <c>true</c> if the pointer is over a UI object in the EventSystem,
    /// <c>false</c> otherwise.
    /// </returns>
    public static bool PointerOverUI => EventSystem.current.IsPointerOverGameObject();
}
