using UnityEngine;

namespace Blueprints
{
    [CreateAssetMenu(menuName = "Units/Unit", fileName = "NewUnit")]
    public class UnitBlueprint : GameElementBlueprint
    {
        [Header("Unit Settings")]
        public int damagePoints;
    }
}
