using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throw : MonoBehaviour
{
    Rigidbody rB;
    public float speed = 5f;
    private void Awake()
    {
        rB = GetComponent<Rigidbody>();
    }
    void Start()
    {
        rB.AddForce(transform.forward * speed);
    }
}
