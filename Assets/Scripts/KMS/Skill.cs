using UnityEngine;

public class Skill : MonoBehaviour
{

    InputManager _InputManager;
    Animator _animator;

    int _animTriggerSkill;
    int _animFloatSkillGauge;
    int _animTriggerSkillEnd;
    bool _isAttacking;
    [SerializeField] DamageBox _damageBox;
    [SerializeField] PlayerMaster _master;

    [SerializeField] public float _rangedSkill1 = 5f;
     [SerializeField] public float _rangedSkill2 = 7f;
     [SerializeField] public float _rangedSkill3 = 1f;
     [SerializeField] public float _rangedSkill4 = 10f;
     [SerializeField] public float _meleeSkill1 = 10f;
     [SerializeField] public float _meleeSkill2 = 10f;
     [SerializeField] public float _meleeSkill3_1 = 0.25f;
     [SerializeField] public float _meleeSkill3_2 = 0.25f;
     [SerializeField] public float _meleeSkill3_3 = 0.5f;
     [SerializeField] public float _meleeSkill3_4 = 10f;
     [SerializeField] public float _meleeSkill4 = 15f;

    [SerializeField] public float _rangedSkill1Range =1f;
    [SerializeField] public float _rangedSkill2Range =1f;
    [SerializeField] public float _rangedSkill3Range =1f;
    [SerializeField] public float _rangedSkill4Range =1f;
    [SerializeField] public float _meleeSkill1Range =1f;
    [SerializeField] public float _meleeSkill2Range =1f;
    [SerializeField] public float _meleeSkill3Range =1f;
    [SerializeField] public float _meleeSkill4Range =1f;

    [SerializeField] public float _rangedSkill1Distance = 1f;
    [SerializeField] public float _rangedSkill2Distance = 1f;
    [SerializeField] public float _rangedSkill3Distance = 1f;
    [SerializeField] public float _rangedSkill4Distance = 1f;
    [SerializeField] public float _meleeSkill4Distance = 20f;

    public float SkillPower
    {
        get { return _master._PlayerInstanteState.GetSkillDmg(); }
    }



    [SerializeField] SO_Skill so_Skill;
    private void Awake()
    {
        _master = GetComponent<PlayerMaster>();
        _animator = GetComponent<Animator>();
        Init(_animator);
        InitSkillData(so_Skill);
    }
    public void Init(Animator animator)
    {
        _animator = animator;
        _animTriggerSkill = Animator.StringToHash("Skill");
        _animFloatSkillGauge = Animator.StringToHash("SkillGauge");
        _animTriggerSkillEnd = Animator.StringToHash("SkillEnd");
    }
    public void InitSkillData(SO_Skill sO_Skill)
    {
        if(so_Skill!=null)
        {
            _rangedSkill1 = so_Skill._rangedSkill1;
            _rangedSkill2 = so_Skill._rangedSkill2;
            _rangedSkill3 = so_Skill._rangedSkill3;
            _rangedSkill4 = so_Skill._rangedSkill4;
            _meleeSkill1 = so_Skill._meleeSkill1;
            _meleeSkill2 = so_Skill._meleeSkill2;
            _meleeSkill3_1 = so_Skill._meleeSkill3_1;
            _meleeSkill3_2 = so_Skill._meleeSkill3_2;
            _meleeSkill3_3 = so_Skill._meleeSkill3_3;
            _meleeSkill3_4 = so_Skill._meleeSkill3_4;
            _meleeSkill4 = so_Skill._meleeSkill4;

            _rangedSkill1Range =so_Skill._rangedSkill1; 
            _rangedSkill2Range =so_Skill._rangedSkill2; 
            _rangedSkill3Range =so_Skill._rangedSkill3;
            _rangedSkill4Range = so_Skill._rangedSkill4;
            _meleeSkill1Range = so_Skill._meleeSkill1Range;
            _meleeSkill2Range = so_Skill._meleeSkill2Range;
            _meleeSkill3Range = so_Skill._meleeSkill3Range;
            _meleeSkill4Range = so_Skill._meleeSkill4Range;

            _rangedSkill1Distance = so_Skill._rangedSkill1Distance;
            _rangedSkill2Distance = so_Skill._rangedSkill2Distance;
            _rangedSkill3Distance = so_Skill._rangedSkill3Distance;
            _rangedSkill4Distance = so_Skill._rangedSkill4Distance;
            _meleeSkill4Distance =  so_Skill._meleeSkill4Distance;
        }
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
    public void SkillDamege()
    {
        //_damageBox.EnableDamageBox(float damage, float range = 1f, Action onHitCallBack = null, float time = 0f)
    }
    public void InvokeSkillDamage(string skillName)
    {
        Debug.Log(SkillPower);
        Debug.Log(skillName);
        switch (skillName)
        {
            case "Skill1":
                _damageBox.EnableDamageBox(so_Skill._rangedSkill1*SkillPower, so_Skill._rangedSkill1Range, null, 0f) ;
                break;

            case "Skill2":
                _damageBox.EnableDamageBox(so_Skill._rangedSkill2* SkillPower, so_Skill._rangedSkill2Range, null, 0f);
                break;

            case "Skill3":
                _damageBox.EnableDamageBox(so_Skill._rangedSkill3* SkillPower, so_Skill._rangedSkill3Range, null, 0f);
                break;

            case "Skill4":
                _damageBox.EnableDamageBox(so_Skill._rangedSkill4*SkillPower, so_Skill._rangedSkill4Range, null, 0f);
                break;

            // 필요에 따라 더 많은 스킬 추가 가능
            default:
                Debug.LogWarning($"Unrecognized skill: {skillName}");
                break;
        }
    }
}
