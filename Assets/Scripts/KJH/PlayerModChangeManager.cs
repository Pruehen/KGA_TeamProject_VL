using System;
using System.ComponentModel;
using UnityEngine;

public class PlayerModChangeManager : MonoBehaviour
{
    PlayerMaster _PlayerMaster;

    public bool IsAbsorptState
    {
        get { return _PlayerMaster.IsAbsorptState; }
        set
        {            
            _PlayerMaster.IsAbsorptState = value;
        }
    }
    public bool IsMeleeMode
    {
        get { return _PlayerMaster.IsMeleeMode; }
        set
        {            
            _PlayerMaster.IsMeleeMode = value;
            OnSucceseAbsorpt?.Invoke(value);
        }
    }
    public bool IsAttackState
    {
        get { return _PlayerMaster.IsAttackState; }
    }
     public bool isDashing
    {
        get { return _PlayerMaster.isDashing; }
    }

    private void Awake()
    {
        _PlayerMaster = GetComponent<PlayerMaster>();
        InputManager.Instance.PropertyChanged += OnInputPropertyChanged;
    }
    void OnInputPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(InputManager.Instance.IsLControlBtnClick):                
                if (InputManager.Instance.IsLControlBtnClick == true && IsAbsorptState == false&&IsAttackState==false&& isDashing==false)
                {
                    EnterAbsorptState();
                }
                if (InputManager.Instance.IsLControlBtnClick == false && IsAbsorptState == true&& IsAttackState==false && isDashing == false)
                {

                    IsAbsorptState = false;
                    //EndAbsorptState();
                    EnterRangeMode();
                }
                break;

            case nameof(InputManager.Instance.IsLMouseBtnClick):                
                if (InputManager.Instance.IsLControlBtnClick == true && IsAbsorptState == true && IsAttackState == false && isDashing == false)
                {
                    if (IsAbsorptState)
                    {
                        //_PlayerMaster._PlayerSkill.Effect2(_PlayerMaster._AttackSystem.endAbsorbing);
                    }
                    EnterMeleeMode();
                }
                break;
        }
    }

    public Action OnEnterAbsorptState;
    public Func<int> OnSucceseAbsorptState;
    public Func<int> OnSucceseAbsorptState_EntryMelee;
    public Action OnEndAbsorptState;

    //모드 변환시 캐릭터 공격 방식을 바꾸기 위해
    public Action<bool> OnSucceseAbsorpt;

    public void EnterAbsorptState()
    {
        IsAbsorptState = true;
        OnEnterAbsorptState.Invoke();
        Debug.Log($"흡수 모드 진입");

    }
    public void EnterRangeMode()
    {
        PlayerInstanteState state = _PlayerMaster._PlayerInstanteState;
        state.AcquireBullets(state.meleeBullets * state.MeleeToRangeRatio);
        //
        _PlayerMaster._PlayerInstanteState.BulletClear_Melee();

        IsAbsorptState = false;

        int value = OnSucceseAbsorptState.Invoke();

        _PlayerMaster._PlayerInstanteState.AcquireBullets(value);
        Debug.Log($"{value}개 흡수");
        ObjectPoolManager.Instance.AllDestroyObject(_PlayerMaster._AttackSystem.startAbsorbing.preFab);
        if (value <= 0)
        {
            EndAbsorptState();

        }
        else
        {
            _PlayerMaster._PlayerSkill.Effect2(_PlayerMaster._AttackSystem.endAbsorbing);
            EndAbsorptState();
        }

        if (HasBlueChip5_AutoChange() == false)
        {
            IsMeleeMode = false;
        }
    }
    public void EnterMeleeMode()
    {
        ObjectPoolManager.Instance.AllDestroyObject(_PlayerMaster._AttackSystem.startAbsorbing.preFab);

        //_PlayerMaster._PlayerSkill.Effect2(_PlayerMaster._AttackSystem.endAbsorbing);
        PlayerInstanteState state = _PlayerMaster._PlayerInstanteState;

        IsAbsorptState = false;
        if (HasBlueChip5_AutoChange() == true)
        {
            int value = OnSucceseAbsorptState.Invoke();

            state.AcquireBullets(value);
            Debug.Log($"{value}개 흡수");
            if (value <= 0)
            {
                //_PlayerMaster._PlayerSkill.Effect2(_PlayerMaster._AttackSystem.endAbsorbing);
                EndAbsorptState();
            }
        }
        else
        {
            int value = OnSucceseAbsorptState_EntryMelee.Invoke();

            if (value > 1)
            {
                //_PlayerMaster._PlayerSkill.Effect2(_PlayerMaster._AttackSystem.endAbsorbing);
                Debug.Log($"{value}개 흡수, 근접 모드 변환");
                state.AcquireBullets_Melee(value);
                IsMeleeMode = true;                
            }
            else
            {
                state.AcquireBullets(value);
                EndAbsorptState();
            }
        }
    }

    public void EndAbsorptState()
    {

        IsAbsorptState = false;
        if (HasBlueChip5_AutoChange() == false)
        {
            IsMeleeMode = false;
        }

        OnEndAbsorptState.Invoke();
    }

    bool HasBlueChip5_AutoChange()
    {
        return _PlayerMaster.GetBlueChipLevel(EnumTypes.BlueChipID.Hybrid2) > 0;
    }

    float autoChangeDelayTime = 0;
    private void Update()
    {
        if(HasBlueChip5_AutoChange())
        {
            autoChangeDelayTime += Time.deltaTime;
            float autoChangeDelay = JsonDataManager.GetBlueChipData(EnumTypes.BlueChipID.Hybrid2).Level_VelueList[1][0];

            if(autoChangeDelay < autoChangeDelayTime)
            {
                Debug.Log("자동 모드 변환");
                autoChangeDelayTime = 0;
                IsMeleeMode = !IsMeleeMode;
            }
        }
    }
}

