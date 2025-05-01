using Blueprints;
using UnityEngine;

namespace GameElements
{
    public class Building : GameElement
    {
        public BuildingBlueprint BuildingBlueprint => Blueprint as BuildingBlueprint;
        public SpriteRenderer BuildingSpriteRenderer { get; set; }

        protected override void OnInitialize()
        {
            BuildingSpriteRenderer = GetComponent<SpriteRenderer>();
        }

        public override void Select()
        {
            BuildingSpriteRenderer.color = Color.yellow;
        }

        public override void Deselect()
        {
            BuildingSpriteRenderer.color = Color.white;
        }
    }
}