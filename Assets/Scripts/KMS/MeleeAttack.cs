using System;
using UnityEngine;

public class MeleeAttack: MonoBehaviour
{
    
    InputManager _InputManager;
    Animator _animator;

    int _animTriggerAttack;
    int _animBoolChargingEnd;
    int _animTriggerAttackEnd;
    int _animTriggerDashEnd;
    int _animTriggerDash;

    bool _isCharging;
    bool _isCharged;

    public float ChargeTime = 1.5f;
    public bool IsCharging { get => IsCharging; set => IsCharging = value; }
    float _chargingTime;
    public float ChargingTime { get => _chargingTime; set => _chargingTime = value; }

    Action OnCharged;
    Action OnChargeEnd;
    Action OnChargeFail;

    private void Update()
    {
        if (_isCharging)
        {
            _chargingTime += Time.deltaTime;
            if(_chargingTime >= ChargeTime && !_isCharged)
            {
                _isCharged = true;
                OnCharged?.Invoke();
            }
        }
        else
        {
            _chargingTime = 0f;
            _isCharged = false;
        }
    }

    public void Init(Animator animator, Action onCharged = null, Action onChargeEnd = null, Action onChargeFail = null)
    {
        _animator = animator;
        _animTriggerAttack = Animator.StringToHash("Attack");
        _animBoolChargingEnd = Animator.StringToHash("ChargingEnd");
        _animTriggerAttackEnd = Animator.StringToHash("AttackEnd");
        _animTriggerDashEnd = Animator.StringToHash("DashEnd");
        _animTriggerDash = Animator.StringToHash("Dash");

        OnCharged = onCharged;
        OnChargeEnd = onChargeEnd;
        OnChargeFail = onChargeFail;
    }

    private void OnDestroy()
    {
        OnChargeFail = null;
        OnChargeEnd = null;
        OnCharged = null;
    }
    public void EndAttack()
    {
        _animator.SetTrigger(_animTriggerAttackEnd);
        ChargeEnd();
        Debug.Log("MeleeAttackEnd");
    }

    public void ATKEnd()
    {
        if (!_animator.GetBool(_animTriggerAttack))
        {
            _animator.SetTrigger(_animTriggerAttackEnd);
            Debug.Log("ATKEnd");
        }
    }
    public void DashEnd()
    {
        _animator.SetTrigger(_animTriggerDashEnd);
        Debug.Log("DashEnd");
    }
    public void DashAtkEnd()
    {
        _animator.ResetTrigger(_animTriggerAttackEnd);
        _animator.ResetTrigger(_animTriggerDashEnd);
        _animator.ResetTrigger(_animTriggerDash);

    }

    public void ChargeStart()
    {
        _isCharging = true;
    }
    
    public void ChargeEnd()
    {
        _isCharging = false;
        OnChargeEnd?.Invoke();
    }

    public void ChargeFail()
    {
        _isCharging = false;
        _chargingTime = 0f;
        OnChargeFail?.Invoke();
    }
}
