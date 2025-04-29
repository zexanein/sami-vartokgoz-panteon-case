using Blueprints;
using PlacementSystem;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace GameElements
{
    public class GameElement : MonoBehaviour
    {
        private Vector3Int _coordinates;
        public Vector3Int Coordinates
        {
            get => _coordinates;
            set
            {
                if (value != _coordinates) OnCoordinatesChanged?.Invoke(this, (Vector2Int) _coordinates, (Vector2Int) value);
                _coordinates = value;
            }
        }
        public GameElementBlueprint Blueprint { get; private set; }
        public Tilemap ParentTilemap { get; private set; }
        protected SpriteRenderer SpriteRenderer { get; private set; }
        public int Health { get; private set; }
        
        public delegate void OnCoordinatesChangedHandler(GameElement element, Vector2Int oldCoordinates, Vector2Int newCoordinates);
        public event OnCoordinatesChangedHandler OnCoordinatesChanged;
        
        public void Initialize(GameElementBlueprint blueprint, Vector3Int coordinates, Tilemap parentTilemap)
        {
            Blueprint = blueprint;
            ParentTilemap = parentTilemap;
            Health = blueprint.healthPoints;
            _coordinates = coordinates;
            SpriteRenderer = GetComponent<SpriteRenderer>();
            OnInitialize();
        }
    
        protected virtual void OnInitialize() { }
        public virtual void OnSelected() { }
        public virtual void OnDeselected() { }
        public virtual void SecondaryMouseInteraction(Vector3 mousePosition, GameElement otherElement) { }
    }
}
