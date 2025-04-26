using System.Collections.Generic;
using BuildingSystem.Models;
using Extensions;
using UnityEngine;

namespace BuildingSystem.TilemapLayers
{
    public class BuildingSystemLayer : TilemapLayer
    {
        private Dictionary<Vector3Int, Buildable> _buildables = new();
        public CollisionLayer collisionLayer;
        public void Build(Vector3 worldCoordinates, BuildingData building)
        {
            if (!(building != null && building.gameObject != null)) return;
            
            var coordinates = Tilemap.WorldToCell(worldCoordinates);
            
            var buildableObject = Instantiate(
                    original: building.gameObject,
                    position: Tilemap.CellToWorld(coordinates) + Tilemap.cellSize / 2 + building.placementOffset,
                    rotation: Quaternion.identity);
            
            var buildable = new Buildable(building, buildableObject, coordinates, Tilemap);

            if (building.useCustomCollisionSpace)
            {
                collisionLayer.SetCollisions(buildable, true);
                RegisterBuildableCollisionSpace(buildable);
            }
            
            else _buildables.Add(coordinates, buildable);
        }

        public void Destroy(Vector3 worldCoordinates)
        {
            var coordinates = Tilemap.WorldToCell(worldCoordinates);
            if (!_buildables.TryGetValue(coordinates, out var buildable)) return;

            if (buildable.buildableType.useCustomCollisionSpace)
            {
                collisionLayer.SetCollisions(buildable, false);
                UnregisterBuildableCollisionSpace(buildable);
            }

            _buildables.Remove(coordinates);
            buildable.Destroy();
        }

        public bool IsCoordinatesValid(Vector3 worldCoordinates, RectInt collisionSpace = default)
        {
            var coordinates = Tilemap.WorldToCell(worldCoordinates);
            if (!collisionSpace.Equals(default))
            {
                return !IsRectOccupied(coordinates, collisionSpace);
            }
            return !_buildables.ContainsKey(coordinates);
        }

        private void RegisterBuildableCollisionSpace(Buildable buildable)
        {
            buildable.IterateCollisionSpace(tileCoordinates =>
            {
                _buildables.Add(tileCoordinates, buildable);
            });
        }

        private void UnregisterBuildableCollisionSpace(Buildable buildable)
        {
            buildable.IterateCollisionSpace(tileCoordinates =>
            {
                _buildables.Remove(tileCoordinates);
            });
        }

        private bool IsRectOccupied(Vector3Int coordinates, RectInt rect)
        {
            return rect.Iterate(coordinates, tileCoordinates => _buildables.ContainsKey(tileCoordinates));
        }
    }
}
