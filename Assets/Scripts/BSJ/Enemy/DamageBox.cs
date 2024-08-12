using UnityEngine;

public class DamageBox : MonoBehaviour
{
    [SerializeField] private ITargetable _owner;
    [SerializeField] private Vector3 _halfSize = new Vector3(1f, 1f, 1f);
    [SerializeField] private LayerMask _targetLayer;

    private Coroutine _DisableBoxCoroutine;

    private float _enableTimer = 0f;

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
        _owner = transform.parent.GetComponent<ITargetable>();
    }
    private void Start()
    {
        enabled = false;
    }
    private void OnEnable()
    {
        Collider[] result = Physics.OverlapBox(transform.position, HalfSize, transform.rotation, _targetLayer);        

        foreach (Collider hit in result)
        {
            if(hit.attachedRigidbody == null)
            {
                continue;
            }
            ITargetable combat = hit.attachedRigidbody.GetComponent<ITargetable>();
            if (combat == null)
            {
                continue;
            }
            if (_owner == hit)
            {
                continue;
            }            
            combat.Hit(_damage);
        }
    }

    private void Update()
    {
        _enableTimer -= Time.deltaTime;
        if(_enableTimer <= 0f)
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
        Gizmos.DrawWireCube(transform.position, HalfSize);
    }

    /// <summary>
    /// 0이면 한번만 데미지 적용
    /// 시간만큼 데미지 박스를 켜 둠
    /// </summary>
    /// <param name="time"></param>
    public void EnableDamageBox(float damage, float time = 0f)
    {
        _damage = damage;
        enabled = true;
        _enableTimer = time;
    }
}
