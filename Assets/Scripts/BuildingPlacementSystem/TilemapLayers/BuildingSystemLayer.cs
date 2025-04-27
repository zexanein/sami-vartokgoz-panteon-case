using System.Collections.Generic;
using Buildings;
using Extensions;
using UnityEngine;

namespace BuildingPlacementSystem.TilemapLayers
{
    /// <summary>
    /// This Tilemap Layer handles spawning, collisions and tilemap stuff for building placement.
    /// </summary>
    public class BuildingSystemLayer : TilemapLayer
    {
        private Dictionary<Vector2Int, Building> _buildings = new();
        public CollisionLayer collisionLayer;
        public void Build(Vector2 worldCoordinates, BuildingBlueprint buildingBlueprint)
        {
            if (buildingBlueprint == null || buildingBlueprint.buildingPrefab == null) return;
            
            var coordinates = Tilemap.WorldToCell(worldCoordinates);
            
            //  Instantiate building object
            var buildingGameObject = Instantiate(
                original: buildingBlueprint.buildingPrefab,
                position: GetPositionForBuilding(coordinates, buildingBlueprint),
                rotation: Quaternion.identity);

            // Initialize Building component
            if (!buildingGameObject.TryGetComponent(out Building building))
                building = buildingGameObject.AddComponent<Building>();
            building.Initialize(buildingBlueprint, coordinates, Tilemap);
            
            // Place Unit SpawnPoint
            if (building is UnitSpawnerBuilding unitSpawnerBuilding)
            {
                var unitSpawnPointCellPosition = unitSpawnerBuilding.Coordinates + (Vector3Int) buildingBlueprint.unitSpawnPointCoordinates;
                unitSpawnerBuilding.unitSpawnPoint.transform.position = Tilemap.CellToWorld(unitSpawnPointCellPosition) + Tilemap.cellSize / 2;
            }
            
            collisionLayer.SetCollisions(building, true);
            RegisterBuildableTiles(building);
        }

        public void Destroy(Vector2 worldCoordinates)
        {
            var coordinates = (Vector2Int) Tilemap.WorldToCell(worldCoordinates);
            if (!_buildings.TryGetValue(coordinates, out var selectableBuilding)) return;
            collisionLayer.SetCollisions(selectableBuilding, false);
            UnregisterBuildableTiles(selectableBuilding);
            _buildings.Remove(coordinates);
            selectableBuilding.Destroy();
        }

        public bool AreCoordinatesValid(Vector2 worldCoordinates, Vector2Int dimensions)
        {
            var coordinates = (Vector2Int) Tilemap.WorldToCell(worldCoordinates);
            return !IsRectOccupied(coordinates, dimensions);
        }

        private void RegisterBuildableTiles(Building building)
        {
            building.IterateAllTiles(tileCoordinates =>
            {
                _buildings.Add(tileCoordinates, building);
            });
        }

        private void UnregisterBuildableTiles(Building building)
        {
            building.IterateAllTiles(tileCoordinates =>
            {
                _buildings.Remove(tileCoordinates);
            });
        }

        private bool IsRectOccupied(Vector2Int coordinates, Vector2Int dimensions)
        {
            return dimensions.Iterate(coordinates - dimensions / 2, tileCoordinates => collisionLayer.HasCollisionTile(tileCoordinates));
        }
    }
}
