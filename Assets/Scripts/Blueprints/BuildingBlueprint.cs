using Blueprints;
using Extensions;
using UnityEngine;
using UnityEngine.Serialization;

namespace Buildings
{
    [CreateAssetMenu(menuName = "Building System/Building Data", fileName = "NewBuildingData")]
    public class BuildingBlueprint : GameElementBlueprint
    {
        
        [Header("Production Data")]
        public UnitProductionData productionData;
        public Vector2Int unitSpawnPointCoordinates;
    }
}