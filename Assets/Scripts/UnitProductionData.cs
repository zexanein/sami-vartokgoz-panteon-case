using System.Collections.Generic;
using Blueprints;
using UnityEngine;

[CreateAssetMenu(menuName = "Units/Production Data", fileName = "UnitProductionData")]
public class UnitProductionData : ScriptableObject
{
    public List<UnitBlueprint> blueprints = new();
}