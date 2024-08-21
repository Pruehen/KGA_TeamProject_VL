using System;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditorInternal.ReorderableList;

public class DamageBox : MonoBehaviour
{
    [SerializeField] private ITargetable _owner;
    [SerializeField] private Vector3 _halfSize = new Vector3(1f, 1f, 1f);
    [SerializeField] private LayerMask _targetLayer;
    [SerializeField] private Vector3 _offset;
    private Vector3 Default;
    private Coroutine _DisableBoxCoroutine;

    private float _enableTimer = 0f;

    public Action OnHit;
    public Vector3 target;
    private Vector3 HalfSize
    {
        get
        {
            return new Vector3()
            {
                x = _halfSize.x * transform.lossyScale.x,
                y = _halfSize.y * transform.lossyScale.y,
                z = _halfSize.z * transform.lossyScale.z
            };
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
            return transform.position + Vector3.Scale(_offset+target, transform.lossyScale);
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
            OnHit?.Invoke();
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
        if (enabled == false)
        {
            return;
        }
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(Center, HalfSize);
    }

    /// <summary>
    /// 0이면 한번만 데미지 적용
    /// 시간만큼 데미지 박스를 켜 둠
    /// </summary>
    /// <param name="time"></param>
    public void EnableDamageBox(float damage, float range = 1f, Action onHitCallBack = null, float time = 0f)
    {
        if(onHitCallBack != null)
        {
            OnHit += onHitCallBack;
        }
        SetRange(range);

        _damage = damage;
        transform.localPosition = Default;
        enabled = true;
        _enableTimer = time;
    }
    public void EnableSkillDamageBox(float damage, float range = 1f, Action onHitCallBack = null, float time = 0f)
    {
        if (onHitCallBack != null)
        {
            OnHit += onHitCallBack;
        }
        //transform.localPosition = target;
        SetRange(range);

        _damage = damage;

        enabled = true;
        _enableTimer = time;
    }

    private void SetRange(float range)
    {
        transform.localScale = new Vector3(range, range, range);
    }

    private void OnDestroy()
    {
        OnHit = null;
    }
}
