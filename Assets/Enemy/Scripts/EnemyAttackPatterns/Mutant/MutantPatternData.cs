using System.Collections;
using UnityEngine;

public class MutantPatternData : EnemyPatternData
{
    public override void Initialize()
    {
        AttackPatterns.Add(new JumpAttackPattern());
    }

    public class JumpAttackPattern : EnemyAttackPattern
    {
        protected override void StartAttack()
        {
            
        }
    }
}