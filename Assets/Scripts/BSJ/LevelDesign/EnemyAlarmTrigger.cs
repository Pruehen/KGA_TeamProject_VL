using UnityEngine;

public class EnemyAlarmTrigger : MonoBehaviour
{
    private float maxScale;
    private EnemyBase[] _enemyList;



    private void Awake()
    {
        _enemyList = GetComponentsInChildren<EnemyBase>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody == null)
        {
            return;
        }
        if (other.attachedRigidbody.CompareTag("Player"))
        {
            AlramAll();
            GetComponent<Collider>().enabled = false;
        }
    }
    private void AlramAll()
    {
        foreach (EnemyBase enemy in _enemyList)
        {
            enemy.ForceAlram();
        }
    }
}
