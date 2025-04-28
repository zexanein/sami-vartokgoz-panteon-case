using Blueprints;

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
    }
}
