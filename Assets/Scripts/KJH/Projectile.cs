using EnumTypes;
using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    float _dmg;    
    Rigidbody _Rigidbody;
    Action<PlayerAttackKind, PlayerAttackKind, int> onHit;
    PlayerAttackKind _attackMod;
    PlayerAttackKind _attackKind;
    int _attackCount;
    GameObject _owner;

    public void Init(float dmg, Vector3 initPos, Vector3 projectionVector,
        PlayerAttackKind attackMod, PlayerAttackKind attackKind, int attackCount,
        Action<PlayerAttackKind, PlayerAttackKind,int> onHitCallBack,
        GameObject owner)
    {
        _owner = owner;
        _dmg = dmg;

        transform.position = initPos;

        if(_Rigidbody == null)
        {
            _Rigidbody = GetComponent<Rigidbody>();
        }

        _Rigidbody.velocity = Vector3.zero;
        _Rigidbody.angularVelocity = Vector3.zero;

        _Rigidbody.AddForce(projectionVector, ForceMode.Impulse);

        Vector3 randomAxis = UnityEngine.Random.onUnitSphere;
        Vector3 randomTorque = randomAxis * UnityEngine.Random.Range(0, 10f);
        _Rigidbody.AddTorque(randomTorque);

        _attackMod = attackMod;
        _attackKind = attackKind;
        _attackCount = attackCount;

        onHit = onHitCallBack;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.rigidbody == null)
        {
            ObjectPoolManager.Instance.EnqueueObject(this.gameObject);
            return;
        }
        if (collision.rigidbody.TryGetComponent(out ITargetable targetable))
        {
            onHit?.Invoke(_attackMod,_attackKind,_attackCount);
            targetable.Hit(_dmg, _owner.transform);
        }

        ObjectPoolManager.Instance.EnqueueObject(this.gameObject);
    }
}
