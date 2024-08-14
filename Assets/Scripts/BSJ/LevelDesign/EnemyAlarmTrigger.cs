using UnityEngine;

public class EnemyAlarmTrigger : MonoBehaviour
{
    private float maxScale;
    private Enemy[] _enemyList;



    private void OnAwake()
    {
        _enemyList = GetComponentsInChildren<Enemy>();
    }
    private void OnValidate()
    {
        _enemyList = GetComponentsInChildren<Enemy>();
    }
    private void OnTriggerEnter(Collider other)
    {
        AlramAll();
        GetComponent<Collider>().enabled = false;
    }
    private void AlramAll()
    {
        foreach (Enemy enemy in _enemyList)
        {
            enemy.ForceAlram();
        }
    }
}
