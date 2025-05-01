using Blueprints;
using UnityEngine;

namespace PlacementSystem.TilemapLayers
{
    /// <summary>
    /// Manages the visual preview of a game element on the tilemap during placement mode.
    /// Displays a transparent sprite to indicate where an element will be placed and whether the position is valid.
    /// </summary>
    public class PreviewLayer : TilemapLayer
    {
        [SerializeField] private SpriteRenderer previewRenderer;

        /// <summary>
        /// Semi-transparent green color used for valid placement tiles.
        /// </summary>
        private readonly Color _validColor = new(0, 1, 0, 0.5f);

        /// <summary>
        /// Semi-transparent red color used for invalid placement tiles.
        /// </summary>
        private readonly Color _invalidColor = new(1, 0, 0, 0.5f);

        /// <summary>
        /// Displays a visual preview of the given blueprint at the specified world position.
        /// Applies color feedback based on placement validity.
        /// </summary>
        /// <param name="elementBlueprint">The blueprint of the element being previewed.</param>
        /// <param name="worldCoordinates">The target world position for the preview.</param>
        /// <param name="isValid">Whether the current position is valid for placement.</param>
        public void ShowPreview(GameElementBlueprint elementBlueprint, Vector3 worldCoordinates, bool isValid)
        {
            var coordinates = TilemapReference.WorldToCell(worldCoordinates);
            previewRenderer.enabled = true;

            // Position and update preview sprite
            previewRenderer.transform.position = GetPositionForElement(coordinates, elementBlueprint);
            previewRenderer.sprite = elementBlueprint.displaySprite;
            previewRenderer.color = isValid ? _validColor : _invalidColor;
        }

        /// <summary>
        /// Hides the preview sprite.
        /// </summary>
        public void HidePreview()
        {
            previewRenderer.enabled = false;
        }
    }
}
