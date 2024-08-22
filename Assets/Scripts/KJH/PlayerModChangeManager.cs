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
        set
        {
            _PlayerMaster.IsAttackState = value;
        }
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
                    EnterRangeMode();
                }
                break;

            case nameof(InputManager.Instance.IsLMouseBtnClick):                
                if (InputManager.Instance.IsLControlBtnClick == true && IsAbsorptState == true && IsAttackState == false && isDashing == false)
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

    //��� ��ȯ�� ĳ���� ���� ����� �ٲٱ� ����
    public Action<bool> OnSucceseAbsorpt;

    public void EnterAbsorptState()
    {
        IsAbsorptState = true;
        OnEnterAbsorptState.Invoke();
        Debug.Log($"��� ��� ����");

    }
    public void EnterRangeMode()
    {
        IsAbsorptState = false;

        int value = OnSucceseAbsorptState.Invoke();

        _PlayerMaster._PlayerInstanteState.AcquireBullets(value);
        Debug.Log($"{value}�� ���");
        if (value <= 0)
        {
            EndAbsorptState();
        }

        if (HasBlueChip5_AutoChange() == false)
        {
            IsMeleeMode = false;
        }
    }
    public void EnterMeleeMode()
    {
        IsAbsorptState = false;
        if (HasBlueChip5_AutoChange() == true)
        {
            int value = OnSucceseAbsorptState.Invoke();

            _PlayerMaster._PlayerInstanteState.AcquireBullets(value);
            Debug.Log($"{value}�� ���");
            if (value <= 0)
            {
                EndAbsorptState();
            }
        }
        else
        {
            int value = OnSucceseAbsorptState_EntryMelee.Invoke();

            if (value > 0)
            {
                Debug.Log($"{value}�� ���, ���� ��� ��ȯ");
                _PlayerMaster._PlayerInstanteState.AcquireBullets_Melee(value);
                IsMeleeMode = true;                
            }
            else
            {
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
        Debug.Log($"��� ����");
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
                Debug.Log("�ڵ� ��� ��ȯ");
                autoChangeDelayTime = 0;
                IsMeleeMode = !IsMeleeMode;
            }
        }
    }
}

