using UnityEngine;

namespace Blueprints
{
    /// <summary>
    /// A blueprint for a constructible building in the game.
    /// Inherits basic properties from <see cref="GameElementBlueprint"/> and adds unit production settings
    /// </summary>
    [CreateAssetMenu(menuName = "Building System/Building Data", fileName = "NewBuildingData")]
    public class BuildingBlueprint : GameElementBlueprint
    {
        /// <summary>
        /// Optional unit production data for the building. If assigned, the building can produce units.
        /// </summary>
        [Header("Production Data")]
        public UnitProductionData productionData;
        
        /// <summary>
        /// Grid coordinates relative to the building origin where spawned units should appear.
        /// Used for determining the unit spawn point in the world.
        /// </summary>
        public Vector2Int unitSpawnPointCoordinates;
    }
}