using System.Collections.Generic;
using BuildingPlacementSystem.Models;
using BuildingPlacementSystem.TilemapLayers;
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
        
        [field: SerializeField]
        public bool InBuildMode { get; private set; } = false;
        
        [Header("Configuration")]
        public BuildingBlueprint selectedBuildingBlueprint;
        
        
        private void Update()
        {
            if (!InBuildMode || buildingSystemLayer == null)
            {
                previewLayer.HidePreview();
                return;
            }
            
            var mouseWorldPosition = InputManager.MouseWorldPosition;
            
            if (Input.GetMouseButtonDown(1) && !InputManager.PointerOverUI)
            {
                if (selectedBuildingBlueprint != null) ExitBuildMode();
                else buildingSystemLayer.Destroy(mouseWorldPosition);
            }
            
            if (selectedBuildingBlueprint == null) return;

            var coordinatesValid = buildingSystemLayer.IsCoordinatesValid(
                worldCoordinates: mouseWorldPosition,
                collisionSpace: selectedBuildingBlueprint.useCustomCollisionSpace
                    ? selectedBuildingBlueprint.collisionSpace
                    : default);
            
            previewLayer.ShowPreview
            (
                building: selectedBuildingBlueprint,
                worldCoordinates: mouseWorldPosition,
                isValid: coordinatesValid
            );
            
            if (Input.GetMouseButtonDown(0) && coordinatesValid && !InputManager.PointerOverUI)
            {
                buildingSystemLayer.Build(mouseWorldPosition, selectedBuildingBlueprint);
            }
        }
        
        public void SelectBuildingData(BuildingBlueprint buildingBlueprint)
        {
            selectedBuildingBlueprint = buildingBlueprint;
        }

        public void EnterBuildMode()
        {
            if (InBuildMode) return;
            InBuildMode = true;
        }

        public void ExitBuildMode()
        {
            if (!InBuildMode) return;
            selectedBuildingBlueprint = null;
            InBuildMode = false;
        }
    }
}
