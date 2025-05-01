using Extensions;
using GameElements;
using UnityEngine;

namespace CombatSystem
{
    /// <summary>
    /// This class manages the health bar of a game element.
    /// </summary>
    public class HealthBar : MonoBehaviour
    {
        public GameElement gameElement;
        [SerializeField] private Transform pivotTransform;

        /// <summary>
        /// Adds a listener to the OnHealthChanged event of the game element.
        /// </summary>
        private void OnEnable() => gameElement.OnHealthChanged += OnHealthChanged;

        /// <summary>
        /// Removes the listener from the OnHealthChanged event of the game element.
        /// </summary>
        private void OnDisable() => gameElement.OnHealthChanged -= OnHealthChanged;

        /// <summary>
        /// Updates the health bar's fill amount when the health changes.
        /// </summary>
        private void OnHealthChanged() => SetHealthValues(gameElement.Health, gameElement.MaxHealth);

        /// <summary>
        /// Updates the health bar's fill amount based on the current and maximum health values.
        /// </summary>
        private void SetHealthValues(float health, float maxHealth) => pivotTransform.localScale = pivotTransform.localScale.WithX(Mathf.Clamp01(health / maxHealth));
    }
}
