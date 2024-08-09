using System;
using UnityEngine;

public class PlayerInstanteState : MonoBehaviour
{
    public float hp { get; private set; }
    public float stamina { get; private set; }
    public int bullets { get; private set; }
    public int meleeBullets { get; private set; }
    public float skillGauge { get; private set; }
    public bool IsAbsorptState { get; set; }

    bool _isMeleeMode;
    public bool IsMeleeMode
    {
        get { return _isMeleeMode; }
        set
        {
            _isMeleeMode = value;
            OnMeleeModeChanged?.Invoke(_isMeleeMode);
        }
    }

    public bool IsDead { get; private set; }

    [SerializeField]
    public float maxHp;

    [SerializeField]
    public float MaxStamina;

    [SerializeField]
    private float staminaRecoverySpeed;
    [SerializeField]
    private float MaxskillGauge = 100;


    [SerializeField] public int maxBullets = 50;
    [SerializeField] int maxMeleeBullets = 50;

    [SerializeField]
    private float attackPower;
    public float GetAttackPower() { return attackPower; }

    [SerializeField]
    private float moveSpeed;
    public float GetMoveSpeed() { return moveSpeed; }


    public event Action HealthChanged;
    public event Action StaminaChanged;
    public event Action BulletChanged;
    public event Action MeleeBulletChanged;
    public event Action SkillGaugeChanged;
    public Action<bool> OnMeleeModeChanged;

    private void Awake()
    {
        Restore();
        UIManager.Instance.setPlayer(this);
    }
    private void Start()
    {        
        //UIManager.Instance.UpdateStamina(stamina, MaxStamina);
        UpdateHealth();
        UpdateStamina();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            StaminaConsumption(10);
            Hit(10);
        }

        StaminaAutoRecovery();
    }

    //���¹̳� �Ҹ� 
    public void StaminaConsumption(float power)
    {
        if (stamina > power)
        {
            stamina -= power;
            //UIManager.Instance.UpdateStamina(stamina, MaxStamina);
            UpdateStamina();
        }
        else
            return;

    }

    //���¹̳� �ڵ� ȸ��
    public void StaminaAutoRecovery()
    {
        if (stamina < MaxStamina)
        {
            stamina += staminaRecoverySpeed * Time.deltaTime;

            //UIManager.Instance.UpdateStamina(stamina, MaxStamina);
            UpdateStamina();
        }
        else if (stamina > MaxStamina)
        {
            stamina = MaxStamina;
            return;
        }
    }

    public void Hit(float dmg)
    {
        //dmg��ŭ ü�� ����
        if (hp > 0)
        {
            hp -= dmg;

        }
        //ü���� 0�� �� ��� IsDead�� true��.
        if (hp == 0)
        {
            IsDead = true;
            Debug.Log("����");
        }
        //ü�� ��ġ�� UI ����.

        //UIManager.Instance.UpdatehealthPoint(hp, maxHp);
        UpdateHealth();
    }

    //źȯ ȹ��
    public void AcquireBullets(int _bullets)
    {
        bullets += _bullets;
        if(bullets > maxBullets)
        {
            bullets = maxBullets;
        }

        UpdateBullet();
    }

    //źȯ �Ҹ�
    public void BulletConsumption()
    {
        if (bullets != 0)
            bullets--;
        else
        {
            Debug.Log("ź�� ����");
            return;
        }
        UpdateBullet();
    }
    //źȯ ȹ��
    public void AcquireBullets_Melee(int _bullets)
    {
        meleeBullets += _bullets;
        if(meleeBullets > maxBullets)
        {
            meleeBullets = maxBullets;
        }

        UpdateBullet_Melee();
    }

    //źȯ �Ҹ�
    public bool TryBulletConsumption_Melee(int value)
    {
        if(meleeBullets >= value)
        {
            meleeBullets -= value;
            UpdateBullet_Melee();
            return true;
        }
        else
        {
            return false;
        }
    }


    public void SkillGaugeRecovery(float value)
    {
        skillGauge += value;

        if(skillGauge > MaxskillGauge)
        {
            skillGauge = MaxskillGauge;
        }

        UpdateSkillGauge();
    }
    public bool TryUseSkillGauge(float value)
    {
        if(skillGauge >= value)
        {
            skillGauge -= value;
            UpdateSkillGauge();
            return true;
        }
        else
        {
            return false;
        }                
    }


    void Restore()
    {
        hp = maxHp;
        IsDead = false;
        stamina = MaxStamina;
        skillGauge = MaxskillGauge;
        bullets = maxBullets / 3;
    }
    public void UpdateHealth()
    {
        HealthChanged?.Invoke();
    }
    public void UpdateStamina()
    {
        StaminaChanged?.Invoke();
    }
    public void UpdateBullet()
    {
        BulletChanged?.Invoke();
    }
    public void UpdateBullet_Melee()
    {
        MeleeBulletChanged?.Invoke();
    }
    public void UpdateSkillGauge()
    {
        SkillGaugeChanged?.Invoke();
    }    
}