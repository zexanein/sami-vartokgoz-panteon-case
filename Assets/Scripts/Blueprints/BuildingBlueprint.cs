using UnityEngine;

namespace Blueprints
{
    [CreateAssetMenu(menuName = "Building System/Building Data", fileName = "NewBuildingData")]
    public class BuildingBlueprint : GameElementBlueprint
    {
        [Header("Production Data")]
        public UnitProductionData productionData;
        public Vector2Int unitSpawnPointCoordinates;
    }
}