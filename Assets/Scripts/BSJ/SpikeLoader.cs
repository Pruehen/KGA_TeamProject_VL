using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeLoader : MonoBehaviour
{
    [SerializeField] private GameObject _poolPrefab;
    [SerializeField] private int _count = 5000;
    private void Awake()
    {
        ObjectPoolManager.Instance.CreatePool(_poolPrefab, _count);
    }
}
