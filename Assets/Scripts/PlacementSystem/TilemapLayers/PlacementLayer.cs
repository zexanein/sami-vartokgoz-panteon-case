using System;
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
        
        public delegate void OnTileStateChangedEvent(Vector2Int coordinates, bool state);
        public event OnTileStateChangedEvent OnTileStateChanged;
        
        public GameElement PlaceElement(Vector2 worldCoordinates, GameElementBlueprint elementBlueprint)
        {
            if (elementBlueprint == null || elementBlueprint.elementPrefab == null) return null;

            var coordinates = TilemapReference.WorldToCell(worldCoordinates);
            
            //  Instantiate element prefab
            var elementGameObject = Instantiate(
                original: elementBlueprint.elementPrefab,
                position: GetPositionForElement(coordinates, elementBlueprint),
                rotation: Quaternion.identity);

            // Initialize element component
            if (!elementGameObject.TryGetComponent(out GameElement gameElement))
                gameElement = elementGameObject.AddComponent<GameElement>();
            
            gameElement.Initialize(elementBlueprint, coordinates, TilemapReference);
            RegisterArea((Vector2Int) gameElement.Coordinates, gameElement.Blueprint.dimensions, gameElement);

            gameElement.OnCoordinatesChanged += OnElementCoordinatesChanged;
            return gameElement;
        }

        private void OnElementCoordinatesChanged(GameElement element, Vector2Int oldCoordinates, Vector2Int newCoordinates)
        {
            UnregisterArea(oldCoordinates, element.Blueprint.dimensions);
            RegisterArea(newCoordinates, element.Blueprint.dimensions, element);   
        }

        public void Destroy(Vector2 worldCoordinates)
        {
            var coordinates = (Vector2Int) TilemapReference.WorldToCell(worldCoordinates);
            if (!_gameElements.TryGetValue(coordinates, out var gameElement)) return;
            
            gameElement.OnCoordinatesChanged -= OnElementCoordinatesChanged;
            UnregisterArea((Vector2Int) gameElement.Coordinates, gameElement.Blueprint.dimensions);
            
            _gameElements.Remove(coordinates);
            Destroy(gameElement.gameObject);
        }

        public bool AreCoordinatesEmpty(Vector2 worldCoordinates, Vector2Int dimensions)
        {
            var coordinates = (Vector2Int) TilemapReference.WorldToCell(worldCoordinates);
            return !IsAreaOccupied(coordinates, dimensions);
        }

        public void RegisterElement(GameElement gameElement) => RegisterArea((Vector2Int) gameElement.Coordinates, gameElement.Blueprint.dimensions, gameElement); 

        public void RegisterArea(Vector2Int coordinates, Vector2Int dimensions, GameElement gameElement)
        {
            dimensions.Iterate(coordinates, tileCoordinates =>
            {
                _gameElements.TryAdd(tileCoordinates, gameElement);
                OnTileStateChanged?.Invoke(tileCoordinates, true);
            });
        }

        public void UnregisterArea(Vector2Int coordinates, Vector2Int dimensions)
        {
            dimensions.Iterate(coordinates, tileCoordinates =>
            {
                _gameElements.Remove(tileCoordinates);
                OnTileStateChanged?.Invoke(tileCoordinates, false);
            });
        }

        public bool IsAreaOccupied(Vector2 worldCoordinates, Vector2Int dimensions)
        {
            var coordinates = (Vector2Int) TilemapReference.WorldToCell(worldCoordinates);
            return dimensions.Iterate(coordinates, tileCoordinates => _gameElements.ContainsKey(tileCoordinates));
        }
    }
}
