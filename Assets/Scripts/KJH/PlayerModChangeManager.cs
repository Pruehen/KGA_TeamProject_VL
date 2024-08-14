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
        }
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
                if (InputManager.Instance.IsLControlBtnClick == true && IsAbsorptState == false)
                {
                    EnterAbsorptState();
                }
                if (InputManager.Instance.IsLControlBtnClick == false && IsAbsorptState == true)
                {
                    IsAbsorptState = false;
                    EnterRangeMode();
                }
                break;

            case nameof(InputManager.Instance.IsLMouseBtnClick):                
                if (InputManager.Instance.IsLControlBtnClick == true && IsAbsorptState == true)
                {
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
        IsAbsorptState = false;
        IsMeleeMode = false;
        int value = OnSucceseAbsorptState.Invoke();

        _PlayerMaster._PlayerInstanteState.AcquireBullets(value);
        Debug.Log($"{value}개 흡수");
        if (value > 0)
        {
            OnSucceseAbsorpt?.Invoke(IsMeleeMode);
        }
        else
        {
            OnEndAbsorptState.Invoke();
            IsAbsorptState = false;
        }
    }
    public void EnterMeleeMode()
    {  
        int value = OnSucceseAbsorptState_EntryMelee.Invoke();

        if (value > 0)
        {
            Debug.Log($"{value}개 흡수, 근접 모드 변환");
            _PlayerMaster._PlayerInstanteState.AcquireBullets_Melee(value);
            IsMeleeMode = true;
            OnSucceseAbsorpt?.Invoke(IsMeleeMode);
        }
        else
        {
            EndAbsorptState();
        }
        IsAbsorptState = false;
    }

    public void EndAbsorptState()
    {
        IsAbsorptState = false;
        IsMeleeMode = false;
        Debug.Log($"흡수 실패");
        OnEndAbsorptState.Invoke();
    }
}

