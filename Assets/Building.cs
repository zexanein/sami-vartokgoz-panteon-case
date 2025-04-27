using BuildingPlacementSystem.Models;
using Extensions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class Building : MonoBehaviour
{
    public BuildingBlueprint Blueprint { get; private set; }
    public Tilemap ParentTilemap { get; private set; }
    public Vector3Int Coordinates { get; private set; }

    public void Initialize(BuildingBlueprint blueprint, Vector3Int coordinates, Tilemap parentTilemap)
    {
        Blueprint = blueprint;
        ParentTilemap = parentTilemap;
        Coordinates = coordinates;
    }

    public void IterateCollisionSpace(ExtensionMethods.RectAction action)
    {
        Blueprint.collisionSpace.Iterate(Coordinates, action);
    }

    public bool IterateCollisionSpace(ExtensionMethods.RectActionBool action)
    {
        return Blueprint.collisionSpace.Iterate(Coordinates, action);
    }

    public void Destroy() => Destroy(gameObject);
}