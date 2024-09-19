using EnumTypes;
using System;
using UnityEngine;

[Serializable]
public class PlayerRangeAttack : PlayerAttackModule
{
    [SerializeField] GameObject Prefab_Projectile;

    [SerializeField] Transform projectile_InitPos;
    [SerializeField] float projectileSpeed_Forward = 15;
    [SerializeField] float projectileSpeed_Up = 3;

    Animator _animator;

    int _currentAttackCount;

    Transform transform;

    PlayerMaster _PlayerMaster;

    int initialAttackComboIndex;
    [Header("total count of attack animation")]
    [SerializeField] int _totalAttackAnimCount = 4;

    [SerializeField] PlayerAttackKind _currentAttackMod = PlayerAttackKind.RangeNormalAttack;
    [SerializeField]
    PlayerAttackKind CurrentAttackKind
    {
        get
        {
            if (IsDashAttack && !IsLastAttack)
            {
                return (_currentAttackMod == PlayerAttackKind.RangeNormalAttack) ? PlayerAttackKind.RangeDashAttack : PlayerAttackKind.MeleeDashAttack;
            }
            return _currentAttackKind;
        }
        set
        {
            _currentAttackKind = value;
        }
    }
    [SerializeField] PlayerAttackKind _currentAttackKind = PlayerAttackKind.RangeNormalAttack;


    public void Init(Transform transform)
    {
        this.transform = transform;
        _animator = transform.GetComponent<Animator>();
        _PlayerMaster = transform.GetComponent<PlayerMaster>();
    }

    public bool IsDashAttack
    {
        get
        {
            bool ran = AnimatorHelper.IsOnlyAnimationPlaying(_animator, 0, "Base Layer.Attack.Dash Melee");
            bool mel = AnimatorHelper.IsOnlyAnimationPlaying(_animator, 0, "Base Layer.Attack.Dash Range");
            return ran || mel;
        }
    }
    public bool IsLastAttack
    {
        get
        {
            if (_currentAttackCount == 0)
                return false;
            if ((_currentAttackCount) % (_totalAttackAnimCount - initialAttackComboIndex) == 0)
            {
                Debug.Log("막타");
                return true;
            }
            return false;
        }
    }

    public bool IsNormalAttack { get; internal set; }

    public override void StartAttack()
    {
        int blueChip2Level = _PlayerMaster.GetBlueChipLevel(EnumTypes.BlueChipID.Range1);
        initialAttackComboIndex = (blueChip2Level > 0) ? (int)JsonDataManager.GetBlueChipData(EnumTypes.BlueChipID.Range1).Level_VelueList[blueChip2Level][0] : 0;

        _animator.SetTrigger("Attack");

        _animator.SetInteger("AttackComboInitialIndex", initialAttackComboIndex);
        _animator.SetFloat("AttackSpeed", _PlayerMaster._PlayerInstanteState.AttackSpeed());
    }
    public override void ReleaseAttack()
    {
    }

    public override void ResetAttack()
    {
        _currentAttackCount = 0;
    }

    private void OnRangeHit(bool isDashAttack, bool isLastAttack)
    {
        PlayerInstanteState stat = _PlayerMaster._PlayerInstanteState;
        stat.SkillGaugeRecovery(isDashAttack,isLastAttack);
    }

    public void ShootProjectile()
    {
        if (_animator.IsInTransition(0))
        {
            return;
        }

        _currentAttackCount++;
        Projectile projectile = ObjectPoolManager.Instance.DequeueObject(Prefab_Projectile).GetComponent<Projectile>();

        Vector3 projectionVector = transform.forward * projectileSpeed_Forward + Vector3.up * projectileSpeed_Up;

        projectile.Init(_PlayerMaster._PlayerInstanteState.GetDmg(this,
            IsLastAttack),
            projectile_InitPos.position, projectionVector,
            IsDashAttack, IsLastAttack, _currentAttackCount,
            OnRangeHit);

        _PlayerMaster._PlayerInstanteState.BulletConsumption();

        int level_blueChip_Range2 = _PlayerMaster.GetBlueChipLevel(BlueChipID.Range2);
        if (IsLastAttack && level_blueChip_Range2 > 0)//"원거리 마지막 공격 시, {0}%의 확률로 {1}% 위력의 무작위 스킬 발동",
        {
            BlueChip chip_Range2 = JsonDataManager.GetBlueChipData(BlueChipID.Range2);

            float skillActivationProbability = chip_Range2.Level_VelueList[level_blueChip_Range2][0] * 0.01f;
            float skillActivationProbabilityValue = UnityEngine.Random.Range(0f, 1f);

            if (skillActivationProbabilityValue < skillActivationProbability)
            {
                //PlayerSkill randomSkill1 = (PlayerSkill)chip_Range2.Level_VelueList[level_blueChip_Range2][2];
                //PlayerSkill randomSkill2 = (PlayerSkill)chip_Range2.Level_VelueList[level_blueChip_Range2][3];
                PlayerSkill randomSkill1 = PlayerSkill.RangeSkillAttack1;
                PlayerSkill randomSkill2 = PlayerSkill.RangeSkillAttack2;
                PlayerSkill randomSkill3 = PlayerSkill.RangeSkillAttack3;
                PlayerSkill randomSkill4 = PlayerSkill.RangeSkillAttack4;
                float skillPower = _PlayerMaster._PlayerSkill.SkillPower * chip_Range2.Level_VelueList[level_blueChip_Range2][1] / 100f;
                float skillSelectionValue = UnityEngine.Random.Range(((int)chip_Range2.Level_VelueList[level_blueChip_Range2][2]), (int)chip_Range2.Level_VelueList[level_blueChip_Range2][3] + 1);
                if (skillSelectionValue == 0)
                {
                    _PlayerMaster._PlayerSkill.InvokeSkillDamage(randomSkill1, chip_Range2.Level_VelueList[level_blueChip_Range2][1] / 100f);
                    _PlayerMaster._PlayerSkill.Effect2(_PlayerMaster._PlayerSkill.RangeSkill1);
                }
                else if (skillSelectionValue == 1)
                {
                    _PlayerMaster._PlayerSkill.InvokeSkillDamage(randomSkill2, chip_Range2.Level_VelueList[level_blueChip_Range2][1] / 100f);
                    _PlayerMaster._PlayerSkill.Effect2(_PlayerMaster._PlayerSkill.RangeSkill2);
                }
                else if (skillSelectionValue == 2)
                {
                    _PlayerMaster._PlayerSkill.StartRangeSkill3(randomSkill3, chip_Range2.Level_VelueList[level_blueChip_Range2][1] / 100f);
                    //_PlayerMaster._PlayerSkill.Effect2(_PlayerMaster._PlayerSkill.RangeSkill3);
                }
                else if (skillSelectionValue == 3)
                {
                    _PlayerMaster._PlayerSkill.InvokeSkillDamage(randomSkill4, chip_Range2.Level_VelueList[level_blueChip_Range2][1] / 100f);
                    _PlayerMaster._PlayerSkill.Effect2(_PlayerMaster._PlayerSkill.RangeSkill4);
                }
            }
        }
    }
}
