using Buildings;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace BuildingPlacementSystem.TilemapLayers
{
    [RequireComponent(typeof(Tilemap))]
    public class TilemapLayer : MonoBehaviour
    {
        protected Tilemap Tilemap { get; private set; }

        protected void Awake()
        {
            Tilemap = GetComponent<Tilemap>();
        }

        protected Vector3 GetPositionForBuilding(Vector3Int coordinates, BuildingBlueprint buildingBlueprint)
        {
            return
                Tilemap.CellToWorld(coordinates) +
                Tilemap.cellSize / 2 +
                (Vector3) buildingBlueprint.CenterOffset +
                (Vector3Int) buildingBlueprint.placementOffset;
        }
    }
}
