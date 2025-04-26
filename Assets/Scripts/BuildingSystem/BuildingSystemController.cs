using System.Collections.Generic;
using BuildingSystem.Models;
using BuildingSystem.TilemapLayers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BuildingSystem
{
    public class BuildingSystemController : MonoBehaviour
    {
        #region Singleton
        private static BuildingSystemController _instance;

        public static BuildingSystemController Instance
        {
            get
            {
                if (_instance == null) _instance = FindObjectOfType<BuildingSystemController>();
                return _instance;
            }
        }
        #endregion
        
        [Header("References")]
        public BuildingSystemLayer buildingSystemLayer;
        public PreviewLayer previewLayer;
        public List<BuildingData> buildingDataList = new();
        
        [Header("Configuration")]
        public BuildingData selectedBuildingData;
        
        private bool PointerOverUI => EventSystem.current.IsPointerOverGameObject();
        
        private void Update()
        {
            if (buildingSystemLayer == null)
            {
                previewLayer.HidePreview();
                return;
            }
            
            var mouseWorldPosition = InputController.MouseWorldPosition;
            
            if (Input.GetMouseButtonDown(1) && !PointerOverUI)
            {
                buildingSystemLayer.Destroy(mouseWorldPosition);
            }
            
            if (selectedBuildingData == null) return;

            var coordinatesValid = buildingSystemLayer.IsCoordinatesValid(
                worldCoordinates: mouseWorldPosition,
                collisionSpace: selectedBuildingData.useCustomCollisionSpace
                    ? selectedBuildingData.collisionSpace
                    : default);
            
            previewLayer.ShowPreview
            (
                building: selectedBuildingData,
                worldCoordinates: mouseWorldPosition,
                isValid: coordinatesValid
            );
            
            if (Input.GetMouseButtonDown(0) && coordinatesValid && !PointerOverUI)
            {
                buildingSystemLayer.Build(mouseWorldPosition, selectedBuildingData);
            }
        }
        
        public void SelectBuildingData(BuildingData buildingData)
        {
            selectedBuildingData = buildingData;
        }
    }
}
