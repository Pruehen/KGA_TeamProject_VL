using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicItemGen : MonoBehaviour
{
    [SerializeField] private GameObject[] items;

    [SerializeField] private float _count = 4f;

    public void SpawnItem()
    {
        for (int i = 0; i < _count; i++)
        {
            int itemIndex = Random.Range(0, items.Length);
            GameObject item = ObjectPoolManager.Instance.DequeueObject(items[itemIndex]);
            SpikeManager.Instance.Trashs.Add(item.GetComponent<TrashItem>());

            item.transform.position = transform.position;
            item.transform.rotation = Quaternion.identity;
        }
    }
}
