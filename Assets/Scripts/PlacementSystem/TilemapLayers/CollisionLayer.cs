using GameElements;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace PlacementSystem.TilemapLayers
{
    public class CollisionLayer : TilemapLayer
    {
        public TileBase collisionTileBase;

        public void SetCollisions(GameElement gameElement, bool value)
        {
            var tile = value ? collisionTileBase : null;
            gameElement.IterateOccupiedTiles(tileCoordinates => Tilemap.SetTile((Vector3Int) tileCoordinates, tile));
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
