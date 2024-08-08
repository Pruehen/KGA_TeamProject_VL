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

    public void Hit(float dmg);
}