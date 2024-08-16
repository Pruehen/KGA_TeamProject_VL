using UnityEngine;

public class Skill : MonoBehaviour
{

    InputManager _InputManager;
    Animator _animator;

    int _animTriggerSkill;
    int _animFloatSkillGauge;
    int _animTriggerSkillEnd;
    bool _isAttacking;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        Init(_animator);
    }
    public void Init(Animator animator)
    {
        _animator = animator;
        _animTriggerSkill = Animator.StringToHash("Skill");
        _animFloatSkillGauge = Animator.StringToHash("SkillGauge");
        _animTriggerSkillEnd = Animator.StringToHash("SkillEnd");
    }
    public void EndSkill()
    {
        _animator.SetTrigger(_animTriggerSkillEnd);
    }

    public void SkillEnd()
    {
        if (!_animator.GetBool(_animTriggerSkill))
        {
            _animator.SetTrigger(_animTriggerSkillEnd);
            Debug.Log("ATKEnd");
        }
    }
}
