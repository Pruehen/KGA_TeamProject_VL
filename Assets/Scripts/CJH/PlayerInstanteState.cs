using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Zenject.SpaceFighter;

public class PlayerInstanteState : MonoBehaviour
{
    public float hp;
    public float stamina;
    public int bullets;


    public bool IsDead { get; private set; }

    [SerializeField]
    public float maxHp;

    [SerializeField]
    public float MaxStamina;

    [SerializeField]
    private float staminaRecoverySpeed;


    [SerializeField]
    public int maxBullets;

    // ���� ���� źȯ�� ���� �߰�
    // �ִ� ���� ���� źȯ�� ���� �߰� (�ø���������ʵ�)
    // źȯ �Ҹ� �޼��� �߰�
    // źȯ ȹ�� �޼��� �߰�



    public event Action HealthChanged;
    public event Action StaminaChanged;
    public event Action BulletChanged;

    private void Awake()
    {
        UIManager.Instance.setPlayer(this);
    }
    private void Start()
    {
        IsDead = false;
        stamina = MaxStamina;

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
        else
            return;

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
            return;
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


    public void Restore()
    {
        hp = maxHp;
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
        StaminaChanged?.Invoke();
    }

}








