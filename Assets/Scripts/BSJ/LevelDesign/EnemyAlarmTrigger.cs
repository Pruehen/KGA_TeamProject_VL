using UnityEngine;

public class EnemyAlarmTrigger : MonoBehaviour
{
    private float maxScale;
    private Enemy[] _enemyList;



    private void Awake()
    {
        _enemyList = GetComponentsInChildren<Enemy>();
    }
    private void OnValidate()
    {
        _enemyList = GetComponentsInChildren<Enemy>();
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
        foreach (Enemy enemy in _enemyList)
        {
            enemy.ForceAlram();
        }
    }
}
