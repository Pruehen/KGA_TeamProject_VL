using UnityEngine;

public class AttackSystem : MonoBehaviour
{
    PlayerMaster _PlayerMaster;

    Animator _animator;
    PlayerAttack _playerAttack;
    int hashAttackType = Animator.StringToHash("AttackType");
    int hashAttack = Animator.StringToHash("Attack");
    int hashAttackComboInitialIndex = Animator.StringToHash("AttackComboInitialIndex");
    int hasAttackSpeed = Animator.StringToHash("AttackSpeed");
    int hashSkill = Animator.StringToHash("Skill");
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
        TryGetComponent(out _playerAttack);
        TryGetComponent(out _closeAttack);
        TryGetComponent(out _closeSkill);
        _closeAttack.Init(_animator);
        _closeSkill.Init(_animator);
        _PlayerMaster = GetComponent<PlayerMaster>();

    }



    bool _attackLcokMove;

    public bool AttackLockMove
    {
        get => _attackLcokMove;
    }
    public void StartAttack(int index, int comboIndex)
    {
        _attackLcokMove = true;
        _animator.SetTrigger(hashAttack);
        _animator.SetInteger(hashAttackType, index);
        _animator.SetInteger(hashAttackComboInitialIndex, comboIndex);
        _animator.SetFloat(hasAttackSpeed, _PlayerMaster._PlayerInstanteState.AttackSpeed);
    }
    public void StartSkill(int index, float skillGauge)
    {
        if (skillGauge >= 100)
        {
            _attackLcokMove = true;
            _animator.SetTrigger(hashSkill);
            _animator.SetInteger(hashAttackType, index);
            _animator.SetFloat("SkillGauge", skillGauge);
            Debug.Log(skillGauge);
        }
        else
        {
            Debug.Log(skillGauge);
        }
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
        _playerAttack.IncreaseAttackCount();
    }

    public void ResetEndAttack()
    {
        _animator.ResetTrigger("AttackEnd");
    }
    public void ResetMod()
    {
        _animator.SetTrigger("Reset");
        
    }
    public void Absober()
    {
        Debug.Log("흡수");
        _animator.SetTrigger("Absorbeing");
    }
    public void AbsoberEnd()
    {
        _animator.SetTrigger("AbsorbeingEnd");
        _animator.ResetTrigger("Attack");
        _animator.ResetTrigger("AttackEnd");
        Debug.Log("absoberEnd");
    }
    public void ModTransform()
    {
        _animator.SetTrigger("Transform");
        _animator.SetTrigger("AbsorbeingEnd");
        _animator.ResetTrigger("Attack");
        _animator.ResetTrigger("AttackEnd");
        Debug.Log("Transform");
    }
}
