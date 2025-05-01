using System.Collections.Generic;
using UnityEngine;

namespace Blueprints
{
    /// <summary>
    /// Holds data related to unit production, including a list of unit blueprints.
    /// This ScriptableObject allows for the management and configuration of units that can be produced.
    /// </summary>
    [CreateAssetMenu(menuName = "Units/Production Data", fileName = "UnitProductionData")]
    public class UnitProductionData : ScriptableObject
    {
        /// <summary>
        /// A list of blueprints for the units that can be produced by the building.
        /// </summary>
        public List<UnitBlueprint> blueprints = new();
    }
}