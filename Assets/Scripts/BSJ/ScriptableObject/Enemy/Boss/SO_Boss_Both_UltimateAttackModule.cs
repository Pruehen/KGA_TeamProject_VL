using EnumTypes;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Boss_Both_UltimateAttackModule", menuName = "Enemy/AttackModule/Boss_Both_UltimateAttack")]
public class SO_Boss_Both_UltimateAttackModule : SO_AttackModule
{
    public GameObject Prefab_areaAttack;
    public GameObject Prefab_Spike;
    public float MaxArea = 50f;
    public float SpikeGap = 4f;
    public float SpikeRandomOffset = 2f;
    public float SpikeProbability = .3f;

    public override void StartAttack(EnemyBase owner, int type)
    {

        switch (type)
        {
            case 0:
                // Start VFXs
                owner.StartVFX("Boss_Teleport_End1");
                owner.StartVFX("Boss_Teleport_End2");

                // Spawn Attack Area
                Transform targetTrf = owner.transform;
                Vector3 targetPos = targetTrf.position;
                targetPos.y = 0f;
                GameObject attackArea_GO = GameObject.Instantiate(Prefab_areaAttack,
                    targetPos, Quaternion.identity);
                BossDoubleAreaAttack areaAttack = attackArea_GO.GetComponent<BossDoubleAreaAttack>();
                areaAttack.Init(Damage, owner.Attack.RangeTypeThreshold, IsClose(owner));
                owner.Attack.CurrentProjectile = areaAttack;

                // Destroy Previous spikes
                SpikeManager.Instance.DestroyAllSpike();
                SpikeManager.Instance.DestroyAllTrash();

                // Spawn New spikes
                SpawnMultipleSpikeInAreaAndStore(owner, Prefab_Spike, MaxArea,
                SpikeGap, SpikeProbability, SpikeRandomOffset,
                owner.Attack.CurrentSpikeSpawners);

                break;
            case 1:
                // Trigger Attack Area
                owner.Attack.CurrentProjectile.Trigger();

                // Trigger EnableSpikes
                foreach (SpikeSpawner spike in owner.Attack.CurrentSpikeSpawners)
                {
                    spike.Trigger();
                }
                owner.Attack.CurrentSpikeSpawners.Clear();

                // Spawn New spikes
                SpawnMultipleSpikeInAreaAndStore(owner, Prefab_Spike, MaxArea,
                SpikeGap, SpikeProbability, SpikeRandomOffset,
                owner.Attack.CurrentSpikeSpawners);
                break;
            case 2:
                // Trigger Attack Area
                owner.Attack.CurrentProjectile.Trigger();

                // Trigger EnableSpikes
                foreach (SpikeSpawner spike in owner.Attack.CurrentSpikeSpawners)
                {
                    spike.Trigger();
                }

                // Clear SpikeSpawners
                owner.Attack.CurrentProjectile = null;
                owner.Attack.CurrentSpikeSpawners.Clear();
                break;
        }

    }

    public override void StartAttackMove(EnemyBase owner, int type)
    {
        // Start VFX
        owner.StartVFX("Boss_Teleport");

        switch (type)
        {
            case 0:
                // Launch up
                owner.Move.Launch(Vector3.up * 100);
                break;
            case 1:
                // Set Position To Center
                owner.transform.position = owner.SpawnedPosition + Vector3.up * 15;

                // Launch down
                owner.Move.Launch(-Vector3.up * 100);
                break;
        }
    }

    private void SpawnMultipleSpikeInAreaAndStore(EnemyBase owner, GameObject spike, float areaRadius, float gap,
        float probability, float spikeRandomOffset, List<SpikeSpawner> spikeContainer)
    {
        Vector3 centerOffset = new Vector3(areaRadius * .5f, 0f, areaRadius * .5f);
        for (int i = 0; i < areaRadius / gap; i++)
            for (int k = 0; k < areaRadius / gap; k++)
            {
                if (Random.value < probability)
                {
                    Vector3 offset = new Vector3(i * gap, 0f, k * gap) - centerOffset;
                    Vector3 pos = owner.transform.position + offset;
                    if (Vector3.Distance(pos, owner.transform.position) < areaRadius * .5f)
                    {
                        SpikeSpawner curSpike = SpawnSpike(owner, spike, pos, spikeRandomOffset);
                        spikeContainer.Add(curSpike);
                        SpikeManager.Instance.Spikes.Add(curSpike);
                    }
                }
            }
    }

    private AttackRangeType IsClose(EnemyBase owner)
    {
        return owner.Attack.GetAttackRangeType();
    }

    private SpikeSpawner SpawnSpike(EnemyBase owner, GameObject spike, Vector3 position, float spikeRandomOffset)
    {
        Vector3 targetPos = position;
        targetPos.y = 0f;
        targetPos += new Vector3(Random.value * spikeRandomOffset, 0f, Random.value * spikeRandomOffset);
        GameObject projectileObject = ObjectPoolManager.Instance.DequeueObject(spike);
        projectileObject.transform.position = targetPos;
        projectileObject.transform.rotation = Quaternion.identity;


        SpikeSpawner spikeInst = projectileObject.GetComponent<SpikeSpawner>();
        spikeInst.Init(false);

        return spikeInst;
    }
}