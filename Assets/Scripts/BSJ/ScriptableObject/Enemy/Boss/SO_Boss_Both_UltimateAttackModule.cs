using EnumTypes;
using UnityEngine;

[CreateAssetMenu(fileName = "Boss_Both_UltimateAttackModule", menuName = "Enemy/AttackModule/Boss_Both_UltimateAttack")]
public class SO_Boss_Both_UltimateAttackModule : SO_AttackModule
{
    public GameObject Prefab_areaAttack;
    public GameObject Prefab_Spike;
    public float MaxArea = 50f;
    public float SpikeGap = 4f;
    public float SpikeRandomOffset = 2f;
    public float Probability = .3f;

    public override void StartAttack(EnemyBase owner, int type)
    {
        switch (type)
        {
            case 0:
                Transform targetTrf = owner.transform;
                Vector3 targetPos = targetTrf.position;
                targetPos.y = 0f;
                GameObject projectileObject = GameObject.Instantiate(Prefab_areaAttack,
                    targetPos, Quaternion.identity);
                BossDoubleAreaAttack areaAttack = projectileObject.GetComponent<BossDoubleAreaAttack>();
                areaAttack.Init(Damage, owner.Attack.RangeTypeThreshold, IsClose(owner));
                owner.Attack.CurrentProjectile = areaAttack;
                break;
            case 1:
                owner.Attack.CurrentProjectile.Trigger();
                SpawnMultipleSpikeInArea(owner, Prefab_Spike, MaxArea, SpikeGap, Probability, SpikeRandomOffset);
                break;
        }

    }
    public override void StartAttackMove(EnemyBase owner, int type)
    {
        owner.transform.position = owner.SpawnedPosition;
    }

    private void SpawnMultipleSpikeInArea(EnemyBase owner, GameObject spike, float areaRadius, float gap, float probability, float spikeRandomOffset)
    {
        Vector3 centerOffset = new Vector3(areaRadius * .5f, 0f, areaRadius * .5f);
        for (int i = 0; i < areaRadius / gap; i++)
            for (int k = 0; k < areaRadius / gap; k++)
            {
                if (Random.value < probability)
                {
                    Vector3 offset = new Vector3(i * gap, 0f, k * gap) - centerOffset;
                    Vector3 pos = owner.transform.position + offset;
                    SpawnSpike(owner, spike, pos, spikeRandomOffset);
                }
            }
    }

    private AttackRangeType IsClose(EnemyBase owner)
    {
        return owner.Attack.GetAttackRangeType();
    }

    private void SpawnSpike(EnemyBase owner, GameObject spike, Vector3 position, float spikeRandomOffset)
    {
        Vector3 targetPos = position;
        targetPos.y = 0f;
        targetPos += new Vector3(Random.value * spikeRandomOffset, 0f, Random.value * spikeRandomOffset);
        GameObject projectileObject = GameObject.Instantiate(spike,
            targetPos, Quaternion.identity);
    }
}
