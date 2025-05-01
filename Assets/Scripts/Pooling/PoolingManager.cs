using UnityEngine;

namespace Pooling
{
    /// <summary>
    /// This class contains a singleton instance.
    /// It is working as a shorthand to access different pools.
    /// </summary>
    public class PoolingManager : MonoBehaviour
    {
        #region Singleton
        private static PoolingManager _instance;
        public static PoolingManager Instance
        {
            get
            {
                if (_instance == null) _instance = FindObjectOfType<PoolingManager>();
                return _instance;
            }
        }
        #endregion
    
        /// <summary>
        /// Pool for the path visual elements.
        /// </summary>
        [field: SerializeField] public GameObjectPool PathVisualPool { get; private set; }
    
        /// <summary>
        /// Pool for the health bar elements.
        /// </summary>
        [field: SerializeField] public GameObjectPool HealthBarPool { get; private set; }
    
        /// <summary>
        /// Pool for the unit produce buttons.
        /// </summary>
        [field: SerializeField] public GameObjectPool UnitProduceButtonPool { get; private set; }
    }
}
