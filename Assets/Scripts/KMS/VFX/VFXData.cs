using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
[CreateAssetMenu(fileName = "VFXData", menuName = "VFX/VFXData", order = 0)]
public class VFXData : ScriptableObject
{
    [Header("NPC")]
    public SO_SKillEvent Bose_Back_Jump;
    public SO_SKillEvent Bose_Spike_Trail;
    public SO_SKillEvent Bose_Spike_Hand;
    public SO_SKillEvent Bose_Spike_Multi_Hand;
    public SO_SKillEvent Bose_MeleeAttack;
    public SO_SKillEvent Bose_DashAttack;
    public SO_SKillEvent Boss_Teleport;
    public SO_SKillEvent Boss_Teleport_End1;
    public SO_SKillEvent Boss_Teleport_End2;
    public SO_SKillEvent Boss_Charge_Energy;
    public SO_SKillEvent Boss_Ulti_Boom;
    public SO_SKillEvent Boss_Footprint;
    public SO_SKillEvent Boss_Run;
}