using Blueprints;
using UnityEngine;

namespace GameElements
{
    public class Unit : GameElement
    {
        public int Damage { get; private set; }
        public UnitBlueprint UnitBlueprint => Blueprint as UnitBlueprint;
        
        protected override void OnInitialize()
        {
            Damage = UnitBlueprint.damagePoints;
        }

        public override void OnSelected()
        {
            SpriteRenderer.color = Color.yellow;
        }

        public override void OnDeselected()
        {
            SpriteRenderer.color = Color.white;
        }

        public void MoveToPosition(Vector3 position)
        {
            
        }
    }
}
