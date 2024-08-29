using System;
using System.Collections;
using TreeEditor;
using UnityEngine;

public class DashFire : MonoBehaviour
{
    public enum AttackType
    {
        Range,
        Melee,
        All
    }
    public int currentType;
    [SerializeField] private ITargetable _owner;
    [SerializeField] private LayerMask _targetLayer;
    private Vector3 _RangeSize = new Vector3(1f, 1f, 3f);
    public float _MeleeSize = 3f;
    public Vector3 Defaultoffset = new Vector3(0f, 0.5f, 0.5f);
    [SerializeField] public Vector3 DefaultRange = new Vector3(1f, 1f, 1f);
    private Coroutine _DisableBoxCoroutine;


    private float _enableTimer = 0f;

    public Action OnHitCallback;
    public Vector3 target;
    [SerializeField]
    private float _damage = 10f;

    [SerializeField]
    private float _FireDotDuration = 5f;

    [SerializeField]
    private float _FireExploRadius = 1.3f;

    [SerializeField]
    private float _FireExploDmg = 50f;

    [SerializeField]
    private Vector3 _halfSize;

    [SerializeField]
    private float TIcDmg
    {
        get
        {
            return _damage != 0f ? _damage : GetDmg(_damage);
        }
        set
        {
            _damage = value;
        }
    }

    [SerializeField]
    private float radius
    {
        get
        {
            return _FireExploRadius != 0f ? _FireExploRadius : GetRadius(_FireExploRadius);
        }
        set
        {
            _FireExploRadius = value;
        }
    }

    [SerializeField]
    private float FireDotDuration
    {
        get
        {
            return _FireDotDuration != 0f ? _FireDotDuration : GetfireDotDuration(_FireDotDuration);
        }
        set
        {
            _FireDotDuration = value;
        }
    }

    [SerializeField]
    private float FireExploDmg
    {
        get
        {
            return _FireExploDmg != 0f ? _FireExploDmg : GetfireExploDmg(_FireExploDmg);
        }
        set
        {
            _FireExploDmg = value;
        }
    }

    // SetStat ¸Þ¼­µå¿¡¼­ »ç¿ë
    public void SetStat(float Dmg, float fireDotDuration, float fireExploRadius, float fireExploDmg)
    {
        TIcDmg = Dmg;
        FireDotDuration = fireDotDuration;
        radius = GetRadius(fireExploRadius);
        FireExploDmg = fireExploDmg;
        transform.localScale = Vector3.one * fireExploRadius;
    }

    // Private ÇïÆÛ ¸Þ¼­µåµé
    private Vector3 GetRange(float fireExploRadius)
    {
        return Vector3.one * fireExploRadius;
    }

    private float GetRadius(float fireExploRadius)
    {
        return 1 * fireExploRadius;
    }

    private float GetDmg(float Dmg)
    {
        return Dmg;
    }

    private float GetfireDotDuration(float fireDotDuration)
    {
        return fireDotDuration;
    }

    private float GetfireExploDmg(float fireExploDmg)
    {
        return fireExploDmg;
    }
    public void StartFire(AttackType attackType)
    {
        //Collider[] result = Physics.OverlapBox(Center, HalfSize, transform.rotation, _targetLayer);
        currentType = (int)attackType;
        if (attackType == AttackType.Range)
        {
            StartCoroutine(ExecuteOverlapBoxRoutine());
        }
        else if (attackType == AttackType.All)
        {
            All();
        }
        else
        {
            Melee();
        }
    }
    public void All()
    {
        transform.GetChild(2).gameObject.SetActive(true);

        bool onHit = false;
        Collider[] result = Physics.OverlapSphere(transform.position, _MeleeSize*_FireExploRadius, _targetLayer);
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
            combat.Hit(FireExploDmg);
            Debug.Log(FireExploDmg + "ÆøÆÄµô");
            onHit = true;
        }
        if (onHit)
        {
            OnHitCallback?.Invoke();
        }
        StartCoroutine(ExecuteOverlapSphereRoutine());
    }
    IEnumerator ExecuteOverlapSphereRoutine()
    {
        for (int i = 0; i < 5; i++)
        {
            Collider[] result = Physics.OverlapSphere(transform.position, _MeleeSize*_FireExploRadius, _targetLayer);
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

                combat.Hit(TIcDmg, DamageType.NonKnockback);
                onHit = true;
            }

            if (onHit)
            {
                OnHitCallback?.Invoke();
            }
            yield return new WaitForSeconds(1.0f);
        }
        yield return new WaitForSeconds(1.0f);
       ObjectPoolManager.Instance.EnqueueObject(gameObject);    
    }

    IEnumerator ExecuteOverlapBoxRoutine()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        for (int i = 0; i < 5; i++)
        {
            Collider[] result = Physics.OverlapBox(transform.position, _RangeSize * _FireExploRadius, transform.rotation, _targetLayer);
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

                combat.Hit(TIcDmg);
                onHit = true;
            }

            if (onHit)
            {
                OnHitCallback?.Invoke();
            }
            yield return new WaitForSeconds(1.0f);
        }
        yield return new WaitForSeconds(1.0f);
        ObjectPoolManager.Instance.EnqueueObject(gameObject);
    }

    public void Melee()
    {
        transform.GetChild(1).gameObject.SetActive(true);
        bool onHit = false;
        Collider[] result = Physics.OverlapSphere(transform.position, _MeleeSize*_FireExploRadius, _targetLayer);
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
            combat.Hit(FireExploDmg);
            Debug.Log(FireExploDmg + "ÆøÆÄµô");
            onHit = true;
        }
        if (onHit)
        {
            OnHitCallback?.Invoke();
        }
    }

    private void OnDrawGizmos()
    {
        if (!enabled)
        {
            return;
        }
        Gizmos.color = Color.red;
        
        if (currentType > 0)
        {
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
            Gizmos.DrawSphere(Vector3.zero, _MeleeSize* _FireExploRadius);
            Gizmos.matrix = Matrix4x4.identity;
        }
        else
        {
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
            Gizmos.DrawCube(Vector3.zero, _RangeSize* _FireExploRadius);
            Gizmos.matrix = Matrix4x4.identity;
        }
        
    }

    private void OnDestroy()
    {
        OnHitCallback = null;
    }
    private void OnDisable()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(2).gameObject.SetActive(false);

    }
}
