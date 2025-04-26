using UnityEngine;

namespace BuildingSystem.Models
{
    [CreateAssetMenu(menuName = "Building System/Building Data", fileName = "NewBuildingData")]
    public class BuildingData : ScriptableObject
    {
        [Header("Building Data")]
        public string buildingName;
        public Sprite previewSprite;
        public Sprite displaySprite;
        public GameObject gameObject;
        public int healthPoints;
        
        [Header("Placement Data")]
        public Vector3 placementOffset;
        public bool useCustomCollisionSpace;
        public RectInt collisionSpace;
    }
}