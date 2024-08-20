using System;
using UnityEngine;

public enum ItemState
{
    Normal,
    PullToRevolve,
    Revolving,
    PullToAcquire,
    Revolve
}

public class TrashItem : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Collider[] _collider;

    public Action<TrashItem> OnRevolve;

    private ItemState _itemState = ItemState.Normal;

    public ItemState State
    {
        get => _itemState;
        set => _itemState = value;
    }

    private void Awake()
    {
        _collider = transform.GetChild(0).GetComponentsInChildren<Collider>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void Update()
    {
        switch (_itemState)
        {
            case ItemState.Normal:
                break;
            case ItemState.PullToRevolve:
                UpdatePullToRevolve(_revolveRadius,_abolsionSpeed);
                break;
            case ItemState.PullToAcquire:
                UpdatePullToCenterAndDestroy(_acquireSpeed);
                break;
        }
    }
    public void DisablePhysics()
    {
        _rigidbody.isKinematic = true;
        foreach (var collider in _collider)
        {
            collider.isTrigger = true;
        }
    }
    public void EnablePhysics()
    {
        _rigidbody.isKinematic = false;
        foreach (var collider in _collider)
        {
            collider.isTrigger = false;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.rigidbody == null)
        {
            return;
        }
        if (collision.rigidbody.CompareTag("Player"))
        {
            collision.rigidbody.GetComponent<PlayerInstanteState>().AcquireBullets(1);
            Destroy(gameObject);
        }
    }
    public void PullToCenterAndDestroy(float acquireSpeed)
    {
        State = ItemState.PullToAcquire;
        _acquireSpeed = acquireSpeed;
    }
    private void UpdatePullToCenterAndDestroy(float aquireSpeed)
    {
        Vector3 targetPos = Vector3.zero;
        if (Vector3.Distance(targetPos, transform.localPosition) <= 0.1f)
        {
            Destroy(transform.gameObject);
        }
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, Time.deltaTime * aquireSpeed);
        Debug.Log("EndOfAbsolsion");
    }

    float _revolveRadius;
    float _abolsionSpeed;
    float _acquireSpeed;
    public void PullToRevolve(float revolveRadius, float abolsionSpeed)
    {
        State = ItemState.PullToRevolve;
        _revolveRadius = revolveRadius;
        _abolsionSpeed = abolsionSpeed;
    }

    private void UpdatePullToRevolve(float revolveRadius, float abolsionSpeed)
    {
        Vector3 targetPos = transform.localPosition;
        targetPos = targetPos.normalized * revolveRadius;
        if (Vector3.Distance(targetPos, transform.localPosition) >= 0.1f)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, Time.deltaTime * abolsionSpeed);
        }
        else
        {
            OnRevolve?.Invoke(this);
            State = ItemState.Revolve;
        }
    }



    public void DropItem()
    {
        if (State == ItemState.PullToAcquire)
            return;
        EnablePhysics();
        transform.SetParent(null, true);
        State = ItemState.Normal;
        OnRevolve = null;
    }

    public void StartAbsorbe(Transform parent)
    {
        transform.SetParent(parent, true);
        DisablePhysics();
    }
}
