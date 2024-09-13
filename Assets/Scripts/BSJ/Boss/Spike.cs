using EnumTypes;
using System;
using UnityEngine;

[Serializable]
public class Spike : MonoBehaviour, ITargetable
{
    [SerializeField] private float _cooldown;
    [SerializeField] private float _damage;
    [SerializeField] private float _hp;
    [SerializeField] private GameObject _trashPrefab;
    [SerializeField] private DynamicItemGen _dynamicItemSpawn;
    private float _damageTimeStamp;

    private Collider _collision;

    public Action OnDead { get; internal set; }

    private bool _isDestroyable = false;

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    private void Awake()
    {
        _collision = GetComponent<Collider>();
    }

    public void Hit(float dmg, DamageType type = DamageType.Normal)
    {
        if (!_isDestroyable)
            return;
        _hp -= dmg;
        if (_hp <= 0f)
        {
            OnDead?.Invoke();
            SpawnTrash();
        }
    }

    public bool IsDead()
    {
        return _hp <= 0f;
    }

    private void SpawnTrash()
    {
        SM.Instance.PlaySound2("boss_SpikeBroken", transform.position);
        _dynamicItemSpawn.SpawnItem();
    }

    private void OnCollisionStay(Collision collision)
    {
        Rigidbody rbOther = collision.rigidbody;
        if (rbOther == null)
        {
            return;
        }

        if (rbOther.CompareTag("Player"))
        {
            if (Time.time >= _damageTimeStamp + _cooldown)
            {
                _damageTimeStamp = Time.time;
                rbOther.GetComponent<PlayerMaster>().Hit(_damage, DamageType.NonKnockback);
                SM.Instance.PlaySound2("boss_Spikehit", transform.position);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rbOther = other.attachedRigidbody;
        if (rbOther == null)
        {
            return;
        }
        if (rbOther.CompareTag("EnemyBoss"))
        {
            Hit(999f);
        }
    }

    public void DisablePhysics()
    {
        _collision.enabled = false;
    }
    public void EnablePhysics()
    {
        _collision.enabled = true;
        _isDestroyable = false;
    }

    public void EnableSpike()
    {
        gameObject.SetActive(true);
    }

    public void EnableDestroy()
    {
        _isDestroyable = true;
    }
}
