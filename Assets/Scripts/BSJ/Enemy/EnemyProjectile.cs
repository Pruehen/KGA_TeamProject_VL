using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    private Rigidbody rb;
    private float projectileDamage;
    private GameObject _owner;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Fire(Vector3 vel, float damage, GameObject owner = null)
    {
        _owner = owner;
        rb.velocity = vel;
        projectileDamage = damage;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.rigidbody == null)
        {
            Destroy(gameObject);
            return;
        }
        if(other.rigidbody.CompareTag("Player"))
        {
            other.rigidbody.GetComponent<ITargetable>().Hit(projectileDamage, _owner.transform);
            Destroy(gameObject);
        }
    }
}
