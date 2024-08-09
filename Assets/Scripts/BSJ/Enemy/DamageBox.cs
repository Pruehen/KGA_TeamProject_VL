using UnityEngine;

public class DamageBox : MonoBehaviour
{
    [SerializeField] private ITargetable _owner;
    [SerializeField] private Vector3 _halfSize = new Vector3(1f, 1f, 1f);
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
        Collider[] result = Physics.OverlapBox(transform.position, HalfSize, transform.rotation);        

        foreach (Collider hit in result)
        {

            ITargetable combat = hit.GetComponent<ITargetable>();
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
        enabled = false;
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

    public void SetDamage(float damage)
    {
        _damage = damage;
    }
}
