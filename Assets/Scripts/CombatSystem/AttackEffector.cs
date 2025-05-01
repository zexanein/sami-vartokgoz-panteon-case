using System.Collections;
using CombatSystem;
using Extensions;
using UnityEngine;

public class AttackEffector : MonoBehaviour
{
    private Coroutine _attackCoroutine;

    public delegate void OnBeforeAttackHandler();
    public event OnBeforeAttackHandler OnBeforeAttack;
    
    public void StartAttack(IAttacker attacker, IDamageable damageable)
    {
        if (_attackCoroutine != null) StopCoroutine(_attackCoroutine);
        _attackCoroutine = StartCoroutine(AttackCoroutine(attacker, damageable));
    }

    public void StopAttack()
    {
        if (_attackCoroutine != null) StopCoroutine(_attackCoroutine);
        _attackCoroutine = null;
    }

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
