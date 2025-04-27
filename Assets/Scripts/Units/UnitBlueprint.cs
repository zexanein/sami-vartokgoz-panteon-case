using UnityEngine;
using UnityEngine.Serialization;

namespace Units
{
    [CreateAssetMenu(menuName = "Unit", fileName = "NewUnit")]
    public class UnitBlueprint : ScriptableObject
    {
        public string unitName;
        public int healthPoints;
        public int damagePoints;
        public Sprite displaySprite;
        public Sprite uiIcon;
        public GameObject unitPrefab;
    }
}
