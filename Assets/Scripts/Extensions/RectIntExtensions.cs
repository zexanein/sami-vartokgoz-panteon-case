using UnityEngine;

namespace Extensions
{
    public static partial class ExtensionMethods
    {
        #region RectInt Extensions
        public delegate void RectAction(Vector3Int coordinates);
        public delegate bool RectActionBool(Vector3Int coordinates);
        
        public static void Iterate(this RectInt rect, Vector3Int coordinates, RectAction action)
        {
            for (var x = rect.x; x < rect.xMax; x++)
            for (var y = rect.y; y < rect.yMax; y++)
            {
                action(coordinates + new Vector3Int(x, y, 0));
            }
        }
        
        public static bool Iterate(this RectInt rect, Vector3Int coordinates, RectActionBool action)
        {
            for (var x = rect.x; x < rect.xMax; x++)
            for (var y = rect.y; y < rect.yMax; y++)
            {
                if (action(coordinates + new Vector3Int(x, y, 0)))
                    return true;
            }
            
            return false;
        }
        #endregion
    }
}