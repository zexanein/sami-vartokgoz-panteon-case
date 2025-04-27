using Buildings;
using Units;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace BuildingPlacementSystem.TilemapLayers
{
    public class CollisionLayer : TilemapLayer
    {
        public TileBase collisionTileBase;

        public void SetCollisions(Building building, bool value)
        {
            var tile = value ? collisionTileBase : null;
            building.IterateAllTiles(tileCoordinates => Tilemap.SetTile((Vector3Int) tileCoordinates, tile));
        }

        public void SetCollisions(Unit unit, bool value)
        {
            var tile = value ? collisionTileBase : null;
            Tilemap.SetTile(unit.Coordinates, tile);
        }

        public bool HasCollisionTile(Vector2Int coordinates)
        {
            return Tilemap.HasTile((Vector3Int) coordinates);
        }
    }
}
