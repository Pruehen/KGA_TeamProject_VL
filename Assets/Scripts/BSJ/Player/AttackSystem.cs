using UnityEngine;

public class AttackSystem : MonoBehaviour
{
    Animator _animator;
    int hashAttackType = Animator.StringToHash("AttackType");
    int hashAttack = Animator.StringToHash("Attack");
    int hashAttackComboInitialIndex = Animator.StringToHash("AttackComboInitialIndex");

    ATKTest _closeAttack;

    [SerializeField] DamageBox _damageBox;

    private void Awake()
    {
        Init();
    }
    public void Init()
    {
        TryGetComponent(out _animator);
        TryGetComponent(out _closeAttack);
        _closeAttack.Init(_animator);
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
        _damageBox.EnableDamageBox(30);
    }

    public void ResetEndAttack()
    {
        _animator.ResetTrigger("AttackEnd");
    }
}
