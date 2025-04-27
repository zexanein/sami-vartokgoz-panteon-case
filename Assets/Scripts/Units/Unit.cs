using UnityEngine;

namespace Units
{
    public class Unit : MonoBehaviour
    {
        public UnitBlueprint Blueprint { get; private set; }
        public Vector3Int Coordinates { get; private set; }

        public void Initialize(UnitBlueprint unitBlueprint, Vector3Int coordinates)
        {
            Blueprint = unitBlueprint;
            Coordinates = coordinates;
        }
    }
}
