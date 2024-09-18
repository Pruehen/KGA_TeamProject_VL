using EnumTypes;
using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    float _dmg;    
    Rigidbody _Rigidbody;
    Action<bool,bool> onHit;
    bool _isDashAttack;
    bool _isLastAttack;
    int _attackCount;

    public void Init(float dmg, Vector3 initPos, Vector3 projectionVector,
        bool isDashAttack, bool isLastAttack, int attackCount,
        Action<bool, bool> onHitCallBack)
    {
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

        _isDashAttack = isDashAttack;
        _isLastAttack = isLastAttack;
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
            onHit?.Invoke(_isDashAttack,_isLastAttack);
            targetable.Hit(_dmg);
        }

        ObjectPoolManager.Instance.EnqueueObject(this.gameObject);
        SM.Instance.PlaySound2("playerRangeProjectileHIt", transform.position);
    }
}
