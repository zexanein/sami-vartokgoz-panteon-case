using BuildingSystem.Models;
using BuildingSystem.TilemapLayers;
using UnityEngine;

namespace BuildingSystem
{
    public class BuildingSystemController : MonoBehaviour
    {
        public BuildingSystemLayer buildingSystemLayer;
        public PreviewLayer previewLayer;
        public BuildingData selectedBuildingData;
        
        private void Update()
        {
            if (buildingSystemLayer == null)
            {
                previewLayer.HidePreview();
                return;
            }
            
            var mouseWorldPosition = InputController.MouseWorldPosition;
            
            if (Input.GetMouseButtonDown(1))
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
            
            if (Input.GetMouseButtonDown(0) && coordinatesValid)
            {
                buildingSystemLayer.Build(mouseWorldPosition, selectedBuildingData);
            }
        }
    }
}
