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
    public bool TryAddBlueChip(BlueChipID targetBlueChip)
    {
        int targetChipLevel = GetBlueChipLevel(targetBlueChip);

        if(targetChipLevel == 0)//칩이 없는 경우
        {
            Debug.Log("새로운 칩을 생성합니다.");
            useBlueChipDic.Add(targetBlueChip, new BlueChipSlot(targetBlueChip, 1));
            return true;
        }
        else if(targetChipLevel > 0 && targetChipLevel < JsonDataManager.GetBlueChipData(targetBlueChip).Level_VelueList.Count)//칩이 있으며, 최대 레벨 미만일 경우
        {
            Debug.Log("칩 레벨을 강화합니다.");
            useBlueChipDic[targetBlueChip].LevelUp();
            return true;
        }
        else//칩이 최대 레벨일 경우
        {
            Debug.LogError("칩이 최대 레벨입니다");
            return false;
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q)) 
        {
            TryAddBlueChip(BlueChipID.근거리1);
        }
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

    public void LevelUp()
    {
        Level++;
    }
}