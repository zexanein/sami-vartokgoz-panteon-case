using System.Collections.Generic;
using Blueprints;
using GameElements;
using PlacementSystem.TilemapLayers;
using UnityEngine;

namespace PlacementSystem
{
    /// <summary>
    /// Manages the placement of game elements onto the tilemap.
    /// Handles blueprint selection, preview rendering, validation, and final placement.
    /// Also manages placement mode state and exposes relevant events.
    /// </summary>
    public class PlacementManager : MonoBehaviour
    {
        #region Singleton

        private static PlacementManager _instance;

        /// <summary>
        /// Singleton instance of the PlacementManager.
        /// </summary>
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

        /// <summary>
        /// A list of all available placeable element blueprints.
        /// populated via the Inspector.
        /// </summary>
        public List<GameElementBlueprint> blueprints = new();

        [Header("Configuration")]
        private GameElementBlueprint _selectedBlueprint;

        /// <summary>
        /// Indicates whether placement mode is currently active.
        /// When active, the system listens for input and displays previews.
        /// </summary>
        public bool InPlacementMode { get; private set; }

        /// <summary>
        /// Invoked when a new element is successfully placed.
        /// </summary>
        public delegate void OnElementBuiltEvent(GameElement element);
        public event OnElementBuiltEvent OnElementBuilt;

        /// <summary>
        /// Invoked when the placement mode is entered or exited.
        /// </summary>
        public delegate void OnPlaceModeChangedEvent(bool state);
        public event OnPlaceModeChangedEvent OnPlaceModeChanged;

        /// <summary>
        /// Subscribes to tile state changes from the PlacementLayer.
        /// </summary>
        public event PlacementLayer.OnTileStateChangedEvent OnTileStateChanged;

        private void OnEnable() => placementLayer.OnTileStateChanged += OnTileStateChanged;
        private void OnDisable() => placementLayer.OnTileStateChanged -= OnTileStateChanged;

        private void Update()
        {
            if (!InPlacementMode || placementLayer == null)
            {
                previewLayer.HidePreview();
                return;
            }

            if (_selectedBlueprint == null) return;

            // Exit placement mode on right-click
            if (Input.GetMouseButtonDown(1) && !InputManager.PointerOverUI)
            {
                ExitPlacementMode();
                return;
            }

            var mouseWorldPosition = InputManager.MouseWorldPosition - _selectedBlueprint.CenterOffset;
            var coordinatesValid = AreCoordinatesEmpty(mouseWorldPosition);

            HandlePreview(mouseWorldPosition, coordinatesValid);

            // Place element on left-click
            if (Input.GetMouseButtonDown(0) && coordinatesValid && !InputManager.PointerOverUI)
            {
                var builtElement = Place(mouseWorldPosition, _selectedBlueprint);
                OnElementBuilt?.Invoke(builtElement);
            }
        }

        /// <summary>
        /// Checks if the grid coordinates under the mouse are empty and valid for placement.
        /// </summary>
        private bool AreCoordinatesEmpty(Vector3 mouseWorldPosition)
        {
            return placementLayer.AreCoordinatesEmpty(
                worldCoordinates: mouseWorldPosition,
                dimensions: _selectedBlueprint.dimensions
            );
        }

        /// <summary>
        /// Updates the preview layer with the selected blueprint and placement status.
        /// </summary>
        private void HandlePreview(Vector3 mouseWorldPosition, bool coordinatesValid)
        {
            previewLayer.ShowPreview(
                elementBlueprint: _selectedBlueprint,
                worldCoordinates: mouseWorldPosition,
                isValid: coordinatesValid
            );
        }

        /// <summary>
        /// Places the selected blueprint on the map at the given world position.
        /// </summary>
        /// <param name="mouseWorldPosition">Target world position for placement.</param>
        /// <param name="elementBlueprint">The blueprint to place.</param>
        /// <returns>The instantiated game element.</returns>
        public GameElement Place(Vector3 mouseWorldPosition, GameElementBlueprint elementBlueprint)
        {
            return placementLayer.PlaceElement(mouseWorldPosition, elementBlueprint);
        }

        /// <summary>
        /// Sets the currently selected blueprint to be placed.
        /// </summary>
        /// <param name="elementBlueprint">The blueprint to select.</param>
        public void SelectElementData(GameElementBlueprint elementBlueprint)
        {
            _selectedBlueprint = elementBlueprint;
        }

        /// <summary>
        /// Enables placement mode and starts listening for placement input.
        /// </summary>
        public void EnterPlacementMode()
        {
            if (InPlacementMode) return;
            InPlacementMode = true;
            OnPlaceModeChanged?.Invoke(true);
        }

        /// <summary>
        /// Disables placement mode and clears the selected blueprint.
        /// </summary>
        public void ExitPlacementMode()
        {
            if (!InPlacementMode) return;
            _selectedBlueprint = null;
            InPlacementMode = false;
            OnPlaceModeChanged?.Invoke(false);
        }
    }
}
