using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour, ITargetable
{
    Combat _combat;
    private void Awake()
    {
        _combat = new Combat();
        _combat.Init(100f);
        _combat.OnDead += OnDead;
    }

    private void OnDead()
    {
        Destroy(gameObject);
    }

    public Vector3 GetPosition()
    {
        throw new System.NotImplementedException();
    }

    public void Hit(float dmg)
    {
        _combat.Damaged(dmg);
    }

    public bool IsDead()
    {
        throw new System.NotImplementedException();
    }
}
