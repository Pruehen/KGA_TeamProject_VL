using System;
using UnityEditor;
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

    private float _chargeTime = 1.5f;
    private float _currentChargeTime = 0f;
    public bool IsCharging { get => IsCharging; set => IsCharging = value; }
    public float ChargeTime { get => _chargeTime; set => _chargeTime = value; }

    Action OnCharged;
    Action OnChargeEnd;
    Action OnChargeFail;
    Action OnChargeStart;
    [SerializeField] Skill skill;
    private void Update()
    {
        if (_isCharging)
        {
            _currentChargeTime += Time.deltaTime;

            if (!_isCharged && _currentChargeTime >= _chargeTime)
            {
                _isCharged = true;
                OnCharged?.Invoke();
                ChargeVFX();
                _currentChargeTime = 0f;
            }
        }
        else
        {
            _currentChargeTime = 0f;
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
        _currentChargeTime = 0f;
        _isCharged = false;
        _isCharging = true;
        Debug.Log("차-지 시작");
        ChargeStartVFX();
    }
    
    public void ChargeEnd()
    {
        _isCharging = false;
        OnChargeEnd?.Invoke();
        ChargeEndVFX();
    }

    public void ChargeFail()
    {
        _isCharging = false;
        _currentChargeTime = 0f;
        OnChargeFail?.Invoke();
        ChargeEndVFX();
    }
    public void ChargeVFX()
    {
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0); // 0은 레이어 인덱스
        if (stateInfo.IsName("Charge loop R"))
        {
            skill.Effect2(ChargedR);
        }
        else if (stateInfo.IsName("Charge loop L"))
        {
            skill.Effect2(ChargedL);
        }
    }
    [SerializeField] SO_SKillEvent ChargedR;
    [SerializeField] SO_SKillEvent ChargedL;
    [SerializeField] SO_SKillEvent StartChargeR;
    [SerializeField] SO_SKillEvent StartChargeL;
    public GameObject CurrentChargedVFX;
    public void ChargeStartVFX()
    {
        skill.Effect2(StartChargeR);
    }
    public void ChargeEndVFX()
    {
        ObjectPoolManager.Instance.AllDestroyObject(StartChargeR.preFab);
    }

}
