using Blueprints;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace PlacementSystem.TilemapLayers
{
    [RequireComponent(typeof(Tilemap))]
    public class TilemapLayer : MonoBehaviour
    {
        protected Tilemap TilemapReference { get; private set; }

        protected void Awake() => TilemapReference = GetComponent<Tilemap>();

        protected Vector3 GetPositionForElement(Vector3Int coordinates, GameElementBlueprint elementBlueprint)
        {
            return elementBlueprint switch
            {
                BuildingBlueprint buildingBlueprint => GetPositionForBuilding(coordinates, buildingBlueprint),
                UnitBlueprint unitBlueprint => GetPositionForUnit(coordinates, unitBlueprint),
                _ => Vector3.zero
            };
        }

        private Vector3 GetPositionForBuilding(Vector3Int coordinates, BuildingBlueprint buildingBlueprint)
        {
            return
                TilemapReference.CellToWorld(coordinates) +
                TilemapReference.cellSize / 2 +
                (Vector3) buildingBlueprint.CenterOffset +
                (Vector3Int) buildingBlueprint.placementOffset;
        }

        private Vector3 GetPositionForUnit(Vector3Int coordinates, UnitBlueprint unitBlueprint)
        {
            return
                TilemapReference.CellToWorld(coordinates) +
                TilemapReference.cellSize / 2;
        }
    }
}
