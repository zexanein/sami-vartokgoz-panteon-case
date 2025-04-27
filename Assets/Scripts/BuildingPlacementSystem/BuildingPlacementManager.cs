using System.Collections.Generic;
using BuildingPlacementSystem.TilemapLayers;
using Buildings;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BuildingPlacementSystem
{
    /// <summary>
    /// This manager handles building placement interactions and logic
    /// </summary>
    public class BuildingPlacementManager : MonoBehaviour
    {
        #region Singleton
        private static BuildingPlacementManager _instance;

        public static BuildingPlacementManager Instance
        {
            get
            {
                if (_instance == null) _instance = FindObjectOfType<BuildingPlacementManager>();
                return _instance;
            }
        }
        #endregion
        
        [Header("References")]
        public BuildingSystemLayer buildingSystemLayer;
        public PreviewLayer previewLayer;
        public List<BuildingBlueprint> buildingBlueprints = new();
        
        [Header("Configuration")]
        private BuildingBlueprint _selectedBuildingBlueprint;
        
        public bool InBuildMode { get; private set; } = false;
        
        private void Update()
        {
            if (!InBuildMode || buildingSystemLayer == null)
            {
                previewLayer.HidePreview();
                return;
            }

            if (_selectedBuildingBlueprint == null) return;

            if (Input.GetMouseButtonDown(1) && !InputManager.PointerOverUI)
            {
                ExitBuildMode();
                return;
            }

            var mouseWorldPosition = InputManager.MouseWorldPosition - _selectedBuildingBlueprint.CenterOffset;
            var coordinatesValid = AreCoordinatesValid(mouseWorldPosition);
            HandlePreview(mouseWorldPosition, coordinatesValid);
            if (coordinatesValid) HandleBuild(mouseWorldPosition);
        }

        private bool AreCoordinatesValid(Vector3 mouseWorldPosition)
        {
            return buildingSystemLayer.AreCoordinatesValid(
                worldCoordinates: mouseWorldPosition,
                dimensions: _selectedBuildingBlueprint.dimensions
            );
        }

        private void HandlePreview(Vector3 mouseWorldPosition, bool coordinatesValid)
        {
            previewLayer.ShowPreview(
                building: _selectedBuildingBlueprint,
                worldCoordinates: mouseWorldPosition,
                isValid: coordinatesValid
            );
        }

        private void HandleBuild(Vector3 mouseWorldPosition)
        {
            if (!Input.GetMouseButtonDown(0) || InputManager.PointerOverUI) return;
            buildingSystemLayer.Build(mouseWorldPosition, _selectedBuildingBlueprint);
        }
        
        public void SelectBuildingData(BuildingBlueprint buildingBlueprint)
        {
            _selectedBuildingBlueprint = buildingBlueprint;
        }

        public void EnterBuildMode()
        {
            if (InBuildMode) return;
            InBuildMode = true;
        }

        public void ExitBuildMode()
        {
            if (!InBuildMode) return;
            _selectedBuildingBlueprint = null;
            InBuildMode = false;
        }
    }
}
