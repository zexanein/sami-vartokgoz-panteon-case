using System;
using System.Collections.Generic;
using CombatSystem;
using GameElements;
using UnityEngine;

/// <summary>
/// This class manages the health bars and their positions for game elements.
/// Used to reduce draw calls by collecting all health bars in one canvas
/// </summary>
public class HealthBarManager : MonoBehaviour
{
    #region Singleton
    private static HealthBarManager _instance;

    public static HealthBarManager Instance
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<HealthBarManager>();
            return _instance;
        }
    }
    #endregion
    
    public RectTransform barsParent;
    private Dictionary<GameElement, HealthBarEventData> _bars = new();
    private float HealthBarYOffset => 8f;

    private class HealthBarEventData
    {
        public HealthBarEventData(HealthBar healthBar) => HealthBarReference = healthBar;
        public HealthBar HealthBarReference { get; }
        public IDamageable.OnHealthChangedHandler HealthChangedHandler;
    }
    
    /// <summary>
    /// Registers a health bar for the given game element.
    /// </summary>
    /// <param name="element">The game element to register.</param>
    public void RegisterHealthBar(GameElement element)
    {
        if (element == null) return;
        if (_bars.ContainsKey(element)) return;
        
        var healthBar= PoolingManager.Instance.HealthBarPool.GetFromPool(barsParent).GetComponent<HealthBar>();
        healthBar.SetHealthValues(element.Health, element.MaxHealth);
        
        var eventData = new HealthBarEventData(healthBar);
        element.OnElementDestroyed += OnElementDestroyed;
        eventData.HealthChangedHandler = () => OnElementHealthChanged(element);
        element.OnHealthChanged += eventData.HealthChangedHandler;
        _bars.Add(element, eventData);
    }

    /// <summary>
    /// Unregisters the health bar for the given game element.
    /// </summary>
    /// <param name="element">The game element to unregister.</param>
    private void UnregisterHealthBar(GameElement element)
    {
        if (element == null) return;
        if (!_bars.TryGetValue(element, out var healthBar)) return;
        
        element.OnElementDestroyed -= OnElementDestroyed;
        element.OnHealthChanged -= healthBar.HealthChangedHandler;
        PoolingManager.Instance.HealthBarPool.Return(healthBar.HealthBarReference.gameObject);
        _bars.Remove(element);
    }

    /// <summary>
    /// Called when a registered game element is destroyed.
    /// </summary>
    /// <param name="element"></param>
    private void OnElementDestroyed(GameElement element) => UnregisterHealthBar(element);
    
    private void OnElementHealthChanged(GameElement element)
    {
        if (element == null) return;
        if (!_bars.TryGetValue(element, out var healthBar)) return;
        
        healthBar.HealthBarReference.SetHealthValues(element.Health, element.MaxHealth);
    }

    /// <summary>
    /// Updates the health bar positions.
    /// </summary>
    public void Update()
    {
        // Sync health bar positions
        foreach (var kvp in _bars)
        {
            SyncHealthBarPosition(kvp.Key, kvp.Value.HealthBarReference);
        }
    }

    /// <summary>
    /// Syncs the health bar position with the game element's position.
    /// </summary>
    /// <param name="element">The game element to sync with.</param>
    /// <param name="healthBar">The health bar to sync.</param>
    private void SyncHealthBarPosition(GameElement element, HealthBar healthBar)
    {
        if (element == null || healthBar == null) return;
        var screenPos = element.transform.position * 32f;
        screenPos.y += element.Blueprint.dimensions.y / 2f * 32f + HealthBarYOffset;
        healthBar.RectTransform.sizeDelta = new Vector2(element.Blueprint.dimensions.x * 32f, healthBar.RectTransform.sizeDelta.y);
        healthBar.RectTransform.anchoredPosition = screenPos;
    }
}