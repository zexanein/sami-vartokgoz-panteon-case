using Blueprints;
using PlacementSystem;
using PlacementSystem.TilemapLayers;
using UnityEngine;

namespace GameElements
{
    public class UnitSpawnerBuilding : Building
    {
        public Transform unitSpawnPoint;
        public CollisionLayer collisionLayer;

        protected override void OnInitialize()
        {
            // Set position of unit spawn point
            var unitSpawnPointCellPosition = Coordinates + (Vector3Int) BuildingBlueprint.unitSpawnPointCoordinates;
            unitSpawnPoint.transform.position = ParentTilemap.CellToWorld(unitSpawnPointCellPosition) + ParentTilemap.cellSize / 2;
        }

        public void SpawnUnit(UnitBlueprint unitBlueprint)
        {
            PlacementManager.Instance.Place(unitSpawnPoint.position, unitBlueprint);
        }

        public bool IsSpawnPointEmpty()
        {
            var hit = Physics2D.OverlapCircle(unitSpawnPoint.position, 0.1f);
            return hit == null;
        }
    }
}
