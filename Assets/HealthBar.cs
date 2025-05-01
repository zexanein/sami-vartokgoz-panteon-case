using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class represents a health bar UI element.
/// </summary>
public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image barImage;
    
    private RectTransform _rectTransform;

    /// <summary>
    /// Gets the RectTransform component of the health bar.
    /// </summary>
    public RectTransform RectTransform
    {
        get
        {
            if (_rectTransform == null) _rectTransform = transform as RectTransform;
            return _rectTransform;
        }
    }

    /// <summary>
    /// Updates the health bar's fill amount based on the current and maximum health values.
    /// </summary>
    public void SetHealthValues(float health, float maxHealth) => barImage.fillAmount = Mathf.Clamp01(health / maxHealth);
}
