using System;
using UnityEngine;


[Serializable]
public class Combat
{
    GameObject _owner;

    [SerializeField] private float _maxHp = 100f;

    [SerializeField] private float _hp = 100f;

    [SerializeField] private bool _dead = false;

    [SerializeField] private float _invincibleTimeOnHit = .1f;

    [SerializeField] private float _prevHitTime = 0f;

    public Func<bool> AdditionalDamageableCheck { get; set; }
    public Action OnDamaged;
    public Action OnHeal;
    public Action OnDead;


    public Action OnAttackSucceeded;
    public Action OnKillEnemy;
    public Action<Combat, float> OnAttack;

    private bool _isInvincible;
    private float _invincibleTimer = .1f;
    public Action OnInvincible;
    public Action OnReleaseInvincible;

    private bool _isSuperArmor;
    private float _superArmorTimer = .1f;
    public Action OnSuperArmor;
    public Action OnReleaseSuperArmor;

    private bool _isEvade;
    private float _evadeTimer = .1f;
    public Action OnEvade;
    public Action OnReleaseEvade;

    public Action OnKnockBack;

    public void Init(GameObject owner, float maxHp)
    {
        _owner = owner;
        _defaultLayer = owner.layer;
        _maxHp = maxHp;
        _hp = maxHp;
        ResetDead();
    }

    public void DoUpdate(float deltaTime)
    {
        if(_evadeTimer > 0f)
        {
            _evadeTimer -= deltaTime;
        }
        else
        {
            if (_isEvade)
            {
                _owner.layer = _defaultLayer;
                OnReleaseEvade?.Invoke();
                _isEvade = false;
            }
        }


        if (_invincibleTimer > 0f)
        {
            _invincibleTimer -= deltaTime;
        }
        else
        {
            if(_isInvincible)
            {
                OnReleaseInvincible?.Invoke();
                _isInvincible = false;
            }
        }

        if (_superArmorTimer > 0f)
        {
            _superArmorTimer -= deltaTime;
        }
        else
        {
            if(_isSuperArmor)
            {
                OnReleaseSuperArmor?.Invoke();
                _isSuperArmor = false;
            }
        }
    }

    public float GetHp() { return _hp; }
    public float GetMaxHp()
    {
        return _maxHp;
    }


    private bool IsDamageable()
    {
        if (_isInvincible)
        {
            return false;
        }
        if (_dead)
        {
            return false;
        }
        bool result = true;
        if (AdditionalDamageableCheck != null)
        {
            result = result && AdditionalDamageableCheck.Invoke();
        }
        if (!result)
        {
            return false;
        }
        return true;
    }
    public bool Damaged(float damage, Transform attacker = null)
    {
        if (!IsDamageable())
            return false;

        if (!_isSuperArmor)
        {
            OnKnockBack?.Invoke();
        }

        SetInvincible(_invincibleTimeOnHit);
        _prevHitTime = Time.time;
        damage = Mathf.Max(0f, damage);
        _hp -= damage;

        OnDamaged?.Invoke();

        if (_hp <= 0f)
        {
            _dead = true;
            OnDead?.Invoke();
        }
        return true;
    }
    public void Heal(float amount)
    {
        if (_hp < _maxHp)
        {
            _hp += amount;
            if (_hp > _maxHp)
            {
                _hp = _maxHp;
            }
        }
        if (OnHeal != null)
        {
            OnHeal.Invoke();
        }
    }
    public bool IsDead()
    {
        return _dead;
    }
    public void Die()
    {
        Damaged(9999999999f);
    }
    public void ResetDead()
    {
        ResetHp();
        _dead = false;
    }
    private void ResetHp()
    {
        Heal(9999999999f);
    }

    int _defaultLayer;
    public void SetEvade(float time)
    {
        OnEvade?.Invoke();

        if(_invincibleTimer < time)
        {
            _evadeTimer = time;
            _owner.layer = LayerMask.GetMask("Evade");
        }
    }
    public void ResetEvade()
    {
        _evadeTimer = 0f;
        _owner.layer = _defaultLayer;
        OnReleaseEvade?.Invoke();
    }

    public void SetSuperArmor(float time)
    {
        OnInvincible?.Invoke();

        if(_superArmorTimer < time)
        {
            _superArmorTimer = time;
        }
    }
    public void ResetSuperArmor()
    {
        _superArmorTimer = 0f;
        OnReleaseSuperArmor?.Invoke();
    }

    public void SetInvincible(float time)
    {
        OnSuperArmor?.Invoke();

        if(_invincibleTimer < time)
        {
            _invincibleTimer = time;
        }
    }
    public void ResetInvincible()
    {
        _invincibleTimer = 0f;
        OnReleaseInvincible?.Invoke();
    }

}