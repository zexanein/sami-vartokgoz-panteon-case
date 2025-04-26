using BuildingSystem.Models;
using UnityEngine.Tilemaps;

namespace BuildingSystem.TilemapLayers
{
    public class CollisionLayer : TilemapLayer
    {
        public TileBase collisionTileBase;

        public void SetCollisions(Buildable buildable, bool value)
        {
            var tile = value ? collisionTileBase : null;
            buildable.IterateCollisionSpace(tileCoordinates => Tilemap.SetTile(tileCoordinates, tile));
        }
    }
}
