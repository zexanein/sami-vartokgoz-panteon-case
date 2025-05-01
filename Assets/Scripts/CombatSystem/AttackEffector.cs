using System.Collections;
using Extensions;
using UnityEngine;

namespace CombatSystem
{
    /// <summary>
    /// Handles the attack logic between an attacker and a damageable target.
    /// Controls attack timing, damage application, and coroutine management.
    /// </summary>
    public class AttackEffector : MonoBehaviour
    {
        private Coroutine _attackCoroutine;

        /// <summary>
        /// Event triggered before each attack.
        /// Can be used to play animations, sound effects, etc.
        /// </summary>
        public delegate void OnBeforeAttackHandler();
        public event OnBeforeAttackHandler OnBeforeAttack;

        /// <summary>
        /// Starts the attack process between an attacker and a damageable target.
        /// If an attack is already in progress, it will be restarted.
        /// </summary>
        /// <param name="attacker">The entity initiating the attack.</param>
        /// <param name="damageable">The target receiving the damage.</param>
        public void StartAttack(IAttacker attacker, IDamageable damageable)
        {
            if (_attackCoroutine != null) StopCoroutine(_attackCoroutine);
            _attackCoroutine = StartCoroutine(AttackCoroutine(attacker, damageable));
        }

        /// <summary>
        /// Stops the current attack process if it is in progress.
        /// </summary>
        public void StopAttack()
        {
            if (_attackCoroutine != null) StopCoroutine(_attackCoroutine);
            _attackCoroutine = null;
        }

        /// <summary>
        /// Coroutine that applies damage to the target repeatedly, based on the attacker's cooldown.
        /// Continues until the attacker or target becomes invalid, or the target dies.
        /// </summary>
        private IEnumerator AttackCoroutine(IAttacker attacker, IDamageable damageable)
        {
            while (
                !Utils.IsUnityObjectNull(attacker) &&
                !Utils.IsUnityObjectNull(damageable) &&
                !damageable.IsDead )
            {
                OnBeforeAttack?.Invoke();
                damageable.TakeDamage(attacker.AttackDamage);
                yield return new WaitForSeconds(attacker.AttackCooldown);
            }
        
            StopAttack();
        }
    }
}
