using EnumTypes;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPassive : MonoBehaviour
{
    [Header("���� ī�װ� �нú� ����")]    
    [SerializeField] PassiveID PassiveID_Offensive1 = PassiveID.None;
    [SerializeField] PassiveID PassiveID_Offensive2 = PassiveID.None;

    [Header("ü�� ī�װ� �нú� ����")]    
    [SerializeField] PassiveID PassiveID_Defensive1 = PassiveID.None;
    [SerializeField] PassiveID PassiveID_Defensive2 = PassiveID.None;

    [Header("��ƿ ī�װ� �нú� ����")]    
    [SerializeField] PassiveID PassiveID_Utility1 = PassiveID.None;
    [SerializeField] PassiveID PassiveID_Utility2 = PassiveID.None;

    [Header("����� ��� ��� : üũ ��, �����ͻ󿡼� ������ �нú긦 ������.")]
    [SerializeField] bool IsDebugMode = false;

    public Dictionary<PassiveID, bool> PassiveDic { get; private set; }

    public void Init()
    {
        PassiveDic = new Dictionary<PassiveID, bool>();

        if (IsDebugMode)
        {
            if (PassiveID_Offensive1 != PassiveID.None)
            {
                PassiveDic.TryAdd(PassiveID_Offensive1, true);
            }
            if (PassiveID_Offensive2 != PassiveID.None)
            {
                PassiveDic.TryAdd(PassiveID_Offensive2, true);
            }
            if (PassiveID_Defensive1 != PassiveID.None)
            {
                PassiveDic.TryAdd(PassiveID_Defensive1, true);
            }
            if (PassiveID_Defensive2 != PassiveID.None)
            {
                PassiveDic.TryAdd(PassiveID_Defensive2, true);
            }
            if (PassiveID_Utility1 != PassiveID.None)
            {
                PassiveDic.TryAdd(PassiveID_Utility1, true);
            }
            if (PassiveID_Utility2 != PassiveID.None)
            {
                PassiveDic.TryAdd(PassiveID_Utility2, true);
            }
        }
    }

    public bool ContainPassiveId(PassiveID id)
    {
        return PassiveDic.ContainsKey(id);
    }
}
