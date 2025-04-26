using UnityEngine;
using UnityEngine.Serialization;

namespace BuildingSystem.Models
{
    [CreateAssetMenu(menuName = "Building System/Building Data", fileName = "NewBuildingData")]
    public class BuildingData : ScriptableObject
    {
        public string buildingName;
        public Sprite previewSprite;
        public GameObject gameObject;
        public Vector3 placementOffset;
        public bool useCustomCollisionSpace;
        public RectInt collisionSpace;
    }
}