using System;
using System.ComponentModel;
using UnityEngine;

[Serializable]
public class PlayerModChangeManager
{
    Transform transform;
    PlayerMaster _PlayerMaster;
    PlayerAttackSystem _AttackSystem;
    Animator _animator;

    public bool IsAbsorbing;
    public bool IsMeleeMode;

    public Action OnEnterAbsorb;
    public Action OnActiveAbsorb;
    public Func<int> OnSucceseRange;
    public Func<int> OnSucceseMelee;
    public Action OnEndAbsorptState;
    public Action<bool> OnModChanged;
    public Action<bool> OnModChangedVfx;
    public Action OnResetMod;

    float autoChangeDelayTime = 0;


    [SerializeField] GameObject[] Glove;

    public void Init(Transform transform)
    {
        this.transform = transform;
        _PlayerMaster = transform.GetComponent<PlayerMaster>();
        _AttackSystem = transform.GetComponent<PlayerAttackSystem>();
        _animator = transform.GetComponent<Animator>();
        InputManager.Instance.PropertyChanged += OnInputPropertyChanged;
    }
    void OnInputPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(InputManager.Instance.IsLControlBtnClick):
                if (HasBlueChip5_AutoChange())
                    break;
                if (InputManager.Instance.IsLControlBtnClick == false && IsAbsorbing == true && _PlayerMaster.IsDashing == false)
                {
                    EnterRangeMode();
                    break;
                }
                if (AnimatorHelper.IsTagedAnimPlaying(_animator, 0, "Absorb"))
                    break;
                if (AnimatorHelper.IsTagedAnimPlaying(_animator, 0, "Move") || AnimatorHelper.IsTagedAnimPlaying(_animator, 0, "Idle"))
                {
                    if (InputManager.Instance.IsLControlBtnClick == true && IsAbsorbing == false && _PlayerMaster.IsDashing == false)
                    {
                        StartAbsorbing();
                    }
                }
                break;

            case nameof(InputManager.Instance.IsLMouseBtnClick):
                if (InputManager.Instance.IsLControlBtnClick == true && IsAbsorbing == true && _PlayerMaster.IsDashing == false)
                {
                    EnterMeleeMode();
                }
                break;
        }
    }

    public void StartAbsorbing()
    {
        IsAbsorbing = true;
        OnEnterAbsorb?.Invoke();
    }

    public void ActiveAbsorb()
    {
        OnActiveAbsorb?.Invoke();
    }

    public void TryAbsorptFail()
    {
        IsAbsorbing = false;
        OnEndAbsorptState?.Invoke();
    }
    public void EnterRangeMode()
    {
        PlayerInstanteState state = _PlayerMaster._PlayerInstanteState;

        IsAbsorbing = false;
        int value = OnSucceseRange.Invoke();

        state.BulletClear_Melee();
        state.AcquireBullets(state.meleeBullets * state.MeleeToRangeRatio);

        state.AcquireBullets(value);
        if (value > 0)
        {
            _PlayerMaster._PlayerSkill.Effect2(_AttackSystem.endAbsorbing);
        }
        IsMeleeMode = false;
        ActiveGlove(IsMeleeMode);
        OnModChanged?.Invoke(IsMeleeMode);
        OnModChangedVfx?.Invoke(IsMeleeMode);

        OnEndAbsorptState?.Invoke();
    }
    public void EnterMeleeMode()
    {
        PlayerInstanteState state = _PlayerMaster._PlayerInstanteState;

        IsAbsorbing = false;

        int value = OnSucceseMelee.Invoke();

        if (value > 1)
        {
            state.AcquireBullets_Melee(value);
            IsMeleeMode = true;
        }
        else
        {
            state.AcquireBullets(value);
            IsMeleeMode = false;
        }
        ActiveGlove(IsMeleeMode);
        OnModChanged?.Invoke(IsMeleeMode);
        OnModChangedVfx?.Invoke(IsMeleeMode);
        OnEndAbsorptState?.Invoke();
    }
    public void ChangeModOnly(bool isMelee, bool isInit = false)
    {
        PlayerInstanteState state = _PlayerMaster._PlayerInstanteState;

        IsAbsorbing = false;
        IsMeleeMode = isMelee;
        OnModChanged?.Invoke(IsMeleeMode);
        
        if(!isInit)
            OnModChangedVfx?.Invoke(IsMeleeMode);
        
        ResetMod();
    }

    public void DoUpdate()
    {
        if (HasBlueChip5_AutoChange())
        {
            autoChangeDelayTime += Time.deltaTime;
            float autoChangeDelay = JsonDataManager.GetBlueChipData(EnumTypes.BlueChipID.Hybrid2).Level_VelueList[1][0];

            if (autoChangeDelay < autoChangeDelayTime)
            {
                autoChangeDelayTime = 0;
                ChangeModOnly(!IsMeleeMode);
            }
        }
    }
    public bool HasBlueChip5_AutoChange()
    {
        return _PlayerMaster.GetBlueChipLevel(EnumTypes.BlueChipID.Hybrid2) > 0;
    }

    public void ResetMod()
    {
        IsAbsorbing = false;
        OnResetMod?.Invoke();
    }


    public void ActiveGlove(bool condition)
    {
        if (condition)
        {
            foreach (GameObject gloves in Glove)
            {
                gloves.SetActive(true);
            }
        }
        else
        {
            foreach (GameObject gloves in Glove)
            {
                gloves.SetActive(false);
            }
        }
    }
}

