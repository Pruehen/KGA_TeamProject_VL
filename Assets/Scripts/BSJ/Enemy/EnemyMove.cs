using System;
using UnityEngine;

[Serializable]
public class EnemyMove
{
    public bool IsJumping;
    public bool IsGrounded { get; set; }
    public bool IsLanded { get; set; }
    public float JumpedTime { get; set; }
    public bool IsMoving { get; internal set; }

    Transform transform;
    private Rigidbody _rigidbody;
    internal bool isAlmostGrouonded;

    public Rigidbody Rigidbody => _rigidbody;

    public bool IsCollideOnJump { get; internal set; }

    public void Init(Transform transform, Rigidbody rigidbody)
    {
        this.transform = transform;
        _rigidbody = rigidbody;
    }
    public void DoUpdate(float deltaTime)
    {
        IsGrounded = Physics.CheckSphere(transform.position + (Vector3.up * .35f),
            .4f, LayerMask.GetMask("Environment"));
    }

    public void OnCollisionStay(Collision collision)
    {
    }
}
