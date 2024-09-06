using UnityEngine;

[CreateAssetMenu(fileName = "Boss_Range_ComboAttackModuleData", menuName = "Enemy/AttackModule/Boss_Range_ComboAttackModule")]
public class SO_Boss_Range_ComboAttackModule : SO_RangeModule
{
    public float Interval;

    public override void StartAttack(EnemyBase owner, int type)
    {
        //base.StartAttack(owner, type);
    }
    public override void UpdateAttack(EnemyBase owner, int type, float deltaTime)
    {
        base.UpdateAttack(owner, type, deltaTime);
        //������ �����ϰ������ ��� ������ ��� ���� �����ߴ� �ð��� ������ ���� �ʿ��ϴ�
        if (Time.time > owner.Attack.CurrentAttack.PrevFireTime + Interval)
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
