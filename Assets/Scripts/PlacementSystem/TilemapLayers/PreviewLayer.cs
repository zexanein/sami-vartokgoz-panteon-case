using Blueprints;
using UnityEngine;

namespace PlacementSystem.TilemapLayers
{
    public class PreviewLayer : TilemapLayer
    {
        [SerializeField] private SpriteRenderer previewRenderer;
        [SerializeField] private SpriteRenderer spawnPointRenderer;
        private readonly Color _validColor = new(0, 1, 0, 0.5f);
        private readonly Color _invalidColor = new(1, 0, 0, 0.5f);

        public void ShowPreview(GameElementBlueprint elementBlueprint, Vector3 worldCoordinates, bool isValid)
        {
            var coordinates = TilemapReference.WorldToCell(worldCoordinates);
            previewRenderer.enabled = true;
            
            // Move Preview Renderer
            previewRenderer.transform.position = GetPositionForElement(coordinates, elementBlueprint);
            
            previewRenderer.sprite = elementBlueprint.displaySprite;
            previewRenderer.color = isValid ? _validColor : _invalidColor;
        }

        public void HidePreview()
        {
            previewRenderer.enabled = false;
        }
    }
}
