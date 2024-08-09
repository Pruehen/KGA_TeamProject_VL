using UnityEngine;

public class Projectile : MonoBehaviour
{
    float _dmg;    
    Rigidbody _Rigidbody;
    public void Init(float dmg, Vector3 initPos, Vector3 projectionVector)
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

        Vector3 randomAxis = Random.onUnitSphere;
        Vector3 randomTorque = randomAxis * Random.Range(0, 10f);
        _Rigidbody.AddTorque(randomTorque);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out ITargetable targetable))
        {
            targetable.Hit(_dmg);
        }

        ObjectPoolManager.Instance.EnqueueObject(this.gameObject);
    }
}
