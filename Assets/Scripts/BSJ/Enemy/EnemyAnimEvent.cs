using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimEvent : MonoBehaviour
{
    EnemyBase _owner;
    private void Awake()
    {
        _owner = GetComponent<EnemyBase>();
    }
    private void StartAttackModulAttack(int type)
    {
        _owner.Attack.StartModulAttack(type);
    }
    private void StartAttackModulMove(int type)
    {
        _owner.Attack.StartAttackMove(type);
    }
}
