using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInstanteState : MonoBehaviour
{
    public float hp;
    public float stamina;

    public bool IsDead { get; private set; }

    [SerializeField]
    public float maxHp;

    [SerializeField]
    public float MaxStamina;

    [SerializeField]
    private float staminaRecoverySpeed;

    public event Action HealthChanged;
    public event Action StaminaChanged;

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

}








