using System;
using UnityEngine;
using UnityEngine.AI;

[Serializable]
public class EnemyMove
{
    private EnemyBase _owner;
    public bool IsLaunching;
    public float LaunchedTime;
    public bool IsGrounded;
    public bool IsLanded;
    public bool IsMoving;

    Transform transform;
    private Rigidbody _rigidbody;
    private NavMeshAgent _agent;

    public Rigidbody Rigidbody => _rigidbody;

    public bool IsHommingEnd { get; internal set; }

    public bool IsLaunchEnd = false;

    public bool IsCrashed = false;

    public bool IsCollided = false;

    public bool isHomming = false;

    public void Init(EnemyBase owner, Transform transform, Rigidbody rigidbody, NavMeshAgent agent)
    {
        _owner = owner;
        this.transform = transform;
        _rigidbody = rigidbody;
        _agent = agent;
    }
    public void DoUpdate(float deltaTime)
    {
        int layerNum = LayerMask.NameToLayer("Environment");
        LayerMask layer = 1 << layerNum;
        Vector3 center = transform.position + (Vector3.up * .35f);
        IsGrounded = Physics.CheckSphere(center,
            .65f, layer);
        Debug.DrawLine(center, center + (Vector3.down * .65f), Color.magenta);

        if (Rigidbody.velocity.magnitude > .1f || IsNavMeshMoving(_agent))
        {
            IsMoving = true;
        }
        else
        {
            IsMoving = false;
        }

        IsLaunchEnd = false;
        IsHommingEnd = false;
        if (IsLaunching)
        {
            if (Time.time >= LaunchedTime + .25f)
            {
                IsLanded = IsLanded || (IsGrounded && Rigidbody.velocity.y <= 0.1f);
                IsCrashed = IsCrashed || IsCollided;
            }
            if(IsLanded || IsCrashed)
            {
                if (isHomming)
                    IsHommingEnd = true;
                SetEnableRigidbody(false);
                IsLaunching = false;
                IsLaunchEnd = true;
                isHomming = false;
                _owner.gameObject.layer = LayerMask.NameToLayer("EnemyCollider");
                _owner.CharacterCollider.gameObject.layer = LayerMask.NameToLayer("EnemyCollider");
            }
        }

        if (isHomming)
        {
            ProjectileCalc.Homming(Rigidbody, _hommingTarget, _hommingForce);
            Vector3 a = transform.position;
            Vector3 b = _hommingTarget.position;
            a.y = b.y;
            if(Vector3.Distance(a,b) <= .3f)
            {
                IsHommingEnd = true;
                isHomming = false;
            }
        }
    }
    private bool IsNavMeshMoving(NavMeshAgent agent)
    {
        return agent.updatePosition && agent.velocity.magnitude >= 0.1f && !agent.isStopped;
    }
    private bool CheckIsGrounded()
    {
        return Physics.CheckSphere(transform.position, .1f, LayerMask.NameToLayer("Environment"));
    }

    public void OnCollisionStay(Collision collision)
    {
        IsCollided = true;
    }

    public void Launch(Vector3 vel)
    {
        isHomming = false;
        Launch_Only(vel);
    }

    private Transform _hommingTarget;
    private float _hommingForce;
    public void HommoingLaunch(Vector3 vel, Transform target, float force)
    {
        _hommingTarget = target;
        _hommingForce = force;
        isHomming = true;
        Launch_Only(vel);
    }

    private void Launch_Only(Vector3 vel)
    {
        SetEnableRigidbody(true);
        LaunchedTime = Time.time;
        IsLaunching = true;
        IsLanded = false;
        IsCrashed = false;
        IsCollided = false;

        Rigidbody.velocity = vel;

        _owner.gameObject.layer = LayerMask.NameToLayer("LaunchedEnemy");
        _owner.CharacterCollider.gameObject.layer = LayerMask.NameToLayer("LaunchedEnemy");
    }


    public void SetEnableRigidbody(bool condition)
    {
        _owner.SetEnableRigidbody(condition);
    }
}

