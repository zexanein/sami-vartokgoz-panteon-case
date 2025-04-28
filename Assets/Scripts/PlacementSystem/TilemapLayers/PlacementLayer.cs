using System.Collections.Generic;
using Blueprints;
using Extensions;
using GameElements;
using UnityEngine;

namespace PlacementSystem.TilemapLayers
{
    /// <summary>
    /// This Tilemap Layer handles spawning, collisions and tilemap stuff for element placement.
    /// </summary>
    public class PlacementLayer : TilemapLayer
    {
        private Dictionary<Vector2Int, GameElement> _gameElements = new();
        [SerializeField] private CollisionLayer collisionLayer;
        
        public bool PlaceElement(Vector2 worldCoordinates, GameElementBlueprint elementBlueprint)
        {
            if (elementBlueprint == null || elementBlueprint.elementPrefab == null) return false;
            
            var coordinates = Tilemap.WorldToCell(worldCoordinates);
            
            //  Instantiate element prefab
            var elementGameObject = Instantiate(
                original: elementBlueprint.elementPrefab,
                position: GetPositionForElement(coordinates, elementBlueprint),
                rotation: Quaternion.identity);

            // Initialize element component
            if (!elementGameObject.TryGetComponent(out GameElement gameElement))
                gameElement = elementGameObject.AddComponent<GameElement>();
            
            gameElement.Initialize(elementBlueprint, coordinates, Tilemap);
            
            collisionLayer.SetCollisions(gameElement, true);
            RegisterElementArea(gameElement);
            return true;
        }

        public void Destroy(Vector2 worldCoordinates)
        {
            var coordinates = (Vector2Int) Tilemap.WorldToCell(worldCoordinates);
            if (!_gameElements.TryGetValue(coordinates, out var gameElement)) return;
            
            collisionLayer.SetCollisions(gameElement, false);
            UnregisterElementArea(gameElement);
            
            _gameElements.Remove(coordinates);
            gameElement.Destroy();
        }

        public bool AreCoordinatesEmpty(Vector2 worldCoordinates, Vector2Int dimensions)
        {
            var coordinates = (Vector2Int) Tilemap.WorldToCell(worldCoordinates);
            return !IsAreaOccupied(coordinates, dimensions);
        }

        private void RegisterElementArea(GameElement gameElement)
        {
            gameElement.IterateOccupiedTiles(tileCoordinates =>
            {
                _gameElements.Add(tileCoordinates, gameElement);
            });
        }

        private void UnregisterElementArea(GameElement gameElement)
        {
            gameElement.IterateOccupiedTiles(tileCoordinates =>
            {
                _gameElements.Remove(tileCoordinates);
            });
        }

        private bool IsAreaOccupied(Vector2Int coordinates, Vector2Int dimensions)
        {
            return dimensions.Iterate(coordinates - dimensions / 2, tileCoordinates => collisionLayer.HasCollisionTile(tileCoordinates));
        }
    }
}
