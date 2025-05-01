using UnityEngine;

/// <summary>
/// This interface defines the contract for selectable objects in the game.
/// </summary>
public interface ISelectable
{
    /// <summary>
    /// Selects the object.
    /// </summary>
    void Select();
    
    /// <summary>
    /// Deselects the object.
    /// </summary>
    void Deselect();
    
    /// <summary>
    /// Interacts with the object using the mouse position.
    /// Additional logic can be implemented in the derived classes.
    /// </summary>
    void InteractWithOther(Vector3 mousePosition, ISelectable other);
}