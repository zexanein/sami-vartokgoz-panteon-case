using System;
using System.Collections.Generic;
using Blueprints;
using GameElements;
using PlacementSystem.TilemapLayers;
using UnityEngine;

namespace PlacementSystem
{
    /// <summary>
    /// This manager handles placement interactions and logic
    /// </summary>
    public class PlacementManager : MonoBehaviour
    {
        #region Singleton
        private static PlacementManager _instance;
        public static PlacementManager Instance
        {
            get
            {
                if (_instance == null) _instance = FindObjectOfType<PlacementManager>();
                return _instance;
            }
        }
        #endregion
        
        [Header("References")]
        [SerializeField] private PlacementLayer placementLayer;
        [SerializeField] private PreviewLayer previewLayer;
        public List<GameElementBlueprint> blueprints = new();
        
        [Header("Configuration")]
        private GameElementBlueprint _selectedBlueprint;
        
        public bool InPlacementMode { get; private set; }
        
        public delegate void OnElementBuiltEvent(GameElement element);
        public event OnElementBuiltEvent OnElementBuilt;
        
        public delegate void OnPlaceModeChangedEvent(bool state);
        public event OnPlaceModeChangedEvent OnPlaceModeChanged;
        
        public event PlacementLayer.OnTileStateChangedEvent OnTileStateChanged;

        private void OnEnable() => placementLayer.OnTileStateChanged += OnTileStateChanged;
        private void OnDisable() => placementLayer.OnTileStateChanged -= OnTileStateChanged;

        public bool IsAreaOccupied(Vector2 worldCoordinates, Vector2Int dimensions) => placementLayer.IsAreaOccupied(worldCoordinates, dimensions);
        
        private void Update()
        {
            if (!InPlacementMode || placementLayer == null)
            {
                previewLayer.HidePreview();
                return;
            }

            if (_selectedBlueprint == null) return;

            if (Input.GetMouseButtonDown(1) && !InputManager.PointerOverUI)
            {
                ExitPlacementMode();
                return;
            }

            var mouseWorldPosition = InputManager.MouseWorldPosition - _selectedBlueprint.CenterOffset;
            var coordinatesValid = AreCoordinatesEmpty(mouseWorldPosition);
            HandlePreview(mouseWorldPosition, coordinatesValid);
                
            if (Input.GetMouseButtonDown(0) && coordinatesValid && !InputManager.PointerOverUI)
            {
                var builtElement = Place(mouseWorldPosition, _selectedBlueprint);
                OnElementBuilt?.Invoke(builtElement);
            }
        }

        public void DestroyElementFrom(Vector3 worldCoordinates) => placementLayer.Destroy(worldCoordinates);

        private bool AreCoordinatesEmpty(Vector3 mouseWorldPosition)
        {
            return placementLayer.AreCoordinatesEmpty(
                worldCoordinates: mouseWorldPosition,
                dimensions: _selectedBlueprint.dimensions
            );
        }

        private void HandlePreview(Vector3 mouseWorldPosition, bool coordinatesValid)
        {
            previewLayer.ShowPreview(
                elementBlueprint: _selectedBlueprint,
                worldCoordinates: mouseWorldPosition,
                isValid: coordinatesValid
            );
        }

        public GameElement Place(Vector3 mouseWorldPosition, GameElementBlueprint elementBlueprint)
        {
            return placementLayer.PlaceElement(mouseWorldPosition, elementBlueprint);
        }
        
        public void SelectElementData(GameElementBlueprint elementBlueprint)
        {
            _selectedBlueprint = elementBlueprint;
        }

        public void EnterPlacementMode()
        {
            if (InPlacementMode) return;
            InPlacementMode = true;
            OnPlaceModeChanged?.Invoke(true);
        }

        public void ExitPlacementMode()
        {
            if (!InPlacementMode) return;
            _selectedBlueprint = null;
            InPlacementMode = false;
            OnPlaceModeChanged?.Invoke(false);
        }
    }
}
