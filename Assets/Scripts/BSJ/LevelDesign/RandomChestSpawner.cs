using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class RandomChestSpawner : MonoBehaviour
{
    private Chest _chest;

    [SerializeField] private float _percentage;

    private void Awake()
    {
        _chest = GetComponent<Chest>();
    }

    private void Start()
    {
        float r = Random.value;
        if(r >= _percentage * .01f)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnDrawGizmos()
    {
        if (_percentage == 15f)
        {
            Gizmos.color = Color.green;
        }
        if (_percentage == 50f)
        {
            Gizmos.color = Color.red;
        }
        Gizmos.DrawSphere(transform.position + Vector3.up * 2f, .5f);
    }
}
