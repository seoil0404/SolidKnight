using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using Combo = PlayerCombatHandler.Combo;

[CreateAssetMenu(fileName = "ComboData", menuName = "Scriptable Objects/ComboData")]
public class ComboData : ScriptableObject
{
    public List<Combo> List;
}
