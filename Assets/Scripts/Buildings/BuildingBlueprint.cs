using Extensions;
using Units;
using UnityEngine;
using UnityEngine.Serialization;

namespace Buildings
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
        public int healthPoints;
        
        [Header("Placement Data")]
        public Vector2Int placementOffset;
        public Vector2Int dimensions;
        public Vector2 CenterOffset => dimensions.Add(1).Mod(2) * -0.5f;
        
        [Header("Production Data")]
        public UnitProductionData productionData;
        public Vector2Int unitSpawnPointCoordinates;
    }
}