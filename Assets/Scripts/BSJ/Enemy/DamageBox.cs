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

    public Action OnHitCallback;
    public Vector3 target;
    [SerializeField] PlayerSkill playerSkill;
    [SerializeField] private SO_Skill skillData;
    private Vector3 _halfSize;

    [SerializeField]
    private Vector3 HalfSize
    {
        get
        {
            return _halfSize != Vector3.zero ? _halfSize : GetSkillRange(playerSkill);
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
            return DefaultRange; // 기본값 반환
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
                return DefaultRange; // 기본 크기 반환
        }
    }

    private Vector3 GetSkillOffset(PlayerSkill skill)
    {
        if (skillData == null)
        {
            return Defaultoffset; // 기본 오프셋 반환
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
                return Defaultoffset; // 기본 오프셋 반환
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
        Collider[] result = Physics.OverlapBox(Center, HalfSize, transform.rotation, _targetLayer);
        bool onHit = false;
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
        }
        if (onHit)
        {
            OnHitCallback?.Invoke();
        }
        Debug.Log("HalfSize: " + HalfSize);
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
        Gizmos.DrawWireCube(Center, HalfSize);
    }

    public void EnableDamageBox(float damage, Vector3? range = null, Action onHitCallBack = null, float time = 0f, Vector3? offset = null)
    {
        OnHitCallback = onHitCallBack;
        _damage = damage;

        // range가 null이 아니면 설정하고, null이면 기본 크기 설정
        SetRange(range ?? GetSkillRange(playerSkill));

        // offset이 null이 아니면 설정하고, null이면 기본값을 설정
        SetOffset(offset ?? GetSkillOffset(playerSkill));

        enabled = true;
        _enableTimer = time;
    }

    public void EnableSkillDamageBox(float damage, Vector3? range = null, Action onHitCallBack = null, float time = 0f, Vector3? offset = null)
    {
        OnHitCallback = onHitCallBack;
        _damage = damage;

        // range가 null이 아니면 설정하고, null이면 기본 크기 설정
        SetRange(range ?? GetSkillRange(playerSkill));

        // offset이 null이 아니면 설정하고, null이면 기본값을 설정
        SetOffset(offset ?? GetSkillOffset(playerSkill));

        enabled = true;
        _enableTimer = time;
    }

    private void SetRange(Vector3 range)
    {
        HalfSize = range;
        Debug.Log("Range set to: " + HalfSize);
    }

    private void SetOffset(Vector3 offset)
    {
        Offset = offset;
        Debug.Log("Offset set to: " + Offset);
    }

    private void OnDestroy()
    {
        OnHitCallback = null;
    }
}