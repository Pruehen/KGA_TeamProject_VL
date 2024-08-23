using UnityEngine;

public class RandomEnemyGroupSpawner : MonoBehaviour
{
    void Start()
    {
        int childCount = transform.childCount;

        int r = Random.Range(0, childCount);

        for (int i = 0; i < childCount; i++)
        {
            if (r == i)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    private void OnDrawGizmos()
    {

        int childCount = transform.childCount;

        for (int i = 0; i < childCount; i++)
        {
            Gizmos.color = new Color(.1f * ((i + 3f) % 5f), .1f * ((i + 4f) % 5f), .1f * ((i + 5f) % 5f));

            int enemyCount = transform.GetChild(i).childCount;

            for (int j = 0; j < enemyCount; j++)
            {
                Gizmos.DrawSphere(transform.GetChild(i).GetChild(j).transform.position + Vector3.up * 2f, .5f);
            }
        }
    }
}
