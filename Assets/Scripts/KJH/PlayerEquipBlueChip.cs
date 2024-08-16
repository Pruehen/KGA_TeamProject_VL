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
            if (useBlueChipDic.Count >= 3)
            {
                Debug.LogWarning("�̹� 3���� Ĩ�� �����ϰ� �ֽ��ϴ�");
                return false;
            }
            else
            {
                Debug.Log("���ο� Ĩ�� �����մϴ�.");
                useBlueChipDic.Add(targetBlueChip, new BlueChipSlot(targetBlueChip, 1));
                return true;
            }
        }
        else if(targetChipLevel > 0 && targetChipLevel < JsonDataManager.GetBlueChipData(targetBlueChip).Level_VelueList.Count)//Ĩ�� ������, �ִ� ���� �̸��� ���
        {            
            useBlueChipDic[targetBlueChip].LevelUp();
            Debug.Log($"Ĩ ������ ��ȭ�մϴ�. : {useBlueChipDic[targetBlueChip].Level}����");
            return true;
        }
        else//Ĩ�� �ִ� ������ ���
        {
            Debug.LogWarning("Ĩ�� �ִ� �����Դϴ�");
            return false;
        }
    }
    public void SwapBlueChip(BlueChipID targetId, BlueChipID newId)
    {
        if(useBlueChipDic.ContainsKey(targetId))
        {
            useBlueChipDic.Remove(targetId);
            Debug.Log("���ο� Ĩ�� �����մϴ�.");
            useBlueChipDic.Add(newId, new BlueChipSlot(newId, 1));
        }
        else
        {
            Debug.LogWarning($"�������� �ʴ� ��� Ĩ�Դϴ�. : {targetId}");
        }
    }

    public Dictionary<BlueChipID, BlueChipSlot> GetRandomBlueChip()
    {
        Dictionary<BlueChipID, BlueChipSlot> selectSlotDic = new Dictionary<BlueChipID, BlueChipSlot>();

        while (selectSlotDic.Count < 3)
        {
            BlueChipID id = (BlueChipID)Random.Range(0, 9);

            if (selectSlotDic.ContainsKey(id))
                continue;

            int targetChipLevel = GetBlueChipLevel(id);

            if (targetChipLevel < JsonDataManager.GetBlueChipData(id).Level_VelueList.Count)//Ĩ�� �ִ� ���� �̸��� ���
            {
                selectSlotDic.Add(id, new BlueChipSlot(id, targetChipLevel + 1));
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

    public void LevelUp()
    {
        Level++;
    }
}