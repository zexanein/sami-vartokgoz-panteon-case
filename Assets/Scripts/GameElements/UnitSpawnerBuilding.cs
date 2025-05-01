using Blueprints;
using PlacementSystem;
using UnityEngine;

namespace GameElements
{
    /// <summary>
    /// Represents a building that can spawn units in the game world.
    /// </summary>
    public class UnitSpawnerBuilding : Building
    {
        public Transform unitSpawnPoint;

        /// <summary>
        /// Initializes the unit spawn point position based on the building's coordinates.
        /// </summary>
        protected override void OnInitialize()
        {
            base.OnInitialize();
            
            // Set position of unit spawn point
            var unitSpawnPointCellPosition = Coordinates + (Vector3Int) BuildingBlueprint.unitSpawnPointCoordinates;
            unitSpawnPoint.transform.position = ParentTilemap.CellToWorld(unitSpawnPointCellPosition) + ParentTilemap.cellSize / 2;
        }

        /// <summary>
        /// Spawns a unit at the specified spawn point.
        /// </summary>
        /// <param name="unitBlueprint">The blueprint of the unit to spawn.</param>
        public void SpawnUnit(UnitBlueprint unitBlueprint)
        {
            PlacementManager.Instance.Place(unitSpawnPoint.position, unitBlueprint);
        }

        /// <summary>
        /// Checks if the spawn point is empty (i.e., no other units or buildings are present).
        /// </summary>
        /// <returns><c>true</c> if the spawn point is empty; otherwise, <c>false</c>.</returns>
        public bool IsSpawnPointEmpty()
        {
            var hit = Physics2D.OverlapCircle(unitSpawnPoint.position, 0.1f);
            return hit == null;
        }
    }
}
