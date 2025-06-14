using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public abstract class EnemyPatternData : MonoBehaviour
{
    [HideInInspector] public List<IEnemyAttackPattern> AttackPatterns;

    private void Awake() => Initialize();
    public abstract void Initialize();
}