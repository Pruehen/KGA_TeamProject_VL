using EnumTypes;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPassive : MonoBehaviour
{
    [Header("공격 카테고리 패시브 선택")]    
    [SerializeField] PassiveID PassiveID_Offensive1 = PassiveID.None;
    [SerializeField] PassiveID PassiveID_Offensive2 = PassiveID.None;

    [Header("체력 카테고리 패시브 선택")]    
    [SerializeField] PassiveID PassiveID_Defensive1 = PassiveID.None;
    [SerializeField] PassiveID PassiveID_Defensive2 = PassiveID.None;

    [Header("유틸 카테고리 패시브 선택")]    
    [SerializeField] PassiveID PassiveID_Utility1 = PassiveID.None;
    [SerializeField] PassiveID PassiveID_Utility2 = PassiveID.None;

    [Header("디버그 모드 사용 : 체크 시, 에디터상에서 선택한 패시브를 적용함.")]
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
