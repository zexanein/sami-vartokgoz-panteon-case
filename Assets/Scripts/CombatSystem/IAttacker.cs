namespace CombatSystem
{
    public interface IAttacker
    {
        int AttackDamage { get; }
        float AttackCooldown { get; }
        void Attack(IDamageable damageable);
    }
}