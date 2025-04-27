using BuildingPlacementSystem;
using BuildingPlacementSystem.TilemapLayers;
using Units;
using UnityEngine;
 
namespace Buildings
{
    public class UnitSpawnerBuilding : Building
    {
        public Transform unitSpawnPoint;
        public CollisionLayer collisionLayer;
        
        public void SpawnUnit(UnitBlueprint unit)
        {
            if (unit.unitPrefab == null) return;
            if (!IsSpawnPointValid())
            {
                Debug.Log("<color=red>SpawnPoint is not empty!</color>");
                return;
            }
            
            var unitGameObject = Instantiate(unit.unitPrefab, unitSpawnPoint.position, Quaternion.identity);
            
            if (!unitGameObject.TryGetComponent(out Unit unitComponent))
                unitComponent = unitGameObject.AddComponent<Unit>();
            
            unitComponent.Initialize(unit, ParentTilemap.WorldToCell(unitGameObject.transform.position));
            BuildingPlacementManager.Instance.buildingSystemLayer.collisionLayer.SetCollisions(unitComponent, true);
        }

        public bool IsSpawnPointValid()
        {
            var hit = Physics2D.OverlapCircle(unitSpawnPoint.position, 0.1f);
            return hit == null;
        }
    }
}
