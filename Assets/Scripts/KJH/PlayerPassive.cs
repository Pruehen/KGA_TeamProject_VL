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

    public HashSet<PassiveID> PassiveHashSet { get; private set; }

    public void Init()
    {
        if (IsDebugMode)
        {
            PassiveHashSet = new HashSet<PassiveID>();

            if (PassiveID_Offensive1 != PassiveID.None)
            {
                PassiveHashSet.Add(PassiveID_Offensive1);
            }
            if (PassiveID_Offensive2 != PassiveID.None)
            {
                PassiveHashSet.Add(PassiveID_Offensive2);
            }
            if (PassiveID_Defensive1 != PassiveID.None)
            {
                PassiveHashSet.Add(PassiveID_Defensive1);
            }
            if (PassiveID_Defensive2 != PassiveID.None)
            {
                PassiveHashSet.Add(PassiveID_Defensive2);
            }
            if (PassiveID_Utility1 != PassiveID.None)
            {
                PassiveHashSet.Add(PassiveID_Utility1);
            }
            if (PassiveID_Utility2 != PassiveID.None)
            {
                PassiveHashSet.Add(PassiveID_Utility2);
            }
        }
        else
        {
            PassiveHashSet = JsonDataManager.GetUserData().UsePassiveHashSet;
        }
    }

    public bool ContainPassiveId(PassiveID id)
    {
        return PassiveHashSet.Contains(id);
    }
}
