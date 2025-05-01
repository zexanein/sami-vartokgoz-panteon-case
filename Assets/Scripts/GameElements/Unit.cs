using Blueprints;
using CombatSystem;
using Pathfinding;
using UnityEngine;

namespace GameElements
{
    /// <summary>
    /// Represents a unit in the game that can move, attack, and interact with other game elements.
    /// Inherits from <see cref="GameElement"/> and implements <see cref="IAttacker"/> for combat functionality.
    /// </summary>
    public class Unit : GameElement, IAttacker
    {
        /// <summary>
        /// Casts the base blueprint to a <see cref="UnitBlueprint"/>.
        /// Use this to access unit-specific data such as damage points and cooldown.
        /// </summary>
        public UnitBlueprint UnitBlueprint => Blueprint as UnitBlueprint;

        #region Movement
        private UnitPathFollower _pathFollower;

        /// <summary>
        /// The path follower component responsible for moving the unit along a path.
        /// Implements lazy initialization to ensure the component is created only when needed.
        /// </summary>
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
        
        /// <summary>
        /// The attack effector component responsible for handling combat logic.
        /// Implements lazy initialization to ensure the component is created only when needed.
        /// </summary>
        public AttackEffector AttackEffector
        {
            get
            {
                if (_attackEffector == null && !TryGetComponent(out _attackEffector))
                    _attackEffector = gameObject.AddComponent<AttackEffector>();
                return _attackEffector;
            }
        }
        
        /// <summary>
        /// The target game element that the unit is currently attacking.
        /// </summary>
        public GameElement AttackTarget { get; private set; }
        
        /// <summary>
        /// The amount of damage dealt by the unit per attack.
        /// </summary>
        public int AttackDamage => UnitBlueprint.damagePoints;
        
        /// <summary>
        /// The cooldown time between attacks, in seconds.
        /// </summary>
        public float AttackCooldown => UnitBlueprint.damageCooldown;
        
        /// <summary>
        /// Initiates an attack on a specified damageable target.
        /// </summary>
        /// <param name="damageable">The target that will receive damage.</param>
        public void Attack(IDamageable damageable) => AttackEffector.StartAttack(this, damageable);
        #endregion

        /// <summary>
        /// Subscribes to events when the unit is enabled.
        /// </summary>
        private void OnEnable()
        {
            PathFollower.OnReachedGameElement += OnReachedToElement;   
            AttackEffector.OnBeforeAttack += OnBeforeAttack;
        }

        /// <summary>
        /// Unsubscribes from events when the unit is disabled.
        /// </summary>
        private void OnDisable()
        {
            PathFollower.OnReachedGameElement -= OnReachedToElement;   
            AttackEffector.OnBeforeAttack -= OnBeforeAttack;
        }

        /// <summary>
        /// Visual indication of the unit is selected.
        /// </summary>
        public override void Select()
        {
            if (enableOnSelected != null)
                enableOnSelected.SetActive(true);
        }

        /// <summary>
        /// Disables the visual indication of the unit is selected.
        /// </summary>
        public override void Deselect()
        {
            if (enableOnSelected != null)
                enableOnSelected.SetActive(false);
        }

        /// <summary>
        /// Handles interaction logic when this unit is commanded to interact with another selectable
        /// </summary>
        /// <param name="mousePosition">Position where the mouse is clicked.</param>
        /// <param name="otherSelectable">The other selectable element to interact with.</param>
        public override void InteractWithOther(Vector3 mousePosition, ISelectable otherSelectable)
        {
            AttackTarget = null;
            
            // Move to the target element and set it as the attack target
            if (otherSelectable is GameElement otherElement)
            {
                PathFollower.MoveToElement(otherElement);
                AttackTarget = otherElement;
            }
            
            // Just move to the clicked position if the target is not a game element
            else PathFollower.MoveToPosition(mousePosition);
        }
        
        /// <summary>
        /// Called when the unit reaches a target element.
        /// Begins the attack process on the target element.
        /// </summary>
        /// <param name="targetElement">The target element that was reached.</param>
        private void OnReachedToElement(GameElement targetElement) => Attack(targetElement);

        /// <summary>
        /// Called before each attack to validate the target is still in range.
        /// Stops the attack if the target is no longer nearby.
        /// </summary>
        private void OnBeforeAttack()
        {
            if (!PathFollower.IsElementNearby((Vector2Int)Coordinates, AttackTarget)) AttackEffector.StopAttack();
        }
    }
}