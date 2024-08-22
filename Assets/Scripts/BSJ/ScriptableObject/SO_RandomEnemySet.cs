using UnityEngine;

[CreateAssetMenu(fileName = "RandomEnemySetData", menuName = "Enemy/RandomEnemySet", order = 0)]
public class SO_RandomEnemySet : ScriptableObject
{
    [Header("Рћ С§Че")]
    public SO_EnemyBase[] RandomEnemySet;
}
