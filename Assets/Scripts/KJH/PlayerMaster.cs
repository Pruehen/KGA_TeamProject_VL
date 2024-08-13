using System;
using UnityEngine;

public class PlayerMaster : MonoBehaviour, ITargetable
{
    public PlayerInstanteState _PlayerInstanteState { get; private set; }
    PlayerMove _PlayerMove;
    PlayerAttack _PlayerAttack;
    PlayerModChangeManager _PlayerModChangeManager;
    [SerializeField] ItemAbsorber _ItemAbsorber;

    public bool IsAbsorptState
    {
        get { return _PlayerInstanteState.IsAbsorptState; }
        set
        {
            _PlayerInstanteState.IsAbsorptState = value;
        }
    }
    public bool IsMeleeMode
    {
        get { return _PlayerInstanteState.IsMeleeMode; }
        set
        {
            _PlayerInstanteState.IsMeleeMode = value;

        }
    }

    private void Awake()
    {
        _PlayerInstanteState = GetComponent<PlayerInstanteState>();
        _PlayerMove = GetComponent<PlayerMove>();
        _PlayerAttack = GetComponent<PlayerAttack>();
        _PlayerModChangeManager = GetComponent<PlayerModChangeManager>();

        _ItemAbsorber.Init();
    }

    public void OnAttackState(Vector3 lookTarget)
    {
        _PlayerMove.OnAttackState(lookTarget);
    }

    public Vector3 GetPosition()
    {
        return this.transform.position;
    }

    public void Register_PlayerModChangeManager(Action callBack_StartAbsorb, Func<int> callBack_SucceseAbsorb, Func<int> callBack_AcquireAll, Action callBack_DropAbsorbingItems)
    {
        _PlayerModChangeManager.OnEnterAbsorptState = callBack_StartAbsorb;
        _PlayerModChangeManager.OnSucceseAbsorptState = callBack_AcquireAll;
        _PlayerModChangeManager.OnSucceseAbsorptState_EntryMelee = callBack_SucceseAbsorb;
        _PlayerModChangeManager.OnEndAbsorptState = callBack_DropAbsorbingItems;
    }


    public void Hit(float dmg)
    {
        _PlayerInstanteState.Hit(dmg);
        DmgTextManager.Instance.OnDmged(dmg, this.transform.position);
    }

    public bool IsDead()
    {
        return _PlayerInstanteState.IsDead;
    }
}
