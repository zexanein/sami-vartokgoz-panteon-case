using PlacementSystem;
using UnityEngine;

namespace Pathfinding
{
    /// <summary>
    /// This class adapts the placement system to work with pathfinding.
    /// </summary>
    public class PathfindingPlacementAdapter : MonoBehaviour
    {
        /// <summary>
        /// Subscribes to the tile state change event from the PlacementManager when the script is enabled.
        /// </summary>
        private void OnEnable() => PlacementManager.Instance.OnTileStateChanged += OnCollisionsSet;
        
        /// <summary>
        /// Unsubscribes from the tile state change event from the PlacementManager when the script is disabled.
        /// </summary>
        private void OnDisable() => PlacementManager.Instance.OnTileStateChanged -= OnCollisionsSet;

        /// <summary>
        /// This method is called when the state of a tile changes in the placement system.
        /// </summary>
        /// <param name="coordinates">Coordinates of the tile in the grid.</param>
        /// <param name="state">State of the tile. <c>true</c> for occupied, <c>false</c> for free.</param>
        private void OnCollisionsSet(Vector2Int coordinates, bool state)
        {
            if (state) PathfindingManager.Instance.RegisterObstacle((Vector2) coordinates);
            else PathfindingManager.Instance.UnregisterObstacle((Vector2) coordinates);
        }
    }
}