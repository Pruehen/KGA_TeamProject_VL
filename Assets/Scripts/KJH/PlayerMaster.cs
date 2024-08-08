using UnityEngine;

public class PlayerMaster : MonoBehaviour, ITargetable
{
    public PlayerInstanteState _PlayerInstanteState { get; private set; }
    PlayerMove _PlayerMove;
    PlayerAttack _PlayerAttack;

    
    private void Awake()
    {
        _PlayerInstanteState = GetComponent<PlayerInstanteState>();
        _PlayerMove = GetComponent<PlayerMove>();
        _PlayerAttack = GetComponent<PlayerAttack>();
    }

    public void OnAttackState(Vector3 lookTarget)
    {
        _PlayerMove.OnAttackState(lookTarget);
    }

    public Vector3 GetPosition()
    {
        return this.transform.position;
    }

    public void Hit(float dmg)
    {
        _PlayerInstanteState.Hit(dmg);
    }

    public bool IsDead()
    {
        return _PlayerInstanteState.IsDead;
    }
}
