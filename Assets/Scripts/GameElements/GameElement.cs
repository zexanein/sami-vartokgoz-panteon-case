using System;
using System.Collections.Generic;
using Blueprints;
using CombatSystem;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace GameElements
{
    public class GameElement : MonoBehaviour, IDamageable, ISelectable
    {
        private Vector3Int _coordinates;
        public Vector3Int Coordinates
        {
            get => _coordinates;
            set
            {
                if (value != _coordinates) OnCoordinatesChanged?.Invoke(this, (Vector2Int) _coordinates, (Vector2Int) value);
                _coordinates = value;
            }
        }
        public GameElementBlueprint Blueprint { get; private set; }
        public Tilemap ParentTilemap { get; private set; }
        public int Health { get; private set; }
        private int MaxHealth { get; set; }
        public bool IsDead => Health <= 0;
        public GameObject enableOnSelected;
        
        public delegate void OnCoordinatesChangedHandler(GameElement element, Vector2Int oldCoordinates, Vector2Int newCoordinates);
        public event OnCoordinatesChangedHandler OnCoordinatesChanged;
        public delegate void OnElementDestroyedHandler(GameElement element);
        public event OnElementDestroyedHandler OnElementDestroyed;
        public event IDamageable.OnDamagedHandler OnDamaged;
        
        public void Initialize(GameElementBlueprint blueprint, Vector3Int coordinates, Tilemap parentTilemap)
        {
            Blueprint = blueprint;
            ParentTilemap = parentTilemap;
            MaxHealth = blueprint.healthPoints;
            Health = MaxHealth;
            _coordinates = coordinates;
            OnInitialize();
        }
    
        protected virtual void OnInitialize() { }
        public virtual void Select() { }
        public virtual void Deselect() { }
        public virtual void InteractWithOther(Vector3 mousePosition, ISelectable other) { }

        public void TakeDamage(int amount)
        {
            Health -= amount;
            OnDamaged?.Invoke();
            if (Health > 0) return;
            Die();
        }

        private void Die()
        {
            Health = 0;
            DestroyElement();
        }

        public void DestroyElement()
        {   
            OnElementDestroyed?.Invoke(this);
            Destroy(gameObject);
        }

        public List<Vector2Int> GetCoordinatesAround()
        {
            var bottomLeft = (Vector2Int) Coordinates - Blueprint.dimensions / 2;
            var topRight   = (Vector2Int) Coordinates + Blueprint.dimensions / 2 + Vector2Int.FloorToInt(Blueprint.CenterOffset - (Vector2)ParentTilemap.cellSize / 2f);
            
            var expandedBottomLeft = bottomLeft + new Vector2Int(-1, -1);
            var expandedTopRight = topRight + new Vector2Int(1, 1);

            var perimeterPoints = new List<Vector2Int>();

            for (var x = expandedBottomLeft.x; x <= expandedTopRight.x; x++)
            {
                perimeterPoints.Add(new Vector2Int(x, expandedBottomLeft.y));
                perimeterPoints.Add(new Vector2Int(x, expandedTopRight.y));
            }

            for (var y = expandedBottomLeft.y + 1; y < expandedTopRight.y; y++)
            {
                perimeterPoints.Add(new Vector2Int(expandedBottomLeft.x, y));
                perimeterPoints.Add(new Vector2Int(expandedTopRight.x, y));
            }

            return perimeterPoints;
        }
    }
}
