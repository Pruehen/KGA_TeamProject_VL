using System;
using UnityEngine;
using EnumTypes;

public class PlayerMaster : SceneSingleton<PlayerMaster>, ITargetable
{
    public PlayerInstanteState _PlayerInstanteState { get; private set; }
    public PlayerEquipBlueChip _PlayerEquipBlueChip { get; private set; }
    public PlayerBuff _PlayerBuff { get; private set; }
    public PlayerPassive _PlayerPassive { get; private set; }
    PlayerMove _PlayerMove;
    PlayerAttack _PlayerAttack;
    PlayerModChangeManager _PlayerModChangeManager;    
    [SerializeField] ItemAbsorber _ItemAbsorber;    

    public bool IsAbsorptState
    {
        get { return _PlayerInstanteState.IsAbsorptState; }
        set
        {
            _PlayerInstanteState.IsAbsorptState = value;
        }
    }
    public bool IsMeleeMode
    {
        get { return _PlayerInstanteState.IsMeleeMode; }
        set
        {
            _PlayerInstanteState.IsMeleeMode = value;
            Execute_BlueChip1_OnModeChange(value);
            Execute_BlueChip4_OnModeChange();
        }
    }
    public int GetBlueChipLevel(BlueChipID iD)
    {
        return _PlayerEquipBlueChip.GetBlueChipLevel(iD);
    }


    public void OnMeleeHit()
    {
        _PlayerInstanteState.BulletConsumption_Melee();

        Execute_BlueChip1_OnMeleeHit();

        if(_PlayerInstanteState.meleeBullets <= 0)
        {
            _PlayerModChangeManager.EnterRangeMode();
        }
    }
    void Execute_BlueChip1_OnMeleeHit()
    {
        int level = GetBlueChipLevel(BlueChipID.Melee2);

        if(level > 0)
        {
            _PlayerInstanteState.ChangeShield(JsonDataManager.GetBlueChipData(BlueChipID.Melee2).Level_VelueList[level][2]);
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
            }
        }
        else
        {
            _PlayerInstanteState.ChangeShield(-9999);
        }
    }
    void Execute_BlueChip4_OnModeChange()
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

        _PlayerPassive.Init();
        _PlayerInstanteState.Init(_PlayerPassive);

        _PlayerMove = GetComponent<PlayerMove>();
        _PlayerAttack = GetComponent<PlayerAttack>();
        _PlayerModChangeManager = GetComponent<PlayerModChangeManager>();

        _ItemAbsorber.Init();
    }

    public void OnAttackState(Vector3 lookTarget)
    {
        _PlayerMove.OnAttackState(lookTarget);
    }

    public Vector3 GetPosition()
    {
        return this.transform.position;
    }

    public void Register_PlayerModChangeManager(Action callBack_StartAbsorb, Func<int> callBack_SucceseAbsorb, Func<int> callBack_AcquireAll, Action callBack_DropAbsorbingItems)
    {
        _PlayerModChangeManager.OnEnterAbsorptState += callBack_StartAbsorb;
        _PlayerModChangeManager.OnSucceseAbsorptState += callBack_AcquireAll;
        _PlayerModChangeManager.OnSucceseAbsorptState_EntryMelee += callBack_SucceseAbsorb;
        _PlayerModChangeManager.OnEndAbsorptState += callBack_DropAbsorbingItems;
    }


    public void Hit(float dmg)
    {
        _PlayerInstanteState.Hit(dmg);
        DmgTextManager.Instance.OnDmged(dmg, this.transform.position);
    }

    public bool IsDead()
    {
        return _PlayerInstanteState.IsDead;
    }
}