using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BlueChipUIManager : MonoBehaviour
{
    [Header("현재 착용 중인 블루칩 UI")]
    [SerializeField] List<BlueChipIcon> Icon_EquipChipList;

    [Header("블루칩 선택 UI")]
    [SerializeField] List<BlueChipIcon> Icon_SelectChipList;

    [SerializeField] GameObject Btn_ReRoll;
    [SerializeField] bool CanReRoll = false;

    public void Init()
    {
        //Debug.Log("블루칩 창 초기화");

        //1. 왼쪽에 있는 UI에 현재 착용 중인 블루칩을 표시.    
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

        SetRandomBlueChip();

        if (CanReRoll)
        {
            Btn_ReRoll.SetActive(true);
        }
    }

    public void SetRandomBlueChip()
    {
        List<BlueChipSlot> equipedSlotList = new List<BlueChipSlot>();
        //2. 중앙에 있는 UI에 임의의 3개의 블루칩 정보를 표시

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
        Btn_ReRoll.SetActive(false);
        EventSystem.current.SetSelectedGameObject(Icon_SelectChipList[0].gameObject);
    }
}
