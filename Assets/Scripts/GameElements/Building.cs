using Blueprints;
using UnityEngine;

namespace GameElements
{
    /// <summary>
    /// Represents a placed building in the game world.
    /// Inherits behavior from <see cref="GameElement"/> and adds building-specific logic and visual handling.
    /// </summary>
    public class Building : GameElement
    {
        /// <summary>
        /// Casts the base blueprint to a <see cref="BuildingBlueprint"/>.
        /// Use this to access building-specific data such as production and spawn point.
        /// </summary>
        public BuildingBlueprint BuildingBlueprint => Blueprint as BuildingBlueprint;
        
        /// <summary>
        /// The SpriteRenderer used to visually represent the building.
        /// </summary>
        private SpriteRenderer BuildingSpriteRenderer { get; set; }

        /// <summary>
        /// Called once when the building is initialized.
        /// Sets up the sprite renderer for selection effects.
        /// </summary>
        protected override void OnInitialize()
        {
            BuildingSpriteRenderer = GetComponent<SpriteRenderer>();
        }

        /// <summary>
        /// Visually selects the building by changing its color.
        /// </summary>
        public override void Select() => BuildingSpriteRenderer.color = Color.yellow;

        /// <summary>
        /// Visually deselects the building by resetting its color to white.
        /// </summary>
        public override void Deselect() => BuildingSpriteRenderer.color = Color.white;
    }
}