using Extensions;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Buildings
{
    public class Building : MonoBehaviour
    {
        public BuildingBlueprint Blueprint { get; private set; }
        public Tilemap ParentTilemap { get; private set; }
        public Vector3Int Coordinates { get; private set; }

        public void Initialize(BuildingBlueprint blueprint, Vector3Int coordinates, Tilemap parentTilemap)
        {
            Blueprint = blueprint;
            ParentTilemap = parentTilemap;
            Coordinates = coordinates;
        }

        public void IterateAllTiles(ExtensionMethods.Vector2IntAction action)
        {
            Blueprint.dimensions.Iterate((Vector2Int) Coordinates - Blueprint.dimensions / 2, action);
        }

        public void Destroy() => Destroy(gameObject);
    }
}