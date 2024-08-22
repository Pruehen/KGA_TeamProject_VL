using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    private Rigidbody rb;
    private float projectileDamage;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Fire(Vector3 vel, float damage)
    {
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
            other.rigidbody.GetComponent<ITargetable>().Hit(projectileDamage);
            Destroy(gameObject);
        }
    }
}
