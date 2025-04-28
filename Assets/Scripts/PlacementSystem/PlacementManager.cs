using System.Collections.Generic;
using Blueprints;
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
        
        public delegate void OnElementPlacedHandler();
        public OnElementPlacedHandler OnElementPlaced;
        
        public delegate void OnEnterPlacementModeHandler();
        public OnEnterPlacementModeHandler OnEnterPlacementMode;
        
        public delegate void OnExitPlacementModeHandler();
        public OnExitPlacementModeHandler OnExitPlacementMode;
        
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
                Place(mouseWorldPosition, _selectedBlueprint);
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

        public void Place(Vector3 mouseWorldPosition, GameElementBlueprint elementBlueprint)
        {
            if (placementLayer.PlaceElement(mouseWorldPosition, elementBlueprint)) OnElementPlaced?.Invoke();
        }
        
        public void SelectElementData(GameElementBlueprint elementBlueprint)
        {
            _selectedBlueprint = elementBlueprint;
        }

        public void EnterPlacementMode()
        {
            if (InPlacementMode) return;
            InPlacementMode = true;
            OnEnterPlacementMode?.Invoke();
        }

        public void ExitPlacementMode()
        {
            if (!InPlacementMode) return;
            _selectedBlueprint = null;
            InPlacementMode = false;
            OnExitPlacementMode?.Invoke();
        }
    }
}
