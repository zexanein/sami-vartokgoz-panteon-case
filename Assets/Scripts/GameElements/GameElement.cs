using System.Collections.Generic;
using Blueprints;
using CombatSystem;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace GameElements
{
    /// <summary>
    /// Represents a game element that can be placed on a tilemap
    /// Common base class for buildings, units, or other grid-based entities.
    /// Implements <see cref="IDamageable"/> for health management and <see cref="ISelectable"/> for selection handling.
    /// </summary>
    public class GameElement : MonoBehaviour, IDamageable, ISelectable
    {
        private Vector3Int _coordinates;
        
        /// <summary>
        /// The current grid coordinates of the element.
        /// Fires <see cref="OnCoordinatesChanged"/> when updated.
        /// </summary>
        public Vector3Int Coordinates
        {
            get => _coordinates;
            set
            {
                if (value != _coordinates) OnCoordinatesChanged?.Invoke(this, (Vector2Int) _coordinates, (Vector2Int) value);
                _coordinates = value;
            }
        }
        
        /// <summary>
        /// The blueprint that defines the properties and behavior of this game element.
        /// </summary>
        public GameElementBlueprint Blueprint { get; private set; }
        
        /// <summary>
        /// The tilemap this element is placed on.
        /// </summary>
        public Tilemap ParentTilemap { get; private set; }
        
        /// <summary>
        /// The current health of the element.
        /// </summary>
        public int Health { get; private set; }
        
        /// <summary>
        /// The maximum health of the element.
        /// </summary>
        private int MaxHealth { get; set; }
        
        /// <summary>
        /// Whether the element is dead
        /// </summary>
        public bool IsDead => Health <= 0;
        
        /// <summary>
        /// Optional GameObject that will be enabled when the element is selected. and disabled when deselected.
        /// </summary>
        public GameObject enableOnSelected;

        /// <summary>
        /// Triggered when the element's coordinates change.
        /// </summary>
        public delegate void OnCoordinatesChangedHandler(GameElement element, Vector2Int oldCoordinates, Vector2Int newCoordinates);
        public event OnCoordinatesChangedHandler OnCoordinatesChanged;
        
        /// <summary>
        /// Triggered when the element is destroyed. (via damage or manual destruction)
        /// </summary>
        public delegate void OnElementDestroyedHandler(GameElement element);
        public event OnElementDestroyedHandler OnElementDestroyed;
        
        /// <summary>
        /// Triggered when the element takes damage.
        /// </summary>
        public event IDamageable.OnDamagedHandler OnDamaged;

        /// <summary>
        /// Initializes the game element with the provided blueprint, coordinates, and parent tilemap.
        /// </summary>
        /// <param name="blueprint">The blueprint that defines the properties and behavior of this game element.</param>
        /// <param name="coordinates">The grid coordinates where the element will be placed.</param>
        /// <param name="parentTilemap">The tilemap this element is placed on.</param>
        public void Initialize(GameElementBlueprint blueprint, Vector3Int coordinates, Tilemap parentTilemap)
        {
            Blueprint = blueprint;
            ParentTilemap = parentTilemap;
            MaxHealth = blueprint.healthPoints;
            Health = MaxHealth;
            _coordinates = coordinates;
            OnInitialize();
        }

        /// <summary>
        /// Called when the element is initialized. Can be overridden in derived classes to perform additional setup.
        /// </summary>
        protected virtual void OnInitialize() { }
        
        /// <summary>
        /// Called when the element is selected. Can be overridden
        /// </summary>
        public virtual void Select() { }
        
        /// <summary>
        /// Called when the element is deselected. Can be overridden
        /// </summary>
        public virtual void Deselect() { }
        
        /// <summary>
        /// Handles contextual interaction with another selectable at the given world position.
        /// </summary>
        public virtual void InteractWithOther(Vector3 mousePosition, ISelectable other) { }

        /// <summary>
        /// Applies damage to the element and triggers death if health reaches zero.
        /// </summary>
        public void TakeDamage(int amount)
        {
            Health -= amount;
            OnDamaged?.Invoke();
            if (Health > 0) return;
            Die();
        }

        /// <summary>
        /// Marks the element as dead and triggers destruction.
        /// </summary>
        private void Die()
        {
            Health = 0;
            DestroyElement();
        }

        /// <summary>
        /// Destroys the element and triggers the OnElementDestroyed event.
        /// </summary>
        public void DestroyElement()
        {   
            OnElementDestroyed?.Invoke(this);
            Destroy(gameObject);
        }

        /// <summary>
        /// Gets the coordinates of the element's perimeter points.
        /// </summary>
        /// <returns>A list of coordinates representing the perimeter points of the element.</returns>
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
