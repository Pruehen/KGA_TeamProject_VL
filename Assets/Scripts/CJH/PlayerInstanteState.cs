using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class PlayerInstanteState : MonoBehaviour
{
    float hp;
    float stamina;

    [SerializeField]
    private float maxHp;

    [SerializeField]
    private float MaxStamina;

    [SerializeField]
    private float staminaRecoverySpeed;

    private void Start()
    {
        stamina = MaxStamina;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
           
            StaminaConsumption(10);
        }

        StaminaAutoRecovery();
    }

    //���¹̳� �Ҹ� 
    public void StaminaConsumption(float power)
    {
        if (stamina > power)
            stamina -= power;
        else
            return;

        Debug.Log("stamina : " + stamina);

    }

    //���¹̳� �ڵ� ȸ��
    public void StaminaAutoRecovery()
    {
        if (stamina < MaxStamina)
        {
            stamina += staminaRecoverySpeed * Time.deltaTime;
            Debug.Log("stamina : " + stamina);
        }
        else if(stamina > MaxStamina)
        {
            stamina = MaxStamina;
            return;
        }

    
    
    }




}








