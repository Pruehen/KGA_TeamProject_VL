using System.Collections.Generic;
using UnityEngine;

public class BlueChipUIManager : MonoBehaviour
{
    [Header("���� ���� ���� ���Ĩ UI")]
    [SerializeField] List<BlueChipIcon> Icon_EquipChipList;

    [Header("���Ĩ ���� UI")]
    [SerializeField] List<BlueChipIcon> Icon_SelectChipList;

 

    public void Init()
    {
        //Debug.Log("���Ĩ â �ʱ�ȭ");

        //1. ���ʿ� �ִ� UI�� ���� ���� ���� ���Ĩ�� ǥ��.    
        List<BlueChipSlot> equipedSlotList = new List<BlueChipSlot>();
        foreach (var item in PlayerMaster.Instance._PlayerEquipBlueChip.GetBlueChipDic())
        {
            equipedSlotList.Add(item.Value);
        }                
        for (int i = 0; i < Icon_EquipChipList.Count; i++)
        {
            if (equipedSlotList.Count > i)
            {
                Icon_EquipChipList[i].SetChipData(equipedSlotList[i]);
            }
            else
            {
                Icon_EquipChipList[i].SetChipData(null);
            }    
        }

        equipedSlotList.Clear();
        //2. �߾ӿ� �ִ� UI�� ������ 3���� ���Ĩ ������ ǥ��

        foreach (var item in PlayerMaster.Instance._PlayerEquipBlueChip.GetRandomBlueChip())
        {
            equipedSlotList.Add(item.Value);
        }
        for (int i = 0; i < Icon_SelectChipList.Count; i++)
        {
            if (equipedSlotList.Count > i)
            {
                Icon_SelectChipList[i].SetChipData(equipedSlotList[i]);
            }
            else
            {
                Icon_SelectChipList[i].SetChipData(null);
            }
        }
    }
}
