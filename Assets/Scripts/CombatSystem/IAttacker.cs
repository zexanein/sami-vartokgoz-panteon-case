namespace CombatSystem
{
    /// <summary>
    /// Represents an entity capable of attacking and dealing damage to a target.
    /// Provides properties for attack damage and cooldown, and a method to perform the attack.
    /// </summary>
    public interface IAttacker
    {
        /// <summary>
        /// The amount of damage dealt per attack.
        /// </summary>
        int AttackDamage { get; }
        
        /// <summary>
        /// The cooldown time between attacks, in seconds.
        /// </summary>
        float AttackCooldown { get; }
        
        /// <summary>
        /// Performs an attack on a specified damageable target.
        /// </summary>
        /// <param name="damageable">The target that will receive damage.</param>
        void Attack(IDamageable damageable);
    }
}