using UnityEngine;

public interface IInteractable
{
    public string GetName();
    public Vector3 GetPos();

    public bool TryInteract(Vector3 originPos, float checkRange);
}

public interface ITargetable
{
    public Vector3 GetPosition();    

    public void Hit(float dmg, DamageType type = DamageType.Normal);
    public bool IsDead();
}

public abstract class Passive
{
    protected PlayerInstanteState _state;
    protected PassiveData _passiveData;

    public void Init(PlayerInstanteState playerState)
    {
        _state = playerState;
        SetPassiveData();
    }
    public abstract void SetPassiveData();
    public abstract void Active();
    public abstract void DeActive();    
}