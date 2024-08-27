using System.Collections.Generic;
using UnityEngine;
using EnumTypes;

public class PlayerEquipBlueChip : MonoBehaviour
{
    Dictionary<BlueChipID, BlueChipSlot> useBlueChipDic = new Dictionary<BlueChipID, BlueChipSlot>();

    public int GetBlueChipLevel(BlueChipID targetBlueChip)
    {
        int level = 0;

        if(useBlueChipDic.ContainsKey(targetBlueChip))
        {
            level = useBlueChipDic[targetBlueChip].Level;
        }

        return level;
    }
    public Dictionary<BlueChipID, BlueChipSlot> GetBlueChipDic()
    {
        return useBlueChipDic;
    }
    public void Init_OnSceneLode()//씬 로드 시 (Playermaster의 Init 메서드 또는 awake 호출 시)
    {
        useBlueChipDic = new Dictionary<BlueChipID, BlueChipSlot>();

        if (JsonDataManager.GetUserData().TryGetPlayData(out PlayData playData))
        {
            foreach (var item in playData.InGame_BlueChip_Level)
            {
                useBlueChipDic.Add(item.Key, new BlueChipSlot(item.Key, item.Value));
            }
        }
    }    

    public bool TryAddBlueChip(BlueChipID targetBlueChip)
    {
        int targetChipLevel = GetBlueChipLevel(targetBlueChip);

        if(targetChipLevel == 0)//칩이 없는 경우
        {
            if (useBlueChipDic.Count >= 3)
            {
                Debug.LogWarning("이미 3종의 칩을 장착하고 있습니다");
                return false;
            }
            else
            {
                Debug.Log("새로운 칩을 생성합니다.");
                useBlueChipDic.Add(targetBlueChip, new BlueChipSlot(targetBlueChip, 1));
                return true;
            }
        }
        else if(targetChipLevel > 0 && targetChipLevel < JsonDataManager.GetBlueChipData(targetBlueChip).Level_VelueList.Count)//칩이 있으며, 최대 레벨 미만일 경우
        {            
            useBlueChipDic[targetBlueChip].LevelUp(1);
            Debug.Log($"칩 레벨을 강화합니다. : {useBlueChipDic[targetBlueChip].Level}레벨");
            return true;
        }
        else//칩이 최대 레벨일 경우
        {
            Debug.LogWarning("칩이 최대 레벨입니다");
            return false;
        }
    }
    public bool TryAddBlueChip(BlueChipID targetBlueChip, int targetLevel)
    {
        int targetChipLevel = GetBlueChipLevel(targetBlueChip);

        if (targetChipLevel == 0)//칩이 없는 경우
        {
            if (useBlueChipDic.Count >= 3)
            {
                Debug.LogWarning("이미 3종의 칩을 장착하고 있습니다");
                return false;
            }
            else
            {
                Debug.Log("새로운 칩을 생성합니다.");
                useBlueChipDic.Add(targetBlueChip, new BlueChipSlot(targetBlueChip, targetLevel));
                return true;
            }
        }
        else if (targetChipLevel > 0 && targetChipLevel < JsonDataManager.GetBlueChipData(targetBlueChip).Level_VelueList.Count)//칩이 있으며, 최대 레벨 미만일 경우
        {
            useBlueChipDic[targetBlueChip].SetLevel(targetLevel);
            Debug.Log($"칩 레벨을 강화합니다. : {useBlueChipDic[targetBlueChip].Level}레벨");
            return true;
        }
        else//칩이 최대 레벨일 경우
        {
            Debug.LogWarning("칩이 최대 레벨입니다");
            return false;
        }
    }
    public void SwapBlueChip(BlueChipID targetId, BlueChipID newId, int level)
    {
        if(useBlueChipDic.ContainsKey(targetId))
        {
            useBlueChipDic.Remove(targetId);
            Debug.Log("새로운 칩을 생성합니다.");
            useBlueChipDic.Add(newId, new BlueChipSlot(newId, level));
        }
        else
        {
            Debug.LogWarning($"존재하지 않는 대상 칩입니다. : {targetId}");
        }
    }

    public Dictionary<BlueChipID, BlueChipSlot> GetRandomBlueChip(int count, int addLevel)
    {
        Dictionary<BlueChipID, BlueChipSlot> selectSlotDic = new Dictionary<BlueChipID, BlueChipSlot>();

        while (selectSlotDic.Count < count)
        {
            BlueChipID id = (BlueChipID)Random.Range(0, 9);

            if (selectSlotDic.ContainsKey(id))
                continue;

            int targetChipLevel = GetBlueChipLevel(id);
            int targetChipMaxLevel = JsonDataManager.GetBlueChipData(id).Level_VelueList.Count;

            if (targetChipLevel < targetChipMaxLevel)//칩이 최대 레벨 미만일 경우
            {
                targetChipLevel += addLevel;
                if(targetChipLevel > targetChipMaxLevel)
                {
                    targetChipLevel = targetChipMaxLevel;
                }
                selectSlotDic.Add(id, new BlueChipSlot(id, targetChipLevel));
            }
        }

        return selectSlotDic;
    }
}


public class BlueChipSlot
{
    public BlueChipID Id { get; private set; }
    public int Level { get; private set; }    

    public BlueChipSlot(BlueChipID id, int level)
    {
        Id = id;        
        Level = level;
    }

    public void LevelUp(int addLevel)
    {
        Level += addLevel;        
    }
    public void SetLevel(int level)
    {
        Level = level;
    }

}