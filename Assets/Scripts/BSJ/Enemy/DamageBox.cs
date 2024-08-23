using EnumTypes;
using System;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditorInternal.ReorderableList;

public class DamageBox : MonoBehaviour
{
    [SerializeField] private ITargetable _owner;
    [SerializeField] private LayerMask _targetLayer;
    [SerializeField] private Vector3 _offset;
    private Vector3 Default;
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

    public enum PlayerSkill
    {
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
        switch (skill)
        {
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
                return Vector3.one; // 기본 크기 반환
        }
    }

    private float _damage;

    private void Awake()
    {
        Default = transform.localPosition;
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
            return transform.position + Vector3.Scale(_offset + target, transform.lossyScale);
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
        Debug.Log(HalfSize);
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

    public void EnableDamageBox(float damage, Vector3? range = null, Action onHitCallBack = null, float time = 0f)
    {
        OnHitCallback = onHitCallBack;
        _damage = damage;

        // range가 null이 아니면 설정하고, null이면 기본 크기 설정
        SetRange(range ?? GetSkillRange(playerSkill));

        transform.localPosition = Default;
        enabled = true;
        _enableTimer = time;
    }

    public void EnableSkillDamageBox(float damage, Vector3? range = null, Action onHitCallBack = null, float time = 0f)
    {
        OnHitCallback = onHitCallBack;
        _damage = damage;

        // range가 null이 아니면 설정하고, null이면 기본 크기 설정
        SetRange(range ?? GetSkillRange(playerSkill));

        enabled = true;
        _enableTimer = time;
    }

    private void SetRange(Vector3 range)
    {
        HalfSize = range;
        Debug.Log(HalfSize);
        Debug.Log(range + " Range");
    }

    private void OnDestroy()
    {
        OnHitCallback = null;
    }
}