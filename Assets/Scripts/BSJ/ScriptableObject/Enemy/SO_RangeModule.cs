using EnumTypes;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "RangeAttackModuleData", menuName = "Enemy/AttackModule/AttackRange")]
public class SO_RangeModule : SO_AttackModule
{
    public GameObject Prefab_projectile;
    public float ProjectileDamage;
    public float ProjectileSpeed;
}
