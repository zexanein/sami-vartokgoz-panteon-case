namespace CombatSystem
{
    /// <summary>
    /// Represents an entity that can take damage and potentially die.
    /// Includes death state tracking and an event for damage notifications.
    /// </summary>
    public interface IDamageable
    {
        /// <summary>
        /// Applies a specified amount of damage to the object
        /// </summary>
        /// <param name="amount"> The amount of damage to apply.</param>
        void TakeDamage(int amount);
        
        /// <summary>
        /// Indicates whether the object is dead.
        /// </summary>
        bool IsDead { get; }

        /// <summary>
        /// Event triggered when the object takes damage.
        /// Useful for triggering animations, sound effects, etc.
        /// </summary>
        delegate void OnHealthChangedHandler();
        event OnHealthChangedHandler OnHealthChanged;
    }
}