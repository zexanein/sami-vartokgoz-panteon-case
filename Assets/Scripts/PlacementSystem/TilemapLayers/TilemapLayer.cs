using Blueprints;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace PlacementSystem.TilemapLayers
{
    /// <summary>
    /// Base class for systems that interact with a Tilemap.
    /// Provides utility methods for converting grid coordinates to precise world positions
    /// </summary>
    [RequireComponent(typeof(Tilemap))]
    public class TilemapLayer : MonoBehaviour
    {
        /// <summary>
        /// Reference to the tilemap component used for placement and positioning.
        /// </summary>
        protected Tilemap TilemapReference { get; private set; }

        /// <summary>
        /// Automatically caches the Tilemap component on Awake.
        /// </summary>
        protected void Awake() => TilemapReference = GetComponent<Tilemap>();

        /// <summary>
        /// Calculates the final world position for a game element based on its type and grid coordinates.
        /// </summary>
        /// <param name="coordinates">The grid coordinates where the element is to be placed.</param>
        /// <param name="elementBlueprint">The blueprint describing the element's properties.</param>
        /// <returns>The exact world position for rendering or instantiation.</returns>
        protected Vector3 GetPositionForElement(Vector3Int coordinates, GameElementBlueprint elementBlueprint)
        {
            return elementBlueprint switch
            {
                BuildingBlueprint buildingBlueprint => GetPositionForBuilding(coordinates, buildingBlueprint),
                UnitBlueprint unitBlueprint => GetPositionForUnit(coordinates, unitBlueprint),
                _ => Vector3.zero
            };
        }

        /// <summary>
        /// Computes the world position for a building element.
        /// Takes into account cell size, center offset, and placement offset.
        /// </summary>
        private Vector3 GetPositionForBuilding(Vector3Int coordinates, BuildingBlueprint buildingBlueprint)
        {
            return
                TilemapReference.CellToWorld(coordinates) +
                TilemapReference.cellSize / 2 +
                (Vector3)buildingBlueprint.CenterOffset +
                (Vector3Int)buildingBlueprint.placementOffset;
        }

        /// <summary>
        /// Computes the world position for a unit element.
        /// Assumes center alignment within a single tile.
        /// </summary>
        private Vector3 GetPositionForUnit(Vector3Int coordinates, UnitBlueprint unitBlueprint)
        {
            return
                TilemapReference.CellToWorld(coordinates) +
                TilemapReference.cellSize / 2;
        }
    }
}
