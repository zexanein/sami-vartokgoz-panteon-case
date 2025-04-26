using UnityEngine;
using UnityEngine.Tilemaps;

namespace BuildingSystem.TilemapLayers
{
    [RequireComponent(typeof(Tilemap))]
    public class TilemapLayer : MonoBehaviour
    {
        protected Tilemap Tilemap { get; private set; }

        protected void Awake()
        {
            Tilemap = GetComponent<Tilemap>();
        }
    }
}
