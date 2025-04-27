using BuildingPlacementSystem.Models;
using UnityEngine.Tilemaps;

namespace BuildingPlacementSystem.TilemapLayers
{
    public class CollisionLayer : TilemapLayer
    {
        public TileBase collisionTileBase;

        public void SetCollisions(Building building, bool value)
        {
            var tile = value ? collisionTileBase : null;
            building.IterateCollisionSpace(tileCoordinates => Tilemap.SetTile(tileCoordinates, tile));
        }
    }
}
