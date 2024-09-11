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
        SM.Instance.PlaySound2("boss_SpikeBroken", transform.position);
        return _hp <= 0f;
    }

    private void SpawnTrash()
    {
        GameObject spike = ObjectPoolManager.Instance.DequeueObject(_trashPrefab, transform.position);
        spike.transform.rotation = transform.rotation;
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
        else if (rbOther.CompareTag("EnemyBoss"))
        {
            Hit(999f);
        }
    }
}
