using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class PlayerInstanteState : MonoBehaviour
{
    float hp;
    float stamina;
    int bullets;

    public bool IsDead { get; private set; }

    [SerializeField]
    private float maxHp;

    [SerializeField]
    private float MaxStamina;

    [SerializeField]
    private float staminaRecoverySpeed;

    [SerializeField]
    private int maxBullets;

    [SerializeField]
    private float attackPower;
    public float GetAttackPower() { return attackPower; }

    [SerializeField]
    private float moveSpeed;
    public float GetMoveSpeed() { return moveSpeed; }

    private void Start()
    {
        IsDead = false;
        stamina = MaxStamina;
        hp = maxHp;
        UIManager.Instance.UpdateStamina(stamina,MaxStamina);
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
            UIManager.Instance.UpdateStamina(stamina, MaxStamina);
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
           
            UIManager.Instance.UpdateStamina(stamina, MaxStamina);
        }
        else if (stamina > MaxStamina)
        {
            stamina = MaxStamina;
            return;
        }
    }


    //ü�� ����
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
            Destroy(gameObject);

        }

        //ü�� ��ġ�� UI ����.
        UIManager.Instance.UpdatehealthPoint(hp , maxHp);

    }


    //źȯ ȹ��
    public void AcquireBullets(int  _bullets)
    {
        if (bullets < maxBullets)
        {
            bullets += _bullets;
            Debug.Log("bullets : " + bullets);
        }
        else
            return;
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
        
    }

   

}








