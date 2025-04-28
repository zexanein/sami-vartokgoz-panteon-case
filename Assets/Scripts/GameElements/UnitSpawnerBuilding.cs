using Blueprints;
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
            if (unitBlueprint.elementPrefab == null) return;
            if (!IsSpawnPointEmpty())
            {
                Debug.Log("<color=red>SpawnPoint is not empty!</color>");
                return;
            }
            
            var unitGameObject = Instantiate(unitBlueprint.elementPrefab, unitSpawnPoint.position, Quaternion.identity);
            
            if (!unitGameObject.TryGetComponent(out Unit unitComponent))
                unitComponent = unitGameObject.AddComponent<Unit>();
            
            unitComponent.Initialize(unitBlueprint, ParentTilemap.WorldToCell(unitGameObject.transform.position), ParentTilemap);
            //BuildingPlacementManager.Instance.buildingSystemLayer.collisionLayer.SetCollisions(unitComponent, true);
        }

        public bool IsSpawnPointEmpty()
        {
            var hit = Physics2D.OverlapCircle(unitSpawnPoint.position, 0.1f);
            return hit == null;
        }
    }
}
