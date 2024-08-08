using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class PlayerInstanteState : MonoBehaviour
{
    float hp;
    float stamina;

    public bool IsDead { get; private set; }

    [SerializeField]
    private float maxHp;

    [SerializeField]
    private float MaxStamina;

    [SerializeField]
    private float staminaRecoverySpeed;

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
}








