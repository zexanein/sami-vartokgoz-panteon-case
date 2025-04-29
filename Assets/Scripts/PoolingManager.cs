using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    
    [field: SerializeField] public GameObjectPool PathVisualPool { get; private set; }
}
