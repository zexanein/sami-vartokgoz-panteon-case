using System.Collections.Generic;
using BuildingPlacementSystem.Models;
using Extensions;
using UnityEngine;

namespace BuildingPlacementSystem.TilemapLayers
{
    /// <summary>
    /// This Tilemap Layer handles spawning, collisions and tilemap stuff for building placement.
    /// </summary>
    public class BuildingSystemLayer : TilemapLayer
    {
        private Dictionary<Vector3Int, Building> _buildings = new();
        public CollisionLayer collisionLayer;
        public void Build(Vector3 worldCoordinates, BuildingBlueprint buildingBlueprint)
        {
            if (!(buildingBlueprint != null && buildingBlueprint.buildingPrefab != null)) return;
            
            var coordinates = Tilemap.WorldToCell(worldCoordinates);
            
            var buildingGameObject = Instantiate(
                original: buildingBlueprint.buildingPrefab,
                position: Tilemap.CellToWorld(coordinates) + Tilemap.cellSize / 2 + buildingBlueprint.placementOffset,
                rotation: Quaternion.identity);

            var building = buildingGameObject.AddComponent<Building>();
            
            building.Initialize(buildingBlueprint, coordinates, Tilemap);

            if (buildingBlueprint.useCustomCollisionSpace)
            {
                collisionLayer.SetCollisions(building, true);
                RegisterBuildableCollisionSpace(building);
            }
            
            else _buildings.Add(coordinates, building);
        }

        public void Destroy(Vector3 worldCoordinates)
        {
            var coordinates = Tilemap.WorldToCell(worldCoordinates);
            if (!_buildings.TryGetValue(coordinates, out var selectableBuilding)) return;

            if (selectableBuilding.Blueprint.useCustomCollisionSpace)
            {
                collisionLayer.SetCollisions(selectableBuilding, false);
                UnregisterBuildableCollisionSpace(selectableBuilding);
            }

            _buildings.Remove(coordinates);
            selectableBuilding.Destroy();
        }

        public bool IsCoordinatesValid(Vector3 worldCoordinates, RectInt collisionSpace = default)
        {
            var coordinates = Tilemap.WorldToCell(worldCoordinates);
            if (!collisionSpace.Equals(default))
            {
                return !IsRectOccupied(coordinates, collisionSpace);
            }
            return !_buildings.ContainsKey(coordinates);
        }

        private void RegisterBuildableCollisionSpace(Building building)
        {
            building.IterateCollisionSpace(tileCoordinates =>
            {
                _buildings.Add(tileCoordinates, building);
            });
        }

        private void UnregisterBuildableCollisionSpace(Building building)
        {
            building.IterateCollisionSpace(tileCoordinates =>
            {
                _buildings.Remove(tileCoordinates);
            });
        }

        private bool IsRectOccupied(Vector3Int coordinates, RectInt rect)
        {
            return rect.Iterate(coordinates, tileCoordinates => _buildings.ContainsKey(tileCoordinates));
        }
    }
}
