using UnityEngine;

[CreateAssetMenu(fileName = "Boss_Range_ComboAttackModuleData", menuName = "Enemy/AttackModule/Boss_Range_ComboAttackModule")]
public class SO_Boss_Range_ComboAttackModule : SO_RangeModule
{
    public float Interval;
    public override void StartAttack(EnemyBase owner)
    {
    }

    public override void UpdateAttack(float time, EnemyBase owner, ref float prevAttackTime)
    {
        //������ �����ϰ������ ��� ������ ��� ���� �����ߴ� �ð��� ������ ���� �ʿ��ϴ�
        if(Time.time > prevAttackTime + Interval )
        {
            Transform targetTrf = owner.Detector.GetLatestTarget();
            Transform firePos = owner.FirePos;

            ShootProjectile(targetTrf, firePos);
            prevAttackTime = Time.time;
        }
    }
    protected void ComboShootProjectile(Transform targetTrf, Transform firePos)
    {
        ShootProjectile(targetTrf, firePos);
    }
}
