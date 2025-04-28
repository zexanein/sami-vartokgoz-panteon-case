using Extensions;
using UnityEngine;

namespace Blueprints
{
    public class GameElementBlueprint : ScriptableObject
    {
        [Header("Game Element Settings")]
        public string elementName;
        [TextArea(3, 10)] public string elementDescription;
        public Sprite displaySprite;
        public Sprite uiIcon;
        public GameObject elementPrefab;
        public int healthPoints;
    
        [Header("Placement Settings")]
        public Vector2Int dimensions;
        public Vector2 CenterOffset => dimensions.Add(1).Mod(2) * -0.5f;
        public Vector2Int placementOffset;
    }
}
