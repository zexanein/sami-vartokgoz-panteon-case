using Extensions;
using UnityEngine;

namespace Blueprints
{
    /// <summary>
    /// Represents the blueprint data for a game element such as a building, unit or any placeable object.
    /// Used to store visual, descriptive and placement-related properties.
    /// </summary>
    public class GameElementBlueprint : ScriptableObject
    {
        /// <summary>
        /// The display name of the game element.
        /// </summary>
        [Header("Game Element Settings")]
        public string elementName;
        
        /// <summary>
        /// The description of the game element, providing additional information.
        /// </summary>
        [TextArea(3, 10)] public string elementDescription;
        
        /// <summary>
        /// The display sprite for the game element, used in the game world.
        /// </summary>
        public Sprite displaySprite;
        
        /// <summary>
        /// The icon sprite for the game element, used in the UI.
        /// </summary>
        public Sprite uiIcon;
        
        /// <summary>
        /// The prefab that will be instantiated into the scene for this element.
        /// </summary>
        public GameObject elementPrefab;
        
        /// <summary>
        /// The maximum health points of the game element when placed in the game world.
        /// </summary>
        public int healthPoints;
    
        /// <summary>
        /// The size of the element in grid units. For example, (2, 2) means the element occupies a 2x2 area.
        /// </summary>
        [Header("Placement Settings")]
        public Vector2Int dimensions;
        
        /// <summary>
        /// The center offset used to properly align the object on the grid, accounting for even/odd sizes.
        /// </summary>
        public Vector2 CenterOffset => dimensions.Add(1).Mod(2) * -0.5f;
        
        /// <summary>
        /// Additional offset applied during placement
        /// </summary>
        public Vector2Int placementOffset;
    }
}
