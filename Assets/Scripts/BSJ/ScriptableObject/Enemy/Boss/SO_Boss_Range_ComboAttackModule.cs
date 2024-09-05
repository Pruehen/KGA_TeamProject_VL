using UnityEngine;

[CreateAssetMenu(fileName = "Boss_Range_ComboAttackModuleData", menuName = "Enemy/AttackModule/Boss_Range_ComboAttackModule")]
public class SO_Boss_Range_ComboAttackModule : SO_RangeModule
{
    public float Interval;
    public override void StartAttack(EnemyBase owner)
    {
    }

    public override void UpdateAttack(float deltatime, EnemyBase owner)
    {
        base.UpdateAttack(deltatime, owner);
        //여러번 공격하고싶은데 멤버 변수가 없어서 전에 공격했던 시간을 저장할 곳이 필요하다
        if (Time.time > owner.Attack.CurrentAttack.PrevFireTime + Interval )
        {
            Transform targetTrf = owner.Detector.GetLatestTarget();
            Transform firePos = owner.FirePos;

            ShootProjectile(targetTrf, firePos);
            owner.Attack.CurrentAttack.PrevFireTime = Time.time;
        }
    }
    protected void ComboShootProjectile(Transform targetTrf, Transform firePos)
    {
        ShootProjectile(targetTrf, firePos);
    }
}
