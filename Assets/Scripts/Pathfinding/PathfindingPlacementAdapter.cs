using PlacementSystem;
using UnityEngine;

namespace Pathfinding
{
    public class PathfindingPlacementAdapter : MonoBehaviour
    {
        private void OnEnable() => PlacementManager.Instance.OnTileStateChanged += OnCollisionsSet;
        private void OnDisable() => PlacementManager.Instance.OnTileStateChanged -= OnCollisionsSet;

        private void OnCollisionsSet(Vector2Int coordinates, bool state)
        {
            if (state) PathfindingManager.Instance.RegisterObstacle((Vector2) coordinates);
            else PathfindingManager.Instance.UnregisterObstacle((Vector2) coordinates);
        }
    }
}