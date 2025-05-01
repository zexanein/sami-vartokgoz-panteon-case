using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A generic object pool for reusing GameObjects to avoid unnecessary instantiations and destructions.
/// </summary>
public class GameObjectPool : MonoBehaviour
{
    /// <summary>
    /// The prefab that will be instantiated and reused by this pool.
    /// </summary>
    public GameObject prefabToSpawn;

    /// <summary>
    /// Internal stack storing inactive GameObjects available for reuse.
    /// </summary>
    private Stack<GameObject> _pooledGameObjects = new();

    /// <summary>
    /// Retrieves an object from the pool or instantiates a new one if the pool is empty.
    /// Sets its position, rotation, and parent.
    /// </summary>
    /// <param name="position">The world position for the object.</param>
    /// <param name="rotation">The rotation to apply.</param>
    /// <param name="parent">Optional parent transform. Defaults to this pool object.</param>
    /// <returns>A GameObject to use.</returns>
    public GameObject GetFromPool(Vector3 position, Quaternion rotation, Transform parent = null)
    {
        var pooledGameObject = _pooledGameObjects.Count == 0
            ? Instantiate(prefabToSpawn)
            : _pooledGameObjects.Pop();

        pooledGameObject.transform.SetParent(parent == null ? transform : parent);
        pooledGameObject.transform.position = position;
        pooledGameObject.transform.rotation = rotation;
        pooledGameObject.transform.localScale = prefabToSpawn.transform.localScale;
        pooledGameObject.SetActive(true);

        return pooledGameObject;
    }

    /// <summary>
    /// Retrieves an object from the pool and attaches it to the given parent.
    /// Uses default position and rotation.
    /// </summary>
    /// <param name="parent">The parent transform for the object.</param>
    public GameObject GetFromPool(Transform parent) => 
        GetFromPool(transform.position, Quaternion.identity, parent);

    /// <summary>
    /// Returns a GameObject to the pool and disables it.
    /// Resets its transform and prepares it for future reuse.
    /// </summary>
    /// <param name="gameObjectToBeAdded">The GameObject to return to the pool.</param>
    public void Return(GameObject gameObjectToBeAdded)
    {
        gameObjectToBeAdded.SetActive(false);
        gameObjectToBeAdded.transform.SetParent(transform);
        gameObjectToBeAdded.transform.localPosition = Vector3.zero;
        gameObjectToBeAdded.transform.localRotation = Quaternion.identity;
        gameObjectToBeAdded.transform.localScale = prefabToSpawn.transform.localScale;

        _pooledGameObjects.Push(gameObjectToBeAdded);
    }

    /// <summary>
    /// Returns a list of GameObjects to the pool.
    /// </summary>
    /// <param name="gameObjectsToBeAdded">The list of GameObjects to return.</param>
    public void ReturnAll(List<GameObject> gameObjectsToBeAdded)
    {
        foreach (var gameObjectToBeAdded in gameObjectsToBeAdded)
            Return(gameObjectToBeAdded);
    } 
    
    /// <summary>
    /// Returns all children of the specified parent transform to the pool.
    /// </summary>
    /// <param name="parentTransform">The parent transform containing the GameObjects to return.</param>
    public void ReturnChildren(Transform parentTransform)
    {
        for (var i = 0; i < parentTransform.childCount;)
        {
            var child = parentTransform.GetChild(0).gameObject;
            Return(child);
        }
    }
}
