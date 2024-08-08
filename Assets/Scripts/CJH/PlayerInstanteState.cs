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
        UIManager.Instance.UpdateStamina(stamina,MaxStamina);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
           
            StaminaConsumption(10);
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

        Debug.Log("stamina : " + stamina);

    }

    //스태미나 자동 회복
    public void StaminaAutoRecovery()
    {
        if (stamina < MaxStamina)
        {
            stamina += staminaRecoverySpeed * Time.deltaTime;
            Debug.Log("stamina : " + stamina);
            UIManager.Instance.UpdateStamina(stamina,MaxStamina);
        }
        else if(stamina > MaxStamina)
        {
            stamina = MaxStamina;
            return;
        }
    }
}








