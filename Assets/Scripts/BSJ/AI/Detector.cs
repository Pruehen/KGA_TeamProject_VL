using System;
using System.Linq;
using UnityEngine;

public class Detector : MonoBehaviour
{
    [SerializeField] private EnemyBase _owner;
    private string _targetTag;
    private float _detectionRadius;
    public bool IsForceAlramed { get; private set; }

    private Vector3 _lastValidPostion;

    public bool isPlayerInRange { get; private set; }

    public Action<Detector> OnDetect;

    [SerializeField]
    private Collider Target
    {
        get => _target;
        set
        {
            if (value != null)
            {
                _lastTarget = _target;

                if (_latestTarget == null)
                {
                    _latestTarget = value.transform;
                    OnDetect?.Invoke(this);
                }
            }
            _target = value;
        }
    }

    public float TargetDistance { get { return Vector3.Distance(transform.position, _latestTarget.position); } }

    private Collider _target;

    private Collider _lastTarget;
    private Transform _latestTarget;

    [SerializeField] private LayerMask _targetLayer;
    [SerializeField] private LayerMask _groundLayer;
    public void Init(EnemyBase owner, string targetTag, float detectionRadius)
    {
        _owner = owner;
        _targetTag = targetTag;
        _detectionRadius = detectionRadius;
    }

    private void OnValidate()
    {
        if (_owner == null)
        {
            _owner = transform.parent.GetComponent<EnemyBase>();
        }
    }

    public Transform GetTarget()
    {
        if (Target == null)
        {
            return null;
        }
        return Target.transform;
    }

    public void FixedUpdate()
    {
        Collider[] overlap = Physics.OverlapSphere(transform.position, _detectionRadius, _targetLayer);

        if (overlap.Length == 0)
        {
            return;
        }
        //check overlap contains player taged

        Collider playerCol = overlap.FirstOrDefault((col) => col.CompareTag("Player"));

        isPlayerInRange = playerCol != null;

        if (isPlayerInRange)
        {
            Target = playerCol;
            _lastValidPostion = Target.bounds.center;
        }
        else
        {
            Target = null;
        }
    }
    public bool IsTargetVisible()
    {
        if (Target == null)
        {
            return false;
        }
        return IsTargetVisible(Target);
    }


    public bool IsTargetVisible(Collider target)
    {
        Vector3 center = transform.position;
        Vector3 targetCenter = target.bounds.center;
        if (Physics.Raycast(center,
            targetCenter - (center),
            out RaycastHit hit, _detectionRadius, _groundLayer | _targetLayer))
        {
            Debug.DrawLine(center, hit.point, Color.magenta);
            if (hit.collider.CompareTag("Player"))
            {
                return true;
            }
        }
        else
        {
            Debug.DrawRay(center, targetCenter - (center), Color.green);
        }
        return false;
    }
    public Vector3 GetPosition()
    {
        return _lastValidPostion;
    }

    public Transform GetLastTarget()
    {
        if (_lastTarget == null)
        {
            return null;
        }
        return _lastTarget.transform;
    }

    public Vector3 GetLastPosition()
    {
        return _lastTarget.bounds.center;
    }

    public Transform GetLatestTarget()
    {
        return _latestTarget;
    }


    public bool IsTargetFar(float range)
    {
        Transform target = GetLatestTarget();

        if (target == null)
        {
            return false;
        }
        float dist = Vector3.Distance(GetLatestTarget().position, transform.position);
        if (dist >= range)
        {
            return true;
        }
        return false;
    }
    public bool IsTargetNear(float range)
    {
        return !IsTargetFar(range);
    }

    public void ActiveForceAlarm()
    {
        _detectionRadius = 9999f;
        IsForceAlramed = true;
    }
}
