using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Pathfinding
{
    /// <summary>
    /// This class manages the path followers in the game.
    /// </summary>
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

        /// <summary>
        /// List of active path followers.
        /// </summary>
        private List<UnitPathFollower> ActivePathFollowers { get; } = new();

        /// <summary>
        /// Registers a unit path follower to the list of active path followers.
        /// </summary>
        /// <param name="follower">The unit path follower to register.</param>
        public void RegisterUnitPathFollower(UnitPathFollower follower)
        {
            if (!ActivePathFollowers.Contains(follower)) ActivePathFollowers.Add(follower);
        }

        /// <summary>
        /// Unregisters a unit path follower from the list of active path followers.
        /// </summary>
        /// <param name="follower">The unit path follower to unregister.</param>
        public void UnregisterUnitPathFollower(UnitPathFollower follower)
        {
            ActivePathFollowers.Remove(follower);
        }

        /// <summary>
        /// Checks if any unit is moving to the specified coordinates.
        /// </summary>
        /// <param name="coordinates">The coordinates to check.</param>
        /// <returns></returns>
        private bool IsAnyUnitMovingTo(Vector2Int coordinates)
        {
            return ActivePathFollowers.Any(f => f.Destination == coordinates);
        }

        /// <summary>
        /// Removes coordinates from end of the path if any unit is moving to that coordinate.
        /// </summary>
        /// <param name="path">List of coordinates representing the path.</param>
        public void GetBestAvailablePath(List<Vector2Int> path)
        {   
            while (path.Count > 1 && IsAnyUnitMovingTo(path[^1])) path.RemoveAt(path.Count - 1);
        }
    }
}
