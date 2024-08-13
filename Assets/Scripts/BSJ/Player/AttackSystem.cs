using UnityEngine;

public class AttackSystem : MonoBehaviour
{
    PlayerMaster _PlayerMaster;

    Animator _animator;
    int hashAttackType = Animator.StringToHash("AttackType");
    int hashAttack = Animator.StringToHash("Attack");
    int hashAttackComboInitialIndex = Animator.StringToHash("AttackComboInitialIndex");
    MeleeAttack _closeAttack;
    Skill _closeSkill; 

    [SerializeField] DamageBox _damageBox;

    private void Awake()
    {
        Init();
    }
    public void Init()
    {
        TryGetComponent(out _animator);
        TryGetComponent(out _closeAttack);
        TryGetComponent(out _closeSkill);
        _closeAttack.Init(_animator);
        _PlayerMaster = GetComponent<PlayerMaster>();
    }



    bool _attackLcokMove;

    public bool AttackLockMove
    {
        get => _attackLcokMove;
    }
    public int AttackIndex
    {
        get => _animator.GetInteger(hashAttackType);
        set => _animator.SetInteger(hashAttackType, value);
    }

    public void StartAttack(int index, int comboIndex)
    {
        _attackLcokMove = true;
        _animator.SetTrigger(hashAttack);
        _animator.SetInteger(hashAttackType, index);
        _animator.SetInteger(hashAttackComboInitialIndex, comboIndex);
    }

    public void LockMove()
    {
        _attackLcokMove = true;
    }
    public void ReleaseLockMove()
    {
        _attackLcokMove = false;
    }

    public void OnRelease()
    {
        _closeAttack.EndAttack();
    }

    private void EnableDamageBox()
    {
        _damageBox.EnableDamageBox(30, _PlayerMaster.OnMeleeHit);
    }

    public void ResetEndAttack()
    {
        _animator.ResetTrigger("AttackEnd");
    }
}
