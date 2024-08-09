using System;
using System.ComponentModel;
using UnityEngine;

public class PlayerModChangeManager : MonoBehaviour
{
    PlayerMaster _PlayerMaster;

    bool _isAbsorptState = false;
    bool _isMeleeMode = false;

    public bool IsAbsorptState
    {
        get { return _isAbsorptState; }
        set
        {
            _isAbsorptState = value;
            _PlayerMaster.IsAbsorptState = value;
        }
    }
    public bool IsMeleeMode
    {
        get { return _isMeleeMode; }
        set
        {
            _isMeleeMode = value;
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
    }
    public void EnterMeleeMode()
    {  
        int value = OnSucceseAbsorptState_EntryMelee.Invoke();

        if (value > 0)
        {
            Debug.Log($"{value}개 흡수, 근접 모드 변환");
            _PlayerMaster._PlayerInstanteState.AcquireBullets_Melee(value);
            IsMeleeMode = true;
        }
        IsAbsorptState = false;
    }

    public void EndAbsorptState()
    {
        IsAbsorptState = false;
        IsMeleeMode = false;
        Debug.Log($"흡수 실패");
    }
}

