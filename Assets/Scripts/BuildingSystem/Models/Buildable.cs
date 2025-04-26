using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using Extensions;
using Object = UnityEngine.Object;

namespace BuildingSystem.Models
{
    [Serializable]
    public class Buildable
    {
        public Tilemap parentTilemap;
        public BuildingData buildableType;
        public GameObject gameObject;
        public Vector3Int coordinates;

        public Buildable (BuildingData buildableType, GameObject gameObject, Vector3Int coordinates, Tilemap parentTilemap)
        {
             this.buildableType = buildableType;
             this.gameObject = gameObject;
             this.coordinates = coordinates;
             this.parentTilemap = parentTilemap;
        }

        public void Destroy()
        {
            if (gameObject != null) Object.Destroy(gameObject);
        }

        public void IterateCollisionSpace(ExtensionMethods.RectAction action)
        {
            buildableType.collisionSpace.Iterate(coordinates, action);
        }

        public bool IterateCollisionSpace(ExtensionMethods.RectActionBool action)
        {
            return buildableType.collisionSpace.Iterate(coordinates, action);
        }
    }
}