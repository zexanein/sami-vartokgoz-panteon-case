using UnityEngine;
using UnityEngine.Serialization;

namespace BuildingPlacementSystem.Models
{
    [CreateAssetMenu(menuName = "Building System/Building Data", fileName = "NewBuildingData")]
    public class BuildingBlueprint : ScriptableObject
    {
        [Header("Building Data")]
        public string buildingName;
        [TextArea(3, 10)]
        public string buildingDescription;
        public Sprite displaySprite;
        public Sprite uiIcon;
        public GameObject buildingPrefab;
        public bool producingUnits;
        public int healthPoints;
        
        [Header("Placement Data")]
        public Vector3 placementOffset;
        public bool useCustomCollisionSpace;
        public RectInt collisionSpace;
    }
}