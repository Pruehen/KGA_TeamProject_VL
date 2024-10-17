using System;
using UnityEngine;

public class DamageBox : MonoBehaviour
{
    [SerializeField] private ITargetable _owner;
    [SerializeField] private LayerMask _targetLayer;
    private Vector3 _offset;
    public Vector3 Defaultoffset = new Vector3(0f, 0.5f, 0.5f);
    [SerializeField] public Vector3 DefaultRange = new Vector3(1f, 1f, 1f);
    private Coroutine _DisableBoxCoroutine;


    private float _enableTimer = 0f;

    public Action<int> OnHitCallback;
    public Vector3 target;
    [SerializeField] PlayerSkill playerSkill;
    [SerializeField] private SO_Skill skillData;
    private Vector3 _halfSize;

    [SerializeField]
    private Vector3 HalfExtend
    {
        get
        {
            return _halfSize != Vector3.zero ? Vector3.Scale(_halfSize,transform.lossyScale) : Vector3.Scale(GetSkillRange(playerSkill), transform.lossyScale);
        }
        set
        {
            _halfSize = value;
        }
    }
    private Vector3 Offset
    {
        get
        {
            return _offset != Vector3.zero ? _offset : GetSkillOffset(playerSkill);
        }
        set
        {
            _offset = value;
        }
    }

    public enum PlayerSkill
    {
        MeleeAttack,
        MeleeAttackCharged,
        RangeSkillAttack1,
        RangeSkillAttack2,
        RangeSkillAttack3,
        RangeSkillAttack4,
        MeleeSkillAttack1,
        MeleeSkillAttack2,
        MeleeSkillAttack3_1,
        MeleeSkillAttack3_2,
        MeleeSkillAttack3_3,
        MeleeSkillAttack4
    }

    private Vector3 GetSkillRange(PlayerSkill skill)
    {
        if (skillData == null)
        {
            return DefaultRange; // �⺻�� ��ȯ
        }

        switch (skill)
        {
            case PlayerSkill.MeleeAttack:
                return skillData.MeleedefaultAttackRange;
            case PlayerSkill.MeleeAttackCharged:
                return skillData.MeleeChargedAttackRange;
            case PlayerSkill.RangeSkillAttack1:
                return skillData._rangedSkill1Range;
            case PlayerSkill.RangeSkillAttack2:
                return skillData._rangedSkill2Range;
            case PlayerSkill.RangeSkillAttack3:
                return skillData._rangedSkill3Range;
            case PlayerSkill.RangeSkillAttack4:
                return skillData._rangedSkill4Range;
            case PlayerSkill.MeleeSkillAttack1:
                return skillData._meleeSkill1Range;
            case PlayerSkill.MeleeSkillAttack2:
                return skillData._meleeSkill2Range;
            case PlayerSkill.MeleeSkillAttack3_1:
            case PlayerSkill.MeleeSkillAttack3_2:
            case PlayerSkill.MeleeSkillAttack3_3:
                return skillData._meleeSkill3Range;
            case PlayerSkill.MeleeSkillAttack4:
                return skillData._meleeSkill4Range;
            default:
                return DefaultRange; // �⺻ ũ�� ��ȯ
        }
    }

    private Vector3 GetSkillOffset(PlayerSkill skill)
    {
        if (skillData == null)
        {
            return Defaultoffset; // �⺻ ������ ��ȯ
        }
        switch (skill)
        {
            case PlayerSkill.MeleeAttack:
                return skillData.MeleedefaultAttackOffset;
            case PlayerSkill.MeleeAttackCharged:
                return skillData.MeleeChargedAttackOffset;
            case PlayerSkill.RangeSkillAttack1:
                return skillData._rangedSkill1OffSet;
            case PlayerSkill.RangeSkillAttack2:
                return skillData._rangedSkill2OffSet;
            case PlayerSkill.RangeSkillAttack3:
                return skillData._rangedSkill3OffSet;
            case PlayerSkill.RangeSkillAttack4:
                return skillData._rangedSkill4OffSet;
            case PlayerSkill.MeleeSkillAttack1:
                return skillData._meleeSkill1OffSet;
            case PlayerSkill.MeleeSkillAttack2:
                return skillData._meleeSkill2OffSet;
            case PlayerSkill.MeleeSkillAttack3_1:
            case PlayerSkill.MeleeSkillAttack3_2:
            case PlayerSkill.MeleeSkillAttack3_3:
                return skillData._meleeSkill3OffSet;
            case PlayerSkill.MeleeSkillAttack4:
                return skillData._meleeSkill4OffSet;
            default:
                return Defaultoffset; // �⺻ ������ ��ȯ
        }
    }

    private float _damage;

    private void Awake()
    {
        _owner = transform.parent.GetComponent<ITargetable>();
    }

    private void Start()
    {
        enabled = false;
    }

    private Vector3 Center
    {
        get
        {
            return (transform.position + Vector3.Scale(transform.rotation * Offset, transform.lossyScale));
        }
    }

    private void OnEnable()
    {
        Collider[] result = Physics.OverlapBox(Center, HalfExtend, transform.rotation, _targetLayer);
        bool onHit = false;
        int hitCount = 0;
        foreach (Collider hit in result)
        {
            if (hit.attachedRigidbody == null)
            {
                continue;
            }
            ITargetable combat = hit.attachedRigidbody.GetComponent<ITargetable>();
            if (combat == null)
            {
                continue;
            }
            ITargetable hitTarget = null;
            if (hit.TryGetComponent(out hitTarget))
            {
                if (_owner == hitTarget)
                {
                    continue;
                }
            }
            combat.Hit(_damage);
            onHit = true;
            hitCount++;
        }
        if (onHit)
        {
            OnHitCallback?.Invoke(hitCount);
        }
    }

    private void Update()
    {
        _enableTimer -= Time.deltaTime;
        if (_enableTimer <= 0f)
        {
            enabled = false;
        }
    }

    private void OnDrawGizmos()
    {
        if (!enabled)
        {
            return;
        }
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(Center, HalfExtend);
    }
    public void EnableOnly(float damage)
    {
        _damage = damage;
        enabled = true;
        _enableTimer = 0f;
    }
    public void EnableDamageBox(float damage, Vector3? range = null, Action<int> onHitCallBack = null, float time = 0f, Vector3? offset = null)
    {
        OnHitCallback = onHitCallBack;
        _damage = damage;

        // range�� null�� �ƴϸ� �����ϰ�, null�̸� �⺻ ũ�� ����
        SetRange(range ?? GetSkillRange(playerSkill));

        // offset�� null�� �ƴϸ� �����ϰ�, null�̸� �⺻���� ����
        SetOffset(offset ?? GetSkillOffset(playerSkill));

        enabled = true;
        _enableTimer = time;
    }

    public void EnableSkillDamageBox(float damage, Vector3? range = null, Action<int> onHitCallBack = null, float time = 0f, Vector3? offset = null)
    {
        OnHitCallback = onHitCallBack;
        _damage = damage;

        // range�� null�� �ƴϸ� �����ϰ�, null�̸� �⺻ ũ�� ����
        SetRange(range ?? GetSkillRange(playerSkill));

        // offset�� null�� �ƴϸ� �����ϰ�, null�̸� �⺻���� ����
        SetOffset(offset ?? GetSkillOffset(playerSkill));

        enabled = true;
        _enableTimer = time;
    }

    public void SetRange(Vector3 range)
    {
        HalfExtend = range;
    }

    public void SetOffset(Vector3 offset)
    {
        Offset = offset;
    }

    private void OnDestroy()
    {
        OnHitCallback = null;
    }
}