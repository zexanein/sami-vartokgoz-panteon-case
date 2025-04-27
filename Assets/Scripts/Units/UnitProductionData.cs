using System.Collections.Generic;
using UnityEngine;

namespace Units
{
    [CreateAssetMenu(menuName = "Units/ProductionData", fileName = "UnitProductionData")]
    public class UnitProductionData : ScriptableObject
    {
        public List<UnitBlueprint> unitBlueprints = new();
    }
}
