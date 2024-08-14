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

        if(targetChipLevel == 0)//Ĩ�� ���� ���
        {
            Debug.Log("���ο� Ĩ�� �����մϴ�.");
            useBlueChipDic.Add(targetBlueChip, new BlueChipSlot(targetBlueChip, 1));
            return true;
        }
        else if(targetChipLevel > 0 && targetChipLevel < JsonDataManager.GetBlueChipData(targetBlueChip).Level_VelueList.Count)//Ĩ�� ������, �ִ� ���� �̸��� ���
        {
            Debug.Log("Ĩ ������ ��ȭ�մϴ�.");
            useBlueChipDic[targetBlueChip].LevelUp();
            return true;
        }
        else//Ĩ�� �ִ� ������ ���
        {
            Debug.LogError("Ĩ�� �ִ� �����Դϴ�");
            return false;
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q)) 
        {
            TryAddBlueChip(BlueChipID.�ٰŸ�1);
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