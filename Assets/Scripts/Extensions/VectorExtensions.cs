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
        #endregion
    }
}
