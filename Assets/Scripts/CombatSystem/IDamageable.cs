namespace CombatSystem
{
    public interface IDamageable
    {
        void TakeDamage(int amount);
        bool IsDead { get; }

        delegate void OnDamagedHandler();
        event OnDamagedHandler OnDamaged;
    }
}