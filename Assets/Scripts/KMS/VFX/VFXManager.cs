using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : GlobalSingleton<VFXManager>
{

    public VFXData VFXDATA;
    private Dictionary<string, SO_SKillEvent> VFXDictionary;

    private void Awake()
    {
        InitializeAudioDictionary();
    }

    private void InitializeAudioDictionary() // AWAKE할 때 sfxData로부터 오디오클립을 받아옴
    {
        VFXDictionary = new Dictionary<string, SO_SKillEvent>
    {
        { "Bose_Back_Jump", VFXDATA.Bose_Back_Jump },
        { "Bose_Spike_Trail", VFXDATA.Bose_Spike_Trail },
        { "Bose_Spike_Hand", VFXDATA.Bose_Spike_Hand },
        { "Bose_Spike_Ground", VFXDATA.Bose_Spike_Hand },
        { "Bose_Spike_Multi_Hand", VFXDATA.Bose_Spike_Multi_Hand },
        { "Bose_MeleeAttack", VFXDATA.Bose_Spike_Multi_Hand },
        { "Bose_DashAttack", VFXDATA.Bose_Spike_Multi_Hand },
        { "Bose_DashAttack_Jump", VFXDATA.Bose_Spike_Multi_Hand },
        { "Boss_Teleport", VFXDATA.Boss_Teleport },
        { "Boss_Teleport_End1", VFXDATA.Boss_Teleport_End1 },
        { "Boss_Teleport_End2", VFXDATA.Boss_Teleport_End2 },
        { "Boss_Charge_Energy", VFXDATA.Boss_Charge_Energy },
        { "Boss_Ulti_Boom", VFXDATA.Boss_Ulti_Boom },
        { "Boss_Footprint", VFXDATA.Boss_Footprint },
        { "Boss_Run", VFXDATA.Boss_Run }
    };
    }
    public void Effect(string VFXName, Transform transform)
    {
        if (VFXDictionary.TryGetValue(VFXName, out SO_SKillEvent SFX))
        {
            if (SFX != null)
            {
                GameObject VFX = ObjectPoolManager.Instance.DequeueObject(SFX.preFab);
                //Vector3 position = Vector3.zero;

                Vector3 finalPosition = transform.position + transform.TransformDirection(SFX.offSet);
                VFX.transform.position = finalPosition;
                //Quaternion playerRotation = transform.rotation;
                Quaternion finalRotation = Quaternion.Euler(SFX.rotation);
                VFX.transform.rotation = finalRotation;
                VFX.transform.localScale = Vector3.one * SFX.size;

                // 파티클 자동 반환 스크립트 추가
                if (VFX.GetComponent<VFXAutoReturn>() == null)
                {
                    VFX.AddComponent<VFXAutoReturn>();
                }
            }
        }
    }
    public void Effect(string VFXName, Transform transform, Transform Rotation)
    {
        if (VFXDictionary.TryGetValue(VFXName, out SO_SKillEvent SFX))
        {
            if (SFX != null)
            {
                GameObject VFX = ObjectPoolManager.Instance.DequeueObject(SFX.preFab);
                //Vector3 position = Vector3.zero;
                Vector3 finalPosition = transform.position + transform.TransformDirection(SFX.offSet);
                VFX.transform.position = finalPosition;
                Quaternion playerRotation = Rotation.rotation;
                Quaternion finalRotation = playerRotation * Quaternion.Euler(SFX.rotation);
                VFX.transform.rotation = finalRotation;
                VFX.transform.localScale = Vector3.one * SFX.size;

                // 파티클 자동 반환 스크립트 추가
                if (VFX.GetComponent<VFXAutoReturn>() == null)
                {
                    VFX.AddComponent<VFXAutoReturn>();
                }
            }
        }
    }
    public SO_SKillEvent.PlayerPos SetPos(string VFXName)
    {
        if(VFXDictionary.TryGetValue(VFXName,out SO_SKillEvent VFX))
        {
            if (VFX != null)
            {
                return VFX.playerPos;
            }
        }
        return SO_SKillEvent.PlayerPos.Foot;
    }

}
