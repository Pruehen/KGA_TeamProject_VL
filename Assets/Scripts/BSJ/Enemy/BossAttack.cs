using System;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack
{
    private int _phase = 0;
    private BossAttackRangeType _attackRangeType = BossAttackRangeType.Close;

    [SerializeField] private BossAttackModule[] _modules;
    [SerializeField] private BossAttackModule _defaultCloseAttack;
    public void SetAttackPhaseType(int phase)
    {
        _phase = phase;
    }

    public void SetAttackRangeType(BossAttackRangeType attackRangeType)
    {
        _attackRangeType = attackRangeType;
    }

    public BossAttackModule TryGetRandomAvailableAttack()
    {

        List<BossAttackModule> availableAttacks = new List<BossAttackModule>();

        foreach (BossAttackModule attack in _modules)
        {
            if (attack.IsAttackable(_attackRangeType, _phase))
            {
                availableAttacks.Add(attack);
            }
        }

        if (availableAttacks.Count == 0 && _attackRangeType == BossAttackRangeType.Close)
        {
            availableAttacks.Add(_defaultCloseAttack);
        }

        if (availableAttacks.Count == 0)
        {
            return null;
        }

        return availableAttacks[UnityEngine.Random.Range(0, availableAttacks.Count)];

    }
}
