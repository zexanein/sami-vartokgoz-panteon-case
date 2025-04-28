using Blueprints;
using Extensions;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace GameElements
{
    public class GameElement : MonoBehaviour
    {
        public GameElementBlueprint Blueprint { get; private set; }
        public Tilemap ParentTilemap { get; private set; }
        public Vector3Int Coordinates { get; private set; }
        
        public int Health { get; private set; }
    
        public void Initialize(GameElementBlueprint blueprint, Vector3Int coordinates, Tilemap parentTilemap)
        {
            Blueprint = blueprint;
            ParentTilemap = parentTilemap;
            Coordinates = coordinates;
            Health = blueprint.healthPoints;
            OnInitialize();
        }
    
        protected virtual void OnInitialize() { }

        public void IterateOccupiedTiles(ExtensionMethods.Vector2IntAction action)
        {
            Blueprint.dimensions.Iterate((Vector2Int) Coordinates - Blueprint.dimensions / 2, action);
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}
