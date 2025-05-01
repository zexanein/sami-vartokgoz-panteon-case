using UnityEngine;

namespace Blueprints
{
    /// <summary>
    /// Represents a blueprint for a unit in the game.
    /// </summary>
    [CreateAssetMenu(menuName = "Units/Unit", fileName = "NewUnit")]
    public class UnitBlueprint : GameElementBlueprint
    {
        [Header("Unit Settings")]
        public int damagePoints;
        public float damageCooldown;
        public float movementSpeed;
    }
}