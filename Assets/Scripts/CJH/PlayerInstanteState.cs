using System;
using UnityEngine;

public class PlayerInstanteState : MonoBehaviour
{
    public float hp { get; private set; }
    public float stamina { get; private set; }
    public int bullets { get; private set; }
    public float skillGauge { get; private set; }


    public bool IsDead { get; private set; }

    [SerializeField]
    public float maxHp;

    [SerializeField]
    public float MaxStamina;

    [SerializeField]
    private float staminaRecoverySpeed;
    [SerializeField]
    public float MaxskillGauge = 100;


    [SerializeField]
    public int maxBullets;

    [SerializeField]
    private float attackPower;
    public float GetAttackPower() { return attackPower; }

    [SerializeField]
    private float moveSpeed;
    public float GetMoveSpeed() { return moveSpeed; }


    public event Action HealthChanged;
    public event Action StaminaChanged;
    public event Action BulletChanged;
    public event Action SkillGaugeChanged;

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
        UpdateSkillGauge();
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
        if (bullets < maxBullets)
        {
            bullets += _bullets;
            Debug.Log("bullets : " + bullets);
        }
        else
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
        skillGauge = 0;
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
    public void UpdateSkillGauge()
    {
        SkillGaugeChanged?.Invoke();
    }
}