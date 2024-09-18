using System;
using UnityEngine;

[Serializable]
public class PlayerMeleeAttack : PlayerAttackModule
{
    public PlayerMaster _PlayerMaster;
    InputManager _InputManager;
    Animator _animator;

    bool _isCharging;
    bool _isCharged;

    private float _chargeTime = 1.5f;
    private float _currentChargeTime = 0f;
    public bool IsCharging { get => _isCharging; set => _isCharging = value; }
    public float ChargeTime { get => _chargeTime; set => _chargeTime = value; }
    public bool IsDashAttack { get; internal set; }
    public bool IsCharged { get; internal set; }

    [SerializeField] Skill skill;
    public override void DoUpdate()
    {
        if (_isCharging)
        {
            _currentChargeTime += Time.deltaTime;

            if (!_isCharged && _currentChargeTime >= _chargeTime)
            {
                _isCharged = true;
                ChargeVFX();
                _animator.SetBool("IsCharged", true);
                _currentChargeTime = 0f;
            }
        }
    }

    public void Init(Transform transform)
    {
        transform.TryGetComponent(out _PlayerMaster);
        transform.TryGetComponent(out _animator);
    }


    public override void StartAttack()
    {
        _animator.SetTrigger("Attack");

        _animator.SetFloat("AttackSpeed", _PlayerMaster._PlayerInstanteState.AttackSpeed());
    }
    public override void ReleaseAttack()
    {
    }
    public override void ResetAttack()
    {
        _isCharging = false;
        _isCharged = false;
        _animator.SetBool("IsCharged", false);
        _currentChargeTime = 0f;
        ChargeEndVFX();
    }
    public void ChargeStart(bool isLeftHand)//애니메이션 이벤트 MeleeAttack
    {
        _currentChargeTime = 0f;
        _isCharged = false;
        _isCharging = true;
        ChargeStartVFX(isLeftHand);
    }
    public void ChargeEnd()
    {
        ResetAttack();
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
    public void ChargeStartVFX(bool isLeftHand)
    {
        if (isLeftHand)
        {
            skill.Effect3(StartChargeL);
        }
        else
        {
            skill.Effect3(StartChargeR);
        }
    }
    public void ChargeEndVFX()
    {
        ObjectPoolManager.Instance.AllDestroyObject(StartChargeR.preFab);
    }
}
