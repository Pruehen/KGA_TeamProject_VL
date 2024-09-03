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
    private void EnableDamageBox()
    {
        _owner.Attack.EnableDamageBox();
    }
    private void ShootProjectile()
    {
        _owner.Attack.ShootProjectile();
    }
    private void Move_Attack(int type)
    {
        _owner.Attack.StartAttackMove(type);
    }
}
