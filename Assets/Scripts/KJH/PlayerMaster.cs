using EnumTypes;
using UnityEngine;

public class PlayerMaster : SceneSingleton<PlayerMaster>, ITargetable
{
    public PlayerInstanteState _PlayerInstanteState { get; private set; }
    public PlayerEquipBlueChip _PlayerEquipBlueChip { get; private set; }
    public PlayerBuff _PlayerBuff { get; private set; }
    public PlayerPassive _PlayerPassive { get; private set; }
    public Skill _PlayerSkill { get; private set; }
    public bool IsMelee => PlayerAttack.IsMelee;
    public bool IsAttacking => PlayerAttack.IsAttacking;
    public bool IsAbsorbing => Mod.IsAbsorbing;
    public bool IsDashing => _PlayerMove.IsDashing;

    Animator _PlayerAnimator;
    PlayerMove _PlayerMove;
    public PlayerAttackSystem PlayerAttack;
    public SO_Skill SkillData;
    public ItemAbsorber ItemAbsorber;
    [SerializeField] SO_SKillEvent hit;

    public PlayerModChangeManager Mod => PlayerAttack.PlayerMod;

    public int GetBlueChipLevel(BlueChipID iD)
    {
        return _PlayerEquipBlueChip.GetBlueChipLevel(iD);
    }


    public void OnMeleeHit()
    {

        Execute_BlueChip1_OnMeleeHit();

        if (GetBlueChipLevel(BlueChipID.Hybrid2) > 0)//자동 변신시
        {
            return;//얼리리턴
        }

        _PlayerInstanteState.BulletConsumption_Melee();

        if (_PlayerInstanteState.meleeBullets <= 0)
        {
            Mod.EnterRangeMode();
        }
    }
    void Execute_BlueChip1_OnMeleeHit()
    {
        int level = GetBlueChipLevel(BlueChipID.Melee2);

        if (level > 0)
        {
            _PlayerInstanteState.HealShield(JsonDataManager.GetBlueChipData(BlueChipID.Melee2).Level_VelueList[level][2]);
        }
    }
    void Execute_BlueChip1_OnModeChange(bool isMeleeMode)
    {
        if (isMeleeMode)
        {
            int level = GetBlueChipLevel(BlueChipID.Melee2);

            if (level > 0)
            {
                _PlayerInstanteState.ChangeShield(JsonDataManager.GetBlueChipData(BlueChipID.Melee2).Level_VelueList[level][0]);
                _PlayerInstanteState.SetMaxShield(JsonDataManager.GetBlueChipData(BlueChipID.Melee2).Level_VelueList[level][1]);
            }
        }
        else
        {
            _PlayerInstanteState.ChangeShield(0);
        }
    }
    void Execute_BlueChip4_OnModeChange(bool isMeleeMode)
    {
        int level = GetBlueChipLevel(BlueChipID.Hybrid1);
        if (level > 0)
        {
            int count = (int)JsonDataManager.GetBlueChipData(BlueChipID.Hybrid1).Level_VelueList[level][0];
            float value = JsonDataManager.GetBlueChipData(BlueChipID.Hybrid1).Level_VelueList[level][1] * 0.01f;

            _PlayerBuff.blueChip4_Buff_NextHitAddDmg.Clear();
            for (int i = 0; i < count; i++)
            {
                _PlayerBuff.blueChip4_Buff_NextHitAddDmg.Enqueue(value);
                Debug.Log("데미지 버프 추가");
            }
        }
    }

    private void Awake()
    {
        _PlayerInstanteState = GetComponent<PlayerInstanteState>();
        _PlayerEquipBlueChip = GetComponent<PlayerEquipBlueChip>();
        _PlayerBuff = GetComponent<PlayerBuff>();
        _PlayerPassive = GetComponent<PlayerPassive>();
        _PlayerSkill = GetComponent<Skill>();
        _PlayerAnimator = GetComponent<Animator>();

        UIManager.Instance.SetPlayerMaster(this);

        PlayerAttack = GetComponent<PlayerAttackSystem>();
        PlayerAttack.Init();

        _PlayerMove = GetComponent<PlayerMove>();

        _PlayerPassive.Init();
        _PlayerInstanteState.Init();

        UIManager.Instance.Init();

        ItemAbsorber.Init(_PlayerInstanteState._playerStatData);

        Mod.OnModChanged += Execute_BlueChip1_OnModeChange;
        Mod.OnModChanged += Execute_BlueChip4_OnModeChange;

        _PlayerEquipBlueChip.Init_OnSceneLoad();
        _PlayerInstanteState.Init_OnSceneLoad();

        UIManager.Instance.UpdateGoldInfoUI();
    }

    private void Start()
    {
        GameManager.Instance.OnPlayerSpawn();
    }

    public Vector3 GetPosition()
    {
        return this.transform.position;
    }


    public void Hit(float dmg, DamageType damageType = DamageType.Normal)
    {
        _PlayerInstanteState.Hit(dmg, out float finalDmg, damageType);
        Mod.TryAbsorptFail();

        if (finalDmg <= 0) return;
        DmgTextManager.Instance.OnDmged(finalDmg, this.transform.position);
        GameObject VFX = ObjectPoolManager.Instance.DequeueObject(hit.preFab);
        Vector3 finalPosition = this.transform.position + transform.TransformDirection(hit.offSet);
        VFX.transform.position = finalPosition;
        Mod.TryAbsorptFail();
    }

    public bool IsDead()
    {
        return _PlayerInstanteState.IsDead;
    }

    public void OnKnockback()
    {
        _PlayerAnimator.SetTrigger("Hit");
    }
}