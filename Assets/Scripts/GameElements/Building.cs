using Blueprints;

namespace GameElements
{
    public class Building : GameElement
    {
        public BuildingBlueprint BuildingBlueprint => Blueprint as BuildingBlueprint;
    }
}