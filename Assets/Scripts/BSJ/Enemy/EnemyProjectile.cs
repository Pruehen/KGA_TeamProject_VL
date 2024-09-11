using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    private Rigidbody rb;
    private float projectileDamage;
    [SerializeField] private Timer timer;
    [SerializeField] private float limitTime = 8f;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        timer.Init(limitTime, Enqueue);
    }

    private void OnEnable()
    {
        timer.StartTimer();
    }

    private void Update()
    {
        timer.DoUpdate(Time.deltaTime);
    }

    public void Fire(Vector3 vel, float damage)
    {
        rb.velocity = vel;
        projectileDamage = damage;
    }

    private void Enqueue()
    {
        ObjectPoolManager.Instance.EnqueueObject(gameObject);
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
            Enqueue();
        }
    }
}
