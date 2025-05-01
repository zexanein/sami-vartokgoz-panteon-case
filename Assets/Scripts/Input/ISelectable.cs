using UnityEngine;

public interface ISelectable
{
    void Select();
    void Deselect();
    void InteractWithOther(Vector3 mousePosition, ISelectable other);
}