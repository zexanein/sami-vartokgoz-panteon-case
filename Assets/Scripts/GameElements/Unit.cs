using Blueprints;
using CombatSystem;
using Pathfinding;
using UnityEngine;

namespace GameElements
{
    public class Unit : GameElement, IAttacker
    {
        public UnitBlueprint UnitBlueprint => Blueprint as UnitBlueprint;

        #region Movement
        private UnitPathFollower _pathFollower;

        private UnitPathFollower PathFollower
        {
            get
            {
                if (_pathFollower == null && !TryGetComponent(out _pathFollower))
                    _pathFollower = gameObject.AddComponent<UnitPathFollower>();
                return _pathFollower;
            }
        }
        #endregion
        
        #region Attacking
        private AttackEffector _attackEffector;
        public AttackEffector AttackEffector
        {
            get
            {
                if (_attackEffector == null && !TryGetComponent(out _attackEffector))
                    _attackEffector = gameObject.AddComponent<AttackEffector>();
                return _attackEffector;
            }
        }
        public GameElement AttackTarget { get; private set; }
        public int AttackDamage => UnitBlueprint.damagePoints;
        public float AttackCooldown => UnitBlueprint.damageCooldown;
        public void Attack(IDamageable damageable) => AttackEffector.StartAttack(this, damageable);
        #endregion

        private void OnEnable()
        {
            PathFollower.OnReachedGameElement += OnReachedToElement;   
            AttackEffector.OnBeforeAttack += OnBeforeAttack;
        }

        private void OnDisable()
        {
            PathFollower.OnReachedGameElement -= OnReachedToElement;   
            AttackEffector.OnBeforeAttack -= OnBeforeAttack;
        }

        public override void Select()
        {
            if (enableOnSelected != null)
                enableOnSelected.SetActive(true);
        }

        public override void Deselect()
        {
            if (enableOnSelected != null)
                enableOnSelected.SetActive(false);
        }
        
        public override void InteractWithOther(Vector3 mousePosition, ISelectable otherSelectable)
        {
            AttackTarget = null;
            
            if (otherSelectable is GameElement otherElement)
            {
                PathFollower.MoveToElement(otherElement);
                AttackTarget = otherElement;
            }
            
            else PathFollower.MoveToPosition(mousePosition);
        }
        
        private void OnReachedToElement(GameElement targetElement) => Attack(targetElement);

        private void OnBeforeAttack()
        {
            if (!PathFollower.IsElementNearby((Vector2Int)Coordinates, AttackTarget)) AttackEffector.StopAttack();
        }
    }
}