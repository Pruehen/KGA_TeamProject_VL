using System.Collections.Generic;
using UnityEngine;
using EnumTypes;

public class PlayerEquipBlueChip : MonoBehaviour
{
    List<BlueChipID> useBlueChipList = new List<BlueChipID>();

    public bool HasBlueChip(BlueChipID targetBlueChip)
    {
        bool hasKey = false;
        foreach (var item in useBlueChipList)
        {
            if(item == targetBlueChip)
            {
                hasKey = true;
                break;
            }
        }
        return hasKey;
    }
    public List<BlueChipID> GetBlueChipList()
    {
        return useBlueChipList;
    }
    public bool TryAddBlueChip(BlueChipID targetBlueChip)
    {
        if(HasBlueChip(targetBlueChip) == false)
        {
            useBlueChipList.Add(targetBlueChip);
            return true;
        }
        else
        {
            return false;
        }
    }
}
