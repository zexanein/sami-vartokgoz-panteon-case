using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool : MonoBehaviour
{
    public GameObject prefabToSpawn;
    private Stack<GameObject> _pooledGameObjects = new();

    public GameObject GetFromPool(Vector3 position, Quaternion rotation, Transform parent = null)
    {
        var pooledGameObject = _pooledGameObjects.Count == 0
            ? Instantiate(prefabToSpawn)
            : _pooledGameObjects.Pop();
        
        pooledGameObject.transform.SetParent(parent == null ? transform : parent);
        pooledGameObject.transform.position = position;
        pooledGameObject.transform.rotation = rotation;
        pooledGameObject.SetActive(true);
        return pooledGameObject;
    }

    public GameObject GetFromPool(Transform parent) => GetFromPool(transform.position, Quaternion.identity, parent);

    public void ReturnToPool(GameObject gameObjectToBeAdded)
    {
        gameObjectToBeAdded.SetActive(false);
        gameObjectToBeAdded.transform.SetParent(transform);
        gameObjectToBeAdded.transform.localPosition = Vector3.zero;
        gameObjectToBeAdded.transform.localRotation = Quaternion.identity;
        gameObjectToBeAdded.transform.localScale = prefabToSpawn.transform.localScale;
        _pooledGameObjects.Push(gameObjectToBeAdded);
    }
    public void ReturnAllToPool(List<GameObject> gameObjectsToBeAdded)
    {
        foreach (var gameObjectToBeAdded in gameObjectsToBeAdded)
            ReturnToPool(gameObjectToBeAdded);
    }
}
