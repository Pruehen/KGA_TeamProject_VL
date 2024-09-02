using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour, ITargetable
{
    [SerializeField] protected Combat _combat;
    [SerializeField] private SO_SKillEvent hitVFX;
    // interface
    float _debuff_Passive_Offensive2_IncreasedDamageTakenMulti = 1;
    public void ActiveDebuff_Passive_Offensive2(float value)
    {
        _debuff_Passive_Offensive2_IncreasedDamageTakenMulti = 1 + value;
    }
    public virtual Vector3 GetPosition()
    {
        return transform.position;
    }

    public virtual void Hit(float dmg, DamageType type = DamageType.Normal)
    {
        dmg *= _debuff_Passive_Offensive2_IncreasedDamageTakenMulti;
        _debuff_Passive_Offensive2_IncreasedDamageTakenMulti = 1;
        _combat.Damaged(dmg, type);
        GameObject hitEF = ObjectPoolManager.Instance.DequeueObject(hitVFX.preFab);
        Vector3 finalPosition = this.transform.position + transform.TransformDirection(hitVFX.offSet);
        hitEF.transform.position = finalPosition;
        DmgTextManager.Instance.OnDmged(dmg, this.transform.position);
    }

    public virtual bool IsDead()
    {
        return _combat.IsDead();
    }
}
