using System.Collections.Generic;
using UnityEngine;
using EnumTypes;

public class PlayerEquipBlueChip : MonoBehaviour
{
    List<BlueChipSlot> useBlueChipList = new List<BlueChipSlot>();

    public bool HasBlueChip(BlueChipID targetBlueChip)
    {
        bool hasKey = false;
        foreach (var item in useBlueChipList)
        {
            if(item.Id == targetBlueChip)
            {
                hasKey = true;
                break;
            }
        }
        return hasKey;
    }
    public List<BlueChipSlot> GetBlueChipList()
    {
        return useBlueChipList;
    }
    public bool TryAddBlueChip(BlueChipID targetBlueChip)
    {
        if(HasBlueChip(targetBlueChip) == false)
        {
            useBlueChipList.Add(new BlueChipSlot(targetBlueChip));
            return true;
        }
        else
        {
            return false;
        }
    }
}

public class BlueChipSlot
{
    public BlueChipID Id { get; private set; }
    public BlueChip blueChipData { get; private set; }

    public BlueChipSlot(BlueChipID id)
    {
        Id = id;
        blueChipData = JsonDataManager.jsonCache.BlueChipTableCache.dic[(int)Id];
    }
}