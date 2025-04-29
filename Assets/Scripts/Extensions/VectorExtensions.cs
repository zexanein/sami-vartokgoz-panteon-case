using UnityEngine;

namespace Extensions
{
    public static partial class ExtensionMethods 
    {
        #region Vector3 Extensions
        public static Vector3 WithX(this Vector3 v3, float newX)
        {
            v3.x = newX;
            return v3;
        }
                    
        public static Vector3 WithY(this Vector3 v3, float newY)
        {
            v3.y = newY;
            return v3;
        }
                    
        public static Vector3 WithZ(this Vector3 v3, float newZ)
        {
            v3.z = newZ;
            return v3;
        }
                    
        public static Vector3 Add(this Vector3 v3, float add)
        {
            v3.x += add;
            v3.y += add;
            v3.z += add;
            return v3;
        }
        #endregion

        #region Vector3Int Extensions
                    
        public static Vector3Int Add(this Vector3Int v3, int add)
        {
            v3.x += add;
            v3.y += add;
            v3.z += add;
            return v3;
        }
                    
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
        public delegate void Vector2IntAction(Vector2Int coordinates);
        public delegate bool Vector2IntActionBool(Vector2Int coordinates);

        public static void Iterate(this Vector2Int dimensions, Vector2Int coordinates, Vector2IntAction action)
        {
            coordinates -= dimensions / 2;
            
            for (var x = 0; x < dimensions.x; x++)
            for (var y = 0; y < dimensions.y; y++)
            {
                action(coordinates + new Vector2Int(x, y));
            }
        }

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

        public static Vector2Int Add(this Vector2Int v2Int, int add)
        {
            v2Int.x += add;
            v2Int.y += add;
            return v2Int;
        }
                    
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
