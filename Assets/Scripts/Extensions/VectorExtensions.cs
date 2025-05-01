using UnityEngine;

namespace Extensions
{
    /// <summary>
    /// Extension methods for Unity's Vector2Int, Vector3Int, and Vector3 types.
    /// Provides common transformation and iteration utilities.
    /// </summary>
    public static partial class ExtensionMethods 
    {
        #region Vector3 Extensions
        /// <summary>
        /// Returns a copy of the vector with a modified X component.
        /// </summary>
        public static Vector3 WithX(this Vector3 v3, float newX)
        {
            v3.x = newX;
            return v3;
        }

        /// <summary>
        /// Returns a copy of the vector with a modified Y component.
        /// </summary>
        public static Vector3 WithY(this Vector3 v3, float newY)
        {
            v3.y = newY;
            return v3;
        }

        /// <summary>
        /// Returns a copy of the vector with a modified Z component.
        /// </summary>
        public static Vector3 WithZ(this Vector3 v3, float newZ)
        {
            v3.z = newZ;
            return v3;
        }

        /// <summary>
        /// Returns a new vector by adding the same value to all components (x, y, z).
        /// </summary>
        public static Vector3 Add(this Vector3 v3, float add)
        {
            v3.x += add;
            v3.y += add;
            v3.z += add;
            return v3;
        }
        #endregion

        #region Vector3Int Extensions
        /// <summary>
        /// Returns a new vector by adding the same integer value to all components (x, y, z).
        /// </summary>
        public static Vector3Int Add(this Vector3Int v3, int add)
        {
            v3.x += add;
            v3.y += add;
            v3.z += add;
            return v3;
        }

        /// <summary>
        /// Returns a new Vector3 with each component of the Vector3Int modulo the given float value.
        /// </summary>
        public static Vector3 Mod(this Vector3Int v3, float mod)
        {
            Vector3 result = v3;
            result.x %= mod;
            result.y %= mod;
            result.z %= mod;
            return result;
        }
        #endregion

        #region Vector2Int Extensions
        /// <summary>
        /// Delegate for applying an action to a Vector2Int coordinate.
        /// </summary>
        public delegate void Vector2IntAction(Vector2Int coordinates);

        /// <summary>
        /// Delegate for applying a boolean-returning action to a Vector2Int coordinate.
        /// </summary>
        public delegate bool Vector2IntActionBool(Vector2Int coordinates);

        /// <summary>
        /// Iterates over a rectangular grid (based on dimensions) centered at the given coordinates, 
        /// executing the specified action on each cell.
        /// </summary>
        /// <param name="dimensions">The width and height of the area to iterate.</param>
        /// <param name="coordinates">The center point of the iteration.</param>
        /// <param name="action">An action to execute per coordinate.</param>
        public static void Iterate(this Vector2Int dimensions, Vector2Int coordinates, Vector2IntAction action)
        {
            coordinates -= dimensions / 2;

            for (var x = 0; x < dimensions.x; x++)
            for (var y = 0; y < dimensions.y; y++)
            {
                action(coordinates + new Vector2Int(x, y));
            }
        }

        /// <summary>
        /// Iterates over a rectangular grid (based on dimensions) centered at the given coordinates,
        /// returning true immediately if the action returns true for any coordinate.
        /// </summary>
        /// <param name="dimensions">The width and height of the area to iterate.</param>
        /// <param name="coordinates">The center point of the iteration.</param>
        /// <param name="action">A boolean-returning action to test each coordinate.</param>
        /// <returns>True if the action returns true for any coordinate; otherwise, false.</returns>
        public static bool Iterate(this Vector2Int dimensions, Vector2Int coordinates, Vector2IntActionBool action)
        {
            coordinates -= dimensions / 2;

            for (var x = 0; x < dimensions.x; x++)
            for (var y = 0; y < dimensions.y; y++)
            {
                if (action(coordinates + new Vector2Int(x, y))) return true;
            }

            return false;
        }

        /// <summary>
        /// Returns a new Vector2Int by adding the same integer value to both X and Y components.
        /// </summary>
        public static Vector2Int Add(this Vector2Int v2Int, int add)
        {
            v2Int.x += add;
            v2Int.y += add;
            return v2Int;
        }

        /// <summary>
        /// Returns a Vector2 with each component of the Vector2Int modulo the given float value.
        /// </summary>
        public static Vector2 Mod(this Vector2Int v2Int, float mod)
        {
            Vector2 result = v2Int;
            result.x %= mod;
            result.y %= mod;
            return result;
        }
        #endregion
    }
}
