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
    Action OnChargeStart;
    [SerializeField] Skill skill;
    private void Update()
    {
        if (_isCharging)
        {
            _chargingTime += Time.deltaTime;

            if (!_isCharged && _chargingTime >= ChargeTime)
            {
                _isCharged = true;
                OnCharged?.Invoke();
                ChargeVFX();
                _chargingTime = 0f;
            }
        }
        else
        {
            _chargingTime = 0f;
            _isCharged = false;
        }
    }

    public void Init(Animator animator, Action onCharged = null, Action onChargeEnd = null, Action onChargeFail = null,Action onChargeStart = null)
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
        OnChargeStart = onChargeStart;
    }

    private void OnDestroy()
    {
        OnChargeFail = null;
        OnChargeEnd = null;
        OnCharged = null;
        OnChargeStart =null;
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

    public void ChargeStart()//애니메이션 이벤트 MeleeAttack
    {
        OnChargeStart?.Invoke();
        _chargingTime = 0f;
        _isCharged = false;
        _isCharging = true;
        Debug.Log("차-지 시작");
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
    public void ChargeVFX()
    {
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0); // 0은 레이어 인덱스

        if (stateInfo.IsName("Charge loop R"))
        {
            skill.Effect2(ChargeR);
        }
        else if (stateInfo.IsName("Charge loop L"))
        {
            skill.Effect2(ChargeL);
        }

    }
    [SerializeField] SO_SKillEvent ChargeR;
    [SerializeField] SO_SKillEvent ChargeL;
}
