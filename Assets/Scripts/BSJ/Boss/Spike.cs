using EnumTypes;
using System;
using UnityEngine;

public class Spike : MonoBehaviour, ITargetable
{
    [SerializeField] private float _cooldown;
    [SerializeField] private float _damage;
    [SerializeField] private float _hp;
    [SerializeField] private GameObject _trashPrefab;
    private float _damageTimeStamp;

    public Action OnDead { get; internal set; }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void Hit(float dmg, DamageType type = DamageType.Normal)
    {
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
        Instantiate(_trashPrefab, transform.position, transform.rotation);
    }

    private void OnCollisionEnter(Collision collision)
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
                rbOther.GetComponent<PlayerMaster>().Hit(_damage, DamageType.NonKnockback);
            }
        }
    }
}
