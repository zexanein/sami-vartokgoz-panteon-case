using System.Collections.Generic;
using System.Linq;
using Pathfinding;
using UnityEngine;

public class PathFollowerManager : MonoBehaviour
{
    #region Singleton
    private static PathFollowerManager _instance;
    public static PathFollowerManager Instance
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<PathFollowerManager>();
            return _instance;
        }
    }
    #endregion

    private List<UnitPathFollower> ActivePathFollowers { get; } = new();

    public void RegisterUnitPathFollower(UnitPathFollower follower)
    {
        if (!ActivePathFollowers.Contains(follower)) ActivePathFollowers.Add(follower);
    }

    public void UnregisterUnitPathFollower(UnitPathFollower follower)
    {
        ActivePathFollowers.Remove(follower);
    }

    public bool IsAnyUnitMovingTo(Vector2Int coordinates)
    {
        return ActivePathFollowers.Any(f => f.Destination == coordinates);
    }

    public void GetBestAvailablePath(List<Vector2Int> path)
    {   
        while (path.Count > 1 && IsAnyUnitMovingTo(path[^1])) path.RemoveAt(path.Count - 1);
    }
}
