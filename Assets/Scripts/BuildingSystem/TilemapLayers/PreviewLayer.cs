using BuildingSystem.Models;
using UnityEngine;

namespace BuildingSystem.TilemapLayers
{
    public class PreviewLayer : TilemapLayer
    {
        [SerializeField] private SpriteRenderer previewRenderer;
        private readonly Color _validColor = new(0, 1, 0, 0.5f);
        private readonly Color _invalidColor = new(1, 0, 0, 0.5f);

        public void ShowPreview(BuildingData building, Vector3 worldCoordinates, bool isValid)
        {
            var coordinates = Tilemap.WorldToCell(worldCoordinates);
            previewRenderer.enabled = true;
            
            // Move Preview Renderer
            previewRenderer.transform.position =
                Tilemap.CellToWorld(coordinates) +
                Tilemap.cellSize / 2 +
                building.placementOffset;
            
            previewRenderer.sprite = building.previewSprite;
            previewRenderer.color = isValid ? _validColor : _invalidColor;
        }

        public void HidePreview()
        {
            previewRenderer.enabled = false;
        }
    }
}
