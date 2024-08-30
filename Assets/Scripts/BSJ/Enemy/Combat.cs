using System;
using UnityEngine;


[Serializable]
public class Combat
{
    GameObject _owner;

    [SerializeField] private float _maxHp = 100f;

    [SerializeField] private float _hp = 100f;

    [SerializeField] private bool _dead = false;

    [SerializeField] public float InvincibleTimeOnHit = .1f;

    [SerializeField] private float _prevHitTime = 0f;

    public Func<bool> AdditionalDamageableCheck { get; set; }
    public Action<DamageType> OnDamaged;
    public Action OnHeal;
    public Action OnDead;


    public Action OnAttackSucceeded;
    public Action OnKillEnemy;
    public Action<Combat, float> OnAttack;

    private bool _isInvincible;
    public bool IsInvincible { get => _isInvincible; private set => _isInvincible = value; }
    private float _invincibleTimer = .1f;
    public Action OnInvincible;
    public Action OnReleaseInvincible;

    private bool _isSuperArmor;
    public bool IsSuperArmor {get => _isSuperArmor; private set => _isSuperArmor = value; }
    private float _superArmorTimer = .1f;
    public Action OnSuperArmor;
    public Action OnReleaseSuperArmor;

    private bool _isEvade;
    public bool IsEvade {get => _isEvade; private set => _isEvade = value; }
    private float _evadeTimer = .1f;
    public Action OnEvade;
    public Action OnReleaseEvade;

    public Action OnKnockback;

    public void Init(GameObject owner, float maxHp)
    {
        _owner = owner;
        _defaultLayer = owner.layer;
        _maxHp = maxHp;
        _hp = maxHp;
        ResetCombat();
    }

    public void DoUpdate()
    {
        float deltaTime = Time.deltaTime;
        if (_evadeTimer > 0f)
        {
            _evadeTimer -= deltaTime;
        }
        else
        {
            if (IsEvade)
            {
                ResetEvade();
            }
        }


        if (_invincibleTimer > 0f)
        {
            _invincibleTimer -= deltaTime;
        }
        else
        {
            if (IsInvincible)
            {
                ResetInvincible();
            }
        }

        if (_superArmorTimer > 0f)
        {
            _superArmorTimer -= deltaTime;
        }
        else
        {
            if (IsSuperArmor)
            {
                ResetSuperArmor();
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
        if (IsInvincible)
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
    public bool Damaged(float damage, DamageType type = DamageType.Normal, Transform attacker = null)
    {
        if (!IsDamageable())
            return false;

        if (!IsSuperArmor && type != DamageType.NonKnockback)
        {
            OnKnockback?.Invoke();
        }

        SetInvincible(InvincibleTimeOnHit);
        _prevHitTime = Time.time;
        damage = Mathf.Max(0f, damage);
        _hp -= damage;

        OnDamaged?.Invoke(type);

        Debug.Log($"{damage}");

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
    public void ResetCombat()
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
        IsEvade = true;
        OnEvade?.Invoke();

        if (_evadeTimer < time)
        {
            _evadeTimer = time;
            _owner.layer = LayerMask.NameToLayer("Evade");
        }
    }
    public void ResetEvade()
    {
        IsEvade = false;
        _evadeTimer = 0f;
        _owner.layer = _defaultLayer;
        OnReleaseEvade?.Invoke();
    }

    public void SetSuperArmor(float time)
    {
        IsSuperArmor = true;
        OnSuperArmor?.Invoke();

        if (_superArmorTimer < time)
        {
            _superArmorTimer = time;
        }
    }
    public void ResetSuperArmor()
    {
        IsSuperArmor = false;
        _superArmorTimer = 0f;
        OnReleaseSuperArmor?.Invoke();
    }

    public void SetInvincible(float time)
    {
        IsInvincible = true;
        OnInvincible?.Invoke();

        if (_invincibleTimer < time)
        {
            _invincibleTimer = time;
        }
    }
    public void ResetInvincible()
    {
        IsInvincible = false;
        _invincibleTimer = 0f;
        OnReleaseInvincible?.Invoke();
    }

    public void SetMaxHp(float v)
    {
        _maxHp = v;
    }

    public float GetHpRatio()
    {
        return _hp / GetMaxHp();
    }

    public void ForceChangeHp(float value)
    {
        _hp = value;
        if (_hp > GetMaxHp())
        {
            _hp = GetMaxHp();
        }
        _dead = false;
    }
}