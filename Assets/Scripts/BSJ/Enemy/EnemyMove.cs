using System;
using UnityEngine;
using UnityEngine.AI;

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
    private NavMeshAgent _agent;
    internal bool isAlmostGrouonded;

    public Rigidbody Rigidbody => _rigidbody;

    public bool IsCollideOnJump { get; internal set; }

    public void Init(Transform transform, Rigidbody rigidbody, NavMeshAgent agent)
    {
        this.transform = transform;
        _rigidbody = rigidbody;
        _agent = agent;
    }
    public void DoUpdate(float deltaTime)
    {
        IsGrounded = Physics.CheckSphere(transform.position + (Vector3.up * .35f),
            .4f, LayerMask.GetMask("Environment"));

        if(Rigidbody.velocity.magnitude > .1f || IsNavMeshMoving(_agent))
        {
            IsMoving = true;
        }
        else
        {
            IsMoving = false;
        }
    }
    private bool IsNavMeshMoving(NavMeshAgent agent)
    {
        return agent.updatePosition && agent.velocity .magnitude >= 0.1f && !agent.isStopped;
    }
    private bool CheckIsGrounded()
    {
        return Physics.CheckSphere(transform.position, .1f, LayerMask.NameToLayer("Environment"));
    }

    public void OnCollisionStay(Collision collision)
    {
    }
}
