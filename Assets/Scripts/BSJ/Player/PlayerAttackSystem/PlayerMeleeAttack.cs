using System;
using UnityEngine;

[Serializable]
public class PlayerMeleeAttack : PlayerAttackModule
{
    public PlayerMaster _PlayerMaster;

    PlayerAttackSystem _PlayerAttackSystem;
    InputManager _InputManager;
    Animator _animator;

    bool _isCharging;
    bool _isLeftHand;
    bool _isCharged;
    bool _isChargeVfxOn;

    private float _chargeTime = 1.5f;
    private float _currentChargeTime = 0f;
    public bool IsCharging { get => _isCharging; set => _isCharging = value; }
    public float ChargeTime { get => _chargeTime; set => _chargeTime = value; }
    public bool IsDashAttack => AnimatorHelper.IsAnimationPlaying(_animator, 0, "Base Layer.Attack.Dash Melee");
    public bool IsCharged => _isCharged;

    [SerializeField] Skill skill;
    public override void DoUpdate()
    {
        if (_isCharging)
        {
            if (IsCharged)
            {
                return;
            }
            _currentChargeTime += Time.deltaTime;

            //Todo Set Ratio By AttackSpeed if needed
            if (!_isChargeVfxOn && _currentChargeTime > (0.2f))
            {
                _isChargeVfxOn = true;
                ChargeStartVFX(_isLeftHand);
                Debug.Log("IsVFXON");
            }
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
        transform.TryGetComponent(out _PlayerAttackSystem);
        _PlayerAttackSystem.PlayerMod.OnModChanged += ResetMelee;
    }
    public override void StartAttack()
    {
        _animator.SetTrigger("Attack");
        _animator.SetFloat("AttackSpeed", _PlayerMaster._PlayerInstanteState.AttackSpeed());
        _currentChargeTime = 0f;
    }
    public override void ReleaseAttack()
    {
    }
    public override void ResetAttack()
    {
        _currentChargeTime = 0f;
        _isCharging = false;
        _animator.SetBool("IsCharged", false);
        ChargeEndVFX();
    }
    public void ChargeStart(bool isLeftHand)//애니메이션 이벤트 MeleeAttack
    {
        _currentChargeTime = 0f;
        _isCharged = false;
        _isChargeVfxOn = false;
        _isCharging = true;
        _isLeftHand = isLeftHand;
    }
    private void ResetMelee(bool isMelee)
    {
        if (isMelee)
            ResetAttack();
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
