using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashItem : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Collider[] _collider;

    private void Awake()
    {
        _collider = transform.GetChild(0).GetComponentsInChildren<Collider>();
        _rigidbody = GetComponent<Rigidbody>();
    }
    public void DisablePhysics()
    {
        _rigidbody.isKinematic = true;
        foreach (var collider in _collider)
        {
            collider.isTrigger = true;
        }
    }
    public void EnablePhysics()
    {
        _rigidbody.isKinematic = false; 
        foreach (var collider in _collider)
        {
            collider.isTrigger = false;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.rigidbody == null)
        {
            return;
        }
        if(collision.rigidbody.CompareTag("Player"))
        {
            collision.rigidbody.GetComponent<PlayerInstanteState>().AcquireBullets(1);
            Destroy(gameObject);
        }
    }
}
