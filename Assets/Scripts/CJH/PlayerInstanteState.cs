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

    //스태미나 소모 
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

    //스태미나 자동 회복
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


    //체력 감소
    public void Hit(float dmg)
    {
        //dmg만큼 체력 감소
        if (hp > 0)
        {
            hp -= dmg;
            
        }
        else
            return;

        //체력이 0이 될 경우 IsDead를 true로.
        if (hp == 0)
        {
            IsDead = true;
            Debug.Log("죽음");
            Destroy(gameObject);

        }

        //체력 수치와 UI 연동.
        UIManager.Instance.UpdatehealthPoint(hp , maxHp);

    }


    //탄환 획득
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

    //탄환 소모
    public void BulletConsumption()
    {
        if (bullets != 0)
            bullets--;
        else
        {
            Debug.Log("탄알 없음");
            return;
        }
        
    }

   

}








